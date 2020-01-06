set netcoreappversion=netcoreapp1.1
set localnuget=\\c348zwraudvwb.ecomqc.tlrg.com\nuget
set remotenuget=https://api.nuget.org/v3/index.json
set /p serviceName="Please enter a name for the Service:"
cd ..
mkdir Service.%serviceName%
cd Service.%serviceName%
dotnet new sln --name Service.%serviceName%
mkdir API
mkdir Domain
mkdir Repository
cd API
dotnet new webapi --name TAGov.Services.Core.%serviceName%.API --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.API
dotnet add package Swashbuckle.AspNetCore -s %remotenuget%
dotnet add package TAGov.Common.ExceptionHandler -s %localnuget%
dotnet add package TAGov.Common.Http.Versioning -s %localnuget%
dotnet add package TAGov.Common.Logging -s %localnuget%
cd ..
dotnet new xUnit --name TAGov.Services.Core.%serviceName%.API.Test --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.API.Test
dotnet add package Moq -s %remotenuget%
dotnet add package Shouldly -s %remotenuget%
cd ..
cd ..
cd Domain
dotnet new classlib --name TAGov.Services.Core.%serviceName%.Domain --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.Domain
dotnet add package AutoMapper -s %remotenuget%
dotnet add package TAGov.Common.ExceptionHandler -s %localnuget%
dotnet add package TAGov.Common.Http -s %localnuget%
cd ..
dotnet new classlib --name TAGov.Services.Core.%serviceName%.Domain.Models --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.Domain.Models
cd ..
dotnet new xUnit --name TAGov.Services.Core.%serviceName%.Domain.Test --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.Domain.Test
dotnet add package Moq -s %remotenuget%
dotnet add package Shouldly -s %remotenuget%
cd ..
cd ..
cd Repository
dotnet new classlib --name TAGov.Services.Core.%serviceName%.Repository --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.Repository
dotnet add package Microsoft.EntityFrameworkCore -s %remotenuget%
dotnet add package Microsoft.EntityFrameworkCore.Design -s %remotenuget%
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -s %remotenuget%
dotnet add package Microsoft.EntityFrameworkCore.Tools -s %remotenuget%
dotnet add package Microsoft.Extensions.Configuration -s %remotenuget%
cd ..
dotnet new xUnit --name TAGov.Services.Core.%serviceName%.Repository.Test --framework %netcoreappversion%
cd TAGov.Services.Core.%serviceName%.Repository.Test
dotnet add package Moq -s %remotenuget%
dotnet add package Shouldly -s %remotenuget%
cd ..
cd ..
dotnet add API\TAGov.Services.Core.%serviceName%.API.Test\TAGov.Services.Core.%serviceName%.API.Test.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj
dotnet add API\TAGov.Services.Core.%serviceName%.API.Test\TAGov.Services.Core.%serviceName%.API.Test.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain.Models\TAGov.Services.Core.%serviceName%.Domain.Models.csproj
dotnet add API\TAGov.Services.Core.%serviceName%.API.Test\TAGov.Services.Core.%serviceName%.API.Test.csproj reference API\TAGov.Services.Core.%serviceName%.API\TAGov.Services.Core.%serviceName%.API.csproj
dotnet add API\TAGov.Services.Core.%serviceName%.API\TAGov.Services.Core.%serviceName%.API.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj
dotnet add Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.Test.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain.Models\TAGov.Services.Core.%serviceName%.Domain.Models.csproj
dotnet add Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.Test.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj
dotnet add Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj reference Domain\TAGov.Services.Core.%serviceName%.Domain.Models\TAGov.Services.Core.%serviceName%.Domain.Models.csproj
dotnet add Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj reference Repository\TAGov.Services.Core.%serviceName%.Repository\TAGov.Services.Core.%serviceName%.Repository.csproj
dotnet add Repository\TAGov.Services.Core.%serviceName%.Repository.Test\TAGov.Services.Core.%serviceName%.Repository.Test.csproj reference Repository\TAGov.Services.Core.%serviceName%.Repository\TAGov.Services.Core.%serviceName%.Repository.csproj
dotnet sln add API\TAGov.Services.Core.%serviceName%.API\TAGov.Services.Core.%serviceName%.API.csproj
dotnet sln add API\TAGov.Services.Core.%serviceName%.API.Test\TAGov.Services.Core.%serviceName%.API.Test.csproj
dotnet sln add Domain\TAGov.Services.Core.%serviceName%.Domain\TAGov.Services.Core.%serviceName%.Domain.csproj
dotnet sln add Domain\TAGov.Services.Core.%serviceName%.Domain.Models\TAGov.Services.Core.%serviceName%.Domain.Models.csproj
dotnet sln add Domain\TAGov.Services.Core.%serviceName%.Domain.Test\TAGov.Services.Core.%serviceName%.Domain.Test.csproj
dotnet sln add Repository\TAGov.Services.Core.%serviceName%.Repository\TAGov.Services.Core.%serviceName%.Repository.csproj
dotnet sln add Repository\TAGov.Services.Core.%serviceName%.Repository.Test\TAGov.Services.Core.%serviceName%.Repository.Test.csproj
pause