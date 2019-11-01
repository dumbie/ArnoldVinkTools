using System;
using System.Threading.Tasks;
using static AppImport.AppImport;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Send single key press
        async Task KeySendSingle(byte virtualKey, IntPtr WindowHandle)
        {
            try
            {
                PostMessage(WindowHandle, WM_KEYDOWN, virtualKey, 0); //Key Press
                await Task.Delay(25);
                PostMessage(WindowHandle, WM_KEYUP, virtualKey, 0); //Key Release
            }
            catch { }
        }

        //Simulate single key press
        async Task KeyPressSingle(byte virtualKey, bool ExtendedKey)
        {
            try
            {
                byte scanByte = Convert.ToByte(MapVirtualKey(virtualKey, MAPVK_VK_TO_VSC));
                uint KeyFlagsDown = KEYEVENTF_NONE;
                uint KeyFlagsUp = KEYEVENTF_KEYUP;

                if (ExtendedKey)
                {
                    scanByte = Convert.ToByte(MapVirtualKey(virtualKey, MAPVK_VK_TO_VSC_EX));
                    KeyFlagsDown = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_NONE;
                    KeyFlagsUp = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                }

                keybd_event(virtualKey, scanByte, KeyFlagsDown, 0); //Key Press
                await Task.Delay(25);
                keybd_event(virtualKey, scanByte, KeyFlagsUp, 0); //Key Release
            }
            catch { }
        }

        //Simulate combo key press
        async Task KeyPressCombo(byte Modifier, byte virtualKey, bool ExtendedKey)
        {
            try
            {
                byte scanByteVk = Convert.ToByte(MapVirtualKey(virtualKey, MAPVK_VK_TO_VSC));
                byte scanByteMod = Convert.ToByte(MapVirtualKey(Modifier, MAPVK_VK_TO_VSC));
                uint KeyFlagsDown = KEYEVENTF_NONE;
                uint KeyFlagsUp = KEYEVENTF_KEYUP;

                if (ExtendedKey)
                {
                    scanByteVk = Convert.ToByte(MapVirtualKey(virtualKey, MAPVK_VK_TO_VSC_EX));
                    scanByteMod = Convert.ToByte(MapVirtualKey(Modifier, MAPVK_VK_TO_VSC_EX));
                    KeyFlagsDown = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_NONE;
                    KeyFlagsUp = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                }

                keybd_event(Modifier, scanByteMod, KeyFlagsDown, 0); //Modifier Press
                keybd_event(virtualKey, scanByteVk, KeyFlagsDown, 0); //Key Press
                await Task.Delay(25);
                keybd_event(virtualKey, scanByteVk, KeyFlagsUp, 0); //Key Release
                keybd_event(Modifier, scanByteMod, KeyFlagsUp, 0); //Modifier Release
            }
            catch { }
        }
    }
}