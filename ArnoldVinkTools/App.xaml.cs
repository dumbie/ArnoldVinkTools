using System.Windows;

namespace ArnoldVinkTools
{
    public partial class App : Application
    {
        //Application windows
        public static MainPage vMainPage = new MainPage();

        //Application startup
        protected override async void OnStartup(StartupEventArgs e)
        {
            await vMainPage.Startup();
        }
    }
}