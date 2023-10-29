namespace GatewayService
{
    public static class BuilderHelperClass
    {
        public static void ConfigureReverseProxy(this WebApplicationBuilder builder)
        {
            IConfigurationSection configurationSection = builder.Configuration.GetSection("ReverseProxy");
            //builder.Services.AddReverseProxy().LoadFromConfig(configurationSection);
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = builder.Configuration["IdentityServiceUrl"];
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters.ValidateAudience = false;
            //        options.TokenValidationParameters.NameClaimType = "username";
            //    });
        }

        public static void ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("customPolicy", b =>
                {
                    string origins = builder.Configuration["ClientApp"];
                    b.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(origins);
                });
            });
        }
    }
}
