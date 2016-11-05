using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DF.Finance.Admin.Startup))]
namespace DF.Finance.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
