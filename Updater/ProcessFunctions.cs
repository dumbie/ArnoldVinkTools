using System;
using System.Diagnostics;
using System.IO;

namespace Updater
{
    partial class MainWindow
    {
        //Launch an app manually
        void LaunchProcessManually(string PathExe, string PathLaunch, string Arguments, bool RunAsAdmin)
        {
            try
            {
                //Launch Win32
                Process LaunchProcess = new Process();
                LaunchProcess.StartInfo.FileName = PathExe;
                if (RunAsAdmin)
                {
                    LaunchProcess.StartInfo.UseShellExecute = true;
                    LaunchProcess.StartInfo.Verb = "runas";
                }
                else { LaunchProcess.StartInfo.UseShellExecute = false; }
                if (!String.IsNullOrWhiteSpace(PathLaunch)) { LaunchProcess.StartInfo.WorkingDirectory = PathLaunch; } else { LaunchProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(PathExe); }
                if (!String.IsNullOrWhiteSpace(Arguments)) { LaunchProcess.StartInfo.Arguments = Arguments; }
                LaunchProcess.Start();

                Debug.WriteLine("Launching: " + Path.GetFileNameWithoutExtension(PathExe));
            }
            catch { }
        }
    }
}