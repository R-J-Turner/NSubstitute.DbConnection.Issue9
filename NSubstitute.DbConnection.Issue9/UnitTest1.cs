using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.DbConnection;

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
    }
}