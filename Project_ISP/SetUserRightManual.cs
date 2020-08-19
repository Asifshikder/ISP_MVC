

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
    public class SetUserRightManual : ActionFilterAttribute
    {
        private ISPContext db = new ISPContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var loginID = AppUtils.GetLoginUserID();
             if (AppUtils.GetLoginRoleID() == AppUtils.ClientRole )
            {
                HttpContext.Current.Session["CurrentUserRightPermission"] = db.ClientDetails.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole || AppUtils.GetLoginRoleID() == AppUtils.SuperUserRole || AppUtils.GetLoginRoleID() == AppUtils.EmployeeRole)
            {
                HttpContext.Current.Session["CurrentUserRightPermission"] = db.Employee.Where(s => s.EmployeeID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
            }
            else
            {
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
                HttpContext.Current.Session["lstAccessList"] = ((bool)HttpContext.Current.Session["MikrotikOptionEnable"] == true && (bool)HttpContext.Current.Session["SMSOptionEnable"] == true) ? lstAcessList.ToList()
                    : ((bool)HttpContext.Current.Session["MikrotikOptionEnable"]) ? lstAcessList.Where(s => !AppUtils.lstSMSReleated.Contains(s)).ToList()
                        : ((bool)HttpContext.Current.Session["SMSOptionEnable"] == true) ? lstAcessList.Where(s => !AppUtils.lstMikrotikReleated.Contains(s)).ToList()
                            : lstAcessList.Where(s => (!AppUtils.lstMikrotikReleated.Contains(s) && !AppUtils.lstSMSReleated.Contains(s))).ToList();

                AppUtils.GetTempNotUpdateEmployee = ConfigurationManager.AppSettings["EmployeeList"].Split(',').ToList();

                if (AppUtils.LstAccessCount() < 1)
                {
                    filterContext.Result = new Http403Result();
                    //filterContext.Result = new RedirectResult("~/Account/LoginByClient");
                    return;
                }
            }

            else
            {
                filterContext.Result = new Http403Result();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}