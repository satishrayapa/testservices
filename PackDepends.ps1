param($Version, $LocalNugetDirectory, $Project, $DestNugetDirectory)

if (!$DestNugetDirectory){
  Write-Host "Setting destination as local nuget directory."
  $DestNugetDirectory = $LocalNugetDirectory
}

If(!(Test-Path $LocalNugetDirectory))
{
  Write-Host "Creating local nuget dir: $LocalNugetDirectory"
  New-Item -ItemType Directory -Force -Path $LocalNugetDirectory
}

If(!(Test-Path $DestNugetDirectory))
{
  Write-Host "Creating local dest nuget dir: $DestNugetDirectory"
  New-Item -ItemType Directory -Force -Path $DestNugetDirectory
}

Write-Host "Processing $project"
Push-Location $project
$result = dotnet restore -s http://api.nuget.org/v3/index.json -s $LocalNugetDirectory *>&1

if($LASTEXITCODE -ne 0)
{
	# Failed, you can reconstruct stderr strings with:
	$ErrorString = $result -join [System.Environment]::NewLine
	throw $ErrorString
} else {
	Write-Host $result
}

$result = dotnet pack -c Release /p:Version=$Version -o $DestNugetDirectory *>&1
    
if($LASTEXITCODE -ne 0)
{
	# Failed, you can reconstruct stderr strings with:
	$ErrorString = $result -join [System.Environment]::NewLine
	throw $ErrorString
} else {
	Write-Host $result
}
	
Pop-Location
