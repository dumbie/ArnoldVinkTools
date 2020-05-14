using ArnoldVinkCode;
using System.Configuration;
using System.Globalization;

namespace ArnoldVinkTools
{
    partial class AppVariables
    {
        //Application Variables
        public static Configuration vConfigurationApplication = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static CultureInfo vAppCultureInfo = CultureInfo.InvariantCulture;

        //Wallpaper Variables
        public static long vWallpaperFilesize = 0;

        //Update Variables
        public static bool vCheckingForUpdate = false;

        //Sockets Variables
        public static ArnoldVinkSockets vArnoldVinkSockets = null;
    }
}