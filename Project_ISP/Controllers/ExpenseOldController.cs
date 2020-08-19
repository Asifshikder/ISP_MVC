using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel.Models;
using Project_ISP;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]
    public class ExpenseController : Controller
    {
        public ExpenseController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Expense_List)]
        public ActionResult Index()
        {
            var lstExpense = db.Expenses.Where(s=>s.PaymentDate.Year == AppUtils.RunningYear && s.PaymentDate.Month == AppUtils.RunningMonth).AsEnumerable();
            ViewBag.TotalExpenseAmount = lstExpense.Where(s => s.PaymentDate.Year == AppUtils.RunningYear && s.PaymentDate.Month == AppUtils.RunningMonth).Sum(s => s.Amount);

            //var result = db.Expenses.ToList();
            //List<int> lstUpdatedByID = lstExpense.Where(s => s.UpdateBy != null).Select(s => s.UpdateBy.Value).ToList();
            return View(new List<Expense>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllExpenseAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;

                var StartDate = Request.Form.Get("StartDate");
                var EndDate = Request.Form.Get("EndDate");
                var ExpSubject = Request.Form.Get("ExpSubject");
                

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                DateTime convertStartDate = new DateTime();
                DateTime convertEndDate = new DateTime();

                DateTime startDate = AppUtils.ThisMonthStartDate();
                DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
                IEnumerable<Expense> ExpenseEnumerable = Enumerable.Empty<Expense>();

                if (StartDate != "")
                {
                    convertStartDate = Convert.ToDateTime(StartDate);
                }
                if (EndDate != "")
                {
                    convertEndDate = Convert.ToDateTime(EndDate);
                }

                var firstPart =  (StartDate != "" && EndDate != "" && ExpSubject != "") ? db.Expenses.Where(s => s.PaymentDate >= convertStartDate && s.PaymentDate <= convertEndDate && s.Details.Contains(ExpSubject)).AsEnumerable()
                    : (StartDate != "" && EndDate != "" && ExpSubject == "") ? db.Expenses.Where(s => s.PaymentDate >= convertStartDate && s.PaymentDate <= convertEndDate ).AsEnumerable()
                    : (StartDate == "" && EndDate == "" && ExpSubject != "") ? db.Expenses.Where(s =>   s.Details.Contains(ExpSubject)).AsEnumerable()
                                    : db.Expenses.AsEnumerable();



                //db.Transaction.Where(s =>  s.PaymentDate >= startDate && s.PaymentDate <= endDate && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
              //  List <CustomSignUpBills> lstArchiveBillsInformation = new List<CustomSignUpBills>();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPart.Any()) ? firstPart.Where(p =>   p.Subject.ToString().ToLower().Contains(search.ToLower())
                                                                                                || p.Details.ToString().ToLower().Contains(search.ToLower())
                                                                                                || p.PaidTo.ToString().ToLower().Contains(search.ToLower())
                                                                                                || p.PaidTo.ToString().ToLower().Contains(search.ToLower())
                                                                                                || p.Amount.ToString().Contains(search.ToLower()))
                                                                                                 .Count() : 0;

                    // Apply search   
                    firstPart = firstPart.Where(p => p.Subject.ToString().ToLower().Contains(search.ToLower())
                                                                      || p.Details.ToString().ToLower().Contains(search.ToLower())
                                                                      || p.PaidTo.ToString().ToLower().Contains(search.ToLower())
                                                                      || p.PaidTo.ToString().ToLower().Contains(search.ToLower())
                                                                      || p.Amount.ToString().Contains(search.ToLower())).AsEnumerable();
                }
                //var a = transactionEnumerable.ToList();
                

                // Verification.   
                List<CustomExpense> lstCustom = new List<CustomExpense>();
                if (firstPart.Count() > 0)
                {
                    totalRecords = firstPart.Count();
                    lstCustom = firstPart.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            s => new CustomExpense
                            {
                                ExpenseID = s.ExpenseID,
                                Subject = s.Subject,
                                Details = s.Details,
                                PaidTo = s.PaidTo,
                                PaidBy = s.Employee != null ? s.Employee.Name : "",
                                Amount = s.Amount.ToString(),
                                CreateDate = s.PaymentDate,
                                UpdateDate = s.UpdateDate != null ? s.UpdateDate.Value.ToString() : "",
                                UpdateExpense = (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Expense)) ? true : false,
                                DeleteExpense = (ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Expense))  ? true: false
                            }).ToList(); 
                }

                // Sorting.   
                lstCustom = this.SortByColumnWithOrder(order, orderDir, lstCustom);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = lstCustom
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


        private List<CustomExpense> SortByColumnWithOrder(string order, string orderDir, List<CustomExpense> data)
        {

            // Initialization.   
            List<CustomExpense> lst = new List<CustomExpense>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpenseID).ToList() : data.OrderBy(p => p.ExpenseID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLineStatusID).ToList() : data.OrderBy(p => p.ClientLineStatusID).ToList();
                    //    break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Details).ToList() : data.OrderBy(p => p.Details).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidTo).ToList() : data.OrderBy(p => p.PaidTo).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PaidBy).ToList() : data.OrderBy(p => p.PaidBy).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.UpdateDate).ToList() : data.OrderBy(p => p.UpdateDate).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpenseID).ToList() : data.OrderBy(p => p.ExpenseID).ToList();
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


        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Expense)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Expense expense)
        {
            Expense expenseObj = new Expense();
            try
            {
                expense.EmployeeID = AppUtils.GetLoginUserID();
                expense.PaymentDate = AppUtils.GetDateTimeNow();

                expenseObj = db.Expenses.Add(expense);
                db.SaveChanges();
                if (expenseObj.ExpenseID > 0)
                {
                    return Json(new { SuccessInsert = true, expenseObj = expenseObj }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetExpenseById(int ExpenseID)
        {
            var expense = db.Expenses.Where(s => s.ExpenseID == ExpenseID).Select(s => new { Subject = s.Subject, Details = s.Details, PaidTo = s.PaidTo, Amount = s.Amount }).FirstOrDefault();

            var JSON = Json(new { expense = expense }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        public ActionResult UpdateExpense(Expense expense)
        {
            try
            {
                var expenses = db.Expenses.Where(s => s.ExpenseID == expense.ExpenseID);

                expense.EmployeeID = expenses.FirstOrDefault().EmployeeID;
                expense.PaymentDate = expenses.FirstOrDefault().PaymentDate;
                expense.UpdateBy = AppUtils.GetLoginUserID();
                expense.UpdateDate = AppUtils.GetDateTimeNow();


                db.Entry(expenses.SingleOrDefault()).CurrentValues.SetValues(expense);
                db.SaveChanges();

                var expensesss = expenses.Select(s => new { Subject = s.Subject, Details = s.Details, PaidTo = s.PaidTo, Amount = s.Amount, Date = s.UpdateDate, ExpenseID = s.ExpenseID });
                var JSON = Json(new { UpdateSuccess = true, expensesss = expensesss }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, expensesss = "" }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExpense(int ExpenseID)
        {
            try
            {
                int countTransaction = db.Expenses.RemoveRange(db.Expenses.Where(s => s.ExpenseID == ExpenseID)).Count();
               
                db.SaveChanges();
                return Json(new { DeleteStatus = true, ExpenseID = ExpenseID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, ExpenseID = ExpenseID }, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPost]
        public ActionResult FindExpenseByDateWithSubject(DateTime? start_date, DateTime? end_date, string exp_subject)
        {
            if (end_date != null)
            {
                // timespan = end_date.Value.AddHours(23),;
                end_date = end_date.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
            }
            
            var TotalExpenseAmount = "";
            //  DateTime startDate = 
            //dekh query kemne kori....
            if (start_date != null && end_date != null && !string.IsNullOrEmpty(exp_subject))
            {
                var lstExpenses = db.Expenses.Where(s => (s.PaymentDate >= start_date && s.PaymentDate <= end_date) && s.Subject.Contains(exp_subject))
                     .Select(s => new { ExpenseID = s.ExpenseID, Subject = s.Subject, Details = s.Details, PaidTo = s.PaidTo, PaidBy = s.Employee.Name, Amount = s.Amount, Date = s.PaymentDate, UpdateDate = s.UpdateDate})
                     .ToList();
                TotalExpenseAmount = lstExpenses.Sum(s => s.Amount).ToString();
                return Json(new { Success = true, lstExpense = lstExpenses, TotalExpenseAmount = TotalExpenseAmount }, JsonRequestBehavior.AllowGet);
            }
            else if (start_date != null && end_date != null)
            {
                var lstExpenses =
                     db.Expenses.Where(
                         s =>
                             (s.PaymentDate >= start_date && s.PaymentDate <= end_date))
                             .Select(s => new { ExpenseID = s.ExpenseID, Subject = s.Subject, Details = s.Details, PaidTo = s.PaidTo, PaidBy = s.Employee.Name, Amount = s.Amount, Date = s.PaymentDate, UpdateDate = s.UpdateDate })
                         .ToList();
                TotalExpenseAmount = lstExpenses.Sum(s => s.Amount).ToString();
                return Json(new { Success = true, lstExpense = lstExpenses, TotalExpenseAmount = TotalExpenseAmount }, JsonRequestBehavior.AllowGet);
            }
            else if (!string.IsNullOrEmpty(exp_subject))
            {
                var lstExpenses =
                     db.Expenses.Where(
                         s =>
                             (s.Subject.Contains(exp_subject)))
                             .Select(s => new { ExpenseID = s.ExpenseID, Subject = s.Subject, Details = s.Details, PaidTo = s.PaidTo, PaidBy = s.Employee.Name, Amount = s.Amount, Date = s.PaymentDate, UpdateDate = s.UpdateDate })
                         .ToList();
                TotalExpenseAmount = lstExpenses.Sum(s => s.Amount).ToString();
                return Json(new { Success = true, lstExpense = lstExpenses, TotalExpenseAmount = TotalExpenseAmount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, lstExpense = "", TotalExpenseAmount = "" }, JsonRequestBehavior.AllowGet);
            }

            return View();
        }

    }
}