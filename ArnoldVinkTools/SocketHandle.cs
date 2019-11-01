using System;
using System.Threading.Tasks;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Handle received socket data
        public async Task<string> SocketHandle(string[] SocketData)
        {
            try
            {
                if (SocketData[0].StartsWith("MeBatteryLevel"))
                {
                    return Convert.ToString((System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100) + "/" + System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifeRemaining + "/" + System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus);
                }
                else if (SocketData[0].StartsWith("MeReceiveVideo"))
                {
                    //vIPEndPoint = (IPEndPoint)Socket.RemoteEndPoint;
                    //vIPEndPoint.Port = 1000;
                    //DXInitialize();
                }
                else
                {
                    await KeyCommand(SocketData[0]);
                    return "Ok";
                }

                return String.Empty;
            }
            catch { return String.Empty; }
        }
    }
}