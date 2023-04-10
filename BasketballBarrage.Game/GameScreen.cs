using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace BasketballBarrage.Game;

public partial class GameScreen : Screen
{
    public const float TRANSITION_DURATION = 500;

    protected virtual bool ExitViaShortcut => true;

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        this.FadeInFromZero(TRANSITION_DURATION, Easing.OutQuint);
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);

        return base.OnExiting(e);
    }

    public override void OnResuming(ScreenTransitionEvent e)
    {
        base.OnResuming(e);

        this.FadeIn(TRANSITION_DURATION, Easing.OutQuint);
    }

    public override void OnSuspending(ScreenTransitionEvent e)
    {
        base.OnSuspending(e);

        this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                exit();
                break;
        }

        return base.OnKeyDown(e);
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        switch (e.Button)
        {
            case MouseButton.Button1:
                exit();
                break;
        }

        return base.OnMouseDown(e);
    }

    private void exit()
    {
        if (ExitViaShortcut)
            this.Exit();
    }
}
