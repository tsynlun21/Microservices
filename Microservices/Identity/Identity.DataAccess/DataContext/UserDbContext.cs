using Identity.Domain.EntititesContext;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.DataContext;

public class UserDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, string>
{
    public UserDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
         // if (Database.GetPendingMigrations().Any())
         //     Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<UserEntity>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        builder.Entity<IdentityRoleEntity>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();
        
        builder.Entity<IdentityRoleEntity>().HasData(
        [
            new IdentityRoleEntity
            {
                Id             = "@dm1n",
                Name           = RoleConstants.Admin,
                NormalizedName = RoleConstants.Admin.ToUpper(),
            },
            new IdentityRoleEntity
            {
                Id             = "m39ch4n7",
                Name           = RoleConstants.Merchant,
                NormalizedName = RoleConstants.Merchant.ToUpper(),
            },
            new IdentityRoleEntity
            {
                Id             = "u539",
                Name           = RoleConstants.User,
                NormalizedName = RoleConstants.User.ToUpper(),
            },
        ]);

        var adminUser = new UserEntity
        {
            Id                 = Guid.NewGuid().ToString(),
            Email              = "admin@admin.com",
            NormalizedEmail    = "admin@admin.com",
            EmailConfirmed     = true,
            NormalizedUserName = "ADMIN",
            UserName           = "ADMIN",
        };
            
        var hasher = new PasswordHasher<UserEntity>();
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "gigachad");
        
        builder.Entity<UserEntity>().HasData(adminUser);
        builder.Entity<IdentityUserRole<string>>().HasData(new[]
        {
            new IdentityUserRole<string>
            {
                RoleId = "@dm1n",
                UserId = adminUser.Id
            }
        });
        
        base.OnModelCreating(builder);
    }
}