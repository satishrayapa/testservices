pushd AumentumDatabase\SQLScripts
if (!(Test-Path .\*.sql)
{
    ExecScript
}
popd

function ExecScript()
{
    $ErrorActionPreference = "Stop"
    import-module sqlps

    Foreach-Object {
      $content = Get-Content $_.FullName

      #filter and save content to the original file
      $content | Where-Object {$_ -match 'step[49]'} | Set-Content $_.FullName

      #filter and save content to a new file 
      $content | Where-Object {$_ -match 'step[49]'} | Set-Content ($_.BaseName + '_out.log')
    }


    $sqlfile = "$env:workspace\SQLScripts\20170313-01-BVS-Scripts.sql"
    write-host "using file $sqlfile"
    foreach ($server in "C588ZQYLUMDB1","C906ZZWLUMDB2", "C108TSWLUMDB3") 
    {
      Write-Host "updating server $server..."
      Invoke-Sqlcmd -ServerInstance "${server}.ecomqc.tlrg.com" -Database CA-Riverside-QA-SCL -Username Lum3n -Password 123Lum3n -InputFile $sqlfile
    }
}


Get-ChildItem -Filter *.txt |
$content = select-object FullName
Foreach-Object {     $content = Get-Content $_.FullName }