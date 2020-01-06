Function LogIt {
    [CmdletBinding()]
    
    param([string]$logMessage)
    
    $message=(Get-Date).ToString() + "`t" + $logMessage

    Write-Output $message

    Write-Host $message
}