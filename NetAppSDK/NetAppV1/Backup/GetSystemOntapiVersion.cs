using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using NetApp;
using NetApp.Filer;
using NetApp.Filer.System;


namespace NetApp.Powershell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SystemOntapiVersion")]
    public class GetSystemOntapiVersionCommand : Cmdlet
    {
        private string _server = null;
        private string _user = null;
        private string _password = null;


        [Parameter(Mandatory = true)]
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }
        

        [Parameter(Mandatory = true)]
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
        
        [Parameter(Mandatory = true)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        


        protected override void ProcessRecord()
        {
            NaFiler server = null;
            server = new NaFiler(_server);

            server.Credentials = new NetworkCredential(_user, _password);

            try
            {
                SystemGetOntapiVersion input = new SystemGetOntapiVersion();
                SystemGetOntapiVersionResult output = input.Invoke(server);

                WriteObject("Major Version: " + output.MajorVersion);
                WriteObject("Minor Version: " + output.MinorVersion);
            }
            catch (NaException e)
            {
                WriteObject(e.Message);
            }
        }

    }
}
