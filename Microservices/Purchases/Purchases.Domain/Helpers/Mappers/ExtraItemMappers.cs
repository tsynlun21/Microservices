using Infrastructure.Models.Showrooms;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.Domain.Helpers.Mappers;

public static partial class PurchasesMappers
{
    public static ExtraItemEntity ToExtraItemEntity(this ExtraPart extraPart)
    {
        if (extraPart == null)
            return null;

        return new ExtraItemEntity()
        {
            Name = extraPart.Name,
            VehicleModel = extraPart.VehicleModel,
            Price        = extraPart.Price,
            Count        = extraPart.Count,
            Type         = extraPart.Type,
        };
    }

    public static ExtraPart ToExtraItem(this ExtraItemEntity entity)
    {
        if (entity == null)
            return null;

        return new ExtraPart()
        {
            Name         = entity.Name,
            VehicleModel = entity.VehicleModel,
            Price        = entity.Price,
            Count        = entity.Count,
            Type         = entity.Type,
        };
    }
}