using Apprenda.SaaSGrid.Addons;
using Apprenda.SaaSGrid.Addons.NetApp;
using NetAppBatchTests.Models;
using System;
using System.Collections.Generic;

namespace NetAppBatchTests
{
    internal class Program
    {
        public static List<string> developerOptionsTestCases = new List<string>();
        public static List<ExpectedResults> assertions = new List<ExpectedResults>();

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
            //developerOptionsTestCases.Add("");
            // these two should fail as not enough information is available
            //developerOptionsTestCases.Add("provisioningType=vol");
            //developerOptionsTestCases.Add("provisioningType=aggr");
            // these should pass, given a good manifest
            developerOptionsTestCases.Add("name=testvolume1&size=20M");
        }

        public class ExpectedResults
        {
            public bool expectedSuccess { get; set; }

            public string expectedErrorMessage { get; set; }

            public string expectedSuccessMessage { get; set; }
        }

        public static void LoadExpectedResults()
        {
            //assertions.Add(new ExpectedResults() { expectedSuccess = false, expectedErrorMessage = "Object reference not set to an instance of an object."});
            //assertions.Add(new ExpectedResults() { expectedSuccess = false});
            //assertions.Add(new ExpectedResults() { expectedSuccess = false});
            assertions.Add(new ExpectedResults() { expectedSuccess = true });
        }

        public static void LoadManifest()
        {
            List<AddonProperty> workingManifestProperties = new List<AddonProperty>();
            workingManifestProperties.Add(new AddonProperty()
            {
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
            LoadExpectedResults();
            Console.WriteLine("Complete.");
            int testRun = 0;
            int overallScore = 0;
            int numberOfPassedTests = 0;
            int numberOfFailedTests = 0;
            List<TestExecutionResult> testResults = new List<TestExecutionResult>();
            foreach (string d in developerOptionsTestCases)
            {
                TestExecutionResult t = ExecuteTest(d, addonManifest, assertions[testRun]);
                testResults.Add(t);
                testRun++;
                if (t.ProvisionScore == 100) numberOfPassedTests++; else numberOfFailedTests++;
                if (t.DeProvisionScore == 100) numberOfPassedTests++;
                if (t.DeProvisionScore == 0) numberOfFailedTests++;

                // overall score is the calculation of provisioning and deprovisioning scores
                //
                if (t.DeProvisionScore != -1)
                {
                    int testScore = ((t.DeProvisionScore + t.ProvisionScore) / 2);
                    overallScore = (((overallScore * (testRun - 1)) + testScore) / testRun);
                }
                else
                {
                    // if we skipped deprov, just get the provisioning score
                    int testScore = t.ProvisionScore;
                    overallScore = (((overallScore * (testRun - 1)) + testScore) / testRun);
                }
                // we'll do some analytics here.
            }
            Console.WriteLine("*****************Test Run Results***********");
            foreach (TestExecutionResult t in testResults)
            {
                Console.WriteLine("Test Result: " + t.ExecutionResult);
            }
            Console.WriteLine("***********Summary**************************");
            Console.WriteLine("Number of Tests Run: " + testRun);
            Console.WriteLine("Overall Score: " + overallScore);
            Console.WriteLine();
            Console.WriteLine("Number of Passed Tests: " + numberOfPassedTests);
            Console.WriteLine("Number of Failed Tests: " + numberOfFailedTests);
        }

        public static TestExecutionResult ExecuteTest(string d, AddonManifest m, ExpectedResults assertions)
        {
            Addon a = new Addon();
            // test 1 - provision
            var provisionResult = a.Provision(new AddonProvisionRequest()
                {
                    DeveloperOptions = d,
                    Manifest = m
                });
            var testResult = new TestExecutionResult();
            // if we got the result we expected, show 100. else 0.
            if ((provisionResult.IsSuccess && assertions.expectedSuccess) || (!provisionResult.IsSuccess && !assertions.expectedSuccess))
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
                if ((deprovisionResult.IsSuccess && assertions.expectedSuccess) || (!deprovisionResult.IsSuccess && !assertions.expectedSuccess))
                { testResult.DeProvisionScore = 100; }
                else { testResult.DeProvisionScore = 0; }
            }
            else
            {   // unable to run deprovision test, skip (-1)
                testResult.DeProvisionScore = -1;
            }
            return testResult;
        }
    }
}