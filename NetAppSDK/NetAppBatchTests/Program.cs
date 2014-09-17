using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apprenda.SaaSGrid.Addons.NetApp;
using Apprenda.SaaSGrid.Addons;
using NetAppBatchTests.Models;

namespace NetAppBatchTests
{
    class Program
    {
        public static List<string> developerOptionsTestCases = new List<string>();
        

        // for now, create a base manifest
        public static AddonManifest addonManifest = new AddonManifest() 
        { 
            Author = "Chris Dutra", 
            ProvisioningUsername = "chris@dutronlabs.com", 
            ProvisioningPassword = "cyrixm2r",
            ProvisioningLocation = "Apprenda-NYC",
            IsEnabled = true,
            Version = "2.0",
            Vendor = "Apprenda",
            Name = "NetApp"
        };

        public static void LoadDeveloperOptionsTestCases()
        {
            developerOptionsTestCases.Add("");
            // these two should fail as not enough information is available
            developerOptionsTestCases.Add("provisioningType=vol");
            developerOptionsTestCases.Add("provisioningType=aggr");
            // these should pass, given a good manifest
            developerOptionsTestCases.Add("provisioningType=vol&name=testvolume1&size=20M&aggregatename=raid4_test&junctionpath=/testvolume1");
        }


        public static void LoadManifest()
        {
            List<AddonProperty> workingManifestProperties = new List<AddonProperty>();
            workingManifestProperties.Add(new AddonProperty() { 
                DisplayName = "VServer",
                Value = "apprenda-svm"
            });
            workingManifestProperties.Add(new AddonProperty()
            {
                DisplayName = "AdminUserName",
                Value = "admin"
            });
            workingManifestProperties.Add(new AddonProperty()
            {
                DisplayName = "AdminPassword",
                Value = "cyrixm2r"
            });
            workingManifestProperties.Add(new AddonProperty()
            {
                DisplayName = "ClusterMgtEndpoint",
                Value = "192.168.233.101"
            });

            addonManifest.Properties = workingManifestProperties;
        }

        // this class is used for automated testing. we'll run the console on the command line to
        // conduct testing for each use case

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            LoadManifest();
            LoadDeveloperOptionsTestCases();
            Console.Write("Complete.");
            int testRun = 0;
            int overallScore = 0;
            int numberOfPassedTests = 0;
            int numberOfFailedTests = 0;
            List<TestExecutionResult> testResults = new List<TestExecutionResult>();
            foreach(string d in developerOptionsTestCases)
            {
                TestExecutionResult t = ExecuteTest(d, addonManifest);
                testResults.Add(t);
                testRun++;
                if (t.ProvisionScore == 100) numberOfPassedTests++; else numberOfFailedTests++;
                if (t.DeProvisionScore == 100) numberOfPassedTests++; else numberOfFailedTests++;
                overallScore = (overallScore + (t.ProvisionScore + t.DeProvisionScore / 2) / testRun);
            }
            // we'll do some analytics here.
            
            Console.WriteLine("*****************Test Run Results***********");
            Console.WriteLine("Overall Score: " + overallScore);
        }

        public static TestExecutionResult ExecuteTest(string d, AddonManifest m)
        {
            Addon a = new Addon();
            // test 1 - provision
            var provisionResult = a.Provision(new AddonProvisionRequest()
                {
                    DeveloperOptions = d,
                    Manifest = m
                });
            var testResult = new TestExecutionResult();
            if(provisionResult.IsSuccess)
            {
                testResult.ProvisionScore = 100;
            }
            else
            {
                testResult.ProvisionScore = 0;
            }
            testResult.ExecutionResult = provisionResult.EndUserMessage + "\n Connection Data: " + provisionResult.ConnectionData;
            // test 2 - deprovision (well, we should only run this is the first test passed)
            if (provisionResult.IsSuccess)
            {
                var deprovisionResult = a.Deprovision(new AddonDeprovisionRequest()
                    {
                        DeveloperOptions = d,
                        Manifest = m
                    });
                if(deprovisionResult.IsSuccess)
                {
                    testResult.DeProvisionScore = 100;
                }
                else
                {
                    testResult.DeProvisionScore = 0;
                }
            }
            else
            {
                // unable to run deprovision test
                testResult.DeProvisionScore = 0;
            }
            return testResult;
        }
    }
}
