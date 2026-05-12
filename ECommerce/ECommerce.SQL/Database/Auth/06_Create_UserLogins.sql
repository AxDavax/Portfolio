/* 
    -- =============================================
    -- Create table: UserLogins
    -- Stores external OAuth provider associations
    -- =============================================
*/
CREATE TABLE UserLogins (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    ProviderUserId NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

ALTER TABLE UserLogins
ADD CONSTRAINT UQ_UserLogins_Provider UNIQUE (Provider, ProviderUserId);