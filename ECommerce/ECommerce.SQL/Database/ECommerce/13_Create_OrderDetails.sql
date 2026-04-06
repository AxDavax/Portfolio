CREATE TABLE OrderDetail (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderHeaderId INT NOT NULL,
    ProductId INT NOT NULL,
    Count INT NOT NULL,
    Price FLOAT NOT NULL,
    ProductName NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_OrderDetail_OrderHeader_OrderHeaderId
        FOREIGN KEY (OrderHeaderId) REFERENCES OrderHeader(Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetail_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES Product(Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_OrderDetail_OrderHeaderId
    ON OrderDetail (OrderHeaderId);
GO

CREATE INDEX IX_OrderDetail_ProductId
    ON OrderDetail (ProductId);
GO