using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Custom_Model;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{

    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class LeaveSalaryController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: LeaveSalary
        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.View_LeaveSalary)]
        public ActionResult LeaveSalaryType()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllLeaveTypeData()
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

                var leaveTypes = db.LeaveSallaryTypes.AsEnumerable();
                int ifSearch = 0;
                List<LeaveSallaryType> datas = leaveTypes.Any() ? leaveTypes.Skip(startRec).Take(pageSize).AsEnumerable()
                    .Select(
                     s => new LeaveSallaryType
                     {
                         LeaveTypeId = s.LeaveTypeId,
                         LeaveTypeName = s.LeaveTypeName,
                         Persent = s.Persent,

                     }).ToList() : new List<LeaveSallaryType>();

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (datas.Any()) ? datas.Where(p => p.LeaveTypeId.ToString().ToLower().Contains(search.ToLower()) || p.LeaveTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    datas = datas.Where(p => p.LeaveTypeId.ToString().ToLower().Contains(search.ToLower()) || p.LeaveTypeName.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                datas = this.SortByColumnWithOrder(order, orderDir, datas);
                int totalRecords = leaveTypes.AsEnumerable().Count();
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : leaveTypes.AsEnumerable().Count();

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordTotal = totalRecords,
                    recordFiltered = recFilter,
                    data = datas
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return result;
        }

        private List<LeaveSallaryType> SortByColumnWithOrder(string order, string orderDir, List<LeaveSallaryType> datas)
        {
            // Initialization.   
            List<LeaveSallaryType> lst = new List<LeaveSallaryType>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? datas.OrderByDescending(p => p.LeaveTypeId).ToList() : datas.OrderBy(p => p.LeaveTypeId).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? datas.OrderByDescending(p => p.LeaveTypeName).ToList() : datas.OrderBy(p => p.LeaveTypeName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? datas.OrderByDescending(p => p.Persent).ToList() : datas.OrderBy(p => p.Persent).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? datas.OrderByDescending(p => p.LeaveTypeId).ToList() : datas.OrderBy(p => p.LeaveTypeId).ToList();
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
        public JsonResult InsertLeaveType(LeaveSallaryType leaveType)
        {
            int LeaveTypeCounts = db.LeaveSallaryTypes.Count();

            try
            {
                leaveType.TableStatusID = 1;
                db.LeaveSallaryTypes.Add(leaveType);
                db.SaveChanges();

                LeaveSallaryType TypeInfo = new LeaveSallaryType
                {
                    LeaveTypeId = leaveType.LeaveTypeId,
                    LeaveTypeName = leaveType.LeaveTypeName,
                    Persent = leaveType.Persent,
                };


                return Json(new { SuccessInsert = true, TypeInfos = TypeInfo, TypeCount = LeaveTypeCounts }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTypeDetailsByID(int TypeID)
        {
            var leaveTypeDetails = db.LeaveSallaryTypes.Where(s => s.LeaveTypeId == TypeID).Select(s => new { LeaveTypeId = s.LeaveTypeId, LeaveTypeName = s.LeaveTypeName, Persent = s.Persent }).FirstOrDefault();

            var JSON = Json(new { leaveTypeDetails = leaveTypeDetails }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateType(LeaveSallaryType TypeInfoForUpdate)
        {
            try
            {
                var Type = db.LeaveSallaryTypes.Where(s => s.LeaveTypeId == TypeInfoForUpdate.LeaveTypeId);

                TypeInfoForUpdate.TableStatusID = 1;
                db.Entry(Type.SingleOrDefault()).CurrentValues.SetValues(TypeInfoForUpdate);
                db.SaveChanges();

                var type =
                    new LeaveSallaryType()
                    {
                        LeaveTypeId = TypeInfoForUpdate.LeaveTypeId,
                        LeaveTypeName = TypeInfoForUpdate.LeaveTypeName,
                        Persent = TypeInfoForUpdate.Persent,
                    };
                var JSON = Json(new { UpdateSuccess = true, TypeUpdateInformation = type }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, EMPLH = "" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpGet]
        public ActionResult EmployeeLeaveHistoryIndex()
        {
            ViewBag.EmployeeIDS = new SelectList(db.Employee, "EmployeeID", "Name");
            ViewBag.AddEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name");
            ViewBag.AddLeaveType = new SelectList(db.LeaveSallaryTypes, "LeaveTypeId", "LeaveTypeName");
            ViewBag.EditEmployeeID = new SelectList(db.Employee, "EmployeeID", "Name");
            ViewBag.EditLeaveType = new SelectList(db.LeaveSallaryTypes, "LeaveTypeId", "LeaveTypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllLeaveHistoryAJAXData()
        {

            JsonResult result = new JsonResult();
            try
            {
                int employeeFromDDL = 0;
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var EmployeeID = Request.Form.Get("EmployeeIDS");
                var StartDateID = Request.Form.Get("StartDateID");
                var EndDateID = Request.Form.Get("EndDateID");
                DateTime? startDate = new DateTime?();
                DateTime? endDate = new DateTime?();
                int totalRecords = 0;
                if (!string.IsNullOrEmpty(StartDateID))
                {
                    startDate = Convert.ToDateTime(StartDateID);
                }

                if (!string.IsNullOrEmpty(EndDateID))
                {
                    endDate = DateTime.Parse(EndDateID);
                }

                if (!string.IsNullOrEmpty(EmployeeID))
                {
                    employeeFromDDL = int.Parse(EmployeeID);
                }


                var leaveHistory = db.EmployeeLeaveHistories.Where(a => a.Status == AppUtils.TableStatusIsActive).AsEnumerable();

                int ifSearch = 0;
                List<EmployeeLeaveViewModel> data = new List<EmployeeLeaveViewModel>();
                var firstPartOfQuery =
                     (StartDateID != "" && EndDateID != "" && EmployeeID != "") ? leaveHistory.Where(s => s.StartDate >= startDate && s.EndDate <= endDate && s.EmployeeID == employeeFromDDL).AsQueryable()
                         : (StartDateID != "" && EndDateID != "" && EmployeeID == "") ? leaveHistory.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).AsQueryable()
                             : (StartDateID != "" && EndDateID == "" && EmployeeID != "") ? leaveHistory.Where(s => s.StartDate >= startDate && s.EmployeeID == employeeFromDDL).AsQueryable()
                                 : (StartDateID != "" && EndDateID == "" && EmployeeID == "") ? leaveHistory.Where(s => s.StartDate >= startDate).AsQueryable()
                                  : (StartDateID == "" && EndDateID == "" && !string.IsNullOrEmpty(EmployeeID)) ? leaveHistory.Where(s => s.EmployeeID == employeeFromDDL).AsQueryable()
                                         : (StartDateID == "" && EndDateID != "" && EmployeeID == "") ? leaveHistory.Where(s => s.EndDate <= endDate).AsQueryable()
                                         : leaveHistory.AsQueryable();



                var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.EmployeeID.ToString().ToLower().Contains(search.ToLower()) || p.Reason.ToString().ToLower().Contains(search.ToLower()) ||
                                                              p.StartDate.ToString().ToLower().Contains(search.ToLower()) || p.EndDate.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.EmployeeLeaveHistoryID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.EmployeeID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Reason.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.StartDate.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.EndDate.ToString().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.AsEnumerable().Count();
                    data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                            s => new EmployeeLeaveViewModel
                            {
                                ID = s.EmployeeLeaveHistoryID,
                                Reason = s.Reason,
                                LoginName = db.Employee.Find(s.EmployeeID).LoginName,
                                LeaveTypeName = db.LeaveSallaryTypes.Find(s.LeaveType).LeaveTypeName,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                            })
                        .ToList();

                }

                data = this.SortByColumnWithOrder(order, orderDir, data);
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : leaveHistory.AsEnumerable().Count();

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
                Console.Write(ex);
            }
            return result;
        }
        private List<EmployeeLeaveViewModel> SortByColumnWithOrder(string order, string orderDir, List<EmployeeLeaveViewModel> data)
        {
            // Initialization.   
            List<EmployeeLeaveViewModel> lst = new List<EmployeeLeaveViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ID).ToList() : data.OrderBy(p => p.ID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StartDate).ToList() : data.OrderBy(p => p.StartDate).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EndDate).ToList() : data.OrderBy(p => p.EndDate).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LoginName).ToList() : data.OrderBy(p => p.LoginName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LeaveTypeName).ToList() : data.OrderBy(p => p.LeaveTypeName).ToList();
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
        public JsonResult InsertEmployeLeaveHistory(EmployeeLeaveHistory LeaveStory)
        {
            int leaveHistories = db.EmployeeLeaveHistories.Count();

            try
            {
                LeaveStory.Status = AppUtils.TableStatusIsActive;
                db.EmployeeLeaveHistories.Add(LeaveStory);
                db.SaveChanges();

                EmployeeLeaveViewModel LeaveHistoryInfo = new EmployeeLeaveViewModel
                {
                    ID = LeaveStory.EmployeeLeaveHistoryID,
                    LoginName = db.Employee.Find(LeaveStory.EmployeeID).LoginName,
                    Reason = LeaveStory.Reason,
                    StartDate = LeaveStory.EndDate,
                    EndDate = LeaveStory.EndDate,
                    LeaveTypeName = db.LeaveSallaryTypes.Find(LeaveStory.LeaveType).LeaveTypeName,
                };


                return Json(new { SuccessInsert = true, LeaveHistoryInfo = LeaveHistoryInfo, leaveHistories = leaveHistories }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLeaveHistoryByID(int id)
        {
            var leaveHistoryDetails = db.EmployeeLeaveHistories.Where(s => s.EmployeeLeaveHistoryID == id).Select(s => new
            {
                EmployeeLeaveHistoryID = s.EmployeeLeaveHistoryID,
                reason = s.Reason,
                LeaveType = s.LeaveType,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                EmployeeID = s.EmployeeID
            }).FirstOrDefault();

            var JSON = Json(new { leaveHistoryDetails = leaveHistoryDetails }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]

        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateLeaveHistoryType(EmployeeLeaveHistory LeaveHistoryIformation)
        {
            try
            {
                var leaveHistory = db.EmployeeLeaveHistories.Where(s => s.EmployeeLeaveHistoryID == LeaveHistoryIformation.EmployeeLeaveHistoryID);

                LeaveHistoryIformation.Status = AppUtils.TableStatusIsActive;
                db.Entry(leaveHistory.SingleOrDefault()).CurrentValues.SetValues(LeaveHistoryIformation);
                db.SaveChanges();

                var LeaveHistory =
                    new EmployeeLeaveViewModel()
                    {
                        ID = LeaveHistoryIformation.EmployeeLeaveHistoryID,
                        Reason = LeaveHistoryIformation.Reason,
                        LeaveTypeName = db.LeaveSallaryTypes.Find(LeaveHistoryIformation.LeaveType).LeaveTypeName,
                        StartDate = LeaveHistoryIformation.StartDate,
                        EndDate = LeaveHistoryIformation.EndDate,
                        LoginName = db.Employee.Find(LeaveHistoryIformation.EmployeeID).LoginName,
                    };
                var JSON = Json(new { UpdateSuccess = true, EMPLH = LeaveHistory }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DelecteLeaveHistory(int id)
        {
            EmployeeLeaveHistory leaveHistory = new EmployeeLeaveHistory();
            leaveHistory = db.EmployeeLeaveHistories.Find(id);
            leaveHistory.EmployeeID = leaveHistory.EmployeeID;
            leaveHistory.Reason = leaveHistory.Reason;
            leaveHistory.StartDate = leaveHistory.EndDate;
            leaveHistory.LeaveType = leaveHistory.LeaveType;
            leaveHistory.Status = AppUtils.TableStatusIsDelete;
            db.Entry(leaveHistory).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { leaveHistoryID = leaveHistory.EmployeeLeaveHistoryID, success = true }, JsonRequestBehavior.AllowGet);
        }


    }
}