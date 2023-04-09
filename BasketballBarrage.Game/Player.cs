using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace BasketballBarrage.Game;

public partial class Player : ClickableContainer
{
    public Action? ShootBasketball;

    private double timeLastShot;

    public const float WIDTH = 100;

    public readonly Bindable<bool> GameInProgress = new Bindable<bool>();

    public Player()
    {
        AutoSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Child = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Children = new Drawable[]
            {
                new CircularContainer
                {
                    Size = new Vector2(WIDTH),
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Radius = 5,
                        Colour = Colour4.Black,
                    },
                    Child = new Box { RelativeSizeAxes = Axes.Both }
                },
                new Triangle
                {
                    Size = new Vector2(WIDTH),
                },
            }
        };

        Action = () =>
        {
            if (Time.Current - timeLastShot < 500 || !GameInProgress.Value) return;

            timeLastShot = Time.Current;
            ShootBasketball?.Invoke();
        };
    }

    private void updateState()
    {
        Colour = IsHovered ? Colour4.Aqua : Colour4.White;
    }

    protected override bool OnHover(HoverEvent e)
    {
        updateState();

        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        updateState();
    }
}
