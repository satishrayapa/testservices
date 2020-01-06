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
        ${env:common.resourceLocator.connectionString}="$aumentumConnectionString" 
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
        ${env:common.security.connectionString}="$identityConnectionString"
        ${env:aumentumSecurity.connectionString}="$aumentumConnectionString"
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
        ${env:service.legalpartysearch.connectionString}="$aumentumConnectionString"
        ${env:service.legalpartysearch.commandTimeout}="150000"
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
    # End of code block for when RunOnly = $false
}

) *>&1 >> $outputFile
