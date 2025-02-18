using Identity.Domain.Services;
using Infrastructure.Models.Identity;
using MassTransit;

namespace Identity.API.Consumers;

public class LoginConsumer(IAuthService service) : IConsumer<UserLogin>
{
    public async Task Consume(ConsumeContext<UserLogin> context)
    {
        var res = await service.Login(context.Message);
        
        await context.RespondAsync(res);
    }
}