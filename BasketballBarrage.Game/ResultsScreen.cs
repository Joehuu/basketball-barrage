using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

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
            new StatisticCounter("Points")
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                CounterValue = { BindTarget = Points },
            }
        };
    }
}
