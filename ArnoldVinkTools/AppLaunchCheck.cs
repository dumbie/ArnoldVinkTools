﻿using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVFiles;

namespace ArnoldVinkTools
{
    public partial class AppLaunchCheck
    {
        public static async Task Application_LaunchCheck(string ApplicationName, string ProcessName, ProcessPriorityClass ProcessPriority, bool skipFileCheck)
        {
            try
            {
                Debug.WriteLine("Checking application status.");

                //Check - If application is already running
                if (Process.GetProcessesByName(ProcessName).Length > 1)
                {
                    Debug.WriteLine("Application is already running.");
                    Environment.Exit(0);
                    return;
                }

                //Set the working directory to executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                //Set the application priority level
                try
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriority;
                }
                catch { }

                //Check for missing application files
                if (!skipFileCheck)
                {
                    string[] ApplicationFiles = { "Resources/Microsoft.Win32.TaskScheduler.dll", "Resources/Newtonsoft.Json.dll", "ArnoldVinkTools.exe", "ArnoldVinkTools.exe.Config", "ArnoldVinkTools-Admin.exe", "ArnoldVinkTools-Admin.exe.Config", "Updater.exe", "Updater.exe.Config" };
                    foreach (string checkFile in ApplicationFiles)
                    {
                        if (!File.Exists(checkFile))
                        {
                            List<string> messageAnswers = new List<string>();
                            messageAnswers.Add("Ok");
                            await new AVMessageBox().Popup(null, "File not found", checkFile + " could not be found, please check your installation.", messageAnswers);

                            //Close the application
                            Environment.Exit(0);
                            return;
                        }
                    }
                }

                //Check - If updater has been updated
                File_Move("UpdaterNew.exe", "Updater.exe", true);
                File_Move("Resources/UpdaterReplace.exe", "Updater.exe", true);

                //Check - If updater failed to cleanup
                File_Delete("Resources/AppUpdate.zip");
            }
            catch { }
        }
    }
}