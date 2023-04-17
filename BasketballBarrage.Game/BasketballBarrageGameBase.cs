using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using BasketballBarrage.Resources;

namespace BasketballBarrage.Game
{
    public partial class BasketballBarrageGameBase : osu.Framework.Game
    {
        protected override Container<Drawable> Content { get; }

        protected BasketballBarrageGameBase()
        {
            Name = @"Basketball Barrage";

            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768)
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(BasketballBarrageResources).Assembly));
        }
    }
}
