using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hanashop.Startup))]
namespace Hanashop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
