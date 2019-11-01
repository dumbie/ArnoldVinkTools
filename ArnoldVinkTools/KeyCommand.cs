using System;
using System.Threading.Tasks;
using static Classes.Classes;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //MediaRemoteMe Key Command
        async Task KeyCommand(string RemoteData)
        {
            try
            {
                if (RemoteData.Contains("MeCenterCamera")) { await KeyPressSingle((byte)VirtualKeys.Home, false); }
                else if (RemoteData.Contains("MeStop")) { await KeyPressSingle((byte)VirtualKeys.MediaStop, false); }
                else if (RemoteData.Contains("MePlayPause")) { await KeyPressSingle((byte)VirtualKeys.MediaPlayPause, false); }
                else if (RemoteData.Contains("MeNextSong")) { await KeyPressSingle((byte)VirtualKeys.MediaNextTrack, false); }
                else if (RemoteData.Contains("MePrevSong")) { await KeyPressSingle((byte)VirtualKeys.MediaPrevTrack, false); }
                else if (RemoteData.Contains("MeMenu"))
                {
                    await KeyPressSingle((byte)VirtualKeys.C, false); //C
                    await KeyPressSingle((byte)VirtualKeys.Menu, false); //Alt
                }
                else if (RemoteData.Contains("MeEsc")) { await KeyPressSingle((byte)VirtualKeys.Escape, false); }
                else if (RemoteData.Contains("MeTab")) { await KeyPressSingle((byte)VirtualKeys.Tab, false); }
                else if (RemoteData.Contains("MeEnter")) { await KeyPressSingle((byte)VirtualKeys.Return, false); }
                else if (RemoteData.Contains("MeSpace")) { await KeyPressSingle((byte)VirtualKeys.Space, false); }
                else if (RemoteData.Contains("MeBackSpace")) { await KeyPressSingle((byte)VirtualKeys.Back, false); }
                else if (RemoteData.Contains("MeArrowLeft")) { await KeyPressSingle((byte)VirtualKeys.Left, true); }
                else if (RemoteData.Contains("MeArrowUp")) { await KeyPressSingle((byte)VirtualKeys.Up, true); }
                else if (RemoteData.Contains("MeArrowRight")) { await KeyPressSingle((byte)VirtualKeys.Right, true); }
                else if (RemoteData.Contains("MeArrowDown")) { await KeyPressSingle((byte)VirtualKeys.Down, true); }
                else if (RemoteData.Contains("Me0")) { await KeyPressSingle((byte)VirtualKeys.N0, false); }
                else if (RemoteData.Contains("Me1")) { await KeyPressSingle((byte)VirtualKeys.N1, false); }
                else if (RemoteData.Contains("Me2")) { await KeyPressSingle((byte)VirtualKeys.N2, false); }
                else if (RemoteData.Contains("Me3")) { await KeyPressSingle((byte)VirtualKeys.N3, false); }
                else if (RemoteData.Contains("Me4")) { await KeyPressSingle((byte)VirtualKeys.N4, false); }
                else if (RemoteData.Contains("Me5")) { await KeyPressSingle((byte)VirtualKeys.N5, false); }
                else if (RemoteData.Contains("Me6")) { await KeyPressSingle((byte)VirtualKeys.N6, false); }
                else if (RemoteData.Contains("Me7")) { await KeyPressSingle((byte)VirtualKeys.N7, false); }
                else if (RemoteData.Contains("Me8")) { await KeyPressSingle((byte)VirtualKeys.N8, false); }
                else if (RemoteData.Contains("Me9")) { await KeyPressSingle((byte)VirtualKeys.N9, false); }
                else if (RemoteData.Contains("MeVolMute")) { await KeyPressSingle((byte)VirtualKeys.VolumeMute, false); }

                else if (RemoteData.Contains("MeVolUp"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(RemoteData.Replace("MeVolUp", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingle((byte)VirtualKeys.VolumeUp, false); }
                }

                else if (RemoteData.Contains("MeVolDown"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(RemoteData.Replace("MeVolDown", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingle((byte)VirtualKeys.VolumeDown, false); }
                }

                else if (RemoteData.Contains("MeFullscreen"))
                {
                    await KeyPressCombo((byte)VirtualKeys.Menu, (byte)VirtualKeys.Return, false);
                }

                else if (RemoteData.Contains("MeAltTab"))
                {
                    await KeyPressCombo((byte)VirtualKeys.Menu, (byte)VirtualKeys.Tab, false);
                }
            }
            catch { }
        }
    }
}