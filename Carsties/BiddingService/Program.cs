using BiddingService.Consumers;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string username = builder.Configuration.GetValue("RabbitMq:Username", "guest");
string password = builder.Configuration.GetValue("RabbitMq:Password", "guest");
string host = builder.Configuration["RabbitMq:Host"];
builder.Services.ConfigureMassTransitConsumer<AuctionCreatedConsumer>(username, password, host, "bids", "bid-created");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

//await Policy.Handle<TimeoutException>()
//    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(10))
//    .ExecuteAndCaptureAsync(async () =>
//    {
//        await DB.InitAsync("BidDb", MongoClientSettings
//            .FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));
//    });

app.Run();
