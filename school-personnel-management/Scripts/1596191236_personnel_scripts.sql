IF (OBJECT_ID('usp_create_permission') is not null)
BEGIN
	drop procedure usp_create_permission;
END
GO

IF (OBJECT_ID('usp_search_permission') is not null)
BEGIN
	drop procedure usp_search_permission;
END
GO

IF (OBJECT_ID('usp_get_permission_by_id') is not null)
BEGIN
	drop procedure usp_get_permission_by_id;
END
GO

IF (OBJECT_ID('usp_update_permission_by_id') is not null)
BEGIN
	drop procedure usp_update_permission_by_id;
END
GO


create procedure usp_create_permission
	@permission_name varchar (50),
	@permission_description varchar(100),
	@isAppAdminRole bit
as

insert into tbl_app_permissions (permission_name, permission_description, is_app_admin_role)
values (@permission_name, @permission_description, @isAppAdminRole) SELECT SCOPE_IDENTITY();
GO

create procedure usp_update_permission_by_id
	@id bigint,
	@permission_name varchar (50),
	@permission_description varchar(100),
	@isAppAdminRole bit
as

update tbl_app_permissions set permission_name = @permission_name, permission_description = @permission_description, is_app_admin_role = @isAppAdminRole
where id = @id
GO


create procedure usp_search_permission
@pageNumber int = 1,
@pageSize int,
@search_text varchar(50) = null
as
declare @from_row int = 1;

if @pageNumber > 1
begin
	set @from_row = ((@pageNumber * @pagesize) - (@pagesize)) + 1;
end;

with records as
(
select *,ROW_NUMBER()  over(order by created_at desc) as Row_Num from tbl_app_permissions (nolock)

where ((@search_text is null)
 or (permission_name like '%' + @search_text+ '%')
  or (permission_description like '%'+ @search_text+ '%')
 ) 
),
rec_count as
(
	select count(*) totalcount from records
)
select * from records,rec_count where Row_Num between @from_row and (@from_row + @pagesize -1) order by created_at  desc 
GO

create procedure usp_get_permission_by_id
	@id bigint
as
SELECT Top 1 * FROM tbl_app_permissions where id = @id;
GO

IF (OBJECT_ID('usp_create_faculty') is not null)
BEGIN
	drop procedure usp_create_faculty;
END
GO

IF (OBJECT_ID('usp_search_faculty') is not null)
BEGIN
	drop procedure usp_search_faculty;
END
GO

IF (OBJECT_ID('usp_get_faculty_by_id') is not null)
BEGIN
	drop procedure usp_get_faculty_by_id;
END
GO

IF (OBJECT_ID('usp_update_faculty_by_id_code') is not null)
BEGIN
	drop procedure usp_update_faculty_by_id_code;
END
GO

IF (OBJECT_ID('usp_get_faculty_by_code') is not null)
BEGIN
	drop procedure usp_get_faculty_by_code;
END
GO

create procedure usp_create_faculty
	@faculty_name varchar (50),
	@faculty_description text,
	@faculty_code varchar(30)
as

insert into tbl_faculty (faculty_name, faculty_description, faculty_code)
values (@faculty_name, @faculty_description, @faculty_code) SELECT SCOPE_IDENTITY();
GO

create procedure usp_update_faculty_by_id_code
	@id bigint,
	@faculty_name varchar (50),
	@faculty_description text,
	@faculty_code varchar(30)
as

update tbl_faculty set faculty_name = @faculty_name , faculty_description =@faculty_description, last_updated = getdate()
where id = @id and  faculty_code = @faculty_code
GO

create procedure usp_search_faculty
@pageNumber int = 1,
@pageSize int,
@search_text varchar(50) = null
as
declare @from_row int = 1;

if @pageNumber > 1
begin
	set @from_row = ((@pageNumber * @pagesize) - (@pagesize)) + 1;
end;

with records as
(
select *,ROW_NUMBER()  over(order by created_at desc) as Row_Num from tbl_faculty (nolock)

where ((@search_text is null)
 or (faculty_name like '%' + @search_text+ '%')
  or (faculty_description like '%'+ @search_text+ '%')
  or (faculty_code like '%'+ @search_text+ '%')
 ) 
),
rec_count as
(
	select count(*) totalcount from records
)
select * from records,rec_count where Row_Num between @from_row and (@from_row + @pagesize -1) order by created_at  desc 
GO

create procedure usp_get_faculty_by_id
	@id bigint
as
SELECT Top 1 * FROM tbl_faculty where id = @id;
GO

create procedure usp_get_faculty_by_code
	@code varchar(30)
as
SELECT Top 1 * FROM tbl_faculty where faculty_code = @code;
GO

IF (OBJECT_ID('usp_create_department') is not null)
BEGIN
	drop procedure usp_create_department;
END
GO

IF (OBJECT_ID('usp_search_department') is not null)
BEGIN
	drop procedure usp_search_department;
END
GO

IF (OBJECT_ID('usp_get_department_by_id') is not null)
BEGIN
	drop procedure usp_get_department_by_id;
END
GO

IF (OBJECT_ID('usp_update_department_by_id_code') is not null)
BEGIN
	drop procedure usp_update_department_by_id_code;
END
GO

IF (OBJECT_ID('usp_get_department_by_code') is not null)
BEGIN
	drop procedure usp_get_department_by_code;
END
GO

create procedure usp_create_department
	@dept_name varchar (50),
	@dept_description text,
	@dept_code varchar(30),
	@faculty_code varchar(30)
as

insert into tbl_department (dept_name, dept_description, dept_code, faculty_code)
values (@dept_name, @dept_description,@dept_code, @faculty_code) SELECT SCOPE_IDENTITY();
GO

create procedure usp_update_department_by_id_code
	@id bigint,
	@dept_name varchar (50),
	@dept_description text,
	@dept_code varchar(30),
	@faculty_code varchar(30)
as

update tbl_department set dept_name = @dept_name , dept_description =@dept_description, faculty_code =@faculty_code, last_updated = getdate()
where id = @id and  dept_code = @dept_code
GO

create procedure usp_search_department
@pageNumber int = 1,
@pageSize int,
@search_text varchar(50) = null
as
declare @from_row int = 1;

if @pageNumber > 1
begin
	set @from_row = ((@pageNumber * @pagesize) - (@pagesize)) + 1;
end;

with records as
(
select *,ROW_NUMBER()  over(order by created_at desc) as Row_Num from tbl_department (nolock)

where ((@search_text is null)
 or (dept_name like '%' + @search_text+ '%')
  or (dept_description like '%'+ @search_text+ '%')
  or (dept_code like '%'+ @search_text+ '%')
  or (faculty_code like '%'+ @search_text+ '%')
 ) 
),
rec_count as
(
	select count(*) totalcount from records
)
select * from records,rec_count where Row_Num between @from_row and (@from_row + @pagesize -1) order by created_at  desc 
GO

create procedure usp_get_department_by_id
	@id bigint
as
SELECT Top 1 * FROM tbl_department where id = @id;
GO

create procedure usp_get_department_by_code
	@code varchar(30)
as
SELECT Top 1 * FROM tbl_department where dept_code = @code;
GO

