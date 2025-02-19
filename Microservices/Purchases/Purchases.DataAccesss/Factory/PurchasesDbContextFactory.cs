using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Purchases.DataAccesss.DataContext;

namespace Purchases.DataAccesss.Factory;

public class PurchasesDbContextFactory : IDesignTimeDbContextFactory<PurchasesDbContext>
{
    public PurchasesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PurchasesDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=postgres;Port=5432;Username=postgres;password=admin;database=homeproject-cars-purchase");
        
        // optionsBuilder.UseNpgsql(
        //     "Host=localhost;Port=5432;Username=postgres;password=admin;database=homeproject-cars-purchase");
        
        return new PurchasesDbContext(optionsBuilder.Options);
    }
}