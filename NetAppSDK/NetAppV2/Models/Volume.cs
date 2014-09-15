using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2.Models
{
    public class Volume
    {
        // required parameters for Volume Creation
        public string Name {get; set;}
        public string AggregateName {get; set;}
        public string JunctionPath {get; set;}


        // optional parameters for Volume Creation
        public string Comment {get; set;}
        public string AntiVirusOnAccessPolicy {get; set;}
        public string ExportPolicy { get; set; }
        public string FlexCacheCachePolicy { get; set; }
        public string FlexCacheFillPolicy { get; set; }
        public string FlexCacheOriginVolume { get; set; }
        public Int32 GroupId { get; set; }
        public string IndexDirectoryFormat { get; set; }
        public bool JunctionActive { get; set; }
        public double MaxDirectorySize { get; set; }
        public bool NvFailEnabled { get; set; }
        public string SecurityStyle { get; set; }
        public string SnapshotPolicy { get; set; }
        public string SpaceReserver { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string UnixPermissions { get; set; }
        public Int32 UserId { get; set; }
        public bool VserverRoot { get; set; }
        public Int32 SnapshotReserver { get; set; }
        public Int32 VmAlignSector { get; set; }
        public string VmAlignSuffic { get; set; }
        public string QosPolicyGroup { get; set; }
        public string Language { get; set; }
    }
}
