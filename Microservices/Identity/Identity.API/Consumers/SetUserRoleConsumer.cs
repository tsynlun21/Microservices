using Identity.Domain.Services;
using Infrastructure.Exceptions;
using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Identity.Requests;
using MassTransit;

namespace Identity.API.Consumers;

public class SetUserRoleConsumer(IAuthService service) : IConsumer<SetUserRoleRequest>
{
    public async Task Consume(ConsumeContext<SetUserRoleRequest> context)
    {
        var res = await service.SetUserRole(context.Message.UserId, context.Message.Role);

        await context.RespondAsync(res);
    }
}