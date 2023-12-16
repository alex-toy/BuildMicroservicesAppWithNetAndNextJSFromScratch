using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.ServiceBus
{
    public static class ServiceBusConfigHelper
    {
        public static void ConfigureServiceBus<T, TContext>(this IServiceCollection services, string formatter, string username, string password, string host) where TContext : DbContext
        {
            services.AddMassTransit(x =>
            {
                ConfigureEntityFramework<TContext>(x);

                x.AddConsumersFromNamespaceContaining<T>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(formatter, false));

                ConfigureRabbitMQ(username, password, host, x);
            });
        }

        public static void ConfigureServiceBus<T>(this IServiceCollection services, string username, string password, string host, string formatter, string receive = null) where T : class, IConsumer
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<T>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(formatter, false));

                ConfigureRabbitMQ<T>(username, password, host, receive, x);
            });
        }

        private static void ConfigureEntityFramework<TContext>(IBusRegistrationConfigurator x) where TContext : DbContext
        {
            x.AddEntityFrameworkOutbox<TContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(10);

                o.UsePostgres();
                o.UseBusOutbox();
            });
        }

        private static void ConfigureRabbitMQ(string username, string password, string host, IBusRegistrationConfigurator x)
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                ConfigureMessageRetry(cfg);

                ConfigureHost(username, password, host, cfg);

                cfg.ConfigureEndpoints(context);
            });
        }

        private static void ConfigureRabbitMQ<T>(string username, string password, string host, string receive, IBusRegistrationConfigurator x) where T : class, IConsumer
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                ConfigureMessageRetry(cfg);

                ConfigureHost(username, password, host, cfg);

                if (receive != null) ConfigureReceiveEndpoint<T>(receive, context, cfg);

                cfg.ConfigureEndpoints(context);
            });
        }

        private static void ConfigureHost(string username, string password, string host, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Host(host, "/", host =>
            {
                host.Username(username);
                host.Password(password);
            });
        }

        private static void ConfigureMessageRetry(IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.UseMessageRetry(r =>
            {
                r.Handle<RabbitMqConnectionException>();
                r.Interval(5, TimeSpan.FromSeconds(10));
            });
        }

        private static void ConfigureReceiveEndpoint<T>(string receive, IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg) where T : class, IConsumer
        {
            cfg.ReceiveEndpoint(receive, e =>
            {
                e.UseMessageRetry(r => r.Interval(5, 5));
                e.ConfigureConsumer<T>(context);
            });
        }
    }
}
