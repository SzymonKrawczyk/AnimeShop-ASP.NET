using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnimeShop.Startup))]
namespace AnimeShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
