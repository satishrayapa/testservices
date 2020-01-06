
function BuildConnectionString {
	param (
         [Parameter(Mandatory=$true)]   [string]$Server
        ,[Parameter(Mandatory=$true)]   [string]$DbName
        ,[Parameter(Mandatory=$false)]  [string]$User
        ,[Parameter(Mandatory=$false)]  [string]$Password
        ,[Parameter(Mandatory=$false)]  [int]   $TimeOut = 0
        ,[Parameter(Mandatory=$false)]  [switch]$UseIntegratedSecurity
    )

    if($useIntegratedSecurity){
        $connectionString = "Data Source=$Server;Database=$DbName;integrated security=SSPI;Connection Timeout=$TimeOut"
    } else {
        $connectionString = "Data Source=$Server;Database=$DbName;User Id=$User;Password=$Password;Connection Timeout=$TimeOut"
    }

    return $connectionString
}

function ExecuteSQLQuery {
	param (
         [Parameter(Mandatory=$true)]  [string] $ConnectionString
        ,[Parameter(Mandatory=$true)]  [string] $Query
        ,[Parameter(Mandatory=$false)] [switch] $SilentlyContinue
        ,[Parameter(Mandatory=$false)] [string]$OutputLabel
    )

	#Write-Host "Start ..." -ForegroundColor Green

	$SqlConnection = New-Object System.Data.SqlClient.SqlConnection
	$SqlConnection.ConnectionString = $ConnectionString

    $SqlConnection.Open()
    $SqlCmd = New-Object System.Data.SQLClient.SQLCommand
	$SqlCmd.Connection = $SqlConnection
	$SqlCmd.CommandTimeout = 0
	$SqlCmd.CommandText = $Query

	$asError = $false
	try {
	    $SqlCmd.ExecuteNonQuery() | Out-Null
    }
    catch {
        $asError = $true
        if (!$SilentlyContinue) {throw $_}
        $errorMessage = $_.Exception.InnerException.Message
    }
    finally {
	    $SqlConnection.Close()
    }

	if ($asError) {
        Write-Host "There was a non-fatal error: $errorMessage" -ForegroundColor Yellow
    }
    else {
        Write-Host "Finished $OutputLabel" -ForegroundColor Green
    }
}
