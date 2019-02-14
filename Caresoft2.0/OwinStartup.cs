using System;
using System.Threading.Tasks;
using Caresoft2.Areas;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Caresoft2._0.OwinStartup))]

namespace Caresoft2._0
{
    public class OwinStartup : Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
