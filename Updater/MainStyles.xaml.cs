using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Updater
{
    public partial class MainStyles : ResourceDictionary
    {
        //Handle horizontal scrollviewer scrolling
        void ScrollViewerHorizontalScrolling(object sender, MouseWheelEventArgs e)
        {
            try
            {
                ScrollViewer MouseScroll = sender as ScrollViewer;
                if (e.Delta > 0) { MouseScroll.LineLeft(); } else { MouseScroll.LineRight(); }
            }
            catch { }
        }
    }
}