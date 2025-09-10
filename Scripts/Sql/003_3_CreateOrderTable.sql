/* ---------- Module: <YourModuleName> ---------- */
USE AgmelloECommerce;  -- Ensure the correct database context
/* Version target for this script */
DECLARE @ModuleName sysname = N'Orders';
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
    PRINT 'Creating Orders table...';
        /* create only if not already present */
        IF NOT EXISTS (
            SELECT 1
            FROM sys.tables
            WHERE name = @ModuleName AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN        
            CREATE TABLE dbo.Orders
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_Orders PRIMARY KEY 
                             DEFAULT NEWSEQUENTIALID(),
                UserId       uniqueidentifier NOT NULL, 
                CreatedAt    DATETIME2(0)  NOT NULL CONSTRAINT DF_Orders_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
                Status       NVARCHAR(50) NULL DEFAULT 'pending',
                TotalAmount  DECIMAL(18,2) NOT NULL DEFAULT 0.00
            ); 
            CREATE INDEX IX_Orders_UserId ON dbo.Orders(UserId);
        END
        /* create only if not already present */
        IF NOT EXISTS (
            SELECT 1
            FROM sys.tables
            WHERE name = 'OrderItems' AND schema_id = SCHEMA_ID('dbo')
        )
        BEGIN        
            CREATE TABLE dbo.OrderItems
            (
                Id           uniqueidentifier NOT NULL 
                             CONSTRAINT PK_OrderItems PRIMARY KEY 
                             DEFAULT NEWSEQUENTIALID(),
                OrderId      uniqueidentifier NOT NULL,
                CatalogItemId uniqueidentifier NOT NULL,
                CatalogItemName nvarchar(50) NOT NULL,
                Quantity     int NOT NULL DEFAULT 1,
                UnitPrice    DECIMAL(18,2) NOT NULL DEFAULT 0.00,

                CONSTRAINT FK_OrderItems_Orders
                    FOREIGN KEY (OrderId)
                    REFERENCES dbo.Orders(Id)
                    ON DELETE CASCADE
            ); 
            CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);

        END
        /* Insert version row */
        INSERT INTO dbo.VersionTable (TableName, VersionNumber)
        VALUES (@ModuleName, @TargetVersion);



        PRINT @ModuleName + ' initialized to version ' + CAST(@TargetVersion AS nvarchar(10));
    END
    ELSE IF @CurrentVersion < @TargetVersion
    BEGIN
        PRINT 'Upgrading Orders table from version ' + CAST(@CurrentVersion AS nvarchar(10)) + ' to ' + CAST(@TargetVersion AS nvarchar(10));
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
