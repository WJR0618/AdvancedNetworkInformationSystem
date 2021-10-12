using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(finalProject_V1.Startup))]
namespace finalProject_V1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
