namespace Infrastructure.Models.CarHistory;

public class CarHistory : CarHistoryBase
{ 
    public CarInfo CarInfo { get; set; }
}

public class CarHistoryBase
{
    public required string Vin { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public decimal Mileage { get; set; }
}