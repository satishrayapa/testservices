param($OutputDirectory, $ZipFilePath, $LocalNugetDirectory, $ModelNugetDirectory, $ProjectPath, $ProjectName, 
  $SignFile, $Version, $BuildOnly)

function AssertLastExitCode {
  param($result)

  if($LASTEXITCODE -ne 0)
  {
	  # Failed, you can reconstruct stderr strings with:
	  $ErrorString = $result -join [System.Environment]::NewLine
	  throw $ErrorString
  } else {
	  Write-Host $result
  }
}

function RestoreProject {
  param($path, $localNugetDirectory, [switch]$selfContained)
    Push-Location $path

    if ($selfContained) {
      $result = dotnet restore -s http://api.nuget.org/v3/index.json -s $localNugetDirectory --runtime win-x64
    }
    else {
      $result = dotnet restore -s http://api.nuget.org/v3/index.json -s $localNugetDirectory
    }
    AssertLastExitCode -result $result

    Pop-Location
}

function BuildProject {
  param($path, $outputDirectory, $localNugetDirectory, $Version, [switch]$selfContained)

  if ($selfContained) {
    RestoreProject -path $path -localNugetDirectory $localNugetDirectory -selfContained
  } else {
    RestoreProject -path $path -localNugetDirectory $localNugetDirectory
  }
  
  Push-Location $path

  if ($selfContained) {
    $result = dotnet publish -c Release -o $outputDirectory --no-restore --framework netcoreapp2.2 --self-contained --runtime win-x64 /p:AssemblyVersion=$Version
  } else {
    $result = dotnet publish -c Release -o $outputDirectory --no-restore /p:AssemblyVersion=$Version
  }
  
  AssertLastExitCode -result $result

  Pop-Location
}

Write-Host "Processing $ProjectName"

$parts = $ProjectPath.Split("\")
$projectDir = $parts[0]
$testPath = "$projectDir\Domain.Tests"

Write-Host "Testing if path exist: $testPath"
if (Test-Path $testPath) 
{
  Write-Host "Restoring test project."
  RestoreProject -path $testPath -LocalNugetDirectory $LocalNugetDirectory
  Write-Host "Running tests in $testPath."
  Push-Location $testPath
  $result = dotnet test --logger trx
  Pop-Location
  AssertLastExitCode -result $result
}

$modelPath = "$ProjectPath.Domain.Models"

Write-Host "Testing if path exist: $modelPath"

$createModelNuget = 'false'
if (Test-Path $modelPath) 
{
  $createModelNuget = 'true'
	Write-Host "Packing $modelPath with version $Version"
	.\PackDepends.ps1 -Version $Version -LocalNugetDirectory $LocalNugetDirectory -Project $modelPath -DestNugetDirectory $ModelNugetDirectory
}
Write-Host "##vso[task.setvariable variable=CreateModelNuget]$createModelNuget"

BuildProject -path "$ProjectPath.API" -OutputDirectory $OutputDirectory -LocalNugetDirectory $LocalNugetDirectory -Version $Version
BuildProject -path "$ProjectPath.Operations" -OutputDirectory $OutputDirectory\Operations -LocalNugetDirectory $LocalNugetDirectory -Version $Version -selfContained 

$appSettingsFile = "$OutputDirectory\appsettings.json"
if (Test-Path $appSettingsFile ) 
{
  Write-Host "Removing $appSettingsFile"
  Remove-Item -Path $appSettingsFile -Force
}

if ($SignFile -ne 'none') {
	
	$content = [Environment]::GetEnvironmentVariable($SignFile.Replace(".","_"))
	Write-Host "Deploying signing file: $content"
	$content | Out-File -FilePath $OutputDirectory\sign.json
}

if ($BuildOnly -eq "true") {
  Write-Host "Skip zipping of project directory."
}
else {
  Write-Host "Including Web Job"
  $efWebJobPath = "$OutputDirectory\App_Data\jobs\triggered\efmigrate"
  If(!(Test-Path $efWebJobPath))
  {
    Write-Host "Creating web job dir: $efWebJobPath"
    New-Item -ItemType Directory -Force -Path $efWebJobPath
  }

  $repo = $env:Build_Repository_LocalPath
  $efWebJobContent = Get-Content -Path "$repo\deployment\EfMigrationWebJob.ps1"
  $exeName = $parts[1] + ".Operations"

  $efWebJobContent = $efWebJobContent.Replace("@APPNAME@", $exeName)

  Set-Content -Path "$efWebJobPath\EfMigrationWebJob.ps1" -Value $efWebJobContent
}

Write-Host "Zip project directory."
Compress-Archive -Path  $OutputDirectory\* -DestinationPath $ZipFilePath
