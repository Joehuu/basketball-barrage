using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace BasketballBarrage.Game;

public partial class StatisticCounter : Container
{
    private SpriteText counterText = null!;
    private readonly string labelString;

    public string CounterText
    {
        set => counterText.Text = value;
    }

    public StatisticCounter(string labelString)
    {
        this.labelString = labelString;

        AutoSizeAxes = Axes.Both;

        Padding = new MarginPadding(5);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 10,
                Child = new Box
                {
                    Colour = Colour4.Black,
                    Alpha = 0.5f,
                    RelativeSizeAxes = Axes.Both,
                },
            },
            new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Padding = new MarginPadding(10),
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Font = FontUsage.Default.With(size: 35),
                        Text = labelString,
                    },
                    counterText = new SpriteText
                    {
                        Font = FontUsage.Default.With(size: 50),
                        Text = "0",
                    },
                }
            }
        };
    }
}
