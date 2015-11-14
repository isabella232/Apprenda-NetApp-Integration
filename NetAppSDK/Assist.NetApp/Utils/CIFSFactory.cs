using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Assist.NetApp.Utils
{
    public class CifsFactory
    {
        private string _remoteUncName;
        private string _remoteComputerName;

        public string RemoteComputerName {
            get
            {
                return _remoteComputerName;
            }
            set
            {
                _remoteComputerName = value;
                _remoteUncName = @"\\" + _remoteComputerName;
                
            }
        }

        public string ToFtp(bool secure)
        {
            return secure ? "sftp://" + RemoteComputerName : "ftp://" + RemoteComputerName;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        #region Consts

        private static int ResourceConnected { get; }
        public static int ResourceGlobalnet { get; }
        public static int ResourceRemembered { get; }

        public static int ResourcetypeAny { get; }
        public static int ResourcetypeDisk { get; }
        public static int ResourcetypePrint { get; }

        public static int ResourcedisplaytypeGeneric { get; }
        public static int ResourcedisplaytypeDomain { get; }
        public static int ResourcedisplaytypeServer { get; }
        public static int ResourcedisplaytypeShare { get; }
        public static int ResourcedisplaytypeFile { get; }
        public static int ResourcedisplaytypeGroup { get; }
        public static int ResourceusageConnectable { get; }
        public static int ResourceusageContainer { get; }


        public static int ConnectInteractive { get; }
        public static int ConnectPrompt { get; }
        public static int ConnectRedirect { get; }
        public static int ConnectUpdateProfile { get; }
        public static int ConnectCommandline { get; }
        public static int ConnectCmdSavecred { get; }

        public static int ConnectLocaldrive { get; }

        #endregion

        #region Errors

        public static int NoError { get; }
        public static int ErrorAccessDenied { get; }
        public static int ErrorAlreadyAssigned { get; }
        public static int ErrorBadDevice { get; }
        public static int ErrorBadNetName { get; }
        public static int ErrorBadProvider { get; }
        public static int ErrorCancelled { get; }
        public static int ErrorExtendedError { get; }
        public static int ErrorInvalidAddress { get; }
        public static int ErrorInvalidParameter { get; }
        public static int ErrorInvalidPassword { get; }
        public static int ErrorMoreData { get; }
        public static int ErrorNoMoreItems { get; }
        public static int ErrorNoNetOrBadPath { get; }
        public static int ErrorNoNetwork { get; }

        public static int ErrorBadProfile { get; }
        public static int ErrorCannotOpenProfile { get; }
        public static int ErrorDeviceInUse { get; }
        public static int ErrorNotConnected { get; }
        public static int ErrorOpenFiles { get; }

        #endregion

        #region PInvoke Signatures

        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            Netresource lpNetResource,
            string lpPassword,
            string lpUserId,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult
            );

        [StructLayout(LayoutKind.Sequential)]
        private class Netresource
        {
            private string _lpRemoteName = "";
            //public int DwScope { get; private set; } = 0;
            public int DwType { get; set; }
            //public int DwDisplayType { get; } = 0;
            //public int DwUsage { get; } = 0;
            //public string LpLocalName { get; } = "";
            public string LpRemoteName
            {
                get { return _lpRemoteName; }
                set { _lpRemoteName = value; }
            }

            //public string LpComment { get; } = "";
            //public string LpProvider { get; } = "";
        }

        #endregion

        public static CifsFactory Access(string remoteComputerName, string userName, string password)
        {
            return new CifsFactory(remoteComputerName,
                                            userName,
                                            password);
        }

        private CifsFactory(string remoteComputerName, string userName, string password)
        {
            RemoteComputerName = remoteComputerName;
            UserName = userName;
            Password = password;

            ConnectToShare(_remoteUncName, UserName, Password, false);
        }

        static CifsFactory()
        {
            ErrorOpenFiles = 2401;
            ErrorNotConnected = 2250;
            ErrorDeviceInUse = 2404;
            ErrorCannotOpenProfile = 1205;
            ErrorBadProfile = 1206;
            ErrorNoNetwork = 1222;
            ErrorNoNetOrBadPath = 1203;
            ErrorNoMoreItems = 259;
            ErrorMoreData = 234;
            ErrorInvalidPassword = 1216;
            ErrorInvalidParameter = 87;
            ErrorInvalidAddress = 487;
            ErrorExtendedError = 1208;
            ErrorCancelled = 1223;
            ErrorBadProvider = 1204;
            ErrorBadNetName = 67;
            ErrorBadDevice = 1200;
            ErrorAlreadyAssigned = 85;
            ErrorAccessDenied = 5;
            NoError = 0;
            ConnectLocaldrive = 0x00000100;
            ConnectCmdSavecred = 0x00001000;
            ConnectCommandline = 0x00000800;
            ConnectUpdateProfile = 0x00000001;
            ConnectRedirect = 0x00000080;
            ConnectPrompt = 0x00000010;
            ConnectInteractive = 0x00000008;
            ResourceusageContainer = 0x00000002;
            ResourceusageConnectable = 0x00000001;
            ResourcedisplaytypeGroup = 0x00000005;
            ResourcedisplaytypeFile = 0x00000004;
            ResourcedisplaytypeShare = 0x00000003;
            ResourcedisplaytypeServer = 0x00000002;
            ResourcedisplaytypeDomain = 0x00000001;
            ResourcedisplaytypeGeneric = 0x00000000;
            ResourcetypePrint = 0x00000002;
            ResourcetypeDisk = 0x00000001;
            ResourcetypeAny = 0x00000000;
            ResourceRemembered = 0x00000003;
            ResourceGlobalnet = 0x00000002;
            ResourceConnected = 0x00000001;
        }

        private static void ConnectToShare(string remoteUnc, string username, string password, bool promptUser)
        {
            var nr = new Netresource
            {
                DwType = ResourcetypeDisk,
                LpRemoteName = remoteUnc
            };

            var result = promptUser ? WNetUseConnection(IntPtr.Zero, nr, "", "", ConnectInteractive | ConnectPrompt, null, null, null) : WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);

            if (result != NoError)
            {
                throw new Win32Exception(result);
            }
        }
    }
}

