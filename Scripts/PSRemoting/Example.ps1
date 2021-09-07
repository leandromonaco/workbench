#Import Utilities Module
$CurrentFolderLocation = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

Import-Module -Name ($CurrentFolderLocation  + "\Utilities.psm1") -Force -DisableNameChecking

#Backup-File -FileToBackup "C:\File.xml" -BackupName "FileBackup" -BackupFolder "C:\Backup"
#[xml] $ConfigContent = Get-Content "C:\File.xml"
#Add-Node -XmlContent $ConfigContent -NodeToAddLocation "//node[@attribute='value']" -NodeContent "<NewNode>TEST</NewNode>"
#Remove-Node -XmlContent $ConfigContent -NodeToRemoveLocation "//node[@attribute='value2']"
#Save-XmlFile -XmlContent $ConfigContent -XmlLocation "C:\File.xml"

#Create-NugetPackage -SourceFolder 'C:\NugetTest\Source\SomeFolder' -OutputDirectory 'C:\NugetTest\Output' -PackageVersion '1.2.0.0' -Id 'SomePackage'
#Push-NugetPackage -FeedUrl 'http://feed.com/nuget/packages' -ApiKey 'API-ABCDESWBRPO6AXVQQQRDRLE' -PackageLocation 'C:\NugetTest\Output\SomePackage.1.2.0.0.nupkg'
