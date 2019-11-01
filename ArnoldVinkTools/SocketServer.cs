using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Socket variables
        private bool vSocketServerBusy = false;
        private TcpListener vTcpListener = null;

        //Switch the server on or off
        public async Task SocketServerSwitch(bool ForceOff, bool Restart)
        {
            try
            {
                if (!vSocketServerBusy)
                {
                    vSocketServerBusy = true;
                    if (Restart)
                    {
                        Debug.WriteLine("Restarting the socket server...");
                        await SocketServerDisable();
                        SocketServerEnable();
                        vSocketServerBusy = false;
                        return;
                    }

                    if (AVActions.TaskRunningCheck(vSocketServerToken) || ForceOff)
                    {
                        Debug.WriteLine("Disabling the socket server...");
                        await SocketServerDisable();
                        vSocketServerBusy = false;
                        return;
                    }
                    else
                    {
                        Debug.WriteLine("Enabling the socket server...");
                        SocketServerEnable();
                        vSocketServerBusy = false;
                        return;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Failed switching the socket server on or off.");
                vSocketServerBusy = false;
            }
        }

        //Enable the socket server
        void SocketServerEnable()
        {
            try
            {
                Action TaskAction = async () => { await SocketListener(); };
                vSocketServerToken = new CancellationTokenSource();
                vSocketServerTask = AVActions.TaskStart(TaskAction, vSocketServerToken);
            }
            catch (Exception ex) { Debug.WriteLine("Failed to enable the socket server: " + ex.Message); }
        }

        //Disable the socket server
        async Task SocketServerDisable()
        {
            try
            {
                //Stop the tcp listener
                if (vTcpListener != null)
                {
                    vTcpListener.Stop();
                    vTcpListener = null;
                }

                //Stop the server task
                await AVActions.TaskStop(vSocketServerTask, vSocketServerToken);
            }
            catch (Exception ex) { Debug.WriteLine("Failed to disable the socket server: " + ex.Message); }
        }

        //Receive socket message
        async Task SocketListener()
        {
            try
            {
                //Start the tcp listener
                int ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);
                vTcpListener = new TcpListener(IPAddress.Any, ServerPort);
                vTcpListener.Start();

                Debug.WriteLine("The server is running on: " + vTcpListener.LocalEndpoint);

                //Socket Listening loop
                while (AVActions.TaskRunningCheck(vSocketServerToken))
                {
                    //Receive sockets
                    using (TcpClient tcpClient = await vTcpListener.AcceptTcpClientAsync())
                    {
                        using (NetworkStream tcpStream = tcpClient.GetStream())
                        {
                            try
                            {
                                //Receive message
                                byte[] bytesReceived = new byte[tcpClient.ReceiveBufferSize];
                                int bytesReceivedLength = await tcpStream.ReadAsync(bytesReceived, 0, tcpClient.ReceiveBufferSize);
                                string StringReceived = Encoding.UTF8.GetString(bytesReceived, 0, bytesReceivedLength);
                                Debug.WriteLine("Received string: " + StringReceived);

                                //Prepare response message
                                string[] SocketData = StringReceived.Split('‡');
                                string StringResponse = await SocketHandle(SocketData);

                                //Send response message
                                byte[] bytesResponse = Encoding.UTF8.GetBytes(StringResponse);
                                await tcpStream.WriteAsync(bytesResponse, 0, bytesResponse.Length);
                                Debug.WriteLine("Sended response: " + StringResponse);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147467259)
                {
                    MessageBox.Show("Failed to launch the socket server, please make sure the used server port is not already in use.", "Arnold Vink Tools");
                    Debug.WriteLine("The socket port is in use: " + ex.Message);
                    await SocketServerDisable();
                }
                else
                {
                    Debug.WriteLine("The socket server crashed: " + ex.Message);
                    await SocketServerSwitch(false, true);
                }
            }
        }
    }
}