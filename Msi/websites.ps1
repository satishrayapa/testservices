Param (
    [Parameter(Mandatory=$true)]
    [String[]]
    $appPool,
    [Parameter(Mandatory=$true)]
    [String[]]
    $webSite,
    [Parameter(Mandatory=$true)]
    [String[]]
    $components
)

function ExitWithCode 
{ 
    param 
    ( 
        $exitcode 
    )

    $host.SetShouldExit($exitcode) 
    exit 
}

Write-Host "AppPool: $appPool"

Write-Host "WebSite: $webSite"

Write-Host 'Checking these components:'
foreach ($component in $components) {
    Write-Host $component
}
Write-Host "Exiting with code 2" 
ExitWithCode -exitcode 2
Write-Host "After exit - this shouldn't be seen"