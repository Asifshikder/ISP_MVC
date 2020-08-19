using ISP_ManagementSystemModel.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Timers;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Project_ISP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Add the following to the top of the main class, outside of any subroutines:
        private static double TimerIntervalInMilliseconds = Convert.ToDouble(WebConfigurationManager.AppSettings["TimerIntervalInMilliseconds"]);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            Debug.WriteLine(string.Concat("Application_Start Called: ", DateTime.Now.ToString()));

            // This will raise the Elapsed event every 'x' millisceonds (whatever you set in the
            // Web.Config file for the added TimerIntervalInMilliseconds AppSetting
            Timer timer = new Timer(TimerIntervalInMilliseconds);

            timer.Enabled = true;

            // Setup Event Handler for Timer Elapsed Event
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            timer.Start();
        }
        // Added the following procedure:
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Get the TimerStartTime web.config value
            DateTime MyScheduledRunTime = DateTime.Parse(WebConfigurationManager.AppSettings["TimerStartTime"]);

            // Get the current system time
            DateTime CurrentSystemTime = DateTime.Now;

            Debug.WriteLine(string.Concat("Timer Event Handler Called: ", CurrentSystemTime.ToString()));

            // This makes sure your code will only run once within the time frame of (Start Time) to
            // (Start Time+Interval). The timer's interval and this (Start Time+Interval) must stay in sync
            // or your code may not run, could run once, or may run multiple times per day.
            DateTime LatestRunTime = MyScheduledRunTime.AddMilliseconds(TimerIntervalInMilliseconds);

            // If within the (Start Time) to (Start Time+Interval) time frame - run the processes
            if ((CurrentSystemTime.CompareTo(MyScheduledRunTime) >= 0) && (CurrentSystemTime.CompareTo(LatestRunTime) <= 0))
            {
                Debug.WriteLine(String.Concat("Timer Event Handling MyScheduledRunTime Actions: ", DateTime.Now.ToString()));
                ClientController cc = new ClientController();
                cc.LockSystemClientList();
                ////ClientController.LockSystemClientList();
                //var routeData = new RouteData();
                //routeData.Values["controller"] = "Client";
                //   routeData.Values["action"] = "Index"; 

                //   IController controller = new HomeController();
                //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                //controller.Execute(rc);
            }
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }
    }
}
