using NotificationService;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureMassTransit();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
