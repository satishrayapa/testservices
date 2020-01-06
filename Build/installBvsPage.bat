cd ..\..\dbbuild
powershell -ExecutionPolicy Bypass -NoProfile -File ./cidb.ps1 importxml updateriverside -File ..\AA\XML\Runtime\CA\Riverside\AAMenu.xml
powershell -ExecutionPolicy Bypass -NoProfile -File ./cidb.ps1 importxml updateriverside -File ..\Roll\XML\Roll\options.xml
powershell -ExecutionPolicy Bypass -NoProfile -File ./cidb.ps1 importxml updateriverside -File ..\Roll\XML\Runtime\CA\RollRuntime.xml
pause