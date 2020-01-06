cd ..\..\
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\tf" get $/AumentumTax/Modules /recursive
powershell -ExecutionPolicy Bypass -NoProfile -File ./build.ps1 clean
powershell -ExecutionPolicy Bypass -NoProfile -File ./build.ps1 buildsources
"%programfiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\tf" history . /r /noprompt /stopafter:1 /version:W
pause