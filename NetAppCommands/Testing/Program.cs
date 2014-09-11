using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.Apprenda.NetApp;



namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetSystemOntapiVersionCommand command = new GetSystemOntapiVersionCommand();
            //command.ProcessRecord();

            VolumeOptions options = new VolumeOptions()
            {
                volumeName = "apprendaTestVol",
                containingAggregateName = "aggr1",
                volumeType = "Flexible",
                size = "20M"
            };

            // creates a 16MB volume (or, it should)
            VolumeCommands.AddVolume(options);
            // takes volume offline, then destroys it.
            VolumeCommands.RemoveVolume(options);
        }
    }
}
