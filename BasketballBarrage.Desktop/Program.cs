using osu.Framework.Platform;
using osu.Framework;
using BasketballBarrage.Game;

namespace BasketballBarrage.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"BasketballBarrage"))
            using (osu.Framework.Game game = new BasketballBarrageGame())
                host.Run(game);
        }
    }
}
