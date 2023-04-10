using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK;

namespace BasketballBarrage.Game;

public partial class ResultsScreen : GameScreen
{
    public IBindable<int> Points = new Bindable<int>();

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.DimGray,
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(25),
                Children = new Drawable[]
                {
                    new StatisticCounter("Points")
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        CounterValue = { BindTarget = Points },
                    },
                    new GameButton
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Back",
                        Action = this.Exit
                    },
                }
            }
        };
    }
}
