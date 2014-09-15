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
        public string Size { get; set; }

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

        public List<Tuple<String, String>> ToPsArguments()
        {
            List<Tuple<String, String>> pList = new List<Tuple<String, String>>();
            pList.Add(new Tuple<String,String>("Name", Name));
            pList.Add(new Tuple<String, String>("AggregateName", AggregateName)); 
            pList.Add(new Tuple<String, String>("JunctionPath", JunctionPath));

            if (Comment != null) pList.Add(new Tuple<string,string>("Comment", Comment));
            if (AntiVirusOnAccessPolicy != null) pList.Add(new Tuple<string, string>("AntiVirusOnAccessPolicy", AntiVirusOnAccessPolicy));
            if (ExportPolicy != null) pList.Add(new Tuple<string, string>("ExportPolicy", ExportPolicy));
            if (FlexCacheCachePolicy != null) pList.Add(new Tuple<string, string>("FlexCacheCachePolicy", FlexCacheCachePolicy));
            if (FlexCacheFillPolicy != null) pList.Add(new Tuple<string, string>("FlexCacheFillPolicy", FlexCacheFillPolicy));
            if (FlexCacheOriginVolume != null) pList.Add(new Tuple<string, string>("FlexCacheOriginVolume", FlexCacheOriginVolume));
            if (GroupId != null) pList.Add(new Tuple<string, string>("GroupId", GroupId.ToString()));
            if (IndexDirectoryFormat != null) pList.Add(new Tuple<string, string>("IndexDirectoryFormat", IndexDirectoryFormat));
            if (JunctionActive != null) pList.Add(new Tuple<string, string>("JunctionActive", JunctionActive.ToString()));
            if (MaxDirectorySize != null) pList.Add(new Tuple<string, string>("Comment", MaxDirectorySize.ToString()));
            if (NvFailEnabled != null) pList.Add(new Tuple<string, string>("Comment", NvFailEnabled.ToString()));
            if (SecurityStyle != null) pList.Add(new Tuple<string, string>("Comment", SecurityStyle));
            if (SnapshotPolicy != null) pList.Add(new Tuple<string, string>("Comment", SnapshotPolicy));
            if (SpaceReserver != null) pList.Add(new Tuple<string, string>("Comment", SnapshotReserver.ToString());
            if (State != null) pList.Add(new Tuple<string, string>("Comment", State));
            if (Type != null) pList.Add(new Tuple<string, string>("Comment", Type));
            if (UserId != null) pList.Add(new Tuple<string, string>("Comment", UserId.ToString()));
            if (VserverRoot != null) pList.Add(new Tuple<string, string>("Comment", VserverRoot.ToString()));
            if (SnapshotReserver != null) pList.Add(new Tuple<string, string>("Comment", SnapshotReserver.ToString()));
            if (VmAlignSector != null) pList.Add(new Tuple<string, string>("Comment", VmAlignSector.ToString()));
            if (VmAlignSuffic != null) pList.Add(new Tuple<string, string>("Comment", VmAlignSuffic));
            if (QosPolicyGroup != null) pList.Add(new Tuple<string, string>("Comment", QosPolicyGroup));
            if (Language != null) pList.Add(new Tuple<string, string>("Comment", Language));

            return pList;
        }
    }

    
}
