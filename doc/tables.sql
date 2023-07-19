CREATE DATABASE "thomasgreg";
USE "thomasgreg";

CREATE TABLE "Customers" (
	"Id" INT PRIMARY KEY IDENTITY,
	"Name" NVARCHAR(80) NULL DEFAULT NULL,
	"Email" NVARCHAR(100) NULL DEFAULT NULL,
	"Logo" TEXT NULL DEFAULT NULL
);
ALTER TABLE Customers ADD CONSTRAINT UQ_Email UNIQUE (Email)

CREATE TABLE "CustomerAddresses" (
	"Id" INT PRIMARY KEY IDENTITY,
	"CustomerId" INT NULL FOREIGN KEY REFERENCES Customers(Id),
	"Street" VARCHAR(100) NULL DEFAULT NULL,
	"Number" VARCHAR(30) NULL DEFAULT NULL,
	"Complement" VARCHAR(30) NULL DEFAULT NULL,
	"ZipCode" VARCHAR(15) NULL DEFAULT NULL
);

CREATE TABLE "Users" (
	"Id" INT PRIMARY KEY IDENTITY,
	"Name" VARCHAR(100) NULL DEFAULT NULL,
	"Login" VARCHAR(30) NULL DEFAULT NULL,
	"Password" VARCHAR(30) NULL DEFAULT NULL,
	"Email" VARCHAR(100) NULL DEFAULT NULL
);

INSERT INTO "Customers" ("Name", "Email", "Logo") VALUES ('Adlênia Ribeiro', 'adleniaribeiro@gmail.com', '');
INSERT INTO "Customers" ("Name", "Email", "Logo") VALUES ('Dieter Marno', 'dieter@customdev.com.br', '');

INSERT INTO "CustomerAddresses" ("CustomerId", "Street", "Number", "Complement", "ZipCode") VALUES (1, 'Rua General Caldwell', '968', 'AP 303', '90130-050');
INSERT INTO "CustomerAddresses" ("CustomerId", "Street", "Number", "Complement", "ZipCode") VALUES (1, 'Rua Luiz de Camões', '864', 'AP 201', '90620-150');
INSERT INTO "CustomerAddresses" ("CustomerId", "Street", "Number", "Complement", "ZipCode") VALUES (2, 'Rua Marcílio Dias', '358', 'Ap 101', '90000-000');
INSERT INTO "CustomerAddresses" ("CustomerId", "Street", "Number", "Complement", "ZipCode") VALUES (2, 'Rua General Caldwell', '303', 'AP 303', '90130-050');

INSERT INTO "Users" ("Name", "Login", "Password", "Email") VALUES ('Dieter Marno', 'dietermarno', '12345', 'dieter@customdev.com.br');

dotnet ef dbcontext scaffold "Server=localhost;Database=thomasgreg;User Id=sa;Password=R353t3282@;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f -c AppDbContext
