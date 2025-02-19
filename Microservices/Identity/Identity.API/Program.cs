using Identity.API.Extensions;
using Identity.Domain.Services;
using Infrastructure.Middlewares;

namespace Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(opt => opt.AddFilters());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting(o => o.LowercaseUrls = true);

        Console.WriteLine($"ADDING SERVICES TO ASP.NET Core Web API");
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        builder
            .AddDataContext()
            .AddAuthorizationAndIdentity()
            .AddAuthenticationOptions()
            .AddSwagger()
            .AddMassTransit();

        Console.WriteLine($"services: {builder.Services.Count} {builder.Services.ToList()}");
        foreach (var serviceDescriptor in builder.Services.ToList())
        {
            Console.WriteLine($"{serviceDescriptor.ServiceType.FullName}: {serviceDescriptor.ImplementationType}");
        }

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<JwtMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapControllers();

        app.Urls.Add("http://*:5001");
        app.Run();
    }
}