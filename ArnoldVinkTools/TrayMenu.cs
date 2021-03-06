﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Tray Variables
        public static NotifyIcon TrayNotifyIcon = new NotifyIcon();
        public static ContextMenu TrayContextMenu = new ContextMenu();

        void Application_CreateTrayMenu()
        {
            try
            {
                Debug.WriteLine("Creating application tray menu.");

                // Create a context menu for systray.  
                TrayContextMenu.MenuItems.Add("Settings", OnSettings);
                TrayContextMenu.MenuItems.Add("Website", OnWebsite);
                TrayContextMenu.MenuItems.Add("Exit", OnExit);

                // Initialize the tray notify icon. 
                TrayNotifyIcon.Text = "Arnold Vink Tools";
                TrayNotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("ArnoldVinkTools.Assets.AppIcon.ico"));

                // Handle Double Click event
                TrayNotifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);

                // Add menu to tray icon and show it.  
                TrayNotifyIcon.ContextMenu = TrayContextMenu;
                TrayNotifyIcon.Visible = true;
            }
            catch { }
        }

        void NotifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Showing the main window.");
                Show();
            }
            catch { }
        }

        void OnSettings(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Showing the main window.");
                Show();
            }
            catch { }
        }

        void OnWebsite(object sender, EventArgs e) { Process.Start("https://projects.arnoldvink.com"); }

        async void OnExit(object sender, EventArgs e)
        {
            try
            {
                await Application_Exit();
            }
            catch { }
        }
    }
}