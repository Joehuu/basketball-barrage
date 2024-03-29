using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace BasketballBarrage.Game;

/// <summary>
/// A target that appears randomly and is worth <see cref="POINTS"/>.
/// </summary>
/// <remarks>
/// Needs to be a buffered container for fade to show correctly. Not overly concerned on performance right now.
/// </remarks>
public partial class BonusTarget : BufferedContainer
{
    public const int POINTS = 10;

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
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Radius = 6,
                    Colour = Colour4.Black,
                    Hollow = true,
                },
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
