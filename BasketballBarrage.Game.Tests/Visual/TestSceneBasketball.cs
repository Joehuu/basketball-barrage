using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace BasketballBarrage.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneBasketball : BasketballBarrageTestScene
    {
        public TestSceneBasketball()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DimGray,
                },
                new Basketball
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };
        }
    }
}
