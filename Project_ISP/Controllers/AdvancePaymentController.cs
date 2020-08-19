using ISP_ManagementSystemModel.Models;
using Project_ISP;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ISP_ManagementSystemModel.AppUtils;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class AdvancePaymentController : Controller
    {

        public AdvancePaymentController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();
        //
        // GET: /AdvancePayment/
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Advance_Payment)]
        public ActionResult AddAdvancePayment()
        {
            return View();
        }

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Advance_Payment_Reseller_Clients_By_Admin)]
        public ActionResult AddAdvancePaymentForResellerClientsByAdmin()
        { 
            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            ViewBag.ResellerID = new SelectList(db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            return View();
        }

        [HttpPost]
        public ActionResult getAutoCompleateInformation(string Name,int resellerid = 0)
        {
            try
            {
                if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole && resellerid  > 0)
                {
                    var clientDetails = db.ClientDetails.Where(s => s.ResellerID == resellerid && s.Name.Contains(Name) && s.IsNewClient != AppUtils.isNewClient).Select(s => new { label = s.Name, val = s.ClientDetailsID }).ToList();
                    return Json(new { clientDetails }, JsonRequestBehavior.AllowGet);
                }
                if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
                {
                    var clientDetails = db.ClientDetails.Where(s => s.ResellerID == null && s.Name.Contains(Name) && s.IsNewClient != AppUtils.isNewClient).Select(s => new { label = s.Name, val = s.ClientDetailsID }).ToList();
                    return Json(new { clientDetails }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var resellerID = AppUtils.GetLoginUserID();
                    var clientDetails = db.ClientDetails.Where(s => s.ResellerID == resellerID && s.Name.Contains(Name) && s.IsNewClient != AppUtils.isNewClient).Select(s => new { label = s.Name, val = s.ClientDetailsID }).ToList();
                    return Json(new { clientDetails }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getAutoCompleateDetailsInformation(int ClientDetsilsID)
        {
            try
            {
                var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetsilsID).Select(s => new { Mobile = s.ContactNumber, ClientAdress = s.Address }).FirstOrDefault();
                return Json(new { Success = true, ClientDetails = ClientDetails }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveAdvanceAmount(int ClientDetailsID, int Amount, string Remarks)
        {
            //db.Entry(new Transaction()).CurrentValues.SetValues(new Transaction());

            AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();

            try
            {
                if (advancePayment != null)
                {
                    advancePayment.UpdatePaymentBy = AppUtils.GetLoginEmployeeName();
                    advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                    advancePayment.AdvanceAmount += Amount;
                    advancePayment.Remarks = Remarks;
                    db.Entry(advancePayment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    AdvancePayment insertAdvancePayment = new AdvancePayment();
                    insertAdvancePayment.ClientDetailsID = ClientDetailsID;
                    insertAdvancePayment.AdvanceAmount = Amount;
                    insertAdvancePayment.Remarks = Remarks;
                    insertAdvancePayment.CreatePaymentBy = AppUtils.GetLoginEmployeeName();
                    insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                    db.AdvancePayment.Add(insertAdvancePayment);
                    db.SaveChanges();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
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

        [HttpGet] 
        [UserRIghtCheck(ControllerValue = AppUtils.View_Advance_Payment_List)]
        public ActionResult ViewAdvancePayment()
        {

            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            //ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName"); 
            //ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            //ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");

            //ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            //ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");

            //int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            //var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            //ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            //ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            //ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            //var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            //ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            //ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            bool forReseller = false;
            int resellerID = 0;
            IEnumerable<AdvancePayment> advancePayments = Enumerable.Empty<AdvancePayment>();
            if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole)
            {
                forReseller = false;
                resellerID = 0;
                advancePayments = db.AdvancePayment.Where(x => x.ClientDetils.ResellerID == null).AsEnumerable();
            }
            else
            {
                forReseller = true;
                resellerID = AppUtils.GetLoginUserID();
                advancePayments = db.AdvancePayment.Where(x => x.ClientDetils.ResellerID != null).AsEnumerable();
            }
            setViewBagList(forReseller, resellerID);

            //List<int> clientDetailsID = advancePayments.Where(s => s.ClientDetils != null).Select(s => s.ClientDetailsID).Distinct().ToList();
            //if (clientDetailsID.Count > 0)
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
            //        .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), tns => tns.ClientDetailsID, cls => cls.ClientDetailsID, (tns, cls) => new ClientSetByViewBag
            //        {
            //            ClientDetailsID = tns.ClientDetailsID,
            //            TransactionID = tns.TransactionID,
            //            PaymentAmount = tns.PaymentAmount.Value,
            //            // LineStatusActiveDate = cls.FirstOrDefault().LineStatusWillActiveInThisDate.HasValue ? cls.FirstOrDefault().LineStatusWillActiveInThisDate.Value + " " + AppUtils.GetStatusDivByStatusID(cls.FirstOrDefault().LineStatusID) : "",
            //            ClientLineStatus = cls.FirstOrDefault()
            //        })
            //        //.Select(s => new ClientSetByViewBag
            //        //{
            //        //    ClientDetailsID = s.ClientDetailsID,
            //        //    TransactionID = s.TransactionID,
            //        //    PaymentAmount = s.PaymentAmount.Value,

            //        //})
            //        .ToList()
            //        .Select(s => new ClientSetByViewBag
            //        {
            //            ClientDetailsID = s.ClientDetailsID,
            //            TransactionID = s.TransactionID,
            //            PaymentAmount = s.PaymentAmount,
            //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
            //        }).ToList();
            //}
            //else
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //         .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), tns => tns.ClientDetailsID, cls => cls.ClientDetailsID, (tns, cls) => new ClientSetByViewBag
            //         {
            //             ClientDetailsID = tns.ClientDetailsID,
            //             TransactionID = tns.TransactionID,
            //             PaymentAmount = tns.PaymentAmount.Value,
            //             // LineStatusActiveDate = cls.FirstOrDefault().LineStatusWillActiveInThisDate.HasValue ? cls.FirstOrDefault().LineStatusWillActiveInThisDate.Value + " " + AppUtils.GetStatusDivByStatusID(cls.FirstOrDefault().LineStatusID) : "",
            //             ClientLineStatus = cls.FirstOrDefault()
            //         })
            //        .ToList()
            //        .Select(s => new ClientSetByViewBag
            //        {
            //            ClientDetailsID = s.ClientDetailsID,
            //            TransactionID = s.TransactionID,
            //            PaymentAmount = s.PaymentAmount,
            //            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
            //        }).ToList();
            //}

            return View(advancePayments);
        }

        [HttpGet] 
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Advance_Payment_Reseller_Clients_By_Admin)]
        public ActionResult ViewAdvancePaymentForResellerClientsByAdmin()
        {
            int resellerID = 0;
            bool forReseller = false;
            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            ViewBag.ResellerID = new SelectList(db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");
            IEnumerable<AdvancePayment> advancePayments = Enumerable.Empty<AdvancePayment>();
            if (resellerID > 0)
            {
                forReseller = true; 
                //advancePayments = db.AdvancePayment.Where(x => x.ClientDetils.ResellerID == resellerID).AsEnumerable();
                setViewBagList(forReseller, resellerID);
            }
            else
            {
                setViewBagListForResellerAccountsByAdmin();
            }
            return View(advancePayments);
        }

        [HttpPost]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Advance_Payment_Reseller_Clients_By_Admin)]
        public ActionResult ViewAdvancePaymentForResellerClientsByAdmin(int resellerid = 0)
        { 
            bool forReseller = false;
            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            ViewBag.resellerListid = new SelectList(db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName", resellerid);
            IEnumerable<AdvancePayment> advancePayments = Enumerable.Empty<AdvancePayment>();
            if (resellerid > 0)
            {
                forReseller = true; 
                advancePayments = db.AdvancePayment.Where(x => x.ClientDetils.ResellerID == resellerid).AsEnumerable();
                setViewBagList(forReseller, resellerid);
            }
            else
            {
                setViewBagListForResellerAccountsByAdmin();
            }
             
            return View(advancePayments);
        }

        private string GetLineActiveDateWithStatus(IEnumerable<ClientLineStatus> cls)
        {
            return cls.FirstOrDefault().LineStatusWillActiveInThisDate.Value.Date + " " + AppUtils.GetStatusDivByStatusID(cls.FirstOrDefault().LineStatusID);
        }

        [HttpPost]
        public ActionResult ViewAdvancePaymentIDForUpdate(int AdvancePaymentID)
        {
            var advancePayment = db.AdvancePayment.Where(s => s.AdvancePaymentID == AdvancePaymentID).
                Select(s => new
                {
                    LoginName = s.ClientDetils.LoginName,
                    AdvanceAmount = s.AdvanceAmount,
                    Remarks = s.Remarks,
                    ContactNumber = s.ClientDetils.ContactNumber,
                    Address = s.ClientDetils.Address
                }).FirstOrDefault();

            var JSON = Json(new { ViewAdvancePayment = advancePayment }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        public ActionResult UpdateAdvancePayment(AdvancePayment UpdateAdvancePaymentInformation)
        {
            try
            {
                var advancePayment =
                    db.AdvancePayment.Where(s => s.AdvancePaymentID == UpdateAdvancePaymentInformation.AdvancePaymentID);
                UpdateAdvancePaymentInformation.ClientDetailsID = advancePayment.FirstOrDefault().ClientDetailsID;
                UpdateAdvancePaymentInformation.CollectBy = advancePayment.FirstOrDefault().CollectBy;
                UpdateAdvancePaymentInformation.CreatePaymentBy = advancePayment.FirstOrDefault().CreatePaymentBy;
                UpdateAdvancePaymentInformation.FirstPaymentDate = advancePayment.FirstOrDefault().FirstPaymentDate;
                UpdateAdvancePaymentInformation.UpdatePaymentBy = "Hasan";
                UpdateAdvancePaymentInformation.UpdatePaymentDate = AppUtils.GetDateTimeNow();



                db.Entry(advancePayment.FirstOrDefault()).CurrentValues.SetValues(UpdateAdvancePaymentInformation);
                db.SaveChanges();
                var advancePayments =
                    advancePayment.Select(s => new { AdvancePaymentID = s.AdvancePaymentID, AdvanceAmount = s.AdvanceAmount, Remarks = s.Remarks });
                var JSON = Json(new { UpdateSuccess = true, UpdateAdvancePaymentInformation = advancePayments }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, UpdateAdvancePaymentInformation = "" }, JsonRequestBehavior.AllowGet);
            }


        }


    }
}