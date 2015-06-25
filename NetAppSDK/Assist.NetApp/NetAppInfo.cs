using System;
using System.Configuration;
using Newtonsoft.Json;

namespace Assist.NetApp
{
    public class NetAppInfo
    {
        public string DirectoryPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string errorInfo { get; set; }

    public static NetAppInfo Parse(string token)
        {
            try
            {
                // check for the token in the connection string of app settings
                var json = ConfigurationManager.AppSettings["token"];
                return JsonConvert.DeserializeObject<NetAppInfo>(json);
            }
            catch (Exception e)
            {
                return new NetAppInfo
                {
                    errorInfo = e.Message
                };
            }
            
        }

    }
}
