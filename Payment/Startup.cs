using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResComm.Web.Payment.Startup))]
namespace ResComm.Web.Payment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
