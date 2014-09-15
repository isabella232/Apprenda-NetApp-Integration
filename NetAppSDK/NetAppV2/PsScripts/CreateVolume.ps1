# Ok, let's connect to our filer
param(
    [string] $username = $(throw "-username is required"),
    [string] $password = $(throw "-password is required"),
    [string] $endpoint = $(throw "-endpoint is required"),
    [string] $volName = $(throw "-volName is required"),
    [string] $aggregateName = $(throw "-aggregateName is required"),
    [string] $junctionPath = $(throw "-junctionPath is required"),
    [string] $size = $(throw "-size is required")
)


# create powershell credential
$secPassword = ConvertTo-SecureString $password -AsPlainText -Force
$netappCreds = New-Object System.Management.Automation.PSCredential($username, $secPassword)
# connect to storage controller
Connect-NaController -Name $endpoint -Credential $netappCreds

# ok, we should be in
New-NaVol -Name $volName -Aggregate $aggregateName -Size $size