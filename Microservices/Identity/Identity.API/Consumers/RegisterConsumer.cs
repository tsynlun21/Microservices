using System.Text.Json;
using Identity.Domain.Services;
using Infrastructure.Exceptions;
using Infrastructure.Masstransit;
using Infrastructure.Models.Identity;
using MassTransit;

namespace Identity.API.Consumers;

public class RegisterConsumer(IAuthService authService) : IConsumer<UserRegister>
{
    public async Task Consume(ConsumeContext<UserRegister> context)
    {
        var res = await authService.Register(context.Message);
        
        await context.RespondAsync(res);
    }
}