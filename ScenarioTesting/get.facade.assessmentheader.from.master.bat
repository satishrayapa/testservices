
echo *********** cleaning local micro services directories...... **********

REM CLEAN UP

del /q "C:\local_aa\sites\Facade.AssessmentHeader\*"
FOR /D %%p IN ("C:\local_aa\sites\Facade.AssessmentHeader\*.*") DO rmdir "%%p" /s /q

echo *********** getting micro services from master...... **********

REM COPY FILES

echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\deployedsites\Facade.AssessmentHeader C:\local_aa\sites\Facade.AssessmentHeader /E

echo *********** setting local LOCALTEST .Net Core Enviroment variable...... **********

setx -m ASPNETCORE_ENVIRONMENT "LOCALTEST"

echo *********** restarting iis...... **********

REM RESTART IIS
IISRESET /RESTART

echo ALL DONE !!!! press any key to close window...

pause