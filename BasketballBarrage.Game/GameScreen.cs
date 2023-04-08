using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace BasketballBarrage.Game;

public partial class GameScreen : Screen
{
    protected const float TRANSITION_DURATION = 500;

    public override void OnEntering(ScreenTransitionEvent e)
    {
        this.FadeIn(TRANSITION_DURATION, Easing.OutQuint);
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);

        return base.OnExiting(e);
    }
}
