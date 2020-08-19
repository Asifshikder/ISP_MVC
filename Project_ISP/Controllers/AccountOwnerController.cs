using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
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
    public class AccountOwnerController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: AccountOwner
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Account_Owner_List)]
        public ActionResult Index()
        {
            var OwnerList = db.AccountOwner.Where(x => x.Status == AppUtils.TableStatusIsActive).ToList();
            return View(OwnerList);
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertAccountOwner(AccountOwner OwnerDetails)
        {
            try
            {
                db.AccountOwner.Add(OwnerDetails);
                OwnerDetails.CreateBy = AppUtils.GetLoginUserID();
                OwnerDetails.CreateDate = AppUtils.GetDateTimeNow();
                OwnerDetails.Status = AppUtils.TableStatusIsActive;
                db.SaveChanges();
                AccountOwnerViewModel accountOwner = new AccountOwnerViewModel
                {
                    OwnerID = OwnerDetails.OwnerID,
                    OwnerName = OwnerDetails.OwnerName
                };


                return Json(new { success = true, accountOwner = accountOwner }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetOwnerByID(int OwnerID)
        {
            var OwnerInfo = db.AccountOwner.Where(s => s.OwnerID == OwnerID).Select(s => new { OwnerID = s.OwnerID, OwnerName = s.OwnerName }).FirstOrDefault();


            var JSON = Json(new { OwnerInfo = OwnerInfo }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateOwnerDetails(AccountOwner OwnerDetails)
        {

            try
            {
                AccountOwner accountOwner = new AccountOwner();
                accountOwner = db.AccountOwner.Find(OwnerDetails.OwnerID);
                accountOwner.OwnerName = OwnerDetails.OwnerName;
                accountOwner.UpdateBy = AppUtils.GetLoginUserID();
                accountOwner.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(accountOwner).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var owner =
                    new AccountOwnerViewModel()
                    {
                        OwnerID = accountOwner.OwnerID,
                        OwnerName = accountOwner.OwnerName,
                    };
                var JSON = Json(new { success = true, owner = owner }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteOwner(int ID)
        {
            AccountOwner owner = new AccountOwner();
            owner = db.AccountOwner.Find(ID);
            owner.DeleteBy = AppUtils.GetLoginUserID();
            owner.DeleteDate = AppUtils.GetDateTimeNow();
            owner.Status = AppUtils.TableStatusIsDelete;


            db.Entry(owner).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true}, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }
    }
}