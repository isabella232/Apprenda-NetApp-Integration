using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace NetApp.Powershell.Commands
{
    [RunInstaller(true)]
    public class GetSystemOntapiVersionPSSnapIn : PSSnapIn
    {
        /// <summary>Creates an instance of DemoSnapin class.</summary>
        public GetSystemOntapiVersionPSSnapIn() 
            : base()
        {
        }
        ///<summary>The snap-in name that is used for registration</summary>
        public override string Name
        {
            get { return "GetSystemOntapiVersionPSSnapIn"; }
        }
        /// <summary>Gets vendor of the snap-in.</summary>
        public override string Vendor
        {
            get { return "NetApp"; }
        }
        /// <summary>Gets description of the snap-in. </summary>
        public override string Description
        {
            get { return "Powershell snap-in that includes Get-SystemOntapiVersion cmdlet"; }
        }
    }
}
