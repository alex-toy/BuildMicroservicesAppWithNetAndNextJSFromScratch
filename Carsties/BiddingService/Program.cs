using BiddingService;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureInterfaces();

builder.Services.AddControllers();

builder.ConfigureServiceBus();

builder.ConfigureAuthentication();

builder.ConfigureAutoMapper();




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
