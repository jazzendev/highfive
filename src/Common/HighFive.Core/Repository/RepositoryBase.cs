using System;
using Microsoft.Extensions.Logging;

namespace HighFive.Core.Repository
{
    public class RepositoryBase : IRepository
    {
        private readonly ILogger _logger;

        public RepositoryBase(ILogger logger)
        {
            _logger = logger;
        }
    }
}
