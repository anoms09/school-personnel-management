using Dapper;
using School.Personnel.Management.Interfaces;
using School.Personnel.Management.Models.Miscellaneous;
using School.Personnel.Management.Repositories.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace School.Personnel.Management.Repositories.AppAdmin
{
    public class PermissionRepository : BaseRepository
    {
        public PermissionRepository(IAppConfiguration appConfiguration) : base(appConfiguration)
        {

        }

        public async Task CreateNewPermission(Permission request)
        {
            long permissionId = 0;
            var parameters = new DynamicParameters();

            parameters.Add("@permission_name", request.PermissionName);
            parameters.Add("@permission_description", request.PermissionDescription);
            parameters.Add("@isAppAdminRole", request.IsAppAdminRole);

            permissionId =  await Save("usp_create_permission", parameters);

        }

        public async Task<Permission> GetPermissionById(long id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            return await GetAsync<Permission>("usp_get_permission_by_id", parameters);           
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissions(int pageNumber, int pageSize, string searchKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@pagesize", pageSize);

            if (!string.IsNullOrEmpty(searchKey))
                parameters.Add("@search_text", searchKey);

            return await GetListAsync<PermissionDto>("usp_search_permission", parameters);
        }
    }
}
