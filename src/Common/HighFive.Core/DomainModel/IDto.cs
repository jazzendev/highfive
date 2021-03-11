using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Core.DomainModel
{
    public interface IDto
    {
        string Id { get; set; }
        bool IsValid { get; set; }
    }
}
