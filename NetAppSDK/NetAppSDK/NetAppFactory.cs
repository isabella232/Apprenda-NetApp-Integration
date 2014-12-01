using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Apprenda.SaaSGrid.Addons.NetApp.V2.Models;
using Apprenda.SaaSGrid.Addons.NetApp;
using System.Collections.ObjectModel;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2
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
        public NetAppResponse CreateVolume(DeveloperOptions d)
        { 
            using (PowerShell PsInstance = PowerShell.Create())
            {
                Console.WriteLine("We are getting here!");
                
                String commandBuilder = "./PsScripts/CreateVolume.ps1" + " -username " + d.AdminUserName + " -password " + d.AdminPassword + " -vserver " + d.VServer +
                    " -endpoint " + d.ClusterMgtEndpoint;
                
                foreach(Tuple<String, String> p in d.VolumeToProvision.ToPsArguments())
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
        public NetAppResponse DeleteVolume(DeveloperOptions d) 
        {
            using (PowerShell PsInstance = PowerShell.Create())
            {
                // this will chang to reflect the actual invocation. we're going to 
                // have to build the command execution based on the volume's properties
                String commandBuilder = "./PsScripts/CreateVolume.ps1" + " -username " + d.AdminUserName + " -password " + d.AdminPassword + " -vserver " + d.VServer +
                " -endpoint " + d.ClusterMgtEndpoint;
                foreach (Tuple<String, String> p in d.VolumeToProvision.ToPsArguments())
                {
                    commandBuilder += " " + p.Item1 + " " + p.Item2;
                }
                PsInstance.AddScript("./PsScripts/DeleteVolume.ps1");
                
                Collection<PSObject> output = PsInstance.Invoke();
                // build the NetAppResponse
                NetAppResponse response = NetAppResponse.ParseOutput(output);
                return response;
            } 
        }

        // aggregate methods
        // this will create an aggregate on the cluster
        public NetAppResponse CreateAggregate(Aggregate a) 
        {
            using (PowerShell PsInstance = PowerShell.Create())
            {
                // this will chang to reflect the actual invocation. we're going to 
                // have to build the command execution based on the volume's properties
                PsInstance.AddScript("./PsScripts/CreateAggregate.ps1");
                foreach (Tuple<String, String> p in a.ToPsArguments())
                {
                    PsInstance.AddParameter(p.Item1, p.Item2);
                }
                Collection<PSObject> output = PsInstance.Invoke();
                // build the NetAppResponse
                NetAppResponse response = NetAppResponse.ParseOutput(output);
                return response;
            }
        }
        
        // this will delete the aggregate on the cluster
        public NetAppResponse DeleteAggregate(Aggregate a) 
        {
            using (PowerShell PsInstance = PowerShell.Create())
            {
                // this will chang to reflect the actual invocation. we're going to 
                // have to build the command execution based on the volume's properties
                PsInstance.AddScript("./PsScripts/DeleteAggregate.ps1");
                foreach (Tuple<String, String> p in a.ToPsArguments())
                {
                    PsInstance.AddParameter(p.Item1, p.Item2);
                }
                Collection<PSObject> output = PsInstance.Invoke();
                // build the NetAppResponse
                NetAppResponse response = NetAppResponse.ParseOutput(output);
                return response;
            }
        }

        public string GetVolumeInfo(string p)
        {
            throw new NotImplementedException();
        }

        public string GetAggregateInfo(string a)
        {
            throw new NotImplementedException();
        }
    }
}
