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


create procedure usp_create_permission
	@permission_name varchar (50),
	@permission_description varchar(100),
	@isAppAdminRole bit
as

insert into tbl_app_permissions (permission_name, permission_description, is_app_admin_role)
values (@permission_name, @permission_description, @isAppAdminRole) SELECT SCOPE_IDENTITY();
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