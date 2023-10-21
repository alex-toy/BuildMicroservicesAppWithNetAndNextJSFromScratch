
namespace SearchService
{
    public static class BuilderHelperClass
    {
        public static void AddMassTransit(this WebApplicationBuilder builder)
        {
            //builder.Services.AddMassTransit(x =>
            //{
            //    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

            //    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.UseRetry(r =>
            //        {
            //            r.Handle<RabbitMqConnectionException>();
            //            r.Interval(5, TimeSpan.FromSeconds(10));
            //        });

            //        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
            //        {
            //            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            //            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
            //        });

            //        cfg.ReceiveEndpoint("search-auction-created", e =>
            //        {
            //            e.UseMessageRetry(r => r.Interval(5, 5));

            //            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
            //        });

            //        cfg.ConfigureEndpoints(context);
            //    });
            //});
        }
    }
}
