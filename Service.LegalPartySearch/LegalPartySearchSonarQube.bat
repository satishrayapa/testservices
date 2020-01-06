SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.legalpartysearch" /n:"Legal Party Search Microservice" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\LegalPartySearchCoverage.html"
msbuild /t:Rebuild Service.LegalPartySearch.sln
SonarQube.Scanner.MSBuild.exe end