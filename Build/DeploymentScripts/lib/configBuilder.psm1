Function BuildConfigurations {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)]   [hashtable]$configs
    )

    ## Config wheter the services should be installed as Primary or Secondary
    if($configs.Get_Item("ServerRole").ToLower() -eq "primary"){
        $configs.Add("IsPrimary", $true);
    }
    else{
        $configs.Add("IsPrimary", $false);
    }

    ## Configs setup
    $configs.Add("clientScope", "api.common.resourcelocator api.service.basevaluesegment api.service.assessmentevent api.common.resourcelocator api.facade.assessmentheader api.facade.basevaluesegment api.service.grmevent api.service.legalparty api.service.revenueobject api.service.legalpartysearch")
    $configs.Add("legalPartySearchFolder", "Process.Sync.LegalPartySearch")
    $configs.Add("legalPartySearchExecutable", "TAGov.Process.Sync.LegalPartySearch.exe")
    $configs.Add("legalPartySearchCoordinatorFolder", "Process.Sync.LegalPartySearch.Coordinator")
    $configs.Add("legalPartySearchCoordinatorExecutable", "TAGov.Process.Sync.LegalPartySearch.Coordinator.exe")

    # Default AspNetCoreEnvVariable to prod if not provided
    if($configs.Get_Item("AspNetCoreEnvVariable") -eq ""){
        $configs.Set_Item("AspNetCoreEnvVariable", "prod")
    }

    # Configure protocol
    If ($configs.Get_Item("UseHTTP")) {
        $configs.Add("protocol", "http")
        $configs.Add("enableSSL", $false)
    }else{
        $configs.Add("protocol", "https")
        $configs.Add("enableSSL", $true)
    }

    # Get public DNS Name is switch is enabled and force ToLower otherwise there is issues with Security endpoint
    if($configs.Get_Item("EnableDNSResolution")){
        $configs.Add("ServerDNSName", [Net.DNS]::GetHostEntry($configs.Get_Item("ServerName")).hostname.ToLower())
    }
    else{
        $configs.Add("ServerDNSName", $configs.Get_Item("ServerName").ToLower())
    }

    # Resource locator
    if($configs.Get_Item("UseLocalResourceLocator")){
        $configs.Add("resourceLocatorServername", "localhost")
    }
    else{
        $configs.Add("resourceLocatorServername", $configs.Get_Item("ServerDNSName"))
    }

    if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
        if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriUseHTTPS"))){
            if($configs.Get_Item("ExternalUriUseHTTPS")){
                $configs.Add("externaluriprotocol", "https")
            }
            else{
                $configs.Add("externaluriprotocol", "http")
            }
        }
        else{
            $configs.Add("externaluriprotocol", "http")
        }
    }

    # Configure service nesting
    if(![string]::IsNullOrEmpty( $configs.Get_Item("AumentumSiteName") )){
        $configs.Add("SecurityURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("ServerDNSName") + "/" + $configs.Get_Item("AumentumSiteName") + "/common.security")
        $configs.Add("ResourceLocatorURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("ServerDNSName") + "/" + $configs.Get_Item("AumentumSiteName") + "/common.resourcelocator")
        if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
            $configs.Add("ResourceLocatorExternalURI", $configs.Get_Item("externaluriprotocol") + "://" + $configs.Get_Item("ExternalUriDNSName") + "/" + $configs.Get_Item("AumentumSiteName") + "/common.resourcelocator")
        }
        $configs.Add("BypassedResourceLocatorURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("resourceLocatorServername") + "/" + $configs.Get_Item("AumentumSiteName") + "/common.resourcelocator")
        $configs.Add("AppPoolName", $configs.Get_Item("AumentumSiteName") + "ServicePool")
    }
    else{
        $configs.Add("SecurityURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("ServerDNSName") + "/common.security")
        $configs.Add("ResourceLocatorURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("ServerDNSName") + "/common.resourcelocator")
        if(![string]::IsNullOrEmpty($configs.Get_Item("ExternalUriDNSName"))){
            $configs.Add("ResourceLocatorExternalURI", $configs.Get_Item("externaluriprotocol") + "://" + $configs.Get_Item("ExternalUriDNSName") + "/common.resourcelocator")
        }
        $configs.Add("BypassedResourceLocatorURI", $configs.Get_Item("protocol") + "://" + $configs.Get_Item("resourceLocatorServername") + "/common.resourcelocator")
        $configs.Add("AppPoolName", $configs.Get_Item("WebSiteName") + "ServicePool")
    }

    # Configure workflow URL
    if(![string]::IsNullOrEmpty( $configs.Get_Item("aumentumWorkflowURL")) ){
        if( $configs.Get_Item("EnableDNSResolution")){
            $configs.Set_Item("aumentumWorkflowURL", $configs.Get_Item("aumentumWorkflowURL").Replace($configs.Get_Item("ServerName"), $configs.Get_Item("ServerDNSName")))
        }
    }

    return $configs
}
