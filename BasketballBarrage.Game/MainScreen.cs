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

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Spacing = new Vector2(25),
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
                            Size = new Vector2(150, 50),
                            Text = "Play",
                            Action = () => this.Push(new GameplayScreen())
                        },
                        new GameButton
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Size = new Vector2(150, 50),
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
