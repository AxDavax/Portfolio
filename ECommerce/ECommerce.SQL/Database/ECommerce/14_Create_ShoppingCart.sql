CREATE TABLE ShoppingCart (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    ProductId INT NOT NULL,
    Count INT NOT NULL,
    CONSTRAINT FK_ShoppingCart_AspNetUsers_UserId
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_ShoppingCart_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES Product(Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_ShoppingCart_ProductId
    ON ShoppingCart (ProductId);
GO

CREATE INDEX IX_ShoppingCart_UserId
    ON ShoppingCart (UserId);
GO