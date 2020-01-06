param($DestinationDirectory, $ContainerName, $StorageName, $BlobPrefix, $ResourceGroup)

If(!(Test-Path $DestinationDirectory))
{
  Write-Host "Creating local destination dir: $DestinationDirectory"
  New-Item -ItemType Directory -Force -Path $DestinationDirectory
}

Write-Host "Connecting to Azure Blob Storage: $StorageName"
$storageAccount = Get-AzStorageAccount -ResourceGroupName $ResourceGroup -Name $StorageName
$ctx = $storageAccount.Context

Write-Host "Downloading blob list from: $ContainerName"

$names = Get-AzStorageBlob -Container $ContainerName -Context $ctx -Prefix $BlobPrefix | select Name
$names | foreach {
  $name = $_.Name
  Write-Host "Downloading $name"
  Get-AzStorageBlobContent -Blob $name `
  -Container $ContainerName `
  -Destination $DestinationDirectory `
  -Context $ctx

  Write-Host "Copy file to destination directory."
  Copy-Item -Path $DestinationDirectory\$name -Destination $DestinationDirectory
}

# Write all items in destination directory.
Get-Item "$DestinationDirectory\*"