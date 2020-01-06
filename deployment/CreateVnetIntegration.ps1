#
param(
    $ProjectName, 
    $SubscriptionKey,
    $SubscriptionId,
    $ResourceGroup,
    $ApiBearerToken,
    $Region)

$headers = @{
    "Ocp-Apim-Subscription-Key" = $SubscriptionKey;
    "Authorization"             = "Bearer $ApiBearerToken";
    "Content-Type"              = "application/json";
}

$subnetName = "$Region-$ProjectName-backend"

$baseUrl = "https://azure-landing-zone.azure-api.net/vnetintegration"
$url = "$baseUrl/subscriptions/$SubscriptionId/resourcegroups/$ResourceGroup/vnetintegration/$ProjectName"

$body = @{
    "subnetname" = "$subnetName";
} | ConvertTo-Json -Depth 3

Write-Host "Url: $url Request: $body"

try {
    $response = Invoke-RestMethod -Uri $url -Method POST -Headers $headers -Body $body
    Write-Host "Response: $response"

    $taskId = $response.TaskId
    $url = "$baseUrl/tasks/$taskId"
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
    
    throw $_
}
  