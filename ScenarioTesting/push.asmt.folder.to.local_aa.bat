
IISRESET /STOP

xcopy C:\Modules\MSIPackaging\BuildTools\Build\Dist\webroot\UI\Roll\AsmtEventMaint\*.* C:\local_aa\sites\Aumentum\UI\Roll\AsmtEventMaint\ /F /Y
xcopy C:\Modules\MSIPackaging\BuildTools\Build\Dist\webroot\UI\Roll\Shared\*.* C:\local_aa\sites\Aumentum\UI\Roll\Shared\ /F /Y
xcopy C:\Modules\MSIPackaging\BuildTools\Build\Dist\webroot\UI\Roll\BaseValueSegmentHistory\*.* C:\local_aa\sites\Aumentum\UI\Roll\BaseValueSegmentHistory\ /F /Y
xcopy C:\Modules\MSIPackaging\BuildTools\Build\Dist\webroot\Configuration\Roll\*.* C:\local_aa\sites\Aumentum\Configuration\Roll\ /F /Y

IISRESET /START

pause
