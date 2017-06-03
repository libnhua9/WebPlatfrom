using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //请勿删除该路由 添加人 ：李彬华  日期2017/4/13  131314646464
            routes.MapRoute("AddContollerRoute", "{controller}/{action}/{id}/{device}", 
                new { controller = "DevManage", action = "AddMark", id = UrlParameter.Optional , device = UrlParameter.Optional }
                );
        }
    }
}
