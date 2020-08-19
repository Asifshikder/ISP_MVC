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
    public class SectionController : Controller
    {
        public SectionController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Section_List)]
        public ActionResult Index()
        {
            List<Section> lstSection = db.Section.ToList();
            return View(lstSection);
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Section)]
        public ActionResult InsertSection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertSection(Section Section_Client)
        {
            Section Section_Check = db.Section.Where(s => s.SectionName == Section_Client.SectionName.Trim()).FirstOrDefault();

            if (Section_Check != null)
            {
                TempData["AlreadyInsert"] = "Section Already Added. Choose different Section. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Section Section_Return = new Section();

            try
            {
                Section_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Section_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Section_Return = db.Section.Add(Section_Client);
                db.SaveChanges();

                if (Section_Return.SectionID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Section = Section_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertSectionFromPopUp(Section Section_Client)
        {
            Section Section_Check = db.Section.Where(s => s.SectionName == Section_Client.SectionName.Trim()).FirstOrDefault();

            if (Section_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Section Already Added. Choose different Section. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Section Section_Return = new Section();

            try
            {
                Section_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Section_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Section_Return = db.Section.Add(Section_Client);
                db.SaveChanges();

                if (Section_Return.SectionID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Section = Section_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetSectionDetailsByID(int SectionID)
        {
            var Section = db.Section.Where(s => s.SectionID == SectionID).Select(s => new { SectionName = s.SectionName }).FirstOrDefault();


            var JSON = Json(new { SectionDetails = Section }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateSection(Section SectionInfoForUpdate)
        {

            try
            {

                Section Section_Check = db.Section.Where(s => s.SectionID != SectionInfoForUpdate.SectionID && s.SectionName == SectionInfoForUpdate.SectionName.Trim()).FirstOrDefault();

                if (Section_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Section Already Added. Choose different Section. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Section_db = db.Section.Where(s => s.SectionID == SectionInfoForUpdate.SectionID);
                SectionInfoForUpdate.CreatedBy = Section_db.FirstOrDefault().CreatedBy;
                SectionInfoForUpdate.CreatedDate = Section_db.FirstOrDefault().CreatedDate;
                SectionInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                SectionInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Section_db.SingleOrDefault()).CurrentValues.SetValues(SectionInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Section_Return = Section_db.Select(s => new { SectionID = s.SectionID, PackageName = s.SectionName });
                var JSON = Json(new { UpdateSuccess = true, SectionUpdateInformation = Section_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, SectionUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}