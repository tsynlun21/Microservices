using CarHistory.Domain.EntitesContext;

namespace CarHistory.Domain.Repository;

public interface ICarHistoryRepo
{
    public Task AddAsync(CarHistoryEntity carHistory);
    public Task<CarHistoryEntity?> GetAsync(string vin);
}