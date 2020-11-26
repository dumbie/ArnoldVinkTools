using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.ApiGitHub;
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

                    string onlineVersion = await ApiGitHub_GetLatestVersion("dumbie", "ArnoldVinkTools");
                    string currentVersion = "v" + Assembly.GetEntryAssembly().FullName.Split('=')[1].Split(',')[0];
                    if (!string.IsNullOrWhiteSpace(onlineVersion) && onlineVersion != currentVersion)
                    {
                        MessageBoxResult Result = MessageBox.Show("A newer version has been found: " + onlineVersion + ", do you want to update the application to the newest version now?", "Arnold Vink Tools", MessageBoxButton.YesNo);
                        if (Result == MessageBoxResult.Yes)
                        {
                            await ProcessLauncherWin32Async("Updater.exe", "", "", false, false);
                            await App.vMainPage.Application_Exit();
                        }
                    }
                    else
                    {
                        if (!Silent)
                        {
                            MessageBox.Show("No new application update has been found.", "Arnold Vink Tools");
                        }
                    }

                    vCheckingForUpdate = false;
                }
            }
            catch
            {
                vCheckingForUpdate = false;
                if (!Silent)
                {
                    MessageBox.Show("Failed to check for the latest application version,\nplease check your internet connection and try again.", "Arnold Vink Tools");
                }
            }
        }
    }
}