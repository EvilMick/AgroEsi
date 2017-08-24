using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(usuarios.Startup))]
namespace usuarios
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
