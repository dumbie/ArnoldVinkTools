using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ArnoldVinkTools
{
    partial class MainPage : Window
    {
        //Load - Application Help
        void Help_Load()
        {
            try
            {
                if (stackpanel_HelpText.Children.Count == 0)
                {
                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "Support and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"], FontSize = (double)App.Current.Resources["TextSizeMedium"] });
                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "When you are walking into any problems or a bug you can go to my help page at https://help.arnoldvink.com so I can try to help you out and get everything working.", Style = (Style)App.Current.Resources["TextBlockGray"], TextWrapping = TextWrapping.Wrap });

                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "\r\nDeveloper donation", Style = (Style)App.Current.Resources["TextBlockBlack"], FontSize = (double)App.Current.Resources["TextSizeMedium"] });
                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "If you appreciate my project and want to support me with my projects you can make a donation through https://donation.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockGray"], TextWrapping = TextWrapping.Wrap });

                    //Set the version text
                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"], FontSize = (double)App.Current.Resources["TextSizeMedium"] });
                    stackpanel_HelpText.Children.Add(new TextBlock() { Text = "Version: v" + Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0], Style = (Style)App.Current.Resources["TextBlockGray"], TextWrapping = TextWrapping.Wrap });
                }
            }
            catch { }
        }
    }
}