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
            if (stackpanel_HelpText.Children.Count == 0)
            {
                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "Support and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "When you are walking into any problem or bug you can goto the support page here: http://help.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "\r\nDevelopment donation support", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "Feel free to make a donation on: http://donation.arnoldvink.com", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                //Set the version text
                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                stackpanel_HelpText.Children.Add(new TextBlock() { Text = "Version: v" + Assembly.GetExecutingAssembly().FullName.Split('=')[1].Split(',')[0], Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });
            }
        }
    }
}