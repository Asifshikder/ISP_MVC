using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_ISP.Controllers
{
    public class DataTableController : Controller
    {
        // GET: DataTable
        public ActionResult GetAllClients()
        {
            //DataTable dt = RunQuery.GetData("select * from r");
            return View();
        }
    }
}