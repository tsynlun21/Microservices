using Microsoft.EntityFrameworkCore;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.DataAccesss.DataContext;

public class PurchasesDbContext : DbContext
{
    public PurchasesDbContext(DbContextOptions options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }
    
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<VehicleEntity> Vehicles { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    
}