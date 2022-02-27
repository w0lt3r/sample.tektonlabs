using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.core.Interfaces
{
    public interface IExternalPriceProvider
    {
        Task<double> GetPrice(string key);
    }
}
