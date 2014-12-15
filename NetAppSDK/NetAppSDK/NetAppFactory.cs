using Apprenda.SaaSGrid.Addons.NetApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class NetAppFactory
    {
        // pragma properties
        private static NetAppFactory _factory;

        // pragma constructors

        private NetAppFactory()
        {
            // exists to defeat instantiation
        }

        public static NetAppFactory GetInstance()
        {
            if (_factory == null)
            {
                _factory = new NetAppFactory();
                _factory.init();
                return _factory;
            }
            else return _factory;
        }

        private void init()
        {
            // create a powershell connection to be reused for each command
        }

        // pragma public methods - these (and their overloads) will be used by the consumers.

        // This will create a volume off a given filer.
        public static NetAppResponse CreateVolume(DeveloperOptions d)
        {
            using (PowerShell PsInstance = PowerShell.Create())
            {
                Console.WriteLine("We are getting here!");

                String commandBuilder = "./PsScripts/CreateVolume.ps1" + " -username " + d.AdminUserName + " -password " + d.AdminPassword + " -vserver " + d.VServer +
                    " -endpoint " + d.ClusterMgtEndpoint;

                foreach (Tuple<String, String> p in d.VolumeToProvision.ToPsArguments())
                {
                    Console.WriteLine("Debug - p1: " + p.Item1 + " p2: " + p.Item2);
                    commandBuilder = commandBuilder + " " + p.Item1 + " " + p.Item2;
                }
                PsInstance.AddScript(commandBuilder);
                Collection<PSObject> output = PsInstance.Invoke();
                // build the NetAppResponse
                return NetAppResponse.ParseOutput(output);
            }
        }

        // This will delete a volume off of a given filer.
        public static NetAppResponse DeleteVolume(DeveloperOptions d)
        {
            using (var psInstance = PowerShell.Create())
            {
                // this will chang to reflect the actual invocation. we're going to
                // have to build the command execution based on the volume's properties
                var commandBuilder = " -username " + d.AdminUserName + " -password " + d.AdminPassword + " -vserver " + d.VServer +
                " -endpoint " + d.ClusterMgtEndpoint;
                var psArguments = d.VolumeToProvision.ToPsArguments();
                if (psArguments != null) commandBuilder = psArguments.Aggregate(commandBuilder, (current, p) => current + (" " + p.Item1 + " " + p.Item2));
                psInstance.AddScript("./PsScripts/DeleteVolume.ps1");
                psInstance.AddArgument(commandBuilder);
                Collection<PSObject> output = psInstance.Invoke();
                // build the NetAppResponse
                NetAppResponse response = NetAppResponse.ParseOutput(output);
                return response;
            }
        }

        // int would be the error code that comes back. need to do a better job of error handling
        internal static int CreateSnapMirror(DeveloperOptions developerOptions)
        {
            // TODO
            return 0;
        }

        // int would be the error code that comes back. need to do a better job of error handling
        internal static int CreateSnapVault(DeveloperOptions developerOptions)
        {
            //TODO
            return 0;
        }
    }
}