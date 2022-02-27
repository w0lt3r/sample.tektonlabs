using Microsoft.EntityFrameworkCore;
using sample.tektonlabs.core.Models;

namespace sample.tektonlabs.infrastructure;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    { }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductProvider> ProductProviders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(x => x.Key);
        modelBuilder.Entity<Product>().HasMany(x => x.Providers);
        modelBuilder.Entity<ProductProvider>().HasKey(x => x.Key);
    }
}

