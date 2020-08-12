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
    public class FacultyRepository : BaseRepository
    {

        public FacultyRepository(IAppConfiguration appConfiguration) : base(appConfiguration)
        {

        }

        public async Task<IEnumerable<FacultyDto>> GetFaculties(int pageNumber, int pageSize, string searchKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@pagesize", pageSize);

            if (!string.IsNullOrEmpty(searchKey))
                parameters.Add("@search_text", searchKey);

            return await GetListAsync<FacultyDto>("usp_search_faculty", parameters);
        }

        public async Task<Faculty> GetFacultyById(long id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            return await GetAsync<Faculty>("usp_get_faculty_by_id", parameters);
        }

        public async Task<Faculty> GetFacultyByCode(string code)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            return await GetAsync<Faculty>("usp_get_faculty_by_code", parameters);
        }

        public async Task CreateNewFaculty(Faculty request)
        {
            long permissionId = 0;
            var parameters = new DynamicParameters();

            parameters.Add("@faculty_name", request.FacultyName);
            parameters.Add("@faculty_description", request.FacultyDescription);
            parameters.Add("@faculty_code", request.FacultyCode);

            permissionId = await Save("usp_create_faculty", parameters);
        }

        public async Task UpdateFaculty(Faculty request)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@id", request.Id);
            parameters.Add("@faculty_name", request.FacultyName);
            parameters.Add("@faculty_description", request.FacultyDescription);
            parameters.Add("@faculty_code", request.FacultyCode);

            await SaveOrUpdate("usp_update_faculty_by_id_code", parameters);
        }
    }
}
