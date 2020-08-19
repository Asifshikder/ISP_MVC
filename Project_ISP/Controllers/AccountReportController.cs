using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ISP_ManagementSystemModel.AppUtils;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class AccountReportController : Controller
    {
        private ISPContext db = new ISPContext();
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_AccountReport)]
        public ActionResult Index()
        {
            List<SelectListItem> TransactionType = new List<SelectListItem>();
            TransactionType.Add(new SelectListItem() { Text = "Deposit", Value = "1" });
            TransactionType.Add(new SelectListItem() { Text = "Expense", Value = "2" });
            TransactionType.Add(new SelectListItem() { Text = "Transfer", Value = "3" });
            TransactionType.Add(new SelectListItem() { Text = "Purchase", Value = "4" });
            ViewBag.TransactionTypeID = new SelectList(TransactionType, "Value", "Text");
            ViewBag.AccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
            ViewBag.HeadID = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive), "HeadID", "HeadeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllTransactionAjaxData()
        {
            JsonResult result = new JsonResult();
            try
            {
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;

                int TransactionFromDDL = 0;
                int AccountListFromDDl = 0;
                int HeadFromDDL = 0;

                var TransactionTypeID = Request.Form.Get("TransactionTypeID");
                var AccountList = Request.Form.Get("AccountListID");
                var HeadType = Request.Form.Get("Head");
                var StartDateID = Request.Form.Get("StartDate");
                var EndDateID = Request.Form.Get("EndDate");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                DateTime? startDate = new DateTime?();
                DateTime? endDate = new DateTime?();

                List<CustomAccountListReport> data = new List<CustomAccountListReport>();

                if (!string.IsNullOrEmpty(TransactionTypeID))
                {
                    TransactionFromDDL = int.Parse(TransactionTypeID);
                }

                if (!string.IsNullOrEmpty(AccountList))
                {
                    AccountListFromDDl = int.Parse(AccountList);
                }

                if (!string.IsNullOrEmpty(HeadType))
                {
                    HeadFromDDL = int.Parse(HeadType);
                }

                if (!string.IsNullOrEmpty(StartDateID))
                {
                    startDate = Convert.ToDateTime(StartDateID);
                }

                if (!string.IsNullOrEmpty(EndDateID))
                {
                    endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(DateTime.Parse(EndDateID));
                }

                if (TransactionFromDDL == (int)TransactionType.Deposit)
                {
                    //startDate endate account head

                    var firstPartOfQuery = db.Deposit.Where(a => a.Status == AppUtils.TableStatusIsActive).AsQueryable();
                    if (StartDateID != "" && EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.DepositDate >= startDate && s.DepositDate <= endDate).AsQueryable();
                    }
                    else if (StartDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.DepositDate >= startDate).AsQueryable();
                    }
                    else if (EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.DepositDate <= endDate).AsQueryable();
                    }
                    else
                    {

                    }

                    if (!string.IsNullOrEmpty(AccountList))
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.AccountListID == AccountListFromDDl).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(HeadType))
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.HeadID == HeadFromDDL).AsQueryable();
                    }

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {

                        ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p =>
                        p.Description.ToString().ToLower().Contains(search.ToLower())
                        || p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
                        ).Count() : 0;

                        // Apply search   
                        firstPartOfQuery = firstPartOfQuery.Where(p =>
                        p.Description.ToString().ToLower().Contains(search.ToLower())
                        || p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())).AsQueryable(); ;
                    }
                    if (firstPartOfQuery.Count() > 0)
                    {
                        totalRecords = firstPartOfQuery.AsEnumerable().Count();
                        data = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                                s => new CustomAccountListReport
                                {
                                    ReportID = s.DepositID,
                                    Date = s.DepositDate,
                                    AccountListName = s.AccountList.AccountTitle,
                                    transactionType = "Deposit",
                                    Amount = s.Amount,
                                    Description = s.Description,
                                    Debit = s.Amount,
                                    Credit = 0,
                                    Button = GetButtonForDeposit(s),
                                })
                            .ToList(); 
                    }

                    data = this.SortByColumnWithOrder(order, orderDir, data);
                    recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = recFilter,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }

                else if (TransactionFromDDL == (int)TransactionType.Expense)
                {

                    var firstPartOfQuery = db.Expenses.Where(a => a.Status == AppUtils.TableStatusIsActive).AsQueryable();
                    if (StartDateID != "" && EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).AsQueryable();
                    }
                    else if (StartDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.PaymentDate >= startDate).AsQueryable();
                    }
                    else if (EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.PaymentDate <= endDate).AsQueryable();
                    }
                    else
                    {

                    }

                    if (!string.IsNullOrEmpty(AccountList))
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.AccountListID == AccountListFromDDl).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(HeadType))
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.HeadID == HeadFromDDL).AsQueryable();
                    }

                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {

                        ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => 
                        p.Descriptions.ToString().ToLower().Contains(search.ToLower()) 
                        || p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
                        ).Count() : 0;

                        // Apply search   
                        firstPartOfQuery = firstPartOfQuery.Where(p =>  p.Descriptions.ToString().ToLower().Contains(search.ToLower())
                                                                         || p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
                                                                         ).AsQueryable();
                    }
                    if (firstPartOfQuery.Count() > 0)
                    {
                        totalRecords = firstPartOfQuery.AsEnumerable().Count();
                        data = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                                s => new CustomAccountListReport
                                {
                                    ReportID = s.ExpenseID,
                                    Date = s.PaymentDate,
                                    AccountListName = s.AccountList.AccountTitle,
                                    transactionType = "Expense",
                                    Amount = Convert.ToDecimal(s.Amount),
                                    Description = s.Descriptions,
                                    Debit = 0,
                                    Credit = Convert.ToDecimal(s.Amount),
                                    Button = GetButtonForExpense(s),
                                })
                            .ToList();

                    }

                    data = this.SortByColumnWithOrder(order, orderDir, data);
                    recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = recFilter,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }

                else if (TransactionFromDDL == (int)TransactionType.Transfer)
                {
                    var firstPartOfQuery = db.AccountListVsAmountTransfer.Where(a => a.Status == AppUtils.TableStatusIsActive && a.BreakDownAccountListID != 0).AsQueryable();
                    //var firstPartOfQuery =
                    //     (StartDateID != "" && EndDateID != "" && !string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate >= startDate && s.TransferDate <= endDate && (s.FromAccountID == AccountListFromDDl || s.ToAccountID == AccountListFromDDl)).AsQueryable()
                    //         : (StartDateID != "" && EndDateID != "" && string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate >= startDate && s.TransferDate <= endDate).AsQueryable()
                    //             : (StartDateID != "" && EndDateID == "" && string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate >= startDate && (s.FromAccountID == AccountListFromDDl || s.ToAccountID == AccountListFromDDl)).AsQueryable()
                    //                 : (StartDateID != "" && EndDateID == "" && string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate >= startDate).AsQueryable()
                    //                    : (StartDateID == "" && EndDateID == "" && !string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.FromAccountID == AccountListFromDDl || s.ToAccountID == AccountListFromDDl).AsQueryable()
                    //                        : (StartDateID == "" && EndDateID != "" && !string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate <= endDate && (s.FromAccountID == AccountListFromDDl || s.ToAccountID == AccountListFromDDl)).AsQueryable()
                    //                            : (StartDateID == "" && EndDateID != "" && string.IsNullOrEmpty(AccountList)) ? Transfers.Where(s => s.TransferDate <= endDate).AsQueryable()
                    //                               : Transfers.AsQueryable();

                    if (StartDateID != "" && EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.TransferDate >= startDate && s.TransferDate <= endDate).AsQueryable();
                    }
                    else if (StartDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.TransferDate >= startDate).AsQueryable();
                    }
                    else if (EndDateID != "")
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => s.TransferDate <= endDate).AsQueryable();
                    }
                    else
                    {

                    }

                    if (!string.IsNullOrEmpty(AccountList))
                    {
                        firstPartOfQuery = firstPartOfQuery.Where(s => (s.FromAccountID == AccountListFromDDl || s.ToAccountID == AccountListFromDDl)).AsQueryable();
                    }

                    var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                    {

                        ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p =>  p.Description.ToString().ToLower().Contains(search.ToLower())
                        || p.Amount.ToString().ToLower().Contains(search.ToLower())
                        ).Count() : 0;

                        secondPartOfQuery = secondPartOfQuery.Where(p => p.Description.ToString().ToLower().Contains(search.ToLower())
                                                                         ).AsEnumerable();
                    }
                    if (secondPartOfQuery.Count() > 0)
                    {
                        totalRecords = secondPartOfQuery.AsEnumerable().Count();
                        data = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(

                                s => new CustomAccountListReport
                                {
                                    ReportID = s.AccountListVsAmountTransferID,
                                    Date = s.TransferDate,
                                    AccountListName = s.FromAccountID != 0 ? db.AccountList.Where(a => a.AccountListID == s.FromAccountID).FirstOrDefault().AccountTitle : db.AccountList.Where(a => a.AccountListID == s.ToAccountID).FirstOrDefault().AccountTitle,
                                    transactionType = s.TransferType,
                                    Amount = s.Amount,
                                    Description = s.Description,
                                    Debit = s.FromAccountID != 0 ? s.Amount : 0,
                                    Credit = s.ToAccountID != 0 ? s.Amount : 0,
                                    Button = GetButtonForTransfer(s),
                                })
                            .ToList();

                    }

                    data = this.SortByColumnWithOrder(order, orderDir, data);
                    recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondPartOfQuery.AsEnumerable().Count();

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = recFilter,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    result = this.Json(new
                    {
                        draw = 0,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = data
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        private List<CustomAccountListReport> SortByColumnWithOrder(string order, string orderDir, List<CustomAccountListReport> data)
        {
            // Initialization.   
            List<CustomAccountListReport> lst = new List<CustomAccountListReport>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReportID).ToList() : data.OrderBy(p => p.ReportID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccountListName).ToList() : data.OrderBy(p => p.AccountListName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Debit).ToList() : data.OrderBy(p => p.Debit).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Credit).ToList() : data.OrderBy(p => p.Credit).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReportID).ToList() : data.OrderBy(p => p.ReportID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }

        private string GetButtonForDeposit(Deposit Deposits)
        {
            string s = "<a class='glyphicon glyphicon-edit btn-circle btn-default' href='/Deposit/Manage?id=" + Deposits.DepositID + "'></a>";

            return s;


        }

        private string GetButtonForExpense(Expense expense)
        {
            string s = "<a class='glyphicon glyphicon-edit btn-circle btn-default' href='/Expense/Manage?id=" + expense.ExpenseID + "'></a>";
            return s;
        }

        private string GetButtonForTransfer(AccountListVsAmountTransfer transfer)
        {
            AccountListVsAmountTransfer accountListVsAmount = new AccountListVsAmountTransfer();


            if (transfer.FromAccountID != 0)
            {
                accountListVsAmount = db.AccountListVsAmountTransfer.Where(x => x.Description == transfer.Description && x.TransferDate == transfer.TransferDate && x.Amount == transfer.Amount && x.References == transfer.References && x.FromAccountID == transfer.FromAccountID).FirstOrDefault();
            }
            if (transfer.ToAccountID != 0)
            {
                accountListVsAmount = db.AccountListVsAmountTransfer.Where(x => x.Description == transfer.Description && x.TransferDate == transfer.TransferDate && x.Amount == transfer.Amount && x.References == transfer.References && x.ToAccountID == transfer.ToAccountID).FirstOrDefault();
            }

            string s = "<a class='glyphicon glyphicon-edit btn-circle btn-default' href='/AccountListVsAmountTransfer/Manage?id=" + accountListVsAmount.AccountListVsAmountTransferID + "'></a>";

            return s;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetHeadList(int tid)
        {
            int HeadType = 0;
            if (tid == 1)
            {
                HeadType = 2;
            }
            else if (tid == 2)
            {
                HeadType = 1;
            }
            ViewBag.HeadList = new SelectList(db.Head.Where(s => s.HeadTypeID == HeadType && s.Status == AppUtils.TableStatusIsActive), "HeadID", "HeadeName");
            return PartialView("GetHeadList");
        }

    }
}