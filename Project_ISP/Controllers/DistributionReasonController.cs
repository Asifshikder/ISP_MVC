using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;

namespace Project_ISP.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]
    public class DistributionReasonController : Controller
    {
        public DistributionReasonController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Distribution_Reason_List)]
        public ActionResult Index()
        {
            List<DistributionReason> lstDistributionReason = db.DistributionReason.ToList();
            return View(lstDistributionReason);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Distribution_Reason)]
        public ActionResult InsertDistributionReason()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertDistributionReason(DistributionReason DistributionReason_Client)
        {
            DistributionReason distributionReason_Check = db.DistributionReason.Where(s => s.DistributionReasonName == DistributionReason_Client.DistributionReasonName.Trim()).FirstOrDefault();

            if (distributionReason_Check != null)
            {
                TempData["AlreadyInsert"] = "DistributionReason Already Added. Choose different DistributionReason. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            DistributionReason distributionReason_Return = new DistributionReason();

            try
            {
                DistributionReason_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                DistributionReason_Client.CreatedDate = AppUtils.GetDateTimeNow();

                distributionReason_Return = db.DistributionReason.Add(DistributionReason_Client);
                db.SaveChanges();

                if (distributionReason_Return.DistributionReasonID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, DistributionReason = distributionReason_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertDistributionReasonFromPopUp(DistributionReason DistributionReason_Client)
        {
            DistributionReason distributionReason_Check = db.DistributionReason.Where(s => s.DistributionReasonName == DistributionReason_Client.DistributionReasonName.Trim()).FirstOrDefault();

            if (distributionReason_Check != null)
            {
                //  TempData["AlreadyInsert"] = "DistributionReason Already Added. Choose different DistributionReason. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            DistributionReason distributionReason_Return = new DistributionReason();

            try
            {
                DistributionReason_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                DistributionReason_Client.CreatedDate = AppUtils.GetDateTimeNow();

                distributionReason_Return = db.DistributionReason.Add(DistributionReason_Client);
                db.SaveChanges();

                if (distributionReason_Return.DistributionReasonID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, DistributionReason = distributionReason_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetDistributionReasonDetailsByID(int DistributionReasonID)
        {
            var DistributionReason = db.DistributionReason.Where(s => s.DistributionReasonID == DistributionReasonID).Select(s => new { DistributionReasonName = s.DistributionReasonName }).FirstOrDefault();


            var JSON = Json(new { DistributionReasonDetails = DistributionReason }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateDistributionReason(DistributionReason DistributionReasonInfoForUpdate)
        {

            try
            {

                DistributionReason distributionReason_Check = db.DistributionReason.Where(s => s.DistributionReasonID != DistributionReasonInfoForUpdate.DistributionReasonID && s.DistributionReasonName == DistributionReasonInfoForUpdate.DistributionReasonName.Trim()).FirstOrDefault();

                if (distributionReason_Check != null)
                {
                    //TempData["AlreadyInsert"] = "DistributionReason Already Added. Choose different DistributionReason. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var distributionReason_db = db.DistributionReason.Where(s => s.DistributionReasonID == DistributionReasonInfoForUpdate.DistributionReasonID);
                DistributionReasonInfoForUpdate.CreatedBy = distributionReason_db.FirstOrDefault().CreatedBy;
                DistributionReasonInfoForUpdate.CreatedDate = distributionReason_db.FirstOrDefault().CreatedDate;
                DistributionReasonInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                DistributionReasonInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(distributionReason_db.SingleOrDefault()).CurrentValues.SetValues(DistributionReasonInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var distributionReason_Return = distributionReason_db.Select(s => new { DistributionReasonID = s.DistributionReasonID, PackageName = s.DistributionReasonName });
                var JSON = Json(new { UpdateSuccess = true, DistributionReasonUpdateInformation = distributionReason_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, DistributionReasonUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}