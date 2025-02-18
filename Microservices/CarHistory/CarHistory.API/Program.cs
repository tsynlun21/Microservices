using CarHistory.API.Extensions;
using CarHistory.API.Settings;
using Infrastructure.Configuration;
using MongoDB.Driver;

namespace CarHistory.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder
            .AddSwagger()
            .AddDataContext()
            .AddMongoDb()
            .AddDomainServices()
            .AddAuthentification()
            .AddMassTransit();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Urls.Add("http://*:5004");
        app.Run();
    }
}