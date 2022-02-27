using sample.tektonlabs.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.tektonlabs.infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _context;
        public UnitOfWork(MyContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
