using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;

namespace BasketballBarrage.Game;

public partial class ModeScreen : GameScreen
{
    private FillFlowContainer flow = null!;

    public ModeScreen()
    {
        ValidForResume = false;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Spacing = new Vector2(25),
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Pick a mode",
                        Font = FontUsage.Default.With(size: 40),
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(25),
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Children = new Drawable[]
                        {
                            new GameButton
                            {
                                Size = new Vector2(150, 50),
                                Text = "Classic"
                            },
                            new GameButton
                            {
                                Size = new Vector2(150, 50),
                                Text = "Endless",
                                Action = () => this.Push(new GameplayScreen()),
                            },
                        }
                    }
                }
            }
        };
    }

    public override void OnEntering(ScreenTransitionEvent e)
    {
        base.OnEntering(e);

        flow.TransformSpacingTo(new Vector2(150)).TransformSpacingTo(new Vector2(25), TRANSITION_DURATION, Easing.OutQuint);
    }
}