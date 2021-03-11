using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Core.Repository
{
    public interface ICRUDRepository<D>
    {
        IEnumerable<D> GetAll(bool? isValid = true);
        D Get(string id);
        D Create(D dto);
        D Update(D dto);
        bool Remove(string id);
        bool Delete(string id);
    }
}
