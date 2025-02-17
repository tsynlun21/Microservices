using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.Exceptions;
using Infrastructure.Models.Showrooms.Enums;

namespace Infrastructure.Models.Showrooms;

[ExtraItemValidation]
public class ExtraItem
{
    [JsonIgnore]
    public long? Id { get; set; }
    public string Name { get; set; }
    public ExtraItemType Type { get; set; }
    public string VehicleModel { get; set; }
    public int Count { get; set; }
    
    public decimal Price { get; set; }
}

public class ExtraItemOrder
{
    public string Name { get; set; }
    public int Count { get; set; }
}

internal class ExtraItemValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var item = value as ExtraItem;
        if (item == null)
            throw new BadRequestException("Extra item cannot be null");
        
        if (string.IsNullOrEmpty(item.VehicleModel))
            throw new BadRequestException($"Invalid extra item vehicle model reference: VehicleModel cannot be empty");
        
        if (item.Count <= 0)
            throw new BadRequestException($"Invalid count: Extra item count cannot be {item.Count}");
        
        if (item.Price <= 0)
            throw new BadRequestException($"Invalid price: Extra item price cannot be {item.Price}");
        return ValidationResult.Success;
    }
}