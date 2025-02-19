using System.Text.Json.Serialization;
using CarHistory.API.Consumers;
using CarHistory.API.Settings;
using CarHistory.DataAccess.Repository;
using CarHistory.Domain.Repository;
using CarHistory.Domain.Services;
using Infrastructure.Configuration;
using Infrastructure.Masstransit;
using Infrastructure.Middlewares;
using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace CarHistory.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(
                name: "v1",
                info: new OpenApiInfo()
                {
                    Title   = "CarHistory API",
                    Version = "v1"
                });
            
            opt.AddSecurityDefinition(
                name: "Bearer",
                securityScheme: new OpenApiSecurityScheme
                {
                    In           = ParameterLocation.Header,
                    Description  = "Please insert JWT token",
                    Name         = "Authorization",
                    Type         = SecuritySchemeType.Http,
                    Scheme       = "Bearer",
                    BearerFormat = "JWT"
                });

            opt.AddSecurityRequirement(
                securityRequirement: new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });
        
        return builder;
    }
    
    public static WebApplicationBuilder AddMongoDb(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        return builder;
    }

    public static WebApplicationBuilder AddDataContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMongoDatabase>(sp =>
        {
            var client   = sp.GetService<IMongoClient>();
            var settings = sp.GetService<IOptions<MongoDbSettings>>().Value;

            return client.GetDatabase(settings.DatabaseName);
        });

        builder.Services.AddScoped<ICarHistoryRepo, CarHistoryRepo>();
        return builder;
    }

    public static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICarHistoryService, CarHistoryService>();
        return builder;
    }
    
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<AddCarHistoryConsumer>();
            x.AddConsumer<AddCarHistoryWithAutoGenerationConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri("rabbitmq://host.docker.internal/"), hostConfigurator =>
                {
                    hostConfigurator.Username("guest");
                    hostConfigurator.Password("guest");
                });
                
                // configurator.Host("localhost", "/", hostConfigurator =>
                // {
                //     hostConfigurator.Username("guest");
                //     hostConfigurator.Password("guest");
                // });

                configurator.ReceiveEndpoint(
                    queueName: RabbitQueueNames.CAR_HISTORY,
                    configureEndpoint: endpoint =>
                    {
                        endpoint.PrefetchCount = 5;
                        endpoint.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(2)));
                        endpoint.UseConsumeFilter(typeof(LogConsumeMessageFilter<>), context);

                        endpoint.Consumer<AddCarHistoryConsumer>(context);
                        endpoint.Consumer<AddCarHistoryWithAutoGenerationConsumer>(context);
                    });
                configurator.ConfigureJsonSerializerOptions(o =>
                {
                    o.ReferenceHandler = ReferenceHandler.Preserve;

                    return o;
                });
                configurator.ConfigureEndpoints(context);
            });
        });
        return builder;
    }
    
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtAuthentication(builder.Configuration["Authentication:SecretKey"]);
        
        return builder;
    }
}