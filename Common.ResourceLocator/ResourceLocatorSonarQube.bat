SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.resourcelocator" /n:"Common Resource Locator" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\ResourceLocatorCoverage.html"
msbuild /t:Rebuild Common.ResourceLocator.sln
SonarQube.Scanner.MSBuild.exe end