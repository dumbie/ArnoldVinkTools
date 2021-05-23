using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
                await Application_LaunchCheck("Arnold Vink Tools", "ArnoldVinkTools", ProcessPriorityClass.Normal, false);

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

                //Start the background tasks
                TasksBackgroundStart();

                //Enable the socket server
                await EnableSocketServer();

                //Check for available application update
                await AppUpdate.CheckForAppUpdate(true);
            }
            catch { }
        }

        //Enable the socket server
        private async Task EnableSocketServer()
        {
            try
            {
                int SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

                vArnoldVinkSockets = new ArnoldVinkSockets("127.0.0.1", SocketServerPort, true, false);
                vArnoldVinkSockets.vSocketTimeout = 2000;
                vArnoldVinkSockets.EventBytesReceived += ReceivedSocketHandler;
                await vArnoldVinkSockets.SocketServerEnable();
            }
            catch { }
        }

        //Application Exit
        public async Task Application_Exit()
        {
            try
            {
                Debug.WriteLine("Exiting application.");

                //Stop the background tasks
                await TasksBackgroundStop();

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

        //Show the device detected ip addresses
        private async void btn_ShowIpAdres_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string LocalIpAdresses = string.Empty;

                IPHostEntry IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress IPAddress in IPHostEntry.AddressList)
                {
                    if (IPAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalIpAdresses += IPAddress.ToString() + ", ";
                    }
                }

                if (LocalIpAdresses == string.Empty) { LocalIpAdresses = "Unknown"; }
                else { LocalIpAdresses = AVFunctions.StringRemoveEnd(LocalIpAdresses, ", "); }

                List<string> messageAnswers = new List<string>();
                messageAnswers.Add("Ok");

                await new AVMessageBox().Popup(this, "Device IP addresses", "Currently detected IP addresses: " + LocalIpAdresses, messageAnswers);
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