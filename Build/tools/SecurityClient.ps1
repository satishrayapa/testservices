Param(
    [string]$url,
    [string]$scopes,
    [string]$clientId,
    [string]$clientPassword,
    [string]$userProfileId,
    [string]$endpoint,
    [string]$setClientId,
    [string]$setClientPassword,
    [string]$setClientType,
    [string]$setClientScopes
)

$clientInfoEndpoint = "$endpoint/v1/ClientInfos"

Import-Module "$PSScriptRoot\GetTokenClient.psm1"

$result = GetSecurityToken $url $scopes $clientId $clientPassword $userProfileId

if ($result.IsError -eq $true){

    Write-Output "Error has occured"
    Write-Output $result.Error
    Write-Output $result.ErrorDescription
    Write-Output $result.ErrorType

}else {

    $accessToken=$result.AccessToken
    
    $restResult = Invoke-RestMethod $clientInfoEndpoint -ContentType "application/json" -Headers @{ Authorization = "Bearer $accessToken" } -Method Get
    
    $conditions = {$_.username -eq "$setClientId" }
    $exist = $restResult | Where-Object $conditions
    $outFile = $PSScriptRoot + "\client.template.out.json"
    $inFile = $PSScriptRoot + "\client.template.json" 
    $clientDto = Get-Content $inFile | Out-String | ConvertFrom-Json
                     
    if ($exist) {
        $clientEndpoint = "$endpoint/v1/Clients/$setClientId"
        $method = "Put"
    }else{
        $clientEndpoint = "$endpoint/v1/Clients"        
        $method = "Post"        
    }

    $clientDto.username = $setClientId
    $clientDto.password = $setClientPassword
    
    if ($setClientType -eq "ApplicationService"){
        $clientDto.clientType = "0"
    }

    if ($setClientType -eq "ServiceToService"){
        $clientDto.clientType = "1"
    }

    if ($clientDto.clientType -eq "") {
        throw "Invalid clientType."
    }

    if ($setClientScopes -eq $null) {
        throw "Invalid client scopes."
    }

    $setClientScopesList = $setClientScopes -split ' '
    
    if ($setClientScopesList.Count -gt 0) {
        
        $outItems = New-Object System.Collections.Generic.List[System.Object]
        foreach ($item in $setClientScopesList) {

            $allowedScope = New-Object -TypeName PSObject -Property @{ 
                name = "$item" 
                description = "$item" 
            }

            $outItems.Add($allowedScope)
        }
        $clientDto.allowedScopes = $outItems
    }
        
    Set-Content -Path $outFile -Value ($clientDto | ConvertTo-Json -Depth 4)
    Invoke-RestMethod $clientEndpoint -InFile $outFile -ContentType "application/json" -Headers @{ Authorization = "Bearer $accessToken" } -Method $method
}
