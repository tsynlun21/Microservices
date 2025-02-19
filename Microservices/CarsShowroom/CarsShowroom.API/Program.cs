using CarsShowroom.API.Extensions;
using Infrastructure.Middlewares;

namespace CarsShowroom.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting(o => o.LowercaseUrls = true);
        builder.Services.AddControllers(opts =>
        {
            opts.AddFilters();
        });

        builder
            .AddSwagger()
            .AddDataContext()
            .AddDomainServices()
            .AddMassTransit()
            .AddAuthentication();
        
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.MapControllers();

        app.Urls.Add("http://*:5003");
        app.Run();
    }
}