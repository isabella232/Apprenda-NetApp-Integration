using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Apprenda.SaaSGrid.Addons.NetApp.Models
{
    public class NetAppResponse
    {
        public bool IsSuccess { get; set; }

        public NetAppResponse()
        {
            IsSuccess = false;
        }

        public static NetAppResponse ParseOutput(Collection<PSObject> output)
        {
            return new NetAppResponse();
        }
    }
}