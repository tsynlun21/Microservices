using Infrastructure.Configuration;
using Infrastructure.Middlewares;
using Purchases.API.Extensions;

namespace Purchases.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting(o =>
        {
            o.LowercaseUrls = true;
        });
        builder.Services.AddControllers(opts =>
        {
            opts.AddFilters();
        });

        builder.Services.AddJwtAuthentication(builder.Configuration["Authentication:SecretKey"]);
        builder
            .AddDataContext()
            .AddSwagger()
            .AddDomainServices()
            .AddMassTransit();

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<JwtMiddleware>();
        app.MapControllers();

        app.Urls.Add("http://*:5002");
        app.Run();
    }
}