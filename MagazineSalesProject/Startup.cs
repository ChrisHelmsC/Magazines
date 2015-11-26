using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MagazineSalesProject.Startup))]
namespace MagazineSalesProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
