using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;
using Infrastructure.Models.Showrooms.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.DataAccess.DataContext;

public class ShowroomDbContext : DbContext
{
    public ShowroomDbContext(DbContextOptions<ShowroomDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        // if (Database.GetPendingMigrations().Any())
        // {
        //     Database.Migrate();
        // }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SoldExtraItemsEntity>()
            .HasOne(s => s.VehicleModel)
            .WithMany()
            .HasForeignKey(s => s.VehicleModelKey)
            .HasPrincipalKey(m => m.Model);


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
                Type                  = ExtraPartType.Electronic,
                VehicleModelEntityKey = 1,
                Count                 = 10,
                Price                 = 185_000,
            },
            new ExtraItemEntity()
            {
                Id                    = 4,
                Name                  = "Power Wilkins Audio system",
                Type                  = ExtraPartType.Electronic,
                VehicleModelEntityKey = 2,
                Count                 = 10,
                Price                 = 160_000
            },
            new ExtraItemEntity()
            {
                Id                    = 5,
                Name                  = "19R Alpine wheels set",
                Type                  = ExtraPartType.Exterior,
                VehicleModelEntityKey = 2,
                Count                 = 3,
                Price                 = 750_000
            },
            new ExtraItemEntity()
            {
                Id                    = 6,
                Name                  = "AMG kit",
                Type                  = ExtraPartType.Exterior,
                VehicleModelEntityKey = 3,
                Count                 = 3,
                Price                 = 300_000
            },
            new ExtraItemEntity()
            {
                Id                    = 7,
                Name                  = "Ceramic breaks system",
                Type                  = ExtraPartType.Exterior,
                VehicleModelEntityKey = 4,
                Count                 = 3,
                Price                 = 915_000
            },
            new ExtraItemEntity()
            {
                Id                    = 2,
                Name                  = "Lip spoiler",
                Type                  = ExtraPartType.Exterior,
                VehicleModelEntityKey = 1,
                Count                 = 10,
                Price                 = 45_000
            },
            new ExtraItemEntity()
            {
                Id                    = 3,
                Name                  = "LED lights",
                Type                  = ExtraPartType.Electronic,
                VehicleModelEntityKey = 1,
                Count                 = 10,
                Price                 = 310_000
            },
        });

        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 1,
            ShowroomEntityKey = 1,
            ReleaseDate       = new DateOnly(year: 2015, month: 1, day: 1),
            Brand             = "BMW",
            VehicleModelId    = 1,
            Color             = LPCColors.CianGray,
            Price             = 2_350_000,
            Vin               = "23D34EC8-F83F-4BED-95D6-05BDE4F76222",
            Mileage           = 120_000
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 2,
            ShowroomEntityKey = 1,
            ReleaseDate       = new DateOnly(year: 2017, month: 6, day: 13),
            Brand             = "BMW",
            VehicleModelId    = 1,
            Color             = LPCColors.Blue,
            Price             = 3_200_000,
            Vin               = "6786D736-1C1D-4375-8A35-6594E5133823",
            Mileage           = 85_000,
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 3,
            ShowroomEntityKey = 1,
            ReleaseDate       = new DateOnly(year: 2020, month: 11, day: 1),
            Brand             = "BMW",
            VehicleModelId    = 2,
            Color             = LPCColors.Blue,
            Price             = 10_200_000,
            Vin               = "8E10D3F9-FE4A-4023-9E44-505249AE95C5",
            Mileage           = 20_000,
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 4,
            ShowroomEntityKey = 2,
            ReleaseDate       = new DateOnly(year: 2016, month: 5, day: 10),
            Brand             = "Mercedes-Benz",
            VehicleModelId    = 3,
            Color             = LPCColors.Red,
            Price             = 3_200_000,
            Vin               = "80070953-A133-41C2-AD49-BD51C30FC7AE",
            Mileage           = 89_000,
        });
        modelBuilder.Entity<VehicleEntity>().HasData(new VehicleEntity()
        {
            Id                = 5,
            ShowroomEntityKey = 2,
            ReleaseDate       = new DateOnly(year: 2018, month: 3, day: 12),
            Brand             = "Mercedes-Benz",
            VehicleModelId    = 4,
            Color             = LPCColors.Silver,
            Price             = 9_450_000,
            Vin               = "BCC89D79-35D9-4AC2-8597-A837DED42699",
            Mileage           = 52_000,
        });
    }

    public DbSet<ShowroomEntity> Showrooms { get; set; } = null!;
    public DbSet<VehicleEntity> Vehicles { get; set; } = null!;
    public DbSet<VehicleModelEntity> VehicleModels { get; set; } = null!;
    public DbSet<ExtraItemEntity> ExtraItems { get; set; } = null!;
    public DbSet<ReceiptEntity> Receipts { get; set; } = null!;
}