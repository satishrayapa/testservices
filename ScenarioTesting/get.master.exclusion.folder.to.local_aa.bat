
IISRESET /STOP

del /q C:\local_aa\sites\Aumentum\UI\Roll\ExclusionMaintenance\*"

xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\ExclusionMaintenance\*.* C:\local_aa\sites\Aumentum\UI\Roll\ExclusionMaintenance\ /F /Y

IISRESET /START

pause
