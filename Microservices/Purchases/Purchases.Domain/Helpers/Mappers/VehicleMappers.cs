using Infrastructure.Models.Purchases;
using Infrastructure.Models.Showrooms;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.Domain.Helpers.Mappers;

public static partial class PurchasesMappers
{
    public static VehicleEntity ToVehicleEntity(this Vehicle vehicle)
    {
        if (vehicle == null)
            return null;

        return new VehicleEntity()
        {
            Model       = vehicle.Model,
            Price       = vehicle.Price,
            Brand       = vehicle.Brand,
            Color       = vehicle.Color,
            Vin         = vehicle.Vin,
            ReleaseDate = vehicle.ReleaseDate
        };
    }

    public static Vehicle ToVehicle(this VehicleEntity vehicleEntity)
    {
        if (vehicleEntity == null)
            return null;

        return new Vehicle()
        {
            Model       = vehicleEntity.Model,
            Price       = vehicleEntity.Price,
            Brand       = vehicleEntity.Brand,
            Color       = vehicleEntity.Color,
            Vin         = vehicleEntity.Vin,
            ReleaseDate = vehicleEntity.ReleaseDate
        };
    }
}