using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static ArnoldVinkTools.AppVariables;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Handle received socket data
        public async Task ReceivedSocketHandler(TcpClient tcpClient, byte[] receivedBytes)
        {
            try
            {
                async void TaskAction()
                {
                    try
                    {
                        await ReceivedSocketHandlerThread(tcpClient, receivedBytes);
                    }
                    catch { }
                }
                await AVActions.TaskStart(TaskAction, null);
            }
            catch { }
        }

        async Task ReceivedSocketHandlerThread(TcpClient tcpClient, byte[] receivedBytes)
        {
            try
            {
                //Receive message
                string StringReceived = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
                StringReceived = WebUtility.UrlDecode(StringReceived);
                StringReceived = WebUtility.HtmlDecode(StringReceived);
                StringReceived = StringReceived.TrimEnd('\0');
                Debug.WriteLine("Received string: " + StringReceived);

                //Prepare response message
                string[] SocketData = StringReceived.Split('‡');
                string StringResponse = SocketStringHandle(SocketData);
                byte[] bytesResponse = Encoding.UTF8.GetBytes(StringResponse);

                //Return response message
                await vArnoldVinkSockets.TcpClientSendBytes(tcpClient, bytesResponse, vArnoldVinkSockets.vTcpClientTimeout, false);
            }
            catch { }
        }

        //Handle received socket string
        public string SocketStringHandle(string[] socketStringArray)
        {
            try
            {
                if (socketStringArray[0].StartsWith("MeBatteryLevel"))
                {
                    return Convert.ToString((System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100) + "/" + System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifeRemaining + "/" + System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus);
                }
                else if (socketStringArray[0].StartsWith("SwitchMonitor"))
                {
                    CommandSwitchMonitor(socketStringArray[1]);
                }
                else
                {
                    CommandKey(socketStringArray[0]);
                }
            }
            catch { }
            return "Ok";
        }
    }
}