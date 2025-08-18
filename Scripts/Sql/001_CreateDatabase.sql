DECLARE @dbName NVARCHAR(128) = 'AgmelloECommerce';

IF DB_ID(@dbName) IS NOT NULL
   print 'db exists'
ELSE
BEGIN
   DECLARE @sql NVARCHAR(MAX) = 'CREATE DATABASE [' + @dbName + ']';
   EXEC sp_executesql @sql;
   print 'db created'
END
GO