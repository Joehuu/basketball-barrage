using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Input;

namespace BasketballBarrage.Game
{
    public partial class MainScreen : GameScreen
    {
        private BasicButton playGameButton = null!;
        private FillFlowContainer flow = null!;

        protected override bool ExitViaShortcut => false;

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
                    Children = new Drawable[]
                    {
                        new MenuBasketball
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        },
                        new SpriteText
                        {
                            Text = "Basketball Barrage",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = FontUsage.Default.With(size: 40)
                        },
                        playGameButton = new GameButton
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = "Play",
                            Action = () => this.Push(new ModeScreen())
                        },
                        new GameButton
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = "Leaderboards",
                            Action = () => this.Push(new LeaderboardScreen())
                        },
                        new GameButton
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = "Quit",
                            Action = this.Exit
                        }
                    }
                }
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Space:
                case Key.Enter:
                    playGameButton.TriggerClick();
                    break;
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);

            transition();
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);

            transition();
        }

        private void transition()
        {
            flow.TransformSpacingTo(new Vector2(150)).TransformSpacingTo(new Vector2(25), TRANSITION_DURATION, Easing.OutQuint);
            flow.FadeInFromZero(TRANSITION_DURATION, Easing.OutQuint);
        }

        private partial class MenuBasketball : Basketball
        {
            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(Colour4.Green, 500, Easing.OutQuint);

                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);

                this.FadeColour(BaseColour, 500, Easing.OutQuint);
            }
        }
    }
}
