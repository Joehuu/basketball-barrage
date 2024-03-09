using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace BasketballBarrage.Game;

public partial class Hoop : VisibilityContainer
{
    public readonly IBindable<int> Combo = new Bindable<int>();
    private FillFlowContainer flow = null!;

    public Hoop()
    {
        AutoSizeAxes = Axes.Both;
        Anchor = Anchor.BottomLeft;
        Origin = Anchor.BottomLeft;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Child = flow = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Children = new Drawable[]
            {
                new Container
                {
                    Size = new Vector2(125, 100),
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.5f),
                            BorderColour = Colour4.OrangeRed,
                            BorderThickness = 5,
                            Masking = true,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        }
                    }
                },
                new CircularContainer
                {
                    Size = new Vector2(100),
                    Masking = true,
                    BorderColour = Colour4.OrangeRed,
                    BorderThickness = 5,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        AlwaysPresent = true,
                        Alpha = 0,
                    }
                }
            }
        };
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Combo.BindValueChanged(combo =>
        {
            switch (combo.NewValue)
            {
                case >= 3:
                    flow.Children.ForEach(d =>
                    {
                        ((Container)d).EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Glow,
                            Radius = 20,
                            Colour = Colour4.Red,
                            Hollow = true,
                        };
                    });
                    break;

                default:
                    flow.Children.ForEach(d =>
                    {
                        ((Container)d).EdgeEffect = new EdgeEffectParameters
                        {
                            Type = EdgeEffectType.Shadow,
                            Radius = 5,
                            Colour = Colour4.Black,
                            Hollow = true,
                        };
                    });
                    break;
            }
        }, true);
    }

    protected override void PopIn() =>
        this.FadeIn(GameScreen.TRANSITION_DURATION, Easing.OutQuint).MoveToY(-Height).MoveToY(0, GameScreen.TRANSITION_DURATION, Easing.OutQuint);

    protected override void PopOut() =>
        this.MoveToY(0).MoveToY(-Height, GameScreen.TRANSITION_DURATION, Easing.OutQuint).FadeOut(GameScreen.TRANSITION_DURATION, Easing.OutQuint);
}
