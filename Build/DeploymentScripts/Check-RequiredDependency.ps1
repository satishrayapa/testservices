# Set current dir
Set-location $PSScriptRoot
Import-Module $PSScriptRoot\lib\DependencyHelper.psm1  -force


$versions = GetInstalledDotNetCoreWindowsServerHostingVersion
foreach ($element in $versions) {$element}

Write-Host "Core Framework:"
$versions = GetInstalledDotNetCoreVersion
foreach ($element in $versions) {$element}
