# Ok, let's connect to our filer
param(
    [string] $username = $(throw "-username is required"),
    [string] $password = $(throw "-password is required"),
	[string] $vserver = $(throw "-vserver is required"),
    [string] $endpoint = $(throw "-endpoint is required"),
    [string] $volName = $(throw "-volName is required"),
    [string] $aggregateName = $(throw "-aggregateName is required"),
    [string] $junctionPath = $(throw "-junctionPath is required"),
    [string] $size = $(throw "-size is required"),
    [string] $protocol = $(throw "-protocol is required")
	[bool] $enableSnapMirror = $(throw "-enableSnapMirror is required"),
	[string] $snapmirrorpolicyname = if($enableSnapMirror) $(throw "-snapmirrorpolicyname missing from manifest. please contact your platform operator"),
	[string] $snapmirrorschedule = if($enableSnapMirror) $(throw "-snapmirrorschedule is required"),
	# if the destination volume for snapmirror is different than where we created it (common for enterprises practicing good HA)
	[string] $snapmirrorvserver = $vserver,
	[string] $snapmirrorendpoint = $endpoint,
	[string] $snapmirroraggregateName = $aggregateName,
	[string] $snapmirrorjunctionpaht=  $junctionPath,
	[bool] $enableVaults = $(throw "-enableVaults is required")
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
	exit 1
}

try
{
# ok, we should be in
New-NcVol -Name $volName -Aggregate $aggregateName -Size $size -JunctionPath $junctionpath
Get-NcVol
}
catch [system.exception]
{
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
	 exit 3
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
	 exit 4
	}
}
else
{
	exit 5
}

# if snapmirror is enabled, then we will create the secondary volume at the appropriate destination and set up the snapmirror policy and schedule
# again, this stuff happens away from the developer - which is good
if($enableSnapMirror)
{
    if($snapmirrordestinationendpoint = $endpoint)
	{
		if($snapmirrordestinationserver = $vserver)
		{
			New-NcVol -Name $volname+"_mirror" -Aggregate $snapmirroraggregateName -Size $size -JunctionPath $junctionPath+"_mirror"
		}
		else
		{
			Connect-NcController -Name $endpoint -Credential $netappCreds -VServer $snapmirrordestinationserver
			New-NcVol -Name $volname+"_mirror" -Aggregate $aggregateName
		}
	}
	else
}

if($enableVaults)
{


}
# if we get here, then everything went successfully.
exit 0
