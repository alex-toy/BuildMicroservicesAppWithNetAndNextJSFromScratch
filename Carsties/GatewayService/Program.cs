using GatewayService;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureReverseProxy();

builder.ConfigureAuthentication();

builder.ConfigureCors();


var app = builder.Build();

app.UseCors();

//app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
