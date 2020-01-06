Param(
    [string]$environment,
    [string]$jsonFile,
    [string]$disableSvc,
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

$jsonSettingsFile = Get-Content $jsonFile | Out-String | ConvertFrom-Json


$(


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

    $servicesToDisable = $disableSvc.Split(',') | Where-Object { $_ }

    if ($servicesToDisable.Length -gt 0)
    {
        LogIt "The following services(s) will be disabled."
        $servicesToDisable | ForEach {
            LogIt "$_"
        }
    }
    else
    {
        $servicesToDisable = @{}
        LogIt "No services(s) will be disabled."
    }

    LogIt "Starting EF Core migration(s)."

    $hash = @{}

    $dbJobs = @()

    $jsonSettingsFile.Microservices | ForEach {


        $hash.Add($_.ServiceName, $_.ServicePort)

        if ($_.IsAuthority -eq "true") {
            $securitySvcPort = $_.ServicePort
        }

        if ($_.IsResourceLocator -eq "true") {
            $resourceLocatorSvcPort = $_.ServicePort
        }

        $serviceName = $_.ServiceName
        if ($servicesToDisable.Contains($serviceName)) {
            LogIt "Ignoring build for $serviceName"
        }
        else
        {
            $job = {
                Param($rootDirectory,$identityConnectionString,$aumentumConnectionString,$svcDefn)

                    cd $rootDirectory
                    Import-Module .\tools\Logger.psm1

                    $serviceName = $svcDefn.ServiceName

                    LogIt "Prepare $serviceName database using EF Core migration."

                    if ($svcDefn.RepositoryDirectory -eq "") {
                        LogIt "(No migration) Completed successfully for $serviceName."
                        exit
                    }
                    pushd $svcDefn.RepositoryDirectory

					$svcDefn.DbEnvironmentVariables | ForEach {
                        $key = $_.key
                        $value = $_.value
                        [Environment]::SetEnvironmentVariable("$key","$value")
                    }

                    $svcDefn.DbSetIdentityConnectionStrings | ForEach {
                        [Environment]::SetEnvironmentVariable("$_","$identityConnectionString")
                    }

                    $svcDefn.DbSetAumentumConnectionStrings | ForEach {
                        [Environment]::SetEnvironmentVariable("$_","$aumentumConnectionString")
                    }

                    dotnet restore -s https://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global

                    $svcDefn.UpdateDbContexts | ForEach {
                        dotnet ef database update -c $_
                    }

                    LogIt "EF Core migration completed successfully for $serviceName."
            }

            $dbJobs += Start-Job -ScriptBlock $job -argumentlist @($PSscriptRoot,$identityConnectionString,$aumentumConnectionString,$_)
        }
    }

    Wait-Job $dbJobs

    foreach($dbJob in $dbJobs) {
        Receive-Job $dbJob
    }

    LogIt "EF Core migration(s) all done."

    LogIt "Starting Builds(s)."

    $svcJobs = @()

    $jsonSettingsFile.Microservices | ForEach {

        $serviceName = $_.ServiceName
        if ($servicesToDisable.Contains($serviceName)) {
            LogIt "Ignoring ef core migration(s) for $serviceName"
        }
        else
        {
            $job = {
                Param($rootDirectory,$identityConnectionString,$aumentumConnectionString,$svcDefn,$securitySvcPort,$hash)
                    cd $rootDirectory
                    Import-Module .\tools\Logger.psm1

                    $serviceName = $svcDefn.ServiceName

                    cd $rootDirectory
                    Import-Module .\tools\Logger.psm1
                    LogIt "Building $serviceName."
                    pushd $svcDefn.APIDirectory
                    dotnet restore -s https://api.nuget.org/v3/index.json  -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -v detailed
                    popd

                    pushd $svcDefn.Startup 
                    $localAppSettings = Get-Content appsettings.json | Out-String | ConvertFrom-Json

                    $svcDefn.WebIdentityConnectionStrings | ForEach {
                        $localAppSettings.ConnectionStrings.$_ ="$identityConnectionString"
                    }

                    $svcDefn.WebAumentumConnectionStrings | ForEach {
                        $localAppSettings.ConnectionStrings.$_ ="$aumentumConnectionString"
                    }

                    $svcDefn.SwaggerJsonURL | ForEach {

                        $index=$svcDefn.SwaggerJsonURL.IndexOf($_)
                        $localAppSettings.Swagger.Endpoints[$index].SwaggerJsonURL="/swagger/$_/swagger.json"
                    }

                    $localAppSettings.Swagger.UiCustomization = "/SwaggerUiCustomization.js"

                    if ($localAppSettings.Security.Authority) {
                        $localAppSettings.Security.Authority = "http://localhost:$securitySvcPort"
                    }

                    $svcDefn.ServiceEndpoints | ForEach {

                        $api = $_.key
                        $source = $_.source

                        $port = $hash.Get_Item("$source")
                        $localAppSettings.ServiceApiUrls.$api = "http://localhost:$port"

                    }

                    Set-Content -Path appsettings.local.json -Value ($localAppSettings | ConvertTo-Json -Depth 4)
                    LogIt "Completed building $serviceName."
            }

            $svcJobs += Start-Job -ScriptBlock $job -argumentlist @($PSscriptRoot,$identityConnectionString,$aumentumConnectionString,$_,$securitySvcPort,$hash)
        }
    }

    $jsonSettingsFile.WindowsServices | ForEach {
        $job = {
            Param($rootDirectory,$aumentumConnectionString,$resourceLocatorSvcPort,$winSvc)
            cd $rootDirectory
            Import-Module .\tools\Logger.psm1

            $serviceName = $winSvc.ServiceName

            LogIt "Building $serviceName."
            pushd $winSvc.Directory
            msbuild $winSvc.Solution /t:Clean
            ..\Build\tools\nuget restore $winSvc.Solution -Source https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -Source https://api.nuget.org/v3/index.json
            msbuild $winSvc.Solution /p:Configuration=Debug

            $config = $winSvc.ConfigFile
            $doc = (Get-Content $config) -as [Xml]

            $item = $doc.configuration.connectionStrings.add | Where-Object { $_.name -eq "Aumentum" }
            $item.connectionString = "$aumentumConnectionString"

            if ($winSvc.RequireResourceLocatorInConfigFile) {

                if ($winSvc.RequireResourceLocatorInConfigFile -eq "true") {
                    $item2 = $doc.configuration.appSettings.add | Where-Object { $_.key -eq "Common.ResourceLocator.Uri" }
                    $item2.value = "http://localhost:$resourceLocatorSvcPort"
                }
            }

            $doc.Save($config)
            popd
            LogIt "Completed building $serviceName."
        }

        $svcJobs += Start-Job -ScriptBlock $job -argumentlist @($PSscriptRoot,$aumentumConnectionString,$resourceLocatorSvcPort,$_)
    }

    Wait-Job $svcJobs

    foreach($svcJob in $svcJobs) {
        Receive-Job $svcJob
    }

    LogIt "Builds(s) all done."

    LogIt "Starting up all microservices."

    $svcJobs = [System.Collections.ArrayList]@()
    $svcJobsNames = [System.Collections.ArrayList]@()

    $jsonSettingsFile.Microservices | ForEach {

        $serviceName = $_.ServiceName
        if ($servicesToDisable.Contains($serviceName)) {
            LogIt "Ignoring running $serviceName"
        }
        else
        {
            $job = {
                Param($rootDirectory,$svcDefn)

                    cd $rootDirectory
                    Import-Module .\tools\Logger.psm1

                    $serviceName = $svcDefn.ServiceName

                    cd $rootDirectory
                    pushd $svcDefn.Startup
                    $Env:ASPNETCORE_ENVIRONMENT = "local"
                    $svcPort = $svcDefn.ServicePort
                    $Env:ASPNETCORE_URLS = "http://*:$svcPort"

                    LogIt "Running $serviceName."

                    dotnet run --no-launch-profile
            }

            $svcJobs += Start-Job -ScriptBlock $job -argumentlist @($PSscriptRoot,$_)
            $svcJobsNames += $serviceName
        }
    }

    $jsonSettingsFile.WindowsServices | ForEach {
        $job = {
            Param($rootDirectory,$winSvc)
            cd $rootDirectory
            Import-Module .\tools\Logger.psm1

            $winSvcApp = $winSvc.Startup

            LogIt "Running WINSVC $winSvcApp."

            Start-Process -NoNewWindow -PassThru -FilePath $winSvcApp
        }

        $svcJobs += Start-Job -ScriptBlock $job -argumentlist @($PSscriptRoot,$_)
    }

    LogIt "Updating resource value(s)..."

    pushd deployments
    $args = @()
    $args += ("-sslEnabled","false")
    $args += ("-partition","dev")
    $args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
    Invoke-Expression ".\configure.features.ps1 $args"

    $args = @()
    $args += ("-sslEnabled","false")
    $args += ("-partition","dev")
    $args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
    Invoke-Expression ".\configure.urlservices.ps1 $args"

    $jsonSettingsFile.ResourceLocator | ForEach {

        $sslEnabled = $_.sslEnabled
        $partition = $_.partition
        $key=$_.key
        $value=$_.value

        if ($value -eq "INFER_URL_FROM_KEY") {

            $value = "NOT_FOUND_PLEASE_CHECK_JSON_TO_ENSURE_SVC_EXIST"

            foreach($local in  $jsonSettingsFile.Microservices) {

                if ($local.ServiceName -eq $key) {
                    $port = $local.ServicePort
                    $value = "http://localhost:$port"
                }
            }
        }

        $args = @()
        $args += ("-sslEnabled","$sslEnabled")
        $args += ("-partition","$partition")
        $args += ("-key","$key")
        $args += ("-value","$value")
        $args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
        Invoke-Expression ".\configure.single.resource.ps1 $args"
    }

    popd

    LogIt "All resource value(s) updated."

    if (!$nobuild) {

        LogIt "Attempting to update your web.config file"

        AddOrUpdateWebConfig -key "Common.ResourceLocator.Uri" -value "http://localhost:$resourceLocatorSvcPort"
        AddOrUpdateWebConfig -key "Common.ResourceLocator.ExternalUri" -value "http://localhost:$resourceLocatorSvcPort"
        AddOrUpdateWebConfig -key "Common.ResourceLocator.Partition" -value "DEV"
        AddOrUpdateWebConfig -key "TAGov.Common.Security.Authority" -value "http://localhost:$securitySvcPort"

        $jsonSettingsFile.WebConfigOverrides | ForEach {

            $key = $_.key
            $value = $_.value

            AddOrUpdateWebConfig -key "$key" -value "$value"
        }

        LogIt "Web.config file has been updated"
    }

    $input = "true"

    while ($input -eq "true")
    {
        $input = Read-Host -Prompt "Press Enter to stop service(s) OR enter name of service to stop."

        if ($input -eq [string]::empty) {
            break
        }

        $svcJobToStopIndex = $svcJobsNames.IndexOf($input)

        LogIt "Index $svcJobToStopIndex"

        if ($svcJobToStopIndex -gt -1) {
            LogIt "Stopping $input..."
            $svcJob = $svcJobs[$svcJobToStopIndex]
            Stop-Job -Id $svcJob.Id
            Receive-Job $svcJob
            LogIt "$input is stopped."
            $svcJobs.RemoveAt($svcJobToStopIndex)
            $svcJobsNames.RemoveAt($svcJobToStopIndex)
        }
        else
        {
            LogIt "Unable to locate $input."
        }

        $input = "true"
    }

    LogIt "Attempting to stop service(s)..."

    $jsonSettingsFile.WindowsServices | ForEach {
        $winSvcApp = [string]$_.Startup
        $index = $winSvcApp.LastIndexOf("\") + 1
        $fileName = $winSvcApp.Substring($index)
        LogIt "Stopping $fileName"
        Get-Process | Where-Object {$_.Path -like "*$fileName*"} | Stop-Process
    }

    foreach($svcJob in $svcJobs) {
        Stop-Job -Id $svcJob.Id
        Receive-Job $svcJob
    }

    LogIt "All service(s) stopped"
}



) *>&1 >> $outputFile
