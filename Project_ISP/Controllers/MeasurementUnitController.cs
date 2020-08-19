using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class MeasurementUnitController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: MeasurementUnit
        [HttpGet]
        [UserRIghtCheck(ControllerValue =AppUtils.View_Measurement_Unit)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllMeasurementUnit()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var measurementUnit = db.MeasurementUnits.Where(x=>x.Status==AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<CustomMeasuremetUnit> data = new List<CustomMeasuremetUnit>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (measurementUnit.Any()) ? measurementUnit.Where(p => p.MeasurementUnitID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.MeasurementUnitID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.UnitName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.UnitName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    measurementUnit = measurementUnit.Where(p =>
                    p.MeasurementUnitID.ToString().ToLower().Contains(search.ToLower())
                    || p.MeasurementUnitID.ToString().ToLower().Contains(search.ToLower())
                    || p.UnitName.ToString().ToLower().Contains(search.ToLower())
                    || p.UnitName.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = measurementUnit.Any() ? measurementUnit.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new CustomMeasuremetUnit
                            {
                                MeasurementUnitID = x.MeasurementUnitID,
                                UnitName = x.UnitName,
                                UpdateMeasurementUnit = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Measurement_Unit) ? true : false
                            })
                        .ToList() : new List<CustomMeasuremetUnit>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = measurementUnit.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : measurementUnit.AsEnumerable().Count();

                ////////////////////////////////////


                // Loading drop down lists.   
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
            // Return info.   
            return result;
        }


        private List<CustomMeasuremetUnit> SortByColumnWithOrder(string order, string orderDir, List<CustomMeasuremetUnit> data)
        {
            // Initialization.   
            List<CustomMeasuremetUnit> lst = new List<CustomMeasuremetUnit>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MeasurementUnitID).ToList() : data.OrderBy(p => p.MeasurementUnitID).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UnitName).ToList() : data.OrderBy(p => p.UnitName).ToList();
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
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertMeasurementUnit(MeasurementUnits measureUnit)
        {
            try
            {
                db.MeasurementUnits.Add(measureUnit);
                measureUnit.CreateBy = AppUtils.GetLoginUserID();
                measureUnit.CreateDate = AppUtils.GetDateTimeNow();
                measureUnit.Status = AppUtils.TableStatusIsActive;
                db.SaveChanges();
                CustomMeasuremetUnit UnitInfo = new CustomMeasuremetUnit
                {
                    MeasurementUnitID = measureUnit.MeasurementUnitID,
                    UnitName = measureUnit.UnitName,
                    UpdateMeasurementUnit = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Measurement_Unit) ? true : false,
                };


                return Json(new { SuccessInsert = true, UnitInfo = UnitInfo }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMeasurementUnitID(int MeasurementUnitID)
        {
            var measureUnit = db.MeasurementUnits.Where(s => s.MeasurementUnitID == MeasurementUnitID).Select(s => new { MeasurementUnitID = s.MeasurementUnitID, UnitName = s.UnitName }).FirstOrDefault();


            var JSON = Json(new { measureUnit = measureUnit }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateMeasurementUnit(MeasurementUnits UNitDetails)
        {

            try
            {
                MeasurementUnits measurement = new MeasurementUnits();
                measurement = db.MeasurementUnits.Find(UNitDetails.MeasurementUnitID);
                measurement.UpdateBy = AppUtils.GetLoginUserID();
                measurement.UpdateDate = AppUtils.GetDateTimeNow();
                measurement.UnitName = UNitDetails.UnitName;
                db.Entry(measurement).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var measurementUnit =
                    new CustomMeasuremetUnit()
                    {
                        MeasurementUnitID = UNitDetails.MeasurementUnitID,
                        UnitName = UNitDetails.UnitName,
                        UpdateMeasurementUnit = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Attendance_Type) ? true : false,
                    };
                var JSON = Json(new { UpdateSuccess = true, Units = measurementUnit }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch(Exception ex)
            {
                return Json(new { UpdateSuccess = false, EMPLH = "" }, JsonRequestBehavior.AllowGet);

            }

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMeasurement(int MeasurementUnitID)
        {
            MeasurementUnits measureUnit = new MeasurementUnits();
            measureUnit = db.MeasurementUnits.Find(MeasurementUnitID);
            measureUnit.DeleteBy = AppUtils.GetLoginUserID();
            measureUnit.DeleteDate = AppUtils.GetDateTimeNow();
            measureUnit.Status = AppUtils.TableStatusIsDelete;


            db.Entry(measureUnit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { DeleteSuccess = true,measureUnitID= measureUnit.MeasurementUnitID }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

    }
}