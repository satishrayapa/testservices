
echo *********** cleaning local micro services directories...... **********

REM CLEAN UP
del /q "C:\local_aa\sites\Common.ResourceLocator\*"
FOR /D %%p IN ("C:\local_aa\sites\Common.ResourceLocator\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Common.Security\*"
FOR /D %%p IN ("C:\local_aa\sites\Common.Security\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Facade.AssessmentHeader\*"
FOR /D %%p IN ("C:\local_aa\sites\Facade.AssessmentHeader\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Facade.BaseValueSegment\*"
FOR /D %%p IN ("C:\local_aa\sites\Facade.BaseValueSegment\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.AssessmentEvent\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.AssessmentEvent\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.BaseValueSegment\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.BaseValueSegment\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.GrmEvent\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.GrmEvent\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.LegalParty\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.LegalParty\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.LegalPartySearch\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.LegalPartySearch\*.*") DO rmdir "%%p" /s /q

del /q "C:\local_aa\sites\Service.RevenueObject\*"
FOR /D %%p IN ("C:\local_aa\sites\Service.RevenueObject\*.*") DO rmdir "%%p" /s /q

echo *********** getting micro services from master...... **********

REM COPY FILES
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Common.ResourceLocator C:\local_aa\sites\Common.ResourceLocator /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Common.Security C:\local_aa\sites\Common.Security /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Facade.AssessmentHeader C:\local_aa\sites\Facade.AssessmentHeader /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Facade.BaseValueSegment C:\local_aa\sites\Facade.BaseValueSegment /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.AssessmentEvent C:\local_aa\sites\Service.AssessmentEvent /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.BaseValueSegment C:\local_aa\sites\Service.BaseValueSegment /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.GrmEvent C:\local_aa\sites\Service.GrmEvent /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.LegalParty C:\local_aa\sites\Service.LegalParty /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.LegalPartySearch C:\local_aa\sites\Service.LegalPartySearch /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Service.RevenueObject C:\local_aa\sites\Service.RevenueObject /E

echo *********** setting local LOCALTEST .Net Core Enviroment variable...... **********

setx -m ASPNETCORE_ENVIRONMENT "LOCALTEST"

echo *********** restarting iis...... **********

REM RESTART IIS
IISRESET /RESTART

echo ALL DONE !!!! press any key to close window...

pause