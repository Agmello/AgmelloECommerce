/* ---------- Module: <YourModuleName> ---------- */
/* Version target for this script */
DECLARE @ModuleName sysname = N'<YourModuleName>';  -- e.g., 'Catalog'
DECLARE @TargetVersion int = 1;
DECLARE @CurrentVersion int;

SET NOCOUNT ON;
SET XACT_ABORT ON;

/* Optional: serialize concurrent runs to avoid races */
EXEC sp_getapplock 
     @Resource = 'schema_migration_' + @ModuleName, 
     @LockMode = 'Exclusive',
     @LockTimeout = 60000;  -- 60s

BEGIN TRY
    BEGIN TRAN;

    /* Ensure a row exists for this module */
    SELECT @CurrentVersion = VersionNumber
    FROM dbo.VersionTable
    WHERE TableName = @ModuleName;

    IF @CurrentVersion IS NULL
    BEGIN
        /* ---- v1 bootstrap (initial create) ---- */
        /* put all CREATE SCHEMA/TABLE statements for v1 here */
        /* example:
           IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'MySchema')
               EXEC('CREATE SCHEMA MySchema');
        */

        /* ... your DDL ... */

        /* Insert version row */
        INSERT INTO dbo.VersionTable (TableName, VersionNumber)
        VALUES (@ModuleName, @TargetVersion);

        PRINT @ModuleName + ' initialized to version ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE IF @CurrentVersion < @TargetVersion
    BEGIN
        /* ---- upgrades from @CurrentVersion to @TargetVersion ---- */
        /* Example pattern:
           IF @CurrentVersion = 1 AND @TargetVersion >= 2
           BEGIN
               -- ALTERs for v2
               UPDATE dbo.VersionTable SET VersionNumber = 2, ModifiedAtUtc = SYSUTCDATETIME()
               WHERE TableName = @ModuleName;
               SET @CurrentVersion = 2;
           END

           IF @CurrentVersion = 2 AND @TargetVersion >= 3
           BEGIN
               -- ALTERs for v3
               UPDATE dbo.VersionTable SET VersionNumber = 3, ModifiedAtUtc = SYSUTCDATETIME()
               WHERE TableName = @ModuleName;
               SET @CurrentVersion = 3;
           END
        */
    END
    ELSE
    BEGIN
        PRINT @ModuleName + ' already at version ' + CAST(@CurrentVersion AS nvarchar(10));
    END

    COMMIT;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK;
    DECLARE @msg nvarchar(4000) = ERROR_MESSAGE();
    RAISERROR(N'[%s] migration failed: %s', 16, 1, @ModuleName, @msg);
END CATCH;

EXEC sp_releaseapplock @Resource = 'schema_migration_' + @ModuleName;
