using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace BasketballBarrage.Game;

public partial class BonusTarget : CompositeDrawable
{
    public BonusTarget()
    {
        AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new CircularContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Scale = new Vector2(0.95f),
                Masking = true,
                RelativeSizeAxes = Axes.Both,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                }
            },
            new SpriteIcon
            {
                Icon = FontAwesome.Solid.Bullseye,
                Size = new Vector2(100),
                Colour = Colour4.Red,
            }
        };
    }
}
