using AuctionService;
using AuctionService.Data;
using Contracts.ServiceBus;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IServiceBusHelper, ServiceBusHelper>();

builder.Services.AddControllers();

builder.ConfigureDatabase();

builder.ConfigureAutoMapper();

builder.ConfigureMassTransit();

builder.ConfigureInterfaces();

builder.ConfigureAuthentication();




var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

DbInitializer.InitDb(app);

app.Run();
