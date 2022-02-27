using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.core.Models
{
    public class ProductResponse
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public DateTime ExpireOn { get; set; }
        public double Price { get; set; }
        public List<ProductProvider> Providers { get; set; }
    }
}
