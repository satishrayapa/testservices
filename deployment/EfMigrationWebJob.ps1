# Template for ef migration web job.
Push-Location D:\home\site\wwwroot\Operations

# TODO: Assert if successful because an error here wouldn't be known.
.\@APPNAME@.exe --ef-migrate
Pop-Location