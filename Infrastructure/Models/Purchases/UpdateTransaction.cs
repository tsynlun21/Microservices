using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Purchases.Enums;
using Infrastructure.Models.Showrooms;

namespace Infrastructure.Models.Purchases;

[ValidateUpdateTransaction]
public class UpdateTransaction
{
    [Required]
    public long Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public Vehicle Vehicle { get; set; }
    
    public List<ExtraPart> ExtraItems { get; set; }
    
    [RequiredTransactionType]
    public TransactionType Type { get; set; }
}

public class ValidateUpdateTransactionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var id = ((UpdateTransaction)validationContext.ObjectInstance).Id;
        
        if (id <= 0)
            return new ValidationResult("Id must be greater than 0");
        
        return ValidationResult.Success;
    }
}

public class RequiredTransactionTypeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var type = (int?)value;
        if (type == 0 || type == 1)
            return ValidationResult.Success;
        
        return new ValidationResult("Invalid transaction type");
    }
}

