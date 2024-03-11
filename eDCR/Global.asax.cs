using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using eDCR.Universal.Common;

namespace eDCR
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Application["SiteVisitedCounter"] = 0;
            Application["OnlineCounter"] = 0;
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalFilters.Filters.Add(new HandleErrorAttribute());
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<System.Web.Mvc.JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new MyJsonValueProviderFactory());



        }


        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown  

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs  

        }
        //https://dailydotnettips.com/how-to-count-number-of-active-session-for-state-server-session-mode/
        void Session_Start(object sender, EventArgs e)
        {
            if (Application["OnlineCounter"] != null)
            {            
                Application.Lock();
                Application["SiteVisitedCounter"] = Convert.ToInt32(Application["SiteVisitedCounter"]) + 1;
                Application["OnlineCounter"] = ((int)Application["OnlineCounter"]) + 1;
                Session["Counter"] = Application["OnlineCounter"];
                Application.UnLock();
            }     
        }

        void Session_End(object sender, EventArgs e)
        {
            if (Application["OnlineCounter"] != null)
            {
                Application.Lock();
                Application["OnlineCounter"] = (int)Application["OnlineCounter"] - 1;
                Session["Counter"] = Application["OnlineCounter"];
                Application.UnLock();
            }
        }
    }
}