$Global:CurrentLocation = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
$Global:LogFolder = "$Global:CurrentLocation\Logs"
$Global:BackupFolder = "$Global:CurrentLocation\Backup"

Function Create-Folder
{
    param(
           [parameter(Mandatory=$true)][string] $FolderName
         )
    
    #If folder does not exist create it
    if(-not (Test-Path $FolderName))
    {
        New-Item -ItemType directory -Path $FolderName
    }
}

Function Write-Log
{
   param(
          [parameter(Mandatory=$true)][string]$Message
        )

   Create-Folder -FolderName $Global:LogFolder
   $timestamp = Get-Date -Format o | foreach {$_ -replace ":", "-"}
   $Logfile = "$Global:LogFolder\log.txt"
   Add-content $Logfile -value "$timestamp $message"  
}

Function Show-ErrorMessage
{
    param(
           [parameter(Mandatory=$true)][string]$Message
         )

    Write-Host $Message -ForegroundColor Red
    Write-Log $Message
}

Function Show-WarningMessage
{
    param(
           [parameter(Mandatory=$true)][string]$Message
         )

    Write-Host $Message -ForegroundColor Yellow
    Write-Log $Message
}

Function Show-SuccessMessage
{
    param(
           [parameter(Mandatory=$true)][string]$Message
         )

    Write-Host $Message -ForegroundColor Green
    Write-Log $Message
}

Function Show-Message
{
    param(
           [parameter(Mandatory=$true)][string]$Message
         )

    Write-Host $Message
    Write-Log $Message
}

Function Backup-FileRecurse
{

    Param (
            [parameter(Mandatory=$true)][string]$FileName,
            [parameter(Mandatory=$true)][string]$TargetFolder
          )

    $Files = Search-Files -SearchTerm $FileName -Folder $TargetFolder

    foreach($file in $Files)
    {
        $DirectoryName=$file.DirectoryName
        $BackupSubfolder = $DirectoryName.Replace($TargetFolder, $BackupFolder)
        Create-Folder -FolderName $BackupSubfolder
        Copy-File -SourceFileLocation $file.FullName -TargetFileLocation $BackupSubfolder
    }

}

Function Backup-File
{
    Param ([parameter(Mandatory=$true)][string]$FileToBackup,
           [parameter(Mandatory=$true)][string]$BackupName)

    Create-Folder -FolderName $BackupFolder

    #Initialize variables
    $timestamp = Get-Date -Format o | foreach {$_ -replace ":", "-"}

    #Define Backup Filename
    $BackupFile = "$BackupFolder\$BackupName.$timestamp.bak";

    #Backup
    Copy-Item $FileToBackup $BackupFile

    Show-SuccessMessage "Backup file has been created: $BackupFile"
}

Function Replace-File
{

    Param (
            [parameter(Mandatory=$true)][string]$FileName,
            [parameter(Mandatory=$true)][string]$SourceFolder,
            [parameter(Mandatory=$true)][string]$TargetFolder
          )

    $SourceFile = Search-Files -SearchTerm $FileName -Folder $SourceFolder
    $TargetFiles = Search-Files -SearchTerm $FileName -Folder $TargetFolder

    foreach($TargetFile in $TargetFiles)
    {
        Copy-File -SourceFileLocation $SourceFile.FullName -TargetFileLocation $TargetFile
    }

}

Function Validate-FileByVersion
{

    Param (
            [parameter(Mandatory=$true)][string]$FileName,
            [parameter(Mandatory=$true)][string]$TargetFolder,
            [parameter(Mandatory=$true)][string]$ExpectedVersion
          )

    $TargetFiles = Search-Files -SearchTerm $FileName -Folder $TargetFolder

    foreach($TargetFile in $TargetFiles)
    {
        Show-Message -Message $TargetFile
        $FileVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($TargetFile).FileVersion
        Validate-String -ActualValue $FileVersion -ExpectedValue $ExpectedVersion
    }

}

Function Search-Files
{
    Param (
            [parameter(Mandatory=$true)][string]$SearchTerm,
            [parameter(Mandatory=$true)][string]$Folder
           )
    
    return Get-Childitem –Path $Folder -Include $SearchTerm -File -Recurse | Select-Object
}

Function Rollback-FileRecurse
{
    Param (
            [parameter(Mandatory=$true)][string]$FileName,
            [parameter(Mandatory=$true)][string]$TargetFolder
          )

    #Search for all the backup files using $FileName
    $BackupFiles = Get-Childitem –Path $BackupFolder -Include $FileName -File -Recurse | Select-Object

    foreach($BackupFile in $BackupFiles)
    {
        $DirectoryName=$BackupFile.DirectoryName
        $TargetSubfolder = $DirectoryName.Replace($BackupFolder, $TargetFolder)
        Copy-File -SourceFileLocation $BackupFile.FullName -TargetFileLocation $TargetSubfolder
    }
}

Function Rollback-File
{
    Param ([parameter(Mandatory=$true)][string]$FileToRestore,
           [parameter(Mandatory=$true)][string]$BackupName,
           [parameter(Mandatory=$true)][string]$BackupFolder)

    #Search for all the backup files using $BackupName
    $SearchTerm =  "$BackupName*.bak"
    $FirstBackupFile = Get-Childitem –Path $BackupFolder -Include $SearchTerm -File -Recurse | Select-Object -First 1

    #Is there any backup?
    if(($FirstBackupFile.count -gt 0) -and (Test-Path $FirstBackupFile[0]))
    {
        #Yes - Restore the latest backup file
        Show-SuccessMessage "Restoring the latest backup file: $FirstBackupFile";

        Copy-Item $FirstBackupFile[0] $FileToRestore

        Show-SuccessMessage "Restore successful: $FirstBackupFile"
    }
    else
    {
        #No - There is no backup file to restore
        Show-ErrorMessage "$BackupName was not found."
    }
    exit
}

Function Change-AttributeValue
{
    param(
           [parameter(Mandatory=$true)][xml] $XmlContent,
           [parameter(Mandatory=$true)][string] $NodeLocation,
           [parameter(Mandatory=$true)][string]$AttributeName,
           [parameter(Mandatory=$true)][string]$AttributeValue
         )

    if($NodeLocation -eq "")
    {
        Show-ErrorMessage "NodeLocation can't be empty"
        exit
    }

    if($AttributeName -eq "")
    {
        Show-ErrorMessage "AttributeName can't be empty"
        exit
    }
	
	if($AttributeValue -eq "")
    {
        Show-ErrorMessage "AttributeValue can't be empty"
        exit
    }

    $XmlNode = $XmlContent.SelectSingleNode($NodeLocation)

    $XmlNode.SetAttribute($AttributeName, $AttributeValue);

    Show-SuccessMessage "Attribute $AttributeName has been set to $AttributeValue"

}

Function Change-TextValue
{
    param(
           [parameter(Mandatory=$true)][string] $NodeLocation,
           [parameter(Mandatory=$true)][string]$TextValue
         )

    if($NodeLocation -eq "")
    {
        Show-ErrorMessage "NodeLocation can't be empty"
        exit
    }

    $XmlNode = $XmlToPatch.SelectSingleNode($NodeLocation)

    $OldText = $XmlNode.'#text';
    $XmlNode.'#text' = $TextValue;

    Show-SuccessMessage "Value $OldText has been replaced with $TextValue"
}

Function Remove-Node
{
    param(
           [parameter(Mandatory=$true)][xml] $XmlContent,
           [parameter(Mandatory=$true)][string]$NodeToRemoveLocation
         )

    $NodeToRemove = $XmlContent.SelectSingleNode($NodeToRemoveLocation)
    $NodeToRemoveParent = $NodeToRemove.ParentNode
    $NodeToRemoveParent.RemoveChild($NodeToRemove)
}

Function Add-Node
{
    param(
           [xml] $XmlContent,
           [string]$NodeToAddLocation,
           [string]$NodeContent
         )

    $ParentNodeForAddition = $XmlContent.SelectSingleNode($NodeToAddLocation)
    $XmlNodeContentToAdd = [xml] $NodeContent
    $nodeToAddImported = $XmlContent.ImportNode($XmlNodeContentToAdd.DocumentElement, $true)
    $ParentNodeForAddition.AppendChild($nodeToAddImported)
}

Function Read-Node
{
    param(
           [xml] $XmlContent,
           [string]$NodeLocation
         )

    $XmlNode = $XmlContent.SelectSingleNode($NodeLocation)
	return $XmlNode.OuterXml
}

Function Save-XmlFile
{
    param(
           [xml]$XmlContent,
           [string]$XmlLocation
         )
    $XmlContent.Save($XmlLocation);
    Show-SuccessMessage "File saved successfully $XmlLocation"
}

Function Copy-File
{
    Param([string]$SourceFileLocation, 
          [string]$TargetFileLocation) 
    
	Copy-Item $SourceFileLocation -Destination $TargetFileLocation -Recurse -Force
}

Function Copy-RemoteFolder
{

    param(
           [parameter(Mandatory=$true)][string] $FolderToCopy,
           [parameter(Mandatory=$true)][string] $TargetFolder,
           [parameter(Mandatory=$true)][string] $ServerName,
           [parameter(Mandatory=$true)][System.Management.Automation.PSCredential] $AuthCredentials
         )

    Invoke-Command -ComputerName $ServerName -authentication credssp -credential $AuthCredentials -scriptblock {
			
			param([String]$FolderToCopy,
                  [String]$TargetFolder)
		
            if(-not (Test-Path $TargetFolder))
            {
			    Copy-Item -Path $FolderToCopy -Destination $TargetFolder -Recurse 
            }

    } -ArgumentList $FolderToCopy, $TargetFolder
}

Function Copy-Folder
{

    param(
           [parameter(Mandatory=$true)][string] $FolderToCopy,
           [parameter(Mandatory=$true)][string] $TargetFolder
         )

        if(-not (Test-Path $TargetFolder))
        {
			Copy-Item -Path $FolderToCopy -Destination $TargetFolder -Recurse 
        }
}

Function Validate-String
{
    param(
           [string] $ActualValue,
           [string] $ExpectedValue
         )

    if($ActualValue -ne $ExpectedValue) 
    {
        Show-ErrorMessage -Message "Validation Failed"
        Show-ErrorMessage -Message "ActualValue: $ActualValue"
        Show-ErrorMessage -Message "ExpectedValue:$ExpectedValue"
    }
    else
    {
        Show-SuccessMessage -Message "Validation Sucessful"
        Show-SuccessMessage -Message "ActualValue: $ActualValue"
        Show-SuccessMessage -Message "ExpectedValue:$ExpectedValue"
    }
}

Function Execute-LocalScript
{
	    param(
           [parameter(Mandatory=$true)][string] $Script,
           [parameter(Mandatory=$true)][string] $Parameters
         )

	Invoke-Expression "$Script $Parameters"
}

Function Execute-RemoteScript
{
	    param(
           [parameter(Mandatory=$true)][string] $ServerName,
		   [parameter(Mandatory=$true)][string] $Script,
           [parameter(Mandatory=$true)][string] $Parameters,
           [parameter(Mandatory=$true)][System.Management.Automation.PSCredential] $AuthCredentials
         )

	    Invoke-Command -ComputerName $ServerName -authentication credssp -credential $AuthCredentials -scriptblock {
				
        param([String]$Script,
              [String]$Parameters)

        Invoke-Expression "$Script $Parameters"

        } -ArgumentList $Script, $Parameters
}

#Creates a NUPKG from files on disk, without needing a .nuspec
Function Create-NugetPackage
{

	 param(
           [parameter(Mandatory=$true)][string] $SourceFolder,
		   [parameter(Mandatory=$true)][string] $OutputDirectory,
           [parameter(Mandatory=$true)][string] $PackageVersion,
		   [parameter(Mandatory=$true)][string] $Id
         )

	$octoTool = "$Global:CurrentLocation\Tools\Octo.Exe"

	#https://octopus.com/docs/api-and-integration/octo.exe-command-line
	Invoke-Expression "$octoTool pack --id=$Id --basePath=$SourceFolder --outFolder=$OutputDirectory --version=$PackageVersion --overwrite" 
}

#Push Nuget packages to feed
Function Push-NugetPackage
{
	param(
           [parameter(Mandatory=$true)][string] $FeedUrl,
		   [parameter(Mandatory=$true)][string] $ApiKey,
           [parameter(Mandatory=$true)][string] $PackageLocation
         )

	$nugetTool = "$Global:CurrentLocation\Tools\NuGet.Exe"

	#https://docs.microsoft.com/en-us/nuget/tools/cli-ref-push
	Invoke-Expression "$nugetTool push $PackageLocation -ApiKey $ApiKey -Source $FeedUrl"
}

Function Stop-IIS
{
	iisreset /stop
}

Function Start-IIS
{
	iisreset /start
}

Function Stop-WindowsService
{
    param(
           [parameter(Mandatory=$true)][string] $DisplayName
	     )

	Stop-Service -DisplayName  $DisplayName
}

Function Start-WindowsService
{
    param(
           [parameter(Mandatory=$true)][string] $ServiceName
	     )

	Start-Service -DisplayName  $ServiceName
}
