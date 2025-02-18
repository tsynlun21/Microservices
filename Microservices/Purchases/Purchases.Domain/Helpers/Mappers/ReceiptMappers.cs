using Infrastructure.Models.Purchases;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.Domain.Helpers.Mappers;

public static partial class PurchasesMappers
{
    public static ReceiptEntity ToReceiptEntity(this Receipt receipt)
    {
        if (receipt == null)
            return null;
    
        return new ReceiptEntity()
        {
            Quantity   = receipt.TotalQuantity,
            ShowroomId = receipt.ShoowRoomId,
            Date       = receipt.Date.ToUniversalTime(),
            TotalPrice = receipt.TotalPrice
        };
    }

    public static Receipt ToReceipt(this ReceiptEntity entity)
    {
        if (entity == null)
            return null;

        return new Receipt()
        {
            TotalQuantity = entity.Quantity,
            TotalPrice    = entity.TotalPrice,
            Date          = entity.Date.ToUniversalTime(),
            ShoowRoomId   = entity.ShowroomId,
        };
    }
}