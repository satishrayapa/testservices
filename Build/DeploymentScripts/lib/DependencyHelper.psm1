

function CheckIfVersionIsGreaterThan{
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$SourceVersion
       ,[Parameter(Mandatory=$true)] [string]$TargetVersion
   )

    $isGreaterOrEqual = [System.Version]$sourceVersion -ge [System.Version]$targetVersion

    return $isGreaterOrEqual
}

function CheckIfVersionArrayIsGreaterThan{
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$SourceVersion
       ,[Parameter(Mandatory=$true)] [string[]]$TargetVersion
    )
    $isGreaterOrEqual= false;
    $isGreater = CheckIfVersionIsGreaterThan -sourceVersion $sourceVersion -targetVersion $targetVersion

    if($isGreater -eq $true){
        $isGreaterOrEqual = $true
    }

    return $isGreaterOrEqual
}


function GetInstalledDotNetCoreWindowsServerHostingVersion{
    [CmdletBinding()]

    $installedVersions = New-Object System.Collections.ArrayList
    $DotNETCoreUpdatesPath = "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Updates\.NET Core"
    $DotNetCoreItems = Get-Item -ErrorAction Stop -Path $DotNETCoreUpdatesPath
    $DotNetCoreItems.GetSubKeyNames() | Where-Object { $_ -Match "Microsoft .NET Core.*Windows Server Hosting" } | ForEach-Object {
        $version= $_.ToString().Replace("Microsoft .NET Core ", "").Replace(" - Windows Server Hosting", "").TrimEnd().TrimStart()
        $installedVersions += $version
    }
    return $installedVersions
}

function GetInstalledDotNetCoreVersion{
    [CmdletBinding()]

    $installedVersions = New-Object System.Collections.ArrayList

    if(Test-Path "$env:programfiles/dotnet/"){
        try{
            foreach($element in (Get-ChildItem "$env:programfiles/dotnet/shared/Microsoft.NETCore.App").Name){
                $element
            }
        }
        catch{
            $errorMessage = $_.Exception.Message
            Write-Host "Something went wrong`r`nError: $errorMessage"
            throw
        }
    }
    else{
        Write-Host 'No SDK installed'
    }

    return $installedVersions
}

