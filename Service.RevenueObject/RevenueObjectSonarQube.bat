SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.revenueobject.microservice" /n:"Revenue Object Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\RevenueObjectCoverage.html"
msbuild /t:Rebuild Service.RevenueObject.sln
SonarQube.Scanner.MSBuild.exe end