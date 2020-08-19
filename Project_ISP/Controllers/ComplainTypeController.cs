using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;

namespace Project_ISP.Controllers
{
    public class ComplainTypeController : Controller
    {
        public ComplainTypeController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [UserRIghtCheck(ControllerValue = AppUtils.View_ComplainType_List)]
        public ActionResult Index()
        {

            List<ComplainType> lstComplainType = db.ComplainType.ToList();
            return View(lstComplainType);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_ComplainType)]
        public ActionResult InsertComplainType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertComplainType(ComplainType ComplainType_Client)
        {
            ComplainType ComplainType_Check = db.ComplainType.Where(s => s.ComplainTypeName == ComplainType_Client.ComplainTypeName.Trim()).FirstOrDefault();

            if (ComplainType_Check != null)
            {
                //  TempData["AlreadyInsert"] = "ComplainType Already Added. Choose different ComplainType. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            ComplainType ComplainType_Return = new ComplainType();

            try
            {
                ComplainType_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                ComplainType_Client.CreatedDate = AppUtils.GetDateTimeNow();

                ComplainType_Return = db.ComplainType.Add(ComplainType_Client);
                db.SaveChanges();

                if (ComplainType_Return.ComplainTypeID > 0)
                {
                    //   TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, ComplainType = ComplainType_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //     TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertComplainTypeFromPopUp(ComplainType ComplainType_Client)
        {
            ComplainType band_Check = db.ComplainType.Where(s => s.ComplainTypeName == ComplainType_Client.ComplainTypeName.Trim()).FirstOrDefault();

            if (band_Check != null)
            {
                //  TempData["AlreadyInsert"] = "ComplainType Already Added. Choose different ComplainType. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            ComplainType ComplainType_Return = new ComplainType();

            try
            {
                ComplainType_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                ComplainType_Client.CreatedDate = AppUtils.GetDateTimeNow();

                ComplainType_Return = db.ComplainType.Add(ComplainType_Client);
                db.SaveChanges();

                if (ComplainType_Return.ComplainTypeID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, ComplainType = ComplainType_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetComplainTypeDetailsByID(int ComplainTypeID)
        {
            var ComplainType = db.ComplainType.Where(s => s.ComplainTypeID == ComplainTypeID).Select(s => new { ComplainTypeName = s.ComplainTypeName, ShowMessageBox = s.ShowMessageBox }).FirstOrDefault();


            var JSON = Json(new { ComplainTypeDetails = ComplainType }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }
        [ValidateAntiForgeryToken]
        public ActionResult MessageBoxShowOrHide(int ComplainTypeID)
        {
            var ComplainType = db.ComplainType.Where(s => s.ComplainTypeID == ComplainTypeID).Select(s => new { ShowMessageBox = s.ShowMessageBox }).FirstOrDefault();


            var JSON = Json(new { ShowMesssageBox = ComplainType.ShowMessageBox }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateComplainType(ComplainType ComplainTypeInfoForUpdate)
        {

            try
            {

                ComplainType band_Check = db.ComplainType.Where(s => s.ComplainTypeID != ComplainTypeInfoForUpdate.ComplainTypeID && s.ComplainTypeName == ComplainTypeInfoForUpdate.ComplainTypeName.Trim()).FirstOrDefault();

                if (band_Check != null)
                {
                    //TempData["AlreadyInsert"] = "ComplainType Already Added. Choose different ComplainType. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var ComplainType_db = db.ComplainType.Where(s => s.ComplainTypeID == ComplainTypeInfoForUpdate.ComplainTypeID);
                ComplainTypeInfoForUpdate.CreatedBy = ComplainType_db.FirstOrDefault().CreatedBy;
                ComplainTypeInfoForUpdate.CreatedDate = ComplainType_db.FirstOrDefault().CreatedDate;
                ComplainTypeInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                ComplainTypeInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(ComplainType_db.SingleOrDefault()).CurrentValues.SetValues(ComplainTypeInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var ComplainTypes = ComplainType_db.Select(s => new { ComplainTypeID = s.ComplainTypeID, PackageName = s.ComplainTypeName, ShowMessageBox = s.ShowMessageBox == true ? "Yes":"No" });
                var JSON = Json(new { UpdateSuccess = true, ComplainTypeUpdateInformation = ComplainTypes }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ComplainTypeUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}