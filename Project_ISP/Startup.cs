using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_ISP.Startup))]
namespace Project_ISP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
