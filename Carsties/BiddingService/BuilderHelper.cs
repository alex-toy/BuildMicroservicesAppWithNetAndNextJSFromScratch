using BiddingService.Consumers;
using BiddingService.Services;
using Contracts.ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;

namespace BiddingService
{
    public static class BuilderHelper
    {
        public static void ConfigureInterfaces(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IServiceBusHelper, ServiceBusHelper>();
            builder.Services.AddScoped<GrpcAuctionClient>();
            builder.Services.AddHostedService<CheckAuctionFinished>();
        }

        public static void ConfigureServiceBus(this WebApplicationBuilder builder)
        {
            string username = builder.Configuration.GetValue("RabbitMq:Username", "guest");
            string password = builder.Configuration.GetValue("RabbitMq:Password", "guest");
            string host = builder.Configuration["RabbitMq:Host"];
            builder.Services.ConfigureServiceBus<AuctionCreatedConsumer>(username, password, host, "bids");
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["IdentityServiceUrl"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.NameClaimType = "username";
                });
        }

        public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
