using BiddingService.Consumers;
using Contracts.ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BiddingService
{
    public static class BuilderHelper
    {
        public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        {
            string username = builder.Configuration.GetValue("RabbitMq:Username", "guest");
            string password = builder.Configuration.GetValue("RabbitMq:Password", "guest");
            string host = builder.Configuration["RabbitMq:Host"];
            builder.Services.ConfigureMassTransitConsumer<AuctionCreatedConsumer>(username, password, host, "bids", "bid-created");
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
    }
}
