using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infrastructure.Exceptions;
using Infrastructure.Models.Showrooms.Enums;

namespace Infrastructure.Models.Showrooms;

[VehicleValidation]
public class Vehicle
{
    public long VehicleId { get; set; }
    public string Vin { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public LPCColors Color { get; set; }
    public decimal Price { get; set; }
    public decimal Mileage { get; set; }

    [JsonIgnore] public List<ExtraItem>? ExtraItems { get; set; } = null;
}

internal class VehicleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var vehicle = value as Vehicle;

        if (vehicle == null)
            throw new BadRequestException($"Vehicle cannot be null");
        
        if (vehicle.Brand == null)
            throw new BadRequestException($"Invalid Brand: cannot be null");
        
        if (vehicle.Model == null)
            throw new BadRequestException($"Invalid Model: cannot be null");
        
        if (vehicle.Price <= 0)
            throw new BadRequestException($"Invalid Price: price cannot be less than 0");

        return ValidationResult.Success;
    }
}
