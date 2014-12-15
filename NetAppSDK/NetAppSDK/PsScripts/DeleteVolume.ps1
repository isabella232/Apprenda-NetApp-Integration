param(
    [string] $username = $(throw "-username is required"),
    [string] $password = $(throw "-password is required"),
	[string] $vserver = $(throw "-vserver is required"),
    [string] $endpoint = $(throw "-endpoint is required"),
    [string] $volName = $(throw "-volName is required")
)
# create powershell credential
$secPassword = ConvertTo-SecureString $password -AsPlainText -Force
$netappCreds = New-Object System.Management.Automation.PSCredential($username, $secPassword)
# connect to storage controller
Connect-NcController -Name $endpoint -Credential $netappCreds -VServer $vserver

# ok, we should be in
Dismount-NcVol $volName
Set-NcVol -Offline $volName
Remove-NcVol -Name $volName
Get-NcVol