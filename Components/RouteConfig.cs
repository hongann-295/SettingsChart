using DotNetNuke.Web.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Christoc.Modules.SettingsChart2.Components
{
    public class RouteConfig : IMvcRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("SettingsChart2", "default", "{controller}/{action}",
                new[] { "Christoc.Modules.SettingsChart2.Controllers" });
        }
    }
}