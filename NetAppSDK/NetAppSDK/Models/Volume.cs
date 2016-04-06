namespace Apprenda.SaaSGrid.Addons.NetApp.Models
{
    public class Volume
    {
        // required parameters for Volume Creation from the developer
        internal string Name { get; set; }
        internal string Size { get; set; }
        // we're going to do this at the manifest level. platform operator should set the default junciton path
        internal string JunctionPath { get; set; }
        internal string AggregateName { get; set; }
        internal string Protocol { get; set; }
        internal bool SnapEnable { get; set; }
        internal string SnapType { get; set; }
        internal string SnapMirrorPolicyName { get; set; }
        internal string SnapVaultPolicyName { get; set; }
        internal string SnapMirrorSchedule { get; set; }
        internal string SnapVaultSchedule { get; set; }
        // optional parameters for Volume Creation
        internal string Comment { get; set; }
        internal string AntiVirusOnAccessPolicy { get; set; }
        internal string ExportPolicy { get; set; }
        internal string FlexCacheCachePolicy { get; set; }
        internal string FlexCacheFillPolicy { get; set; }
        internal string FlexCacheOriginVolume { get; set; }
        internal int GroupId { get; set; }
        internal string IndexDirectoryFormat { get; set; }
        internal bool JunctionActive { get; set; }
        internal double MaxDirectorySize { get; set; }
        internal bool NvFailEnabled { get; set; }
        internal string SecurityStyle { get; set; }
        internal string SnapshotPolicy { get; set; }
        internal string SnapshotSchedule { get; set; }
        internal string SpaceReserver { get; set; }
        internal string State { get; set; }
        internal string Type { get; set; }
        internal int UserId { get; set; }
        internal bool VserverRoot { get; set; }
        internal int SnapshotReserver { get; set; }
        internal int VmAlignSector { get; set; }
        internal string VmAlignSuffic { get; set; }
        internal string QosPolicyGroup { get; set; }
        internal string Language { get; set; }
        internal string UnixPermissions { get; set; }
        internal string CifsRootServer { private get; set; }
        internal bool VaultEnable { get; set; }

        // the idea here is to optimize. if the parameters aren't default or null, add them.

        internal string BuildConnectionString()
        {
            // if CIFS, return the share path
            if (JunctionPath.Contains("/"))
            {
                return "//" + CifsRootServer + JunctionPath + "/" + Name;
            }
            return "//" + CifsRootServer + "/" + JunctionPath + "/" + Name;
        }
    }
}