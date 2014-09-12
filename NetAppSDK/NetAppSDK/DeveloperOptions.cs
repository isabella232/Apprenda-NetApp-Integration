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

        // For Provisioning / Deprovisioning a NetApp Volume
        public Volume VolumeToProvision { get; set; } 

        // For Provisioning / Deprovisioning a NetApp Aggregate
        public Aggregate AggregateToProvision { get; set; }

        // these are deprecated, we'll be getting rid of these once V2 is fully ported.
        public String volumeType { get; set; }
        public String volumeName { get; set; }
        public String containingAggregateName { get; set; }
        public String size { get; set; }
        public long diskcount { get; set; }
        public long disksize { get; set; }

        // Method takes in a string and parses it into a DeveloperOptions class.
        public static DeveloperOptions Parse(string developerOptions)
        {
            DeveloperOptions options = new DeveloperOptions();

            if (!string.IsNullOrWhiteSpace(developerOptions))
            {
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
            }

            return options;
        }

        // Interior method takes in instance of DeveloperOptions (aptly named existingOptions) and maps them to the proper value. In essence, a setter method.
        private static void MapToOption(DeveloperOptions existingOptions, string key, string value)
        {
            // this is called only if the developer wishes to overwrite the platform operator's default aggregate
            if ("aggregatename".Equals(key))
            {
                existingOptions.containingAggregateName = value;
                return;
            }
            // this is called only if the developer requests a different size of storage
            if("size".Equals(key))
            {
                existingOptions.size = value;
                return;
            }
            // REQUIRED: developer must choose a name for the volume.
            if ("volumename".Equals(key))
            {
                existingOptions.volumeName = value;
                return;
            }
            
            throw new ArgumentException(string.Format("The developer option '{0}' was not expected and is not understood.", key));
        }












        // Deprecated in V2
        public void LoadItemsFromManifest(AddonManifest manifest)
        {
            var manifestProperties = manifest.GetProperties();
            foreach(var manifestProperty in manifestProperties)
            {
                switch(manifestProperty.Key)
                {
                    case("aggregateName"):
                        containingAggregateName = manifestProperty.Value;
                        break;
                    case("size"):
                        size = manifestProperty.Value;
                        break;
                    case("volumename"):
                        volumeName = manifestProperty.Value;
                        break;
                    default: // means there are other manifest properties we don't need.
                        break;
                }
            }
        }
        
    }
}