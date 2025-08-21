/* ---------- Module: <YourModuleName> ---------- */
USE AgmelloECommerce;  -- Ensure the correct database context
/* Version target for this script */
DECLARE @ModuleName sysname = N'Users';
DECLARE @TargetVersion int = 1;
DECLARE @CurrentVersion int;


SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @LockName NVARCHAR(128) = N'schema_migration_' + @ModuleName;

BEGIN TRAN;
/* Optional: serialize concurrent runs to avoid races */
EXEC sp_getapplock 
     @Resource = @LockName, 
     @LockMode = 'Exclusive',
     @LockTimeout = 60000;  -- 60s

BEGIN TRY

    /* Ensure a row exists for this module */
    SELECT @CurrentVersion = v.VersionNumber
    FROM dbo.VersionTable v
    WHERE v.TableName = @ModuleName;

    IF @CurrentVersion IS NULL
    BEGIN
    PRINT 'Creating Users table...';
        /* create only if not already present */
        IF NOT EXISTS (
            SELECT 1
            FROM sys.tables
            WHERE name = @ModuleName AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN        
            CREATE TABLE dbo.Users
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_Users PRIMARY KEY 
                             DEFAULT NEWSEQUENTIALID(),
                USername         NVARCHAR(200)  NOT NULL,
                Email        NVARCHAR(200)  NOT NULL UNIQUE,
                Password NVARCHAR(200)  NOT NULL,
                Accessibility NVARCHAR(50) NULL DEFAULT 'customer',
                CreatedAt DATETIME2(0)  NOT NULL CONSTRAINT DF_Users_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
                LastLoginAt DATETIME2(0) NULL
            ); 
            CREATE INDEX IX_Users_Name ON dbo.Users(Name);
            CREATE INDEX IX_Users_Email ON dbo.Users(Email);
        END
        /* Insert version row */
        INSERT INTO dbo.VersionTable (TableName, VersionNumber)
        VALUES (@ModuleName, @TargetVersion);

        PRINT @ModuleName + ' initialized to version ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE IF @CurrentVersion < @TargetVersion
    BEGIN
        PRINT 'Upgrading Users table from version ' + CAST(@CurrentVersion AS nvarchar(10)) + ' to ' + CAST(@TargetVersion AS nvarchar(10));
        /* ---- upgrades from @CurrentVersion to @TargetVersion ---- */
        UPDATE dbo.VersionTable
        SET VersionNumber = @TargetVersion,
            ModifiedAtUtc = SYSUTCDATETIME()
        WHERE TableName = @ModuleName;

        PRINT @ModuleName + ' upgraded from ' 
                + CAST(@CurrentVersion AS NVARCHAR(10)) + ' to ' + CAST(@TargetVersion AS NVARCHAR(10));
   
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

EXEC sp_releaseapplock @Resource = @LockName;
