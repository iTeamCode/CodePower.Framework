using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Dashboard.WebPortal.Startup))]
[assembly: OwinStartup(typeof(FellowshipOne.Dashboard.WebPortal.Startup))]
namespace FellowshipOne.Dashboard.WebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
