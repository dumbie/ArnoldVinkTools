using System;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputKeyboard;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //MediaRemoteMe Command Key
        async Task CommandKey(string remoteData)
        {
            try
            {
                if (remoteData.Contains("MeCenterCamera")) { await KeyPressSingle((byte)KeysVirtual.Home, false); }
                else if (remoteData.Contains("MeStop")) { await KeyPressSingle((byte)KeysVirtual.MediaStop, false); }
                else if (remoteData.Contains("MePlayPause")) { await KeyPressSingle((byte)KeysVirtual.MediaPlayPause, false); }
                else if (remoteData.Contains("MeNextSong")) { await KeyPressSingle((byte)KeysVirtual.MediaNextTrack, false); }
                else if (remoteData.Contains("MePrevSong")) { await KeyPressSingle((byte)KeysVirtual.MediaPrevTrack, false); }
                else if (remoteData.Contains("MeMenu"))
                {
                    await KeyPressSingle((byte)KeysVirtual.C, false); //C
                    await KeyPressSingle((byte)KeysVirtual.Menu, false); //Alt
                }
                else if (remoteData.Contains("MeEsc")) { await KeyPressSingle((byte)KeysVirtual.Escape, false); }
                else if (remoteData.Contains("MeTab")) { await KeyPressSingle((byte)KeysVirtual.Tab, false); }
                else if (remoteData.Contains("MeEnter")) { await KeyPressSingle((byte)KeysVirtual.Return, false); }
                else if (remoteData.Contains("MeSpace")) { await KeyPressSingle((byte)KeysVirtual.Space, false); }
                else if (remoteData.Contains("MeBackSpace")) { await KeyPressSingle((byte)KeysVirtual.Back, false); }
                else if (remoteData.Contains("MeArrowLeft")) { await KeyPressSingle((byte)KeysVirtual.Left, true); }
                else if (remoteData.Contains("MeArrowUp")) { await KeyPressSingle((byte)KeysVirtual.Up, true); }
                else if (remoteData.Contains("MeArrowRight")) { await KeyPressSingle((byte)KeysVirtual.Right, true); }
                else if (remoteData.Contains("MeArrowDown")) { await KeyPressSingle((byte)KeysVirtual.Down, true); }
                else if (remoteData.Contains("Me0")) { await KeyPressSingle((byte)KeysVirtual.N0, false); }
                else if (remoteData.Contains("Me1")) { await KeyPressSingle((byte)KeysVirtual.N1, false); }
                else if (remoteData.Contains("Me2")) { await KeyPressSingle((byte)KeysVirtual.N2, false); }
                else if (remoteData.Contains("Me3")) { await KeyPressSingle((byte)KeysVirtual.N3, false); }
                else if (remoteData.Contains("Me4")) { await KeyPressSingle((byte)KeysVirtual.N4, false); }
                else if (remoteData.Contains("Me5")) { await KeyPressSingle((byte)KeysVirtual.N5, false); }
                else if (remoteData.Contains("Me6")) { await KeyPressSingle((byte)KeysVirtual.N6, false); }
                else if (remoteData.Contains("Me7")) { await KeyPressSingle((byte)KeysVirtual.N7, false); }
                else if (remoteData.Contains("Me8")) { await KeyPressSingle((byte)KeysVirtual.N8, false); }
                else if (remoteData.Contains("Me9")) { await KeyPressSingle((byte)KeysVirtual.N9, false); }
                else if (remoteData.Contains("MeVolMute")) { await KeyPressSingle((byte)KeysVirtual.VolumeMute, false); }

                else if (remoteData.Contains("MeVolUp"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolUp", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingle((byte)KeysVirtual.VolumeUp, false); }
                }

                else if (remoteData.Contains("MeVolDown"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolDown", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingle((byte)KeysVirtual.VolumeDown, false); }
                }

                else if (remoteData.Contains("MeFullscreen"))
                {
                    await KeyPressCombo((byte)KeysVirtual.Menu, (byte)KeysVirtual.Return, false);
                }

                else if (remoteData.Contains("MeAltTab"))
                {
                    await KeyPressCombo((byte)KeysVirtual.Menu, (byte)KeysVirtual.Tab, false);
                }
            }
            catch { }
        }
    }
}