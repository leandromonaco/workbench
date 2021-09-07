#Install Build Tools
#https://docs.microsoft.com/en-us/visualstudio/install/command-line-parameter-examples?view=vs-2019
$expression = "C:\Installation\vs_buildtools_2019.exe --quiet --norestart --wait --config ""C:\Installation\vs_buildtools_2019_workload.vsconfig"""
Invoke-Expression -Command $expression

#Install NodeJS
$expression = "msiexec /passive /log ""C:\logs\node_installation.log"" /package ""C:\Installation\node-v13.7.0-x64.msi"""
Invoke-Expression -Command $expression

#Import Windows Roles and Features
Import-Csv C:\Installation\RolesAndFeatures.csv | foreach{Add-WindowsFeature $_.name  }