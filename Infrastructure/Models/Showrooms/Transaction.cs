using Infrastructure.Models.Purchases;
using Infrastructure.Models.Purchases.Enums;

namespace Infrastructure.Models.Showrooms;

public class Transaction
{
    public long Id { get; set; }
    
    public List<Vehicle> Vehicles { get; set; }
    
    public DateTime Date { get; set; }
    
    public TransactionType Type { get; set; }
    
    public bool IsPerfomedByShowroom { get; set; }
    
    public Receipt Receipt { get; set; }
}