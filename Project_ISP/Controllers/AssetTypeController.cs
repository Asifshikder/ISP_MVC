using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_ISP.Controllers
{
    public class AssetTypeController : Controller
    {
        public AssetTypeController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [UserRIghtCheck(ControllerValue = AppUtils.View_AssetType_List)]
        public ActionResult Index()
        {

            List<AssetType> lstAssetType = db.AssetType.ToList();
            return View(lstAssetType);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_AssetType)]
        public ActionResult InsertAssetType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertAssetType(AssetType AssetType_Client)
        {
            AssetType AssetType_Check = db.AssetType.Where(s => s.AssetTypeName == AssetType_Client.AssetTypeName.Trim()).FirstOrDefault();

            if (AssetType_Check != null)
            {
                //  TempData["AlreadyInsert"] = "AssetType Already Added. Choose different AssetType. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            AssetType AssetType_Return = new AssetType();

            try
            {
                AssetType_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                AssetType_Client.CreatedDate = AppUtils.GetDateTimeNow();

                AssetType_Return = db.AssetType.Add(AssetType_Client);
                db.SaveChanges();

                if (AssetType_Return.AssetTypeID > 0)
                {
                    //   TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, AssetType = AssetType_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertAssetTypeFromPopUp(AssetType AssetType_Client)
        {
            AssetType band_Check = db.AssetType.Where(s => s.AssetTypeName == AssetType_Client.AssetTypeName.Trim()).FirstOrDefault();

            if (band_Check != null)
            {
                //  TempData["AlreadyInsert"] = "AssetType Already Added. Choose different AssetType. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            AssetType AssetType_Return = new AssetType();

            try
            {
                AssetType_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                AssetType_Client.CreatedDate = AppUtils.GetDateTimeNow();

                AssetType_Return = db.AssetType.Add(AssetType_Client);
                db.SaveChanges();

                if (AssetType_Return.AssetTypeID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, AssetType = AssetType_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetAssetTypeDetailsByID(int AssetTypeID)
        {
            var AssetType = db.AssetType.Where(s => s.AssetTypeID == AssetTypeID).Select(s => new { AssetTypeName = s.AssetTypeName }).FirstOrDefault();


            var JSON = Json(new { AssetTypeDetails = AssetType }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateAssetType(AssetType AssetTypeInfoForUpdate)
        {

            try
            {

                AssetType band_Check = db.AssetType.Where(s => s.AssetTypeID != AssetTypeInfoForUpdate.AssetTypeID && s.AssetTypeName == AssetTypeInfoForUpdate.AssetTypeName.Trim()).FirstOrDefault();

                if (band_Check != null)
                {
                    //TempData["AlreadyInsert"] = "AssetType Already Added. Choose different AssetType. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var AssetType_db = db.AssetType.Where(s => s.AssetTypeID == AssetTypeInfoForUpdate.AssetTypeID);
                AssetTypeInfoForUpdate.CreatedBy = AssetType_db.FirstOrDefault().CreatedBy;
                AssetTypeInfoForUpdate.CreatedDate = AssetType_db.FirstOrDefault().CreatedDate;
                AssetTypeInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                AssetTypeInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(AssetType_db.SingleOrDefault()).CurrentValues.SetValues(AssetTypeInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var AssetTypes = AssetType_db.Select(s => new { AssetTypeID = s.AssetTypeID, PackageName = s.AssetTypeName });
                var JSON = Json(new { UpdateSuccess = true, AssetTypeUpdateInformation = AssetTypes }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, AssetTypeUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}