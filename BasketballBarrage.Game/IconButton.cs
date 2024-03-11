using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace BasketballBarrage.Game;

public partial class IconButton : GameButton
{
    public IconButton()
    {
        Size = new Vector2(50);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(new SpriteIcon
        {
            Size = new Vector2(20),
            Icon = Icon,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        });
    }

    public IconUsage Icon { get; init; }
}
