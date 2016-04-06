using Apprenda.SaaSGrid.Addons.NetApp.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Apprenda.SaaSGrid.Addons.NetApp.Models
{
    public class Volume
    {
        // required parameters for Volume Creation from the developer
        internal string Name { get; set; }

        internal string Size { get; set; }

        // we're going to do this at the manifest level. platform operator should set the default junciton path
        internal string JunctionPath { private get; set; }

        internal string AggregateName { private get; set; }

        internal string Protocol { private get; set; }

        internal bool SnapEnable { private get; set; }

        internal string SnapType { private get; set; }

        internal string SnapMirrorPolicyName { private get; set; }

        internal string SnapVaultPolicyName { private get; set; }

        internal string SnapMirrorSchedule { private get; set; }

        internal string SnapVaultSchedule { private get; set; }

    // optional parameters for Volume Creation
        internal string Comment { private get; set; }

        internal string AntiVirusOnAccessPolicy { private get; set; }

        internal string ExportPolicy { private get; set; }

        internal string FlexCacheCachePolicy { private get; set; }

        internal string FlexCacheFillPolicy { private get; set; }

        internal string FlexCacheOriginVolume { private get; set; }

        internal Int32 GroupId { private get; set; }

        internal string IndexDirectoryFormat { private get; set; }

        internal bool JunctionActive { private get; set; }

        internal double MaxDirectorySize { private get; set; }

        internal bool NvFailEnabled { private get; set; }

        internal string SecurityStyle { private get; set; }

        internal string SnapshotPolicy { get; set; }

        internal string SnapshotSchedule { get; set; }

        internal string SpaceReserver { private get; set; }

        internal string State { private get; set; }

        internal string Type { private get; set; }

        internal Int32 UserId { private get; set; }

        internal bool VserverRoot { private get; set; }

        internal Int32 SnapshotReserver { private get; set; }

        internal Int32 VmAlignSector { private get; set; }

        internal string VmAlignSuffic { private get; set; }

        internal string QosPolicyGroup { private get; set; }

        internal string Language { private get; set; }

        internal string UnixPermissions { private get; set; }

        internal string CIFSRootServer { private get; set; }

        // the idea here is to optimize. if the parameters aren't default or null, add them.

        internal string BuildConnectionString()
        {
            // if CIFS, return the share path
            if (JunctionPath.Contains("/"))
            {
                return "//" + CIFSRootServer + JunctionPath + "/" + Name;
            }
            else
            {
                return "//" + CIFSRootServer + "/" + JunctionPath + "/" + Name;
            }
        }


        [CanBeNull]
        public IEnumerable<Tuple<string, string>> ToPsArguments()
        {
            var pList = new List<Tuple<String, String>>
            {
                new Tuple<String, String>("-aggregateName", AggregateName),
                new Tuple<String, String>("-junctionPath", JunctionPath),
                new Tuple<String, String>("-Size", Size),
                new Tuple<String, String>("-volName", Name)
            };
            if (Comment != null) pList.Add(new Tuple<string, string>("-Comment", Comment));
            if (AntiVirusOnAccessPolicy != null) pList.Add(new Tuple<string, string>("-AntiVirusOnAccessPolicy", AntiVirusOnAccessPolicy));
            if (ExportPolicy != null) pList.Add(new Tuple<string, string>("-ExportPolicy", ExportPolicy));
            if (FlexCacheCachePolicy != null) pList.Add(new Tuple<string, string>("-FlexCacheCachePolicy", FlexCacheCachePolicy));
            if (FlexCacheFillPolicy != null) pList.Add(new Tuple<string, string>("-FlexCacheFillPolicy", FlexCacheFillPolicy));
            if (FlexCacheOriginVolume != null) pList.Add(new Tuple<string, string>("-FlexCacheOriginVolume", FlexCacheOriginVolume));
            if (!((-1).Equals(GroupId))) pList.Add(new Tuple<string, string>("-GroupId", GroupId.ToString(CultureInfo.InvariantCulture)));
            if (IndexDirectoryFormat != null) pList.Add(new Tuple<string, string>("-IndexDirectoryFormat", IndexDirectoryFormat));
            if (JunctionActive) pList.Add(new Tuple<string, string>("-JunctionActive", "$true"));
            if (!(Math.Abs(MaxDirectorySize - (-1)) < 0)) pList.Add(new Tuple<string, string>("-MaxDirectorySize", MaxDirectorySize.ToString(CultureInfo.InvariantCulture)));
            if (!NvFailEnabled == false) pList.Add(new Tuple<string, string>("-NvFailEnabled", NvFailEnabled.ToString()));
            if (SecurityStyle != null) pList.Add(new Tuple<string, string>("-SecurityStyle", SecurityStyle));
            if (SnapshotPolicy != null) pList.Add(new Tuple<string, string>("-SnapshotPolicy", SnapshotPolicy));
            if (SpaceReserver != null) pList.Add(new Tuple<string, string>("-SpaceReserver", SnapshotReserver.ToString(CultureInfo.InvariantCulture)));
            if (State != null) pList.Add(new Tuple<string, string>("-State", State));
            if (Type != null) pList.Add(new Tuple<string, string>("-Type", Type));
            if (UserId != -1) pList.Add(new Tuple<string, string>("-UserId", UserId.ToString(CultureInfo.InvariantCulture)));
            if (VserverRoot) pList.Add(new Tuple<string, string>("-VserverRoot", "$true"));
            if (SnapshotReserver != -1) pList.Add(new Tuple<string, string>("-SnapshotReserver", SnapshotReserver.ToString(CultureInfo.InvariantCulture)));
            if (VmAlignSector != -1) pList.Add(new Tuple<string, string>("-VmAlignSector", VmAlignSector.ToString(CultureInfo.InvariantCulture)));
            if (VmAlignSuffic != null) pList.Add(new Tuple<string, string>("-VmAlignSuffic", VmAlignSuffic));
            if (QosPolicyGroup != null) pList.Add(new Tuple<string, string>("-QosPolicyGroup", QosPolicyGroup));
            if (Language != null) pList.Add(new Tuple<string, string>("-Language", Language));
            if (Protocol != null) pList.Add(new Tuple<string, string>("-Protocol", Protocol));
            if (UnixPermissions != null) pList.Add(new Tuple<string, string>("-UnixPermissions", UnixPermissions));
            if (SnapEnable) pList.Add(new Tuple<string, string>("-EnableSnapMirror", "$true"));
            if (SnapMirrorPolicyName != null) pList.Add(new Tuple<string, string>("-snapmirrorpolicyname", SnapMirrorPolicyName));
            if (SnapVaultPolicyName != null) pList.Add(new Tuple<string, string>("-snapvaultpolicyname", SnapVaultPolicyName));
            if (SnapMirrorSchedule != null) pList.Add(new Tuple<string, string>("-snapmirrorschedule", SnapMirrorSchedule));
            if (SnapVaultSchedule != null) pList.Add(new Tuple<string, string>("-snapvaultschedule", SnapVaultSchedule));
            if (SnapType != null) pList.Add(new Tuple<string, string>("-snaptype", SnapType));
            return pList;
        }
    }
}