using Infrastructure.Models.Showrooms;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.Domain.Helpers.Mappers;

public static partial class PurchasesMappers
{
    public static ExtraItemEntity ToExtraItemEntity(this ExtraItem extraItem)
    {
        if (extraItem == null)
            return null;

        return new ExtraItemEntity()
        {
            Name = extraItem.Name,
            VehicleModel = extraItem.VehicleModel,
            Price        = extraItem.Price,
            Count        = extraItem.Count,
            Type         = extraItem.Type,
        };
    }

    public static ExtraItem ToExtraItem(this ExtraItemEntity entity)
    {
        if (entity == null)
            return null;

        return new ExtraItem()
        {
            Name         = entity.Name,
            VehicleModel = entity.VehicleModel,
            Price        = entity.Price,
            Count        = entity.Count,
            Type         = entity.Type,
        };
    }
}