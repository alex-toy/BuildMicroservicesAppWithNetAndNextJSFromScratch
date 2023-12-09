using BiddingService;
using BiddingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureMassTransit();

builder.ConfigureAuthentication();

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
