using Infrastructure.Models.CarHistory;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarHistory.Domain.EntitesContext;

public class CarHistoryEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required string Vin { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public decimal Mileage { get; set; }
    public CarInfo CarInfo { get; set; }
}