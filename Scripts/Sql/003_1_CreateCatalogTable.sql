/* ---------- Module: <YourModuleName> ---------- */
USE AgmelloECommerce;  -- Ensure the correct database context
/* Version target for this script */
DECLARE @ModuleName sysname = N'Catalog';  -- e.g., 'Catalog'
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
        /* create only if not already present */
        IF NOT EXISTS (
            SELECT 1
            FROM sys.tables
            WHERE name = @ModuleName AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN        
            CREATE TABLE dbo.Catalog
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_Catalog PRIMARY KEY 
                             DEFAULT NEWSEQUENTIALID(),
                Name         NVARCHAR(200)  NOT NULL,
                Description  NVARCHAR(MAX)  NULL,
                Availability NVARCHAR(50)   NULL,
                Price        DECIMAL(18,2)  NOT NULL CONSTRAINT DF_Catalog_Price DEFAULT (0),
                ImageUrl     NVARCHAR(500)  NULL
            );
            CREATE INDEX IX_Catalog_Name ON dbo.Catalog(Name);
            -- Add many-to-many relationships
            CREATE TABLE dbo.Category
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_Category PRIMARY KEY
                             DEFAULT NEWSEQUENTIALID(),
                Name         NVARCHAR(100) NOT NULL,
                CreatedAtUtc DATETIME2(0)  NOT NULL CONSTRAINT DF_Category_CreatedAtUtc DEFAULT SYSUTCDATETIME()
            );
            -- prevent duplicate names (uses DB collation; add explicit CI collation if needed)
            CREATE UNIQUE INDEX UX_Category_Name ON dbo.Category(Name);

            CREATE TABLE dbo.Tag
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_Tag PRIMARY KEY
                             DEFAULT NEWSEQUENTIALID(),
                Name         NVARCHAR(100) NOT NULL,
                CreatedAtUtc DATETIME2(0)  NOT NULL CONSTRAINT DF_Tag_CreatedAtUtc DEFAULT SYSUTCDATETIME()
            );
            CREATE UNIQUE INDEX UX_Tag_Name ON dbo.Tag(Name);

            CREATE TABLE dbo.CatalogCategories
            (
                CatalogId  uniqueidentifier NOT NULL,
                CategoryId uniqueidentifier NOT NULL,
                CONSTRAINT PK_CatalogCategories PRIMARY KEY (CatalogId, CategoryId),
                CONSTRAINT FK_CatalogCategories_Catalog
                    FOREIGN KEY (CatalogId)  REFERENCES dbo.Catalog(Id)  ON DELETE CASCADE,
                CONSTRAINT FK_CatalogCategories_Category
                    FOREIGN KEY (CategoryId) REFERENCES dbo.Category(Id) ON DELETE CASCADE
            );
            -- accelerates "items by category"
            CREATE INDEX IX_CatalogCategories_CategoryId ON dbo.CatalogCategories(CategoryId, CatalogId);

            CREATE TABLE dbo.CatalogTags
            (
                CatalogId uniqueidentifier NOT NULL,
                TagId     uniqueidentifier NOT NULL,
                CONSTRAINT PK_CatalogTags PRIMARY KEY (CatalogId, TagId),
                CONSTRAINT FK_CatalogTags_Catalog
                    FOREIGN KEY (CatalogId) REFERENCES dbo.Catalog(Id) ON DELETE CASCADE,
                CONSTRAINT FK_CatalogTags_Tag
                    FOREIGN KEY (TagId)     REFERENCES dbo.Tag(Id)     ON DELETE CASCADE
            );
            -- accelerates "items by tag"
            CREATE INDEX IX_CatalogTags_TagId ON dbo.CatalogTags(TagId, CatalogId);

        END
        /* Insert version row */
        INSERT INTO dbo.VersionTable (TableName, VersionNumber)
        VALUES (@ModuleName, @TargetVersion);

        PRINT @ModuleName + ' initialized to version ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE IF @CurrentVersion < @TargetVersion
    BEGIN
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
