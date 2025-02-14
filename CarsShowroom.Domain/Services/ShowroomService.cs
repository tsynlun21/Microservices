using CarsShowroom.DataAccess.ContextEntities;
using CarsShowroom.DataAccess.DataContext;
using CarsShowroom.Domain.Helpers.Mappers;
using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Response;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.Domain.Services;

public class ShowroomService(ShowroomDbContext context) : IShowroomService
{
    private const int MaxExtraItemsPerRequest = 10;

    public async Task<ICollection<Vehicle>> GetAllVehiclesByShowroom(long showroomId)
    {
        var showRoom = await context.Showrooms
            .Include(s => s.Vehicles)
                .ThenInclude(v => v.VehicleModelEntity)
                    .ThenInclude(ve => ve.ExtraItems)
            .FirstOrDefaultAsync(s => s.Id == showroomId);
        
        if (showRoom == null)
            throw new NotFoundException($"Shop with id [{showroomId}] not found");
        
        var result = showRoom?.Vehicles
            .Select(v => v.ToVehicleDto())
            .ToArray();
        
        return result;
    }

    public async Task<ICollection<Vehicle>> GetAllVehiclesByBrandAndModel(BrandAndModelRequest request)
    {
        var showRoom = await context.Showrooms
            .Include(s => s.Vehicles)
            .ThenInclude(v => v.VehicleModelEntity)
            .ThenInclude(ve => ve.ExtraItems)
            .FirstOrDefaultAsync(s => s.Id == request.ShowroomId);
        
        if (showRoom == null)
            throw new NotFoundException($"Shop with id [{request.ShowroomId}] not found");

        var result = showRoom?.Vehicles
            .Where(v => v.Brand == request.Brand && (string.IsNullOrWhiteSpace(request.Model) != false ||
                                                     v.VehicleModelEntity.Model == request.Model))
            .Select(v => v.ToVehicleDto()).ToArray();
        
        return result;
    }

    public async Task<ICollection<VehiclesForShowroom>> GetAllVehiclesInPrice(PriceLimitRequest request)
    {
        var res = await context.Vehicles
            .Include(v => v.Showroom)
            .Include(x => x.VehicleModelEntity)
            .AsNoTracking()
            .Where(v => v.Price > request.MinPrice && v.Price < (request.MaxPrice ?? int.MaxValue))
            .GroupBy(x => x.Showroom)
            .Select(x => new VehiclesForShowroom()
            {
                Adress = x.Key.Address,
                ContactNumber = x.Key.ContactNumber,
                Vehicles = x.Select(x => x.ToVehicleDto()).ToList()
            })
            .ToListAsync();
        
        return res;
    }

    public async Task<ICollection<ExtraItem>> GetAllExtrasByModelName(string modelName)
    {
        var model = await context.VehicleModels
            .Include(v => v.ExtraItems)
            .FirstOrDefaultAsync(v => v.Model == modelName);
        
        if (model == null)
            throw new NotFoundException($"Model name [{modelName}] not found");
        
        var result = model.ExtraItems.Select(ei => ei.ToExtraItemDto()).ToArray();
        return result;
    }

    public async Task AddVehiclesToShowroom(ICollection<VehiclesPerShowroom> vehiclesPerShowrooms)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        {
            var vehiclesToInsert = await AddNewCarModels(vehiclesPerShowrooms);
            
            await context.Vehicles.AddRangeAsync(vehiclesToInsert);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }

    private async Task<ICollection<VehicleEntity>> AddNewCarModels(ICollection<VehiclesPerShowroom> vehiclesPerShowrooms)
    {
        var res  = new List<VehicleEntity>();
        
        var alreadyExistingCarModels = await context.VehicleModels.AsNoTracking().Select(v => new
        {
            Id = v.Id,
            Model = v.Model,
        }).ToListAsync();

        //var newCarModels = pendingCarModels.Except(alreadyExistingCarModels).ToList();
        
        
        foreach (var vehiclesPerShowroom in vehiclesPerShowrooms)
        {
            foreach (var vehicle in vehiclesPerShowroom.Vehicles)
            {
                var alreadyExists = alreadyExistingCarModels.FirstOrDefault(x => x.Model == vehicle.Model);

                if (alreadyExists != null)
                {
                    res.Add(vehicle.ToVehicleEntity(vehiclesPerShowroom.ShowroomId, alreadyExists.Id));
                }
                else
                {
                    var vehicleModelId = (await context.VehicleModels.AddAsync(new VehicleModelEntity()
                    {
                        Model = vehicle.Model,
                    })).Entity.Id;
                    
                    res.Add(vehicle.ToVehicleEntity(vehiclesPerShowroom.ShowroomId, vehicleModelId));
                }

                await context.SaveChangesAsync();
            }
        }

        return res;
    } 

    public async Task AddExtraItemsToVehicleInShowroom(ICollection<ExtraItemToVehicle> vehicles)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Vehicle>> BuyVehicle(long showroomId, ICollection<Vehicle> vehicles)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> CreateTransaction(int showroomId, ICollection<Vehicle> vehicles)
    {
        throw new NotImplementedException();
    }

    public async Task AddReceipt(Receipt receipt)
    {
        throw new NotImplementedException();
    }
}