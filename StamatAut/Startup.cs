using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StamatAut.Startup))]
namespace StamatAut
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
