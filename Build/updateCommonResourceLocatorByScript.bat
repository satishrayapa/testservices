cd ..\Common.ResourceLocator\Repository\TAGov.Common.ResourceLocator.Repository
dotnet ef migrations script -i -o common.resourcelocator.migrate.sql
osql -E -S localhost -d Resource -i common.resourcelocator.migrate.sql
pause