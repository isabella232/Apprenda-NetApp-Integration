using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetAppBatchTests.Models
{
    public class TestExecutionResult
    {
        public int ProvisionScore { get; set; }
        public int DeProvisionScore { get; set; }
        public string ExecutionResult { get; set; }
    }
}
