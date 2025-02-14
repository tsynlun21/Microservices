using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;
using Infrastructure.Models.Showrooms.Enums;

namespace CarsShowroom.Domain.Helpers.Mappers;

public static partial class ShowRoomMappers
{
    public static Vehicle ToVehicleDto(this VehicleEntity? entity)
    {
        if (entity == null)
            return null;

        return new Vehicle()
        {
            VehicleId   = entity.Id,
            Brand       = entity.Brand,
            Model       = entity.VehicleModelEntity.Model,
            Color       = entity.Color,
            Price       = entity.Price,
            ExtraItems  = entity.VehicleModelEntity.ExtraItems != null ? entity.VehicleModelEntity.ExtraItems.Select(x => x.ToExtraItemDto()).ToList() : null,
            ReleaseDate = entity.ReleaseDate,
            Vin         = entity.Uid
        };
    }

    public static VehicleEntity ToVehicleEntity(this Vehicle vehicle, long showroomId, long modelId)
    {
        if (vehicle == null)
            return null;

        return new VehicleEntity()
        {
            Brand             = vehicle.Brand,
            VehicleModelId    = modelId,
            Color             = vehicle.Color,
            Price             = vehicle.Price,
            ReleaseDate       = vehicle.ReleaseDate,
            Uid               = vehicle.Vin,
            ShowroomEntityKey = showroomId
        };
    }
}