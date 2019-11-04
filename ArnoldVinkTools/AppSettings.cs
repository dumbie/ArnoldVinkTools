using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

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

                //Load - Application Windows Startup
                RegistryKey StartupRegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                foreach (string StartupApp in StartupRegistryKey.GetValueNames())
                {
                    try
                    {
                        if (StartupApp == "Arnold Vink Tools")
                        {
                            cb_StartupWindows.IsChecked = true;
                            //Update to the current application directory
                            StartupRegistryKey.SetValue("Arnold Vink Tools", "\"" + Directory.GetCurrentDirectory() + "\"");
                        }
                    }
                    catch { }
                }
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
                    if (String.IsNullOrWhiteSpace(txt_ServerPort.Text)) { return; }

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
                    await vSocketServer.SocketServerSwitch(false, true);
                };

                //Save - TimeMe Wallpaper
                cb_TimeMeWallpaper.Click += (sender, e) =>
                {
                    vConfiguration.AppSettings.Settings.Remove("TimeMeWallpaper");
                    vConfiguration.AppSettings.Settings.Add("TimeMeWallpaper", cb_TimeMeWallpaper.IsChecked.ToString());
                    vConfiguration.Save();
                    ConfigurationManager.RefreshSection("appSettings");
                };

                //Save - Application Windows Startup
                cb_StartupWindows.Click += (sender, e) =>
                {
                    try
                    {
                        RegistryKey StartupRegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                        if (cb_StartupWindows.IsChecked == true) { StartupRegistryKey.SetValue("Arnold Vink Tools", "\"" + Directory.GetCurrentDirectory() + "\""); }
                        else { StartupRegistryKey.DeleteValue("Arnold Vink Tools", false); }
                    }
                    catch { }
                };
            }
            catch (Exception Ex)
            {
                MessageBox.Show("SettingsSaveError: " + Ex.Message, "Arnold Vink Tools");
            }
        }
    }
}