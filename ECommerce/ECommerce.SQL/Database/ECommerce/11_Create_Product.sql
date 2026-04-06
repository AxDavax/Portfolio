CREATE TABLE Product (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    SpecialTag NVARCHAR(MAX) NULL,
    CategoryId INT NOT NULL,
    ImageUrl NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Product_Category_CategoryId
        FOREIGN KEY (CategoryId) REFERENCES Category(Id)
        ON DELETE CASCADE
);

GO

CREATE INDEX IX_Product_CategoryId
    ON Product (CategoryId);
GO