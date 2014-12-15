# Cretes a SnapMirror based off of a volume

param
(
	[string] $username = $(throw "-username is required"),
    [string] $password = $(throw "-password is required"),
	[string] $vserver = $(throw "-vserver is required"),
    [string] $endpoint = $(throw "-endpoint is required"),
    [string] $volName = $(throw "-volName is required"),
	[bool] $enableSnapMirror = $(throw "-enableSnapMirror is required"),
	[string] $snapmirrorpolicyname = if($enableSnapMirror) $(throw "-snapmirrorpolicyname missing from manifest. please contact your platform operator"),
	[string] $snapmirrorschedule = if($enableSnapMirror) $(throw "-snapmirrorschedule is required"),
	[string] $snapmirrordestinationserver = $vserver,
	[string] $snapmirrordestinationendpoint = $endpoint
)


