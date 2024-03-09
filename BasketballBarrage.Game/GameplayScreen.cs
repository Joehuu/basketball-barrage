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
using osuTK;

namespace BasketballBarrage.Game;

public partial class GameplayScreen : GameScreen
{
    private Players players = null!;

    private readonly Bindable<bool> gameInProgress = new Bindable<bool>();

    private readonly Bindable<int> points = new Bindable<int>();

    private readonly Bindable<int> combo = new Bindable<int>();

    private readonly Bindable<int> maxCombo = new Bindable<int>();

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

    public const float GAME_WIDTH = 800;
    private const int hoop_y_pos = -500;
    private const int round_transition_delay = 2000;
    public const int CLASSIC_ROUND_TIME = 30;

    public GameplayScreen(GameplayMode mode)
    {
        this.mode = mode;

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
                        GameInProgress = { BindTarget = gameInProgress },
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

        startGame();

        combo.BindValueChanged(c =>
        {
            maxCombo.Value = Math.Max(maxCombo.Value, c.NewValue);
        });
    }

    private void startGame()
    {
        prepareForRound();
        displayInstruction();

        // TODO: speed up when combo increases
        Scheduler.AddDelayed(() =>
        {
            hoopContainer.MoveToX(GAME_WIDTH, 3000).Then()
                         .MoveToX(0, 6000).Then()
                         .MoveToX(GAME_WIDTH / 2f, 3000).Then()
                         .Loop();

            gameInProgress.Value = true;

            if (mode == GameplayMode.Classic)
            {
                startTime = DateTime.Now;

                // based on original game
                endTime = startTime + TimeSpan.FromSeconds(CLASSIC_ROUND_TIME + 1);
            }
        }, round_transition_delay);
    }

    private void prepareForRound()
    {
        hudOverlay.ResetTimer(mode);

        combo.Value = 0;
        hoopContainer.MoveToX(GAME_WIDTH / 2f);
        players.FadeIn(TRANSITION_DURATION, Easing.OutQuint).MoveToY(players.Height).MoveToY(0, TRANSITION_DURATION, Easing.OutQuint);
        hoop.Show();
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

        if (gameInProgress.Value)
            spawnBonusTarget();

        if (mode == GameplayMode.Classic && gameInProgress.Value)
        {
            var secondsLeft = (endTime - DateTime.Now).Seconds;

            hudOverlay.UpdateTimer(secondsLeft.ToString());

            if (secondsLeft <= 0)
                endGame();
        }
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

    private void endGame()
    {
        if (!gameInProgress.Value) return;

        readySetGoText.Colour = Colour4.White;
        readySetGoText.Text = rounds == 1 ? "Game over!" : "Time out!";
        readySetGoText.Pop(600, 1000);

        Scheduler.AddDelayed(() =>
        {
            rounds--;

            if (rounds == 0)
            {
                this.Push(new ResultsScreen(mode)
                {
                    Points = { BindTarget = points }
                });
            }
            else
                startGame();
        }, round_transition_delay);

        gameInProgress.Value = false;

        players.MoveToY(players.Height, TRANSITION_DURATION, Easing.OutQuint).FadeOut(TRANSITION_DURATION, Easing.OutQuint);

        if (basketballsVisible == 0)
            hoop.Hide();
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

            basketball.MoveToY(hoop_y_pos, 1000, Easing.OutQuint).Then().FadeOut().Expire();
            bonusTarget.FadeOut(1000, Easing.InQuint).ScaleTo(1.2f, 1000, Easing.OutQuint).OnComplete(_ =>
            {
                const int earned_points = 10;
                points.Value += earned_points;

                pointEarnedText.Text = earned_points.ToString();
                pointEarnedText.Position = bonusTarget.Position + new Vector2(0, 100);
                pointEarnedText.Pop();

                bonusTarget = null;
                bonusTargetHit = false;
            });

            return;
        }

        basketball.MoveToY(hoop_y_pos, 1000, Easing.OutBack)
                  .ScaleTo(0.9f, 1000)
                  .OnComplete(b => scorePoints(b, extraPoint));
    }

    private void scorePoints(Basketball basketball, bool extraPoint)
    {
        // TODO: implement logic when ball hits hoop
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
                endGame();
        }

        basketball.FadeOut(300, Easing.OutQuint).OnComplete(b =>
        {
            basketballsVisible--;

            if (!gameInProgress.Value && basketballsVisible == 0)
                hoop.Hide();

            b.Expire();
        });
    }
}
