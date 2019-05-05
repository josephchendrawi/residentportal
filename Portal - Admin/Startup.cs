using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResComm.Web.Startup))]
namespace ResComm.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
