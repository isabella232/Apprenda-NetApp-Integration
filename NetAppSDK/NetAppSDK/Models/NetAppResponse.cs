namespace Apprenda.SaaSGrid.Addons.NetApp.Models
{
    public class NetAppResponse
    {
        public bool IsSuccess { private get; set; }

        public int ReturnCode { private get; set; }

        public string ConnectionData { private get; set; }

        public string ConsoleOut { private get; set; }

        public string ErrorOut { private get; set; }

        public ProvisionAddOnResult ToAddOnResult()
        {
            // check addonresult return codes from
            // 1. create volume
            // 2. create snapmirror
            string endUserData;
            if (ReturnCode.Equals(0))
            {
                // set connection data as part of the connectionData object
                endUserData = ConsoleOut;
            }
            else if (ReturnCode.Equals(1))
            {
                // make sure we get the error messages.
                endUserData = "Completed with warnings. Please check."
                              + "\n Info Trace: " + ConsoleOut + "\n " +
                              "Error Trace: " + ErrorOut;
            }
            else
            {
                endUserData = "Unable to complete action. Please check errors." +
                              "\n Error Trace: " + ErrorOut;
            }
            return new ProvisionAddOnResult(ConnectionData, IsSuccess, endUserData);
        }

        public OperationResult ToOperationResult()
        {// check addonresult return codes from
            // 1. create volume
            // 2. create snapmirror
            string endUserData;
            if (ReturnCode.Equals(0))
            {
                // set connection data as part of the connectionData object
                endUserData = ConsoleOut;
            }
            else if (ReturnCode.Equals(1))
            {
                // make sure we get the error messages.
                endUserData = "Completed with warnings. Please check."
                              + "\n Info Trace: " + ConsoleOut + "\n " +
                              "Error Trace: " + ErrorOut;
            }
            else
            {
                IsSuccess = false;
                endUserData = "Unable to complete action. Please check errors." +
                              "\n Error Trace: " + ErrorOut;
            }
            return new OperationResult
            {
                IsSuccess = IsSuccess,
                EndUserMessage = endUserData
            };
        }
    }
}