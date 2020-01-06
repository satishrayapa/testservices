SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.common.security" /n:"Common Security" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\SecurityCoverage.html"
msbuild /t:Rebuild Common.Security.sln
SonarQube.Scanner.MSBuild.exe end