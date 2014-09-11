using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApp;
using NetApp.Filer;
using NetApp.Filer.System;
using System.Net;
using Apprenda.SaaSGrid.Addons;

namespace Apprenda.SaaSGrid.Addons.NetApp.v1.Models
{
    public static class NetAppModel
    {
        private static string _server = "192.168.246.69";
        private static string _user = "root";
        private static string _password = "cyrixm2r";

        public static string Server
        {
            get { return _server; }
            set { _server = value; }
        }


        public static string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public static void LoadInfoFromManifest(AddonManifest manifest)
        {
            var manifestProperties = manifest.GetProperties();
            foreach(var manifestProperty in manifestProperties)
            {
                switch(manifestProperty.Key)
                {
                    case("server"):
                        NetAppModel.Server = manifestProperty.Value;
                        break;
                    case("username"):
                        NetAppModel.User = manifestProperty.Value;
                        break;
                    case("password"):
                        NetAppModel.Password = manifestProperty.Value;
                        break;
                    default: // means there are other manifest properties we don't need.
                        break;
                }
            }
        }

        public static NaFiler GetFiler()
        {
            NaFiler server = new NaFiler(Server);
            // for testing purposes
            server.ForceUseUnsecure = true;
            server.Protocol = ServerProtocol.HTTPS;
            server.Port = (int)ServerPort.HttpsOntapSecurePort;
            server.Trace = true;
            server.Credentials = new NetworkCredential(User, Password);
            return server;
        }
    }
}
