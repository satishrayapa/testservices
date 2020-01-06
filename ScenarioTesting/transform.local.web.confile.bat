echo *********** applying local transformation to web.config...... **********

start %~dp0ctt.exe s:C:\local_aa\sites\Aumentum\web.config t:%~dp0LumenTransform.config d:C:\local_aa\sites\Aumentum\web.config i

echo *********** restarting iis...... **********

REM RESTART IIS
IISRESET /RESTART

echo ALL DONE !!!! press any key to close window...

pause
