
echo *********** recreating user to database from login...... **********

SQLCmd -E -S localhost -Q "USE LumenGold CREATE USER [IIS APPPOOL\DefaultAppPool] FROM LOGIN [IIS APPPOOL\DefaultAppPool] exec sp_addrolemember 'db_owner', 'IIS APPPOOL\DefaultAppPool' CREATE USER [IIS APPPOOL\LumenPool] FROM LOGIN [IIS APPPOOL\LumenPool] exec sp_addrolemember 'db_owner', 'IIS APPPOOL\LumenPool' CREATE USER [Lum3n] FROM LOGIN [Lum3n] exec sp_addrolemember 'db_owner', 'Lum3n';"

