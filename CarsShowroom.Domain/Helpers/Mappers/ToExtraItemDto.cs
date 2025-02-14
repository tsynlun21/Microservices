using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Helpers.Mappers;

public static partial class ShowRoomMappers
{
    public static ExtraItem ToExtraItemDto(this ExtraItemEntity entity)
    {
        if (entity == null)
            return null;

        return new ExtraItem
        {
            Type = entity.Type,
            Id = entity.Id,
            Name = entity.Name,
            Count = entity.Count,
            VehicleModel = entity.VehicleModelEntityKey
        };
    }
}