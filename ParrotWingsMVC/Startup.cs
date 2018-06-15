using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParrotWingsMVC.Startup))]
namespace ParrotWingsMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
