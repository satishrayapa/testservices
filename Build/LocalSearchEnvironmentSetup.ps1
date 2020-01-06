Param(
    [string]$environment,
    [switch]$nobuild
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

$securitySvcPort = "50000"
$resourceLocatorSvcPort = "50001"
$searchLegalPartySvcPort = "50002"

if (!$nobuild) {

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

    $identityDatabaseName="Identity"

    if ($integratedSecurity -eq "true") {
        $aumentumConnectionString = "Data Source=$dataSource;Database=$databaseName;Trusted_Connection=True;Connection Timeout=$commandTimeout;"
        $identityConnectionString = "Data Source=$dataSource;Database=$identityDatabaseName;Trusted_Connection=True;Connection Timeout=$commandTimeout;"
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
        $identityConnectionString = "Data Source=$dataSource;Database=$identityDatabaseName;User Id=$userId;Password=$password;Connection Timeout=$commandTimeout;"
    }

    if (!$aumentumConnectionString){
        Write-Host "aumentumConnectionString NOT found."
        exit -1
    }

    if (!$identityConnectionString){
        Write-Host "identityConnectionString NOT found."
        exit -1
    }

    LogIt "Connection string has been built up successfully."

    LogIt $aumentumConnectionString
    LogIt $identityConnectionString

    LogIt "Prepare the nuget dependencies."

    # Prepare the nuget dependencies.
    pushd ..\Common
    dotnet restore -s https://api.nuget.org/v3/index.json
    dotnet pack --output ..\..\Build\Nuget
    popd

    LogIt "Nuget dependencies created."

    LogIt "Starting EF Core migration(s)."

    # Prepare Common.ResourceLocator database using EF Core migration
    $resourceLocatorDbCodeBlock = {
        Param($rootDirectory,$aumentumConnectionString)
        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Prepare Common.ResourceLocator database using EF Core migration."
        pushd ..\Common.ResourceLocator\Repository\TAGov.Common.ResourceLocator.Repository
        SET common.resourceLocator.connectionString="$aumentumConnectionString"
        dotnet restore -s https://api.nuget.org/v3/index.json
        dotnet ef database update
        LogIt "EF Core migration completed successfully for Common.ResourceLocator."
    }

    # Prepare Common.Security database using EF Core migration
    $securityDbCodeBlock = {
        Param($rootDirectory,$identityConnectionString,$aumentumConnectionString)
        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Prepare Common.Security database using EF Core migration."
        pushd ..\Common.Security\Repository\TAGov.Common.Security.Repository
        SET common.security.connectionString="$identityConnectionString"
        SET aumentumSecurity.connectionString="$aumentumConnectionString"
        dotnet restore -s https://api.nuget.org/v3/index.json
        dotnet ef database update --context ProxyConfigurationDbContext
        dotnet ef database update --context ProxyPersistedGrantDbContext
        dotnet ef database update --context AumentumSecurityContext
        LogIt "EF Core migration completed successfully for Common.Security."
    }

    # Prepare Service.LegalPartySearch database using EF Core migration
    $searchLegalPartyDbCodeBlock={
        Param($rootDirectory,$aumentumConnectionString)

        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Prepare Service.LegalPartySearch database using EF Core migration."
        LogIt "Using the following connection string: $aumentumConnectionString"
        pushd ..\Service.LegalPartySearch\Repository\TAGov.Services.Core.LegalPartySearch.Repository
        SET service.legalpartysearch.connectionString="$aumentumConnectionString"
        SET service.legalpartysearch.commandTimeout="150000"
        dotnet restore -s https://api.nuget.org/v3/index.json
        dotnet ef database update --context SearchLegalPartyContext
        LogIt "EF Core migration completed successfully for Service.LegalPartySearch."
    }

    $dbJobs = @()

    $dbJobs += Start-Job -ScriptBlock $resourceLocatorDbCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString)
    $dbJobs += Start-Job -ScriptBlock $securityDbCodeBlock -argumentlist @($PSscriptRoot,$identityConnectionString,$aumentumConnectionString)
    $dbJobs += Start-Job -ScriptBlock $searchLegalPartyDbCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString)

    Wait-Job $dbJobs

    foreach($dbJob in $dbJobs) {
        Receive-Job $dbJob
    }

    LogIt "EF Core migration(s) all done."



    # Build Common.ResourceLocator
    $resourceLocatorBuildCodeBlock = {
        Param($rootDirectory,$aumentumConnectionString,$securitySvcPort)
        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Building Common.ResourceLocator."
        pushd ..\Common.ResourceLocator
        dotnet restore -s ..\Build\Nuget -v detailed
        dotnet restore -s https://api.nuget.org/v3/index.json -v detailed
        popd

        pushd ..\Common.ResourceLocator\API\TAGov.Common.ResourceLocator.API
        $localAppSettings = Get-Content appsettings.json | Out-String | ConvertFrom-Json
        $localAppSettings.ConnectionStrings.Resource="$aumentumConnectionString"
        $localAppSettings.Swagger.Endpoints[0].SwaggerJsonURL="/swagger/v1/swagger.json"
        $localAppSettings.Swagger.Endpoints[1].SwaggerJsonURL="/swagger/v1.1/swagger.json"
        $localAppSettings.Swagger.UiCustomization="/SwaggerUiCustomization.js"
        $localAppSettings.Security.Authority="http://localhost:$securitySvcPort"
        Set-Content -Path appsettings.local.json -Value ($localAppSettings | ConvertTo-Json -Depth 4)
        popd
        LogIt "Completed building Common.ResourceLocator."
    }

    # Build Common.Security
    $securityBuildCodeBlock = {
        Param($rootDirectory,$identityConnectionString,$aumentumConnectionString,$securitySvcPort)
        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Building Common.Security."
        pushd ..\Common.Security
        dotnet restore -s ..\Build\Nuget -v detailed
        dotnet restore -s https://api.nuget.org/v3/index.json -v detailed
        popd

        pushd ..\Common.Security\API\TAGov.Common.Security.API
        $localAppSettings = Get-Content appsettings.json | Out-String | ConvertFrom-Json
        $localAppSettings.Swagger.Endpoints[0].SwaggerJsonURL="/swagger/v1/swagger.json"
        $localAppSettings.Swagger.UiCustomization="/SwaggerUiCustomization.js"
        $localAppSettings.ConnectionStrings.DefaultConnection="$identityConnectionString"
        $localAppSettings.ConnectionStrings.Permissions="$aumentumConnectionString"
        $localAppSettings.Security.Authority="http://localhost:$securitySvcPort"
        Set-Content -Path appsettings.local.json -Value ($localAppSettings | ConvertTo-Json -Depth 4)
        popd
        LogIt "Completed building Common.Security."
    }

    # Build Service.LegalPartySearch
    $searchLegalPartyBuildCodeBlock = {
        Param($rootDirectory,$aumentumConnectionString,$securitySvcPort)
        cd $rootDirectory
        Import-Module .\tools\Logger.psm1
        LogIt "Building Service.LegalPartySearch."
        pushd ..\Service.LegalPartySearch
        dotnet restore -s ..\Build\Nuget -v detailed
        dotnet restore -s https://api.nuget.org/v3/index.json -v detailed
        popd

        pushd ..\Service.LegalPartySearch\API\TAGov.Services.Core.LegalPartySearch.API
        $localAppSettings = Get-Content appsettings.json | Out-String | ConvertFrom-Json
        $localAppSettings.ConnectionStrings.Aumentum="$aumentumConnectionString"
        $localAppSettings.Swagger.Endpoints[0].SwaggerJsonURL="/swagger/v1/swagger.json"
        $localAppSettings.Swagger.Endpoints[1].SwaggerJsonURL="/swagger/v1.1/swagger.json"
        $localAppSettings.Swagger.UiCustomization="/SwaggerUiCustomization.js"
        $localAppSettings.Security.Authority="http://localhost:$securitySvcPort"
        Set-Content -Path appsettings.local.json -Value ($localAppSettings | ConvertTo-Json -Depth 4)
        popd
        LogIt "Completed building Service.LegalPartySearch."
    }

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

    $buildJobs += Start-Job -ScriptBlock $resourceLocatorBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString,$securitySvcPort)
    $buildJobs += Start-Job -ScriptBlock $securityBuildCodeBlock -argumentlist @($PSscriptRoot,$identityConnectionString,$aumentumConnectionString,$securitySvcPort)
    $buildJobs += Start-Job -ScriptBlock $searchLegalPartyBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString,$securitySvcPort)
    $buildJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString)
    $buildJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncCoordinatorBuildCodeBlock -argumentlist @($PSscriptRoot,$aumentumConnectionString,$resourceLocatorSvcPort)

    Wait-Job $buildJobs

    foreach($buildJob in $buildJobs) {

        Receive-Job $buildJob
    }
    # End of code block for when RunOnly = $false
}

# Start Svcs
$securityAPIJob = {
    Param($rootDirectory,$securitySvcPort)
    cd $rootDirectory
    pushd ..\Common.Security\API\TAGov.Common.Security.API
    $Env:ASPNETCORE_ENVIRONMENT = "local"
    $Env:ASPNETCORE_URLS = "http://*:$securitySvcPort"
    dotnet run
}

$resourceLocatorAPIJob = {
    Param($rootDirectory,$resourceLocatorSvcPort)
    cd $rootDirectory
    pushd ..\Common.ResourceLocator\API\TAGov.Common.ResourceLocator.API
    $Env:ASPNETCORE_ENVIRONMENT = "local"
    $Env:ASPNETCORE_URLS = "http://*:$resourceLocatorSvcPort"
    dotnet run
}

$searchLegalPartyAPIJob = {
    Param($rootDirectory,$searchLegalPartySvcPort)
    cd $rootDirectory
    pushd ..\Service.LegalPartySearch\API\TAGov.Services.Core.LegalPartySearch.API
    $Env:ASPNETCORE_ENVIRONMENT = "local"
    $Env:ASPNETCORE_URLS = "http://*:$searchLegalPartySvcPort"
    dotnet run
}

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
$svcJobs += Start-Job -ScriptBlock $resourceLocatorAPIJob -argumentlist @($PSscriptRoot,$resourceLocatorSvcPort)
$svcJobs += Start-Job -ScriptBlock $securityAPIJob -argumentlist @($PSscriptRoot,$securitySvcPort)
$svcJobs += Start-Job -ScriptBlock $searchLegalPartyAPIJob -argumentlist @($PSscriptRoot,$searchLegalPartySvcPort)
$svcJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncJob -argumentlist @($PSscriptRoot)
$svcJobs += Start-Job -ScriptBlock $legalPartySearchDataSyncCoordinatorJob -argumentlist @($PSscriptRoot)

# Update Features
LogIt "Updating resource value(s)..."

pushd deployments
$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","DEV")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.features.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","DEV")
$args += ("-key","LegalPartySearchFeature")
$args += ("-value","true")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","DEV")
$args += ("-key","RevenueObjectSearchFeature")
$args += ("-value","true")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:DEV")
$args += ("-key","service.legalpartysearch")
$args += ("-value","http://localhost:$searchLegalPartySvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

popd

LogIt "All resource value(s) updated."

if (!$nobuild) {

    LogIt "Attempting to update your web.config file"

    AddOrUpdateWebConfig -key "Common.ResourceLocator.Uri" -value "http://localhost:$resourceLocatorSvcPort"
    AddOrUpdateWebConfig -key "Common.ResourceLocator.ExternalUri" -value "http://localhost:$resourceLocatorSvcPort"
    AddOrUpdateWebConfig -key "Common.ResourceLocator.Partition" -value "DEV"
    AddOrUpdateWebConfig -key "TAGov.Common.Security.Authority" -value "http://localhost:$securitySvcPort"
    AddOrUpdateWebConfig -key "TAGov.Common.Security.ClientScope" -value "api.common.resourcelocator api.service.legalpartysearch"

    LogIt "Web.config file has been updated"
}

Read-Host -Prompt "Press Enter to stop service(s)"

LogIt "Attempting to stop service(s)..."

foreach($svcJob in $svcJobs) {
    Stop-Job -Id $svcJob.Id
    Receive-Job $svcJob
}

LogIt "All service(s) stopped"

) *>&1 >> $outputFile
