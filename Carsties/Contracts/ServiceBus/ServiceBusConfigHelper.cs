using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.ServiceBus
{
    public static class ServiceBusConfigHelper
    {
        public static void ConfigureMassTransitProducer<T, TContext>(this IServiceCollection services, string formatter, string username, string password, string host) where TContext : DbContext
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
                    cfg.UseMessageRetry(r =>
                    {
                        r.Handle<RabbitMqConnectionException>();
                        r.Interval(5, TimeSpan.FromSeconds(10));
                    });

                    cfg.Host(host, "/", host =>
                    {
                        host.Username(username);
                        host.Password(password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public static void ConfigureMassTransitConsumer<T>(this IServiceCollection services, string username, string password, string host, string formatter, string receive) where T : class, IConsumer
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<T>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(formatter, false));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                    {
                        r.Handle<RabbitMqConnectionException>();
                        r.Interval(5, TimeSpan.FromSeconds(10));
                    });

                    cfg.Host(host, "/", host =>
                    {
                        host.Username(username);
                        host.Password(password);
                    });

                    cfg.ReceiveEndpoint(receive, e =>
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
