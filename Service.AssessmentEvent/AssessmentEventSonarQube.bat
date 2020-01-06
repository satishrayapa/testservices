SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.assessmentevent.microservice" /n:"Assessment Event Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\AssessmentEventCoverage.html"
msbuild /t:Rebuild Service.AssessmentEvent.sln
SonarQube.Scanner.MSBuild.exe end