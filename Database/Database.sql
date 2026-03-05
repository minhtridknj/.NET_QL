-- Script tạo bảng và dữ liệu mẫu cho CSDL Quản lý áo quần

CREATE DATABASE QuanLyAoQuan;
GO

USE QuanLyAoQuan;
GO

CREATE TABLE AoQuan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(255) NOT NULL,
    Size NVARCHAR(50),
    GiaBan DECIMAL(18, 2) NOT NULL,
    SoLuong INT NOT NULL
);
GO

-- Insert dữ liệu mẫu
INSERT INTO AoQuan (TenSanPham, Size, GiaBan, SoLuong) VALUES
(N'Áo thun nam Cổ tròn', 'M', 150000, 50),
(N'Áo thun nam Cổ tròn', 'L', 150000, 30),
(N'Áo sơ mi tay dài', 'S', 250000, 20),
(N'Quần Jean rách gối', '32', 350000, 15),
(N'Quần Kaki túi hộp', '30', 280000, 40),
(N'Áo khoác gió', 'XL', 400000, 10),
(N'Váy công sở', 'M', 300000, 25),
(N'Áo len tay dài', 'L', 200000, 60),
(N'Quần đùi dạo phố', 'M', 100000, 80),
(N'Đầm dự tiệc', 'S', 500000, 5);
GO
