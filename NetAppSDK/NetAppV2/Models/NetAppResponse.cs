using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2.Models
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
