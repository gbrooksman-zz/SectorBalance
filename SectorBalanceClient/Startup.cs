using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SectorBalanceClient
{
    public class Startup
    {
        AppStateContainer state = new AppStateContainer();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppStateContainer>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
