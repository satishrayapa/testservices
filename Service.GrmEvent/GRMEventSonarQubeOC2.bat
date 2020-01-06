MSBuild.SonarQube.Runner.exe begin /k:"tr.aumentum.grmevent.microservice" /n:"GRM Event Microservice" /v:"1.0" /d:sonar.cs.opencover.reportsPaths="%CD%\results.xml"
dotnet Build  Service.GrmEvent.sln /t:Rebuild
MSBuild.SonarQube.Runner.exe end

