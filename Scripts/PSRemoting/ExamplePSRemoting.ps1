##############################################################################################################################

# .\ExamplePSRemoting.ps1 -ServerGroup "GROUP_A"

##############################################################################################################################

Param
(
    [parameter(Mandatory=$true)][string]$ServerGroup = ""
)

#Initialize current script location
$CurrentFolderLocation = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Import-Module -Name ($CurrentFolderLocation + "\Utilities.psm1") -Force -DisableNameChecking

# Installer credentials
$Credentials= Get-Credential

# Get Server Names
[xml] $ServerGroups = Get-Content $CurrentFolderLocation"\Servers.xml"
$ServerGroupNode = $ServerGroups.SelectSingleNode("//ServerGroup[@name='$ServerGroup']")
$ServerGroupXml = [xml] $ServerGroupNode.OuterXml

foreach ($Server in $ServerGroupXml.ServerGroup.Servers.Server)
{
    $ServerName = $Server.name
	
    $LocalInstallationFolder = "c:\Installation_Folder"
    $NetworkInstallationFolder = "\\$ServerName\c$\Installation_Folder"

    $LocalPatchFolder = "$LocalInstallationFolder\Patch_Folder"
    $NetworkPatchFolder = "$NetworkInstallationFolder\Patch_Folder"

    $PatchScriptName = "Example.ps1"
    $ScriptToRun = "$LocalPatchFolder\$PatchScriptName"
    
    #Create remote Installation folder
    Create-RemoteFolder -FolderName $LocalInstallationFolder -ServerName $ServerName -AuthCredentials $Credentials    

    #Copy Package to Server
    Copy-Folder -FolderToCopy $CurrentFolderLocation -TargetFolder $NetworkPatchFolder

    #If deployment server is detected, execute script locally
    if($ServerName -eq "$env:computername")
    {
        $ArgumentList = "-Action ""$Action"""
        Execute-LocalScript -Script $ScriptToRun -Parameters $ArgumentList
    }
    else
    {
        $ArgumentList = "-Action ""$Action"""
        Execute-RemoteScript -ServerName $ServerName -Script $ScriptToRun -Parameters $ArgumentList -AuthCredentials $Credentials
    }
}