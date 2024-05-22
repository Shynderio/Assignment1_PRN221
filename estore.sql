-- Sample Staffs data
INSERT INTO [MyStore].[dbo].[Staffs] ([Name], [Password], [Role]) 
VALUES 
    ('John Doe', 'password123', 1),
    ('Jane Smith', 'pass123', 2),
    ('Michael Johnson', 'securepwd', 1),
    ('Emily Brown', 'mysecretpass', 2),
    ('William Davis', 'password123', 1),
    ('Sarah Wilson', 'pass123', 2),
    ('Christopher Taylor', 'securepwd', 1),
    ('Jessica Martinez', 'mysecretpass', 2),
    ('Daniel Anderson', 'password123', 1),
    ('Elizabeth Thomas', 'pass123', 2);

-- Sample Categories data
INSERT INTO [MyStore].[dbo].[Categories] ([CategoryName])
VALUES 
    ('Electronics'),
    ('Clothing'),
    ('Books'),
    ('Toys');

-- Sample Products data
INSERT INTO [MyStore].[dbo].[Products] ([ProductName], [CategoryId], [UnitPrice]) 
VALUES 
    ('Smartphone', 1, 500),
    ('Laptop', 1, 1000),
    ('T-Shirt', 2, 20),
    ('Jeans', 2, 50),
    ('Novel', 3, 15),
    ('Textbook', 3, 100),
    ('Action Figure', 4, 10),
    ('Board Game', 4, 30);

INSERT INTO [MyStore].[dbo].[Orders] ([OrderDate], [StaffId]) 
VALUES 
    ('2024-05-01 08:00:00', 2),
    ('2024-05-02 09:15:00', 3),
    ('2024-05-03 10:30:00', 4),
    ('2024-05-04 11:45:00', 5),
    ('2024-05-05 12:00:00', 7),
    ('2024-05-06 13:30:00', 2),
    ('2024-05-07 14:45:00', 3),
    ('2024-05-08 15:00:00', 9),
    ('2024-05-09 16:15:00', 1),
    ('2024-05-10 17:30:00', 10),
    ('2024-05-11 18:45:00', 10),
    ('2024-05-12 19:00:00', 2),
    ('2024-05-13 20:15:00', 6),
    ('2024-05-14 21:30:00', 4),
    ('2024-05-15 22:45:00', 8),
    ('2024-05-16 08:00:00', 6),
    ('2024-05-17 09:15:00', 9),
    ('2024-05-18 10:30:00', 2),
    ('2024-05-19 11:45:00', 3),
    ('2024-05-20 12:00:00', 4),
    ('2024-05-21 13:30:00', 6),
    ('2024-05-22 14:45:00', 10),
    ('2024-05-23 15:00:00', 5),
    ('2024-05-24 16:15:00', 8),
    ('2024-05-25 17:30:00', 7),
    ('2024-05-26 18:45:00', 10),
    ('2024-05-27 19:00:00', 7),
    ('2024-05-28 20:15:00', 5),
    ('2024-05-29 21:30:00', 6),
    ('2024-05-30 22:45:00', 4);

-- Sample OrderDetails data
INSERT INTO [MyStore].[dbo].[OrderDetails] ([OrderId], [ProductId], [Quantity], [UnitPrice]) 
VALUES 
    (1, 1, 2, 500),
    (1, 3, 3, 20),
    (2, 2, 1, 1000),
    (2, 4, 2, 50),
    (3, 5, 5, 15),
    (3, 7, 1, 10),
    (4, 6, 1, 100),
    (4, 8, 2, 30),
    (5, 1, 3, 500),
    (5, 7, 1, 10),
    (6, 2, 2, 1000),
    (6, 6, 1, 100),
    (7, 3, 4, 20),
    (7, 5, 2, 15),
    (8, 4, 3, 50),
    (8, 8, 1, 30),
    (9, 1, 1, 500),
    (9, 2, 1, 1000),
    (10, 3, 2, 20),
    (10, 4, 1, 50),
    (11, 5, 3, 15),
    (11, 6, 2, 100),
    (12, 7, 2, 10),
    (12, 8, 3, 30),
    (13, 1, 4, 500),
    (13, 3, 1, 20),
    (14, 2, 3, 1000),
    (14, 7, 1, 10),
    (15, 4, 2, 50),
    (15, 6, 2, 100);

-- Sample Orders data (as provided in the previous response)
