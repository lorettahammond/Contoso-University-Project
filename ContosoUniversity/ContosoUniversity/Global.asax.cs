using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ContosoUniversity.DAL;
using System.Data.Entity.Infrastructure.Interception;

namespace ContosoUniversity
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new SchoolInterceptorTransientErrors());    //DbInterception lines: causes your interceptor code to be run when Entity Framework 
                                                                           //sends queries to the database. Notice that because you created separate interceptor 
                                                                           //classes for transient error simulation and logging, you can independently enable and disable them.
            DbInterception.Add(new SchoolInterceptorLogging());
        }
    }
}

//pg. 86, Contoso University

//Interceptors are executed in the order of registration (the order in which the DbInterception.Add method is called).


