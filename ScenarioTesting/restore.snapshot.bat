echo *********** restoring snapshot...... **********

REM Restoring snapshot
SQLCmd -E -S localhost -Q "USE [MASTER] ALTER DATABASE LumenGold SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE LumenGold FROM DATABASE_SNAPSHOT = 'LumenGoldSnapshot';  ALTER DATABASE LumenGold SET MULTI_USER;"

echo ALL DONE !!!! press any key to close window...
pause