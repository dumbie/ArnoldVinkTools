using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        void StartupCheck()
        {
            try
            {
                Debug.WriteLine("Checking current application status...");

                //Check - If application is already running
                Thread.Sleep(1000); //Restart fix
                if (Process.GetProcessesByName("ArnoldVinkTools").Length > 1)
                {
                    Debug.WriteLine("Application is already running, closing this process.");
                    Environment.Exit(0);
                }

                //Check - Missing application config
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\ArnoldVinkTools.exe.config"))
                {
                    MessageBox.Show("File: ArnoldVinkTools.exe.config could not be found, please check your installation.", "Arnold Vink Tools");
                    Environment.Exit(0);
                }

                //Check - Missing application Updater
                if (!File.Exists("Updater.exe"))
                {
                    MessageBox.Show("File: Updater.exe could not be found, please check your installation.", "Arnold Vink Tools");
                    Environment.Exit(0);
                    return;
                }

                //Check - If the updater has been updated
                if (File.Exists("UpdaterNew.exe"))
                {
                    try
                    {
                        Debug.WriteLine("Renaming: UpdaterNew.exe to Updater.exe");
                        if (File.Exists("Updater.exe")) { File.Delete("Updater.exe"); }
                        File.Move("UpdaterNew.exe", "Updater.exe");
                    }
                    catch { }
                }

                //Check - If the updater failed to cleanup
                if (File.Exists("AppUpdate.zip")) { try { File.Delete("AppUpdate.zip"); } catch { } }
            }
            catch { }
        }
    }
}