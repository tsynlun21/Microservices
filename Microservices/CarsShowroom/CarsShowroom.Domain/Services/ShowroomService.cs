using CarsShowroom.DataAccess.ContextEntities;
using CarsShowroom.DataAccess.DataContext;
using CarsShowroom.Domain.Helpers.Mappers;
using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Masstransit.Showrooms.Requests;
using Infrastructure.Masstransit.Showrooms.Responses;
using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;
using Microsoft.EntityFrameworkCore;

namespace CarsShowroom.Domain.Services;

public class ShowroomService(ShowroomDbContext context) : IShowroomService
{
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

    public async Task<ICollection<Vehicle>> GetAllVehiclesByBrandAndModel(GetVehiclesByBrandAndModelRequest request)
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

    public async Task<ICollection<VehiclesInPriceForShowroom>> GetAllVehiclesInPrice(PriceLimitRequest request)
    {
        var res = await context.Vehicles
            .Include(v => v.Showroom)
            .Include(x => x.VehicleModelEntity)
            .AsNoTracking()
            .Where(v => v.Price > request.MinPrice && v.Price < (request.MaxPrice ?? int.MaxValue))
            .GroupBy(x => x.Showroom)
            .Select(x => new VehiclesInPriceForShowroom()
            {
                Adress        = x.Key.Address,
                ContactNumber = x.Key.ContactNumber,
                Vehicles      = x.Select(x => x.ToVehicleDto()).ToList()
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

    public async Task<ICollection<Vehicle>> AddVehiclesToShowroom(ICollection<VehiclesPerShowroom> vehiclesPerShowrooms)
    {
        var showroomIds = vehiclesPerShowrooms.Select(x => x.ShowroomId);

        var allShowroomExists = showroomIds.All(x => context.Showrooms.Any(s => s.Id == x));
        if (!allShowroomExists)
            throw new NotFoundException($"One or more showroom not found");

        await using var transaction = await context.Database.BeginTransactionAsync();

        var vehiclesToInsert = await AddNewCarModels(vehiclesPerShowrooms);
        
        await context.Vehicles.AddRangeAsync(vehiclesToInsert);
        
        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        foreach (var vehicleEntity in vehiclesToInsert)
        {
            await context.Entry(vehicleEntity)
                .Reference(v => v.VehicleModelEntity)
                .LoadAsync();
        }
        
        return vehiclesToInsert.Select(v => v.ToVehicleDto()).ToList();
    }

    private async Task<ICollection<VehicleEntity>> AddNewCarModels(
        ICollection<VehiclesPerShowroom> vehiclesPerShowrooms)
    {
        var res = new List<VehicleEntity>();

        var alreadyExistingCarModels = await context.VehicleModels.AsNoTracking().Select(v => new
        {
            Id    = v.Id,
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
                    var newVehicleModel = new VehicleModelEntity()
                    {
                        Model = vehicle.Model,
                    };

                    await context.VehicleModels.AddAsync(newVehicleModel);

                    await context.SaveChangesAsync();
                    res.Add(vehicle.ToVehicleEntity(vehiclesPerShowroom.ShowroomId, newVehicleModel.Id));
                }

                await context.SaveChangesAsync();
            }
        }

        return res;
    }

    public async Task<ICollection<ExtraItem>> AddExtraItems(ICollection<ExtraItem> extraItems)
    {
        var res = new List<ExtraItem>();
        foreach (var extraItem in extraItems)
        {
            var vehicleModel = await context.VehicleModels.FirstOrDefaultAsync(vm => vm.Model == extraItem.VehicleModel);
            
            if (vehicleModel == null)
                throw new BadRequestException($"Vehicle model [{extraItem.VehicleModel}] not found");
            
            var existingExtraItem = await context.ExtraItems.FirstOrDefaultAsync(e =>
                e.Name == extraItem.Name && e.VehicleModelEntityKey == vehicleModel.Id &&
                e.Type == extraItem.Type);
            if (existingExtraItem != null)
            {
                existingExtraItem.Count += extraItem.Count;
                res.Add(existingExtraItem.ToExtraItemDto());
            }
            else
            {
                res.Add((await context.ExtraItems.AddAsync(extraItem.ToExtraItemEntity(vehicleModel.Id))).Entity.ToExtraItemDto()); 
            }

            await context.SaveChangesAsync();
        }

        return res;
    }

    public async Task<Purchase> BuyVehicle(long showroomId, PurchaseOrder purchaseOrder)
    {
        var purchaseResult = new Purchase()
        {
            ExtraItems = new List<ExtraItem>()
        };
        
        var showroom = await context.Showrooms
            .Include(x => x.Vehicles)
                .ThenInclude(v => v.VehicleModelEntity)
                    .ThenInclude(ve=> ve.ExtraItems)
            .FirstOrDefaultAsync(s => s.Id == showroomId);
        
        if (showroom == null)
            throw new BadRequestException($"Showroom [{showroomId}] not found");

        var vehicleToBuy = showroom.Vehicles.FirstOrDefault(v =>
            v.Id == purchaseOrder.VehicleId
        );
        
        if (vehicleToBuy == null)
            throw new BadRequestException($"Vehicle [{purchaseOrder.VehicleId}] not found");
        
        purchaseResult.Vehicle = vehicleToBuy.ToVehicleDto();
        
        var vehicleModel = vehicleToBuy.VehicleModelEntity;

        foreach (var extraItem in purchaseOrder.ExtraItems)
        {
            var extraItemEntity = vehicleModel.ExtraItems.FirstOrDefault(ei => ei.Name == extraItem.Name && ei.VehicleModelEntity.Model == vehicleModel.Model);
            
            if (extraItemEntity == null)
                throw new BadRequestException($"Extra item [{extraItem.Name}] not found for vehicle model [{vehicleModel.Model}] you want to buy");

            if (extraItemEntity.Count < extraItem.Count)
                throw new BadRequestException($"There are not enough extra items available to buy");
            
            extraItemEntity.Count -= extraItem.Count;
            var extraItemConfirmed = extraItemEntity.ToExtraItemDto();
            extraItemConfirmed.Count = extraItem.Count;
            
            purchaseResult.ExtraItems.Add(extraItemConfirmed);
        }
        
        context.Vehicles.Remove(vehicleToBuy);

        await context.SaveChangesAsync();

        return purchaseResult;
    }

    public Task<Transaction> CreateTransaction(long showroomId, Purchase purchase)
    {
        var response = new Transaction()
        {
            Vehicle              = purchase.Vehicle,
            ExtraItems           = purchase.ExtraItems.ToList(),
            IsPerfomedByShowroom = true,
            Date                 = DateTime.Now,
            Receipt = new Receipt()
            {
                ShoowRoomId   = showroomId,
                Vehicle       = purchase.Vehicle,
                ExtraItems    = purchase.ExtraItems.ToList(),
                Date          = DateTime.Now,
                TotalPrice    = (purchase.Vehicle.Price + purchase.ExtraItems.Sum(e => e.Price)),
                TotalQuantity = 1 + purchase.ExtraItems.Count,
            }
        };
        
        return Task.FromResult(response);
    }

    public async Task AddReceipt(Receipt receipt)
    {
        var receiptContext = new ReceiptEntity()
        {
            TotalPrice = receipt.TotalPrice,
            TotalItemsCount = receipt.TotalQuantity,
            ShowroomKey = receipt.ShoowRoomId,
            SoldExtraItems = receipt.ExtraItems!.Select(item => item.ToSoldExtraItem()).ToList(),
            SoldVehicle = receipt.Vehicle.ToSoldVehicleEntity()
        };
        await context.Receipts.AddAsync(receiptContext);
        await context.SaveChangesAsync();
    }
}