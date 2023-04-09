using osu.Framework.Graphics;

namespace BasketballBarrage.Game;

public static class TransformExtensions
{
    public static void Pop(this Drawable drawable, float fadeInTime = 300, float fadeOutTime = 500)
    {
        drawable.FadeIn(fadeInTime, Easing.OutQuint).Then()
                .FadeOut(fadeOutTime, Easing.OutQuint);

        drawable.ScaleTo(1.2f, fadeInTime, Easing.OutQuint).Then()
                .ScaleTo(1f, fadeOutTime, Easing.OutQuint);
    }
}
