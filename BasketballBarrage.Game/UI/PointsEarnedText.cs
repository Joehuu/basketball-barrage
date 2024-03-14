using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace BasketballBarrage.Game.UI;

public partial class PointsEarnedText : SpriteText
{
    public PointsEarnedText()
    {
        Anchor = Anchor.BottomLeft;
        Origin = Anchor.BottomCentre;
        Font = FontUsage.Default.With(size: 50);
        Alpha = 0;
        Shadow = true;
    }

    public void PopPoints(int points, Vector2 position)
    {
        Text = points.ToString();
        Position = position;
        this.Pop();
    }
}
