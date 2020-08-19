using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]
    public class MikrotikController : Controller
    {
     

        public MikrotikController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Mikrotik_List)]
        public ActionResult Index()
        {
            ViewBag.IPPoolID = new SelectList(db.IPPool.Select(s=>new {s.IPPoolID,s.PoolName}), "IPPoolID", "PoolName");
            ViewBag.IPPoolIDs = new SelectList(db.IPPool.Select(s => new { s.IPPoolID, s.PoolName }), "IPPoolID", "PoolName");
            List<Mikrotik> lstMikrotik = db.Mikrotik.ToList();
            return View(lstMikrotik);
        }

        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Mikrotik)]
        public ActionResult InsertMikrotik()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertMikrotik(Mikrotik Mikrotik_Client)
        {
            Mikrotik Mikrotik_Check = db.Mikrotik.Where(s => s.RealIP == Mikrotik_Client.RealIP.Trim()).FirstOrDefault();

            if (Mikrotik_Check != null)
            {
                TempData["AlreadyInsert"] = "Mikrotik Already Added. Choose different Mikrotik. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Mikrotik Mikrotik_Return = new Mikrotik();

            try
            {
                Mikrotik_Client.CreatedBy = int.Parse(Session["LoggedUserID"].ToString());
                Mikrotik_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Mikrotik_Return = db.Mikrotik.Add(Mikrotik_Client);
                db.SaveChanges();

                if (Mikrotik_Return.MikrotikID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Mikrotik = Mikrotik_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult InsertMikrotikFromPopUp(Mikrotik Mikrotik_Client)
        {
            int mikrotikCount = db.Mikrotik.Count();
            Mikrotik Mikrotik_Check = db.Mikrotik.Where(s => s.RealIP == Mikrotik_Client.RealIP.Trim()).FirstOrDefault();

            if (Mikrotik_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Mikrotik Already Added. Choose different Mikrotik. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Mikrotik Mikrotik_Return = new Mikrotik();

            try
            {
                Mikrotik_Client.CreatedBy = int.Parse(Session["LoggedUserID"].ToString());//AppUtils.LoginUserID;
                Mikrotik_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Mikrotik_Return = db.Mikrotik.Add(Mikrotik_Client);
                db.SaveChanges();

                if (Mikrotik_Return.MikrotikID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Mikrotik = Mikrotik_Return, MikrotikCount = mikrotikCount }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMikrotikDetailsByID(int MikrotikID)
        {
            var Mikrotik = db.Mikrotik.Where(s => s.MikrotikID == MikrotikID).FirstOrDefault();


            var JSON = Json(new { MikrotikDetails = Mikrotik }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult UpdateMikrotik(Mikrotik MikrotikInfoForUpdate)
        {

            try
            {

                Mikrotik Mikrotik_Check = db.Mikrotik.Where(s => s.MikrotikID != MikrotikInfoForUpdate.MikrotikID && s.RealIP == MikrotikInfoForUpdate.RealIP.Trim()).FirstOrDefault();

                if (Mikrotik_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Mikrotik Already Added. Choose different Mikrotik. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Mikrotik_db = db.Mikrotik.Where(s => s.MikrotikID == MikrotikInfoForUpdate.MikrotikID);
                MikrotikInfoForUpdate.CreatedBy = Mikrotik_db.FirstOrDefault().CreatedBy;
                MikrotikInfoForUpdate.CreatedDate = Mikrotik_db.FirstOrDefault().CreatedDate;
                MikrotikInfoForUpdate.UpdateBy = AppUtils.GetLoginUserID();
                MikrotikInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Mikrotik_db.SingleOrDefault()).CurrentValues.SetValues(MikrotikInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Mikrotik_Return = Mikrotik_db;
                var JSON = Json(new { UpdateSuccess = true, MikrotikUpdateInformation = Mikrotik_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch(Exception ex)
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, MikrotikUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}