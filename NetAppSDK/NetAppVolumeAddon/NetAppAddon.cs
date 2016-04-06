using System;

namespace Apprenda.SaaSGrid.Addons.NetApp
{
    public class NetAppAddon : AddonBase
    {
        // Deprovision NetApp
        // Input: AddonDeprovisionRequest request
        // Output: OperationResult
        public override OperationResult Deprovision(AddonDeprovisionRequest request)
        {
            var deprovisionResult = new OperationResult {IsSuccess = false};
            try
            {
                // this loads in the developer options and the manifest parameters
                // validation will also occur here, so if this fails it will be caught prior to any invocation on the cluster.
                var developerOptions = DeveloperParameters.Parse(request.DeveloperParameters, request.Manifest);
                // for assumptions now, delete a volume
                var netappresponse = NetAppFactory.DeleteVolume(developerOptions, request.ConnectionData);
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
            var provisionResult = new ProvisionAddOnResult("") {IsSuccess = false};
            try
            {
                var developerParameters = DeveloperParameters.Parse(request.DeveloperParameters, request.Manifest);
                var netappresponse = NetAppFactory.CreateVolume(developerParameters);
                provisionResult = netappresponse.ToAddOnResult();
                // this appears to be wrong. we need to check what's coming back from the powershell script
                //provisionResult.IsSuccess = true;
                provisionResult.ConnectionData = developerParameters.VolumeToProvision.BuildConnectionString();
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
            var testResult = new OperationResult {IsSuccess = false};
            var apr = new AddonProvisionRequest
            {
                DeveloperParameters = request.DeveloperParameters,
                Manifest = request.Manifest
            };

            var dpr = new AddonDeprovisionRequest
            {
                DeveloperParameters = request.DeveloperParameters,
                Manifest = request.Manifest
            };
            var provisionTest = Provision(apr);
            if (!provisionTest.IsSuccess)
            {
                return provisionTest;
            }
            var testProgress = provisionTest.EndUserMessage;
            var deprovisionTest = Deprovision(dpr);
            if (!deprovisionTest.IsSuccess)
            {
                return deprovisionTest;
            }
            testProgress += deprovisionTest.EndUserMessage;
            testResult.IsSuccess = true;
            testResult.EndUserMessage = testProgress;
            return testResult;
        }
    }
}