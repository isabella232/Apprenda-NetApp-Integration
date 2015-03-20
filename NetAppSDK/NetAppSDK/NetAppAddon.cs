using System;
using System.Linq;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class NetAppAddon : AddonBase
    {
        // Deprovision NetApp
        // Input: AddonDeprovisionRequest request
        // Output: OperationResult
        public override OperationResult Deprovision(AddonDeprovisionRequest request)
        {
            var deprovisionResult = new OperationResult { IsSuccess = false };
            try
            {
                // this loads in the developer options and the manifest parameters
                // validation will also occur here, so if this fails it will be caught prior to any invocation on the cluster.
                var developerOptions = DeveloperOptions.Parse(request.DeveloperOptions);
                developerOptions.LoadItemsFromManifest(request.Manifest);
                // for assumptions now, create a volume
                var netappresponse = NetAppFactory.DeleteVolume(developerOptions);
                // use the class's conversion method.
                return netappresponse.ToOperationResult();
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
            var provisionResult = new ProvisionAddOnResult("") { IsSuccess = false };
            try
            {
                // this loads in the developer options and the manifest parameters
                // validation will also occur here, so if this fails it will be caught prior to any invocation on the cluster.
                var developerOptions = DeveloperOptions.Parse(request.DeveloperOptions);
                developerOptions.LoadItemsFromManifest(request.Manifest);
                // for assumptions now, create a volume
                // we're handling snapmirror at the factory level
                var netappresponse = NetAppFactory.CreateVolume(developerOptions);
                provisionResult = netappresponse.ToAddOnResult();
                provisionResult.IsSuccess = true;
                provisionResult.ConnectionData = developerOptions.VolumeToProvision.BuildConnectionString();
                // well, it has to return the cifs or nfs share information. so, we'll need a method here.
            }
            catch (Exception e)
            {
                provisionResult.IsSuccess = false;
                provisionResult.EndUserMessage = e.Message + "\n" + e.StackTrace;
            }
            return provisionResult;
        }

        // Testing Instance
        // Input: AddonTestRequest request
        // Output: OperationResult
        public override OperationResult Test(AddonTestRequest request)
        {
            var manifest = request.Manifest;
            var developerOptions = request.DeveloperOptions;
            var testResult = new OperationResult { IsSuccess = false };
            var testProgress = "";

            if (manifest.Properties != null && manifest.Properties.Any())
            {
                DeveloperOptions devOptions;

                var parseOptionsResult = ParseDevOptions(developerOptions, out devOptions);
                if (!parseOptionsResult.IsSuccess)
                {
                    return parseOptionsResult;
                }
                testProgress += parseOptionsResult.EndUserMessage;
                try
                {
                    // load critical info from manifest to connect to netapp filer
                    devOptions.LoadItemsFromManifest(request.Manifest);

                    var apr = new AddonProvisionRequest
                    {
                        DeveloperOptions = request.DeveloperOptions,
                        Manifest = request.Manifest
                    };

                    var dpr = new AddonDeprovisionRequest
                    {
                        DeveloperOptions = request.DeveloperOptions,
                        Manifest = request.Manifest
                    };
                    var provisionTest = Provision(apr);
                    if (!provisionTest.IsSuccess)
                    {
                        return provisionTest;
                    }
                    testProgress += provisionTest.EndUserMessage;
                    var deprovisionTest = Deprovision(dpr);
                    if (!deprovisionTest.IsSuccess)
                    {
                        return deprovisionTest;
                    }
                    testProgress += deprovisionTest.EndUserMessage;
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

        private static OperationResult ParseDevOptions(string developerOptions, out DeveloperOptions devOptions)
        {
            devOptions = null;
            var result = new OperationResult { IsSuccess = false };
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