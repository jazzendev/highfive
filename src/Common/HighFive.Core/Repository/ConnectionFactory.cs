using HighFive.Core.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;

namespace HighFive.Core.Repository
{
    public sealed class ConnectionFactory : IConnectionFactory
    {
        private string _constr;

        public ConnectionFactory(IOptions<DbConnectionStringConfig> config)
        {
#if (DEBUG)
            _constr = config.Value.DefaultConnection;
#endif
#if (RELEASE)
            _constr = config.Value.ProductionConnection;
#endif

        }

        public DbConnection GetConnection()
        {
            var connection = new System.Data.SqlClient.SqlConnection(_constr);

            return connection;
        }
    }
}