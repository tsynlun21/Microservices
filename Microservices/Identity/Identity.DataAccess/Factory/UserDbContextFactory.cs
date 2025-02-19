using Identity.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.DataAccess.Factory;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=postgres;Port=5432;Username=postgres;password=admin;database=homeproject-cars-identity");
        
        // optionsBuilder.UseNpgsql(
        //     "Host=localhost;Port=5432;Username=postgres;password=admin;database=homeproject-cars-identity");
        
        return new UserDbContext(optionsBuilder.Options);
    }
}