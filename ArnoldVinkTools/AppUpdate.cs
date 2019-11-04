using ArnoldVinkCode;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.ProcessWin32Functions;
using static ArnoldVinkTools.AppVariables;

namespace ArnoldVinkTools
{
    class AppUpdate
    {
        //Check for available application update
        public static async Task CheckForAppUpdate(bool Silent)
        {
            try
            {
                if (!vCheckingForUpdate)
                {
                    vCheckingForUpdate = true;

                    //Download Current Version
                    string ResCurrentVersion = await AVDownloader.DownloadStringAsync(5000, "Arnold Vink Tools", null, new Uri("http://download.arnoldvink.com/ArnoldVinkTools.zip-version.txt" + "?nc=" + Environment.TickCount));
                    if (ResCurrentVersion != Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0])
                    {
                        MessageBoxResult Result = MessageBox.Show("A newer version has been found: v" + ResCurrentVersion + ", do you want to update the application to the newest version now?", "Arnold Vink Tools", MessageBoxButton.YesNo);
                        if (Result == MessageBoxResult.Yes)
                        {
                            await ProcessLauncherWin32Async("Updater.exe", "", "", false, false);
                            await App.vMainPage.Application_Exit();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No new update has been found.", "Arnold Vink Tools");
                    }

                    vCheckingForUpdate = false;
                }
            }
            catch
            {
                vCheckingForUpdate = false;
                MessageBox.Show("Failed to check for the latest application version,\nplease check your internet connection and try again.", "Arnold Vink Tools");
            }
        }
    }
}