using AuctionService.Data;

namespace AuctionService
{
    public static class WebApplicationHelper
    {
        public static void SeedDatabase(this WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();

                DbInitializer.SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
