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

        public Volume()
        {
            GroupId = -1;
            UserId = -1;
            SnapshotReserver = -1;
            VmAlignSector = -1;
            MaxDirectorySize = -1;
            JunctionActive = false;
            NvFailEnabled = false;
            VserverRoot = false;
        }

        // the idea here is to optimize. if the parameters aren't default or null, add them.
        public List<Tuple<String, String>> ToPsArguments()
        {
            List<Tuple<String, String>> pList = new List<Tuple<String, String>>();
            pList.Add(new Tuple<String,String>("-volName", Name));
            pList.Add(new Tuple<String, String>("-aggregateName", AggregateName)); 
            pList.Add(new Tuple<String, String>("-junctionPath", JunctionPath));
            pList.Add(new Tuple<String, String>("-Size", Size));

            if (Comment != null) pList.Add(new Tuple<string,string>("-Comment", Comment));
            if (AntiVirusOnAccessPolicy != null) pList.Add(new Tuple<string, string>("-AntiVirusOnAccessPolicy", AntiVirusOnAccessPolicy));
            if (ExportPolicy != null) pList.Add(new Tuple<string, string>("-ExportPolicy", ExportPolicy));
            if (FlexCacheCachePolicy != null) pList.Add(new Tuple<string, string>("-FlexCacheCachePolicy", FlexCacheCachePolicy));
            if (FlexCacheFillPolicy != null) pList.Add(new Tuple<string, string>("-FlexCacheFillPolicy", FlexCacheFillPolicy));
            if (FlexCacheOriginVolume != null) pList.Add(new Tuple<string, string>("-FlexCacheOriginVolume", FlexCacheOriginVolume));
            if (!(GroupId.Equals(-1))) pList.Add(new Tuple<string, string>("-GroupId", GroupId.ToString()));
            if (IndexDirectoryFormat != null) pList.Add(new Tuple<string, string>("-IndexDirectoryFormat", IndexDirectoryFormat));
            if (!(JunctionActive == false)) pList.Add(new Tuple<string, string>("-JunctionActive", JunctionActive.ToString()));
            if (!(MaxDirectorySize == -1)) pList.Add(new Tuple<string, string>("-MaxDirectorySize", MaxDirectorySize.ToString()));
            if (!NvFailEnabled == false) pList.Add(new Tuple<string, string>("-NvFailEnabled", NvFailEnabled.ToString()));
            if (SecurityStyle != null) pList.Add(new Tuple<string, string>("-SecurityStyle", SecurityStyle));
            if (SnapshotPolicy != null) pList.Add(new Tuple<string, string>("-SnapshotPolicy", SnapshotPolicy));
            if (SpaceReserver != null) pList.Add(new Tuple<string, string>("-SpaceReserver", SnapshotReserver.ToString()));
            if (State != null) pList.Add(new Tuple<string, string>("-State", State));
            if (Type != null) pList.Add(new Tuple<string, string>("-Type", Type));
            if (!(UserId == -1)) pList.Add(new Tuple<string, string>("-UserId", UserId.ToString()));
            if (VserverRoot != false) pList.Add(new Tuple<string, string>("-VserverRoot", VserverRoot.ToString()));
            if (SnapshotReserver != -1) pList.Add(new Tuple<string, string>("-SnapshotReserver", SnapshotReserver.ToString()));
            if (VmAlignSector != -1) pList.Add(new Tuple<string, string>("-VmAlignSector", VmAlignSector.ToString()));
            if (VmAlignSuffic != null) pList.Add(new Tuple<string, string>("-VmAlignSuffic", VmAlignSuffic));
            if (QosPolicyGroup != null) pList.Add(new Tuple<string, string>("-QosPolicyGroup", QosPolicyGroup));
            if (Language != null) pList.Add(new Tuple<string, string>("-Language", Language));

            return pList;
        }
    }

    
}
