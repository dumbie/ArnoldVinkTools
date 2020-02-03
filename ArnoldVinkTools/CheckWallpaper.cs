using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static AppImport.AppImport;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkTools.AppVariables;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Start checking for TimeMe wallpaper
        async void StartCheckWallpaper()
        {
            try
            {
                Debug.WriteLine("Checking for TimeMe wallpaper...");
                while (TaskRunningCheck(vCheckWallpaperToken))
                {
                    //Check for current jpg wallpaper file
                    string WallpaperLocationJpg = Environment.GetEnvironmentVariable("LocalAppData") + @"\Packages\54655ArnoldVink.TimeMeTile_hky69t2svm98c\LocalState\TimeMeTilePhoto.jpg";
                    CheckAndSetWallpaper(WallpaperLocationJpg);

                    //Check for current png wallpaper file
                    string WallpaperLocationPng = Environment.GetEnvironmentVariable("LocalAppData") + @"\Packages\54655ArnoldVink.TimeMeTile_hky69t2svm98c\LocalState\TimeMeTilePhoto.png";
                    CheckAndSetWallpaper(WallpaperLocationPng);

                    await Task.Delay(600000);
                }
            }
            catch { }
        }

        private void CheckAndSetWallpaper(string WallpaperLocation)
        {
            try
            {
                if (File.Exists(WallpaperLocation))
                {
                    //Set and check current wallpaper file size
                    long WallpaperFilesizeOld = vWallpaperFilesize;
                    vWallpaperFilesize = new FileInfo(WallpaperLocation).Length;
                    if (WallpaperFilesizeOld != vWallpaperFilesize)
                    {
                        //Set Registery to Stretch
                        RegistryKey WallRegistryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                        WallRegistryKey.SetValue("WallpaperStyle", "2");
                        WallRegistryKey.SetValue("TileWallpaper", "0");

                        //Set current TimeMe Wallpaper
                        SystemParametersInfo(20, 0, WallpaperLocation, 0x1);

                        //Update TimeMe Preview
                        Dispatcher.Invoke(delegate ()
                        {
                            sp_TimeMeWallpaper.Visibility = Visibility.Visible;

                            //Load the wallpaper as bitmapimage
                            BitmapImage ImageToBitmapImage = new BitmapImage();
                            ImageToBitmapImage.BeginInit();
                            ImageToBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            ImageToBitmapImage.UriSource = new Uri(WallpaperLocation, UriKind.RelativeOrAbsolute);
                            ImageToBitmapImage.EndInit();
                            image_TimeMeWallpaper.Source = ImageToBitmapImage;
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(delegate () { sp_TimeMeWallpaper.Visibility = Visibility.Collapsed; });
                    }
                }
            }
            catch { }
        }
    }
}