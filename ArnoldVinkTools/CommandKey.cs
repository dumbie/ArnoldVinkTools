using System;
using static ArnoldVinkCode.AVInputOutputKeyboard;
using static Classes.Classes;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //MediaRemoteMe Command Key
        void CommandKey(string remoteData)
        {
            try
            {
                if (remoteData.Contains("MeCenterCamera")) { KeyPressSingle((byte)VirtualKeys.Home, false); }
                else if (remoteData.Contains("MeStop")) { KeyPressSingle((byte)VirtualKeys.MediaStop, false); }
                else if (remoteData.Contains("MePlayPause")) { KeyPressSingle((byte)VirtualKeys.MediaPlayPause, false); }
                else if (remoteData.Contains("MeNextSong")) { KeyPressSingle((byte)VirtualKeys.MediaNextTrack, false); }
                else if (remoteData.Contains("MePrevSong")) { KeyPressSingle((byte)VirtualKeys.MediaPrevTrack, false); }
                else if (remoteData.Contains("MeMenu"))
                {
                    KeyPressSingle((byte)VirtualKeys.C, false); //C
                    KeyPressSingle((byte)VirtualKeys.Menu, false); //Alt
                }
                else if (remoteData.Contains("MeEsc")) { KeyPressSingle((byte)VirtualKeys.Escape, false); }
                else if (remoteData.Contains("MeTab")) { KeyPressSingle((byte)VirtualKeys.Tab, false); }
                else if (remoteData.Contains("MeEnter")) { KeyPressSingle((byte)VirtualKeys.Return, false); }
                else if (remoteData.Contains("MeSpace")) { KeyPressSingle((byte)VirtualKeys.Space, false); }
                else if (remoteData.Contains("MeBackSpace")) { KeyPressSingle((byte)VirtualKeys.Back, false); }
                else if (remoteData.Contains("MeArrowLeft")) { KeyPressSingle((byte)VirtualKeys.Left, true); }
                else if (remoteData.Contains("MeArrowUp")) { KeyPressSingle((byte)VirtualKeys.Up, true); }
                else if (remoteData.Contains("MeArrowRight")) { KeyPressSingle((byte)VirtualKeys.Right, true); }
                else if (remoteData.Contains("MeArrowDown")) { KeyPressSingle((byte)VirtualKeys.Down, true); }
                else if (remoteData.Contains("Me0")) { KeyPressSingle((byte)VirtualKeys.N0, false); }
                else if (remoteData.Contains("Me1")) { KeyPressSingle((byte)VirtualKeys.N1, false); }
                else if (remoteData.Contains("Me2")) { KeyPressSingle((byte)VirtualKeys.N2, false); }
                else if (remoteData.Contains("Me3")) { KeyPressSingle((byte)VirtualKeys.N3, false); }
                else if (remoteData.Contains("Me4")) { KeyPressSingle((byte)VirtualKeys.N4, false); }
                else if (remoteData.Contains("Me5")) { KeyPressSingle((byte)VirtualKeys.N5, false); }
                else if (remoteData.Contains("Me6")) { KeyPressSingle((byte)VirtualKeys.N6, false); }
                else if (remoteData.Contains("Me7")) { KeyPressSingle((byte)VirtualKeys.N7, false); }
                else if (remoteData.Contains("Me8")) { KeyPressSingle((byte)VirtualKeys.N8, false); }
                else if (remoteData.Contains("Me9")) { KeyPressSingle((byte)VirtualKeys.N9, false); }
                else if (remoteData.Contains("MeVolMute")) { KeyPressSingle((byte)VirtualKeys.VolumeMute, false); }

                else if (remoteData.Contains("MeVolUp"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolUp", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { KeyPressSingle((byte)VirtualKeys.VolumeUp, false); }
                }

                else if (remoteData.Contains("MeVolDown"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolDown", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { KeyPressSingle((byte)VirtualKeys.VolumeDown, false); }
                }

                else if (remoteData.Contains("MeFullscreen"))
                {
                    KeyPressCombo((byte)VirtualKeys.Menu, (byte)VirtualKeys.Return, false);
                }

                else if (remoteData.Contains("MeAltTab"))
                {
                    KeyPressCombo((byte)VirtualKeys.Menu, (byte)VirtualKeys.Tab, false);
                }
            }
            catch { }
        }
    }
}