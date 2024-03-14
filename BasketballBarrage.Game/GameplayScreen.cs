using System;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Utils;
using osuTK;

namespace BasketballBarrage.Game;

public partial class GameplayScreen : GameScreen
{
    private Players players = null!;

    private readonly Bindable<bool> roundInProgress = new Bindable<bool>();

    private readonly Bindable<int> points = new Bindable<int>();

    private readonly Bindable<int> combo = new Bindable<int>();

    private readonly Bindable<int> maxCombo = new Bindable<int>();

    private readonly int[] pointsArray;

    private Hoop hoop = null!;
    private SpriteText pointEarnedText = null!;
    private Container gameContainer = null!;
    private Sample scoreSample = null!;
    private Sample throwSample = null!;
    private SpriteText readySetGoText = null!;
    private Container hoopContainer = null!;
    private readonly GameplayMode mode;
    private int basketballsVisible;
    private DateTime startTime;
    private DateTime endTime;
    private int rounds;
    private HUDOverlay hudOverlay = null!;
    private BonusTarget? bonusTarget;
    private int bonusTargetPos;
    private bool bonusTargetHit;
    private int hoopDirection = 1;
    private readonly bool isTwoPlayers;
    private int currentPlayer;

    public const float GAME_WIDTH = 800;
    private const int hoop_y_pos = -500;
    private const int round_transition_delay = 2000;
    public const int CLASSIC_ROUND_TIME = 30;

    public GameplayScreen(GameplayMode mode, bool isTwoPlayers = false)
    {
        this.mode = mode;
        this.isTwoPlayers = isTwoPlayers;
        pointsArray = new int[isTwoPlayers ? 2 : 1];

        ValidForResume = false;

        switch (mode)
        {
            case GameplayMode.Classic:
                // based on original game
                rounds = 2;
                break;

            case GameplayMode.Endless:
                rounds = 1;
                break;
        }
    }

    [BackgroundDependencyLoader]
    private void load(ISampleStore samples)
    {
        // samples taken from https://mixkit.co/free-sound-effects/basketball/
        scoreSample = samples.Get("basketball-score.wav");
        throwSample = samples.Get("basketball-throw.wav");

        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientVertical(Colour4.DimGray, Colour4.Black),
            },
            gameContainer = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = GAME_WIDTH,
                RelativeSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    players = new Players
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        GameInProgress = { BindTarget = roundInProgress },
                        Alpha = 0,
                    },
                    hoopContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomCentre,
                        Y = hoop_y_pos,
                        Child = hoop = new Hoop
                        {
                            Combo = { BindTarget = combo },
                            Alpha = 0,
                        },
                    },
                    pointEarnedText = new SpriteText
                    {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomCentre,
                        Font = FontUsage.Default.With(size: 50),
                        Alpha = 0,
                        Shadow = true,
                    },
                }
            },
            hudOverlay = new HUDOverlay
            {
                Combo = { BindTarget = combo },
                MaxCombo = { BindTarget = maxCombo },
                Points = { BindTarget = points },
                Mode = mode,
            },
            readySetGoText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = FontUsage.Default.With(size: 50),
                Alpha = 0,
                Shadow = true,
            },
        };

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            var isEdge = i == 0 || i == players.Count - 1;
            var playerIndex = i;
            player.ShootBasketball = () => spawnBasketBall(player, isEdge, playerIndex);
        }
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        startRound();

        combo.BindValueChanged(c =>
        {
            maxCombo.Value = Math.Max(maxCombo.Value, c.NewValue);

            if (c.NewValue % 5 == 0)
                loopHoop(c.NewValue);
        });
    }

    private void startRound()
    {
        prepareForRound();
        displayInstruction();

        Scheduler.AddDelayed(() =>
        {
            loopHoop(combo.Value);

            roundInProgress.Value = true;

            if (mode == GameplayMode.Classic)
            {
                startTime = DateTime.Now;

                // based on original game
                endTime = startTime + TimeSpan.FromSeconds(CLASSIC_ROUND_TIME + 1);
            }
        }, round_transition_delay);
    }

    private void loopHoop(int currentCombo)
    {
        var currentHoopX = hoopContainer.X;

        float secondsPerLoop;

        switch (currentCombo)
        {
            case >= 20:
                secondsPerLoop = 3000;
                break;

            case >= 15:
                secondsPerLoop = 3500;
                break;

            case >= 10:
                secondsPerLoop = 4000;
                break;

            case >= 5:
                secondsPerLoop = 5000;
                break;

            default:
                secondsPerLoop = 6000;
                break;
        }

        var rightDuration = secondsPerLoop * ((GAME_WIDTH - currentHoopX) / GAME_WIDTH);
        var leftDuration = secondsPerLoop * (currentHoopX / GAME_WIDTH);

        hoopContainer.MoveToX(hoopDirection == 1 ? GAME_WIDTH : 0, hoopDirection == 1 ? rightDuration : leftDuration).Then()
                     .MoveToX(hoopDirection == 1 ? 0 : GAME_WIDTH, secondsPerLoop).Then()
                     .MoveToX(currentHoopX, hoopDirection == 1 ? leftDuration : rightDuration).Then()
                     .Loop();
    }

    private void prepareForRound()
    {
        if (isTwoPlayers)
        {
            currentPlayer++;
            if (currentPlayer == 3) currentPlayer = 1;
        }
        else
            currentPlayer = 1;

        points.Value = pointsArray[currentPlayer - 1];
        hudOverlay.ResetTimer(mode);

        combo.Value = 0;
        hoopContainer.MoveToX(GAME_WIDTH / 2f);
        players.FadeIn(TRANSITION_DURATION, Easing.OutQuint).MoveToY(players.Height).MoveToY(0, TRANSITION_DURATION, Easing.OutQuint);

        players.Colour = currentPlayer == 1 ? Colour4.Red : Colour4.Blue;
        hoop.Show();
        hoopDirection = 1;
    }

    private void displayInstruction()
    {
        Scheduler.AddDelayed(() =>
        {
            readySetGoText.Colour = Colour4.Red;
            readySetGoText.Text = "Ready";
            readySetGoText.Pop();
        }, 100);

        Scheduler.AddDelayed(() =>
        {
            readySetGoText.Colour = Colour4.Yellow;
            readySetGoText.Text = "Set";
            readySetGoText.Pop();
        }, 1050);

        Scheduler.AddDelayed(() =>
        {
            readySetGoText.Colour = Colour4.Green;
            readySetGoText.Text = "Go!";
            readySetGoText.Pop();
        }, round_transition_delay);
    }

    protected override void UpdateAfterChildren()
    {
        base.UpdateAfterChildren();

        if (roundInProgress.Value)
            spawnBonusTarget();

        if (mode == GameplayMode.Classic && roundInProgress.Value)
        {
            var secondsLeft = (endTime - DateTime.Now).Seconds;

            hudOverlay.UpdateTimer(secondsLeft.ToString());

            if (secondsLeft <= 0)
                endRound();
        }

        if (Precision.AlmostEquals(hoopContainer.X, GAME_WIDTH, 1))
            hoopDirection = -1;
        else if (Precision.AlmostEquals(hoopContainer.X, 0, 1))
            hoopDirection = 1;
    }

    private void spawnBonusTarget()
    {
        var rand = new Random();

        if (rand.Next(1, 10000) != 1 || bonusTarget != null) return;

        bonusTargetPos = new Random().Next(0, players.Count);
        gameContainer.Add(bonusTarget = new BonusTarget
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.Centre,
            X = gameContainer.ToLocalSpace(players[bonusTargetPos].ScreenSpaceDrawQuad.Centre).X,
            Y = hoop_y_pos - 50,
            Alpha = 0,
        });

        bonusTarget.FadeIn(250, Easing.OutQuint);
    }

    private void endRound()
    {
        if (!roundInProgress.Value) return;

        readySetGoText.Colour = Colour4.White;
        readySetGoText.Text = rounds == 1 ? "Game over!" : "Time out!";
        readySetGoText.Pop(600, 1000);

        Scheduler.AddDelayed(() =>
        {
            if (currentPlayer == (isTwoPlayers ? 2 : 1))
                rounds--;

            if (rounds == 0)
            {
                this.Push(new ResultsScreen(mode, pointsArray, isTwoPlayers));
            }
            else
                startRound();
        }, round_transition_delay);

        roundInProgress.Value = false;

        players.MoveToY(players.Height, TRANSITION_DURATION, Easing.OutQuint).FadeOut(TRANSITION_DURATION, Easing.OutQuint);

        if (basketballsVisible == 0)
        {
            hoop.Hide();
            pointsArray[currentPlayer - 1] = points.Value;
        }
    }

    private void spawnBasketBall(IDrawable player, bool extraPoint, int playerIndex)
    {
        basketballsVisible++;
        throwSample.Play();

        var basketball = new Basketball
        {
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomCentre,
        };

        if (combo.Value >= 3)
        {
            basketball.EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Glow,
                Radius = 20,
                Colour = Colour4.Red,
                Hollow = true,
            };
        }

        if (points.Value >= 150)
        {
            // TODO: implement proper "frogs" or whatever
            basketball.Colour = Colour4.Green;
        }

        gameContainer.Add(basketball);

        basketball.X = gameContainer.ToLocalSpace(player.ScreenSpaceDrawQuad.Centre).X;

        if (bonusTarget != null && bonusTargetPos == playerIndex && !bonusTargetHit)
        {
            bonusTargetHit = true;

            basketball.MoveToY(hoop_y_pos, 500, Easing.OutQuad).Then().OnComplete(b =>
            {
                basketballsVisible--;

                b.FadeOut(300, Easing.OutQuint).Expire();

                bonusTarget.FadeOut(300, Easing.InQuint).ScaleTo(1.5f, 300, Easing.OutQuad).OnComplete(_ =>
                {
                    points.Value += BonusTarget.POINTS;

                    pointEarnedText.Text = BonusTarget.POINTS.ToString();
                    pointEarnedText.Position = bonusTarget.Position + new Vector2(0, 100);
                    pointEarnedText.Pop();

                    bonusTarget = null;
                    bonusTargetHit = false;
                });
            });

            return;
        }

        basketball.MoveToY(hoop_y_pos, 1000, Easing.OutBack)
                  .ScaleTo(0.9f, 1000)
                  .OnComplete(b => scorePoints(b, extraPoint));
    }

    private void scorePoints(Basketball basketball, bool extraPoint)
    {
        // TODO: implement logic when ball hits rim
        const int lenience_range = 50;

        var difference = basketball.X - hoopContainer.X;

        if (Math.Abs(difference) <= lenience_range)
        {
            var channel1 = scoreSample.GetChannel();
            channel1.Volume.Value = 0.30;
            channel1.Play();

            // based on original game
            int earnedPoints = Math.Clamp(combo.Value, extraPoint ? 3 : 2, 5);

            points.Value += earnedPoints;

            pointEarnedText.Text = earnedPoints.ToString();
            pointEarnedText.Position = hoopContainer.Position + new Vector2(0, 100);
            pointEarnedText.Pop();

            combo.Value++;
        }
        else
        {
            combo.Value = 0;
            if (mode == GameplayMode.Endless)
                endRound();
        }

        basketball.FadeOut(300, Easing.OutQuint).OnComplete(b =>
        {
            basketballsVisible--;

            if (!roundInProgress.Value && basketballsVisible == 0)
            {
                hoop.Hide();
                pointsArray[currentPlayer - 1] = points.Value;
            }

            b.Expire();
        });
    }
}
