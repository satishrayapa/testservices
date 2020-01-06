param(
  $SourceDirectory,
  $DestinationDirectory, 
  $ArchiveFile)

  If(!(Test-Path $DestinationDirectory))
  {
    Write-Host "Creating destination dir: $DestinationDirectory"
    New-Item -ItemType Directory -Force -Path $DestinationDirectory
  }

  Write-Host "Processing services zip files"

  $servicesZip = Get-ChildItem -Path $SourceDirectory -Name -File

  $servicesZip | ForEach-Object {
    $file = $_
    Write-Host "Processing $file"
    $destFolder = $file.Replace(".zip","")

    $destFolderPath = "$DestinationDirectory\$destFolder"

    If(!(Test-Path $destFolderPath))
    {
      Write-Host "Creating dir: $destFolderPath"
      New-Item -ItemType Directory -Force -Path $destFolderPath
    }

    Expand-Archive -LiteralPath "$SourceDirectory\$file" -DestinationPath $destFolderPath

    Get-ChildItem -Path $destFolderPath -Name
  }

  Write-Host "Creating zip file ArchiveFile"  
  Compress-Archive -Path "$DestinationDirectory\*" -DestinationPath $ArchiveFile