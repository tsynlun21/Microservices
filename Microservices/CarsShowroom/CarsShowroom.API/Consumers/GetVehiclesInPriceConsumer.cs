﻿using CarsShowroom.Domain.Requests;
using CarsShowroom.Domain.Services.Interfaces;
using Infrastructure.Masstransit.Showrooms.Requests;
using MassTransit;

namespace CarsShowroom.API.Consumers;

public class GetVehiclesInPriceConsumer(IShowroomService service) : IConsumer<PriceLimitRequest>
{
    public async Task Consume(ConsumeContext<PriceLimitRequest> context)
    {
        var res = await service.GetAllVehiclesInPrice(context.Message);

        await context.RespondAsync(res.ToArray());
    }
}