using System;
using HighFive.Extensions.Dapper.Contrib;

namespace HighFive.Core.Model
{
    public class DatabaseModel<TKey> : IDatabaseModel<TKey>
    {
        [ExplicitKey]
        public TKey Id { get; set; }
        public bool IsValid { get; set; }
    }
}
