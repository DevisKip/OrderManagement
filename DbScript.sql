CREATE DATABASE OrderManagementDB;
GO

USE OrderManagementDB;
GO

-- Create Customers table
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Segment NVARCHAR(50) NOT NULL
);
GO

-- Create Orders table
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    ShippedAt DATETIME2 NULL,
    DeliveredAt DATETIME2 NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

-- Seed Customers
INSERT INTO Customers (Name, Segment) VALUES 
('Devis', 'VIP'),
('Mercy', 'Premium'),
('Carol', 'Regular');
GO

-- Seed Orders
INSERT INTO Orders (CustomerId, TotalAmount, Status, CreatedAt, ShippedAt, DeliveredAt) VALUES
-- Devis (CustomerId = 1) - Delivered orders to trigger discount logic
(1, 120.00, 'Delivered', GETDATE() - 20, GETDATE() - 19, GETDATE() - 18),
(1, 80.00, 'Delivered', GETDATE() - 17, GETDATE() - 16, GETDATE() - 15),
(1, 75.00, 'Delivered', GETDATE() - 14, GETDATE() - 13, GETDATE() - 12),
(1, 60.00, 'Delivered', GETDATE() - 11, GETDATE() - 10, GETDATE() - 9),
(1, 90.00, 'Delivered', GETDATE() - 8, GETDATE() - 7, GETDATE() - 6),
(1, 110.00, 'Delivered', GETDATE() - 5, GETDATE() - 4, GETDATE() - 3),
(1, 130.00, 'Processing', GETDATE() - 2, NULL, NULL),

-- Mercy (CustomerId = 2)
(2, 200.00, 'Processing', GETDATE() - 3, NULL, NULL),

-- Carol (CustomerId = 3)
(3, 150.00, 'Shipped', GETDATE() - 2, GETDATE() - 1, NULL);
GO
