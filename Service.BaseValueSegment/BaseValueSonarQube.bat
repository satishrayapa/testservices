SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.basevalue.microservice" /n:"Base Value Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\BaseValueCoverage.html"
msbuild /t:Rebuild Service.BaseValueSegment.sln
SonarQube.Scanner.MSBuild.exe end