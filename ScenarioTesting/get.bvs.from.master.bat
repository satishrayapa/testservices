
echo cleaning...

REM CLEAN UP
del /q "C:\local_aa\sites\Aumentum\CA-Riverside-QA-SCL\UI\Roll\AsmtEventMaint\*"

del /q "C:\local_aa\sites\Aumentum\CA-Riverside-QA-SCL\UI\Roll\BaseValueSegmentHistory\*"

REM COPY FILES
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\AsmtEventMaint C:\local_aa\sites\Aumentum\CA-Riverside-QA-SCL\UI\Roll\AsmtEventMaint /E
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL\UI\Roll\BaseValueSegmentHistory C:\local_aa\sites\Aumentum\CA-Riverside-QA-SCL\UI\Roll\BaseValueSegmentHistory /E

pause