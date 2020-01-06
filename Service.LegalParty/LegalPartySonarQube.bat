SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.legalparty" /n:"Legal Party Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\LegalPartyCoverage.html"
msbuild /t:Rebuild Service.LegalParty.sln
SonarQube.Scanner.MSBuild.exe end