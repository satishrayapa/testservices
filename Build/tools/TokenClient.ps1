Param(
    [string]$url,
    [string]$scopes,
    [string]$clientId,
    [string]$clientPassword,
    [string]$userProfileId
)


Import-Module "$PSScriptRoot\GetTokenClient.psm1"

$result = GetSecurityToken $url $scopes $clientId $clientPassword $userProfileId

if ($result.IsError -eq $true){

    Write-Output "Error has occured"
    Write-Output $result.Error
    Write-Output $result.ErrorDescription
    Write-Output $result.ErrorType

}else {
    Write-Output $result.AccessToken
}