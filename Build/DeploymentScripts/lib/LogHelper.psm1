Function WriteEventLogHelper {
    [CmdletBinding()]

    param(
        [Parameter(Mandatory=$true)] [string]$Message
    )

   $logSource = "Thomson Reuters Aumentum MicroServices"

   $logFileExists = [System.Diagnostics.EventLog]::SourceExists($logSource)
   if (! $logFileExists) {
       New-EventLog -Source $logSource -LogName "Application"
   }
   Write-EventLog -LogName "Application" -Source $logSource -EventId 3001 -EntryType Information -Message $Message
}

