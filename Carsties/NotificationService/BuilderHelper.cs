using Contracts.ServiceBus;
using NotificationService.Consumers;

namespace NotificationService
{
    public static class BuilderHelper
    {
        public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        {
            string username = builder.Configuration.GetValue("RabbitMq:Username", "guest");
            string password = builder.Configuration.GetValue("RabbitMq:Password", "guest");
            string host = builder.Configuration["RabbitMq:Host"];
            builder.Services.ConfigureServiceBus<AuctionCreatedConsumer>(username, password, host, "notification");
        }
    }
}
