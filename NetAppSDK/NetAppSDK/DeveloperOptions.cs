using System;
using System.Collections.Generic;
using Apprenda.SaaSGrid.Addons;
using Apprenda.SaaSGrid.Addons.NetApp.V2.Models;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class DeveloperOptions
    {
        // i'm going to break this into sections, and then divy those into required and optional groups
        
        // General Options
        // Required
        public string ProvisioningType { get; set; }

        // these are used to connect to the NetApp Appliance
        public string VServer { get; set; }
        public string ClusterMgtEndpoint { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }

        // For Provisioning / Deprovisioning a NetApp Volume
        public Volume VolumeToProvision { get; set; } 

        // For Provisioning / Deprovisioning a NetApp Aggregate
        public Aggregate AggregateToProvision { get; set; }

        // these are deprecated, we'll be getting rid of these once V2 is fully ported.
        //public String volumeType { get; set; }
        //public String volumeName { get; set; }
        //public String containingAggregateName { get; set; }
        //public String size { get; set; }
        //public long diskcount { get; set; }
        //public long disksize { get; set; }

        // constructor for volume provisioning
        public DeveloperOptions(string provisioningType)
        {
            ProvisioningType = provisioningType;
            // construct volume to be passed with required args
            if (provisioningType.Equals(@"vol"))
            {
                VolumeToProvision = new Volume();
            }
            if (provisioningType.Equals(@"aggr"))
            {
                AggregateToProvision = new Aggregate();
            }
        }


        // Method takes in a string and parses it into a DeveloperOptions class.
        public static DeveloperOptions Parse(string developerOptions)
        {
            // ok, so some validation rules
            // 1) first argument must be provisioning type
            // 2) next batch of arguments must be required for that type
            // 3) then optional arguments can be passed (but must be recognized by netapp)

            // going to push the constructor back until we know we have the required parameters

            DeveloperOptions options = new DeveloperOptions("");
            if (!string.IsNullOrWhiteSpace(developerOptions))
            {
                int count = 0;
                var optionPairs = developerOptions.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var optionPair in optionPairs)
                {
                    var optionPairParts = optionPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if(count == 0)
                    {
                        if (optionPairParts.Length == 2)
                        {
                            string provisioningType = MapProvisioningType(optionPairParts[0].Trim().ToLowerInvariant(), optionPairParts[1].Trim());
                            if(provisioningType != null)
                            {
                                options = new DeveloperOptions(provisioningType);
                            }
                            else
                            {
                                throw new ArgumentException(
                                "Provisioning Type should be specified first. Accepted values are 'vol' and 'aggr'");
                            }
                        }
                        else
                        {
                            throw new ArgumentException(
                                string.Format(
                                    "Unable to parse developer options which should be in the form of 'option=value&nextOption=nextValue'. The option '{0}' was not properly constructed",
                                    optionPair));
                        }
                    }
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
            }

            // validate that we have the required parameters

            return options;
        }

        private static string MapProvisioningType(string p1, string p2)
        {
            if(p1.Equals(@"provisioningtype"))
            {
                if(p2.Equals("vol"))
                {
                    return p2;
                }
                if (p2.Equals("aggr"))
                {
                    return p2;
                }
                return null;
            }
            return null;
        }

        // Interior method takes in instance of DeveloperOptions (aptly named existingOptions) and maps them to the proper value. In essence, a setter method.
        private static void MapToOption(DeveloperOptions requiredParams, string key, string value)
        {
            // Provisioning Type will drive the parsing
            if (requiredParams.ProvisioningType.Equals("vol"))
            {
                // Begin Required Parameters.
                // this is called only if the developer wishes to overwrite the platform operator's default aggregate
                if ("Name".Equals(key))
                {
                    requiredParams.VolumeToProvision.Name = value;
                    return;
                }
                // this is called only if the developer requests a different size of storage
                if ("AggregateName".Equals(key))
                {
                    requiredParams.VolumeToProvision.AggregateName = value;
                    return;
                }
                // REQUIRED: developer must choose a name for the volume.
                if ("JunctionPath".Equals(key))
                {
                    requiredParams.VolumeToProvision.JunctionPath = value;
                    return;
                }

                // Begin Optional Parameters
                if ("Comment".Equals(key))
                {
                    requiredParams.VolumeToProvision.Comment = value;
                    return;
                }

                if ("AntiVirusOnAccessPolicy".Equals(key))
                {
                    requiredParams.VolumeToProvision.AntiVirusOnAccessPolicy = value;
                    return;
                }

                if ("ExportPolicy".Equals(key))
                {
                    requiredParams.VolumeToProvision.ExportPolicy = value;
                    return;
                }

                if ("FlexCacheCachePolicy".Equals(key))
                {
                    requiredParams.VolumeToProvision.FlexCacheCachePolicy = value;
                    return;
                }

                if ("FlexCacheFillPolicy".Equals(key))
                {
                    requiredParams.VolumeToProvision.FlexCacheFillPolicy = value;
                    return;
                }

                if ("FlexCacheOriginVolume".Equals(key))
                {
                    requiredParams.VolumeToProvision.FlexCacheOriginVolume = value;
                    return;
                }

                if ("GroupId".Equals(key))
                {
                    int tmp;
                    if(!(Int32.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("GroupId must be an integer value");
                    }
                    requiredParams.VolumeToProvision.GroupId = tmp;
                    return;
                }

                if ("IndexDirectoryFormat".Equals(key))
                {
                    requiredParams.VolumeToProvision.IndexDirectoryFormat = value;
                    return;
                }

                if("JunctionActive".Equals(key))
                {
                    bool tmp;
                    if(!(Boolean.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("JunctionActive must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.JunctionActive = tmp;
                    return;
                }

                if ("MaxDirectorySize".Equals(key))
                {
                    double tmp;
                    if (!(Double.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("MaxDirectorySize must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.MaxDirectorySize = tmp;
                    return;
                }

                if ("NvFailEnabled".Equals(key))
                {
                    bool tmp;
                    if (!(Boolean.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("NvFailEnabled must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.NvFailEnabled = tmp;
                    return;
                }

                if ("SecurityStyle".Equals(key))
                {
                    requiredParams.VolumeToProvision.SecurityStyle = value;
                    return;
                }

                if ("SnapshotPolicy".Equals(key))
                {
                    requiredParams.VolumeToProvision.SnapshotPolicy = value;
                    return;
                }

                if ("SpaceReserver".Equals(key))
                {
                    requiredParams.VolumeToProvision.SpaceReserver = value;
                    return;
                }

                if ("State".Equals(key))
                {
                    requiredParams.VolumeToProvision.State = value;
                    return;
                }

                if ("Type".Equals(key))
                {
                    requiredParams.VolumeToProvision.Type = value;
                    return;
                }

                if ("UnixPermissions".Equals(key))
                {
                    requiredParams.VolumeToProvision.UnixPermissions = value;
                    return;
                }

                if ("UserId".Equals(key))
                {
                    Int32 tmp;
                    if (!(Int32.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("MaxDirectorySize must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.UserId = tmp;
                    return;
                }

                if ("VserverRoot".Equals(key))
                {
                    bool tmp;
                    if (!(Boolean.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("NvFailEnabled must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.VserverRoot = tmp;
                    return;
                }

                if ("SnapshotReserver".Equals(key))
                {
                    Int32 tmp;
                    if (!(Int32.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("MaxDirectorySize must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.SnapshotReserver = tmp;
                    return;
                }

                if ("VmAlignSector".Equals(key))
                {
                    Int32 tmp;
                    if (!(Int32.TryParse(value, out tmp)))
                    {
                        throw new ArgumentException("NvFailEnabled must be a boolean type");
                    }
                    requiredParams.VolumeToProvision.VmAlignSector = tmp;
                    return;
                }

                if ("VmAlignSuffic".Equals(key))
                {
                    requiredParams.VolumeToProvision.VmAlignSuffic = value;
                    return;
                }

                if ("QosPolicyGroup".Equals(key))
                {
                    requiredParams.VolumeToProvision.QosPolicyGroup = value;
                    return;
                }

                if ("Language".Equals(key))
                {
                    requiredParams.VolumeToProvision.Language = value;
                    return;
                }

                throw new ArgumentException(string.Format("The developer option '{0}' was not expected and is not understood.", key));
            }
            if(requiredParams.ProvisioningType.Equals("aggr"))
            {
                // we'll add aggregate parsing here.
            }
            throw new ArgumentException("Internal Error, provisioning type was invalid.");
        }

        public void LoadItemsFromManifest(AddonManifest manifest)
        {
            var manifestProperties = manifest.GetProperties();
            foreach(var manifestProperty in manifestProperties)
            {
                switch(manifestProperty.Key)
                {
                    case("VServer"):
                        VServer = manifestProperty.Value;
                        break;
                    case("AdminUserName"):
                        AdminUserName = manifestProperty.Value;
                        break;
                    case("AdminPassword"):
                        AdminPassword = manifestProperty.Value;
                        break;
                    case("ClusterMgtEndpoint"):
                        ClusterMgtEndpoint = manifestProperty.Value;
                        break;
                    default: // means there are other manifest properties we don't need.
                        break;
                }
            }
        }
        
    }
}