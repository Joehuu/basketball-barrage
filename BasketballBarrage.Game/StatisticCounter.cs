using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace BasketballBarrage.Game;

public partial class StatisticCounter : Container
{
    private SpriteText? counterText;
    private readonly string labelString;

    public readonly IBindable<int> CounterValue = new Bindable<int>();

    public ColourInfo CounterColour
    {
        get => counterText?.Colour ?? Colour4.White;
        set
        {
            if (counterText != null) counterText.Colour = value;
        }
    }

    public StatisticCounter(string labelString)
    {
        this.labelString = labelString;

        AutoSizeAxes = Axes.Both;
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
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    counterText = new SpriteText
                    {
                        Font = FontUsage.Default.With(size: 50),
                        Text = "0",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Colour = CounterColour,
                    },
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        CounterValue.BindValueChanged(number =>
        {
            if (counterText != null) counterText.Text = number.NewValue.ToString();
        }, true);
    }
}
