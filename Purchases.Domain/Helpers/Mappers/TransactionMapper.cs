using Infrastructure.Models.Purchases;
using Purchases.DataAccesss.ContextEntities;

namespace Purchases.Domain.Helpers.Mappers;

public static partial class PurchasesMappers
{
    public static TransactionEntity ToTransactionEntity(this Transaction transaction)
    {
        if (transaction is null)
            return null;

        return new TransactionEntity()
        {
            Vehicle = transaction.Vehicle.ToVehicleEntity(),
            ExtraItems = transaction.ExtraItems.Select(ei => ei.ToExtraItemEntity()).ToList(),
            Date = transaction.Date.ToUniversalTime(),
            IsPerfomedByShowroom = transaction.IsPerfomedByShowroom,
            Type = transaction.Type,
            Receipt = transaction.Receipt.ToReceiptEntity()
        };
    }

    public static Transaction ToTransaction(this TransactionEntity entity)
    {
        if (entity is null)
            return null;

        return new Transaction()
        {
            Date       = entity.Date.ToUniversalTime(),
            ExtraItems = entity.ExtraItems.Select(ei => ei.ToExtraItem()).ToList(),
            Vehicle = entity.Vehicle.ToVehicle(),
            IsPerfomedByShowroom = entity.IsPerfomedByShowroom,
            Type = entity.Type,
            Receipt = entity.Receipt.ToReceipt()
        };
    }
}