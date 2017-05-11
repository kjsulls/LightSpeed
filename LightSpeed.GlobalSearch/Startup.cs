using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LightSpeed.GlobalSearch.Startup))]
namespace LightSpeed.GlobalSearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
