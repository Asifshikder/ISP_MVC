//using ISP_ManagementSystemModel;
//using ISP_ManagementSystemModel.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Project_ISP.Controllers
//{
//    public class EmployeeStatus
//    {
//        public int EmployeeStatusID { get; set; }
//        public string EmployeeStatusName { get; set; }
//    }

//    [SessionTimeout]
//    [AjaxAuthorizeAttribute]
//    public class EmployeeController : Controller
//    {
//        public EmployeeController()
//        {
//            AppUtils.dateTimeNow = DateTime.Now;
//        }
//        private ISPContext db = new ISPContext();

//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Employee_List)]
//        public ActionResult Index()
//        {

//            List<EmployeeStatus> lstEmployeeStatus = new List<EmployeeStatus>();
//            lstEmployeeStatus.Add(new EmployeeStatus { EmployeeStatusID = 1, EmployeeStatusName = "Active" });
//            lstEmployeeStatus.Add(new EmployeeStatus { EmployeeStatusID = 2, EmployeeStatusName = "Lock" });

//            ViewBag.EmployeeStatusDD = new SelectList(lstEmployeeStatus, "EmployeeStatusID", "EmployeeStatusName");
//            ViewBag.NewEmployeeStatusDD = new SelectList(lstEmployeeStatus, "EmployeeStatusID", "EmployeeStatusName");



//            var result = db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).ToList();

//            ViewBag.DepartmentID = new SelectList(db.Department.Select(s => new { DepartmentID = s.DepartmentID, DepartmentName = s.DepartmentName }).ToList(), "DepartmentID", "DepartmentName");
//            ViewBag.RoleID = new SelectList(db.Role.ToList(), "RoleID", "RoleNae");
//            ViewBag.UserRightPermissionID = new SelectList(db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList(), "UserRightPermissionID", "UserRightPermissionName");

//            ViewBag.NewDepartmentID = new SelectList(db.Department.ToList(), "DepartmentID", "DepartmentName");
//            ViewBag.NewRoleID = new SelectList(db.Role.ToList(), "RoleID", "RoleNae");
//            ViewBag.NewUserRightPermissionID = new SelectList(db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList(), "UserRightPermissionID", "UserRightPermissionName");
//            return View(result);
//        }


//        [HttpPost]
//        public ActionResult InsertEmployee(Employee Employee)
//        {
//            Employee employeeDB = db.Employee.Where(s => s.LoginName == Employee.LoginName).FirstOrDefault();
//            if (employeeDB != null)
//            {
//                return Json(new { SuccessInsert = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
//            }

//            Employee employee = new Employee();
//            try
//            {
//                Employee.CreatedBy = AppUtils.GetLoginEmployeeName();
//                Employee.CreatedDate = AppUtils.GetDateTimeNow();

//                employee = db.Employee.Add(Employee);
//                db.SaveChanges();
//                if (employee.EmployeeID > 0)
//                {
//                    return Json(new { SuccessInsert = true, Employee = Employee }, JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch
//            {
//                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        public ActionResult GetEmployeeDetailsByID(int EmployeeID)
//        {
//            var Employee = db.Employee.Where(s => s.EmployeeID == EmployeeID).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name, LoginName = s.LoginName, Password = s.Password, Phone = s.Phone, Address = s.Address, Email = s.Email, DepartmentID = s.Department.DepartmentID, RoleID = s.Role.RoleID, UserRightPermissionID = s.UserRightPermissionID, EmployeeStatus = s.EmployeeStatus }).FirstOrDefault();

//            var JSON = Json(new { EmployeeDetails = Employee }, JsonRequestBehavior.AllowGet);
//            JSON.MaxJsonLength = int.MaxValue;
//            return JSON;
//        }

//        [HttpPost]
//        public ActionResult UpdateEmployeeInformation(Employee Employee)
//        {
//            try
//            {
//                Employee employeeDB = db.Employee.Where(s => s.LoginName == Employee.LoginName && s.EmployeeID != Employee.EmployeeID).FirstOrDefault();
//                if (employeeDB != null)
//                {
//                    return Json(new { UpdateSuccess = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
//                }


//                var employeess = db.Employee.Where(s => s.EmployeeID == Employee.EmployeeID);

//                Employee.UpdateBy = AppUtils.GetLoginEmployeeName();
//                Employee.UpdateDate = AppUtils.GetDateTimeNow();

//                //if (employeess.Count()>0 )
//                //{
//                //    Employee.UserRightPermissionID = employeess.SingleOrDefault().UserRightPermissionID;
//                //}

//                db.Entry(employeess.SingleOrDefault()).CurrentValues.SetValues(Employee);
//                db.SaveChanges();

//                var employees = employeess.Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name, LoginName = s.LoginName, Phone = s.Phone, Address = s.Address, Email = s.Email, DepartmentName = s.Department.DepartmentName, RoleName = s.Role.RoleNae, UserRightPermissionName = s.UserRightPermission.UserRightPermissionName, EmployeeStatus = s.EmployeeStatus });
//                var JSON = Json(new { UpdateSuccess = true, employees = employees }, JsonRequestBehavior.AllowGet);
//                JSON.MaxJsonLength = int.MaxValue;
//                return JSON;
//            }
//            catch
//            {
//                return Json(new { UpdateSuccess = false, employees = "" }, JsonRequestBehavior.AllowGet);
//            }
//        }
//    }
//}

using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    public class EmployeeStatus
    {
        public int EmployeeStatusID { get; set; }
        public string EmployeeStatusName { get; set; }
    }

    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class EmployeeController : Controller
    {
        public EmployeeController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Employee_List)]
        public ActionResult Index()
        {

            List<EmployeeStatus> lstEmployeeStatus = new List<EmployeeStatus>();
            lstEmployeeStatus.Add(new EmployeeStatus { EmployeeStatusID = 1, EmployeeStatusName = "Active" });
            lstEmployeeStatus.Add(new EmployeeStatus { EmployeeStatusID = 2, EmployeeStatusName = "Lock" });

            ViewBag.EmployeeStatusDD = new SelectList(lstEmployeeStatus, "EmployeeStatusID", "EmployeeStatusName");
            ViewBag.NewEmployeeStatusDD = new SelectList(lstEmployeeStatus, "EmployeeStatusID", "EmployeeStatusName");



            //var result = db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).ToList();

            ViewBag.DepartmentID = new SelectList(db.Department.Select(s => new { DepartmentID = s.DepartmentID, DepartmentName = s.DepartmentName }).ToList(), "DepartmentID", "DepartmentName");
            ViewBag.RoleID = new SelectList(db.Role.ToList(), "RoleID", "RoleNae");
            ViewBag.UserRightPermissionID = new SelectList(db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList(), "UserRightPermissionID", "UserRightPermissionName");

            ViewBag.NewDepartmentID = new SelectList(db.Department.ToList(), "DepartmentID", "DepartmentName");
            ViewBag.NewRoleID = new SelectList(db.Role.ToList(), "RoleID", "RoleNae");
            ViewBag.NewUserRightPermissionID = new SelectList(db.UserRightPermission.Where(s => s.UserRightPermissionID != AppUtils.UserRightPermissionIDIsSuperTallentUser).Select(s => new { UserRightPermissionID = s.UserRightPermissionID, UserRightPermissionName = s.UserRightPermissionName }).ToList(), "UserRightPermissionID", "UserRightPermissionName");

            ViewBag.OffDay = new SelectList(db.Days.ToList(), "DayID", "DayName");
            ViewBag.NewOffDay = new SelectList(db.Days.ToList(), "DayID", "DayName");

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

            List<Model> DutyShift = new List<Model>();
            foreach (var item in db.DutyShifts)
            {
                Model model = new Model();
                model.DutyShiftID = item.DutyShiftID;
                model.StartTime = item.StartHour + " : " + item.StartMinute + " " + ((item.StartHour < 12) ? "AM" : "PM");
                model.EndTime = item.EndHour + " : " + item.EndMinute + " " + ((item.EndHour < 12) ? "AM" : "PM");
                model.CombineTime = model.StartTime + " - " + model.EndTime;
                DutyShift.Add(model);

            }
            List<Employee> AllEmployee = new List<Employee>();
            foreach (var item in db.Employee.Where(s => s.EmployeeStatus != AppUtils.EmployeeStatusIsDelete && (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)))
            {
                Model models = new Model();
                if (item.DutyShiftID != 0)
                {
                    var employee = db.Employee.Find(item.EmployeeID);
                    var dutyShift = db.DutyShifts.Find(employee.DutyShiftID);
                    models.StartTime = dutyShift.StartHour + " : " + dutyShift.StartMinute + " " + ((dutyShift.StartHour < 12) ? "AM" : "PM");
                    models.EndTime = dutyShift.EndHour + " : " + dutyShift.EndMinute + " " + ((dutyShift.EndHour < 12) ? "AM" : "PM");
                    models.CombineTime = models.StartTime + " - " + models.EndTime;
                    employee.DutyShiftCombined = models.CombineTime;
                    AllEmployee.Add(employee);
                }
            }

            ViewBag.BreakHour = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakMinute = new SelectList(lstMin, "Value", "Text");
            ViewBag.NewBreakHour = new SelectList(lstHour, "Value", "Text");
            ViewBag.NewBreakMinute = new SelectList(lstMin, "Value", "Text");

            ViewBag.DutyShift = new SelectList(DutyShift, "DutyShiftID", "CombineTime");
            ViewBag.NewDutyShift = new SelectList(DutyShift, "DutyShiftID", "CombineTime");

            SetTimeScheduleInDropDownViewBag(lstHour, lstMin);

            return View(AllEmployee);
        }

        private void SetTimeScheduleInDropDownViewBag(List<SelectListItem> lstHour, List<SelectListItem> lstMin)
        {

            ViewBag.StartH1 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM1 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH1 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM1 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH1 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM1 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH1 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM1 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH2 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM2 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH2 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM2 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH2 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM2 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH2 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM2 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH3 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM3 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH3 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM3 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH3 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM3 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH3 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM3 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH4 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM4 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH4 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM4 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH4 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM4 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH4 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM4 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH5 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM5 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH5 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM5 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH5 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM5 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH5 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM5 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH6 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM6 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH6 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM6 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH6 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM6 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH6 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM6 = new SelectList(lstMin, "Value", "Text");

            ViewBag.StartH7 = new SelectList(lstHour, "Value", "Text");
            ViewBag.StartM7 = new SelectList(lstMin, "Value", "Text");
            ViewBag.RunH7 = new SelectList(lstHour, "Value", "Text");
            ViewBag.RunM7 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakSH7 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakSM7 = new SelectList(lstMin, "Value", "Text");
            ViewBag.BreakEH7 = new SelectList(lstHour, "Value", "Text");
            ViewBag.BreakEM7 = new SelectList(lstMin, "Value", "Text");
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertEmployee(FormCollection file, HttpPostedFileBase EmployeeOwnCreateImage, HttpPostedFileBase EmployeeNIDCreateImage)
        {

            Employee EmployeeDetails = JsonConvert.DeserializeObject<Employee>(file["Employee_details"]);
            Employee EmployeeDb = db.Employee.Where(s => s.LoginName == EmployeeDetails.LoginName).FirstOrDefault();
            if (EmployeeDb != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }

            Employee Employee_Return = new Employee();

            try
            {
                EmployeeDetails.CreatedBy = AppUtils.GetLoginEmployeeName();
                EmployeeDetails.CreatedDate = AppUtils.GetDateTimeNow();
                Employee_Return = db.Employee.Add(EmployeeDetails);
                db.SaveChanges();
                if (EmployeeNIDCreateImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref EmployeeDetails, AppUtils.ImageIsNID, EmployeeNIDCreateImage);
                }
                if (EmployeeOwnCreateImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref EmployeeDetails, AppUtils.ImageIsOWN, EmployeeOwnCreateImage);
                }
                db.SaveChanges();
                if (Employee_Return.EmployeeID > 0)
                {
                    db.SaveChanges();
                    return Json(new { SuccessInsert = true, Vendor = Employee_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        public ActionResult GetEmployeeDetailsByID(int EmployeeID)
        {
            var Employee = db.Employee.Where(s => s.EmployeeID == EmployeeID)
                .Select(s => new
                {
                    EmployeeID = s.EmployeeID,
                    Name = s.Name,
                    DOB = s.DOB,
                    LoginName = s.LoginName,
                    Password = s.Password,
                    Phone = s.Phone,
                    Address = s.Address,
                    Email = s.Email,
                    DepartmentID = s.Department.DepartmentID,
                    RoleID = s.Role.RoleID,
                    UserRightPermissionID = s.UserRightPermissionID,
                    EmployeeStatus = s.EmployeeStatus,
                    Salary = s.Salary,
                    OffDay = s.Days.DayID,
                    DutyShift = s.DutyShift.DutyShiftID,
                    BreakHour = s.BreakHour,
                    BreakMinute = s.BreakMinute,
                    OwnImagePath = s.EmployeeOwnImageBytesPaths,
                    NIDImagePath = s.EmployeeNIDImageBytesPaths,

                }).FirstOrDefault();

            Employee em = new Employee();
            em = db.Employee.Find(EmployeeID);
            DutyShift shift = new DutyShift();
            shift = db.DutyShifts.Find(em.DutyShiftID);
            var StartTime = ((shift.StartHour < 12) ? shift.StartHour + " : " + shift.StartMinute + " " + "AM " : (shift.StartHour - 12) + " : " + shift.StartMinute + " " + "PM");
            var EndTime = ((shift.EndHour < 12) ? shift.EndHour + " : " + shift.EndMinute + " " + "AM " : (shift.EndHour - 12) + " : " + shift.EndMinute + " " + "PM");
            var ShiftTime = StartTime + " - " + EndTime;

            var JSON = Json(new { EmployeeDetails = Employee, ShiftTime = ShiftTime }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateEmployeeInformation(FormCollection form, HttpPostedFileBase EmployeeOwnUpdateImage, HttpPostedFileBase EmployeeNIDUpdateImage)
        {

            Employee Employee_details = JsonConvert.DeserializeObject<Employee>(form["Employee_details"]);
            Employee EmployeeDb = db.Employee.Where(s => s.EmployeeID == Employee_details.EmployeeID).FirstOrDefault();
            Employee Employee_Check = db.Employee.Where(s => s.LoginName == Employee_details.LoginName).FirstOrDefault();
            if (Employee_Check != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }
            try
            {

                AddGivenImageInCurrentRow(ref EmployeeDb, Employee_details, "OWN", EmployeeOwnUpdateImage, Employee_details.EmployeeOwnImageBytesPaths);
                AddGivenImageInCurrentRow(ref EmployeeDb, Employee_details, "NID", EmployeeNIDUpdateImage, Employee_details.EmployeeNIDImageBytesPaths);
                if (EmployeeDb.EmployeeID > 0)
                {
                    Employee em = new Employee();
                    em = db.Employee.Find(Employee_details.EmployeeID);
                    DutyShift shift = new DutyShift();
                    shift = db.DutyShifts.Find(em.DutyShiftID);
                    var StartTime = ((shift.StartHour < 12) ? shift.StartHour + " : " + shift.StartMinute + " " + "AM" : (shift.StartHour - 12) + " : " + shift.StartMinute + " " + "PM");
                    var EndTime = ((shift.EndHour < 12) ? shift.EndHour + " : " + shift.EndMinute + " " + "AM" : (shift.EndHour - 12) + " : " + shift.EndMinute + " " + "PM");
                    var ShiftTime = StartTime + " - " + EndTime;

                    var BreakRunTime = em.BreakHour + " : " + em.BreakMinute;
                    db.Entry(EmployeeDb).CurrentValues.SetValues(Employee_details);
                    db.SaveChanges();
                    return Json(new { UpdateSuccess = true, employees = EmployeeDb, ShiftTime = ShiftTime, BreakRunTime = BreakRunTime }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void AddGivenImageInCurrentRow(ref Employee employeeDb, Employee employee_details, string type, HttpPostedFileBase EmployeeImageBytes, string imagePath)
        {
            if (type == "OWN")
            {
                if (EmployeeImageBytes != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringEmployeeUpdate(ref employee_details, employeeDb, "OWN", EmployeeImageBytes);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    employee_details.EmployeeOwnImageBytes = employeeDb.EmployeeOwnImageBytes;
                    employee_details.EmployeeOwnImageBytesPaths = employeeDb.EmployeeOwnImageBytesPaths;
                }
                else
                {
                    RemoveImageFromServerFolder(type, employeeDb);
                    employee_details.EmployeeOwnImageBytes = null;
                    employee_details.EmployeeOwnImageBytesPaths = null;
                }
            }
            if (type == "NID")
            {
                if (EmployeeImageBytes != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringEmployeeUpdate(ref employee_details, employeeDb, "NID", EmployeeImageBytes);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    employee_details.EmployeeNIDImageBytes = employeeDb.EmployeeNIDImageBytes;
                    employee_details.EmployeeNIDImageBytesPaths = employeeDb.EmployeeNIDImageBytesPaths;
                }
                else
                {
                    RemoveImageFromServerFolder(type, employeeDb);
                    employee_details.EmployeeOwnImageBytes = null;
                    employee_details.EmployeeOwnImageBytesPaths = null;
                }
            }

        }
        private void RemoveImageFromServerFolder(string WhichPic, Employee employeeDb)
        {
            string removeImageName = "";
            if (WhichPic == "NID")
            {
                removeImageName = employeeDb.EmployeeNIDImageBytesPaths != null ? employeeDb.EmployeeNIDImageBytesPaths.Split('/')[3] : "";

            }
            else if (WhichPic == "OWN")
            {
                removeImageName = employeeDb.EmployeeOwnImageBytesPaths != null ? employeeDb.EmployeeOwnImageBytesPaths.Split('/')[3] : "";
            }

            var filePath = Server.MapPath("~/Images/EmployeeImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void RemoveOldImageAndThenSaveImageDuringEmployeeUpdate(ref Employee employee_details, Employee employeeDb, string WhichPic, HttpPostedFileBase clientImageBytes)
        {


            RemoveImageFromServerFolder(WhichPic, employeeDb);



            if (!IsValidContentType(clientImageBytes.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            else if (!IsValidContentLength(clientImageBytes.ContentLength))
            {
                ViewBag.ErrorFileTooLarge = "Your file is too large.";
            }

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clientImageBytes.FileName);
            string extension = Path.GetExtension(clientImageBytes.FileName);
            var fileName = employee_details.EmployeeID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/EmployeeImage"), fileName);
            clientImageBytes.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(clientImageBytes.InputStream);
            imagebyte = reader.ReadBytes(clientImageBytes.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "NID")
            {
                //clientDetails.image = fileName;
                employee_details.EmployeeNIDImageBytes = imagebyte;
                employee_details.EmployeeNIDImageBytesPaths = "/Images/EmployeeImage/" + fileName;

            }
            else if (WhichPic == "OWN")
            {
                //clientDetails.image = fileName;
                employee_details.EmployeeOwnImageBytes = imagebyte;
                employee_details.EmployeeOwnImageBytesPaths = "/Images/EmployeeImage/" + fileName;
            }
        }



        //public ActionResult UpdateEmployeeInformation(Employee Employee)
        //{
        //    try
        //    {
        //        Employee employeeDB = db.Employee.Where(s => s.LoginName == Employee.LoginName && s.EmployeeID != Employee.EmployeeID).FirstOrDefault();
        //        if (employeeDB != null)
        //        {
        //            return Json(new { UpdateSuccess = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
        //        }


        //        var employeess = db.Employee.Where(s => s.EmployeeID == Employee.EmployeeID);

        //        Employee.UpdateBy = AppUtils.GetLoginEmployeeName();
        //        Employee.UpdateDate = AppUtils.GetDateTimeNow();

        //        db.Entry(employeess.SingleOrDefault()).CurrentValues.SetValues(Employee);
        //        db.SaveChanges();

        //        Employee em = new Employee();
        //        em = db.Employee.Find(Employee.EmployeeID);
        //        DutyShift shift = new DutyShift();
        //        shift = db.DutyShifts.Find(em.DutyShiftID);
        //        var StartTime = ((shift.StartHour < 12) ? shift.StartHour + " : " + shift.StartMinute + " " + "AM" : (shift.StartHour - 12) + " : " + shift.StartMinute + " " + "PM");
        //        var EndTime = ((shift.EndHour < 12) ? shift.EndHour + " : " + shift.EndMinute + " " + "AM" : (shift.EndHour - 12) + " : " + shift.EndMinute + " " + "PM");
        //        var ShiftTime = StartTime + " - "+EndTime;

        //        var employees = employeess.Select(s => new { EmployeeID = s.EmployeeID, DOB = s.DOB, Name = s.Name, LoginName = s.LoginName, Phone = s.Phone, Address = s.Address, Email = s.Email, DepartmentName = s.Department.DepartmentName, RoleName = s.Role.RoleNae, UserRightPermissionName = s.UserRightPermission.UserRightPermissionName, EmployeeStatus = s.EmployeeStatus,Salary=s.Salary,OffDay=s.Days.DayName });
        //        var BreakRunTime = Employee.BreakHour + " : " + Employee.BreakMinute;
        //        var JSON = Json(new { UpdateSuccess = true, employees = employees, BreakRunTime=BreakRunTime, ShiftTime= ShiftTime }, JsonRequestBehavior.AllowGet);
        //        JSON.MaxJsonLength = int.MaxValue;
        //        return JSON;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { UpdateSuccess = false, employees = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult InsertWorkScheduleForEmployee(int empID)
        {

            try
            {
                var EmployeeID = empID;
                List<EmployeeVsWorkSchedule> lstEmployeeSchedule = db.EmployeeVsWorkSchedules.Where(x => x.EmployeeID == empID).ToList();
                EmployeeVsWorkSchedule sceheduleForDay1 = lstEmployeeSchedule.Where(x => x.DayID == 1).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay2 = lstEmployeeSchedule.Where(x => x.DayID == 2).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay3 = lstEmployeeSchedule.Where(x => x.DayID == 3).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay4 = lstEmployeeSchedule.Where(x => x.DayID == 4).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay5 = lstEmployeeSchedule.Where(x => x.DayID == 5).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay6 = lstEmployeeSchedule.Where(x => x.DayID == 6).FirstOrDefault();
                EmployeeVsWorkSchedule sceheduleForDay7 = lstEmployeeSchedule.Where(x => x.DayID == 7).FirstOrDefault();

                List<Day> dayList = db.Days.ToList();
                return Json(new { lstEmployeeSchedule = lstEmployeeSchedule, dayList = dayList, EmployeeID = EmployeeID, success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult SaveEmployeeWorkSchedule(List<EmployeeVsWorkSchedule> lstEmployeeVsSchedules)
        {
            if (lstEmployeeVsSchedules[0].EmployeeVsWorkScheduleID > 0)
            {
                try
                {
                    foreach (var item in lstEmployeeVsSchedules)
                    {
                        EmployeeVsWorkSchedule employeeVsSchedule = db.EmployeeVsWorkSchedules.Find(item.EmployeeVsWorkScheduleID);
                        employeeVsSchedule.StartHour = item.StartHour;
                        employeeVsSchedule.StartMinute = item.StartMinute;
                        employeeVsSchedule.RunHour = item.RunHour;
                        employeeVsSchedule.RunMinute = item.RunMinute;
                        employeeVsSchedule.BreakStartHour = item.BreakStartHour;
                        employeeVsSchedule.BreakStartMinute = item.BreakStartMinute;
                        employeeVsSchedule.BreakEndHour = item.BreakEndHour;
                        employeeVsSchedule.BreakEndMinute = item.BreakEndMinute;
                        employeeVsSchedule.UpdateBy = AppUtils.GetLoginUserID();
                        employeeVsSchedule.UpdateDate = AppUtils.GetDateTimeNow();

                        db.Entry(employeeVsSchedule).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                try
                {
                    foreach (var item in lstEmployeeVsSchedules)
                    {
                        EmployeeVsWorkSchedule employeeVsSchedule = new EmployeeVsWorkSchedule();
                        employeeVsSchedule.DayID = item.DayID;
                        employeeVsSchedule.StartHour = item.StartHour;
                        employeeVsSchedule.StartMinute = item.StartMinute;
                        employeeVsSchedule.RunHour = item.RunHour;
                        employeeVsSchedule.RunMinute = item.RunMinute;
                        employeeVsSchedule.BreakStartHour = item.BreakStartHour;
                        employeeVsSchedule.BreakStartMinute = item.BreakStartMinute;
                        employeeVsSchedule.BreakEndHour = item.BreakEndHour;
                        employeeVsSchedule.BreakEndMinute = item.BreakEndMinute;
                        employeeVsSchedule.EmployeeID = item.EmployeeID;
                        employeeVsSchedule.CreateDate = AppUtils.GetDateTimeNow();
                        employeeVsSchedule.CreatedBy = AppUtils.GetLoginUserID();

                        db.EmployeeVsWorkSchedules.Add(employeeVsSchedule);
                        db.SaveChanges();
                    }

                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

            }

        }

        public class Model
        {
            public int DutyShiftID { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string CombineTime { get; set; }
        }

        [HttpPost]
        public ActionResult DutyShiftForEmployee(int empID)
        {
            try
            {
                var Employee = db.Employee.Find(empID);
                DutyShift dse = new DutyShift();
                dse = db.DutyShifts.Find(Employee.DutyShiftID);
                Model shiftTime = new Model();
                if (dse.StartHour > 12)
                {
                    int starthour = dse.StartHour - 12;
                    shiftTime.StartTime = starthour + " " + " : " + " " + dse.StartMinute + " " + " PM";
                }
                else
                {
                    shiftTime.StartTime = dse.StartHour + " " + " : " + " " + dse.StartMinute + " " + " AM";
                }
                if (dse.EndHour > 12)
                {
                    int endHour = dse.EndHour - 12;
                    shiftTime.EndTime = endHour + " " + " : " + " " + dse.EndMinute + " " + " PM";
                }
                else
                {
                    shiftTime.EndTime = dse.EndHour + " " + ":" + dse.EndMinute + " " + " AM";
                }

                return Json(new { shift = shiftTime, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveImageInFolderAndAddInformationInDVDSTable(ref Employee employeeDetails, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            else if (!IsValidContentLength(image.ContentLength))
            {
                ViewBag.ErrorFileTooLarge = "Your file is too large.";
            }

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = employeeDetails.EmployeeID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/EmployeeImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "NID")
            {
                employeeDetails.EmployeeNIDImageBytes = imagebyte;
                employeeDetails.EmployeeNIDImageBytesPaths = "/Images/EmployeeImage/" + fileName;

            }
            else if (WhichPic == "OWN")
            {
                employeeDetails.EmployeeOwnImageBytes = imagebyte;
                employeeDetails.EmployeeOwnImageBytesPaths = "/Images/EmployeeImage/" + fileName;
            }
        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }
        private bool IsValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1 MB
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

    }

}