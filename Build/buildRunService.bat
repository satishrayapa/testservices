REM setlocal EnableDelayedExpansion
set proj="%~1"
set port=%~3

if "%~2"=="service" (
 set projtype=Service
 set projdir=Services.Core
)

if "%~2"=="facade" (
 set projtype=Facade
 set projdir=Services.Facade
)

if "%~2"=="common" (
 set projtype=Common
 set projdir=Common
)

dotnet restore ..\%projtype%.%proj%\%projtype%.%proj%.sln
cd ..\%projtype%.%proj%\API\TAGov.%projdir%.%proj%.API
SET ASPNETCORE_URLS=http://*:%port% && dotnet run --no-launch-profile