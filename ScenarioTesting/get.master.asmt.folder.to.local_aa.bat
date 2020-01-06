
IISRESET /STOP

del /q C:\local_aa\sites\Aumentum\UI\Roll\AsmtEventMaint\*"

xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\AsmtEventMaint\*.* C:\local_aa\sites\Aumentum\UI\Roll\AsmtEventMaint\ /F /Y

IISRESET /START

pause
