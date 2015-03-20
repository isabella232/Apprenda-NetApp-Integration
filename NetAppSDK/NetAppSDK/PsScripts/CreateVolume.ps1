param(
    [string] $username = $(throw "-username is required"),
    [string] $password = $(throw "-password is required"; exit -1),
	[string] $vserver = $(throw "-vserver is required"; exit -1),
    [string] $endpoint = $(throw "-endpoint is required"; exit -1),
    [string] $volName = $(throw "-volName is required"; exit -1),
    [string] $aggregateName = $(throw "-aggregateName is required"; exit -1),
    [string] $junctionPath = $(throw "-junctionPath is required"; exit -1),
    [string] $size = $(throw "-size is required"; exit -1),
    [string] $protocol = $(throw "-protocol is required"; exit -1),
	[bool] $enableSnapMirror = $false,
	[string] $snapmirrorpolicyname = $(if($enableSnapMirror) { throw "-snapmirrorpolicyname missing from manifest. please contact your platform operator"; exit -1}),
	[string] $snapmirrorschedule = $(if($enableSnapMirror){ throw "-snapmirrorschedule is missing from manifest. please contact your platform operator"; exit -1}),
	# if the destination volume for snapmirror is different than where we created it (common for enterprises practicing good HA)
	[string] $snapmirrorvserver = $vserver,
	[string] $snapmirrorendpoint = $endpoint,
	[string] $snapmirroraggregateName = $aggregateName,
	[string] $snapmirrorjunctionpath=  $junctionPath,
    [string] $snaptype = $(if($enableSnapMirror){ throw "-snaptype is missing from manifest. please contact your platform operator"; exit -1})
    )

# create powershell credential
try
{
$secPassword = ConvertTo-SecureString $password -AsPlainText -Force
$netappCreds = New-Object System.Management.Automation.PSCredential($username, $secPassword)
# connect to storage controller
Connect-NcController -Name $endpoint -Credential $netappCreds -VServer $vserver
}
catch [system.exception]
{
	throw "Unable to connect to the VServer specified. This probably happened due to: \n - Incorrect Username and/or Password \n - DNS Lookup failure or Incorrect Endpoint"
	exit 2
}

try
{
# ok, we should be in
$rootpath = $junctionPath + "/" + $volName
New-NcVol -Name $volName -Aggregate $aggregateName -Size $size -JunctionPath $rootPath
Get-NcVol
}
catch [system.exception]
{
    throw "Error during the creation of the volume. This probably happened due to: \n - Configuration issue on filer, or use of reserved words. \n - Network packet loss or timeout during creation \n - Invalid aggregate, vserver, junction path, or endpoint (check with your storage administrator) \n - Disks/Aggregates are full."
	exit 2
}


# now that we have the volume, let's create the share, based on the options
if($protocol = "nfs")
{
	try
	{
    Add-NcNfsExport -Path $junctionPath  -ReadWrite all-hosts -NoSuid -SecurityFlavors sys
	}
	catch [system.exception]
	{
	 throw "Error during NFS share provisioning. Your volume is still provisioned, but due to a configuration error you must set up the NFS share manually."
	 exit 1
	}
}
elseif($protocol = "cifs")
{
	try
	{
    Add-NcCifsShare -Name $volName -Path $junctionPath
	}
	catch [system.exception]
	{
	 throw "Error during CIFS share provisioning. Your volume is still provisioned, but due to a configuration error you must set up the CIFS share manually."
	 exit 1
	}
}
else
{
	throw "Error during share provisioning. A type was not specified during the provisioning, so you will not have access to this volume until a protocol is assigned."
	exit 1
}

# if snapmirror is enabled, then we will create the secondary volume at the appropriate destination and set up the snapmirror policy and schedule
# again, this stuff happens away from the developer - which is good
if($enableSnapMirror)
{
	try
	{
    if($snapmirrorendpoint = $endpoint)
	{
		if($snapmirrorvserver = $vserver)
		{
            $destvol = $volName + "mirror"
            $mirrorsource = "//"+$vserver+"/"+$volName
            $mirrordest = "//"+$vserver+"/"+$destvol
			New-NcVol -Name $destvol -Aggregate $snapmirroraggregateName -Size $size -JunctionPath $null -Type dp
            New-NcSnapmirror -DestinationVserver $vserver -DestinationVolume $destvol -SourceVserver $vserver -SourceVolume $volName -Type $snaptype
            if($snaptype = "dp")
            {
                Invoke-NcSnapmirrorInitialize -DestinationVserver $vserver -DestinationVolume $destvol -SourceVserver $vserver -SourceVolume $volName
            }
            elseif($snaptype = "ls")
            {
                Invoke-NcSnapmirrorLsInitialize -SourceVserver $vserver -SourceVolume $volName
            }
		}
		else
		{
			Connect-NcController -Name $endpoint -Credential $netappCreds -VServer $snapmirrorvserver
			New-NcVol -Name $volname"mirror" -Aggregate $snapmirroraggregateName
		}
	}
	else
	# vserver and endpoint are different
	{
		Connect-NcController -Name $snapmirrorendpoint -Credential $netappCreds -VServer $snapmirrorvserver
		New-NcVol -Name '$volname' -Aggregate $snapmirroraggregateName
	}

	} catch [system.exception]
    {
        #rollback on failure - we were using this to troubleshoot New-NcSnapMirror
        # remove the destination
        Set-NcVol -Offline $destvol
        Remove-NcVol -Name $destvol
        # now remove the source
        Dismount-NcVol $volName
        Set-NcVol -Offline $volName
        Remove-NcVol -Name $volName
        Write-Error $Error + "\n" + $StackTrace
    }
}

exit 0