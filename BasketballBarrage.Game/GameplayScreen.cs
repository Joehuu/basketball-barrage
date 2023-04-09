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

    public const float GAME_WIDTH = 800;
    private const int hoop_y_pos = -500;

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
                        GameInProgress = { BindTarget = gameInProgress }
                    },
                    hoop = new Hoop
                    {
                        Combo = { BindTarget = combo },
                        X = GAME_WIDTH / 2f,
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
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    // original game calls these run and high run, but call them combo for now
                    new StatisticCounter("Combo")
                    {
                        CounterValue = { BindTarget = combo }
                    },
                    new StatisticCounter("Max Combo")
                    {
                        CounterValue = { BindTarget = maxCombo }
                    },
                }
            },
            new StatisticCounter("Points")
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                CounterValue = { BindTarget = points }
            },
            readySetGoText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = FontUsage.Default.With(size: 50),
                Alpha = 0,
                Shadow = true,
            }
        };

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            var isEdge = i == 0 || i == players.Count - 1;
            player.ShootBasketball = () => spawnBasketBall(player, isEdge);
        }
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

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

        // TODO: speed up when combo increases
        Scheduler.AddDelayed(() =>
        {
            readySetGoText.Colour = Colour4.Green;
            readySetGoText.Text = "Go!";
            readySetGoText.Pop();

            hoop.MoveToX(GAME_WIDTH, 3000).Then()
                .MoveToX(0, 6000).Then()
                .MoveToX(GAME_WIDTH / 2f, 3000).Then()
                .Loop();

            gameInProgress.Value = true;
        }, 2000);

        combo.BindValueChanged(c =>
        {
            maxCombo.Value = Math.Max(maxCombo.Value, c.NewValue);
        });
    }

    private void spawnBasketBall(IDrawable player, bool extraPoint)
    {
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

        basketball.MoveToY(hoop_y_pos, 1000, Easing.OutBack)
                  .ScaleTo(0.9f, 1000)
                  .OnComplete(b => scorePoints(b, extraPoint));
    }

    private void scorePoints(Basketball basketball, bool extraPoint)
    {
        // TODO: implement logic when ball hits hoop
        const int lenience_range = 50;

        var difference = basketball.X - hoop.X;

        if (Math.Abs(difference) <= lenience_range)
        {
            var channel1 = scoreSample.GetChannel();
            channel1.Volume.Value = 0.30;
            channel1.Play();

            // based on original game
            int earnedPoints = Math.Clamp(combo.Value, extraPoint ? 3 : 2, 5);

            points.Value += earnedPoints;

            pointEarnedText.Text = earnedPoints.ToString();
            pointEarnedText.Position = hoop.Position + new Vector2(0, 100);
            pointEarnedText.Pop();

            combo.Value++;
        }
        else
            combo.Value = 0;

        basketball.FadeOut(300, Easing.OutQuint).Expire();
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        players.MoveToY(players.Height).MoveToY(0, TRANSITION_DURATION, Easing.OutQuint);
        hoop.MoveToY(hoop_y_pos - hoop.Height).MoveToY(hoop_y_pos, TRANSITION_DURATION, Easing.OutQuint);
    }
}
