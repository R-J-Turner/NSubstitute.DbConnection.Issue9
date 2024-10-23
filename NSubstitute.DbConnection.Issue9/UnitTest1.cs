using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.DbConnection;
using System.Dynamic;

namespace NSubstitute.DbConnection.Issue9
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            IOptionsMonitor<DbOptions> dbOpt = Substitute.For<IOptionsMonitor<DbOptions>>();
            DbOptions o = new DbOptions() { ConnectionString = "ASASASAS" };
            dbOpt.CurrentValue.ReturnsForAnyArgs(o);


            IDbConnectionFactory conFac = Substitute.For<IDbConnectionFactory>();
            System.Data.Common.DbConnection connection = Substitute.For<System.Data.Common.DbConnection>()
                .SetupCommands();

            connection.SetupQuery(x => x.Contains("SELECT COUNT(DISTINCT 1) AS Id")).Returns(new { Id = true });

            conFac.GetConnection(default).ReturnsForAnyArgs(connection);

            HostService sut = new HostService(conFac, dbOpt);

            bool result = await sut.GetBool(1);

            Assert.True(result);
        }

        [Fact]
        public async void Test2()
        {
            System.Data.Common.DbConnection dbconn = Substitute.For<System.Data.Common.DbConnection>().SetupCommands();
            dbconn.SetupQuery(q => q.Contains("Select Distinct FileName"))
                .Returns(new { FileName = "File1" }, new { FileName = "File2" });
            IDbConnectionFactory connFactory = Substitute.For<IDbConnectionFactory>();
            connFactory.GetConnection(default).ReturnsForAnyArgs(dbconn);

            IOptionsMonitor<DbOptions> opt = Substitute.For<IOptionsMonitor<DbOptions>>();
            DbOptions o = new DbOptions() { ConnectionString = "Test" };
            opt.CurrentValue.ReturnsForAnyArgs(o);

            HostService sut = new HostService(connFactory, opt);

            List<string> ret = await sut.GetListOfString();

            Assert.NotEmpty(ret);
        }
    }
}