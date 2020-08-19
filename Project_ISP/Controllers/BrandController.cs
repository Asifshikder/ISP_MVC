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
    public class BrandController : Controller
    {
        public BrandController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }

        private ISPContext db = new ISPContext();

        [UserRIghtCheck(ControllerValue = AppUtils.View_Brand_List)]
        public ActionResult Index()
        {

            List<Brand> lstBrand = db.Brand.ToList();
            return View(lstBrand);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Brand)]
        public ActionResult InsertBrand()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult InsertBrand(Brand Brand_Client)
        {
            Brand brand_Check = db.Brand.Where(s => s.BrandName == Brand_Client.BrandName.Trim()).FirstOrDefault();

            if (brand_Check != null)
            {
              //  TempData["AlreadyInsert"] = "Brand Already Added. Choose different Brand. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Brand brand_Return = new Brand();

            try
            {
                Brand_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Brand_Client.CreatedDate = AppUtils.GetDateTimeNow();

                brand_Return = db.Brand.Add(Brand_Client);
                db.SaveChanges();

                if (brand_Return.BrandID > 0)
                {
                 //   TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, brand = brand_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertBrandFromPopUp(Brand Brand_Client)
        {
            Brand band_Check = db.Brand.Where(s => s.BrandName == Brand_Client.BrandName.Trim()).FirstOrDefault();

            if (band_Check != null)
            {
              //  TempData["AlreadyInsert"] = "Brand Already Added. Choose different Brand. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Brand brand_Return = new Brand();

            try
            {
                Brand_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Brand_Client.CreatedDate = AppUtils.GetDateTimeNow();

                brand_Return = db.Brand.Add(Brand_Client);
                db.SaveChanges();

                if (brand_Return.BrandID > 0)
                {
                  //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Brand = brand_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetBrandDetailsByID(int BrandID)
        {
            var Brand = db.Brand.Where(s => s.BrandID == BrandID).Select(s => new { BrandName = s.BrandName }).FirstOrDefault();


            var JSON = Json(new { BrandDetails = Brand }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateBrand(Brand BrandInfoForUpdate)
        {

            try
            {

                Brand band_Check = db.Brand.Where(s => s.BrandID != BrandInfoForUpdate.BrandID && s.BrandName == BrandInfoForUpdate.BrandName.Trim()).FirstOrDefault();

                if (band_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Brand Already Added. Choose different Brand. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var brand_db = db.Brand.Where(s => s.BrandID == BrandInfoForUpdate.BrandID);
                BrandInfoForUpdate.CreatedBy = brand_db.FirstOrDefault().CreatedBy;
                BrandInfoForUpdate.CreatedDate = brand_db.FirstOrDefault().CreatedDate;
                BrandInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                BrandInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(brand_db.SingleOrDefault()).CurrentValues.SetValues(BrandInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var brands = brand_db.Select(s => new { BrandID = s.BrandID, PackageName = s.BrandName });
                var JSON = Json(new { UpdateSuccess = true, BrandUpdateInformation = brands }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, BrandUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}