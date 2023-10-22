using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts
{
    public static class ServiceBusHelper
    {
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                //x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
                //{
                //    o.QueryDelay = TimeSpan.FromSeconds(10);

                //    o.UsePostgres();
                //    o.UseBusOutbox();
                //});

                //x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

                //x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

                x.UsingRabbitMq((context, cfg) =>
                {
                    //cfg.UseRetry(r =>
                    //{
                    //    r.Handle<RabbitMqConnectionException>();
                    //    r.Interval(5, TimeSpan.FromSeconds(10));
                    //});

                    //cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
                    //{
                    //    host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
                    //    host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
                    //});

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
