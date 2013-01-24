using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComputerShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*");

            //routes.Add(new ServiceRoute("Service", new WebServiceHostFactory(), typeof(Data.Context.ComputerShopContext)));

            //routes.MapHttpRoute(
            //    name: "Api",
            //    routeTemplate: "api"
            //    );

            //routes.MapHttpRoute(
            //    name: "ComputerApi",
            //    routeTemplate: "computerApi/{id}",
            //    defaults: new {controller = "ComputerApi", id = RouteParameter.Optional}
            //    );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}