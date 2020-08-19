using ISP_ManagementSystemModel.Models;
using Project_ISP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel.ViewModel;
using System.Configuration;
using System.Globalization;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ComplainController : Controller
    {
        public ComplainController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        public ActionResult GetAllComplainListForSpecificUser()
        {
            //setViewBagList();

            var loginID = AppUtils.GetLoginUserID();
            List<Complain> lstComplain = db.Complain.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/).OrderByDescending(s => s.ComplainTime).ToList();
            return View(lstComplain);
        }


        [HttpGet]
        public ActionResult GetAllComplainListForSpecificUserFromDashBoardSearch(int CID)
        {
            setViewBagList();
            List<Complain> lstComplain = db.Complain.Where(s => s.ClientDetailsID == CID).OrderByDescending(s => s.ComplainTime).ToList();
            ViewBag.popName = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.Name : "";
            ViewBag.popLoginName = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.LoginName : "";
            ViewBag.ClientZone = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.Zone.ZoneName : "";
            ViewBag.ClientAddress = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.Address : "";
            ViewBag.ConnectionType = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.ConnectionType.ConnectionTypeName : "";
            ViewBag.ContactNumber = lstComplain.Count > 0 ? lstComplain[0].ClientDetails.ContactNumber : "";
            return View(lstComplain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchComplainBySearchCriteria(int? YearID, int? MonthID, int? EmployeeID)
        {

            if (YearID != null)
            {
                YearID = Convert.ToInt32(db.Year.Where(s => s.YearID == YearID.Value).Select(s => s.YearName).FirstOrDefault());
            }

            List<Complain> lstComplain = new List<Complain>();

            if (YearID > 0 && MonthID > 0 && EmployeeID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate && s.EmployeeID == EmployeeID).OrderByDescending(s => s.ComplainTime).ToList();
            }
            else if (YearID > 0 && MonthID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
            }
            else if (YearID > 0 && EmployeeID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, 1, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate && s.EmployeeID == EmployeeID).OrderByDescending(s => s.ComplainTime).ToList();
            }
            else if (YearID > 0)
            {

                DateTime startDate = new DateTime(YearID.Value, 1, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
            }
            else if (EmployeeID != null)
            {

                lstComplain = db.Complain.Where(s => s.EmployeeID == EmployeeID).OrderByDescending(s => s.ComplainTime).ToList();
            }
            else
            {
                lstComplain = db.Complain.ToList();
            }
            var ss = lstComplain.Select(s => new
            {
                CName = (s.ClientDetails != null) ? s.ClientDetails.Name : "",
                ClientDetailsID = (s.ClientDetails != null) ? s.ClientDetailsID.ToString() : "",
                TransactionID = (s.ClientDetails != null) ? db.Transaction.Where(sss => s.ClientDetailsID == s.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",


                ComplainID = s.ComplainID,
                TokenNo = s.TokenNo,
                LoginName = s.ClientDetails.LoginName,
                Address = s.ClientDetails.Address,
                ZoneName = s.ClientDetails.Zone.ZoneName,
                ContactNumber = s.ClientDetails.ContactNumber,
                ComplainDetails = s.ComplainDetails,
                Name = s.Employee.Name,
                LineStatusName = s.LineStatus.LineStatusName,
                LineStatusID = s.LineStatus.LineStatusID,
                ComplainTime = s.ComplainTime,
                ComplainOpenBy = db.Employee.Find(s.ComplainOpenBy).Name
            });
            //$("#tblComplainList>tbody").append("<div><tr role='row' class='odd'><td><input type='hidden' value=" + item.ComplainID + "></td>\
            //       < td > "+ item.TokenNo + " </ td >< td > " + item.ClientDetails.LoginName + " </ td >< td > " + item.ClientDetails.Address + " </ td >< td > " + item.ClientDetails.Zone.ZoneName + " </ td >\
            //       < td > "+ item.ClientDetails.ContactNumber + " </ td >< td > " + item.ComplainDetails + " </ td >< td ></ td >< td > " + item.Employee.Name + " </ td >< td ></ td >< td > " + item.LineStatus.LineStatusName + " </ td >\
            //       < td < div style = 'width: 20%; float: left' >< button type = 'button' id = 'btnEdit' class='btn btn-success btn-block' style='width: 20px;'>Edit@*<span class='glyphicon glyphicon-ok'></span>*@</button></div>\
            //           <div style = 'width: 20%; float: right' >< button type='button' id='btnDelete' class='btn btn-danger btn-block' style='width: 20px;'>Delete@*<span class='glyphicon glyphicon-remove'></span>*@</button></div>></td>\
            //      </tr></div>");


            return Json(new { Success = true, ComplainList = ss, ComplainCount = lstComplain.Count() }, JsonRequestBehavior.AllowGet);

        }

        //
        // GET: /Complain/


        [UserRIghtCheck(ControllerValue = AppUtils.Create_Complain)]
        public ActionResult CreateComplain()
        {
            setViewBagList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getAutoCompleateDetailsInformation(int ClientDetsilsID)
        {
            try
            {
                var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetsilsID).Select(s => new { Mobile = s.ContactNumber, ClientAdress = "Zone: " + s.Zone.ZoneName + " Address: " + s.Address }).FirstOrDefault();
                return Json(new { Success = true, ClientDetails = ClientDetails, itemgiven = GetElements(ClientDetsilsID) }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveComplain(int ClientDetailsID, string Complain, int ComplainTypeID, string WhichOrWhere, bool SendSMS, int EmployeeID = 0)
        {
            //db.Entry(new Transaction()).CurrentValues.SetValues(new Transaction());

            try
            {
                string message = "";
                if (ComplainTypeID > 0)
                {
                    message += " Type: " + db.ComplainType.Find(ComplainTypeID).ComplainTypeName + " ";
                }
                if (!string.IsNullOrEmpty(WhichOrWhere))
                {
                    message += "Where: " + WhichOrWhere + "  ";
                }
                if (!string.IsNullOrEmpty(Complain))
                {
                    message += "Complain: " + Complain + "";
                }
                message += ".";

                Token ticket = db.Token.Find(1);

                Complain insertComplain = new Complain();
                insertComplain.MonthlySerialNo = getLatestSerialOnThisMonth();
                insertComplain.ComplainTypeID = ComplainTypeID;
                insertComplain.ClientDetailsID = ClientDetailsID;
                if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
                {
                    insertComplain.EmployeeID = EmployeeID;
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    insertComplain.ResellerID = AppUtils.GetLoginUserID();
                }

                insertComplain.ComplainDetails = Complain;
                insertComplain.WhichOrWhere = WhichOrWhere;
                insertComplain.ComplainTime = AppUtils.GetDateTimeNow();
                insertComplain.LineStatusID = AppUtils.ComplainPendingStatus;
                insertComplain.ComplainOpenBy = AppUtils.GetLoginUserID();
                insertComplain.TokenNo = ticket.TokenNumber;
                db.Complain.Add(insertComplain);
                db.SaveChanges();

                if (insertComplain.ComplainID > 0)
                {
                    ticket.TokenNumber += 1;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Complain Save Successfull.";
                }
                if (SendSMS)
                {
                    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                    if ((bool)Session["SMSOptionEnable"])
                    {
                        try
                        {
                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMS smsMember = db.SMS.Where(s => s.SMSCode == AppUtils.SMS_Member_Complain_Open && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (smsMember != null)
                                {
                                    //[TICKET-NO] [LOGIN-NAME] [ZONE] [ADDRESS] [CLIENT-DETAILS] [COMPLAIN] [ASSIGN-TO] [CLIENT-MOBILE] 
                                    var messageEmployee = smsMember.SendMessageText;
                                    ClientDetails clientDetails = db.ClientDetails.Find(insertComplain.ClientDetailsID);
                                    Employee employee = db.Employee.Find(insertComplain.EmployeeID);

                                    messageEmployee = messageEmployee.Replace("[TICKET-NO]", insertComplain.TokenNo.ToString() + "."); messageEmployee = messageEmployee.Replace("[LOGIN-NAME]", clientDetails.Name + ".");
                                    messageEmployee = messageEmployee.Replace("[ZONE]", clientDetails.Zone.ZoneName + "."); messageEmployee = messageEmployee.Replace("[ADDRESS]", clientDetails.Address + ".");
                                    messageEmployee = messageEmployee.Replace("[COMPLAIN]", message); messageEmployee = messageEmployee.Replace("[CLIENT-MOBILE]", clientDetails.ContactNumber + ".");
                                    SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, employee.Phone, messageEmployee);
                                    if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                    {
                                        smsMember.SMSCounter += 1;
                                        db.Entry(smsMember).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                                SMS smsCLient = db.SMS.Where(s => s.SMSCode == AppUtils.SMS_User_Complain_Open && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (smsCLient != null)
                                {//[TICKET-NO] [COMPLAIN] [EmployeeName][EmployeeAddress]  
                                    var messageClient = smsCLient.SendMessageText;
                                    Employee employee = db.Employee.Find(insertComplain.EmployeeID);
                                    ClientDetails clientDetails = db.ClientDetails.Find(insertComplain.ClientDetailsID);
                                    messageClient = messageClient.Replace("[TICKET-NO]", insertComplain.TokenNo.ToString() + "."); messageClient = messageClient.Replace("[COMPLAIN]", message);
                                    messageClient = messageClient.Replace("[EmployeeName]", employee.Name + "."); messageClient = messageClient.Replace("[EmployeeMobile]", employee.Phone + ".");

                                    SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, clientDetails.ContactNumber, messageClient);
                                    if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                    {
                                        smsCLient.SMSCounter += 1;
                                        db.Entry(smsCLient).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        { }
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult CreateComplainByClient()
        {
            setViewBagList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveComplainByClient(string Complain)
        {
            try
            {
                Token ticket = db.Token.Find(1);

                Complain complainByClient = new Complain();
                complainByClient.ComplainTypeID = 1;
                complainByClient.ClientDetailsID = AppUtils.LoginUserID;
                complainByClient.ComplainDetails = Complain;
                complainByClient.MonthlySerialNo = getLatestSerialOnThisMonth();
                complainByClient.ComplainTime = AppUtils.GetDateTimeNow();
                complainByClient.LineStatusID = AppUtils.ComplainByClientPendingStatus;
                complainByClient.TokenNo = ticket.TokenNumber;

                int cid = AppUtils.GetLoginUserID();
                int? reserllerID = db.ClientDetails.Find(cid).ResellerID;
                if (reserllerID != null)
                {
                    complainByClient.ResellerID = db.ClientDetails.Find(cid).ResellerID;
                }

                db.Complain.Add(complainByClient);
                db.SaveChanges();


                if (complainByClient.ComplainID > 0)
                {
                    ticket.TokenNumber += 1;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Complain Save Successfull.";
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        private string getLatestSerialOnThisMonth()
        {
            var monthName = AppUtils.GetDateTimeNow().ToString("MMM", CultureInfo.InvariantCulture);
            string newSerialForThisMonth = "";
            DateTime std = AppUtils.ThisMonthStartDate();
            DateTime lstd = AppUtils.ThisMonthLastDate();
            Complain complain = db.Complain.Where(s => s.ComplainTime >= std && s.ComplainTime <= lstd).OrderByDescending(s => s.ComplainID).FirstOrDefault();
            if (complain != null)
            {
                string serial = complain.MonthlySerialNo;
                if (string.IsNullOrEmpty(serial))
                {
                    newSerialForThisMonth = monthName + "-1";
                }
                else
                {
                    string[] onlySerial = serial.Split('-');
                    newSerialForThisMonth = monthName + "-" + (int.Parse(onlySerial[1]) + 1);
                }
            }
            else
            {

                newSerialForThisMonth = monthName + "-1";

            }
            return newSerialForThisMonth;
        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_Complain_List)]
        public ActionResult GetAllComplainList()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");


            setViewBagList();
            //List<Complain> lstComplain = db.Complain.Where(s => s.LineStatusID != AppUtils.DeleteStatus && s.LineStatusID != AppUtils.SolveStatus).OrderByDescending(s => s.ComplainTime).ToList();

            //List<int> clientDetailsID = lstComplain.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID).Distinct().ToList();
            //if (clientDetailsID.Count > 0)
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
            //                  .Select(s => new ClientSetByViewBag
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}
            //else
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //                  .Select(s => new
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}


            return View(new List<Complain>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OnRequestChange(int ComplainID, bool Status)
        {

            try
            {
                var complain = db.Complain.Where(s => s.ComplainID == ComplainID).FirstOrDefault();
                if (complain != null)
                {
                    complain.OnRequest = Status ? true : false;
                    complain.UpdateBy = AppUtils.LoginUserID.ToString();
                    complain.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(complain).State = EntityState.Modified;
                    db.SaveChanges();


                    var JSON = Json(new { UpdateSuccess = true, Status = Status == true ? true : false }, JsonRequestBehavior.AllowGet);
                    JSON.MaxJsonLength = int.MaxValue;
                    return JSON;
                }
                else
                {

                    var JSON = Json(new { UpdateSuccess = false, Status = Status == true ? false : true }, JsonRequestBehavior.AllowGet);
                    JSON.MaxJsonLength = int.MaxValue;
                    return JSON;
                }
            }
            catch (Exception ex)
            {
                return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetComplainAJAXData()
        {
            List<TimePeriodForSignal> timePeriodForSignals = db.TimePeriodForSignal.OrderBy(s => s.UpToHours).ToList();
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int employeeFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var EmployeeID = Request.Form.Get("EmployeeID");
                var ComplainStatus = Request.Form.Get("ComplainStatus");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                int complainStatus = int.Parse(ComplainStatus);
                if (!string.IsNullOrEmpty(YearID))
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (!string.IsNullOrEmpty(EmployeeID))
                {
                    employeeFromDDL = int.Parse(EmployeeID);
                }

                List<CustomComplain> lstComplainInformation = new List<CustomComplain>();

                DateTime StartDate = AppUtils.GetDateTimeNow();
                DateTime EndDate = AppUtils.GetDateTimeNow();
                SettingsDateTime(YearID, MonthID, EmployeeID, ref StartDate, ref EndDate);
                var complainQureable = db.Complain.AsQueryable();

                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    var resellerid = AppUtils.GetLoginUserID();
                    complainQureable = complainQureable.Where(x => x.ResellerID == resellerid).AsQueryable();
                }
                else
                {
                    complainQureable = complainQureable.Where(x => x.ResellerID == null).AsQueryable();
                }

                var firstPartOfQuery =
                        (YearID != "" && MonthID != "" && EmployeeID != "") ? complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate && s.EmployeeID == employeeFromDDL).AsQueryable()
                            : (YearID != "" && MonthID != "" && EmployeeID == "") ? complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate).AsQueryable()
                                : (YearID != "" && MonthID == "" && EmployeeID != "") ? complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate && s.EmployeeID == employeeFromDDL).AsQueryable()
                                    : (YearID != "" && MonthID == "" && EmployeeID == "") ? complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate).AsQueryable()
                                     : (YearID == "" && MonthID == "" && EmployeeID != "") ? complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate && s.LineStatusID != AppUtils.DeleteStatus && s.EmployeeID == employeeFromDDL).AsQueryable()
                                            : complainQureable.Where(s => s.ComplainTime >= StartDate && s.ComplainTime <= EndDate && s.LineStatusID != AppUtils.DeleteStatus).AsQueryable()
                    ;
                if (complainStatus > 0)
                {
                    var onRequestID = Convert.ToInt32(ConfigurationManager.AppSettings["OnRequestID"]);
                    if (complainStatus == AppUtils.ComplainPendingStatus)
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus || s.OnRequest == true).AsQueryable();
                    }
                    else if (complainStatus == AppUtils.ComplainSolveStatus)
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.LineStatusID == AppUtils.ComplainSolveStatus).AsQueryable();
                    }
                    else if (complainStatus == onRequestID)
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.OnRequest == true).AsQueryable();
                    }
                    else
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.LineStatusID == AppUtils.ComplainByClientPendingStatus).AsQueryable();
                    }
                }

                var secondPartOfQuery = firstPartOfQuery
                         .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
                             Complain => Complain.ClientDetailsID,
                             Transaction => Transaction.ClientDetailsID, (Complain, Transaction) => new
                             {
                                 Complain = Complain,
                                 Transaction = Transaction
                             })
                             .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), complain => complain.Complain.ClientDetailsID, cls => cls.ClientDetailsID, (complain, cls) => new
                             {
                                 Complain = complain.Complain,
                                 Transaction = complain.Transaction,
                                 ClientLineStatus = cls.FirstOrDefault()
                             })
                     .AsEnumerable();

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    //var test = secondPartOfQuery.ToList();

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.Complain.TokenNo.ToString().ToLower().Contains(search.ToLower())
                                                                                        //|| p.Complain.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Complain.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Complain.ClientDetails.LoginName.ToLower().ToString().ToLower().Contains(search.ToLower())
                                                                                        ).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.Complain.TokenNo.ToString().ToLower().Contains(search.ToLower())
                                                                     //|| p.Complain.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Complain.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Complain.ClientDetails.LoginName.ToLower().ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    //     var a = secondPartOfQuery.ToList();
                    lstComplainInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new CustomComplain()
                        {
                            ComplainID = s.Complain.ComplainID,
                            ClientDetailsID = s.Complain.ClientDetailsID,
                            TransactionID = s.Transaction.FirstOrDefault().TransactionID,
                            TicketNo = s.Complain.TokenNo.ToString(),
                            RunningMonthSerial = "<div id = 'Status' class='label label-success' style='font-weight:bold'>" + s.Complain.MonthlySerialNo + "</div>",
                            ClientName = s.Complain.ClientDetails.Name,
                            ClientLoginName = s.Complain.ClientDetails.LoginName,
                            Address = s.Complain.ClientDetails.Address,
                            Zone = s.Complain.ClientDetails.Zone.ZoneName,
                            Contact = s.Complain.ClientDetails.ContactNumber,
                            ComplainType = s.Complain.ComplainType.ComplainTypeName,
                            Complain = s.Complain.ComplainDetails,
                            WhichOrWhere = s.Complain.WhichOrWhere,
                            Time = s.Complain.ComplainTime.ToString(),
                            AssignTo = s.Complain.Employee != null ? s.Complain.Employee.Name : "",
                            ComplainOpenBy = (s.Complain.ResellerID != null) ? db.Reseller.Find(s.Complain.ResellerID).ResellerBusinessName : s.Complain.ComplainOpenBy > 0 ? db.Employee.Find(s.Complain.ComplainOpenBy).Name : "",
                            Status = getStatus(s.Complain.LineStatusID),
                            SignalInString = GetSignalInString(timePeriodForSignals, s.Complain.ComplainTime),
                            DeleteUpdate = (ISP_ManagementSystemModel.AppUtils.IsGranted(ISP_ManagementSystemModel.AppUtils.Update_Complain_Status)) ? true : false,
                            ButtonList = getButtonList(s.Complain, s.Complain.LineStatusID),
                            IsPriorityClient = s.Complain.ClientDetails.IsPriorityClient,
                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                            TotalComplainCount = db.Complain.Where(x => x.ClientDetailsID == s.Complain.ClientDetailsID).Count(),
                            totalelementsgiven = GetElements(s.Complain.ClientDetailsID)
                        }).ToList();

                }
                // Sorting.   
                lstComplainInformation = this.SortByColumnWithOrder(order, orderDir, lstComplainInformation);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstComplainInformation
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }

        private string GetElements(int clientDetailsID)
        {
            string totalElementsGiven = "Item: ";
            var lstItemDistributions = db.Distribution.Where(x => x.ClientDetailsID == clientDetailsID).Select(x => x.StockDetails.Stock.Item.ItemName).ToList();
            var lstCableDistributions = db.CableDistribution.Where(x => x.ClientDetailsID == clientDetailsID).Select(x => new { boxName = x.CableStock.CableBoxName, quantiry = x.AmountOfCableUsed }).ToList();

            foreach (var item in lstItemDistributions)
            {
                totalElementsGiven += item + "/ ";
            }
            totalElementsGiven = totalElementsGiven.Trim('/') + "</br> Cable: ";
            foreach (var item in lstCableDistributions)
            {
                totalElementsGiven += item + "/ ";
            }
            totalElementsGiven = totalElementsGiven.Trim('/');
            return totalElementsGiven;
        }

        private string getButtonList(Complain complain, int lineStatusID)
        {//"<div style='width:100%; text-align:center;'><button type='button' id='btnEdit' class='btn btn-danger btn-sm'><span class='glyphicon glyphicon-edit'></span></button><button type='button' id='btnDelete' class='btn btn-danger btn-sm ' data-toggle='modal' data-target='#popModalForDeletePermently'><span class='glyphicon glyphicon-remove'></span></button><button type='button' id='btnSolve' class='btn btn-success btn-sm  '><span class='glyphicon glyphicon-ok'></span></button>    <label  class='label label-success'>On Request</label>  </div>": ""
         //<div class='checkbox checkbox-primary'><input id='' type='checkbox' checked=''><label for='checkbox2'></label ></div>

            if (lineStatusID == AppUtils.ComplainByClientPendingStatus)
            {
                return "<button title='accept client complain' type='button' id='btnAcceptClientComplain' class='btn btn-success btn-sm'  data-toggle='modal' data-target='#popModalForAcceptClientComplain'><span class='glyphicon glyphicon-ok'></span></button>";
            }
            else
            {
                var isOnRequest = complain.OnRequest ? "checked" : "";
                string s = "";
                s = (ISP_ManagementSystemModel.AppUtils.IsGranted(ISP_ManagementSystemModel.AppUtils.Update_Complain_Status)) ?
                    "<div style='width:100%; text-align:center;'><button type='button' id='btnEdit' class='btn btn-success btn-sm'><span class='glyphicon glyphicon-edit'></span></button><button type='button' id='btnDelete' class='btn btn-danger btn-sm ' data-toggle='modal' data-target='#popModalForDeletePermently'><span class='glyphicon glyphicon-remove'></span></button><button type='button' id='btnSolve' class='btn btn-success btn-sm  '><span class='glyphicon glyphicon-ok'></span></button>    <label  class='label label-success'>On Request<div class='checkbox checkbox-danger'><input name='checkboxtest' onclick='checkCheckbox(chk" + complain.ComplainID + ")' id='chk" + complain.ComplainID + "' type='checkbox' " + isOnRequest + " ><label for= 'chk" + complain.ComplainID + "'> </ label ></div></label>  </div>"
                    :
                    "<div class='checkbox checkbox-primary'><input id='chk" + complain.ComplainID + "' type='checkbox' ><label for='checkbox2'></label ></div>";
                return s;
            }


        }

        private string GetSignalInString(List<TimePeriodForSignal> lstTimePeriod, DateTime complainTime)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            var hourDIfference = Double.Parse(dt.Subtract(complainTime).TotalHours.ToString());
            var returnSignal = "";
            if (hourDIfference > 0 && hourDIfference <= lstTimePeriod[0].UpToHours)
            {
                returnSignal = lstTimePeriod[0].SignalSign == 1 ? "<div id='Status' class='label label-success'>Green</div>"
                                               : lstTimePeriod[0].SignalSign == 2 ? "<div id='Status' class='label label-warning'>Yellow</div>"
                                               : "<div id='Status' class='label label-danger'>Red</div>";
            }
            else if (hourDIfference > lstTimePeriod[0].UpToHours && hourDIfference <= lstTimePeriod[1].UpToHours)
            {
                returnSignal = lstTimePeriod[1].SignalSign == 1 ? "<div id='Status' class='label label-success'>Green</div>"
                                                : lstTimePeriod[1].SignalSign == 2 ? "<div id='Status' class='label label-warning'>Yellow</div>"
                                                : "<div id='Status' class='label label-danger'>Red</div>";
            }
            else
            {
                returnSignal = lstTimePeriod[2].SignalSign == 1 ? "<div id='Status' class='label label-success'>Green</div>"
                                               : lstTimePeriod[2].SignalSign == 2 ? "<div id='Status' class='label label-warning'>Yellow</div>"
                                               : "<div id='Status' class='label label-danger'>Red</div>";
            }
            return returnSignal;

        }

        private string getStatus(int status)
        {
            if (status == 6)
            {
                return "<div id = 'Status' class='label label-warning'>Pending</div>";
            }
            else if (status == 7)
            {
                return "<div id = 'Status' class='label label-error'>Delete</div>";
            }
            else if (status == 10)
            {
                return "<div id = 'Status' class='label label-warning'>Aproval Pending</div>";
            }
            else
            {
                return "<div id = 'Status' class='label label-success'>Solve</div>";
            }


        }

        private void SettingsDateTime(string YearID, string MonthID, string EmployeeID, ref DateTime startDate, ref DateTime endDate)
        {
            if (YearID != "" && MonthID != "" && EmployeeID != "")
            {
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));

            }
            else if (YearID != "" && MonthID != "" && EmployeeID == "")
            {
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));

            }
            else if (YearID != "" && MonthID == "" && EmployeeID != "")
            {
                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(int.Parse(YearID), 12)));

            }
            else if (YearID != "")
            {

                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(int.Parse(YearID), 12)));

            }
            else if (EmployeeID != "")
            {

                startDate = new DateTime(AppUtils.RunningYear, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, 12, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));
            }
            else
            {
                startDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, DateTime.DaysInMonth(AppUtils.RunningYear, AppUtils.RunningMonth)));
            }
        }

        private List<CustomComplain> SortByColumnWithOrder(string order, string orderDir, List<CustomComplain> data)
        {

            // Initialization.   
            List<CustomComplain> lst = new List<CustomComplain>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ComplainID).ToList() : data.OrderBy(p => p.ComplainID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TicketNo).ToList() : data.OrderBy(p => p.TicketNo).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Zone).ToList() : data.OrderBy(p => p.Zone).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ComplainType).ToList() : data.OrderBy(p => p.ComplainType).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Contact).ToList() : data.OrderBy(p => p.Contact).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Complain).ToList() : data.OrderBy(p => p.Complain).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Time).ToList() : data.OrderBy(p => p.Time).ToList();
                        break;
                    case "11":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignTo).ToList() : data.OrderBy(p => p.AssignTo).ToList();
                        break;
                    case "12":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ComplainOpenBy).ToList() : data.OrderBy(p => p.ComplainOpenBy).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ComplainID).ToList() : data.OrderBy(p => p.ComplainID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }


        private void setViewBagList()
        {
            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                ViewBag.EmployeeID = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.EmployeeIDForUpdate = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
                ViewBag.EmployeeIDForUpdate = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

            }
            var complainType = new SelectList(db.ComplainType.ToList(), "ComplainTypeID", "ComplainTypeName");
            ViewBag.ComplainType = complainType;
            ViewBag.ComplainTypeUpdate = complainType;

        }

        [HttpPost]
        public ActionResult UpdateComplainStatusToPending(int ComplainID)
        {
            try
            {
                Complain complain = db.Complain.Where(s => s.ComplainID == ComplainID).FirstOrDefault();
                complain.LineStatusID = AppUtils.ComplainPendingStatus;
                db.Entry(complain).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Status = complain.LineStatusID, ComplainID = ComplainID, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "", ComplainID = "", Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdateStatusByDelete(int ComplainID)
        {
            try
            {
                Complain complain = db.Complain.Where(s => s.ComplainID == ComplainID).FirstOrDefault();

                if (complain.LineStatusID != 7)
                {
                    complain.LineStatusID = 7;
                }
                db.Entry(complain).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { Status = complain.LineStatusID, ComplainID = ComplainID, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "", ComplainID = "", Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdateStatusBySolve(int ComplainID)
        {
            try
            {
                Complain complain = db.Complain.Where(s => s.ComplainID == ComplainID).FirstOrDefault();

                if (complain.LineStatusID != AppUtils.ComplainSolveStatus)
                {
                    complain.LineStatusID = AppUtils.ComplainSolveStatus;
                }
                db.Entry(complain).State = EntityState.Modified;
                db.SaveChanges();

                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                if ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMS smsMember = db.SMS.Where(s => s.SMSCode == AppUtils.SMS_Member_Complain_Close && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (smsMember != null)
                            {
                                //[TICKET-NO] [LOGIN-NAME] [ZONE] [ADDRESS] [CLIENT-DETAILS] [COMPLAIN] [ASSIGN-TO] [CLIENT-MOBILE] 
                                var messageEmployee = smsMember.SendMessageText;
                                //ClientDetails clientDetails = db.ClientDetails.Find(insertComplain.ClientDetailsID);
                                //Employee employee = db.Employee.Find(insertComplain.EmployeeID);

                                messageEmployee = messageEmployee.Replace("[TICKET-NO]", complain.TokenNo.ToString()); messageEmployee = messageEmployee.Replace("[NAME]", complain.ClientDetails.Name);
                                messageEmployee = messageEmployee.Replace("[ADDRESS]", complain.ClientDetails.Address); messageEmployee = messageEmployee.Replace("[COMPLAIN]", complain.ComplainDetails);
                                messageEmployee = messageEmployee.Replace("[SOLVED-TIME]", AppUtils.GetDateTimeNow().ToString());
                                SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, complain.Employee.Phone, messageEmployee);
                                if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    smsMember.SMSCounter += 1;
                                    db.Entry(smsMember).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            SMS smsCLient = db.SMS.Where(s => s.SMSCode == AppUtils.SMS_User_Complain_Close && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (smsCLient != null)
                            {//[TICKET-NO] [COMPLAIN] [EmployeeName][EmployeeAddress]  
                                var messageClient = smsCLient.SendMessageText;
                                messageClient = messageClient.Replace("[TICKET-NO]", complain.TokenNo.ToString()); messageClient = messageClient.Replace("[COMPLAIN]", complain.ComplainDetails);
                                messageClient = messageClient.Replace("[SOLVED-TIME]", AppUtils.GetDateTimeNow().ToString());

                                SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, complain.ClientDetails.ContactNumber, messageClient);
                                if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    smsCLient.SMSCounter += 1;
                                    db.Entry(smsCLient).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    { }
                }

                return Json(new { Status = complain.LineStatusID, ComplainID = ComplainID, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "", ComplainID = "", Success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult ShowComplainByIdForUpdate(int ComplainID)
        {
            List<string> lstShowComplainBox = System.Configuration.ConfigurationManager.AppSettings["ComposeTypeID"].Split(',').ToList();
            var result = db.Complain.Where(s => s.ComplainID == ComplainID).Select(
                s => new
                {
                    ComplainTypeID = s.ComplainTypeID,
                    ComplainID = s.ComplainID,
                    ComplainDetails = s.ComplainDetails,
                    Name = s.EmployeeID,
                    ContactNumber = s.ClientDetails.ContactNumber,
                    Address = s.ClientDetails.Address,
                    LoginName = s.ClientDetails.LoginName,
                    EmployeeID = s.EmployeeID,
                    WhichOrWhere = s.WhichOrWhere,
                    ShowComplainDiv = (lstShowComplainBox.Contains(s.ComplainTypeID.ToString())) ? true : false,
                    ShowWhichOrWhereDiv = db.ComplainType.Where(ss => ss.ComplainTypeID == s.ComplainTypeID).FirstOrDefault().ShowMessageBox == true ? true : false,
                    HasComplain = s.ComplainDetails.Length > 0 ? true : false,
                    HasWhichOrWhere = s.WhichOrWhere.Length > 0 ? true : false
                }).FirstOrDefault();
            var JSON = Json(new { result = result }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        public ActionResult UpdateComplain(Complain complain)
        {
            try
            {
                var result = db.Complain.Where(s => s.ComplainID == complain.ComplainID).ToList();


                complain.ComplainOpenBy = result.FirstOrDefault().ComplainOpenBy;
                complain.ComplainTime = result.FirstOrDefault().ComplainTime;
                complain.ClientDetailsID = result.FirstOrDefault().ClientDetailsID;
                //      complain.ComplainTypeID = result.FirstOrDefault().ComplainTypeID;
                complain.LineStatusID = result.FirstOrDefault().LineStatusID;
                complain.TokenNo = result.FirstOrDefault().TokenNo;
                complain.UpdateBy = AppUtils.LoginUserID.ToString();
                complain.UpdateDate = AppUtils.GetDateTimeNow();
                complain.ResellerID = result.FirstOrDefault().ResellerID;
                complain.EmployeeID = complain.EmployeeID;
                complain.MonthlySerialNo = result.FirstOrDefault().MonthlySerialNo;

                db.Entry(result.FirstOrDefault()).CurrentValues.SetValues(complain);
                db.SaveChanges();

                var complains = result.Select(s =>
                new
                {
                    ComplainID = s.ComplainID,
                    ComplainTypeName = s.ComplainType.ComplainTypeName,
                    ComplainDetails = s.ComplainDetails,
                    EmployeeID = s.EmployeeID != null ? s.Employee.Name : "",
                    WhichOrWhere = s.WhichOrWhere
                });
                var JSON = Json(new { UpdateSuccess = true, complains = complains }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateSuccess = false, complains = "" }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}