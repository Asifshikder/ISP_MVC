using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using Project_ISP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Zone = System.Security.Policy.Zone;
using static Project_ISP.JSON_Antiforgery_Token_Validation;
using tik4net;
using Project_ISP.Custom_Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Project_ISP.Models;
using ISP_ManagementSystemModel.ViewModel.CustomClass;
using static ISP_ManagementSystemModel.AppUtils;
using System.Web.Script.Serialization;
using ISP_ManagementSystemModel.Custom_Model;

namespace ISP_ManagementSystemModel.Controllers
{
    public class ClientSerachTempData
    {
        public int ClientDetailsID { get; set; }
        public int ClientLineStatusID { get; set; }
        public string ClientName { get; set; }
        public string LoginID { get; set; }
        public string Package { get; set; }
        public string PackageThisMonth { get; set; }
        public string PackageNextMonth { get; set; }
        public string Address { get; set; }
        public string Zone { get; set; }
        public string Contact { get; set; }
        public string Status { get; set; }
        public string StatusThisMonth { get; set; }
        public string StatusNextMonth { get; set; }

    }

    public class SendSMSCustomInformation
    {
        public int ClientID { get; set; }
        public string Phone { get; set; }
    }

    public class PayBillCustomInformation
    {
        public int TransactionID { get; set; }
    }

    [SessionTimeout]
    [AjaxAuthorizeAttribute]//[UserRIghtCheck]

    public class ClientController : Controller
    {
        public ClientController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }

        public int request = 1;
        public int newConnection = 2;
        public int active = 3;
        public int inActive = 4;
        public int Lock = 5;

        ISPContext db = new ISPContext();



        [HttpPost]
        public ActionResult GetAutoCompleateInformationForSearchCriteria(string SearchText, int resellerid = 0)
        {
            try
            {
                List<AutoSearchBoxModel> lstAutoSearchBoxModel = new List<AutoSearchBoxModel>();
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                    lstAutoSearchBoxModel = db.ClientDetails.Where(s => s.ResellerID == resellerID && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole && resellerid > 0)
                {
                    lstAutoSearchBoxModel = db.ClientDetails.Where(s => s.ResellerID == resellerid && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }
                else
                {
                    lstAutoSearchBoxModel = db.ClientDetails.Where(s => s.ResellerID == null && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }

                return Json(new { lstAutoSearchBoxModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, lstClientDetails = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Line_Status_History)]
        public ActionResult GetAllClientStatusChangeHistoryByClientDetilsID(int? SearchID)
        {
            //string referer = Request.Headers["Referer"].ToString();

            ViewBag.SearchByClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient == null || s.IsNewClient == 0).Select(s => new { ClientDetailsID = s.ClientDetailsID, Name = s.Name }).ToList(), "ClientDetailsID", "Name");

            string macResellerType = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString())); ;
            var lstReseller = db.Reseller.Where(x => x.ResellerTypeListID == macResellerType).Select(x => new { x.ResellerID, x.ResellerLoginName });
            ViewBag.SearchByResellerID = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");

            List<CustomClientLineStatus> lstCustomClientLineStatus = new List<CustomClientLineStatus>();
            //if (SearchID.HasValue)
            //{
            //    lstCustomClientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == SearchID.Value).OrderByDescending(s => s.LineStatusChangeDate).Take(10)
            //                              .Select(s => new CustomClientLineStatus
            //                              {
            //                                  Name = s.ClientDetails.Name,
            //                                  LoginName = s.ClientDetails.LoginName,
            //                                  Package = s.Package.PackageName,
            //                                  Address = s.ClientDetails.Address,
            //                                  Zone = s.ClientDetails.Zone.ZoneName,
            //                                  Contact = s.ClientDetails.ContactNumber,
            //                                  Status = s.LineStatus.LineStatusID,
            //                                  Employee = s.Employee.Name,
            //                                  Time = s.LineStatusChangeDate != null ? s.LineStatusChangeDate.Value.ToString() : "",
            //                                  Reason = s.StatusChangeReason
            //                              })
            //                              .ToList();
            //}

            return View(lstCustomClientLineStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LineStatusChangeHistoryByClientDetailsID(int SearchID)
        {
            try
            {
                var lstClientLineStatus =
                          db.ClientLineStatus.Where(s => s.ClientDetailsID == SearchID).OrderByDescending(s => s.LineStatusChangeDate).Take(10)
                          .Select(s =>
                          new
                          {
                              Name = s.ClientDetails.Name,
                              LoginName = s.ClientDetails.LoginName,
                              Package = s.Package.PackageName,
                              Address = s.ClientDetails.Address,
                              Zone = s.ClientDetails.Zone.ZoneName,
                              Contact = s.ClientDetails.ContactNumber,
                              Status = s.LineStatus.LineStatusID,
                              Employee = s.EmployeeID,
                              Reseller = s.ResellerID,
                              Time = s.LineStatusChangeDate,
                              Reason = s.StatusChangeReason
                          }).AsEnumerable()
                          .Select(x => new
                          {
                              Name = x.Name,
                              LoginName = x.LoginName,
                              Package = x.Package,
                              Address = x.Address,
                              Zone = x.Zone,
                              Contact = x.Contact,
                              Status = x.Status,
                              Employee = x.Employee != null ? db.Employee.Find(x.Employee).Name : "",
                              Reseller = x.Reseller != null ? db.Reseller.Find(x.Reseller).ResellerLoginName : "",
                              Time = x.Time,
                              Reason = x.Reason
                          })
                          .ToList();

                return Json(new { Success = true, lstClientLineStatus = lstClientLineStatus }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, lstClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetClientListBySearchKeyWord(string KeyWord)
        {
            var lstTransactionForShowingCurenetMonthLineStatusInSearchFromDashBoard = db.Transaction.Where(s =>
                    s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
                .Select(s => new
                {
                    TransactionID = s.TransactionID,
                    ClientDetailsID = s.ClientDetailsID,
                    Package = s.Package.PackageName,
                    StatusThisMonth = s.LineStatusID
                });

            List<ClientSerachTempData> lstClientSerachTempData = db.ClientLineStatus.Where(s => s.ClientDetails.Name.Contains(KeyWord) || s.ClientDetails.LoginName.Contains(KeyWord)).GroupBy(s => s.ClientDetailsID, (Key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                .GroupJoin(lstTransactionForShowingCurenetMonthLineStatusInSearchFromDashBoard, ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID, (ClientLineStatus, Transaction) => new { ClientLineStatus = ClientLineStatus, Transaction = Transaction.FirstOrDefault() })

                .Select(s => new ClientSerachTempData
                {
                    ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                    ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                    ClientName = s.ClientLineStatus.ClientDetails.Name,
                    LoginID = s.ClientLineStatus.ClientDetails.LoginName,
                    Package = s.ClientLineStatus.Package.PackageName,
                    PackageNextMonth = s.ClientLineStatus.Package.PackageName,
                    PackageThisMonth = lstTransactionForShowingCurenetMonthLineStatusInSearchFromDashBoard.Any() ? s.Transaction.Package : s.ClientLineStatus.Package.PackageName,
                    Address = s.ClientLineStatus.ClientDetails.Address,
                    Zone = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                    Contact = s.ClientLineStatus.ClientDetails.ContactNumber,
                    StatusNextMonth = s.ClientLineStatus.LineStatusID.ToString(),
                    StatusThisMonth = lstTransactionForShowingCurenetMonthLineStatusInSearchFromDashBoard.Any() ? s.Transaction.StatusThisMonth.ToString() : s.ClientLineStatus.LineStatusID.ToString(),
                }).ToList();
            return Json(new { Success = true, ClientList = lstClientSerachTempData }, JsonRequestBehavior.AllowGet);
        }
        // [Authorize(Roles = "1")]

        [UserRIghtCheck(ControllerValue = AppUtils.View_Lock_To_Active_List)]
        public ActionResult LockToActive()
        {

            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");


            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");
            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");


            LoadYearAndMonth();
            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.ThisMonthLastDate();
            DateTime getDateTime = AppUtils.GetDateTimeNow();

            List<ClientLineStatus> lstClientLineStatus = db.ClientLineStatus.ToList();

            List<int> lstLockClientOnPreviousMonthall = lstClientLineStatus
              .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
              .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
              .Where(s => s.LineStatusID == 5).Select(s => s.ClientDetailsID).ToList();
            var lstActiveClientOnThisMonthAndLockOnPreviousMonth =
                lstClientLineStatus
                     .Where(s => s.LineStatusID == AppUtils.LineIsActive && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                        .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault())
                           .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection)
                                , ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID, (ClientLineStatus, Transaction) => new { ClientLineStatus = ClientLineStatus, Transaction = Transaction.FirstOrDefault() }
                           )
                           .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly)
                              , ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, TransactionM => TransactionM.ClientDetailsID, (ClientLineStatus, TransactionM) => new { ClientLineStatus = ClientLineStatus.ClientLineStatus, TransactionNewConnection = ClientLineStatus.Transaction, TransactionM = TransactionM.FirstOrDefault() }
                            )
                            .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                                     , ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, ClientLineNewStatus => ClientLineNewStatus.ClientDetailsID,
                                         (ClientLineStatus, ClientLineNewStatus) => new
                                         {
                                             ClientLineStatus = ClientLineStatus.ClientLineStatus,
                                             TransactionNewConnection = ClientLineStatus.TransactionNewConnection,
                                             TransactionM = ClientLineStatus.TransactionM,
                                             ClientLineNewStatus = ClientLineNewStatus.FirstOrDefault()
                                         })
                                     .Select(
                                            s => new LockToActiveOrActiveToLockCustom()
                                            {
                                                ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                                                TransactionID = s.TransactionNewConnection != null ? s.TransactionNewConnection.TransactionID : 0,
                                                ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                                                Name = s.ClientLineStatus.ClientDetails.Name,
                                                ClientLoginName = s.ClientLineStatus.ClientDetails.LoginName,
                                                Address = s.ClientLineStatus.ClientDetails.Address,
                                                ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                                                Zone = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                                                PackageName = s.TransactionM != null ? s.TransactionM.Package.PackageName : s.ClientLineStatus.ClientDetails.Package.PackageName,
                                                PackagePrice = s.TransactionM != null ? s.TransactionM.PaymentAmount.ToString() : s.ClientLineStatus.ClientDetails.Package.PackagePrice.ToString(),
                                                EmployeeName = db.Employee.Find(s.ClientLineStatus.EmployeeID).Name,
                                                LineStatusChangeDate = s.ClientLineStatus.LineStatusChangeDate.Value,
                                                IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,
                                                LineStatusActiveDate = s.ClientLineNewStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineNewStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                                            }).ToList();


            return View(lstActiveClientOnThisMonthAndLockOnPreviousMonth);
        }

        //    [Authorize(Roles = "1")]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Active_To_Lock_List)]
        public ActionResult ActiveToLock()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");


            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");


            //ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            //ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            //ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            //ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            //ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");

            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");
            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");

            LoadYearAndMonth();
            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.ThisMonthLastDate();
            DateTime getDateTime = AppUtils.GetDateTimeNow();

            List<ClientLineStatus> lstClientLineStatus = db.ClientLineStatus.ToList();
            List<int> lstActiveClientOnPreviousMonthall = lstClientLineStatus
               .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
               .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
               .Where(s => s.LineStatusID == 3).Select(s => s.ClientDetailsID).ToList();

            var lstLockClientOnThisMonthAndActiveOnPreviousMonth =
                lstClientLineStatus
                     .Where(s => s.LineStatusID == AppUtils.LineIsLock && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                        .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault())
                           .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection)
                                , ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID, (ClientLineStatus, Transaction) => new
                                {
                                    ClientLineStatus = ClientLineStatus,
                                    Transaction = Transaction.FirstOrDefault()
                                }
                           )
                           .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly), ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, TransactionM => TransactionM.ClientDetailsID,
                              (ClientLineStatus, TransactionM) => new
                              {
                                  ClientLineStatus = ClientLineStatus.ClientLineStatus,
                                  TransactionNewConnection = ClientLineStatus.Transaction,
                                  TransactionM = TransactionM.FirstOrDefault()
                              }
                            )
                            .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                                     , ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, ClientLineNewStatus => ClientLineNewStatus.ClientDetailsID,
                                         (ClientLineStatus, ClientLineNewStatus) => new
                                         {
                                             ClientLineStatus = ClientLineStatus.ClientLineStatus,
                                             TransactionNewConnection = ClientLineStatus.TransactionNewConnection,
                                             TransactionM = ClientLineStatus.TransactionM,
                                             ClientLineNewStatus = ClientLineNewStatus.FirstOrDefault()
                                         })
                                     .Select(
                                            s => new LockToActiveOrActiveToLockCustom()
                                            {
                                                ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                                                TransactionID = s.TransactionNewConnection != null ? s.TransactionNewConnection.TransactionID : 0,
                                                ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                                                Name = s.ClientLineStatus.ClientDetails.Name,
                                                ClientLoginName = s.ClientLineStatus.ClientDetails.LoginName,
                                                Address = s.ClientLineStatus.ClientDetails.Address,
                                                ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                                                Zone = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                                                PackageName = s.TransactionM != null ? s.TransactionM.Package.PackageName : s.ClientLineStatus.ClientDetails.Package.PackageName,
                                                PackagePrice = s.TransactionM != null ? s.TransactionM.PaymentAmount.ToString() : s.ClientLineStatus.ClientDetails.Package.PackagePrice.ToString(),
                                                EmployeeName = db.Employee.Find(s.ClientLineStatus.EmployeeID).Name,
                                                LineStatusChangeDate = s.ClientLineStatus.LineStatusChangeDate.Value,
                                                IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,
                                                LineStatusActiveDate = s.ClientLineNewStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineNewStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineNewStatus.LineStatusID) : "",

                                            })

                                                                        .ToList();

            //List<int> clientDetailsID = lstLockClientOnThisMonthAndActiveOnPreviousMonth.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID).Distinct().ToList();

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



            return View(lstLockClientOnThisMonthAndActiveOnPreviousMonth);
        }
        private void LoadYearAndMonth()
        {
            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
        }

        //    [Authorize(Roles = "2")]
        public ActionResult MyProfile()
        {
            //LoadViewBag();

            var loginID = AppUtils.GetLoginUserID();
            ClientLineStatus clientLineStatus = db.ClientLineStatus
                .Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/)
                .OrderByDescending(s => s.LineStatusChangeDate)
                .FirstOrDefault();
            if (clientLineStatus != null)
            {
                Transaction ts = db.Transaction.Where(s => s.ClientDetailsID == clientLineStatus.ClientDetailsID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).FirstOrDefault();
                ViewBag.SingUpFee = ts.PaymentAmount;
            }


            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName", (clientLineStatus != null) ? clientLineStatus.ClientDetails.ZoneID : 0);
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName", (clientLineStatus != null) ? clientLineStatus.ClientDetails.ConnectionTypeID : 0);
            ViewBag.PackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName", (clientLineStatus != null) ? clientLineStatus.ClientDetails.PackageID : 0);
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName", (clientLineStatus != null) ? clientLineStatus.ClientDetails.SecurityQuestionID : 0);


            return View(clientLineStatus);
        }

        //    [Authorize(Roles = "2")]
        public ActionResult ChangeCredentials()
        {
            return View();
        }


        //     [Authorize]
        [HttpPost]
        public ActionResult ClientLoginExistOrNot(string LoginName)
        {
            ClientDetails clientDetails = db.ClientDetails.Where(s => s.LoginName == LoginName.Trim()).FirstOrDefault();

            if (clientDetails != null)
            {
                return Json(new { Exist = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Exist = false }, JsonRequestBehavior.AllowGet);
            }
        }


        //      [Authorize]
        [HttpPost]
        public ActionResult ClientLoginExistOrNotIncludeID(string LoginName, int ClientDetailsID)
        {
            ClientDetails clientDetails = db.ClientDetails.Where(s => s.LoginName == LoginName.Trim() && s.ClientDetailsID != ClientDetailsID).FirstOrDefault();

            if (clientDetails != null)
            {
                return Json(new { Exist = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Exist = false }, JsonRequestBehavior.AllowGet);
            }
        }


        ///   [Authorize]
        [HttpPost]
        public ActionResult UpdateLoginNameIfGivenAndPassword(string OldPassword, string NewPassword)
        {
            var returnMessage = "";
            var updateStatus = false;
            try
            {
                var loginID = AppUtils.GetLoginUserID();
                ClientDetails clientDetailsByLoginUserIDForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/).FirstOrDefault();

                if (!string.IsNullOrEmpty(OldPassword))
                {
                    if (clientDetailsByLoginUserIDForUpdate != null)
                    {
                        if (clientDetailsByLoginUserIDForUpdate.Password == OldPassword)
                        {
                            clientDetailsByLoginUserIDForUpdate.Password = NewPassword;
                            db.Entry(clientDetailsByLoginUserIDForUpdate).State = EntityState.Modified;
                            returnMessage += (!string.IsNullOrEmpty(returnMessage)) ? " & Password " : " Password ";
                            db.SaveChanges();
                            updateStatus = true;
                            return Json(new { UpdateStatus = updateStatus, ReturnMessage = returnMessage }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { OldPasswordNotMatched = true, UpdateStatus = updateStatus, ReturnMessage = returnMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { LogInfirst = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { UpdateStatus = updateStatus, ReturnMessage = returnMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [UserRIghtCheck(ControllerValue = AppUtils.Add_New_Client)]
        public ActionResult Create()
        {
            //var Stock = db.Stock.Where(s => s.ItemID == AppUtils.ItemIsPop).FirstOrDefault();
            //int StockIdForPop = 0;
            //if (Stock != null)
            //{
            //    StockIdForPop = Stock.StockID;
            //}

            ViewBag.CableTypePopUpID = new SelectList(db.CableType.Select(x => new { CableTypeID = x.CableTypeID, CableTypeName = x.CableTypeName }).ToList(), "CableTypeID", "CableTypeName");

            ViewBag.lstStockID = new SelectList(db.Stock.Select(x => new { StockID = x.StockID, ItemName = x.Item.ItemName }).ToList(), "StockID", "ItemName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.lstEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).Select(x => new { EmployeeID = x.EmployeeID, Name = x.Name }).ToList(), "EmployeeID", "Name");
            ViewBag.ZoneID = new SelectList(db.Zone.Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList(), "ZoneID", "ZoneName");
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.Select(x => new { ConnectionTypeID = x.ConnectionTypeID, ConnectionTypeName = x.ConnectionTypeName }).ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.Select(x => new { CableTypeID = x.CableTypeID, CableTypeName = x.CableTypeName }).ToList(), "CableTypeID", "CableTypeName");

            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageID = new SelectList(lstPackage, "PackageID", "PackageName");
            //ViewBag.PackageID = new SelectList(db.Package.Select(x => new { PackageID = x.PackageID, PackageName = x.PackageName }).ToList(), "PackageID", "PackageName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.Select(x => new { SecurityQuestionID = x.SecurityQuestionID, SecurityQuestionName = x.SecurityQuestionName }).ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            if ((int)Session["role_id"] == AppUtils.ResellerRole)
            {
                Reseller reseller = db.Reseller.Find((int)Session["LoggedUserID"]);
                List<int> lstResellerBillingCycle = string.IsNullOrEmpty(reseller.ResellerBillingCycleList) ? new List<int>() : reseller.ResellerBillingCycleList.ToString().Trim(',').Split(',').Select(Int32.Parse).ToList();
                ViewBag.BillPaymentDate = new SelectList(lstResellerBillingCycle.Select(x => new { BillPaymentDateID = x, BillPaymentDateName = x }).ToList(), "BillPaymentDateID", "BillPaymentDateName");
            }
            ClientDetails cd = new ClientDetails();
            cd.ClientNIDImageBytesPaths = "/";
            cd.ClientOwnImageBytesPaths = "/";

            return View(cd);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertClientDetails(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*, ClientDetails ClientDetails, Transaction Transaction, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign*/)
        //dnt need to add month and year cause its for  the payment for the month bill to identify in future
        {
            #region CLient Details Old
            //ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            //Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);
            //List<ClientStockAssign> ItemListForEmployee = JsonConvert.DeserializeObject<List<ClientStockAssign>>(file["ItemListForEmployee"]);
            //List<ClientCableAssign> ClientCableAssign = JsonConvert.DeserializeObject<List<ClientCableAssign>>(file["ClientCableAssign"]);
            //PaymentHistory ph = new PaymentHistory();

            //double thisMonthFee = 0;
            //ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            //if (clientLoginNameExistOrNot != null)
            //{
            //    return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            //}

            ////ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;
            //ITikConnection connection;
            //if ((bool)Session["MikrotikOptionEnable"])
            //{
            //    try
            //    {
            //        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
            //        connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
            //    }

            //    try
            //    {
            //        if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
            //        {
            //            MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
            //        }
            //        else
            //        {
            //            Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
            //            InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        var code = e.HResult;
            //        //-2146233088
            //        return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            //        throw;
            //    }
            //}

            //ClientDetails ClientDetailsSave = new ClientDetails();
            //Transaction TransactonSave = new Transaction();
            //ClientLineStatus ClientLineStatusSave = new ClientLineStatus();

            //if (ItemListForEmployee != null)
            //{
            //    List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

            //    List<string> lstStockDetails =
            //        db.StockDetails.Where(
            //            s =>
            //                lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
            //                (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
            //                s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

            //    if (lstStockDetails.Count > 0)
            //    {
            //        return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //if (ClientCableAssign != null)
            //{
            //    bool duplicateCableStockID = false;
            //    string cableBoxName = "";
            //    var lenghtGreaterThanCableAmount = false;
            //    var greaterBoxNameList = "";
            //    int cSID = 0;

            //    List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
            //    foreach (var item in cableStockID)
            //    {
            //        List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
            //        if (duplicateCableStockIDExistOrNot.Count > 1)
            //        {
            //            cSID = duplicateCableStockIDExistOrNot[0];
            //            duplicateCableStockID = true;
            //            cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
            //        }
            //    }
            //    foreach (var cable in ClientCableAssign)
            //    {
            //        CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
            //        if (cableStock != null)
            //        {
            //            if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
            //            {
            //                lenghtGreaterThanCableAmount = true;
            //                greaterBoxNameList += " " + cableStock.CableBoxName;
            //            }
            //        }
            //    }
            //    if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
            //    {
            //        return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            //try
            //{
            //    ClientDetails.CreateBy = AppUtils.GetLoginEmployeeName();
            //    ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
            //    ClientDetails.RoleID = AppUtils.ClientRole;
            //    //newreseller//
            //    if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            //    {
            //        ClientDetails.ResellerID = AppUtils.GetLoginUserID();
            //    }
            //    //newreseller//

            //    double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
            //    ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;

            //    ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
            //    ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

            //    if (AppUtils.BillIsCycleWise)
            //    {
            //        ClientDetails.RunningCycle = "1";
            //    }


            //    ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
            //    db.SaveChanges();

            //    ////// Now Saving The Image Data Into Folder And Path
            //    if (ClientNIDImage != null)
            //    {
            //        SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
            //    }
            //    if (ClientOwnImageBytes != null)
            //    {
            //        SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
            //    }
            //    db.SaveChanges();
            //    ////////////////////////////////////////////////////


            //    if (ClientDetailsSave.ClientDetailsID > 0)
            //    {
            //        Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //        //Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //        Transaction.IsNewClient = AppUtils.isNewClient;
            //        Transaction.EmployeeID = AppUtils.GetLoginUserID();

            //        //Transaction.RemarksNo = "RNEW" + RemarksNo();
            //        //Transaction.ResetNo = "RNEW" + SerialNo();


            //        Transaction.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            //        Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
            //        Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //        //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //        //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow();
            //        Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
            //        Transaction.PackageID = ClientDetails.PackageID;
            //        Transaction.BillCollectBy = AppUtils.GetLoginUserID();
            //        Transaction.PaymentFromWhichPage = "Create";
            //        //Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
            //        Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
            //        //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //        //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //        TransactonSave = db.Transaction.Add(Transaction);
            //        db.SaveChanges();
            //        //if (Transaction.PaymentAmount > 0)
            //        {
            //            ph = UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
            //        }
            //        if (Transaction.TransactionID > 0)
            //        {
            //            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            //            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);


            //            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            //            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            //            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            //            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            //            Transaction forMonthlyBill = new Transaction();
            //            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            //            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes; ;

            //            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            //            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            //            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            //            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            //            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            //            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            //            forMonthlyBill.PackageID = Transaction.PackageID;
            //            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            //            if (!AppUtils.BillIsCycleWise)
            //            {
            //                forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();
            //            }
            //            else
            //            {
            //                forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);
            //            }

            //            double packagePricePerday = 0;
            //            int DaysRemains = 0;
            //            double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            bool CountRegularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            //            // here we are taking from config that we have a day settings or not. like how much day for a month? like 30 or any other day like 28
            //            // Or We will continue with the regular month value.
            //            if (CountRegularMonthlyBase == true)
            //            {
            //                //packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
            //                packagePricePerday = (MainPackagePrice / Totaldays);
            //                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            //            }
            //            else
            //            {
            //                int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
            //                packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
            //                DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            //            }
            //            // done for package price and day remains depend on day settings.

            //            string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
            //                            : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
            //                            : "0";
            //            float tmp = 0;
            //            float.TryParse(amount, out tmp);
            //            //////forMonthlyBill.PaymentAmount = tmp;
            //            //////thisMonthFee = tmp;
            //            forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp);


            //            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            //            if (paidDirect == 1)
            //            {
            //                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
            //                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
            //                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
            //                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
            //                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
            //                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
            //                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
            //                forMonthlyBill.DueAmount = 0;
            //            }
            //            else
            //            {
            //                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //                forMonthlyBill.PaidAmount = 0;
            //                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            //            }

            //            thisMonthFee = tmp;
            //            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //            if (AppUtils.BillIsCycleWise)
            //            {
            //                forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            //            }

            //            db.Transaction.Add(forMonthlyBill);
            //            db.SaveChanges();

            //            ClientLineStatus ClientLineStatus = new ClientLineStatus();
            //            ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            //            ClientLineStatus.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            //            ClientLineStatus.PackageID = ClientDetailsSave.PackageID;
            //            ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
            //            ClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
            //            ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
            //            ClientLineStatus.StatusChangeReason = "New Connection";

            //            DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

            //            ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
            //            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
            //            db.SaveChanges();
            //        }
            //    }
            //    //******************************** Assigning Item For EMployee****************************
            //    if (ClientDetails.ClientDetailsID > 0)
            //    {
            //        if (ItemListForEmployee != null)
            //        {
            //            foreach (var item in ItemListForEmployee)
            //            {
            //                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

            //                if (stockDetails != null)
            //                {
            //                    stockDetails.SectionID = AppUtils.WorkingSection;
            //                    stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
            //                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
            //                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
            //                    db.Entry(stockDetails).State = EntityState.Modified;
            //                    db.SaveChanges();

            //                    Distribution distribution = new Distribution();
            //                    SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
            //                    db.Distribution.Add(distribution);
            //                    db.SaveChanges();
            //                }
            //            }

            //        }
            //        if (ClientCableAssign != null)
            //        {
            //            foreach (var cableUsedFromClient in ClientCableAssign)
            //            {
            //                CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

            //                if (CableStock != null)
            //                {
            //                    CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
            //                    CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
            //                    CableStock.UpdateDate = AppUtils.GetDateTimeNow();
            //                    db.Entry(CableStock).State = EntityState.Modified;
            //                    db.SaveChanges();

            //                    CableDistribution CableDistribution = new CableDistribution();
            //                    SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
            //                    CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
            //                    CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            //                    CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
            //                    db.CableDistribution.Add(CableDistribution);
            //                    db.SaveChanges();
            //                }
            //            }

            //            // List<>
            //        }
            //    }

            //    ///////Mikrotik///////////////////////////////////////////////////////////////////////////////////////////

            //    //   SendInformationIntoMikrotikIfEnable(ClientDetails);


            //    /////*****************************************************************************************************
            //    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //    if ((bool)Session["SMSOptionEnable"])
            //    {
            //        try
            //        {
            //            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
            //            if (smsSenderIdPass != null)
            //            {
            //                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //                if (sms != null)
            //                {
            //                    var message = sms.SendMessageText;
            //                    Package package = db.Package.Find(ClientDetails.PackageID);
            //                    message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
            //                    message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
            //                    message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
            //                    message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

            //                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
            //                    if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
            //                    {
            //                        sms.SMSCounter += 1;
            //                        db.Entry(sms).State = EntityState.Modified;
            //                        db.SaveChanges();
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }

            //    return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
            //    return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            //}


            //return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);

            #endregion

            #region Initialization And Client Given Information
            ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);
            List<ClientStockAssign> ItemListForEmployee = JsonConvert.DeserializeObject<List<ClientStockAssign>>(file["ItemListForEmployee"]);
            List<ClientCableAssign> ClientCableAssign = JsonConvert.DeserializeObject<List<ClientCableAssign>>(file["ClientCableAssign"]);
            PaymentHistory ph = new PaymentHistory();
            ClientDetails ClientDetailsSave = new ClientDetails();
            Transaction TransactonSave = new Transaction();
            ClientLineStatus ClientLineStatusSave = new ClientLineStatus();
            double thisMonthFee = 0;
            int ResellerID = 0;
            #endregion

            #region Check Pre Condition And Return
            ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            if (clientLoginNameExistOrNot != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }

            //ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;

            if (ItemListForEmployee != null)
            {
                List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

                List<string> lstStockDetails =
                    db.StockDetails.Where(
                        s =>
                            lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
                            (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
                            s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

                if (lstStockDetails.Count > 0)
                {
                    return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
                }
            }

            if (ClientCableAssign != null)
            {
                bool duplicateCableStockID = false;
                string cableBoxName = "";
                var lenghtGreaterThanCableAmount = false;
                var greaterBoxNameList = "";
                int cSID = 0;

                List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
                foreach (var item in cableStockID)
                {
                    List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
                    if (duplicateCableStockIDExistOrNot.Count > 1)
                    {
                        cSID = duplicateCableStockIDExistOrNot[0];
                        duplicateCableStockID = true;
                        cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
                    }
                }
                foreach (var cable in ClientCableAssign)
                {
                    CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
                    if (cableStock != null)
                    {
                        if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
                        {
                            lenghtGreaterThanCableAmount = true;
                            greaterBoxNameList += " " + cableStock.CableBoxName;
                        }
                    }
                }
                if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
                {
                    return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
                }
            }
            #endregion

            #region First Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ITikConnection connection;
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
                    {
                        MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
                    }
                    else
                    {
                        Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
                        InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
                    }

                }
                catch (Exception e)
                {
                    var code = e.HResult;
                    //-2146233088
                    return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            #endregion 

            try
            {
                #region adding client in client details table and image in image folder
                ClientDetails.CreateBy = AppUtils.GetLoginEmployeeName();
                ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
                ClientDetails.RoleID = AppUtils.ClientRole;
                ////newreseller//
                //if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                //{
                //    ClientDetails.ResellerID = AppUtils.GetLoginUserID();
                //}
                ////newreseller//

                double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
                ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;

                ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
                ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

                if (AppUtils.BillIsCycleWise)
                {
                    ClientDetails.RunningCycle = "1";
                }

                ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
                db.SaveChanges();

                ////// Now Saving The Image Data Into Folder And Path
                if (ClientNIDImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
                }
                if (ClientOwnImageBytes != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
                }
                db.SaveChanges();
                ////////////////////////////////////////////////////
                #endregion

                #region Finishing Add Or Update package in Transaction Table 
                if (!BillIsCycleWise)
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreate(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateIfBillIsCycleWise(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Update Client Details table And Client Line Status And Send SMS And Return
                //******************************** Assigning Item For EMployee****************************
                if (ClientDetails.ClientDetailsID > 0)
                {
                    if (ItemListForEmployee != null)
                    {
                        foreach (var item in ItemListForEmployee)
                        {
                            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

                            if (stockDetails != null)
                            {
                                stockDetails.SectionID = AppUtils.WorkingSection;
                                stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
                                stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                                stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                                db.Entry(stockDetails).State = EntityState.Modified;
                                db.SaveChanges();

                                Distribution distribution = new Distribution();
                                SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
                                db.Distribution.Add(distribution);
                                db.SaveChanges();
                            }
                        }

                    }
                    if (ClientCableAssign != null)
                    {
                        foreach (var cableUsedFromClient in ClientCableAssign)
                        {
                            CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

                            if (CableStock != null)
                            {
                                CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
                                CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                                CableStock.UpdateDate = AppUtils.GetDateTimeNow();
                                db.Entry(CableStock).State = EntityState.Modified;
                                db.SaveChanges();

                                CableDistribution CableDistribution = new CableDistribution();
                                SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
                                CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
                                CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
                                CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
                                db.CableDistribution.Add(CableDistribution);
                                db.SaveChanges();
                            }
                        }

                        // List<>
                    }
                }
                #endregion

                #region SMS send Regionif ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (sms != null)
                            {
                                var message = sms.SendMessageText;
                                Package package = db.Package.Find(ClientDetails.PackageID);
                                message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
                                message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
                                message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
                                message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

                                SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
                                if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    sms.SMSCounter += 1;
                                    db.Entry(sms).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                #endregion

                /////*****************************************************************************************************
                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();

                return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
                return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetRegularMonthlyBillDuringClientCreateIfBillIsCycleWise(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave)
        {
            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0; //AppUtils.ChangePackageHowMuchTimes; ;

            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);

            double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;

            forMonthlyBill.PaymentAmount = (float)MainPackagePrice - (float)Transaction.PermanentDiscount;
            forMonthlyBill.PermanentDiscount = Transaction.PermanentDiscount;


            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = MainPackagePrice - Transaction.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            if (AppUtils.BillIsCycleWise)
            {
                forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            }
        }

        private void GetRegularMonthlyBillDuringClientCreate(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0;//AppUtils.ChangePackageHowMuchTimes;
            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            //if (!AppUtils.BillIsCycleWise)
            //{
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();
            //}
            //else
            //{
            //    forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);
            //}

            double packagePricePerday = 0;
            int DaysRemains = 0;
            double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;

            // here we are taking from config that we have a day settings or not. like how much day for a month? like 30 or any other day like 28 Or We will continue with the regular month value.
            if (regularMonthlyBase == true)
            {
                //packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
                packagePricePerday = (MainPackagePrice / Totaldays);
                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            }
            else
            {
                int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
                packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
                DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            }
            // done for package price and day remains depend on day settings.

            string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
                            : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
                            : "0";
            float tmp = 0;
            float.TryParse(amount, out tmp);
            //////forMonthlyBill.PaymentAmount = tmp;
            //////thisMonthFee = tmp;
            forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp) - (float?)Transaction.PermanentDiscount;
            forMonthlyBill.PermanentDiscount = Transaction.PermanentDiscount;

            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = tmp - Transaction.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //if (AppUtils.BillIsCycleWise)
            //{
            //    forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            //}
        }

        private void SetClientLineStatusDuringClientCreate(ref ClientLineStatus ClientLineStatus, ClientDetails ClientDetailsSave, int ResellerID = 0)
        {
            ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            ClientLineStatus.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            ClientLineStatus.PackageID = ClientDetailsSave.PackageID;
            ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
            if (ResellerID > 0)
            {
                ClientLineStatus.ResellerID = ResellerID;
            }
            else
            {
                ClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
            }
            ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
            ClientLineStatus.StatusChangeReason = "New Connection";

            DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

            ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
        }

        private void GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction Transaction, ClientDetails ClientDetailsSave, ClientDetails ClientDetails, int ResellerID)
        {
            Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
            Transaction.IsNewClient = AppUtils.isNewClient;
            if (ResellerID > 0)
            {
                Transaction.ResellerID = ResellerID;

            }
            else
            {
                Transaction.EmployeeID = AppUtils.GetLoginUserID();
            }

            //Transaction.RemarksNo = "RNEW" + RemarksNo();
            //Transaction.ResetNo = "RNEW" + SerialNo();


            Transaction.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
            Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow();
            Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
            Transaction.PackageID = ClientDetails.PackageID;
            Transaction.BillCollectBy = AppUtils.GetLoginUserID();
            Transaction.PaymentFromWhichPage = "Create";
            //Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
            Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
                                                                //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
                                                                //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);
        }



        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_New_Client_By_Reseller)]
        public ActionResult ResellerClientCreate()
        {
            //if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            //{

            //}
            //if (AppUtils.GetLoginRoleID() == AppUtils.SuperUserRole || AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            //{

            //}
            int resellerID = AppUtils.GetLoginUserID();
            Reseller reseller = db.Reseller.Find(resellerID);
            List<int> lstMikrotik = string.IsNullOrEmpty(reseller.MacResellerAssignMikrotik) ? new List<int>()
                            : reseller.MacResellerAssignMikrotik.Trim(',').Split(',').Select(int.Parse).ToList();
            List<int> lstResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ?
                            new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).Select(x => x.PID).ToList()
                            : new List<int>();
            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Where(x => lstMikrotik.Contains(x.MikrotikID)).Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.PackageID = new SelectList(db.Package.Where(x => lstResellerPackage.Contains(x.PackageID)).Select(x => new { PackageID = x.PackageID, PackageName = x.PackageName }).ToList(), "PackageID", "PackageName");
            ViewBag.ZoneID = new SelectList(db.Zone.Where(x => x.ResellerID == resellerID).Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList(), "ZoneID", "ZoneName");
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.Select(x => new { ConnectionTypeID = x.ConnectionTypeID, ConnectionTypeName = x.ConnectionTypeName }).ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.Select(x => new { SecurityQuestionID = x.SecurityQuestionID, SecurityQuestionName = x.SecurityQuestionName }).ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.BoxID = new SelectList(db.Box.Where(x => x.ResellerID == resellerID).Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

            ClientDetails cd = new ClientDetails();
            cd.ClientNIDImageBytesPaths = "/";
            cd.ClientOwnImageBytesPaths = "/";

            return View(cd);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertResellerClientDetails(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*, ClientDetails ClientDetails, Transaction Transaction, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign*/)
        //dnt need to add month and year cause its for  the payment for the month bill to identify in future
        {

            #region Initialization And Client Given Information
            ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);
            //List<ClientStockAssign> ItemListForEmployee = JsonConvert.DeserializeObject<List<ClientStockAssign>>(file["ItemListForEmployee"]);
            //List<ClientCableAssign> ClientCableAssign = JsonConvert.DeserializeObject<List<ClientCableAssign>>(file["ClientCableAssign"]);
            PaymentHistory ph = new PaymentHistory();
            ClientDetails ClientDetailsSave = new ClientDetails();
            Transaction TransactonSave = new Transaction();
            ClientLineStatus ClientLineStatusSave = new ClientLineStatus();
            double thisMonthFee = 0;

            int ClientAddingByResellerOrAdmin = 1;//mean Reseller Himself
            int ResellerID = AppUtils.GetLoginUserID();
            Reseller reseller = db.Reseller.Find(ResellerID);
            List<macReselleGivenPackageWithPriceModel> lstResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ?
                             new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).ToList()
                             : new List<macReselleGivenPackageWithPriceModel>();
            double ResellerPackagePriceByAdmin = lstResellerPackage.Where(x => x.PID == ClientDetails.PackageID).FirstOrDefault().PPAdmin;
            double ResellerPackagePriceForUser = lstResellerPackage.Where(x => x.PID == ClientDetails.PackageID).FirstOrDefault().PPFromRS;


            #endregion

            #region Check Pre Condition And Return

            if ((ResellerPackagePriceForUser - ClientDetails.PermanentDiscount) < ResellerPackagePriceByAdmin)
            {
                return Json(new { Success = false, ResellerPermanentDiscountIsLessThenPackagepriceGivenByaAdmin = true }, JsonRequestBehavior.AllowGet);
            }
            if (reseller.ResellerBalance < ResellerPackagePriceByAdmin)
            {
                return Json(new { Success = false, ResellerBalanceLow = true }, JsonRequestBehavior.AllowGet);
            }

            ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            if (clientLoginNameExistOrNot != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }

            //ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;

            #region item and Cable given if need in future by reseller
            //if (ItemListForEmployee != null)
            //{
            //    List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

            //    List<string> lstStockDetails =
            //        db.StockDetails.Where(
            //            s =>
            //                lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
            //                (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
            //                s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

            //    if (lstStockDetails.Count > 0)
            //    {
            //        return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            //if (ClientCableAssign != null)
            //{
            //    bool duplicateCableStockID = false;
            //    string cableBoxName = "";
            //    var lenghtGreaterThanCableAmount = false;
            //    var greaterBoxNameList = "";
            //    int cSID = 0;

            //    List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
            //    foreach (var item in cableStockID)
            //    {
            //        List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
            //        if (duplicateCableStockIDExistOrNot.Count > 1)
            //        {
            //            cSID = duplicateCableStockIDExistOrNot[0];
            //            duplicateCableStockID = true;
            //            cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
            //        }
            //    }
            //    foreach (var cable in ClientCableAssign)
            //    {
            //        CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
            //        if (cableStock != null)
            //        {
            //            if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
            //            {
            //                lenghtGreaterThanCableAmount = true;
            //                greaterBoxNameList += " " + cableStock.CableBoxName;
            //            }
            //        }
            //    }
            //    if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
            //    {
            //        return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            #endregion
            #endregion

            #region First Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ITikConnection connection;
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
                    {
                        MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
                    }
                    else
                    {
                        Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
                        InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
                    }

                }
                catch (Exception e)
                {
                    var code = e.HResult;
                    //-2146233088
                    return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            #endregion 

            try
            {
                #region adding client in client details table and image in image folder
                ClientDetails.CreateBy = AppUtils.GetLoginUserID().ToString();
                ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
                ClientDetails.RoleID = AppUtils.ClientRole;
                //newreseller//
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    ClientDetails.ResellerID = AppUtils.GetLoginUserID();
                }
                //newreseller//

                double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
                ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;

                ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
                ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

                if (AppUtils.BillIsCycleWise)
                {
                    ClientDetails.RunningCycle = "1";
                }

                ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
                db.SaveChanges();

                ////// Now Saving The Image Data Into Folder And Path
                if (ClientNIDImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
                }
                if (ClientOwnImageBytes != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
                }
                db.SaveChanges();
                ////////////////////////////////////////////////////
                #endregion

                #region Finishing Add Or Update package in Transaction Table 
                if (!BillIsCycleWise)
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        Transaction.PermanentDiscount = 0;
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving sign up payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("SignUp:" + SerialNo(), Transaction, ClientAddingByResellerOrAdmin);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateForReseller(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave, ResellerID, ResellerPackagePriceForUser, ResellerPackagePriceByAdmin);
                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
                            if (paidDirect == 1)//saving monthly bill history in payment history
                            {
                                UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("MonthlyBill:" + SerialNo(), forMonthlyBill, ClientAddingByResellerOrAdmin);
                            }

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        Transaction.PermanentDiscount = 0;
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("SignUp:" + Transaction.ResetNo, Transaction, ClientAddingByResellerOrAdmin);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateForResellerIfBillIsCycleWise(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave, ResellerPackagePriceForUser, ResellerPackagePriceByAdmin);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
                            if (paidDirect == 1)//saving monthly bill history in payment history
                            {
                                UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller(forMonthlyBill.ResetNo, forMonthlyBill, ClientAddingByResellerOrAdmin);
                            }

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Minus Reseller Balance
                reseller.ResellerBalance -= ResellerPackagePriceByAdmin;
                db.SaveChanges();
                #endregion


                #region Update item status in stock table in future if need for reseller. if we want to give permission to give item by reseller
                //******************************** Assigning Item For EMployee****************************
                //if (ClientDetails.ClientDetailsID > 0)
                //{
                //    if (ItemListForEmployee != null)
                //    {
                //        foreach (var item in ItemListForEmployee)
                //        {
                //            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

                //            if (stockDetails != null)
                //            {
                //                stockDetails.SectionID = AppUtils.WorkingSection;
                //                stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
                //                stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                //                stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                //                db.Entry(stockDetails).State = EntityState.Modified;
                //                db.SaveChanges();

                //                Distribution distribution = new Distribution();
                //                SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
                //                db.Distribution.Add(distribution);
                //                db.SaveChanges();
                //            }
                //        }

                //    }
                //    if (ClientCableAssign != null)
                //    {
                //        foreach (var cableUsedFromClient in ClientCableAssign)
                //        {
                //            CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

                //            if (CableStock != null)
                //            {
                //                CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
                //                CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                //                CableStock.UpdateDate = AppUtils.GetDateTimeNow();
                //                db.Entry(CableStock).State = EntityState.Modified;
                //                db.SaveChanges();

                //                CableDistribution CableDistribution = new CableDistribution();
                //                SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
                //                CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
                //                CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
                //                CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
                //                db.CableDistribution.Add(CableDistribution);
                //                db.SaveChanges();
                //            }
                //        }

                //        // List<>
                //    }
                //}
                #endregion

                #region SMS send Regionif ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (sms != null)
                            {
                                var message = sms.SendMessageText;
                                Package package = db.Package.Find(ClientDetails.PackageID);
                                message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
                                message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
                                message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
                                message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

                                SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
                                if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    sms.SMSCounter += 1;
                                    db.Entry(sms).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                #endregion

                /////*****************************************************************************************************
                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();

                return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
                return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }



            #region Old ResellerCLientCreate

            //ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            //Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);

            //PaymentHistory ph = new PaymentHistory();

            //int ClientAddingByResellerOrAdmin = 1;//mean Reseller Himself
            //int resellerID = AppUtils.GetLoginUserID();
            //Reseller reseller = db.Reseller.Find(ResellerID);
            //List<macReselleGivenPackageWithPriceModel> lstResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ?
            //                 new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).ToList()
            //                 : new List<macReselleGivenPackageWithPriceModel>();
            //double ResellerPackagePrice = lstResellerPackage.Where(x => x.PID == ClientDetails.PackageID).FirstOrDefault().PPAdmin;
            //if (reseller.ResellerBalance < ResellerPackagePrice)
            //{
            //    return Json(new { Success = false, ResellerBalanceLow = true }, JsonRequestBehavior.AllowGet);
            //}

            //double thisMonthFee = 0;
            //ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            //if (clientLoginNameExistOrNot != null)
            //{
            //    return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            //}

            ////ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;
            //ITikConnection connection;
            //if ((bool)Session["MikrotikOptionEnable"])
            //{
            //    try
            //    {
            //        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
            //        connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
            //    }

            //    try
            //    {
            //        if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
            //        {
            //            MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
            //        }
            //        else
            //        {
            //            Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
            //            InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        var code = e.HResult;
            //        //-2146233088
            //        return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            //        throw;
            //    }
            //}



            //ClientDetails ClientDetailsSave = new ClientDetails();
            //Transaction TransactonSave = new Transaction();
            //ClientLineStatus ClientLineStatusSave = new ClientLineStatus();

            //try
            //{
            //    ClientDetails.CreateBy = AppUtils.GetLoginUserID().ToString();
            //    ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
            //    ClientDetails.RoleID = AppUtils.ClientRole;
            //    //newreseller//
            //    if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            //    {
            //        ClientDetails.ResellerID = AppUtils.GetLoginUserID();
            //    }
            //    //newreseller//

            //    double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
            //    ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;
            //    ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
            //    ///check///
            //    ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateTimeNow();
            //    db.SaveChanges();

            //    ////// Now Saving The Image Data Into Folder And Path
            //    if (ClientNIDImage != null)
            //    {
            //        SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
            //    }
            //    if (ClientOwnImageBytes != null)
            //    {
            //        SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
            //    }
            //    db.SaveChanges();
            //    ////////////////////////////////////////////////////


            //    if (ClientDetailsSave.ClientDetailsID > 0)
            //    {
            //        Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //        //Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //        Transaction.IsNewClient = AppUtils.isNewClient;
            //        //Transaction.EmployeeID = AppUtils.GetLoginUserID();
            //        Transaction.ResellerID = AppUtils.GetLoginUserID();

            //        //Transaction.RemarksNo = "RNEW" + RemarksNo();
            //        //Transaction.ResetNo = "RNEW" + SerialNo();


            //        Transaction.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            //        Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
            //        Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //        Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
            //        //Transaction.PackageID = ClientDetails.PackageID;
            //        Transaction.PackageID = ClientDetails.PackageID;
            //        Transaction.BillCollectBy = AppUtils.GetLoginUserID();
            //        Transaction.PaymentFromWhichPage = "Create";
            //        //Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
            //        Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date

            //        TransactonSave = db.Transaction.Add(Transaction);
            //        db.SaveChanges();
            //        //if (Transaction.PaymentAmount > 0)
            //        {
            //            ph = UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("SignUp:" + SerialNo(), Transaction, ClientAddingByResellerOrAdmin);
            //        }
            //        if (Transaction.TransactionID > 0)
            //        {
            //            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            //            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);


            //            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            //            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            //            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            //            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            //            Transaction forMonthlyBill = new Transaction();
            //            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            //            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes; ;

            //            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            //            //forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            //            forMonthlyBill.ResellerID = AppUtils.GetLoginUserID();
            //            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            //            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            //            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            //            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            //            forMonthlyBill.PackageID = Transaction.PackageID;
            //            //Transaction.PackageID = ClientDetails.PackageID;
            //            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
            //            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();

            //            //double packagePricePerday = 0;
            //            //int DaysRemains = 0;
            //            //double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            //bool CountRegularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);

            //            //if (CountRegularMonthlyBase == true)
            //            //{
            //            //    //packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
            //            //    packagePricePerday = (MainPackagePrice / Totaldays);
            //            //    DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            //            //}
            //            //else
            //            //{
            //            //    int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
            //            //    packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
            //            //    DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            //            //}


            //            //string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
            //            //                : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
            //            //                : "0";
            //            //float tmp = 0;
            //            //float.TryParse(amount, out tmp);
            //            ////////forMonthlyBill.PaymentAmount = tmp;
            //            ////////thisMonthFee = tmp;
            //            //forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp);

            //            forMonthlyBill.PaymentAmount = float.Parse(ResellerPackagePrice.ToString());
            //            //forMonthlyBill.PaidAmount = float.Parse(ResellerPackagePrice.ToString());
            //            //forMonthlyBill.DueAmount = 0;

            //            int paidDirect = 1;//int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            //            if (paidDirect == 1)
            //            {
            //                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
            //                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
            //                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
            //                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
            //                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
            //                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
            //                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
            //                forMonthlyBill.DueAmount = 0;
            //            }
            //            else
            //            {
            //                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //                forMonthlyBill.PaidAmount = 0;
            //                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            //            }

            //            //thisMonthFee = tmp;
            //            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //            db.Transaction.Add(forMonthlyBill);
            //            db.SaveChanges();

            //            reseller.ResellerBalance = reseller.ResellerBalance - ResellerPackagePrice;
            //            db.SaveChanges();

            //            ClientLineStatus ClientLineStatus = new ClientLineStatus();
            //            ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            //            ClientLineStatus.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            //            ClientLineStatus.PackageID = ClientDetailsSave.PackageID;
            //            ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
            //            ClientLineStatus.ResellerID = AppUtils.GetLoginUserID();
            //            ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
            //            ClientLineStatus.StatusChangeReason = "New Connection";

            //            DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

            //            ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
            //            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
            //            db.SaveChanges();
            //        }
            //    }

            //    ///////Mikrotik///////////////////////////////////////////////////////////////////////////////////////////

            //    //   SendInformationIntoMikrotikIfEnable(ClientDetails);


            //    /////*****************************************************************************************************
            //    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //    if ((bool)Session["SMSOptionEnable"])
            //    {
            //        try
            //        {
            //            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
            //            if (smsSenderIdPass != null)
            //            {
            //                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //                if (sms != null)
            //                {
            //                    var message = sms.SendMessageText;
            //                    Package package = db.Package.Find(ClientDetails.PackageID);
            //                    message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
            //                    message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
            //                    message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
            //                    message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

            //                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
            //                    if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
            //                    {
            //                        sms.SMSCounter += 1;
            //                        db.Entry(sms).State = EntityState.Modified;
            //                        db.SaveChanges();
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }

            //    return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
            //    return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            //}


            //return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        private void GetRegularMonthlyBillDuringClientCreateForResellerIfBillIsCycleWise(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave, double ResellerPackagePrice, double ResellerAdminGivenPrice)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0;//AppUtils.ChangePackageHowMuchTimesForReseller;
            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            //forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ResellerID = Transaction.ResellerID;//AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            //if (!AppUtils.BillIsCycleWise)
            //{
            //forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();
            //}
            //else
            //{
            forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);
            //} 

            double MainPackagePrice = ResellerPackagePrice;

            forMonthlyBill.PaymentAmount = (float)MainPackagePrice - (float)ClientDetails.PermanentDiscount;
            forMonthlyBill.PermanentDiscount = ClientDetails.PermanentDiscount;
            forMonthlyBill.ResellerPaymentAmount = (float)ResellerAdminGivenPrice;
            forMonthlyBill.PackagePriceForResellerByAdminDuringCreateOrUpdate = (float)ResellerAdminGivenPrice;
            forMonthlyBill.PackagePriceForResellerUserByResellerDuringCreateOrUpdate = (float)ResellerPackagePrice;

            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEWMonthlyBill" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = MainPackagePrice - ClientDetails.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //if (AppUtils.BillIsCycleWise)
            {
                forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            }
        }

        private void GetRegularMonthlyBillDuringClientCreateForReseller(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave, int ResellerID, double ResellerPackagePrice, double ResellerAdminGivenPrice)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDateForReseller"]);
            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDateForReseller"]);
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBaseForReseller"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDateForReseller"]) : totalDaysInThisMonth;

            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0;//AppUtils.ChangePackageHowMuchTimesForReseller;
            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            //forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ResellerID = Transaction.ResellerID;//AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            //if (!AppUtils.BillIsCycleWise)
            //{
            //forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();
            //}
            //else
            //{
            forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);
            //}

            double packagePricePerday = 0;
            int DaysRemains = 0;
            double MainPackagePrice = ResellerPackagePrice;//db.Package.Find(Transaction.PackageID).PackagePrice;

            // here we are taking from config that we have a day settings or not. like how much day for a month? like 30 or any other day like 28 Or We will continue with the regular month value.
            if (regularMonthlyBase == true)
            {
                //packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
                packagePricePerday = (MainPackagePrice / Totaldays);
                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            }
            else
            {
                int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
                packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
                DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            }
            // done for package price and day remains depend on day settings.

            string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
                            : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
                            : "0";
            float tmp = 0;
            float.TryParse(amount, out tmp);
            //////forMonthlyBill.PaymentAmount = tmp;
            //////thisMonthFee = tmp;
            forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp) - (float?)ClientDetails.PermanentDiscount;
            forMonthlyBill.ResellerPaymentAmount = (float)ResellerAdminGivenPrice;
            forMonthlyBill.PermanentDiscount = ClientDetails.PermanentDiscount;
            forMonthlyBill.PackagePriceForResellerByAdminDuringCreateOrUpdate = (float)ResellerAdminGivenPrice;
            forMonthlyBill.PackagePriceForResellerUserByResellerDuringCreateOrUpdate = (float)ResellerPackagePrice;
            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = tmp - ClientDetails.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //if (AppUtils.BillIsCycleWise)
            //{
            //    forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            //}
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_New_Client_For_Reseller_By_Admin)]
        public ActionResult ResellerClientCreateByAdmin()
        {
            //int resellerID = AppUtils.GetLoginUserID();
            //Reseller reseller = db.Reseller.Find(resellerID);
            //List<int> lstMikrotik = string.IsNullOrEmpty(reseller.MacResellerAssignMikrotik) ? new List<int>()
            //                : reseller.MacResellerAssignMikrotik.Trim(',').Split(',').Select(int.Parse).ToList();
            //List<int> lstResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ?
            //                new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).Select(x => x.PID).ToList()
            //                : new List<int>();

            string macResellerType = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            var lstReseller = db.Reseller.Where(x => x.ResellerStatus != AppUtils.TableStatusIsDelete && x.ResellerTypeListID == macResellerType).Select(x => new { x.ResellerID, x.ResellerLoginName });
            ViewBag.lstResellerID = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            ViewBag.lstMikrotik = new SelectList(new List<SelectListItem>() { }, "Value", "Text");
            ViewBag.PackageID = new SelectList(new List<SelectListItem>() { }, "Value", "Text");
            ViewBag.ZoneID = new SelectList(new List<SelectListItem>() { }, "Value", "Text");
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.Select(x => new { ConnectionTypeID = x.ConnectionTypeID, ConnectionTypeName = x.ConnectionTypeName }).ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.Select(x => new { SecurityQuestionID = x.SecurityQuestionID, SecurityQuestionName = x.SecurityQuestionName }).ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.BoxID = new SelectList(new List<SelectListItem>() { }, "Value", "Text");

            ClientDetails cd = new ClientDetails();
            cd.ClientNIDImageBytesPaths = "/";
            cd.ClientOwnImageBytesPaths = "/";

            return View(cd);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertResellerClientDetailsByAdmin(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*, ClientDetails ClientDetails, Transaction Transaction, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign*/)
        //dnt need to add month and year cause its for  the payment for the month bill to identify in future
        {
            #region Initialization And Client Given Information
            ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);
            //List<ClientStockAssign> ItemListForEmployee = JsonConvert.DeserializeObject<List<ClientStockAssign>>(file["ItemListForEmployee"]);
            //List<ClientCableAssign> ClientCableAssign = JsonConvert.DeserializeObject<List<ClientCableAssign>>(file["ClientCableAssign"]);
            PaymentHistory ph = new PaymentHistory();
            ClientDetails ClientDetailsSave = new ClientDetails();
            Transaction TransactonSave = new Transaction();
            ClientLineStatus ClientLineStatusSave = new ClientLineStatus();
            double thisMonthFee = 0;

            int ClientAddingByResellerOrAdmin = 2;//mean Admin Himself
            int ResellerID = ClientDetails.ResellerID.Value;
            Reseller reseller = db.Reseller.Find(ResellerID);
            List<macReselleGivenPackageWithPriceModel> lstResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ?
                             new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).ToList()
                             : new List<macReselleGivenPackageWithPriceModel>();
            double ResellerPackagePriceByAdmin = lstResellerPackage.Where(x => x.PID == ClientDetails.PackageID).FirstOrDefault().PPAdmin;
            double ResellerPackagePriceForUser = lstResellerPackage.Where(x => x.PID == ClientDetails.PackageID).FirstOrDefault().PPFromRS;


            #endregion

            #region Check Pre Condition And Return

            if ((ResellerPackagePriceForUser - ClientDetails.PermanentDiscount) < ResellerPackagePriceByAdmin)
            {
                return Json(new { Success = false, ResellerPermanentDiscountIsLessThenPackagepriceGivenByaAdmin = true }, JsonRequestBehavior.AllowGet);
            }
            if (reseller.ResellerBalance < ResellerPackagePriceByAdmin)
            {
                return Json(new { Success = false, ResellerBalanceLow = true }, JsonRequestBehavior.AllowGet);
            }

            ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            if (clientLoginNameExistOrNot != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }

            //ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;

            #region item and Cable given if need in future by reseller
            //if (ItemListForEmployee != null)
            //{
            //    List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

            //    List<string> lstStockDetails =
            //        db.StockDetails.Where(
            //            s =>
            //                lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
            //                (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
            //                s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

            //    if (lstStockDetails.Count > 0)
            //    {
            //        return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            //if (ClientCableAssign != null)
            //{
            //    bool duplicateCableStockID = false;
            //    string cableBoxName = "";
            //    var lenghtGreaterThanCableAmount = false;
            //    var greaterBoxNameList = "";
            //    int cSID = 0;

            //    List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
            //    foreach (var item in cableStockID)
            //    {
            //        List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
            //        if (duplicateCableStockIDExistOrNot.Count > 1)
            //        {
            //            cSID = duplicateCableStockIDExistOrNot[0];
            //            duplicateCableStockID = true;
            //            cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
            //        }
            //    }
            //    foreach (var cable in ClientCableAssign)
            //    {
            //        CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
            //        if (cableStock != null)
            //        {
            //            if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
            //            {
            //                lenghtGreaterThanCableAmount = true;
            //                greaterBoxNameList += " " + cableStock.CableBoxName;
            //            }
            //        }
            //    }
            //    if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
            //    {
            //        return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            #endregion
            #endregion

            #region First Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ITikConnection connection;
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
                    {
                        MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
                    }
                    else
                    {
                        Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
                        InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
                    }

                }
                catch (Exception e)
                {
                    var code = e.HResult;
                    //-2146233088
                    return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            #endregion 

            try
            {
                #region adding client in client details table and image in image folder
                ClientDetails.CreateBy = AppUtils.GetLoginUserID().ToString();
                ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
                ClientDetails.RoleID = AppUtils.ClientRole;
                //newreseller//
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    ClientDetails.ResellerID = AppUtils.GetLoginUserID();
                }
                //newreseller//

                double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
                ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;

                ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
                ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

                if (AppUtils.BillIsCycleWise)
                {
                    ClientDetails.RunningCycle = "1";
                }

                ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
                db.SaveChanges();

                ////// Now Saving The Image Data Into Folder And Path
                if (ClientNIDImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
                }
                if (ClientOwnImageBytes != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
                }
                db.SaveChanges();
                ////////////////////////////////////////////////////
                #endregion

                #region Finishing Add Or Update package in Transaction Table 
                if (!BillIsCycleWise)
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        Transaction.PermanentDiscount = 0;
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving sign up payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("SignUp:" + SerialNo(), Transaction, ClientAddingByResellerOrAdmin);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateForReseller(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave, ResellerID, ResellerPackagePriceForUser, ResellerPackagePriceByAdmin);
                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
                            if (paidDirect == 1)//saving monthly bill history in payment history
                            {
                                UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("MonthlyBill:" + SerialNo(), forMonthlyBill, ClientAddingByResellerOrAdmin);
                            }

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        Transaction.PermanentDiscount = 0;
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller("SignUp:" + Transaction.ResetNo, Transaction, ClientAddingByResellerOrAdmin);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateForResellerIfBillIsCycleWise(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave, ResellerPackagePriceForUser, ResellerPackagePriceByAdmin);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
                            if (paidDirect == 1)//saving monthly bill history in payment history
                            {
                                UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller(forMonthlyBill.ResetNo, forMonthlyBill, ClientAddingByResellerOrAdmin);
                            }

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Minus Reseller Balance
                reseller.ResellerBalance -= ResellerPackagePriceByAdmin;
                db.SaveChanges();
                #endregion


                #region Update item status in stock table in future if need for reseller. if we want to give permission to give item by reseller
                //******************************** Assigning Item For EMployee****************************
                //if (ClientDetails.ClientDetailsID > 0)
                //{
                //    if (ItemListForEmployee != null)
                //    {
                //        foreach (var item in ItemListForEmployee)
                //        {
                //            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

                //            if (stockDetails != null)
                //            {
                //                stockDetails.SectionID = AppUtils.WorkingSection;
                //                stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
                //                stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                //                stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                //                db.Entry(stockDetails).State = EntityState.Modified;
                //                db.SaveChanges();

                //                Distribution distribution = new Distribution();
                //                SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
                //                db.Distribution.Add(distribution);
                //                db.SaveChanges();
                //            }
                //        }

                //    }
                //    if (ClientCableAssign != null)
                //    {
                //        foreach (var cableUsedFromClient in ClientCableAssign)
                //        {
                //            CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

                //            if (CableStock != null)
                //            {
                //                CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
                //                CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                //                CableStock.UpdateDate = AppUtils.GetDateTimeNow();
                //                db.Entry(CableStock).State = EntityState.Modified;
                //                db.SaveChanges();

                //                CableDistribution CableDistribution = new CableDistribution();
                //                SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
                //                CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
                //                CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
                //                CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
                //                db.CableDistribution.Add(CableDistribution);
                //                db.SaveChanges();
                //            }
                //        }

                //        // List<>
                //    }
                //}
                #endregion

                #region SMS send Regionif ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (sms != null)
                            {
                                var message = sms.SendMessageText;
                                Package package = db.Package.Find(ClientDetails.PackageID);
                                message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
                                message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
                                message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
                                message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

                                SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
                                if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    sms.SMSCounter += 1;
                                    db.Entry(sms).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                #endregion

                /////*****************************************************************************************************
                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();

                return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
                return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        public ActionResult UpdateClientDetailsDashBoard(Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus)
        {
            int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
            if (ClientWithLoginNameCount > 0)
            {
                return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }
            var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).AsQueryable();
            var TransactionForUpdate = db.Transaction.Where(s => s.TransactionID == ClientTransaction.TransactionID).AsQueryable();
            var ClientLineStatusForUpdate = db.ClientLineStatus.Where(s => s.ClientLineStatusID == ClientClientLineStatus.ClientLineStatusID).AsQueryable();
            try
            {
                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    ClientClientDetails.RoleID = ClientDetailsForUpdate.FirstOrDefault().RoleID;
                    db.Entry(ClientDetailsForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientDetails);
                    //db.Entry(ClientClientDetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //if (Transaction.TransactionID > 0)
                //{
                //    Transaction.ClientDetailsID = ClientDetails.ClientDetailsID;
                //    Transaction.PaymentTypeID = 1;
                //    db.Entry(Transaction).State = EntityState.Modified;
                //    db.SaveChanges();
                //}
                if (ClientClientLineStatus.ClientLineStatusID > 0)
                {
                    //if (ClientClientLineStatus.LineStatusID != ClientLineStatusForUpdate.FirstOrDefault().LineStatusID)
                    //{
                    //    ClientClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
                    //}
                    //ClientClientLineStatus.ClientDetailsID = ClientClientDetails.ClientDetailsID;

                    //ClientClientLineStatus.EmployeeID = AppUtils.EmployeeID;
                    //ClientClientLineStatus.CreateDate = AppUtils.GetDateTimeNow();

                    //db.Entry(ClientLineStatusForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientLineStatus);
                    ////db.Entry(ClientClientLineStatus).State = EntityState.Modified;
                    //db.SaveChanges();
                    // if (ClientClientLineStatus.LineStatusID != ClientLineStatusForUpdate.FirstOrDefault().LineStatusID)
                    {
                        ClientClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
                    }
                    ClientClientLineStatus.ClientDetailsID = ClientClientDetails.ClientDetailsID;

                    ClientClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
                    ClientClientLineStatus.CreateDate = AppUtils.GetDateTimeNow();
                    db.ClientLineStatus.Add(ClientClientLineStatus);
                    db.SaveChanges();
                }


                var ClientDetails = ClientDetailsForUpdate.Select(s => new
                {
                    ClientDetailsID = s.ClientDetailsID,
                    Name = s.Name,
                    LoginName = s.LoginName,
                    PackageName = s.Package.PackageName,
                    Address = s.Address,
                    Email = s.Email,
                    ZoneName = s.Zone.ZoneName,
                    ContactNumber = s.ContactNumber
                });
                var ClientLineStatus = ClientLineStatusForUpdate.Select(s => new { LineStatusID = s.LineStatusID });

                //var ClientDetailsIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientDetailsForUpdate);
                //var ClientLineStatusIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientLineStatusForUpdate);

                var JSON = Json(new { LoginNameExist = false, UpdateStatus = true, ClientDetails = ClientDetails, ClientLineStatus = ClientLineStatus }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UpdateClientDetailsOnlyAllClientForMKT(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth*/)
        {
            #region Initialization And Client Given Information 

            ClientDetails ClientClientDetails = JsonConvert.DeserializeObject<ClientDetails>(file["ClientClientDetails"]);
            Transaction ClientTransaction = JsonConvert.DeserializeObject<Transaction>(file["ClientTransaction"]);
            ClientLineStatus ClientClientLineStatus = JsonConvert.DeserializeObject<ClientLineStatus>(file["ClientClientLineStatus"]);
            //bool? chkPackageFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkPackageFromRunningMonth"]);
            //bool? chkStatusFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkStatusFromRunningMonth"]);

            var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();

            bool chkPackageFromRunningMonth = ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth ? true : false;
            bool chkStatusFromRunningMonth = ClientClientDetails.StatusThisMonth != ClientDetailsForUpdate.StatusThisMonth ? true : false;
            BillGenerateHistory bgh = new BillGenerateHistory();

            double packageChangeAmountCalculation = 0; var mikrotikUserInsert = false; DateTime dateTime = AppUtils.GetDateTimeNow();
            //this is for showing bill payment checkbox. now system is working fine but the problem is when new client signup and change the package then it will show
            //the informatyion fine but check box is not showing cause bill is paid. so we have to it custom for all.
            int TransactionID = 0;

            DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
            DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            Transaction billGenerateOrNotCount;

            #endregion

            #region Check Pre Condition And Return
            //checking bill is generate or not if its not billing cycle wise
            if (!BillIsCycleWise)
            {
                bgh = db.BIllGenerateHistory.Where(s => s.Year == AppUtils.RunningYear.ToString() && s.Month == AppUtils.RunningMonth.ToString() && s.Status == AppUtils.TableStatusIsActive).FirstOrDefault();

                if (bgh == null)
                {
                    return Json(new { BillNotGenerate = true }, JsonRequestBehavior.AllowGet);
                }

            }

            //first checking old mikrotik is live or not. and if the admin given new mikrotik then checking new mikrotik is live or not also.
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    //checking old mikrotik is live or not 
                    ITikConnection connForRemoveOldUserFromMik;
                    connForRemoveOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword);
                    connForRemoveOldUserFromMik.Close();

                    if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID)
                    {//checking if the mikrotik is different then the new mikrotik is exist or not
                        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                        ITikConnection connectionForGivenByClientMKs = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
                        connectionForGivenByClientMKs.Close();
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }
            }

            //checking the login has same for others or not
            int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
            if (ClientWithLoginNameCount > 0)
            {
                return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region First Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ///first check mikrotik is active or not *******************************************************
            ITikConnection connectionForGivenMK = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                    connectionForGivenMK = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

                    //checking new mikrotik is exist or not if then continue 
                    Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
                    if (ClientDetailsForUpdate.MikrotikID == null)//create
                    {   //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first 
                        //i have to insert new information with new clientlinestatus package id 
                        //and should have to update information in accounts if this month information found. cause package are not save.

                        if (chkPackageFromRunningMonth)
                        {// creating the user in mikrotik if mikrotik is given now but mikrotik was not given before.
                            MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, ClientClientDetails.StatusThisMonth);
                        }
                    }
                    else //need to update information
                    {
                        ITikConnection connOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword); ;

                        if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID) //first remove the old information from the old mikrotik
                        {
                            #region condition 1
                            //c1
                            // first we are creating user information in new mikrotik then we wil check then we will check the first
                            //mikrotik. cause if somehow we delete the information from the first mikrotik and second mikrotik has
                            //the same name then error occourd.

                            //if (chkPackageFromRunningMonth && chkStatusFromRunningMonth)
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}
                            //else if (chkPackageFromRunningMonth)
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}
                            //else if (chkStatusFromRunningMonth == true)
                            //{
                            //    if (ClientClientDetails.StatusThisMonth == AppUtils.LineIsActive)
                            //    {
                            //        MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //    }
                            //    else
                            //    {
                            //        MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //    }
                            //}
                            //else
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}

                            #endregion

                            //create user in new mikrotik
                            MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);

                            //removing user from old mikrotik 
                            int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                            if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
                            {
                                MikrotikLB.RemoveUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            }
                            connOldUserFromMik.Close();

                            //c1 done
                        }
                        else //here we have to update information in same mikrotik depend on chk running month
                        {
                            #region User Update In Same Mikrotik
                            ////if some how some one delete information from mikrotik then we will get error during update 
                            //// for this reason first we will count the user in mikrotik. if <1 then we first add user in mikrotik then update
                            ///{//suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
                            //    //and should have to update information in accounts if this month information found. cause package are not save.


                            //if (chkPackageFromRunningMonth && chkStatusFromRunningMonth == true)
                            //{
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientLineStatus.LineStatusID);
                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            //}
                            ////else if (chkPackageFromRunningMonth == true)
                            //else if (chkPackageFromRunningMonth)
                            //{
                            //    int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientLineStatus.PackageID.Value, lineStatusID);
                            //}
                            //else if (chkStatusFromRunningMonth == true)
                            //{//logic is if checked running month but old status and given status is save no need to update informaition in Mikrotik.

                            //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                            //    if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                            //    {
                            //        MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, AppUtils.LineIsActive);
                            //    }
                            //    else
                            //    {
                            //        MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID/*int.Parse(ConfigurationManager.AppSettings["PaymentDuePackage"])*/, AppUtils.LineIsActive);
                            //    }

                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);

                            //}
                            //else
                            //{
                            //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                            //    int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, lineStatusID);
                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            //}
                            #endregion
                            MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            RemoveUserFromActiveConnectionByStatusID(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails.StatusThisMonth);
                        }
                        //connOldUserFromMik.Close();
                    }
                    connectionForGivenMK.Close();
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }
            }
            #endregion

            #region Finishing Add Or Update Client package  in Transaction Table 
            if (!BillIsCycleWise)
            {
                int countForBillIsNotGenerateForThisComingClientID = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).Count();
                //logic: bill is generate ? yes. > client present in transaction table no meaning lock client Or Delete Client.
                if (bgh != null && countForBillIsNotGenerateForThisComingClientID == 0 /*&& ThisClientIsNew == 0*/)
                {
                    Transaction ts = new Transaction();
                    GenerateBillInTransactionTableIfTheClientIsLock(ref ts, ClientClientLineStatus, ClientClientDetails);
                    db.Transaction.Add(ts);
                    db.SaveChanges();
                    packageChangeAmountCalculation = (double)ts.PaymentAmount;

                }
                else
                {
                    Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID
                    && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).FirstOrDefault();
                    if (chkPackageFromRunningMonth && chkStatusFromRunningMonth)
                    {
                        UpdateTransactionTableDuringPackageChange(transactionIDExistForThisClient, ClientClientDetails);
                        packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                    }
                    else if (chkPackageFromRunningMonth)
                    {
                        if (transactionIDExistForThisClient != null)
                        {
                            if (transactionIDExistForThisClient.PackageID != ClientClientDetails.PackageThisMonth)
                            {
                                //if (transactionIDExistOrNot.IsNewClient == AppUtils.isNewClient)//checking client is new 
                                //{
                                //    SettingForNewClient(transactionIDExistOrNot, ClientClientLineStatus);
                                //}
                                //else
                                //{//meaning cient is not new
                                UpdateTransactionTableDuringPackageChange(transactionIDExistForThisClient, ClientClientDetails);
                                //}
                                packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                            }

                        }
                    }
                    else if (chkStatusFromRunningMonth == true)
                    {
                        //Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID 
                        //&& s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).FirstOrDefault();
                        if (transactionIDExistForThisClient != null)
                        {
                            transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
                            db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    else
                    {

                    }
                }
            }
            else
            {//mean bill is cycle wise

                int countForBillIsNotGenerateForThisComingClientIDRunningCycle = db.Transaction.Where(s => s.TransactionForWhichCycle == ClientDetailsForUpdate.RunningCycle && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).Count();
                //logic: bill is generate ? yes. > client present in transaction table no meaning lock client Or Delete Client.
                if (countForBillIsNotGenerateForThisComingClientIDRunningCycle == 0 /*&& ThisClientIsNew == 0*/)
                {
                    Transaction ts = new Transaction();
                    GenerateBillInTransactionTableIfTheClientIsLockOrDeleteForCycleClient(ref ts, ClientClientLineStatus, ref ClientClientDetails, ref ClientDetailsForUpdate);
                    db.Transaction.Add(ts);
                    db.SaveChanges();
                    packageChangeAmountCalculation = (double)ts.PaymentAmount;

                }
                else
                {
                    Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.TransactionForWhichCycle == ClientDetailsForUpdate.RunningCycle && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();

                    if (chkPackageFromRunningMonth && chkStatusFromRunningMonth)
                    {
                        UpdateTransactionTableDuringPackageChangeForCycleClient(transactionIDExistForThisClient, ClientClientDetails);
                        packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                    }
                    else if (chkPackageFromRunningMonth)
                    {
                        if (transactionIDExistForThisClient != null)
                        {
                            if (transactionIDExistForThisClient.PackageID != ClientClientDetails.PackageThisMonth)
                            {
                                //if (transactionIDExistOrNot.IsNewClient == AppUtils.isNewClient)//checking client is new 
                                //{
                                //    SettingForNewClient(transactionIDExistOrNot, ClientClientLineStatus);
                                //}
                                //else
                                //{//meaning cient is not new
                                UpdateTransactionTableDuringPackageChangeForCycleClient(transactionIDExistForThisClient, ClientClientDetails);
                                //} 
                                packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                            }

                        }
                    }
                    else if (chkStatusFromRunningMonth == true)
                    {
                        if (transactionIDExistForThisClient != null)
                        {
                            transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
                            db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    else
                    {

                    }
                }
            }
            #endregion

            #region Update Client Details table And Client Line Status And Send SMS And Return
            try
            {
                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    ClientClientDetails.RoleID = ClientDetailsForUpdate.RoleID;

                    //newreseller//
                    if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                    {
                        ClientClientDetails.ResellerID = ClientDetailsForUpdate.ResellerID.HasValue ? ClientDetailsForUpdate.ResellerID : AppUtils.GetLoginUserID();
                    }
                    //newreseller//

                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "OWN", ClientOwnImageBytes, file["ClientOWNImagePath"]);
                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "NID", ClientNIDImage, file["ClientNIDImagePath"]);
                    ClientClientDetails.ProfileUpdatePercentage = GetProfileUpdateInPercentage(ClientClientDetails, ClientOwnImageBytes, ClientNIDImage);

                    //ClientClientDetails.PackageThisMonth = ClientClientDetails.PackageThisMonth;
                    //////////////////// this is for when we add package this or next month in client details//////////////////

                    //ClientClientDetails.StatusThisMonth = ClientDetailsForUpdate.StatusThisMonth;
                    //if (chkStatusFromRunningMonth)
                    //{
                    //    ClientClientDetails.StatusThisMonth = ClientClientDetails.StatusNextMonth;
                    //}
                    //else
                    //{
                    //    ClientClientDetails.StatusThisMonth = ClientDetailsForUpdate.StatusThisMonth;
                    //}
                    //////////////////////////////////////////////////

                }


                if (ClientClientLineStatus.ClientDetailsID > 0)
                {
                    SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
                }
                ClientClientDetails.ClientLineStatusID = ClientClientLineStatus.ClientLineStatusID;
                ClientClientDetails.RunningCycle = ClientDetailsForUpdate.RunningCycle;
                db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
                db.SaveChanges();

                VM_ClientDetails ClientDetails = new VM_ClientDetails();
                ClientDetails.ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID;
                ClientDetails.Name = ClientDetailsForUpdate.Name;
                ClientDetails.LoginName = ClientDetailsForUpdate.LoginName;
                //ClientDetails.PackageName = ClientDetailsForUpdate.Package.PackageName;
                ClientDetails.Address = ClientDetailsForUpdate.Address;
                ClientDetails.Email = ClientDetailsForUpdate.Email;
                ClientDetails.ZoneName = ClientDetailsForUpdate.Zone.ZoneName;
                ClientDetails.ContactNumber = ClientDetailsForUpdate.ContactNumber;
                ClientDetails.IsPriorityClient = ClientDetailsForUpdate.IsPriorityClient;
                ClientDetails.ProfileUpdatePercentage = GetProfileUpdatePercent(ClientDetailsForUpdate.ProfileUpdatePercentage, ClientDetailsForUpdate.ClientDetailsID);
                ClientDetails.PermanentDiscount = ClientDetailsForUpdate.PermanentDiscount; ;


                var ClientLineStatus = ClientClientLineStatus.LineStatusID;
                var ThisMonthPackage = db.Package.Find(ClientClientDetails.PackageThisMonth).PackageName;
                var NexrMonthPackage = db.Package.Find(ClientClientDetails.PackageNextMonth).PackageName;
                var StatusThisMonth = db.LineStatus.Find(ClientClientDetails.StatusThisMonth).LineStatusID;
                var StatusNexrMonth = db.LineStatus.Find(ClientClientDetails.StatusNextMonth).LineStatusID;

                #region Send SMS
                if ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMSReturnDetails message = SetMessage(ClientClientDetails, ClientTransaction, ClientClientLineStatus, chkPackageFromRunningMonth, chkStatusFromRunningMonth, packageChangeAmountCalculation, smsSenderIdPass);

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                #endregion




                var JSON = Json(new
                {
                    LoginNameExist = false,
                    UpdateStatus = true,
                    ClientDetails = ClientDetails,
                    ClientLineStatus = ClientLineStatus,
                    ThisMonthPackage = ThisMonthPackage,
                    NextMonthPackage = NexrMonthPackage,
                    StatusThisMonth = StatusThisMonth,
                    StatusNexrMonth = StatusNexrMonth,
                    MikrotikUserInsert = mikrotikUserInsert,
                    packageChangeAmountCalculation = packageChangeAmountCalculation,
                    chkPackageFromRunningMonth = chkPackageFromRunningMonth,
                    chkStatusFromRunningMonth = chkStatusFromRunningMonth,
                    TransactionID = TransactionID,
                    LineStatusActiveDate = ClientClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? ClientClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(ClientClientLineStatus.LineStatusID) : ""
                }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "", RemoveMikrotikInformation = true, MKUserName = ClientClientDetails.LoginName }, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }


        [HttpPost]
        public ActionResult UpdateClientDetailsOnlyAllClientForMKTByResellerOrByAdminForReseller(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth*/)
        {
            #region Initialization And Client Given Information
            int resellerID = 0;
            bool UserTypeIsReseller = true;
            Reseller reseller = new Reseller();

            int ClientAddingByResellerOrAdmin = 0;

            ClientDetails ClientClientDetails = JsonConvert.DeserializeObject<ClientDetails>(file["ClientClientDetails"]);
            Transaction ClientTransaction = JsonConvert.DeserializeObject<Transaction>(file["ClientTransaction"]);
            ClientLineStatus ClientClientLineStatus = JsonConvert.DeserializeObject<ClientLineStatus>(file["ClientClientLineStatus"]);
            //bool? chkPackageFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkPackageFromRunningMonth"]);
            //bool? chkStatusFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkStatusFromRunningMonth"]);

            var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();

            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                resellerID = AppUtils.GetLoginUserID();
                reseller = db.Reseller.Find(resellerID);
                ClientAddingByResellerOrAdmin = 1;//reseller himself
            }
            else if (ClientClientDetails.ResellerID != null && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                resellerID = ClientClientDetails.ResellerID.Value;
                reseller = db.Reseller.Find(resellerID);
                ClientAddingByResellerOrAdmin = 2;
            }
            else
            {
                resellerID = 0;
            }
            List<macReselleGivenPackageWithPriceModel> lstMacResellerPackagePrice = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>();


            bool chkPackageFromRunningMonth = ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth ? true : false;
            bool chkStatusFromRunningMonth = ClientClientDetails.StatusThisMonth != ClientDetailsForUpdate.StatusThisMonth ? true : false;
            BillGenerateHistory bgh = new BillGenerateHistory();

            double packageChangeAmountCalculation = 0; var mikrotikUserInsert = false; DateTime dateTime = AppUtils.GetDateTimeNow();
            //this is for showing bill payment checkbox. now system is working fine but the problem is when new client signup and change the package then it will show
            //the informatyion fine but check box is not showing cause bill is paid. so we have to it custom for all.
            int TransactionID = 0;

            DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
            DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            Transaction billGenerateOrNotCount;



            #endregion

            #region Check Pre Condition And Return 

            //first checking old mikrotik is live or not. and if the admin given new mikrotik then checking new mikrotik is live or not also.
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    //checking old mikrotik is live or not 
                    ITikConnection connForRemoveOldUserFromMik;
                    connForRemoveOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword);
                    connForRemoveOldUserFromMik.Close();

                    if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID)
                    {//checking if the mikrotik is different then the new mikrotik is exist or not
                        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                        ITikConnection connectionForGivenByClientMKs = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
                        connectionForGivenByClientMKs.Close();
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }
            }

            //checking the login has same for others or not
            int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
            if (ClientWithLoginNameCount > 0)
            {
                return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }
            #endregion


            #region Finishing Add Or Update Client package  in Transaction Table   
            int countForBillIsNotGenerateForThisComingClientIDRunningCycle = db.Transaction.Where(s => s.TransactionForWhichCycle == ClientDetailsForUpdate.RunningCycle && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).Count();
            //logic: bill is generate ? yes. > client present in transaction table no meaning lock client Or Delete Client.
            bool ResellerHasBalance = true;
            if (countForBillIsNotGenerateForThisComingClientIDRunningCycle == 0 /*&& ThisClientIsNew == 0*/)
            {
                Transaction ts = new Transaction();
                double resellerAmountNeedToActiveThisPackage = 0;
                GenerateBillInTransactionTableIfTheClientIsLockOrDeleteForCycleClientByResellerOrByAdminForReseller(ref ts, ClientClientLineStatus, ref ClientClientDetails, ref ClientDetailsForUpdate, ref resellerAmountNeedToActiveThisPackage, reseller, ref ResellerHasBalance);

                if (!ResellerHasBalance)
                {
                    return Json(new { Success = false, ResellerHasBalance = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
                    if (paidDirect == 1)//saving monthly bill history in payment history
                    {
                        UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller(ts.ResetNo, ts, ClientAddingByResellerOrAdmin);
                    }
                }
                db.Transaction.Add(ts);
                db.SaveChanges();
                packageChangeAmountCalculation = (double)ts.PaymentAmount;

            }
            else
            {
                Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.TransactionForWhichCycle == ClientDetailsForUpdate.RunningCycle && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();

                if (chkPackageFromRunningMonth && chkStatusFromRunningMonth)
                {
                    if (PackageChangeNumberIsAlreadyFinish(UserTypeIsReseller, transactionIDExistForThisClient))
                    {
                        return Json(new { Success = false, PackageChangeNumberIsFinish = true }, JsonRequestBehavior.AllowGet);
                    }
                    UpdateTransactionTableDuringPackageChangeForCycleClientByResellerOrByAdminForReseller(transactionIDExistForThisClient, ClientClientDetails, ClientDetailsForUpdate, reseller, lstMacResellerPackagePrice, ref ResellerHasBalance);
                    if (!ResellerHasBalance)
                    {
                        return Json(new { Success = false, ResellerHasBalance = false }, JsonRequestBehavior.AllowGet);
                    }
                    packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                }
                else if (chkPackageFromRunningMonth)
                {
                    if (transactionIDExistForThisClient != null)
                    {
                        if (transactionIDExistForThisClient.PackageID != ClientClientDetails.PackageThisMonth)
                        {
                            if (PackageChangeNumberIsAlreadyFinish(UserTypeIsReseller, transactionIDExistForThisClient))
                            {
                                return Json(new { Success = false, PackageChangeNumberIsFinish = true }, JsonRequestBehavior.AllowGet);
                            }
                            UpdateTransactionTableDuringPackageChangeForCycleClientByResellerOrByAdminForReseller(transactionIDExistForThisClient, ClientClientDetails, ClientDetailsForUpdate, reseller, lstMacResellerPackagePrice, ref ResellerHasBalance);
                            if (!ResellerHasBalance)
                            {
                                return Json(new { Success = false, ResellerHasBalance = false }, JsonRequestBehavior.AllowGet);
                            }
                            packageChangeAmountCalculation = (double)transactionIDExistForThisClient.PaymentAmount;
                        }
                    }
                }
                else if (chkStatusFromRunningMonth == true)
                {
                    if (transactionIDExistForThisClient != null)
                    {
                        //transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
                        transactionIDExistForThisClient.LineStatusID = ClientClientDetails.StatusThisMonth;
                        db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                else
                {

                }
            }
            #endregion


            #region Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ///first check mikrotik is active or not *******************************************************
            ITikConnection connectionForGivenMK = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                    connectionForGivenMK = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

                    //checking new mikrotik is exist or not if then continue 
                    //Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
                    if (ClientDetailsForUpdate.MikrotikID == null)//create
                    {   //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first 
                        //i have to insert new information with new clientlinestatus package id 
                        //and should have to update information in accounts if this month information found. cause package are not save.

                        if (chkPackageFromRunningMonth)
                        {// creating the user in mikrotik if mikrotik is given now but mikrotik was not given before.
                            MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, ClientClientDetails.StatusThisMonth);
                        }
                    }
                    else //need to update information
                    {
                        ITikConnection connOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword); ;

                        if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID) //first remove the old information from the old mikrotik
                        {
                            #region condition 1
                            //c1
                            // first we are creating user information in new mikrotik then we wil check then we will check the first
                            //mikrotik. cause if somehow we delete the information from the first mikrotik and second mikrotik has
                            //the same name then error occourd.

                            //if (chkPackageFromRunningMonth && chkStatusFromRunningMonth)
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}
                            //else if (chkPackageFromRunningMonth)
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}
                            //else if (chkStatusFromRunningMonth == true)
                            //{
                            //    if (ClientClientDetails.StatusThisMonth == AppUtils.LineIsActive)
                            //    {
                            //        MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //    }
                            //    else
                            //    {
                            //        MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //    }
                            //}
                            //else
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            //}

                            #endregion

                            //create user in new mikrotik
                            MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);

                            //removing user from old mikrotik 
                            int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                            if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
                            {
                                MikrotikLB.RemoveUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            }
                            connOldUserFromMik.Close();

                            //c1 done
                        }
                        else //here we have to update information in same mikrotik depend on chk running month
                        {
                            #region User Update In Same Mikrotik
                            ////if some how some one delete information from mikrotik then we will get error during update 
                            //// for this reason first we will count the user in mikrotik. if <1 then we first add user in mikrotik then update
                            ///{//suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
                            //    //and should have to update information in accounts if this month information found. cause package are not save.


                            //if (chkPackageFromRunningMonth && chkStatusFromRunningMonth == true)
                            //{
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientLineStatus.LineStatusID);
                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            //}
                            ////else if (chkPackageFromRunningMonth == true)
                            //else if (chkPackageFromRunningMonth)
                            //{
                            //    int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientLineStatus.PackageID.Value, lineStatusID);
                            //}
                            //else if (chkStatusFromRunningMonth == true)
                            //{//logic is if checked running month but old status and given status is save no need to update informaition in Mikrotik.

                            //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                            //    if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                            //    {
                            //        MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, AppUtils.LineIsActive);
                            //    }
                            //    else
                            //    {
                            //        MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID/*int.Parse(ConfigurationManager.AppSettings["PaymentDuePackage"])*/, AppUtils.LineIsActive);
                            //    }

                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);

                            //}
                            //else
                            //{
                            //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                            //    int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                            //    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, lineStatusID);
                            //    RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            //}
                            #endregion
                            MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusThisMonth);
                            RemoveUserFromActiveConnectionByStatusID(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails.StatusThisMonth);
                        }
                        //connOldUserFromMik.Close();
                    }
                    connectionForGivenMK.Close();
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }
            }
            #endregion

            #region Update Client Details table And Client Line Status And Send SMS And Return
            try
            {
                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    ClientClientDetails.RoleID = ClientDetailsForUpdate.RoleID;

                    //newreseller//
                    if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                    {
                        ClientClientDetails.ResellerID = AppUtils.GetLoginUserID();
                    }
                    //newreseller//

                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "OWN", ClientOwnImageBytes, file["ClientOWNImagePath"]);
                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "NID", ClientNIDImage, file["ClientNIDImagePath"]);
                    ClientClientDetails.ProfileUpdatePercentage = GetProfileUpdateInPercentage(ClientClientDetails, ClientOwnImageBytes, ClientNIDImage);

                    //ClientClientDetails.PackageThisMonth = ClientClientDetails.PackageThisMonth;
                    //////////////////// this is for when we add package this or next month in client details//////////////////

                    //ClientClientDetails.StatusThisMonth = ClientDetailsForUpdate.StatusThisMonth;
                    //if (chkStatusFromRunningMonth)
                    //{
                    //    ClientClientDetails.StatusThisMonth = ClientClientDetails.StatusNextMonth;
                    //}
                    //else
                    //{
                    //    ClientClientDetails.StatusThisMonth = ClientDetailsForUpdate.StatusThisMonth;
                    //}
                    //////////////////////////////////////////////////

                }


                if (ClientClientLineStatus.ClientLineStatusID > 0)
                {
                    SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
                }
                ClientClientDetails.ClientLineStatusID = ClientClientLineStatus.ClientLineStatusID;
                ClientClientDetails.RunningCycle = ClientDetailsForUpdate.RunningCycle;
                //only if login role is reseller
                ClientClientDetails.ApproxPaymentDate = ClientDetailsForUpdate.ApproxPaymentDate;
                //ClientClientDetails.PermanentDiscount = ClientDetailsForUpdate.PermanentDiscount;
                ClientClientDetails.PermanentDiscount = ClientClientDetails.PermanentDiscount;
                //Finish approximate date set if role is reseller
                db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
                db.SaveChanges();

                VM_ClientDetails ClientDetails = new VM_ClientDetails();
                ClientDetails.ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID;
                ClientDetails.Name = ClientDetailsForUpdate.Name;
                ClientDetails.LoginName = ClientDetailsForUpdate.LoginName;
                //ClientDetails.PackageName = ClientDetailsForUpdate.Package.PackageName;
                ClientDetails.Address = ClientDetailsForUpdate.Address;
                ClientDetails.Email = ClientDetailsForUpdate.Email;
                ClientDetails.ZoneName = ClientDetailsForUpdate.Zone.ZoneName;
                ClientDetails.ContactNumber = ClientDetailsForUpdate.ContactNumber;
                ClientDetails.IsPriorityClient = ClientDetailsForUpdate.IsPriorityClient;
                ClientDetails.ProfileUpdatePercentage = GetProfileUpdatePercent(ClientDetailsForUpdate.ProfileUpdatePercentage, ClientDetailsForUpdate.ClientDetailsID);
                ClientDetails.PermanentDiscount = ClientDetailsForUpdate.PermanentDiscount; ;


                var ClientLineStatus = ClientClientLineStatus.LineStatusID;
                var ThisMonthPackage = db.Package.Find(ClientClientDetails.PackageThisMonth).PackageName;
                var NexrMonthPackage = db.Package.Find(ClientClientDetails.PackageNextMonth).PackageName;
                var StatusThisMonth = db.LineStatus.Find(ClientClientDetails.StatusThisMonth).LineStatusID;
                var StatusNexrMonth = db.LineStatus.Find(ClientClientDetails.StatusNextMonth).LineStatusID;

                #region Send SMS
                if ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMSReturnDetails message = SetMessage(ClientClientDetails, ClientTransaction, ClientClientLineStatus, chkPackageFromRunningMonth, chkStatusFromRunningMonth, packageChangeAmountCalculation, smsSenderIdPass);

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                #endregion




                var JSON = Json(new
                {
                    LoginNameExist = false,
                    UpdateStatus = true,
                    ClientDetails = ClientDetails,
                    ClientLineStatus = ClientLineStatus,
                    ThisMonthPackage = ThisMonthPackage,
                    NextMonthPackage = NexrMonthPackage,
                    StatusThisMonth = StatusThisMonth,
                    StatusNexrMonth = StatusNexrMonth,
                    MikrotikUserInsert = mikrotikUserInsert,
                    packageChangeAmountCalculation = packageChangeAmountCalculation,
                    chkPackageFromRunningMonth = chkPackageFromRunningMonth,
                    chkStatusFromRunningMonth = chkStatusFromRunningMonth,
                    TransactionID = TransactionID,
                    LineStatusActiveDate = ClientClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? ClientClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(ClientClientLineStatus.LineStatusID) : ""
                }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "", RemoveMikrotikInformation = true, MKUserName = ClientClientDetails.LoginName }, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }

        private bool PackageChangeNumberIsAlreadyFinish(bool userTypeIsReseller, Transaction transactionIDExistForThisClient)
        {
            bool limitFinish = false; int howMuchTime = 0;
            howMuchTime = (userTypeIsReseller) ? AppUtils.ChangePackageHowMuchTimesForReseller : AppUtils.ChangePackageHowMuchTimes;
            limitFinish = howMuchTime <= transactionIDExistForThisClient.ChangePackageHowMuchTimes ? true : false;
            return limitFinish;
        }

        [HttpPost]
        public ActionResult UpdateResellerClientDetailsOnlyAllClientForMKT(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth*/)
        {//here?

            DateTime dateTime = AppUtils.GetDateTimeNow();
            ClientDetails ClientClientDetails = null; Transaction ClientTransaction = null; ClientLineStatus ClientClientLineStatus = null;
            bool? chkPackageFromRunningMonth = false; bool? chkStatusFromRunningMonth = false;
            // Initialize Information From Client File to my model.
            SetClientItemsIntoMyModel(file, ref ClientClientDetails, ref ClientTransaction, ref ClientClientLineStatus, ref chkPackageFromRunningMonth, ref chkStatusFromRunningMonth);

            //client details information for the db old information.
            var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();
            /////for reseller we will not give him to change the date of paymen date////
            ClientClientDetails.ApproxPaymentDate = ClientDetailsForUpdate.ApproxPaymentDate;
            // has transaction in this month or not?
            Transaction _hasTransactionThisMonth = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();

            // meaning reseller want to change the package with lock status and from current month its not possible so return.
            if (_hasTransactionThisMonth != null && (ClientClientDetails.StatusNextMonth == AppUtils.LineIsLock && chkStatusFromRunningMonth.Value))
            {
                return Json(new { Success = false, Message = "Sorry you cant choose different package with lock status And From current month." }, JsonRequestBehavior.AllowGet);
            }
            // meaning reseler wants to active the user cause he was lock.
            if (_hasTransactionThisMonth == null && ClientClientDetails.StatusNextMonth == AppUtils.LineIsActive)
            {
                int resellerID = AppUtils.GetLoginUserID();
                Reseller reseller = db.Reseller.Find(resellerID);

                Package clientPackageOldOrNew = db.Package.Find(ClientClientDetails.PackageThisMonth);
                if (reseller.ResellerBalance < clientPackageOldOrNew.PackagePrice)// we are check directly client side package cause client can give the different package too.
                {
                    return Json(new { Success = false, Message = "Sorry you cant choose lower package for this month." }, JsonRequestBehavior.AllowGet);
                }
            }
            if (_hasTransactionThisMonth != null && ClientClientDetails.StatusNextMonth == AppUtils.LineIsActive && ClientClientDetails.PackageThisMonth == ClientDetailsForUpdate.PackageThisMonth)// meaning the same thing no change
            {
                // no change
            }
            if (_hasTransactionThisMonth != null && ClientClientDetails.StatusNextMonth == AppUtils.LineIsActive && ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth)// meaning reseller want to change the package.
            {
                int resellerID = AppUtils.GetLoginUserID();
                Reseller reseller = db.Reseller.Find(resellerID);
                Package newPackage = db.Package.Find(ClientClientDetails.PackageID);
                double newPackagePerDay = newPackage.PackagePrice / 30;
                double oldPackagePerDay = ClientDetailsForUpdate.Package.PackagePrice / 30;
                Double oldPackageUsedDays = AppUtils.GetDateTimeNow().Subtract(new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, AppUtils.GetDateTimeNow().Second)).TotalDays;
                if (oldPackageUsedDays < 1)
                {
                    oldPackageUsedDays = 1;
                }
                double oldPackageAmountLoss = oldPackagePerDay * oldPackageUsedDays;
                double newPackageAmountNeed = newPackagePerDay * (30 - oldPackageUsedDays);
                double AmountCalcumaion = newPackageAmountNeed + oldPackageAmountLoss;
                double amountNeedMore = AmountCalcumaion - Double.Parse(_hasTransactionThisMonth.PaymentAmount.ToString());
                if (reseller.ResellerBalance < amountNeedMore)
                {
                    return Json(new { Success = false, Message = "Sorry you cant choose lower package for this month." }, JsonRequestBehavior.AllowGet);
                }
            }


            // chk reseller has balance if the package is different //
            if (ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth)
            {
                int resellerID = AppUtils.GetLoginUserID();
                Reseller reseller = db.Reseller.Find(resellerID);
                List<macReselleGivenPackageWithPriceModel> lstMacResellerPackage = !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ? (List<macReselleGivenPackageWithPriceModel>)new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>();
                double changePackageAmountRequired = GetPriceRequiredForChangeThePackage(reseller.ResellerID, ClientClientDetails, ClientDetailsForUpdate, lstMacResellerPackage);

                if (changePackageAmountRequired < reseller.ResellerBalance)
                {
                    return Json(new { Success = false, Message = "Sorry you dont have sufficient balanceto change the package." }, JsonRequestBehavior.AllowGet);
                }
            }


            ////
            //BillGenerateHistory bgh = db.BIllGenerateHistory.Where(s => s.Year == AppUtils.RunningYear.ToString() && s.Month == AppUtils.RunningMonth.ToString() && s.Status == AppUtils.TableStatusIsActive).FirstOrDefault();

            //if (bgh == null)
            //{
            //    return Json(new { BillNotGenerate = true }, JsonRequestBehavior.AllowGet);
            //}
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {

                    //ClientDetails oldClientDetailsFromDB = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID);
                    ITikConnection connForRemoveOldUserFromMik;
                    connForRemoveOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword);
                    connForRemoveOldUserFromMik.Close();

                    if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID)
                    {
                        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                        ITikConnection connectionForGivenByClientMKs = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
                        connectionForGivenByClientMKs.Close();
                    }


                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }
            }


            double packageChangeAmountCalculation = 0; var mikrotikUserInsert = false;
            //this is for showing bill payment checkbox. now system is working fine but the problem is when new client signup and change the package then it will show
            //the informatyion fine but check box is not showing cause bill is paid. so we have to it custom for all.
            int TransactionID = 0;
            //if (chkPackageFromRunningMonth == true && chkStatusFromRunningMonth == true)
            //{
            //    return Json(new { BothChecked = true }, JsonRequestBehavior.AllowGet);
            //}
            int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
            if (ClientWithLoginNameCount > 0)
            {
                return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }
            ///first check mikrotik is active or not *******************************************************
            ITikConnection connectionForGivenByClientMK = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
                    connectionForGivenByClientMK = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

                    //checking new mikrotik is exist or not if then continue
                    //ClientDetails oldClientDetailsFromDB = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID);
                    Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
                    if (ClientDetailsForUpdate.MikrotikID == null)//create
                    {   //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first 
                        //i have to insert new information with new clientlinestatus package id 
                        //and should have to update information in accounts if this month information found. cause package are not save.




                        //if (chkPackageFromRunningMonth == true && chkStatusFromRunningMonth == true)
                        //{
                        //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, ClientClientLineStatus.LineStatusID);
                        //}
                        //else 
                        //if (chkPackageFromRunningMonth == true)
                        if (ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth)
                        {
                            //MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);
                            MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, checkForExistingTransaction.LineStatusID.Value);
                        }
                        //else if (chkStatusFromRunningMonth == true)
                        //{
                        //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                        //    //MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, packageID, ClientClientLineStatus.LineStatusID);
                        //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, packageID/*int.Parse(ConfigurationManager.AppSettings["PaymentDuePackage"])*/, AppUtils.LineIsActive);
                        //}
                        //else
                        //{
                        //    int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                        //    int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                        //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, packageID, lineStatusID);
                        //}

                        ////MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK,);
                        ////Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
                        //if (checkForExistingTransaction != null)
                        //{
                        //    if (checkForExistingTransaction.PackageID != ClientClientDetails.PackageID)
                        //    {
                        //        checkForExistingTransaction.PackageID = ClientClientDetails.PackageID;
                        //        checkForExistingTransaction.PaymentAmount = db.Package.Find(ClientClientDetails.PackageID).PackagePrice;
                        //        checkForExistingTransaction.PaymentStatus = checkForExistingTransaction.PaymentStatus;
                        //        db.Entry(checkForExistingTransaction).State = EntityState.Modified;
                        //        db.SaveChanges();
                        //    }
                        //}
                    }
                    else //(oldClientDetailsFromDB.MikrotikID != null)//need to update information
                    {
                        ITikConnection connOldUserFromMik = null;
                        //connOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, oldClientDetailsFromDB.Mikrotik.RealIP, 8728, oldClientDetailsFromDB.Mikrotik.MikUserName, oldClientDetailsFromDB.Mikrotik.MikPassword);
                        //connOldUserFromMik.Close();

                        if (ClientDetailsForUpdate.MikrotikID != ClientClientDetails.MikrotikID) //first remove the old information from the old mikrotik
                        {
                            //c1
                            // first we are creating user information in new mikrotik then we wil check then we will check the first
                            //mikrotik. cause if somehow we delete the information from the first mikrotik and second mikrotik has
                            //the same name then error occourd.

                            //c2
                            // now another checkign for if user check from this month then create information in new mikrotik with the given package
                            //other wise we will create with the existing package.
                            //c22
                            //if (chkPackageFromRunningMonth == true && chkStatusFromRunningMonth == true)
                            //{
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, ClientClientLineStatus.LineStatusID);
                            //}
                            //else 
                            //if (chkPackageFromRunningMonth == true)
                            if (ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth && chkStatusFromRunningMonth == true)
                            {
                                //int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                                //MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value, lineStatusID);
                                MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientDetailsForUpdate.StatusThisMonth);
                            }
                            else if (ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth)
                            {
                                MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientDetailsForUpdate.StatusThisMonth);
                            }
                            else if (chkStatusFromRunningMonth == true)
                            {
                                if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                                {
                                    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientDetailsForUpdate.PackageThisMonth, AppUtils.LineIsActive);
                                }
                                else
                                {
                                    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientDetailsForUpdate.PackageThisMonth, AppUtils.LineIsLock);
                                }
                            }
                            else
                            {
                                MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connectionForGivenByClientMK, ClientClientDetails, ClientDetailsForUpdate.PackageThisMonth, ClientDetailsForUpdate.StatusThisMonth);
                            }
                            //c2done
                            //c1start
                            // logic is if status is same and from this month then what happen here that is they will create user in new mikrotik and delete
                            // from existing mikrotik but under chkpackagefromthis month it will return cause it will check both given and old status is same. 
                            // so below part of clientdetails will not update but new mikrotik will save the new information mea loginname, profile.
                            //thats why we keep thischecking if package check from this month and status is same then no effect on old mikrotik.


                            connOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword);
                            int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                            if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
                            {
                                MikrotikLB.RemoveUserInMikrotik(connOldUserFromMik, ClientDetailsForUpdate);
                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            }
                            connOldUserFromMik.Close();

                            //c1done
                        }
                        else //here we have to update information in same mikrotik depend on chk running month
                        {
                            ////if some how some one delete information from mikrotik then we will get error during update 
                            //// for this reason first we will count the user in mikrotik. if <1 then we first add user in mikrotik then update
                            //int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connOldUserFromMik, oldClientDetailsFromDB);
                            //if (userCountFromOldMikrotk < 1)
                            //{//suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
                            //    //and should have to update information in accounts if this month information found. cause package are not save.

                            //    //MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientLineStatus);
                            //    //MikrotikLB.CreateUserInMikrotikDuringUpdate(connOldUserFromMik, oldClientDetailsFromDB, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);
                            //    MikrotikLB.CreateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, oldClientDetailsFromDB, ClientClientLineStatus.PackageID.Value, ClientClientLineStatus.LineStatusID);
                            //}
                            ///////////////////////////////////////////
                            connOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, ClientDetailsForUpdate.Mikrotik.RealIP, 8728, ClientDetailsForUpdate.Mikrotik.MikUserName, ClientDetailsForUpdate.Mikrotik.MikPassword);

                            if ((ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth) && chkStatusFromRunningMonth == true)
                            {
                                MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientDetails.PackageThisMonth, ClientClientDetails.StatusNextMonth/*ClientClientLineStatus.LineStatusID*/);
                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            }
                            else if (ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth)//chkPackageFromRunningMonth == true)
                            {
                                int lineStatusID = ClientDetailsForUpdate.StatusThisMonth;//checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                                MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, ClientClientLineStatus.PackageID.Value, lineStatusID);
                            }
                            else if (chkStatusFromRunningMonth == true)
                            {//logic is if checked running month but old status and given status is save no need to update informaition in Mikrotik.

                                int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                                //MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, oldClientDetailsFromDB, ClientClientDetails, packageID, ClientClientLineStatus.LineStatusID);
                                if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                                {
                                    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, AppUtils.LineIsActive);
                                }
                                else
                                {
                                    MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID/*int.Parse(ConfigurationManager.AppSettings["PaymentDuePackage"])*/, AppUtils.LineIsActive);
                                }

                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);

                            }
                            else
                            {
                                //MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
                                int packageID = checkForExistingTransaction != null ? checkForExistingTransaction.PackageID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().PackageID.Value;
                                int lineStatusID = checkForExistingTransaction != null ? checkForExistingTransaction.LineStatusID.Value : db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
                                MikrotikLB.UpdateUserInMikrotikWithPackageAndStatus(connOldUserFromMik, ClientDetailsForUpdate, ClientClientDetails, packageID, lineStatusID);
                                RemoveUserFromActiveConnection(connOldUserFromMik, ClientDetailsForUpdate, ClientClientLineStatus);
                            }
                        }
                        //connOldUserFromMik.Close();
                    }
                    connectionForGivenByClientMK.Close();
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
                        JsonRequestBehavior.AllowGet);
                }

            }

            ////////////////////////////////////////////////////////////////////////////////////

            DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
            DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            Transaction billGenerateOrNotCount;

            //int countForBillIsNotGenerateForThisComingClientID = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientClientDetails.ClientDetailsID).Count();
            ////int ThisClientIsNew = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.IsNewClient == AppUtils.isNewClient).Count();
            ////logic: bill is generate ? yes. > client present in transaction table no meaning lock client.
            //if (bgh != null && countForBillIsNotGenerateForThisComingClientID == 0 /*&& ThisClientIsNew == 0*/)
            //{
            //    Transaction ts = new Transaction();
            //    GenerateBillInTransactionTableIfTheClientIsLock(ref ts, ClientClientLineStatus);
            //    db.Transaction.Add(ts);
            //    db.SaveChanges();
            //    packageChangeAmountCalculation = (double)ts.PaymentAmount;

            //}
            //else
            //{
            if (/*chkPackageFromRunningMonth == true*/ClientClientDetails.PackageThisMonth != ClientDetailsForUpdate.PackageThisMonth && chkStatusFromRunningMonth == true)
            {

            }
            else if (chkPackageFromRunningMonth == true)
            {
                //Transaction transactionIDExistOrNot = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
                if (_hasTransactionThisMonth != null)
                {
                    if (_hasTransactionThisMonth.PackageID != ClientClientLineStatus.PackageID)
                    {
                        if (_hasTransactionThisMonth.IsNewClient == AppUtils.isNewClient)//checking client is new 
                        {
                            SettingForNewClient(_hasTransactionThisMonth, ClientClientLineStatus);
                        }
                        else
                        {//meaning cient is not new
                            SettingForNewClient(_hasTransactionThisMonth, ClientClientLineStatus);
                            //SettingForOldClient(transactionIDExistOrNot, ClientClientLineStatus);
                        }

                        //TransactionID = transactionIDExistOrNot.TransactionID;
                        ////SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
                        //SaveInformationInEmployeeTransactoinLockUnlockTableIfBillInNotPaidForFractionBill(dateTime, transactionIDExistOrNot, ClientClientLineStatus, ref packageChangeAmountCalculation);
                        ////return Json(new { PackageIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            else if (chkStatusFromRunningMonth == true)
            {
                Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
                if (transactionIDExistForThisClient != null)
                {
                    //if (transactionIDExistForThisClient.LineStatusID == ClientClientLineStatus.LineStatusID)
                    //{
                    //    return Json(new { StatusIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
                    //}
                    transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
                    db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            else
            {

            }
            //}


            //var a = ClientDetailsForUpdate.FirstOrDefault();
            try
            {
                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    ClientClientDetails.RoleID = ClientDetailsForUpdate.RoleID;

                    //newreseller//
                    if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                    {
                        ClientClientDetails.ResellerID = ClientDetailsForUpdate.ResellerID.HasValue ? ClientDetailsForUpdate.ResellerID : AppUtils.GetLoginUserID();
                    }
                    //newreseller//

                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "OWN", ClientOwnImageBytes, file["ClientOWNImagePath"]);
                    AddGivenImageInCurrentRow(ref ClientDetailsForUpdate, ClientClientDetails, "NID", ClientNIDImage, file["ClientNIDImagePath"]);
                    ClientClientDetails.ProfileUpdatePercentage = GetProfileUpdateInPercentage(ClientClientDetails, ClientOwnImageBytes, ClientNIDImage);

                    ClientClientDetails.PackageThisMonth = ClientClientDetails.PackageThisMonth;
                    //////////////////// this is for when we add package this or next month in client details//////////////////
                    //if (chkPackageFromRunningMonth.Value)
                    //{
                    //    ClientClientDetails.PackageThisMonth = ClientClientDetails.PackageNextMonth;
                    //}
                    //else {
                    //ClientClientDetails.PackageThisMonth = ClientDetailsForUpdate.FirstOrDefault().PackageThisMonth;
                    //}

                    if (chkStatusFromRunningMonth.Value)
                    {
                        ClientClientDetails.StatusThisMonth = ClientClientDetails.StatusNextMonth;
                    }
                    else
                    {
                        ClientClientDetails.StatusThisMonth = ClientDetailsForUpdate.StatusThisMonth;
                    }
                    //////////////////////////////////////////////////


                    //db.Entry(ClientDetailsForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientDetails);
                    //db.SaveChanges();
                }


                if (ClientClientLineStatus.ClientLineStatusID > 0)
                {
                    SaveResellerClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
                }
                ClientClientDetails.ClientLineStatusID = ClientClientLineStatus.ClientLineStatusID;
                db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
                db.SaveChanges();

                VM_ClientDetails ClientDetails = new VM_ClientDetails();
                ClientDetails.ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID;
                ClientDetails.Name = ClientDetailsForUpdate.Name;
                ClientDetails.LoginName = ClientDetailsForUpdate.LoginName;
                ClientDetails.PackageName = ClientDetailsForUpdate.Package.PackageName;
                ClientDetails.Address = ClientDetailsForUpdate.Address;
                ClientDetails.Email = ClientDetailsForUpdate.Email;
                ClientDetails.ZoneName = ClientDetailsForUpdate.Zone.ZoneName;
                ClientDetails.ContactNumber = ClientDetailsForUpdate.ContactNumber;
                ClientDetails.IsPriorityClient = ClientDetailsForUpdate.IsPriorityClient;
                ClientDetails.ProfileUpdatePercentage = GetProfileUpdatePercent(ClientDetailsForUpdate.ProfileUpdatePercentage, ClientDetailsForUpdate.ClientDetailsID);

                var ClientLineStatus = ClientClientLineStatus.LineStatusID;
                var ClientPackage = db.Package.Find(ClientClientLineStatus.PackageID).PackageName;

                if ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMSReturnDetails message = SetMessage(ClientClientDetails, ClientTransaction, ClientClientLineStatus, chkPackageFromRunningMonth, chkStatusFromRunningMonth, packageChangeAmountCalculation, smsSenderIdPass);

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                var JSON = Json(new { LoginNameExist = false, UpdateStatus = true, ClientDetails = ClientDetails, ClientLineStatus = ClientLineStatus, ClientPackage = ClientPackage, MikrotikUserInsert = mikrotikUserInsert, packageChangeAmountCalculation = packageChangeAmountCalculation, chkPackageFromRunningMonth = chkPackageFromRunningMonth, chkStatusFromRunningMonth = chkStatusFromRunningMonth, TransactionID = TransactionID, LineStatusActiveDate = ClientClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? ClientClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(ClientClientLineStatus.LineStatusID) : "" }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "", RemoveMikrotikInformation = true, MKUserName = ClientClientDetails.LoginName }, JsonRequestBehavior.AllowGet);
            }
        }

        private double GetPriceRequiredForChangeThePackage(int resellerID, ClientDetails ClientClientDetails, ClientDetails DBClientDetails, List<macReselleGivenPackageWithPriceModel> lstMacResellerPackage)
        {

            //DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            //bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            //int totalDaysInThisMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            //int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            //// bill and used days for old package
            //int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays + 1);
            //double totalBillForOldPackageForStoringInPackageChangeHistoryTable = (transactionIDExistOrNot.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;

            ////bill for new package 
            //int remainDaysForTheNewPackage = (Totaldays - AppUtils.GetDateTimeNow().Date.Day);
            //double newPackagePricePerday = db.Package.Find(clientClientLineStatus.PackageID).PackagePrice / Totaldays;
            //double newBillForThisClient = (newPackagePricePerday * remainDaysForTheNewPackage);

            ////double newPackagePerDay = newPackage.PackagePrice / 30;
            ////double oldPackagePerDay = ClientDetailsForUpdate.Package.PackagePrice / 30;
            ////Double oldPackageUsedDays = AppUtils.GetDateTimeNow().Subtract(new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, AppUtils.GetDateTimeNow().Second)).TotalDays;
            ////if (oldPackageUsedDays < 1)
            ////{
            ////    oldPackageUsedDays = 1;
            ////}
            ////double oldPackageAmountLoss = oldPackagePerDay * oldPackageUsedDays;
            ////double newPackageAmountNeed = newPackagePerDay * (30 - oldPackageUsedDays);
            return 0;
        }

        private void SetClientItemsIntoMyModel(FormCollection file, ref ClientDetails clientClientDetails, ref Transaction clientTransaction, ref ClientLineStatus clientClientLineStatus, ref bool? chkPackageFromRunningMonth, ref bool? chkStatusFromRunningMonth)
        {
            clientClientDetails = JsonConvert.DeserializeObject<ClientDetails>(file["ClientClientDetails"]);
            clientTransaction = JsonConvert.DeserializeObject<Transaction>(file["ClientTransaction"]);
            clientClientLineStatus = JsonConvert.DeserializeObject<ClientLineStatus>(file["ClientClientLineStatus"]);
            chkPackageFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkPackageFromRunningMonth"]);
            chkStatusFromRunningMonth = JsonConvert.DeserializeObject<bool>(file["chkStatusFromRunningMonth"]);
        }

        private double GetProfileUpdateInPercentage(ClientDetails clientClientDetails, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage)
        {
            var lstOfProfilePercentCalculationRow = db.ProfilePercentageFields.AsEnumerable();
            double perPointValue = 100 / lstOfProfilePercentCalculationRow.Count();
            double totalPoint = 0;
            foreach (var item in lstOfProfilePercentCalculationRow)
            {
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsClientPhoto)
                {
                    if (ClientOwnImageBytes != null || !string.IsNullOrEmpty(clientClientDetails.ClientOwnImageBytesPaths))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalID)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.NationalID))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalIDImage)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.ClientNIDImageBytesPaths) || ClientNIDImage != null)
                    {
                        totalPoint += perPointValue;
                    }

                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsAddress)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.Address))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsMobileNo)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.ContactNumber))
                    {
                        totalPoint += perPointValue;
                    }
                }
            }
            return totalPoint;
        }

        private List<CustomProfilePercentageFields> GetProfileUpdatePointsInListDoneOrNot(int ClientDetailsID)
        {

            List<CustomProfilePercentageFields> lstCustomProfilePercentageFields = new List<CustomProfilePercentageFields>();
            ClientDetails clientDetails = db.ClientDetails.Find(ClientDetailsID);
            var lstOfProfilePercentCalculationRow = db.ProfilePercentageFields.AsEnumerable();
            foreach (var item in lstOfProfilePercentCalculationRow)
            {
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsClientPhoto)
                {
                    if (clientDetails.ClientOwnImageBytes != null && !string.IsNullOrEmpty(clientDetails.ClientOwnImageBytesPaths))
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><div class='checkbox checkbox-success'><input type='checkbox' id='chkAlreadyDataAdded' name='chkAlreadyDataAdded' checked disabled/><label for='chkAlreadyDataAdded'> </label></div></div>" });
                    }
                    else
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><button id='' type='button' class='btn btn-danger btn-sm padding03 ' data-placement='top' data-toggle='modal' > <span class='glyphicon glyphicon-remove'></span> </button></div>" });
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalID)
                {
                    if (!string.IsNullOrEmpty(clientDetails.NationalID))
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><div class='checkbox checkbox-success '><input type='checkbox' id='chkAlreadyDataAdded' name='chkAlreadyDataAdded' checked disabled/><label for='chkAlreadyDataAdded'> </label></div></div>" });
                    }
                    else
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><button id='' type='button' class='btn btn-danger btn-sm padding03 ' data-placement='top' data-toggle='modal' > <span class='glyphicon glyphicon-remove'></span> </button></div>" });
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalIDImage)
                {
                    if (!string.IsNullOrEmpty(clientDetails.ClientNIDImageBytesPaths) && clientDetails.ClientNIDImageBytes != null)
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><div class='checkbox checkbox-success '><input type='checkbox' id='chkAlreadyDataAdded' name='chkAlreadyDataAdded' checked disabled/><label for='chkAlreadyDataAdded'> </label></div></div>" });
                    }
                    else
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><button id='' type='button' class='btn btn-danger btn-sm padding03 ' data-placement='top' data-toggle='modal' > <span class='glyphicon glyphicon-remove'></span> </button></div>" });
                    }

                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsAddress)
                {
                    if (!string.IsNullOrEmpty(clientDetails.Address))
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><div class='checkbox checkbox-success '><input type='checkbox' id='chkAlreadyDataAdded' name='chkAlreadyDataAdded' checked disabled/><label for='chkAlreadyDataAdded'> </label></div></div>" });
                    }
                    else
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><button id='' type='button' class='btn btn-danger btn-sm padding03 ' data-placement='top' data-toggle='modal' > <span class='glyphicon glyphicon-remove'></span> </button></div>" });
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsMobileNo)
                {
                    if (!string.IsNullOrEmpty(clientDetails.ContactNumber))
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><div class='checkbox checkbox-success '><input type='checkbox' id='chkAlreadyDataAdded' name='chkAlreadyDataAdded' checked disabled/><label for='chkAlreadyDataAdded'> </label></div></div>" });
                    }
                    else
                    {
                        lstCustomProfilePercentageFields.Add(new CustomProfilePercentageFields { FieldsName = item.FieldsName, CheckBoxDoneOrNot = "<div class='marginleft20px'><button id='' type='button' class='btn btn-danger btn-sm padding03 ' data-placement='top' data-toggle='modal' > <span class='glyphicon glyphicon-remove'></span> </button></div>" });
                    }
                }
            }
            return lstCustomProfilePercentageFields;
        }

        private string GetProfileUpdatePercent(double profileUpdatePercentage, int ClientDetailsID)
        {
            if (profileUpdatePercentage <= 30)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-danger'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else if (profileUpdatePercentage >= 30 && profileUpdatePercentage <= 75)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-warning'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else if (profileUpdatePercentage >= 75 && profileUpdatePercentage < 100)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-success'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-primary'> " + profileUpdatePercentage + " %</span>   </a>";
            }
        }

        private void AddGivenImageInCurrentRow(ref ClientDetails ClientDetailsForUpdate, ClientDetails ClientClientDetails, string type, HttpPostedFileBase ClientImageBytes, string imagePath)
        {
            if (type == "OWN")
            {
                if (ClientImageBytes != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientClientDetails, ClientDetailsForUpdate, "OWN", ClientImageBytes);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    ClientClientDetails.ClientOwnImageBytes = ClientDetailsForUpdate.ClientOwnImageBytes;
                    ClientClientDetails.ClientOwnImageBytesPaths = ClientDetailsForUpdate.ClientOwnImageBytesPaths;
                }
                else
                {
                    RemoveImageFromServerFolder(type, ClientDetailsForUpdate);
                    ClientClientDetails.ClientOwnImageBytes = null;
                    ClientClientDetails.ClientOwnImageBytesPaths = null;
                }
            }
            if (type == "NID")
            {
                if (ClientImageBytes != null && imagePath != null)
                {
                    //SaveImageInFolderAndAddInformationInDVDSTable(ref ClientClientDetails, "NID", ClientImageBytes);
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientClientDetails, ClientDetailsForUpdate, "NID", ClientImageBytes);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    ClientClientDetails.ClientNIDImageBytes = ClientDetailsForUpdate.ClientNIDImageBytes;
                    ClientClientDetails.ClientNIDImageBytesPaths = ClientDetailsForUpdate.ClientNIDImageBytesPaths;
                }
                else
                {
                    RemoveImageFromServerFolder(type, ClientDetailsForUpdate);
                    ClientClientDetails.ClientNIDImageBytes = null;
                    ClientClientDetails.ClientNIDImageBytes = null;
                }
            }

        }

        //private void AddGivenImageInCurrentRow(ref IQueryable<ClientDetails> ClientDetailsForUpdate, ClientDetails ClientClientDetails, string type, HttpPostedFileBase ClientImageBytes, string imagePath)
        //{
        //    if (type == "OWN")
        //    {
        //        if (ClientImageBytes != null && imagePath != null)
        //        {
        //            RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientClientDetails, ClientDetailsForUpdate, "OWN", ClientImageBytes);
        //        }
        //        else if (!string.IsNullOrEmpty(imagePath))
        //        {
        //            ClientClientDetails.ClientOwnImageBytes = ClientDetailsForUpdate.FirstOrDefault().ClientOwnImageBytes;
        //            ClientClientDetails.ClientOwnImageBytesPaths = ClientDetailsForUpdate.FirstOrDefault().ClientOwnImageBytesPaths;
        //        }
        //        else
        //        {
        //            RemoveImageFromServerFolder(type, ClientDetailsForUpdate.FirstOrDefault());
        //            ClientClientDetails.ClientOwnImageBytes = null;
        //            ClientClientDetails.ClientOwnImageBytesPaths = null;
        //        }
        //    }
        //    if (type == "NID")
        //    {
        //        if (ClientImageBytes != null && imagePath != null)
        //        {
        //            //SaveImageInFolderAndAddInformationInDVDSTable(ref ClientClientDetails, "NID", ClientImageBytes);
        //            RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientClientDetails, ClientDetailsForUpdate, "NID", ClientImageBytes);
        //        }
        //        else if (!string.IsNullOrEmpty(imagePath))
        //        {
        //            ClientClientDetails.ClientNIDImageBytes = ClientDetailsForUpdate.FirstOrDefault().ClientNIDImageBytes;
        //            ClientClientDetails.ClientNIDImageBytesPaths = ClientDetailsForUpdate.FirstOrDefault().ClientNIDImageBytesPaths;
        //        }
        //        else
        //        {
        //            RemoveImageFromServerFolder(type, ClientDetailsForUpdate.FirstOrDefault());
        //            ClientClientDetails.ClientNIDImageBytes = null;
        //            ClientClientDetails.ClientNIDImageBytes = null;
        //        }
        //    }

        //}


        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientDetails ClientClientDetails, ClientDetails clientDetailsForUpdate, string WhichPic, HttpPostedFileBase clientImageBytes)
        {


            RemoveImageFromServerFolder(WhichPic, clientDetailsForUpdate);



            if (!IsValidContentType(clientImageBytes.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            else if (!IsValidContentLength(clientImageBytes.ContentLength))
            {
                ViewBag.ErrorFileTooLarge = "Your file is too large.";
            }

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clientImageBytes.FileName);
            string extension = Path.GetExtension(clientImageBytes.FileName);
            var fileName = ClientClientDetails.ClientDetailsID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ClientsImage"), fileName);
            clientImageBytes.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(clientImageBytes.InputStream);
            imagebyte = reader.ReadBytes(clientImageBytes.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "NID")
            {
                //clientDetails.image = fileName;
                ClientClientDetails.ClientNIDImageBytes = imagebyte;
                ClientClientDetails.ClientNIDImageBytesPaths = "/Images/ClientsImage/" + fileName;

            }
            else if (WhichPic == "OWN")
            {
                //clientDetails.image = fileName;
                ClientClientDetails.ClientOwnImageBytes = imagebyte;
                ClientClientDetails.ClientOwnImageBytesPaths = "/Images/ClientsImage/" + fileName;
            }
        }


        //private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ClientDetails ClientClientDetails, IQueryable<ClientDetails> clientDetailsForUpdate, string WhichPic, HttpPostedFileBase clientImageBytes)
        //{


        //    RemoveImageFromServerFolder(WhichPic, clientDetailsForUpdate.FirstOrDefault());



        //    if (!IsValidContentType(clientImageBytes.ContentType))
        //    {
        //        ViewBag.Error = "Only PNG image are allowed";
        //    }
        //    else if (!IsValidContentLength(clientImageBytes.ContentLength))
        //    {
        //        ViewBag.ErrorFileTooLarge = "Your file is too large.";
        //    }

        //    byte[] imagebyte = null;

        //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clientImageBytes.FileName);
        //    string extension = Path.GetExtension(clientImageBytes.FileName);
        //    var fileName = ClientClientDetails.ClientDetailsID + "_" + WhichPic + "" + extension;

        //    string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ClientsImage"), fileName);
        //    clientImageBytes.SaveAs(fileSaveInFolder);


        //    BinaryReader reader = new BinaryReader(clientImageBytes.InputStream);
        //    imagebyte = reader.ReadBytes(clientImageBytes.ContentLength);

        //    Image returnImage = byteArrayToImage(imagebyte);
        //    Bitmap bp = ResizeImage(returnImage, 200, 200);
        //    imagebyte = imageToByteArray(bp);

        //    if (WhichPic == "NID")
        //    {
        //        //clientDetails.image = fileName;
        //        ClientClientDetails.ClientNIDImageBytes = imagebyte;
        //        ClientClientDetails.ClientNIDImageBytesPaths = "/Images/ClientsImage/" + fileName;

        //    }
        //    else if (WhichPic == "OWN")
        //    {
        //        //clientDetails.image = fileName;
        //        ClientClientDetails.ClientOwnImageBytes = imagebyte;
        //        ClientClientDetails.ClientOwnImageBytesPaths = "/Images/ClientsImage/" + fileName;
        //    }
        //}

        private void RemoveImageFromServerFolder(string WhichPic, ClientDetails clientDetails)
        {
            string removeImageName = "";
            if (WhichPic == "NID")
            {
                removeImageName = clientDetails.ClientNIDImageBytesPaths != null ? clientDetails.ClientNIDImageBytesPaths.Split('/')[3] : "";

            }
            else if (WhichPic == "OWN")
            {
                removeImageName = clientDetails.ClientOwnImageBytesPaths != null ? clientDetails.ClientOwnImageBytesPaths.Split('/')[3] : "";
            }

            var filePath = Server.MapPath("~/Images/ClientsImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void SettingForOldClient(Transaction transactionIDExistOrNot, ClientLineStatus clientClientLineStatus)
        {
            DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            // bill and used days for old package
            int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays + 1);
            double totalBillForOldPackageForStoringInPackageChangeHistoryTable = (transactionIDExistOrNot.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;

            //bill for new package 
            int remainDaysForTheNewPackage = (Totaldays - AppUtils.GetDateTimeNow().Date.Day);
            double newPackagePricePerday = db.Package.Find(clientClientLineStatus.PackageID).PackagePrice / Totaldays;
            double newBillForThisClient = (newPackagePricePerday * remainDaysForTheNewPackage);

            //////1  keep history for package change in  lockUnlock table
            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = transactionIDExistOrNot.TransactionID,
                Amount = totalBillForOldPackageForStoringInPackageChangeHistoryTable,
                PackageID = transactionIDExistOrNot.PackageID,
                FromDate = transactionIDExistOrNot.AmountCountDate.Value,
                ToDate = dateTimeNow,
                LockOrUnlockDate = dateTimeNow,
                EmployeeID = AppUtils.LoginUserID
            };
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();
            ////1 done

            //this bill is the total old bill for this month and for this client  id
            double OldBillForThisClient = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.Amount) : 0;


            //2 now modifing informaiton in transaction table
            transactionIDExistOrNot.PackageID = clientClientLineStatus.PackageID;
            transactionIDExistOrNot.PaymentAmount = (float)(OldBillForThisClient + newBillForThisClient);

            //3 if Package is lower and Already Paid bill is greate than new bill we have to add this bill into advance payment table.
            var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (transactionIDExistOrNot.PaymentAmount > alreadyDBPaidAmount)
            {
                transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - alreadyDBPaidAmount;
            }
            else
            {
                transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;

                float advanceAmount = alreadyDBPaidAmount - transactionIDExistOrNot.PaymentAmount.Value;
                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == transactionIDExistOrNot.ClientDetailsID).FirstOrDefault();

                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += advanceAmount;
                    advancePayment.Remarks = "Payment Remarks";
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null? "rst"+SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, advancePayment, advanceAmount);
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = transactionIDExistOrNot.ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = advanceAmount;
                    insertAdvancePayment.Remarks = "Payment Remarks";
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null ? "rst" + SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, insertAdvancePayment, advanceAmount);
                }
            }
            ///3 Advance Payment done

            transactionIDExistOrNot.AmountCountDate = dateTimeNow;
            transactionIDExistOrNot.ChangePackageHowMuchTimes += 1;

            db.Entry(transactionIDExistOrNot).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void SettingForNewClient(Transaction transactionIDExistOrNot, ClientLineStatus clientClientLineStatus)
        {
            #region initialization
            DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            // bill and used days for old package
            int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays /*+ 1*/);
            double totalBillForOldPackageForStoringInPackageChangeHistoryTable = (transactionIDExistOrNot.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;

            //var ifLockUnlockHistoryTableContainsOldBill = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).ToList();
            //if (ifLockUnlockHistoryTableContainsOldBill.Count() > 0)
            //{
            //    totalBillForOldPackageForStoringInPackageChangeHistoryTable += ifLockUnlockHistoryTableContainsOldBill.Sum(s => s.Amount); 
            //}

            //this bill is the total old bill for this month and for this client  id. Cause if they count small amount then this need.
            double OldBillForThisClient = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.Amount) : 0;
            #endregion 

            #region Client Transaction History Insertion in Transaction Lock Unlock Table
            //bill for new package 
            int remainDaysForTheNewPackage = (Totaldays - AppUtils.GetDateTimeNow().Date.Day);
            double newPackagePricePerday = db.Package.Find(clientClientLineStatus.PackageID).PackagePrice / Totaldays;
            double newBillForThisClient = (newPackagePricePerday * remainDaysForTheNewPackage);
            //var a = Math.Round(newBillForThisClient);
            //////1  keep history for package change in  lockUnlock table
            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = transactionIDExistOrNot.TransactionID,
                Amount = totalBillForOldPackageForStoringInPackageChangeHistoryTable,
                PackageID = transactionIDExistOrNot.PackageID,
                FromDate = transactionIDExistOrNot.AmountCountDate.Value,
                ToDate = dateTimeNow,
                LockOrUnlockDate = dateTimeNow,
                EmployeeID = AppUtils.LoginUserID
            };
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();
            ////1 done
            #endregion


            //2 now modifing informaiton in transaction table
            transactionIDExistOrNot.PackageID = clientClientLineStatus.PackageID;
            transactionIDExistOrNot.PaymentAmount = (float)Math.Round((OldBillForThisClient + newBillForThisClient));

            //3 if Package is lower and Already Paid bill is greate than new bill we have to add this bill into advance payment table.
            var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (transactionIDExistOrNot.PaymentAmount > alreadyDBPaidAmount)
            {
                transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - alreadyDBPaidAmount;
            }
            else
            {
                transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                float advanceAmount = alreadyDBPaidAmount - transactionIDExistOrNot.PaymentAmount.Value;
                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == transactionIDExistOrNot.ClientDetailsID).FirstOrDefault();

                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += advanceAmount;
                    advancePayment.Remarks = "Payment Remarks";
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null? "rst"+SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, advancePayment, advanceAmount);
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = transactionIDExistOrNot.ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = advanceAmount;
                    insertAdvancePayment.Remarks = "Payment Remarks";
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null ? "rst" + SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, insertAdvancePayment, advanceAmount);
                }
            }
            ///3 Advance Payment done

            transactionIDExistOrNot.AmountCountDate = dateTimeNow;
            transactionIDExistOrNot.ChangePackageHowMuchTimes += 1;

            db.Entry(transactionIDExistOrNot).State = EntityState.Modified;
            db.SaveChanges();
        }


        private void UpdateTransactionTableDuringPackageChange(Transaction transactionIDExistOrNot, ClientDetails clientClientDetails)
        {
            #region initialization
            DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            // bill and used days for old package
            int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays /*+ 1*/);
            double totalBillForOldPackageForStoringInPackageChangeHistoryTable = (transactionIDExistOrNot.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;

            //var ifLockUnlockHistoryTableContainsOldBill = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).ToList();
            //if (ifLockUnlockHistoryTableContainsOldBill.Count() > 0)
            //{
            //    totalBillForOldPackageForStoringInPackageChangeHistoryTable += ifLockUnlockHistoryTableContainsOldBill.Sum(s => s.Amount); 
            //}
            //////1  keep history for package change in  lockUnlock table
            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = transactionIDExistOrNot.TransactionID,
                Amount = totalBillForOldPackageForStoringInPackageChangeHistoryTable,
                PackageID = transactionIDExistOrNot.PackageID,
                FromDate = transactionIDExistOrNot.AmountCountDate.Value,
                ToDate = dateTimeNow,
                LockOrUnlockDate = dateTimeNow,
                EmployeeID = AppUtils.LoginUserID
            };
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();
            ////1 done
            //this bill is the total old bill for this month and for this client  id. Cause if they count small amount then this need.
            double OldBillForThisClient = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.Amount) : 0;
            #endregion 

            #region Client Transaction History Insertion in Transaction Lock Unlock Table
            //bill for new package 
            int remainDaysForTheNewPackage = (Totaldays - AppUtils.GetDateTimeNow().Date.Day);
            double newPackagePricePerday = db.Package.Find(clientClientDetails.PackageThisMonth).PackagePrice / Totaldays;
            double newBillForThisClient = (newPackagePricePerday * remainDaysForTheNewPackage);
            //var a = Math.Round(newBillForThisClient);

            #endregion

            #region Transaction Update in the transaction table including advance payment if need
            //2 now modifing informaiton in transaction table
            transactionIDExistOrNot.PackageID = clientClientDetails.PackageThisMonth;
            transactionIDExistOrNot.PaymentAmount = (float)Math.Round((OldBillForThisClient + newBillForThisClient));

            //3 if Package is lower and Already Paid bill is greate than new bill we have to add this bill into advance payment table.
            var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (transactionIDExistOrNot.PaymentAmount > alreadyDBPaidAmount)
            {
                transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - alreadyDBPaidAmount;
            }
            else
            {
                transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                float advanceAmount = alreadyDBPaidAmount - transactionIDExistOrNot.PaymentAmount.Value;
                #region Adding Or Update In Advance Payment Table
                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == transactionIDExistOrNot.ClientDetailsID).FirstOrDefault();
                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += advanceAmount;
                    advancePayment.Remarks = "Payment Remarks";
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null? "rst"+SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, advancePayment, advanceAmount);
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = transactionIDExistOrNot.ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = advanceAmount;
                    insertAdvancePayment.Remarks = "Payment Remarks";
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null ? "rst" + SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, insertAdvancePayment, advanceAmount);
                }
                #endregion
            }
            ///3 Advance Payment done

            transactionIDExistOrNot.AmountCountDate = dateTimeNow;
            transactionIDExistOrNot.LineStatusID = clientClientDetails.StatusThisMonth;
            transactionIDExistOrNot.ChangePackageHowMuchTimes += 1;

            db.Entry(transactionIDExistOrNot).State = EntityState.Modified;
            db.SaveChanges();
            #endregion 
        }
        private void UpdateTransactionTableDuringPackageChangeForCycleClient(Transaction transactionIDExistOrNot, ClientDetails clientClientDetails)
        {
            #region initialization


            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            DateTime CycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, clientClientDetails.ApproxPaymentDate);
            DateTime NExtCycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, clientClientDetails.ApproxPaymentDate).AddMonths(1);


            DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int Totaldays = (int)(NExtCycleDate - CycleDate).TotalDays;

            // bill and used days for old package
            int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays /*+ 1*/) /*- 1*/;
            double totalBillForOldPackageForStoringInPackageChangeHistoryTable = (transactionIDExistOrNot.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;
            //var a = Math.Round(newBillForThisClient);
            //////1  keep history for package change in  lockUnlock table
            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = transactionIDExistOrNot.TransactionID,
                Amount = totalBillForOldPackageForStoringInPackageChangeHistoryTable,
                PackageID = transactionIDExistOrNot.PackageID,
                FromDate = transactionIDExistOrNot.AmountCountDate.Value,
                ToDate = dateTimeNow,
                LockOrUnlockDate = dateTimeNow,
                EmployeeID = AppUtils.LoginUserID
            };
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();
            ////1 done
            //this bill is the total old bill for this month and for this client  id. Cause if they count small amount then this need.
            double OldBillForThisClient = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.Amount) : 0;
            #endregion 

            #region Client Transaction History Insertion in Transaction Lock Unlock Table
            //bill for new package 
            int passedDays = Math.Abs((CycleDate - AppUtils.GetDateTimeNow().Date).Days);
            int remainDaysForTheNewPackage = (Totaldays - passedDays) - 1;
            double newPackagePricePerday = db.Package.Find(clientClientDetails.PackageThisMonth).PackagePrice / Totaldays;
            double newBillForThisClient = (newPackagePricePerday * remainDaysForTheNewPackage);

            #endregion

            #region Transaction Update in the transaction table including advance payment if need
            //2 now modifing informaiton in transaction table
            transactionIDExistOrNot.PackageID = clientClientDetails.PackageThisMonth;
            transactionIDExistOrNot.PaymentAmount = (float)Math.Round((OldBillForThisClient + newBillForThisClient));

            //3 if Package is lower and Already Paid bill is greate than new bill we have to add this bill into advance payment table.
            var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (transactionIDExistOrNot.PaymentAmount > alreadyDBPaidAmount)
            {
                transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - alreadyDBPaidAmount;
            }
            else
            {
                transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                float advanceAmount = alreadyDBPaidAmount - transactionIDExistOrNot.PaymentAmount.Value;
                #region Adding Or Update In Advance Payment Table
                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == transactionIDExistOrNot.ClientDetailsID).FirstOrDefault();
                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += advanceAmount;
                    advancePayment.Remarks = "Payment Remarks";
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null? "rst"+SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, advancePayment, advanceAmount);
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = transactionIDExistOrNot.ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = advanceAmount;
                    insertAdvancePayment.Remarks = "Payment Remarks";
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null ? "rst" + SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, insertAdvancePayment, advanceAmount);
                }
                #endregion
            }
            ///3 Advance Payment done

            transactionIDExistOrNot.AmountCountDate = dateTimeNow;
            transactionIDExistOrNot.LineStatusID = clientClientDetails.StatusThisMonth;
            transactionIDExistOrNot.ChangePackageHowMuchTimes += 1;

            db.Entry(transactionIDExistOrNot).State = EntityState.Modified;
            db.SaveChanges();
            #endregion 
        }

        private void UpdateTransactionTableDuringPackageChangeForCycleClientByResellerOrByAdminForReseller(Transaction transactionIDExistOrNot, ClientDetails clientClientDetails, ClientDetails clientDetailsForUpdate, Reseller reseller, List<macReselleGivenPackageWithPriceModel> lstMacResellerPackagePrice, ref bool ResellerHasBalance)
        {
            #region initialization


            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            //for reseller we are setting approx payment from db but when admin then it should be from client coming client
            DateTime CycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, clientDetailsForUpdate.ApproxPaymentDate);
            DateTime NExtCycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, clientDetailsForUpdate.ApproxPaymentDate).AddMonths(1);
            //// end reseller setting for approx payment

            DateTime dateTimeNow = AppUtils.GetDateTimeNow();
            int Totaldays = (int)(NExtCycleDate - CycleDate).TotalDays;

            // bill and used days for old package
            double OldResellerPackagePriceByAdmin = lstMacResellerPackagePrice.Where(x => x.PID == transactionIDExistOrNot.PackageID).FirstOrDefault().PPAdmin;
            double OldResellerPackagePriceByReseller = lstMacResellerPackagePrice.Where(x => x.PID == transactionIDExistOrNot.PackageID).FirstOrDefault().PPFromRS;

            int totalUsedDaysWithThisPackage = Convert.ToInt32((AppUtils.GetDateTimeNow() - transactionIDExistOrNot.AmountCountDate.Value).TotalDays /*+ 1*/) /*- 1*/;
            double totalBillForOldPackageForStoringInPackageChangeHistoryTableForReseller = (float)(transactionIDExistOrNot.PackagePriceForResellerByAdminDuringCreateOrUpdate / Totaldays) * totalUsedDaysWithThisPackage;
            double totalBillForOldPackageForStoringInPackageChangeHistoryTableForUser = (float)(transactionIDExistOrNot.PackagePriceForResellerUserByResellerDuringCreateOrUpdate / Totaldays) * totalUsedDaysWithThisPackage;
            double resellerOldAlreadyPaymentAmount = 0;
            //var a = Math.Round(newBillForThisClient);
            //////1  keep history for package change in  lockUnlock table
            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = transactionIDExistOrNot.TransactionID,
                Amount = totalBillForOldPackageForStoringInPackageChangeHistoryTableForUser,
                AmountForReseller = totalBillForOldPackageForStoringInPackageChangeHistoryTableForReseller,
                PackageID = transactionIDExistOrNot.PackageID,
                FromDate = transactionIDExistOrNot.AmountCountDate.Value,
                ToDate = dateTimeNow,
                LockOrUnlockDate = dateTimeNow,
                ResellerID = reseller.ResellerID
            };
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();
            ////1 done
            //this bill is the total old bill for this month and for this client  id. Cause if they count small amount then this need.
            double OldBillForThisClientForReseller = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.AmountForReseller) : 0;
            double OldBillForThisClientForUser = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == transactionIDExistOrNot.TransactionID).Sum(s => s.Amount) : 0;
            #endregion 

            #region Client Transaction History Insertion in Transaction Lock Unlock Table
            //bill for new package 
            int passedDays = Math.Abs((CycleDate - AppUtils.GetDateTimeNow().Date).Days);
            //int remainDaysForTheNewPackage = (Totaldays - passedDays) - 1;
            int remainDaysForTheNewPackage = (Totaldays - passedDays);
            double newPackagePriceForReseller = lstMacResellerPackagePrice.Where(z => z.PID == clientClientDetails.PackageThisMonth).FirstOrDefault().PPAdmin;
            double newPackagePricePerdayForReseller = newPackagePriceForReseller / Totaldays;
            double newBillForThisClientForReseller = (newPackagePricePerdayForReseller * remainDaysForTheNewPackage);

            double newPackagePriceForResellerUser = lstMacResellerPackagePrice.Where(z => z.PID == clientClientDetails.PackageThisMonth).FirstOrDefault().PPFromRS;
            double newPackagePricePerdayForResellerUser = newPackagePriceForResellerUser / Totaldays;
            double newBillForThisClientForResellerUser = (newPackagePricePerdayForResellerUser * remainDaysForTheNewPackage);

            #endregion

            #region Transaction Update in the transaction table including advance payment if need
            //2 now modifing informaiton in transaction table
            transactionIDExistOrNot.PackageID = clientClientDetails.PackageThisMonth;
            resellerOldAlreadyPaymentAmount = (double)transactionIDExistOrNot.ResellerPaymentAmount;
            transactionIDExistOrNot.PaymentAmount = (float)Math.Round((OldBillForThisClientForUser + newBillForThisClientForResellerUser) - clientClientDetails.PermanentDiscount);

            ////checking reseller has balance or not and change the package Package is lower and Already Paid bill is greate than new bill we have to add this bill into Reseller Balance.
            double resellerPaymentAmount = OldBillForThisClientForReseller + newBillForThisClientForReseller;
            //var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (resellerPaymentAmount > resellerOldAlreadyPaymentAmount)
            {
                double resellerNeedBalance = (resellerPaymentAmount - resellerOldAlreadyPaymentAmount);
                if (reseller.ResellerBalance < resellerNeedBalance)
                {
                    db.EmployeeTransactionLockUnlock.Remove(employeeTransactionLockUnlock);
                    db.SaveChanges();
                    ResellerHasBalance = false;
                    return;
                }
                else
                {
                    reseller.ResellerBalance -= resellerNeedBalance;
                    db.SaveChanges();
                    transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                    transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - transactionIDExistOrNot.PaidAmount;
                    //transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsPaid;
                    //transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                    //transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                }
            }
            else
            {
                reseller.ResellerBalance += Math.Round(resellerOldAlreadyPaymentAmount - resellerPaymentAmount);
                db.SaveChanges();
                //transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                //transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                if (transactionIDExistOrNot.PaymentStatus == AppUtils.PaymentIsPaid)
                {
                    transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsPaid;
                    transactionIDExistOrNot.DueAmount = AppUtils.NoDueAmount;
                    transactionIDExistOrNot.PaidAmount = transactionIDExistOrNot.PaymentAmount;
                    //////////////////////////////////// here need to work for payment history if bill is paid for old transaction and now for new package we have to keep that information into payment history table
                    ///////////////////////////////////////////// done //////////////////////////////////////////////
                }
                else
                {
                    transactionIDExistOrNot.PaymentStatus = AppUtils.PaymentIsNotPaid;
                    transactionIDExistOrNot.DueAmount = transactionIDExistOrNot.PaymentAmount - transactionIDExistOrNot.PaidAmount;
                }

            }
            ///chack done for reseller balance



            transactionIDExistOrNot.PackagePriceForResellerByAdminDuringCreateOrUpdate = (float)newPackagePriceForReseller;
            transactionIDExistOrNot.PackagePriceForResellerUserByResellerDuringCreateOrUpdate = (float)newPackagePriceForResellerUser;
            transactionIDExistOrNot.ResellerPaymentAmount = (float)Math.Round(OldBillForThisClientForReseller + newBillForThisClientForReseller);
            transactionIDExistOrNot.AmountCountDate = dateTimeNow;
            transactionIDExistOrNot.LineStatusID = clientClientDetails.StatusThisMonth;
            transactionIDExistOrNot.ChangePackageHowMuchTimes += 1;

            db.Entry(transactionIDExistOrNot).State = EntityState.Modified;
            db.SaveChanges();
            #endregion 
        }

        private void GenerateBillInTransactionTableIfTheClientIsLock(ref Transaction forMonthlyBill, ClientLineStatus clientClientLineStatuss, ClientDetails ClientClientDetails)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);


            //forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;

            forMonthlyBill.IsNewClient = AppUtils.isNotNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = ClientClientDetails.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = ClientClientDetails.PackageThisMonth;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();

            double packagePricePerday = 0;
            int DaysRemains = 0;
            double MainPackagePrice = db.Package.Find(ClientClientDetails.PackageThisMonth).PackagePrice;
            bool CountRegularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);

            if (CountRegularMonthlyBase == true)//this is for normal day schedule of a month
            {
                packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            }
            else//this is for fix day of a month for example 30
            {
                int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
                packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
                DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            }

            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);

            string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
                            : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
                            : "0";
            float tmp = 0;
            float.TryParse(amount, out tmp);
            //////forMonthlyBill.PaymentAmount = tmp;
            //////thisMonthFee = tmp;
            forMonthlyBill.PaymentAmount = tmp;


            //var dueTransaction = db.ClientDueBills.Where(x=>x.ClientDetailsID == clientClientLineStatus.ClientDetailsID).FirstOrDefault();

            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                //forMonthlyBill.DueAmount = dueTransaction != null ? (float)dueTransaction.DueAmount : 0;
                forMonthlyBill.DueAmount = 0;
                //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
                //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                //forMonthlyBill.DueAmount = dueTransaction != null ? ((float)dueTransaction.DueAmount + forMonthlyBill.PaymentAmount) : forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

        }
        private void GenerateBillInTransactionTableIfTheClientIsLockOrDeleteForCycleClient(ref Transaction forMonthlyBill, ClientLineStatus clientClientLineStatuss, ref ClientDetails ClientClientDetails, ref ClientDetails ClientClientDetailsForUpdate)
        {
            bool keepSameCycleIfClientWasLockOrDeleteInThisMonth = bool.Parse(ConfigurationManager.AppSettings["KeepCycleSameIfClientIsLockOrDeleteInThisMonth"]);

            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            DateTime CycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, ClientClientDetails.ApproxPaymentDate);
            DateTime NExtCycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, ClientClientDetails.ApproxPaymentDate).AddMonths(1);
            string transactionForWhichCycle = "";
            double packagePricePerday = 0; int DaysRemains = 0;

            int totalDaysUploNextCycle = (int)(CycleDate - NExtCycleDate).TotalDays;//DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month) - DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);

            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;
            forMonthlyBill.IsNewClient = AppUtils.isNotNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = ClientClientDetails.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = ClientClientDetails.PackageThisMonth;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = ClientClientDetails.StatusThisMonth;
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();

            double MainPackagePrice = db.Package.Find(ClientClientDetails.PackageThisMonth).PackagePrice;

            if (keepSameCycleIfClientWasLockOrDeleteInThisMonth)//mean we will keep set the old cycle date
            {
                packagePricePerday = (MainPackagePrice / totalDaysUploNextCycle);
                DaysRemains = Convert.ToInt32((NExtCycleDate.Date - currenDateTime.Date).TotalDays) + 1;
                string amount = (packagePricePerday * DaysRemains).ToString();
                float tmp = 0;
                float.TryParse(amount, out tmp);
                forMonthlyBill.PaymentAmount = tmp;
                transactionForWhichCycle = currenDateTime.Year + "" + currenDateTime.Month + "" + ClientClientDetails.ApproxPaymentDate + "_"
                                         + NExtCycleDate.Year + "" + NExtCycleDate.Month + "" + ClientClientDetails.ApproxPaymentDate;
            }
            else
            {
                forMonthlyBill.PaymentAmount = (float)MainPackagePrice;
                ClientClientDetails.ApproxPaymentDate = currenDateTime.Day;//we are adding new billing cycle cause if admin dont want to keep the same billing date then we have to set the new date.
                transactionForWhichCycle = currenDateTime.Year + "" + currenDateTime.Month + "" + currenDateTime.Day + "_"
                                         + NExtCycleDate.Year + "" + NExtCycleDate.Month + "" + currenDateTime.Day;
            }

            forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
            forMonthlyBill.PaidAmount = 0;
            forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            forMonthlyBill.PaymentGenerateUptoWhichDate = NExtCycleDate.AddDays(-1);
            forMonthlyBill.TransactionForWhichCycle = transactionForWhichCycle;

        }
        private void GenerateBillInTransactionTableIfTheClientIsLockOrDeleteForCycleClientByResellerOrByAdminForReseller(ref Transaction forMonthlyBill, ClientLineStatus clientClientLineStatuss, ref ClientDetails ClientClientDetails, ref ClientDetails ClientClientDetailsForUpdate, ref double resellerAmountNeedToActiveThisPackage, Reseller Reseller, ref bool ResellerHasBalanceOrNot)
        {
            bool keepSameCycleIfClientWasLockOrDeleteInThisMonth = bool.Parse(ConfigurationManager.AppSettings["KeepCycleSameIfClientIsLockOrDeleteInThisMonthForReseller"]);

            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            DateTime CycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, ClientClientDetails.ApproxPaymentDate);
            DateTime NExtCycleDate = new DateTime(currenDateTime.Year, currenDateTime.Month, ClientClientDetails.ApproxPaymentDate).AddMonths(1);
            string transactionForWhichCycle = "";
            double packagePricePerdayByReseller = 0;
            double packagePricePerdayByAdmin = 0;
            int DaysRemains = 0;

            int totalDaysUploNextCycle = (int)(CycleDate - NExtCycleDate).TotalDays;//DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month) - DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);

            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimesForReseller;
            forMonthlyBill.IsNewClient = AppUtils.isNotNewClient;
            forMonthlyBill.ResellerID = Reseller.ResellerID;
            forMonthlyBill.ClientDetailsID = ClientClientDetails.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = Reseller.ResellerID;
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = ClientClientDetails.PackageThisMonth;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = ClientClientDetails.StatusThisMonth;
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();

            //forMonthlyBill.ResellerPaymentAmount = ;
            //forMonthlyBill.PackagePriceForResellerByAdminDuringCreateOrUpdate = ;

            List<macReselleGivenPackageWithPriceModel> lstMacResellerPackagePrice = !string.IsNullOrEmpty(Reseller.macReselleGivenPackageWithPrice) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(Reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>();

            int clientclientdetailspackagethismonth = ClientClientDetails.PackageID.Value;
            double MainPackagePriceByReseller = lstMacResellerPackagePrice.Count() > 0 ? lstMacResellerPackagePrice.Where(x => x.PID == clientclientdetailspackagethismonth).FirstOrDefault().PPFromRS : 0;
            double MainPackagePriceByAdmin = lstMacResellerPackagePrice.Count() > 0 ? lstMacResellerPackagePrice.Where(x => x.PID == clientclientdetailspackagethismonth).FirstOrDefault().PPAdmin : 0;

            if (keepSameCycleIfClientWasLockOrDeleteInThisMonth)//mean we will keep set the old cycle date
            {
                packagePricePerdayByReseller = (MainPackagePriceByReseller / totalDaysUploNextCycle);
                packagePricePerdayByAdmin = (MainPackagePriceByAdmin / totalDaysUploNextCycle);
                DaysRemains = Convert.ToInt32((NExtCycleDate.Date - currenDateTime.Date).TotalDays) + 1;
                double amountForResellerUser = (packagePricePerdayByReseller * DaysRemains);
                double amountForReseller = (packagePricePerdayByAdmin * DaysRemains);
                forMonthlyBill.PaymentAmount = (float)amountForResellerUser;
                forMonthlyBill.ResellerPaymentAmount = (float)amountForReseller;
                forMonthlyBill.PackagePriceForResellerByAdminDuringCreateOrUpdate = (float)MainPackagePriceByAdmin;
                transactionForWhichCycle = currenDateTime.Year + "" + currenDateTime.Month + "" + ClientClientDetailsForUpdate.ApproxPaymentDate + "_"
                                         + NExtCycleDate.Year + "" + NExtCycleDate.Month + "" + ClientClientDetailsForUpdate.ApproxPaymentDate;
                resellerAmountNeedToActiveThisPackage = (float)amountForReseller;
            }
            else
            {
                forMonthlyBill.PaymentAmount = (float)MainPackagePriceByReseller;
                forMonthlyBill.ResellerPaymentAmount = (float)MainPackagePriceByAdmin;
                forMonthlyBill.PackagePriceForResellerByAdminDuringCreateOrUpdate = (float)MainPackagePriceByAdmin;
                ClientClientDetails.ApproxPaymentDate = currenDateTime.Day;//we are adding new billing cycle cause if admin dont want to keep the same billing date then we have to set the new date.
                transactionForWhichCycle = currenDateTime.Year + "" + currenDateTime.Month + "" + currenDateTime.Day + "_"
                                         + NExtCycleDate.Year + "" + NExtCycleDate.Month + "" + currenDateTime.Day;
                resellerAmountNeedToActiveThisPackage = (float)MainPackagePriceByAdmin;
            }

            ////double MainPackagePriceByAdmin = lstMacResellerPackagePrice.Count() > 0 ? lstMacResellerPackagePrice.Where(x => x.PID == clientclientdetailspackagethismonth).FirstOrDefault().PPAdmin : 0;

            //if (keepSameCycleIfClientWasLockOrDeleteInThisMonth)//mean we will keep set the old cycle date
            //{
            //    packagePricePerdayByReseller = (MainPackagePriceByAdmin / totalDaysUploNextCycle);
            //    DaysRemains = Convert.ToInt32((NExtCycleDate.Date - currenDateTime.Date).TotalDays) + 1;
            //    string amount = (packagePricePerdayByReseller * DaysRemains).ToString();
            //    float tmp = 0;
            //    float.TryParse(amount, out tmp);
            //    resellerAmountNeedToActiveThisPackage = tmp;
            //}
            //else
            //{
            //    resellerAmountNeedToActiveThisPackage = (float)MainPackagePriceByReseller;
            //}

            double resellerPaymentAmount = (float)forMonthlyBill.ResellerPaymentAmount;
            //var alreadyDBPaidAmount = (transactionIDExistOrNot.PaidAmount != null ? transactionIDExistOrNot.PaidAmount.Value : 0);
            if (resellerPaymentAmount < Reseller.ResellerBalance)
            {
                ResellerHasBalanceOrNot = false;
            }
            else
            {
                ResellerHasBalanceOrNot = true;
                Reseller.ResellerBalance -= resellerPaymentAmount;
                db.SaveChanges();
            }

            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreateForReseller"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RLOrDNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RLOrDMonthlyBill" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }


            //forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
            //forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
            //forMonthlyBill.DueAmount = 0;
            //forMonthlyBill.PaymentGenerateUptoWhichDate = NExtCycleDate.AddDays(-1);
            //forMonthlyBill.TransactionForWhichCycle = transactionForWhichCycle;

        }

        private void RemoveUserFromActiveConnection(ITikConnection connection, ClientDetails oldClientDetailsFromDB, ClientLineStatus ClientClientLineStatus)
        {
            try
            {
                if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock)
                {
                    if (MikrotikLB.CountNumbeOfUserInActive(connection, oldClientDetailsFromDB) > 0)
                    {
                        MikrotikLB.RemoveUserInActiveConenction(connection, oldClientDetailsFromDB);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void RemoveUserFromActiveConnectionByStatusID(ITikConnection connection, ClientDetails oldClientDetailsFromDB, int lineStatus)
        {
            try
            {
                if (lineStatus == AppUtils.LineIsLock)
                {
                    if (MikrotikLB.CountNumbeOfUserInActive(connection, oldClientDetailsFromDB) > 0)
                    {
                        MikrotikLB.RemoveUserInActiveConenction(connection, oldClientDetailsFromDB);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }



        //[HttpPost]
        //public ActionResult UpdateClientDetailsOnlyAllClientForMKT(Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth)
        //{//here?

        //    BillGenerateHistory bgh = db.BIllGenerateHistory.Where(s => s.Year == AppUtils.RunningYear.ToString() && s.Month == AppUtils.RunningMonth.ToString() && s.Status == AppUtils.TableStatusIsActive).FirstOrDefault();

        //    if (bgh == null)
        //    {
        //        return Json(new { BillNotGenerate = true }, JsonRequestBehavior.AllowGet);
        //    }

        //    double packageChangeAmountCalculation = 0; var mikrotikUserInsert = false; DateTime dateTime = AppUtils.GetDateTimeNow();
        //    //this is for showing bill payment checkbox. now system is working fine but the problem is when new client signup and change the package then it will show
        //    //the informatyion fine but check box is not showing cause bill is paid. so we have to it custom for all.
        //    int TransactionID = 0;
        //    if (chkPackageFromRunningMonth == true && chkStatusFromRunningMonth == true)
        //    {
        //        return Json(new { BothChecked = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
        //    if (ClientWithLoginNameCount > 0)
        //    {
        //        return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    ///first check mikrotik is active or not *******************************************************
        //    ITikConnection connectionForGivenByClientMK = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
        //    if (AppUtils.MikrotikOptionEnable)
        //    {
        //        try
        //        {
        //            Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
        //            connectionForGivenByClientMK = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

        //            //checking new mikrotik is exist or not if then continue
        //            ClientDetails oldClientDetailsFromDB = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID);
        //            Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
        //            if (oldClientDetailsFromDB.MikrotikID == null)//create
        //            {   //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first 
        //                //i have to insert new information with new clientlinestatus package id 
        //                //and should have to update information in accounts if this month information found. cause package are not save.


        //                MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);
        //                ////MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK,);
        //                ////Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
        //                //if (checkForExistingTransaction != null)
        //                //{
        //                //    if (checkForExistingTransaction.PackageID != ClientClientDetails.PackageID)
        //                //    {
        //                //        checkForExistingTransaction.PackageID = ClientClientDetails.PackageID;
        //                //        checkForExistingTransaction.PaymentAmount = db.Package.Find(ClientClientDetails.PackageID).PackagePrice;
        //                //        checkForExistingTransaction.PaymentStatus = checkForExistingTransaction.PaymentStatus;
        //                //        db.Entry(checkForExistingTransaction).State = EntityState.Modified;
        //                //        db.SaveChanges();
        //                //    }
        //                //}
        //            }
        //            else //(oldClientDetailsFromDB.MikrotikID != null)//need to update information
        //            {
        //                ITikConnection connForRemoveOldUserFromMik;
        //                connForRemoveOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, oldClientDetailsFromDB.Mikrotik.RealIP, 8728, oldClientDetailsFromDB.Mikrotik.MikUserName, oldClientDetailsFromDB.Mikrotik.MikPassword);

        //                if (oldClientDetailsFromDB.MikrotikID != ClientClientDetails.MikrotikID) //first remove the old information from the old mikrotik
        //                {
        //                    //c1
        //                    // first we are creating user information in new mikrotik then we wil check then we will check the first
        //                    //mikrotik. cause if somehow we delete the information from the first mikrotik and second mikrotik has
        //                    //the same name then error occourd.

        //                    //c2
        //                    // now another checkign for if user check from this month then create information in new mikrotik with the given package
        //                    //other wise we will create with the existing package.
        //                    //c22
        //                    if (chkPackageFromRunningMonth == true)
        //                    {
        //                        //          if (checkForExistingTransaction.ChangePackageHowMuchTimes < AppUtils.MaxPackageChangeHowMuchTimes)//checking if package change already done or not
        //                        //       {
        //                        if (checkForExistingTransaction.PackageID != ClientClientLineStatus.PackageID)
        //                        {
        //                            //          if (checkForExistingTransaction.PaymentStatus == AppUtils.PaymentIsPaid && checkForExistingTransaction.IsNewClient == AppUtils.isNewClient)
        //                            //          {
        //                            MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value);
        //                            //        }
        //                        }
        //                        //     }
        //                    }
        //                    else if (chkStatusFromRunningMonth == true)
        //                    {
        //                        if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        //                        {
        //                            string status = ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive ? AppUtils.MakeUserEnableInMikrotik
        //                                                               : ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik
        //                                                                   : "";

        //                            MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, checkForExistingTransaction.PackageID.Value);
        //                            MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK, ClientClientDetails.LoginName, status);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, checkForExistingTransaction.PackageID.Value);
        //                    }
        //                    //c2done
        //                    //c1start
        //                    // logic is if status is same and from this month then what happen here that is they will create user in new mikrotik and delete
        //                    // from existing mikrotik but under chkpackagefromthis month it will return cause it will check both given and old status is same. 
        //                    // so below part of clientdetails will not update but new mikrotik will save the new information mea loginname, profile.
        //                    //thats why we keep thischecking if package check from this month and status is same then no effect on old mikrotik.

        //                    //if (chkStatusFromRunningMonth == true)
        //                    //{
        //                    //    if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        //                    //    {
        //                    //        int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        //                    //        if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
        //                    //        {
        //                    //            MikrotikLB.RemoveUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        //                    //        }
        //                    //    }

        //                    //}
        //                    //else
        //                    //{
        //                    int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        //                    if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
        //                    {
        //                        MikrotikLB.RemoveUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        //                    }
        //                    //}

        //                    //c1done
        //                }
        //                else //here we have to update information in same mikritik depend on chk running month
        //                {
        //                    //if some how some one delete information from mikrotik then we will get error during update 
        //                    // for this reason first we will count the user in mikrotik. if <1 then we first add user in mikrotik then update
        //                    int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        //                    if (userCountFromOldMikrotk < 1)
        //                    {//suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
        //                        //and should have to update information in accounts if this month information found. cause package are not save.

        //                        //MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientLineStatus);
        //                        MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);

        //                    }
        //                    ///////////////////////////////////////////
        //                    if (chkPackageFromRunningMonth == true)
        //                    {
        //                        //        if (checkForExistingTransaction.ChangePackageHowMuchTimes < AppUtils.MaxPackageChangeHowMuchTimes)//checking if package change already done or not
        //                        //        {
        //                        if (checkForExistingTransaction.PackageID != ClientClientLineStatus.PackageID)
        //                        {// logic is if bill is paid and client is new then we will give an optin to update the package for another time.
        //                         //other wise if client is not new and bill is paid then we will not give an option to update the package. 
        //                         //        if (checkForExistingTransaction.PaymentStatus == AppUtils.PaymentIsPaid && checkForExistingTransaction.IsNewClient == AppUtils.isNewClient)
        //                         //       {
        //                            MikrotikLB.UpdateUserInMikrotikWithPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails, ClientClientLineStatus);
        //                            //      }
        //                            //else
        //                            //{
        //                            //    MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        //                            //}
        //                        }
        //                        //       }
        //                    }
        //                    else if (chkStatusFromRunningMonth == true)
        //                    {//logic is if checked running month but old status and given status is save no need to update informaition in Mikrotik.
        //                        if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        //                        {
        //                            string status = ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive ? AppUtils.MakeUserEnableInMikrotik
        //                            : ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik
        //                                : "";
        //                            MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        //                            MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK, oldClientDetailsFromDB.LoginName, status);
        //                            try
        //                            {
        //                                if (status == AppUtils.MakeUserDisabledInMikrotik)
        //                                {
        //                                    if (MikrotikLB.CountNumbeOfUserInActive(connectionForGivenByClientMK, oldClientDetailsFromDB) > 0)
        //                                    {
        //                                        MikrotikLB.RemoveUserInActiveConenction(connectionForGivenByClientMK, oldClientDetailsFromDB);
        //                                    }

        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
        //                JsonRequestBehavior.AllowGet);
        //        }

        //    }

        //    ////////////////////////////////////////////////////////////////////////////////////

        //    DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
        //    DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
        //    Transaction billGenerateOrNotCount;
        //    if (chkPackageFromRunningMonth == true)
        //    {
        //        Transaction transactionIDExistOrNot = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
        //        if (transactionIDExistOrNot != null)
        //        {
        //            if (transactionIDExistOrNot.PackageID == ClientClientLineStatus.PackageID)
        //            {
        //                return Json(new { PackageIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        //            }
        //            TransactionID = transactionIDExistOrNot.TransactionID;
        //            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        //            SaveInformationInEmployeeTransactoinLockUnlockTableIfBillInNotPaidForFractionBill(dateTime, transactionIDExistOrNot, ClientClientLineStatus, ref packageChangeAmountCalculation);
        //        }
        //    }

        //    if (chkStatusFromRunningMonth == true)
        //    {
        //        Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
        //        if (transactionIDExistForThisClient != null)
        //        {
        //            if (transactionIDExistForThisClient.LineStatusID == ClientClientLineStatus.LineStatusID)
        //            {
        //                return Json(new { StatusIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        //            }
        //            transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
        //            db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //    }



        //    var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).AsQueryable();
        //    try
        //    {
        //        if (ClientClientDetails.ClientDetailsID > 0)
        //        {
        //            ClientClientDetails.RoleID = ClientDetailsForUpdate.FirstOrDefault().RoleID;
        //            db.Entry(ClientDetailsForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientDetails);
        //            db.SaveChanges();
        //        }

        //        if (ClientClientLineStatus.ClientLineStatusID > 0)
        //        {
        //            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        //        }

        //        var ClientDetails = ClientDetailsForUpdate.Select(s => new
        //        {
        //            ClientDetailsID = s.ClientDetailsID,
        //            Name = s.Name,
        //            LoginName = s.LoginName,
        //            PackageName = s.Package.PackageName,
        //            Address = s.Address,
        //            Email = s.Email,
        //            ZoneName = s.Zone.ZoneName,
        //            ContactNumber = s.ContactNumber,
        //            IsPriorityClient = s.IsPriorityClient
        //        });


        //        //var ClientLineStatus = ClientLineStatusForUpdate.Select(s => new { LineStatusID = s.LineStatusID });
        //        var ClientLineStatus = ClientClientLineStatus.LineStatusID;
        //        var ClientPackage = db.Package.Find(ClientClientLineStatus.PackageID).PackageName;

        //        //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //        if (AppUtils.SMSOptionEnable)
        //        {
        //            try
        //            {
        //                SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
        //                if (smsSenderIdPass != null)
        //                {
        //                    SMSReturnDetails message = SetMessage(ClientClientDetails, ClientTransaction, ClientClientLineStatus, chkPackageFromRunningMonth, chkStatusFromRunningMonth, packageChangeAmountCalculation, smsSenderIdPass);

        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }



        //        var JSON = Json(new { LoginNameExist = false, UpdateStatus = true, ClientDetails = ClientDetails, ClientLineStatus = ClientLineStatus, ClientPackage = ClientPackage, MikrotikUserInsert = mikrotikUserInsert, packageChangeAmountCalculation = packageChangeAmountCalculation, chkPackageFromRunningMonth = chkPackageFromRunningMonth, chkStatusFromRunningMonth = chkStatusFromRunningMonth, TransactionID = TransactionID, LineStatusActiveDate = ClientClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? ClientClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(ClientClientLineStatus.LineStatusID) : "" }, JsonRequestBehavior.AllowGet);
        //        JSON.MaxJsonLength = int.MaxValue;
        //        return JSON;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "", RemoveMikrotikInformation = true, MKUserName = ClientClientDetails.LoginName }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //ClientDetails clientDetailsFromDB = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID);

        //var oldIDExistOrNot = connection.CreateCommandAndParameters("/ppp/secret/print", "name", clientDetailsFromDB.LoginName).ExecuteList();
        ////var oldIDExistOrNot1 = connection.CreateCommandAndParameters("/ppp/secret/print", "name", loginName).ExecuteScalar();
        ////var oldIDExistOrNot2 = connection.CreateCommandAndParameters("/ppp/secret/print", "name", loginName).ExecuteSingleRow();
        //if (oldIDExistOrNot.Count() < 1)//means need to add information in mikrotik
        //{
        //    //checking mikrotik old and given new is not same then first delete the information from old one and then add in new mikrotik.
        //    if (clientDetailsFromDB.MikrotikID != null)
        //    {
        //        if (clientDetailsFromDB.MikrotikID != ClientClientDetails.MikrotikID)
        //        {
        //            connection.CreateCommandAndParameters("/ppp/secret/remove", ".id", clientDetailsFromDB.LoginName).ExecuteNonQuery();
        //        }
        //    }
        //    var userPackageUpdatecheckPackageFromRunningMonth = connection.CreateCommandAndParameters("/ppp/secret/add", "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "profile", db.Package.Find(ClientClientLineStatus.PackageID).PackageName.Trim());
        //    userPackageUpdatecheckPackageFromRunningMonth.ExecuteNonQuery();
        //    mikrotikUserInsert = true;
        //    // if package is different when add new entry in mikrotik then we have to udapte package information in accounts
        //    Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();

        //    if (checkForExistingTransaction != null)//  package will change
        //    {
        //        checkForExistingTransaction.PackageID = ClientClientDetails.PackageID;
        //        checkForExistingTransaction.PaymentAmount = db.Package.Find(ClientClientDetails.PackageID).PackagePrice;
        //        checkForExistingTransaction.PaymentStatus = checkForExistingTransaction.PaymentStatus;
        //        db.Entry(checkForExistingTransaction).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    //return Json(new { Success = false, MikrotikFailed = true, Message = "Old login name: " + loginName + " was not exist in Mikrotik. Please Assign First." }, JsonRequestBehavior.AllowGet);
        //}
        //else// need to udpate information in mikrotk
        //{
        //    //var userPackageUpdate = connection.CreateCommandAndParameters("/ppp/secret/set", ".id", "shohel1", "password", "shohels", "profile", "sp111");
        //    //userPackageUpdate.ExecuteNonQuery();

        //    //checking mikrotik old and given new is not same then first delete the information from old one and then add in new mikrotik.
        //    if (clientDetailsFromDB.MikrotikID != null)
        //    {
        //        if (clientDetailsFromDB.MikrotikID != ClientClientDetails.MikrotikID)
        //        {//fist add info in new mikrotik
        //            var userCerate = connection.CreateCommandAndParameters("/ppp/secret/add", "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", "pppoe", "profile", db.Package.Find(ClientClientLineStatus.PackageID).PackageName.Trim());
        //            userCerate.ExecuteNonQuery();
        //            //then delete info from old mikrotik
        //            connection.CreateCommandAndParameters("/ppp/secret/remove", ".id", clientDetailsFromDB.LoginName).ExecuteNonQuery(); 
        //        }
        //    }
        //    if (chkPackageFromRunningMonth == true)
        //    {
        //        var userPackageUpdatecheckPackageFromRunningMonth = connection.CreateCommandAndParameters("/ppp/secret/set", ".id",
        //            clientDetailsFromDB.LoginName, "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password,
        //            "profile", db.Package.Find(ClientClientLineStatus.PackageID).PackageName.Trim());
        //        userPackageUpdatecheckPackageFromRunningMonth.ExecuteNonQuery();
        //    }
        //    else
        //    {
        //        var userPackageUpdate = connection.CreateCommandAndParameters("/ppp/secret/set", ".id", clientDetailsFromDB.LoginName, "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password);
        //        userPackageUpdate.ExecuteNonQuery();
        //    }
        //}

        ////[HttpPost]
        ////public ActionResult UpdateClientDetails(Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth)
        ////{
        ////    double packageChangeAmountCalculation = 0;
        ////    //this is for showing bill payment checkbox. now system is working fine but the problem is when new client signup and change the package then it will show
        ////    //the informatyion fine but check box is not showing cause bill is paid. so we have to it custom for all.
        ////    int TransactionID = 0;
        ////    if (chkPackageFromRunningMonth == true && chkStatusFromRunningMonth == true)
        ////    {
        ////        return Json(new { BothChecked = true }, JsonRequestBehavior.AllowGet);
        ////    }
        ////    int ClientWithLoginNameCount = db.ClientDetails.Where(s => s.LoginName == ClientClientDetails.LoginName && s.ClientDetailsID != ClientClientDetails.ClientDetailsID).Count();
        ////    if (ClientWithLoginNameCount > 0)
        ////    {
        ////        return Json(new { LoginNameExist = true }, JsonRequestBehavior.AllowGet);
        ////    }

        ////    ///first check mikrotik is active or not *******************************************************
        ////    ITikConnection connectionForGivenByClientMK = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
        ////    if (AppUtils.MikrotikOptionEnable)
        ////    {
        ////        try
        ////        {
        ////            Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientClientDetails.MikrotikID.Value).FirstOrDefault();
        ////            connectionForGivenByClientMK = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

        ////            //checking new mikrotik is exist or not if then continue
        ////            ClientDetails oldClientDetailsFromDB = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID);
        ////            Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
        ////            if (oldClientDetailsFromDB.MikrotikID == null)//create
        ////            {
        ////                //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
        ////                //and should have to update information in accounts if this month information found. cause package are not save.

        ////                //MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus);
        ////                MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);



        ////                //MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK,);
        ////                //Transaction checkForExistingTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).FirstOrDefault();
        ////                if (checkForExistingTransaction != null)
        ////                {
        ////                    if (checkForExistingTransaction.PackageID != ClientClientDetails.PackageID)
        ////                    {
        ////                        checkForExistingTransaction.PackageID = ClientClientDetails.PackageID;
        ////                        checkForExistingTransaction.PaymentAmount = db.Package.Find(ClientClientDetails.PackageID).PackagePrice;
        ////                        checkForExistingTransaction.PaymentStatus = checkForExistingTransaction.PaymentStatus;
        ////                        db.Entry(checkForExistingTransaction).State = EntityState.Modified;
        ////                        db.SaveChanges();
        ////                    }
        ////                }
        ////            }
        ////            else //(oldClientDetailsFromDB.MikrotikID != null)//need to update information
        ////            {
        ////                ITikConnection connForRemoveOldUserFromMik;
        ////                connForRemoveOldUserFromMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, oldClientDetailsFromDB.Mikrotik.RealIP, 8728, oldClientDetailsFromDB.Mikrotik.MikUserName, oldClientDetailsFromDB.Mikrotik.MikPassword);

        ////                if (oldClientDetailsFromDB.MikrotikID != ClientClientDetails.MikrotikID) //first remove the old information from the old mikrotik
        ////                {
        ////                    //c1
        ////                    // first we are creating user information in new mikrotik then we wil check then we will check the first
        ////                    //mikrotik. cause if somehow we delete the information from the first mikrotik and second mikrotik has
        ////                    //the same name then error occourd.

        ////                    //c2
        ////                    // now another checkign for if user check from this month then create information in new mikrotik with the given package
        ////                    //other wise we will create with the existing package.
        ////                    //c22
        ////                    if (chkPackageFromRunningMonth == true)
        ////                    {
        ////                        if (checkForExistingTransaction.ChangePackageHowMuchTimes < AppUtils.MaxPackageChangeHowMuchTimes)//checking if package change already done or not
        ////                        {
        ////                            if (checkForExistingTransaction.PackageID != ClientClientLineStatus.PackageID)
        ////                            {
        ////                                if (checkForExistingTransaction.PaymentStatus == AppUtils.PaymentIsPaid && checkForExistingTransaction.IsNewClient == AppUtils.isNewClient)
        ////                                {
        ////                                    MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, ClientClientLineStatus.PackageID.Value);
        ////                                }
        ////                            }
        ////                        }
        ////                    }
        ////                    else if (chkStatusFromRunningMonth == true)
        ////                    {
        ////                        if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        ////                        {
        ////                            string status = ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive ? AppUtils.MakeUserEnableInMikrotik
        ////                                : ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik
        ////                                    : "";

        ////                            MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, checkForExistingTransaction.PackageID.Value);
        ////                            MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK, ClientClientDetails.LoginName, status);
        ////                        }

        ////                    }
        ////                    else
        ////                    {
        ////                        MikrotikLB.CreateUserInMikrotikByPackageID(connectionForGivenByClientMK, ClientClientDetails, checkForExistingTransaction.PackageID.Value);
        ////                    }
        ////                    //c2done
        ////                    //c1start
        ////                    // logic is if status is same and from this month then what happen here that is they will create user in new mikrotik and delete
        ////                    // from existing mikrotik but under chkpackagefromthis month it will return cause it will check both given and old status is same. 
        ////                    // so below part of clientdetails will not update but new mikrotik will save the new information mea loginname, profile.
        ////                    //thats why we keep thischecking if package check from this month and status is same then no effect on old mikrotik.
        ////                    if (chkStatusFromRunningMonth == true)
        ////                    {
        ////                        if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        ////                        {
        ////                            int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        ////                            if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
        ////                            {
        ////                                MikrotikLB.RemoveUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        ////                            }
        ////                        }

        ////                    }
        ////                    else
        ////                    {
        ////                        int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        ////                        if (userCountFromOldMikrotk > 0) //this is checking for if somehow someine delete information from oldmikrotik then we will get error. 
        ////                        {
        ////                            MikrotikLB.RemoveUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        ////                        }
        ////                    }

        ////                    //c1done
        ////                }
        ////                else //here we have to update information in same mikritik depend on chk running month
        ////                {
        ////                    //if some how some one delete information from mikrotik then we will get error during update 
        ////                    // for this reason first we will count the user in mikrotik. if <1 then we first add user in mikrotik then update
        ////                    int userCountFromOldMikrotk = MikrotikLB.CountNumbeOfUserInMikrotik(connForRemoveOldUserFromMik, oldClientDetailsFromDB);
        ////                    if (userCountFromOldMikrotk < 1)
        ////                    {
        ////                        MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientLineStatus, chkStatusFromRunningMonth, checkForExistingTransaction);
        ////                        //MikrotikLB.CreateUserInMikrotikDuringUpdate(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientLineStatus);
        ////                        //suppose i have new package for mikrotik. sp110 but old package for this user is packageX then what happen first i have to insert new information with new clientlinestatus package id 
        ////                        //and should have to update information in accounts if this month information found. cause package are not save.

        ////                    }
        ////                    ///////////////////////////////////////////
        ////                    if (chkPackageFromRunningMonth == true)
        ////                    {
        ////                        if (checkForExistingTransaction.ChangePackageHowMuchTimes < AppUtils.MaxPackageChangeHowMuchTimes)//checking if package change already done or not
        ////                        {
        ////                            if (checkForExistingTransaction.PackageID != ClientClientLineStatus.PackageID)
        ////                            {// logic is if bill is paid and client is new then we will give an optin to update the package for another time.
        ////                                //other wise if client is not new and bill is paid then we will not give an option to update the package. 
        ////                                if (checkForExistingTransaction.PaymentStatus == AppUtils.PaymentIsPaid && checkForExistingTransaction.IsNewClient == AppUtils.isNewClient)
        ////                                {
        ////                                    MikrotikLB.UpdateUserInMikrotikWithPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails, ClientClientLineStatus);
        ////                                }
        ////                                //else
        ////                                //{
        ////                                //    MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        ////                                //}
        ////                            }
        ////                        }
        ////                    }

        ////                    else if (chkStatusFromRunningMonth == true)
        ////                    {//logic is if checked running month but old status and given status is save no need to update informaition in Mikrotik.
        ////                        if (checkForExistingTransaction.LineStatusID != ClientClientLineStatus.LineStatusID)
        ////                        {
        ////                            string status = ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive ? AppUtils.MakeUserEnableInMikrotik
        ////                                : ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik
        ////                                    : "";
        ////                            MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        ////                            MikrotikLB.SetStatusOfUserInMikrotik(connectionForGivenByClientMK, oldClientDetailsFromDB.LoginName, status);
        ////                        }

        ////                    }
        ////                    else
        ////                    {
        ////                        MikrotikLB.UpdateUserInMikrotikWithOutPackageInformation(connectionForGivenByClientMK, oldClientDetailsFromDB, ClientClientDetails);
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message },
        ////                JsonRequestBehavior.AllowGet);
        ////        }

        ////    }

        ////    ////////////////////////////////////////////////////////////////////////////////////


        ////    ////if some how sign up transaction is came then we have to return cause if client check the checkbox of running
        ////    ////month for the package then it will change in to the sign up bill column. then we have to return
        ////    //bool signUpBillIndicator = db.Transaction.Find(ClientTransaction.TransactionID).PaymentTypeID == AppUtils.PaymentTypeIsConnection ? true: false;
        ////    //if (signUpBillIndicator)
        ////    //{
        ////    //    return Json(new { ThisIsSignUpBill = true }, JsonRequestBehavior.AllowGet);
        ////    //}
        ////    ///////return if bill already generated then we will not give an option for change the package. Beafore bill generate we can change andthe package this will effect on bill generate. 
        ////    DateTime dateTime = AppUtils.GetDateTimeNow();
        ////    DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
        ////    DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
        ////    Transaction billGenerateOrNotCount;
        ////    if (chkPackageFromRunningMonth == true)
        ////    {
        ////        ////////////////////if bill not generate then return
        ////        billGenerateOrNotCount = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).FirstOrDefault();
        ////        if (billGenerateOrNotCount == null)
        ////        {
        ////            return Json(new { BillNotGenerate = true }, JsonRequestBehavior.AllowGet);
        ////        }
        ////        //checking transactionID exist or not
        ////        Transaction transactionIDExistOrNot = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
        ////        if (transactionIDExistOrNot != null)
        ////        {

        ////            if (transactionIDExistOrNot.PackageID == ClientClientLineStatus.PackageID)
        ////            {///////////////////if bill is  generated and payment is done for this client then return that you cant change package.
        ////                return Json(new { PackageIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        ////            }

        ////            //if (transactionIDExistOrNot.PaymentStatus == AppUtils.PaymentIsPaid && transactionIDExistOrNot.IsNewClient != AppUtils.isNewClient)
        ////            //{///////////////////if bill is  generated and payment is done for this client then return that you cant change package.
        ////            //    return Json(new { BillAlreadyPaid = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        ////            //}
        ////            //else///change the package now and not more then one time
        ////            //{
        ////            //    if (transactionIDExistOrNot.ChangePackageHowMuchTimes >= AppUtils.MaxPackageChangeHowMuchTimes)
        ////            //    {
        ////            //        return Json(new { PackageCantChangeInThisMonthLimitExist = true }, JsonRequestBehavior.AllowGet);
        ////            //    }
        ////            //    else
        ////            //    {
        ////            TransactionID = transactionIDExistOrNot.TransactionID;
        ////            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        ////            SaveInformationInEmployeeTransactoinLockUnlockTableIfBillInNotPaidForFractionBill(dateTime, transactionIDExistOrNot, ClientClientLineStatus, ref packageChangeAmountCalculation);


        ////            //    }

        ////            //}//change the package

        ////        }///// you can not change the package cause bill is Paid.
        ////        else
        ////        {
        ////            Transaction setInformationForAddTransactionInDBIfTheClientTransactionIsNotExist = new Transaction();
        ////            AddNewTransactionForThisClient(ref setInformationForAddTransactionInDBIfTheClientTransactionIsNotExist, ClientClientDetails, ClientClientLineStatus);
        ////            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);

        ////        }

        ////    }

        ////    if (chkStatusFromRunningMonth == true)
        ////    {
        ////        billGenerateOrNotCount = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).FirstOrDefault();
        ////        if (billGenerateOrNotCount == null)
        ////        {
        ////            return Json(new { BillNotGenerate = true }, JsonRequestBehavior.AllowGet);
        ////        }
        ////        ///////////// if client  this month bill is exist then set the status to coming status
        ////        Transaction transactionIDExistForThisClient = db.Transaction.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator).FirstOrDefault();
        ////        if (transactionIDExistForThisClient != null)
        ////        {

        ////            if (transactionIDExistForThisClient.LineStatusID == ClientClientLineStatus.LineStatusID)
        ////            {///////////////////if bill is  generated and payment is done for this client then return that you cant change package.
        ////                return Json(new { StatusIsSameButRunningMonthChecked = true, CantChangePackageCauseStatusIsLock = "", Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        ////            }
        ////            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        ////            transactionIDExistForThisClient.LineStatusID = ClientClientLineStatus.LineStatusID;
        ////            db.Entry(transactionIDExistForThisClient).State = EntityState.Modified;
        ////            db.SaveChanges();
        ////        }
        ////        else///////////// if client  this month bill is not exist then generate the bill
        ////        {
        ////            Transaction setInformationForAddTransactionInDBIfTheClientTransactionIsNotExist = new Transaction();
        ////            AddNewTransactionForThisClient(ref setInformationForAddTransactionInDBIfTheClientTransactionIsNotExist, ClientClientDetails, ClientClientLineStatus);
        ////            SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        ////        }

        ////    }




        ////    ///////////return if status is lock then nothing can change
        ////    ////Transaction retIfLastStatusIsLockButPackageIsDifferene = db.Transaction.Where(s =>
        ////    ////    s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month && s.LineStatusID == AppUtils.LineIsLock).FirstOrDefault();

        ////    ////if (retIfLastStatusIsLockButPackageIsDifferene != null)
        ////    ////{
        ////    ////    return Json(new { BillAlreadyGenerate = "", CantChangePackageCauseStatusIsLock = true, Success = "", Count = "", LoginNameExist = "", UpdateStatus = "", ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        ////    ////    /////// you can not change anything mean client Information cause this id is lock.
        ////    ////}



        ////    var ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).AsQueryable();
        ////    try
        ////    {
        ////        if (ClientClientDetails.ClientDetailsID > 0)
        ////        {
        ////            ClientClientDetails.RoleID = ClientDetailsForUpdate.FirstOrDefault().RoleID;
        ////            db.Entry(ClientDetailsForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientDetails);
        ////            db.SaveChanges();
        ////        }

        ////        if (ClientClientLineStatus.ClientLineStatusID > 0)
        ////        {
        ////            if (chkStatusFromRunningMonth == false && chkPackageFromRunningMonth == false)
        ////            //if checked mean the line status is already insert from the upper part
        ////            //means this is normal line status which will affect from next month
        ////            {
        ////                SaveClientLineStatusInformation(ref ClientClientLineStatus, ClientClientDetails, chkStatusFromRunningMonth);
        ////            }

        ////        }

        ////        var ClientDetails = ClientDetailsForUpdate.Select(s => new
        ////        {
        ////            ClientDetailsID = s.ClientDetailsID,
        ////            Name = s.Name,
        ////            LoginName = s.LoginName,
        ////            PackageName = s.Package.PackageName,
        ////            Address = s.Address,
        ////            Email = s.Email,
        ////            ZoneName = s.Zone.ZoneName,
        ////            ContactNumber = s.ContactNumber,
        ////            IsPriorityClient = s.IsPriorityClient
        ////        });
        ////        //var ClientLineStatus = ClientLineStatusForUpdate.Select(s => new { LineStatusID = s.LineStatusID });
        ////        var ClientLineStatus = ClientClientLineStatus.LineStatusID;
        ////        var ClientPackage = db.Package.Find(ClientClientLineStatus.PackageID).PackageName;


        ////        //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        ////        if (AppUtils.SMSOptionEnable)
        ////        {
        ////            try
        ////            {
        ////                SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();

        ////                if (smsSenderIdPass != null)
        ////                {
        ////                    SMSReturnDetails message = SetMessage(ClientClientDetails, ClientTransaction, ClientClientLineStatus, chkPackageFromRunningMonth, chkStatusFromRunningMonth, packageChangeAmountCalculation, smsSenderIdPass);

        ////                }
        ////            }
        ////            catch (Exception ex)
        ////            {

        ////            }
        ////        }



        ////        var JSON = Json(new { LoginNameExist = false, UpdateStatus = true, ClientDetails = ClientDetails, ClientLineStatus = ClientLineStatus, LineStatusActiveDate = ClientClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? ClientClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(ClientClientLineStatus.LineStatusID) : "", ClientPackage = ClientPackage, packageChangeAmountCalculation = packageChangeAmountCalculation, chkPackageFromRunningMonth = chkPackageFromRunningMonth, chkStatusFromRunningMonth = chkStatusFromRunningMonth, TransactionID = TransactionID }, JsonRequestBehavior.AllowGet);
        ////        JSON.MaxJsonLength = int.MaxValue;
        ////        return JSON;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
        ////    }
        ////}

        private SMSReturnDetails SetMessage(ClientDetails clientClientDetails, Transaction clientTransaction, ClientLineStatus clientClientLineStatus, bool? chkPackageFromRunningMonth, bool? chkStatusFromRunningMonth, double Amount, SMSSenderIDPass smsSenderIdPass)
        {
            SMS sms = new SMS();
            string message = "";
            if (chkStatusFromRunningMonth.Value == true)
            {
                var thismonthpackage = "";
                Transaction ts = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth &&
                    s.ClientDetailsID == clientClientLineStatus.ClientDetailsID).FirstOrDefault();
                if (ts != null)
                {
                    thismonthpackage = ts.Package.PackageName;
                }
                else
                {
                    ClientLineStatus clientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == clientClientLineStatus.ClientDetailsID)
                        .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault();
                    thismonthpackage = clientLineStatus.Package.PackageName;
                }
                if (clientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                {// You are locked from this month.Package This Month = [PACKAGETHisMonth] Next Month Package =[PackageNextMonth]
                    sms = db.SMS.Where(s => s.SMSCode == AppUtils.Member_Active_This_Month && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                    if (sms != null)
                    {
                        message = sms.SendMessageText;

                        message = message.Replace("PACKAGETHisMonth", thismonthpackage);
                        message = message.Replace("PackageNextMonth",
                            db.Package.Find(clientClientLineStatus.PackageID).PackageName);
                    }
                }
                if (clientClientLineStatus.LineStatusID == AppUtils.LineIsLock)
                {
                    sms = db.SMS.Where(s => s.SMSCode == AppUtils.Member_Locked_This_Month && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                    if (sms != null)
                    {
                        message = sms.SendMessageText;
                        message = message.Replace("PACKAGETHisMonth", thismonthpackage);
                        message = message.Replace("PackageNextMonth",
                            db.Package.Find(clientClientLineStatus.PackageID).PackageName);
                    }
                }
            }
            else if (chkPackageFromRunningMonth.Value == true)
            {
                sms = db.SMS.Where(s => s.SMSCode == AppUtils.Package_Change_This_Month && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                if (sms != null)
                {
                    message = sms.SendMessageText;
                    var thismonthStatus = "";
                    Transaction ts1 = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth &&
                                                                s.ClientDetailsID == clientClientLineStatus.ClientDetailsID).FirstOrDefault();
                    if (ts1 != null)
                    {
                        thismonthStatus = ts1.LineStatus.LineStatusName;
                    }
                    else
                    {
                        ClientLineStatus clientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == clientClientLineStatus.ClientDetailsID)
                            .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault();
                        thismonthStatus = clientLineStatus.LineStatus.LineStatusName;
                    }

                    message = message.Replace("[PACKAGE]", db.Package.Find(clientClientLineStatus.PackageID).PackageName);
                    message = message.Replace("[AMOUNT]", Amount.ToString());
                    message = message.Replace("[StatusThisMonth]", thismonthStatus);
                    if (clientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
                    {
                        message = message.Replace("[STATUSNextMonth]", "Active");
                    }
                    if (clientClientLineStatus.LineStatusID == AppUtils.LineIsLock)
                    {
                        message = message.Replace("STATUSNextMonth", "Lock");
                    }
                }

            }
            else if (clientClientLineStatus.LineStatusID == AppUtils.LineIsLock)
            {//  You are locked from Next month . Package = [PACKAGE]
                sms = db.SMS.Where(s => s.SMSCode == AppUtils.Member_Locked_Next_Month && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();

                message = sms.SendMessageText;
                if (sms != null)
                {
                    message = message.Replace("PACKAGE", db.Package.Find(clientClientLineStatus.PackageID).PackageName);
                }
            }
            else if (clientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
            {//  You are locked from Next month . Package = [PACKAGE]
                sms = db.SMS.Where(s => s.SMSCode == AppUtils.Member_Active_Next_Month && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                message = sms.SendMessageText;
                if (sms != null)
                {
                    message = message.Replace("[PACKAGE]", db.Package.Find(clientClientLineStatus.PackageID).PackageName);
                }
            }

            if (sms != null)
            {
                SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, clientClientDetails.ContactNumber, message);
                if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                {
                    sms.SMSCounter += 1;
                    db.Entry(sms).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return SMSReturnDetails;
            }
            else
            {
                return new SMSReturnDetails();
            }
        }


        //if (chkStatusFromRunningMonth == true)
        //{
        //    //////////////***********checking if the bill already generate for this id then we have to store those days payment in EmpTsLockUnlockTabe For specific bill for those days.
        //    Transaction tsClienTransactionForAmount = db.Transaction.Where(s =>
        //        s.ClientDetailsID == ClientClientDetails.ClientDetailsID && s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month).FirstOrDefault();

        //    if (tsClienTransactionForAmount != null)
        //    {
        //        if (tsClienTransactionForAmount.PaymentStatus == AppUtils.PaymentIsPaid) //then no need to stroe information in employeetransaciotnlockunlock table. just change status in transaction table.
        //        {
        //            ChangeLineStatutsIfBillIsPaid(ref tsClienTransactionForAmount);
        //        }
        //        else//payment is not paid then we have to calculte the fraction bill for rest of the day for this reason we have to add infor in lockunlock table
        //        {
        //            SaveInformationInEmployeeTransactoinLockUnlockTableIfBillInNotPaidForFractionBill(dateTime, tsClienTransactionForAmount,ClientClientLineStatus);
        //            if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsActive)
        //            {
        //                tsClienTransactionForAmount.LineStatusID = AppUtils.LineIsActive;
        //                tsClienTransactionForAmount.EmployeeID = AppUtils.LoginUserID;
        //                db.Entry(tsClienTransactionForAmount).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //            if (ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock)
        //            {
        //                tsClienTransactionForAmount.LineStatusID = AppUtils.LineIsLock;
        //                tsClienTransactionForAmount.EmployeeID = AppUtils.LoginUserID;
        //                db.Entry(tsClienTransactionForAmount).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //}

        private void AddNewTransactionForThisClient(ref Transaction forMonthlyBill, ClientDetails clientClientDetails, ClientLineStatus ClientClientLineStatus)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            forMonthlyBill = new Transaction();
            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;
            forMonthlyBill.IsNewClient = AppUtils.isNotNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = clientClientDetails.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            //forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
            forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
            forMonthlyBill.PackageID = ClientClientLineStatus.PackageID;
            double packagePricePerday = (db.Package.Find(ClientClientLineStatus.PackageID).PackagePrice / totalDaysInThisMonth);
            int DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            string amount = (packagePricePerday * DaysRemains).ToString();
            float tmp = 0;
            float.TryParse(amount, out tmp);
            forMonthlyBill.PaymentAmount = tmp;
            forMonthlyBill.LineStatusID = ClientClientLineStatus.LineStatusID;
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();
            db.Transaction.Add(forMonthlyBill);
            db.SaveChanges();
        }

        private void SaveInformationInEmployeeTransactoinLockUnlockTableIfBillInNotPaidForFractionBill(DateTime dateTime, Transaction tsClienTransactionForAmount, ClientLineStatus ClientLineStatus, ref double packageChangeAmountCalculation)
        {
            //tsClienTransactionForAmount.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //tsClienTransactionForAmount.ResetNo = "";

            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            int dbPaymentStatus = tsClienTransactionForAmount.PaymentStatus;
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            //DateTime dt = new DateTime(dateTime.Year, dateTime.Month, 1);
            //int geenrateBillDifference = (int)(tsClienTransactionForAmount.AmountCountDate.Value.Date - dt.Date).TotalDays;

            int totalUsedDaysWithThisPackage = Convert.ToInt32((dateTime - tsClienTransactionForAmount.AmountCountDate.Value).TotalDays + 1);
            double totalBillForOldPackage = (tsClienTransactionForAmount.Package.PackagePrice / Totaldays) * totalUsedDaysWithThisPackage;

            int remainDaysForTheNewPackage = (Totaldays - AppUtils.GetDateTimeNow().Date.Day);
            double newPackagePricePerday = db.Package.Find(ClientLineStatus.PackageID).PackagePrice / Totaldays;
            double totalBillForTheNewPackage = (newPackagePricePerday * remainDaysForTheNewPackage);

            EmployeeTransactionLockUnlock employeeTransactionLockUnlock = new EmployeeTransactionLockUnlock
            {
                TransactionID = tsClienTransactionForAmount.TransactionID,
                Amount = totalBillForOldPackage,
                PackageID = tsClienTransactionForAmount.PackageID,
                FromDate = tsClienTransactionForAmount.AmountCountDate.Value,
                ToDate = dateTime,
                LockOrUnlockDate = dateTime,
                EmployeeID = AppUtils.LoginUserID
            };
            //employeeTransactionLockUnlock.PaymentYear = AppUtils.RunningYear;
            //employeeTransactionLockUnlock.PaymentMonth = AppUtils.RunningMonth;
            db.EmployeeTransactionLockUnlock.Add(employeeTransactionLockUnlock);
            db.SaveChanges();

            tsClienTransactionForAmount.AmountCountDate = dateTime;
            tsClienTransactionForAmount.PackageID = ClientLineStatus.PackageID;
            var a = AppUtils.ThisMonthStartDate();
            double previousBill = db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == tsClienTransactionForAmount.TransactionID && s.FromDate >= a).Count() > 0 ? db.EmployeeTransactionLockUnlock.Where(s => s.TransactionID == tsClienTransactionForAmount.TransactionID && s.FromDate >= a).Sum(s => s.Amount) : totalBillForOldPackage;
            float f = 0;
            string sum = Convert.ToInt32(previousBill + totalBillForTheNewPackage).ToString();
            float.TryParse(sum, out f);
            tsClienTransactionForAmount.PaymentAmount = f;

            var alreadyDBPaidAmount = (tsClienTransactionForAmount.PaidAmount != null ? tsClienTransactionForAmount.PaidAmount.Value : 0);
            if (tsClienTransactionForAmount.PaymentAmount > alreadyDBPaidAmount)
            {
                tsClienTransactionForAmount.PaymentStatus = AppUtils.PaymentIsNotPaid;
                tsClienTransactionForAmount.DueAmount = tsClienTransactionForAmount.PaymentAmount - alreadyDBPaidAmount;
            }
            else
            {
                float advanceAmount = alreadyDBPaidAmount - tsClienTransactionForAmount.PaymentAmount.Value;
                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == tsClienTransactionForAmount.ClientDetailsID).FirstOrDefault();

                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += advanceAmount;
                    advancePayment.Remarks = "Payment Remarks";
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null? "rst"+SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, advancePayment, advanceAmount);
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = tsClienTransactionForAmount.ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = advanceAmount;
                    insertAdvancePayment.Remarks = "Payment Remarks";
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                    //UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(tsClienTransactionForAmount.ResetNo == null ? "rst" + SerialNo() : tsClienTransactionForAmount.ResetNo, tsClienTransactionForAmount, insertAdvancePayment, advanceAmount);
                }
            }

            tsClienTransactionForAmount.ChangePackageHowMuchTimes += 1;
            db.Entry(tsClienTransactionForAmount).State = EntityState.Modified;
            db.SaveChanges();
            packageChangeAmountCalculation = tsClienTransactionForAmount.PaymentAmount.Value;
        }


        private void UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(string resetNo, Transaction ts, AdvancePayment advancePayment, float advanceAmount)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = ts.EmployeeID.Value;
            ph.CollectByID = AppUtils.GetLoginUserID();
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.AdvancePaymentID = advancePayment.AdvancePaymentID;
            ph.PaidAmount = advanceAmount;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        private PaymentHistory UpdatePaymentIntoPaymentHistoryForClientCreate(string resetNo, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.CollectByID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = ts.PaymentAmount.Value;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
            return ph;
        }
        private PaymentHistory UpdatePaymentIntoPaymentHistoryForClientCreateFromReseller(string resetNo, Transaction ts, int ClientAddingByResellerOrAdmin)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            if (ClientAddingByResellerOrAdmin == 1)//meaning its reseller himself
            {
                ph.ResellerID = int.Parse(Session["LoggedUserID"].ToString());
            }
            else
            {
                ph.ResellerID = ts.ResellerID;
                ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
            }

            //meaning if admin login then we are setting bill collect by is reseller id which is coming from client side. So if id is present its mean its added by admin otherwise reseller added it by himself.
            ph.CollectByID = ts.BillCollectBy != null ? ts.BillCollectBy.Value : int.Parse(Session["LoggedUserID"].ToString());
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = ts.PaymentAmount.Value;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
            return ph;
        }
        private void ChangeLineStatutsIfBillIsPaid(ref Transaction tsClienTransactionForAmount)
        {
            tsClienTransactionForAmount.LineStatusID = AppUtils.LineIsLock;
            tsClienTransactionForAmount.EmployeeID = AppUtils.GetLoginUserID();
            db.Entry(tsClienTransactionForAmount).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void SaveClientLineStatusInformation(ref ClientLineStatus ClientClientLineStatus, ClientDetails ClientClientDetails, bool? chkStatusFromRunningMonth)
        {

            if (chkStatusFromRunningMonth != false)
            {
                ClientClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            }
            else
            {
                ClientClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromNextMonth;
            }

            ClientClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
            ClientClientLineStatus.LineStatusWillActiveInThisDate = ClientClientLineStatus.LineStatusWillActiveInThisDate;
            ClientClientLineStatus.StatusThisMonth = ClientClientDetails.StatusThisMonth;
            ClientClientLineStatus.StatusNextMonth = ClientClientDetails.StatusNextMonth;
            ClientClientLineStatus.ClientDetailsID = ClientClientDetails.ClientDetailsID;
            ClientClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
            ClientClientLineStatus.CreateDate = AppUtils.GetDateTimeNow();
            db.ClientLineStatus.Add(ClientClientLineStatus);
            db.SaveChanges();
        }

        private void SaveResellerClientLineStatusInformation(ref ClientLineStatus ClientClientLineStatus, ClientDetails ClientClientDetails, bool? chkStatusFromRunningMonth)
        {

            if (chkStatusFromRunningMonth != false)
            {
                ClientClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            }
            else
            {
                ClientClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromNextMonth;
            }

            ClientClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
            ClientClientLineStatus.LineStatusWillActiveInThisDate = ClientClientLineStatus.LineStatusWillActiveInThisDate;
            ClientClientLineStatus.StatusThisMonth = ClientClientDetails.StatusThisMonth;
            ClientClientLineStatus.StatusNextMonth = ClientClientDetails.StatusNextMonth;
            ClientClientLineStatus.ClientDetailsID = ClientClientDetails.ClientDetailsID;
            ClientClientLineStatus.ResellerID = AppUtils.GetLoginUserID();
            ClientClientLineStatus.CreateDate = AppUtils.GetDateTimeNow();
            db.ClientLineStatus.Add(ClientClientLineStatus);
            db.SaveChanges();
        }

        private double GetBillByPackageUsingStartDateAndEndDate(int packageID, DateTime FromDate, DateTime ToDate)
        {

            Package package = db.Package.Find(packageID);
            double perDayAmount = packageID / 30;

            int difference = Convert.ToInt32((FromDate - ToDate).TotalDays);
            double bill = perDayAmount * difference;
            return bill;


        }

        [HttpPost]
        //[Authorize(Roles = "2")]
        public ActionResult UpdateClientInformationFromClient(Models.ClientDetails ClientClientDetails)
        {
            ClientDetails ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();
            try
            {
                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    //ClientClientDetails.RoleID = AppUtils.ClientRole;
                    //db.Entry(ClientDetailsForUpdate.FirstOrDefault()).CurrentValues.SetValues(ClientClientDetails);
                    SetInformationFromClientToPulledClientFromDatabase(ref ClientDetailsForUpdate, ClientClientDetails);
                    db.Entry(ClientDetailsForUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var JSON = Json(new { UpdateStatus = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "", ClientLineStatus = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetInformationFromClientToPulledClientFromDatabase(ref ClientDetails clientDetails, ClientDetails clientClientDetails)
        {
            clientDetails.Name = clientClientDetails.Name;
            clientDetails.Email = clientClientDetails.Email;
            clientDetails.ContactNumber = clientClientDetails.ContactNumber;
            clientDetails.Address = clientClientDetails.Address;
            clientDetails.SMSCommunication = clientClientDetails.SMSCommunication;
            clientDetails.Occupation = clientClientDetails.Occupation;
            clientDetails.NationalID = clientClientDetails.NationalID;
            clientDetails.Reference = clientClientDetails.Reference;
            clientDetails.SecurityQuestionID = clientClientDetails.SecurityQuestionID;
            clientDetails.SecurityQuestionAnswer = clientClientDetails.SecurityQuestionAnswer;
            clientDetails.SocialCommunicationURL = clientClientDetails.SocialCommunicationURL;
        }

        //private void UpdatePaymentIntoPaymentHistoryTableDuringCreateForSignUpBill(Transaction transaction, Transaction ts)
        //{
        //    PaymentHistory ph = new PaymentHistory();
        //    ph.TransactionID = ts.TransactionID;
        //    ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*ts.EmployeeID.Value*/;
        //    ph.CollectByID = transaction.BillCollectBy.Value;
        //    ph.PaymentDate = ts.PaymentDate.Value;
        //    ph.PaidAmount = transaction.PaidAmount.Value;
        //    ph.ResetNo = transaction.ResetNo;
        //    ph.Status = AppUtils.TableStatusIsActive;
        //    db.PaymentHistory.Add(ph);
        //    db.SaveChanges();
        //}


        private void SendInformationIntoMikrotikIfEnable(ClientDetails clientDetails)
        {
            Mikrotik mk = db.Mikrotik.Where(s => s.MikrotikID == clientDetails.MikrotikID).FirstOrDefault();
            if (mk != null)
            {
                using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                {
                    // connection.Open(ConfigurationManager.AppSettings["host"], ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["pass"]);
                    connection.Open(mk.RealIP, mk.MikUserName, mk.MikPassword);

                    var userCerate = connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", "pppoe", "profile", clientDetails.Package.PackageName);
                    userCerate.ExecuteNonQuery();

                }
            }
        }

        public int RemarksNo()
        {
            Remarks remarks = db.Remarks.Find(1);
            remarks.RemarksNo += 1;
            db.SaveChanges();
            return remarks.RemarksNo - 1;
        }

        public int SerialNo()
        {
            Serial serial = db.Serial.Find(1);
            serial.SerialNo += 1;
            db.SaveChanges();
            return serial.SerialNo - 1;
        }
        private void SetCableStockDistribution(ref CableDistribution cableDistribution, CableStock cableStock, ClientCableAssign clientCableAssign, ClientDetails clientDetails)
        {
            cableDistribution.AmountOfCableUsed = clientCableAssign.CableQuantity;
            cableDistribution.CableStockID = cableStock.CableStockID;
            cableDistribution.CableAssignFromWhere = AppUtils.CableAssignFromNewClient;
            cableDistribution.ClientDetailsID = clientDetails.ClientDetailsID;
            //cableDistribution.EmployeeID = clientCableAssign.EmployeeID;
            cableDistribution.CableForEmployeeID = clientCableAssign.EmployeeID;
            cableDistribution.Purpose = "Client Purpose";
        }

        private void SetNewStockDistribution(ref Distribution distribution, StockDetails stockDetails, List<ClientStockAssign> itemListForEmployee, ClientDetails ClientDetails)
        {
            distribution.EmployeeID = itemListForEmployee[0].EmployeeID;
            distribution.StockDetailsID = stockDetails.StockDetailsID;
            distribution.ClientDetailsID = ClientDetails.ClientDetailsID;
            distribution.DistributionReasonID = AppUtils.DistributionReasonIsNewConenction;
            distribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            distribution.CreatedDate = AppUtils.GetDateTimeNow();
        }

        private void SetNewStockDistribution(ref Distribution distribution, Client_Stock_StockDetails_ForDistribution item)
        {
            distribution.EmployeeID = item.EmployeeID;
            distribution.StockDetailsID = item.StockDetailsID;
            distribution.PopID = item.PopID;
            distribution.BoxID = item.BoxID;
            distribution.ClientDetailsID = item.CustomerID;
            distribution.DistributionReasonID = item.DistributionReasonID;
            distribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            distribution.CreatedDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Client)]
        public ActionResult DeleteClientDetails(int ClientDetailsID)
        {
            //var firstPart = (ZoneID == "") ? db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole).AsQueryable() : db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == zoneFromDDL).AsQueryable();

            ////db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == ((ZoneID == "")  ? 1: int.Parse(ZoneID)))
            //var secondpart = firstPart.AsEnumerable().GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //                     , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //                         (ClientDetails, ClientLineStatus) => new
            //                         {
            //                             ClientDetails = ClientDetails,
            //                             ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //                         })
            //                     .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
            //                         Transaction => Transaction.ClientDetailsID,
            //                         (ClientDetails, Transaction) => new
            //                         {
            //                             ClientDetails = ClientDetails,
            //                             ClientLineStatus = ClientDetails.ClientLineStatus,
            //                             Transaction = Transaction.FirstOrDefault()
            //                         }).AsEnumerable();

            var clientlinestatusid = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID)
                                          //.GroupJoin(
                                          //            db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate)/*.FirstOrDefault()*/)
                                          //                , clndtl => clndtl.ClientDetailsID, cls => cls.FirstOrDefault().ClientDetailsID, (clndtl, cls) => new
                                          //                {
                                          //                    clndtl = clndtl,
                                          //                    cls = cls.FirstOrDefault()
                                          //                }
                                          //          )
                                          //.GroupJoin(
                                          //            db.Transaction.Where(s => s.PaymentMonth == AppUtils.RunningMonth && s.PaymentYear == AppUtils.RunningYear)
                                          //                , clndtl => clndtl.clndtl.ClientDetailsID, clstrn => clstrn.ClientDetailsID, (clndt, clstrn) => new
                                          //                {
                                          //                    clndt = clndt,
                                          //                    cls = clndt.cls.FirstOrDefault(),
                                          //                    clstrn = clstrn.FirstOrDefault()
                                          //                }
                                          //          )
                                          //          .Select(s => new
                                          //          {
                                          //              LineStatusInThisMonth = s.clstrn != null ? s.clstrn.LineStatusID : s.cls.LineStatus.LineStatusID
                                          //          })
                                          //          .AsEnumerable()
                                          ;
            if (clientlinestatusid.Any())
            {
                //var val = clientlinestatusid.FirstOrDefault().LineStatusInThisMonth;
                //if (clientlinestatusid.FirstOrDefault().LineStatusInThisMonth != AppUtils.LineIsLock)
                //{
                //    return Json(new { DeleteStatus = false, Reason = "This User Can Not Remove From the system.Cause This is Active Client. Please Lock First. ", ClientDetailsID = ClientDetailsID }, JsonRequestBehavior.AllowGet);
                //}
                try
                {
                    if (AppUtils.MikrotikOptionEnable)
                    {

                        ClientDetails clientDetails = db.ClientDetails.Find(ClientDetailsID);
                        if (clientDetails.Mikrotik != null)
                        {
                            ITikConnection connection = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
                            connection = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, clientDetails.Mikrotik.RealIP, 8728, clientDetails.Mikrotik.MikUserName, clientDetails.Mikrotik.MikPassword);

                            try
                            {
                                if (MikrotikLB.CountNumbeOfUserInActive(connection, clientDetails) > 0)
                                {
                                    MikrotikLB.RemoveUserInActiveConenction(connection, clientDetails);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            MikrotikLB.RemoveUserInMikrotik(connection, clientDetails);

                        }
                    }



                    //int countTransactionLockUnlock = db.EmployeeTransactionLockUnlock.RemoveRange(db.EmployeeTransactionLockUnlock.Where(s => s.Transaction.ClientDetailsID == ClientDetailsID)).Count();
                    //int countTransaction = db.Transaction.RemoveRange(db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countClientLineStatus = db.ClientLineStatus.RemoveRange(db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countAdvancePayment = db.AdvancePayment.RemoveRange(db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countClientDueBills = db.ClientDueBills.RemoveRange(db.ClientDueBills.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countComplain = db.Complain.RemoveRange(db.Complain.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countItemDistribution = db.Distribution.RemoveRange(db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countCableDistribution = db.CableDistribution.RemoveRange(db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();
                    //int countClientDetails = db.ClientDetails.RemoveRange(db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID)).Count();


                    clientlinestatusid.FirstOrDefault().Status = AppUtils.TableStatusIsDelete;
                    //clientlinestatusid.FirstOrDefault().StatusThisMonth = AppUtils.TableStatusIsDelete; //notsurejustassumption
                    db.SaveChanges();
                    return Json(new { DeleteStatus = true, Reason = "", ClientDetailsID = ClientDetailsID }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { DeleteStatus = false, Reason = "", ClientDetailsID = ClientDetailsID }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { DeleteStatus = false, Reason = "", ClientDetailsID = ClientDetailsID }, JsonRequestBehavior.AllowGet);
            }



        }

        private void DeleteClientDetails_Transaction_Status(ClientLineStatus ClientLineStatusSave, Transaction TransactonSave, ClientDetails ClientDetailsSave, PaymentHistory ph)
        {
            if (ClientLineStatusSave.ClientLineStatusID > 0)
            {
                ClientLineStatus ClienLineStatusDelete = db.ClientLineStatus.Find(ClientLineStatusSave.ClientLineStatusID);
                db.ClientLineStatus.Remove(ClienLineStatusDelete);
                db.SaveChanges();
            }
            if (ph.PaymentHistoryID > 0)
            {
                PaymentHistory paymentHistory = db.PaymentHistory.Find(ph.PaymentHistoryID);
                db.PaymentHistory.Remove(paymentHistory);
                db.SaveChanges();
            }
            if (TransactonSave.TransactionID > 0)
            {
                Transaction TransactionDelete = db.Transaction.Find(TransactonSave.TransactionID);
                db.Transaction.Remove(TransactionDelete);
                db.SaveChanges();
            }
            if (ClientDetailsSave.ClientDetailsID > 0)
            {
                ClientDetails ClientDetailsDelete = db.ClientDetails.Find(ClientDetailsSave.ClientDetailsID);
                db.ClientDetails.Remove(ClientDetailsDelete);
                db.SaveChanges();
            }
        }

        public ActionResult GetSpecificClientDetailsFromDashBoard(int CID)
        {

            ClientLineStatus cls = new ClientLineStatus();
            ClientLineStatus clientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == CID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault();
            Transaction ts = db.Transaction.Where(s => s.ClientDetailsID == CID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection).FirstOrDefault();

            var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == CID)
                                      join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                                      from cblscka in a.DefaultIfEmpty()
                                      join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                                      from cblTypeb in b.DefaultIfEmpty()

                                      select new
                                      {
                                          CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                                          CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                                          AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                                      }).GroupBy(s => s.CableTypeID).ToList();

            var itemForThisClient =
                (from proddis in db.Distribution.Where(s => s.ClientDetailsID == CID)
                 join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                 from clnproddistribution in a.DefaultIfEmpty()

                 join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                 from clnstock in b.DefaultIfEmpty()

                 join items in db.Item on clnstock.ItemID equals items.ItemID into c
                 from clnItems in c.DefaultIfEmpty()

                 select new
                 {
                     ItemID = clnItems.ItemID,
                     ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                     ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                 }).GroupBy(s => s.ItemID).ToList();

            var totalProductList = "";
            foreach (var itemRow in cableForThisClient)
            {
                foreach (var itemColm in itemRow)
                {
                    totalProductList += "<b>CableType:&nbsp;&nbsp;" + itemColm.CableType + "&nbsp;&nbspQuantity:&nbsp;" + itemColm.AmountOfCableGiven + "&nbsp;</b><br/>";
                }
            }
            foreach (var itemRow in itemForThisClient)
            {
                foreach (var itemColm in itemRow)
                {
                    totalProductList += "<b>Item Name:&nbsp;&nbsp;" + itemColm.ItemName + "&nbsp;&nbsp;Serial:&nbsp;" + itemColm.ItemSerial + " </b><br/>";
                }
            }
            ViewBag.TotalProductList = totalProductList;
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName", clientLineStatus.ClientDetails.ResellerID);
            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName", clientLineStatus.ClientDetails.MikrotikID);


            ViewBag.Password = clientLineStatus.ClientDetails.Password;
            ViewBag.SingUpFee = ts.PaymentAmount;
            ViewBag.BillPaymentDate = clientLineStatus.ClientDetails.ApproxPaymentDate;//ts.PaymentDate;
            ViewBag.LineStatusActiveDate = clientLineStatus.LineStatusWillActiveInThisDate.HasValue ? clientLineStatus.LineStatusWillActiveInThisDate.Value.ToString("MM/dd/yyyy") : "";
            ViewBag.TransactionID = ts.TransactionID;
            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName", clientLineStatus.ClientDetails.ZoneID);
            //ViewBag.SearchByZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName", clientLineStatus.ClientDetails.ConnectionTypeID);
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName", clientLineStatus.ClientDetails.CableTypeID);
            ViewBag.PackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName", clientLineStatus.PackageID);
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName", clientLineStatus.ClientDetails.SecurityQuestionID);
            ViewBag.LineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName", clientLineStatus.LineStatusID);
            //ViewBag.BannedID = new SelectList(db.BannedStatus.ToList(), "BannedStatusID", "BannedStatusName");



            return View(clientLineStatus);
        }
        private void LoadViewBag(int ResellerID = 0)
        {
            var ClientTypeEnum = new SelectList(Enum.GetValues(typeof(ClientTypeEnum)).Cast<ClientTypeEnum>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(ClientTypeEnum), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            //List<SelectList> selectListItems = new List<SelectList>();
            //selectListItems.Add(new SelectListItem() { Value = AppUtils.LineIsLock, Text = "All Clients" }, new SelectListItem() { Value = 1, Text = "All Clients" });
            ViewBag.SearchByClientType = ClientTypeEnum;

            if (ResellerID > 0)
            {
                ViewBag.BoxID = new SelectList(db.Box.Where(x => x.ResellerID == ResellerID).Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

                var lstZone = db.Zone.Where(x => x.ResellerID == ResellerID).Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList();
                ViewBag.ZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
                ViewBag.SearchByZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
            }
            else
            {
                ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

                var lstZone = db.Zone.Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList();
                ViewBag.ZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
                ViewBag.SearchByZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
            }


            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");

            //ViewBag.PackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");

            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.LineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            ViewBag.BannedID = new SelectList(db.BannedStatus.ToList(), "BannedStatusID", "BannedStatusName");
            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Client_List)]
        public ActionResult GetAllCLients()
        {
            //var demoActionData =
            //    db.Form.
            //    Join(db.Action.Where(s => s.ShowingStatus == 1 && s.ActionDescription != ""),
            //        Form => Form.FormID,
            //        Action => Action.FormID,
            //        (Form, Action) => new { Form = Form, Action = Action })
            //   .Join(
            //        db.ControllerName,
            //        Forms => Forms.Form.ControllerNameID,
            //        Controller => Controller.ControllerNameID,
            //        (Forms, Controller) => new { Forms = Forms, Action = Forms.Action, Controller = Controller })

            //        .OrderBy(s => s.Controller.ControllerNameID).ThenBy(s => s.Forms.Form.FormID).AsEnumerable().Select(
            //        s => new
            //        {
            //            MyButtonInfo = "public const string " + s.Controller.ControllerNames + "_" + s.Forms.Form.FormName + "_" + s.Action.ActionDescription.Trim().Replace(" ", "__") + " = \"" + s.Action.ActionValue + "\";"

            //            // Action = 
            //        }
            //        );
            LoadViewBag();
            //var test = db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole)
            //    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //        (ClientDetails, ClientLineStatus) => new
            //        {
            //            ClientDetails = ClientDetails,
            //            ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //        });

            //List<ClientCustomInformation> lstClientCustomInformation =
            //    db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole)
            //        .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //            (ClientDetails, ClientLineStatus) => new
            //            {
            //                ClientDetails = ClientDetails,
            //                ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //            })
            //        .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
            //            Transaction => Transaction.ClientDetailsID,
            //            (ClientDetails, Transaction) => new
            //            {
            //                ClientDetails = ClientDetails,
            //                ClientLineStatus = ClientDetails.ClientLineStatus,
            //                Transaction = Transaction.FirstOrDefault()
            //            }).AsEnumerable()
            //            .Select(
            //        s => new ClientCustomInformation
            //        {
            //            ClientDetailsID = s.ClientDetails.ClientDetails.ClientDetailsID,
            //            ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
            //            Name = s.ClientDetails.ClientDetails.Name,
            //            LoginName = s.ClientDetails.ClientDetails.LoginName,
            //            PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
            //            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
            //            Address = s.ClientDetails.ClientDetails.Address,
            //            Email = s.ClientDetails.ClientDetails.Email,
            //            Zone = s.ClientDetails.ClientDetails.Zone.ZoneName,
            //            ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
            //            StatusThisMonthID = s.Transaction != null ? s.Transaction.LineStatusID.ToString() : s.ClientLineStatus.LineStatus.LineStatusID.ToString(),
            //            StatusNextMonthID = s.ClientLineStatus.LineStatus.LineStatusID.ToString(),

            //        }).ToList();


            //return View(lstClientCustomInformation);

            return View(new List<ClientCustomInformation>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllClientsAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {


            //var path = Server.MapPath("~/Log/Logss.txt");
            //System.IO.File.AppendAllText(@"" + path + "", "Test1>" + Environment.NewLine);

            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                int SearchClientTypeDDL = 0;
                bool isCheckAllFromCln = false;
                bool IsCheckedRealIpUser = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };
                int[] SMSSendAry = new int[] { };
                // Initialization.   
                //var YearID = Request.Form.Get("YearID");
                //var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var IsCheckAll = Request.Form.Get("IsCheckAll");
                var CheckedRealIpUser = Request.Form.Get("checkedRealIpUser");
                //var clnIfIsCheckAllThenNonCheckList = Request.Form.Get("IfIsCheckAllThenNonCheckList");
                //var clnIfNotCheckAllThenCheckList = Request.Form.Get("IfNotCheckAllThenCheckList");
                var SearchClientType = Request.Form.Get("SearchClientType");
                var testt = Request.Form.Get("test");
                //var clnSMSSendArray = Request.Form.Get("SMSSendAry");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (SearchClientType != "")
                {
                    SearchClientTypeDDL = int.Parse(SearchClientType);
                }

                //if (YearID != "")
                //{
                //    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                //}
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (CheckedRealIpUser != null)
                {
                    IsCheckedRealIpUser = bool.Parse(CheckedRealIpUser);
                }
                //if (clnIfIsCheckAllThenNonCheckList != null)
                //{
                //    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(clnIfIsCheckAllThenNonCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}
                //if (clnIfNotCheckAllThenCheckList != null)
                //{
                //    IfNotCheckAllThenCheckLists = Array.ConvertAll(clnIfNotCheckAllThenCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}


                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }
                //if (clnSMSSendArray != null)
                //{
                //    SMSSendAry = Array.ConvertAll(clnSMSSendArray.ToArray(), c => (int)char.GetNumericValue(c));
                //}

                // Loading.   




                //System.IO.File.AppendAllText(@"" + path + "", "Test2>" + Environment.NewLine);

                #region Linq Client Details
                //// Apply pagination.   
                ////data = data.Skip(startRec).Take(pageSize).ToList();
                //var firstPart = (ZoneID == "") ? db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole).AsQueryable() : db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == zoneFromDDL).AsQueryable();

                ////db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == ((ZoneID == "")  ? 1: int.Parse(ZoneID)))
                //var secondpart = firstPart.AsEnumerable().GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //                     , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                //                         (ClientDetails, ClientLineStatus) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientLineStatus.FirstOrDefault()
                //                         })
                //                     .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
                //                         Transaction => Transaction.ClientDetailsID,
                //                         (ClientDetails, Transaction) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientDetails.ClientLineStatus,
                //                             Transaction = Transaction.FirstOrDefault()
                //                         }).AsEnumerable();



                //int ifSearch = 0;
                //if (!string.IsNullOrEmpty(search) &&
                //    !string.IsNullOrWhiteSpace(search))
                //{
                //    //    var test = secondpart.ToList();
                //    //var sec = secondpart.ToList();
                //    ifSearch = (secondpart.Any()) ? secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).Count() : 0;

                //    // Apply search   
                //    secondpart = secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) || 
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.Package.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                      (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //                       p.ClientDetails.ClientDetails.Zone.ToString().ToLower().Contains(search.ToLower()) || 
                //    //                       p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    //).ToList();
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).AsQueryable();
                //}

                //Session["IdListForSMSSend"] = secondpart.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //    Phone = s.ClientDetails.ClientDetails.ContactNumber
                //}).ToList();

                //List<ClientCustomInformation> data =
                //        secondpart.Any() ? secondpart.Skip(startRec).Take(pageSize)
                //         .Select(
                //             s => new ClientCustomInformation
                //             {
                //                 ClientDetailsID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //                 chkSMS = CheckOrNot(s.ClientDetails.ClientDetails.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),
                //                 //       chkSMS = "<div style='margin-left:12px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' name='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' onclick='enableDisableSMSSendOption(chk" + s.ClientDetails.ClientDetails.ClientDetailsID + ")'> <label for= 'chk" + s.ClientDetails.ClientDetails.ClientDetailsID + "'> </label ></div>",
                //                 //name="'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + " onclick='enableDisableSMSSendOption("'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + ")'
                //                 ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                //                 Name = s.ClientDetails.ClientDetails.Name,
                //                 LoginName = s.ClientDetails.ClientDetails.LoginName,
                //                 PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
                //                 PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                //                 Address = s.ClientDetails.ClientDetails.Address,
                //                 Email = s.ClientDetails.ClientDetails.Email,
                //                 Zone = s.ClientDetails.ClientDetails.Zone.ZoneName,
                //                 ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
                //                 StatusThisMonthID = s.Transaction != null ? s.Transaction.LineStatusID.ToString() : AppUtils.LineIsLock.ToString()/*s.ClientLineStatus.LineStatus.LineStatusID.ToString()*/,
                //                 StatusNextMonthID = s.ClientLineStatus.LineStatus.LineStatusID.ToString(),

                //                 IsPriorityClient = s.ClientDetails.ClientDetails.IsPriorityClient,
                //                 Show = " < a href = '' id = 'ShowPopUps' > Show </ a > ",
                //                 LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //                 //Button = "@if (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client)) { <button id='btnDelete' type='button' class='btn btn - danger btn - sm padding' data-placement='top' data-toggle='modal' data-target='#popModalForDeletePermently'> <span class='glyphicon glyphicon-remove'></span> </button> }",
                //                 Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client) ? true : false,

                //             }).ToList() : new List<ClientCustomInformation>();
                //// Verification.   

                //// Sorting.   
                //data = this.SortByColumnWithOrder(order, orderDir, data);
                //// Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //                                                     // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = data
                //}, JsonRequestBehavior.AllowGet);
                #endregion
                List<ClientCustomInformation> data = new List<ClientCustomInformation>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    string whichSP = "";

                    if (SearchClientTypeDDL == AppUtils.TableStatusIsDelete)
                    {
                        whichSP = "SP_SearchDeleteClient";
                    }
                    else
                    {
                        whichSP = "SP_SearchClient";
                    }

                    using (SqlCommand sqlCmd = new SqlCommand(whichSP, sqlConn))
                    {

                        //System.IO.File.AppendAllText(@"" + path + "", "Test2>DB:" + whichSP + "" + Environment.NewLine);

                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        //sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        sqlCmd.Parameters.Add("@SearchClientType", SearchClientTypeDDL.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        //sqlCmd.Parameters.Add("@endDate", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@IsCheckedRealIpUser", SqlDbType.Bit).Value = IsCheckedRealIpUser;
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.MyUser;
                        sqlCmd.Parameters.Add("@ResellerID", "".ToString());
                        sqlCmd.Parameters.Add("@Status", AppUtils.TableStatusIsDelete);

                        //SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientList);
                        //SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        //SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(testValue);

                        sqlConn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
                        {
                            DataSet ds = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            adp.Fill(ds); //get select list from temp table
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                ClientCustomInformation tec = new ClientCustomInformation();
                                SetClientOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                data.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";

                            recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[1].Rows[0]["totalUserCount"] : 0;
                            totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[1].Rows[0]["totalUserCount"] : 0;


                            //recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[0].Rows[0]["TotalCount"] : 0;
                            //totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[0].Rows[0]["TotalCount"] : 0;

                            //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                            //{
                            //    //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                            //    SendSMSCustomInformation sms = new SendSMSCustomInformation();
                            //    sms.ClientID = (int)ds.Tables[1].Rows[i]["ClientID"];
                            //    sms.Phone = ds.Tables[1].Rows[i]["Phone"].ToString();
                            //    lstSendSMSCustomInformation.Add(sms);
                            //}

                            //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                            //{
                            //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                            //    Phone = s.Transaction.ClientDetails.ContactNumber
                            //}).ToList();
                        }

                        sqlConn.Close();
                    }
                }
                stopwatch.Stop();
                var totalTimeRequired = stopwatch.Elapsed;
                //System.IO.File.AppendAllText(@"C:\WriteLines.txt", "single>" + stopwatch.Elapsed.TotalMilliseconds.ToString() + " > " + Environment.NewLine);
                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? totalRecords : totalRecords;
                ////////////////////////////////////

                //System.IO.File.AppendAllText(@"" + path + "", "single>" + Environment.NewLine);


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {



                //var path = Server.MapPath("~/Log/Logss.txt");
                //System.IO.File.AppendAllText(@"" + path + "", "Test>" + Environment.NewLine);

                var a = ex.InnerException;
                // Info   
                result = this.Json(new
                {
                    data = ex
                }, JsonRequestBehavior.AllowGet);
                Console.Write(ex.InnerException);


                //var path = Server.MapPath("~/Log/Logss.txt");
                //System.IO.File.AppendAllText(@"" + path + "", "singless>" + ex.InnerException + " > " + Environment.NewLine);

            }
            // Return info.   
            return result;
        }




        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Client_List_By_Reseller)]
        public ActionResult GetAllResellerClients()
        {
            int resellerID = AppUtils.GetLoginUserID();
            //var demoActionData =
            //    db.Form.
            //    Join(db.Action.Where(s => s.ShowingStatus == 1 && s.ActionDescription != ""),
            //        Form => Form.FormID,
            //        Action => Action.FormID,
            //        (Form, Action) => new { Form = Form, Action = Action })
            //   .Join(
            //        db.ControllerName,
            //        Forms => Forms.Form.ControllerNameID,
            //        Controller => Controller.ControllerNameID,
            //        (Forms, Controller) => new { Forms = Forms, Action = Forms.Action, Controller = Controller })

            //        .OrderBy(s => s.Controller.ControllerNameID).ThenBy(s => s.Forms.Form.FormID).AsEnumerable().Select(
            //        s => new
            //        {
            //            MyButtonInfo = "public const string " + s.Controller.ControllerNames + "_" + s.Forms.Form.FormName + "_" + s.Action.ActionDescription.Trim().Replace(" ", "__") + " = \"" + s.Action.ActionValue + "\";"

            //            // Action = 
            //        }
            //        );
            LoadViewBag(resellerID);
            Reseller reseller = db.Reseller.Find(resellerID);
            List<int> lstResellerPackage = (reseller.macReselleGivenPackageWithPrice != null) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice).Select(x => x.PID).ToList() : new List<int>();

            int PackageForResellerUser = int.Parse(AppUtils.PackageForResellerUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForResellerUser && lstResellerPackage.Contains(x.PackageID))
                .Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            //var test = db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole)
            //    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //        (ClientDetails, ClientLineStatus) => new
            //        {
            //            ClientDetails = ClientDetails,
            //            ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //        });

            //List<ClientCustomInformation> lstClientCustomInformation =
            //    db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole)
            //        .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //            (ClientDetails, ClientLineStatus) => new
            //            {
            //                ClientDetails = ClientDetails,
            //                ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //            })
            //        .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
            //            Transaction => Transaction.ClientDetailsID,
            //            (ClientDetails, Transaction) => new
            //            {
            //                ClientDetails = ClientDetails,
            //                ClientLineStatus = ClientDetails.ClientLineStatus,
            //                Transaction = Transaction.FirstOrDefault()
            //            }).AsEnumerable()
            //            .Select(
            //        s => new ClientCustomInformation
            //        {
            //            ClientDetailsID = s.ClientDetails.ClientDetails.ClientDetailsID,
            //            ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
            //            Name = s.ClientDetails.ClientDetails.Name,
            //            LoginName = s.ClientDetails.ClientDetails.LoginName,
            //            PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
            //            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
            //            Address = s.ClientDetails.ClientDetails.Address,
            //            Email = s.ClientDetails.ClientDetails.Email,
            //            Zone = s.ClientDetails.ClientDetails.Zone.ZoneName,
            //            ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
            //            StatusThisMonthID = s.Transaction != null ? s.Transaction.LineStatusID.ToString() : s.ClientLineStatus.LineStatus.LineStatusID.ToString(),
            //            StatusNextMonthID = s.ClientLineStatus.LineStatus.LineStatusID.ToString(),

            //        }).ToList();


            //return View(lstClientCustomInformation);

            return View(new List<ClientCustomInformation>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllResellerClientsAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                int SearchClientTypeDDL = 0;
                bool isCheckAllFromCln = false;
                bool IsCheckedRealIpUser = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };
                int[] SMSSendAry = new int[] { };
                // Initialization.   
                var SearchClientType = Request.Form.Get("SearchClientType");
                var ZoneID = Request.Form.Get("ZoneID");
                var IsCheckAll = Request.Form.Get("IsCheckAll");
                var CheckedRealIpUser = Request.Form.Get("checkedRealIpUser");
                //var clnIfIsCheckAllThenNonCheckList = Request.Form.Get("IfIsCheckAllThenNonCheckList");
                //var clnIfNotCheckAllThenCheckList = Request.Form.Get("IfNotCheckAllThenCheckList");
                var SearchTypeForAll = Request.Form.Get("SearchTypeForAll");
                var testt = Request.Form.Get("test");
                //var clnSMSSendArray = Request.Form.Get("SMSSendAry");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (SearchClientType != "")
                {
                    SearchClientTypeDDL = int.Parse(SearchClientType);
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (CheckedRealIpUser != null)
                {
                    IsCheckedRealIpUser = bool.Parse(CheckedRealIpUser);
                }
                //if (clnIfIsCheckAllThenNonCheckList != null)
                //{
                //    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(clnIfIsCheckAllThenNonCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}
                //if (clnIfNotCheckAllThenCheckList != null)
                //{
                //    IfNotCheckAllThenCheckLists = Array.ConvertAll(clnIfNotCheckAllThenCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}


                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }
                //if (clnSMSSendArray != null)
                //{
                //    SMSSendAry = Array.ConvertAll(clnSMSSendArray.ToArray(), c => (int)char.GetNumericValue(c));
                //}

                // Loading.   

                #region Linq Client Details
                //// Apply pagination.   
                ////data = data.Skip(startRec).Take(pageSize).ToList();
                //var firstPart = (ZoneID == "") ? db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole).AsQueryable() : db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == zoneFromDDL).AsQueryable();

                ////db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == ((ZoneID == "")  ? 1: int.Parse(ZoneID)))
                //var secondpart = firstPart.AsEnumerable().GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //                     , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                //                         (ClientDetails, ClientLineStatus) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientLineStatus.FirstOrDefault()
                //                         })
                //                     .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
                //                         Transaction => Transaction.ClientDetailsID,
                //                         (ClientDetails, Transaction) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientDetails.ClientLineStatus,
                //                             Transaction = Transaction.FirstOrDefault()
                //                         }).AsEnumerable();



                //int ifSearch = 0;
                //if (!string.IsNullOrEmpty(search) &&
                //    !string.IsNullOrWhiteSpace(search))
                //{
                //    //    var test = secondpart.ToList();
                //    //var sec = secondpart.ToList();
                //    ifSearch = (secondpart.Any()) ? secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).Count() : 0;

                //    // Apply search   
                //    secondpart = secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) || 
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.Package.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                      (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //                       p.ClientDetails.ClientDetails.Zone.ToString().ToLower().Contains(search.ToLower()) || 
                //    //                       p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    //).ToList();
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).AsQueryable();
                //}

                //Session["IdListForSMSSend"] = secondpart.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //    Phone = s.ClientDetails.ClientDetails.ContactNumber
                //}).ToList();

                //List<ClientCustomInformation> data =
                //        secondpart.Any() ? secondpart.Skip(startRec).Take(pageSize)
                //         .Select(
                //             s => new ClientCustomInformation
                //             {
                //                 ClientDetailsID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //                 chkSMS = CheckOrNot(s.ClientDetails.ClientDetails.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),
                //                 //       chkSMS = "<div style='margin-left:12px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' name='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' onclick='enableDisableSMSSendOption(chk" + s.ClientDetails.ClientDetails.ClientDetailsID + ")'> <label for= 'chk" + s.ClientDetails.ClientDetails.ClientDetailsID + "'> </label ></div>",
                //                 //name="'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + " onclick='enableDisableSMSSendOption("'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + ")'
                //                 ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                //                 Name = s.ClientDetails.ClientDetails.Name,
                //                 LoginName = s.ClientDetails.ClientDetails.LoginName,
                //                 PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
                //                 PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                //                 Address = s.ClientDetails.ClientDetails.Address,
                //                 Email = s.ClientDetails.ClientDetails.Email,
                //                 Zone = s.ClientDetails.ClientDetails.Zone.ZoneName,
                //                 ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
                //                 StatusThisMonthID = s.Transaction != null ? s.Transaction.LineStatusID.ToString() : AppUtils.LineIsLock.ToString()/*s.ClientLineStatus.LineStatus.LineStatusID.ToString()*/,
                //                 StatusNextMonthID = s.ClientLineStatus.LineStatus.LineStatusID.ToString(),

                //                 IsPriorityClient = s.ClientDetails.ClientDetails.IsPriorityClient,
                //                 Show = " < a href = '' id = 'ShowPopUps' > Show </ a > ",
                //                 LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //                 //Button = "@if (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client)) { <button id='btnDelete' type='button' class='btn btn - danger btn - sm padding' data-placement='top' data-toggle='modal' data-target='#popModalForDeletePermently'> <span class='glyphicon glyphicon-remove'></span> </button> }",
                //                 Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client) ? true : false,

                //             }).ToList() : new List<ClientCustomInformation>();
                //// Verification.   

                //// Sorting.   
                //data = this.SortByColumnWithOrder(order, orderDir, data);
                //// Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //                                                     // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = data
                //}, JsonRequestBehavior.AllowGet);
                #endregion
                List<ClientCustomInformation> data = new List<ClientCustomInformation>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    string whichSP = "";
                    if (SearchClientTypeDDL == AppUtils.TableStatusIsDelete)
                    {
                        whichSP = "SP_SearchDeleteClient";
                    }
                    else
                    {
                        whichSP = "SP_SearchClient";
                    }
                    using (SqlCommand sqlCmd = new SqlCommand(whichSP, sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());

                        sqlCmd.Parameters.Add("@SearchClientType", SearchClientTypeDDL.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@IsCheckedRealIpUser", SqlDbType.Bit).Value = IsCheckedRealIpUser;
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                        string resellerID = AppUtils.GetLoginUserID().ToString();
                        sqlCmd.Parameters.Add("@ResellerID", resellerID);
                        sqlCmd.Parameters.Add("@Status", AppUtils.TableStatusIsDelete);

                        //SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientList);
                        //SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        //SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(testValue);

                        sqlConn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
                        {
                            DataSet ds = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            adp.Fill(ds); //get select list from temp table
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                ClientCustomInformation tec = new ClientCustomInformation();
                                SetClientOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                data.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";

                            recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[2].Rows[0]["totalUserCount"] : 0;
                            totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[2].Rows[0]["totalUserCount"] : 0;

                            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                SendSMSCustomInformation sms = new SendSMSCustomInformation();
                                sms.ClientID = (int)ds.Tables[1].Rows[i]["ClientID"];
                                sms.Phone = ds.Tables[1].Rows[i]["Phone"].ToString();
                                lstSendSMSCustomInformation.Add(sms);
                            }

                            //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                            //{
                            //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                            //    Phone = s.Transaction.ClientDetails.ContactNumber
                            //}).ToList();
                        }

                        sqlConn.Close();
                    }
                }



                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? totalRecords : totalRecords;
                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
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


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Client_List_For_Reseller_By_Admin)]
        public ActionResult GetAllResellerClientsByAdmin()
        {

            LoadViewBag();
            ViewBag.PackageThisMonth = new SelectList(new List<SelectListItem>(), "Text", "Value");
            ViewBag.PackageNextMonth = new SelectList(new List<SelectListItem>(), "Text", "Value");

            return View(new List<ClientCustomInformation>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllResellerClientsByAdminAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                int resellerFromDDL = 0;
                int SearchClientTypeDDL = 0;
                bool isCheckAllFromCln = false;
                bool IsCheckedRealIpUser = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };
                int[] SMSSendAry = new int[] { };
                // Initialization.   
                var SearchClientType = Request.Form.Get("SearchClientType");
                var ResellerID = Request.Form.Get("ResellerID");
                var ZoneID = Request.Form.Get("ZoneID");
                var IsCheckAll = Request.Form.Get("IsCheckAll");
                var CheckedRealIpUser = Request.Form.Get("checkedRealIpUser");
                //var clnIfIsCheckAllThenNonCheckList = Request.Form.Get("IfIsCheckAllThenNonCheckList");
                //var clnIfNotCheckAllThenCheckList = Request.Form.Get("IfNotCheckAllThenCheckList");
                var SearchTypeForAll = Request.Form.Get("SearchTypeForAll");
                var testt = Request.Form.Get("test");
                //var clnSMSSendArray = Request.Form.Get("SMSSendAry");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (SearchClientType != "")
                {
                    SearchClientTypeDDL = int.Parse(SearchClientType);
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (ResellerID != "")
                {
                    resellerFromDDL = int.Parse(ResellerID);
                }
                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (CheckedRealIpUser != null)
                {
                    IsCheckedRealIpUser = bool.Parse(CheckedRealIpUser);
                }
                //if (clnIfIsCheckAllThenNonCheckList != null)
                //{
                //    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(clnIfIsCheckAllThenNonCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}
                //if (clnIfNotCheckAllThenCheckList != null)
                //{
                //    IfNotCheckAllThenCheckLists = Array.ConvertAll(clnIfNotCheckAllThenCheckList.ToArray(), c => (int)char.GetNumericValue(c));
                //}


                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }
                //if (clnSMSSendArray != null)
                //{
                //    SMSSendAry = Array.ConvertAll(clnSMSSendArray.ToArray(), c => (int)char.GetNumericValue(c));
                //}

                // Loading.   

                #region Linq Client Details
                //// Apply pagination.   
                ////data = data.Skip(startRec).Take(pageSize).ToList();
                //var firstPart = (ZoneID == "") ? db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole).AsQueryable() : db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == zoneFromDDL).AsQueryable();

                ////db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == ((ZoneID == "")  ? 1: int.Parse(ZoneID)))
                //var secondpart = firstPart.AsEnumerable().GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //                     , ClientDetails => ClientDetails.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                //                         (ClientDetails, ClientLineStatus) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientLineStatus.FirstOrDefault()
                //                         })
                //                     .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth), ClientDetails => ClientDetails.ClientDetails.ClientDetailsID,
                //                         Transaction => Transaction.ClientDetailsID,
                //                         (ClientDetails, Transaction) => new
                //                         {
                //                             ClientDetails = ClientDetails,
                //                             ClientLineStatus = ClientDetails.ClientLineStatus,
                //                             Transaction = Transaction.FirstOrDefault()
                //                         }).AsEnumerable();



                //int ifSearch = 0;
                //if (!string.IsNullOrEmpty(search) &&
                //    !string.IsNullOrWhiteSpace(search))
                //{
                //    //    var test = secondpart.ToList();
                //    //var sec = secondpart.ToList();
                //    ifSearch = (secondpart.Any()) ? secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).Count() : 0;

                //    // Apply search   
                //    secondpart = secondpart.Where(p =>
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) || 
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.Package.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                      (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //                       p.ClientDetails.ClientDetails.Zone.ToString().ToLower().Contains(search.ToLower()) || 
                //    //                       p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //    //                       p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    //).ToList();
                //    //p.ClientDetails.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    //p.ClientLineStatus.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientDetails.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    //(p.Transaction != null ? p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                //    //p.ClientDetails.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.ClientDetails.ClientDetails.Email) ? p.ClientDetails.ClientDetails.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.ClientDetails.ClientDetails.Zone != null ? p.ClientDetails.ClientDetails.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ClientDetails.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //    //(p.Transaction != null ? p.Transaction.LineStatus.LineStatusName.ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    //p.ClientLineStatus.LineStatus.LineStatusName.ToString().ToLower().Contains(search.ToLower())
                //    ).AsQueryable();
                //}

                //Session["IdListForSMSSend"] = secondpart.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //    Phone = s.ClientDetails.ClientDetails.ContactNumber
                //}).ToList();

                //List<ClientCustomInformation> data =
                //        secondpart.Any() ? secondpart.Skip(startRec).Take(pageSize)
                //         .Select(
                //             s => new ClientCustomInformation
                //             {
                //                 ClientDetailsID = s.ClientDetails.ClientDetails.ClientDetailsID,
                //                 chkSMS = CheckOrNot(s.ClientDetails.ClientDetails.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),
                //                 //       chkSMS = "<div style='margin-left:12px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' name='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' onclick='enableDisableSMSSendOption(chk" + s.ClientDetails.ClientDetails.ClientDetailsID + ")'> <label for= 'chk" + s.ClientDetails.ClientDetails.ClientDetailsID + "'> </label ></div>",
                //                 //name="'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + " onclick='enableDisableSMSSendOption("'chk' + s.ClientDetails.ClientDetails.ClientDetailsID + ")'
                //                 ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                //                 Name = s.ClientDetails.ClientDetails.Name,
                //                 LoginName = s.ClientDetails.ClientDetails.LoginName,
                //                 PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
                //                 PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                //                 Address = s.ClientDetails.ClientDetails.Address,
                //                 Email = s.ClientDetails.ClientDetails.Email,
                //                 Zone = s.ClientDetails.ClientDetails.Zone.ZoneName,
                //                 ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
                //                 StatusThisMonthID = s.Transaction != null ? s.Transaction.LineStatusID.ToString() : AppUtils.LineIsLock.ToString()/*s.ClientLineStatus.LineStatus.LineStatusID.ToString()*/,
                //                 StatusNextMonthID = s.ClientLineStatus.LineStatus.LineStatusID.ToString(),

                //                 IsPriorityClient = s.ClientDetails.ClientDetails.IsPriorityClient,
                //                 Show = " < a href = '' id = 'ShowPopUps' > Show </ a > ",
                //                 LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //                 //Button = "@if (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client)) { <button id='btnDelete' type='button' class='btn btn - danger btn - sm padding' data-placement='top' data-toggle='modal' data-target='#popModalForDeletePermently'> <span class='glyphicon glyphicon-remove'></span> </button> }",
                //                 Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client) ? true : false,

                //             }).ToList() : new List<ClientCustomInformation>();
                //// Verification.   

                //// Sorting.   
                //data = this.SortByColumnWithOrder(order, orderDir, data);
                //// Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //                                                     // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = data
                //}, JsonRequestBehavior.AllowGet);
                #endregion
                List<ClientCustomInformation> data = new List<ClientCustomInformation>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    string whichSP = "";
                    if (SearchClientTypeDDL == AppUtils.TableStatusIsDelete)
                    {
                        whichSP = "SP_SearchDeleteClient";
                    }
                    else
                    {
                        whichSP = "SP_SearchClient";
                    }
                    using (SqlCommand sqlCmd = new SqlCommand(whichSP, sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());

                        sqlCmd.Parameters.Add("@SearchClientType", SearchClientTypeDDL.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@IsCheckedRealIpUser", SqlDbType.Bit).Value = IsCheckedRealIpUser;
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                        sqlCmd.Parameters.Add("@ResellerID", resellerFromDDL);
                        sqlCmd.Parameters.Add("@Status", AppUtils.TableStatusIsDelete);

                        //SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientList);
                        //SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        //SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(testValue);

                        sqlConn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
                        {
                            DataSet ds = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            adp.Fill(ds); //get select list from temp table
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                ClientCustomInformation tec = new ClientCustomInformation();
                                SetClientOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                data.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";

                            recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[2].Rows[0]["totalUserCount"] : 0;
                            totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[2].Rows[0]["totalUserCount"] : 0;

                            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                SendSMSCustomInformation sms = new SendSMSCustomInformation();
                                sms.ClientID = (int)ds.Tables[1].Rows[i]["ClientID"];
                                sms.Phone = ds.Tables[1].Rows[i]["Phone"].ToString();
                                lstSendSMSCustomInformation.Add(sms);
                            }

                            //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                            //{
                            //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                            //    Phone = s.Transaction.ClientDetails.ContactNumber
                            //}).ToList();
                        }

                        sqlConn.Close();
                    }
                }



                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? totalRecords : totalRecords;
                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
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



        private void SetClientOneByOneInTheList(DataSet ds, ref ClientCustomInformation tec, int i, bool isCheckAllFromCln, int[] IfIsCheckAllThenNonCheckLists, int[] IfNotCheckAllThenCheckLists)
        {
            tec.ClientDetailsID = (int)ds.Tables[0].Rows[i]["ClientDetailsID"];
            tec.chkSMS = CheckOrNot((int)ds.Tables[0].Rows[i]["ClientDetailsID"], isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
            tec.ClientLineStatusID = (int)ds.Tables[0].Rows[i]["ClientLineStatusID"];
            tec.Name = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Name"].ToString()) ? "" : ds.Tables[0].Rows[i]["Name"].ToString();
            tec.LoginName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LoginName"].ToString()) ? "" : ds.Tables[0].Rows[i]["LoginName"].ToString();
            //tec.UserID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserID"].ToString()) ? "" : ds.Tables[0].Rows[i]["UserID"].ToString();
            tec.Address = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Address"].ToString()) ? "" : ds.Tables[0].Rows[i]["Address"].ToString();
            tec.LatitudeLongitude = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LatitudeLongitude"].ToString()) ? "" : ds.Tables[0].Rows[i]["LatitudeLongitude"].ToString();
            tec.Email = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Email"].ToString()) ? "" : ds.Tables[0].Rows[i]["Email"].ToString();
            tec.Zone = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Zone"].ToString()) ? "" : ds.Tables[0].Rows[i]["Zone"].ToString();
            tec.ContactNumber = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ContactNumber"].ToString()) ? "" : ds.Tables[0].Rows[i]["ContactNumber"].ToString();

            //tec.TransactionID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TransactionID"].ToString()) ? "" : ds.Tables[0].Rows[i]["TransactionID"].ToString();

            tec.StatusThisMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString();
            tec.StatusNextMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString();
            //tec.PackageNameThisMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TransactionID"].ToString()) ? ds.Tables[0].Rows[i]["PackageThisMonthIDFromTransaction"].ToString() : ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString();
            tec.PackageNameThisMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString();
            tec.PackageNameNextMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString();

            tec.IsPriorityClient = ds.Tables[0].Rows[i]["IsPriorityClient"].ToString() == "False" ? false : true;
            tec.Show = " <a href = '' id = 'ShowPopUps' > Show </a> ";
            tec.LineStatusActiveDate = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString()) ? "" : ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString();
            //Button = "@if (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client)) { <button id='btnDelete' type='button' class='btn btn - danger btn - sm padding' data-placement='top' data-toggle='modal' data-target='#popModalForDeletePermently'> <span class='glyphicon glyphicon-remove'></span> </button> }",
            tec.Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client) ? ds.Tables[0].Rows[i]["Status"].ToString() != "7" ? true : false : false;
            tec.ProfileStatusUpdateInPercent = GetProfileUpdatePercent(Convert.ToDouble(ds.Tables[0].Rows[i]["ProfileStatusUpdateInPercent"].ToString()), tec.ClientDetailsID);

            tec.PermanentDiscount = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PermanentDiscount"].ToString()) ? 0 : (double)ds.Tables[0].Rows[i]["PermanentDiscount"];


        }

        private void SetClientOneByOneInTheListForActiveOrLockClient(DataSet ds, ref ClientCustomInformation tec, int i, List<LineStatus> lstLineStatus)
        {

            tec.ClientDetailsID = (int)ds.Tables[0].Rows[i]["ClientDetailsID"];
            tec.chkSMS = "";
            tec.ClientLineStatusID = (int)ds.Tables[0].Rows[i]["ClientLineStatusID"];
            //tec.ClientLineStatusName = s.ClientLineStatus.LineStatus.LineStatusName;
            tec.Name = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Name"].ToString()) ? "" : ds.Tables[0].Rows[i]["Name"].ToString();
            tec.LoginName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LoginName"].ToString()) ? "" : ds.Tables[0].Rows[i]["LoginName"].ToString();
            tec.PackageName = "";// s.Transaction.Package.PackageName;
            tec.PackageNameThisMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString();
            tec.PackageNameNextMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString();
            tec.Address = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Address"].ToString()) ? "" : ds.Tables[0].Rows[i]["Address"].ToString();
            tec.Email = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Email"].ToString()) ? "" : ds.Tables[0].Rows[i]["Email"].ToString();
            tec.Zone = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Zone"].ToString()) ? "" : ds.Tables[0].Rows[i]["Zone"].ToString();
            tec.ContactNumber = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ContactNumber"].ToString()) ? "" : ds.Tables[0].Rows[i]["ContactNumber"].ToString();
            tec.StatusThisMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString();
            tec.StatusNextMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString();
            tec.StatusThisMonthName = !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString()) ? lstLineStatus.Where(x => x.LineStatusID == int.Parse(ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString())).FirstOrDefault().LineStatusName : "";
            tec.StatusNextMonthName = !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString()) ? lstLineStatus.Where(x => x.LineStatusID == int.Parse(ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString())).FirstOrDefault().LineStatusName : "";

            tec.LineStatusActiveDate = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString()) ? "" : ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString();
            tec.IsPriorityClient = ds.Tables[0].Rows[i]["IsPriorityClient"].ToString() == "False" ? false : true;

            //tec.ClientDetailsID = (int)ds.Tables[0].Rows[i]["ClientDetailsID"];
            //tec.chkSMS = "";//(int)ds.Tables[0].Rows[i]["ClientDetailsID"];// CheckOrNot(, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
            //tec.ClientLineStatusID = (int)ds.Tables[0].Rows[i]["ClientLineStatusID"];
            //tec.Name = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Name"].ToString()) ? "" : ds.Tables[0].Rows[i]["Name"].ToString();
            //tec.LoginName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LoginName"].ToString()) ? "" : ds.Tables[0].Rows[i]["LoginName"].ToString();
            ////tec.UserID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserID"].ToString()) ? "" : ds.Tables[0].Rows[i]["UserID"].ToString();
            //tec.Address = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Address"].ToString()) ? "" : ds.Tables[0].Rows[i]["Address"].ToString();
            //tec.Email = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Email"].ToString()) ? "" : ds.Tables[0].Rows[i]["Email"].ToString();
            //tec.Zone = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Zone"].ToString()) ? "" : ds.Tables[0].Rows[i]["Zone"].ToString();
            //tec.ContactNumber = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ContactNumber"].ToString()) ? "" : ds.Tables[0].Rows[i]["ContactNumber"].ToString();

            ////tec.TransactionID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TransactionID"].ToString()) ? "" : ds.Tables[0].Rows[i]["TransactionID"].ToString();

            //tec.StatusThisMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusThisMonthID"].ToString();
            //tec.StatusNextMonthID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString()) ? "" : ds.Tables[0].Rows[i]["StatusNextMonthID"].ToString();
            ////tec.PackageNameThisMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TransactionID"].ToString()) ? ds.Tables[0].Rows[i]["PackageThisMonthIDFromTransaction"].ToString() : ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString();
            //tec.PackageNameThisMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameThisMonth"].ToString();
            //tec.PackageNameNextMonth = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageNameNextMonth"].ToString();

            //tec.IsPriorityClient = ds.Tables[0].Rows[i]["IsPriorityClient"].ToString() == "False" ? false : true;
            //tec.Show = " <a href = '' id = 'ShowPopUps' > Show </a> ";
            //tec.LineStatusActiveDate = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString()) ? "" : ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString();
            ////Button = "@if (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client)) { <button id='btnDelete' type='button' class='btn btn - danger btn - sm padding' data-placement='top' data-toggle='modal' data-target='#popModalForDeletePermently'> <span class='glyphicon glyphicon-remove'></span> </button> }",
            //tec.Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Client) ? ds.Tables[0].Rows[i]["Status"].ToString() != "7" ? true : false : false;
            //tec.ProfileStatusUpdateInPercent = GetProfileUpdatePercent(Convert.ToDouble(ds.Tables[0].Rows[i]["ProfileStatusUpdateInPercent"].ToString()), tec.ClientDetailsID);

            //tec.PermanentDiscount = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PermanentDiscount"].ToString()) ? 0 : (double)ds.Tables[0].Rows[i]["PermanentDiscount"];


        }

        private string CheckOrNot(int clnID, bool isCheckAllFromCln/*,int[] SMSSendArray*/, int[] IfIsCheckAllThenNonCheckList, int[] IfNotCheckAllThenCheckList)
        {
            bool chkBoxCheck = false;
            var chk = "";
            if (isCheckAllFromCln)
            {
                if (IfIsCheckAllThenNonCheckList.Contains(clnID))
                // if (SMSSendArray.Contains(clnID))
                {
                    chkBoxCheck = false;
                }
                else
                {
                    chkBoxCheck = true;
                    chk = "checked";
                    //checked=" + chkBoxCheck + "
                }

            }
            else
            {
                if (IfNotCheckAllThenCheckList.Contains(clnID))
                // if (SMSSendArray.Contains(clnID))
                {
                    chkBoxCheck = true;
                    chk = "checked";
                }
                else
                {
                    chkBoxCheck = false;
                }
            }
            return "<div style='margin-left:1px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + clnID + "' name='chkSMS" + clnID + "' onclick='enableDisableSMSSendOption(chkSMS" + clnID + "," + clnID + ")' " + chk + " > <label for= 'chkSMS" + clnID + "'> </label ></div>";
            //"<div style='margin-left:1px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' name='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' onclick='enableDisableSMSSendOption(chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "," + s.ClientDetails.ClientDetails.ClientDetailsID + ")' checked="+checkOrNot+"> <label for= 'chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "'> </label ></div>"
        }

        private List<ClientCustomInformation> SortByColumnWithOrder(string order, string orderDir, List<ClientCustomInformation> data)
        {
            // Initialization.   
            List<ClientCustomInformation> lst = new List<ClientCustomInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientDetailsID).ToList() : data.OrderBy(p => p.ClientDetailsID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLineStatusID).ToList() : data.OrderBy(p => p.ClientLineStatusID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LoginName).ToList() : data.OrderBy(p => p.LoginName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageNameThisMonth).ToList() : data.OrderBy(p => p.PackageNameThisMonth).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageNameNextMonth).ToList() : data.OrderBy(p => p.PackageNameNextMonth).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Zone).ToList() : data.OrderBy(p => p.Zone).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StatusThisMonthID).ToList() : data.OrderBy(p => p.StatusThisMonthID).ToList();
                        break;
                    case "11":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StatusNextMonthID).ToList() : data.OrderBy(p => p.StatusNextMonthID).ToList();
                        break;


                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientDetailsID).ToList() : data.OrderBy(p => p.ClientDetailsID).ToList();
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


        private string GetLineStatusName(int? lineStatusID)
        {
            if (lineStatusID == AppUtils.LineIsActive)
            {
                return "Active";
            }

            if (lineStatusID == AppUtils.LineIsLock)
            {
                return "Lock";
            }
            return "";
        }

        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.View_Lock_CLient)]
        public ActionResult GetAllLockCLients()
        {
            LoadViewBag();

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            //List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
            ///// this are the clients from transaction who is Active
            //List<int> lstActiveClients = db.Transaction
            //    .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList();
            ////// Now i am searching for the Lock client. this are the final active clients cause bill is generate or sign up in this month
            //lstClientCustomInformation = db.Transaction
            //    .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstActiveClients.Contains(s.ClientDetailsID))
            //    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
            //    Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
            //    {
            //        Transaction = Transaction,
            //        ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //    })
            //    .Select(
            //    s => new ClientCustomInformation()
            //    {
            //        ClientDetailsID = s.Transaction.ClientDetailsID,
            //        Name = s.Transaction.ClientDetails.Name,
            //        LoginName = s.Transaction.ClientDetails.LoginName,
            //        PackageName = s.Transaction.Package.PackageName,
            //        PackageNameThisMonth = s.Transaction.Package.PackageName,
            //        PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
            //        Address = s.Transaction.ClientDetails.Address,
            //        Email = s.Transaction.ClientDetails.Email,
            //        Zone = s.Transaction.ClientDetails.Zone.ZoneName,
            //        ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //        StatusThisMonthID = "5",
            //        StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
            //    }).ToList();

            //if (lstActiveClients.Count > 0 || lstClientCustomInformation.Count > 0)
            //{
            //    return View(lstClientCustomInformation);

            //}
            ////////////////////// if No Information found in Lock list and active list in accounts there is a possibilities that bill is not generate so then we have 
            ///// to check the latest status from the clientLineStatus table this are the approximate active client list in this month. :D

            //else
            //{
            //    lstClientCustomInformation = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock)

            //                    .Select(s => new ClientCustomInformation
            //                    {
            //                        ClientDetailsID = s.ClientDetails.ClientDetailsID,
            //                        Name = s.ClientDetails.Name,
            //                        LoginName = s.ClientDetails.LoginName,
            //                        PackageName = s.Package.PackageName,
            //                        PackageNameThisMonth = s.Package.PackageName,
            //                        PackageNameNextMonth = s.Package.PackageName,
            //                        Address = s.ClientDetails.Address,
            //                        Email = s.ClientDetails.Email,
            //                        Zone = s.ClientDetails.Zone.ZoneName,
            //                        ContactNumber = s.ClientDetails.ContactNumber,
            //                        StatusThisMonthID = "5",
            //                        StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

            //                    }).ToList();

            return View(new List<ClientCustomInformation>());
            //}
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllLockClientsAJAXData()
        {


            // Initialization.   
            List<ClientCustomInformation> data = new List<ClientCustomInformation>();
            JsonResult result = new JsonResult();
            //      AppUtils.RunningMonth = 9;
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                //DateTime systemFirstDate = Convert.ToDateTime(ConfigurationManager.AppSettings["SystemFirstDate"]);
                //DateTime FromDateFromDDL = systemFirstDate;
                //DateTime ToDateFromDDL = Convert.ToDateTime(DateTime.Now).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);

                string FromDateFromDDL = "";
                string ToDateFromDDL = "";
                // Initialization.   
                var ZoneID = Request.Form.Get("ZoneID");
                var FromDate = Request.Form.Get("FromDate");
                var ToDate = Request.Form.Get("ToDate");
                var SearchTypeForActive = Request.Form.Get("SearchTypeForActive");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FromDateFromDDL = Convert.ToDateTime(FromDate).ToString();
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    ToDateFromDDL = Convert.ToDateTime(ToDate).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59).ToString();
                }



                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    string whichSP = "";

                    //if (SearchClientTypeDDL == AppUtils.TableStatusIsDelete)
                    //{
                    //    whichSP = "SP_SearchDeleteClient";
                    //}
                    //else
                    //{
                    whichSP = "SP_ActiveSearchClient";
                    //}

                    using (SqlCommand sqlCmd = new SqlCommand(whichSP, sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        //sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        //sqlCmd.Parameters.Add("@SearchClientType", SearchClientTypeDDL.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@startDate", FromDateFromDDL == null ? "" : FromDateFromDDL);
                        sqlCmd.Parameters.Add("@endDate", FromDateFromDDL == null ? "" : FromDateFromDDL);
                        //sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        //sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        //sqlCmd.Parameters.Add("@endDate", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@IsCheckedRealIpUser", SqlDbType.Bit).Value = false;
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.MyUser;
                        sqlCmd.Parameters.Add("@ResellerID", "".ToString());
                        sqlCmd.Parameters.Add("@Status", AppUtils.TableStatusIsDelete);

                        //SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientList);
                        //SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        //SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        //sqlCmd.Parameters.Add(testValue);

                        sqlConn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
                        {
                            DataSet ds = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            adp.Fill(ds); //get select list from temp table
                            List<LineStatus> lineStatuses = db.LineStatus.ToList();
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                var i1 = ds.Tables[0].Rows[i][0].ToString();
                                ClientCustomInformation tec = new ClientCustomInformation();
                                SetClientOneByOneInTheListForActiveOrLockClient(ds, ref tec, i, lineStatuses);
                                data.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";
                            System.IO.File.AppendAllText(@"C:\WriteLines.txt", "Lock Client:Linq: >" + watch.Elapsed.TotalMilliseconds.ToString() + " > " + Environment.NewLine);

                            recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[1].Rows[0]["totalUserCount"] : 0;
                            totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[1].Rows[0]["totalUserCount"] : 0;


                            sqlConn.Close();
                        }
                    }







                    //IQueryable<ClientCustomInformation> lstIQuerable = Enumerable.Empty<ClientCustomInformation>().AsQueryable();
                    //List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
                    ///// this are the clients from transaction who is active
                    //List<int> lstActiveClients = (ZoneID == "") ?
                    //    db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList()
                    //    : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList();

                    ////now this is the list of client who is lock. 
                    //List<int> lstOfClientByZoneOrNot = new List<int>();
                    //if (ZoneID != "")
                    //{
                    //    lstOfClientByZoneOrNot = db.ClientDetails.Where(s => s.ZoneID == int.Parse(ZoneID) && s.IsNewClient != AppUtils.isNewClient).Select(s => s.ClientDetailsID).ToList();
                    //    lstOfClientByZoneOrNot.RemoveAll(x => lstActiveClients.Contains(x));
                    //}
                    //else
                    //{
                    //    lstOfClientByZoneOrNot = db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).Select(s => s.ClientDetailsID).ToList();
                    //    lstOfClientByZoneOrNot.RemoveAll(x => lstActiveClients.Contains(x));
                    //}
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    ////// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
                    //var firstPartOfQuery =
                    //    db.ClientLineStatus.Where(s => lstOfClientByZoneOrNot.Contains(s.ClientDetailsID)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).AsQueryable();
                    ////var a = firstPartOfQuery.ToList();
                    //var secondPartOfQuery = firstPartOfQuery
                    //    .GroupJoin(
                    //        db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth),
                    //        ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID,
                    //        (ClientLineStatus, Transaction) => new
                    //        {
                    //            Transaction = Transaction.FirstOrDefault(),
                    //            ClientLineStatus = ClientLineStatus
                    //        }).AsQueryable();


                    //if (lstActiveClients.Count > 0 || secondPartOfQuery.Count() > 0)//(lstLockClients.Count > 0 && secondPartOfQuery.Count() > 0)
                    //{
                    //    totalRecords = secondPartOfQuery.Count();
                    //    lstIQuerable = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                    //        s => new ClientCustomInformation()
                    //        {
                    //            ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                    //            ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                    //            ClientLineStatusName = s.ClientLineStatus.LineStatus.LineStatusName,
                    //            Name = s.ClientLineStatus.ClientDetails.Name,
                    //            LoginName = s.ClientLineStatus.ClientDetails.LoginName,
                    //            PackageName = "",// s.Transaction.Package.PackageName,
                    //            PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : "BNG: " + s.ClientLineStatus.Package.PackageName,
                    //            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                    //            Address = s.ClientLineStatus.ClientDetails.Address,
                    //            Email = s.ClientLineStatus.ClientDetails.Email,
                    //            Zone = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                    //            ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                    //            StatusThisMonthID = AppUtils.LineIsLock.ToString(),
                    //            StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
                    //            StatusThisMonthName = s.Transaction != null ? s.Transaction.LineStatus.LineStatusName : "BNG: Lock",
                    //            StatusNextMonthName = s.ClientLineStatus.LineStatus.LineStatusName,

                    //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.ToString("MM/dd/yyyy") : "",
                    //            IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,

                    //        }).AsQueryable();

                    //}
                    ////////////////////// if No Information found in Lock list and active list in accounts there is a possibilities that bill is not generate so then we have 
                    ///// to check the latest status from the clientLineStatus table this are the approximate active client list in this month. :D

                    //else
                    //{

                    //    totalRecords = (ZoneID == "") ? db.ClientLineStatus
                    //        .GroupBy(s => s.ClientDetailsID,
                    //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                    //        .Where(s => s.LineStatusID == AppUtils.LineIsLock).AsQueryable().Count()
                    //        : db.ClientLineStatus.Where(s => s.ClientDetails.ZoneID == zoneFromDDL)
                    //        .GroupBy(s => s.ClientDetailsID,
                    //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                    //        .Where(s => s.LineStatusID == AppUtils.LineIsLock).AsQueryable().Count();


                    //    if (ZoneID == "")
                    //    {
                    //        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                    //                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                    //            .Where(s => s.LineStatusID == AppUtils.LineIsLock).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                    //            .Select(s => new ClientCustomInformation
                    //            {
                    //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                    //                ClientLineStatusID = s.ClientLineStatusID,
                    //                ClientLineStatusName = s.LineStatus.LineStatusName,
                    //                Name = s.ClientDetails.Name,
                    //                LoginName = s.ClientDetails.LoginName,
                    //                PackageName = s.Package.PackageName,
                    //                PackageNameThisMonth = s.Package.PackageName,
                    //                PackageNameNextMonth = s.Package.PackageName,
                    //                Address = s.ClientDetails.Address,
                    //                Email = s.ClientDetails.Email,
                    //                Zone = s.ClientDetails.Zone.ZoneName,
                    //                ContactNumber = s.ClientDetails.ContactNumber,
                    //                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                    //                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                    //            }).AsQueryable();

                    //    }
                    //    else
                    //    {
                    //        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                    //                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                    //            .Where(s => s.LineStatusID == AppUtils.LineIsLock && s.ClientDetails.ZoneID == zoneFromDDL).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                    //            .Select(s => new ClientCustomInformation
                    //            {
                    //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                    //                ClientLineStatusID = s.ClientLineStatusID,
                    //                ClientLineStatusName = s.LineStatus.LineStatusName,
                    //                Name = s.ClientDetails.Name,
                    //                LoginName = s.ClientDetails.LoginName,
                    //                PackageName = s.Package.PackageName,
                    //                PackageNameThisMonth = s.Package.PackageName,
                    //                PackageNameNextMonth = s.Package.PackageName,
                    //                Address = s.ClientDetails.Address,
                    //                Email = s.ClientDetails.Email,
                    //                Zone = s.ClientDetails.Zone.ZoneName,
                    //                ContactNumber = s.ClientDetails.ContactNumber,
                    //                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                    //                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                    //            }).AsQueryable();
                    //    }
                    //}

                    //// Verification.   
                    //if (!string.IsNullOrEmpty(search) &&
                    //    !string.IsNullOrWhiteSpace(search))
                    //{
                    //    //var a = lstIQuerable.ToList();

                    //    //var b = lstIQuerable.Any() ? true : false;
                    //    //var c = lstIQuerable.Count() > 0 ? lstIQuerable.Count() : 0;

                    //    ifSearch = (lstIQuerable.Any() ? (lstIQuerable.Where(p =>
                    //      p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                    //    p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                    //    p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                    //    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    //    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    //    p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                    //    ).Count()) : 0);

                    //    // Apply search   
                    //    lstIQuerable = lstIQuerable.Where(p =>
                    //      p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                    //    p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                    //    p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                    //    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    //    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    //    p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                    //    p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                    //    ).AsQueryable();
                    //}
                    ////after i querable now i am making the list.
                    //lstClientCustomInformation = lstIQuerable.ToList();
                    stopwatch.Stop();
                    var totalTimeRequired = stopwatch.Elapsed;
                    //System.IO.File.AppendAllText(@"C:\WriteLines.txt", "Lock Client:Linq: >" + stopwatch.Elapsed.TotalMilliseconds.ToString() + " > " + Environment.NewLine);

                    // Sorting.   
                    //lstClientCustomInformation = this.SortByColumnWithOrder(order, orderDir, lstClientCustomInformation);
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
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
                result = this.Json(new
                {
                    data = ex,
                }, JsonRequestBehavior.AllowGet);
            }
            // Return info.   
            return result;
        }

        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.View_Active_Client_List)]
        public ActionResult GetAllActiveClient()
        {

            LoadViewBag();

            //List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
            ///// this are the clients from transaction who is lock
            //List<int> lstLockClients = db.Transaction
            //    .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList();
            ////// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
            //var queryClientCustomInformation = db.Transaction
            //    .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth &&
            //                !lstLockClients.Contains(s.ClientDetailsID))
            //    .GroupJoin(
            //        db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
            //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
            //        Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
            //        (Transaction, ClientLineStatus) => new
            //        {
            //            Transaction = Transaction,
            //            ClientLineStatus = ClientLineStatus.FirstOrDefault()
            //        }).AsQueryable();

            //if (lstLockClients.Count > 0 || lstClientCustomInformation.Count > 0)
            //{
            //    queryClientCustomInformation.Select(
            //        s => new ClientCustomInformation()
            //        {
            //            ClientDetailsID = s.Transaction.ClientDetailsID,
            //            Name = s.Transaction.ClientDetails.Name,
            //            LoginName = s.Transaction.ClientDetails.LoginName,
            //            PackageName = s.Transaction.Package.PackageName,
            //            PackageNameThisMonth = s.Transaction.Package.PackageName,
            //            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
            //            Address = s.Transaction.ClientDetails.Address,
            //            Email = s.Transaction.ClientDetails.Email,
            //            Zone = s.Transaction.ClientDetails.Zone.ZoneName,
            //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //            StatusThisMonthID = "3",
            //            StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
            //        }).ToList();
            //    return View(lstClientCustomInformation);

            //}
            ////////////////////// if No Information found in Lock list and active list in accounts there is a possibilities that bill is not generate so then we have 
            ///// to check the latest status from the clientLineStatus table this are the approximate active client list in this month. :D

            //else
            //{
            //    lstClientCustomInformation = db.ClientLineStatus
            //        .GroupBy(s => s.ClientDetailsID,
            //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        .Where(s => s.LineStatusID == AppUtils.LineIsActive)

            //        .Select(s => new ClientCustomInformation
            //        {
            //            ClientDetailsID = s.ClientDetails.ClientDetailsID,
            //            Name = s.ClientDetails.Name,
            //            LoginName = s.ClientDetails.LoginName,
            //            PackageName = s.Package.PackageName,
            //            PackageNameThisMonth = s.Package.PackageName,
            //            PackageNameNextMonth = s.Package.PackageName,
            //            Address = s.ClientDetails.Address,
            //            Email = s.ClientDetails.Email,
            //            Zone = s.ClientDetails.Zone.ZoneName,
            //            ContactNumber = s.ClientDetails.ContactNumber,
            //            StatusThisMonthID = "3",
            //            StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

            //        }).ToList();

            //    return View(lstClientCustomInformation);
            return View(new List<ClientCustomInformation>());

            // }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllActiveClientsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            //      AppUtils.RunningMonth = 9;
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                // Initialization.   
                var ZoneID = Request.Form.Get("ZoneID");
                var SearchTypeForActive = Request.Form.Get("SearchTypeForActive");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                IQueryable<ClientCustomInformation> lstIQuerable = Enumerable.Empty<ClientCustomInformation>().AsQueryable()
        ;
                List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
                /// this are the clients from transaction who is active
                List<int> lstLockClients = (ZoneID == "") ?
                    db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList()
                    : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList();

                //now this is the list of client who is lock. 
                List<int> lstOfClientByZoneOrNot = new List<int>();
                if (ZoneID != "")
                {
                    lstOfClientByZoneOrNot = db.ClientDetails.Where(s => s.ZoneID == int.Parse(ZoneID) && s.IsNewClient != AppUtils.isNewClient).Select(s => s.ClientDetailsID).ToList();
                    lstOfClientByZoneOrNot.RemoveAll(x => lstLockClients.Contains(x));
                }
                else
                {
                    lstOfClientByZoneOrNot = db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).Select(s => s.ClientDetailsID).ToList();
                    lstOfClientByZoneOrNot.RemoveAll(x => lstLockClients.Contains(x));
                }

                //// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
                var firstPartOfQuery =
                    db.ClientLineStatus.Where(s => lstOfClientByZoneOrNot.Contains(s.ClientDetailsID)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).AsQueryable();
                //var a = firstPartOfQuery.ToList();
                var secondPartOfQuery = firstPartOfQuery
                    .GroupJoin(
                        db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth),
                        ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID,
                        (ClientLineStatus, Transaction) => new
                        {
                            Transaction = Transaction.FirstOrDefault(),
                            ClientLineStatus = ClientLineStatus
                        }).AsQueryable();


                if (lstLockClients.Count > 0 || secondPartOfQuery.Count() > 0)//(lstLockClients.Count > 0 && secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstIQuerable = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new ClientCustomInformation()
                        {
                            ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                            ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                            ClientLineStatusName = s.ClientLineStatus.LineStatus.LineStatusName,
                            Name = s.ClientLineStatus.ClientDetails.Name,
                            LoginName = s.ClientLineStatus.ClientDetails.LoginName,
                            PackageName = "",// s.Transaction.Package.PackageName,
                            PackageNameThisMonth = s.Transaction != null ? s.Transaction.Package.PackageName : "BNG: " + s.ClientLineStatus.Package.PackageName,
                            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                            Address = s.ClientLineStatus.ClientDetails.Address,
                            Email = s.ClientLineStatus.ClientDetails.Email,
                            Zone = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                            ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                            StatusThisMonthID = AppUtils.LineIsActive.ToString(),
                            StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
                            StatusThisMonthName = s.Transaction != null ? s.Transaction.LineStatus.LineStatusName : "BNG: Lock",
                            StatusNextMonthName = s.ClientLineStatus.LineStatus.LineStatusName,

                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.ToString("MM/dd/yyyy") : "",
                            IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,

                        }).AsQueryable();

                }
                //////////////////// if No Information found in Lock list and active list in accounts there is a possibilities that bill is not generate so then we have 
                /// to check the latest status from the clientLineStatus table this are the approximate active client list in this month. :D

                else
                {

                    totalRecords = (ZoneID == "") ? db.ClientLineStatus
                        .GroupBy(s => s.ClientDetailsID,
                            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                        .Where(s => s.LineStatusID == AppUtils.LineIsActive).AsQueryable().Count()
                        : db.ClientLineStatus.Where(s => s.ClientDetails.ZoneID == zoneFromDDL)
                        .GroupBy(s => s.ClientDetailsID,
                            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                        .Where(s => s.LineStatusID == AppUtils.LineIsActive).AsQueryable().Count();


                    if (ZoneID == "")
                    {
                        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                            .Where(s => s.LineStatusID == AppUtils.LineIsInActive).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                            .Select(s => new ClientCustomInformation
                            {
                                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                                ClientLineStatusID = s.ClientLineStatusID,
                                ClientLineStatusName = s.LineStatus.LineStatusName,
                                Name = s.ClientDetails.Name,
                                LoginName = s.ClientDetails.LoginName,
                                PackageName = s.Package.PackageName,
                                PackageNameThisMonth = s.Package.PackageName,
                                PackageNameNextMonth = s.Package.PackageName,
                                Address = s.ClientDetails.Address,
                                Email = s.ClientDetails.Email,
                                Zone = s.ClientDetails.Zone.ZoneName,
                                ContactNumber = s.ClientDetails.ContactNumber,
                                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                            }).AsQueryable();

                    }
                    else
                    {
                        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                            .Where(s => s.LineStatusID == AppUtils.LineIsActive && s.ClientDetails.ZoneID == zoneFromDDL).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                            .Select(s => new ClientCustomInformation
                            {
                                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                                ClientLineStatusID = s.ClientLineStatusID,
                                ClientLineStatusName = s.LineStatus.LineStatusName,
                                Name = s.ClientDetails.Name,
                                LoginName = s.ClientDetails.LoginName,
                                PackageName = s.Package.PackageName,
                                PackageNameThisMonth = s.Package.PackageName,
                                PackageNameNextMonth = s.Package.PackageName,
                                Address = s.ClientDetails.Address,
                                Email = s.ClientDetails.Email,
                                Zone = s.ClientDetails.Zone.ZoneName,
                                ContactNumber = s.ClientDetails.ContactNumber,
                                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                            }).AsQueryable();
                    }
                }

                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    //var a = lstIQuerable.ToList();

                    //var b = lstIQuerable.Any() ? true : false;
                    //var c = lstIQuerable.Count() > 0 ? lstIQuerable.Count() : 0;

                    ifSearch = (lstIQuerable.Any() ? (lstIQuerable.Where(p =>
                    //p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                    //p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                    //p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    p.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                    //p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                    ).Count()) : 0);

                    // Apply search   
                    lstIQuerable = lstIQuerable.Where(p =>
                    //  p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                    //p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                    p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                    p.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                    //p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                    //p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                //after i querable now i am making the list.
                lstClientCustomInformation = lstIQuerable.ToList();
                // Sorting.   
                lstClientCustomInformation = this.SortByColumnWithOrder(order, orderDir, lstClientCustomInformation);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                ////// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
                //var firstPartOfQuery =
                //    (ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstLockClients.Contains(s.ClientDetailsID)).AsQueryable()
                //    : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstLockClients.Contains(s.ClientDetailsID)).AsQueryable();
                //var secondPartOfQuery = firstPartOfQuery
                //    .GroupJoin(
                //        db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
                //        Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                //        (Transaction, ClientLineStatus) => new
                //        {
                //            Transaction = Transaction,
                //            ClientLineStatus = ClientLineStatus.FirstOrDefault()
                //        }).AsQueryable();

                //if (lstLockClients.Count > 0 || secondPartOfQuery.Count() > 0)//(lstLockClients.Count > 0 && secondPartOfQuery.Count() > 0)
                //{
                //    totalRecords = secondPartOfQuery.Count();
                //    lstIQuerable = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                //        s => new ClientCustomInformation()
                //        {
                //            ClientDetailsID = s.Transaction.ClientDetailsID,
                //            ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                //            ClientLineStatusName = s.ClientLineStatus.LineStatus.LineStatusName,
                //            Name = s.Transaction.ClientDetails.Name,
                //            LoginName = s.Transaction.ClientDetails.LoginName,
                //            PackageName = s.Transaction.Package.PackageName,
                //            PackageNameThisMonth = s.Transaction.Package.PackageName,
                //            PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                //            Address = s.Transaction.ClientDetails.Address,
                //            Email = s.Transaction.ClientDetails.Email,
                //            Zone = s.Transaction.ClientDetails.Zone.ZoneName,
                //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                //            StatusThisMonthID = AppUtils.LineIsActive.ToString(),
                //            StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
                //            StatusThisMonthName = s.Transaction.LineStatus.LineStatusName,
                //            StatusNextMonthName = s.ClientLineStatus.LineStatus.LineStatusName,
                //            IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,
                //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.ToString("MM/dd/yyyy") : ""
                //        }).AsQueryable();

                //}
                ////////////////////// if No Information found in Lock list and active list in accounts there is a possibilities that bill is not generate so then we have 
                ///// to check the latest status from the clientLineStatus table this are the approximate active client list in this month. :D

                //else
                //{

                //    totalRecords = (ZoneID == "") ? db.ClientLineStatus
                //        .GroupBy(s => s.ClientDetailsID,
                //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //        .Where(s => s.LineStatusID == AppUtils.LineIsActive).AsQueryable().Count()
                //        : db.ClientLineStatus.Where(s => s.ClientDetails.ZoneID == zoneFromDDL)
                //        .GroupBy(s => s.ClientDetailsID,
                //            (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //        .Where(s => s.LineStatusID == AppUtils.LineIsActive).AsQueryable().Count();


                //    if (ZoneID == "")
                //    {
                //        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                //                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //            .Where(s => s.LineStatusID == AppUtils.LineIsActive).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                //            .Select(s => new ClientCustomInformation
                //            {
                //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                //                ClientLineStatusID = s.ClientLineStatusID,
                //                ClientLineStatusName = s.LineStatus.LineStatusName,
                //                Name = s.ClientDetails.Name,
                //                LoginName = s.ClientDetails.LoginName,
                //                PackageName = s.Package.PackageName,
                //                PackageNameThisMonth = s.Package.PackageName,
                //                PackageNameNextMonth = s.Package.PackageName,
                //                Address = s.ClientDetails.Address,
                //                Email = s.ClientDetails.Email,
                //                Zone = s.ClientDetails.Zone.ZoneName,
                //                ContactNumber = s.ClientDetails.ContactNumber,
                //                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                //                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                //            }).AsQueryable();

                //    }
                //    else
                //    {
                //        lstIQuerable = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                //                (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                //            .Where(s => s.LineStatusID == AppUtils.LineIsActive && s.ClientDetails.ZoneID == zoneFromDDL).OrderBy(s => s.ClientDetailsID).Skip(startRec).Take(pageSize)
                //            .Select(s => new ClientCustomInformation
                //            {
                //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                //                ClientLineStatusID = s.ClientLineStatusID,
                //                ClientLineStatusName = s.LineStatus.LineStatusName,
                //                Name = s.ClientDetails.Name,
                //                LoginName = s.ClientDetails.LoginName,
                //                PackageName = s.Package.PackageName,
                //                PackageNameThisMonth = s.Package.PackageName,
                //                PackageNameNextMonth = s.Package.PackageName,
                //                Address = s.ClientDetails.Address,
                //                Email = s.ClientDetails.Email,
                //                Zone = s.ClientDetails.Zone.ZoneName,
                //                ContactNumber = s.ClientDetails.ContactNumber,
                //                StatusThisMonthID = s.LineStatus.LineStatusID.ToString(),
                //                StatusNextMonthID = s.LineStatus.LineStatusID.ToString(),

                //            }).AsQueryable();
                //    }
                //}

                //// Verification.   
                //if (!string.IsNullOrEmpty(search) &&
                //    !string.IsNullOrWhiteSpace(search))
                //{
                //    //var a = lstIQuerable.ToList();

                //    //var b = lstIQuerable.Any() ? true : false;
                //    //var c = lstIQuerable.Count() > 0 ? lstIQuerable.Count() : 0;

                //    ifSearch = (lstIQuerable.Any() ? (lstIQuerable.Where(p =>
                //      p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                //    p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                //    p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                //    ).Count()) : 0);

                //    // Apply search   
                //    lstIQuerable = lstIQuerable.Where(p =>
                //      p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.ClientLineStatusID.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.LoginName.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.PackageNameThisMonth.ToLower().Contains(search.ToLower()) ||
                //    p.PackageNameNextMonth.ToLower().Contains(search.ToLower()) ||
                //    p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                //    (!string.IsNullOrEmpty(p.Email) ? p.Email.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    (p.Zone != null ? p.Zone.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower())) ||
                //    p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.StatusThisMonthName.ToString().ToLower().Contains(search.ToLower()) ||
                //    p.StatusNextMonthName.ToString().ToLower().Contains(search.ToLower())
                //    ).AsQueryable();
                //}
                //after i querable now i am making the list.
                lstClientCustomInformation = lstIQuerable.ToList();
                // Sorting.   
                lstClientCustomInformation = this.SortByColumnWithOrder(order, orderDir, lstClientCustomInformation);
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
                    data = lstClientCustomInformation
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetClientDetailsByIDForUpdateFormSeveralPopUp(int ClientDetailsID)
        {
            //  ClientLineStatus ClientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

            var cls = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID).GroupBy(s => s.ClientDetailsID, (Key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault()).AsQueryable();


            var ClientLineStatus = cls.Select(s => new
            {
                ClientDetailsID = s.ClientDetailsID,
                ClientLineStatusID = s.ClientLineStatusID,
                Name = s.ClientDetails.Name,
                Email = s.ClientDetails.Email,
                LoginName = s.ClientDetails.LoginName,
                Password = s.ClientDetails.Password,
                Address = s.ClientDetails.Address,
                LatitudeLongitude = s.ClientDetails.LatitudeLongitude,
                ContactNumber = s.ClientDetails.ContactNumber,
                ZoneID = s.ClientDetails.ZoneID,
                SMSCommunication = s.ClientDetails.SMSCommunication,
                Occupation = s.ClientDetails.Occupation,
                SocialCommunicationURL = s.ClientDetails.SocialCommunicationURL,
                Remarks = s.ClientDetails.Remarks,
                ConnectionTypeID = s.ClientDetails.ConnectionTypeID,
                BoxNumber = s.ClientDetails.BoxNumber,
                PopDetails = s.ClientDetails.PopDetails,
                RequireCable = s.ClientDetails.RequireCable,
                CableTypeID = s.ClientDetails.CableTypeID,
                Reference = s.ClientDetails.Reference,
                NationalID = s.ClientDetails.NationalID,
                PackageID = s.ClientDetails.PackageID,
                SecurityQuestionID = s.ClientDetails.SecurityQuestionID,
                SecurityQuestionAnswer = s.ClientDetails.SecurityQuestionAnswer,
                MacAddress = s.ClientDetails.MacAddress,
                ClientSurvey = s.ClientDetails.ClientSurvey,
                ConnectionDate = s.ClientDetails.ConnectionDate,
                LineStatusID = s.LineStatusID,
                StatusChangeReason = s.StatusChangeReason,
                ResellerID = s.ClientDetails.ResellerID != null ? s.ClientDetails.ResellerID.Value : -1
            });


            //var ignoreCircularLineStatusResults = AppUtils.IgnoreCircularLoop(cls);
            //var ignoreCircularTnsResults = AppUtils.IgnoreCircularLoop(tns);



            var JSON = Json(new { ClientLineStatus = ClientLineStatus.ToList() }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetClientDetailsByID(int ClientDetailsID)
        {
            //  ClientLineStatus ClientLineStatus = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

            var tns = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                .Select(x => new { x.TransactionID, x.BillCollectBy, x.PaymentAmount, x.PaymentDate, x.PaymentFrom, x.PaymentType, x.PaymentTypeID });
            var Transaction = tns.Select(s => new { TransactionID = s.TransactionID, PaymentAmount = s.PaymentAmount/*, PaymentDate = s.PaymentDate*/ }).ToList();
            //var sa = Transaction.ToList();
            var cls = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID)
                .GroupJoin(
                    db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientDetailsID),
                    ClientDetails => ClientDetails.ClientDetailsID, Transactions => Transactions.ClientDetailsID,
                    (ClientDetails, Transactions) => new { ClientDetails = ClientDetails, Transactions = Transactions })
                .AsEnumerable()
            .Select(
                s => new
                {
                    TransactionID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().TransactionID : 0,
                    ClientDetailsID = s.ClientDetails.ClientDetailsID,
                    //ClientLineStatusID = s.ClientDetails.ClientLineStatusID,
                    ThisMonthLineStatusID = s.ClientDetails.StatusThisMonth,
                    NextMonthLineStatusID = s.ClientDetails.StatusNextMonth,
                    Name = s.ClientDetails.Name,
                    Email = s.ClientDetails.Email,
                    LoginName = s.ClientDetails.LoginName,
                    Password = s.ClientDetails.Password,
                    Address = s.ClientDetails.Address,
                    LatitudeLongitude = s.ClientDetails.LatitudeLongitude,
                    ContactNumber = s.ClientDetails.ContactNumber,
                    ZoneID = s.ClientDetails.ZoneID,
                    SMSCommunication = s.ClientDetails.SMSCommunication,
                    Occupation = s.ClientDetails.Occupation,
                    IsPriorityClient = s.ClientDetails.IsPriorityClient,
                    SocialCommunicationURL = s.ClientDetails.SocialCommunicationURL,
                    Remarks = s.ClientDetails.Remarks,
                    ConnectionTypeID = s.ClientDetails.ConnectionTypeID,
                    BoxNumber = s.ClientDetails.BoxNumber,
                    PopDetails = s.ClientDetails.PopDetails,
                    RequireCable = s.ClientDetails.RequireCable,
                    CableTypeID = s.ClientDetails.CableTypeID,
                    Reference = s.ClientDetails.Reference,
                    NationalID = s.ClientDetails.NationalID,

                    //PackageID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().PackageID : s.ClientDetails.PackageID,//PackageNameNextMonth
                    PackageThisMonth = s.ClientDetails.PackageThisMonth,
                    PackageNextMonth = s.ClientDetails.PackageNextMonth,

                    SecurityQuestionID = s.ClientDetails.SecurityQuestionID,
                    SecurityQuestionAnswer = s.ClientDetails.SecurityQuestionAnswer,
                    MacAddress = s.ClientDetails.MacAddress,
                    ClientSurvey = s.ClientDetails.ClientSurvey,
                    ConnectionDate = s.ClientDetails.ConnectionDate,
                    //LineStatusID = s.ClientDetails.LineStatusID,
                    StatusChangeReason = db.ClientLineStatus.Where(x => x.ClientDetailsID == s.ClientDetails.ClientDetailsID).OrderByDescending(x => x.ClientLineStatusID).FirstOrDefault().StatusChangeReason,
                    LineStatusActiveDate = s.ClientDetails.LineStatusWillActiveInThisDate != null ? s.ClientDetails.LineStatusWillActiveInThisDate.ToString() : "",
                    ClientOWNImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientOwnImageBytesPaths) ? "" : s.ClientDetails.ClientOwnImageBytesPaths,
                    ClientNIDImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientNIDImageBytesPaths) ? "" : s.ClientDetails.ClientNIDImageBytesPaths,
                    ProfileStatusUpdateInPercent = GetProfileUpdatePercent(s.ClientDetails.ProfileUpdatePercentage, s.ClientDetails.ClientDetailsID),

                    MikrotikID = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.MikrotikID.Value : 0,
                    IP = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.IP : "",
                    Mac = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.Mac : "",

                    ResellerID = s.ClientDetails.ResellerID != null ? s.ClientDetails.ResellerID.Value : 0,

                    PaymentDate = s.ClientDetails.ApproxPaymentDate,

                    PermanentDiscount = s.ClientDetails.PermanentDiscount
                }).FirstOrDefault();

            //var cls = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID)
            //            .GroupBy(s => s.ClientDetailsID == ClientDetailsID, (key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault())
            //            .GroupJoin(
            //                db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientDetailsID),
            //                ClientLineStatus => ClientLineStatus.ClientDetailsID, Transactions => Transactions.ClientDetailsID,
            //                (ClientLineStatus, Transactions) => new { ClientDetails = ClientLineStatus, Transactions = Transactions })
            //            .AsEnumerable()
            //        .Select(
            //            s => new
            //            {
            //                TransactionID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().TransactionID : 0,
            //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
            //                ClientLineStatusID = s.ClientDetails.ClientLineStatusID,
            //                Name = s.ClientDetails.ClientDetails.Name,
            //                Email = s.ClientDetails.ClientDetails.Email,
            //                LoginName = s.ClientDetails.ClientDetails.LoginName,
            //                Password = s.ClientDetails.ClientDetails.Password,
            //                Address = s.ClientDetails.ClientDetails.Address,
            //                ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
            //                ZoneID = s.ClientDetails.ClientDetails.ZoneID,
            //                SMSCommunication = s.ClientDetails.ClientDetails.SMSCommunication,
            //                Occupation = s.ClientDetails.ClientDetails.Occupation,
            //                IsPriorityClient = s.ClientDetails.ClientDetails.IsPriorityClient,
            //                SocialCommunicationURL = s.ClientDetails.ClientDetails.SocialCommunicationURL,
            //                Remarks = s.ClientDetails.ClientDetails.Remarks,
            //                ConnectionTypeID = s.ClientDetails.ClientDetails.ConnectionTypeID,
            //                BoxNumber = s.ClientDetails.ClientDetails.BoxNumber,
            //                PopDetails = s.ClientDetails.ClientDetails.PopDetails,
            //                RequireCable = s.ClientDetails.ClientDetails.RequireCable,
            //                CableTypeID = s.ClientDetails.ClientDetails.CableTypeID,
            //                Reference = s.ClientDetails.ClientDetails.Reference,
            //                NationalID = s.ClientDetails.ClientDetails.NationalID,

            //                //PackageID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().PackageID : s.ClientDetails.PackageID,//PackageNameNextMonth
            //                PackageThisMonth = s.ClientDetails.ClientDetails.PackageThisMonth,
            //                PackageNextMonth = s.ClientDetails.ClientDetails.PackageNextMonth,

            //                SecurityQuestionID = s.ClientDetails.ClientDetails.SecurityQuestionID,
            //                SecurityQuestionAnswer = s.ClientDetails.ClientDetails.SecurityQuestionAnswer,
            //                MacAddress = s.ClientDetails.ClientDetails.MacAddress,
            //                ClientSurvey = s.ClientDetails.ClientDetails.ClientSurvey,
            //                ConnectionDate = s.ClientDetails.ClientDetails.ConnectionDate,
            //                //LineStatusID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().LineStatusID : s.ClientDetails.LineStatusID,
            //                LineStatusID = s.ClientDetails.LineStatusID,
            //                StatusChangeReason = s.ClientDetails.StatusChangeReason,
            //                LineStatusActiveDate = s.ClientDetails.LineStatusWillActiveInThisDate.HasValue ? s.ClientDetails.LineStatusWillActiveInThisDate.Value.ToString() : "",
            //                ClientOWNImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientDetails.ClientOwnImageBytesPaths) ? "" : s.ClientDetails.ClientDetails.ClientOwnImageBytesPaths,
            //                ClientNIDImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientDetails.ClientNIDImageBytesPaths) ? "" : s.ClientDetails.ClientDetails.ClientNIDImageBytesPaths,
            //                ProfileStatusUpdateInPercent = GetProfileUpdatePercent(s.ClientDetails.ClientDetails.ProfileUpdatePercentage, s.ClientDetails.ClientDetailsID),

            //                MikrotikID = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.MikrotikID.Value : 0,
            //                IP = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.IP : "",
            //                Mac = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.Mac : "",

            //                ResellerID = s.ClientDetails.ClientDetails.ResellerID != null ? s.ClientDetails.ClientDetails.ResellerID.Value : 0,

            //                PaymentDate = s.ClientDetails.ClientDetails.ApproxPaymentDate
            //            }).FirstOrDefault();

            var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                                      join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                                      from cblscka in a.DefaultIfEmpty()
                                      join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                                      from cblTypeb in b.DefaultIfEmpty()

                                      select new
                                      {
                                          CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                                          CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                                          AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                                      }).GroupBy(s => s.CableTypeID).ToList();

            var itemForThisClient =
                (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                 join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                 from clnproddistribution in a.DefaultIfEmpty()

                 join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                 from clnstock in b.DefaultIfEmpty()

                 join items in db.Item on clnstock.ItemID equals items.ItemID into c
                 from clnItems in c.DefaultIfEmpty()

                 select new
                 {
                     ItemID = clnItems.ItemID,
                     ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                     ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                 }).GroupBy(s => s.ItemID).ToList();
            //var ignoreCircularLineStatusResults = AppUtils.IgnoreCircularLoop(cls);
            //var ignoreCircularTnsResults = AppUtils.IgnoreCircularLoop(tns);

            bool ControlNeedToDisable = false;
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                ControlNeedToDisable = true;
            }

            var JSON = Json(new { ControlNeedToDisable = ControlNeedToDisable, Transaction = Transaction.ToList(), ClientLineStatus = cls, CableForThisClient = cableForThisClient, ItemForThisClient = itemForThisClient }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetClientDetailsByIDForResellerUserByAdmin(int ClientDetailsID)
        {
            var tns = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                .Select(x => new { x.TransactionID, x.BillCollectBy, x.PaymentAmount, x.PaymentDate, x.PaymentFrom, x.PaymentType, x.PaymentTypeID });
            var Transaction = tns.Select(s => new { TransactionID = s.TransactionID, PaymentAmount = s.PaymentAmount/*, PaymentDate = s.PaymentDate*/ }).ToList();
            var cls = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID)
               .GroupJoin(
                   db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientDetailsID),
                   ClientDetails => ClientDetails.ClientDetailsID, Transactions => Transactions.ClientDetailsID,
                   (ClientDetails, Transactions) => new { ClientDetails = ClientDetails, Transactions = Transactions })
               .AsEnumerable()
           .Select(
               s => new
               {
                   TransactionID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().TransactionID : 0,
                   ClientDetailsID = s.ClientDetails.ClientDetailsID,
                   //ClientLineStatusID = s.ClientDetails.ClientLineStatusID,
                   ThisMonthLineStatusID = s.ClientDetails.StatusThisMonth,
                   NextMonthLineStatusID = s.ClientDetails.StatusNextMonth,
                   Name = s.ClientDetails.Name,
                   Email = s.ClientDetails.Email,
                   LoginName = s.ClientDetails.LoginName,
                   Password = s.ClientDetails.Password,
                   Address = s.ClientDetails.Address,
                   ContactNumber = s.ClientDetails.ContactNumber,
                   ZoneID = s.ClientDetails.ZoneID,
                   SMSCommunication = s.ClientDetails.SMSCommunication,
                   Occupation = s.ClientDetails.Occupation,
                   IsPriorityClient = s.ClientDetails.IsPriorityClient,
                   SocialCommunicationURL = s.ClientDetails.SocialCommunicationURL,
                   Remarks = s.ClientDetails.Remarks,
                   ConnectionTypeID = s.ClientDetails.ConnectionTypeID,
                   BoxNumber = s.ClientDetails.BoxNumber,
                   PopDetails = s.ClientDetails.PopDetails,
                   RequireCable = s.ClientDetails.RequireCable,
                   CableTypeID = s.ClientDetails.CableTypeID,
                   Reference = s.ClientDetails.Reference,
                   NationalID = s.ClientDetails.NationalID,

                   //PackageID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().PackageID : s.ClientDetails.PackageID,//PackageNameNextMonth
                   PackageThisMonth = s.ClientDetails.PackageThisMonth,
                   PackageNextMonth = s.ClientDetails.PackageNextMonth,

                   SecurityQuestionID = s.ClientDetails.SecurityQuestionID,
                   SecurityQuestionAnswer = s.ClientDetails.SecurityQuestionAnswer,
                   MacAddress = s.ClientDetails.MacAddress,
                   ClientSurvey = s.ClientDetails.ClientSurvey,
                   ConnectionDate = s.ClientDetails.ConnectionDate,
                   //LineStatusID = s.ClientDetails.LineStatusID,
                   StatusChangeReason = db.ClientLineStatus.Where(x => x.ClientDetailsID == s.ClientDetails.ClientDetailsID).OrderByDescending(x => x.ClientLineStatusID).FirstOrDefault().StatusChangeReason,
                   LineStatusActiveDate = s.ClientDetails.LineStatusWillActiveInThisDate != null ? s.ClientDetails.LineStatusWillActiveInThisDate.ToString() : "",
                   ClientOWNImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientOwnImageBytesPaths) ? "" : s.ClientDetails.ClientOwnImageBytesPaths,
                   ClientNIDImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientNIDImageBytesPaths) ? "" : s.ClientDetails.ClientNIDImageBytesPaths,
                   ProfileStatusUpdateInPercent = GetProfileUpdatePercent(s.ClientDetails.ProfileUpdatePercentage, s.ClientDetails.ClientDetailsID),

                   MikrotikID = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.MikrotikID.Value : 0,
                   IP = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.IP : "",
                   Mac = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.Mac : "",

                   ResellerID = s.ClientDetails.ResellerID != null ? s.ClientDetails.ResellerID.Value : 0,

                   PaymentDate = s.ClientDetails.ApproxPaymentDate,

                   PermanentDiscount = s.ClientDetails.PermanentDiscount
               }).FirstOrDefault();

            var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                                      join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                                      from cblscka in a.DefaultIfEmpty()
                                      join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                                      from cblTypeb in b.DefaultIfEmpty()

                                      select new
                                      {
                                          CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                                          CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                                          AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                                      }).GroupBy(s => s.CableTypeID).ToList();

            var itemForThisClient =
                (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                 join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                 from clnproddistribution in a.DefaultIfEmpty()

                 join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                 from clnstock in b.DefaultIfEmpty()

                 join items in db.Item on clnstock.ItemID equals items.ItemID into c
                 from clnItems in c.DefaultIfEmpty()

                 select new
                 {
                     ItemID = clnItems.ItemID,
                     ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                     ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                 }).GroupBy(s => s.ItemID).ToList();

            bool ControlNeedToDisable = false;
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                ControlNeedToDisable = true;
            }

            Reseller reseller = db.Reseller.Find(cls.ResellerID);
            List<macReselleGivenPackageWithPriceModel> lstMacResellerPackage = reseller != null ? (List<macReselleGivenPackageWithPriceModel>)new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>();
            List<int> lstMirkotik = string.IsNullOrEmpty(reseller.MacResellerAssignMikrotik) ? new List<int>() : reseller.MacResellerAssignMikrotik.Trim(',').Split(',').Select(x => int.Parse(x)).ToList();
            var lstMacResellerMikrotik = db.Mikrotik.Where(x => lstMirkotik.Contains(x.MikrotikID)).Select(x => new { mikid = x.MikrotikID, mikname = x.MikName });

            var lstZone = db.Zone.Where(x => x.ResellerID == reseller.ResellerID).Select(x => new { zoneid = x.ZoneID, zonename = x.ZoneName }).ToList();
            var lstPackage = lstMacResellerPackage.Select(x => new { packageid = x.PID, packagename = x.PName }).ToList();
            var lstBox = db.Box.Where(x => x.ResellerID == reseller.ResellerID).Select(x => new { boxid = x.BoxID, boxname = x.BoxName }).ToList();
            var lstMIkrotik = lstMacResellerMikrotik;

            var JSON = Json(new { ControlNeedToDisable = ControlNeedToDisable, Transaction = Transaction.ToList(), ClientLineStatus = cls, CableForThisClient = cableForThisClient, ItemForThisClient = itemForThisClient, resellerzone = lstZone, resellerpackage = lstPackage, resellerbox = lstBox, resellerMikrotik = lstMacResellerMikrotik }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProfileUpdatePointsBycdid(int cdid)
        {
            List<CustomProfilePercentageFields> lstCustomProfilePercentageFields = GetProfileUpdatePointsInListDoneOrNot(cdid);
            return Json(new { lstCustomProfilePercentageFields = lstCustomProfilePercentageFields }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchClientListByZone(int searchID, int searchType)
        {
            // is any transaction or not in this month or year cause if any found then this is the final all or active or lock in this month
            int countTransactionExistOrNot = db.Transaction.Where(s =>
                s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Count();

            //Stopwatch stopWatch2 = new Stopwatch();
            //stopWatch2.Start();
            //////////////put the code here for checking the time//////////////////

            //var lstTransactions2 =
            //    searchType == 0 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
            //    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).ToList()
            //        : searchType == 3 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
            //                .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).ToList()
            //            : searchType == 5 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
            //                    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).ToList()
            //                : new List<Transaction>()
            //                    .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).ToList();


            //stopWatch2.Stop();
            //TimeSpan ts2 = stopWatch2.Elapsed;

            //// Format and display the TimeSpan value. 
            //string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts2.Hours, ts2.Minutes, ts2.Seconds,
            //    ts2.Milliseconds / 10);

            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            ////////////put the code here for checking the time//////////////////


            var a = db.Transaction
                  .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth &&
                              s.ClientDetails.ZoneID == searchID).GroupJoin(
                      db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                          (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
                      Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                      (Transaction, ClientLineStatus) => new
                      {
                          Transaction = Transaction,
                          ClientLineStatus = ClientLineStatus.FirstOrDefault()
                      }).AsEnumerable();

            var lstTransactions1 =
                          searchType == 0 ?
                            searchID == 0 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                                        : db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetails.ZoneID == searchID).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                        : searchType == 3 ?
                            searchID == 0 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                                        : db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive && s.ClientDetails.ZoneID == searchID).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                        : searchType == 5 ?
                            searchID == 0 ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                                        : db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock && s.ClientDetails.ZoneID == searchID).GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable()
                        : new List<Transaction>().GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new { Transaction = Transaction, ClientLineStatus = ClientLineStatus.FirstOrDefault() }).AsEnumerable();

            stopWatch1.Stop();
            TimeSpan ts1 = stopWatch1.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);


            //Stopwatch stopWatch3 = new Stopwatch();
            //stopWatch3.Start();
            ////////////put the code here for checking the time//////////////////

            var lstTransactions3 = (lstTransactions1.Any()) ? lstTransactions1.Select(s => new ClientSearch
            {
                ClientDetailsID = s.ClientLineStatus.ClientDetailsID,
                Name = s.ClientLineStatus.ClientDetails.Name,
                LoginName = s.ClientLineStatus.ClientDetails.LoginName,
                PackageName = s.ClientLineStatus.ClientDetails.Package.PackageName,
                PackageNameThisMonth = s.Transaction.Package.PackageName,
                PackageNameNextMonth = s.ClientLineStatus.Package.PackageName,
                Address = s.ClientLineStatus.ClientDetails.Address,
                Email = s.ClientLineStatus.ClientDetails.Email,
                ZoneName = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                StatusNextMonthID = s.ClientLineStatus.LineStatusID.ToString(),
                StatusThisMonthID = s.Transaction.LineStatusID.ToString()
            }).ToList() : new List<ClientSearch>();

            //stopWatch3.Stop();
            //TimeSpan ts3 = stopWatch3.Elapsed;

            //// Format and display the TimeSpan value. 
            //string elapsedTime3 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts3.Hours, ts3.Minutes, ts3.Seconds,
            //    ts3.Milliseconds / 10);




            return Json(new { SearchClientList = lstTransactions3 }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SearchActiveClientListByZone(int searchID, int searchType)
        //{
        //    List<ClientLineStatus> lstClientLineStatusDB = (searchID == 0) ? db.ClientLineStatus.ToList().GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).ToList() :
        //                                                                     db.ClientLineStatus.ToList().Where(s => s.ClientDetails.ZoneID == searchID).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).ToList();
        //    List<ClientLineStatus> lstClientLineStatusByType = new List<ClientLineStatus>();

        //    var sssw = lstClientLineStatusDB.Select(s => new
        //    {
        //        ClientDetailsID = s.ClientDetailsID,
        //        Name = s.ClientDetails.Name,
        //        LoginName = s.ClientDetails.LoginName,
        //        PackageName = s.ClientDetails.Package.PackageName,
        //        Address = s.ClientDetails.Address,
        //        Email = s.ClientDetails.Email,
        //        ZoneName = s.ClientDetails.Zone.ZoneName,
        //        ContactNumber = s.ClientDetails.ContactNumber,
        //        LineStatusID = s.LineStatusID
        //    }).ToList();

        //    if (searchType == 0)
        //    {
        //        lstClientLineStatusByType = lstClientLineStatusDB;
        //    }
        //    if (searchType > 0)
        //    {
        //        lstClientLineStatusByType = lstClientLineStatusDB.Where(s => s.LineStatusID == searchType).ToList();
        //    }
        //    //item.ClientDetailsID + '></td><td>' + item.ClientDetails.Name + '</td><td>' + item.ClientDetails.LoginName + '</td>
        //    //<td>' + item.ClientDetails.Package.PackageName + '</td><td>' + item.ClientDetails.Address + '</td><td>' + item.ClientDetails.Email + '</td>
        //    //<td>' + item.ClientDetails.Zone.ZoneName + '</td><td>' + item.ClientDetails.ContactNumber + '</td><td>' + ClientStatusHtml
        //    // var listClientLineStatus = AppUtils.IgnoreCircularLoop(lstClientLineStatusByType);
        //    IList dd = new List<dynamic>();
        //    int index = 0;
        //    foreach (var clientLineStaus in lstClientLineStatusByType)
        //    {
        //        dd.Add(
        //            new
        //            {
        //                ClientDetailsID = clientLineStaus.ClientDetailsID,
        //                Name = clientLineStaus.ClientDetails.Name,
        //                LoginName = clientLineStaus.ClientDetails.LoginName,
        //                PackageName = clientLineStaus.ClientDetails.Package.PackageName,
        //                Address = clientLineStaus.ClientDetails.Address,
        //                Email = clientLineStaus.ClientDetails.Email,
        //                ZoneName = clientLineStaus.ClientDetails.Zone.ZoneName,
        //                ContactNumber = clientLineStaus.ClientDetails.ContactNumber,
        //                LineStatusID = clientLineStaus.LineStatusID
        //            });
        //        // d = clientLineStaus.ClientDetailsID;
        //        index++;
        //    }


        //    return Json(new { SearchClientList = sssw }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewClientPhoneListDependOnCriteria(string ZoneID, string LineStatusID)
        {
            try
            {
                List<ClientLineStatus> lstClientLineStatus = db.ClientLineStatus.ToList().GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).ToList();

                if (!string.IsNullOrEmpty(ZoneID))
                {
                    lstClientLineStatus = lstClientLineStatus.Where(s => s.ClientDetails.ZoneID == int.Parse(ZoneID)).ToList();
                }
                if (!string.IsNullOrEmpty(LineStatusID))
                {
                    lstClientLineStatus = lstClientLineStatus.Where(s => s.LineStatusID == int.Parse(LineStatusID)).ToList();
                }

                List<string> lstPhoneNumber = lstClientLineStatus.Select(s => s.ClientDetails.ContactNumber).ToList();
                return Json(new { lstPhoneNumber = AppUtils.IgnoreCircularLoop(lstPhoneNumber), Success = true, DataFound = (lstPhoneNumber.Count > 0) ? true : false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { lstPhoneNumber = "", Success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ViewClientPhoneList()
        {
            ViewBag.LineStatusID = new SelectList(db.LineStatus.ToList().Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock), "LineStatusID", "LineStatusName", 3);
            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            List<ClientLineStatus> lstClientLineStatus = db.ClientLineStatus.ToList().Where(s => s.LineStatusID == 3).ToList();
            return View(lstClientLineStatus);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchActiveToLockBySearchCriteria(int? YearID, int? MonthID)
        {
            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.ThisMonthLastDate();
            DateTime getDateTime = AppUtils.GetDateTimeNow();


            if (YearID != null)
            {
                YearID = Convert.ToInt32(db.Year.Where(s => s.YearID == YearID.Value).Select(s => s.YearName).FirstOrDefault());
            }

            if (YearID != null && MonthID != null)
            {
                startDateOfThisMonth = new DateTime(YearID.Value, MonthID.Value, 1);
                endDateOfThisMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
            }

            List<int> lstActiveClientOnPreviousMonthall = db.ClientLineStatus
               .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
               .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
               .Where(s => s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList();

            //  if (YearID > 0 && MonthID > 0)
            //  {
            DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
            DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

            var lstClientLineStatuses = db.ClientLineStatus
                .Where(s => (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= lastDate) && s.LineStatusID == AppUtils.LineIsLock && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(a => a.LineStatusChangeDate).FirstOrDefault())
                .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly)
                            , ClientLineStatus => ClientLineStatus.ClientDetailsID,
                            Transaction => Transaction.ClientDetailsID, (ClientLineStatus, Transaction) => new
                            {
                                ClientLineStatus = ClientLineStatus,
                                Transaction = Transaction.FirstOrDefault()
                            })
                            .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                                     , ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, ClientLineNewStatus => ClientLineNewStatus.ClientDetailsID,
                                         (ClientLineStatus, ClientLineNewStatus) => new
                                         {
                                             ClientLineStatus = ClientLineStatus.ClientLineStatus,
                                             Transaction = ClientLineStatus.Transaction,
                                             ClientLineNewStatus = ClientLineNewStatus.FirstOrDefault()
                                         })
                .ToList();
            //   lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsLock && (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= lastDate) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID)).GroupBy(s=>s.).ToList();
            //   }
            //else
            //{
            //    lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsLock && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(a => a.LineStatusChangeDate).FirstOrDefault()).ToList();
            //    //   lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsLock && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID)).ToList();
            //}
            var ss = lstClientLineStatuses.Select(s => new
            {

                Name = (s.ClientLineStatus.ClientDetails != null) ? s.ClientLineStatus.ClientDetails.Name : "",
                ClientDetailsID = (s.ClientLineStatus.ClientDetails != null) ? s.ClientLineStatus.ClientDetailsID.ToString() : "",
                TransactionID = (s.ClientLineStatus.ClientDetails != null) ? db.Transaction.Where(sss => s.ClientLineStatus.ClientDetailsID == s.ClientLineStatus.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",


                ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                LoginName = s.ClientLineStatus.ClientDetails.LoginName,
                Address = s.ClientLineStatus.ClientDetails.Address,
                ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                ZoneName = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                PackageName = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
                PackagePrice = s.Transaction != null ? s.Transaction.PaymentAmount : s.ClientLineStatus.Package.PackagePrice,
                EmployeeID = db.Employee.Find(s.ClientLineStatus.EmployeeID).Name,
                LineStatusChangeDate = s.ClientLineStatus.LineStatusChangeDate,
                IsPriorityClient = s.ClientLineStatus.ClientDetails.IsPriorityClient,
                LineStatusActiveDate = s.ClientLineNewStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineNewStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineNewStatus.LineStatusID) : "",

            });


            return Json(new { Success = true, lstClientLineStatusesList = ss, lstClientLineStatusesCount = lstClientLineStatuses.Count() }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchLockToActiveBySearchCriteria(int? YearID, int? MonthID)
        {
            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.ThisMonthLastDate();
            DateTime getDateTime = AppUtils.GetDateTimeNow();
            //List<ClientLineStatus> lstClientLineStatuses = new List<ClientLineStatus>();

            if (YearID != null)
            {
                YearID = Convert.ToInt32(db.Year.Where(s => s.YearID == YearID.Value).Select(s => s.YearName).FirstOrDefault());
            }

            if (YearID != null && MonthID != null)
            {
                startDateOfThisMonth = new DateTime(YearID.Value, MonthID.Value, 1);
                endDateOfThisMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
            }
            DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
            DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

            List<int> lstLockClientOnPreviousMonthall = db.ClientLineStatus
               .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
               .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
               .Where(s => s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList();

            var lstClientLineStatuses = db.ClientLineStatus
                .Where(s => (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= lastDate) && s.LineStatusID == AppUtils.LineIsLock && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(a => a.LineStatusChangeDate).FirstOrDefault())

                .GroupJoin(db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly)
                    , ClientLineStatus => ClientLineStatus.ClientDetailsID, Transaction => Transaction.ClientDetailsID, (ClientLineStatus, Transaction) => new
                    {
                        ClientLineStatus = ClientLineStatus,
                        Transaction = Transaction.FirstOrDefault()
                    })

                  .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                                     , ClientLineStatus => ClientLineStatus.ClientLineStatus.ClientDetailsID, ClientLineNewStatus => ClientLineNewStatus.ClientDetailsID,
                                         (ClientLineStatus, ClientLineNewStatus) => new
                                         {
                                             ClientLineStatus = ClientLineStatus.ClientLineStatus,
                                             Transaction = ClientLineStatus.Transaction,
                                             ClientLineNewStatus = ClientLineNewStatus.FirstOrDefault()
                                         })
                .ToList();

            //if (YearID > 0 && MonthID > 0)
            //{
            //    DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
            //    DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
            //    //lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive && (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= lastDate) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID)).ToList();
            //    lstClientLineStatuses = db.ClientLineStatus.Where(s => (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= lastDate) && s.LineStatusID == AppUtils.LineIsActive && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(a => a.LineStatusChangeDate).FirstOrDefault()).ToList();
            //}
            //else
            //{
            //    lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(a => a.LineStatusChangeDate).FirstOrDefault()).ToList();
            //    //lstClientLineStatuses = db.ClientLineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID)).ToList();
            //}
            var ss = lstClientLineStatuses.Select(s => new
            {

                Name = (s.ClientLineStatus.ClientDetails != null) ? s.ClientLineStatus.ClientDetails.Name : "",
                ClientDetailsID = (s.ClientLineStatus.ClientDetails != null) ? s.ClientLineStatus.ClientDetailsID.ToString() : "",
                TransactionID = (s.ClientLineStatus.ClientDetails != null) ? db.Transaction.Where(sss => s.ClientLineStatus.ClientDetailsID == s.ClientLineStatus.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",


                ClientLineStatusID = s.ClientLineStatus.ClientLineStatusID,
                LoginName = s.ClientLineStatus.ClientDetails.LoginName,
                Address = s.ClientLineStatus.ClientDetails.Address,
                ContactNumber = s.ClientLineStatus.ClientDetails.ContactNumber,
                ZoneName = s.ClientLineStatus.ClientDetails.Zone.ZoneName,
                PackageName = s.Transaction != null ? s.Transaction.Package.PackageName : s.ClientLineStatus.Package.PackageName,
                PackagePrice = s.Transaction != null ? s.Transaction.PaymentAmount : s.ClientLineStatus.Package.PackagePrice,
                EmployeeID = db.Employee.Find(s.ClientLineStatus.EmployeeID).Name,
                LineStatusChangeDate = s.ClientLineStatus.LineStatusChangeDate,
                LineStatusActiveDate = s.ClientLineNewStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineNewStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineNewStatus.LineStatusID) : "",

            });


            return Json(new { Success = true, lstClientLineStatusesList = ss, lstClientLineStatusesCount = lstClientLineStatuses.Count() }, JsonRequestBehavior.AllowGet);
        }


        private void SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails clientDetails, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            else if (!IsValidContentLength(image.ContentLength))
            {
                ViewBag.ErrorFileTooLarge = "Your file is too large.";
            }

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = clientDetails.ClientDetailsID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ClientsImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "NID")
            {
                //clientDetails.image = fileName;
                clientDetails.ClientNIDImageBytes = imagebyte;
                clientDetails.ClientNIDImageBytesPaths = "/Images/ClientsImage/" + fileName;

            }
            else if (WhichPic == "OWN")
            {
                //clientDetails.image = fileName;
                clientDetails.ClientOwnImageBytes = imagebyte;
                clientDetails.ClientOwnImageBytesPaths = "/Images/ClientsImage/" + fileName;
            }
        }


        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }

        private bool IsValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1 MB
        }

        private void InsertClientDetailsInMikrotik(ITikConnection connection, ClientDetails clientDetails, Package packageSearch)
        {
            //// add user 
            var userCerate = connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", "pppoe", "profile", packageSearch.PackageName.Trim());
            userCerate.ExecuteNonQuery();

        }

        //private void InsertResellerClientDetailsInMikrotik(ITikConnection connection, ClientDetails clientDetails, ResellerPackage packageSearch)
        //{
        //    //// add user 
        //    var userCerate = connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", "pppoe", "profile", packageSearch.PackageName.Trim());
        //    userCerate.ExecuteNonQuery();

        //}

        internal void LockSystemClientList()
        {
            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            foreach (var reseller in db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).ToList())
            {

            }
            var clientList = db.ClientDetails.Where(x => x.ResellerID.HasValue).AsQueryable();
            foreach (var clientOneByOne in clientList)
            {
                //if (clientOneByOne.ApproxPaymentDate)
                //{

                //}
            }
        }

        [HttpPost]
        public ActionResult GetLastID(int resellerID = 0)
        {
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                resellerID = AppUtils.GetLoginUserID();
            }
            if (resellerID > 0)
            {
                ClientDetails clientDetails = db.ClientDetails.Where(x => x.ResellerID == resellerID).OrderByDescending(x => x.ClientDetailsID).FirstOrDefault();
                return Json(new { id = clientDetails != null ? db.Reseller.Find(resellerID).ResellerLoginName + "" + (clientDetails.ClientDetailsID + 1).ToString() : "0" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ClientDetails clientDetails = db.ClientDetails.OrderByDescending(x => x.ClientDetailsID).FirstOrDefault();
                return Json(new { id = clientDetails != null ? (clientDetails.ClientDetailsID + 1) : 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}