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
    public class ProductStatusController : Controller
    {
        public ProductStatusController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Status_List)]
        public ActionResult Index()
        {
            List<ProductStatus> lstProductStatus = db.ProductStatus.ToList();
            return View(lstProductStatus);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Product_Status)]
        public ActionResult InsertProductStatus()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertProductStatus(ProductStatus ProductStatus_Client)
        {
            ProductStatus ProductStatus_Check = db.ProductStatus.Where(s => s.ProductStatusName == ProductStatus_Client.ProductStatusName.Trim()).FirstOrDefault();

            if (ProductStatus_Check != null)
            {
                TempData["AlreadyInsert"] = "ProductStatus Already Added. Choose different ProductStatus. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            ProductStatus ProductStatus_Return = new ProductStatus();

            try
            {
                ProductStatus_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                ProductStatus_Client.CreatedDate = AppUtils.GetDateTimeNow();

                ProductStatus_Return = db.ProductStatus.Add(ProductStatus_Client);
                db.SaveChanges();

                if (ProductStatus_Return.ProductStatusID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, ProductStatus = ProductStatus_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertProductStatusFromPopUp(ProductStatus ProductStatus_Client)
        {
            ProductStatus ProductStatus_Check = db.ProductStatus.Where(s => s.ProductStatusName == ProductStatus_Client.ProductStatusName.Trim()).FirstOrDefault();

            if (ProductStatus_Check != null)
            {
                //  TempData["AlreadyInsert"] = "ProductStatus Already Added. Choose different ProductStatus. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            ProductStatus ProductStatus_Return = new ProductStatus();

            try
            {
                ProductStatus_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                ProductStatus_Client.CreatedDate = AppUtils.GetDateTimeNow();

                ProductStatus_Return = db.ProductStatus.Add(ProductStatus_Client);
                db.SaveChanges();

                if (ProductStatus_Return.ProductStatusID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, ProductStatus = ProductStatus_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetProductStatusDetailsByID(int ProductStatusID)
        {
            var ProductStatus = db.ProductStatus.Where(s => s.ProductStatusID == ProductStatusID).Select(s => new { ProductStatusName = s.ProductStatusName }).FirstOrDefault();


            var JSON = Json(new { ProductStatusDetails = ProductStatus }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProductStatus(ProductStatus ProductStatusInfoForUpdate)
        {

            try
            {

                ProductStatus ProductStatus_Check = db.ProductStatus.Where(s => s.ProductStatusID != ProductStatusInfoForUpdate.ProductStatusID && s.ProductStatusName == ProductStatusInfoForUpdate.ProductStatusName.Trim()).FirstOrDefault();

                if (ProductStatus_Check != null)
                {
                    //TempData["AlreadyInsert"] = "ProductStatus Already Added. Choose different ProductStatus. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var ProductStatus_db = db.ProductStatus.Where(s => s.ProductStatusID == ProductStatusInfoForUpdate.ProductStatusID);
                ProductStatusInfoForUpdate.CreatedBy = ProductStatus_db.FirstOrDefault().CreatedBy;
                ProductStatusInfoForUpdate.CreatedDate = ProductStatus_db.FirstOrDefault().CreatedDate;
                ProductStatusInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                ProductStatusInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(ProductStatus_db.SingleOrDefault()).CurrentValues.SetValues(ProductStatusInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var ProductStatus_Return = ProductStatus_db.Select(s => new { ProductStatusID = s.ProductStatusID, PackageName = s.ProductStatusName });
                var JSON = Json(new { UpdateSuccess = true, ProductStatusUpdateInformation = ProductStatus_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ProductStatusUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}