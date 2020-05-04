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
                //Restart wait fix
                if (e.Args != null && e.Args.Contains("-restart"))
                {
                    await Task.Delay(2000);
                }

                //Allow application in firewall
                string appFilePath = Assembly.GetEntryAssembly().Location;
                Firewall_ExecutableAllow("Arnold Vink Tools", appFilePath, true);

                await vMainPage.Application_Startup();
            }
            catch { }
        }
    }
}