using System.Text;
using System.Text.Json.Serialization;
using Identity.API.Consumers;
using Identity.DataAccess.DataContext;
using Identity.Domain.EntititesContext;
using Identity.Domain.Services;
using Infrastructure.Configuration;
using Infrastructure.Filters;
using Infrastructure.Masstransit;
using Infrastructure.Middlewares;
using Infrastructure.Models.Identity;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Identity.API.Extensions;

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
                    Title   = "Identity API",
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

    public static WebApplicationBuilder AddDataContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("IdentityDB");

        builder.Services.AddDbContext<UserDbContext>(opt => opt.UseNpgsql(connectionString));

        return builder;
    }

    public static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddAuthorizationAndIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwtAuthentication(builder.Configuration["Authentification:TokenPrivateKey"]);
        
        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("Admin", policy => policy.RequireClaim(RoleConstants.Admin));
            opts.AddPolicy("Merchant", policy => policy.RequireClaim(RoleConstants.Merchant));
            opts.AddPolicy("User", policy => policy.RequireClaim(RoleConstants.User));
        });

        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddDefaultIdentity<UserEntity>(opts =>
            {
                opts.SignIn.RequireConfirmedAccount  = false;
                opts.Password.RequiredLength         = 6;
                opts.Password.RequireDigit           = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase       = false;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddUserStore<UserStore<UserEntity, IdentityRoleEntity, UserDbContext, long>>();

        return builder;
    }

    public static WebApplicationBuilder AddAuthentificationOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Authentification"));

        return builder;
    }

    public static MvcOptions AddFilters(this MvcOptions options)
    {
        options.Filters.Add<LogEndpointExecutionFilter>();
        return options;
    }

    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<RegisterConsumer>();
            x.AddConsumer<LoginConsumer>();
            x.AddConsumer<SetUserRoleConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host("localhost", "/", hostConfigurator =>
                {
                    hostConfigurator.Username("guest");
                    hostConfigurator.Password("guest");
                });

                configurator.ReceiveEndpoint(
                    queueName: RabbitQueueNames.IDENTITY,
                    configureEndpoint: endpoint =>
                    {
                        endpoint.PrefetchCount = 5;
                        endpoint.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        endpoint.UseConsumeFilter(typeof(LogConsumeMessageFilter<>), context);

                        endpoint.Consumer<RegisterConsumer>(context);
                        endpoint.Consumer<LoginConsumer>(context);
                        endpoint.Consumer<SetUserRoleConsumer>(context);
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
}