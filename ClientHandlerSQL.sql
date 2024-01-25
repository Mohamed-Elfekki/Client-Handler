-- DDL

CREATE DATABASE ClientHandlerDB;

USE ClientHandlerDB;

CREATE TABLE Roles
(
id INT IDENTITY PRIMARY KEY,
role_name NVARCHAR(25) NOT NULL
);

CREATE TABLE Users
(
id INT IDENTITY PRIMARY KEY,
username nvarchar(256)  NOT NULL,
hashed_password NVARCHAR(MAX) NOT NULL,
role_id INT NOT NULL
CONSTRAINT fk_roles FOREIGN KEY(role_id) REFERENCES Roles(id)
);

CREATE TABLE Clients 
(
name NVARCHAR(50) NOT NULL,
national_id NVARCHAR(14) PRIMARY KEY,
phone_number NVARCHAR(11) NOT NULL,
governorate NVARCHAR(50) NOT NULL,
center NVARCHAR(50) NOT NULL,
village_section NVARCHAR(50) NOT NULL,
salary  DECIMAL(19,4) NOT NULL,
user_id INT NOT NULL,

CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES Users(id),
CONSTRAINT check_national_id CHECK(national_id LIKE '[2-3]_____________'),
CONSTRAINT check_phone CHECK(phone_number LIKE '01_________'),
CONSTRAINT check_salary CHECK(salary BETWEEN 5000 AND 20000)
);


CREATE TABLE Governorates
(
id INT IDENTITY PRIMARY KEY,
governorate_name NVARCHAR(50) NOT NULL
);

CREATE TABLE Cities
(
id INT IDENTITY PRIMARY KEY,
city_name NVARCHAR(50) NOT NULL,
governorate_id INT NOT NULL,
CONSTRAINT fk_governorate FOREIGN KEY(governorate_id) REFERENCES Governorates(id)
);

CREATE TABLE Villages
(
id INT IDENTITY PRIMARY KEY,
village_name NVARCHAR(50) NOT NULL,
city_id INT NOT NULL,
CONSTRAINT fk_city FOREIGN KEY (city_id) REFERENCES Cities(id)
);

ALTER TABLE Clients
DROP COLUMN governorate, center, village_section;

ALTER TABLE Clients
ADD 
city_id INT FOREIGN KEY REFERENCES Cities(id),
village_id INT FOREIGN KEY REFERENCES Villages(id);


-- DML

DELETE FROM Governorates;
INSERT INTO Governorates(governorate_name) VALUES
(N'القاهرة'),
(N'الإسكندرية'),
(N'الإسماعيلية'),
(N'أسوان'),
(N'أسيوط'),
(N'الأقصر'),
(N'البحر الأحمر'),
(N'البحيرة'),
(N'بني سويف'),
(N'بور سعيد'),
(N'جنوب سيناء'),
(N'الجيزة'),
(N'الدقهلية'),
(N'دمياط'),
(N'سوهاج'),
(N'السويس'),
(N'الشرقية'),
(N'الغربية'),
(N'الفيوم'),
(N'القليوبية'),
(N'قنا'),
(N'كفر الشيخ'),
(N'شمال سيناء'),
(N'مطروح'),
(N'المنوفية'),
(N'المنيا'),
(N'الوادي الجديد');
