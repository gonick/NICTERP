using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NICTERP.Startup))]
namespace NICTERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
