using Microsoft.EntityFrameworkCore;
using sample.tektonlabs.core.Interfaces;
using sample.tektonlabs.core.Models;

namespace sample.tektonlabs.infrastructure.Implementations;
public class Repository : IRepository<Product>
{
    private readonly MyContext _context;
    public Repository(MyContext context)
    {
        _context = context;
    }
    public async Task<Product> GetAsync(string Key)
    {
        return await _context.Set<Product>().Include(x => x.Providers).Where(x => x.Key == Key).AsNoTracking().SingleOrDefaultAsync();
    }

    public async Task InsertAsync(Product data)
    {
        await _context.AddAsync(data);
    }

    public async Task UpdateAsync(Product data)
    {
        _context.Update(data);
    }
}
