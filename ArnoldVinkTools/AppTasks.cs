using System.Threading;
using System.Threading.Tasks;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        public static Task vSocketServerTask;
        public static CancellationTokenSource vSocketServerToken;

        public static Task vCheckWallpaperTask;
        public static CancellationTokenSource vCheckWallpaperToken;
    }
}