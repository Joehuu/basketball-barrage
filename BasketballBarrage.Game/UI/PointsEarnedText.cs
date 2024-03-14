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
        switch (points)
        {
            case 2:
                Colour = Colour4.LightYellow;
                break;

            case 3:
                Colour = Colour4.Yellow;
                break;

            case 4:
                Colour = Colour4.Orange;
                break;

            case 5:
                Colour = Colour4.OrangeRed;
                break;

            default:
                Colour = Colour4.White;
                break;
        }

        Text = points.ToString();
        Position = position;
        this.Pop();
    }
}
