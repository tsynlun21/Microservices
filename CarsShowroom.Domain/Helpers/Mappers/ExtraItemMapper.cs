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
            Type         = entity.Type,
            Id           = entity.Id,
            Name         = entity.Name,
            Count        = entity.Count,
            VehicleModel = entity.VehicleModelEntity.Model,
            Price        = entity.Price,
        };
    }

    public static ExtraItemEntity ToExtraItemEntity(this ExtraItem entity, long modelId)
    {
        if (entity == null)
            return null;

        return new ExtraItemEntity()
        {
            Name                  = entity.Name,
            Type                  = entity.Type,
            Count                 = entity.Count,
            VehicleModelEntityKey = modelId,
            Price                 = entity.Price,
        };
    }

    public static SoldExtraItemsEntity ToSoldExtraItem(this ExtraItem entity)
    {
        if (entity == null)
            return null;
        
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