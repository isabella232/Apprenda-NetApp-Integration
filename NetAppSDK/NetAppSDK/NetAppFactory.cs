using System.IO;
using Apprenda.SaaSGrid.Addons.NetApp.Annotations;
using Apprenda.SaaSGrid.Addons.NetApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public static class NetAppFactory
    {
        // This will create a volume off a given filer.
        [NotNull]
        public static NetAppResponse CreateVolume([NotNull] DeveloperOptions d)
        {
            try
            {
                using (var psInstance = PowerShell.Create())
                {
                    Console.WriteLine("We are getting here!");
                    var ps1File = Path.GetFullPath("./PsScripts/CreateVolume.ps1");
                    var commandBuilder = ps1File + " -username " + d.AdminUserName +
                                         " -password " +
                                         d.AdminPassword + " -vserver " + d.VServer +
                                         " -endpoint " + d.ClusterMgtEndpoint;
                    var psArguments = d.VolumeToProvision.ToPsArguments();
                    if (psArguments != null)
                        foreach (var p in psArguments)
                        {
                            Console.WriteLine("Debug - p1: " + p.Item1 + " p2: " + p.Item2);
                            commandBuilder = commandBuilder + " " + p.Item1 + " " + p.Item2;
                        }
                    psInstance.AddScript(commandBuilder);
                    var output = psInstance.Invoke();
                    var debugStream = "";
                    var errorStream = "";
                    if (psInstance.Streams.Error.Count <= 0)
                        return new NetAppResponse()
                        {
                            ConnectionData = d.VolumeToProvision.Name,
                            IsSuccess = true,
                            ReturnCode = 0,
                            ConsoleOut = output.ToString()
                        };
                    errorStream = psInstance.Streams.Error.Aggregate(errorStream, (current, error) => current + error);
                    debugStream = psInstance.Streams.Progress.Aggregate(debugStream, (current, debug) => current + debug);
                    return new NetAppResponse()
                    {
                        IsSuccess = false,
                        ReturnCode = 1,
                        ConsoleOut = debugStream,
                        ErrorOut = errorStream
                    };
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        // This will delete a volume off of a given filer.
        public static NetAppResponse DeleteVolume(DeveloperOptions d)
        {
            using (var psInstance = PowerShell.Create())
            {
                // this will chang to reflect the actual invocation. we're going to
                // have to build the command execution based on the volume's properties
                var commandBuilder = "./PsScripts/DeleteVolume.ps1" + " -username " + d.AdminUserName + " -password " +
                                     d.AdminPassword + " -vserver " + d.VServer +
                                     " -endpoint " + d.ClusterMgtEndpoint;
                var psArguments = d.VolumeToProvision.ToPsArguments();
                if (psArguments != null)
                    commandBuilder = psArguments.Aggregate(commandBuilder,
                        (current, p) => current + (" " + p.Item1 + " " + p.Item2));
                psInstance.AddScript(commandBuilder);
                psInstance.AddArgument(commandBuilder);
                var output = psInstance.Invoke();
                // build the NetAppResponse
                return new NetAppResponse() { IsSuccess = true, ReturnCode = 0, ConsoleOut = output.ToString() };
            }
        }

        private static NetAppResponse ProcessPsOutput([NotNull] IEnumerable<PSObject> output, PowerShell psInstance, Volume volume)
        {
            if (output == null) throw new ArgumentNullException("output");
            var netappResponse = new NetAppResponse
            {
                ErrorOut = psInstance.Streams.Error.ReadAll().ToString(),
                ConsoleOut = output.ToString()
            };

            foreach (var outputItem in output)
            {
                if (outputItem != null && outputItem.BaseObject.GetType().IsPrimitive)
                {
                    int returnCode;
                    int.TryParse(outputItem.BaseObject.ToString(), out returnCode);

                    if (returnCode > 1) // failed
                    {
                        netappResponse.IsSuccess = false;
                        netappResponse.ReturnCode = 2;
                    }
                    else switch (returnCode)
                        {
                            case 1:
                                // ok, check errors.
                                netappResponse.IsSuccess = true;
                                break;

                            case 0:
                                // good.
                                netappResponse.IsSuccess = true;
                                break;

                            default:
                                // no-op
                                netappResponse.IsSuccess = false;
                                break;
                        }
                }
                // we'll only find this IF a) we're adding an instance AND b) it worked.
                else if (outputItem != null && (outputItem.BaseObject is string) && outputItem.BaseObject.ToString().Contains("Connection Data: "))
                {
                    var temp = outputItem.BaseObject.ToString();
                    // do some string manipulation and get the connection data back.
                    netappResponse.ConnectionData = temp.Substring(17);
                }

                netappResponse.ConnectionData = volume.Name;
            }
            return netappResponse;
        }
    }
}