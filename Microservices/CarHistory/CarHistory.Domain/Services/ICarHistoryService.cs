﻿namespace CarHistory.Domain.Services;

public interface ICarHistoryService
{
    public Task<Infrastructure.Models.CarHistory.CarHistory> GetCarHistoryAsync(string vin);
    public Task<ICollection<Infrastructure.Models.CarHistory.CarHistory>> AddCarHistoryAsync(
        ICollection<Infrastructure.Models.CarHistory.CarHistory> carHistories);
    public Task AddCarHistoryWithGenerationAsync(ICollection<Infrastructure.Models.CarHistory.CarHistoryBase> carHistories);
}