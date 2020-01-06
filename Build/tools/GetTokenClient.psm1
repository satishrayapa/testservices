Function GetSecurityToken {
    [CmdletBinding()]    
    param(
        [string]$url, 
        [string]$scopes, 
        [string]$clientId, 
        [string]$clientPassword, 
        [string]$userProfileId)

    $srcDirectory = $PSScriptRoot + "\libs\"    

    [Reflection.Assembly]::LoadFile($srcDirectory + "IdentityModel.dll") > $null
    [Reflection.Assembly]::LoadFile($srcDirectory + "System.ValueTuple.dll") > $null
    [Reflection.Assembly]::LoadFile($srcDirectory + "Newtonsoft.Json.dll") > $null
    
    $disco = New-Object IdentityModel.Client.DiscoveryClient $url
    $disco.Policy.RequireHttps = $false

    $cancellationToken = New-Object System.Threading.CancellationToken

    $clientResult = $disco.GetAsync($cancellationToken).GetAwaiter().GetResult()
    $tokenClient = New-Object IdentityModel.Client.TokenClient $clientResult.TokenEndpoint, $clientId, $clientPassword

    If ($clientId.ToLower().StartsWith("service.") -eq $true){
        $tokenResponse = [IdentityModel.Client.TokenClientExtensions]::RequestClientCredentialsAsync($tokenClient, $scopes, $null, $cancellationToken).GetAwaiter().GetResult()
    }
    else {
        $tokenResponse = [IdentityModel.Client.TokenClientExtensions]::RequestResourceOwnerPasswordAsync($tokenClient, $userProfileId, $userProfileId, $scopes, $null, $cancellationToken).GetAwaiter().GetResult()
    }

    return $tokenResponse
}
