using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.Exceptions;
using Infrastructure.Models.Showrooms.Enums;

namespace Infrastructure.Models.Showrooms;

[ExtraPartValidation]
public class ExtraPart
{
    [JsonIgnore]
    public long? Id { get; set; }
    public string Name { get; set; }
    public ExtraPartType Type { get; set; }
    public string VehicleModel { get; set; }
    public int Count { get; set; }
    
    public decimal Price { get; set; }
}

public class ExtraPartOrder
{
    public string Name { get; set; }
    public int Count { get; set; }
}

internal class ExtraPartValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var item = value as ExtraPart;
        if (item == null)
            throw new BadRequestException("Extra part cannot be null");
        
        if (string.IsNullOrEmpty(item.VehicleModel))
            throw new BadRequestException($"Invalid extra part vehicle model reference: VehicleModel cannot be empty");
        
        if (item.Count <= 0)
            throw new BadRequestException($"Invalid count: Extra part count cannot be {item.Count}");
        
        if (item.Price <= 0)
            throw new BadRequestException($"Invalid price: Extra part price cannot be {item.Price}");
        return ValidationResult.Success;
    }
}