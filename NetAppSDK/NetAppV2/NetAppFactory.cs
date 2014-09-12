using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Apprenda.SaaSGrid.Addons.NetApp.V2.Models;

namespace Apprenda.SaaSGrid.Addons.NetApp.V2
{
    public class NetAppFactory
    {
        // pragma properties
        private static NetAppFactory _factory;

        // pragma constructors

        private NetAppFactory()
        {
            // exists to defeat instantiation
        }

        public NetAppFactory GetInstance()
        {
            if (_factory == null)
            {
                _factory = new NetAppFactory();
                _factory.init();
                return _factory;
            }
            else return _factory;
        }

        private void init()
        {
            // create a powershell connection to be reused for each command
        }

        // pragma public methods - these (and their overloads) will be used by the consumers.

        // This will create a volume off a given filer.
        public NetAppResponse CreateVolume(Volume v)
        { 
            return new NetAppResponse(); 
        }
        // overload, for ease.
        public NetAppResponse CreateVolume(String VolumeName, String AggregateName, String JunctionPath) 
        { 
            return CreateVolume(new Volume(VolumeName, AggregateName, JunctionPath)); 
        }
        // This will delete a volume off of a given filer.
        public NetAppResponse DeleteVolume(Volume v) 
        { 
            return new NetAppResponse(); 
        }
        // overload, for ease.
        public NetAppResponse DeleteVolume(String VolumeName, String AggregateName, String JunctionPath)
        { 
            return DeleteVolume(new Volume(VolumeName, AggregateName, JunctionPath)); 
        }

        // aggregate methods
        public NetAppResponse CreateAggregate(Aggregate a) { return new NetAppResponse(); }
        public NetAppResponse CreateAggregate(String aggregateName) { return CreateAggregate(new Aggregate()); }
        public NetAppResponse DeleteAggregate(Aggregate a) { return new NetAppResponse(); }
        public NetAppResponse DeleteAggregate(String aggregateName) { return DeleteAggregate(new Aggregate()); }

        // disk methods
        public NetAppResponse CreateDisk(Disk a) { return new NetAppResponse(); }
        public NetAppResponse CreateDisk(String DiskName) { return CreateDisk(new Disk()); }
        public NetAppResponse DeleteDisk(Disk a) { return new NetAppResponse(); }
        public NetAppResponse DeleteDisk(String DiskName) { return DeleteDisk(new Disk()); }


    }
}
