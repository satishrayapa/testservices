SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.assessmentheader.facade" /n:"Assessment Header Facade" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\AsmtHeaderCoverage.html"
msbuild /t:Rebuild Facade.AssessmentHeader.sln
SonarQube.Scanner.MSBuild.exe end