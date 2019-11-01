using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Updater
{
    public partial class MainWindow : Window
    {
        //Application Launch
        public MainWindow()
        {
            try
            {
                //Initialize Component
                InitializeComponent();

                //Check application state
                Application_LaunchCheck();

                //Check application arguments
                Application_LaunchArguments();

                //Start loading the application
                Loaded += Application_Load;
            }
            catch { }
        }

        //Application Loading
        async void Application_Load(object sender, RoutedEventArgs args)
        {
            try
            {
                //Check if previous update files are in the way
                if (File.Exists("UpdaterNew.exe")) { try { File.Delete("UpdaterNew.exe"); } catch { } }
                if (File.Exists("AppUpdate.zip")) { try { File.Delete("AppUpdate.zip"); } catch { } }

                //Check if ArnoldVinkTools is still running and close them
                bool AppRunning = false;
                foreach (Process CloseProcess in Process.GetProcessesByName("ArnoldVinkTools")) { CloseProcess.Kill(); AppRunning = true; }

                //Wait for applications to have closed
                await Task.Delay(1000);

                //Download application update from the website
                try
                {
                    WebClient WebClient = new WebClient();
                    WebClient.Headers[HttpRequestHeader.UserAgent] = "ArnoldVinkTools Update";
                    WebClient.DownloadProgressChanged += (object Object, DownloadProgressChangedEventArgs Args) =>
                    {
                        pb_UpdateProgress.IsIndeterminate = false;
                        pb_UpdateProgress.Value = Args.ProgressPercentage;
                        txt_UpdateStatus.Text = "Downloading update file: " + Args.ProgressPercentage + "%";
                    };
                    await WebClient.DownloadFileTaskAsync(new Uri("http://download.arnoldvink.com/?dl=ArnoldVinkTools.zip"), "AppUpdate.zip");
                    Debug.WriteLine("Update file has been downloaded");
                }
                catch { await UpdateFailed("Failed to download update, closing in a few seconds..."); }

                try
                {
                    //Extract the downloaded update archive
                    txt_UpdateStatus.Text = "Updating the application to the latest version...";
                    using (ZipArchive ZipArchive = ZipFile.OpenRead("AppUpdate.zip"))
                    {
                        foreach (ZipArchiveEntry ZipFile in ZipArchive.Entries)
                        {
                            string ExtractPath = AVFunctions.StringReplaceFirst(ZipFile.FullName, "Arnold Vink Tools/", "", false);
                            if (!String.IsNullOrEmpty(ExtractPath))
                            {
                                if (String.IsNullOrEmpty(ZipFile.Name)) { Directory.CreateDirectory(ExtractPath); }
                                else
                                {
                                    if (File.Exists(ExtractPath) && ExtractPath.ToLower().EndsWith("ArnoldVinkTools.exe.Config".ToLower())) { Debug.WriteLine("Skipping: ArnoldVinkTools.exe.Config"); continue; }
                                    if (File.Exists(ExtractPath) && ExtractPath.ToLower().EndsWith("Updater.exe".ToLower())) { Debug.WriteLine("Renaming: Updater.exe"); ExtractPath = ExtractPath.Replace("Updater.exe", "UpdaterNew.exe"); }
                                    ZipFile.ExtractToFile(ExtractPath, true);
                                }
                            }
                        }
                    }
                }
                catch { await UpdateFailed("Failed to extract update, closing in a few seconds..."); }

                //Delete the update installation zip file
                txt_UpdateStatus.Text = "Cleaning up the update installation files...";
                if (File.Exists("AppUpdate.zip")) { Debug.WriteLine("Removing: AppUpdate.zip"); File.Delete("AppUpdate.zip"); }

                //Start ArnoldVinkTools after the update has completed.
                if (AppRunning)
                {
                    txt_UpdateStatus.Text = "Running the updated version of ArnoldVinkTools...";
                    LaunchProcessManually("ArnoldVinkTools.exe", "", "", false);
                }

                //Close the updater after ArnoldVinkTools has launched.
                txt_UpdateStatus.Text = "Closing the updater...";
                Environment.Exit(0);
            }
            catch { await UpdateFailed("Failed to update, closing in a few seconds..."); }
        }

        //Failed to update
        async Task UpdateFailed(string FailedMessage)
        {
            try
            {
                //Set the failed reason text message
                txt_UpdateStatus.Text = FailedMessage;

                //Delete the update installation zip file
                if (File.Exists("AppUpdate.zip")) { Debug.WriteLine("Removing: AppUpdate.zip"); File.Delete("AppUpdate.zip"); }

                //Close the updater after 5 seconds
                await Task.Delay(5000);
                Environment.Exit(0);
            }
            catch { }
        }
    }
}