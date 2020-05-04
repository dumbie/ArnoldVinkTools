using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.ProcessWin32Functions;
using static ArnoldVinkTools.AppLaunchCheck;
using static ArnoldVinkTools.AppVariables;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Application Startup
        public async Task Application_Startup()
        {
            try
            {
                //Initialize application
                Application_LaunchCheck("Arnold Vink Tools", "ArnoldVinkTools", false, false);

                //Create the tray menu
                Application_CreateTrayMenu();

                //Check application settings
                Settings_Check();
                Settings_Load();
                Settings_Save();

                //Load help text
                Help_Load();

                //Whitelist supported windows store apps
                WhitelistApps();

                //Enable the socket server
                EnableSocketServer();

                //Start checking for TimeMe wallpaper
                if (ConfigurationManager.AppSettings["TimeMeWallpaper"] == "True")
                {
                    vCheckWallpaperToken = new CancellationTokenSource();
                    vCheckWallpaperTask = AVActions.TaskStart(StartCheckWallpaper, vCheckWallpaperToken);
                }

                //Check for available application update
                if (DateTime.Now.Subtract(DateTime.Parse(ConfigurationManager.AppSettings["AppUpdateCheck"], vAppCultureInfo)).Days >= 5)
                {
                    await AppUpdate.CheckForAppUpdate(true);
                }

                Debug.WriteLine("Application has launched.");
            }
            catch { }
        }

        //Enable the socket server
        private void EnableSocketServer()
        {
            try
            {
                int SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", SocketServerPort);
                vArnoldVinkSockets.EventBytesReceived += ReceivedSocketHandler;
            }
            catch { }
        }

        //Application Exit
        public async Task Application_Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Disable the socket server
                if (vArnoldVinkSockets != null)
                {
                    await vArnoldVinkSockets.SocketServerDisable();
                }

                //Hide the tray icon
                TrayNotifyIcon.Visible = false;

                //Exit the application
                Environment.Exit(0);
            }
            catch { }
        }

        //Check for application update
        private async void btn_CheckVersion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AppUpdate.CheckForAppUpdate(false);
            }
            catch { }
        }

        //Show the PC's detected ip adres
        private void btn_ShowIpAdres_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string LocalIpAdresses = String.Empty;

                IPHostEntry IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress IPAddress in IPHostEntry.AddressList)
                {
                    if (IPAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalIpAdresses += IPAddress.ToString() + ", ";
                    }
                }

                if (LocalIpAdresses == String.Empty) { LocalIpAdresses = "Unknown"; }
                else { LocalIpAdresses = AVFunctions.StringRemoveEnd(LocalIpAdresses, ", "); }

                MessageBox.Show("Your PC's detected ip adres is currently set to: " + LocalIpAdresses, "Arnold Vink Tools");
            }
            catch { }
        }

        //Restart Arnold Vink Tools
        private async void btn_RestartTools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ProcessLauncherWin32Async("ArnoldVinkTools.exe", "", "-restart", false, false);
                await Application_Exit();
            }
            catch { }
        }

        //Exit Arnold Vink Tools
        private async void btn_ExitTools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Application_Exit();
            }
            catch { }
        }

        //Open the project website
        private void btn_ProjectWebsite_Click(object sender, RoutedEventArgs e) { Process.Start("https://projects.arnoldvink.com"); }

        //Hide the window instead of closing it
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                Debug.WriteLine("Hiding the main window.");
                Hide();
            }
            catch { }
        }
    }
}