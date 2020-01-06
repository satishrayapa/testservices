param(
  $StackName,
  [array]$SiteNames,
  $SubscriptionKey,
  $SubscriptionId,
  $TagApplicationAssetInsightId,
  $TagEnvironmentType,
  $TagFinancialIdentifier,
  $ResourceGroup,
  $ApiBearerToken,
  $Region)

$headers = @{
  "Ocp-Apim-Subscription-Key" = $SubscriptionKey;
  "Authorization"             = "Bearer $ApiBearerToken";
  "Content-Type"              = "application/json";
}

ForEach($SiteName in $SiteNames){
$subnetName = "$Region-$StackName$SiteName-backend"
$url = "https://azure-landing-zone.azure-api.net/inflation-services/subscriptions/$SubscriptionId/subnets/$subnetName"

Write-Host "GET subnet from: $url"

try {
  $response = Invoke-RestMethod -Uri $url -Method GET -Headers $headers
  Write-Host "Response: $response"
}
catch {
    
  $statusCode = $_.Exception.Response.StatusCode.value__

  $result = $_.Exception.Response.GetResponseStream()
  $reader = New-Object System.IO.StreamReader($result)
  $reader.BaseStream.Position = 0
  $reader.DiscardBufferedData()
  $responseBody = $reader.ReadToEnd();

  Write-Host "Status code: $statusCode Response: $responseBody"
}

if ($statusCode -eq 400 -and $responseBody.Contains("does not exist")) {
      
  $body = @{
    "subnetName"       = "$StackName$SiteName";
    "subnetType"       = "backend";
    "addressBlockSize" = 31;
    "region"           = "$Region";
    "resourceGroups"   = @( $ResourceGroup );
    "environment"      = "$TagEnvironmentType";
    "assetId"          = "$TagApplicationAssetInsightId";
    "financialId"      = "$TagFinancialIdentifier";
  } | ConvertTo-Json -Depth 3

  Write-Host "Request: $body"

  $createUri = "https://azure-landing-zone.azure-api.net/inflation-services/subscriptions/$SubscriptionId/subnets"
  try {
    $response = Invoke-RestMethod -Uri $createUri -Method POST -Headers $headers -Body $body
    Write-Host "Response: $response"
  }
  catch {
    $statusCode = $_.Exception.Response.StatusCode.value__

    $result = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($result)
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd();
    
    Write-Host "Status code: $statusCode Response: $responseBody"
  }
}
if (($statusCode -eq 500 -and $responseBody.Contains("Microsoft.IdentityModel.Clients.ActiveDirectory"))) {
  Write-Host "CCOE API returned an error but the subnet was created successfully. Moving on..."
}
else {
  Write-Host "Successful call"
}
}