using BiddingService;
using BiddingService.Services;
using Contracts.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IServiceBusHelper, ServiceBusHelper>();

builder.Services.AddControllers();

builder.ConfigureMassTransit();

builder.ConfigureAuthentication();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();




var app = builder.Build();

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
