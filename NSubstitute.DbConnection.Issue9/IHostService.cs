using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSubstitute.DbConnection.Issue9
{
    public interface IHostService
    {
        public Task<bool> GetBool(int id);
    }
}
