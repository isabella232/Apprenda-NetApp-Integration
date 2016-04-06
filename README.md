#NetApp Integration

## Overview

This integration contains the ability to provision file storage from a NetApp Clustered Data ONTAP Operating System. In addition, it provides system operators with the ability to set up SnapMirror and SnapVault functionality for the file storage volumes that are created. 

All in all, it provides a nice self-service solution for developers already using the Apprenda platform to provision file storage for their apps.

## Decision Points

#### Why only a dedicated vServer, Root Path, and Disk Aggregate?
While we wanted to be flexible, this is not something we wanted to give the developer flexibility in choosing when they provisioned a volume. File storage typically will be set up in its own vServer and Disk Aggregate, no co-mingled with Block stores. We are planning on being able to specify which vServer and Aggregate based on policy, which is in our roadmap.

### Release Notes

#### v1.2
  * Support for ONTAP 8.3
  * Added support for SnapMirror and SnapVault policy declaration
  * Added portability for execution of scripts from a designated location. We copy the PS1 files down into a temporary directory.
  * Fixed an issue with Execution-Policy

#### v1.1
  * Support for ONTAP 8.2.1
  * Added CIFS and NFS support, making simpler connection strings

#### v1.0
  * Support for ONTAP 8.1.3
  * Basic Volume Provisioning

## Installation
  * Upload the ZIP as-is into the Addon Manager component in the SOC of your Apprenda platform.
  * Configure the addon. See *Configuration* for more information.

## Configuration

### Admin Properties
There are several inputs in the AddonManifest.xml file that must be captured. The below table is meant to provide explanation for the properties defined in the file. Use this to configure your connection to the NetApp system.

| Name (Alias) | Description | Example |
| ----- | ------------- | --------- |
| requiredevcredentials | Used to Override Default Credentials and use the ones specified by the developer. Keep false. | False |
| clustermgtendpoint | This is the endpoint needed to connect to the filer.  | <ip-address or dns name> |
| adminusername | User with permissions to perform operations on the filer. Has to at the very least be able to create a volume. | admin |
| adminpassword | Associated password. | password |
| vserver | This is the vServer within NetApp that you want to work with. Currently we only support the creation of volumes on a dedicated vServer. | apprenda01 |
| defaultprotocol | This is the protocol by which you want to access your file storage. CIFS and NFS are supported. | CIFS, NFS |
| defaultaggregate | This is the aggregate name that you want to create the volume one. Currently we only support the creation of volumes on a single aggregate. See the decision points section on why. | aggr1 |
| defaultrootpath | This is the default tree where the volume will be provisioned. Currently we only support the creation of volumes from a single default root path. See the decision points section on why.  | /volumes/fs/ |
| snapenable | Turns on SnapMirror. Set to True or False | True, False |
| vaultenable | Turns on SnapVault. | Set to True or False | True, False |
| snaptype |This is the type of SnapMirror functionality you want to enable. Can be load sharing (ls) or data protection (dp) | ls, dp |
| snapvaultschedule | This the default rate at which snapvault commences. This refers to the Schedules in the Cluster | Hourly | 
| snapmirrorschedule | This is the default rate at which snapmirror commences. This refers to the Schedules in the Cluster  | 8hour | 
| snapvaultpolicyname | This is the policy to use when creating the snapvault relationship between volumes. | <policyname> | 
| snapmirrorpolicyname | This is the policy to use when creating the snapvault relationship between volumes. | <policyname> |
| shareendpoint | The endpoint in which the shares are accessed. ie. absolute path of the CIFS share | \\<shareendpoint>\<rootpath> | 
| scriptrepotype | We introduced fucntionality that enables customers the ability to run their own scripts. To enable this, enter "share" here and specify the absolute path to the share. | "share" |
| scriptrepo | This is the specified script repository. | \\script-repo\script-folder |
| maxallocatedstorage | This is the maxiumum allocated storage a developer is allowed to provision, across all file storage. Follows the format [0-9]* M/G/T | 100G |

### Developer Parameters.

These are the properties that the developer will specify. You have the ability to customize and add additional properties here, just make sure you add your custom logic to the codebase that parses that parameter. The below table outlines the developer parameters that can be used to provision an instance of the addon.

| Name (Alias) | Description | Example | 
| ------------ | ----------- | ------- |
| name | The name of the volume you want to provision. If CIFS/NFS is enabled, this will be the share name as well. | fileshare01 | 
| size | The size of the volume you want to provision. Follows the format of [0-9]* M/G/T (for MB, GB, TB) | 2G |
 

## Troubleshooting
* Please create an issue and we'll throw it in our internal tracking. We actively monitor the Issues page!

## Roadmap
* Full Documentation
* Migration from NetApp POSH toolkit to direct mode (SSH)
* Add aliasing when parsing some developer parameters (ie. 2GB ~ 2G)
