using osu.Framework.Graphics;
using osu.Framework.Screens;
using NUnit.Framework;

namespace BasketballBarrage.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneMainScreen : BasketballBarrageTestScene
    {
        public TestSceneMainScreen()
        {
            Add(new ScreenStack(new MainScreen()) { RelativeSizeAxes = Axes.Both });
        }
    }
}
