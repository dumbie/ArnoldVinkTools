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
                if (remoteData.Contains("MeCenterCamera")) { await KeyPressSingleAuto(KeysVirtual.Home); }
                else if (remoteData.Contains("MeStop")) { await KeyPressSingleAuto(KeysVirtual.MediaStop); }
                else if (remoteData.Contains("MePlayPause")) { await KeyPressSingleAuto(KeysVirtual.MediaPlayPause); }
                else if (remoteData.Contains("MeNextSong")) { await KeyPressSingleAuto(KeysVirtual.MediaNextTrack); }
                else if (remoteData.Contains("MePrevSong")) { await KeyPressSingleAuto(KeysVirtual.MediaPreviousTrack); }
                else if (remoteData.Contains("MeMenu"))
                {
                    await KeyPressSingleAuto(KeysVirtual.C);
                    await KeyPressSingleAuto(KeysVirtual.Alt);
                }
                else if (remoteData.Contains("MeEsc")) { await KeyPressSingleAuto(KeysVirtual.Escape); }
                else if (remoteData.Contains("MeTab")) { await KeyPressSingleAuto(KeysVirtual.Tab); }
                else if (remoteData.Contains("MeEnter")) { await KeyPressSingleAuto(KeysVirtual.Enter); }
                else if (remoteData.Contains("MeSpace")) { await KeyPressSingleAuto(KeysVirtual.Space); }
                else if (remoteData.Contains("MeBackSpace")) { await KeyPressSingleAuto(KeysVirtual.BackSpace); }
                else if (remoteData.Contains("MeArrowLeft")) { await KeyPressSingleAuto(KeysVirtual.Left); }
                else if (remoteData.Contains("MeArrowUp")) { await KeyPressSingleAuto(KeysVirtual.Up); }
                else if (remoteData.Contains("MeArrowRight")) { await KeyPressSingleAuto(KeysVirtual.Right); }
                else if (remoteData.Contains("MeArrowDown")) { await KeyPressSingleAuto(KeysVirtual.Down); }
                else if (remoteData.Contains("Me0")) { await KeyPressSingleAuto(KeysVirtual.Digit0); }
                else if (remoteData.Contains("Me1")) { await KeyPressSingleAuto(KeysVirtual.Digit1); }
                else if (remoteData.Contains("Me2")) { await KeyPressSingleAuto(KeysVirtual.Digit2); }
                else if (remoteData.Contains("Me3")) { await KeyPressSingleAuto(KeysVirtual.Digit3); }
                else if (remoteData.Contains("Me4")) { await KeyPressSingleAuto(KeysVirtual.Digit4); }
                else if (remoteData.Contains("Me5")) { await KeyPressSingleAuto(KeysVirtual.Digit5); }
                else if (remoteData.Contains("Me6")) { await KeyPressSingleAuto(KeysVirtual.Digit6); }
                else if (remoteData.Contains("Me7")) { await KeyPressSingleAuto(KeysVirtual.Digit7); }
                else if (remoteData.Contains("Me8")) { await KeyPressSingleAuto(KeysVirtual.Digit8); }
                else if (remoteData.Contains("Me9")) { await KeyPressSingleAuto(KeysVirtual.Digit9); }
                else if (remoteData.Contains("MeVolMute")) { await KeyPressSingleAuto(KeysVirtual.VolumeMute); }

                else if (remoteData.Contains("MeVolUp"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolUp", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingleAuto(KeysVirtual.VolumeUp); }
                }

                else if (remoteData.Contains("MeVolDown"))
                {
                    int VolSkip = 0;
                    try { VolSkip = Convert.ToInt32(remoteData.Replace("MeVolDown", "")); } catch { VolSkip = 2; }
                    for (int i = 0; i < VolSkip; i++) { await KeyPressSingleAuto(KeysVirtual.VolumeDown); }
                }

                else if (remoteData.Contains("MeFullscreen"))
                {
                    await KeyPressComboAuto(KeysVirtual.Alt, KeysVirtual.Enter);
                }

                else if (remoteData.Contains("MeAltTab"))
                {
                    await KeyPressComboAuto(KeysVirtual.Alt, KeysVirtual.Tab);
                }
            }
            catch { }
        }
    }
}