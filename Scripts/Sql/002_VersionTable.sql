SET NOCOUNT ON;

DECLARE @TargetVersion int = 1;
DECLARE @CurrentVersion int;

-- Ensure version table exists
IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'VersionTable' AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE dbo.VersionTable
    (
        TableName     sysname      NOT NULL CONSTRAINT PK_VersionTable PRIMARY KEY,
        VersionNumber int          NOT NULL,
        ModifiedAtUtc datetime2(0) NOT NULL CONSTRAINT DF_VersionTable_ModifiedAtUtc DEFAULT (SYSUTCDATETIME())
    );

    INSERT INTO dbo.VersionTable (TableName, VersionNumber)
    VALUES ('Version', @TargetVersion);

    PRINT 'VersionTable created and initialized to version ' + CAST(@TargetVersion AS nvarchar(10));
END
ELSE
BEGIN
    SELECT @CurrentVersion = v.VersionNumber
    FROM dbo.VersionTable v
    WHERE v.TableName = 'Version';

    IF @CurrentVersion IS NULL
    BEGIN
        INSERT INTO dbo.VersionTable (TableName, VersionNumber)
        VALUES ('Version', @TargetVersion);

        PRINT 'Version row inserted at version ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE IF @CurrentVersion < @TargetVersion
    BEGIN
        UPDATE dbo.VersionTable
        SET VersionNumber = @TargetVersion,
            ModifiedAtUtc = SYSUTCDATETIME()
        WHERE TableName = 'Version';

        PRINT 'Version upgraded from ' + CAST(@CurrentVersion AS nvarchar(10))
              + ' to ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE
    BEGIN
        PRINT 'Already at version ' + CAST(@CurrentVersion AS nvarchar(10));
    END
END
