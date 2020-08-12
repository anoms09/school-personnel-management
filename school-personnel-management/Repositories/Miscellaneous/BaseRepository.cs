using Dapper;
using School.Personnel.Management.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Repositories.Miscellaneous
{
    public class BaseRepository
    {
        private readonly string _schoolDbConnectionString;

        public BaseRepository(IAppConfiguration appConfiguration)
        {
            _schoolDbConnectionString = appConfiguration.PersonnelDbConnectionString;
        }

        public async Task<List<T>> GetListAsync<T>(string spName, DynamicParameters parameters = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                var result = await conn.QueryAsync<T>(spName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result.AsList();
            }
        }

        public async Task<T> GetAsync<T>(string spName, DynamicParameters parameters = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(spName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<long> Save(string spName, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                var identity = conn.QuerySingleAsync<long>(spName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return await identity;
            }
        }

        public void SaveVoid(string spName, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                conn.Execute(spName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<long> SaveOrUpdate(string spName, DynamicParameters parameters)
        {
            using (var conn = new SqlConnection(_schoolDbConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                var identity = conn.ExecuteAsync(spName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return await identity;
            }
        }
    }
}
