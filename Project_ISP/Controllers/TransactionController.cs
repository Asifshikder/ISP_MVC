using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using Newtonsoft.Json;
using Project_ISP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humanizer;
using tik4net;
using WebGrease.Css.Extensions;
using Month = OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Month;
using System.Configuration;
using System.Data.SqlClient;
using static ISP_ManagementSystemModel.AppUtils;
using System.Web.Script.Serialization;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class TransactionController : Controller
    {
        public TransactionController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
            //AppUtils.dateTimeNow = new DateTime(AppUtils.RunningYear, 9, 02); ;
        }

        private ISPContext db = new ISPContext();


        //  [UserRIghtCheck(ControllerValue = AppUtils.Transaction_)]
        public ActionResult ShowCurrentMonthBills()
        {
            ViewBag.EmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            //ViewBag.DueEmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            VM_ClientLineStatus_Transaction VM_ClientLineStatus_Transaction = new VM_ClientLineStatus_Transaction();
            DateTime date = AppUtils.GetDateTimeNow();
            DateTime firstDayOfMonth = new DateTime(2017, 9, 1);

            VM_ClientLineStatus_Transaction.lstClientLineStatus = db.ClientLineStatus.Where(s => s.LineStatusID == 3 && s.LineStatusChangeDate < firstDayOfMonth).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).ToList();
            List<int> lstClient = VM_ClientLineStatus_Transaction.lstClientLineStatus.Select(s => s.ClientDetailsID).ToList();
            VM_ClientLineStatus_Transaction.lstTransaction = db.Transaction.ToList().Where(s => lstClient.Contains(s.ClientDetailsID) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == date.Year && s.PaymentMonth == date.Month).ToList();

            return View(VM_ClientLineStatus_Transaction);
        }
        private void setViewBagListYear_Month_Employee()
        {
            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
            ViewBag.EmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
        }
        //<th style = "padding:0px;" ></ th >
        //                < th style="padding:0px"></th>
        //                <th style = "padding:10px;" > Login Name</th>
        //                <th style = "padding:10px;" > Address </ th >
        //                < th style="padding:10px;">Mobile</th>
        //                <th style = "padding:10px;" > Zone </ th >
        //                < th style="padding:10px;">Package</th>
        //                <th style = "padding:10px;" > Year </ th >
        //                < th style="padding:10px;">Month</th>
        //                <th style = "padding:10px;" > Amount </ th >
        //                < th style="padding:10px;">Type</th>
        //                <th style = "padding:10px;" > Paid By</th>
        //                <th style = "padding:10px;" > Paid Time</th>


        [UserRIghtCheck(ControllerValue = AppUtils.View_Filter_Bills_List)]
        public ActionResult FilterBills()
        {
            //DateTime thisMonthFirstDate = AppUtils.ThisMonthStartDate();
            //DateTime thisMonthLastDate = AppUtils.ThisMonthLastDate();
            DateTime todayStartDate = AppUtils.GetDateTimeNow().Date;
            DateTime todayEndDate = AppUtils.GetDateTimeNow().AddHours(23).AddMinutes(59).AddMilliseconds(59);
            ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");

            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");


            setViewBagListYear_Month_Employee();
            List<PaymentHistory> lstTransaction = new List<PaymentHistory>();
            var lstTransactions = db.PaymentHistory.Where(s => s.PaymentDate >= todayStartDate && s.PaymentDate <= todayEndDate)
                          .GroupJoin(db.ClientLineStatus, PaymentHistory => PaymentHistory.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (PaymentHistory, ClientLineStatus) => new
                          {
                              PaymentHistoryItself = PaymentHistory,
                              PaymentHistory = PaymentHistory.Transaction,
                              ClientLineStatus = ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault(),

                          })
                .AsEnumerable()
                .Select(
                s => new CustomFilterBills()
                {
                    TransactionID = s.PaymentHistory.TransactionID,
                    ClientDetailsID = s.PaymentHistory.ClientDetailsID,
                    ClientName = s.PaymentHistory.ClientDetails.Name,
                    ClientLoginName = s.PaymentHistory.ClientDetails.LoginName,
                    Address = s.PaymentHistory.ClientDetails.Address,
                    ContactNumber = s.PaymentHistory.ClientDetails.ContactNumber,
                    Zone = s.PaymentHistory.ClientDetails.Zone.ZoneName,
                    PackageName = s.PaymentHistory.Package.PackageName,
                    Year = (s.PaymentHistory.PaymentYear != 0) ? db.Year.Where(ss => ss.YearName == s.PaymentHistory.PaymentYear.ToString()).FirstOrDefault().YearName : db.Year.Where(ss => ss.YearName == s.PaymentHistory.PaymentDate.Value.Year.ToString()).FirstOrDefault().YearName,
                    Month = (s.PaymentHistory.PaymentMonth != 0) ? db.Month.Where(ss => ss.MonthID == s.PaymentHistory.PaymentMonth).FirstOrDefault().MonthName : db.Month.Where(ss => ss.MonthID == s.PaymentHistory.PaymentDate.Value.Month).FirstOrDefault().MonthName,
                    Amount = s.PaymentHistoryItself.PaidAmount.ToString(),
                    PaymentType = s.PaymentHistoryItself.AdvancePaymentID == null ? "Monthly Bill" : "Advance Payment",
                    PaymentTypeID = s.PaymentHistory.PaymentTypeID,
                    //        PaymentType = s.PaymentHistory.PaymentTypeID == AppUtils.PaymentTypeIsConnection ? "Connection Fee" : "Monthly Fee",
                    PaidBy = db.Employee.Where(ss => ss.EmployeeID == s.PaymentHistory.EmployeeID).FirstOrDefault().Name,
                    PaidTime = s.PaymentHistory.PaymentDate.Value,
                    IsPriorityClient = s.PaymentHistory.ClientDetails.IsPriorityClient,
                    LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                })
                .ToList();

            //List<int> clientDetailsID = lstTransactions.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID).Distinct().ToList();
            //if (clientDetailsID.Count > 0)
            //{
            //    ViewData["lstTransaction"] = lstTransactions.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && clientDetailsID.Contains(s.ClientDetailsID))
            //                  .Select(s => new ClientSetByViewBag
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}
            //else
            //{
            //    ViewData["lstTransaction"] = lstTransactions.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly)
            //                  .Select(s => new
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}


            //Login_Name = s.ClientDetails.LoginName,
            //    Address = s.ClientDetails.Address,
            //    Mobile = s.ClientDetails.ContactNumber,
            //    Zone = s.ClientDetails.Zone.ZoneName,
            //    Package = s.ClientDetails.Package.PackageName,
            //    Year = (s.PaymentYear != 0) ? db.Year.Where(ss => ss.YearID == s.PaymentYear).FirstOrDefault().YearName : db.Year.Where(ss => ss.YearID == s.PaymentDate.Value.Year).FirstOrDefault().YearName,
            //    Month = (s.PaymentMonth != 0) ? db.Month.Where(ss => ss.MonthID == s.PaymentMonth).FirstOrDefault().MonthName : db.Month.Where(ss => ss.MonthID == s.PaymentDate.Value.Month).FirstOrDefault().MonthName,
            //    // Month = (s.PaymentMonth != 0) ? db.Month.Find(s.PaymentMonth).MonthName : db.Month.Find(s.PaymentDate.Value.Month).MonthName,
            //    Amount = s.PaymentAmount,
            //    Type = (s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly) ? "Monthly Fee" : "Connection Fee",
            //    Paid_By = db.Employee.Where(ss => ss.EmployeeID == s.EmployeeID).FirstOrDefault().Name,
            //    Paid_Time = s.PaymentDate,



            //ViewBag.lstYear = db.Year.ToList();
            //ViewBag.lstMonth = db.Month.ToList();
            //ViewBag.PaymentStatus = db.PaymentStatus.ToList();
            //ViewBag.lstEmployee = db.Employee.ToList();
            //ViewBag.lstPackage = db.Package.ToList();

            return View(lstTransactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFilterBillsListBySearchCriteria(int? YearID, int? MonthID, int? EmployeeID)
        {

            try
            {
                IEnumerable<Transaction> lstTransaction = new List<Transaction>();
                DateTime thisMonthFirstDate = new DateTime();
                DateTime thisMonthLastDate = new DateTime();

                if (YearID != null && MonthID != null)
                {
                    thisMonthFirstDate = new DateTime(YearID.Value, MonthID.Value, 1);
                    thisMonthLastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                }
                else if (YearID != null)
                {
                    thisMonthFirstDate = new DateTime(YearID.Value, 1, 1);
                    thisMonthLastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                }
                else
                {
                    thisMonthFirstDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 1);
                    thisMonthLastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, DateTime.DaysInMonth(AppUtils.RunningYear, AppUtils.RunningMonth)));
                }


                if (YearID != null && MonthID != null && EmployeeID != null)
                {
                    lstTransaction = db.Transaction.Where(s => ((s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate) || (s.PaymentYear == YearID.Value && s.PaymentMonth == MonthID.Value)) && s.EmployeeID == EmployeeID.Value).AsEnumerable();
                }

                else if (YearID != null && MonthID != null)
                {
                    // List<int> lstTransactionID = db.Transaction.Where(s => ((s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate) || (s.PaymentYear == YearID.Value && s.PaymentMonth == MonthID.Value))).Select(s => s.PaymentYear).ToList();
                    lstTransaction = db.Transaction.Where(s => s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate).AsEnumerable();
                }

                else if (YearID != null && EmployeeID != null)
                {
                    // List<int> lstTransactionID = db.Transaction.Where(s => ((s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate) || (s.PaymentYear == YearID.Value && s.PaymentMonth == MonthID.Value))).Select(s => s.PaymentYear).ToList();
                    lstTransaction = db.Transaction.Where(s => s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate && s.EmployeeID == EmployeeID.Value).AsEnumerable();
                }

                else if (YearID != null)
                {
                    // List<int> lstTransactionID = db.Transaction.Where(s => ((s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate) || (s.PaymentYear == YearID.Value && s.PaymentMonth == MonthID.Value))).Select(s => s.PaymentYear).ToList();
                    lstTransaction = db.Transaction.Where(s => s.PaymentDate >= thisMonthFirstDate && s.PaymentDate <= thisMonthLastDate).AsEnumerable();
                }
                else if (EmployeeID != null)
                {
                    DateTime todayStartDate = AppUtils.GetDateTimeNow().Date;
                    DateTime todayEndDate = AppUtils.GetDateTimeNow().AddHours(23).AddMinutes(59).AddMilliseconds(59);
                    lstTransaction = db.Transaction.Where(s => s.PaymentDate >= todayStartDate && s.PaymentDate <= todayEndDate && s.EmployeeID == EmployeeID).AsEnumerable();
                }
                else
                {
                    DateTime todayStartDate = AppUtils.GetDateTimeNow().Date;
                    DateTime todayEndDate = AppUtils.GetDateTimeNow().AddHours(23).AddMinutes(59).AddMilliseconds(59);
                    lstTransaction = db.Transaction.Where(s => s.PaymentDate >= todayStartDate && s.PaymentDate <= todayEndDate).AsEnumerable();
                }

                var information = lstTransaction
                          .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                          {
                              Transaction = Transaction,
                              ClientLineStatus = ClientLineStatus.FirstOrDefault(),

                          })
                          .Select(s => new
                          {
                              Name = (s.Transaction.ClientDetails != null) ? s.Transaction.ClientDetails.Name : "",
                              ClientDetailsID = (s.Transaction.ClientDetails != null) ? s.Transaction.ClientDetailsID.ToString() : "",
                              TransactionSignUpID = (s.Transaction.ClientDetails != null) ? db.Transaction.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",



                              TransactionID = s.Transaction.TransactionID,
                              Login_Name = s.Transaction.ClientDetails.LoginName,
                              Address = s.Transaction.ClientDetails.Address,
                              Mobile = s.Transaction.ClientDetails.ContactNumber,
                              Zone = s.Transaction.ClientDetails.Zone.ZoneName,
                              Package = s.Transaction.Package.PackageName,
                              Year = (s.Transaction.PaymentYear != 0) ? db.Year.Where(ss => ss.YearName.Trim() == SqlFunctions.StringConvert((Double)s.Transaction.PaymentYear).Trim()).FirstOrDefault().YearName : db.Year.Where(ss => ss.YearName.Trim() == SqlFunctions.StringConvert((Double)s.Transaction.PaymentDate.Value.Year).Trim()).FirstOrDefault().YearName,
                              Month = (s.Transaction.PaymentMonth != 0) ? db.Month.Where(ss => ss.MonthID == s.Transaction.PaymentMonth).FirstOrDefault().MonthName : db.Month.Where(ss => ss.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
                              //// Month = (s.PaymentMonth != 0) ? db.Month.Find(s.PaymentMonth).MonthName : db.Month.Find(s.PaymentDate.Value.Month).MonthName,
                              Amount = s.Transaction.PaymentAmount,
                              Type = (s.Transaction.PaymentTypeID == AppUtils.PaymentTypeIsMonthly) ? true : false,
                              Paid_By = db.Employee.Where(ss => ss.EmployeeID == s.Transaction.EmployeeID).FirstOrDefault().Name,
                              Paid_Time = s.Transaction.PaymentDate,
                              IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient,
                              LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                          }).OrderBy(s => s.ClientDetailsID).ThenBy(s => s.Paid_Time).ToList();
                return Json(new { lstTransaction = information, TotalCount = lstTransaction.Count(), Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { lstTransaction = "", TotalCount = 0, Success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [UserRIghtCheck(ControllerValue = AppUtils.View_Account_Archive_Bills)]
        public ActionResult Accounts()
        {
            ViewBag.Title = "Archive Bills";
            DateTime dtNow = AppUtils.GetDateTimeNow();

            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.Date = AppUtils.RunningYear + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(AppUtils.RunningMonth);
            setViewBagList();


            //List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin =
            //  db.Transaction.Where(s =>
            //          s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //      .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
            //          ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
            //          {
            //              Transaction = Transaction,
            //              ClientDueBills = ClientDueBills
            //          })
            //          .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
            //          (Transaction, EmployeeTransactionLockUnlock) => new
            //          {
            //              Transaction = Transaction.Transaction,
            //              ClientDueBills = Transaction.ClientDueBills,
            //              EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
            //          })
            //          //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
            //          //{
            //          //      Transaction = Transaction.Transaction,

            //          //})
            //          .AsEnumerable()
            //          .Select(
            //      s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
            //      {
            //          TransactionID = s.Transaction.TransactionID,
            //          Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
            //          ClientDetailsID = s.Transaction.ClientDetailsID,
            //          ClientName = s.Transaction.ClientDetails.Name,
            //          Address = s.Transaction.ClientDetails.Address,
            //          ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //          ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
            //          PackageID = s.Transaction.PackageID.Value,
            //          PackageName = s.Transaction.Package.PackageName,
            //          MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
            //          FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
            //          PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentAmount.Value : 0,
            //          //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //          //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
            //          Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
            //          PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //          CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
            //          PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
            //          RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
            //          ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
            //          StatusThisMonthID = s.Transaction.LineStatusID.Value,
            //          //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
            //          //StatusNextMonthID = ,
            //          Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""

            //      }).ToList();


            //SetBillSummary();


            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCurrentMonthArchiveBillsAJAXData()
        {
            //List<int> lstClientDetailsID = db.Transaction.Where(s=> s.PaymentMonth == 12).Select(s => s.ClientDetailsID ).Distinct().ToList();
            //List<int> lstTransactionCLientDetailsID = db.ClientDetails.Where(s=>!lstClientDetailsID.Contains(s.ClientDetailsID)).Select(s => s.ClientDetailsID).ToList();

            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SP_AccountsBills", sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.MyUser;
                        sqlCmd.Parameters.Add("@ResellerID", "".ToString());
                        sqlConn.Open();

                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            //reader.Read();

                            while (reader.Read())
                            {
                                //var a = reader["TransactionID"];
                                //var aa = reader.Cast<object>().Count();

                                VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                recFilter = (int)reader["TotalRecords"];
                                totalRecords = (int)reader["TotalRecords"];
                                tec.TransactionID = (int)reader["TransactionID"];
                                tec.Paid = (int)reader["Paid"] == AppUtils.PaymentIsPaid ? true : false;
                                tec.ClientDetailsID = (int)reader["ClientDetailsID"];
                                tec.ClientName = string.IsNullOrEmpty(reader["ClientName"].ToString()) ? "" : reader["ClientName"].ToString();
                                tec.ClientLoginName = string.IsNullOrEmpty(reader["ClientLoginName"].ToString()) ? "" : reader["ClientLoginName"].ToString();
                                //tec.UserID = string.IsNullOrEmpty(reader["UserID"].ToString()) ? "" : reader["UserID"].ToString();
                                tec.Address = string.IsNullOrEmpty(reader["Addres"].ToString()) ? "" : reader["Addres"].ToString();
                                tec.ContactNumber = string.IsNullOrEmpty(reader["ContactNumber"].ToString()) ? "" : reader["ContactNumber"].ToString();
                                tec.ZoneName = string.IsNullOrEmpty(reader["ZoneName"].ToString()) ? "" : reader["ZoneName"].ToString();
                                var a = reader["PackageID"];
                                var b = reader["PackageName"];
                                tec.PackageID = (int)reader["PackageID"];
                                tec.PackageName = string.IsNullOrEmpty(reader["PackageName"].ToString()) ? "" : reader["PackageName"].ToString();
                                tec.MonthlyFee = Math.Round(Convert.ToDouble(reader["MonthlyFee"]), 2);
                                tec.FeeForThisMonth = Math.Round(Convert.ToDouble(reader["FeeForThisMonth"]), 2);
                                tec.PaidAmount = string.IsNullOrEmpty(reader["PaidAmount"].ToString()) ? 0 : Convert.ToDouble(reader["PaidAmount"]);//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                                tec.Discount = string.IsNullOrEmpty(reader["Discount"].ToString()) ? 0 : Convert.ToDouble(reader["Discount"]);//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                                tec.Due = string.IsNullOrEmpty(reader["Due"].ToString()) ? 0 : Convert.ToDouble(reader["Due"]);
                                tec.PaymentStatusFullyOrPartiallyOrNotPaid = PaymentStatusFullyOrPartiallyOrNotPaid(tec.FeeForThisMonth, tec.PaidAmount, tec.Due, tec.Discount);
                                tec.PaidBy = string.IsNullOrEmpty(reader["PaidBy"].ToString()) ? "" : reader["PaidBy"].ToString();
                                tec.CollectBy = string.IsNullOrEmpty(reader["CollectBy"].ToString()) ? "" : reader["CollectBy"].ToString();
                                tec.PaidTime = string.IsNullOrEmpty(reader["PaidTime"].ToString()) ? "" : reader["PaidTime"].ToString();
                                tec.RemarksNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["RemarksNo"].ToString();
                                tec.ReceiptNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["ResetNo"].ToString();
                                tec.StatusThisMonthID = (int)reader["StatusThisMonthID"];
                                tec.Employeename = string.IsNullOrEmpty(reader["Employeename"].ToString()) ? "" : reader["Employeename"].ToString();
                                tec.IsPriorityClient = Convert.ToBoolean(reader["IsPriorityClient"]);
                                tec.LineStatusActiveDate = !string.IsNullOrEmpty(reader["LineStatusWillActiveInThisDate"].ToString()) ? reader["LineStatusWillActiveInThisDate"].ToString() + " " + AppUtils.GetStatusDivByStatusID(int.Parse(reader["LineStatusID"].ToString())) : "";
                                tec.PermanentDiscount = string.IsNullOrEmpty(reader["PermanentDiscount"].ToString()) ? 0 : (double)reader["PermanentDiscount"];
                                lstArchiveBillsInformation.Add(tec);

                            }
                        }

                        sqlConn.Close();
                    }
                }
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
                }, JsonRequestBehavior.AllowGet);

                #region usingLinqAccounts
                //var firstPartOfQuery =
                //          (YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //        : (YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //        : (YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //        : (YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //        : (YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //        :
                //        db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                //    ;
                //var secondPartOfQuery = firstPartOfQuery
                //          .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                //          ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                //          {
                //              Transaction = Transaction,
                //              ClientDueBills = ClientDueBills
                //          })
                //          .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                //          (Transaction, EmployeeTransactionLockUnlock) => new
                //          {
                //              Transaction = Transaction.Transaction,
                //              ClientDueBills = Transaction.ClientDueBills,
                //              EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                //          })
                //          .GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //          {
                //              Transaction = Transaction.Transaction,
                //              ClientDueBills = Transaction.ClientDueBills,
                //              EmployeeTransactionLockUnlock = Transaction.EmployeeTransactionLockUnlock,
                //              ClientLineStatus = ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault(),

                //          })
                //          .GroupJoin(db.PaymentHistory, Transaction => Transaction.Transaction.TransactionID, PaymentHistory => PaymentHistory.TransactionID, (Transaction, PaymentHistory) => new
                //          {
                //              Transaction = Transaction.Transaction,
                //              ClientDueBills = Transaction.ClientDueBills,
                //              EmployeeTransactionLockUnlock = Transaction.EmployeeTransactionLockUnlock,
                //              ClientLineStatus = Transaction.ClientLineStatus,
                //              PaymentHistory = PaymentHistory
                //          })
                //          .AsEnumerable();

                ////var acount = secondPartOfQuery.Count();
                ////var a = secondPartOfQuery.ToList();
                //// Verification.   
                //if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                //{
                //    //        var a = secondPartOfQuery.ToList();
                //    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                //                                                             p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                          || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                          //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                          || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                          || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                          //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                          //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                          || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))

                //                                                          ).Count() : 0;

                //    // Apply search   
                //    secondPartOfQuery = secondPartOfQuery.Where(p => p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                        || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                        //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                        || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                        //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                        //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                        || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                        ).AsEnumerable();
                //}
                //if (secondPartOfQuery.Count() > 0)
                //{
                //    //var a = secondPartOfQuery.ToList();
                //    totalRecords = secondPartOfQuery.Count();
                //    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                //        s =>
                //        new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                //        {
                //            TransactionID = s.Transaction.TransactionID,
                //            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                //            ClientDetailsID = s.Transaction.ClientDetailsID,
                //            ClientName = s.Transaction.ClientDetails.Name,
                //            ClientLoginName = s.Transaction.ClientDetails.LoginName,
                //            Address = s.Transaction.ClientDetails.Address,
                //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                //            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                //            PackageID = s.Transaction.PackageID.Value,
                //            PackageName = s.Transaction.Package.PackageName,
                //            MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                //            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                //            PaidAmount = s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0,//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            Discount = s.Transaction.Discount != null ? s.Transaction.Discount.Value : 0,//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                //            Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.PaymentHistory.OrderByDescending(ss => ss.PaymentDate).FirstOrDefault().Employee.Name : "",
                //            CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.PaymentHistory.OrderByDescending(ss => ss.PaymentDate).FirstOrDefault().CollectByID).Name : "",
                //            PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.PaymentHistory.OrderByDescending(ss => ss.PaymentDate).FirstOrDefault().PaymentDate.ToString() : "",
                //            RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo == null ? "" : s.Transaction.RemarksNo.ToString() : "",
                //            ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? /*s.Transaction.ResetNo == null ? "" :*/ s.PaymentHistory.OrderByDescending(ss => ss.PaymentDate).FirstOrDefault().ResetNo : "",
                //            //PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            //CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                //            //PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                //            //RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo == null ? "" : s.Transaction.RemarksNo.ToString() : "",
                //            //ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo == null ? "" : s.Transaction.ResetNo.ToString() : "",
                //            StatusThisMonthID = s.Transaction.LineStatusID.Value,
                //            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
                //            //StatusNextMonthID = ,
                //            Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",

                //            IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient,
                //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",

                //        }).ToList();

                //}

                // Sorting.   
                //lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                //// Total record count.   
                //// totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //// Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = lstArchiveBillsInformation
                //}, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }


        [UserRIghtCheck(ControllerValue = AppUtils.View_Reseller_Accounts)]
        public ActionResult ResellerAccounts()
        {
            ViewBag.Title = "Archive Bills";
            DateTime dtNow = AppUtils.GetDateTimeNow();
            ViewBag.Date = AppUtils.RunningYear + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(AppUtils.RunningMonth);
            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");

            bool forReseller = true;
            setViewBagList(forReseller, AppUtils.GetLoginUserID());
            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerCurrentMonthArchiveBillsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SP_AccountsBills", sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                        sqlCmd.Parameters.Add("@ResellerID", AppUtils.GetLoginUserID());
                        sqlConn.Open();

                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            //reader.Read();

                            while (reader.Read())
                            {
                                //var a = reader["TransactionID"];
                                //var aa = reader.Cast<object>().Count();

                                VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                recFilter = (int)reader["TotalRecords"];
                                totalRecords = (int)reader["TotalRecords"];
                                tec.TransactionID = (int)reader["TransactionID"];
                                tec.Paid = (int)reader["Paid"] == AppUtils.PaymentIsPaid ? true : false;
                                tec.ClientDetailsID = (int)reader["ClientDetailsID"];
                                tec.ClientName = string.IsNullOrEmpty(reader["ClientName"].ToString()) ? "" : reader["ClientName"].ToString();
                                tec.ClientLoginName = string.IsNullOrEmpty(reader["ClientLoginName"].ToString()) ? "" : reader["ClientLoginName"].ToString();
                                //tec.UserID = string.IsNullOrEmpty(reader["UserID"].ToString()) ? "" : reader["UserID"].ToString();
                                tec.Address = string.IsNullOrEmpty(reader["Addres"].ToString()) ? "" : reader["Addres"].ToString();
                                tec.ContactNumber = string.IsNullOrEmpty(reader["ContactNumber"].ToString()) ? "" : reader["ContactNumber"].ToString();
                                tec.ZoneName = string.IsNullOrEmpty(reader["ZoneName"].ToString()) ? "" : reader["ZoneName"].ToString();
                                var a = reader["PackageID"];
                                var b = reader["PackageName"];
                                tec.PackageID = (int)reader["PackageID"];
                                tec.PackageName = string.IsNullOrEmpty(reader["PackageName"].ToString()) ? "" : reader["PackageName"].ToString();
                                tec.MonthlyFee = Math.Round(Convert.ToDouble(reader["MonthlyFee"]), 2);
                                tec.FeeForThisMonth = Math.Round(Convert.ToDouble(reader["FeeForThisMonth"]), 2);
                                tec.PaidAmount = string.IsNullOrEmpty(reader["PaidAmount"].ToString()) ? 0 : Convert.ToDouble(reader["PaidAmount"]);//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                                tec.Discount = string.IsNullOrEmpty(reader["Discount"].ToString()) ? 0 : Convert.ToDouble(reader["Discount"]);//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                                tec.Due = string.IsNullOrEmpty(reader["Due"].ToString()) ? 0 : Convert.ToDouble(reader["Due"]);
                                tec.PaymentStatusFullyOrPartiallyOrNotPaid = PaymentStatusFullyOrPartiallyOrNotPaid(tec.FeeForThisMonth, tec.PaidAmount, tec.Due, tec.Discount);
                                tec.PaidBy = string.IsNullOrEmpty(reader["PaidBy"].ToString()) ? "" : reader["PaidBy"].ToString();
                                tec.CollectBy = string.IsNullOrEmpty(reader["CollectBy"].ToString()) ? "" : reader["CollectBy"].ToString();
                                tec.PaidTime = string.IsNullOrEmpty(reader["PaidTime"].ToString()) ? "" : reader["PaidTime"].ToString();
                                tec.RemarksNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["RemarksNo"].ToString();
                                tec.ReceiptNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["ResetNo"].ToString();
                                tec.StatusThisMonthID = (int)reader["StatusThisMonthID"];
                                tec.Employeename = string.IsNullOrEmpty(reader["Employeename"].ToString()) ? "" : reader["Employeename"].ToString();
                                tec.IsPriorityClient = Convert.ToBoolean(reader["IsPriorityClient"]);
                                tec.LineStatusActiveDate = !string.IsNullOrEmpty(reader["LineStatusWillActiveInThisDate"].ToString()) ? reader["LineStatusWillActiveInThisDate"].ToString() + " " + AppUtils.GetStatusDivByStatusID(int.Parse(reader["LineStatusID"].ToString())) : "";
                                tec.PermanentDiscount = string.IsNullOrEmpty(reader["PermanentDiscount"].ToString()) ? 0 : (double)reader["PermanentDiscount"];
                                lstArchiveBillsInformation.Add(tec);

                            }
                        }

                        sqlConn.Close();
                    }
                }
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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


        [UserRIghtCheck(ControllerValue = AppUtils.View_Reseller_Accounts_By_Admin)]
        public ActionResult ResellerAccountsByAdmin()
        {
            ViewBag.Title = "Archive Bills";
            setViewBagListForResellerAccountsByAdmin();
            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());
        }
        private void setViewBagListForResellerAccountsByAdmin()
        {
            ViewBag.BoxID = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.ZoneID = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SearchByZoneID = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.PackageThisMonth = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PackageNextMonth = new SelectList(Enumerable.Empty<SelectListItem>());

            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            ViewBag.ResellerID = new SelectList(db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.Date = AppUtils.RunningYear + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(AppUtils.RunningMonth);

            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");

            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");

            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");

            ViewBag.EmployeeID = new SelectList(Enumerable.Empty<SelectListItem>(), "EmployeeID", "Name");
            ViewBag.DueEmployeeID = new SelectList(Enumerable.Empty<SelectListItem>(), "EmployeeID", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerCurrentMonthArchiveBillsByAdminAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                int resellerFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (ResellerID != "")
                {
                    resellerFromDDL = int.Parse(ResellerID);
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
                if (resellerFromDDL > 0)
                {
                    using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SP_AccountsBills", sqlConn))
                        {
                            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                            sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                            sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                            sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                            sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                            sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                            sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                            sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                            sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                            sqlCmd.Parameters.Add("@ResellerID", resellerFromDDL);
                            sqlConn.Open();

                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                //reader.Read();

                                while (reader.Read())
                                {
                                    //var a = reader["TransactionID"];
                                    //var aa = reader.Cast<object>().Count();

                                    VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                    recFilter = (int)reader["TotalRecords"];
                                    totalRecords = (int)reader["TotalRecords"];
                                    tec.TransactionID = (int)reader["TransactionID"];
                                    tec.Paid = (int)reader["Paid"] == AppUtils.PaymentIsPaid ? true : false;
                                    tec.ClientDetailsID = (int)reader["ClientDetailsID"];
                                    tec.ClientName = string.IsNullOrEmpty(reader["ClientName"].ToString()) ? "" : reader["ClientName"].ToString();
                                    tec.ClientLoginName = string.IsNullOrEmpty(reader["ClientLoginName"].ToString()) ? "" : reader["ClientLoginName"].ToString();
                                    //tec.UserID = string.IsNullOrEmpty(reader["UserID"].ToString()) ? "" : reader["UserID"].ToString();
                                    tec.Address = string.IsNullOrEmpty(reader["Addres"].ToString()) ? "" : reader["Addres"].ToString();
                                    tec.ContactNumber = string.IsNullOrEmpty(reader["ContactNumber"].ToString()) ? "" : reader["ContactNumber"].ToString();
                                    tec.ZoneName = string.IsNullOrEmpty(reader["ZoneName"].ToString()) ? "" : reader["ZoneName"].ToString();
                                    var a = reader["PackageID"];
                                    var b = reader["PackageName"];
                                    tec.PackageID = (int)reader["PackageID"];
                                    tec.PackageName = string.IsNullOrEmpty(reader["PackageName"].ToString()) ? "" : reader["PackageName"].ToString();
                                    tec.MonthlyFee = Math.Round(Convert.ToDouble(reader["MonthlyFee"]), 2);
                                    tec.FeeForThisMonth = Math.Round(Convert.ToDouble(reader["FeeForThisMonth"]), 2);
                                    tec.PaidAmount = string.IsNullOrEmpty(reader["PaidAmount"].ToString()) ? 0 : Convert.ToDouble(reader["PaidAmount"]);//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                                    tec.Discount = string.IsNullOrEmpty(reader["Discount"].ToString()) ? 0 : Convert.ToDouble(reader["Discount"]);//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                                    tec.Due = string.IsNullOrEmpty(reader["Due"].ToString()) ? 0 : Convert.ToDouble(reader["Due"]);
                                    tec.PaymentStatusFullyOrPartiallyOrNotPaid = PaymentStatusFullyOrPartiallyOrNotPaid(tec.FeeForThisMonth, tec.PaidAmount, tec.Due, tec.Discount);
                                    tec.PaidBy = string.IsNullOrEmpty(reader["PaidBy"].ToString()) ? "" : reader["PaidBy"].ToString();
                                    tec.CollectBy = string.IsNullOrEmpty(reader["CollectBy"].ToString()) ? "" : reader["CollectBy"].ToString();
                                    tec.PaidTime = string.IsNullOrEmpty(reader["PaidTime"].ToString()) ? "" : reader["PaidTime"].ToString();
                                    tec.RemarksNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["RemarksNo"].ToString();
                                    tec.ReceiptNo = string.IsNullOrEmpty(reader["RemarksNo"].ToString()) ? "" : reader["ResetNo"].ToString();
                                    tec.StatusThisMonthID = (int)reader["StatusThisMonthID"];
                                    tec.Employeename = string.IsNullOrEmpty(reader["Employeename"].ToString()) ? "" : reader["Employeename"].ToString();
                                    tec.IsPriorityClient = Convert.ToBoolean(reader["IsPriorityClient"]);
                                    tec.LineStatusActiveDate = !string.IsNullOrEmpty(reader["LineStatusWillActiveInThisDate"].ToString()) ? reader["LineStatusWillActiveInThisDate"].ToString() + " " + AppUtils.GetStatusDivByStatusID(int.Parse(reader["LineStatusID"].ToString())) : "";
                                    tec.PermanentDiscount = string.IsNullOrEmpty(reader["PermanentDiscount"].ToString()) ? 0 : (double)reader["PermanentDiscount"];
                                    lstArchiveBillsInformation.Add(tec);
                                }
                            }

                            sqlConn.Close();
                        }
                    }
                    lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                }
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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


        private int PaymentStatusFullyOrPartiallyOrNotPaid(double feeForThisMonth, double paidAmount, double due, double discount)
        {
            int paymentStatus = AppUtils.PaymentIsNotPaid;
            if (paidAmount > 0)
            {
                if ((paidAmount + discount) >= feeForThisMonth)
                {
                    paymentStatus = AppUtils.PaymentIsPaid;
                }
                else
                {
                    paymentStatus = AppUtils.PaymentIsPartiallyPaid;
                }
            }
            return paymentStatus;
        }
        private List<CustomSignUpBills> SortByColumnWithOrder(string order, string orderDir, List<CustomSignUpBills> data)
        {

            // Initialization.   
            List<CustomSignUpBills> lst = new List<CustomSignUpBills>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLineStatusID).ToList() : data.OrderBy(p => p.ClientLineStatusID).ToList();
                    //    break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ZoneName).ToList() : data.OrderBy(p => p.ZoneName).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaymentDate).ToList() : data.OrderBy(p => p.PaymentDate).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RemarksNo).ToList() : data.OrderBy(p => p.RemarksNo).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
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

        private List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> SortByColumnWithOrder(string order, string orderDir, List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> data)
        {
            // Initialization.   
            List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lst = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLineStatusID).ToList() : data.OrderBy(p => p.ClientLineStatusID).ToList();
                    //    break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ZoneName).ToList() : data.OrderBy(p => p.ZoneName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "14":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RemarksNo).ToList() : data.OrderBy(p => p.RemarksNo).ToList();
                        break;
                    case "15":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceiptNo).ToList() : data.OrderBy(p => p.ReceiptNo).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
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

        [UserRIghtCheck(ControllerValue = AppUtils.View_Unpaid_Bills_List)]
        public ActionResult UnpaidBills()
        {

            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.Title = "Unpaid Bills";
            setViewBagList();

            //DateTime dtNow = AppUtils.GetDateTimeNow();
            //List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin =
            //  db.Transaction.

            //  Where(s =>
            //          s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0)
            //      .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
            //          ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
            //          {
            //              Transaction = Transaction,
            //              ClientDueBills = ClientDueBills
            //          })
            //          .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
            //          (Transaction, EmployeeTransactionLockUnlock) => new
            //          {
            //              Transaction = Transaction.Transaction,
            //              ClientDueBills = Transaction.ClientDueBills,
            //              EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
            //          })
            //          .AsEnumerable()
            //          .Select(s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
            //          {
            //              TransactionID = s.Transaction.TransactionID,
            //              Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
            //              ClientDetailsID = s.Transaction.ClientDetailsID,
            //              ClientName = s.Transaction.ClientDetails.Name,
            //              Address = s.Transaction.ClientDetails.Address,
            //              ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //              ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
            //              PackageID = s.Transaction.PackageID.Value,
            //              PackageName = s.Transaction.Package.PackageName,
            //              MonthlyFee = s.Transaction.Package.PackagePrice,
            //              FeeForThisMonth = s.Transaction.PaymentAmount.Value,
            //              PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentAmount.Value : 0,
            //              Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
            //              //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //              //    ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
            //              //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //              //            ? db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).FirstOrDefault().DueAmount : 0,
            //              PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //              CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //              PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
            //              RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
            //              ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
            //              //LineStatusID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
            //              StatusThisMonthID = s.Transaction.LineStatusID.Value,
            //              Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""

            //          }).ToList();




            ////List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            //////var lstTransaction = db.Transaction
            //////    .Join(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new VM_Transaction_ClientDueBills { Transaction = Transaction, ClientDueBills = ClientDueBills })
            //////    .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.Transaction.PaymentStatus == 0)// && s.ClientDueBills.DueAmount != 0
            //////    .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus)).Select(s => s.ToList()).ToList();

            ////var lst = db.Transaction
            ////   .GroupJoin(
            ////       db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            ////       , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            ////       )
            ////       .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.Transaction.PaymentStatus == 0)
            ////       .Select(g => new VM_Transaction_ClientDueBills { Transaction = g.Transaction, ClientDueBills = g.ClientDueBills.FirstOrDefault() })
            ////       .ToList();


            //////var lst = db.Transaction
            //////    .GroupJoin(
            //////        db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            //////        , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            //////        )
            //////        .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //////        .Select(g => new { Transaction = g.Transaction, clientDueBills = g.ClientDueBills })
            //////        .ToList();
            //////var lsts = db.Transaction
            //////    .GroupJoin(
            //////        db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            //////        , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            //////        )
            //////        .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //////        .Select(g => new { Transaction = g.Transaction, clientDueBills = (!g.ClientDueBills.Any()) ? 0 : g.ClientDueBills.Sum(gg => ((double?)gg.DueAmount) ?? 0) })
            //////        .ToList();

            ////foreach (var items in lst)
            ////{
            ////    //if (items.Count > 0)
            ////    //{
            ////    //    foreach (var item in items)
            ////    //    {
            ////    //        lstVM_Transaction_ClientDueBills.Add(item);
            ////    //    }

            ////    //}

            ////    lstVM_Transaction_ClientDueBills.Add(new VM_Transaction_ClientDueBills { Transaction = items.Transaction, ClientDueBills = (items.ClientDueBills == null) ? null : items.ClientDueBills });
            ////}

            // SetBillSummary();
            //return View(lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin);
            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCurrentMonthUnPaidBillsAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                Session["IdListForSMSSend"] = null;

                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   


                bool isCheckAllFromCln = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };

                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");

                var IsCheckAll = Request.Form.Get("IsCheckAll");

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }


                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();

                #region linqUnpaidBills
                //var firstPartOfQuery =
                //        (YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //            : (YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                : (YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                    : (YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                        : (YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                            :
                //                            db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //    ;
                //var secondPartOfQuery = firstPartOfQuery
                //    .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                //        ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                //        {
                //            Transaction = Transaction,
                //            ClientDueBills = ClientDueBills
                //        })
                //    .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                //        (Transaction, EmployeeTransactionLockUnlock) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                //        })

                //        .GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = Transaction.EmployeeTransactionLockUnlock,
                //            ClientLineStatus = ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault(),
                //        })
                //    //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //    //{
                //    //      Transaction = Transaction.Transaction,

                //    //})
                //    .AsEnumerable();


                //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                //    Phone = s.Transaction.ClientDetails.ContactNumber
                //}).ToList();
                //// Verification.   
                //if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                //{

                //    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                //                                                                           p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                                           || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                                        //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        )
                //                                                                        .Count() : 0;

                //    // Apply search   
                //    secondPartOfQuery = secondPartOfQuery.Where(p =>
                //                                                        p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                        || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                     //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                     //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                     //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                     || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                       ).AsEnumerable();
                //    //    var a = secondPartOfQuery.ToList();
                //}
                //if (secondPartOfQuery.Count() > 0)
                //{
                //    totalRecords = secondPartOfQuery.Count();
                //    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                //        s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                //        {
                //            chkSMS = CheckOrNot(s.Transaction.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),

                //            TransactionID = s.Transaction.TransactionID,
                //            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                //            ClientDetailsID = s.Transaction.ClientDetailsID,
                //            ClientName = s.Transaction.ClientDetails.Name,
                //            ClientLoginName = s.Transaction.ClientDetails.LoginName,
                //            Address = s.Transaction.ClientDetails.Address,
                //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                //            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                //            PackageID = s.Transaction.PackageID.Value,
                //            PackageName = s.Transaction.Package.PackageName,
                //            MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                //            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                //            //PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            ////Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            ////            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            //Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidAmount = s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0,//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            Discount = s.Transaction.Discount != null ? s.Transaction.Discount.Value : 0,//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                //            Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                //            PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                //            RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                //            ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                //            StatusThisMonthID = s.Transaction.LineStatusID.Value,
                //            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
                //            //StatusNextMonthID = ,
                //            Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient,
                //            //LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                //        }).ToList();

                //}

                //// Sorting.   
                //lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                //// Total record count.   
                //// totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //// Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = lstArchiveBillsInformation
                //}, JsonRequestBehavior.AllowGet);
                #endregion

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SP_UnpaidBills", sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@UnpainPaidBillCheckedID", SqlDbType.Int).Value = AppUtils.PaymentIsNotPaid;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.MyUser;
                        sqlCmd.Parameters.Add("@ResellerID", "".ToString());


                        SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(UnpaidClientList);
                        SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(testValue);

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
                                VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                SetUnpaidBillOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                lstArchiveBillsInformation.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";
                            //get output param list
                            //int Count1 = Convert.ToInt32(sqlCmd.Parameters["@Out1"].Value);
                            //int Count2 = Convert.ToInt32(sqlCmd.Parameters["@Out2"].Value);
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
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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


        [UserRIghtCheck(ControllerValue = AppUtils.View_Unpaid_Bills_List_Reseller_Clients)]
        public ActionResult ResellerUnpaidBills()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.Title = "Unpaid Bills";
            bool forReseller = true;
            setViewBagList(forReseller, AppUtils.GetLoginUserID());

            //DateTime dtNow = AppUtils.GetDateTimeNow();
            //List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin =
            //  db.Transaction.

            //  Where(s =>
            //          s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0)
            //      .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
            //          ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
            //          {
            //              Transaction = Transaction,
            //              ClientDueBills = ClientDueBills
            //          })
            //          .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
            //          (Transaction, EmployeeTransactionLockUnlock) => new
            //          {
            //              Transaction = Transaction.Transaction,
            //              ClientDueBills = Transaction.ClientDueBills,
            //              EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
            //          })
            //          .AsEnumerable()
            //          .Select(s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
            //          {
            //              TransactionID = s.Transaction.TransactionID,
            //              Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
            //              ClientDetailsID = s.Transaction.ClientDetailsID,
            //              ClientName = s.Transaction.ClientDetails.Name,
            //              Address = s.Transaction.ClientDetails.Address,
            //              ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //              ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
            //              PackageID = s.Transaction.PackageID.Value,
            //              PackageName = s.Transaction.Package.PackageName,
            //              MonthlyFee = s.Transaction.Package.PackagePrice,
            //              FeeForThisMonth = s.Transaction.PaymentAmount.Value,
            //              PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentAmount.Value : 0,
            //              Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
            //              //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //              //    ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
            //              //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //              //            ? db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).FirstOrDefault().DueAmount : 0,
            //              PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //              CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //              PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
            //              RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
            //              ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
            //              //LineStatusID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
            //              StatusThisMonthID = s.Transaction.LineStatusID.Value,
            //              Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""

            //          }).ToList();




            ////List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            //////var lstTransaction = db.Transaction
            //////    .Join(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new VM_Transaction_ClientDueBills { Transaction = Transaction, ClientDueBills = ClientDueBills })
            //////    .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.Transaction.PaymentStatus == 0)// && s.ClientDueBills.DueAmount != 0
            //////    .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus)).Select(s => s.ToList()).ToList();

            ////var lst = db.Transaction
            ////   .GroupJoin(
            ////       db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            ////       , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            ////       )
            ////       .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.Transaction.PaymentStatus == 0)
            ////       .Select(g => new VM_Transaction_ClientDueBills { Transaction = g.Transaction, ClientDueBills = g.ClientDueBills.FirstOrDefault() })
            ////       .ToList();


            //////var lst = db.Transaction
            //////    .GroupJoin(
            //////        db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            //////        , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            //////        )
            //////        .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //////        .Select(g => new { Transaction = g.Transaction, clientDueBills = g.ClientDueBills })
            //////        .ToList();
            //////var lsts = db.Transaction
            //////    .GroupJoin(
            //////        db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID
            //////        , (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills }
            //////        )
            //////        .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //////        .Select(g => new { Transaction = g.Transaction, clientDueBills = (!g.ClientDueBills.Any()) ? 0 : g.ClientDueBills.Sum(gg => ((double?)gg.DueAmount) ?? 0) })
            //////        .ToList();

            ////foreach (var items in lst)
            ////{
            ////    //if (items.Count > 0)
            ////    //{
            ////    //    foreach (var item in items)
            ////    //    {
            ////    //        lstVM_Transaction_ClientDueBills.Add(item);
            ////    //    }

            ////    //}

            ////    lstVM_Transaction_ClientDueBills.Add(new VM_Transaction_ClientDueBills { Transaction = items.Transaction, ClientDueBills = (items.ClientDueBills == null) ? null : items.ClientDueBills });
            ////}

            // SetBillSummary();
            //return View(lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin);
            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerCurrentMonthUnPaidBillsAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                Session["IdListForSMSSend"] = null;

                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   


                bool isCheckAllFromCln = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };

                int zoneFromDDL = 0;
                int resellerFromDDL = 0;

                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");

                var IsCheckAll = Request.Form.Get("IsCheckAll");

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }


                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();

                #region linqUnpaidBills
                //var firstPartOfQuery =
                //        (YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //            : (YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                : (YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                    : (YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                        : (YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                            :
                //                            db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //    ;
                //var secondPartOfQuery = firstPartOfQuery
                //    .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                //        ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                //        {
                //            Transaction = Transaction,
                //            ClientDueBills = ClientDueBills
                //        })
                //    .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                //        (Transaction, EmployeeTransactionLockUnlock) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                //        })

                //        .GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = Transaction.EmployeeTransactionLockUnlock,
                //            ClientLineStatus = ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault(),
                //        })
                //    //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //    //{
                //    //      Transaction = Transaction.Transaction,

                //    //})
                //    .AsEnumerable();


                //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                //    Phone = s.Transaction.ClientDetails.ContactNumber
                //}).ToList();
                //// Verification.   
                //if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                //{

                //    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                //                                                                           p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                                           || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                                        //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        )
                //                                                                        .Count() : 0;

                //    // Apply search   
                //    secondPartOfQuery = secondPartOfQuery.Where(p =>
                //                                                        p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                        || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                     //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                     //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                     //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                     || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                       ).AsEnumerable();
                //    //    var a = secondPartOfQuery.ToList();
                //}
                //if (secondPartOfQuery.Count() > 0)
                //{
                //    totalRecords = secondPartOfQuery.Count();
                //    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                //        s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                //        {
                //            chkSMS = CheckOrNot(s.Transaction.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),

                //            TransactionID = s.Transaction.TransactionID,
                //            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                //            ClientDetailsID = s.Transaction.ClientDetailsID,
                //            ClientName = s.Transaction.ClientDetails.Name,
                //            ClientLoginName = s.Transaction.ClientDetails.LoginName,
                //            Address = s.Transaction.ClientDetails.Address,
                //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                //            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                //            PackageID = s.Transaction.PackageID.Value,
                //            PackageName = s.Transaction.Package.PackageName,
                //            MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                //            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                //            //PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            ////Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            ////            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            //Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidAmount = s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0,//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            Discount = s.Transaction.Discount != null ? s.Transaction.Discount.Value : 0,//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                //            Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                //            PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                //            RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                //            ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                //            StatusThisMonthID = s.Transaction.LineStatusID.Value,
                //            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
                //            //StatusNextMonthID = ,
                //            Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient,
                //            //LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                //        }).ToList();

                //}

                //// Sorting.   
                //lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                //// Total record count.   
                //// totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //// Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = lstArchiveBillsInformation
                //}, JsonRequestBehavior.AllowGet);
                #endregion

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SP_UnpaidBills", sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                        sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                        sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                        sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                        sqlCmd.Parameters.Add("@UnpainPaidBillCheckedID", SqlDbType.Int).Value = AppUtils.PaymentIsNotPaid;
                        sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                        sqlCmd.Parameters.Add("@ResellerID", AppUtils.GetLoginUserID());


                        SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(UnpaidClientList);
                        SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                        SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        sqlCmd.Parameters.Add(testValue);

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
                                VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                SetUnpaidBillOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                lstArchiveBillsInformation.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";
                            //get output param list
                            //int Count1 = Convert.ToInt32(sqlCmd.Parameters["@Out1"].Value);
                            //int Count2 = Convert.ToInt32(sqlCmd.Parameters["@Out2"].Value);
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
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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


        [UserRIghtCheck(ControllerValue = AppUtils.View_Unpaid_Bills_List_Reseller_Clients_By_Admin)]
        public ActionResult ResellerUnpaidBillsByAdmin()
        {
            ViewBag.Title = "Unpaid Bills";
            setViewBagListForResellerAccountsByAdmin();
            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerCurrentMonthUnPaidBillsByAdminAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                Session["IdListForSMSSend"] = null;

                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   


                bool isCheckAllFromCln = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };

                int zoneFromDDL = 0;
                int resellerFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");

                var IsCheckAll = Request.Form.Get("IsCheckAll");

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
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
                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
                List<SendSMSCustomInformation> lstSendSMSCustomInformation = new List<SendSMSCustomInformation>();

                #region linqUnpaidBills
                //var firstPartOfQuery =
                //        (YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //            : (YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                : (YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                    : (YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                        : (YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //                            :
                //                            db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).AsQueryable()
                //    ;
                //var secondPartOfQuery = firstPartOfQuery
                //    .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                //        ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                //        {
                //            Transaction = Transaction,
                //            ClientDueBills = ClientDueBills
                //        })
                //    .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                //        (Transaction, EmployeeTransactionLockUnlock) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                //        })

                //        .GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //        {
                //            Transaction = Transaction.Transaction,
                //            ClientDueBills = Transaction.ClientDueBills,
                //            EmployeeTransactionLockUnlock = Transaction.EmployeeTransactionLockUnlock,
                //            ClientLineStatus = ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).FirstOrDefault(),
                //        })
                //    //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //    //{
                //    //      Transaction = Transaction.Transaction,

                //    //})
                //    .AsEnumerable();


                //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                //{
                //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                //    Phone = s.Transaction.ClientDetails.ContactNumber
                //}).ToList();
                //// Verification.   
                //if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                //{

                //    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                //                                                                           p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                                           || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                                        || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                                        //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                                        //|| p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower())
                //                                                                        //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                        )
                //                                                                        .Count() : 0;

                //    // Apply search   
                //    secondPartOfQuery = secondPartOfQuery.Where(p =>
                //                                                        p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                //                                                        || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                //                                                     //|| p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                //                                                     || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                //                                                     //|| p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                //                                                     //|| (p.Transaction.RemarksNo != null ? p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                     || (p.Transaction.ResetNo != null ? p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                //                                                                       ).AsEnumerable();
                //    //    var a = secondPartOfQuery.ToList();
                //}
                //if (secondPartOfQuery.Count() > 0)
                //{
                //    totalRecords = secondPartOfQuery.Count();
                //    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                //        s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                //        {
                //            chkSMS = CheckOrNot(s.Transaction.ClientDetailsID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),

                //            TransactionID = s.Transaction.TransactionID,
                //            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                //            ClientDetailsID = s.Transaction.ClientDetailsID,
                //            ClientName = s.Transaction.ClientDetails.Name,
                //            ClientLoginName = s.Transaction.ClientDetails.LoginName,
                //            Address = s.Transaction.ClientDetails.Address,
                //            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                //            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                //            PackageID = s.Transaction.PackageID.Value,
                //            PackageName = s.Transaction.Package.PackageName,
                //            MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                //            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                //            //PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            ////Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            ////            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            //Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidAmount = s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0,//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                //            //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                //            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                //            Discount = s.Transaction.Discount != null ? s.Transaction.Discount.Value : 0,//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
                //            Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                //            PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                //            PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                //            RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                //            ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                //            StatusThisMonthID = s.Transaction.LineStatusID.Value,
                //            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
                //            //StatusNextMonthID = ,
                //            Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                //            IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient,
                //            //LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                //        }).ToList();

                //}

                //// Sorting.   
                //lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                //// Total record count.   
                //// totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //// Filter record count.   
                //recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = lstArchiveBillsInformation
                //}, JsonRequestBehavior.AllowGet);
                #endregion
                if (resellerFromDDL > 0)
                {
                    using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SP_UnpaidBills", sqlConn))
                        {
                            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add("@yearID", SqlDbType.NVarChar).Value = YearID;
                            sqlCmd.Parameters.Add("@monthID", MonthID.ToString());
                            sqlCmd.Parameters.Add("@zoneID", ZoneID.ToString());
                            sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                            sqlCmd.Parameters.Add("@runningYear", AppUtils.RunningYear.ToString());
                            sqlCmd.Parameters.Add("@runnigMonth", AppUtils.RunningMonth.ToString());
                            sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                            sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;
                            sqlCmd.Parameters.Add("@UnpainPaidBillCheckedID", SqlDbType.Int).Value = AppUtils.PaymentIsNotPaid;
                            sqlCmd.Parameters.Add("@WhichClient", SqlDbType.Int).Value = AppUtils.ResellerUser;
                            sqlCmd.Parameters.Add("@ResellerID", resellerFromDDL);


                            SqlParameter UnpaidClientList = new SqlParameter("@UnpaidClientList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                            sqlCmd.Parameters.Add(UnpaidClientList);
                            SqlParameter UnpaidClientIDAndPhoneList = new SqlParameter("@UnpaidClientIDAndPhoneList", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                            sqlCmd.Parameters.Add(UnpaidClientIDAndPhoneList);
                            SqlParameter testValue = new SqlParameter("@testValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            sqlCmd.Parameters.Add(testValue);

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
                                    VM_Transaction_EmpTraLockUnlock_ClientDueBills tec = new VM_Transaction_EmpTraLockUnlock_ClientDueBills();
                                    SetUnpaidBillOneByOneInTheList(ds, ref tec, i, isCheckAllFromCln, IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
                                    lstArchiveBillsInformation.Add(tec);
                                }
                                watch.Stop();
                                var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                                var totalsecRequred = watch.Elapsed.Seconds + " s";
                                //get output param list
                                //int Count1 = Convert.ToInt32(sqlCmd.Parameters["@Out1"].Value);
                                //int Count2 = Convert.ToInt32(sqlCmd.Parameters["@Out2"].Value);
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
                    lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                }

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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

        private void SetUnpaidBillOneByOneInTheList(DataSet ds, ref VM_Transaction_EmpTraLockUnlock_ClientDueBills tec, int i, bool isCheckAllFromCln, int[] IfIsCheckAllThenNonCheckLists, int[] IfNotCheckAllThenCheckLists)
        {
            tec.chkSMS = CheckOrNot((int)ds.Tables[0].Rows[i]["ClientDetailsID"], isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists);
            tec.TransactionID = (int)ds.Tables[0].Rows[i]["TransactionID"];
            tec.Paid = (int)ds.Tables[0].Rows[i]["Paid"] == AppUtils.PaymentIsPaid ? true : false;
            tec.ClientDetailsID = (int)ds.Tables[0].Rows[i]["ClientDetailsID"];
            tec.ClientName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ClientName"].ToString()) ? "" : ds.Tables[0].Rows[i]["ClientName"].ToString();
            tec.ClientLoginName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ClientLoginName"].ToString()) ? "" : ds.Tables[0].Rows[i]["ClientLoginName"].ToString();
            //tec.UserID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserID"].ToString()) ? "" : ds.Tables[0].Rows[i]["UserID"].ToString();
            tec.Address = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Addres"].ToString()) ? "" : ds.Tables[0].Rows[i]["Addres"].ToString();
            tec.ContactNumber = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ContactNumber"].ToString()) ? "" : ds.Tables[0].Rows[i]["ContactNumber"].ToString();
            tec.ZoneName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ZoneName"].ToString()) ? "" : ds.Tables[0].Rows[i]["ZoneName"].ToString();
            tec.PackageID = (int)ds.Tables[0].Rows[i]["PackageID"];
            tec.PackageName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PackageName"].ToString()) ? "" : ds.Tables[0].Rows[i]["PackageName"].ToString();
            tec.MonthlyFee = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["MonthlyFee"]), 2);
            tec.FeeForThisMonth = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["FeeForThisMonth"]), 2);
            tec.PaidAmount = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PaidAmount"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[0].Rows[i]["PaidAmount"]);//s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                                                                                                                                                            //              tec.Due = (double)ds.Tables[0].Rows[i]["Due"];
                                                                                                                                                            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
            tec.Discount = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Discount"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[0].Rows[i]["Discount"]);//(s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid && s.Transaction.Discount != null) ? Math.Round(s.Transaction.Discount.Value, 2) : 0,
            tec.Due = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Due"].ToString()) ? 0 : Convert.ToDouble(ds.Tables[0].Rows[i]["Due"]);
            tec.PaymentStatusFullyOrPartiallyOrNotPaid = PaymentStatusFullyOrPartiallyOrNotPaid(tec.FeeForThisMonth, tec.PaidAmount, tec.Due, tec.Discount);
            //tec.PaidBy = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PaidBy"].ToString()) ? "" : ds.Tables[0].Rows[i]["PaidBy"].ToString();
            //tec.CollectBy = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CollectBy"].ToString()) ? "" : ds.Tables[0].Rows[i]["CollectBy"].ToString();
            //tec.PaidTime = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PaidTime"].ToString()) ? "" : ds.Tables[0].Rows[i]["PaidTime"].ToString();
            //tec.RemarksNo = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RemarksNo"].ToString()) ? "" : ds.Tables[0].Rows[i]["RemarksNo"].ToString();
            //tec.ReceiptNo = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RemarksNo"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResetNo"].ToString();
            tec.StatusThisMonthID = (int)ds.Tables[0].Rows[i]["StatusThisMonthID"];
            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
            //StatusNextMonthID = ,
            tec.Employeename = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Employeename"].ToString()) ? "" : ds.Tables[0].Rows[i]["Employeename"].ToString();
            tec.IsPriorityClient = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPriorityClient"]);
            tec.LineStatusActiveDate = !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString()) ? ds.Tables[0].Rows[i]["LineStatusWillActiveInThisDate"].ToString() + " " + AppUtils.GetStatusDivByStatusID(int.Parse(ds.Tables[0].Rows[i]["LineStatusID"].ToString())) : "";
            tec.PermanentDiscount = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PermanentDiscount"].ToString()) ? 0 : (double)ds.Tables[0].Rows[i]["PermanentDiscount"];

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDueBillsListBySearchCriteria(int? YearID, int? MonthID, int? ZoneID, int? ResellerID = null)
        {
            int? _resellerID = null;
            if (ResellerID > 0 && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = ResellerID;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = null;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                _resellerID = AppUtils.GetLoginUserID();
            }
            else
            {

            }

            string date = "";
            try
            {
                IEnumerable<VM_Transaction_ClientDueBills> lstTransactMonthlyBills = new List<VM_Transaction_ClientDueBills>();
                List<Transaction> lstTransactOnlySignUpBill = new List<Transaction>();
                List<Expense> lstExpenses = new List<Expense>();

                if (YearID != null && MonthID != null && ZoneID != null)
                {
                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value) + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }
                else if (YearID != null && MonthID != null && ZoneID == null)
                {
                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value);
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }
                else if (YearID != null && ZoneID != null && MonthID == null)
                {
                    date = YearID.Value.ToString() + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }
                else if (YearID != null && MonthID == null && ZoneID == null)
                {
                    date = YearID.Value.ToString();
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }
                else if (YearID == null && MonthID == null && ZoneID != null)
                {

                    YearID = AppUtils.RunningYear;
                    MonthID = AppUtils.RunningMonth;
                    date = "All Year and Month Based On Zone : " + db.Zone.Find(ZoneID).ZoneName;
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }
                else
                {
                    YearID = AppUtils.RunningYear;
                    MonthID = AppUtils.RunningMonth;
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                }

                dynamic billSummaryDetailss = new ExpandoObject();
                //if (ss.Count > 0)
                {
                    SetBillSummaryForAjaxCall(/*lstTransactMonthlyBills, lstTransactOnlySignUpBill,*/ lstExpenses, ref billSummaryDetailss, YearID, MonthID, ZoneID, _resellerID);
                }
                return Json(new { Success = true/*, lstTransaction = ss*/, billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Date = date }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDueBillsListBySearchCriteriaByAdmin(int? YearID, int? MonthID, int? ZoneID, int? ResellerID = null)
        {
            int? _resellerID = null;
            string date = "";
            if (ResellerID > 0 && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = ResellerID;
                if (ResellerID > 0 && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
                {
                    _resellerID = ResellerID;
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
                {
                    _resellerID = null;
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    _resellerID = ResellerID;
                }
                else
                {

                }

                try
                {
                    IEnumerable<VM_Transaction_ClientDueBills> lstTransactMonthlyBills = new List<VM_Transaction_ClientDueBills>();
                    List<Transaction> lstTransactOnlySignUpBill = new List<Transaction>();
                    List<Expense> lstExpenses = new List<Expense>();

                    if (YearID != null && MonthID != null && ZoneID != null)
                    {
                        date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value) + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }
                    else if (YearID != null && MonthID != null && ZoneID == null)
                    {
                        date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value);
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }
                    else if (YearID != null && ZoneID != null && MonthID == null)
                    {
                        date = YearID.Value.ToString() + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }
                    else if (YearID != null && MonthID == null && ZoneID == null)
                    {
                        date = YearID.Value.ToString();
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }
                    else if (YearID == null && MonthID == null && ZoneID != null)
                    {
                        YearID = AppUtils.RunningYear;
                        MonthID = AppUtils.RunningMonth;
                        date = "All Year and Month Based On Zone : " + db.Zone.Find(ZoneID).ZoneName;
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }
                    else
                    {
                        YearID = AppUtils.RunningYear;
                        MonthID = AppUtils.RunningMonth;
                        //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    }

                    dynamic billSummaryDetailss = new ExpandoObject();
                    //if (ss.Count > 0)
                    {
                        SetBillSummaryForAjaxCall(/*lstTransactMonthlyBills, lstTransactOnlySignUpBill,*/ lstExpenses, ref billSummaryDetailss, YearID, MonthID, ZoneID, _resellerID);
                    }
                    return Json(new { Success = true/*, lstTransaction = ss*/, billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Date = date }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                dynamic billSummaryDetailss = new ExpandoObject();
                return Json(new { Success = true, /*lstTransaction = ss,*/ billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]///i am here
        public ActionResult GetBillsListBySearchCriteria(int? YearID, int? MonthID, int? ZoneID, int? ResellerID = null)
        {
            int? _resellerID = null;

            if (ResellerID > 0 && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = ResellerID;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = null;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                _resellerID = AppUtils.GetLoginUserID();
            }
            else
            {

            }

            string date = "";
            try
            {
                IEnumerable<VM_Transaction_ClientDueBills> lstTransactMonthlyBills = new List<VM_Transaction_ClientDueBills>();

                List<Transaction> lstTransactOnlySignUpBill = new List<Transaction>();
                List<Expense> lstExpenses = new List<Expense>();


                if (YearID != null && MonthID != null && ZoneID != null)
                {
                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value) + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                }
                else if (YearID != null && MonthID != null && ZoneID == null)
                {
                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value);
                }
                else if (YearID != null && ZoneID != null && MonthID == null)
                {
                    date = YearID.Value.ToString() + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                }
                else if (YearID != null && MonthID == null && ZoneID == null)
                {
                    date = YearID.Value.ToString();
                }
                else if (YearID == null && MonthID == null && ZoneID != null)
                {
                    YearID = AppUtils.RunningYear;
                    MonthID = AppUtils.RunningMonth;
                    date = "All Year and Month Based On Zone : " + db.Zone.Find(ZoneID).ZoneName;
                }
                else
                {
                    YearID = AppUtils.RunningYear;
                    MonthID = AppUtils.RunningMonth;
                }
                //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID, _resellerID);

                dynamic billSummaryDetailss = new ExpandoObject();
                //if (ss.Any())
                //{
                SetBillSummaryForAjaxCall(/*lstTransactMonthlyBills, lstTransactOnlySignUpBill,*/ lstExpenses, ref billSummaryDetailss, YearID, MonthID, ZoneID, _resellerID);
                //}
                return Json(new { Success = true, /*lstTransaction = ss,*/ billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Date = date }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]///i am here
        public ActionResult GetBillsListBySearchCriteriaByAdmin(int? YearID, int? MonthID, int? ZoneID, int? ResellerID = null)
        {
            int? _resellerID = null;
            string date = "";

            if (ResellerID > 0 && AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                _resellerID = ResellerID;
                try
                {
                    IEnumerable<VM_Transaction_ClientDueBills> lstTransactMonthlyBills = new List<VM_Transaction_ClientDueBills>();

                    List<Transaction> lstTransactOnlySignUpBill = new List<Transaction>();
                    List<Expense> lstExpenses = new List<Expense>();


                    if (YearID != null && MonthID != null && ZoneID != null)
                    {
                        date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value) + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    }
                    else if (YearID != null && MonthID != null && ZoneID == null)
                    {
                        date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value);
                    }
                    else if (YearID != null && ZoneID != null && MonthID == null)
                    {
                        date = YearID.Value.ToString() + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    }
                    else if (YearID != null && MonthID == null && ZoneID == null)
                    {
                        date = YearID.Value.ToString();
                    }
                    else if (YearID == null && MonthID == null && ZoneID != null)
                    {
                        YearID = AppUtils.RunningYear;
                        MonthID = AppUtils.RunningMonth;
                        date = "All Year and Month Based On Zone : " + db.Zone.Find(ZoneID).ZoneName;
                    }
                    else
                    {
                        YearID = AppUtils.RunningYear;
                        MonthID = AppUtils.RunningMonth;
                    }
                    //GetExpense(ref lstExpenses, YearID, MonthID, ZoneID, _resellerID);

                    dynamic billSummaryDetailss = new ExpandoObject();
                    //if (ss.Any())
                    //{
                    SetBillSummaryForAjaxCall(/*lstTransactMonthlyBills, lstTransactOnlySignUpBill,*/ lstExpenses, ref billSummaryDetailss, YearID, MonthID, ZoneID, _resellerID);
                    //}
                    return Json(new { Success = true, /*lstTransaction = ss,*/ billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Date = date }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                dynamic billSummaryDetailss = new ExpandoObject();
                return Json(new { Success = true, /*lstTransaction = ss,*/ billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<Expense> GetExpense(ref List<Expense> lstExpenses, int? YearID, int? MonthID, int? ZoneID, int ResellerID = 0)
        {
            int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
            int month = MonthID != null ? MonthID.Value : 0;
            lstExpenses = new List<Expense>();
            if (year > 0 && month > 0)
            {
                DateTime expenseStartDateTime = new DateTime(year, month, 1);
                DateTime expenseEndDateTime = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(year, month, Convert.ToInt32(DateTime.DaysInMonth(year, month))));
                lstExpenses = db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= expenseStartDateTime && s.PaymentDate <= expenseEndDateTime).ToList();
                return lstExpenses;
            }
            else if (YearID != null)
            {
                DateTime expenseStartDateTime = new DateTime(year, 1, 1);
                DateTime daysInMonth = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                DateTime expenseEndDateTime = AppUtils.GetLastDayWithHrMinSecMsByMyDate(daysInMonth);
                lstExpenses = db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= expenseStartDateTime && s.PaymentDate <= expenseEndDateTime).ToList();
                return lstExpenses;
            }
            else
            {
                return lstExpenses;
            }
        }


        //private void SetBillSummaryForAjaxCall(ref dynamic billSummaryDetails, int? YearID, int? MonthID, int? ZoneID)
        //{// is this for all?

        //    int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
        //    int month = MonthID != null ? MonthID.Value : 0;
        //    List<Transaction> lstTransaction = new List<Transaction>();
        //    SetLstTransaction(ref lstTransaction, YearID, MonthID, ZoneID);

        //    var lstRegularMonthlyBill = lstTransaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).ToList();
        //    var lstRegularSignUpBill = lstTransaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection).Select(s => new
        //    {
        //        PaymentAmount = s.PaymentAmount
        //    }).ToList();

        //    var expense = !db.Expenses.Any() ? 0 : db.Expenses.Sum(s => s.Amount);

        //    //List<Transaction> lstTransaction = db.Transaction.ToList();
        //    //List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();
        //    //List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
        //    //List<Transaction> lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).ToList();

        //    billSummaryDetails.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
        //    billSummaryDetails.clnCollectedAmount = Math.Round(lstRegularMonthlyBill
        //        .Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount.Value) + lstRegularSignUpBill.Sum(s => s.PaymentAmount.Value), 2);
        //    billSummaryDetails.clnDiscountAmount = lstTransaction.Sum(s => s.Discount);
        //    billSummaryDetails.clnCollectedAmountBIll = lstTransaction
        //        .Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).Sum(s => s.PaymentAmount);
        //    billSummaryDetails.clnOnlinePayment = 0;

        //    billSummaryDetails.clnInstallationAmount = lstRegularSignUpBill.Sum(s => s.PaymentAmount);
        //    // ViewBag.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.PaymentAmount) - ( lstRegularMonthlyBill.Where(s=>s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s=>s.PaymentAmount)+ lstRegularMonthlyBill.Sum(s => s.Discount));
        //    billSummaryDetails.clnDueAmount = (lstRegularMonthlyBill
        //        .Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount)) < 0 ? 0 : (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount));

        //    billSummaryDetails.clnTotalExpense = expense;
        //    billSummaryDetails.clnRestOfAmount = Math.Round(((lstRegularMonthlyBill
        //        .Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s => s.PaymentAmount.Value) + lstRegularSignUpBill.Sum(s => s.PaymentAmount.Value)) - expense), 2);
        //    billSummaryDetails.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
        //    billSummaryDetails.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count();
        //    billSummaryDetails.clnUnpaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == 0).Count();
        //    billSummaryDetails.clnPreviousBillCollection = "";
        //    //if (YearID != null && MonthID != null && ZoneID != null)
        //    //{
        //    //    DateTime startDate = new DateTime(YearID.Value,MonthID.Value,1);

        //    //    ViewBag.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == year && s.PaymentMonth == month && s.ClientDetails.ZoneID == ZoneID.Value) &&  s.PaymentDate >   && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new
        //    //    {
        //    //        PaymentAmount = s.PaymentAmount
        //    //    }).Sum(s => s.PaymentAmount);
        //    //}
        //    //else if (YearID != null && MonthID != null && ZoneID != null)
        //    //{
        //    //    ViewBag.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new
        //    //    {
        //    //        PaymentAmount = s.PaymentAmount
        //    //    }).Sum(s => s.PaymentAmount);
        //    //}





        //    ////int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
        //    ////int month = MonthID != null ? MonthID.Value : 1;

        //    ////DateTime startDate = new DateTime(year, month, 01);
        //    ////DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        //    ////DateTime sDate = new DateTime();
        //    ////DateTime eDate = new DateTime();

        //    ////var lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month).AsEnumerable().Select(s => new
        //    ////{
        //    ////    ClientDetailsID =
        //    ////    s.ClientDetailsID,
        //    ////    ZoneID = s.ClientDetails.ZoneID,
        //    ////    Discount = s.Discount,
        //    ////    PaymentTypeID = s.PaymentTypeID,
        //    ////    Package = s.Package,
        //    ////    PaymentStatus = s.PaymentStatus,
        //    ////    PaymentAmount = s.PaymentAmount
        //    ////}).ToList();
        //    ////if (lstTransactionForBillSummary.Count > 0)
        //    ////{
        //    ////    if (ZoneID.Value > 0)
        //    ////    {
        //    ////        lstTransactionForBillSummary = lstTransactionForBillSummary.Where(s => s.ZoneID == ZoneID).ToList();
        //    ////    }
        //    ////}
        //    ////var lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).ToList();
        //    ////var lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new
        //    ////{
        //    ////    PaymentAmount = s.PaymentAmount
        //    ////}).ToList();
        //    ////var expense = !db.Expenses.Any() ? 0 : db.Expenses.Sum(s => s.Amount);

        //    //////List<Transaction> lstTransaction = db.Transaction.ToList();
        //    //////List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();
        //    //////List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
        //    //////List<Transaction> lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).ToList();

        //    ////ViewBag.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice);
        //    ////ViewBag.clnCollectedAmount = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount);
        //    ////ViewBag.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
        //    ////ViewBag.clnCollectedAmountBIll = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount);
        //    ////ViewBag.clnOnlinePayment = 0;
        //    ////List<Transaction> lstTransaction = new List<Transaction>();
        //    ////if (YearID.Value > 0 && MonthID.Value > 0)
        //    ////{
        //    ////    sDate = new DateTime(YearID.Value, MonthID.Value, 1);
        //    ////    eDate = new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value));
        //    ////}
        //    ////else if (YearID.Value > 0)
        //    ////{
        //    ////    sDate = new DateTime(YearID.Value, 1, 1);
        //    ////    eDate = new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12));
        //    ////}

        //    ////if (ZoneID != null)
        //    ////{
        //    ////    //.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= sDate && s.PaymentDate <= AppUtils.GetLastDayWithHrMinSecMsByMyDate(eDate)).Sum(ss => ss.PaymentAmount);
        //    ////    lstTransaction = db.Transaction.Where(s => s.ClientDetails.ZoneID == ZoneID.Value && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= sDate && s.PaymentDate <= AppUtils.GetLastDayWithHrMinSecMsByMyDate(eDate)).ToList();
        //    ////}
        //    ////if (lstTransaction.Count == 0)
        //    ////{
        //    ////    lstTransaction = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= sDate && s.PaymentDate <= AppUtils.GetLastDayWithHrMinSecMsByMyDate(eDate)).ToList();
        //    ////}
        //    ////ViewBag.clnInstallationAmount = lstTransaction.Sum(s=>s.PaymentAmount);
        //    ////ViewBag.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice) - ((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) + lstTransactionForBillSummary.Sum(s => s.Discount));
        //    ////ViewBag.clnTotalExpense = expense;
        //    ////ViewBag.clnRestOfAmount = (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) - expense;
        //    ////ViewBag.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
        //    ////ViewBag.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count();
        //    ////ViewBag.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count());
        //    ////ViewBag.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new
        //    ////{
        //    ////    PaymentAmount = s.PaymentAmount
        //    ////}).Sum(s => s.PaymentAmount);

        //    //List<Transaction> lstTransaction = db.Transaction.ToList();
        //    //List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month).ToList();
        //    //List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
        //    //List<Transaction> lstRegularSignUpBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).ToList();

        //    //billSummaryDetails.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice);
        //    //billSummaryDetails.clnCollectedAmount = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount);
        //    //billSummaryDetails.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
        //    //billSummaryDetails.clnCollectedAmountBIll = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount);
        //    //billSummaryDetails.clnOnlinePayment = 0;
        //    //billSummaryDetails.clnInstallationAmount = "? is this";
        //    //billSummaryDetails.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice) - ((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) + lstTransactionForBillSummary.Sum(s => s.Discount));
        //    //billSummaryDetails.clnTotalExpense = "? is this";
        //    //billSummaryDetails.clnRestOfAmount = "? is this"; ;
        //    //billSummaryDetails.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
        //    //billSummaryDetails.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count();
        //    //billSummaryDetails.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count());
        //    //billSummaryDetails.clnPreviousBillCollection = lstTransaction.Where(s => !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.PaymentAmount);


        //}

        private double CalculationForShowingDueBills(int ClientDetailsID)
        {
            double returnResult = db.ClientDueBills.Where(s => s.ClientDetailsID == ClientDetailsID && s.Status == true).FirstOrDefault() != null
                ? Math.Round(
                    db.ClientDueBills.Where(ss => ss.ClientDetailsID == ClientDetailsID && ss.Status == true)
                        .FirstOrDefault().DueAmount, 2)
                : 0;
            return returnResult;
        }

        private float GetMoneyAfterCount(int transactionID)
        {
            Transaction ts = db.Transaction.Where(s => s.TransactionID == transactionID).FirstOrDefault();
            if (ts != null)
            {
                return ts.PaymentAmount.Value;
            }
            return 0;
        }

        private float GetMoneyAfterCount()
        {
            throw new NotImplementedException();
        }

        private int Function(int value, int transactionID, int PackageID, DateTime dtNow)
        {
            if (value == AppUtils.LineIsLock)
            {
                Package package = db.Package.Find(PackageID);
                int TotalDayInMonth = DateTime.DaysInMonth(AppUtils.RunningYear, AppUtils.RunningMonth);
                Double perDayBill = package.PackagePrice / TotalDayInMonth;
                int DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - dtNow.Date).TotalDays) + 1;
                Double clientNeedtoPayForMakeLineInActive = 0;

                EmployeeTransactionLockUnlock employeeTransactionLockUnlock = db.EmployeeTransactionLockUnlock
                    .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth)
                    .FirstOrDefault();
                if (employeeTransactionLockUnlock != null)
                {
                    clientNeedtoPayForMakeLineInActive =
                        (perDayBill * DaysRemains) + employeeTransactionLockUnlock.Amount;
                }

                Transaction ts = db.Transaction.Where(s => s.TransactionID == transactionID).FirstOrDefault();
                if (ts != null)
                {
                    float amount = 0;
                    float.TryParse(clientNeedtoPayForMakeLineInActive.ToString(), out amount);

                    ts.PaymentAmount = amount;
                }



            }

            return value;
        }


        [UserRIghtCheck(ControllerValue = AppUtils.View_Sign_Up_Bills_List)]
        public ActionResult SignUpBills()
        {
            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");


            ViewBag.Title = "Sign Up Bills";
            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());

            setViewBagList();

            //VM_ClientLineStatus_Transaction VM_ClientLineStatus_Transaction = new VM_ClientLineStatus_Transaction();
            ////var lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).ToList();

            //var lstTransaction = db.Transaction.Where(s =>
            //        s.PaymentDate >= startDate && s.PaymentDate <= endDate &&
            //        s.PaymentStatus == AppUtils.PaymentIsPaid &&
            //        s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //    .GroupJoin(
            //        db.Transaction.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month),
            //        TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID,
            //        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
            //    .Select(s => new CustomSignUpBills
            //    {
            //        TransactionID = s.TCon.TransactionID,
            //        ClientDetailsID = s.TCon.ClientDetailsID,
            //        Name = s.TCon.ClientDetails.Name,
            //        Address = s.TCon.ClientDetails.Address,
            //        ContactNumber = s.TCon.ClientDetails.ContactNumber,
            //        ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
            //        PackageName = s.TMon.Package.PackageName,
            //        PackagePrice = s.TMon.PaymentAmount.ToString(),
            //        SignUpFee = s.TCon.PaymentAmount.ToString(),
            //        PaymentDate = s.TCon.PaymentDate.Value,
            //        RemarksNo = s.TCon.RemarksNo,

            //    }).ToList();


            ////db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear).ToList()).ToList();
            ////.GroupBy(s => s.PaymentStatus).Select(s => new  {TransactionID = s. });
            ////.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear)).ToList();
            ////.Select(fl => new List<Transaction>() {  PaymentStatus = fl.Select(x => x.PaymentStatus).ToList() })

            //// .GroupBy(s => s.PaymentType).ToList()

            return View(new List<CustomSignUpBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSignUpBillsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                bool forReseller = false;
                int? resellerID = null;
                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }

                DateTime startDate = AppUtils.ThisMonthStartDate();
                DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
                var transactionQueryable = db.Transaction.AsQueryable();
                //IEnumerable<Transaction> transactionEnumerable = Enumerable.Empty<Transaction>();
                setStartDateEndDateTransaction(YearID, MonthID, ZoneID, ref startDate, ref endDate, ref transactionQueryable, forReseller, resellerID);


                var enumerableData = transactionQueryable.AsEnumerable();
                //db.Transaction.Where(s =>  s.PaymentDate >= startDate && s.PaymentDate <= endDate && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                List<CustomSignUpBills> lstArchiveBillsInformation = new List<CustomSignUpBills>();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (transactionQueryable.Any()) ? transactionQueryable.Where(p =>
                                                                                        //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                        /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                                        //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                                                                                        || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                        /*p.RemarksNo.ToString().ToLower().Contains(search.ToLower())*/).Count() : 0;

                    // Apply search   
                    transactionQueryable = transactionQueryable.Where(p =>
                        //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                        /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                        //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                        || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                        || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                        //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                        || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))).AsQueryable();
                }
                //var idlist = transactionEnumerable.Select(s => s.TransactionID).ToList();
                //var a = transactionEnumerable.ToList();
                var secondPartOfQuery =
                    transactionQueryable
                    .GroupJoin(
                        db.Transaction/*.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month)*/,
                        TCon => TCon.TransactionID, TMon => TMon.ForWhichSignUpBills,
                        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })

                        .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), TCon => TCon.TCon.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (TCon, ClientLineStatus) => new
                        {
                            TCon = TCon.TCon,
                            TMon = TCon.TMon,
                            ClientLineStatus = ClientLineStatus.FirstOrDefault(),
                        })
                    .AsEnumerable();

                // Verification.    

                //var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                //                          join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                //                          from cblscka in a.DefaultIfEmpty()
                //                          join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                //                          from cblTypeb in b.DefaultIfEmpty()

                //                          select new
                //                          {
                //                              CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                //                              CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                //                              AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                //                          }).GroupBy(s => s.CableTypeID).ToList();

                //var itemForThisClient =
                //    (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                //     join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                //     from clnproddistribution in a.DefaultIfEmpty()

                //     join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                //     from clnstock in b.DefaultIfEmpty()

                //     join items in db.Item on clnstock.ItemID equals items.ItemID into c
                //     from clnItems in c.DefaultIfEmpty()

                //     select new
                //     {
                //         ItemID = clnItems.ItemID,
                //         ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                //         ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                //     }).GroupBy(s => s.ItemID).ToList();

                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                        s => new CustomSignUpBills
                        {
                            TransactionID = s.TCon.TransactionID,
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            ClientLoginName = s.TCon.ClientDetails.LoginName,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = /*s.TMon.PaymentStatus == AppUtils.PaymentIsPaid ?*/ s.TMon.PaymentAmount.ToString() /*: "----"*/,
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.RemarksNo,
                            IsPriorityClient = s.TCon.ClientDetails.IsPriorityClient,
                            CreateRemarks = s.TCon.ClientDetails.Remarks,
                            Reference = s.TCon.ClientDetails.Reference,
                            CreateBy = s.TCon.ClientDetails.CreateBy,
                            GivenDetails = GetGivenDetails(s.TCon.ClientDetailsID),
                            GivenCableDetails = (string)TempData["totalCableGivenList"],
                            GivenItemsDetails = (string)TempData["totalItemGivenList"],
                            ItemInstalledEmployeeNameList = (string)TempData["lstEmployeeForClient"],
                            // LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                        }).ToList();

                }

                // Sorting.   
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
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
                    data = lstArchiveBillsInformation
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

        private void setStartDateEndDateTransaction(string YearID, string MonthID, string ZoneID, ref DateTime startDate, ref DateTime endDate, ref IQueryable<Transaction> transactionQuerable, bool forReseller = false, int? resellerID = null)
        {
            if (YearID != "" && MonthID != "" && ZoneID != "")
            {
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));
                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && (s.PaymentDate >= sd && s.PaymentDate <= ed) /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
            }
            else if (YearID != "" && MonthID != "" && ZoneID == "")
            {
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));

                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && (s.PaymentDate >= sd && s.PaymentDate <= ed) /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
            else if (YearID != "" && MonthID == "" && ZoneID != "")
            {
                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));

                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && (s.PaymentDate >= sd && s.PaymentDate <= ed) /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
            }
            else if (YearID != "" && MonthID == "" && ZoneID == "")
            {
                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));

                DateTime sd = startDate;
                DateTime ed = endDate;
                //var aa = db.Transaction.Where(s => s.TransactionID == 8157).FirstOrDefault();
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && (s.PaymentDate >= sd && s.PaymentDate <= ed) /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
                //var a = transactionEnumerable.ToList();
            }
            else if (YearID == "" && MonthID == "" && ZoneID != "")
            {
                startDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, DateTime.DaysInMonth(AppUtils.RunningYear, AppUtils.RunningMonth)));

                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
            else
            {
                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionQuerable = db.Transaction.Where(s => s.ResellerID == resellerID && s.PaymentDate >= sd && s.PaymentDate <= ed /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_Sign_Up_Bills_List_Reseller_Clients)]
        public ActionResult ResellerSignUpBills()
        {
            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");


            ViewBag.Title = "Sign Up Bills";
            var forReseller = true;
            int resellerID = AppUtils.GetLoginUserID();
            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());

            setViewBagList(forReseller, resellerID);

            //VM_ClientLineStatus_Transaction VM_ClientLineStatus_Transaction = new VM_ClientLineStatus_Transaction();
            ////var lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).ToList();

            //var lstTransaction = db.Transaction.Where(s =>
            //        s.PaymentDate >= startDate && s.PaymentDate <= endDate &&
            //        s.PaymentStatus == AppUtils.PaymentIsPaid &&
            //        s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //    .GroupJoin(
            //        db.Transaction.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month),
            //        TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID,
            //        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
            //    .Select(s => new CustomSignUpBills
            //    {
            //        TransactionID = s.TCon.TransactionID,
            //        ClientDetailsID = s.TCon.ClientDetailsID,
            //        Name = s.TCon.ClientDetails.Name,
            //        Address = s.TCon.ClientDetails.Address,
            //        ContactNumber = s.TCon.ClientDetails.ContactNumber,
            //        ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
            //        PackageName = s.TMon.Package.PackageName,
            //        PackagePrice = s.TMon.PaymentAmount.ToString(),
            //        SignUpFee = s.TCon.PaymentAmount.ToString(),
            //        PaymentDate = s.TCon.PaymentDate.Value,
            //        RemarksNo = s.TCon.RemarksNo,

            //    }).ToList();


            ////db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear).ToList()).ToList();
            ////.GroupBy(s => s.PaymentStatus).Select(s => new  {TransactionID = s. });
            ////.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear)).ToList();
            ////.Select(fl => new List<Transaction>() {  PaymentStatus = fl.Select(x => x.PaymentStatus).ToList() })

            //// .GroupBy(s => s.PaymentType).ToList()

            return View(new List<CustomSignUpBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerSignUpBillsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                bool forReseller = false;
                int? resellerID = AppUtils.GetLoginUserID();
                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }

                DateTime startDate = AppUtils.ThisMonthStartDate();
                DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
                var transactionQueryable = db.Transaction.AsQueryable();
                //IEnumerable<Transaction> transactionEnumerable = Enumerable.Empty<Transaction>();
                setStartDateEndDateTransaction(YearID, MonthID, ZoneID, ref startDate, ref endDate, ref transactionQueryable, forReseller, resellerID);


                var enumerableData = transactionQueryable.AsEnumerable();
                //db.Transaction.Where(s =>  s.PaymentDate >= startDate && s.PaymentDate <= endDate && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                List<CustomSignUpBills> lstArchiveBillsInformation = new List<CustomSignUpBills>();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (transactionQueryable.Any()) ? transactionQueryable.Where(p =>
                                                                                        //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                        /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                                        //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                                                                                        || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                        /*p.RemarksNo.ToString().ToLower().Contains(search.ToLower())*/).Count() : 0;

                    // Apply search   
                    transactionQueryable = transactionQueryable.Where(p =>
                        //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                        /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                        //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                        || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                        || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                        //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                        || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))).AsQueryable();
                }
                //var idlist = transactionEnumerable.Select(s => s.TransactionID).ToList();
                //var a = transactionEnumerable.ToList();
                var secondPartOfQuery =
                    transactionQueryable
                    .GroupJoin(
                        db.Transaction/*.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month)*/,
                        TCon => TCon.TransactionID, TMon => TMon.ForWhichSignUpBills,
                        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })

                        .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), TCon => TCon.TCon.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (TCon, ClientLineStatus) => new
                        {
                            TCon = TCon.TCon,
                            TMon = TCon.TMon,
                            ClientLineStatus = ClientLineStatus.FirstOrDefault(),
                        })
                    .AsEnumerable();

                // Verification.    

                //var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                //                          join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                //                          from cblscka in a.DefaultIfEmpty()
                //                          join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                //                          from cblTypeb in b.DefaultIfEmpty()

                //                          select new
                //                          {
                //                              CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                //                              CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                //                              AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                //                          }).GroupBy(s => s.CableTypeID).ToList();

                //var itemForThisClient =
                //    (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                //     join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                //     from clnproddistribution in a.DefaultIfEmpty()

                //     join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                //     from clnstock in b.DefaultIfEmpty()

                //     join items in db.Item on clnstock.ItemID equals items.ItemID into c
                //     from clnItems in c.DefaultIfEmpty()

                //     select new
                //     {
                //         ItemID = clnItems.ItemID,
                //         ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                //         ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                //     }).GroupBy(s => s.ItemID).ToList();

                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                        s => new CustomSignUpBills
                        {
                            TransactionID = s.TCon.TransactionID,
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            ClientLoginName = s.TCon.ClientDetails.LoginName,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = /*s.TMon.PaymentStatus == AppUtils.PaymentIsPaid ?*/ s.TMon.PaymentAmount.ToString() /*: "----"*/,
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.RemarksNo,
                            IsPriorityClient = s.TCon.ClientDetails.IsPriorityClient,
                            CreateRemarks = s.TCon.ClientDetails.Remarks,
                            Reference = s.TCon.ClientDetails.Reference,
                            CreateBy = s.TCon.ClientDetails.CreateBy,
                            GivenDetails = GetGivenDetails(s.TCon.ClientDetailsID),
                            GivenCableDetails = (string)TempData["totalCableGivenList"],
                            GivenItemsDetails = (string)TempData["totalItemGivenList"],
                            ItemInstalledEmployeeNameList = (string)TempData["lstEmployeeForClient"],
                            // LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                        }).ToList();

                }

                // Sorting.   
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
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
                    data = lstArchiveBillsInformation
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


        [UserRIghtCheck(ControllerValue = AppUtils.View_Sign_Up_Bills_List_Reseller_Clients_By_Admin)]
        public ActionResult ResellerSignUpBillsByAdmin()
        {
            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.Title = "Sign Up Bills";
            setViewBagListForResellerAccountsByAdmin();

            //VM_ClientLineStatus_Transaction VM_ClientLineStatus_Transaction = new VM_ClientLineStatus_Transaction();
            ////var lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).ToList();

            //var lstTransaction = db.Transaction.Where(s =>
            //        s.PaymentDate >= startDate && s.PaymentDate <= endDate &&
            //        s.PaymentStatus == AppUtils.PaymentIsPaid &&
            //        s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //    .GroupJoin(
            //        db.Transaction.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month),
            //        TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID,
            //        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
            //    .Select(s => new CustomSignUpBills
            //    {
            //        TransactionID = s.TCon.TransactionID,
            //        ClientDetailsID = s.TCon.ClientDetailsID,
            //        Name = s.TCon.ClientDetails.Name,
            //        Address = s.TCon.ClientDetails.Address,
            //        ContactNumber = s.TCon.ClientDetails.ContactNumber,
            //        ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
            //        PackageName = s.TMon.Package.PackageName,
            //        PackagePrice = s.TMon.PaymentAmount.ToString(),
            //        SignUpFee = s.TCon.PaymentAmount.ToString(),
            //        PaymentDate = s.TCon.PaymentDate.Value,
            //        RemarksNo = s.TCon.RemarksNo,

            //    }).ToList();


            ////db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear).ToList()).ToList();
            ////.GroupBy(s => s.PaymentStatus).Select(s => new  {TransactionID = s. });
            ////.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentYear)).ToList();
            ////.Select(fl => new List<Transaction>() {  PaymentStatus = fl.Select(x => x.PaymentStatus).ToList() })

            //// .GroupBy(s => s.PaymentType).ToList()

            return View(new List<CustomSignUpBills>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerSignUpBillsByAdminAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                bool forReseller = true;
                // Initialization.   

                int zoneFromDDL = 0;
                int resellerFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (ResellerID != "")
                {
                    resellerFromDDL = int.Parse(ResellerID);
                }

                List<CustomSignUpBills> lstArchiveBillsInformation = new List<CustomSignUpBills>();
                if (resellerFromDDL > 0)
                {
                    DateTime startDate = AppUtils.ThisMonthStartDate();
                    DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
                    var transactionQueryable = db.Transaction.AsQueryable();
                    setStartDateEndDateTransaction(YearID, MonthID, ZoneID, ref startDate, ref endDate, ref transactionQueryable, forReseller, resellerFromDDL);


                    var enumerableData = transactionQueryable.AsEnumerable();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {
                        ifSearch = (transactionQueryable.Any()) ? transactionQueryable.Where(p =>
                                                                                            //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                            /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                            //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                                                                                            || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                            || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                                            //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                                                                                            || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                            /*p.RemarksNo.ToString().ToLower().Contains(search.ToLower())*/).Count() : 0;

                        // Apply search   
                        transactionQueryable = transactionQueryable.Where(p =>
                            //p.ClientDetails.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                            /*||*/ p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                            //|| p.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                            || p.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                            || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                            //|| p.ClientDetails.Package.PackageName.ToLower().Contains(search.ToLower())
                            || (p.RemarksNo != null ? p.RemarksNo.ToString().ToLower().Contains(search.ToLower()) : "".Contains(search.ToLower()))).AsQueryable();
                    }
                    //var idlist = transactionEnumerable.Select(s => s.TransactionID).ToList();
                    //var a = transactionEnumerable.ToList();
                    var secondPartOfQuery =
                        transactionQueryable
                        .GroupJoin(
                            db.Transaction/*.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month)*/,
                            TCon => TCon.TransactionID, TMon => TMon.ForWhichSignUpBills,
                            (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })

                            .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), TCon => TCon.TCon.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (TCon, ClientLineStatus) => new
                            {
                                TCon = TCon.TCon,
                                TMon = TCon.TMon,
                                ClientLineStatus = ClientLineStatus.FirstOrDefault(),
                            })
                        .AsEnumerable();

                    // Verification.    

                    //var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                    //                          join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                    //                          from cblscka in a.DefaultIfEmpty()
                    //                          join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                    //                          from cblTypeb in b.DefaultIfEmpty()

                    //                          select new
                    //                          {
                    //                              CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                    //                              CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                    //                              AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                    //                          }).GroupBy(s => s.CableTypeID).ToList();

                    //var itemForThisClient =
                    //    (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                    //     join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                    //     from clnproddistribution in a.DefaultIfEmpty()

                    //     join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                    //     from clnstock in b.DefaultIfEmpty()

                    //     join items in db.Item on clnstock.ItemID equals items.ItemID into c
                    //     from clnItems in c.DefaultIfEmpty()

                    //     select new
                    //     {
                    //         ItemID = clnItems.ItemID,
                    //         ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                    //         ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                    //     }).GroupBy(s => s.ItemID).ToList();

                    if (secondPartOfQuery.Count() > 0)
                    {
                        totalRecords = secondPartOfQuery.Count();
                        lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize)
                            .Select(
                            s => new CustomSignUpBills
                            {
                                TransactionID = s.TCon.TransactionID,
                                ClientDetailsID = s.TCon.ClientDetailsID,
                                Name = s.TCon.ClientDetails.Name,
                                ClientLoginName = s.TCon.ClientDetails.LoginName,
                                Address = s.TCon.ClientDetails.Address,
                                ContactNumber = s.TCon.ClientDetails.ContactNumber,
                                ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                                PackageName = s.TMon.Package.PackageName,
                                PackagePrice = /*s.TMon.PaymentStatus == AppUtils.PaymentIsPaid ?*/ s.TMon.PaymentAmount.ToString() /*: "----"*/,
                                SignUpFee = s.TCon.PaymentAmount.ToString(),
                                PaymentDate = s.TCon.PaymentDate.Value,
                                RemarksNo = s.TCon.RemarksNo,
                                IsPriorityClient = s.TCon.ClientDetails.IsPriorityClient,
                                CreateRemarks = s.TCon.ClientDetails.Remarks,
                                Reference = s.TCon.ClientDetails.Reference,
                                CreateBy = s.TCon.ClientDetails.CreateBy,
                                GivenDetails = GetGivenDetails(s.TCon.ClientDetailsID),
                                GivenCableDetails = (string)TempData["totalCableGivenList"],
                                GivenItemsDetails = (string)TempData["totalItemGivenList"],
                                ItemInstalledEmployeeNameList = (string)TempData["lstEmployeeForClient"],
                                // LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") : "",
                                LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                            }).ToList();
                    }

                    // Sorting.   
                    lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
                    // Total record count.   
                    // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                    // Filter record count.   
                    recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                    ////////////////////////////////////

                }


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstArchiveBillsInformation
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


        private string GetGivenDetails(int clientDetailsID)
        {
            string totalGivenListForCableAndItems = "";
            string totalItemGivenList = "";
            string totalCableGivenList = "";
            List<string> lstEmployee = new List<string>();
            string employee = "";
            var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == clientDetailsID)
                                      join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                                      from cblscka in a.DefaultIfEmpty()
                                      join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                                      from cblTypeb in b.DefaultIfEmpty()
                                      join assignedEmployee in db.Employee on cbldis.CableForEmployeeID equals assignedEmployee.EmployeeID into c
                                      from assignedEmployeeDIE in c.DefaultIfEmpty()

                                      select new
                                      {
                                          CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                                          CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                                          AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                                          AssignedEmployee = assignedEmployeeDIE.Name
                                      }).GroupBy(s => s.CableTypeID).ToList();

            var itemForThisClient =
                                    (from proddis in db.Distribution.Where(s => s.ClientDetailsID == clientDetailsID)
                                     join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                                     from clnproddistribution in a.DefaultIfEmpty()

                                     join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                                     from clnstock in b.DefaultIfEmpty()

                                     join items in db.Item on clnstock.ItemID equals items.ItemID into c
                                     from clnItems in c.DefaultIfEmpty()

                                     join assignedEmployee in db.Employee on proddis.EmployeeID equals assignedEmployee.EmployeeID into d
                                     from assignedEmployeeDIE in d.DefaultIfEmpty()
                                     select new
                                     {
                                         ItemID = clnItems.ItemID,
                                         ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                                         ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                                         AssignedEmployee = assignedEmployeeDIE.Name
                                     }).GroupBy(s => s.ItemID).ToList();

            foreach (var cable in cableForThisClient)
            {
                //string a = cableItem.FirstOrDefault().CableType;
                totalItemGivenList = cable.FirstOrDefault().CableType + ": " + cable.FirstOrDefault().AmountOfCableGiven + "      ";
                lstEmployee.Add(cable.FirstOrDefault().AssignedEmployee);
            }
            TempData["totalItemGivenList"] = totalItemGivenList;


            foreach (var Item in itemForThisClient)
            {
                //string a = cableItem.FirstOrDefault().CableType;
                totalCableGivenList = Item.FirstOrDefault().ItemName + ": " + Item.FirstOrDefault().ItemSerial + "      ";
                lstEmployee.Add(Item.FirstOrDefault().AssignedEmployee);
            }
            TempData["totalCableGivenList"] = totalCableGivenList;


            lstEmployee = lstEmployee.Distinct().ToList();
            foreach (var emp in lstEmployee)
            {
                employee += emp + ", ";
            }

            TempData["lstEmployeeForClient"] = employee;


            totalGivenListForCableAndItems = totalItemGivenList + " & " + totalCableGivenList;
            return totalGivenListForCableAndItems;
        }

        //private void setStartDateEndDateTransaction(string YearID, string MonthID, string ZoneID, ref DateTime startDate, ref DateTime endDate, ref IEnumerable<Transaction> transactionEnumerable)
        //{
        //    if (YearID != "" && MonthID != "" && ZoneID != "")
        //    {
        //        startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
        //        endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));
        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
        //    }
        //    else if (YearID != "" && MonthID != "" && ZoneID == "")
        //    {
        //        startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
        //        endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));

        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
        //    }
        //    else if (YearID != "" && MonthID == "" && ZoneID != "")
        //    {
        //        startDate = new DateTime(int.Parse(YearID), 1, 1);
        //        endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));

        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
        //    }
        //    else if (YearID != "" && MonthID == "" && ZoneID == "")
        //    {
        //        startDate = new DateTime(int.Parse(YearID), 1, 1);
        //        endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 12, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));

        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        //var aa = db.Transaction.Where(s => s.TransactionID == 8157).FirstOrDefault();
        //        transactionEnumerable = db.Transaction.Where(s => (s.PaymentDate >= sd && s.PaymentDate <= ed) /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
        //        //var a = transactionEnumerable.ToList();
        //    }
        //    else if (YearID == "" && MonthID == "" && ZoneID != "")
        //    {
        //        startDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 1);
        //        endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear,  AppUtils.RunningMonth, DateTime.DaysInMonth(AppUtils.RunningYear, 12)));

        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        transactionEnumerable = db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
        //    }
        //    else
        //    {
        //        DateTime sd = startDate;
        //        DateTime ed = endDate;
        //        transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed /*&& s.PaymentStatus == AppUtils.PaymentIsPaid*/ && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
        //    }
        //}

        [UserRIghtCheck(ControllerValue = AppUtils.View_Active_To_Lock_List)]
        public ActionResult ActiveToLockClient()
        {
            setViewBagYearMonth();

            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.ThisMonthLastDate();
            List<ClientLineStatus> lstOfLatestAllClientLineStatusInThisMonth = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => (s.LineStatusChangeDate >= startDate && s.LineStatusChangeDate <= endDate) && s.LineStatusID == 3).ToList();



            var lstClientLineStatus = db.ClientLineStatus.Where(s => s.LineStatusID == 5 && (s.LineStatusChangeDate <= startDate)).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault());
            //.Select(s => new {  s.ClientDetailsID,  s.ClientDetails.LoginName,   s.ClientDetails.Address,   s.ClientDetails.Zone.ZoneName,  s.Package.PackageName, s.Package.PackagePrice}).DefaultIfEmpty().ToList();

            if (lstClientLineStatus.ToList().Count > 0)
            {
                foreach (var item in lstClientLineStatus.ToList())
                {
                    if (lstOfLatestAllClientLineStatusInThisMonth.Where(s => s.ClientDetailsID == item.ClientDetailsID).Count() <= 0)
                    {
                        lstClientLineStatus.ToList().Remove(item);
                    }
                }
            }

            List<ClientLineStatus> lstClientLineStatuss = lstClientLineStatus.Select(s => s).ToList();
            // var lstClientLineStatuss = lstClientLineStatus.Select(s => new { ClientDetailsID = s.ClientDetailsID, LoginName = s.ClientDetails.LoginName, Address = s.ClientDetails.Address, ZoneName = s.ClientDetails.Zone.ZoneName, PackageName = (s.PackageID == null) ? "" : s.Package.PackageName, MonthlyFee = (s.PackageID == null) ? "" : (s.Package.PackagePrice).ToString() }).ToList();

            //var lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate)).FirstOrDefault().Where(s => s.LineStatusID == 3 && (s.LineStatusChangeDate >= AppUtils.ThisMonthStartDate() && s.LineStatusChangeDate <= AppUtils.ThisMonthLastDate()));

            return View(lstClientLineStatuss);
        }


        //public ActionResult LockToActiveClient()
        //{

        //}
        private void SetInformationIn_VM_Transaction_ClientDueBills(IQueryable<List<Transaction>> lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        {
            lstVM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            foreach (var items in lstTransaction)
            {
                int index = items.Count();
                for (int j = 0; j < index; j++)
                {
                    lstVM_Transaction_ClientDueBills.Add(new VM_Transaction_ClientDueBills { Transaction = items[j], ClientDueBills = new ClientDueBills() });
                }

            }
        }
        private void SetInformationIn_VM_Transaction_ClientDueBills(List<List<Transaction>> lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        {
            lstVM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            foreach (var items in lstTransaction)
            {
                int index = items.Count();
                for (int j = 0; j < index; j++)
                {
                    lstVM_Transaction_ClientDueBills.Add(new VM_Transaction_ClientDueBills { Transaction = items[j], ClientDueBills = new ClientDueBills() });
                }

            }
            //throw new NotImplementedException();
        }
        private void SetInformationIn_VM_Transaction_ClientDueBills(IQueryable<List<VM_Transaction_ClientDueBills>> lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        {
            foreach (var items in lstTransaction)
            {
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        lstVM_Transaction_ClientDueBills.Add(item);
                    }

                }
            }
        }
        private void SetInformationIn_VM_Transaction_ClientDueBills(List<List<VM_Transaction_ClientDueBills>> lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        {
            foreach (var items in lstTransaction)
            {
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        lstVM_Transaction_ClientDueBills.Add(item);
                    }

                }
            }
        }
        private void setViewBagList(bool ForReseller = false, int ResellerID = 0)
        {
            if (ForReseller)
            {
                ViewBag.BoxID = new SelectList(db.Box.Where(x => x.ResellerID == ResellerID).Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

                var lstZone = db.Zone.Where(x => x.ResellerID == ResellerID).Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList();
                ViewBag.ZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
                ViewBag.SearchByZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
                var reseller = db.Reseller.Where(x => x.ResellerID == ResellerID).FirstOrDefault();
                List<int> lstMikrotik = string.IsNullOrEmpty(reseller.MacResellerAssignMikrotik) ? new List<int>()
                             : reseller.MacResellerAssignMikrotik.Trim(',').Split(',').Select(int.Parse).ToList();
                ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Where(x => lstMikrotik.Contains(x.MikrotikID)).Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

                List<macReselleGivenPackageWithPriceModel> lstResellerPackage = (ResellerID > 0) ? reseller != null ? !string.IsNullOrEmpty(reseller.macReselleGivenPackageWithPrice) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>() : new List<macReselleGivenPackageWithPriceModel>() : new List<macReselleGivenPackageWithPriceModel>();
                var lstPackage = lstResellerPackage.Select(x => new { x.PID, x.PName }).ToList();
                ViewBag.PackageThisMonth = new SelectList(lstPackage, "PID", "PName");
                ViewBag.PackageNextMonth = new SelectList(lstPackage, "PID", "PName");
                ViewBag.ResellerID = new SelectList(Enumerable.Empty<SelectListItem>());//new SelectList(new SelectListItem(),"ResellerID","ResellerName");
            }
            else
            {
                ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

                var lstZone = db.Zone.Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList();
                ViewBag.ZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");
                ViewBag.SearchByZoneID = new SelectList(lstZone, "ZoneID", "ZoneName");

                int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
                var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
                ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
                ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
                ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            }

            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            //ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");

            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");

            //var lstzone = db.Zone.Select(x => new { x.ZoneID, x.ZoneName }).ToList();
            //ViewBag.ZoneID = new SelectList(lstzone, "ZoneID", "ZoneName");
            //ViewBag.SearchByZoneID = new SelectList(lstzone, "ZoneID", "ZoneName");

            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            ViewBag.DueEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
        }
        private void setViewBagYearMonth()
        {

            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
        }

        //private void SetInformationIn_VM_Transaction_ClientDueBills(IQueryable<List<VM_Transaction_ClientDueBills>> lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        //{
        //    throw new NotImplementedException();
        //}



        //private void SetInformationIn_VM_Transaction_ClientDueBills(string lstTransaction, ref List<VM_Transaction_ClientDueBills> lstVM_Transaction_ClientDueBills)
        //{
        //    throw new NotImplementedException();
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateBillForThisMonth()
        {
            DateTime dateTime = AppUtils.GetDateTimeNow();
            DateTime firstDayOfRunningMonth = AppUtils.ThisMonthStartDate();
            DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            //DateTime dateTime = new DateTime(AppUtils.RunningYear, 9, 10); ;
            //DateTime firstDayOfRunningMonth = new DateTime(AppUtils.RunningYear, 9, 01); ;
            //DateTime lastDayOfRunningMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, 9, 30));
            //VM_ClientLineStatus_Transaction VM_ClientLineStatus_Transaction = new VM_ClientLineStatus_Transaction();
            //VM_ClientLineStatus_Transaction.lstTransaction = db.Transaction.Where(s => s.PaymentYear == dateTime.Year && s.PaymentMonth == dateTime.Month).ToList();                 

            try
            {
                List<int> lstCurentMonthSignUpClient = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.SignUpBillIndicator && (s.PaymentDate >= firstDayOfRunningMonth && s.PaymentDate <= lastDayOfRunningMonth) && s.ResellerID == null).Select(s => s.ClientDetailsID).ToList();
                BillGenerateHistory bgh = db.BIllGenerateHistory.Where(s => s.Year == AppUtils.RunningYear.ToString() && s.Month == AppUtils.RunningMonth.ToString() && s.Status == AppUtils.TableStatusIsActive).FirstOrDefault();

                if (bgh != null)
                {
                    return Json(new { BillAlreadyGenerate = true, Success = "", Count = "" }, JsonRequestBehavior.AllowGet);
                }

                List<ClientDetails> lstClientDetails = db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && !lstCurentMonthSignUpClient.Contains(s.ClientDetailsID) && s.ResellerID == null).ToList();
                if (lstClientDetails.Count > 0)
                {
                    if ((bool)Session["MikrotikOptionEnable"])
                    {

                        var searchYear = dateTime.AddMonths(-1).Year;
                        var searchMonth = dateTime.AddMonths(-1).Month;

                        //first taking information for all client 
                        List<VM_Mikrotik_ClientPackage_ID> lstVM_Mikrotik_ClientPackage_ID = lstClientDetails.Where(s => s.MikrotikID != null && s.ResellerID == null)
                            .GroupJoin(db.Transaction.Where(s => s.PaymentYear == searchYear && s.PaymentMonth == searchMonth && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly), ClientDetails => ClientDetails.ClientDetailsID, Transaction => Transaction.ClientDetailsID,
                            (ClientDetails, Transaction) => new { ClientDetails = ClientDetails, Transaction = Transaction.FirstOrDefault() })
                            .Select(s => new VM_Mikrotik_ClientPackage_ID
                            {
                                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                                LoginName = s.ClientDetails.LoginName,
                                Password = s.ClientDetails.Password,
                                ClientLineStatusID = s.ClientDetails.ClientLineStatusID,
                                LineStatusNextMonth = s.ClientDetails.StatusNextMonth,
                                PackageNextMonth = s.ClientDetails.PackageNextMonth,
                                PackageName = s.ClientDetails.Package.PackageName,
                                PackageThisMonth = s.Transaction.PackageID.Value,
                                ClientDetailsMikrotik = s.ClientDetails.Mikrotik
                            }).ToList();
                        //end of taking client information

                        //checking mikrotik are in live or not 
                        List<string> lstString = new List<string>();
                        foreach (var item in lstVM_Mikrotik_ClientPackage_ID)
                        {
                            if (!lstString.Contains(item.ClientDetailsMikrotik.MikName))
                            {
                                string mikName = item.ClientDetailsMikrotik.MikName;
                                try
                                {
                                    ITikConnection conn = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
                                    conn = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, item.ClientDetailsMikrotik.RealIP, 8728, item.ClientDetailsMikrotik.MikUserName, item.ClientDetailsMikrotik.MikPassword);
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { MikrotikFailed = true, Message = "Fix This: " + mikName + " :" + ex.Message }, JsonRequestBehavior.AllowGet);
                                }
                                lstString.Add(item.ClientDetailsMikrotik.MikName);
                            }
                        }
                        //end of checking mikrotik is in live or not.

                        //updating Client In mikrotkk
                        foreach (var item in lstVM_Mikrotik_ClientPackage_ID)
                        {
                            if (item.PackageNextMonth != item.PackageThisMonth)
                            {
                                try
                                {
                                    ITikConnection conn = MikrotikLB.CreateConnectionType(TikConnectionType.Api);
                                    conn = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, item.ClientDetailsMikrotik.RealIP, 8728, item.ClientDetailsMikrotik.MikUserName, item.ClientDetailsMikrotik.MikPassword);

                                    MikrotikLB.UpdateMikrotikUserBySingleSingleData(conn, item.LoginName, item.Password, item.PackageNextMonth);

                                    string status = item.LineStatusNextMonth == AppUtils.LineIsActive ? AppUtils.MakeUserEnableInMikrotik
                                        : item.LineStatusNextMonth == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik
                                            : "";
                                    MikrotikLB.SetStatusOfUserInMikrotik(conn, item.LoginName, status);
                                    if (item.LineStatusNextMonth == AppUtils.LineIsLock)
                                    {
                                        MikrotikLB.RemoveUserInActiveConenctionByLoginName(conn, item.LoginName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { MikrotikFailed = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        //finish update user in mikrotik
                    }

                    DateTime currenDateTime = AppUtils.GetDateTimeNow();
                    int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);

                    List<Transaction> lstGenerateBillSaveInToDB = new List<Transaction>();
                    Package freePackage = db.Package.Where(x => x.PackageName == AppUtils.FreePackageName).FirstOrDefault();
                    foreach (var clientDetails in lstClientDetails)
                    {
                        if (clientDetails.StatusNextMonth == AppUtils.LineIsActive)
                        {
                            Transaction ts = new Transaction();

                            ts.IsNewClient = AppUtils.isNotNewClient;
                            ts.ClientDetailsID = clientDetails.ClientDetailsID;
                            ts.PaymentYear = dateTime.Year;
                            ts.PaymentMonth = dateTime.Month;
                            ts.PackageID = clientDetails.PackageNextMonth;
                            //ts.PackageID = clientLineStatus.PackageID;
                            ts.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;// monthly bill

                            //if we have free package then automatic we are paying bill
                            if (freePackage != null)
                            {
                                ts.PaymentStatus = clientDetails.PackageNextMonth == freePackage.PackageID ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid;
                                ts.EmployeeID = clientDetails.PackageNextMonth == freePackage.PackageID ? AppUtils.LoginUserID : (int?)null;
                                ts.PaymentFromWhichPage = clientDetails.PackageNextMonth == freePackage.PackageID ? AppUtils.PamentIsOccouringFromAccountsPage.ToString() : null;
                                ts.BillCollectBy = clientDetails.PackageNextMonth == freePackage.PackageID ? AppUtils.LoginUserID : (int?)null;
                                ts.PaymentDate = clientDetails.PackageNextMonth == freePackage.PackageID ? AppUtils.dateTimeNow : (DateTime?)null;
                                ts.RemarksNo = clientDetails.PackageNextMonth == freePackage.PackageID ? "RAutoBillPay" + RemarksNo() : null;
                                ts.ResetNo = clientDetails.PackageNextMonth == freePackage.PackageID ? "RAutoBillPay" + SerialNo() : null;
                                //ts.PaymentFromWhichPage = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? AppUtils.PamentIsOccouringFromAccountsPage.ToString() : null;
                                //ts.BillCollectBy = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? AppUtils.LoginUserID : (int?)null;
                                //ts.PaymentDate = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? AppUtils.dateTimeNow : (DateTime?)null;
                                //ts.RemarksNo = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? "RAutoBillPay" + RemarksNo() : null;
                                //ts.ResetNo = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? "RAutoBillPay" + SerialNo() : null;
                                //ts.PaymentStatus = /*clientLineStatus.Package.PackageName*/clientLineStatus.PackageNextMonth == AppUtils.FreePackageName ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid;
                                //ts.EmployeeID = clientLineStatus.Package.PackageName == AppUtils.FreePackageName ? AppUtils.LoginUserID : (int?)null;

                            }
                            else
                            {
                                ts.PaymentStatus = AppUtils.PaymentIsNotPaid;
                                ts.EmployeeID = (int?)null;
                                ts.PaymentFromWhichPage = null;
                                ts.BillCollectBy = (int?)null;
                                ts.PaymentDate = (DateTime?)null;
                                ts.RemarksNo = null;
                                ts.ResetNo = null;

                            }
                            //end of free package automatic payment

                            ts.WhoGenerateTheBill = AppUtils.GetLoginUserID();
                            //ts.AmountCountDate = AppUtils.GetDateTimeNow();


                            //Serial serial = db.Serial.Find(1);
                            //serial.SerialNo += 1;

                            //ts.ResetNo = serial.SerialNo.ToString();
                            ts.LineStatusID = AppUtils.LineIsActive;

                            double packagePricePerday = 0;
                            int DaysRemains = 0;
                            //double MainPackagePrice = db.Package.Find(clientLineStatus.PackageID).PackagePrice;
                            double MainPackagePrice = db.Package.Find(clientDetails.PackageNextMonth).PackagePrice;
                            bool CountRegularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);

                            if (CountRegularMonthlyBase == true)
                            {
                                packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
                                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
                            }
                            else
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


                            ts.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();

                            ts.PaymentAmount = (float?)Math.Round(tmp);
                            ts.PaidAmount = 0;
                            ts.DueAmount = (float?)Math.Round(tmp);

                            lstGenerateBillSaveInToDB.Add(ts);
                        }
                    }
                    if (lstGenerateBillSaveInToDB.Count > 0)
                    {
                        try
                        {
                            db.Transaction.AddRange(lstGenerateBillSaveInToDB);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    var lstDueTransaction = db.Transaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                               .GroupBy(s => s.ClientDetailsID).Select(s => new { ClientDetailsID = s.FirstOrDefault().ClientDetailsID, TransactionID = s.FirstOrDefault().TransactionID, payment = s.FirstOrDefault().TransactionID, DueAmount = s.Sum(w => w.DueAmount) }).ToList();
                    if (lstDueTransaction.Count > 0)
                    {
                        db.ClientDueBills.RemoveRange(db.ClientDueBills.AsEnumerable());
                        db.SaveChanges();
                        //var test =      db.Transaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.ClientDetailsID == 8452  && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                        //             .GroupBy(s => s.ClientDetailsID).ToList();

                        List<ClientDueBills> lstClientDueBills = new List<ClientDueBills>();
                        foreach (var clientDueBillsOneByOne in lstDueTransaction)
                        {
                            ClientDueBills clientDueBills = new ClientDueBills();
                            clientDueBills.ClientDetailsID = clientDueBillsOneByOne.ClientDetailsID;
                            clientDueBills.DueAmount = clientDueBillsOneByOne.DueAmount.Value;
                            clientDueBills.Year = AppUtils.RunningYear;
                            clientDueBills.Month = AppUtils.RunningMonth;
                            clientDueBills.Status = false;
                            lstClientDueBills.Add(clientDueBills);
                        }

                        db.ClientDueBills.AddRange(lstClientDueBills);
                        db.SaveChanges();
                    }

                    GenerateBillHistoryInHistoryTable();

                    return Json(new { Success = true, Count = lstClientDetails.Count }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    GenerateBillHistoryInHistoryTable();
                    return Json(new { Success = true, Count = 0 }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Count = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        private void GenerateBillHistoryInHistoryTable()
        {
            BillGenerateHistory addGenerateHistoryInSystem = new BillGenerateHistory();
            addGenerateHistoryInSystem.Year = AppUtils.RunningYear.ToString();
            addGenerateHistoryInSystem.Month = AppUtils.RunningMonth.ToString();
            addGenerateHistoryInSystem.CreatedBy = AppUtils.GetLoginEmployeeName();
            addGenerateHistoryInSystem.CreatedDate = AppUtils.dateTimeNow;
            addGenerateHistoryInSystem.Status = AppUtils.TableStatusIsActive;
            db.BIllGenerateHistory.Add(addGenerateHistoryInSystem);
            db.SaveChanges();
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

        [HttpPost]
        public ActionResult PayRSClientMonthlyBill(Transaction Transaction)
        {
            float remainingAmount = 0;
            int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
            {
                return Json(new { Success = false, ResetNoAlreadyExist = true });
            }

            if (string.IsNullOrEmpty(Transaction.ResetNo))
            {
                Transaction.ResetNo = "AutoRST" + SerialNo();
            }


            Transaction dbts = db.Transaction.Find(Transaction.TransactionID);
            List<Transaction> lstDueTransactions = db.Transaction.Where(s => s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.ClientDetailsID == dbts.ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
            if (dbts.TransactionID > 0)
            {
                //ts.PaymentAmount = ts.PaymentAmount;
                //ts.EmployeeID = AppUtils.LoginUserID;
                //ts.BillCollectBy = Transaction.BillCollectBy;
                //ts.Discount = Transaction.Discount;
                //ts.PaymentStatus = AppUtils.PaymentIsPaid;// paid
                //ts.PaymentDate = AppUtils.GetDateTimeNow();
                //ts.RemarksNo = Transaction.RemarksNo;
                //ts.ResetNo = Transaction.ResetNo;
                //ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;

                if (Transaction.PaidAmount + Transaction.Discount + (dbts.PaidAmount == null ? 0 : dbts.PaidAmount) > dbts.PaymentAmount)
                {
                    var a = (Transaction.Discount + (dbts.PaidAmount == null ? 0 : dbts.PaidAmount));   //Discount + Old Paid Amount
                    remainingAmount = Transaction.PaidAmount.Value - (dbts.PaymentAmount.Value - a.Value);
                    //New Paid Amount - (Main Payment Amount - (Discount + Old Paid Amount) )
                    Transaction.PaidAmount = dbts.PaymentAmount - a;                                    // (Main Payment Amount - (Discount + Old Paid Amount))
                }
                //ts.PaymentAmount = ts.PaymentAmount;
                //dbts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;

                dbts.PaidAmount = dbts.PaidAmount == null ? Transaction.PaidAmount.Value : dbts.PaidAmount + Transaction.PaidAmount;
                dbts.DueAmount = dbts.PaymentAmount - (dbts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
                dbts.BillCollectBy = Transaction.BillCollectBy;
                dbts.Discount = Transaction.Discount;
                dbts.PaymentStatus = (dbts.PaymentAmount - (dbts.PaidAmount + dbts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
                dbts.PaymentDate = AppUtils.GetDateTimeNow();
                dbts.RemarksNo = Transaction.RemarksNo;
                dbts.ResetNo = Transaction.ResetNo;
                dbts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;



                try
                {
                    db.Entry(dbts).State = EntityState.Modified;
                    db.SaveChanges();

                    UpdatePaymentIntoPaymentHistoryTableForReseller(Transaction, dbts);
                    if (remainingAmount > 0 && lstDueTransactions.Count > 0)
                    {
                        foreach (var dueTransaction in lstDueTransactions)
                        {
                            if (remainingAmount >= 0)
                            {
                                double paymentAmount = dueTransaction.PaymentAmount.Value;
                                double paidAmount = dueTransaction.PaidAmount != null ? dueTransaction.PaidAmount.Value : 0;
                                //double dueAmount = dueTransaction.DueAmount != null ? dueTransaction.DueAmount.Value : paymentAmount;
                                double requireAmount = paymentAmount - paidAmount;

                                if (requireAmount < remainingAmount)//meaning full payment possible cause require is less then remaining amount.
                                {

                                    dueTransaction.PaidAmount += (float?)(remainingAmount - requireAmount);
                                    dueTransaction.DueAmount = 0;
                                    dueTransaction.PaymentStatus = AppUtils.PaymentIsPaid;
                                    dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
                                    dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
                                    dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

                                    remainingAmount -= (float)requireAmount;
                                }
                                else
                                {
                                    dueTransaction.PaidAmount += (float?)(requireAmount - remainingAmount);
                                    dueTransaction.DueAmount = (float?)(paymentAmount - paidAmount);
                                    dueTransaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
                                    dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
                                    dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
                                    dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

                                    remainingAmount -= (float)requireAmount;
                                }
                                db.Entry(dueTransaction).State = EntityState.Modified;
                                db.SaveChanges();

                                UpdatePaymentIntoPaymentHistoryTableForReseller(Transaction, dueTransaction);
                            }

                        }
                    }
                    if (remainingAmount > 0 /*&& lstDueTransactions.Count == 0*/)
                    {
                        AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == dbts.ClientDetailsID).FirstOrDefault();


                        if (advancePayment != null)
                        {
                            //advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                            advancePayment.AdvanceAmount += remainingAmount;
                            advancePayment.Remarks = "Payment Remarks";
                            db.Entry(advancePayment).State = EntityState.Modified;
                            db.SaveChanges();
                            UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForReseller(Transaction, dbts, advancePayment, remainingAmount);
                        }
                        else
                        {
                            AdvancePayment insertAdvancePayment = new AdvancePayment();
                            insertAdvancePayment.ClientDetailsID = dbts.ClientDetailsID;
                            insertAdvancePayment.AdvanceAmount = remainingAmount;
                            insertAdvancePayment.Remarks = "Payment Remarks";
                            //insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                            db.AdvancePayment.Add(insertAdvancePayment);
                            db.SaveChanges();
                            UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForReseller(Transaction, dbts, insertAdvancePayment, remainingAmount);
                        }


                    }
                    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                    if ((bool)Session["SMSOptionEnable"])
                    {
                        try
                        {
                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (sms != null)
                                {
                                    var message = sms.SendMessageText;
                                    message = message.Replace("[NAME]", dbts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(dbts.PaymentAmount.Value, 2).ToString());
                                    message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
                                    message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

                                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, dbts.ClientDetails.ContactNumber, message);
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
                            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                        }

                    }


                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            //return Json(new { }, JsonRequestBehavior.AllowGet);
        }



        private void UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(string resetNo, Transaction ts, AdvancePayment advancePayment, float advanceAmount, int PaymentBy)
        {
            PaymentHistory ph = new PaymentHistory();
            if (ts.TransactionID > 0)
            {
                ph.TransactionID = ts.TransactionID;
            }
            ph.ClientDetailsID = advancePayment.ClientDetailsID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
            ph.CollectByID = ts.BillCollectBy != null ? ts.BillCollectBy.Value : int.Parse(Session["LoggedUserID"].ToString()); //int.Parse(Session["LoggedUserID"].ToString());
            ph.PaymentDate = AppUtils.dateTimeNow;
            ph.AdvancePaymentID = advancePayment.AdvancePaymentID;
            ph.PaidAmount = advanceAmount;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            ph.PaymentByID = PaymentBy;
            ph.PaymentFromWhichPage = ts.PaymentFromWhichPage;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        private void UpdatePaymentIntoPaymentHistoryForNomalPayment(int disPayment, int nmlPayment, string resetNo, Transaction ts, int PaymentBy)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.CollectByID = ts.BillCollectBy != null ? ts.BillCollectBy.Value : int.Parse(Session["LoggedUserID"].ToString());//int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.PaymentDate = AppUtils.dateTimeNow;
            ph.PaidAmount = disPayment + nmlPayment;
            ph.NormalPayment = nmlPayment;
            ph.DiscountPayment = disPayment;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            ph.PaymentByID = PaymentBy;
            ph.PaymentFromWhichPage = ts.PaymentFromWhichPage;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        //[HttpPost]
        //public ActionResult PayMonthlyBill(Transaction Transaction)
        //{
        //    int countForResetIsGeneraetOrNot =
        //        db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count();
        //    int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count();
        //    if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //    {
        //        return Json(new { Success = false, ResetNoAlreadyExist = true });
        //    }

        //    Transaction ts = db.Transaction.Find(Transaction.TransactionID);

        //    if (ts.TransactionID > 0)
        //    {
        //        //ts.PaymentAmount = ts.PaymentAmount;
        //        //ts.EmployeeID = AppUtils.LoginUserID;
        //        //ts.BillCollectBy = Transaction.BillCollectBy;
        //        //ts.Discount = Transaction.Discount;
        //        //ts.PaymentStatus = AppUtils.PaymentIsPaid;// paid
        //        //ts.PaymentDate = AppUtils.GetDateTimeNow();
        //        //ts.RemarksNo = Transaction.RemarksNo;
        //        //ts.ResetNo = Transaction.ResetNo;
        //        //ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;

        //        if (Transaction.PaidAmount + Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount) > ts.PaymentAmount)
        //        {
        //            var a = (Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount));
        //            Transaction.PaidAmount = ts.PaymentAmount - a;
        //        }
        //        //ts.PaymentAmount = ts.PaymentAmount;
        //        ts.EmployeeID = AppUtils.LoginUserID;
        //        ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : ts.PaidAmount + Transaction.PaidAmount;
        //        ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
        //        ts.BillCollectBy = Transaction.BillCollectBy;
        //        ts.Discount = Transaction.Discount;
        //        ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
        //        ts.PaymentDate = AppUtils.GetDateTimeNow();
        //        ts.RemarksNo = Transaction.RemarksNo;
        //        ts.ResetNo = Transaction.ResetNo;
        //        ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;



        //        try
        //        {
        //            db.Entry(ts).State = EntityState.Modified;
        //            db.SaveChanges();


        //            UpdatePaymentIntoPaymentHistoryTable(Transaction, ts);
        //            //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //            if (AppUtils.SMSOptionEnable)
        //            {
        //                try
        //                {
        //                    SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
        //                    if (smsSenderIdPass != null)
        //                    {
        //                        SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //                        if (sms != null)
        //                        {
        //                            var message = sms.SendMessageText;
        //                            message = message.Replace("[NAME]", ts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(ts.PaymentAmount.Value, 2).ToString());
        //                            message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
        //                            message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

        //                            SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ts.ClientDetails.ContactNumber, message);
        //                            if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
        //                            {
        //                                sms.SMSCounter += 1;
        //                                db.Entry(sms).State = EntityState.Modified;
        //                                db.SaveChanges();
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //                }

        //            }


        //            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch
        //        {
        //            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //    //return Json(new { }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult PayDueBill(Transaction Transaction)//commentforlittlebit
        {
            int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;

            if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
            {
                return Json(new { Success = false, ResetNoAlreadyExist = true });
            }

            if (string.IsNullOrEmpty(Transaction.ResetNo))
            {
                Transaction.ResetNo = "AutoRST" + SerialNo();
            }
            Transaction ts = db.Transaction.Find(Transaction.TransactionID);
            //    Transaction tsLatestTransactionForMinusDueAmount = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && (s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)).FirstOrDefault();
            if (ts.TransactionID > 0)
            {
                if (Transaction.PaidAmount + Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount) > ts.PaymentAmount)
                {
                    Transaction.PaidAmount = ts.PaymentAmount - (Transaction.Discount + ts.PaidAmount == null ? 0 : ts.PaidAmount);
                }
                //ts.PaymentAmount = ts.PaymentAmount;
                ts.IsNewClient = AppUtils.isNotNewClient;
                //ts.EmployeeID = AppUtils.GetLoginUserID();
                ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
                ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : ts.PaidAmount + Transaction.PaidAmount;
                ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
                ts.BillCollectBy = Transaction.BillCollectBy;
                ts.Discount = Transaction.Discount;
                ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
                ts.PaymentDate = AppUtils.GetDateTimeNow();
                ts.RemarksNo = Transaction.RemarksNo;
                ts.ResetNo = Transaction.ResetNo;
                ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;
                try
                {
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();

                    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();
                    if (clientDueBills.ClientDueBillsID > 0)
                    {
                        //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
                        //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
                        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
                        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
                    }
                    db.Entry(clientDueBills).State = EntityState.Modified;
                    db.SaveChanges();

                    UpdatePaymentIntoPaymentHistoryTable(Transaction, ts);
                    dynamic duets = new System.Dynamic.ExpandoObject();
                    duets.id = ts.TransactionID;
                    duets.PaidAmount = ts.PaidAmount;
                    duets.Discount = ts.Discount;
                    duets.Due = ts.DueAmount;
                    duets.PaymentStatus = ts.PaymentStatus == 1 ? "Paid" : "Not Paid";


                    if ((bool)Session["SMSOptionEnable"])
                    {
                        try
                        {
                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (sms != null)
                                {
                                    var message = sms.SendMessageText;
                                    message = message.Replace("[NAME]", ts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(ts.PaymentAmount.Value, 2).ToString());
                                    message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
                                    message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

                                    string smsMobileNo = "";
                                    if (!string.IsNullOrEmpty(Transaction.AnotherMobileNo))
                                    {
                                        smsMobileNo = Transaction.AnotherMobileNo;
                                    }
                                    else
                                    {
                                        smsMobileNo = ts.ClientDetails.ContactNumber;
                                    }

                                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, smsMobileNo, message);
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
                            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new { Success = true, duets = duets }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }



            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PayResellerDueBill(Transaction Transaction)//commentforlittlebit
        {
            int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;

            if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
            {
                return Json(new { Success = false, ResetNoAlreadyExist = true });
            }

            if (string.IsNullOrEmpty(Transaction.ResetNo))
            {
                Transaction.ResetNo = "AutoRST" + SerialNo();
            }
            Transaction ts = db.Transaction.Find(Transaction.TransactionID);
            //    Transaction tsLatestTransactionForMinusDueAmount = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && (s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)).FirstOrDefault();
            if (ts.TransactionID > 0)
            {
                if (Transaction.PaidAmount + Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount) > ts.PaymentAmount)
                {
                    Transaction.PaidAmount = ts.PaymentAmount - (Transaction.Discount + ts.PaidAmount == null ? 0 : ts.PaidAmount);
                }
                //ts.PaymentAmount = ts.PaymentAmount;
                ts.IsNewClient = AppUtils.isNotNewClient;
                //ts.EmployeeID = AppUtils.GetLoginUserID();
                //ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
                ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : ts.PaidAmount + Transaction.PaidAmount;
                ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
                //ts.BillCollectBy = Transaction.BillCollectBy;
                ts.Discount = Transaction.Discount;
                ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
                ts.PaymentDate = AppUtils.GetDateTimeNow();
                ts.RemarksNo = Transaction.RemarksNo;
                ts.ResetNo = Transaction.ResetNo;
                ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;
                try
                {
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();

                    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();
                    if (clientDueBills.ClientDueBillsID > 0)
                    {
                        //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
                        //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
                        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
                        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
                    }
                    db.Entry(clientDueBills).State = EntityState.Modified;
                    db.SaveChanges();

                    UpdatePaymentIntoPaymentHistoryTableForReseller(Transaction, ts);
                    dynamic duets = new System.Dynamic.ExpandoObject();
                    duets.id = ts.TransactionID;
                    duets.PaidAmount = ts.PaidAmount;
                    duets.Discount = ts.Discount;
                    duets.Due = ts.DueAmount;
                    duets.PaymentStatus = ts.PaymentStatus == 1 ? "Paid" : "Not Paid";


                    if ((bool)Session["SMSOptionEnable"])
                    {
                        try
                        {
                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (sms != null)
                                {
                                    var message = sms.SendMessageText;
                                    message = message.Replace("[NAME]", ts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(ts.PaymentAmount.Value, 2).ToString());
                                    message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
                                    message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

                                    string smsMobileNo = "";
                                    if (!string.IsNullOrEmpty(Transaction.AnotherMobileNo))
                                    {
                                        smsMobileNo = Transaction.AnotherMobileNo;
                                    }
                                    else
                                    {
                                        smsMobileNo = ts.ClientDetails.ContactNumber;
                                    }

                                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, smsMobileNo, message);
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
                            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new { Success = true, duets = duets }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
        private void UpdatePaymentIntoPaymentHistoryTable(Transaction transaction, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*ts.EmployeeID.Value*/;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.CollectByID = transaction.BillCollectBy.Value;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = transaction.PaidAmount.Value;
            ph.ResetNo = transaction.ResetNo;
            if (transaction.PaymentBy > 0)
            {
                ph.PaymentByID = transaction.PaymentBy;
            }
            ph.PaymentFromWhichPage = transaction.PaymentFromWhichPage;
            ph.DiscountPayment = transaction.Discount != null ? (int)transaction.Discount.Value : 0;
            ph.NormalPayment = (int)ph.PaidAmount + ph.DiscountPayment;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }
        private void UpdatePaymentIntoPaymentHistoryTableForUnpaidBill(Transaction transaction, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*ts.EmployeeID.Value*/;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.CollectByID = transaction.BillCollectBy.Value;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = transaction.PaidAmount.Value;
            ph.ResetNo = transaction.ResetNo;
            if (transaction.PaymentBy > 0)
            {
                ph.PaymentByID = transaction.PaymentBy;
            }
            ph.PaymentFromWhichPage = transaction.PaymentFromWhichPage;
            //ph.DiscountPayment = transaction.Discount != null ? (int)transaction.Discount.Value : 0;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }
        private void UpdatePaymentIntoPaymentHistoryTableForReseller(Transaction transaction, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            //ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*ts.EmployeeID.Value*/;
            ph.ResellerID = ts.ResellerID;
            //ph.CollectByID = transaction.BillCollectBy.Value;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = transaction.PaidAmount.Value;
            ph.ResetNo = transaction.ResetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }
        private void UpdatePaymentIntoPaymentHistoryTableByOneTransactionInformation(Transaction ts, double amount)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*ts.EmployeeID.Value*/;
            ph.CollectByID = int.Parse(Session["LoggedUserID"].ToString());
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = (float)amount;
            ph.ResetNo = ts.ResetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        private void UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(Transaction transaction, Transaction ts, AdvancePayment advancePayment, float remainingAmount)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = ts.EmployeeID.Value;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.CollectByID = transaction.BillCollectBy.Value;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.AdvancePaymentID = advancePayment.AdvancePaymentID;
            ph.PaidAmount = remainingAmount;
            ph.ResetNo = transaction.ResetNo;
            ph.PaymentFromWhichPage = transaction.PaymentFromWhichPage;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }
        private void UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForReseller(Transaction transaction, Transaction ts, AdvancePayment advancePayment, float remainingAmount)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            //ph.EmployeeID = ts.EmployeeID.Value;
            //ph.CollectByID = transaction.BillCollectBy.Value;
            ph.ResellerID = ts.ResellerID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.AdvancePaymentID = advancePayment.AdvancePaymentID;
            ph.PaidAmount = remainingAmount;
            ph.ResetNo = transaction.ResetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }


        private void UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForSelfEmployee(Transaction transaction, Transaction ts, AdvancePayment advancePayment, float remainingAmount)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = ts.EmployeeID.Value;
            ph.CollectByID = AppUtils.GetLoginUserID();
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.AdvancePaymentID = advancePayment.AdvancePaymentID;
            ph.PaidAmount = remainingAmount;
            ph.ResetNo = transaction.ResetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        private void UpdatePaymentIntoPaymentHistoryTableSelfPayment(Transaction transaction, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.EmployeeID = ts.EmployeeID.Value;
            ph.CollectByID = ts.BillCollectBy.Value;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = transaction.PaidAmount.Value;
            ph.ResetNo = transaction.ResetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult PayDueBillByEmployeeHimself(Transaction Transaction)
        {
            if (Transaction.Discount == null)
            {
                Transaction.Discount = 0;
            }

            float remainingAmount = 0;
            int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
            if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
            {
                return Json(new { Success = false, ResetNoAlreadyExist = true });
            }

            if (string.IsNullOrEmpty(Transaction.ResetNo))
            {
                Transaction.ResetNo = "AutoRST" + SerialNo();
            }
            //int countForResetIsGeneraetOrNot =
            //    db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count();
            //int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count();
            //if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
            //{
            //    return Json(new { Success = false, ResetNoAlreadyExist = true });
            //}

            Transaction ts = db.Transaction.Find(Transaction.TransactionID);
            List<Transaction> lstDueTransactions = db.Transaction.Where(s => s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.ClientDetailsID == ts.ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();

            //    Transaction tsLatestTransactionForMinusDueAmount = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && (s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)).FirstOrDefault();
            if (ts.TransactionID > 0)
            {
                if (Transaction.PaidAmount /*+ Transaction.Discount*/ + ((ts.PaidAmount == null ? 0 : ts.PaidAmount) + (ts.Discount == null ? 0 : ts.Discount)) > ts.PaymentAmount)
                {
                    //Transaction.PaidAmount = ts.PaymentAmount - (Transaction.Discount + ts.PaidAmount == null ? 0 : ts.PaidAmount);

                    var a = (/*Transaction.Discount*/ (ts.Discount == null ? 0 : ts.Discount.Value) + (ts.PaidAmount == null ? 0 : ts.PaidAmount));
                    remainingAmount = Transaction.PaidAmount.Value - (ts.PaymentAmount.Value - a.Value);
                    Transaction.PaidAmount = ts.PaymentAmount - a;
                }

                //ts.PaymentAmount = ts.PaymentAmount;

                ts.IsNewClient = AppUtils.isNotNewClient;
                ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
                ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : ts.PaidAmount + Transaction.PaidAmount.Value;
                //var pm = ts.PaymentAmount; var pdds = (ts.PaidAmount + (ts.Discount == null ? 0 : ts.Discount));
                ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + (ts.Discount == null ? 0 : ts.Discount))/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
                ts.BillCollectBy = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;

                ts.Discount = AppUtils.EmployeeHasRightToGiveDiscount ? Transaction.Discount : ((ts.Discount == null) ? 0 : ts.Discount);
                ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
                ts.PaymentDate = AppUtils.GetDateTimeNow();
                ts.RemarksNo = Transaction.RemarksNo;
                ts.ResetNo = Transaction.ResetNo;
                ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;
                try
                {
                    db.Entry(ts).State = EntityState.Modified;
                    db.SaveChanges();

                    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();
                    if (clientDueBills != null)
                    {
                        //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
                        //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
                        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
                        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
                        db.Entry(clientDueBills).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //                    UpdatePaymentIntoPaymentHistoryTable(Transaction, ts);
                    UpdatePaymentIntoPaymentHistoryTableSelfPayment(Transaction, ts);

                    if (remainingAmount > 0 && lstDueTransactions.Count > 0)
                    {
                        foreach (var dueTransaction in lstDueTransactions)
                        {
                            if (remainingAmount >= 0)
                            {
                                double paymentAmount = dueTransaction.PaymentAmount.Value;
                                double paidAmount = dueTransaction.PaidAmount != null ? dueTransaction.PaidAmount.Value : 0;
                                //double dueAmount = dueTransaction.DueAmount != null ? dueTransaction.DueAmount.Value : paymentAmount;
                                double requireAmount = paymentAmount - paidAmount;

                                if (requireAmount < remainingAmount)//meaning full payment possible cause require is less then remaining amount.
                                {

                                    dueTransaction.PaidAmount += (float?)(remainingAmount - requireAmount);
                                    dueTransaction.DueAmount = 0;
                                    dueTransaction.PaymentStatus = AppUtils.PaymentIsPaid;
                                    dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
                                    dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
                                    dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

                                    remainingAmount -= (float)requireAmount;
                                }
                                else
                                {
                                    dueTransaction.PaidAmount += (float?)(requireAmount - remainingAmount);
                                    dueTransaction.DueAmount = (float?)(paymentAmount - paidAmount);
                                    dueTransaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
                                    dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
                                    dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
                                    dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

                                    remainingAmount -= (float)requireAmount;
                                }
                                db.Entry(dueTransaction).State = EntityState.Modified;
                                db.SaveChanges();

                                UpdatePaymentIntoPaymentHistoryTable(Transaction, dueTransaction);
                            }

                        }
                    }
                    if (remainingAmount > 0 /*&& lstDueTransactions.Count == 0*/)
                    {
                        AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();


                        if (advancePayment != null)
                        {
                            advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                            advancePayment.AdvanceAmount += remainingAmount;
                            advancePayment.Remarks = "Payment Remarks";
                            db.Entry(advancePayment).State = EntityState.Modified;
                            db.SaveChanges();
                            UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForSelfEmployee(Transaction, ts, advancePayment, remainingAmount);
                        }
                        else
                        {
                            AdvancePayment insertAdvancePayment = new AdvancePayment();
                            insertAdvancePayment.ClientDetailsID = ts.ClientDetailsID;
                            insertAdvancePayment.AdvanceAmount = remainingAmount;
                            insertAdvancePayment.Remarks = "Payment Remarks";
                            insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                            db.AdvancePayment.Add(insertAdvancePayment);
                            db.SaveChanges();
                            UpdatePaymentIntoPaymentHistoryTableForAdvancePaymentForSelfEmployee(Transaction, ts, insertAdvancePayment, remainingAmount);
                        }
                    }


                    return Json(new { Success = true, TransactionID = Transaction.TransactionID, FullBillPaid = ts.PaymentStatus, PaidAmount = (ts.PaidAmount == null) ? 0 : ts.PaidAmount, Discount = ts.Discount == null ? 0 : ts.Discount, DueAmount = ts.DueAmount == null ? 0 : ts.DueAmount }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { Success = false, TransactionID = Transaction.TransactionID }, JsonRequestBehavior.AllowGet);
                }
            }


            //int countForResetIsGeneraetOrNot =
            //    db.Transaction.Where(s => s.ResetNo == Transaction.ResetNo.Trim()).Count();
            //if (countForResetIsGeneraetOrNot > 0)
            //{
            //    return Json(new { Success = false, ResetNoAlreadyExist = true });
            //}

            //Transaction ts = db.Transaction.Find(Transaction.TransactionID);
            ////    Transaction tsLatestTransactionForMinusDueAmount = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && (s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)).FirstOrDefault();
            //if (ts.TransactionID > 0)
            //{
            //    //ts.PaymentAmount = ts.PaymentAmount;
            //    ts.IsNewClient = AppUtils.isNotNewClient;
            //    ts.EmployeeID = AppUtils.LoginUserID;
            //    ts.BillCollectBy = AppUtils.LoginUserID;
            //    ts.Discount = Transaction.Discount;
            //    ts.PaymentStatus = AppUtils.PaymentIsPaid;// paid
            //    ts.PaymentDate = AppUtils.GetDateTimeNow();
            //    ts.RemarksNo = Transaction.RemarksNo;
            //    ts.ResetNo = Transaction.ResetNo;
            //    ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;
            //    try
            //    {
            //        db.Entry(ts).State = EntityState.Modified;
            //        db.SaveChanges();
            //        //if (tsLatestTransactionForMinusDueAmount.TransactionID > 0)
            //        //{
            //        //    tsLatestTransactionForMinusDueAmount.DueAmount -= ts.Package.PackagePrice;
            //        //    db.Entry(tsLatestTransactionForMinusDueAmount).State = EntityState.Modified;
            //        //    db.SaveChanges();
            //        //}

            //        ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();
            //        if (clientDueBills.ClientDueBillsID > 0)
            //        {
            //            clientDueBills.DueAmount -= ts.PaymentAmount.Value;
            //        }
            //        db.Entry(clientDueBills).State = EntityState.Modified;
            //        db.SaveChanges();

            //        return Json(new { Success = true, TransactionID = Transaction.TransactionID }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch
            //    {
            //        return Json(new { Success = false, TransactionID = Transaction.TransactionID }, JsonRequestBehavior.AllowGet);
            //    }
            //}


            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AdjustDueBill()
        //{

        //    try
        //    {
        //        List<ClientDueBills> lstClientDueBills = db.ClientDueBills.ToList();

        //        //int countTrueStatus = lstClientDueBills.Where(s => s.Status == true).Count();
        //        //int countFalseStatus = lstClientDueBills.Where(s => s.Status == false).Count();

        //        //if (countTrueStatus > 0)
        //        //{
        //        //    return Json(new { Success = true, DueBillAlreadyGenerate = true, GenerateBillEmpty = "" }, JsonRequestBehavior.AllowGet);
        //        //}

        //        //else
        //        {
        //            lstClientDueBills.ForEach(s => s.Status = true);
        //            db.SaveChanges();
        //            return Json(new { Success = true, NoDueBill = lstClientDueBills.Count(), DueBillAlreadyGenerate = "", GenerateBillEmpty = "" }, JsonRequestBehavior.AllowGet);

        //        }


        //        //List<ClientDueBills> lstClientDueBills = db.ClientDueBills.Where(s => s.Year == AppUtils.RunningYear && s.Month == AppUtils.RunningMonth).ToList();

        //        //int countTrueStatus = lstClientDueBills.Where(s => s.Status == true).Count();
        //        //int countFalseStatus = lstClientDueBills.Where(s => s.Status == false).Count();

        //        //if (countTrueStatus > 0)
        //        //{
        //        //    return Json(new { Success = true, DueBillAlreadyGenerate = true, GenerateBillEmpty = "" }, JsonRequestBehavior.AllowGet);
        //        //}

        //        //else if (countFalseStatus == 0)
        //        //{
        //        //    if (db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Count() == 0)
        //        //    {
        //        //        return Json(new { Success = true, DueBillAlreadyGenerate = false, GenerateBillEmpty = true }, JsonRequestBehavior.AllowGet);
        //        //    }
        //        //    else{
        //        //        return Json(new { Success = true, DueBillAlreadyGenerate = "", GenerateBillEmpty = "", NoDueBillFound = true }, JsonRequestBehavior.AllowGet);
        //        //    }
        //        //}

        //        //else
        //        //{
        //        //    lstClientDueBills.ForEach(s => s.Status = true);
        //        //    db.SaveChanges();
        //        //    return Json(new { Success = true, NoDueBill = lstClientDueBills.Count(), DueBillAlreadyGenerate = "", GenerateBillEmpty = "" }, JsonRequestBehavior.AllowGet);

        //        //}

        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, NoDueBill = "" }, JsonRequestBehavior.AllowGet);
        //    }

        //    //try
        //    //{
        //    //    List<Transaction> lstTransactionForCheckingDueBillAddedOrNotInThisMonth = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.DueAmount > 0).ToList();

        //    //    if (lstTransactionForCheckingDueBillAddedOrNotInThisMonth.Count > 0)
        //    //    {
        //    //        return Json(new { Success = true, DueBillAlreadyGenerate = true }, JsonRequestBehavior.AllowGet);
        //    //    }
        //    //    else
        //    //    {
        //    //        var lstTransaction = db.Transaction.ToList().Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
        //    //                     .GroupBy(s => s.ClientDetailsID).Select(s => new { ClientDetailsID = s.First().ClientDetailsID, TransactionID = s.First().TransactionID, payment = s.First().TransactionID, DueAmount = s.Sum(w => w.Package.PackagePrice) }).ToList();
        //    //        List<Transaction> lstTransactionFromDB = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();

        //    //        foreach (var item in lstTransaction)
        //    //        {
        //    //            Transaction ts = lstTransactionFromDB.Where(s => s.ClientDetailsID == item.ClientDetailsID).FirstOrDefault();
        //    //            ts.DueAmount = item.DueAmount;
        //    //            db.Entry(ts).State = EntityState.Modified;
        //    //            db.SaveChanges();
        //    //        }
        //    //        return Json(new { Success = true, NoDueBill = lstTransaction.Count }, JsonRequestBehavior.AllowGet);
        //    //    }

        //    //}
        //    //catch
        //    //{
        //    //    return Json(new { Success = false, NoDueBill = "" }, JsonRequestBehavior.AllowGet);
        //    //}

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowDueBillDetailssByTransactionID(int TransactionID)
        {

            Transaction ts = db.Transaction.Find(TransactionID);
            if (ts.TransactionID > 0)
            {
                int clientID = ts.ClientDetailsID;

                var lstTransaction = db.Transaction.Where(s => s.ClientDetailsID == ts.ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)).Select(s => new { TransactionID = s.TransactionID, ClientDetailsID = s.ClientDetailsID, MonthName = s.PaymentMonth, PackageName = s.Package.PackageName, PackagePrice = Math.Round(s.PaymentAmount.Value, 2), PaidAmount = s.PaidAmount, Due = /*s.DueAmount == null ? */s.DueAmount, Discount = s.Discount, Status = "Not Paid" }).ToList();
                var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ts.ClientDetailsID).Select(s => new { ClientName = s.Name, ClientLoginName = s.LoginName, ClientZoneName = s.Zone.ZoneName, ClientAddress = s.Address, ClientConnectionType = s.ConnectionType.ConnectionTypeName, ClientContactNumber = s.ContactNumber }).FirstOrDefault();

                return Json(new { DueTransactionList = lstTransaction, ClientDetails = ClientDetails, Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { DueTransactionList = "", Success = false }, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowDueBillDetailssByClientDetailsID(int ClientDetailsID)
        {


            var lstTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly /*&& !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)*/).Select(s => new
            {
                TransactionID = s.TransactionID,
                ClientDetailsID = s.ClientDetailsID,
                MonthName = s.PaymentMonth,
                PackageName = s.Package.PackageName,
                PackagePrice = Math.Round(s.PaymentAmount.Value, 2),
                PaymentAmount = s.PaymentAmount,
                PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount,
                Discount = s.Discount == null ? 0 : s.Discount,
                Status = "Not Paid",
                DueAmount = s.DueAmount == null ? s.PaymentAmount - 0 : s.DueAmount
            }).ToList();
            //var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID).Select(s => new
            //{
            //    Name = s.Name,
            //    LoginName = s.LoginName,
            //    Zone = s.Zone.ZoneName,
            //    Address = s.Address,
            //    Contact = s.ContactNumber
            //}).FirstOrDefault();

            return Json(new { DueTransactionList = lstTransaction, /*ClientDetails = ClientDetails,*/ Success = true }, JsonRequestBehavior.AllowGet);

        }

        private void SetLstTransaction(ref List<Transaction> lstTransaction, int? YearID, int? MonthID, int? ZoneID)
        {
            int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
            int month = MonthID != null ? MonthID.Value : 0;

            if (year > 0 && month > 0 && ZoneID != null)
            {
                lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month && s.ClientDetails.ZoneID == ZoneID).ToList();
            }
            else if (year > 0 && MonthID != null)
            {
                lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == MonthID.Value).ToList();
            }
            else if (year > 0 && ZoneID != null)
            {
                lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.ClientDetails.ZoneID == ZoneID).ToList();
            }
            else if (ZoneID != null)
            {
                lstTransaction = db.Transaction.Where(s => s.ClientDetails.ZoneID == ZoneID).ToList();
            }
            else if (YearID != null)
            {
                lstTransaction = db.Transaction.Where(s => s.PaymentYear == YearID.Value).ToList();
            }

            //if (year > 0 && month > 0 && ZoneID != null)
            //{
            //    lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month && s.ClientDetails.ZoneID == ZoneID || (s.PaymentDate.HasValue && s.PaymentDate.Value.Year == YearID.Value)).ToList();
            //}
            //else if (year > 0 && MonthID != null)
            //{
            //    lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == MonthID.Value || (s.PaymentDate.HasValue && s.PaymentDate.Value.Year == YearID.Value)).ToList();
            //}
            //else if (year > 0 && ZoneID != null)
            //{
            //    lstTransaction = db.Transaction.Where(s => s.PaymentYear == year && s.ClientDetails.ZoneID == ZoneID || (s.PaymentDate.HasValue && s.PaymentDate.Value.Year == YearID.Value)).ToList();
            //}
            //else if (ZoneID != null)
            //{
            //    lstTransaction = db.Transaction.Where(s => s.ClientDetails.ZoneID == ZoneID || (s.PaymentDate.HasValue && s.PaymentDate.Value.Year == YearID.Value)).ToList();
            //}
            //else if (YearID != null)
            //{
            //    lstTransaction = db.Transaction.Where(s => s.PaymentYear == YearID.Value || (s.PaymentDate.HasValue && s.PaymentDate.Value.Year == YearID.Value)).ToList();
            //}
        }



        private void lstTransactionSelect(ref List<Transaction> lstTransaction)
        {
            lstTransaction.Select(s => new
            {
                TransactionID = s.TransactionID,
                LoginName = s.ClientDetails.LoginName,
                Address = s.ClientDetails.Address,
                Mobile = s.ClientDetails.ContactNumber,
                Zone = s.ClientDetails.Zone.ZoneName,
                Package = s.Package.PackageName,
                MonthlyFee = s.Package.PackagePrice,
                PaidAmount = s.PaymentAmount,
                DueAmount = s.DueAmount,
                PaidBy = s.Employee.Name,
                CollectBy = s.Employee.Name,
                PaidTime = s.PaymentDate,
                RemarksNo = s.RemarksNo,
                ReceiptNo = s.ResetNo,
            }).ToList();
        }
        private void lstTransactionSelect(ref IQueryable<Transaction> lstTransaction)
        {
            lstTransaction.Select(s => new
            {
                TransactionID = s.TransactionID,
                LoginName = s.ClientDetails.LoginName,
                Address = s.ClientDetails.Address,
                Mobile = s.ClientDetails.ContactNumber,
                Zone = s.ClientDetails.Zone.ZoneName,
                Package = s.Package.PackageName,
                MonthlyFee = s.Package.PackagePrice,
                PaidAmount = s.PaymentAmount,
                DueAmount = s.DueAmount,
                PaidBy = s.Employee.Name,
                CollectBy = s.Employee.Name,
                PaidTime = s.PaymentDate,
                RemarksNo = s.RemarksNo,
                ReceiptNo = s.ResetNo,
            }).ToList();
        }

        private void SetBillSummaryForAjaxCall()
        {
            // DateTime customDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 01);
            DateTime startDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 01);
            DateTime endDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, DateTime.DaysInMonth(AppUtils.RunningYear, AppUtils.RunningMonth));

            List<Transaction> lstTransaction = db.Transaction.ToList();
            List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();
            List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
            List<Transaction> lstRegularSignUpBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).ToList();

            ViewBag.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice);
            ViewBag.clnCollectedAmount = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount);
            ViewBag.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
            ViewBag.clnCollectedAmountBIll = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount);
            ViewBag.clnOnlinePayment = 0;
            ViewBag.clnInstallationAmount = "? is this";
            ViewBag.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice) - ((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) + lstTransactionForBillSummary.Sum(s => s.Discount));
            ViewBag.clnTotalExpense = "? is this";
            ViewBag.clnRestOfAmount = "? is this"; ;
            ViewBag.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
            ViewBag.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count();
            ViewBag.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Count());
            ViewBag.clnPreviousBillCollection = lstTransaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.PaymentAmount);
        }

        //private string MonthNames(string m)
        //{
        //    string res;
        //    switch (m)
        //    {
        //        case "1":
        //            res = "Jan";
        //            break;
        //        case "2":
        //            res = "Feb";
        //            break;
        //        case "3":
        //            res = "Mar";
        //            break;
        //        case "4":
        //            res = "Apr";
        //            break;
        //        case "5":
        //            res = "May";
        //            break;
        //        case "6":
        //            res = "Jun";
        //            break;
        //        case "7":
        //            res = "Jul";
        //            break;
        //        case "8":
        //            res = "Agu";
        //            break;
        //        case "9":
        //            res = "Sep";
        //            break;
        //        case "10":
        //            res = "Oct";
        //            break;
        //        case "11":
        //            res = "Nov";
        //            break;
        //        case "12":
        //            res = "Dec";
        //            break;
        //        default:
        //            res = "Empty";
        //            break;
        //    }
        //    return res;
        //}

        [ValidateAntiForgeryToken]
        public ActionResult PayRunningMonthBillFromAdvanceAmount(string PaymentFromWhichPage)
        {
            List<Transaction> lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == 0).ToList();
            List<AdvancePayment> lstAdvancePayment = db.AdvancePayment.ToList();

            int advancePaymentCount = 0;
            try
            {
                if (lstTransaction.Count > 0 && lstAdvancePayment.Count > 0)
                {
                    foreach (var item in lstTransaction)
                    {
                        if (lstAdvancePayment.Where(s => s.ClientDetailsID == item.ClientDetailsID).Count() > 0)
                        {
                            AdvancePayment advancePayment = lstAdvancePayment.Where(s => s.ClientDetailsID == item.ClientDetailsID).FirstOrDefault();
                            if (advancePayment.AdvanceAmount >= item.PaymentAmount)
                            {
                                // Remarks remarks = db.Remarks.Find(1);
                                // remarks.RemarksNo += 1;

                                SerialNoForAdvancePayment serialNoForAdvancePayment = db.SerialNoForAdvancePayment.Find(1);
                                serialNoForAdvancePayment.SerialNumber += 1;

                                item.PaymentStatus = AppUtils.PaymentIsPaid;
                                item.EmployeeID = AppUtils.GetLoginUserID();
                                item.BillCollectBy = AppUtils.GetLoginUserID();//AppUtils.LoginUserID;
                                item.PaymentDate = AppUtils.GetDateTimeNow();
                                item.PaymentFrom = AppUtils.PaymentByHandCash;
                                item.PaymentAmount = item.PaymentAmount;
                                item.RemarksNo = "Rem ADV " + serialNoForAdvancePayment.SerialNumber;
                                item.ResetNo = "Res ADV" + serialNoForAdvancePayment.SerialNumber;
                                item.PaymentFromWhichPage = PaymentFromWhichPage;
                                //      item.PaymentFromWhichPage = AppUtils.PamentIsOccouringFromAdvancePaymentAccountPage.ToString();
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();

                                db.Entry(serialNoForAdvancePayment).State = EntityState.Modified;
                                db.SaveChanges();

                                //if (item.Package.PackagePrice > 0)
                                //{
                                //    db.Entry(remarks).State = EntityState.Modified;
                                //    db.SaveChanges();
                                //}

                                advancePayment.AdvanceAmount -= item.PaymentAmount.Value;
                                db.Entry(advancePayment).State = EntityState.Modified;
                                db.SaveChanges();

                                advancePaymentCount++;
                            }
                        }
                    }
                }
                if (advancePaymentCount > 0)
                {
                    return Json(new { Success = true, AdvancePaymentCount = advancePaymentCount }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = true, AdvancePaymentCount = advancePaymentCount }, JsonRequestBehavior.AllowGet);
                }
            }

            catch
            {
                return Json(new { Success = false, }, JsonRequestBehavior.AllowGet);
            }
        }


        //[Authorize(Roles = "2")]


        //   [UserRIghtCheck(ControllerValue = AppUtils.Transaction_Pa)]
        public ActionResult GetMyPaymentHistory()
        {

            var loginID = AppUtils.GetLoginUserID();
            List<Transaction> lstTransaction = db.Transaction.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/ && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).ToList();

            return View(lstTransaction);
        }

        public ActionResult GetClientPaymentHistoryByCID(int CID)
        {



            List<Transaction> lstTransaction = db.Transaction.Where(s => s.ClientDetailsID == CID && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).ToList();
            ViewBag.popName = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.Name : "";
            ViewBag.popLoginName = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.LoginName : "";
            ViewBag.ClientZone = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.Zone.ZoneName : "";
            ViewBag.ClientAddress = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.Address : "";
            ViewBag.ConnectionType = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.ConnectionType.ConnectionTypeName : "";
            ViewBag.ContactNumber = lstTransaction.Count > 0 ? lstTransaction[0].ClientDetails.ContactNumber : "";


            return View(lstTransaction);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchSignUpBillBySearchCriteria(int? YearID, int? MonthID, int? ZoneID)
        {
            if (YearID.Value > 0)
            {
                YearID = Convert.ToInt32(db.Year.Where(s => s.YearID == YearID.Value).Select(s => s.YearName).FirstOrDefault());
            }

            //List<Complain> lstComplain = new List<Complain>();

            List<CustomSignUpBills> lstTransactions = new List<CustomSignUpBills>();
            dynamic temp = new ExpandoObject();
            // private EnumerableRowCollection query;
            //var lstTransaction = db.Transaction.Where(s =>
            //        s.PaymentDate >= startDate && s.PaymentDate <= endDate &&
            //        s.PaymentStatus == AppUtils.PaymentIsPaid &&
            //        s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //    .GroupJoin(
            //        db.Transaction.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month),
            //        TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID,
            //        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
            //    .Select(s => new CustomSignUpBills
            //    {
            //        TransactionID = s.TCon.TransactionID,
            //        ClientDetailsID = s.TCon.ClientDetailsID,
            //        Name = s.TCon.ClientDetails.Name,
            //        Address = s.TCon.ClientDetails.Address,
            //        ContactNumber = s.TCon.ClientDetails.ContactNumber,
            //        ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
            //        PackageName = s.TMon.Package.PackageName,
            //        PackagePrice = s.TMon.PaymentAmount.ToString(),
            //        SignUpFee = s.TCon.PaymentAmount.ToString(),
            //        PaymentDate = s.TCon.PaymentDate.Value,
            //        RemarksNo = s.TCon.RemarksNo,

            //    }).ToList();


            if (YearID > 0 && MonthID > 0 && ZoneID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                //  lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate && s.EmployeeID == EmployeeID).OrderByDescending(s => s.ComplainTime).ToList();
                lstTransactions = db.Transaction.Where(s =>
                            s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.ClientDetails.ZoneID == ZoneID
                            && s.PaymentTypeID == AppUtils.SignUpBillIndicator && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                        .GroupJoin(
                            db.Transaction.Where(sss => sss.PaymentYear == startDate.Year && sss.PaymentMonth == startDate.Month),
                            TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID, (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                            .AsEnumerable()
                            .Select(s => new CustomSignUpBills()
                            {
                                ClientDetailsID = s.TCon.ClientDetailsID,
                                Name = s.TCon.ClientDetails.Name,
                                TransactionID = s.TCon.TransactionID,
                                Address = s.TCon.ClientDetails.Address,
                                ContactNumber = s.TCon.ClientDetails.ContactNumber,
                                ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                                PackageName = s.TMon.Package.PackageName,
                                PackagePrice = s.TMon.PaymentAmount.ToString(),
                                SignUpFee = s.TCon.PaymentAmount.ToString(),
                                PaymentDate = s.TCon.PaymentDate.Value,
                                RemarksNo = s.TCon.RemarksNo.ToString(),
                            }).ToList()
                            ;
                //lstTransactions = db.Transaction.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.ClientDetails.ZoneID == ZoneID && s.PaymentTypeID == AppUtils.SignUpBillIndicator).ToList();
            }
            else if (YearID > 0 && MonthID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                // lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
                //lstTransactions = db.Transaction.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.PaymentTypeID == AppUtils.SignUpBillIndicator).OrderByDescending(s => s.PaymentDate).ToList();
                lstTransactions = db.Transaction.Where(s =>
                            s.PaymentDate >= startDate && s.PaymentDate <= lastDate
                            && s.PaymentTypeID == AppUtils.SignUpBillIndicator && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                        .GroupJoin(
                            db.Transaction.Where(sss => sss.PaymentYear == startDate.Year && sss.PaymentMonth == startDate.Month),
                            TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID, (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                        .AsEnumerable()
                        .Select(
                    s => new CustomSignUpBills()
                    {
                        ClientDetailsID = s.TCon.ClientDetailsID,
                        Name = s.TCon.ClientDetails.Name,
                        TransactionID = s.TCon.TransactionID,
                        Address = s.TCon.ClientDetails.Address,
                        ContactNumber = s.TCon.ClientDetails.ContactNumber,
                        ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                        PackageName = s.TMon.Package.PackageName,
                        PackagePrice = s.TMon.PaymentAmount.ToString(),
                        SignUpFee = s.TCon.PaymentAmount.ToString(),
                        PaymentDate = s.TCon.PaymentDate.Value,
                        RemarksNo = s.TCon.RemarksNo.ToString(),
                    }).ToList()
                    ;
            }
            else if (YearID > 0 && ZoneID > 0)
            {
                DateTime startDate = new DateTime(YearID.Value, 1, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                // lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
                //lstTransactions = db.Transaction.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.PaymentTypeID == AppUtils.SignUpBillIndicator).OrderByDescending(s => s.PaymentDate).ToList();
                lstTransactions = db.Transaction.Where(s =>
                            s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.ClientDetails.ZoneID == ZoneID
                            && s.PaymentTypeID == AppUtils.SignUpBillIndicator && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                        .GroupJoin(
                            db.Transaction.Where(sss => sss.PaymentYear == startDate.Year && sss.PaymentMonth == startDate.Month),
                            TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID, (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                        .AsEnumerable()
                        .Select(s => new CustomSignUpBills()
                        {
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            TransactionID = s.TCon.TransactionID,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = s.TMon.PaymentAmount.ToString(),
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.RemarksNo.ToString(),
                        }).ToList()
                    ;
            }
            else if (YearID > 0)
            {

                DateTime startDate = new DateTime(YearID.Value, 1, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                //  lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
                // lstTransactions = db.Transaction.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.PaymentTypeID == AppUtils.SignUpBillIndicator).OrderByDescending(s => s.PaymentDate).ToList();
                lstTransactions = db.Transaction.Where(s =>
                            s.PaymentDate >= startDate && s.PaymentDate <= lastDate
                            && s.PaymentTypeID == AppUtils.SignUpBillIndicator && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                        .GroupJoin(
                            db.Transaction.Where(sss => sss.PaymentYear == startDate.Year && sss.PaymentMonth == startDate.Month),
                            TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID, (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                        .AsEnumerable()
                        .Select(s => new CustomSignUpBills()
                        {
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            TransactionID = s.TCon.TransactionID,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = s.TMon.PaymentAmount.ToString(),
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.RemarksNo.ToString(),
                        }).ToList()
                    ;
            }
            else if (ZoneID > 0)
            {

                DateTime startDate = new DateTime(YearID.Value, 1, 1);
                DateTime lastDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                //  lstComplain = db.Complain.Where(s => s.ComplainTime >= startDate && s.ComplainTime <= lastDate).OrderByDescending(s => s.ComplainTime).ToList();
                // lstTransactions = db.Transaction.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.PaymentTypeID == AppUtils.SignUpBillIndicator).OrderByDescending(s => s.PaymentDate).ToList();
                lstTransactions = db.Transaction.Where(s =>
                            s.PaymentDate >= startDate && s.PaymentDate <= lastDate && s.ClientDetails.ZoneID == ZoneID
                            && s.PaymentTypeID == AppUtils.SignUpBillIndicator && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                        .GroupJoin(
                            db.Transaction.Where(sss => sss.PaymentYear == startDate.Year && sss.PaymentMonth == startDate.Month),
                            TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID, (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                        .AsEnumerable()
                        .Select(s => new CustomSignUpBills()
                        {
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            TransactionID = s.TCon.TransactionID,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = s.TMon.PaymentAmount.ToString(),
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.RemarksNo.ToString(),
                        }).ToList()
                    ;
            }
            else
            {
                //lstTransactions = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.SignUpBillIndicator).ToList();
            }
            //lstTransactions = temp
            //var ss = lstTransactions.Select(s => new
            //{
            //    Name = s.ClientDetails.Name,
            //    ClientDetailsID = s.ClientDetails.ClientDetailsID,
            //    TransactionID = s.TransactionID,
            //    LoginName = s.ClientDetails.LoginName,
            //    Address = s.ClientDetails.Address,
            //    ContactNumber = s.ClientDetails.ContactNumber,
            //    ZoneName = s.ClientDetails.Zone.ZoneName,
            //    PackageName = s.Package.PackageName,
            //    PackagePrice = s.Package.PackagePrice,
            //    PaymentDate = s.PaymentDate,
            //    RemarksNo = s.RemarksNo
            //});


            return Json(new { Success = true, TransactionsList = lstTransactions, TransactionsCount = lstTransactions.Count() }, JsonRequestBehavior.AllowGet);
        }



        private void SetBillSummary()
        {
            // DateTime customDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 01);
            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());

            var lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Select(s => new { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount }).ToList();
            var lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
            var lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new { PaymentAmount = s.PaymentAmount }).ToList();
            var expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

            //List<Transaction> lstTransaction = db.Transaction.ToList();
            //List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();
            //List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
            //List<Transaction> lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).ToList();

            ViewBag.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
            ViewBag.clnCollectedAmount = Math.Round(lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s => s.PaymentAmount.Value) + lstRegularSignUpBill.Sum(s => s.PaymentAmount.Value), 2);
            ViewBag.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
            ViewBag.clnCollectedAmountBIll = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s => s.PaymentAmount);
            ViewBag.clnOnlinePayment = 0;
            DateTime lstDateForLinq = AppUtils.GetLastDayWithHrMinSecMsByMyDate(endDate);
            ViewBag.clnInstallationAmount = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= lstDateForLinq).Sum(ss => ss.PaymentAmount);
            //ViewBag.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice) - ((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) + lstTransactionForBillSummary.Sum(s => s.Discount));
            double a = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount).Value;
            double b = lstRegularMonthlyBill.Sum(s => s.PaymentAmount).Value;
            ViewBag.clnDueAmount = a/*(a - b) < 0 ? 0 : -1*(b - a)*/;
            //ViewBag.clnDueAmount = (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount)) < 0 ? 0 : (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount));
            ViewBag.clnTotalExpense = expense;
            ViewBag.clnRestOfAmount = Math.Round((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount.Value) + lstRegularSignUpBill.Sum(s => s.PaymentAmount.Value)) - expense, 2);
            ViewBag.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
            ViewBag.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count();
            ViewBag.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count());
            ViewBag.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount);
        }
        //i am here???
        private void SetBillSummaryForAjaxCall(/*IEnumerable<VM_Transaction_ClientDueBills> lstTransaction, List<Transaction> lstSignUpBill,*/ List<Expense> lstExpenses, ref dynamic billSummaryDetails, int? YearID, int? MonthID, int? ZoneID, int? ResellerID = null)
        {// is this for all?
            double expense = 0;
            List<RegularSignUpBill> lstRegularSignUpBill = new List<RegularSignUpBill>();

            int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
            int month = MonthID != null ? MonthID.Value : AppUtils.RunningMonth;
            int zone = ZoneID != null ? ZoneID.Value : 0;

            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            List<TransactionTemp> lstTransactionForBillSummary = new List<TransactionTemp>();
            IEnumerable<TransactionTemp> lstRegularMonthlyBill = new List<TransactionTemp>();

            if (YearID != null && MonthID != null && zone > 0)
            {
                startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentYear == year && s.PaymentMonth == month && s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;

            }
            else if (YearID != null && MonthID != null && zone == 0)
            {
                startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentYear == year && s.PaymentMonth == month).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => s.ResellerID == ResellerID && !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;
            }
            else if (YearID != null && MonthID == null && zone > 0)
            {
                startDate = new DateTime(YearID.Value, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentYear == year && s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else if (YearID != null && MonthID == null && zone == 0)
            {
                startDate = new DateTime(YearID.Value, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentYear == year).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else if (YearID == null && MonthID == null && zone > 0)
            {
                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else
            {//date time is default 
                lstTransactionForBillSummary = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value, PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.ResellerID == ResellerID && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ResellerID == ResellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => s.ResellerID == ResellerID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;
            }

            billSummaryDetails.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
            billSummaryDetails.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
            billSummaryDetails.clnCollectedAmountBIll = lstRegularMonthlyBill.Sum(s => s.PaidAmount) + billSummaryDetails.clnDiscountAmount;
            billSummaryDetails.clnCollectedAmount = Math.Round(lstRegularMonthlyBill.Sum(s => s.PaidAmount) + billSummaryDetails.clnDiscountAmount + lstRegularSignUpBill.Sum(s => s.PaymentAmount), 2);

            billSummaryDetails.clnOnlinePayment = 0;
            DateTime lstDateForLinq = AppUtils.GetLastDayWithHrMinSecMsByMyDate(endDate);

            double a = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => (s.PaymentAmount - s.PaidAmount - s.Discount));
            double b = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
            billSummaryDetails.clnDueAmount = a/*(a - b) < 0 ? 0 : -1*(b - a)*/;

            billSummaryDetails.clnTotalExpense = expense;
            billSummaryDetails.clnRestOfAmount = billSummaryDetails.clnCollectedAmount - billSummaryDetails.clnTotalExpense;//Math.Round((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) - expense, 2);
            billSummaryDetails.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
            billSummaryDetails.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count();
            billSummaryDetails.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count());

        }

        class TransactionTemp
        {
            public int ClientDetailsID { get; set; }
            public float Discount { get; set; }
            public int PaymentTypeID { get; set; }
            public Package Package { get; set; }
            public int PaymentStatus { get; set; }
            public float PaymentAmount { get; set; }
            public float PaidAmount { get; set; }
        }
        class RegularSignUpBill
        {
            public float PaymentAmount { get; set; }
        }


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Bill_Submit_By_Me)]
        public ActionResult SubmitBillByMeSpecificMonth()
        {
            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

            return View();
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Bill_Submit_By_Admin)]
        public ActionResult SubmitBillByMeAdminSpecificMonth()
        {
            //      ViewBag.DueEmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }



        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Bill_Submit_By_Me)]
        public ActionResult SubmitBillByMeAnyMonth()
        {
            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            return View();
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Bill_Submit_By_Admin)]
        public ActionResult SubmitBillByMeAdminAnyMonth()
        {
            //      ViewBag.DueEmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            ViewBag.PaymentFrom = new SelectList(db.PaymentBy.Select(s => new { PaymentByID = s.PaymentByID, PaymentByName = s.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            return View();
        }


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Paid_Bills_By_Me)]
        public ActionResult SubmittedBillByMe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSubmittedBillByMe()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var PaymentStartDate = Request.Form.Get("PaymentStartDate");
                var PaymentEndDate = Request.Form.Get("PaymentEndDate");
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (PaymentStartDate != "")
                {
                    startDate = Convert.ToDateTime(PaymentStartDate);
                }
                else
                {
                    startDate = AppUtils.GetDateNow();
                }
                if (PaymentEndDate != "")
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(Convert.ToDateTime(PaymentEndDate));
                }
                else
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.GetDateNow());
                }

                List<VM_Paid_History_Employee> lstVM_Paid_History_Employee = new List<VM_Paid_History_Employee>();

                var loginID = AppUtils.GetLoginUserID();

                var firstPartOfQuery =
                          db.PaymentHistory.OrderByDescending(x => x.PaymentDate).Where(s => s.Status == AppUtils.TableStatusIsActive && s.CollectByID == loginID && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) /*&& s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly*/)/*.Include(x=>x.advancePayment)*/.AsQueryable();
                var secondPartOfQuery = firstPartOfQuery

                          .GroupJoin(db.ClientDetails, ph => ph.ClientDetailsID, ClientDetails => ClientDetails.ClientDetailsID, (PH, ClientDetails) => new
                          {
                              PaymentHistory = PH,
                              ClientDetails = ClientDetails.FirstOrDefault()

                          })
                          .AsEnumerable();

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    //        var a = secondPartOfQuery.ToList();
                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                                                                           p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                          ).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())

                                                                          ).AsEnumerable();
                }
                //     var a = secondPartOfQuery.ToList();
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    var TotalAmount = (secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.PaidAmount == null ? 0 : s.PaymentHistory.PaidAmount) : 0;
                    var TotalActualAmount = (secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.NormalPayment == null ? 0 : s.PaymentHistory.NormalPayment) : 0;
                    var TotalDiscountAmount = (secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.DiscountPayment == null ? 0 : s.PaymentHistory.DiscountPayment) : 0;
                    var TotalAdvanceAmount = (secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.AdvancePaymentID == null ? 0 : s.PaymentHistory.PaidAmount) : 0;

                    //var a = secondPartOfQuery.ToList();
                    lstVM_Paid_History_Employee = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new VM_Paid_History_Employee()
                        {
                            TransactionID = s.PaymentHistory.TransactionID != null ? s.PaymentHistory.TransactionID.Value : 0,
                            LoginID = s.ClientDetails.LoginName,
                            ClientName = s.ClientDetails.Name,
                            Address = s.ClientDetails.Address,
                            ContactNumber = s.ClientDetails.ContactNumber,
                            ZoneName = s.ClientDetails.Zone.ZoneName,
                            TotalAllAmount = TotalAmount.ToString(),
                            TotalActualAmount = TotalActualAmount.ToString(),
                            TotalDiscountAmount = TotalDiscountAmount.ToString(),
                            TotalAdvanceAmount = TotalAdvanceAmount.ToString(),
                            TotalAmount = s.PaymentHistory.PaidAmount.ToString(),//TotalAmount.ToString(),
                            PaidAmount = s.PaymentHistory.NormalPayment != null ? s.PaymentHistory.NormalPayment.Value : 0,
                            Discount = s.PaymentHistory.DiscountPayment != null ? s.PaymentHistory.DiscountPayment.Value : 0,
                            PaidTime = s.PaymentHistory.PaymentDate.ToString(),
                            ReceiptNo = s.PaymentHistory.ResetNo,
                            PaymentType = s.PaymentHistory.AdvancePaymentID == null ? "MonthlyBill" : "Advance",
                            PaymentBy = s.PaymentHistory.PaymentByID != null ? db.PaymentBy.Find(s.PaymentHistory.PaymentByID).PaymentByName : ""

                        }).ToList();

                }

                // Sorting.   
                lstVM_Paid_History_Employee = this.SortByColumnWithOrderForShowingMyBill(order, orderDir, lstVM_Paid_History_Employee);
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
                    data = lstVM_Paid_History_Employee
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

        private List<VM_Paid_History_Employee> SortByColumnWithOrderForShowingMyBill(string order, string orderDir, List<VM_Paid_History_Employee> data)
        {

            // Initialization.   
            List<VM_Paid_History_Employee> lst = new List<VM_Paid_History_Employee>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LoginID).ToList() : data.OrderBy(p => p.LoginID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ZoneName).ToList() : data.OrderBy(p => p.ZoneName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidAmount).ToList() : data.OrderBy(p => p.PaidAmount).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidTime).ToList() : data.OrderBy(p => p.PaidTime).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceiptNo).ToList() : data.OrderBy(p => p.ReceiptNo).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
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


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Paid_Bills_By_Employee)]
        public ActionResult SubmittedBillByEmployee()
        {
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSubmittedBillByEmployee()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var EmployeeID = Request.Form.Get("EmployeeID");
                var PaymentStartDate = Request.Form.Get("PaymentStartDate");
                var PaymentEndDate = Request.Form.Get("PaymentEndDate");

                int empID = 0;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (PaymentStartDate != "")
                {
                    startDate = Convert.ToDateTime(PaymentStartDate);
                }
                else
                {
                    startDate = AppUtils.GetDateNow();
                }
                if (PaymentEndDate != "")
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(Convert.ToDateTime(PaymentEndDate));
                }
                else
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.GetDateNow());
                }

                if (EmployeeID != "")
                {
                    empID = int.Parse(EmployeeID);
                }


                List<VM_Paid_History_Employee> lstVM_Paid_History_Employee = new List<VM_Paid_History_Employee>();

                var firstPartOfQuery =
                    (empID != 0 && PaymentStartDate != "" && PaymentEndDate != "") ? db.PaymentHistory.Where(s => s.Status != AppUtils.TableStatusIsDelete && s.CollectByID == empID && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) /*&& s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly*/).AsQueryable()
                          :/*: (PaymentStartDate != "" && PaymentEndDate != "") ?*/ db.PaymentHistory.Where(s => s.Status != AppUtils.TableStatusIsDelete && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) /*&& s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly*/).AsQueryable();
                //: db.Transaction.Where(s => s.BillCollectBy == empID && (s.PaymentDate.Value >= startDate && s.PaymentDate.Value <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                ;
                var secondPartOfQuery = firstPartOfQuery

                           .GroupJoin(db.ClientDetails, ph => ph.Transaction.ClientDetailsID, ClientDetails => ClientDetails.ClientDetailsID, (PH, ClientDetails) => new
                           {
                               PaymentHistory = PH,
                               ClientDetails = ClientDetails.FirstOrDefault()

                           })
                           .AsEnumerable();

                var a = secondPartOfQuery.ToList();
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                                                                           p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                          ).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())

                                                                          ).AsEnumerable();
                }
                //     var a = secondPartOfQuery.ToList();
                if (secondPartOfQuery.Count() > 0)
                {
                    var showDeleteButton = false;
                    var loginID = AppUtils.GetLoginUserID();

                    Session["CurrentUserRightPermission"] = db.Employee.Where(s => s.EmployeeID == loginID/*AppUtils.LoginUserID*/).Select(s => s.UserRightPermissionID).FirstOrDefault().Value;
                    int CurrentUserRightPermission = (int)Session["CurrentUserRightPermission"];

                    UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();
                    if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                    {
                        List<string> lstAcessList = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                        if (lstAcessList.Contains(AppUtils.Delete_Payment_From_Employee_Payment_For_Mistake))
                        {
                            showDeleteButton = true;
                        }
                    }

                    totalRecords = secondPartOfQuery.Count();
                    var TotalAmount = (secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.PaidAmount == null ? 0 : s.PaymentHistory.PaidAmount) : 0;
                    lstVM_Paid_History_Employee = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new VM_Paid_History_Employee()
                        {
                            TransactionID = s.PaymentHistory.TransactionID != null ? s.PaymentHistory.TransactionID.Value : 0,
                            LoginID = s.ClientDetails.LoginName,
                            ClientName = s.ClientDetails.Name,
                            Address = s.ClientDetails.Address,
                            ContactNumber = s.ClientDetails.ContactNumber,
                            ZoneName = s.ClientDetails.Zone.ZoneName,
                            PaidAmount = s.PaymentHistory.PaidAmount,
                            PaidTime = s.PaymentHistory.PaymentDate.ToString(),
                            ReceiptNo = s.PaymentHistory.ResetNo,
                            PaymentType = s.PaymentHistory.AdvancePaymentID == null ? "MonthlyBill" : "Advance",
                            TotalAmount = TotalAmount.ToString(),
                            PaymentBy = s.PaymentHistory.PaymentByID != null ? db.PaymentBy.Find(s.PaymentHistory.PaymentByID).PaymentByName : "",
                            ShowDeleteButton = showDeleteButton
                        }).ToList();

                }

                // Sorting.   
                lstVM_Paid_History_Employee = this.SortByColumnWithOrderForShowingMyBill(order, orderDir, lstVM_Paid_History_Employee);
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
                    data = lstVM_Paid_History_Employee
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
        [UserRIghtCheck(ControllerValue = AppUtils.Received_Collected_Bill)]
        public ActionResult AcceptEmployeeCollectedBill()
        {
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAcceptEmployeeCollectedBillByEmployee()
        {


            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var EmployeeID = Request.Form.Get("EmployeeID");
                var PaymentStartDate = Request.Form.Get("PaymentStartDate");
                var PaymentEndDate = Request.Form.Get("PaymentEndDate");

                int empID = 0;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (PaymentStartDate != "")
                {
                    startDate = Convert.ToDateTime(PaymentStartDate);
                }
                else
                {
                    startDate = AppUtils.GetDateNow();
                }
                if (PaymentEndDate != "")
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(Convert.ToDateTime(PaymentEndDate));
                }
                else
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.GetDateNow());
                }

                if (EmployeeID != "")
                {
                    empID = int.Parse(EmployeeID);
                }


                List<VM_Paid_History_Employee> lstVM_Paid_History_Employee = new List<VM_Paid_History_Employee>();

                var firstPartOfQuery =
                    (empID != 0 && PaymentStartDate != "" && PaymentEndDate != "") ? db.PaymentHistory.Where(s => s.Status != AppUtils.TableStatusIsDelete && s.AcceptStatus == false && s.CollectByID == empID && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).GroupBy(x => x.CollectByID).AsQueryable()
                          :/*: (PaymentStartDate != "" && PaymentEndDate != "") ?*/ db.PaymentHistory.Where(s => s.Status != AppUtils.TableStatusIsDelete && s.AcceptStatus == false && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).GroupBy(x => x.CollectByID).AsQueryable();
                ;
                var secondPartOfQuery = firstPartOfQuery

                           .GroupJoin(db.ClientDetails, ph => ph.FirstOrDefault().ClientDetailsID, ClientDetails => ClientDetails.ClientDetailsID, (PH, ClientDetails) => new
                           {
                               PaymentHistory = PH,
                               ClientDetails = ClientDetails.FirstOrDefault()

                           })
                           .AsEnumerable();

                //var a = secondPartOfQuery.ToList();
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                                                                           p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                          ).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.ClientDetails.Zone.ZoneName.Contains(search.ToLower())

                                                                          ).AsEnumerable();
                }
                //     var a = secondPartOfQuery.ToList();
                if (secondPartOfQuery.Count() > 0)
                {
                    var showAccepteButton = true;
                    //var loginID = AppUtils.GetLoginUserID();

                    //int CurrentUserRightPermission = AppUtils.GetLoginUserRightPermissionID();

                    //UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();
                    //if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
                    //{
                    //    List<string> lstAcessList = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).Select(s => s.UserRightPermissionDetails).FirstOrDefault().Split(',').ToList();
                    //    if (lstAcessList.Contains(AppUtils.Delete_Payment_From_Employee_Payment_For_Mistake))
                    //    {
                    //        showDeleteButton = true;
                    //    }
                    //}

                    totalRecords = secondPartOfQuery.Count();
                    var TotalAmount = 0;//(secondPartOfQuery.Count() > 0) ? secondPartOfQuery.Sum(s => s.PaymentHistory.ForEach(x=>x.PaidAmount) == null ? 0 : s.PaymentHistory.PaidAmount) : 0;
                    lstVM_Paid_History_Employee = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new VM_Paid_History_Employee()
                        {
                            TransactionID = s.PaymentHistory.FirstOrDefault().TransactionID.Value,
                            PaidAmount = s.PaymentHistory.Sum(x => x.PaidAmount),
                            PaidTime = startDate + "-" + endDate,//s.PaymentHistory.FirstOrDefault().PaymentDate.ToString(),
                            ReceiptNo = s.PaymentHistory.FirstOrDefault().ResetNo,
                            CollectBy = s.PaymentHistory.FirstOrDefault().CollectByID.ToString(),
                            CollectByName = db.Employee.Find(s.PaymentHistory.FirstOrDefault().CollectByID).Name,
                            ShowAcceptButton = showAccepteButton,
                            StartDate = startDate.Date,
                            EndDate = endDate.Date
                        }).ToList();

                }

                // Sorting.   
                lstVM_Paid_History_Employee = this.SortByColumnWithOrderForShowingMyBill(order, orderDir, lstVM_Paid_History_Employee);
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
                    data = lstVM_Paid_History_Employee
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
        [UserRIghtCheck(ControllerValue = AppUtils.Received_Collected_Bill)]
        public ActionResult AcceptBill(int acceptFor, DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime startDate = fromDate;
                DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(toDate);

                List<PaymentHistory> lstPaymentHistory = db.PaymentHistory.Where(x => x.CollectByID == acceptFor && (x.PaymentDate >= startDate && x.PaymentDate <= endDate)).ToList();
                foreach (var paymentHistory in lstPaymentHistory)
                {
                    paymentHistory.BillAcceptBy = AppUtils.GetLoginUserID();
                    paymentHistory.AcceptStatus = true;
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Amount Received Successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Something Wrong Contact With Administrator." }, JsonRequestBehavior.AllowGet);
            }


        }



        private List<VM_Paid_History_Employee> SortByColumnWithOrderForShowingEmployeeBill(string order, string orderDir, List<VM_Paid_History_Employee> data)
        {

            // Initialization.   
            List<VM_Paid_History_Employee> lst = new List<VM_Paid_History_Employee>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LoginID).ToList() : data.OrderBy(p => p.LoginID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ZoneName).ToList() : data.OrderBy(p => p.ZoneName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FeeForThisMonth).ToList() : data.OrderBy(p => p.FeeForThisMonth).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidAmount).ToList() : data.OrderBy(p => p.PaidAmount).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Discount).ToList() : data.OrderBy(p => p.Discount).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CollectBy).ToList() : data.OrderBy(p => p.CollectBy).ToList();
                        break;
                    case "11":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidTime).ToList() : data.OrderBy(p => p.PaidTime).ToList();
                        break;
                    case "12":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RemarksNo).ToList() : data.OrderBy(p => p.RemarksNo).ToList();
                        break;
                    case "13":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceiptNo).ToList() : data.OrderBy(p => p.ReceiptNo).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
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

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Pay_Multiple_Bill)]
        public ActionResult PayMultipleBill()
        {
            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
            ViewBag.SearchByZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPayMultipleBill(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                Session["IdListForPayment"] = null;

                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   


                bool isCheckAllFromCln = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };

                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");

                var IsCheckAll = Request.Form.Get("IsCheckAll");

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }


                if (IsCheckAll != null)
                {
                    isCheckAllFromCln = bool.Parse(IsCheckAll);
                }
                if (IfIsCheckAllThenNonCheckList != null)
                {
                    IfIsCheckAllThenNonCheckLists = Array.ConvertAll(IfIsCheckAllThenNonCheckList.ToArray(), c => int.Parse(c));
                }
                if (IfNotCheckAllThenCheckList != null)
                {
                    IfNotCheckAllThenCheckLists = Array.ConvertAll(IfNotCheckAllThenCheckList.ToArray(), c => int.Parse(c));
                }

                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

                var firstPartOfQuery =
                        (YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                            : (YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                : (YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                    : (YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                        : (YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                            :
                                            db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                    ;
                var secondPartOfQuery = firstPartOfQuery
                    .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                        ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                        {
                            Transaction = Transaction,
                            ClientDueBills = ClientDueBills
                        })
                    .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                        (Transaction, EmployeeTransactionLockUnlock) => new
                        {
                            Transaction = Transaction.Transaction,
                            ClientDueBills = Transaction.ClientDueBills,
                            EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                        })
                    .AsEnumerable();


                Session["IdListForPayment"] = secondPartOfQuery.Select(s => new PayBillCustomInformation
                {
                    TransactionID = s.Transaction.TransactionID
                }).ToList();
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>
                                                                                           p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                           || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                        )
                                                                                        .Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p =>
                                                                        p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                        || p.Transaction.ClientDetails.LoginName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                       ).AsEnumerable();
                    //    var a = secondPartOfQuery.ToList();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                        {
                            chkPayBill = CheckPaymentTransactionIDOrNot(s.Transaction.TransactionID, isCheckAllFromCln,/*SMSSendAry,*/ IfIsCheckAllThenNonCheckLists, IfNotCheckAllThenCheckLists),

                            TransactionID = s.Transaction.TransactionID,
                            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                            ClientName = s.Transaction.ClientDetails.Name,
                            ClientLoginName = s.Transaction.ClientDetails.LoginName,
                            Address = s.Transaction.ClientDetails.Address,
                            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                            Email = s.Transaction.ClientDetails.Email,

                            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                            PackageName = s.Transaction.Package.PackageName,
                            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                            PaidAmount = s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0,
                            Discount = (s.Transaction.Discount == null ? 0 : s.Transaction.Discount.Value),
                            Due = Math.Round(s.Transaction.PaymentAmount.Value, 2) - ((s.Transaction.PaidAmount != null ? s.Transaction.PaidAmount.Value : 0)),
                            IsPriorityClient = s.Transaction.ClientDetails.IsPriorityClient
                        }).ToList();

                }

                // Sorting.   
                //lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
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
                    data = lstArchiveBillsInformation
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
        public ActionResult PayMultipleBill(bool IsCheckAll, string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            try
            {
                List<string> lstMobileNumber = new List<string>();
                List<PayBillCustomInformation> txIdList = Session["IdListForPayment"] as List<PayBillCustomInformation>;
                List<PayBillCustomInformation> lstTSID = new List<PayBillCustomInformation>();
                if (IsCheckAll)
                {
                    if (IfIsCheckAllThenNonCheckList != null)
                    {
                        lstTSID = txIdList.Where(x => !IfIsCheckAllThenNonCheckList.Contains(x.TransactionID.ToString())).ToList();
                    }
                    else
                    {
                        lstTSID = txIdList;
                    }
                }
                else
                {
                    if (IfNotCheckAllThenCheckList != null)
                    {
                        lstTSID = txIdList.Where(x => IfNotCheckAllThenCheckList.Contains(x.TransactionID.ToString())).ToList(); ;
                    }
                }

                foreach (var transaction in lstTSID)
                {
                    var amount = 0.0;
                    Transaction ts = db.Transaction.Where(s => s.TransactionID == transaction.TransactionID).FirstOrDefault();
                    if (ts != null)
                    {
                        amount = ts.DueAmount == null ? ts.PaymentAmount.Value : ts.DueAmount.Value;
                        ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
                        ts.PaidAmount = ts.PaymentAmount;
                        ts.DueAmount = 0;
                        ts.BillCollectBy = int.Parse(Session["LoggedUserID"].ToString());
                        ts.Discount = ts.Discount;
                        ts.PaymentStatus = AppUtils.PaymentIsPaid;
                        ts.PaymentDate = AppUtils.GetDateTimeNow();
                        ts.RemarksNo = "";
                        ts.ResetNo = "";
                        ts.PaymentFromWhichPage = AppUtils.PamentIsOccouringFromPayBulkBill.ToString();

                        db.Entry(ts).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    UpdatePaymentIntoPaymentHistoryTableByOneTransactionInformation(ts, amount);

                }

                //if (AppUtils.SMSOptionEnable)
                //{
                //    foreach (var client in lsttemp)
                //    {
                //        try
                //        {
                //            //send sms here

                //            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                //            if (smsSenderIdPass != null)
                //            {
                //                SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, client.Phone, message);
                //                if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                //                {

                //                }
                //                else
                //                {
                //                    lstMobileNumber.Add(client.Phone);
                //                }

                //            }
                //        }
                //        catch (Exception e)
                //        {
                //            lstMobileNumber.Add(client.Phone);
                //        }
                //    }
                //}
                return Json(new { payMultipleBillSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { payMultipleBillFail = true }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTheBill(int TxID, string rstNO)
        {
            try
            {
                Transaction transaction = db.Transaction.Where(s => s.TransactionID == TxID).FirstOrDefault();
                List<PaymentHistory> lstPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim() == rstNO.Trim() && s.TransactionID == TxID).ToList();
                foreach (var paymentHistory in lstPaymentHistory)
                {
                    if (paymentHistory.AdvancePaymentID != null)
                    {
                        AdvancePayment adv = db.AdvancePayment.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
                        if (adv != null)
                        {
                            adv.AdvanceAmount -= paymentHistory.PaidAmount;
                            adv.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                            adv.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                            db.Entry(adv).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        transaction.PaidAmount -= paymentHistory.PaidAmount;
                    }
                    paymentHistory.Status = AppUtils.TableStatusIsDelete;
                    db.Entry(paymentHistory).State = EntityState.Modified;
                    db.SaveChanges();
                }
                transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
                transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();


                return Json(new { Success = true, TSID = TxID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, TSID = TxID }, JsonRequestBehavior.AllowGet);
            }



        }

        private string CheckPaymentTransactionIDOrNot(int txID, bool isCheckAllFromCln/*,int[] SMSSendArray*/, int[] IfIsCheckAllThenNonCheckList, int[] IfNotCheckAllThenCheckList)
        {
            bool chkBoxCheck = false;
            var chk = "";
            if (isCheckAllFromCln)
            {
                if (IfIsCheckAllThenNonCheckList.Contains(txID))
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
                if (IfNotCheckAllThenCheckList.Contains(txID))
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
            return "<div style='margin-left:1px' class='checkbox checkbox-danger'><input type='checkbox' id='chkTXID" + txID + "' name='chkTXID" + txID + "' onclick='enableDisablePaymentID(chkTXID" + txID + ")' " + chk + " > <label for= 'chkTXID" + txID + "'> </label ></div>";
            //"<div style='margin-left:1px' class='checkbox checkbox-danger'><input type='checkbox' id='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' name='chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "' onclick='enableDisableSMSSendOption(chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "," + s.ClientDetails.ClientDetails.ClientDetailsID + ")' checked="+checkOrNot+"> <label for= 'chkSMS" + s.ClientDetails.ClientDetails.ClientDetailsID + "'> </label ></div>"
        }


        #region Payment

        //for getting the client details and sum of due from ui autocomplete text box
        //from submitbillbymespecificmonth submitbillbymeadminspecificmonth etc...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowClientInfoWIthSumOfTotalDueByClientDetailsID(int ClientDetailsID)
        {

            var lstTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly /*&& !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)*/).Select(s => new
            {
                PaymentAmount = s.PaymentAmount,
                PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount,
                Discount = s.Discount == null ? 0 : s.Discount
            }).AsEnumerable();

            var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID).Select(s => new
            {
                Name = s.Name,
                LoginName = s.LoginName,
                Zone = s.Zone.ZoneName,
                Address = s.Address,
                Contact = s.ContactNumber
            }).FirstOrDefault();

            var ActualAmount = lstTransaction.Sum(s => s.PaymentAmount);
            var PaidAmount = lstTransaction.Sum(s => s.PaidAmount);
            var DiscountAmount = lstTransaction.Sum(s => s.Discount);
            var TotalAmountAfterDiscount = ActualAmount - (PaidAmount + DiscountAmount);

            return Json(new { /*ActualAmount = ActualAmount, DiscountAmount = DiscountAmount,*/ TotalAmountAfterDiscount = TotalAmountAfterDiscount, ClientDetails = ClientDetails, Success = true }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult PayMonthlyBill(Transaction Transaction)
        //{
        //    try
        //    {
        //        float remainingAmount = 0;
        //        int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
        //        int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
        //        if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //        {
        //            return Json(new { Success = false, ResetNoAlreadyExist = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage });
        //        }

        //        if (string.IsNullOrEmpty(Transaction.ResetNo))
        //        {
        //            Transaction.ResetNo = "AutoRST" + SerialNo();
        //        }

        //        Transaction ts = db.Transaction.Find(Transaction.TransactionID);
        //        List<Transaction> lstDueTransactions = db.Transaction.Where(s => s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.ClientDetailsID == ts.ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //        if (ts.TransactionID > 0)
        //        {
        //            if (Transaction.PaidAmount + Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount) > ts.PaymentAmount)
        //            {
        //                var a = (Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount));
        //                remainingAmount = Transaction.PaidAmount.Value - (ts.PaymentAmount.Value - a.Value);
        //                Transaction.PaidAmount = ts.PaymentAmount - a;
        //            }
        //            //ts.PaymentAmount = ts.PaymentAmount;
        //            ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
        //            ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : ts.PaidAmount + Transaction.PaidAmount;
        //            ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
        //            ts.BillCollectBy = Transaction.BillCollectBy;
        //            ts.Discount += Transaction.Discount;
        //            ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
        //            ts.PaymentDate = AppUtils.GetDateTimeNow();
        //            ts.RemarksNo = Transaction.RemarksNo;
        //            ts.ResetNo = Transaction.ResetNo;
        //            ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;

        //            db.Entry(ts).State = EntityState.Modified;
        //            db.SaveChanges();

        //            UpdatePaymentIntoPaymentHistoryTable(Transaction, ts);
        //            if (remainingAmount > 0 && lstDueTransactions.Count > 0)
        //            {
        //                foreach (var dueTransaction in lstDueTransactions)
        //                {
        //                    if (remainingAmount >= 0)
        //                    {
        //                        double paymentAmount = dueTransaction.PaymentAmount.Value;
        //                        double paidAmount = dueTransaction.PaidAmount != null ? dueTransaction.PaidAmount.Value : 0;
        //                        //double dueAmount = dueTransaction.DueAmount != null ? dueTransaction.DueAmount.Value : paymentAmount;
        //                        double requireAmount = paymentAmount - paidAmount;

        //                        if (requireAmount < remainingAmount)//meaning full payment possible cause require is less then remaining amount.
        //                        {

        //                            dueTransaction.PaidAmount += (float?)(remainingAmount - requireAmount);
        //                            dueTransaction.DueAmount = 0;
        //                            dueTransaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //                            dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
        //                            dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
        //                            dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

        //                            remainingAmount -= (float)requireAmount;
        //                        }
        //                        else
        //                        {
        //                            dueTransaction.PaidAmount += (float?)(requireAmount - remainingAmount);
        //                            dueTransaction.DueAmount = (float?)(paymentAmount - paidAmount);
        //                            dueTransaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                            dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
        //                            dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
        //                            dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

        //                            remainingAmount -= (float)requireAmount;
        //                        }
        //                        db.Entry(dueTransaction).State = EntityState.Modified;
        //                        db.SaveChanges();

        //                        //UpdatePaymentIntoPaymentHistoryTable(Transaction, dueTransaction);
        //                        UpdatePaymentIntoPaymentHistoryTable(Transaction, dueTransaction);
        //                    }

        //                }
        //            }
        //            if (remainingAmount > 0 /*&& lstDueTransactions.Count == 0*/)
        //            {
        //                AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();


        //                if (advancePayment != null)
        //                {
        //                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //                    advancePayment.AdvanceAmount += remainingAmount;
        //                    advancePayment.Remarks = "Payment Remarks";
        //                    db.Entry(advancePayment).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                    UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(Transaction, ts, advancePayment, remainingAmount);
        //                }
        //                else
        //                {
        //                    AdvancePayment insertAdvancePayment = new AdvancePayment();
        //                    insertAdvancePayment.ClientDetailsID = ts.ClientDetailsID;
        //                    insertAdvancePayment.AdvanceAmount = remainingAmount;
        //                    insertAdvancePayment.Remarks = "Payment Remarks";
        //                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //                    db.AdvancePayment.Add(insertAdvancePayment);
        //                    db.SaveChanges();
        //                    UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(Transaction, ts, insertAdvancePayment, remainingAmount);
        //                }


        //            }
        //            if ((bool)Session["SMSOptionEnable"])
        //            {
        //                try
        //                {
        //                    SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
        //                    if (smsSenderIdPass != null)
        //                    {
        //                        SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //                        if (sms != null)
        //                        {
        //                            var message = sms.SendMessageText;
        //                            message = message.Replace("[NAME]", ts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(ts.PaymentAmount.Value, 2).ToString());
        //                            message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
        //                            message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

        //                            string smsMobileNo = "";
        //                            if (!string.IsNullOrEmpty(Transaction.AnotherMobileNo))
        //                            {
        //                                smsMobileNo = Transaction.AnotherMobileNo;
        //                            }
        //                            else
        //                            {
        //                                smsMobileNo = ts.ClientDetails.ContactNumber;
        //                            }

        //                            SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, smsMobileNo, message);
        //                            if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
        //                            {
        //                                sms.SMSCounter += 1;
        //                                db.Entry(sms).State = EntityState.Modified;
        //                                db.SaveChanges();
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return Json(new { Success = true, Message = ex.Message, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //                }

        //            }


        //        }
        //        return Json(new { Success = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PayBillByEmployeeOrAdmin(int ClientDetailsID, int PaymentAmount, int Discount, string ResetNo, string RemarksNo, int PaymentFrom)
        //{
        //    int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //    {
        //        return Json(new { Success = false, ResetNoAlreadyExist = true });
        //    }

        //    List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //    foreach (var transaction in lstUnpaidTransaction)
        //    {
        //        int disPayment = 0;
        //        int nmlPayment = 0;

        //        transaction.PaidAmount = transaction.PaidAmount == null ? 0 : transaction.PaidAmount.Value;
        //        transaction.DueAmount = transaction.DueAmount == null ? 0 : transaction.DueAmount.Value;
        //        transaction.Discount = transaction.Discount == null ? 0 : transaction.Discount.Value;
        //        //var howMuchMoneyNeedIfByDiscount = 
        //        if (transaction.DueAmount <= Discount)// mean we can directly pay the bill by discount amount given by admin
        //        {
        //            disPayment = (int)transaction.DueAmount;
        //            //transaction.PaidAmount = transaction.PaymentAmount;
        //            transaction.Discount += transaction.DueAmount;
        //            Discount -= (int)transaction.DueAmount;
        //            transaction.DueAmount = 0;
        //            transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //        }
        //        else//meaning we have pay the amount from payment amount given by client. but if given any discount first we have to use that discount amount first.
        //        {
        //            //cause for if discount amount given by admin
        //            //transaction.PaidAmount += Discount;
        //            if (Discount > 0)
        //            {
        //                disPayment = Discount;
        //                transaction.Discount += Discount;
        //                Discount = 0;
        //            }
        //            //we have to add this dispayment cause if Discount is exist then paidAmount will add this so if we use this  //paid amount then it will not show the actual amount its added discount amount. so we have to sub dispayment amount.
        //            transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);// + disPayment;
        //            //discount amount done

        //            //if still paid amount is not same as payment amount then we have to use given payAmount
        //            if (transaction.DueAmount <= PaymentAmount)
        //            {
        //                nmlPayment = (int)transaction.DueAmount;
        //                PaymentAmount -= (int)transaction.DueAmount;
        //                transaction.PaidAmount += (int)transaction.DueAmount;
        //                transaction.DueAmount = 0;
        //                transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //            }
        //            else
        //            {
        //                nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
        //                transaction.PaidAmount += PaymentAmount;
        //                transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
        //                transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                PaymentAmount = 0;
        //            }
        //        }

        //        transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
        //        transaction.BillCollectBy = int.Parse(Session["LoggedUserID"].ToString());

        //        db.Entry(transaction).State = EntityState.Modified;
        //        db.SaveChanges();

        //        ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //        if (clientDueBills != null)
        //        {
        //            //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
        //            //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
        //            var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //                       .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //            clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //            db.Entry(clientDueBills).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //        UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, transaction, PaymentFrom);

        //    }
        //    if (PaymentAmount > 0)// if payment amount is > 0 mean we have to add this money in advance payment.
        //    {
        //        AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

        //        if (advancePayment != null)
        //        {
        //            advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //            advancePayment.AdvanceAmount += PaymentAmount;
        //            advancePayment.Remarks = "Payment Remarks";
        //            db.Entry(advancePayment).State = EntityState.Modified;
        //            db.SaveChanges();
        //            UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), advancePayment, PaymentAmount, PaymentFrom);
        //        }
        //        else
        //        {
        //            AdvancePayment insertAdvancePayment = new AdvancePayment();
        //            insertAdvancePayment.ClientDetailsID = ClientDetailsID;
        //            insertAdvancePayment.AdvanceAmount = PaymentAmount;
        //            insertAdvancePayment.Remarks = "Payment Remarks";
        //            insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //            db.AdvancePayment.Add(insertAdvancePayment);
        //            db.SaveChanges();
        //            UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), insertAdvancePayment, PaymentAmount, PaymentFrom);
        //        }
        //    }
        //    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //[ValidateJsonAntiForgeryTokenAttribute]
        //public ActionResult PayBillByEmployeeOrAdminSpecificMonth(Transaction Transaction)
        //{
        //    try
        //    {
        //        int ClientDetailsID = Transaction.ClientDetailsID;
        //        int PaymentAmount = Transaction.PaidAmount == null ? 0 : (int)Transaction.PaidAmount;
        //        int Discount = Transaction.Discount == null ? 0 : (int)Transaction.Discount;
        //        string ResetNo = Transaction.ResetNo;
        //        string RemarksNo = Transaction.RemarksNo;
        //        int PaymentFrom = Transaction.PaymentBy;

        //        //Discount = 0;
        //        //if (Discount > 0)
        //        //{
        //        //    return Json(new { Success = false, DiscountGreaterThenZero = true });
        //        //}

        //        if (string.IsNullOrEmpty(Transaction.ResetNo))
        //        {
        //            Transaction.ResetNo = "AutoRST" + SerialNo();
        //        }
        //        else
        //        {

        //            int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //            if (ResetNo != null)
        //            {
        //                int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //                if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //                {
        //                    return Json(new { Success = false, ResetNoAlreadyExist = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage });
        //                }
        //            }
        //        }

        //        List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.TransactionID == Transaction.TransactionID && s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //        foreach (var transaction in lstUnpaidTransaction)
        //        {
        //            int disPayment = 0;
        //            int nmlPayment = 0;
        //            transaction.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;
        //            transaction.PaidAmount = transaction.PaidAmount == null ? 0 : transaction.PaidAmount.Value;
        //            transaction.DueAmount = transaction.DueAmount == null ? 0 : transaction.DueAmount.Value;
        //            transaction.Discount = transaction.Discount == null ? 0 : transaction.Discount.Value;
        //            //var howMuchMoneyNeedIfByDiscount = 
        //            if (transaction.DueAmount <= Discount)// mean we can directly pay the bill by discount amount given by admin
        //            {
        //                disPayment = (int)transaction.DueAmount;
        //                //transaction.PaidAmount = transaction.PaymentAmount;
        //                transaction.Discount += transaction.DueAmount;
        //                Discount -= (int)transaction.DueAmount;
        //                transaction.DueAmount = 0;
        //                transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //            }
        //            else//meaning we have pay the amount from payment amount given by client. but if given any discount first we have to use that discount amount first.
        //            {
        //                //cause for if discount amount given by admin
        //                //transaction.PaidAmount += Discount;
        //                if (Discount > 0)
        //                {
        //                    disPayment = Discount;
        //                    transaction.Discount += Discount;
        //                    Discount = 0;
        //                }
        //                //we have to add this dispayment cause if Discount is exist then paidAmount will add this so if we use this  //paid amount then it will not show the actual amount its added discount amount. so we have to sub dispayment amount.
        //                ////comment in 03122020 
        //                ////transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);// + disPayment;
        //                //discount amount done

        //                //if still paid amount is not same as payment amount then we have to use given payAmount
        //                if (transaction.DueAmount <= PaymentAmount)
        //                {
        //                    nmlPayment = (int)transaction.DueAmount;
        //                    PaymentAmount -= (int)transaction.DueAmount;
        //                    transaction.PaidAmount += (int)transaction.DueAmount;
        //                    transaction.DueAmount = 0;
        //                    transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //                }
        //                else
        //                {
        //                    nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
        //                    transaction.PaidAmount += PaymentAmount;
        //                    transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
        //                    transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                    PaymentAmount = 0;
        //                }
        //            }

        //            transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
        //            transaction.BillCollectBy = Transaction.BillCollectBy;// int.Parse(Session["LoggedUserID"].ToString());

        //            db.Entry(transaction).State = EntityState.Modified;
        //            db.SaveChanges();


        //            // Client Due Bills Update //
        //            //if (transaction.PaymentYear != AppUtils.RunningYear && transaction.PaymentMonth != AppUtils.RunningMonth)
        //            ////Logic: this month bill was not added in due bills list
        //            //{
        //            //    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //            //    if (clientDueBills != null)
        //            //    { 
        //            //        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //            //                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //            //        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //            //        db.Entry(clientDueBills).State = EntityState.Modified;
        //            //        db.SaveChanges();
        //            //    }
        //            //}

        //            // Logic:  payment year is same then we have to check the month to make sure its old bill or not. if payment year is not same then it must old bills.
        //            if (transaction.PaymentYear == AppUtils.RunningYear)
        //            {
        //                if (transaction.PaymentMonth != AppUtils.RunningMonth)
        //                {


        //                    //List<Transaction> totalUnpaidTransactions1 = db.Transaction.Where(x => !(x.PaymentYear == AppUtils.RunningYear && x.PaymentMonth == AppUtils.RunningMonth) && x.ClientDetailsID == transaction.ClientDetailsID && x.PaymentStatus == AppUtils.PaymentIsNotPaid && x.PaymentTypeID == AppUtils.RunningMonthBillIndicator).ToList();

        //                    var totalUnpaidTransactions = db.Transaction.Where(x => !(x.PaymentYear == AppUtils.RunningYear && x.PaymentMonth == AppUtils.RunningMonth) && x.ClientDetailsID == transaction.ClientDetailsID && x.PaymentStatus == AppUtils.PaymentIsNotPaid && x.PaymentTypeID == AppUtils.RunningMonthBillIndicator).GroupBy(x => x.ClientDetails).Select(x => new { DueAmount = x.Sum(w => w.DueAmount) }).FirstOrDefault(); ;
        //                    ClientDueBills clientDueBills = db.ClientDueBills.Where(x => x.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //                    if (clientDueBills != null)
        //                    {
        //                        clientDueBills.DueAmount = totalUnpaidTransactions.DueAmount == null ? 0 : totalUnpaidTransactions.DueAmount.Value;
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //                if (clientDueBills != null)
        //                {
        //                    var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //                               .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //                    clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //                    db.Entry(clientDueBills).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //            }
        //            // End CLient Due Bills //

        //            UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, transaction, PaymentFrom);

        //        }
        //        if (PaymentAmount > 0)// if payment amount is > 0 mean we have to add this money in advance payment.
        //        {
        //            AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

        //            if (advancePayment != null)
        //            {
        //                advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //                advancePayment.AdvanceAmount += PaymentAmount;
        //                advancePayment.Remarks = "Payment Remarks";
        //                db.Entry(advancePayment).State = EntityState.Modified;
        //                db.SaveChanges();
        //                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, advancePayment, PaymentAmount, PaymentFrom);
        //            }
        //            else
        //            {
        //                AdvancePayment insertAdvancePayment = new AdvancePayment();
        //                insertAdvancePayment.ClientDetailsID = ClientDetailsID;
        //                insertAdvancePayment.AdvanceAmount = PaymentAmount;
        //                insertAdvancePayment.Remarks = "Payment Remarks";
        //                insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //                db.AdvancePayment.Add(insertAdvancePayment);
        //                db.SaveChanges();
        //                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, insertAdvancePayment, PaymentAmount, PaymentFrom);
        //            }
        //        }
        //        return Json(new { Success = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PayBillByBillMan(int ClientDetailsID, int PaymentAmount, int Discount, string ResetNo, string RemarksNo, int PaymentFrom)
        //{
        //    Discount = 0;
        //    if (Discount > 0)
        //    {
        //        return Json(new { Success = false, DiscountGreaterThenZero = true });
        //    }


        //    int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //    {
        //        return Json(new { Success = false, ResetNoAlreadyExist = true });
        //    }

        //    List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //    foreach (var transaction in lstUnpaidTransaction)
        //    {
        //        int disPayment = 0;
        //        int nmlPayment = 0;

        //        transaction.PaidAmount = transaction.PaidAmount == null ? 0 : transaction.PaidAmount.Value;
        //        transaction.DueAmount = transaction.DueAmount == null ? 0 : transaction.DueAmount.Value;
        //        transaction.Discount = transaction.Discount == null ? 0 : transaction.Discount.Value;
        //        //var howMuchMoneyNeedIfByDiscount = 
        //        if (transaction.DueAmount <= Discount)// mean we can directly pay the bill by discount amount given by admin
        //        {
        //            disPayment = (int)transaction.DueAmount;
        //            //transaction.PaidAmount = transaction.PaymentAmount;
        //            transaction.Discount += transaction.DueAmount;
        //            Discount -= (int)transaction.DueAmount;
        //            transaction.DueAmount = 0;
        //            transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //        }
        //        else//meaning we have pay the amount from payment amount given by client. but if given any discount first we have to use that discount amount first.
        //        {
        //            //cause for if discount amount given by admin
        //            //transaction.PaidAmount += Discount;
        //            if (Discount > 0)
        //            {
        //                disPayment = Discount;
        //                transaction.Discount += Discount;
        //                Discount = 0;
        //            }
        //            //we have to add this dispayment cause if Discount is exist then paidAmount will add this so if we use this  //paid amount then it will not show the actual amount its added discount amount. so we have to sub dispayment amount.
        //            transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);// + disPayment;
        //            //discount amount done

        //            //if still paid amount is not same as payment amount then we have to use given payAmount
        //            if (transaction.DueAmount <= PaymentAmount)
        //            {
        //                nmlPayment = (int)transaction.DueAmount;
        //                PaymentAmount -= (int)transaction.DueAmount;
        //                transaction.PaidAmount += (int)transaction.DueAmount;
        //                transaction.DueAmount = 0;
        //                transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //            }
        //            else
        //            {
        //                nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
        //                transaction.PaidAmount += PaymentAmount;
        //                transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
        //                transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                PaymentAmount = 0;
        //            }
        //        }

        //        transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
        //        transaction.BillCollectBy = int.Parse(Session["LoggedUserID"].ToString());

        //        db.Entry(transaction).State = EntityState.Modified;
        //        db.SaveChanges();

        //        ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //        if (clientDueBills != null)
        //        {
        //            //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
        //            //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
        //            var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //                       .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //            clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //            db.Entry(clientDueBills).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //        UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, transaction, PaymentFrom);

        //    }
        //    if (PaymentAmount > 0)// if payment amount is > 0 mean we have to add this money in advance payment.
        //    {
        //        AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

        //        if (advancePayment != null)
        //        {
        //            advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //            advancePayment.AdvanceAmount += PaymentAmount;
        //            advancePayment.Remarks = "Payment Remarks";
        //            db.Entry(advancePayment).State = EntityState.Modified;
        //            db.SaveChanges();
        //            UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), advancePayment, PaymentAmount, PaymentFrom);
        //        }
        //        else
        //        {
        //            AdvancePayment insertAdvancePayment = new AdvancePayment();
        //            insertAdvancePayment.ClientDetailsID = ClientDetailsID;
        //            insertAdvancePayment.AdvanceAmount = PaymentAmount;
        //            insertAdvancePayment.Remarks = "Payment Remarks";
        //            insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //            db.AdvancePayment.Add(insertAdvancePayment);
        //            db.SaveChanges();
        //            UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), insertAdvancePayment, PaymentAmount, PaymentFrom);
        //        }
        //    }
        //    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        //    //int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    //int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //    //if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //    //{
        //    //    return Json(new { Success = false, ResetNoAlreadyExist = true });
        //    //}
        //    //if (Discount != 0)
        //    //{
        //    //    return Json(new { Success = false, DiscounGreterThanZero = true });
        //    //}
        //    //List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //    //foreach (var transaction in lstUnpaidTransaction)
        //    //{
        //    //    int disPayment = 0;
        //    //    int nmlPayment = 0;

        //    //    transaction.PaidAmount = transaction.PaidAmount == null ? 0 : transaction.PaidAmount.Value;
        //    //    transaction.DueAmount = transaction.DueAmount == null ? 0 : transaction.DueAmount.Value;
        //    //    transaction.Discount = transaction.Discount == null ? 0 : transaction.Discount.Value;
        //    //    //var howMuchMoneyNeedIfByDiscount = 
        //    //    if (transaction.DueAmount <= Discount)// mean we can directly pay the bill by discount amount given by admin
        //    //    {
        //    //        disPayment = (int)transaction.DueAmount;
        //    //        //transaction.PaidAmount = transaction.PaymentAmount;
        //    //        transaction.Discount += transaction.DueAmount;
        //    //        Discount -= (int)transaction.DueAmount;
        //    //        transaction.DueAmount = 0;
        //    //        transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //    //    }
        //    //    else//meaning we have pay the amount from payment amount given by client. but if given any discount first we have to use that discount amount first.
        //    //    {
        //    //        //cause for if discount amount given by admin
        //    //        //transaction.PaidAmount += Discount;
        //    //        if (Discount > 0)
        //    //        {
        //    //            disPayment = Discount;
        //    //            transaction.Discount += Discount;
        //    //            Discount = 0;
        //    //        }
        //    //        //we have to add this dispayment cause if Discount is exist then paidAmount will add this so if we use this  //paid amount then it will not show the actual amount its added discount amount. so we have to sub dispayment amount.
        //    //        transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);// + disPayment;
        //    //        //discount amount done

        //    //        //if still paid amount is not same as payment amount then we have to use given payAmount
        //    //        if (transaction.DueAmount <= PaymentAmount)
        //    //        {
        //    //            nmlPayment = (int)transaction.DueAmount;
        //    //            PaymentAmount -= (int)transaction.DueAmount;
        //    //            transaction.PaidAmount += (int)transaction.DueAmount;
        //    //            transaction.DueAmount = 0;
        //    //            transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //    //        }
        //    //        else
        //    //        {
        //    //            nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
        //    //            transaction.PaidAmount += PaymentAmount;
        //    //            transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
        //    //            transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //    //            PaymentAmount = 0;
        //    //        }
        //    //    }

        //    //    transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
        //    //    transaction.BillCollectBy = int.Parse(Session["LoggedUserID"].ToString());

        //    //    db.Entry(transaction).State = EntityState.Modified;
        //    //    db.SaveChanges();

        //    //    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //    //    if (clientDueBills != null)
        //    //    {
        //    //        //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
        //    //        //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
        //    //        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //    //                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //    //        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //    //        db.Entry(clientDueBills).State = EntityState.Modified;
        //    //        db.SaveChanges();
        //    //    }

        //    //    UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, transaction);

        //    //}
        //    //if (PaymentAmount > 0)// if payment amount is > 0 mean we have to add this money in advance payment.
        //    //{
        //    //    AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

        //    //    if (advancePayment != null)
        //    //    {
        //    //        advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //    //        advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //    //        advancePayment.AdvanceAmount += PaymentAmount;
        //    //        advancePayment.Remarks = "Payment Remarks";
        //    //        db.Entry(advancePayment).State = EntityState.Modified;
        //    //        db.SaveChanges();
        //    //        UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), advancePayment, PaymentAmount);
        //    //    }
        //    //    else
        //    //    {
        //    //        AdvancePayment insertAdvancePayment = new AdvancePayment();
        //    //        insertAdvancePayment.ClientDetailsID = ClientDetailsID;
        //    //        insertAdvancePayment.AdvanceAmount = PaymentAmount;
        //    //        insertAdvancePayment.Remarks = "Payment Remarks";
        //    //        insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //    //        insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //    //        db.AdvancePayment.Add(insertAdvancePayment);
        //    //        db.SaveChanges();
        //    //        UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, new Transaction(), insertAdvancePayment, PaymentAmount);
        //    //    }
        //    //}
        //    //return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryToken] 
        //[ValidateJsonAntiForgeryTokenAttribute]
        //public ActionResult PayBillByBillManSpecificMonth(Transaction Transaction)
        //{
        //    try
        //    {
        //        int ClientDetailsID = Transaction.ClientDetailsID;
        //        int PaymentAmount = Transaction.PaidAmount == null ? 0 : (int)Transaction.PaidAmount;
        //        int Discount = Transaction.Discount == null ? 0 : (int)Transaction.Discount;
        //        string ResetNo = Transaction.ResetNo;
        //        string RemarksNo = Transaction.RemarksNo;
        //        int PaymentFrom = Transaction.PaymentBy;
        //        Transaction.BillCollectBy = AppUtils.GetLoginUserID();

        //        //Discount = 0;
        //        //if (Discount > 0)
        //        //{
        //        //    return Json(new { Success = false, DiscountGreaterThenZero = true });
        //        //}


        //        if (string.IsNullOrEmpty(Transaction.ResetNo))
        //        {
        //            Transaction.ResetNo = "AutoRST" + SerialNo();
        //        }
        //        else
        //        {
        //            int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //            if (ResetNo != null)
        //            {
        //                int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //                if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //                {
        //                    return Json(new { Success = false, ResetNoAlreadyExist = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage });
        //                }
        //            }
        //        }

        //        //int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //        //int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
        //        //if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //        //{
        //        //    return Json(new { Success = false, ResetNoAlreadyExist = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage });
        //        //}

        //        List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.TransactionID == Transaction.TransactionID && s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //        foreach (var transaction in lstUnpaidTransaction)
        //        {
        //            int disPayment = 0;
        //            int nmlPayment = 0;

        //            transaction.PaidAmount = transaction.PaidAmount == null ? 0 : transaction.PaidAmount.Value;
        //            transaction.DueAmount = transaction.DueAmount == null ? 0 : transaction.DueAmount.Value;
        //            transaction.Discount = transaction.Discount == null ? 0 : transaction.Discount.Value;
        //            //var howMuchMoneyNeedIfByDiscount = 
        //            if (transaction.DueAmount <= Discount)// mean we can directly pay the bill by discount amount given by admin
        //            {
        //                disPayment = (int)transaction.DueAmount;
        //                //transaction.PaidAmount = transaction.PaymentAmount;
        //                transaction.Discount += transaction.DueAmount;
        //                Discount -= (int)transaction.DueAmount;
        //                transaction.DueAmount = 0;
        //                transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //            }
        //            else//meaning we have pay the amount from payment amount given by client. but if given any discount first we have to use that discount amount first.
        //            {
        //                //cause for if discount amount given by admin
        //                //transaction.PaidAmount += Discount;
        //                if (Discount > 0)
        //                {
        //                    disPayment = Discount;
        //                    transaction.Discount += Discount;
        //                    Discount = 0;
        //                }
        //                //we have to add this dispayment cause if Discount is exist then paidAmount will add this so if we use this  //paid amount then it will not show the actual amount its added discount amount. so we have to sub dispayment amount.
        //                ////comment in 03122020 
        //                ////transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);// + disPayment;
        //                //discount amount done

        //                //if still paid amount is not same as payment amount then we have to use given payAmount
        //                if (transaction.DueAmount <= PaymentAmount)
        //                {
        //                    nmlPayment = (int)transaction.DueAmount;
        //                    PaymentAmount -= (int)transaction.DueAmount;
        //                    transaction.PaidAmount += (int)transaction.DueAmount;
        //                    transaction.DueAmount = 0;
        //                    transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //                }
        //                else
        //                {
        //                    nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
        //                    transaction.PaidAmount += PaymentAmount;
        //                    transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
        //                    transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                    PaymentAmount = 0;
        //                }
        //            }

        //            transaction.EmployeeID = int.Parse(Session["LoggedUserID"].ToString());
        //            transaction.BillCollectBy = Transaction.BillCollectBy;// int.Parse(Session["LoggedUserID"].ToString());
        //            transaction.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;// int.Parse(Session["LoggedUserID"].ToString());

        //            db.Entry(transaction).State = EntityState.Modified;
        //            db.SaveChanges();

        //            //if (transaction.PaymentYear != AppUtils.RunningYear && transaction.PaymentMonth != AppUtils.RunningMonth)
        //            //Logic: this month bill was not added in due bills list
        //            //{
        //            //    ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //            //    if (clientDueBills != null)
        //            //    {
        //            //        //clientDueBills.DueAmount -= (Transaction.PaidAmount.Value + Transaction.Discount.Value);
        //            //        //clientDueBills.DueAmount = clientDueBills.DueAmount < 0 ? 0 : clientDueBills.DueAmount;
        //            //        var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //            //                   .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //            //        clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //            //        db.Entry(clientDueBills).State = EntityState.Modified;
        //            //        db.SaveChanges();
        //            //    }
        //            //}

        //            // Logic:  payment year is same then we have to check the month to make sure its old bill or not. if payment year is not same then it must old bills.
        //            if (transaction.PaymentYear == AppUtils.RunningYear)
        //            {
        //                if (transaction.PaymentMonth != AppUtils.RunningMonth)
        //                {


        //                    //List<Transaction> totalUnpaidTransactions1 = db.Transaction.Where(x => !(x.PaymentYear == AppUtils.RunningYear && x.PaymentMonth == AppUtils.RunningMonth) && x.ClientDetailsID == transaction.ClientDetailsID && x.PaymentStatus == AppUtils.PaymentIsNotPaid && x.PaymentTypeID == AppUtils.RunningMonthBillIndicator).ToList();

        //                    var totalUnpaidTransactions = db.Transaction.Where(x => !(x.PaymentYear == AppUtils.RunningYear && x.PaymentMonth == AppUtils.RunningMonth) && x.ClientDetailsID == transaction.ClientDetailsID && x.PaymentStatus == AppUtils.PaymentIsNotPaid && x.PaymentTypeID == AppUtils.RunningMonthBillIndicator).GroupBy(x => x.ClientDetails).Select(x => new { DueAmount = x.Sum(w => w.DueAmount) }).FirstOrDefault(); ;
        //                    ClientDueBills clientDueBills = db.ClientDueBills.Where(x => x.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //                    if (clientDueBills != null)
        //                    {
        //                        clientDueBills.DueAmount = totalUnpaidTransactions.DueAmount == null ? 0 : totalUnpaidTransactions.DueAmount.Value;
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
        //                if (clientDueBills != null)
        //                {
        //                    var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
        //                               .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
        //                    clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
        //                    db.Entry(clientDueBills).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //            }
        //            // End CLient Due Bills //

        //            UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, transaction, PaymentFrom);

        //        }
        //        if (PaymentAmount > 0)// if payment amount is > 0 mean we have to add this money in advance payment.
        //        {
        //            AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

        //            if (advancePayment != null)
        //            {
        //                advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //                advancePayment.AdvanceAmount += PaymentAmount;
        //                advancePayment.Remarks = "Payment Remarks";
        //                db.Entry(advancePayment).State = EntityState.Modified;
        //                db.SaveChanges();
        //                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, advancePayment, PaymentAmount, PaymentFrom);
        //            }
        //            else
        //            {
        //                AdvancePayment insertAdvancePayment = new AdvancePayment();
        //                insertAdvancePayment.ClientDetailsID = ClientDetailsID;
        //                insertAdvancePayment.AdvanceAmount = PaymentAmount;
        //                insertAdvancePayment.Remarks = "Payment Remarks";
        //                insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //                insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //                db.AdvancePayment.Add(insertAdvancePayment);
        //                db.SaveChanges();
        //                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, insertAdvancePayment, PaymentAmount, PaymentFrom);
        //            }
        //        }
        //        return Json(new { Success = true, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message, PaymentFromWhichPage = Transaction.PaymentFromWhichPage }, JsonRequestBehavior.AllowGet);
        //    }

        //    ////float remainingAmount = 0;
        //    ////int countForResetIsGeneraetOrNot = (Transaction.ResetNo != null) ? db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
        //    ////int countForResetIsGenerateOrNotFromPaymentHistory = (Transaction.ResetNo != null) ? db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == Transaction.ResetNo.Trim().ToLower()).Count() : 0;
        //    ////if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
        //    ////{
        //    ////    return Json(new { Success = false, ResetNoAlreadyExist = true });
        //    ////}

        //    ////if (string.IsNullOrEmpty(Transaction.ResetNo))
        //    ////{
        //    ////    Transaction.ResetNo = "AutoRST" + SerialNo();
        //    ////}


        //    ////Transaction ts = db.Transaction.Find(Transaction.TransactionID);
        //    ////List<Transaction> lstDueTransactions = db.Transaction.Where(s => s.PaymentYear != AppUtils.RunningYear && s.PaymentMonth != AppUtils.RunningMonth && s.ClientDetailsID == ts.ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
        //    ////if (ts.TransactionID > 0)
        //    ////{
        //    ////    //ts.PaymentAmount = ts.PaymentAmount;
        //    ////    //ts.EmployeeID = AppUtils.LoginUserID;
        //    ////    //ts.BillCollectBy = Transaction.BillCollectBy;
        //    ////    //ts.Discount = Transaction.Discount;
        //    ////    //ts.PaymentStatus = AppUtils.PaymentIsPaid;// paid
        //    ////    //ts.PaymentDate = AppUtils.GetDateTimeNow();
        //    ////    //ts.RemarksNo = Transaction.RemarksNo;
        //    ////    //ts.ResetNo = Transaction.ResetNo;
        //    ////    //ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;

        //    ////    if (Transaction.PaidAmount + Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount) > ts.PaymentAmount)
        //    ////    {
        //    ////        var a = (Transaction.Discount + (ts.PaidAmount == null ? 0 : ts.PaidAmount));
        //    ////        remainingAmount = Transaction.PaidAmount.Value - (ts.PaymentAmount.Value - a.Value);
        //    ////        Transaction.PaidAmount = ts.PaymentAmount - a;
        //    ////    }
        //    ////    //ts.PaymentAmount = ts.PaymentAmount;
        //    ////    ts.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
        //    ////    ts.PaidAmount = ts.PaidAmount == null ? Transaction.PaidAmount.Value : (ts.PaidAmount + Transaction.PaidAmount);
        //    ////    ts.DueAmount = ts.PaymentAmount - (ts.PaidAmount + Transaction.Discount)/*(ts.PaidAmount + Transaction.Discount)< 0 ? 0 : ts.PaymentAmount - (Transaction.PaidAmount+ts.Discount)*/;
        //    ////    ts.BillCollectBy = Transaction.BillCollectBy;
        //    ////    ts.Discount = Transaction.Discount;
        //    ////    ts.PaymentStatus = (ts.PaymentAmount - (ts.PaidAmount + ts.Discount)) < 1 ? AppUtils.PaymentIsPaid : AppUtils.PaymentIsNotPaid; //AppUtils.PaymentIsPaid;// paid
        //    ////    ts.PaymentDate = AppUtils.GetDateTimeNow();
        //    ////    ts.RemarksNo = Transaction.RemarksNo;
        //    ////    ts.ResetNo = Transaction.ResetNo;
        //    ////    ts.PaymentFromWhichPage = Transaction.PaymentFromWhichPage;



        //    ////    try
        //    ////    {
        //    ////        db.Entry(ts).State = EntityState.Modified;
        //    ////        db.SaveChanges();

        //    ////        UpdatePaymentIntoPaymentHistoryTable(Transaction, ts); 
        //    ////        if (remainingAmount > 0 && lstDueTransactions.Count > 0)
        //    ////        {
        //    ////            foreach (var dueTransaction in lstDueTransactions)
        //    ////            {
        //    ////                if (remainingAmount >= 0)
        //    ////                {
        //    ////                    double paymentAmount = dueTransaction.PaymentAmount.Value;
        //    ////                    double paidAmount = dueTransaction.PaidAmount != null ? dueTransaction.PaidAmount.Value : 0;
        //    ////                    //double dueAmount = dueTransaction.DueAmount != null ? dueTransaction.DueAmount.Value : paymentAmount;
        //    ////                    double requireAmount = paymentAmount - paidAmount;

        //    ////                    if (requireAmount < remainingAmount)//meaning full payment possible cause require is less then remaining amount.
        //    ////                    {

        //    ////                        dueTransaction.PaidAmount += (float?)(remainingAmount - requireAmount);
        //    ////                        dueTransaction.DueAmount = 0;
        //    ////                        dueTransaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //    ////                        dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
        //    ////                        dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
        //    ////                        dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

        //    ////                        remainingAmount -= (float)requireAmount;
        //    ////                    }
        //    ////                    else
        //    ////                    {
        //    ////                        dueTransaction.PaidAmount += (float?)(requireAmount - remainingAmount);
        //    ////                        dueTransaction.DueAmount = (float?)(paymentAmount - paidAmount);
        //    ////                        dueTransaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //    ////                        dueTransaction.PaymentDate = AppUtils.GetDateTimeNow();
        //    ////                        dueTransaction.ResetNo = Transaction.ResetNo == null ? "AutoRST" + SerialNo() : Transaction.ResetNo;
        //    ////                        dueTransaction.RemarksNo = Transaction.RemarksNo == null ? "AutoREM" + RemarksNo() : Transaction.RemarksNo;

        //    ////                        remainingAmount -= (float)requireAmount;
        //    ////                    }
        //    ////                    db.Entry(dueTransaction).State = EntityState.Modified;
        //    ////                    db.SaveChanges();

        //    ////                    UpdatePaymentIntoPaymentHistoryTable(Transaction, dueTransaction);
        //    ////                }

        //    ////            }
        //    ////        }
        //    ////        if (remainingAmount > 0 /*&& lstDueTransactions.Count == 0*/)
        //    ////        {
        //    ////            AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ts.ClientDetailsID).FirstOrDefault();


        //    ////            if (advancePayment != null)
        //    ////            {
        //    ////                advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
        //    ////                advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //    ////                advancePayment.AdvanceAmount += remainingAmount;
        //    ////                advancePayment.Remarks = "Payment Remarks";
        //    ////                db.Entry(advancePayment).State = EntityState.Modified;
        //    ////                db.SaveChanges();
        //    ////                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(Transaction, ts, advancePayment, remainingAmount);
        //    ////            }
        //    ////            else
        //    ////            {
        //    ////                AdvancePayment insertAdvancePayment = new AdvancePayment();
        //    ////                insertAdvancePayment.ClientDetailsID = ts.ClientDetailsID;
        //    ////                insertAdvancePayment.AdvanceAmount = remainingAmount;
        //    ////                insertAdvancePayment.Remarks = "Payment Remarks";
        //    ////                insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
        //    ////                insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //    ////                db.AdvancePayment.Add(insertAdvancePayment);
        //    ////                db.SaveChanges();
        //    ////                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(Transaction, ts, insertAdvancePayment, remainingAmount);
        //    ////            }


        //    ////        }
        //    ////        //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //    ////        if ((bool)Session["SMSOptionEnable"])
        //    ////        {
        //    ////            try
        //    ////            {
        //    ////                SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
        //    ////                if (smsSenderIdPass != null)
        //    ////                {
        //    ////                    SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.Bill_Pay_Code && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
        //    ////                    if (sms != null)
        //    ////                    {
        //    ////                        var message = sms.SendMessageText;
        //    ////                        message = message.Replace("[NAME]", ts.ClientDetails.Name); message = message.Replace("[AMOUNT]", Math.Round(ts.PaymentAmount.Value, 2).ToString());
        //    ////                        message = message.Replace("[DISCOUNT]", Transaction.Discount.ToString()); message = message.Replace("[RECEIPT-NO]", Transaction.ResetNo);
        //    ////                        message = message.Replace("[PAID-BY]", AppUtils.GetLoginEmployeeName()); message = message.Replace("[PAID-TIME]", AppUtils.GetDateTimeNow().ToString());

        //    ////                        string smsMobileNo = "";
        //    ////                        if (!string.IsNullOrEmpty(Transaction.AnotherMobileNo))
        //    ////                        {
        //    ////                            smsMobileNo = Transaction.AnotherMobileNo;
        //    ////                        }
        //    ////                        else
        //    ////                        {
        //    ////                            smsMobileNo = ts.ClientDetails.ContactNumber;
        //    ////                        }

        //    ////                        SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, smsMobileNo, message);
        //    ////                        if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
        //    ////                        {
        //    ////                            sms.SMSCounter += 1;
        //    ////                            db.Entry(sms).State = EntityState.Modified;
        //    ////                            db.SaveChanges();
        //    ////                        }
        //    ////                    }
        //    ////                }
        //    ////            }
        //    ////            catch (Exception ex)
        //    ////            {
        //    ////                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //    ////            }

        //    ////        }


        //    ////        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        //    ////    }
        //    ////    catch
        //    ////    {
        //    ////        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //    ////    }
        //    ////}

        //    ////return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetRemarksAndSleepNoForPayment(int TransactionID)
        //{
        //    try
        //    {
        //        //db.Configuration.ProxyCreationEnabled  = true;
        //        var transaction = db.Transaction.Where(s => s.TransactionID == TransactionID).Select(
        //        s =>
        //        new
        //        {
        //            ClientDetailsID = s.ClientDetails.ClientDetailsID
        //        ,
        //            LoginName = s.ClientDetails.LoginName
        //        ,
        //            UserID = s.ClientDetails.ClientDetailsID
        //        ,
        //            PackagePrice = s.PaymentAmount
        //        ,
        //            PackageName = s.Package.PackageName
        //        ,
        //            PaymentAmount = s.PaymentAmount
        //        ,
        //            PaidAmount = s.PaidAmount == null ? 0 : s.PaidAmount
        //        ,
        //            DueAmount = s.DueAmount == null ? s.PaymentAmount - 0 : s.DueAmount
        //        ,
        //            SerialNo = s.ResetNo
        //        ,
        //            DiscountAmount = s.Discount == null ? 0 : s.Discount
        //        ,
        //            PermanentDiscount = s.PermanentDiscount
        //        ,
        //            MonthName = s.PaymentMonth//GetPaymentMonthName(s.PaymentMonth)
        //            // ,Package = { PackageID = s.Package.PackageID,PackageName = s.Package.PackageName}
        //        }).FirstOrDefault();

        //        if (transaction != null)
        //        {
        //            Remarks remarks = db.Remarks.Find(1);
        //            remarks.RemarksNo += 1;
        //            db.Entry(remarks).State = EntityState.Modified;
        //            db.SaveChanges();

        //            return Json(new { Transaction = transaction, RemarksNo = remarks.RemarksNo, Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Transaction = "", RemarksNo = "", SerialNo = "", Success = false });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var a = ex.Data;
        //        return Json(new { Transaction = "", RemarksNo = "", SerialNo = "", Success = false, data = ex.Message });
        //    }


        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetRemarksAndSleepNoForPaymentForLineMan(int TransactionID)
        //{
        //    //db.Configuration.ProxyCreationEnabled  = true;
        //    var transaction = db.Transaction.Where(s => s.TransactionID == TransactionID).Select(
        //        s =>
        //        new
        //        {
        //            ClientDetailsID = s.ClientDetails.ClientDetailsID
        //        ,
        //            LoginName = s.ClientDetails.LoginName
        //        ,
        //            UserID = s.ClientDetails.ClientDetailsID
        //        ,
        //            PackagePrice = s.PaymentAmount
        //        ,
        //            PackageName = s.Package.PackageName
        //        ,
        //            PaymentAmount = s.PaymentAmount
        //        ,
        //            PaidAmount = s.PaidAmount == null ? 0 : (s.PaidAmount /*+ (s.Discount == null ? 0 : s.Discount)*/)
        //        ,
        //            DueAmount = s.DueAmount == null ? (s.PaymentAmount - 0) : (s.DueAmount /*- (s.Discount == null ? 0 : s.Discount)*/)
        //        ,
        //            SerialNo = s.ResetNo
        //        ,
        //            DiscountAmount = s.Discount == null ? 0 : s.Discount
        //        ,
        //            PermanentDiscount = s.PermanentDiscount
        //        ,
        //            MonthName = s.PaymentMonth
        //            // ,Package = { PackageID = s.Package.PackageID,PackageName = s.Package.PackageName}
        //        }).FirstOrDefault();
        //    try
        //    {
        //        if (transaction != null)
        //        {
        //            Remarks remarks = db.Remarks.Find(1);
        //            remarks.RemarksNo += 1;
        //            db.Entry(remarks).State = EntityState.Modified;
        //            db.SaveChanges();

        //            return Json(new { Transaction = transaction, RemarksNo = remarks.RemarksNo, Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Transaction = "", RemarksNo = "", SerialNo = "", Success = false });
        //        } 
        //    }
        //    catch
        //    {
        //        return Json(new { Transaction = "", RemarksNo = "", SerialNo = "", Success = false });
        //    } 
        //}

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult PayBillByAdminOrEmployeeOrReseller(Transaction Transaction)
        {
            int loginRoleID = AppUtils.GetLoginRoleID();
            int loginUserID = AppUtils.GetLoginUserID();
            try
            {

                int ClientDetailsID = Transaction.ClientDetailsID == 0 ? db.Transaction.Find(Transaction.TransactionID).ClientDetailsID : Transaction.ClientDetailsID;
                int PaymentAmount = Transaction.PaidAmount == null ? 0 : (int)Transaction.PaidAmount;
                string ResetNo = Transaction.ResetNo;
                string RemarksNo = Transaction.RemarksNo;
                int PaymentFrom = Transaction.PaymentBy;
                int Discount = isAllowDiscount(loginRoleID, loginUserID) ? Transaction.Discount == null ? 0 : (int)Transaction.Discount : 0;
                Transaction.BillCollectBy = GetBillCollectBy(Transaction, loginRoleID, loginUserID);// AppUtils.GetLoginUserID();

                // reset No exist Or Not
                if (string.IsNullOrEmpty(Transaction.ResetNo))
                {
                    ResetNo = Transaction.ResetNo = "AutoRST" + SerialNo();
                }
                else
                {
                    if (ResetNoexistOrNot(ResetNo))
                    {
                        return Json(new { success = false, paymentfromwhichpage = Transaction.PaymentFromWhichPage, resetnoalreadyexist = true, message = "Sorrry Reset No Already Exist." });
                    }
                }

                List<Transaction> lstUnpaidTransaction = db.Transaction.Where(s => s.TransactionID == Transaction.TransactionID && s.ClientDetailsID == ClientDetailsID && s.PaymentStatus == AppUtils.PaymentIsNotPaid).ToList();
                foreach (var dbloopUnpaidtransaction in lstUnpaidTransaction)
                {
                    int disPayment = 0;
                    int nmlPayment = 0;

                    SetTransactionData(dbloopUnpaidtransaction, Transaction);//need to check without ref this is working or not.

                    // mean we can directly pay the bill by discount amount given by admin
                    if (dbloopUnpaidtransaction.DueAmount <= Discount)
                    {
                        SetInformationWhenDiscountIsLargerThanDueAmount(ref disPayment, ref Discount, dbloopUnpaidtransaction);
                    }
                    else
                    {
                        //Adding Discount payment in transaction and DisPayment if Dueamount is less than Discount
                        SetInformationWhenDiscountIsLessThanDueAmount(ref disPayment, ref Discount, dbloopUnpaidtransaction);

                        //if paid amount > than Due amount mean we have extra payment amount than this month. if not then its mean user paid less amount then the package amount
                        if (dbloopUnpaidtransaction.DueAmount <= (PaymentAmount + disPayment))
                        {
                            SetInformationWhenPaymentAmountIsLargerThanDueAmount(ref nmlPayment, ref PaymentAmount, disPayment, dbloopUnpaidtransaction);
                        }
                        else
                        {
                            SetInformationWhenPaymentAmountIsLessThanDueAmount(ref nmlPayment, ref PaymentAmount, dbloopUnpaidtransaction);
                        }
                    }

                    db.Entry(dbloopUnpaidtransaction).State = EntityState.Modified;
                    db.SaveChanges();

                    // Logic: if payment year is not same then it must old bills.
                    ReduceOldBillsInClientDueBills(dbloopUnpaidtransaction);
                    // End CLient Due Bills //

                    UpdatePaymentIntoPaymentHistoryForNomalPayment(disPayment, nmlPayment, ResetNo, dbloopUnpaidtransaction, PaymentFrom);
                }

                // if payment amount is > 0 mean after all payment we have extra money and need to add this money in advance payment.
                if (PaymentAmount > 0 || Discount > 0)
                {
                    UpdateAdvancePayment(ClientDetailsID, ResetNo, PaymentAmount, Discount, PaymentFrom, Transaction);
                }
                return Json(new { success = true, paymentfromwhichpage = Transaction.PaymentFromWhichPage, message = "Payment Update Successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, paymentfromwhichpage = Transaction.PaymentFromWhichPage, message = "Payment Update Failed." }, JsonRequestBehavior.AllowGet);
            }


            //if (loginRoleID == AppUtils.SuperUserRole || loginRoleID == AppUtils.AdminRole)
            //{

            //}
            //else if (loginRoleID == AppUtils.SuperUserRole)
            //{

            //}
            //else if (loginRoleID == AppUtils.ResellerRole)
            //{

            //}
            //else
            //{

            //}

            //return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRemarksAndSleepNoForPaymentForAllPayment(int TransactionID)
        {
            var transaction = db.Transaction.Where(s => s.TransactionID == TransactionID).Select(
                s =>
                new
                {
                    ClientDetailsID = s.ClientDetails.ClientDetailsID,
                    LoginName = s.ClientDetails.LoginName,
                    UserID = s.ClientDetails.ClientDetailsID,
                    PackagePrice = s.PaymentAmount,
                    PackageName = s.Package.PackageName,
                    PaymentAmount = s.PaymentAmount,
                    PaidAmount = s.PaidAmount == null ? 0 : (s.PaidAmount /*+ (s.Discount == null ? 0 : s.Discount)*/),
                    DueAmount = s.DueAmount == null ? (s.PaymentAmount - 0) : (s.DueAmount /*- (s.Discount == null ? 0 : s.Discount)*/),
                    SerialNo = s.ResetNo,
                    DiscountAmount = s.Discount == null ? 0 : s.Discount,
                    PermanentDiscount = s.PermanentDiscount,
                    MonthName = s.PaymentMonth
                }).FirstOrDefault();
            try
            {
                if (transaction != null)
                {
                    Remarks remarks = db.Remarks.Find(1);
                    remarks.RemarksNo += 1;
                    db.Entry(remarks).State = EntityState.Modified;
                    db.SaveChanges();

                    int loginRoleID = AppUtils.GetLoginRoleID();
                    int loginUserID = AppUtils.GetLoginUserID();
                    bool isAllowedForDiscount = isAllowDiscount(loginRoleID, loginUserID);

                    return Json(new { transaction = transaction, OneTypePaymentOrMultiplePayment = isAllowedForDiscount, lID = loginUserID, remarksno = remarks.RemarksNo, success = true });
                }
                else
                {
                    return Json(new { transaction = "", remarksno = "", serialno = "", success = false });
                }
            }
            catch
            {
                return Json(new { transaction = "", remarksno = "", serialNo = "", success = false });
            }
        }


        private void UpdateAdvancePayment(int ClientDetailsID, string ResetNo, int PaymentAmount, int remainingDiscount, int PaymentFrom, Transaction Transaction)
        {
            AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();
            int totalAmountForAdvance = (PaymentAmount + remainingDiscount);
            if (advancePayment != null)
            {
                advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                advancePayment.AdvanceAmount += totalAmountForAdvance;
                advancePayment.Remarks = "Payment Remarks";
                db.Entry(advancePayment).State = EntityState.Modified;
                db.SaveChanges();
                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, advancePayment, totalAmountForAdvance, PaymentFrom);
            }
            else
            {
                AdvancePayment insertAdvancePayment = new AdvancePayment();
                insertAdvancePayment.ClientDetailsID = ClientDetailsID;
                insertAdvancePayment.AdvanceAmount = totalAmountForAdvance;
                insertAdvancePayment.Remarks = "Payment Remarks";
                insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                db.AdvancePayment.Add(insertAdvancePayment);
                db.SaveChanges();
                UpdatePaymentIntoPaymentHistoryTableForAdvancePayment(ResetNo, Transaction, insertAdvancePayment, totalAmountForAdvance, PaymentFrom);
            }
        }

        private void ReduceOldBillsInClientDueBills(Transaction transaction)
        {
            if (transaction.PaymentYear == AppUtils.RunningYear)
            {
                if (transaction.PaymentMonth != AppUtils.RunningMonth)
                {
                    var totalUnpaidTransactions = db.Transaction.Where(x => !(x.PaymentYear == AppUtils.RunningYear && x.PaymentMonth == AppUtils.RunningMonth) && x.ClientDetailsID == transaction.ClientDetailsID && x.PaymentStatus == AppUtils.PaymentIsNotPaid && x.PaymentTypeID == AppUtils.RunningMonthBillIndicator).GroupBy(x => x.ClientDetails).Select(x => new { DueAmount = x.Sum(w => w.DueAmount) }).FirstOrDefault(); ;
                    ClientDueBills clientDueBills = db.ClientDueBills.Where(x => x.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
                    if (clientDueBills != null)
                    {
                        clientDueBills.DueAmount = totalUnpaidTransactions == null ? 0 : totalUnpaidTransactions.DueAmount == null ? 0 : totalUnpaidTransactions.DueAmount.Value;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                ClientDueBills clientDueBills = db.ClientDueBills.Where(s => s.ClientDetailsID == transaction.ClientDetailsID).FirstOrDefault();
                if (clientDueBills != null)
                {
                    var lstDueTransaction = db.Transaction.Where(s => s.ClientDetailsID == transaction.ClientDetailsID && !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == AppUtils.RunningMonthBillIndicator/* && s.IsNewClient == AppUtils.isNotNewClient*/)
                               .GroupBy(s => s.ClientDetailsID).Select(s => new { DueAmount = s.Sum(w => w.DueAmount) }).FirstOrDefault();
                    clientDueBills.DueAmount = (double)(lstDueTransaction != null ? lstDueTransaction.DueAmount : 0);
                    db.Entry(clientDueBills).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        private void SetInformationWhenPaymentAmountIsLessThanDueAmount(ref int nmlPayment, ref int PaymentAmount, Transaction transaction)
        {
            nmlPayment = PaymentAmount/*(int)transaction.DueAmount*/;
            transaction.PaidAmount += PaymentAmount;
            transaction.DueAmount = transaction.PaymentAmount - (transaction.PaidAmount + transaction.Discount);
            transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
            PaymentAmount = 0;
        }

        private void SetInformationWhenPaymentAmountIsLargerThanDueAmount(ref int nmlPayment, ref int PaymentAmount, int disPayment, Transaction transaction)
        {
            nmlPayment = (int)transaction.DueAmount;
            if (disPayment > 0)
            {//we are reducing discount dispayment here cause to get the actual payment.
                nmlPayment -= disPayment;
            }
            PaymentAmount -= (int)transaction.DueAmount;
            if (disPayment > 0)
            {//we are adding discount disPayment value here cause with this we are adding only Payment amount.
                PaymentAmount += disPayment;
            }
            transaction.PaidAmount += ((int)transaction.DueAmount - disPayment);
            //transaction.Discount = ???? we are not adding discount amount here cause its already added in SetInformationWhenDiscountIsLessThanDueAmount()function
            transaction.DueAmount = 0;
            transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        }

        private void SetInformationWhenDiscountIsLessThanDueAmount(ref int disPayment, ref int Discount, Transaction transaction)
        {
            if (Discount > 0)
            {
                disPayment = Discount;
                transaction.Discount += Discount;
                Discount = 0;
            }
        }

        private void SetInformationWhenDiscountIsLargerThanDueAmount(ref int disPayment, ref int discount, Transaction transaction)
        {
            disPayment = (int)transaction.DueAmount;
            //transaction.PaidAmount = transaction.PaymentAmount;
            transaction.Discount += transaction.DueAmount;
            discount -= (int)transaction.DueAmount;
            transaction.DueAmount = 0;
            transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        }

        private void SetTransactionData(Transaction dbtransaction, Transaction clientSidetransaction)
        {
            dbtransaction.PaidAmount = dbtransaction.PaidAmount == null ? 0 : dbtransaction.PaidAmount.Value;
            dbtransaction.DueAmount = dbtransaction.DueAmount == null ? 0 : dbtransaction.DueAmount.Value;
            dbtransaction.Discount = dbtransaction.Discount == null ? 0 : dbtransaction.Discount.Value;

            dbtransaction.EmployeeID = AppUtils.GetLoginUserID();
            dbtransaction.BillCollectBy = clientSidetransaction.BillCollectBy;// int.Parse(Session["LoggedUserID"].ToString());
            dbtransaction.PaymentFromWhichPage = clientSidetransaction.PaymentFromWhichPage;// int.Parse(Session["LoggedUserID"].ToString());
        }

        private bool ResetNoexistOrNot(string ResetNo)
        {
            if (ResetNo != null)
            {
                int countForResetIsGeneraetOrNot = db.Transaction.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
                int countForResetIsGenerateOrNotFromPaymentHistory = db.PaymentHistory.Where(s => s.ResetNo.Trim().ToLower() == ResetNo.Trim().ToLower()).Count();
                if (countForResetIsGeneraetOrNot > 0 || countForResetIsGenerateOrNotFromPaymentHistory > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private int? GetBillCollectBy(Transaction transaction, int loginRoleID, int loginUserID)
        {
            if (loginRoleID == AppUtils.SuperUserRole || loginRoleID == AppUtils.AdminRole)
            {
                if ((int)CanSelectBillCollectBy.AdminCan == 1)
                {
                    return transaction.BillCollectBy == null ? loginUserID : transaction.BillCollectBy;
                }
                else
                {
                    return loginUserID;
                }
            }
            else if (loginRoleID == AppUtils.EmployeeRole)
            {
                if ((int)CanSelectBillCollectBy.EmploeeCan == 1)
                {
                    return transaction.BillCollectBy == null ? loginUserID : transaction.BillCollectBy;
                }
                else
                {
                    return loginUserID;
                }
            }
            else if (loginRoleID == AppUtils.ResellerRole)
            {
                if ((int)CanSelectBillCollectBy.ResellerCan == 1)
                {
                    return transaction.BillCollectBy == null ? loginUserID : transaction.BillCollectBy;
                }
                else
                {
                    return loginUserID;
                }
            }
            else
            {
                return transaction.BillCollectBy;
            }
        }

        private bool isAllowDiscount(int loginRoleID, int loginUserID)
        {
            if (loginRoleID == AppUtils.SuperUserRole || loginRoleID == AppUtils.AdminRole)
            {
                if ((int)CanGiveDiscount.AdminCan == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (loginRoleID == AppUtils.EmployeeRole)
            {
                if ((int)CanGiveDiscount.EmploeeCan == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (loginRoleID == AppUtils.ResellerRole)
            {
                if ((int)CanGiveDiscount.ResellerCan == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        #endregion

    }
}