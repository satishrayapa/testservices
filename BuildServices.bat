REM ***********************************************************
REM This script will build the Aumentum services

REM Parm: SERVICE_DIR - Services directory path
REM Parm: BUILD_DIR - Ouput directory path
REM Parm: LOCAL_NUGET_DIR - Local Nuget directory path
REM Parm: PRODUCT_VERSION - Product version or build number
REM ***********************************************************

cd /d %SERVICE_DIR%

REM ** Delete the Output Directory***
RMDIR /Q /S "%BUILD_DIR%\bin"


pushd Common

REM ***************Begin of Common
REM *************** Restore TAGov.Common.ExceptionHandler
dotnet restore -s http://api.nuget.org/v3/index.json TAGov.Common.ExceptionHandler\TAGov.Common.ExceptionHandler.csproj
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build TAGov.Common.ExceptionHandler
dotnet build TAGov.Common.ExceptionHandler\TAGov.Common.ExceptionHandler.csproj
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Publish TAGov.Common.ExceptionHandler
dotnet pack --output %LOCAL_NUGET_DIR% TAGov.Common.ExceptionHandler\TAGov.Common.ExceptionHandler.csproj
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Restore TAGov.Common
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build TAGov.Common
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Common
pushd TAGov.Common.ExceptionHandler.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

REM *************** Pack TAGov.Common.Http
dotnet pack TAGov.Common.Http --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.Logging
dotnet pack TAGov.Common.Logging --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.Caching
dotnet pack TAGov.Common.Caching --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.Paging
dotnet pack TAGov.Common.Paging --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.Http.Versioning
dotnet pack TAGov.Common.Http.Versioning --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.UrlService
dotnet pack TAGov.Common.UrlService --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.EntityFrameworkCore
dotnet pack TAGov.Common.EntityFrameworkCore --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Common.Operations
dotnet pack TAGov.Common.Operations --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

REM ***************End of Common

REM ***************Begin of Common.ResourceLocator

REM Common.ResourceLocator

pushd Common.ResourceLocator

REM *************** Restore Common.ResourceLocator
REM * dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Common.ResourceLocator
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Common.ResourceLocator

pushd Domain.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd
pushd Repository.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

REM *************** Publish Common.ResourceLocator
REM **** old -dotnet publish --output %BUILD_DIR%\bin\Common.ResourceLocator --configuration debug
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Common.ResourceLocator.API\TAGov.Common.ResourceLocator.API.csproj --output %BUILD_DIR%\bin\Common.ResourceLocator --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64  /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Common.ResourceLocator.Operations\TAGov.Common.ResourceLocator.Operations.csproj --output %BUILD_DIR%\bin\Common.ResourceLocator\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

REM ***************End of Common.ResourceLocator

REM ***************Begin of Common.Security

pushd Common.Security

REM Common.Security

REM *************** Restore Common.Security
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Restore Common.Security
dotnet pack TAGov.Common.Security.Claims --output %LOCAL_NUGET_DIR%
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Restore Common.Security
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Common.Security
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Common.Security
REM ****pushd Domain.Tests
REM ****dotnet test
REM ****popd
REM ****pushd Repository.Tests
REM ****dotnet test
REM ****popd

REM *************** Publish Common.Security
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Common.Security.API\TAGov.Common.Security.API.csproj --output %BUILD_DIR%\bin\Common.Security --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

copy TAGov.Common.Security.API\bin\release\netcoreapp1.1\TAGov.Common.Security.API.xml %BUILD_DIR%\bin\Common.Security
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Common.Security.Operations\TAGov.Common.Security.Operations.csproj --output %BUILD_DIR%\bin\Common.Security\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack Claims for Common.Security
dotnet pack TAGov.Common.Security.Http.Authorization --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Service.AssessmentEvent

REM Service.AssessmentEvent

REM *************** Restore Service.AssessmentEvent
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.AssessmentEvent
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.AssessmentEvent.Domain.Models
dotnet pack TAGov.Services.Core.AssessmentEvent.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.AssessmentEvent

pushd Domain.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd
pushd Repository.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

REM *************** Publish Service.AssessmentEvent
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.AssessmentEvent.API\TAGov.Services.Core.AssessmentEvent.API.csproj --output %BUILD_DIR%\bin\Service.AssessmentEvent --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.AssessmentEvent.Operations\TAGov.Services.Core.AssessmentEvent.Operations.csproj --output %BUILD_DIR%\bin\Service.AssessmentEvent\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Service.GrmEvent

REM Service.GrmEvent

REM *************** Restore Service.GrmEvent
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.GrmEvent
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.GrmEvent.Domain.Models
dotnet pack TAGov.Services.Core.GrmEvent.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.GrmEvent

pushd Domain.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%
popd
pushd Repository.Tests
dotnet test
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%
popd

REM *************** Publish Service.GrmEvent
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.GrmEvent.API\TAGov.Services.Core.GrmEvent.API.csproj --output %BUILD_DIR%\bin\Service.GrmEvent --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.GrmEvent.Operations\TAGov.Services.Core.GrmEvent.Operations.csproj --output %BUILD_DIR%\bin\Service.GrmEvent\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Service.LegalParty

REM Service.LegalParty

REM *************** Restore Service.LegalParty
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.LegalParty
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.LegalParty.Domain.Models
dotnet pack TAGov.Services.Core.LegalParty.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.LegalParty


REM *************** Publish Service.LegalParty
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.LegalParty.API\TAGov.Services.Core.LegalParty.API.csproj --output %BUILD_DIR%\bin\Service.LegalParty --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.LegalParty.Operations\TAGov.Services.Core.LegalParty.Operations.csproj --output %BUILD_DIR%\bin\Service.LegalParty\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Service.RevenueObject

REM Service.RevenueObject

REM *************** Restore Service.RevenueObject
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.RevenueObject
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.RevenueObject.Domain.Models
dotnet pack TAGov.Services.Core.RevenueObject.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.RevenueObject


REM *************** Publish Service.RevenueObject
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.RevenueObject.API\TAGov.Services.Core.RevenueObject.API.csproj --output %BUILD_DIR%\bin\Service.RevenueObject --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.RevenueObject.Operations\TAGov.Services.Core.RevenueObject.Operations.csproj --output %BUILD_DIR%\bin\Service.RevenueObject\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Service.BaseValueSegment

REM Service.BaseValueSegment

REM *************** Restore Service.BaseValueSegment
REM dotnet restore -s http://api.nuget.org/v3/index.json -s %LOCAL_NUGET_DIR% -v Detailed
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.BaseValueSegment
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.BaseValueSegment.Domain.Models
dotnet pack TAGov.Services.Core.BaseValueSegment.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.BaseValueSegment

REM *************** Publish Service.BaseValueSegment
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.BaseValueSegment.API\TAGov.Services.Core.BaseValueSegment.API.csproj --output %BUILD_DIR%\bin\Service.BaseValueSegment --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.BaseValueSegment.Operations\TAGov.Services.Core.BaseValueSegment.Operations.csproj --output %BUILD_DIR%\bin\Service.BaseValueSegment\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Facade.AssessmentHeader

REM Facade.AssessmentHeader

REM *************** Restore Facade.AssessmentHeader
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Facade.AssessmentHeader
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Facade.AssessmentHeader

REM *************** Publish Facade.AssessmentHeader
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Facade.AssessmentHeader.API\TAGov.Services.Facade.AssessmentHeader.API.csproj --output %BUILD_DIR%\bin\Facade.AssessmentHeader --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Facade.AssessmentHeader.Operations\TAGov.Services.Facade.AssessmentHeader.Operations.csproj --output %BUILD_DIR%\bin\Facade.AssessmentHeader\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd

pushd Facade.BaseValueSegment

REM Facade.BaseValueSegment

REM *************** dotnet clean
dotnet clean
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Restore Facade.BaseValueSegment
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Facade.BaseValueSegment
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Publish Facade.BaseValueSegment
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Facade.BaseValueSegment.API\TAGov.Services.Facade.BaseValueSegment.API.csproj --output %BUILD_DIR%\bin\Facade.BaseValueSegment --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Facade.BaseValueSegment.Operations\TAGov.Services.Facade.BaseValueSegment.Operations.csproj --output %BUILD_DIR%\bin\Facade.BaseValueSegment\Operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

popd


REM ***************Begin of Service.LegalPartySearch

pushd Service.LegalPartySearch

REM Service.LegalPartySearch

REM *************** dotnet clean
dotnet clean
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM ***************Restore Service.LegalPartySearch
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.LegalPartySearch
REM *** dotnet build /p:AssemblyVersion=2018.1.%BUILD_NUMBER%.0
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.Service.LegalPartySearch.Domain.Models
dotnet pack TAGov.Services.Core.LegalPartySearch.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Test Service.LegalPartySearch


REM *************** Publish Service.LegalPartySearch
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.LegalPartySearch.API\TAGov.Services.Core.LegalPartySearch.API.csproj --output %BUILD_DIR%\bin\Service.LegalPartySearch --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.LegalPartySearch.Operations\TAGov.Services.Core.LegalPartySearch.Operations.csproj --output %BUILD_DIR%\bin\Service.LegalPartySearch\operations --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM ***************End of Service.LegalPartySearch

popd

REM ***************Begin of Service.MyWorklistSearch

pushd Service.MyWorklistSearch

REM Service.MyWorklistSearch

REM *************** dotnet clean
dotnet clean
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM ***************Restore Service.MyWorklistSearch
dotnet restore -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s %LOCAL_NUGET_DIR% -v Detailed
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Build Service.MyWorklistSearch
dotnet build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM *************** Pack TAGov.Services.Core.MyWorklistSearch.Domain.Models
dotnet pack TAGov.Services.Core.MyWorklistSearch.Domain.Models --output %LOCAL_NUGET_DIR% --no-build
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%


REM *************** Test Service.MyWorklistSearch


REM *************** Publish Service.MyWorklistSearch
dotnet clean --configuration Release
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.MyWorklistSearch.API\TAGov.Services.Core.MyWorklistSearch.API.csproj --output %BUILD_DIR%\bin\Service.MyWorklistSearch --configuration Release --framework netcoreapp2.1 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

dotnet publish TAGov.Services.Core.MyWorklistSearch.Operations\TAGov.Services.Core.MyWorklistSearch.Operations.csproj --output %BUILD_DIR%\bin\Service.MyWorklistSearch\operations --configuration Release --framework netcoreapp2.1 --self-contained --runtime win-x64 /p:AssemblyVersion=2018.1.%PRODUCT_VERSION%.0 /p:ProductVersion=2018.1.%PRODUCT_VERSION%.0 /p:AssemblyFileVersion=2018.1.%PRODUCT_VERSION%.0
IF %ERRORLEVEL% GEQ 1 exit %ERRORLEVEL%

REM ***************End of Service.MyWorklistSearch

popd
