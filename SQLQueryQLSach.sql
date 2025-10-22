CREATE DATABASE QLSach;
GO
USE QLSach;
GO

-- Bảng LoaiSach
CREATE TABLE LoaiSach (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(100) NOT NULL
);

-- Bảng Sach
CREATE TABLE Sach (
    MaSach CHAR(6) PRIMARY KEY,
    TenSach NVARCHAR(200) NOT NULL,
    NamXB INT,
    MaLoai INT FOREIGN KEY REFERENCES LoaiSach(MaLoai),
    HinhAnh NVARCHAR(255)
);

-- Dữ liệu mẫu
INSERT INTO LoaiSach (TenLoai)
VALUES (N'Khoa học'), (N'Văn học'), (N'Thiếu nhi'), (N'Lịch sử');

INSERT INTO Sach (MaSach, TenSach, NamXB, MaLoai, HinhAnh)
VALUES
('S00001', N'Vũ trụ trong vỏ hạt dẻ', 2020, 1, 'vutru.png'),
('S00002', N'Truyện Kiều', 2015, 2, 'kieu.png'),
('S00003', N'Tấm Cám', 2018, 3, 'tamcam.png'),
('S00004', N'Lịch sử Việt Nam', 2021, 4, 'lichsu.png'),
('S00005', N'Tiểu thuyết 1984', 2017, 2, '1984.png');

DROP TABLE Sach;