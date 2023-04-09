using osu.Framework.Graphics;

namespace BasketballBarrage.Game;

public static class TransformExtensions
{
    public static void Pop(this Drawable drawable)
    {
        drawable.FadeIn(300, Easing.OutQuint).Then()
                .FadeOut(500, Easing.OutQuint);

        drawable.ScaleTo(1.2f, 300, Easing.OutQuint).Then()
                .ScaleTo(1f, 500, Easing.OutQuint);
    }
}
