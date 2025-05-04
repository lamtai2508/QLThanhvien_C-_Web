-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th5 04, 2025 lúc 02:26 PM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `qltv`
--
create database qltv;
use qltv;
-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `account`
--

CREATE TABLE `account` (
  `account_id` varchar(10) NOT NULL,
  `password` varchar(20) NOT NULL,
  `role` int(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `account`
--

INSERT INTO `account` (`account_id`, `password`, `role`) VALUES
('admin1', '123456', 0),
('TV001', '123456', '1'),
('TV002', '123456', '1'),
('TV003', '123456', '1');
-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `borroweddevices`
--

CREATE TABLE `borroweddevices` (
  `member_id` varchar(10) NOT NULL,
  `device_id` varchar(10) NOT NULL,
  `borrow_date` date NOT NULL,
  `due_date` date NOT NULL,
  `return_date` date DEFAULT NULL,
  `status` enum('Đã trả lại','Mất thiết bị/Hư','Đang mượn') NOT NULL DEFAULT 'Đang mượn'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `borroweddevices`
--




-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `devices`
--

CREATE TABLE `devices` (
  `device_id` varchar(10) NOT NULL,
  `device_name` varchar(30) NOT NULL,
  `device_type` varchar(20) NOT NULL,
  `status` enum('Có sẵn','Được đặt chỗ','Đang được mượn') NOT NULL DEFAULT 'Có sẵn'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `devices`
--

INSERT INTO `devices` (`device_id`, `device_name`, `device_type`, `status`) VALUES
('D001', 'Laptop Dell Latitude 5420', 'Laptop', 'có sẵn'),
('D002', 'Máy in Canon LBP2900', 'Printer', 'có sẵn'),
('D003', 'Màn hình Samsung 24"', 'Monitor', 'có sẵn'),
('D004', 'Bàn phím Logitech K120', 'Keyboard', 'có sẵn'),
('D005', 'Chuột không dây Logitech M185', 'Mouse', 'có sẵn'),
('D006', 'Router TP-Link Archer C6', 'Networking', 'có sẵn');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `memberhistory`
--

CREATE TABLE `memberhistory` (
  `member_id` varchar(10) NOT NULL,
  `device_id` varchar(10) NOT NULL,
  `violation_id` varchar(10) NOT NULL,
  `reservation_id` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `members`
--

CREATE TABLE `members` (
  `member_id` varchar(10) NOT NULL,
  `full_name` varchar(30) NOT NULL,
  `gender` varchar(4) NOT NULL,
  `number_phone` varchar(11) NOT NULL,
  `dob` date NOT NULL,
  `email` varchar(50) NOT NULL,
  `status` enum('Đang hoạt động','Đang bị cánh cáo','Đang bị tạm khóa','Khóa vĩnh viễn') NOT NULL DEFAULT 'Đang hoạt động'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `members`
--

INSERT INTO `members` (`member_id`, `full_name`, `gender`, `number_phone`, `dob`, `email`, `status`) VALUES
('admin1', 'admin', 'nam', '123456789', '2000-01-11', 'admin1@gmail.com', ''),
('TV001', 'Nguyễn Văn A', 'Nam', '0901234567', '1990-05-10', 'a.nguyen@example.com', 'Đang hoạt động'),
('TV002', 'Trần Thị B', 'Nữ', '0912345678', '1992-08-15', 'b.tran@example.com', 'Đang hoạt động'),
('TV003', 'Lê Văn C', 'Nam', '0923456789', '1988-12-20', 'c.le@example.com', 'Đang hoạt động'),
('TV004', 'Phạm Thị D', 'Nữ', '0934567890', '1995-03-25', 'd.pham@example.com', 'Đang hoạt động'),
('TV005', 'Hoàng Văn E', 'Nam', '0945678901', '1993-07-30', 'e.hoang@example.com', 'Đang hoạt động'),
('TV006', 'Đỗ Thị F', 'Nữ', '0956789012', '1991-11-05', 'f.do@example.com', 'Đang hoạt động');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `reservations`
--

CREATE TABLE `reservations` (
  `reservation_id` varchar(10) NOT NULL,
  `member_id` varchar(10) NOT NULL,
  `device_id` varchar(10) NOT NULL,
  `reservation_date` date NOT NULL,
  `borrowed_date` date NOT NULL,
  `returned_date` date NOT NULL,
  `status` enum('Chờ duyệt','Chấp nhận','Từ chối') DEFAULT 'Chờ duyệt'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `reservations`
--


-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `violations`
--

CREATE TABLE `violations` (
  `violation_id` varchar(10) NOT NULL,
  `member_id` varchar(10) NOT NULL,
  `violation_type` varchar(50) NOT NULL,
  `penalty` varchar(50) NOT NULL,
  `violation_date` date NOT NULL,
  `block_date` date DEFAULT NULL,
  `unblock_date` date DEFAULT NULL,
  `status` enum('Đang hoạt động','Phạt đền bù','Khóa tạm thời','Khóa vĩnh viễn') DEFAULT NULL,
  `warning_count` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `violations`
--



--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`account_id`);

--
-- Chỉ mục cho bảng `borroweddevices`
--
ALTER TABLE `borroweddevices`
  ADD PRIMARY KEY (`device_id`) USING BTREE,
  ADD KEY `member_id` (`member_id`);

--
-- Chỉ mục cho bảng `devices`
--
ALTER TABLE `devices`
  ADD PRIMARY KEY (`device_id`);

--
-- Chỉ mục cho bảng `memberhistory`
--
ALTER TABLE `memberhistory`
  ADD KEY `violation_id` (`violation_id`),
  ADD KEY `device_id` (`device_id`),
  ADD KEY `member_id` (`member_id`),
  ADD KEY `reservation_id` (`reservation_id`);

--
-- Chỉ mục cho bảng `members`
--
ALTER TABLE `members`
  ADD PRIMARY KEY (`member_id`);

--
-- Chỉ mục cho bảng `reservations`
--
ALTER TABLE `reservations`
  ADD PRIMARY KEY (`reservation_id`),
  ADD KEY `member_id` (`member_id`),
  ADD KEY `device_id` (`device_id`);

--
-- Chỉ mục cho bảng `violations`
--
ALTER TABLE `violations`
  ADD PRIMARY KEY (`violation_id`),
  ADD KEY `member_id` (`member_id`);

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `account`
--
ALTER TABLE `account`
  ADD CONSTRAINT `account_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `members` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `borroweddevices`
--
ALTER TABLE `borroweddevices`
  ADD CONSTRAINT `borroweddevices_ibfk_1` FOREIGN KEY (`device_id`) REFERENCES `devices` (`device_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `borroweddevices_ibfk_2` FOREIGN KEY (`member_id`) REFERENCES `members` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `memberhistory`
--
ALTER TABLE `memberhistory`
  ADD CONSTRAINT `memberhistory_ibfk_1` FOREIGN KEY (`violation_id`) REFERENCES `violations` (`violation_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `memberhistory_ibfk_2` FOREIGN KEY (`device_id`) REFERENCES `devices` (`device_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `memberhistory_ibfk_3` FOREIGN KEY (`member_id`) REFERENCES `members` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `memberhistory_ibfk_4` FOREIGN KEY (`reservation_id`) REFERENCES `reservations` (`reservation_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `reservations`
--
ALTER TABLE `reservations`
  ADD CONSTRAINT `reservations_ibfk_1` FOREIGN KEY (`member_id`) REFERENCES `members` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `reservations_ibfk_2` FOREIGN KEY (`device_id`) REFERENCES `devices` (`device_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Các ràng buộc cho bảng `violations`
--
ALTER TABLE `violations`
  ADD CONSTRAINT `violations_ibfk_1` FOREIGN KEY (`member_id`) REFERENCES `members` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
