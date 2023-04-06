using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace BasketballBarrage.Game
{
    public partial class BasketballBarrageGame : BasketballBarrageGameBase
    {
        private ScreenStack screenStack = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            screenStack.Push(new MainScreen());

            screenStack.ScreenExited += (_, newScreen) =>
            {
                if (newScreen == null)
                    Exit();
            };
        }
    }
}
