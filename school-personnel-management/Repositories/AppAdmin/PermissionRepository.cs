using Dapper;
using School.Personnel.Management.Interfaces;
using School.Personnel.Management.Models.Miscellaneous;
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
    public class PermissionRepository
    {
        private readonly string _schoolDbConnectionString;

        public PermissionRepository(IAppConfiguration appConfiguration)
        {
            _schoolDbConnectionString = appConfiguration.PersonnelDbConnectionString;
        }

        public async Task CreateNewPermission(Permission request)
        {
            long permissionId = 0;
            using (var connection = new SqlConnection(_schoolDbConnectionString))
            {
                permissionId = await connection.ExecuteAsync(@"usp_create_permission", new
                {
                    permission_name = request.PermissionName,
                    permission_description = request.PermissionDescription,
                    isAppAdminRole = request.IsAppAdminRole
                }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }

        public async Task<Permission> GetPermissionById(long id)
        {
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                var permission = conn.QueryFirstOrDefaultAsync<Permission>(@"usp_get_permission_by_id", new { id },
                    commandType: CommandType.StoredProcedure);
                return await permission;
            }
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissions(int pageNumber, int pageSize, string searchKey)
        {
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                dynamic parameters = new ExpandoObject();

                parameters.pageNumber = pageNumber;
                parameters.pagesize = pageSize;

                if (!string.IsNullOrEmpty(searchKey))
                    parameters.search_text = searchKey;

                var permissions = conn.QueryAsync<PermissionDto>(
                    @"usp_search_permission", (object)parameters, commandType: CommandType.StoredProcedure);
                return await permissions;
            }
        }
    }
}
