using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSubstitute.DbConnection.Issue9
{
    public interface IDbConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString);
    }
}
