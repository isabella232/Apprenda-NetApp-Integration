using System;
using System.IO;
using System.Net;
using Assist.NetApp.Utils;

namespace Assist.NetApp
{
    public class NetAppHelper
    {
        public enum FileProtocol { FTP=0, SFTP=1, SCP=2, UncMapping=3 }

        public static string GetRepositoryInfo(string token, FileProtocol protocol)
        {
            var info = NetAppInfo.Parse(token);
            if (info.errorInfo.Length.Equals(0))
            {
                var repo = CifsFactory.Access(info.DirectoryPath, info.UserName, info.Password);
                switch (protocol)
                {
                    case FileProtocol.FTP:
                        var request = FtpWebRequest.Create(repo.ToFtp(false));
                        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                        request.Credentials = new NetworkCredential(repo.UserName, repo.Password);
                        var response = request.GetResponse();
                        var output = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        return output; 
                    case FileProtocol.SFTP:
                        break;
                    case FileProtocol.SCP:
                        break;
                    case FileProtocol.UncMapping:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(protocol), protocol, null);
                }

            }
            else
            {
                throw new Exception("Unable to retrive info, please check your app.config or web.config");
            }
            return "";
        }
    }
}
