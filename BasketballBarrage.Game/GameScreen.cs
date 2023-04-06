using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace BasketballBarrage.Game;

public partial class GameScreen : Screen
{
    public override void OnEntering(ScreenTransitionEvent e)
    {
        this.FadeIn(250, Easing.OutQuint);
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        this.FadeOut(250, Easing.OutQuint);

        return base.OnExiting(e);
    }
}
