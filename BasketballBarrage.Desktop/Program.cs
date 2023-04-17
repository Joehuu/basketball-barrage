using osu.Framework.Platform;
using osu.Framework;

namespace BasketballBarrage.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"BasketballBarrage"))
            using (osu.Framework.Game game = new BasketballBarrageGameDesktop())
                host.Run(game);
        }
    }
}
