﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using static ArnoldVinkTools.AppVariables;

namespace ArnoldVinkTools
{
    partial class MainPage : Window
    {
        //Check - Application Settings
        void Settings_Check()
        {
            try
            {
                //Check - Server Port
                if (ConfigurationManager.AppSettings["ServerPort"] == null)
                {
                    vConfiguration.AppSettings.Settings.Remove("ServerPort");
                    vConfiguration.AppSettings.Settings.Add("ServerPort", "1000");
                }

                //Check - Last update check
                if (ConfigurationManager.AppSettings["AppUpdateCheck"] == null)
                {
                    vConfiguration.AppSettings.Settings.Remove("AppUpdateCheck");
                    vConfiguration.AppSettings.Settings.Add("AppUpdateCheck", DateTime.Now.ToString(vAppCultureInfo));
                }

                //Check - TimeMe Wallpaper
                if (ConfigurationManager.AppSettings["TimeMeWallpaper"] == null)
                {
                    vConfiguration.AppSettings.Settings.Remove("TimeMeWallpaper");
                    vConfiguration.AppSettings.Settings.Add("TimeMeWallpaper", "False");
                }

                vConfiguration.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }
        }

        //Load - Application Settings
        void Settings_Load()
        {
            try
            {
                //Load - Server Port
                txt_ServerPort.Text = ConfigurationManager.AppSettings["ServerPort"];

                //Load - TimeMe Wallpaper
                cb_TimeMeWallpaper.IsChecked = Convert.ToBoolean(ConfigurationManager.AppSettings["TimeMeWallpaper"]);

                //Check if application is set to launch on Windows startup
                string TargetName_Normal = Assembly.GetEntryAssembly().GetName().Name;
                string TargetFileStartup_Normal = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + TargetName_Normal + ".url";
                if (File.Exists(TargetFileStartup_Normal)) { cb_StartupWindows.IsChecked = true; }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("SettingsLoadError: " + Ex.Message, "Arnold Vink Tools");
            }
        }

        //Save - Application Settings
        void Settings_Save()
        {
            try
            {
                //Save - Server Port
                txt_ServerPort.TextChanged += async (sender, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txt_ServerPort.Text)) { return; }

                    if (Regex.IsMatch(txt_ServerPort.Text, "(\\D+)"))
                    {
                        MessageBox.Show("Please enter a valid server port number.", "Arnold Vink Tools");
                        return;
                    }

                    if (txt_ServerPort.Text.StartsWith("0"))
                    {
                        MessageBox.Show("Please enter a valid server port number,\nThe server port cannot start with a 0.", "Arnold Vink Tools");
                        return;
                    }

                    if (Convert.ToInt32(txt_ServerPort.Text) < 1 || Convert.ToInt32(txt_ServerPort.Text) > 65535)
                    {
                        MessageBox.Show("Please enter a valid server port number,\nYou can use a port between 1 and 65535.", "Arnold Vink Tools");
                        return;
                    }

                    vConfiguration.AppSettings.Settings.Remove("ServerPort");
                    vConfiguration.AppSettings.Settings.Add("ServerPort", txt_ServerPort.Text);
                    vConfiguration.Save();
                    ConfigurationManager.RefreshSection("appSettings");

                    //Restart the socket server
                    vArnoldVinkSockets.vTcpListenerPort = Convert.ToInt32(txt_ServerPort.Text);
                    await vArnoldVinkSockets.SocketServerRestart();
                };

                //Save - TimeMe Wallpaper
                cb_TimeMeWallpaper.Click += (sender, e) =>
                {
                    vConfiguration.AppSettings.Settings.Remove("TimeMeWallpaper");
                    vConfiguration.AppSettings.Settings.Add("TimeMeWallpaper", cb_TimeMeWallpaper.IsChecked.ToString());
                    vConfiguration.Save();
                    ConfigurationManager.RefreshSection("appSettings");
                };

                //Save - Windows Startup
                cb_StartupWindows.Click += (sender, e) => { ManageShortcutStartup(); };
            }
            catch (Exception Ex)
            {
                MessageBox.Show("SettingsSaveError: " + Ex.Message, "Arnold Vink Tools");
            }
        }

        //Create startup shortcut
        void ManageShortcutStartup()
        {
            try
            {
                //Set application shortcut paths
                string TargetFilePath = Assembly.GetEntryAssembly().CodeBase.Replace(".exe", "-Admin.exe");
                string TargetName = Assembly.GetEntryAssembly().GetName().Name;
                string TargetFileShortcut = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + TargetName + ".url";

                //Check if the shortcut already exists
                if (!File.Exists(TargetFileShortcut))
                {
                    Debug.WriteLine("Adding application to Windows startup.");
                    using (StreamWriter StreamWriter = new StreamWriter(TargetFileShortcut))
                    {
                        StreamWriter.WriteLine("[InternetShortcut]");
                        StreamWriter.WriteLine("URL=" + TargetFilePath);
                        StreamWriter.WriteLine("IconFile=" + TargetFilePath.Replace("file:///", ""));
                        StreamWriter.WriteLine("IconIndex=0");
                        StreamWriter.Flush();
                    }
                }
                else
                {
                    Debug.WriteLine("Removing application from Windows startup.");
                    if (File.Exists(TargetFileShortcut))
                    {
                        File.Delete(TargetFileShortcut);
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed creating startup shortcut.");
            }
        }
    }
}