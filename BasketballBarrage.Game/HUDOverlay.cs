using System.IO;
using System.Linq;
using BasketballBarrage.Game.Database;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Realms;

namespace BasketballBarrage.Game;

public partial class HUDOverlay : Container
{
    private SpriteText timerText = null!;
    private SpriteIcon infiniteSign = null!;
    private StatisticCounter highScoreCounter = null!;

    private int highScore;
    private bool newHighScoreAchieved;

    public readonly IBindable<int> Combo = new Bindable<int>();
    public readonly IBindable<int> MaxCombo = new Bindable<int>();
    public readonly IBindable<int> Points = new Bindable<int>();

    public GameplayMode Mode { get; init; }

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;
        Padding = new MarginPadding(5);
        Children = new Drawable[]
        {
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5),
                Children = new Drawable[]
                {
                    // original game calls these run and high run, but call them combo for now
                    new StatisticCounter("Combo")
                    {
                        CounterValue = { BindTarget = Combo }
                    },
                    new StatisticCounter("Max Combo")
                    {
                        CounterValue = { BindTarget = MaxCombo }
                    },
                }
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Children = new Drawable[]
                {
                    highScoreCounter = new StatisticCounter("High Score")
                    {
                        CounterValue = { BindTarget = getHighScore() },
                    },
                    new StatisticCounter("Score")
                    {
                        CounterValue = { BindTarget = Points }
                    },
                }
            },
            new CircularContainer
            {
                Size = new Vector2(75),
                Masking = true,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.5f,
                        Colour = Colour4.Black,
                    },
                    timerText = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = FontUsage.Default.With(size: 50),
                        Alpha = 0,
                    },
                    infiniteSign = new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = FontAwesome.Solid.Infinity,
                        Alpha = 0,
                        Size = new Vector2(30),
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Points.BindValueChanged(p =>
        {
            if (p.NewValue > highScore && !newHighScoreAchieved)
            {
                highScoreCounter.CounterValue.BindTo(Points);
                highScoreCounter.CounterColour = Colour4.LimeGreen;
                newHighScoreAchieved = true;
            }
        });
    }

    public void ResetTimer(GameplayMode mode)
    {
        if (mode == GameplayMode.Classic)
        {
            timerText.Alpha = 1;
            timerText.Text = GameplayScreen.CLASSIC_ROUND_TIME.ToString();
        }
        else
            infiniteSign.Alpha = 1;
    }

    public void UpdateTimer(string secondsLeft)
    {
        timerText.Text = secondsLeft;
    }

    private IBindable<int> getHighScore()
    {
        var realm = Realm.GetInstance($"{Directory.GetCurrentDirectory()}/client.realm");

        var scores = realm.All<Score>().Where(s => s.Mode == Mode.ToString()).OrderByDescending(s => s.Points);

        highScore = scores.FirstOrDefault()?.Points ?? 0;

        return new Bindable<int>(highScore);
    }
}
