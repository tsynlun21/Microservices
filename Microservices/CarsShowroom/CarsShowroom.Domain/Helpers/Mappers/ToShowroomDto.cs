using CarsShowroom.DataAccess.ContextEntities;
using Infrastructure.Models.Showrooms;

namespace CarsShowroom.Domain.Helpers.Mappers;

public static partial class ShowRoomMappers
{
    public static Showroom ToShowroomDto(this ShowroomEntity entity)
    {
        return new Showroom()
        {
            Id            = entity.Id,
            Address       = entity.Address,
            Vehicles      = entity.Vehicles.Select(x => x.ToVehicleDto()).ToList(),
            ContactNumber = entity.ContactNumber,
        };
    }
}