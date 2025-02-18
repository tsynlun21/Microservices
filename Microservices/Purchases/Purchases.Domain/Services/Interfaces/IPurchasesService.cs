using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;

namespace Purchases.Domain.Services.Interfaces;

public interface IPurchasesService
{
    public Task<Transaction> GetTransactionById(User user, long id);
    
    public Task<ICollection<Transaction>> GetTransactions(User user);
    
    public Task<Transaction?> AddTransaction(User user, Transaction transaction);
    
    public Task UpdateTransaction(User user, UpdateTransaction transaction);
}