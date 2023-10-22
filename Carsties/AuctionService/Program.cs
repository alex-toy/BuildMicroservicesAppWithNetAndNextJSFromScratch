using AuctionService;
using AuctionService.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureDatabase();

builder.ConfigureAutoMapper();

builder.ConfigureMassTransit();

builder.ConfigureInterfaces();




var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DbInitializer.InitDb(app);

app.Run();
