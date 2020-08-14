CREATE TABLE [dbo].[Employee](    
    [Id] [int] IDENTITY(1,1) NOT NULL,    
    [Name] [nvarchar](50) NULL,    
    [Position] [nvarchar](50) NULL,    
    [Office] [nvarchar](50) NULL,    
    [Age] [int] NULL,    
    [Salary] [int] NULL,    
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED     
(    
    [Id] ASC    
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]    
) ON [PRIMARY]    
GO  


CREATE PROCEDURE SpGetAllEmployees
AS
BEGIN
	SELECT * FROM NetCoreBaseAdo.dbo.Employee
END;

CREATE PROCEDURE SpAddEmployee
(
	@Name nvarchar(50),
	@Position nvarchar(50),    
	@Office nvarchar(50),    
	@Age int,    
	@Salary int
)
AS
BEGIN
	INSERT INTO Employee(Name, Position, Office, Age, Salary)
	VALUES(@Name, @Position, @Office, @Age, @Salary)
END;

CREATE PROCEDURE SpUpdateEmployee
(
	@Id INT,
	@Name nvarchar(50),
	@Position nvarchar(50),    
	@Office nvarchar(50),    
	@Age int,    
	@Salary int
)
AS
BEGIN	
	UPDATE Employee
	SET Name = @Name, Position = @Position, Office = @Office, Age = @Age, Salary = @Salary
	WHERE Id = @Id
END;

CREATE PROCEDURE SpDeleteEmployee
(
	@Id INT
)
AS
BEGIN
	Delete FROM NetCoreBaseAdo.dbo.Employee where Id = @Id
END;

USE [NetCoreBaseAdo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SpGetEmployeeById]
(
	@Id Int
)
AS
BEGIN
	SELECT * FROM NetCoreBaseAdo.dbo.Employee
END;


------------------- teste scripts da base ------------------------
exec dbo.SpGetEmployee

EXEC dbo.SpAddEmployee 'Anildo', 'Developer', 'Jundiai', 31, 15000

EXEC dbo.SpUpdateEmployee 1, 'Anildo XXX da Silva', 'Developer Sr.', 'Jundiai-SP', 35, 30000

EXEC SpDeleteEmployee 1