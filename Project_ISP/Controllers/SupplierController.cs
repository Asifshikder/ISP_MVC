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
    public class SupplierController : Controller
    {
        public SupplierController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Supplier_List)]
        public ActionResult Index()
        {
            List<Supplier> lstSupplier = db.Supplier.ToList();
            return View(lstSupplier);
        }

        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Supplier)]
        public ActionResult InsertSupplier()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertSupplier(Supplier Supplier_Client)
        {
            Supplier Supplier_Check = db.Supplier.Where(s => s.SupplierName == Supplier_Client.SupplierName.Trim()).FirstOrDefault();

            if (Supplier_Check != null)
            {
                TempData["AlreadyInsert"] = "Supplier Already Added. Choose different Supplier. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Supplier Supplier_Return = new Supplier();

            try
            {
                Supplier_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Supplier_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Supplier_Return = db.Supplier.Add(Supplier_Client);
                db.SaveChanges();

                if (Supplier_Return.SupplierID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Supplier = Supplier_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertSupplierFromPopUp(Supplier Supplier_Client)
        {
            Supplier Supplier_Check = db.Supplier.Where(s => s.SupplierName == Supplier_Client.SupplierName.Trim()).FirstOrDefault();

            if (Supplier_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Supplier Already Added. Choose different Supplier. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Supplier Supplier_Return = new Supplier();

            try
            {
                Supplier_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Supplier_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Supplier_Return = db.Supplier.Add(Supplier_Client);
                db.SaveChanges();

                if (Supplier_Return.SupplierID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Supplier = Supplier_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetSupplierDetailsByID(int SupplierID)
        {
            var Supplier = db.Supplier.Where(s => s.SupplierID == SupplierID).Select(s => new { SupplierName = s.SupplierName, SupplierAddress = s.SupplierAddress }).FirstOrDefault();


            var JSON = Json(new { SupplierDetails = Supplier }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateSupplier(Supplier SupplierInfoForUpdate)
        {

            try
            {

                Supplier Supplier_Check = db.Supplier.Where(s => s.SupplierID != SupplierInfoForUpdate.SupplierID && s.SupplierName == SupplierInfoForUpdate.SupplierName.Trim()).FirstOrDefault();

                if (Supplier_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Supplier Already Added. Choose different Supplier. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Supplier_db = db.Supplier.Where(s => s.SupplierID == SupplierInfoForUpdate.SupplierID);
                SupplierInfoForUpdate.CreatedBy = Supplier_db.FirstOrDefault().CreatedBy;
                SupplierInfoForUpdate.CreatedDate = Supplier_db.FirstOrDefault().CreatedDate;
                SupplierInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                SupplierInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Supplier_db.SingleOrDefault()).CurrentValues.SetValues(SupplierInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Supplier_Return = Supplier_db.Select(s => new { SupplierID = s.SupplierID, PackageName = s.SupplierName, SupplierAddress = s.SupplierAddress });
                var JSON = Json(new { UpdateSuccess = true, SupplierUpdateInformation = Supplier_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, SupplierUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}