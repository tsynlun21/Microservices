using Identity.Domain.EntititesContext;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.DataContext;

public class UserDbContext : IdentityDbContext<UserEntity, IdentityRoleEntity, long>
{
    
    public UserDbContext(DbContextOptions options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityRoleEntity>().HasData(new[]
        {
            new IdentityRoleEntity
            {
                Id             = 1,
                Name           = RoleConstants.Admin,
                NormalizedName = RoleConstants.Admin.ToUpper(),
            },
            new IdentityRoleEntity
            {
                Id             = 2,
                Name           = RoleConstants.Merchant,
                NormalizedName = RoleConstants.Merchant.ToUpper(),
            },
            new IdentityRoleEntity
            {
                Id             = 3,
                Name           = RoleConstants.User,
                NormalizedName = RoleConstants.User.ToUpper(),
            },
        });

        var adminUser = new UserEntity
        {
            Id                 = 1,
            Email              = "admin@admin.com",
            NormalizedEmail    = "admin@admin.com",
            EmailConfirmed     = true,
            NormalizedUserName = "ADMIN",
            UserName           = "ADMIN",
        };
            
        var hasher = new PasswordHasher<UserEntity>();
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "gigachad");
        
        builder.Entity<UserEntity>().HasData(adminUser);
        builder.Entity<IdentityUserRole<long>>().HasData(new[]
        {
            new IdentityUserRole<long>
            {
                RoleId = 1,
                UserId = 1
            }
        });
        
        base.OnModelCreating(builder);
    }
}