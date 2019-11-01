using System.Windows;

namespace ArnoldVinkTools
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            MainPage MainStartup = new MainPage();
            await MainStartup.MainStartup();
        }
    }
}