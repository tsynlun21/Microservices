using Identity.Domain.Services;
using Infrastructure.Masstransit.Identity.Requests;
using MassTransit;

namespace Identity.API.Consumers;

public class GetUserByTokenConsumer(IAuthService service) : IConsumer<TokenRequest>
{
    public async Task Consume(ConsumeContext<TokenRequest> context)
    {
        var res = await service.GetUserByToken(context.Message.Token);

        await context.RespondAsync(res);
    }
}