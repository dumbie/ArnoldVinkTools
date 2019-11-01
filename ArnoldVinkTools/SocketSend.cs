//using System.Diagnostics;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace ArnoldVinkTools
//{
//    partial class MainPage
//    {
//        //Start sending TCP sockets
//        void SocketsSendTcp(string SendString, Bitmap SendBitmap, IPEndPoint SendAdres)
//        {
//            try
//            {
//                using (Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
//                {
//                    byte[] SocketData = null;
//                    if (SendString == "" && SendBitmap != null)
//                    {
//                        MemoryStream stream = new MemoryStream();
//                        SendBitmap.Save(stream, ImageFormat.Jpeg);
//                        stream.Position = 0;

//                        SocketData = stream.ToArray();
//                        stream.Dispose();
//                    }
//                    else
//                    { SocketData = Encoding.UTF8.GetBytes(SendString); }

//                    if (SocketData != null)
//                    {
//                        Socket.Connect(SendAdres);
//                        int bytesSent = Socket.Send(SocketData);
//                        Debug.WriteLine("SendedTcp to " + Socket.RemoteEndPoint.ToString() + ": " + SendString + " bytes: " + bytesSent);
//                    }
//                }
//            }
//            catch { }
//        }
//    }
//}