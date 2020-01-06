
del /q "C:\local_aa\sites\CA-Riverside-QA-SCL.7z"

REM COPY FILES
echo D|xcopy \\c460zvplumwb2.ecomqc.tlrg.com\Inetpub\wwwroot\CA-Riverside-QA-SCL.7z C:\local_aa\sites\ /E

del /q "C:\local_aa\sites\Aumentum\*"
FOR /D %%p IN ("C:\local_aa\sites\Aumentum\*.*") DO rmdir "%%p" /s /q

IISRESET /STOP

"C:\Program Files\7-Zip\7z.exe" x C:\local_aa\sites\CA-Riverside-QA-SCL.7z -oC:\local_aa\sites\Aumentum

start %~dp0ctt.exe s:C:\local_aa\sites\Aumentum\web.config t:%~dp0LumenTransform.config d:C:\local_aa\sites\Aumentum\web.config i

IISRESET /START

pause