using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_ISP.ViewModel;
using ISP_ManagementSystemModel;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class AttedanceController : Controller
    {

        private ISPContext db = new ISPContext();
        // GET: Attedance
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.VIew_Attendance)]
        public ActionResult AttendanceTypeIndex()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllPackageAJAXData()
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var AttendanceType = db.AttendanceTypes.Where(t=>t.Status==AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<AttendanceTypeViewModel> data = new List<AttendanceTypeViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (AttendanceType.Any()) ? AttendanceType.Where(p => p.AttendanceTypeID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.AttendanceTypeID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.AttendanceName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.AttendanceName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    AttendanceType = AttendanceType.Where(p =>
                    p.AttendanceTypeID.ToString().ToLower().Contains(search.ToLower())
                    || p.AttendanceTypeID.ToString().ToLower().Contains(search.ToLower())
                    || p.AttendanceName.ToString().ToLower().Contains(search.ToLower())
                    || p.AttendanceName.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = AttendanceType.Any() ? AttendanceType.AsEnumerable().Skip(startRec).Take(pageSize) 
                        .Select(
                            x => new AttendanceTypeViewModel
                            {
                                AttendanceTypeID = x.AttendanceTypeID,
                                AttendanceName = x.AttendanceName,
                                UpdateAttendanceType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Attendance_Type) ? true : false
                            })
                        .ToList() : new List<AttendanceTypeViewModel>();

                data = this.SortByColumnWithOrder(order, orderDir, data);
                int totalRecords = AttendanceType.AsEnumerable().Count();
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : AttendanceType.AsEnumerable().Count();

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }  
            return result;
        }


        private List<AttendanceTypeViewModel> SortByColumnWithOrder(string order, string orderDir, List<AttendanceTypeViewModel> data)
        {
            // Initialization.   
            List<AttendanceTypeViewModel> lst = new List<AttendanceTypeViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AttendanceName).ToList() : data.OrderBy(p => p.AttendanceName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AttendanceName).ToList() : data.OrderBy(p => p.AttendanceName).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAttendanceTypeID(int AttendanceTypeID)
        {
            var AttendanceType = db.AttendanceTypes.Where(s => s.AttendanceTypeID == AttendanceTypeID).Select(s => new { AttendanceTypeID = s.AttendanceTypeID, AttendanceName = s.AttendanceName }).FirstOrDefault();


            var JSON = Json(new { AttendanceType = AttendanceType }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateAttendanceType(AttendanceType type)
        {

            try
            {
                AttendanceType dbAttendanceType = new AttendanceType();
                dbAttendanceType = db.AttendanceTypes.Find(type.AttendanceTypeID);
                dbAttendanceType.AttendanceName = type.AttendanceName;
                dbAttendanceType.UpdateBy = AppUtils.GetLoginUserID();
                dbAttendanceType.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbAttendanceType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var AttendacnceType =
                    new AttendanceTypeViewModel()
                    {
                        AttendanceTypeID = type.AttendanceTypeID,
                        AttendanceName = type.AttendanceName,
                        UpdateAttendanceType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Attendance_Type) ? true : false,
                    };
                var JSON = Json(new { UpdateSuccess = true, AttenDanceTypeInformation = AttendacnceType }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, EMPLH = "" }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttendanceType(int AttendanceTypeID)
        {
            AttendanceType attendanceType = new AttendanceType();
            attendanceType = db.AttendanceTypes.Find(AttendanceTypeID);
            attendanceType.DeleteBy = AppUtils.GetLoginUserID();
            attendanceType.DeleteDate = AppUtils.GetDateTimeNow();
            attendanceType.Status = AppUtils.TableStatusIsDelete;


            db.Entry(attendanceType).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { DeleteSuccess = true, AttendanceTypeID = attendanceType.AttendanceTypeID }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertAttendanceType(AttendanceType type)
        {
            try
            {
                db.AttendanceTypes.Add(type);
                type.CreateBy = AppUtils.GetLoginUserID();
                type.CreateDate = AppUtils.GetDateTimeNow();
                type.Status = AppUtils.TableStatusIsActive;
                db.AttendanceTypes.Add(type);
                db.SaveChanges();
                AttendanceTypeViewModel AttendanceTypeInfo = new AttendanceTypeViewModel
                {
                    AttendanceTypeID = type.AttendanceTypeID,
                    AttendanceName = type.AttendanceName,
                    UpdateAttendanceType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Attendance_Type) ? true : false,
                };


                return Json(new { SuccessInsert = true, AttendanceTypeInfo = AttendanceTypeInfo }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.VIew_Attendance)]

        public ActionResult AttendanceInOuts()
        {
            ViewBag.InsertAttenDanceType = new SelectList(db.AttendanceTypes.Where(s=>s.Status==AppUtils.TableStatusIsActive), "AttendanceTypeID", "AttendanceName");
            ViewBag.UpdateAttenDanceType = new SelectList(db.AttendanceTypes.Where(s => s.Status == AppUtils.TableStatusIsActive), "AttendanceTypeID", "AttendanceName");
            var AIO = db.AtendaceInOuts.Include(t => t.AttendanceType).Where(a => a.Status == 1).ToList();
            return View(AIO);
        }
        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult InsertAttendanceInOut(AttendanceInOutViewModel atendaceInOut)
        {
            try
            {
                AtendaceInOut at = new AtendaceInOut();
                SetAtendanceInOutInDbModelFromViewModel(ref atendaceInOut, ref at);
                db.AtendaceInOuts.Add(at);
                db.SaveChanges();
                AttendanceInOutViewModel atvm = new AttendanceInOutViewModel();
                SetAtendanceInOutInViewModelFromDbModel(ref at, ref atvm);
                return Json(new { AtendanceINOUT = at, success = true }, JsonRequestBehavior.AllowGet); ;
            }
            catch(Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet); ;
            }
            
        }

        private void SetAtendanceInOutInViewModelFromDbModel(ref AtendaceInOut at, ref AttendanceInOutViewModel atvm)
        {
            atvm.Title = at.Title;
            atvm.start = at.start;
            atvm.end = at.end;
            atvm.InsalaryCut = at.InSalaryCut;
            atvm.OutSalaryCut = at.OutSalaryCut;
        }

        private void SetAtendanceInOutInDbModelFromViewModel(ref AttendanceInOutViewModel atendaceInOut, ref AtendaceInOut at)
        {
            at.Title = atendaceInOut.Title;
            at.start = atendaceInOut.start;
            at.end = atendaceInOut.end;
            at.InSalaryCut = atendaceInOut.InsalaryCut;
            at.OutSalaryCut = atendaceInOut.OutSalaryCut;
            at.AttendanceTypeID = atendaceInOut.typeId;
            at.Status = 1;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAttendanceInOut(int id)
        {
            var attendanceInOut = db.AtendaceInOuts.Find(id);
            return Json(new { attendanceInOut = attendanceInOut, success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateAttendanceInOut(AttendanceInOutViewModel attendaceInOut)
        {

            AtendaceInOut atendaceInOut = new AtendaceInOut();
            atendaceInOut = db.AtendaceInOuts.Find(attendaceInOut.id);
            SetAtendanceInOutInDbModel(ref attendaceInOut, ref atendaceInOut);
            var a = attendaceInOut.typeId;
            db.Entry(atendaceInOut).State = EntityState.Modified;
            db.SaveChanges();
            AttendanceInOutViewModel AtendaceinOut = new AttendanceInOutViewModel();
            SetAtendanceInOutInViewModel(ref atendaceInOut, ref AtendaceinOut);

            AtendaceinOut.typeId = a;
            return Json(new { AtendaceInOut = AtendaceinOut, success = true }, JsonRequestBehavior.AllowGet);

        }
        private void SetAtendanceInOutInViewModel(ref AtendaceInOut atendaceInOut, ref AttendanceInOutViewModel AtendaceinOut)
        {
            AtendaceinOut.Title = atendaceInOut.Title;
            AtendaceinOut.start = atendaceInOut.start;
            AtendaceinOut.end = atendaceInOut.end;
            AtendaceinOut.InsalaryCut = atendaceInOut.InSalaryCut;
            AtendaceinOut.OutSalaryCut = atendaceInOut.OutSalaryCut;
            AtendaceinOut.AtendanceType = atendaceInOut.AttendanceTypeID;
        }

        private void SetAtendanceInOutInDbModel(ref AttendanceInOutViewModel attendaceInOut, ref AtendaceInOut atendaceInOut)
        {
            atendaceInOut.Title = attendaceInOut.Title;
            atendaceInOut.start = attendaceInOut.start;
            atendaceInOut.end = attendaceInOut.end;
            atendaceInOut.InSalaryCut = attendaceInOut.InsalaryCut;
            atendaceInOut.OutSalaryCut = attendaceInOut.OutSalaryCut;
            atendaceInOut.AttendanceTypeID = attendaceInOut.AtendanceType;
            atendaceInOut.Status = 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttendanceInOut(int id)
        {
            AtendaceInOut at = new AtendaceInOut();
            at = db.AtendaceInOuts.Find(id);
            return Json(new { attendanceInOut = at, success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttendanceInOutConfirm(int id)
        {
            AtendaceInOut at = new AtendaceInOut();
            at = db.AtendaceInOuts.Find(id);
            at.Title = at.Title;
            at.start = at.start;
            at.end = at.end;
            at.InSalaryCut = at.InSalaryCut;
            at.OutSalaryCut = at.OutSalaryCut;
            at.AttendanceTypeID = at.AttendanceTypeID;
            at.Status = 3;
            db.Entry(at).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { at = at, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}