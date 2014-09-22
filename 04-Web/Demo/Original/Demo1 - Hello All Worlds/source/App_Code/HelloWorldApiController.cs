using System;
using System.Web.Http;
namespace HelloAllWorlds.Controllers
{
    public class HelloWorldApiController : ApiController
    {
        public string Get()
        {
            return "Hello World (in Web API)";
        }
    }
}