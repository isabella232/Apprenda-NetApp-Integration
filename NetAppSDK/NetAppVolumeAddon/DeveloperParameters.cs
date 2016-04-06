using System;
using System.Collections.Generic;
using Apprenda.SaaSGrid.Addons.NetApp.Models;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class DeveloperParameters
    {
        private DeveloperParameters(Volume volumeToProvision)
        {
            VolumeToProvision = volumeToProvision;
        }

        // type of script repository - local, server, cloudstorage
        public string ScriptRepositoryType { get; private set; }
        // modularizing the script repository
        public string ScriptRepository { get; private set; }
        // these are used to connect to the NetApp Appliance
        public string VServer { get; private set; }
        public string ClusterMgtEndpoint { get; private set; }
        public string AdminUserName { get; private set; }
        // For Provisioning / Deprovisioning a NetApp Volume
        public Volume VolumeToProvision { get; private set; }
        public string AdminPassword { get; private set; }
        public string MaxAllocatedStorage { get; set; }
        // handled on a policy-basis via the manifest

        // Method takes in a string and parses it into a DeveloperOptions class.
        public static DeveloperParameters Parse(IEnumerable<AddonParameter> developerParameters, AddonManifest manifest)
        {
            var dParams = new DeveloperParameters(new Volume());
            foreach (var param in developerParameters)
            {
                MapToOption(dParams, param.Key.Trim().ToLowerInvariant(), param.Value.Trim());
            }
            dParams = LoadItemsFromManifest(dParams, manifest);
            return dParams;
        }

        // Interior method takes in instance of DeveloperOptions (aptly named existingOptions) and maps them to the proper value. In essence, a setter method.
        private static void MapToOption(DeveloperParameters requiredParams, string key, string value)
        {
            // Begin Required Parameters.
            // this is called only if the developer wishes to overwrite the platform operator's default aggregate
            if ("name".Equals(key))
            {
                requiredParams.VolumeToProvision.Name = value;
                return;
            }
            if ("aggregatename".Equals(key))
            {
                requiredParams.VolumeToProvision.AggregateName = value;
                return;
            }
            // with version 1.2, we've removed the need to specify a junction path, that's now taken care of based on policy.
            // this is advantageous since we've abstracted another level of complexity away from the dev.
            if ("size".Equals(key))
            {
                requiredParams.VolumeToProvision.Size = value;
                return;
            }

            // Begin Optional Parameters
            if ("comment".Equals(key))
            {
                requiredParams.VolumeToProvision.Comment = value;
                return;
            }

            if ("antivirusonaccesspolicy".Equals(key))
            {
                requiredParams.VolumeToProvision.AntiVirusOnAccessPolicy = value;
                return;
            }

            if ("exportpolicy".Equals(key))
            {
                requiredParams.VolumeToProvision.ExportPolicy = value;
                return;
            }

            if ("flexcachecachepolicy".Equals(key))
            {
                requiredParams.VolumeToProvision.FlexCacheCachePolicy = value;
                return;
            }

            if ("flexcachefillpolicy".Equals(key))
            {
                requiredParams.VolumeToProvision.FlexCacheFillPolicy = value;
                return;
            }

            if ("flexcacheoriginvolume".Equals(key))
            {
                requiredParams.VolumeToProvision.FlexCacheOriginVolume = value;
                return;
            }

            if ("groupid".Equals(key))
            {
                int tmp;
                if (!(int.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("GroupId must be an integer value");
                }
                requiredParams.VolumeToProvision.GroupId = tmp;
                return;
            }

            if ("indexdirectoryformat".Equals(key))
            {
                requiredParams.VolumeToProvision.IndexDirectoryFormat = value;
                return;
            }

            if ("junctionactive".Equals(key))
            {
                bool tmp;
                if (!(bool.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("JunctionActive must be a boolean type");
                }
                requiredParams.VolumeToProvision.JunctionActive = tmp;
                return;
            }

            if ("maxdirectorysize".Equals(key))
            {
                double tmp;
                if (!(double.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.MaxDirectorySize = tmp;
                return;
            }

            if ("nvfailenabled".Equals(key))
            {
                bool tmp;
                if (!(bool.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("NvFailEnabled must be a boolean type");
                }
                requiredParams.VolumeToProvision.NvFailEnabled = tmp;
                return;
            }

            if ("securitystyle".Equals(key))
            {
                requiredParams.VolumeToProvision.SecurityStyle = value;
                return;
            }

            if ("snapshotpolicy".Equals(key))
            {
                requiredParams.VolumeToProvision.SnapshotPolicy = value;
                return;
            }

            if ("spacereserver".Equals(key))
            {
                requiredParams.VolumeToProvision.SpaceReserver = value;
                return;
            }

            if ("state".Equals(key))
            {
                requiredParams.VolumeToProvision.State = value;
                return;
            }

            if ("type".Equals(key))
            {
                requiredParams.VolumeToProvision.Type = value;
                return;
            }

            if ("unixpermissions".Equals(key))
            {
                requiredParams.VolumeToProvision.UnixPermissions = value;
                return;
            }

            if ("userid".Equals(key))
            {
                int tmp;
                if (!(int.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.UserId = tmp;
                return;
            }

            if ("vserverroot".Equals(key))
            {
                bool tmp;
                if (!(bool.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("NvFailEnabled must be a boolean type");
                }
                requiredParams.VolumeToProvision.VserverRoot = tmp;
                return;
            }

            if ("snapshotreserver".Equals(key))
            {
                int tmp;
                if (!(int.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.SnapshotReserver = tmp;
                return;
            }

            if ("vmalignsector".Equals(key))
            {
                int tmp;
                if (!(int.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("NvFailEnabled must be a boolean type");
                }
                requiredParams.VolumeToProvision.VmAlignSector = tmp;
                return;
            }

            if ("vmalignsuffic".Equals(key))
            {
                requiredParams.VolumeToProvision.VmAlignSuffic = value;
                return;
            }

            if ("qospolicygroup".Equals(key))
            {
                requiredParams.VolumeToProvision.QosPolicyGroup = value;
                return;
            }
            // for multi-national support
            if ("language".Equals(key))
            {
                requiredParams.VolumeToProvision.Language = value;
                return;
            }
            // only if there are both nfs and cifs shares should this ever be used.
            if ("protocol".Equals(key))
            {
                requiredParams.VolumeToProvision.Protocol = value;
            }

            throw new ArgumentException(
                string.Format("The developer option '{0}' was not expected and is not understood.", key));
        }

        public static DeveloperParameters LoadItemsFromManifest(DeveloperParameters parameters, AddonManifest manifest)
        {
            try
            {
                var manifestProperties = manifest.GetProperties();
                foreach (var manifestProperty in manifestProperties)
                {
                    switch (manifestProperty.Key.Trim().ToLowerInvariant())
                    {
                        case ("vserver"):
                            parameters.VServer = manifestProperty.Value;
                            break;

                        case ("adminusername"):
                            parameters.AdminUserName = manifestProperty.Value;
                            break;

                        case ("adminpassword"):
                            parameters.AdminPassword = manifestProperty.Value;
                            break;

                        case ("clustermgtendpoint"):
                            parameters.ClusterMgtEndpoint = manifestProperty.Value;
                            break;

                        case ("defaultprotocol"):
                            parameters.VolumeToProvision.Protocol = manifestProperty.Value;
                            break;

                        case ("defaultaggregate"):
                            parameters.VolumeToProvision.AggregateName = manifestProperty.Value;
                            break;

                        case ("defaultrootpath"):
                            parameters.VolumeToProvision.JunctionPath = manifestProperty.Value;
                            break;

                        case ("snapenable"):
                            bool snaptest;
                            bool.TryParse(manifestProperty.Value, out snaptest);
                            parameters.VolumeToProvision.SnapEnable = snaptest;
                            break;

                        case ("vaultenable"):
                            bool test;
                            bool.TryParse(manifestProperty.Value, out test);
                            parameters.VolumeToProvision.VaultEnable = test;
                            break;

                        case ("snapshotschedule"):
                            parameters.VolumeToProvision.SnapshotSchedule = manifestProperty.Value;
                            break;

                        case ("defaultacl"):
                            break;

                        case ("snapmirrorpolicyname"):
                            parameters.VolumeToProvision.SnapMirrorPolicyName = manifestProperty.Value;
                            break;

                        case ("snapvaultpolicyname"):
                            parameters.VolumeToProvision.SnapVaultPolicyName = manifestProperty.Value;
                            break;

                        case ("snapmirrorschedule"):
                            parameters.VolumeToProvision.SnapMirrorSchedule = manifestProperty.Value;
                            break;

                        case ("snapvaultschedule"):
                            parameters.VolumeToProvision.SnapVaultSchedule = manifestProperty.Value;
                            break;

                        case ("snaptype"):
                            parameters.VolumeToProvision.SnapType = manifestProperty.Value;
                            break;

                        case ("shareendpoint"):
                            parameters.VolumeToProvision.CifsRootServer = manifestProperty.Value;
                            break;

                        case ("scriptrepotype"):
                            parameters.ScriptRepositoryType = manifestProperty.Value;
                            break;

                        case ("scriptrepo"):
                            parameters.ScriptRepository = manifestProperty.Value;
                            break;

                        case ("maxallocatedstorage"):
                            parameters.MaxAllocatedStorage = manifestProperty.Value;
                            break;

                        default: // means there are other manifest properties we don't need.
                            Console.WriteLine("Parse failed on key: " + manifestProperty.Key);
                            break;
                    }
                }
                return parameters;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n Debug information: " + manifest.GetProperties());
            }
        }
    }
}