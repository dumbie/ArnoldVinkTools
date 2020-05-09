using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.AVFirewall;

namespace ArnoldVinkTools
{
    public partial class App : Application
    {
        //Application windows
        public static MainPage vMainPage = new MainPage();

        //Application startup
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Application restart delay
                await Application_RestartDelay(e);

                //Allow application in firewall
                string appFilePath = Assembly.GetEntryAssembly().Location;
                Firewall_ExecutableAllow("Arnold Vink Tools", appFilePath, true);

                await vMainPage.Application_Startup();
            }
            catch { }
        }

        //Application restart delay
        private async Task Application_RestartDelay(StartupEventArgs e)
        {
            try
            {
                if (e.Args != null && e.Args.Contains("-restart"))
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    string processName = currentProcess.ProcessName;
                    while (Process.GetProcessesByName(processName).Length > 1)
                    {
                        await Task.Delay(500);
                    }
                }
            }
            catch { }
        }
    }
}