// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetAppFactory.cs" company="">
//   MIT License
// </copyright>
// <summary>
//   Defines the NetAppFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using Apprenda.SaaSGrid.Addons.NetApp.Annotations;
    using Apprenda.SaaSGrid.Addons.NetApp.Models;

    /// <summary>
    /// The net app factory.
    /// </summary>
    public static class NetAppFactory
    {
        /// <summary>
        /// The create volume.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <returns>
        /// The <see cref="NetAppResponse"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [NotNull]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public static NetAppResponse CreateVolume([NotNull] DeveloperParameters d)
        {
            // need to perform validation checks
            if (!CheckIfStorageRequestIsCompliant(d))
            {
                return new NetAppResponse
                {
                    ConnectionData = string.Empty,
                    IsSuccess = false,
                    ErrorOut =
                        "Your request does not meet the compliance requirements set forth by your platform administrator. Please check your configuration and try again."
                };
            }
            try
            {
                using (var psInstance = PowerShell.Create())
                {
                    // STEP 1: first things first. we need to set the execution policy for this script to be unrestricted. because we're assuming not everyone signs their powershell scripts.
                    psInstance.AddCommand("Set-ExecutionPolicy");
                    psInstance.AddArgument("Unrestricted");
                    psInstance.AddArgument("Process");
                    psInstance.AddParameter("-Force");
                    psInstance.Invoke();
                    var debugStream = string.Empty;
                    debugStream += psInstance.Streams.Debug.Aggregate(debugStream, (current, debug) => current + debug);
                    debugStream += psInstance.Streams.Progress.Aggregate(
                        debugStream,
                        (current, debug) => current + debug);
                    psInstance.Commands.Clear();

                    // -------------------------------------------------------------------------------------------/
                    // STEP 2: We need to copy the file locally. Now - for all intensive purposes, we're going to assume the file is in 1 of three places
                    //  - Locally on the platform (in which an absolute path is provided)
                    //  - Accessible server within the datacenter (in which a server name and path are provided)
                    //  - Cloud Storage (in which an HTTPS link is provided)
                    //
                    //   Anything else is custom or Phase II at this time.
                    // -------------------------------------------------------------------------------------------/
                    const string ExecutingPs1File = ".\\CreateVolume.ps1";

                    if (!File.Exists(ExecutingPs1File))
                    {
                        File.WriteAllText(ExecutingPs1File, Constants.CreateVolume);
                    }
                    
                    // this snipped it for server-based local storage. we need to write some additional routines for this.
                    /*
                    var remotePs1File = d.ScriptRepository + "\\CreateVolume.ps1";
                    psInstance.AddCommand("Copy-Item");
                    psInstance.AddParameter("-Path", remotePs1File);
                    psInstance.AddParameter("-Destination", executingPs1File);
                    psInstance.AddParameter("-Force");
                    psInstance.Invoke();
                    debugStream += psInstance.Streams.Debug.Aggregate(debugStream, (current, debug) => current + debug);
                    debugStream += psInstance.Streams.Progress.Aggregate(debugStream,
                        (current, debug) => current + debug);
                    if (psInstance.Streams.Error.Count > 0)
                    {
                        errorStream += psInstance.Streams.Error.Aggregate(errorStream,
                            (current, error) => current + error);
                        return new NetAppResponse
                        {
                            IsSuccess = false,
                            ErrorOut = errorStream,
                            ConnectionData = "",
                            ConsoleOut = "Could not copy file to execute. Please check connectivity to the server.",
                            ReturnCode = -1
                        };
                    }
                    */

                    // -------------------------------------------------------------------------------------------/
                    // STEP 3: Execute the script!
                    // -------------------------------------------------------------------------------------------/
                    psInstance.AddCommand(ExecutingPs1File);
                    psInstance.AddParameter("-username", d.AdminUserName);
                    psInstance.AddParameter("-password", d.AdminPassword);
                    psInstance.AddParameter("-vserver", d.VServer);
                    psInstance.AddParameter("-endpoint", d.ClusterMgtEndpoint);

                    // add in additional parameters. i tried to make it easier. but PowerShell object is readonly :(
                    // Required Parameters            
                    psInstance.AddParameter("-aggregateName", d.VolumeToProvision.AggregateName);
                    psInstance.AddParameter("-junctionPath", d.VolumeToProvision.JunctionPath);
                    psInstance.AddParameter("-Size", d.VolumeToProvision.Size);
                    psInstance.AddParameter("-volName", d.VolumeToProvision.Name);

                    // and a whole SLEW of optional parameters
                    if (d.VolumeToProvision.Comment != null)
                    {
                        psInstance.AddParameter("-Comment", d.VolumeToProvision.Comment);
                    }

                    if (d.VolumeToProvision.AntiVirusOnAccessPolicy != null)
                    {
                        psInstance.AddParameter("-AntiVirusOnAccessPolicy", d.VolumeToProvision.AntiVirusOnAccessPolicy);
                    }

                    if (d.VolumeToProvision.ExportPolicy != null)
                    {
                        psInstance.AddParameter("-ExportPolicy", d.VolumeToProvision.ExportPolicy);
                    }

                    if (d.VolumeToProvision.FlexCacheCachePolicy != null)
                    {
                        psInstance.AddParameter("-FlexCacheCachePolicy", d.VolumeToProvision.FlexCacheCachePolicy);
                    }

                    if (d.VolumeToProvision.FlexCacheFillPolicy != null)
                    {
                        psInstance.AddParameter("-FlexCacheFillPolicy", d.VolumeToProvision.FlexCacheFillPolicy);
                    }

                    if (d.VolumeToProvision.FlexCacheOriginVolume != null)
                    {
                        psInstance.AddParameter("-FlexCacheOriginVolume", d.VolumeToProvision.FlexCacheOriginVolume);
                    }

                    if (!(-1).Equals(d.VolumeToProvision.GroupId))
                    {
                        psInstance.AddParameter(
                            "-GroupId",
                            d.VolumeToProvision.GroupId.ToString(CultureInfo.InvariantCulture));
                    }

                    if (d.VolumeToProvision.IndexDirectoryFormat != null)
                    {
                        psInstance.AddParameter("-IndexDirectoryFormat", d.VolumeToProvision.IndexDirectoryFormat);
                    }

                    if (d.VolumeToProvision.JunctionActive)
                    {
                        psInstance.AddParameter("-JunctionActive", "$true");
                    }

                    if (!(Math.Abs(d.VolumeToProvision.MaxDirectorySize - (-1)) < 0))
                    {
                        psInstance.AddParameter(
                            "-MaxDirectorySize",
                            d.VolumeToProvision.MaxDirectorySize.ToString(CultureInfo.InvariantCulture));
                    }

                    if (!d.VolumeToProvision.NvFailEnabled == false)
                    {
                        psInstance.AddParameter("-NvFailEnabled", d.VolumeToProvision.NvFailEnabled.ToString());
                    }

                    if (d.VolumeToProvision.SecurityStyle != null)
                    {
                        psInstance.AddParameter("-SecurityStyle", d.VolumeToProvision.SecurityStyle);
                    }

                    if (d.VolumeToProvision.SnapshotPolicy != null)
                    {
                        psInstance.AddParameter("-SnapshotPolicy", d.VolumeToProvision.SnapshotPolicy);
                    }

                    if (d.VolumeToProvision.SpaceReserver != null)
                    {
                        psInstance.AddParameter(
                            "-SpaceReserver",
                            d.VolumeToProvision.SnapshotReserver.ToString(CultureInfo.InvariantCulture));
                    }

                    if (d.VolumeToProvision.State != null)
                    {
                        psInstance.AddParameter("-State", d.VolumeToProvision.State);
                    }

                    if (d.VolumeToProvision.Type != null)
                    {
                        psInstance.AddParameter("-Type", d.VolumeToProvision.Type);
                    }

                    if (d.VolumeToProvision.UserId != -1)
                    {
                        psInstance.AddParameter(
                            "-UserId",
                            d.VolumeToProvision.UserId.ToString(CultureInfo.InvariantCulture));
                    }

                    if (d.VolumeToProvision.VserverRoot)
                    {
                        psInstance.AddParameter("-VserverRoot", "$true");
                    }

                    if (d.VolumeToProvision.SnapshotReserver != -1)
                    {
                        psInstance.AddParameter(
                            "-SnapshotReserver",
                            d.VolumeToProvision.SnapshotReserver.ToString(CultureInfo.InvariantCulture));
                    }

                    if (d.VolumeToProvision.VmAlignSector != -1)
                    {
                        psInstance.AddParameter(
                            "-VmAlignSector",
                            d.VolumeToProvision.VmAlignSector.ToString(CultureInfo.InvariantCulture));
                    }

                    if (d.VolumeToProvision.VmAlignSuffic != null)
                    {
                        psInstance.AddParameter("-VmAlignSuffic", d.VolumeToProvision.VmAlignSuffic);
                    }

                    if (d.VolumeToProvision.QosPolicyGroup != null)
                    {
                        psInstance.AddParameter("-QosPolicyGroup", d.VolumeToProvision.QosPolicyGroup);
                    }

                    if (d.VolumeToProvision.Language != null)
                    {
                        psInstance.AddParameter("-Language", d.VolumeToProvision.Language);
                    }

                    if (d.VolumeToProvision.Protocol != null)
                    {
                        psInstance.AddParameter("-Protocol", d.VolumeToProvision.Protocol);
                    }

                    if (d.VolumeToProvision.UnixPermissions != null)
                    {
                        psInstance.AddParameter("-UnixPermissions", d.VolumeToProvision.UnixPermissions);
                    }

                    if (d.VolumeToProvision.SnapEnable)
                    {
                        psInstance.AddParameter("-EnableSnapMirror", d.VolumeToProvision.SnapEnable);
                    }

                    if (d.VolumeToProvision.SnapMirrorPolicyName != null)
                    {
                        psInstance.AddParameter("-snapmirrorpolicyname", d.VolumeToProvision.SnapMirrorPolicyName);
                    }

                    if (d.VolumeToProvision.SnapVaultPolicyName != null)
                    {
                        psInstance.AddParameter("-snapvaultpolicyname", d.VolumeToProvision.SnapVaultPolicyName);
                    }

                    if (d.VolumeToProvision.SnapMirrorSchedule != null)
                    {
                        psInstance.AddParameter("-snapmirrorschedule", d.VolumeToProvision.SnapMirrorSchedule);
                    }

                    if (d.VolumeToProvision.SnapVaultSchedule != null)
                    {
                        psInstance.AddParameter("-snapvaultschedule", d.VolumeToProvision.SnapVaultSchedule);
                    }

                    if (d.VolumeToProvision.SnapType != null)
                    {
                        psInstance.AddParameter("-snaptype", d.VolumeToProvision.SnapType);
                    }

                    var output = psInstance.Invoke();
                    var errorStream = string.Empty;

                    if (psInstance.Streams.Error.Count <= 0)
                    {
                        return new NetAppResponse
                        {
                            ConnectionData = d.VolumeToProvision.Name,
                            IsSuccess = true,
                            ReturnCode = 0,
                            ConsoleOut = output.ToString()
                        };   
                    }

                    errorStream += psInstance.Streams.Error.Aggregate(errorStream, (current, error) => current + error);
                    debugStream += psInstance.Streams.Progress.Aggregate(debugStream, (current, debug) => current + debug);
                    return new NetAppResponse
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
                throw new Exception(e.ToString(), e);
            }
        }

        // Presumption - developers have knowledge regarding order of magnitude. this doesn't cover inappropriate requests such as 5000MB, 25000GB, etc.
        /// <summary>
        /// The check if storage request is compliant.
        /// </summary>
        /// <param name="developerParameters">
        /// The developer parameters.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool CheckIfStorageRequestIsCompliant(DeveloperParameters developerParameters)
        {
            var mAs = developerParameters.MaxAllocatedStorage;
            var requestedStorage = developerParameters.VolumeToProvision.Size;
            SizeMagnitude mAsMagnitude, requestedMagnitude;
            // testing that the parse works to enum, and that the order of magnitude on the limit is less than the order of magnitude on the size requested
            if (!Enum.TryParse(mAs.Last().ToString(), false, out mAsMagnitude) ||
                !Enum.TryParse(requestedStorage.Last().ToString(), false, out requestedMagnitude) ||
                (requestedMagnitude > mAsMagnitude)) return false;
            // ok, need to put a check in here to make sure mAsMagnitude > requestedMagnitude, return true
            if (mAsMagnitude > requestedMagnitude) return true;
            int mASnumber, requestedNumber;
            return (int.TryParse(mAs.Substring(0, mAs.Length - 1), out mASnumber)) &&
                   (int.TryParse(requestedStorage.Substring(0, requestedStorage.Length - 1), out requestedNumber)) &&
                   requestedNumber < mASnumber;
        }

        // This will delete a volume off of a given filer.
        public static NetAppResponse DeleteVolume(DeveloperParameters d, string connectionData)
        {
            using (var psInstance = PowerShell.Create())
            {
                // STEP 1: first things first. we need to set the execution policy for this script to be unrestricted. because we're assuming not everyone signs their powershell scripts.
                psInstance.AddCommand("Set-ExecutionPolicy");
                psInstance.AddArgument("Unrestricted");
                psInstance.AddArgument("Process");
                psInstance.AddParameter("-Force");
                psInstance.Invoke();
                var debugStream = string.Empty;
                var errorStream = string.Empty;
                debugStream += psInstance.Streams.Debug.Aggregate(debugStream, (current, debug) => current + debug);
                debugStream += psInstance.Streams.Progress.Aggregate(debugStream,
                    (current, debug) => current + debug);
                psInstance.Commands.Clear();

                // -------------------------------------------------------------------------------------------/
                // STEP 2: We need to copy the file locally. Now - for all intensive purposes, we're going to assume the file is in 1 of three places
                //  - Locally on the platform (in which an absolute path is provided)
                //  - Accessible server within the datacenter (in which a server name and path are provided)
                //  - Cloud Storage (in which an HTTPS link is provided)
                //
                //   Anything else is custom or Phase II at this time.
                // -------------------------------------------------------------------------------------------/

                // case 1, 2: local or server
                const string executingPs1File = ".\\DeleteVolume.ps1";
                var remotePs1File = d.ScriptRepository + "\\DeleteVolume.ps1";
                psInstance.AddCommand("Copy-Item");
                psInstance.AddParameter("-Path", remotePs1File);
                psInstance.AddParameter("-Destination", executingPs1File);
                psInstance.AddParameter("-Force");
                psInstance.Invoke();
                debugStream += psInstance.Streams.Debug.Aggregate(debugStream, (current, debug) => current + debug);
                debugStream += psInstance.Streams.Progress.Aggregate(debugStream,
                    (current, debug) => current + debug);
                if (psInstance.Streams.Error.Count > 0)
                {
                    errorStream += psInstance.Streams.Error.Aggregate(errorStream, (current, error) => current + error);
                    return new NetAppResponse
                    {
                        IsSuccess = false,
                        ErrorOut = errorStream,
                        ConnectionData = string.Empty,
                        ConsoleOut = "Could not copy file to execute. Please check connectivity to the server.",
                        ReturnCode = -1
                    };
                }

                // Step 3 - let's remove the volume.
                // So normally the only change here from our existing provisioning process is that we need to pull the volume name from the
                // connection string. so, let's extract it.

                var volumeToDelete = ExtractFromConnectionString(connectionData);
                psInstance.AddCommand(executingPs1File);
                psInstance.AddParameter("-username", d.AdminUserName);
                psInstance.AddParameter("-password", d.AdminPassword);
                psInstance.AddParameter("-vserver", d.VServer);
                psInstance.AddParameter("-endpoint", d.ClusterMgtEndpoint);
                psInstance.AddParameter("-volName", volumeToDelete);

                psInstance.Invoke();
                debugStream += psInstance.Streams.Debug.Aggregate(debugStream, (current, debug) => current + debug);
                debugStream += psInstance.Streams.Progress.Aggregate(debugStream,
                    (current, debug) => current + debug);
                if (psInstance.Streams.Error.Count <= 0)
                    return new NetAppResponse
                    {
                        IsSuccess = true,
                        ErrorOut = string.Empty,
                        ConnectionData = string.Empty,
                        ConsoleOut = debugStream
                    };
                errorStream += psInstance.Streams.Error.Aggregate(errorStream, (current, error) => current + error);
                return new NetAppResponse
                {
                    IsSuccess = false,
                    ErrorOut = errorStream,
                    ConnectionData = string.Empty,
                    ConsoleOut = "Could not copy file to execute. Please check connectivity to the server.",
                    ReturnCode = -1
                };
            }
        }

        private static string ExtractFromConnectionString(string connectionData)
        {
            return connectionData.Substring(connectionData.LastIndexOf("\\", StringComparison.Ordinal));
        }

        // we'll use this to compare size operators
        private enum SizeMagnitude
        {
            [UsedImplicitly] M,
            [UsedImplicitly] G,
            [UsedImplicitly] T,
            [UsedImplicitly] P
        }
    }
}