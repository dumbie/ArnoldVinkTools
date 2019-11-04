using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                Debug.WriteLine("Received string: " + StringReceived);

                //Prepare response message
                string[] SocketData = StringReceived.Split('‡');
                string StringResponse = await SocketStringHandle(SocketData);

                //Send response message
                byte[] bytesResponse = Encoding.UTF8.GetBytes(StringResponse);
                await tcpClient.GetStream().WriteAsync(bytesResponse, 0, bytesResponse.Length);
                Debug.WriteLine("Sended response: " + StringResponse);
            }
            catch { }
        }

        //Handle received socket string
        public async Task<string> SocketStringHandle(string[] socketStringArray)
        {
            try
            {
                if (socketStringArray[0].StartsWith("MeBatteryLevel"))
                {
                    return Convert.ToString((System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100) + "/" + System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifeRemaining + "/" + System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus);
                }
                else
                {
                    await KeyCommand(socketStringArray[0]);
                    return "Ok";
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}