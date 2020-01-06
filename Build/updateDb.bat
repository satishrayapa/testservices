set projname=%~2
set dbname=%~3
set context=%~4

if "%~1"=="service" (
 set projtype=Service
 set projdir=Services.Core
)

if "%~1"=="common" (
 set projtype=Common
 set projdir=Common
)

cd ..\%projtype%.%projname%\Repository\TAGov.%projdir%.%projname%.Repository
set %projtype%.%projname%.connectionString=Data Source=localhost;Database=%dbname%;Trusted_Connection=True;
dotnet ef database update --context %context%Context
cd ..\..\..\Build