SET BUILD_NUMBER=%1
SET SCMChangeSet=%2
SET PUBLISHDIR=%3
SET STAGINGDIR=%4

IF EXIST %PUBLISHDIR% RMDIR /S /Q %PUBLISHDIR% || exit /b

for /f "tokens=3delims=<>    " %%a in ('type ServicesVersion.XML ^|find "MajorVersionNumberFourDigit"') do SET "MAJORVERSION=%%a"
for /f "tokens=3delims=<>    " %%b in ('type ServicesVersion.XML ^|find "MinorVersionNumber"') do SET "MINORVERSION=%%b"
for /f "tokens=3delims=<>    " %%c in ('type ServicesVersion.XML ^|find "UpdateVersionNumber"') do SET "UPDATEVERSION=%%c"


mkdir z:\BuildTest\MSIPackaging\BuildTools\Tools\7-Zip

xcopy z:\7-Zip z:\BuildTest\MSIPackaging\BuildTools\Tools\7-Zip

cd /d z:\BuildTest\MSIPackaging\BuildTools\Tools\7-Zip
7z.exe a -tzip -r -mx=1 -mmt=on %PUBLISHDIR%\AumentumServices\AumentumServices.%MAJORVERSION%.%MINORVERSION%\AumentumServices.%MAJORVERSION%.%MINORVERSION%.%UPDATEVERSION%.%BUILD_NUMBER%.%SCMChangeSet%.zip %STAGINGDIR%\* || exit /b

REM 20170815-0228