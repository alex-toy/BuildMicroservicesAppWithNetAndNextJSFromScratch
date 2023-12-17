using AuctionService;
using AuctionService.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureInterfaces();

builder.Services.AddControllers();

builder.ConfigureGrpc();

builder.ConfigureDatabase();

builder.ConfigureAutoMapper();

builder.ConfigureServiceBus();

builder.ConfigureAuthentication();




var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcAuctionService>();

//var retryPolicy = Policy
//    .Handle<NpgsqlException>()
//    .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10));

//retryPolicy.ExecuteAndCapture(() => DbInitializer.InitDb(app));

app.SeedDatabase();

app.Run();
