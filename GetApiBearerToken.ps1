param(
  $KeyVaultClientSecretName, 
  $KeyVaultName,
  $ClientId,
  $AppResourceId,
  $TenantId)

  Write-Host "GET secrets."
  $clientSecretValue = (Get-AzKeyVaultSecret -VaultName $KeyVaultName -Name $KeyVaultClientSecretName).SecretValueText
  $clientSecretEncoded = [System.Web.HttpUtility]::UrlEncode($clientSecretValue)

  $grantType = "client_credentials"

  $body = "grant_type=$grantType&client_id=$ClientId&client_secret=$clientSecretEncoded&resource=$AppResourceId"

  $oauthUri = "https://login.microsoftonline.com/$TenantId/oauth2/token"
  Write-Host "GET bearer token from: $oauthUri"
  $response = Invoke-RestMethod -Uri  $oauthUri -Method POST -Body $body
  $accessToken = $response.access_token
  Write-Host "##vso[task.setvariable variable=ApiBearerToken;isOutput=true]$accessToken"