
param(
     [Parameter(Mandatory=$true)]    [string] $AumentumAppRootPath
    ,[Parameter(Mandatory=$true)]    [string] $AspNetCoreEnvVariable
    ,[Parameter(Mandatory=$true)]    [string] $SecurityURI
    ,[Parameter(Mandatory=$true)]    [string] $ResourceLocatorURI
    ,[Parameter(Mandatory=$false)]   [string] $ResourceLocatorExternalURI
    ,[Parameter(Mandatory=$true)]    [string] $ClientScope
    ,[Parameter(Mandatory=$false)]   [Switch] $UpdateWebConnectionString
    ,[Parameter(Mandatory=$false)]   [string] $ConnectionString
    ,[Parameter(Mandatory=$false)]   [Switch] $EnableSsl
    ,[Parameter(Mandatory=$false)]   [string] $WebSiteName = "Default Web Site"
    ,[Parameter(Mandatory=$false)]   [string] $AumentumSiteName = "Aumentum"
)

Import-Module $PSScriptRoot\lib\ConfigurationFileHelper.psm1

Write-Host "Updating web.config ..."
if($EnableSsl.IsPresent){
    $sslEnabled = $true

}else{
    $sslEnabled = $false
}


try{

    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "Common.ResourceLocator.Uri" -value "$ResourceLocatorURI"

    if(![string]::IsNullOrEmpty($ResourceLocatorExternalURI)){
        AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "Common.ResourceLocator.ExternalUri" -value "$ResourceLocatorExternalURI"
    }
    else{
        AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "Common.ResourceLocator.ExternalUri" -value "$ResourceLocatorURI"
    }
    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "Common.ResourceLocator.Partition" -value "$AspNetCoreEnvVariable"
    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "TAGov.Common.Security.Authority" -value "$($SecurityURI.ToLower())"
    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "TAGov.Common.Security.ClientScope" -value "$ClientScope"
    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "TAGov.Common.Security.DisableRequireHttps" -value "$(-not $sslEnabled)"
    AddOrUpdateWebConfigAppSetting -ConfigFilePath $AumentumAppRootPath -key "UseHTTPS" -value "$sslEnabled"

    if($UpdateWebConnectionString.IsPresent){
        AddOrUpdateWebConfigConnectionString -ConfigFilePath $AumentumAppRootPath -key "GRMDB" -value $ConnectionString
    }

    ## Configure URL Rewriting
    try{
        if($AumentumSiteName -ne ""){
            $finalSiteName = "IIS:\Sites\$WebSiteName\$AumentumSiteName"
        }
        else {
            $finalSiteName = "IIS:\Sites\$WebSiteName"
        }

        Set-WebconfigurationProperty '/system.webserver/rewrite/rules/rule[@name="Redirect to https"]' -Name enabled -Value $sslEnabled -PSPath $finalSiteName
    }
    catch{
        Write-Host "Unable to update rewrite rules."
        throw
    }

    Write-Host "Done updating web.config."  -ForegroundColor Green
}
catch{
    Write-Host "Failed updating web.config"  -ForegroundColor Red
    throw
}
