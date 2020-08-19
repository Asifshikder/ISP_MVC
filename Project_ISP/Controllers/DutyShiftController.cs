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

namespace Project_ISP.Controllers
{
    public class DutyShiftController : Controller
    {
        private ISPContext db = new ISPContext();
        public ActionResult Index()
        {
            List<SelectListItem> lstHour = new List<SelectListItem>();
            List<SelectListItem> lstMin = new List<SelectListItem>();

            for (int i = 0; i <= 23; i++)
            {
                lstHour.Add(new SelectListItem() { Text = "" + i + "", Value = "" + i + "" });
            }
            for (int i = 0; i <= 59; i++)
            {
                lstMin.Add(new SelectListItem() { Text = "" + i + "", Value = "" + i + "" });
            }



            SetTimeScheduleInDropDownViewBag(lstHour, lstMin);
            List<DutyShift> DutyShifts = new List<DutyShift>();
            DutyShifts = db.DutyShifts.Where(s => s.TableStatusID == 1).ToList();

            return View(DutyShifts);
        }

        private void SetTimeScheduleInDropDownViewBag(List<SelectListItem> lstHour, List<SelectListItem> lstMin)
        {

            ViewBag.StartHour = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartMinute = new SelectList(lstMin, "Value", "Text");
            ViewBag.EndHour = new SelectList(lstHour, "Value", "Text");
            ViewBag.EndMinute = new SelectList(lstMin, "Value", "Text");
            ViewBag.StartHourUpdate = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartMinuteUpdate = new SelectList(lstMin, "Value", "Text");
            ViewBag.EndHourUpdate = new SelectList(lstHour, "Value", "Text");
            ViewBag.EndMinuteUpdate = new SelectList(lstMin, "Value", "Text");

        }
        // DutyDhiftID
        [HttpPost]
        public ActionResult GetDutyShiftDetailsByID(int DutyShiftID)
        {
            var dutyShift = db.DutyShifts.Where(s => s.DutyShiftID == DutyShiftID).Select(s => new { DutyShiftID = s.DutyShiftID, StartHour = s.StartHour, StartMinute = s.StartMinute, EndHour = s.EndHour, EndMinute = s.EndMinute }).FirstOrDefault();


            var JSON = Json(new { success = true, DutyShiftDetails = dutyShift }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateDutyShift(DutyShift DutyShiftForUpdate)
        {


            try
            {
                DutyShift dutyShift = new DutyShift();
                dutyShift = db.DutyShifts.Find(DutyShiftForUpdate.DutyShiftID);
                dutyShift.UpdateBy = AppUtils.GetLoginUserID();
                dutyShift.UpdateTime = AppUtils.GetDateTimeNow();
                dutyShift.StartHour = DutyShiftForUpdate.StartHour;
                dutyShift.StartMinute = DutyShiftForUpdate.StartMinute;
                dutyShift.EndHour = DutyShiftForUpdate.EndHour;
                dutyShift.EndMinute = DutyShiftForUpdate.EndMinute;

                db.Entry(dutyShift).State = EntityState.Modified;
                db.SaveChanges();

                DutyShiftViewModel Shift = new DutyShiftViewModel();
                Shift.DutyShiftID = dutyShift.DutyShiftID;
                Shift.StartHour = dutyShift.StartHour;
                Shift.StartMinute = dutyShift.StartMinute;
                Shift.EndMinute = dutyShift.EndMinute;
                Shift.EndHour = dutyShift.EndHour;


                return Json(new { UpdateSuccess = true, Shift = Shift }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { UpdateSuccess = false, EMPLH = "" }, JsonRequestBehavior.AllowGet);

            }

        }



        [HttpPost]
        public JsonResult InsertDutyShift(DutyShift ShiftForInsert)
        {

            try
            {
                ShiftForInsert.CreatedBy = AppUtils.GetLoginUserID();
                ShiftForInsert.CreateDate = AppUtils.GetDateTimeNow();
                ShiftForInsert.TableStatusID = 1;
                db.DutyShifts.Add(ShiftForInsert);
                db.SaveChanges();

                DutyShiftViewModel ShiftInfo = new DutyShiftViewModel
                {
                    DutyShiftID = ShiftForInsert.DutyShiftID,
                    StartHour = ShiftForInsert.StartHour,
                    StartMinute = ShiftForInsert.StartMinute,
                    EndHour = ShiftForInsert.EndHour,
                    EndMinute = ShiftForInsert.EndMinute

                };


                return Json(new { SuccessInsert = true, ShiftInformation = ShiftInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteDutyShift(int id)
        {


            try
            {
                DutyShift dutyShift = new DutyShift();
                dutyShift = db.DutyShifts.Find(id);
                dutyShift.DeleteBy = AppUtils.GetLoginUserID();
                dutyShift.DeleteDate = AppUtils.GetDateTimeNow();
                dutyShift.TableStatusID = 3;
                db.Entry(dutyShift).State = EntityState.Modified;
                db.SaveChanges();

                var JSON = Json(new { DeleteSuccess = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { DeleteSuccess = false }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}