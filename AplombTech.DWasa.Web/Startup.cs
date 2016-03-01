using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AplombTech.DWasa.Web.Startup))]
namespace AplombTech.DWasa.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
