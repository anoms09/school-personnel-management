IF DB_ID('school_personnel_management') IS NULL
BEGIN
    CREATE DATABASE school_personnel_management;
END


IF OBJECT_ID('tbl_student_biodata','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_student_biodata](
	[id] bigint NOT NULL IDENTITY(1, 1) UNIQUE,
	[title] varchar (50)NOT NULL,
	[matric_number] varchar (50) NOT NULL PRIMARY KEY,	
	[first_name] varchar (50)NOT NULL,
	[middle_name] varchar (50)NOT NULL,
	[last_name] varchar (50)NOT NULL,
	[level] int NOT NULL,
	[phone] varchar (30) NOT NULL,
	[email] varchar (250) UNIQUE NOT NULL,
	[faculty_code] varchar (30) NOT NULL,
	[dept_code] varchar (30) NOT NULL,
	[gender] varchar (30) NOT NULL,	
	[dob] Date NOT NULL,	
	[contact_address_main] Text NOT NULL,
	[contact_address_backup] Text NULL,
	[state_of_origin] varchar (250) NOT NULL,
	[nationality] varchar (250) NOT NULL,
	[admission_status] bit default(0),
	[year_of_admission] bigint NOT NULL,
	[year_of_exit] bigint NULL,
	[cause_of_exit] text NULL,
	[verified] bit default(0),
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_student_sponsor','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_student_sponsor](
	[id] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[matric_number] varchar (50) NOT NULL  REFERENCES dbo.tbl_student_biodata(matric_number) ON DELETE CASCADE ON UPDATE CASCADE,
	[title] varchar (50)NOT NULL,
	[first_name] varchar (50)NOT NULL,
	[last_name] varchar (50)NOT NULL,
	[phone] varchar (30) NOT NULL,
	[relationship] varchar (30) NOT NULL,
	[occupation] varchar (30) NOT NULL,
	[email] varchar (250) NOT NULL,
	[contact_address_main] Text NOT NULL,
	[contact_address_backup] Text NULL,
	[state_of_origin] varchar (250) NOT NULL,
	[nationality] varchar (250) NOT NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_student_nextofkin','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_student_nextofkin](
	[id] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[matric_number] varchar (50) NOT NULL  REFERENCES dbo.tbl_student_biodata(matric_number) ON DELETE CASCADE ON UPDATE CASCADE,
	[title] varchar (50)NOT NULL,
	[first_name] varchar (50)NOT NULL,
	[last_name] varchar (50)NOT NULL,
	[phone] varchar (30) NOT NULL,
	[relationship] varchar (30) NOT NULL,
	[occupation] varchar (30) NOT NULL,
	[email] varchar (250) NOT NULL,
	[contact_address_main] Text NOT NULL,
	[contact_address_backup] Text NULL,
	[state_of_origin] varchar (250) NOT NULL,
	[nationality] varchar (250) NOT NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_staff_biodata','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_staff_biodata](
	[id] bigint NOT NULL IDENTITY(1, 1) UNIQUE,
	[title] varchar (50)NOT NULL,
	[staff_id] varchar (50) NOT NULL PRIMARY KEY,	
	[first_name] varchar (50)NOT NULL,
	[middle_name] varchar (50)NOT NULL,
	[last_name] varchar (50)NOT NULL,
	[email] varchar (250) UNIQUE NOT NULL,
	[phone] varchar (30) NOT NULL,
	[roleId] bigint NOT NULL,
	[employment_level] bigint NOT NULL,	
	[faculty_code] varchar (30) NOT NULL,
	[dept_code] varchar (30) NOT NULL,	
	[gender] varchar (30) NOT NULL,
	[marital_status] varchar (30) NOT NULL,
	[dob] Date NOT NULL,
	[contact_address_main] Text NOT NULL,
	[contact_address_backup] Text NULL,
	[state_of_origin] varchar (250) NOT NULL,
	[nationality] varchar (250) NOT NULL,
	[description] text NULL,
	[employment_status] varchar(50) default('active'),
	[employment_type] varchar(50) default('fulltime'),
	[year_of_admission] bigint NOT NULL,
	[year_of_exit] bigint NULL,
	[cause_of_exit] text NULL,
	[verified] bit default(0),
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_staff_nextofkin','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_staff_nextofkin](
	[id] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[staff_id] varchar (50) NOT NULL  REFERENCES dbo.tbl_staff_biodata(staff_id) ON DELETE CASCADE ON UPDATE CASCADE,
	[title] varchar (50)NOT NULL,
	[first_name] varchar (50)NOT NULL,
	[last_name] varchar (50)NOT NULL,
	[phone] varchar (30) NOT NULL,
	[relationship] varchar (30) NOT NULL,
	[occupation] varchar (30) NOT NULL,
	[email] varchar (250) NOT NULL,
	[contact_address_main] Text NOT NULL,
	[contact_address_backup] Text NULL,
	[state_of_origin] varchar (250) NOT NULL,
	[nationality] varchar (250) NOT NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_staff_roles','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_staff_roles](
	[roleId] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[role_name] varchar (50)NOT NULL,
	[role_description] varchar (250)NULL,
	[faculty_code] varchar (30) NOT NULL,
	[dept_code] varchar (30) NOT NULL,
	[role_manager_staff_number] varchar (50) NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_faculty','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_faculty](
	[id] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[name] varchar (50)NOT NULL,
	[description] Text NULL,
	[faculty_code] varchar (30) UNIQUE NOT NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go

IF OBJECT_ID('tbl_department','U') IS NULL 
begin
CREATE TABLE [dbo].[tbl_department](
	[id] bigint NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[name] varchar (50)NOT NULL,
	[description] Text NULL,
	[dept_code] varchar (30) UNIQUE NOT NULL,
	[faculty_code] varchar (30) NOT NULL,
	[created_at] [datetime] default(getdate()),
	[last_updated] [datetime] default(getdate())
) ON [PRIMARY]
end
go