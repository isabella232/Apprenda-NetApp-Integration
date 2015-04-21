using System;
using Apprenda.SaaSGrid.Addons;
using System.Collections.Generic;
using Apprenda.SaaSGrid.Addons.NetApp;
using Apprenda.SaaSGrid.Addons.NetApp.Annotations;

namespace NetAppBatchTests
{
    internal static class Program
    {
        // for now, create a base manifest
        private static readonly AddonManifest AddonManifest = new AddonManifest
        {
            Author = "Chris Dutra",
            ProvisioningUsername = "chris@dutronlabs.com",
            ProvisioningPassword = "cyrixm2r",
            ProvisioningLocation = "Apprenda-NYC",
            IsEnabled = true,
            Version = "2.0",
            Vendor = "Apprenda",
            Name = "NetApp",
            Properties = new List<AddonProperty>()
        };

        public static void Main([NotNull] string[] args)
        {
            AddonManifest.Properties.Add(new AddonProperty{Key="clustermgtendpoint", Value="10.5.4.150"});
            AddonManifest.Properties.Add(new AddonProperty{Key="shareendpoint", Value="10.5.4.152"});
            AddonManifest.Properties.Add(new AddonProperty { Key = "adminusername", Value = "admin" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "adminpassword", Value = "HQ@N3t@pp!" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "vserver", Value = "apprendaVServer" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultprotocol", Value = "CIFS" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultaggregate", Value = "aggr1" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "defaultRootPath", Value = "/vol" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapenable", Value = "true" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "vaultenable", Value = "true" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapvaultschedule", Value = "weekly" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapmirrorschedule", Value = "hourly" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapvaultpolicyname", Value = "default" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snapmirrorpolicyname", Value = "default" });
            AddonManifest.Properties.Add(new AddonProperty { Key = "snaptype", Value = "ls"});
            AddonManifest.Properties.Add(new AddonProperty { Key = "netappscriptrepo", Value="https://s3.amazonaws.com/apprenda.netapp.scripts/"});
            try
            {
                var addon = new NetAppAddon();
                var prequest = new AddonProvisionRequest
                {
                    DeveloperOptions = "name=netappdemoFullPathTest&size=20M",
                    Manifest = AddonManifest
                };
                var result = addon.Provision(prequest);
                Console.Out.Write(result.ConnectionData);

            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }
            
        }
    }
}