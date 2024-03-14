using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace BasketballBarrage.Game;

public partial class Basketball : CircularContainer
{
    private const int line_thickness = 3;

    protected Colour4 BaseColour => IsInteractive ? Colour4.Black : Colour4.OrangeRed;
    public bool IsFrog { get; init; }
    public bool IsInteractive { get; init; }

    public override bool HandlePositionalInput => IsInteractive;

    public Basketball()
    {
        Size = new Vector2(90);
        Masking = true;
        BorderColour = Colour4.Black;
        BorderThickness = line_thickness;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            ballColour = new Box
            {
                RelativeSizeAxes = Axes.Both,
                // TODO: implement proper "frogs" or whatever
                Colour = IsFrog ? Colour4.Green : BaseColour,
            },
            verticalLine = new Box
            {
                Width = line_thickness,
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = IsInteractive ? Colour4.OrangeRed : Colour4.Black
            },
            horizontalLine = new Box
            {
                Height = line_thickness,
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = IsInteractive ? Colour4.OrangeRed : Colour4.Black
            },
            leftCurve = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                X = -15 * Width / 24,
                Masking = true,
                BorderColour = IsInteractive ? Colour4.OrangeRed : Colour4.Black,
                BorderThickness = line_thickness,
                Child = new Box
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.Centre,
                    Size = new Vector2(68),
                    AlwaysPresent = true,
                    Alpha = 0,
                    Rotation = 45,
                }
            },
            rightCurve = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                X = 15 * Width / 24,
                Masking = true,
                BorderColour = IsInteractive ? Colour4.OrangeRed : Colour4.Black,
                BorderThickness = line_thickness,
                Child = new Box
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    Size = new Vector2(68),
                    AlwaysPresent = true,
                    Alpha = 0,
                    Rotation = 45,
                }
            },
        };
    }

    private Box ballColour = null!;
    private Box verticalLine;
    private Box horizontalLine;
    private CircularContainer leftCurve;
    private CircularContainer rightCurve;

    protected override bool OnHover(HoverEvent e)
    {
        ballColour.FadeColour(Colour4.OrangeRed, 500, Easing.OutQuint);
        verticalLine.Colour = horizontalLine.Colour = leftCurve.BorderColour = rightCurve.BorderColour = Colour4.Black;

        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        ballColour.FadeColour(Colour4.Black, 500, Easing.OutQuint);
        verticalLine.Colour = horizontalLine.Colour = leftCurve.BorderColour = rightCurve.BorderColour = Colour4.OrangeRed;
    }
}
