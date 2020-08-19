using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_ISP.Controllers
{
    public class TimePeriodForSignalController : Controller
    {

        public TimePeriodForSignalController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_TimePeriodForSignal_List)]
        public ActionResult Index()
        {
            //List<TimePeriodForSignal> lstTimePeriodForSignal = db.TimePeriodForSignal.ToList();
            //var lstTimePeriodForSignal = new SelectList(db.TimePeriodForSignal.Select(s => new { TimePeriodForSignalID = s.TimePeriodForSignalID, TimePeriodForSignalName = s.TimePeriodForSignalName }), "TimePeriodForSignalID", "TimePeriodForSignalName");
            //ViewBag.SearchByTimePeriodForSignalID = lstTimePeriodForSignal;
            //ViewBag.lstTimePeriodForSignalUpdate = lstTimePeriodForSignal;
            //ViewBag.lstTimePeriodForSignal = lstTimePeriodForSignal;
            return View(new List<TimePeriodForSignal>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTimePeriodForSignalAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                int TimePeriodForSignalIDFromDDL = 0;
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                //string TimePeriodForSignalID = Request.Form.Get("TimePeriodForSignalID");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                //if (TimePeriodForSignalID != "")
                //{
                //    TimePeriodForSignalIDFromDDL = int.Parse(TimePeriodForSignalID);
                //}
                // Loading.   

                // Apply pagination.   
                //data = data.Skip(startRec).Take(pageSize).ToList();
                var firstPart = db.TimePeriodForSignal.AsEnumerable();

                var TimePeriodForSignalList = new List<TimePeriodForSignal>();


                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (firstPart.Any()) ? firstPart.Where(p => p.UpToHours.ToString().Contains(search.ToLower())
                                                                     || p.SignalSign.ToString().ToLower().Contains(search.ToLower())

                    ).Count() : 0;

                    // Apply search   
                    firstPart = firstPart.Where(p => p.UpToHours.ToString().Contains(search.ToLower())
                                                                     || p.SignalSign.ToString().ToLower().Contains(search.ToLower())

                    ).AsEnumerable();
                }


                List<TimePeriodForSignalCustomList> data =
                        firstPart.Any() ? (from fp in firstPart.Skip(startRec).Take(pageSize)

                                           select new TimePeriodForSignalCustomList
                                           {
                                               TimePeriodForSignalID = fp.TimePeriodForSignalID,
                                               UpToHours = fp.UpToHours,
                                               SignalSign = fp.SignalSign,
                                               SignalSignString =
                                                 fp.SignalSign == 1 ? "<div id='Status' class='label label-success'>Green</div>"
                                               : fp.SignalSign == 2 ? "<div id='Status' class='label label-warning'>Yellow</div>"
                                               : "<div id='Status' class='label label-danger'>Red</div>",
                                               Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_TimePeriodForSignal) ? true : false,
                                           }).ToList() : new List<TimePeriodForSignalCustomList>();
                // Verification.   

                // Sorting.   
                finalItem = SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                totalRecords = firstPart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                                                                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPart.AsEnumerable().Count();

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

        private IEnumerable<dynamic> SortByColumnWithOrder(string order, string orderDir, IEnumerable<dynamic> finalItem)
        {
            // Initialization.   
            List<dynamic> lst = new List<dynamic>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.TimePeriodForSignalID).ToList() : finalItem.OrderBy(p => p.TimePeriodForSignalID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.StartTime).ToList() : finalItem.OrderBy(p => p.StartTime).ToList();
                        break;
                    //case "2":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.SignalSign).ToList() : finalItem.OrderBy(p => p.SignalSign).ToList();
                    //    break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.TimePeriodForSignalID).ToList() : finalItem.OrderBy(p => p.TimePeriodForSignalID).ToList();
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


        //[HttpGet]

        //[UserRIghtCheck(ControllerValue = AppUtils.Add_TimePeriodForSignal)]
        //public ActionResult InsertTimePeriodForSignal()
        //{
        //    ViewBag.lstTimePeriodForSignal = new SelectList(db.TimePeriodForSignal.Select(s => new { TimePeriodForSignalID = s.TimePeriodForSignalID, TimePeriodForSignalName = s.TimePeriodForSignalName }), "TimePeriodForSignalID", "TimePeriodForSignalName");

        //    return View();
        //}


        [HttpPost]
        public ActionResult InsertTimePeriodForSignal(TimePeriodForSignal TimePeriodForSignal_Client)
        {
            TimePeriodForSignal TimePeriodForSignal_Check = db.TimePeriodForSignal.Where(s => s.SignalSign == TimePeriodForSignal_Client.SignalSign).FirstOrDefault();

            if (TimePeriodForSignal_Check != null)
            {
                TempData["AlreadyInsert"] = "Time Period For Signal Already Added. Choose different TimePeriodForSignal. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            TimePeriodForSignal TimePeriodForSignal_Return = new TimePeriodForSignal();

            try
            {
                TimePeriodForSignal_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                TimePeriodForSignal_Client.CreatedDate = AppUtils.GetDateTimeNow();

                TimePeriodForSignal_Return = db.TimePeriodForSignal.Add(TimePeriodForSignal_Client);
                db.SaveChanges();

                if (TimePeriodForSignal_Return.TimePeriodForSignalID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, TimePeriodForSignal = TimePeriodForSignal_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertTimePeriodForSignalFromPopUp(TimePeriodForSignal TimePeriodForSignal_Client)
        {

            TimePeriodForSignal TimePeriodForSignal_Check = db.TimePeriodForSignal.Where(s => s.SignalSign == TimePeriodForSignal_Client.SignalSign).FirstOrDefault();

            if (TimePeriodForSignal_Check != null)
            {
                TempData["AlreadyInsert"] = "Time Period For Signal Already Added. Choose different TimePeriodForSignal. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }


            TimePeriodForSignal TimePeriodForSignal_Return = new TimePeriodForSignal();

            try
            {
                TimePeriodForSignal_Client.UpToHours = TimePeriodForSignal_Client.UpToHours;
                TimePeriodForSignal_Client.SignalSign = TimePeriodForSignal_Client.SignalSign;
                TimePeriodForSignal_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                TimePeriodForSignal_Client.CreatedDate = AppUtils.GetDateTimeNow();

                TimePeriodForSignal_Return = db.TimePeriodForSignal.Add(TimePeriodForSignal_Client);
                db.SaveChanges();

                if (TimePeriodForSignal_Return.TimePeriodForSignalID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, TimePeriodForSignal = TimePeriodForSignal_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetTimePeriodForSignalDetailsByID(int TimePeriodForSignalID)
        {
            var TimePeriodForSignal = db.TimePeriodForSignal.Where(s => s.TimePeriodForSignalID == TimePeriodForSignalID).Select(s => new
            {
                TimePeriodForSignalID = s.TimePeriodForSignalID,
                UpToHours = s.UpToHours,
                SignalSign = s.SignalSign
            }).FirstOrDefault();


            var JSON = Json(new { TimePeriodForSignalDetails = TimePeriodForSignal }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateTimePeriodForSignal(TimePeriodForSignal TimePeriodForSignalInfoForUpdate)
        {

            try
            {

                TimePeriodForSignal TimePeriodForSignal_Check = db.TimePeriodForSignal.Where(s => s.TimePeriodForSignalID == TimePeriodForSignalInfoForUpdate.TimePeriodForSignalID).FirstOrDefault();

                if (TimePeriodForSignal_Check == null)
                {
                    //TempData["AlreadyInsert"] = "TimePeriodForSignal Already Added. Choose different TimePeriodForSignal. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                TimePeriodForSignal TimePeriodForSignal_Check_IfDuplicateDataExist = db.TimePeriodForSignal.Where(s => s.TimePeriodForSignalID != TimePeriodForSignalInfoForUpdate.TimePeriodForSignalID && s.SignalSign == TimePeriodForSignalInfoForUpdate.SignalSign).FirstOrDefault();

                if (TimePeriodForSignal_Check_IfDuplicateDataExist != null)
                {
                    //TempData["AlreadyInsert"] = "TimePeriodForSignal Already Added. Choose different TimePeriodForSignal. ";

                    return Json(new { UpdateSuccess = false, DuplicateSignExist = true }, JsonRequestBehavior.AllowGet);
                }

                var TimePeriodForSignal_db = db.TimePeriodForSignal.Where(s => s.TimePeriodForSignalID == TimePeriodForSignalInfoForUpdate.TimePeriodForSignalID);
                TimePeriodForSignalInfoForUpdate.CreatedBy = TimePeriodForSignal_db.FirstOrDefault().CreatedBy;
                TimePeriodForSignalInfoForUpdate.CreatedDate = TimePeriodForSignal_db.FirstOrDefault().CreatedDate;
                TimePeriodForSignalInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                TimePeriodForSignalInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(TimePeriodForSignal_db.SingleOrDefault()).CurrentValues.SetValues(TimePeriodForSignalInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var temp = TimePeriodForSignal_db.ToList();
                var TimePeriodForSignal_Return = TimePeriodForSignal_db.Select(s =>
                new
                {
                    TimePeriodForSignalID = s.TimePeriodForSignalID,
                    UpToHours = s.UpToHours,
                    SignalSign = s.SignalSign
                });
                var JSON = Json(new { UpdateSuccess = true, TimePeriodForSignalUpdateInformation = TimePeriodForSignal_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, TimePeriodForSignalUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_TimePeriodForSignal)]
        public ActionResult DeleteTimePeriodForSignal(int TimePeriodForSignalID)
        {
            try
            {
                TimePeriodForSignal TimePeriodForSignal = db.TimePeriodForSignal.Find(TimePeriodForSignalID);
                db.TimePeriodForSignal.Remove(TimePeriodForSignal);
                db.SaveChanges();
                return Json(new { DeleteStatus = true, TimePeriodForSignalID = TimePeriodForSignalID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, TimePeriodForSignalID = TimePeriodForSignalID }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}