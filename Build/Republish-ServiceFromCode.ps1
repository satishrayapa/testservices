#ResourceLocator
& dotnet.exe restore "C:\Modules\Services\Common.ResourceLocator\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Common.ResourceLocator\"

& dotnet.exe publish "C:\Modules\Services\Common.ResourceLocator\TAGov.Common.ResourceLocator.API\TAGov.Common.ResourceLocator.API.csproj" --output "C:\output\Common.ResourceLocator" --configuration Release --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Common.ResourceLocator\TAGov.Common.ResourceLocator.Operations\TAGov.Common.ResourceLocator.Operations.csproj" --output "C:\output\Common.ResourceLocator\operations"


#security
& dotnet.exe restore "C:\Modules\Services\Common.Security\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Common.Security\"

& dotnet.exe publish "C:\Modules\Services\Common.Security\TAGov.Common.Security.API\TAGov.Common.Security.API.csproj" --output "C:\output\Common.Security" --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Common.Security\TAGov.Common.Security.Operations\TAGov.Common.Security.Operations.csproj" --output "C:\output\Common.Security\operations" --framework netcoreapp2.0 --self-contained --runtime win-x64

#assessment header
& dotnet.exe restore "C:\Modules\Services\Facade.AssessmentHeader\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Facade.AssessmentHeader\"

& dotnet.exe publish "C:\Modules\Services\Facade.AssessmentHeader\TAGov.Services.Facade.AssessmentHeader.API\TAGov.Services.Facade.AssessmentHeader.API.csproj" --output "C:\output\Facade.AssessmentHeader"
& dotnet.exe publish "C:\Modules\Services\Facade.AssessmentHeader\TAGov.Services.Facade.AssessmentHeader.Operations\TAGov.Services.Facade.AssessmentHeader.Operations.csproj" --output "C:\output\Facade.AssessmentHeader\operations"


#Facade.BaseValueSegment
& dotnet.exe restore "C:\Modules\Services\Facade.BaseValueSegment\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Facade.BaseValueSegment\"

& dotnet.exe publish "C:\Modules\Services\Facade.BaseValueSegment\TAGov.Services.Facade.BaseValueSegment.API\TAGov.Services.Facade.BaseValueSegment.API.csproj" --output "C:\output\Facade.BaseValueSegment"
& dotnet.exe publish "C:\Modules\Services\Facade.BaseValueSegment\TAGov.Services.Facade.BaseValueSegment.Operations\TAGov.Services.Facade.BaseValueSegment.Operations.csproj" --output "C:\output\Facade.BaseValueSegment\operations"


#Service.AssessmentEvent
& dotnet.exe restore "C:\Modules\Services\Service.AssessmentEvent\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.AssessmentEvent\"

& dotnet.exe publish "C:\Modules\Services\Service.AssessmentEvent\TAGov.Services.Core.AssessmentEvent.API\TAGov.Services.Core.AssessmentEvent.API.csproj" --output "C:\output\Service.AssessmentEvent" --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Service.AssessmentEvent\TAGov.Services.Core.AssessmentEvent.Operations\TAGov.Services.Core.AssessmentEvent.Operations.csproj" --output "C:\output\Service.AssessmentEvent\operations" --framework netcoreapp2.0 --self-contained --runtime win-x64


#Service.BaseValueSegment
& dotnet.exe restore "C:\Modules\Services\Service.BaseValueSegment\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.BaseValueSegment\"

& dotnet.exe publish "C:\Modules\Services\Service.BaseValueSegment\TAGov.Services.Core.BaseValueSegment.API\TAGov.Services.Core.BaseValueSegment.API.csproj" --output "C:\output\Service.BaseValueSegment" --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Service.BaseValueSegment\TAGov.Services.Core.BaseValueSegment.Operations\TAGov.Services.Core.BaseValueSegment.Operations.csproj" --output "C:\output\Service.BaseValueSegment\operations" --framework netcoreapp2.0 --self-contained --runtime win-x64


#Service.GrmEvent
& dotnet.exe restore "C:\Modules\Services\Service.GrmEvent\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.GrmEvent\"

& dotnet.exe publish "C:\Modules\Services\Service.GrmEvent\TAGov.Services.Core.GrmEvent.API\TAGov.Services.Core.GrmEvent.API.csproj" --output "C:\output\Service.GrmEvent" --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Service.GrmEvent\TAGov.Services.Core.GrmEvent.Operations\TAGov.Services.Core.GrmEvent.Operations.csproj" --output "C:\output\Service.GrmEvent\operations" --framework netcoreapp2.0 --self-contained --runtime win-x64


#Service.LegalParty
& dotnet.exe restore "C:\Modules\Services\Service.LegalParty\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.LegalParty\"

& dotnet.exe publish "C:\Modules\Services\Service.LegalParty\TAGov.Services.Core.LegalParty.API\TAGov.Services.Core.LegalParty.API.csproj" --output "C:\output\Service.LegalParty"
& dotnet.exe publish "C:\Modules\Services\Service.LegalParty\TAGov.Services.Core.LegalParty.Operations\TAGov.Services.Core.LegalParty.Operations.csproj" --output "C:\output\Service.LegalParty\operations"


#Service.LegalPartySearch
& dotnet.exe restore "C:\Modules\Services\Service.LegalPartySearch\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.LegalPartySearch\"

& dotnet.exe publish "C:\Modules\Services\Service.LegalPartySearch\TAGov.Services.Core.LegalPartySearch.API\TAGov.Services.Core.LegalPartySearch.API.csproj" --output "C:\output\Service.LegalPartySearch" --framework netcoreapp2.0 --self-contained --runtime win-x64
& dotnet.exe publish "C:\Modules\Services\Service.LegalPartySearch\TAGov.Services.Core.LegalPartySearch.Operations\TAGov.Services.Core.LegalPartySearch.Operations.csproj" --output "C:\output\Service.LegalPartySearch\operations" --framework netcoreapp2.0 --self-contained --runtime win-x64


#Service.RevenueObject
& dotnet.exe restore "C:\Modules\Services\Service.RevenueObject\" -s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global
& dotnet.exe build "C:\Modules\Services\Service.RevenueObject\"

& dotnet.exe publish "C:\Modules\Services\Service.RevenueObject\TAGov.Services.Core.RevenueObject.API\TAGov.Services.Core.RevenueObject.API.csproj" --output "C:\output\Service.RevenueObject"
& dotnet.exe publish "C:\Modules\Services\Service.RevenueObject\TAGov.Services.Core.RevenueObject.Operations\TAGov.Services.Core.RevenueObject.Operations.csproj" --output "C:\output\Service.RevenueObject\operations"


#Windows Services
& dotnet.exe restore "C:\Modules\Processes\Process.Sync.LegalPartySearch.Coordinator\TAGov.Process.Sync.LegalPartySearch.Coordinator.sln"-s http://api.nuget.org/v3/index.json -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -v detailed
& msbuild "C:\Modules\Processes\Process.Sync.LegalParty\Process.Sync.LegalPartySearch.sln" /t:Rebuild /p:outdir="C:\output\Process.Sync.LegalPartySearch" /p:Configuration=Release

& dotnet.exe restore "C:\Modules\Processes\Process.Sync.LegalPartySearch.Coordinator\TAGov.Process.Sync.LegalPartySearch.Coordinator.sln" -s https://bams-aws.refinitiv.com/artifactory/api/nuget/default.nuget.global -s http://api.nuget.org/v3/index.json  -v detailed
& msbuild "C:\Modules\Processes\Process.Sync.LegalPartySearch.Coordinator\TAGov.Process.Sync.LegalPartySearch.Coordinator.sln" /t:Rebuild /p:outdir="C:\output\Process.Sync.LegalPartySearch.Coordinator" /p:Configuration=Release


#####################################################################################################
#patch, copy xml doc file to folder
Copy-Item C:\Modules\Services\Common.Security\TAGov.Common.Security.API\bin\Debug\netcoreapp2.0\TAGov.Common.Security.API.xml C:\output\Common.Security
Copy-Item C:\Modules\Services\Service.LegalPartySearch\TAGov.Services.Core.LegalPartySearch.API\bin\Debug\netcoreapp2.0\TAGov.Services.Core.LegalPartySearch.API.xml C:\output\Service.LegalPartySearch

