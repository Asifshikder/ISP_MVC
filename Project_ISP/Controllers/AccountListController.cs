using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class AccountListController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: AccountList
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_AccountList)]
        public ActionResult Index()
        {
            ViewBag.Owner = new SelectList(db.AccountOwner.Where(x => x.Status == AppUtils.TableStatusIsActive), "OwnerID", "OwnerName");
            var AccountList = db.AccountList.Where(x => x.Status == AppUtils.TableStatusIsActive).Include(a => a.AccountOwner).ToList();
            ViewBag.NetWorth = db.AccountList.Where(x => x.Status == AppUtils.TableStatusIsActive).Sum(a => a.InitialBalance);
            return View(AccountList);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Insert_AccountList)]
        public ActionResult Create()
        {
            ViewBag.Owner = new SelectList(db.AccountOwner.Where(x => x.Status == AppUtils.TableStatusIsActive), "OwnerID", "OwnerName");
            return View();
        }

        [HttpPost]
        [UserRIghtCheck(ControllerValue = AppUtils.Insert_AccountList)]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult CreateConfirm(AccountList accountlist)
        {
            var dbAccounlist = db.AccountList.Where(s => s.AccountTitle == accountlist.AccountTitle).FirstOrDefault();
            if (dbAccounlist != null)
            {
                return Json(new { nameExist = true });
            }

            try
            {
                accountlist.CreateBy = AppUtils.GetLoginUserID();
                accountlist.CreateData = AppUtils.GetDateTimeNow();
                accountlist.Status = AppUtils.TableStatusIsActive;
                db.AccountList.Add(accountlist);
                db.SaveChanges();

                AccountingHistory accountingHistory = new AccountingHistory();
                //Mode 1 mean Create 2 mean Update
                SetInformationForAccountHistory(ref accountingHistory, accountlist, 1);
                db.AccountingHistory.Add(accountingHistory);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetInitialBalanceByID(int ID)
        {
            var Accounts = db.AccountList.Where(s => s.AccountListID == ID).Select(s => new { AccountListID = s.AccountListID, InitialBalance = s.InitialBalance, AccountTitle = s.AccountTitle }).FirstOrDefault();

            var JSON = Json(new { Accounts = Accounts }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [UserRIghtCheck(ControllerValue = AppUtils.Record_InitialBalance)]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateInitialBalance(AccountList accountlist)
        {
            try
            {
                AccountList Account = new AccountList();
                Account = db.AccountList.Find(accountlist.AccountListID);
                Account.InitialBalance = accountlist.InitialBalance;
                Account.UpdateBy = AppUtils.GetLoginUserID();
                Account.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.AccountListID == accountlist.AccountListID).FirstOrDefault();
                //Mode 1 mean Create 2 mean Update
                SetInformationForAccountHistory(ref accountingHistory, accountlist, 2);
                db.Entry(accountingHistory).State = EntityState.Modified;
                db.SaveChanges();

                var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDetailsByID(int ID)
        {
            var Accounts = db.AccountList.Where(s => s.AccountListID == ID).FirstOrDefault();

            var JSON = Json(new { Accounts = Accounts }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_AccountList)]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateAccountList(AccountList AccountInfo)
        {

            try
            {
                AccountList Account = new AccountList();
                Account = db.AccountList.Find(AccountInfo.AccountListID);
                Account.AccountTitle = AccountInfo.AccountTitle;
                Account.Description = AccountInfo.Description;
                Account.InitialBalance = AccountInfo.InitialBalance;
                Account.AccountNumber = AccountInfo.AccountNumber;
                Account.ContactPerson = AccountInfo.ContactPerson;
                Account.Phone = AccountInfo.Phone;
                Account.BankUrl = AccountInfo.BankUrl;
                Account.OwnerID = AccountInfo.OwnerID;
                Account.UpdateBy = AppUtils.GetLoginUserID();
                Account.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_AccountList)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(int ID)
        {
            AccountList account = new AccountList();
            account = db.AccountList.Find(ID);
            account.DeleteBy = AppUtils.GetLoginUserID();
            account.DeleteDate = AppUtils.GetDateTimeNow();
            account.Status = AppUtils.TableStatusIsDelete;


            db.Entry(account).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        private void SetInformationForAccountHistory(ref AccountingHistory accountingHistory, AccountList accountlist, int CreateOrUpdate)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            accountingHistory.Amount = Convert.ToDouble(accountlist.InitialBalance.Value);
            if (CreateOrUpdate == 1)//mean create
            {
                accountingHistory.AccountListID = accountlist.AccountListID;
                accountingHistory.ActionTypeID = (int)AppUtils.AccountingHistoryType.AccountList;
                accountingHistory.Date = AppUtils.GetDateTimeNow();
                accountingHistory.DRCRTypeID = (int)AppUtils.AccountTransactionType.CR;
                accountingHistory.Description = "Capital Balance";
                accountingHistory.Year = dt.Year;
                accountingHistory.Month = dt.Month;
                accountingHistory.Day = dt.Day;
                accountingHistory.CreateBy = AppUtils.GetLoginUserID();
                accountingHistory.CreateDate = dt;
                accountingHistory.Status = AppUtils.TableStatusIsActive;
            }
            else
            {
                accountingHistory.UpdateBy = AppUtils.GetLoginUserID();
                accountingHistory.UpdateDate = dt;
            }
        }
    }
}