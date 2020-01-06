
IISRESET /STOP

del /q C:\local_aa\sites\Aumentum\UI\Roll\AsmtEventMaint\*"

xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\AsmtEventMaint\*.* C:\local_aa\sites\Aumentum\UI\Roll\AsmtEventMaint\ /F /Y
xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\BaseValueSegmentHistory\*.* C:\local_aa\sites\Aumentum\UI\Roll\BaseValueSegmentHistory\ /F /Y
xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\Shared\*.* C:\local_aa\sites\Aumentum\UI\Roll\Shared\ /F /Y
xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\Configuration\Roll\BaseValueSegmentHistory.config C:\local_aa\sites\Aumentum\Configuration\Roll\BaseValueSegmentHistory.config /F /Y


IISRESET /START

pause
