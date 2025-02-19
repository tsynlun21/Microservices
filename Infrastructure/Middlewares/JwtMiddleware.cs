using Infrastructure.Masstransit;
using Infrastructure.Masstransit.Identity.Requests;
using Infrastructure.Models.Identity;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middlewares;

public class JwtMiddleware(RequestDelegate next)
{
    private readonly Uri _rabbitIdentityQueue = new Uri($"rabbitmq:/host.docker.internal/{RabbitQueueNames.IDENTITY}");

    public async Task InvokeAsync(HttpContext context, IBusControl busControl)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (string.IsNullOrEmpty(token) == false)
            await AttachUserModelToContext(context, token, busControl);
        
        await next.Invoke(context);
        
    }

    private async Task AttachUserModelToContext(HttpContext context, string token, IBusControl busControl)
    {
        var client = busControl.CreateRequestClient<TokenRequest>(_rabbitIdentityQueue);
        
        if (client == null)
            Console.WriteLine("CLIENT IS NULL");
        Console.WriteLine("CLIENT OK");
        var response = await client.GetResponse<UserModel>(new TokenRequest()
        {
            Token = token
        });
        
        if (response?.Message != null)
            context.Items["User"] = response.Message;
    }
}