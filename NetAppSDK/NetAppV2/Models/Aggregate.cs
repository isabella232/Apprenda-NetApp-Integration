using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2.Models
{
    public class Aggregate
    {
        public String Name { get; set; }

        internal IEnumerable<Tuple<string, string>> ToPsArguments()
        {
            throw new NotImplementedException();
        }
    }
}
