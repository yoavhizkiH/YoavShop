using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YoavShop.Models;

namespace YoavShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalVariables.Initialize();
        }
    }

    public class GlobalVariables
    {
        public static UserInfo StoreUser { get; set; }
        public static string Role{ get; set; }

        public static void Initialize()
        {
            StoreUser = new UserInfo();
            Role = "";
        }
    }
}
