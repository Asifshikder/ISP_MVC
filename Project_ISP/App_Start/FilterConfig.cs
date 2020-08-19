using System.Web;
using System.Web.Mvc;

namespace Project_ISP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SessionTimeoutAttribute());
        }
    }
}
