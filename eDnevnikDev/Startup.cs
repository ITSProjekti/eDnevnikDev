using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eDnevnikDev.Startup))]
namespace eDnevnikDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
