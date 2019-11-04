using System.Security.Principal;
using System.Windows;

namespace ArnoldVinkTools
{
    partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                //Check if application has launched as admin
                if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    this.Title += " (Admin)";
                }

                //Check and monitor the selected tab
                lb_SettingsListBox.SelectionChanged += (xsender, xargs) =>
                {
                    if (lb_SettingsListBox.SelectedIndex == 0)
                    {
                        sp_GeneralTab.Visibility = Visibility.Visible;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 1)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Visible;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 2)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Visible;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 3)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Visible;
                    }
                };
            };
        }
    }
}