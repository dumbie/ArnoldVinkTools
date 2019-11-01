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
                lb_SettingsListBox.SelectionChanged += (xsender, xargs) =>
                {
                    if (lb_SettingsListBox.SelectedIndex == 0)
                    {
                        sp_GeneralTab.Visibility = Visibility.Visible;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        sp_StreamVRTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 1)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Visible;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        sp_StreamVRTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 2)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Visible;
                        sp_StreamVRTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Collapsed;
                    }
                    else if (lb_SettingsListBox.SelectedIndex == 3)
                    {
                        sp_GeneralTab.Visibility = Visibility.Collapsed;
                        sp_TimeMeTab.Visibility = Visibility.Collapsed;
                        sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                        sp_StreamVRTab.Visibility = Visibility.Collapsed;
                        stackpanel_HelpTextTab.Visibility = Visibility.Visible;
                    }
                    //else if (lb_SettingsListBox.SelectedIndex == 4)
                    //{
                    //    sp_GeneralTab.Visibility = Visibility.Collapsed;
                    //    sp_TimeMeTab.Visibility = Visibility.Collapsed;
                    //    sp_MediaRemoteTab.Visibility = Visibility.Collapsed;
                    //    sp_StreamVRTab.Visibility = Visibility.Collapsed;
                    //    stackpanel_HelpTextTab.Visibility = Visibility.Visible;
                    //}
                };
            };
        }
    }
}