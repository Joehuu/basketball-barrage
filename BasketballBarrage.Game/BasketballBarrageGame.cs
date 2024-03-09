using System.Linq;
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

            // some hacky workaround to hot reload not working with Realm installed
            // ref: https://github.com/realm/realm-dotnet/issues/3213
            _ = Enumerable.Range(0, 1).Where(c => c > 0).ToList().AsQueryable().FirstOrDefault();

            screenStack.Push(new MainScreen());

            screenStack.ScreenExited += (_, newScreen) =>
            {
                if (newScreen == null)
                    Exit();
            };
        }
    }
}
