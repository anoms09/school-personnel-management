using Dapper;
using School.Personnel.Management.Interfaces;
using School.Personnel.Management.Models.Staff;
using School.Personnel.Management.Repositories.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Repositories.Staff
{
    public class DepartmentRepository : BaseRepository
    {
        public DepartmentRepository(IAppConfiguration appConfiguration) : base(appConfiguration)
        {

        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartments(int pageNumber, int pageSize, string searchKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@pagesize", pageSize);

            if (!string.IsNullOrEmpty(searchKey))
                parameters.Add("@search_text", searchKey);

            return await GetListAsync<DepartmentDto>("usp_search_department", parameters);
        }

        public async Task<Department> GetDepartmentById(long id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            return await GetAsync<Department>("usp_get_department_by_id", parameters);
        }

        public async Task<Department> GetDepartmentByCode(string code)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            return await GetAsync<Department>("usp_get_department_by_code", parameters);
        }

        public async Task CreateNewDepartment(Department request)
        {
            long permissionId = 0;
            var parameters = new DynamicParameters();

            parameters.Add("@dept_name", request.DeptName);
            parameters.Add("@dept_description", request.DeptDescription);
            parameters.Add("@dept_code", request.DeptCode);
            parameters.Add("@faculty_code", request.FacultyCode);

            permissionId = await Save("usp_create_department", parameters);
        }

        public async Task UpdateDepartment(Department request)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@id", request.Id);
            parameters.Add("@dept_name", request.DeptName);
            parameters.Add("@dept_description", request.DeptDescription);
            parameters.Add("@dept_code", request.DeptCode);
            parameters.Add("@faculty_code", request.FacultyCode);

            await SaveOrUpdate("usp_update_department_by_id_code", parameters);
        }
    }
}
