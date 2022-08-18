using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TextCreate.Startup))]
namespace TextCreate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
