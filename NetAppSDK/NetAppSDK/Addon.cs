using System;
using System.Collections.Generic;
using System.Linq;
using Apprenda.SaaSGrid.Addons;
using System.Threading;
// deprecated
using Apprenda.SaaSGrid.Addons.NetApp.V1;
using Apprenda.SaaSGrid.Addons.NetApp.V1.Models;
// we'll be using this once ready.
using Apprenda.SaaSGrid.Addons.NetApp.V2;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class Addon : AddonBase
    {
        // Deprovision NetApp (volume, aggregate)
        // Input: AddonDeprovisionRequest request
        // Output: OperationResult

        // we really do not need the connection data to deprovision a volume or an aggregate
        public override OperationResult Deprovision(AddonDeprovisionRequest request)
        {
            var deprovisionResult = new ProvisionAddOnResult("");
            AddonManifest manifest = request.Manifest;
            string devOptions = request.DeveloperOptions;

            try
            {
                NetAppModel.LoadInfoFromManifest(request.Manifest);
                DeveloperOptions options = DeveloperOptions.Parse(devOptions);
                // run deprovision here
                VolumeOptions vo = new VolumeOptions()
                {
                    volumeName = options.volumeName
                };
                VolumeCommands.RemoveVolume(vo);
                deprovisionResult.EndUserMessage = "Volume taken offline and removed.";
            }
            catch (Exception e)
            {
                deprovisionResult.EndUserMessage = e.Message;
            }
            return deprovisionResult;
        }

        // Provision NetApp Volume
        // Input: AddonDeprovisionRequest request
        // Output: ProvisionAddOnResult
        public override ProvisionAddOnResult Provision(AddonProvisionRequest request)
        {
            var provisionResult = new ProvisionAddOnResult("");
            AddonManifest manifest = request.Manifest;
            try
            {

                NetAppModel.LoadInfoFromManifest(request.Manifest);
                // this loads in the developer options and the manifest parameters
                // validation will also occur here, so if this fails it will be caught prior to any invocation on the cluster.
                DeveloperOptions developerOptions = DeveloperOptions.Parse(request.DeveloperOptions);
                developerOptions.LoadItemsFromManifest(request.Manifest);
                // for assumptions now, create a volume
                var powershellOutput = NetAppFactory.GetInstance().CreateVolume(developerOptions.VolumeToProvision);
                provisionResult.IsSuccess = true;
                provisionResult.EndUserMessage = "Volume is provisioned.";
                provisionResult.ConnectionData = NetAppFactory.GetInstance().GetVolumeInfo(developerOptions.VolumeToProvision.Name);
            }
            catch (Exception e)
            {
                provisionResult.EndUserMessage = e.Message;
            }
            
            return provisionResult;
        }

        // Testing Instance
        // Input: AddonTestRequest request
        // Output: OperationResult
        public override OperationResult Test(AddonTestRequest request)
        {
            AddonManifest manifest = request.Manifest;
            string developerOptions = request.DeveloperOptions;
            var testResult = new OperationResult { IsSuccess = false };
            var testProgress = "";

            if (manifest.Properties != null && manifest.Properties.Any())
            {
                DeveloperOptions devOptions;

                testProgress += "Evaluating required manifest properties...\n";
                if (!ValidateManifest(manifest, out testResult))
                {
                    return testResult;
                }

                var parseOptionsResult = ParseDevOptions(developerOptions, out devOptions);
                if (!parseOptionsResult.IsSuccess)
                {
                    return parseOptionsResult;
                }
                testProgress += parseOptionsResult.EndUserMessage;
                try
                {
                    // load critical info from manifest to connect to netapp filer
                    NetAppModel.LoadInfoFromManifest(request.Manifest);
                    // run tests here 
                    // Test #1 : test api version
                    var checkAPIResult = GetSystemOntapiVersionCommand.ProcessRecord();
                    if(!checkAPIResult.IsSuccess)
                    {
                        return checkAPIResult;
                    }
                    testProgress += checkAPIResult.EndUserMessage;

                    AddonProvisionRequest apr = new AddonProvisionRequest()
                    {
                        DeveloperOptions = request.DeveloperOptions,
                        Manifest = request.Manifest
                    };

                    AddonDeprovisionRequest dpr = new AddonDeprovisionRequest()
                    {
                        DeveloperOptions = request.DeveloperOptions,
                        Manifest = request.Manifest
                    };
                    var provisionTest = Provision(apr);
                    if(!provisionTest.IsSuccess)
                    {
                        return provisionTest;
                    }
                    testProgress += provisionTest.EndUserMessage;
                    var deprovisionTest = Deprovision(dpr);
                    if(!deprovisionTest.IsSuccess)
                    {
                        return deprovisionTest;
                    }
                    testProgress += deprovisionTest.EndUserMessage;

                    // if we are good, bring back a success!

                    testResult.IsSuccess = true;
                    testResult.EndUserMessage = testProgress;
                }
                catch (Exception e)
                {
                    testResult.EndUserMessage = e.Message;
                }
            }
            else
            {
                testResult.EndUserMessage = "Missing required manifest properties (requireDevCredentials)";
            }

            return testResult;
        }

        /* Begin private methods */

        private bool ValidateManifest(AddonManifest manifest, out OperationResult testResult)
        {
            testResult = new OperationResult();

            var prop =
                    manifest.Properties.FirstOrDefault(
                        p => p.Key.Equals("requireDevCredentials", StringComparison.InvariantCultureIgnoreCase));

            if (prop == null || !prop.HasValue)
            {
                testResult.IsSuccess = false;
                testResult.EndUserMessage = "Missing required property 'requireDevCredentials'. This property needs to be provided as part of the manifest";
                return false;
            }

            if (string.IsNullOrWhiteSpace(manifest.ProvisioningUsername) ||
                string.IsNullOrWhiteSpace(manifest.ProvisioningPassword))
            {
                testResult.IsSuccess = false;
                testResult.EndUserMessage = "Missing credentials 'provisioningUsername' & 'provisioningPassword' . These values needs to be provided as part of the manifest";
                return false;
            }

            return true;
        }

        // TODO: We might be able to extend this. 
        private bool ValidateDevCreds(DeveloperOptions devOptions)
        {
            //return !(string.IsNullOrWhiteSpace(devOptions.AccessKey) || string.IsNullOrWhiteSpace(devOptions.SecretAccessKey));
            return true;
        }

        private OperationResult ParseDevOptions(string developerOptions, out DeveloperOptions devOptions)
        {
            devOptions = null;
            var result = new OperationResult() { IsSuccess = false };
            var progress = "";

            try
            {
                progress += "Parsing developer options...\n";
                devOptions = DeveloperOptions.Parse(developerOptions);
            }
            catch (ArgumentException e)
            {
                result.EndUserMessage = e.Message;
                return result;
            }

            result.IsSuccess = true;
            result.EndUserMessage = progress;
            return result;
        }
    }
}
