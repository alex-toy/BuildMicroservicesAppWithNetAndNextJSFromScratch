using Contracts;

namespace SearchService
{
    public static class BuilderHelperClass
    {
        public static void ConfigureMassTransit(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureMassTransit();
        }
    }
}
