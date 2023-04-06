using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace BasketballBarrage.Game;

public partial class GameButton : BasicButton
{
    public GameButton()
    {
        Masking = true;
        CornerRadius = 10;
        BackgroundColour = Colour4.OrangeRed;
    }

    protected override SpriteText CreateText() => new SpriteText
    {
        Origin = Anchor.Centre,
        Anchor = Anchor.Centre,
    };
}
