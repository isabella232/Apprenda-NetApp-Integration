using System;
using Apprenda.SaaSGrid.Addons;
using System.Collections.Generic;
using Apprenda.SaaSGrid.Addons.NetApp;
using Apprenda.SaaSGrid.Addons.NetApp.Annotations;
using System.Configuration;

namespace NetAppBatchTests
{
    internal static class Program
    {
        // for now, create a base manifest
        private static readonly AddonManifest AddonManifest = new AddonManifest
        {
            Author = "Chris Dutra",
            ProvisioningUsername = "",
            ProvisioningPassword = "",
            ProvisioningLocation = "",
            IsEnabled = true,
            Version = "1.2",
            Vendor = "Apprenda",
            Name = "NetApp",
            Properties = new List<AddonProperty>()
        };

        public static void Main([NotNull] string[] args)
        {
            AddonManifest.Properties.Add(new AddonProperty{Key="clustermgtendpoint", Value=ConfigurationManager.AppSettings["ClusterMgtEndpoint"]});
            AddonManifest.Properties.Add(new AddonProperty { Key = "shareendpoint", Value = ConfigurationManager.AppSettings["ShareEndpoint"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "adminusername", Value = ConfigurationManager.AppSettings["AdminUsername"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "adminpassword", Value = ConfigurationManager.AppSettings["AdminPassword"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "vserver", Value = ConfigurationManager.AppSettings["VServer"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultprotocol", Value = ConfigurationManager.AppSettings["DefaultProtocol"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultaggregate", Value = ConfigurationManager.AppSettings["DefaultAggregate"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultRootPath", Value = ConfigurationManager.AppSettings["DefaultRootPath"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapenable", Value = ConfigurationManager.AppSettings["SnapEnable"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "vaultenable", Value = ConfigurationManager.AppSettings["VaultEnable"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapvaultschedule", Value = ConfigurationManager.AppSettings["SnapVaultSchedule"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapmirrorschedule", Value = ConfigurationManager.AppSettings["SnapMirrorSchedule"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapvaultpolicyname", Value = ConfigurationManager.AppSettings["SnapVaultPolicyName"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapmirrorpolicyname", Value = ConfigurationManager.AppSettings["SnapMirrorPolicyName"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snaptype", Value = ConfigurationManager.AppSettings["SnapType"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "maxallocatedstorage", Value = ConfigurationManager.AppSettings["MaxAllocatedStorage"] });
            AddonManifest.Properties.Add(new AddonProperty { Key = "scriptrepo", Value = ConfigurationManager.AppSettings["ScriptRepo"] });
            try
            {
                var addon = new NetAppAddon();
                var prequest = new AddonProvisionRequest
                {
                    DeveloperParameters = new List<AddonParameter>
                    {
                        new AddonParameter()
                        {
                            Key = "name",
                            Value = ConfigurationManager.AppSettings["VolumeName"]
                        }, 
                        new AddonParameter()
                        {
                            Key = "size",
                            Value = ConfigurationManager.AppSettings["Size"]
                        }
                    },
                    Manifest = AddonManifest
                };
                var result = addon.Provision(prequest);
                Console.Out.Write(result.IsSuccess);
                Console.Out.Write(result.EndUserMessage);
                Console.Out.Write(result.ConnectionData);

            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }
            
        }
    }
}