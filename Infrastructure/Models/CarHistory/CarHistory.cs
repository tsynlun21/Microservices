using System.ComponentModel.DataAnnotations;
using Infrastructure.Exceptions;

namespace Infrastructure.Models.CarHistory;

public class CarHistory : CarHistoryBase
{ 
    public CarInfo CarInfo { get; set; }
}

[ValidateCarHistory]
public class CarHistoryBase
{
    public required string Vin { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public decimal Mileage { get; set; }
}

public class ValidateCarHistoryAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var model = value as CarHistoryBase;
        
        if (model.ReleaseDate < new DateOnly(1960, 1 , 1))
            throw new BadRequestException($"Invalid release date: {model.ReleaseDate}");
        
        if (model.Mileage < 0)
            throw new BadRequestException($"Invalid mileage: {model.Mileage}");
        
        return true;
    }
}