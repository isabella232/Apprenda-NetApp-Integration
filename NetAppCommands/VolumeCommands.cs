using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetApp;
using NetApp.Filer;
using NetApp.Filer.System;
using NetApp.Filer.Volume;
using System.Threading.Tasks;
using Apprenda.SaaSGrid.Addons.NetApp.v1.Models;
using Apprenda.SaaSGrid.Addons;

namespace Apprenda.SaaSGrid.Addons.NetApp.v1
{
    public class VolumeOptions
    {
        public String volumeType {get; set;}
        public String volumeName { get; set; }
        
        // for flexible disks
        public String containingAggregateName { get; set; }
        public String size { get; set; }

        // for traditional disks
        public long diskcount { get; set; }
        public long disksize { get; set; }

        public void LoadInfoFromManifest(AddonManifest manifest)
        {
            var manifestProperties = manifest.GetProperties();
            foreach (var manifestProperty in manifestProperties)
            {
                switch (manifestProperty.Key)
                {
                    case ("aggregateName"):
                        containingAggregateName = manifestProperty.Value;
                        break;
                    case ("disksize"):
                        size = manifestProperty.Value;
                        break;
                    default: // means there are other manifest properties we don't need.
                        break;
                }
            }
        }

    }


    public static class VolumeCommands
    {

        public static void AddVolume(VolumeOptions vo)
        {
            VolumeCreate vc = new VolumeCreate()
                {
                    ContainingAggrName = vo.containingAggregateName,
                    Volume = vo.volumeName,
                    Size = vo.size
                };

            VolumeCreateResult vr = vc.Invoke(NetAppModel.GetFiler());

            Console.WriteLine(vr.APIStatus);

            VolumeListInfo vi = new VolumeListInfo()
            {
                Volume = vo.volumeName
            };

            VolumeListInfoResult vir = vi.Invoke(NetAppModel.GetFiler());
            foreach(VolumeInfo v in vir.Volumes)
            {
                Console.WriteLine(v.Name);
                Console.WriteLine(v.SizeAvailable);
                Console.WriteLine(v.SizeTotal);
                Console.WriteLine(v.RemoteLocation);
            }
            
        }

        public static void RemoveVolume(VolumeOptions rvo)
        {
            try
            {
                Console.WriteLine("Taking volume offline...");
                
                VolumeOffline vo = new VolumeOffline()
                {
                    Name = rvo.volumeName
                };
                VolumeOfflineResult vor = vo.Invoke(NetAppModel.GetFiler());

                Console.WriteLine("Console is offline. Destroying...");
                VolumeDestroy vd = new VolumeDestroy()
                {
                    Name = rvo.volumeName
                };
                VolumeDestroyResult vr = vd.Invoke(NetAppModel.GetFiler());
                Console.WriteLine(vr.APIStatus);
                
            }catch(Exception e)
            {
                Console.WriteLine("Problem: " + e.StackTrace);
            }
        }


        

    }
}
