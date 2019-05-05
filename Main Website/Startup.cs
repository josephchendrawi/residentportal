using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Main_Website.Startup))]
namespace Main_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
