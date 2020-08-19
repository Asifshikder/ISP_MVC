using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;


namespace Project_ISP
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        private ISPContext db = new ISPContext();
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            IEnumerable<Transaction> lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).AsEnumerable();

            System.Web.HttpContext.Current.Session["AllClientInThisMonth"]     = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (Key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault()).Count();
            System.Web.HttpContext.Current.Session["ActiveClientInThisMonth"]  = lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.AmountCountDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsActive).Count();
            System.Web.HttpContext.Current.Session["LockClientInThisMonth"] =(int)System.Web.HttpContext.Current.Session["AllClientInThisMonth"] - (int)System.Web.HttpContext.Current.Session["ActiveClientInThisMonth"];//db.ClientLineStatus.ToList().GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock).Count(); ;

            System.Web.HttpContext.Current.Session["Employee"] = db.Employee.Where(s=>s.LoginName != "ReallyUnknownPerson" && s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).Count();

            System.Web.HttpContext.Current.Session["NewConnection"]     = db.ClientDetails.Where(s=>s.IsNewClient == AppUtils.isNewClient).Count();
            System.Web.HttpContext.Current.Session["AdvancePayment"]     = db.AdvancePayment.GroupBy(s=>s.ClientDetailsID).Count();
            System.Web.HttpContext.Current.Session["PhoneNumber"]     = db.ClientDetails.Where(s=> !string.IsNullOrEmpty( s.ContactNumber)).Count();
            System.Web.HttpContext.Current.Session["ComplainPanding"]  = db.Complain.Where(s=>s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
            System.Web.HttpContext.Current.Session["TotalMikrotikInUsed"] = db.ClientDetails.Select(s=>s.MikrotikID).Distinct().Count();

            ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            //filterContext.ActionParameters["roleId"] = int.Parse(claimsIdentity.FindFirst("Role").Value);
            //FindFirst(System.Security.Claims.ClaimTypes.Role).Value
            //var RoleID = int.Parse(claimsIdentity.FindFirst((System.Security.Claims.ClaimTypes.Role)).Value);


            HttpContext ctx = HttpContext.Current;
            
            if (HttpContext.Current.Session["role_id"] == null || claimsIdentity.IsAuthenticated != true)
            {
                filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}