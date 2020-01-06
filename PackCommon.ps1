param($Version, $LocalNugetDirectory)

$projects = @(
	"Common.Security\TAGov.Common.Security.Claims"
    "Common.Security\TAGov.Common.Security.Http.Authorization"
    "Common\TAGov.Common.Caching",
    "Common\TAGov.Common.ExceptionHandler",
    "Common\TAGov.Common.HealthCheck",
    "Common\TAGov.Common.Http",
    "Common\TAGov.Common.Logging",
    "Common\TAGov.Common.Operations",
    "Common\TAGov.Common.Paging",
    "Common\TAGov.Common.Swagger",
    "Common\TAGov.Common.UrlService")

foreach($project in $projects) {
	.\PackDepends.ps1 -Version $Version -LocalNugetDirectory $LocalNugetDirectory -Project $project
}