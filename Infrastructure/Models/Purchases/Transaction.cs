using Infrastructure.Models.Purchases.Enums;
using Infrastructure.Models.Showrooms;

namespace Infrastructure.Models.Purchases;

public class Transaction
{
    public long Id { get; set; }
    
    public Vehicle Vehicle { get; set; }
    
    public List<ExtraPart> ExtraItems { get; set; }
    
    public DateTime Date { get; set; }
    
    public TransactionType Type { get; set; }
    
    public bool IsPerfomedByShowroom { get; set; }
    
    public Receipt Receipt { get; set; }
}