//using System;
//using System.Diagnostics;
//using System.Drawing.Imaging;

////Check if device is still requesting 10sec timeout stop

//namespace ArnoldVinkTools
//{
//    partial class MainPage
//    {
//        //Application Variables
//        int vDXWidth = 0;
//        int vDXHeight = 0;
//        int vAdapterNumber = 0; // # of graphics card adapter
//        int vAdapterOutput = 0; // # of output device (i.e. monitor)
//        bool vDXInitialized = false;

//        Factory1 vDXFactory1;
//        Adapter1 vDXAdapter1;
//        Device vDXDevice;

//        Output vDXOutput;
//        Output1 vDXOutput1;

//        Texture2D vDXTexture2DScreen;
//        Texture2DDescription vDXTexture2DDescription;

//        OutputDuplication vDXOutputDuplication;
//        OutputDuplicateFrameInformation vDXDuplicateFrameInformation;

//        SharpDX.DXGI.Resource vDXResourceScreen;
//        System.Drawing.Bitmap vBitmapScreen;

//        void DXInitialize()
//        {
//            while (true)
//            {
//                //Initialize DirectX
//                if (!vDXInitialized)
//                {
//                    try
//                    {
//                        Debug.WriteLine("Initializing SharpDX...");

//                        // Create DXGI Factory1
//                        vDXFactory1 = new Factory1();
//                        vDXAdapter1 = vDXFactory1.GetAdapter1(vAdapterNumber);

//                        // Create device from Adapter
//                        vDXDevice = new Device(vDXAdapter1);

//                        // Get DXGI.Output
//                        vDXOutput = vDXAdapter1.GetOutput(vAdapterOutput);
//                        vDXOutput1 = vDXOutput.QueryInterface<Output1>();

//                        // Width/Height of desktop to capture
//                        vDXWidth = ((Rectangle)vDXOutput.Description.DesktopBounds).Width;
//                        vDXHeight = ((Rectangle)vDXOutput.Description.DesktopBounds).Height;

//                        // Create Staging texture CPU-accessible
//                        vDXTexture2DDescription = new Texture2DDescription
//                        {
//                            CpuAccessFlags = CpuAccessFlags.Read,
//                            BindFlags = BindFlags.None,
//                            Format = Format.B8G8R8A8_UNorm,
//                            Width = vDXWidth,
//                            Height = vDXHeight,
//                            OptionFlags = ResourceOptionFlags.None,
//                            MipLevels = 1,
//                            ArraySize = 1,
//                            SampleDescription = { Count = 1, Quality = 0 },
//                            Usage = ResourceUsage.Staging
//                        };

//                        vDXTexture2DScreen = new Texture2D(vDXDevice, vDXTexture2DDescription);

//                        vDXInitialized = true;
//                    }
//                    catch (SharpDXException e)
//                    {
//                        vDXInitialized = false;

//                        vBitmapScreen = new System.Drawing.Bitmap("Resources\\Screens\\BlankScreen.jpg");
//                        SocketsSendTcp("", AfterProcessBitmap(), vIPEndPoint);

//                        Debug.WriteLine("Failed to initialize SharpDX: " + e.Message);
//                        ReleaseDXDevice();
//                        ReleaseDXResources();

//                        continue;
//                    }
//                }

//                try
//                {
//                    Debug.WriteLine("Making a screen capture...");

//                    // Duplicate the output
//                    vDXOutputDuplication = vDXOutput1.DuplicateOutput(vDXDevice);

//                    // Try to get duplicated frame within given time
//                    vDXOutput.WaitForVerticalBlank();
//                    vDXOutput1.WaitForVerticalBlank();

//                    vDXOutputDuplication.AcquireNextFrame(10000, out vDXDuplicateFrameInformation, out vDXResourceScreen);

//                    // copy resource into memory that can be accessed by the CPU
//                    using (Texture2D DXTexture2DScreenFrame = vDXResourceScreen.QueryInterface<Texture2D>()) { vDXDevice.ImmediateContext.CopyResource(DXTexture2DScreenFrame, vDXTexture2DScreen); }

//                    vDXOutputDuplication.ReleaseFrame();

//                    // Get the desktop capture texture
//                    DataBox mapSource = vDXDevice.ImmediateContext.MapSubresource(vDXTexture2DScreen, 0, MapMode.Read, MapFlags.None);

//                    // Create Drawing.Bitmap
//                    System.Drawing.Rectangle boundsRect = new System.Drawing.Rectangle(0, 0, vDXWidth, vDXHeight);
//                    vBitmapScreen = new System.Drawing.Bitmap(vDXWidth, vDXHeight, PixelFormat.Format32bppRgb);

//                    // Copy pixels from screen capture Texture to GDI bitmap
//                    BitmapData mapDest = vBitmapScreen.LockBits(boundsRect, ImageLockMode.WriteOnly, vBitmapScreen.PixelFormat);
//                    IntPtr sourcePtr = mapSource.DataPointer;
//                    IntPtr destPtr = mapDest.Scan0;
//                    for (int y = 0; y < vDXHeight; y++)
//                    {
//                        // Copy a single line 
//                        Utilities.CopyMemory(destPtr, sourcePtr, vDXWidth * 4);

//                        // Advance pointers
//                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
//                        destPtr = IntPtr.Add(destPtr, mapDest.Stride);
//                    }

//                    // Release source and dest locks
//                    vBitmapScreen.UnlockBits(mapDest);
//                    vDXDevice.ImmediateContext.UnmapSubresource(vDXTexture2DScreen, 0);

//                    SocketsSendTcp("", AfterProcessBitmap(), vIPEndPoint);
//                    Debug.WriteLine("Sended screen capture.");
//                    ReleaseDXResources();
//                }
//                catch (SharpDXException e)
//                {
//                    vBitmapScreen = new System.Drawing.Bitmap("Resources\\Screens\\BlankScreen.jpg");
//                    SocketsSendTcp("", AfterProcessBitmap(), vIPEndPoint);

//                    Debug.WriteLine("Failed to make a screen capture: " + e.Message);
//                    ReleaseDXResources();

//                    continue;
//                }
//            }
//        }

//        //Edit the bitmap for stream
//        System.Drawing.Bitmap AfterProcessBitmap()
//        {
//            try
//            {
//                //////Filter out black screens
//                ////BitmapData BitmapData = vBitmapScreen.LockBits(new System.Drawing.Rectangle(0, 0, vBitmapScreen.Width, vBitmapScreen.Height), ImageLockMode.ReadOnly, vBitmapScreen.PixelFormat);
//                ////byte[] BitmapRgbValues = new byte[BitmapData.Stride * BitmapData.Height];
//                ////System.Runtime.InteropServices.Marshal.Copy(BitmapData.Scan0, BitmapRgbValues, 0, BitmapRgbValues.Length);
//                ////vBitmapScreen.UnlockBits(BitmapData);
//                ////if (!BitmapRgbValues.Where((v, i) => i % BitmapData.Stride < vBitmapScreen.Width && v != BitmapRgbValues[0]).Any()) { return null; }

//                //Draw cursor on the bitmap
//                //System.Drawing.Bitmap BitmapCursor = new System.Drawing.Bitmap(BitmapScreen.Width, BitmapScreen.Height, PixelFormat.Format32bppArgb);

//                //Generate headmount profile differences
//                //System.Drawing.Bitmap BitmapHeadMount = new System.Drawing.Bitmap(BitmapScreen.Width, BitmapScreen.Height, PixelFormat.Format32bppArgb);

//                //Set bitmap to set resolution
//                Debug.WriteLine((vBitmapScreen.Width / 100 * 50) + "/ " + (vBitmapScreen.Height / 100 * 50));
//                vBitmapScreen = new System.Drawing.Bitmap(vBitmapScreen, (vBitmapScreen.Width / 100 * 50), (vBitmapScreen.Height / 100 * 50));


//                //////Reduce bitmap jpeg quality
//                //EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, 100L);
//                //ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()[1];
//                //EncoderParameters encoderParams = new EncoderParameters(1);
//                //encoderParams.Param[0] = qualityParam;

//                //BitmapScreen.Save("Screenshots\\" + numRender + ".jpg", jpegCodec, encoderParams);

//                //numRender = numRender + 1;

//                //qualityParam.Dispose();
//                //encoderParams.Dispose();

//                ////Generate fake 3d if enabled
//                //if (ConfigurationManager.AppSettings["StreamVRFakeSBS"] == "True")
//                //{
//                //    System.Drawing.Bitmap BitmapFakeSBS = new System.Drawing.Bitmap(vBitmapScreen.Width, vBitmapScreen.Height, vBitmapScreen.PixelFormat);
//                //    using (System.Drawing.Graphics GraphicsDrawer = System.Drawing.Graphics.FromImage(BitmapFakeSBS))
//                //    {
//                //        GraphicsDrawer.DrawImage(vBitmapScreen, new System.Drawing.Rectangle(0, 0, vBitmapScreen.Width / 2, vBitmapScreen.Height));
//                //        GraphicsDrawer.DrawImage(vBitmapScreen, new System.Drawing.Rectangle(vBitmapScreen.Width / 2, 0, vBitmapScreen.Width / 2, vBitmapScreen.Height));
//                //    }

//                //    return BitmapFakeSBS;
//                //}
//                //else { return vBitmapScreen; }

//                return vBitmapScreen;

//                //NetworkStream stream = new NetworkStream(client_s);
//                //bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
//                //stream.Flush(); //flush everything
//            }
//            catch
//            {
//                Debug.WriteLine("Failed to after process bitmap");
//                return vBitmapScreen = new System.Drawing.Bitmap("Resources\\Screens\\BlankScreen.jpg");
//            }
//        }

//        //Release SharpDX Device
//        void ReleaseDXDevice()
//        {
//            Debug.WriteLine("Releasing SharpDX Device...");

//            //SharpDX Device
//            vDXInitialized = false;
//            if (vDXFactory1 != null) { vDXFactory1.Dispose(); vDXFactory1 = null; }
//            if (vDXAdapter1 != null) { vDXAdapter1.Dispose(); vDXAdapter1 = null; }
//            if (vDXDevice != null) { vDXDevice.Dispose(); vDXDevice = null; }
//            if (vDXOutput != null) { vDXOutput.Dispose(); vDXOutput = null; }
//            if (vDXOutput1 != null) { vDXOutput1.Dispose(); vDXOutput1 = null; }
//            if (vDXTexture2DScreen != null) { vDXTexture2DScreen.Dispose(); vDXTexture2DScreen = null; }
//        }

//        //Release SharpDX Resources
//        void ReleaseDXResources()
//        {
//            Debug.WriteLine("Releasing SharpDX Resources...");

//            //SharpDX Resources
//            if (vDXOutputDuplication != null) { vDXOutputDuplication.Dispose(); vDXOutputDuplication = null; }
//            if (vDXResourceScreen != null) { vDXResourceScreen.Dispose(); vDXResourceScreen = null; }
//            if (vBitmapScreen != null) { vBitmapScreen.Dispose(); vBitmapScreen = null; }
//        }
//    }
//}