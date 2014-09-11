using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2.Models
{
    public class NetAppResponse
    {
        public bool IsSuccess { get; set; }

        public NetAppResponse()
        {
            IsSuccess = false;
        }

    }
}
