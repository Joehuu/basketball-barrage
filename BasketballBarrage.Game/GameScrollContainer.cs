using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace BasketballBarrage.Game;

public partial class GameScrollContainer : BasicScrollContainer
{
    public GameScrollContainer(Direction scrollDirection = Direction.Vertical)
        : base(scrollDirection)
    {
        Content.Anchor = Anchor.Centre;
        Content.Origin = Anchor.Centre;
    }

    protected override ScrollbarContainer CreateScrollbar(Direction direction) => new GameScrollbar(direction);

    private partial class GameScrollbar : BasicScrollbar
    {
        public GameScrollbar(Direction direction)
            : base(direction)
        {
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.OrangeRed,
            };
        }
    }
}
