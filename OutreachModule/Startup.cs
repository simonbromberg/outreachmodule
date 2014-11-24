using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OutreachModule.Startup))]
namespace OutreachModule
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
