using CarsShowroom.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarsShowroom.DataAccess.Factory;

public class ShowroomDbContextFactory : IDesignTimeDbContextFactory<ShowroomDbContext>
{
    public ShowroomDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShowroomDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Username=postgres;password=admin;database=homeproject-cars-showroom");
        
        return new ShowroomDbContext(optionsBuilder.Options);
    }
}