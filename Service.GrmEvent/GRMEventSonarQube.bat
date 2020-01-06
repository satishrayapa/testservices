SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.grmevent.microservice" /n:"GRM Event Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\GRMEventCoverage.html"
msbuild /t:Rebuild Service.GrmEvent.sln
SonarQube.Scanner.MSBuild.exe end