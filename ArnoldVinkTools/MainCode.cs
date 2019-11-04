using ArnoldVinkCode;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Application Startup
        public async Task MainStartup()
        {
            //Initialize application
            await StartupCheck();
            TrayMenu();

            //Check application settings
            Settings_Check();
            Settings_Load();
            Settings_Save();

            //Load help text
            Help_Load();

            //Whitelist supported windows store apps
            WhitelistApps();

            //Enable the socket server
            vSocketServer.vTcpListenerPort = 1000;
            await vSocketServer.SocketServerSwitch(false, false);
            vSocketServer.EventBytesReceived += ReceivedSocketHandler;

            //Start checking for TimeMe wallpaper
            if (ConfigurationManager.AppSettings["TimeMeWallpaper"] == "True")
            {
                vCheckWallpaperToken = new CancellationTokenSource();
                vCheckWallpaperTask = AVActions.TaskStart(StartCheckWallpaper, vCheckWallpaperToken);
            }
        }

        //Check for application update
        private async void btn_CheckVersion_Click(object sender, RoutedEventArgs e)
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
                            TrayNotifyIcon.Visible = false;
                            Process.Start(Directory.GetCurrentDirectory() + "\\Updater.exe");
                            Environment.Exit(0);
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
        private void btn_RestartTools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TrayNotifyIcon.Visible = false;
                Process.Start(Assembly.GetEntryAssembly().Location);
                Environment.Exit(0);
            }
            catch { }
        }

        //Exit Arnold Vink Tools
        private void btn_ExitTools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TrayNotifyIcon.Visible = false;
                Environment.Exit(0);
            }
            catch { }
        }

        //Open the project website
        private void btn_ProjectWebsite_Click(object sender, RoutedEventArgs e) { Process.Start("http://projects.arnoldvink.com"); }

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