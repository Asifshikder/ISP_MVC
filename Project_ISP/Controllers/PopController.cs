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
    public class PopController : Controller
    {
        public PopController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Pop_List)]
        public ActionResult Index()
        {
            List<Pop> lstPop = db.Pop.ToList();
            return View(lstPop);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Pop)]
        public ActionResult InsertPop()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertPop(Pop Pop_Client)
        {
            Pop Pop_Check = db.Pop.Where(s => s.PopName == Pop_Client.PopName.Trim()).FirstOrDefault();

            if (Pop_Check != null)
            {
                TempData["AlreadyInsert"] = "Pop Already Added. Choose different Pop. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Pop Pop_Return = new Pop();

            try
            {
                Pop_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Pop_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Pop_Return = db.Pop.Add(Pop_Client);
                db.SaveChanges();

                if (Pop_Return.PopID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Pop = Pop_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertPopFromPopUp(Pop Pop_Client)
        {
            Pop Pop_Check = db.Pop.Where(s => s.PopName == Pop_Client.PopName.Trim()).FirstOrDefault();

            if (Pop_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Pop Already Added. Choose different Pop. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Pop Pop_Return = new Pop();

            try
            {
                Pop_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Pop_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Pop_Return = db.Pop.Add(Pop_Client);
                db.SaveChanges();

                if (Pop_Return.PopID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Pop = Pop_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetPopDetailsByID(int PopID)
        {
            var Pop = db.Pop.Where(s => s.PopID == PopID).Select(s => new { PopName = s.PopName, PopLocations = s.PopLocation, LatitudeLongitude = s.LatitudeLongitude }).FirstOrDefault();


            var JSON = Json(new { PopDetails = Pop }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatePop(Pop PopInfoForUpdate)
        {

            try
            {

                Pop Pop_Check = db.Pop.Where(s => s.PopID != PopInfoForUpdate.PopID && s.PopName == PopInfoForUpdate.PopName.Trim()).FirstOrDefault();

                if (Pop_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Pop Already Added. Choose different Pop. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Pop_db = db.Pop.Where(s => s.PopID == PopInfoForUpdate.PopID);
                PopInfoForUpdate.CreatedBy = Pop_db.FirstOrDefault().CreatedBy;
                PopInfoForUpdate.CreatedDate = Pop_db.FirstOrDefault().CreatedDate;
                PopInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                PopInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Pop_db.SingleOrDefault()).CurrentValues.SetValues(PopInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Pop_Return = Pop_db.Select(s => new { PopID = s.PopID, PackageName = s.PopName, PopLocation = s.PopLocation });
                var JSON = Json(new { UpdateSuccess = true, PopUpdateInformation = Pop_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, PopUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}