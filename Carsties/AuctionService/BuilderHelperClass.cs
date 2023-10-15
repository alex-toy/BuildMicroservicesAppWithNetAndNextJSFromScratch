using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionService
{
    public static class BuilderHelperClass
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AuctionDbContext>(opt => opt.UseNpgsql(connectionString));
        }

        public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        //public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        //{
        //    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.Authority = builder.Configuration["IdentityServiceUrl"];
        //            options.RequireHttpsMetadata = false;
        //            options.TokenValidationParameters.ValidateAudience = false;
        //            options.TokenValidationParameters.NameClaimType = "username";
        //        });
        //}

        //public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        //{
        //    builder.Services.AddMassTransit(x =>
        //    {
        //        x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
        //        {
        //            o.QueryDelay = TimeSpan.FromSeconds(10);

        //            o.UsePostgres();
        //            o.UseBusOutbox();
        //        });

        //        x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

        //        x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

        //        x.UsingRabbitMq((context, cfg) =>
        //        {
        //            cfg.UseRetry(r =>
        //            {
        //                r.Handle<RabbitMqConnectionException>();
        //                r.Interval(5, TimeSpan.FromSeconds(10));
        //            });

        //            cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        //            {
        //                host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
        //                host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        //            });

        //            cfg.ConfigureEndpoints(context);
        //        });
        //    });
        //}
    }
}
