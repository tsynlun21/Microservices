using CarHistory.Domain.EntitesContext;
using CarHistory.Domain.Repository;
using MongoDB.Driver;

namespace CarHistory.DataAccess.Repository;

public class CarHistoryRepo(IMongoDatabase database) : ICarHistoryRepo
{
    private readonly IMongoCollection<CarHistoryEntity> _collection = database.GetCollection<CarHistoryEntity>("CarHistory");
    
    public async Task AddAsync(CarHistoryEntity carHistory)
    {
        await _collection.InsertOneAsync(carHistory);
    }

    public async Task<CarHistoryEntity?> GetAsync(string vin)
    {
        return await _collection.Find(c => c.Vin == vin).FirstOrDefaultAsync();
    }
}