using AuctionService.Data;
using Contracts;
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

        public static void ConfigureInterfaces(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
        }

        public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureMassTransit();
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
    }
}
