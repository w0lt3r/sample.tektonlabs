using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.core.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(string Key);
        Task InsertAsync(T data);
        Task UpdateAsync(T data);
    }
}
