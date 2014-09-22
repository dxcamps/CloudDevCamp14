using System;
using System.Web.Mvc;
namespace HelloAllWorlds.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return base.View();
        }
    }
}