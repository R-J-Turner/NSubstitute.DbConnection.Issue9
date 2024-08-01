using DapperQueryBuilder;
using InterpolatedSql.Dapper.SqlBuilders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSubstitute.DbConnection.Issue9
{
    public class HostService : IHostService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IOptionsMonitor<DbOptions> _DbOptions;

        public HostService(IDbConnectionFactory dbConnectionFactory, IOptionsMonitor<DbOptions> optionsMonitor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _DbOptions = optionsMonitor;
        }

        public async Task<bool> GetBool(int id)
        {
            using(IDbConnection conn = _dbConnectionFactory.GetConnection(_DbOptions.CurrentValue.ConnectionString))
            {
                if(conn.State != ConnectionState.Open)
                    conn.Open();

                QueryBuilder branchesQuery = conn.QueryBuilder(
                    $@"
                                    SELECT COUNT(DISTINCT 1) AS Id 
                                    FROM Orders
                                    AND OrderId = {id}
                    ");

                bool result = await branchesQuery.ExecuteScalarAsync<bool>();

                return result;
            }
        }
    }
}
