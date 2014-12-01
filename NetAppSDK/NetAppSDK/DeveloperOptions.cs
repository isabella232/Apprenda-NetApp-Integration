using Apprenda.SaaSGrid.Addons.NetApp.Annotations;
using Apprenda.SaaSGrid.Addons.NetApp.Models;
using System;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class DeveloperOptions
    {
        private DeveloperOptions(Volume volumeToProvision)
        {
            VolumeToProvision = volumeToProvision;
        }

        // these are used to connect to the NetApp Appliance
        public string VServer { get; private set; }

        public string ClusterMgtEndpoint { get; private set; }

        public string AdminUserName { get; private set; }

        // For Provisioning / Deprovisioning a NetApp Volume
        public Volume VolumeToProvision { get; private set; }

        public string AdminPassword { get; private set; }

        // handled on a policy-basis via the manifest
        private string LifName { [UsedImplicitly] get; set; }

        // Method takes in a string and parses it into a DeveloperOptions class.
        public static DeveloperOptions Parse(string developerOptions)
        {
            var options = new DeveloperOptions(new Volume());
            if (string.IsNullOrWhiteSpace(developerOptions)) return options;
            var optionPairs = developerOptions.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var optionPair in optionPairs)
            {
                var optionPairParts = optionPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (optionPairParts.Length == 2)
                {
                    MapToOption(options, optionPairParts[0].Trim().ToLowerInvariant(), optionPairParts[1].Trim());
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "Unable to parse developer options which should be in the form of 'option=value&nextOption=nextValue'. The option '{0}' was not properly constructed",
                            optionPair));
                }
            }
            // validate that we have the required parameters -- TODO
            return options;
        }

        // Interior method takes in instance of DeveloperOptions (aptly named existingOptions) and maps them to the proper value. In essence, a setter method.
        private static void MapToOption(DeveloperOptions requiredParams, string key, string value)
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
                if (!(Int32.TryParse(value, out tmp)))
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
                if (!(Boolean.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("JunctionActive must be a boolean type");
                }
                requiredParams.VolumeToProvision.JunctionActive = tmp;
                return;
            }

            if ("maxdirectorysize".Equals(key))
            {
                double tmp;
                if (!(Double.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.MaxDirectorySize = tmp;
                return;
            }

            if ("nvfailenabled".Equals(key))
            {
                bool tmp;
                if (!(Boolean.TryParse(value, out tmp)))
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
                Int32 tmp;
                if (!(Int32.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.UserId = tmp;
                return;
            }

            if ("vserverroot".Equals(key))
            {
                bool tmp;
                if (!(Boolean.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("NvFailEnabled must be a boolean type");
                }
                requiredParams.VolumeToProvision.VserverRoot = tmp;
                return;
            }

            if ("snapshotreserver".Equals(key))
            {
                Int32 tmp;
                if (!(Int32.TryParse(value, out tmp)))
                {
                    throw new ArgumentException("MaxDirectorySize must be a boolean type");
                }
                requiredParams.VolumeToProvision.SnapshotReserver = tmp;
                return;
            }

            if ("vmalignsector".Equals(key))
            {
                Int32 tmp;
                if (!(Int32.TryParse(value, out tmp)))
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

            throw new ArgumentException(string.Format("The developer option '{0}' was not expected and is not understood.", key));
        }

        public void LoadItemsFromManifest(AddonManifest manifest)
        {
            try
            {
                var manifestProperties = manifest.GetProperties();
                foreach (var manifestProperty in manifestProperties)
                {
                    //Console.WriteLine("Debug- manifestProperty Key: " + manifestProperty.DisplayName + " Value: " + manifestProperty.Value);
                    switch (manifestProperty.Key.Trim().ToLowerInvariant())
                    {
                        case ("vserver"):
                            VServer = manifestProperty.Value;
                            break;

                        case ("adminusername"):
                            AdminUserName = manifestProperty.Value;
                            break;

                        case ("adminpassword"):
                            AdminPassword = manifestProperty.Value;
                            break;

                        case ("clustermgtendpoint"):
                            ClusterMgtEndpoint = manifestProperty.Value;
                            break;

                        case ("defaultprotocol"):
                            VolumeToProvision.Protocol = manifestProperty.Value;
                            break;

                        case ("defaultlifname"):
                            LifName = manifestProperty.Value;
                            break;

                        case ("basejunctionpath"):
                            VolumeToProvision.JunctionPath = manifestProperty.Value;
                            break;

                        default: // means there are other manifest properties we don't need.
                            Console.WriteLine("Parse failed on key: " + manifestProperty.DisplayName);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n Debug information: " + manifest.GetProperties());
            }
        }
    }
}