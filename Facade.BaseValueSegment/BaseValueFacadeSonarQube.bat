SonarQube.Scanner.MSBuild.exe begin /k:"tr.aumentum.basevalue.facade" /n:"Base Value Facade" /v:"1.0" /d:sonar.cs.dotcover.reportsPaths="%CD%\BaseValueCoverage.html"
msbuild /t:Rebuild Facade.BaseValueSegment.sln
SonarQube.Scanner.MSBuild.exe end