CREATE DATABASE C21_DB1;
GO

USE C21_DB1;
GO

-- Create the Departments table
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    Name VARCHAR(50)
);
GO

-- Insert sample data
INSERT INTO Departments (DepartmentID, Name)
VALUES
    (1, 'Human Resources'),
    (2, 'Marketing'),
    (3, 'Sales'),
    (4, 'IT');
GO

-- Create the Employees table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    Name VARCHAR(50),
    DepartmentID INT,
    HireDate DATE,
    FOREIGN KEY (DepartmentID)
        REFERENCES Departments(DepartmentID)
);
GO

-- Insert sample data
INSERT INTO Employees
    (EmployeeID, Name, DepartmentID, HireDate)
VALUES
    (1, 'John Smith', 3, '2023-01-10'),
    (2, 'Jane Doe', 3, '2023-02-15'),
    (3, 'Emily Davis', 2, '2023-03-20'),
    (4, 'Michael Brown', 1, '2022-11-05'),
    (5, 'Sarah Miller', 4, '2023-01-05');
GO
