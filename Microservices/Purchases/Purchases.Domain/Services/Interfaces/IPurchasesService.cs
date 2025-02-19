using Infrastructure.Models.Identity;
using Infrastructure.Models.Purchases;

namespace Purchases.Domain.Services.Interfaces;

public interface IPurchasesService
{
    public Task<Transaction> GetTransactionById(UserModel user, long id);
    
    public Task<ICollection<Transaction>> GetTransactions(UserModel user);
    
    public Task<Transaction> AddTransaction(UserModel user, Transaction transaction);
    
    public Task<Transaction> UpdateTransaction(UserModel user, UpdateTransaction transaction);
}