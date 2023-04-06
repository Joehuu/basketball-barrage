using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace BasketballBarrage.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneStatisticCounter : BasketballBarrageTestScene
    {
        public TestSceneStatisticCounter()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DimGray,
                },
                new StatisticCounter("Test")
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };
        }
    }
}
