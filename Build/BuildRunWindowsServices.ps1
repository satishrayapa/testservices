Param(
    [string]$environment
)

Import-Module .\tools\Logger.psm1
Import-Module .\tools\AddOrUpdateWebConfig.psm1

$logDir = "logs"

$outputFile= $logDir + "\" + (Get-Date -format "yyyyMMddHHmmss") + ".txt"


If(!(test-path $logDir))
{
    New-Item -ItemType Directory -Force -Path $logDir
}


$(

$resourceLocatorSvcPort = "50001"

LogIt "Reading environment file to build up the connection string."

if (!$environment){
    LogIt "Parameter: environment must be set."
    exit -1
}

$envConfig = "..\..\DBBuild\Environments.targets"

[Xml]$doc = Get-Content -Path $envConfig

$ns = New-Object System.Xml.XmlNamespaceManager($doc.NameTable)
$ns.AddNamespace("ns", $doc.DocumentElement.NamespaceURI)

$targetedNodes = $doc.SelectNodes("//*[contains(@Condition,'$environment')]")

if ($targetedNodes.Count -eq 0){
    LogIt "Target node $environment NOT found."
    exit -1
}

$condition="`$(Environment)=='$environment'"

foreach($node in $targetedNodes) {

    if ($node.attributes['Condition'].value -eq $condition){
        $targetNode = $node
        break
    }
}

if (!$targetNode){
    Write-Host "Target node $environment NOT found."
    exit -1
}

foreach($childNode in $targetNode.ChildNodes) {

    if ($childNode.Name -eq "DatabaseName") {
        $databaseName = $childNode.InnerText
    }

    if ($childNode.Name -eq "ConnectionString_DataSource") {
        $dataSource = $childNode.InnerText
    }

    if ($childNode.Name -eq "ConnectionString_IntegratedSecurity") {
        $integratedSecurity = $childNode.InnerText
    }

    if ($childNode.Name -eq "CommandTimeout") {
        $commandTimeout = $childNode.InnerText
    }

    if ($childNode.Name -eq "ConnectionString_UserId") {
        $userId = $childNode.InnerText
    }

    if ($childNode.Name -eq "ConnectionString_Password") {
        $password = $childNode.InnerText
    }
}

if (!$dataSource){
    Write-Host "dataSource NOT found."
    exit -1
}

if (!$databaseName){
    Write-Host "databaseName NOT found."
    exit -1
}

if (!$commandTimeout){
    Write-Host "commandTimeout NOT found."
    exit -1
}

if ($integratedSecurity -eq "true") {
    $aumentumConnectionString = "Data Source=$dataSource;Database=$databaseName;Trusted_Connection=True;Connection Timeout=$commandTimeout;"
}else {

    if (!$userId){
        Write-Host "userId NOT found."
        exit -1
    }

    if (!$password){
        Write-Host "password NOT found."
        exit -1
    }

    $aumentumConnectionString = "Data Source=$dataSource;Database=$databaseName;User Id=$userId;Password=$password;Connection Timeout=$commandTimeout;"
}

if (!$aumentumConnectionString){
    Write-Host "aumentumConnectionString NOT found."
    exit -1
}

LogIt "Connection string has been built up successfully."

LogIt $aumentumConnectionString

LogIt "Prepare the nuget dependencies."

# Prepare the nuget dependencies.
pushd ..\Common
dotnet restore -s https://api.nuget.org/v3/index.json
dotnet pack --output ..\..\Build\Nuget
popd

LogIt "Nuget dependencies created."

# Build Process.LegalPartySearchDataSync
$legalPartySearchDataSyncBuildCodeBlock = {
    Param($rootDirectory,$aumentumConnectionString)
    cd $rootDirectory
    Import-Module .\tools\Logger.psm1
    LogIt "Building Process.LegalPartySearchDataSync."
    pushd ..\..\Processes\Process.Sync.LegalParty
    msbuild Process.Sync.LegalPartySearch.sln /t:Clean
    ..\Build\tools\nuget restore Process.Sync.LegalPartySearch.sln -Source https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -Source https://api.nuget.org/v3/index.json
    msbuild Process.Sync.LegalPartySearch.sln /p:Configuration=Debug

    $config = "..\..\Processes\Process.Sync.LegalParty\Process.Sync.LegalParty\bin\Debug\TAGov.Process.Sync.LegalPartySearch.exe.config"
    $doc = (Get-Content $config) -as [Xml]

    $item = $doc.configuration.connectionStrings.add | Where-Object {$_.name -eq "Aumentum" }
    $item.connectionString = "$aumentumConnectionString"
    $doc.Save($config)
    popd
    LogIt "Completed building Process.LegalPartySearchDataSync."
}

# Build Process.LegalPartySearchDataSyncCoordinator
$legalPartySearchDataSyncCoordinatorBuildCodeBlock = {
    Param($rootDirectory,$aumentumConnectionString,$resourceLocatorSvcPort)
    cd $rootDirectory
    Import-Module .\tools\Logger.psm1
    LogIt "Building Process.LegalPartySearchDataSyncCoordinator."
    pushd ..\..\Processes\Process.Sync.LegalPartySearch.Coordinator
    msbuild TAGov.Process.Sync.LegalPartySearch.Coordinator.sln /t:Clean
    ..\Build\tools\nuget restore TAGov.Process.Sync.LegalPartySearch.Coordinator.sln -Source https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -Source https://api.nuget.org/v3/index.json
    msbuild TAGov.Process.Sync.LegalPartySearch.Coordinator.sln /p:Configuration=Debug

    $config = "..\..\Processes\Process.Sync.LegalPartySearch.Coordinator\TAGov.Process.Sync.LegalPartySearch.Coordinator\bin\Debug\TAGov.Process.Sync.LegalPartySearch.Coordinator.exe.config"
    $doc = (Get-Content $config) -as [Xml]

    $item = $doc.configuration.connectionStrings.add | Where-Object {$_.name -eq "Aumentum" }
    $item.connectionString = "$aumentumConnectionString"

    $item2 = $doc.configuration.appSettings.add | Where-Object {$_.key -eq "Common.ResourceLocator.Uri" }
    $item2.value = "http://localhost:$resourceLocatorSvcPort"

    $doc.Save($config)
    popd
    LogIt "Completed building Process.LegalPartySearchDataSyncCoordinator."
}

$buildJobs = @()

$buildJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString)
$buildJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncCoordinatorBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString,$resourceLocatorSvcPort)

Wait-Job $buildJobs

foreach($buildJob in $buildJobs) {

    Receive-Job $buildJob
}
# End of code block for when RunOnly = $false

# Start Svcs
$legalPartySearchDataSyncJob = {
    Param($rootDirectory)
    Start-Sleep -s 10
    cd $rootDirectory
    ..\..\Processes\Process.Sync.LegalParty\Process.Sync.LegalParty\bin\Debug\TAGov.Process.Sync.LegalPartySearch.exe
}

$legalPartySearchDataSyncCoordinatorJob = {
    Param($rootDirectory)
    cd $rootDirectory
    ..\..\Processes\Process.Sync.LegalPartySearch.Coordinator\TAGov.Process.Sync.LegalPartySearch.Coordinator\bin\Debug\TAGov.Process.Sync.LegalPartySearch.Coordinator.exe
}

$svcJobs = @()
$svcJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncJob -argumentlist @($PSscriptRoot)
$svcJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncCoordinatorJob -argumentlist @($PSscriptRoot)

Read-Host -Prompt "Press Enter to stop service(s)"

LogIt "Attempting to stop service(s)..."

foreach($svcJob in $svcJobs) {
    Stop-Job -Id $svcJob.Id
    Receive-Job $svcJob
}

LogIt "All service(s) stopped"

) *>&1 >> $outputFile
