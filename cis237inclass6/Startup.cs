using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cis237inclass6.Startup))]
namespace cis237inclass6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
