using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;
using Infrastructure.Models.Showrooms.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.DataAccess.DataContext;

public class ShowroomDbContext : DbContext
{
    public ShowroomDbContext(DbContextOptions<ShowroomDbContext> options) : base(options)
    {
        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShowroomEntity>().HasData(new ShowroomEntity()
        {
            Id            = 1,
            Address       = "Ростов-на-Дону, ул. Еременко 28, 'BMW Motors'",
            ContactNumber = "79959303330"
        });
        modelBuilder.Entity<ShowroomEntity>().HasData(new ShowroomEntity()
        {
            Id            = 2,
            Address       = "Ростов-на-Дону, ул. Шаболдаева 105, 'Mercedes-Benz club'",
            ContactNumber = "79959303331"
        });

        modelBuilder.Entity<VehicleModelEntity>().HasData(new List<VehicleModelEntity>()
        {
            new VehicleModelEntity()
            {
                Id    = 1,
                Model = "BMW 325"
            },
            new VehicleModelEntity()
            {
                Id    = 2,
                Model = "BMW M5 F90"
            },
            new VehicleModelEntity()
            {
                Id    = 3,
                Model = "Mercedes-Benz E200"
            },
            new VehicleModelEntity()
            {
                Id    = 4,
                Model = "Mercedes-Benz CLS63 AMG"
            }
        });

        modelBuilder.Entity<ExtraItemEntity>().HasData(new List<ExtraItemEntity>()
        {
            new ExtraItemEntity()
            {
                Id                    = 1,
                Name                  = "Harman Kardon audio system",
                Type                  = ExtraItemType.Electronic,
                VehicleModelEntityKey = 1,
                Count                 = 10
            },
            new ExtraItemEntity()
            {
                Id                    = 4,
                Name                  = "Power Wilkins Audio system",
                Type                  = ExtraItemType.Electronic,
                VehicleModelEntityKey = 2,
                Count                 = 10
            },
            new ExtraItemEntity()
            {
                Id                    = 5,
                Name                  = "19R Alpine wheels",
                Type                  = ExtraItemType.Exterior,
                VehicleModelEntityKey = 2,
                Count                 = 3
            },
            new ExtraItemEntity()
            {
                Id                    = 6,
                Name                  = "AMG kit",
                Type                  = ExtraItemType.Exterior,
                VehicleModelEntityKey = 3,
                Count                 = 3
            },
            new ExtraItemEntity()
            {
                Id                    = 7,
                Name                  = "Ceramic breaks system",
                Type                  = ExtraItemType.Exterior,
                VehicleModelEntityKey = 4,
                Count                 = 3
            },
            new ExtraItemEntity()
            {
                Id                    = 2,
                Name                  = "Lip spoiler",
                Type                  = ExtraItemType.Exterior,
                VehicleModelEntityKey = 1,
                Count                 = 10
            },
            new ExtraItemEntity()
            {
                Id                    = 3,
                Name                  = "LED lights",
                Type                  = ExtraItemType.Electronic,
                VehicleModelEntityKey = 1,
                Count                 = 10
            },
        });

        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 1,
            ShowroomEntityKey = 1,
            ReleaseDate       = DateTime.SpecifyKind(new DateTime(2023, 3, 4, 0, 0, 0), DateTimeKind.Utc),
            Brand             = "BMW",
            VehicleModelId    = 1,
            Color             = LPCColors.CianGray,
            Price             = 3_000_000,
            Uid               = "23D34EC8-F83F-4BED-95D6-05BDE4F76222"
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 2,
            ShowroomEntityKey = 1,
            ReleaseDate       = DateTime.SpecifyKind(new DateTime(2023, 3, 4, 0, 0, 0), DateTimeKind.Utc),
            Brand             = "BMW",
            VehicleModelId    = 1,
            Color             = LPCColors.Blue,
            Price             = 3_200_000,
            Uid               = "6786D736-1C1D-4375-8A35-6594E5133823"
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 3,
            ShowroomEntityKey = 1,
            ReleaseDate       = DateTime.SpecifyKind(new DateTime(2023, 3, 4, 0, 0, 0), DateTimeKind.Utc),
            Brand             = "BMW",
            VehicleModelId    = 2,
            Color             = LPCColors.Blue,
            Price             = 10_200_000,
            Uid               = "8E10D3F9-FE4A-4023-9E44-505249AE95C5"
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 4,
            ShowroomEntityKey = 2,
            ReleaseDate       = DateTime.SpecifyKind(new DateTime(2023, 3, 4, 0, 0, 0), DateTimeKind.Utc),
            Brand             = "Mercedes-Benz",
            VehicleModelId    = 3,
            Color             = LPCColors.Red,
            Price             = 3_200_000,
            Uid               = "80070953-A133-41C2-AD49-BD51C30FC7AE"
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 5,
            ShowroomEntityKey = 2,
            ReleaseDate       = DateTime.SpecifyKind(new DateTime(2023, 3, 4, 0, 0, 0), DateTimeKind.Utc),
            Brand             = "Mercedes-Benz",
            VehicleModelId    = 4,
            Color             = LPCColors.Silver,
            Price             = 9_450_000,
            Uid               = "BCC89D79-35D9-4AC2-8597-A837DED42699"
        });
    }

    public DbSet<ShowroomEntity> Showrooms { get; set; } = null!;
    public DbSet<VehicleEntity> Vehicles { get; set; } = null!;
    public DbSet<VehicleModelEntity> VehicleModels { get; set; } = null!;
    public DbSet<ExtraItemEntity> ExtraItems { get; set; } = null!;
    public DbSet<ReceiptEntity> Receipts { get; set; } = null!;
}