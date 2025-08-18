cd ../Sql/
for /r %%v in (*.sql) do sqlcmd -i "%%v"
set /p delExit=Press the ENTER key to exit...: