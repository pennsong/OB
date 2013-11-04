using OB.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace OB
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //建立数据库
            Database.SetInitializer<OBContext>(new OBInitializer());

            using (var context = new OBContext())
            {
                System.Data.Objects.ObjectContext objcontext = ((IObjectContextAdapter)context).ObjectContext;
            }
            //调用WebSecurity的方法前需先调用InitializeDatabaseConnection初始化
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("OB", "User", "Id", "Name", autoCreateTables: true);
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}