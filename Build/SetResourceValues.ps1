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
$assessmentHeaderFcdPort = "50206"
$baseValueSegmentFcdPort = "50205"
$assessmentEventSvcPort = "50201"
$baseValueSegmentSvcPort = "50204"
$grmEventSvcPort = "50207"
$legalPartySvcPort = "50203"
$revenueObjectSvcPort = "50202"

# Update Features
LogIt "Updating resource value(s)..."

pushd deployments
$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","LOCALDEV")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.features.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","LOCALDEV")
$args += ("-key","LegalPartySearchFeature")
$args += ("-value","true")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","LOCALDEV")
$args += ("-key","RevenueObjectSearchFeature")
$args += ("-value","true")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.legalpartysearch")
$args += ("-value","http://localhost:$searchLegalPartySvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","LOCALDEV")
$args += ("-key","BaseValueSegmentFeature")
$args += ("-value","true")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","facade.assessmentheader")
$args += ("-value","http://localhost:$assessmentHeaderFcdPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","facade.basevaluesegment")
$args += ("-value","http://localhost:$baseValueSegmentFcdPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.assessmentevent")
$args += ("-value","http://localhost:$assessmentEventSvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.basevaluesegment")
$args += ("-value","http://localhost:$baseValueSegmentSvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.grmevent")
$args += ("-value","http://localhost:$grmEventSvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.legalparty")
$args += ("-value","http://localhost:$legalPartySvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

$args = @()
$args += ("-sslEnabled","false")
$args += ("-partition","urlservices:LOCALDEV")
$args += ("-key","service.revenueobject")
$args += ("-value","http://localhost:$revenueObjectSvcPort")
$args += ("-resourceLocatorEndPoint","localhost:$resourceLocatorSvcPort")
Invoke-Expression ".\configure.single.resource.ps1 $args"

popd

LogIt "All resource value(s) updated."

) *>&1 >> $outputFile
