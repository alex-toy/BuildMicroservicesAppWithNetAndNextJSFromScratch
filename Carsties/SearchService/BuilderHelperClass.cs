using Contracts;

namespace SearchService
{
    public static class BuilderHelperClass
    {
        public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        {
            string username = builder.Configuration.GetValue("RabbitMq:Username", "guest");
            string password = builder.Configuration.GetValue("RabbitMq:Password", "guest");
            string host = builder.Configuration["RabbitMq:Host"];
            builder.Services.ConfigureMassTransitConsumer<AuctionCreatedConsumer>(username, password, host, "search", "search-auction-created");
        }
    }
}
