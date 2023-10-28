using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts
{
    public static class ServiceBusHelper
    {
        public static void ConfigureMassTransitProducer<T, TContext>(this IServiceCollection services, string formatter) where TContext : DbContext
        {
            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<TContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(10);

                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.AddConsumersFromNamespaceContaining<T>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(formatter, false));

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

        public static void ConfigureMassTransitConsumer<T>(this IServiceCollection services, string username, string password, string host) where T : class, IConsumer
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<T>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseRetry(r =>
                    {
                        r.Handle<RabbitMqConnectionException>();
                        r.Interval(5, TimeSpan.FromSeconds(10));
                    });

                    cfg.Host(host, "/", host =>
                    {
                        host.Username(username);
                        host.Password(password);
                    });

                    cfg.ReceiveEndpoint("search-auction-created", e =>
                    {
                        e.UseMessageRetry(r => r.Interval(5, 5));

                        e.ConfigureConsumer<T>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
