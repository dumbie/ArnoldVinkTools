using System;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputKeyboard;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //MediaRemoteMe Command Key
        void CommandKey(string remoteData)
        {
            try
            {
                if (remoteData.Contains("MeCenterCamera")) { KeyPressSingle((byte)KeysVirtual.Home, false); }
                else if (remoteData.Contains("MeStop")) { KeyPressSingle((byte)KeysVirtual.MediaStop, false); }
                else if (remoteData.Contains("MePlayPause")) { KeyPressSingle((byte)KeysVirtual.MediaPlayPause, false); }
                else if (remoteData.Contains("MeNextSong")) { KeyPressSingle((byte)KeysVirtual.MediaNextTrack, false); }
                else if (remoteData.Contains("MePrevSong")) { KeyPressSingle((byte)KeysVirtual.MediaPrevTrack, false); }
                else if (remoteData.Contains("MeMenu"))
                {
                    KeyPressSingle((byte)KeysVirtual.C, false); //C
                    KeyPressSingle((byte)KeysVirtual.Menu, false); //Alt
                }
                else if (remoteData.Contains("MeEsc")) { KeyPressSingle((byte)KeysVirtual.Escape, false); }
                else if (remoteData.Contains("MeTab")) { KeyPressSingle((byte)KeysVirtual.Tab, false); }
                else if (remoteData.Contains("MeEnter")) { KeyPressSingle((byte)KeysVirtual.Return, false); }
                else if (remoteData.Contains("MeSpace")) { KeyPressSingle((byte)KeysVirtual.Space, false); }
                else if (remoteData.Contains("MeBackSpace")) { KeyPressSingle((byte)KeysVirtual.Back, false); }
                else if (remoteData.Contains("MeArrowLeft")) { KeyPressSingle((byte)KeysVirtual.Left, true); }
                else if (remoteData.Contains("MeArrowUp")) { KeyPressSingle((byte)KeysVirtual.Up, true); }
                else if (remoteData.Contains("MeArrowRight")) { KeyPressSingle((byte)KeysVirtual.Right, true); }
                else if (remoteData.Contains("MeArrowDown")) { KeyPressSingle((byte)KeysVirtual.Down, true); }
                else if (remoteData.Contains("Me0")) { KeyPressSingle((byte)KeysVirtual.N0, false); }
                else if (remoteData.Contains("Me1")) { KeyPressSingle((byte)KeysVirtual.N1, false); }
                else if (remoteData.Contains("Me2")) { KeyPressSingle((byte)KeysVirtual.N2, false); }
                else if (remoteData.Contains("Me3")) { KeyPressSingle((byte)KeysVirtual.N3, false); }
                else if (remoteData.Contains("Me4")) { KeyPressSingle((byte)KeysVirtual.N4, false); }
                else if (remoteData.Contains("Me5")) { KeyPressSingle((byte)KeysVirtual.N5, false); }
                else if (remoteData.Contains("Me6")) { KeyPressSingle((byte)KeysVirtual.N6, false); }
                else if (remoteData.Contains("Me7")) { KeyPressSingle((byte)KeysVirtual.N7, false); }
                else if (remoteData.Contains("Me8")) { KeyPressSingle((byte)KeysVirtual.N8, false); }
                else if (remoteData.Contains("Me9")) { KeyPressSingle((byte)KeysVirtual.N9, false); }
                else if (remoteData.Contains("MeVolMute")) { KeyPressSingle((byte)KeysVirtual.VolumeMute, false); }

                else if (remoteData.Contains("MeVolUp"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolUp", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { KeyPressSingle((byte)KeysVirtual.VolumeUp, false); }
                }

                else if (remoteData.Contains("MeVolDown"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolDown", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { KeyPressSingle((byte)KeysVirtual.VolumeDown, false); }
                }

                else if (remoteData.Contains("MeFullscreen"))
                {
                    KeyPressCombo((byte)KeysVirtual.Menu, (byte)KeysVirtual.Return, false);
                }

                else if (remoteData.Contains("MeAltTab"))
                {
                    KeyPressCombo((byte)KeysVirtual.Menu, (byte)KeysVirtual.Tab, false);
                }
            }
            catch { }
        }
    }
}