using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace BasketballBarrage.Game;

public partial class Basketball : CircularContainer
{
    private const int line_thickness = 3;

    public Basketball()
    {
        Size = new Vector2(90);
        Masking = true;
        BorderColour = Colour4.Black;
        BorderThickness = line_thickness;
        Colour = Colour4.OrangeRed;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            new Box
            {
                RelativeSizeAxes = Axes.Both,
            },
            new Box
            {
                Width = line_thickness,
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Black
            },
            new Box
            {
                Height = line_thickness,
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.Black
            },
            new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                X = -15 * Width / 24,
                Masking = true,
                Colour = Colour4.Black,
                BorderThickness = line_thickness,
                Child = new Box
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(65),
                    AlwaysPresent = true,
                    Alpha = 0,
                }
            },
            new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                X = 15 * Width / 24,
                Masking = true,
                Colour = Colour4.Black,
                BorderThickness = line_thickness,
                Child = new Box
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(65),
                    AlwaysPresent = true,
                    Alpha = 0,
                }
            },
        };
    }
}
