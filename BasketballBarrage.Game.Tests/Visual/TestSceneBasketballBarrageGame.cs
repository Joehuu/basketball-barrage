using osu.Framework.Allocation;
using osu.Framework.Platform;
using NUnit.Framework;

namespace BasketballBarrage.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneBasketballBarrageGame : BasketballBarrageTestScene
    {
        private BasketballBarrageGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new BasketballBarrageGame();
            game.SetHost(host);

            AddGame(game);
        }
    }
}
