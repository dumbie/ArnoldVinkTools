using static ArnoldVinkCode.AVDisplayMonitor;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //MediaRemoteMe Command SwitchMonitor
        void CommandSwitchMonitor(string remoteData)
        {
            try
            {
                if (remoteData.Contains("Primary"))
                {
                    EnableMonitorFirst();
                }
                else if (remoteData.Contains("Secondary"))
                {
                    EnableMonitorSecond();
                }
                else if (remoteData.Contains("Duplicate"))
                {
                    EnableMonitorCloneMode();
                }
                else if (remoteData.Contains("Extend"))
                {
                    EnableMonitorExtendMode();
                }
            }
            catch { }
        }
    }
}