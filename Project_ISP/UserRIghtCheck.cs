using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;

namespace Project_ISP
{
    internal class Http403Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Set the response code to 403.
            context.HttpContext.Response.StatusCode = 403;
        }
    }
    public class UserRIghtCheck : ActionFilterAttribute
    {
        private ISPContext db = new ISPContext();

        public string ControllerValue;
        //public UserRIghtCheck(string s)
        //{
        //    ControllerValue = s;
        //}


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {



            if (HttpContext.Current.Session["LoginEmpName"] == "ReallyUnknownPerson")
            {
                return;
            }
            else
            {
                var loginID = AppUtils.GetLoginUserID();
                if (AppUtils.GetLoginRoleID() != AppUtils.ResellerRole)
                {
                    HttpContext.Current.Session["CurrentUserRightPermission"] = db.Employee.Where(s => s.EmployeeID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
                }
                else {
                    HttpContext.Current.Session["CurrentUserRightPermission"] = db.Reseller.Where(s => s.ResellerID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
                }
                int CurrentUserRightPermission = (int)HttpContext.Current.Session["CurrentUserRightPermission"];

                UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();
                if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                {
                    int MikrotikOptionEnable;
                    int SMSOptionEnable;
                    //string[] lstMikrotikReleated = { "91", "92", "93", "94", "95", "96" };
                    //string[] lstSMSReleated = { "88", "89", "90" };
                    List<OptionSettings> lstOptionSettings = db.OptionSettings.AsNoTracking().ToList();
                    //SMSOptionEnable = lstOptionSettings[0].Status;
                    //MikrotikOptionEnable = lstOptionSettings[1].Status;

                    HttpContext.Current.Session["MikrotikOptionEnable"] = (lstOptionSettings[1].Status == 1) ? true : false;
                    HttpContext.Current.Session["SMSOptionEnable"] = (lstOptionSettings[0].Status == 1) ? true : false;

                    List<string> lstAcessList = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                   HttpContext.Current.Session["lstAccessList"] = ((bool)HttpContext.Current.Session["MikrotikOptionEnable"] && (bool)HttpContext.Current.Session["SMSOptionEnable"] == true) ? lstAcessList.ToList()
                        : ((bool)HttpContext.Current.Session["MikrotikOptionEnable"]) ? lstAcessList.Where(s => !AppUtils.lstSMSReleated.Contains(s)).ToList()
                            : ((bool)HttpContext.Current.Session["SMSOptionEnable"] == true) ? lstAcessList.Where(s => !AppUtils.lstMikrotikReleated.Contains(s)).ToList()
                                : lstAcessList.Where(s => (!AppUtils.lstMikrotikReleated.Contains(s) && !AppUtils.lstSMSReleated.Contains(s))).ToList();

                    AppUtils.GetTempNotUpdateEmployee = ConfigurationManager.AppSettings["EmployeeList"].Split(',').ToList();
                     
                    ClaimsIdentity claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;

                    if (AppUtils.LstAccessCount() < 1)
                    {
                        filterContext.Result = new Http403Result();
                        //filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                        return;
                    }
                    else
                    {

                        if (!AppUtils.HasAccessInTheList(ControllerValue))
                        {
                            filterContext.Result = new Http403Result();

                            //throw new UnauthorizedAccessException();
                            //   throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Forbidden");


                            //return Content(HttpStatusCode.Forbidden, "RFID is disabled for this site.");
                            //HttpContext.Current.Session["role_id"] = null;
                            //claimsIdentity = null;
                            //    filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                            //    return;
                        }
                    }
                }

                else
                {
                    filterContext.Result = new Http403Result();
                }
            }



            base.OnActionExecuting(filterContext);
        }
    }
}