SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.common" /n:"Common" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\CommonCoverage.html"
msbuild /t:Rebuild TAGov.Common.sln
SonarQube.Scanner.MSBuild.exe end