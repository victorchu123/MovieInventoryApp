using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieInventoryApp.Startup))]
namespace MovieInventoryApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
