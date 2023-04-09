using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace BasketballBarrage.Game;

public partial class Players : FillFlowContainer<Player>
{
    private readonly int count;

    public readonly Bindable<bool> GameInProgress = new Bindable<bool>();

    public Players(int count = 5)
    {
        this.count = count;
        AutoSizeAxes = Axes.Both;
        Spacing = new Vector2((GameplayScreen.GAME_WIDTH - Player.WIDTH * (count - 1)) / count);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        for (int i = 0; i < count; i++)
        {
            Add(new Player
            {
                GameInProgress = { BindTarget = GameInProgress }
            });
        }
    }

    protected override bool OnDragStart(DragStartEvent e) => true;

    protected override void OnDrag(DragEvent e)
    {
        base.OnDrag(e);

        Children.ForEach(p =>
        {
            if (p.IsHovered)
                p.TriggerClick();
        });
    }
}
