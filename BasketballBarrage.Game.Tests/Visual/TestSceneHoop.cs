using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace BasketballBarrage.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneHoop : BasketballBarrageTestScene
    {
        public TestSceneHoop()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.DimGray,
                },
                new Hoop
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            };
        }
    }
}
