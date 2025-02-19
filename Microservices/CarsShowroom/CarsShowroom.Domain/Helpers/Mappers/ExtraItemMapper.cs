using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Helpers.Mappers;

public static partial class ShowRoomMappers
{
    public static ExtraPart ToExtraItemDto(this ExtraItemEntity entity)
    {
        return new ExtraPart
        {
            Type         = entity.Type,
            Id           = entity.Id,
            Name         = entity.Name,
            Count        = entity.Count,
            VehicleModel = entity.VehicleModelEntity.Model,
            Price        = entity.Price,
        };
    }

    public static ExtraItemEntity ToExtraItemEntity(this ExtraPart entity, long modelId)
    {
        return new ExtraItemEntity()
        {
            Name                  = entity.Name,
            Type                  = entity.Type,
            Count                 = entity.Count,
            VehicleModelEntityKey = modelId,
            Price                 = entity.Price,
        };
    }

    public static SoldExtraItemsEntity ToSoldExtraItem(this ExtraPart entity)
    {
        return new SoldExtraItemsEntity()
        {
            Name = entity.Name,
            Type         = entity.Type,
            Count             = entity.Count,
            VehicleModelKey = entity.VehicleModel,
            Price               = entity.Price,
        };
    }
}