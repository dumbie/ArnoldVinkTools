using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ArnoldVinkTools
{
    partial class MainPage
    {
        //Application Dll Imports
        [DllImport("FirewallAPI.dll")]
        static extern uint NetworkIsolationEnumAppContainers(uint Flags, out uint pdwNumPublicAppCs, out IntPtr ppPublicAppCs);
        [DllImport("FirewallAPI.dll")]
        static extern uint NetworkIsolationSetAppContainerConfig(uint dwNumPublicAppCs, SID_AND_ATTRIBUTES[] appContainerSids);
        [DllImport("FirewallAPI.dll")]
        static extern uint NetworkIsolationGetAppContainerConfig(out uint pdwCntACs, out IntPtr appContainerSids);
        [DllImport("Advapi32.dll")]
        static extern bool ConvertSidToStringSid(IntPtr pSid, out string strSid);

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct INET_FIREWALL_AC_BINARIES
        {
            public uint Count;
            public IntPtr Binaries;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct INET_FIREWALL_AC_CAPABILITIES
        {
            public uint Count;
            public IntPtr Capabilities;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public uint Attributes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        struct INET_FIREWALL_APP_CONTAINER
        {
            public IntPtr appContainerSid;
            public IntPtr userSid;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string appContainerName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string displayName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string description;
            public INET_FIREWALL_AC_CAPABILITIES Capabilities;
            public INET_FIREWALL_AC_BINARIES Binaries;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string workingDirectory;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string packageFullName;
        }

        //List supported apps from firewall
        List<INET_FIREWALL_APP_CONTAINER> ListAllApps()
        {
            try
            {
                uint arraySize = 0;
                IntPtr arrayValue = IntPtr.Zero;
                NetworkIsolationEnumAppContainers(0, out arraySize, out arrayValue);
                List<INET_FIREWALL_APP_CONTAINER> AppList = new List<INET_FIREWALL_APP_CONTAINER>();
                for (int i = 0; i < arraySize; i++)
                {
                    AppList.Add((INET_FIREWALL_APP_CONTAINER)Marshal.PtrToStructure(arrayValue, typeof(INET_FIREWALL_APP_CONTAINER)));
                    arrayValue = new IntPtr((long)arrayValue + Marshal.SizeOf(typeof(INET_FIREWALL_APP_CONTAINER)));
                }
                return AppList;
            }
            catch { return new List<INET_FIREWALL_APP_CONTAINER>(); }
        }

        //List enabled apps from firewall
        List<SID_AND_ATTRIBUTES> ListEnabledApps()
        {
            try
            {
                uint arraySize = 0;
                IntPtr arrayValue = IntPtr.Zero;
                NetworkIsolationGetAppContainerConfig(out arraySize, out arrayValue);
                List<SID_AND_ATTRIBUTES> AppList = new List<SID_AND_ATTRIBUTES>();
                for (int i = 0; i < arraySize; i++)
                {
                    AppList.Add((SID_AND_ATTRIBUTES)Marshal.PtrToStructure(arrayValue, typeof(SID_AND_ATTRIBUTES)));
                    arrayValue = new IntPtr((long)arrayValue + Marshal.SizeOf(typeof(SID_AND_ATTRIBUTES)));
                }
                return AppList;
            }
            catch { return new List<SID_AND_ATTRIBUTES>(); }
        }

        //Whitelist all supported apps in firewall
        void WhitelistApps()
        {
            try
            {
                //Debug.WriteLine("OldTotalApps:" + ListAllApps().Count);
                //Debug.WriteLine("OldEnabledApps:" + ListEnabledApps().Count);

                //Count new white listing apps
                int CountTotal = 0;
                foreach (INET_FIREWALL_APP_CONTAINER AllStoreApp in ListAllApps())
                {
                    bool IsWhitelisted = false;
                    foreach (SID_AND_ATTRIBUTES EnabledStoreApp in ListEnabledApps())
                    {
                        string EnabledId, AllId;
                        ConvertSidToStringSid(EnabledStoreApp.Sid, out EnabledId);
                        ConvertSidToStringSid(AllStoreApp.appContainerSid, out AllId);
                        if (EnabledId == AllId)
                        {
                            IsWhitelisted = true;
                            //Debug.WriteLine("ReCount: " + CountTotal + "/" + AllStoreApp.displayName + "/" + AllStoreApp.appContainerName + "/" + AllStoreApp.appContainerSid);
                            CountTotal++;
                        }
                    }
                    if (!IsWhitelisted && (AllStoreApp.appContainerName.Contains("54655ArnoldVink") || AllStoreApp.appContainerName.Contains("54655arnoldvink")))
                    {
                        //Debug.WriteLine("NewCount: " + CountTotal + "/" + AllStoreApp.displayName + "/" + AllStoreApp.appContainerName + "/" + AllStoreApp.appContainerSid);
                        CountTotal++;
                    }
                }

                //Set new white listing apps
                int CountSet = 0;
                SID_AND_ATTRIBUTES[] SidAndAttributes = new SID_AND_ATTRIBUTES[CountTotal];
                foreach (INET_FIREWALL_APP_CONTAINER AllStoreApp in ListAllApps())
                {
                    bool IsWhitelisted = false;
                    foreach (SID_AND_ATTRIBUTES EnabledStoreApp in ListEnabledApps())
                    {
                        string EnabledId, AllId;
                        ConvertSidToStringSid(EnabledStoreApp.Sid, out EnabledId);
                        ConvertSidToStringSid(AllStoreApp.appContainerSid, out AllId);
                        if (EnabledId == AllId)
                        {
                            IsWhitelisted = true;
                            //Debug.WriteLine("ReSet: " + CountSet + "/" + AllStoreApp.displayName + "/" + AllStoreApp.appContainerName + "/" + AllStoreApp.appContainerSid);
                            SidAndAttributes[CountSet].Sid = AllStoreApp.appContainerSid;
                            CountSet++;
                        }
                    }
                    if (!IsWhitelisted && (AllStoreApp.appContainerName.Contains("54655ArnoldVink") || AllStoreApp.appContainerName.Contains("54655arnoldvink")))
                    {
                        //Debug.WriteLine("NewSet: " + CountSet + "/" + AllStoreApp.displayName + "/" + AllStoreApp.appContainerName + "/" + AllStoreApp.appContainerSid);
                        SidAndAttributes[CountSet].Sid = AllStoreApp.appContainerSid;
                        CountSet++;
                    }
                }

                //Update to new sid and attributes list
                if (NetworkIsolationSetAppContainerConfig((uint)CountTotal, SidAndAttributes) == 0)
                {
                    //Debug.WriteLine("NewTotalApps:" + ListAllApps().Count);
                    //Debug.WriteLine("NewEnabledApps:" + ListEnabledApps().Count);
                    Debug.WriteLine("Whitelisted all supported and previous store apps.");
                }
            }
            catch
            { Debug.WriteLine("Failed to white list supported store apps."); }
        }
    }
}