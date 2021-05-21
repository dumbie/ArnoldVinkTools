using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
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
                    SettingSave("ServerPort", "1000");
                }

                //Check - TimeMe Wallpaper
                if (ConfigurationManager.AppSettings["TimeMeWallpaper"] == null)
                {
                    SettingSave("TimeMeWallpaper", "False");
                }
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
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load the settings: " + ex.Message);
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
                    //Color brushes
                    BrushConverter BrushConvert = new BrushConverter();
                    Brush BrushInvalid = BrushConvert.ConvertFromString("#cd1a2b") as Brush;
                    Brush BrushValid = BrushConvert.ConvertFromString("#1db954") as Brush;

                    //Check text input and length
                    if (string.IsNullOrWhiteSpace(txt_ServerPort.Text)) { txt_ServerPort.BorderBrush = BrushInvalid; return; }

                    //Check text input has invalid characters
                    if (Regex.IsMatch(txt_ServerPort.Text, "(\\D+)")) { txt_ServerPort.BorderBrush = BrushInvalid; return; }

                    //Check text input number
                    int ServerPort = Convert.ToInt32(txt_ServerPort.Text);
                    if (ServerPort < 1 || ServerPort > 65535) { txt_ServerPort.BorderBrush = BrushInvalid; return; }

                    SettingSave("ServerPort", txt_ServerPort.Text);
                    txt_ServerPort.BorderBrush = BrushValid;

                    //Restart the socket server
                    vArnoldVinkSockets.vSocketServerPort = Convert.ToInt32(txt_ServerPort.Text);
                    await vArnoldVinkSockets.SocketServerRestart();
                };

                //Save - TimeMe Wallpaper
                cb_TimeMeWallpaper.Click += async (sender, e) =>
                {
                    SettingSave("TimeMeWallpaper", cb_TimeMeWallpaper.IsChecked.ToString());
                    if ((bool)cb_TimeMeWallpaper.IsChecked)
                    {
                        TaskStart_TimeMeWallpaper();
                    }
                    else
                    {
                        //Reset wallpaper variables
                        vWallpaperFilesize = 0;

                        //Hide wallpaper preview
                        sp_TimeMeWallpaper.Visibility = Visibility.Collapsed;

                        //Stop the task loop
                        await AVActions.TaskStopLoop(vTask_Wallpaper);
                    }
                };

                //Save - Windows Startup
                cb_StartupWindows.Click += (sender, e) => { ManageShortcutStartup(); };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save the settings: " + ex.Message);
            }
        }

        //Save - Application Setting
        void SettingSave(string name, string value)
        {
            try
            {
                vConfigurationApplication.AppSettings.Settings.Remove(name);
                vConfigurationApplication.AppSettings.Settings.Add(name, value);
                vConfigurationApplication.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }
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