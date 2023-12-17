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

app.ConfigureMongoDb();

app.Run();
