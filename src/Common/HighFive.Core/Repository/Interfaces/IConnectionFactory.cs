using System;
using System.Data.Common;

namespace HighFive.Core.Repository
{
    public interface IConnectionFactory
    {
        DbConnection GetConnection();
    }
}
