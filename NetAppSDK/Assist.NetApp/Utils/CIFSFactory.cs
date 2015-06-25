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

        public static int ResourceConnected { get; } = 0x00000001;
        public static int ResourceGlobalnet { get; } = 0x00000002;
        public static int ResourceRemembered { get; } = 0x00000003;

        public static int ResourcetypeAny { get; } = 0x00000000;
        public static int ResourcetypeDisk { get; } = 0x00000001;
        public static int ResourcetypePrint { get; } = 0x00000002;

        public static int ResourcedisplaytypeGeneric { get; } = 0x00000000;
        public static int ResourcedisplaytypeDomain { get; } = 0x00000001;
        public static int ResourcedisplaytypeServer { get; } = 0x00000002;
        public static int ResourcedisplaytypeShare { get; } = 0x00000003;
        public static int ResourcedisplaytypeFile { get; } = 0x00000004;
        public static int ResourcedisplaytypeGroup { get; } = 0x00000005;
        public static int ResourceusageConnectable { get; } = 0x00000001;
        public static int ResourceusageContainer { get; } = 0x00000002;


        public static int ConnectInteractive { get; } = 0x00000008;
        public static int ConnectPrompt { get; } = 0x00000010;
        public static int ConnectRedirect { get; } = 0x00000080;
        public static int ConnectUpdateProfile { get; } = 0x00000001;
        public static int ConnectCommandline { get; } = 0x00000800;
        public static int ConnectCmdSavecred { get; } = 0x00001000;

        public static int ConnectLocaldrive { get; } = 0x00000100;

        #endregion

        #region Errors

        public static int NoError { get; } = 0;
        public static int ErrorAccessDenied { get; } = 5;
        public static int ErrorAlreadyAssigned { get; } = 85;
        public static int ErrorBadDevice { get; } = 1200;
        public static int ErrorBadNetName { get; } = 67;
        public static int ErrorBadProvider { get; } = 1204;
        public static int ErrorCancelled { get; } = 1223;
        public static int ErrorExtendedError { get; } = 1208;
        public static int ErrorInvalidAddress { get; } = 487;
        public static int ErrorInvalidParameter { get; } = 87;
        public static int ErrorInvalidPassword { get; } = 1216;
        public static int ErrorMoreData { get; } = 234;
        public static int ErrorNoMoreItems { get; } = 259;
        public static int ErrorNoNetOrBadPath { get; } = 1203;
        public static int ErrorNoNetwork { get; } = 1222;

        public static int ErrorBadProfile { get; } = 1206;
        public static int ErrorCannotOpenProfile { get; } = 1205;
        public static int ErrorDeviceInUse { get; } = 2404;
        public static int ErrorNotConnected { get; } = 2250;
        public static int ErrorOpenFiles { get; } = 2401;

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
            //public int DwScope { get; private set; } = 0;
            public int DwType { get; set; } = 0;
            //public int DwDisplayType { get; } = 0;
            //public int DwUsage { get; } = 0;
            //public string LpLocalName { get; } = "";
            public string LpRemoteName { get; set; } = "";
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

