using System.Text;
using CarHistory.Domain.EntitesContext;
using CarHistory.Domain.Helpers;
using CarHistory.Domain.Repository;
using Infrastructure.Exceptions;
using Infrastructure.Models.CarHistory;

namespace CarHistory.Domain.Services;

public class CarHistoryService(ICarHistoryRepo repo) : ICarHistoryService
{
    private static readonly Random _random = new Random();
    
    public async Task<Infrastructure.Models.CarHistory.CarHistory> GetCarHistoryAsync(string vin)
    {
        var entity = await repo.GetAsync(vin);

        if (entity == null)
            throw new NotFoundException($"Car history with vin {vin} not found");

        return new Infrastructure.Models.CarHistory.CarHistory()
        {
            Vin         = entity.Vin,
            Brand       = entity.Brand,
            Model       = entity.Model,
            Color       = entity.Color,
            Mileage     = entity.Mileage,
            CarInfo     = entity.CarInfo,
            ReleaseDate = entity.ReleaseDate,
        };
    }

    public async Task<ICollection<Infrastructure.Models.CarHistory.CarHistory>> AddCarHistoryAsync(
        ICollection<Infrastructure.Models.CarHistory.CarHistory> carHistories)
    {
        var toInsert = carHistories.Select(x => new CarHistoryEntity()
        {
            Vin         = x.Vin,
            Brand       = x.Brand,
            Model       = x.Model,
            Color       = x.Color,
            Mileage     = x.Mileage,
            ReleaseDate = x.ReleaseDate,
            CarInfo     = x.CarInfo
        });
        
        foreach (var carHistoryEntity in toInsert)
        {
            await repo.AddAsync(carHistoryEntity);
        }
        
        return carHistories;
    }

    public async Task AddCarHistoryWithGenerationAsync(
        ICollection<Infrastructure.Models.CarHistory.CarHistoryBase> carHistories)
    {
        var toInsert = carHistories.Select(x => new CarHistoryEntity()
        {
            Vin         = x.Vin,
            Brand       = x.Brand,
            Model       = x.Model,
            Color       = x.Color,
            Mileage     = x.Mileage,
            ReleaseDate = x.ReleaseDate
        });

        foreach (var carHistoryEntity in toInsert)
        {
            var result = await repo.GetAsync(carHistoryEntity.Vin);
            
            if (await repo.GetAsync(carHistoryEntity.Vin) != null)
                continue;
            
            await GenerateCarHistory(carHistoryEntity);
            
            await repo.AddAsync(carHistoryEntity);
        }
    }

    private Task GenerateCarHistory(CarHistoryEntity entity)
    {
        entity.CarInfo = new CarInfo()
        {
            JuridicalInfoDescription = GenerateJuridicalInfoDescription(entity.Mileage),
            GeneralInfoDescription   = GenerateGeneralInfoDescription(entity.ReleaseDate)
        };

        return Task.CompletedTask;
    }

    private string GenerateJuridicalInfoDescription(decimal mileage)
    {
        var sb = new StringBuilder();
        
        double legalStatusProbability = _random.NextDouble() * 100;

        if (legalStatusProbability < 85)
        {
            sb.AppendLine($"Юридический статус: {JuridicalInfoConstants.NO_PROBLEMS}");
        }
        else if (legalStatusProbability is < 95 and > 85)
        {
            sb.AppendLine($"Юридический статус: {JuridicalInfoConstants.PLDGED}");
        }
        else
        {
            sb.AppendLine("Юридический статус: Ограничения на регистрацию");
        }

        int ownerCount = _random.Next(1, 5);
        sb.AppendLine($"Количество владельцев: {ownerCount}");
        
        if (mileage > 100000)
        {
            sb.AppendLine("Пробег высокий, возможен износ.");
        }
        else
        {
            sb.AppendLine("Пробег в пределах нормы.");
        }
        
        return sb.ToString();
    }

    private string GenerateGeneralInfoDescription(DateOnly entityReleaseDate)
    {
        var sb = new StringBuilder();
        
        if (entityReleaseDate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-1)))
        {
            sb.AppendLine("Автомобиль не участвовал в ДТП.");
            return sb.ToString();
        }
        
        double accidentProbability = _random.NextDouble() * 100;

        if (accidentProbability is < 75)
        {
            sb.AppendLine("Автомобиль не участвовал в ДТП.");
        }
        else if (accidentProbability is < 90 and > 70)
        {
            sb.AppendLine("ДТП: Легкое");
        }
        else if (accidentProbability is < 97 and > 90)
        {
            sb.AppendLine("ДТП: Среднее");
        }
        else
        {
            sb.AppendLine("Количество ДТП: 1");
            sb.AppendLine("ДТП: Тяжелое");
        }

        return sb.ToString();
    }
}