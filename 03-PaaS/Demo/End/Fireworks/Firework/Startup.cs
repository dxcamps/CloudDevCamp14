using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Firework.Startup))]

namespace Firework
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseRedis(
              "fireworkcachebss.redis.cache.windows.net", 
              6379,
              "IDKpgxPRYMcPiBgYA6zn3cCG5B7M+Mzvp7GXklClFHw=", 
              "Fireworks");
            app.MapSignalR();
        }
    }
}
