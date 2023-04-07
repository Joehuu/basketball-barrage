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
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;

namespace BasketballBarrage.Game;

public partial class GameplayScreen : GameScreen
{
    private Players players = null!;

    private readonly Bindable<int> points = new Bindable<int>();

    private readonly Bindable<int> combo = new Bindable<int>();
    private int maxCombo;

    private Hoop hoop = null!;
    private SpriteText pointEarnedText = null!;
    private Container gameContainer = null!;
    private StatisticCounter pointStatisticCounter = null!;
    private StatisticCounter comboStatisticCounter = null!;
    private StatisticCounter maxComboStatisticCounter = null!;
    private Sample scoreSample = null!;
    private Sample throwSample = null!;

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
                    comboStatisticCounter = new StatisticCounter("Combo"),
                    maxComboStatisticCounter = new StatisticCounter("Max Combo"),
                }
            },
            pointStatisticCounter = new StatisticCounter("Points")
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            },
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

        // TODO: speed up when combo increases
        Scheduler.AddDelayed(() =>
        {
            hoop.MoveToX(GAME_WIDTH, 3000).Then()
                .MoveToX(0, 6000).Then()
                .MoveToX(GAME_WIDTH / 2f, 3000).Then()
                .Loop();
        }, 500);

        points.BindValueChanged(p =>
        {
            pointStatisticCounter.CounterText = p.NewValue.ToString();
        });

        combo.BindValueChanged(c =>
        {
            comboStatisticCounter.CounterText = c.NewValue.ToString();

            maxCombo = Math.Max(maxCombo, c.NewValue);
            maxComboStatisticCounter.CounterText = maxCombo.ToString();
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

            pointEarnedText.FadeIn(300, Easing.OutQuint).Then()
                           .FadeOut(500, Easing.OutQuint);

            pointEarnedText.ScaleTo(1.2f, 300, Easing.OutQuint).Then()
                           .ScaleTo(1f, 500, Easing.OutQuint);

            combo.Value++;
        }
        else
            combo.Value = 0;

        basketball.FadeOut(300, Easing.OutQuint).Expire();
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        players.MoveToY(players.Height).MoveToY(0, 500, Easing.OutQuint);
        hoop.MoveToY(hoop_y_pos - hoop.Height).MoveToY(hoop_y_pos, 500, Easing.OutQuint);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                this.Exit();
                break;
        }

        return base.OnKeyDown(e);
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        switch (e.Button)
        {
            case MouseButton.Button1:
                this.Exit();
                break;
        }

        return base.OnMouseDown(e);
    }
}
