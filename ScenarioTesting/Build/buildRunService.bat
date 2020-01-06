REM setlocal EnableDelayedExpansion
set proj="%~1"
set port=%~3

set ASPNETCORE_ENVIRONMENT=LOCALDEV

if "%~2"=="service" (
 set projtype=Service
 set projdir=Services.Core
)

if "%~2"=="facade" (
 set projtype=Facade
 set projdir=Services.Facade
)

rem cd ..\%projtype%.%proj%
cd  C:\local_aa\sites\%~2.%proj%\
SET ASPNETCORE_URLS=http://*:%port% && dotnet TAGov.%projdir%.%proj%.API.dll