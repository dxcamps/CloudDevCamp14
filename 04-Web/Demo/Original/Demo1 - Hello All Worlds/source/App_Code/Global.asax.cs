using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HelloAllWorlds
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            string name = "DefaultApi";
            string text = "api/{controller}/{id}";
            object defaults = new
            {
                id = RouteParameter.Optional
            };
            routes.MapHttpRoute(name, text, defaults);
            name = "Default";
            text = "{controller}/{action}/{id}";
            defaults = new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            };
            routes.MapRoute(name, text, defaults);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            MvcApplication.RegisterGlobalFilters(GlobalFilters.Filters);
            MvcApplication.RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }
}