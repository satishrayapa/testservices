param(
	[Parameter(Mandatory=$false)]
	[string]
	$environment = 'DEV',

	[Parameter(Mandatory=$false)]
	[string]
	$propFileName = "AumentumServices.properties.xml"
)

Write-Host "Updating AppSettings..." -ForegroundColor Green

try {

	$xml = [xml](Get-Content $PSScriptRoot\..\$propFileName)
	$ns = @{aum='http://www.thomson.com/financial/re/wics/2013/01/reca'}
	$SERVINSTALLDIR = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='INSTALLDIR']").Node."#text"
	$SERVERNAME = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='SERVERNAME']").Node."#text"
	$DS = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='DS']").Node."#text"
	$DB = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='DB']").Node."#text"
	$DBUSER = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='DBUSER']").Node."#text"
	$DBPASSWORD = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='DBPASSWORD']").Node."#text"
	$DBTIMEOUT = (Select-Xml -Xml $xml -Namespace $ns  -XPath "/aum:PropertyGroup/aum:Environment[@Name='$environment']/aum:Property[@Name='DBTIMEOUT']").Node."#text"

	Get-ChildItem "$PSScriptRoot\..\..\Services\*\API\*.API\appsettings.json" -Recurse | % { $_.IsReadOnly=$false }
	
	Get-ChildItem "$PSScriptRoot\..\..\Services\*\API\*.API\appsettings.json" -Recurse | ForEach-Object -Process {
		(Get-Content $_) `
		-Replace '#SERVERNAME', $SERVERNAME `
		-Replace '#DATASOURCE', $DS `
		-Replace '#DATABASE', $DB `
		-Replace '#USERID', $DBUSER `
		-Replace '#PASSWORD', $DBPASSWORD `
		-Replace '#DBTIMEOUT', $DBTIMEOUT `
		| Set-Content $_
		
		Write-Host "Config updated successfully..." -ForegroundColor Green
    }
	
		Write-Host "All Configs updateded..." -ForegroundColor Green
}	
catch {
    $ErrorMessage = $_.Exception.Message
    $FailedItem = $_.Exception.ItemName
	Write-Host "Updation failed...: $ErrorMessage at $FailedItem" -ForegroundColor Red
}