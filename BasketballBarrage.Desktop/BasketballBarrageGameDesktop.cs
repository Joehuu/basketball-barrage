using BasketballBarrage.Game;
using osu.Framework.Platform;

namespace BasketballBarrage.Desktop;

internal partial class BasketballBarrageGameDesktop : BasketballBarrageGame
{
    public override void SetHost(GameHost host)
    {
        base.SetHost(host);

        host.Window.Title = Name;
    }
}
