using System.Configuration;
using System.Windows;

namespace ArnoldVinkTools
{
    partial class MainPage : Window
    {
        //Application Variables
        Configuration vConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        long vWallpaperFilesize = 0;
        bool vCheckingForUpdate = false;
    }
}