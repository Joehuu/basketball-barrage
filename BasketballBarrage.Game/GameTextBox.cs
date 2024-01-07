using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace BasketballBarrage.Game;

public partial class GameTextBox : BasicTextBox
{
    public GameTextBox()
    {
        CornerRadius = 10;
    }

    protected override Drawable GetDrawableCharacter(char c) => new FallingDownContainer
    {
        AutoSizeAxes = Axes.Both,
        Child = new SpriteText { Text = c.ToString(), Font = FontUsage.Default.With(size: FontSize) }
    };

    protected override SpriteText CreatePlaceholder() => new FadingPlaceholderText
    {
        Anchor = Anchor.CentreLeft,
        Origin = Anchor.CentreLeft,
        X = CaretWidth,
    };
}
