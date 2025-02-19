using Infrastructure.Exceptions;
using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;
using Microsoft.EntityFrameworkCore;
using Purchases.DataAccesss.ContextEntities;
using Purchases.DataAccesss.DataContext;
using Purchases.Domain.Helpers.Mappers;
using Purchases.Domain.Services.Interfaces;

namespace Purchases.Domain.Services;

public class PurchasesService(PurchasesDbContext context) : IPurchasesService
{
    public async Task<Transaction> GetTransactionById(UserModel user, long id)
    {
        var customer = await TryCheckCustomerExists(user);

        customer = await context.Customers
            .Include(c => c.Transactions)
            .ThenInclude(t => t.Vehicle)
            .Include(c => c.Transactions)
            .ThenInclude(t => t.ExtraItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        var transaction = customer.Transactions.FirstOrDefault(t => t.Id == id);
        if (transaction == null)
            throw new NotFoundException($"Transaction with id {id} not found");

        return transaction.ToTransaction();
    }

    public async Task<ICollection<Transaction>> GetTransactions(UserModel user)
    {
        var customer = await TryCheckCustomerExists(user);

        customer = await context.Customers
            .Include(c => c.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);


        var response = customer.Transactions
            .Select(t => t.ToTransaction())
            .ToArray();
        return response;
    }

    public async Task<Transaction> AddTransaction(UserModel user, Transaction transaction)
    {
        var alreadyExistingVehicle = await context.Transactions.Include(t => t.Vehicle).FirstOrDefaultAsync(t =>
            t.Date == transaction.Date.ToUniversalTime() &&
            t.Vehicle.Vin == transaction.Vehicle.Vin
        );

        if (alreadyExistingVehicle != null)
        {
            throw new BadRequestException($"Vehicle with id {transaction.Vehicle.Vin} already exists");
        }

        var customer = await TryCheckCustomerExists(user);


        customer.Transactions.Add(transaction.ToTransactionEntity());
        await context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> UpdateTransaction(UserModel user, UpdateTransaction transaction)
    {
        var customer = await TryCheckCustomerExists(user);

        customer = await context.Customers
            .Include(c => c.Transactions)
            .ThenInclude(t => t.Vehicle)
            .Include(c => c.Transactions)
            .ThenInclude(t => t.ExtraItems).Include(customerEntity => customerEntity.Transactions)
            .ThenInclude(transactionEntity => transactionEntity.Receipt)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

        var currentTransaction = customer.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
        if (currentTransaction == null)
            throw new NotFoundException($"Transaction with id {transaction.Id} not found");

        await UpdateTransaction(currentTransaction, transaction);

        return new Transaction()
        {
            Date                 = currentTransaction.Date.ToUniversalTime(),
            Id                   = currentTransaction.Id,
            ExtraItems           = currentTransaction.ExtraItems.Select(x => x.ToExtraItem()).ToList(),
            IsPerfomedByShowroom = currentTransaction.IsPerfomedByShowroom,
            Type                 = currentTransaction.Type,
            Receipt              = currentTransaction.Receipt.ToReceipt(),
            Vehicle              = currentTransaction.Vehicle.ToVehicle(),
        };
    }

    private async Task UpdateTransaction(TransactionEntity currentTransaction, UpdateTransaction transaction)
    {
        if (currentTransaction.IsPerfomedByShowroom)
        {
            if (transaction.ExtraItems != null || transaction.Vehicle != null || transaction.Date != new DateTime())
            {
                throw new BadRequestException(
                    "The transaction`s content that was perfomed by showroom cannot be changed.");
            }

            currentTransaction.Type = transaction.Type;
            await context.SaveChangesAsync();
        }
    }

    private async Task<CustomerEntity> TryCheckCustomerExists(UserModel user)
    {
        var customerId = user.Id;
        Console.WriteLine($"customerId: {customerId}");

        var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId || c.UserName == user.UserName);
        if (customer == null)
        {
            customer = (await context.Customers.AddAsync(new CustomerEntity()
            {
                CustomerId = customerId,
                UserName = user.UserName,
            })).Entity;
            await context.SaveChangesAsync();
            Console.WriteLine($"created customer id: {customerId}");
        }

        return customer;
    }
}