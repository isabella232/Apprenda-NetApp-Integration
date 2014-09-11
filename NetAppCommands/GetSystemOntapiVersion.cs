using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using NetApp;
using NetApp.Filer;
using NetApp.Filer.System;
using Apprenda.SaaSGrid.Addons.NetApp.v1.Models;
using Apprenda.SaaSGrid.Addons;

// use of this namespace is end of life and will be deprecated in the new release.
namespace Apprenda.SaaSGrid.Addons.NetApp.v1
{
    public static class GetSystemOntapiVersionCommand
    {
        public static OperationResult ProcessRecord()
        {
            OperationResult or = new OperationResult();
            try
            {
                SystemGetOntapiVersion input = new SystemGetOntapiVersion();
                SystemGetOntapiVersionResult output = input.Invoke(NetAppModel.GetFiler());
                or.IsSuccess = true;
                or.EndUserMessage += "Major Version: " + output.MajorVersion + " Minor Version: " + output.MinorVersion + "\n";
                return or;
            }
            catch (NaException e)
            {
                or.IsSuccess = false;
                or.EndUserMessage = e.Message;
                return or;
            }
        }

    }
}
