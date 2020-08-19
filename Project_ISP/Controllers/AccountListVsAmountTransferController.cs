//using ISP_ManagementSystemModel;
//using ISP_ManagementSystemModel.Models;
//using Project_ISP.Models;
//using Project_ISP.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using static Project_ISP.JSON_Antiforgery_Token_Validation;

//namespace Project_ISP.Controllers
//{

//    public class AmountCheck
//    {
//        public int TransferID { get; set; }
//        public int FromAccountID { get; set; }
//        public int ToAccountID { get; set; }
//        public decimal TransferAmount { get; set; }

//    }

//    [SessionTimeout]
//    [AjaxAuthorizeAttribute]
//    public class AccountListVsAmountTransferController : Controller
//    {

//        private ISPContext db = new ISPContext();
//        // GET: AccountListVsAmountTransfer
//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_AccountListVsAmountTransfer)]
//        public ActionResult Index()
//        {
//            List<SelectListItem> Currency = new List<SelectListItem>();
//            Currency.Add(new SelectListItem() { Text = "BDT", Value = "1" });
//            Currency.Add(new SelectListItem() { Text = "USD", Value = "2" });
//            ViewBag.CurrencyID = new SelectList(Currency, "Value", "Text");
//            ViewBag.PaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
//            ViewBag.FromAccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
//            ViewBag.ToAccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult GetAllAmountTransferAjaxData()
//        {
//            JsonResult result = new JsonResult();
//            try
//            {
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
//                var Transfer = db.AccountListVsAmountTransfer.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

//                Transfer = Transfer.OrderByDescending(x => x.TransferDate).AsQueryable();
//                int ifSearch = 0;
//                List<AccountListVSAmountTransferViewModel> data = new List<AccountListVSAmountTransferViewModel>();
//                if (!string.IsNullOrEmpty(search) &&
//                    !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (Transfer.Any()) ? Transfer.Where(p => p.AccountListVsAmountTransferID.ToString().ToLower().Contains(search.ToLower())
//                                                                                  //|| p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
//                                                                                  //|| p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.Amount.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.Description.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


//                    Transfer = Transfer.Where(p =>
//                    p.AccountListVsAmountTransferID.ToString().ToLower().Contains(search.ToLower())
//                    //|| p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
//                    //|| p.AccountList.AccountTitle.ToString().ToLower().Contains(search.ToLower())
//                    || p.Amount.ToString().ToLower().Contains(search.ToLower())
//                    || p.Description.ToString().ToLower().Contains(search.ToLower())
//                    ).AsQueryable();
//                }
//                data = Transfer.Any() ? Transfer.AsEnumerable().Skip(startRec).Take(pageSize)
//                        .Select(
//                            x => new AccountListVSAmountTransferViewModel
//                            {
//                                AccountListVsAmountTransferID = x.AccountListVsAmountTransferID,
//                                Description = x.Description,
//                                FromAccount = x.AccountList.AccountTitle,
//                                ToAccount = db.AccountList.Where(s => s.AccountListID == x.ToAccountID).FirstOrDefault().AccountTitle,
//                                TransferDate = x.TransferDate,
//                                Amount = x.Amount,
//                                UpdateTansfer = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_AccountListVsAmountTransfer) ? true : false
//                            })
//                        .ToList() : new List<AccountListVSAmountTransferViewModel>();

//                // Sorting.   
//                data = this.SortByColumnWithOrder(order, orderDir, data);
//                // Total record count.   
//                int totalRecords = Transfer.AsEnumerable().Count();
//                // Filter record count.   
//                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : Transfer.AsEnumerable().Count();

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = data
//                }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                // Info   
//                Console.Write(ex);
//            }
//            // Return info.   
//            return result;
//        }

//        private List<AccountListVSAmountTransferViewModel> SortByColumnWithOrder(string order, string orderDir, List<AccountListVSAmountTransferViewModel> data)
//        {
//            // Initialization.   
//            List<AccountListVSAmountTransferViewModel> lst = new List<AccountListVSAmountTransferViewModel>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {

//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccountListVsAmountTransferID).ToList() : data.OrderBy(p => p.AccountListVsAmountTransferID).ToList();
//                        break;
//                    case "1":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
//                        break;
//                    case "2":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FromAccount).ToList() : data.OrderBy(p => p.FromAccount).ToList();
//                        break;
//                    case "3":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ToAccount).ToList() : data.OrderBy(p => p.ToAccount).ToList();
//                        break;
//                    case "4":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
//                        break;

//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransferDate).ToList() : data.OrderBy(p => p.TransferDate).ToList();
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                // info.   
//                Console.Write(ex);
//            }
//            // info.   
//            return lst;
//        }




//        [HttpPost]
//        public JsonResult InsertAccountListVsAmountTransfer(AccountListVsAmountTransfer AccountListVsAmountTransferDetails)
//        {

//            try
//            {
//                AccountListVsAmountTransferDetails.CreateBy = AppUtils.GetLoginUserID();
//                AccountListVsAmountTransferDetails.CreateDate = AppUtils.GetDateTimeNow();
//                AccountListVsAmountTransferDetails.Status = AppUtils.TableStatusIsActive;
//                AccountListVsAmountTransferDetails.CurrencyID = 1;

//                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == AccountListVsAmountTransferDetails.AccountListID).FirstOrDefault();
//                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == AccountListVsAmountTransferDetails.ToAccountID).FirstOrDefault();

//                FromAccountForAmountChange.InitialBalance = (FromAccountForAmountChange.InitialBalance - AccountListVsAmountTransferDetails.Amount);
//                ToAccountForAmountChange.InitialBalance = (ToAccountForAmountChange.InitialBalance + AccountListVsAmountTransferDetails.Amount);

//                db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                db.AccountListVsAmountTransfer.Add(AccountListVsAmountTransferDetails);
//                db.SaveChanges();


//                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

//            }
//            catch
//            {
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }


//        }

//        [HttpGet]
//        public ActionResult Manage(int id)
//        {
//            var TransferDetails = db.AccountListVsAmountTransfer.Where(x => x.AccountListVsAmountTransferID == id && x.Status == AppUtils.TableStatusIsActive).FirstOrDefault();
//            List<SelectListItem> CurrencyID = new List<SelectListItem>();
//            CurrencyID.Add(new SelectListItem() { Text = "BDT", Value = "1" });
//            CurrencyID.Add(new SelectListItem() { Text = "USD", Value = "2" });
//            ViewBag.CurrencyID = new SelectList(CurrencyID, "Value", "Text", TransferDetails.CurrencyID);
//            ViewBag.PaymentByID = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName", TransferDetails.PaymentByID);
//            ViewBag.AccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", TransferDetails.AccountListID);
//            ViewBag.ToAccountID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", TransferDetails.ToAccountID);
//            return View(TransferDetails);
//        }

//        [HttpPost]
//        [ValidateJsonAntiForgeryTokenAttribute]
//        public ActionResult InsertAmountCheck(AmountCheck amountforchecking)
//        {
//            var CheckAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.FromAccountID).FirstOrDefault();
//            if (CheckAccount.InitialBalance >= amountforchecking.TransferAmount)
//            {
//                return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
//            }
//            else
//            {
//                return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
//            }
//        }


//        [HttpPost]
//        [ValidateJsonAntiForgeryTokenAttribute]
//        public ActionResult UpdateAmountCheck(AmountCheck amountforchecking)
//        {
//            var CheckFromAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.ToAccountID).FirstOrDefault();
//            var CheckToAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.ToAccountID).FirstOrDefault();
//            var TransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == amountforchecking.TransferID).FirstOrDefault();
//            if (TransferDetails.Amount >= amountforchecking.TransferAmount)
//            {
//                if (CheckToAccount.InitialBalance >= amountforchecking.TransferAmount)
//                {
//                    return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            else if (TransferDetails.Amount <= amountforchecking.TransferAmount)
//            {
//                if (CheckFromAccount.InitialBalance >= amountforchecking.TransferAmount)
//                {
//                    return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            else
//            {
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpPost]
//        [ValidateJsonAntiForgeryTokenAttribute]
//        public ActionResult UpdateAccountListVsAmountTransfer(AccountListVsAmountTransfer accountlistvsamounttransferdetails)
//        {
//            try
//            {
//                AccountListVsAmountTransfer dbTransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == accountlistvsamounttransferdetails.AccountListVsAmountTransferID).FirstOrDefault();

//                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == accountlistvsamounttransferdetails.AccountListID).FirstOrDefault();
//                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == accountlistvsamounttransferdetails.ToAccountID).FirstOrDefault();

//                SetTransferInformationDuringUpdate(ref dbTransferDetails, ref FromAccountForAmountChange, ref ToAccountForAmountChange, accountlistvsamounttransferdetails);

//                db.Entry(dbTransferDetails).State = System.Data.Entity.EntityState.Modified;
//                //db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                //db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        private void SetTransferInformationDuringUpdate(ref AccountListVsAmountTransfer dbTransferDetails, ref AccountList fromAccountForAmountChange, ref AccountList toAccountForAmountChange, AccountListVsAmountTransfer accountlistvsamounttransferdetails)
//        {
//            //dbTransferDetails.AccountListID = accountlistvsamounttransferdetails.AccountListID;
//            //dbTransferDetails.ToAccountID = accountlistvsamounttransferdetails.ToAccountID;
//            dbTransferDetails.TransferDate = accountlistvsamounttransferdetails.TransferDate;
//            dbTransferDetails.Description = accountlistvsamounttransferdetails.Description;
//            dbTransferDetails.PaymentByID = accountlistvsamounttransferdetails.PaymentByID;
//            dbTransferDetails.References = accountlistvsamounttransferdetails.References;
//            dbTransferDetails.UpdateBy = AppUtils.GetLoginUserID();
//            dbTransferDetails.UpdateDate = AppUtils.GetDateNow();
//            //if (dbTransferDetails.Amount > accountlistvsamounttransferdetails.Amount)
//            //{
//            //    var ReducedAmount = dbTransferDetails.Amount - accountlistvsamounttransferdetails.Amount;
//            //    FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance + ReducedAmount;
//            //    ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance - ReducedAmount;
//            //}
//            //else if (dbTransferDetails.Amount < accountlistvsamounttransferdetails.Amount)
//            //{
//            //    var AdditionalAmount = dbTransferDetails.Amount - accountlistvsamounttransferdetails.Amount;
//            //    FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance - AdditionalAmount;
//            //    ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance + AdditionalAmount;
//            //}

//            //dbTransferDetails.Amount = accountlistvsamounttransferdetails.Amount;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteTransfer(int id)
//        {
//            try
//            {
//                AccountListVsAmountTransfer TransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == id).FirstOrDefault();

//                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == TransferDetails.AccountListID).FirstOrDefault();
//                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == TransferDetails.ToAccountID).FirstOrDefault();
//                if (ToAccountForAmountChange.InitialBalance < TransferDetails.Amount)
//                {
//                    return Json(new { success = false, message = "Sorry Balance is not avialable for transfer." }, JsonRequestBehavior.AllowGet);
//                }

//                TransferDetails.DeleteBy = AppUtils.LoginUserID;
//                TransferDetails.DeleteDate = AppUtils.GetDateNow();
//                TransferDetails.Status = AppUtils.TableStatusIsDelete;
//                FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance + TransferDetails.Amount;
//                ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance - TransferDetails.Amount;


//                db.Entry(TransferDetails).State = System.Data.Entity.EntityState.Modified;
//                db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//                TempData["message"] = "Information Removed Successfully";
//                return Json(new { success = true }, JsonRequestBehavior.AllowGet);


//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = "Error occourd. Please contact with adminstrator." }, JsonRequestBehavior.AllowGet);
//            }
//        }
//    }
//}

using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{

    public class AmountCheck
    {
        public int TransferID { get; set; }
        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }
        public decimal TransferAmount { get; set; }

    }

    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class AccountListVsAmountTransferController : Controller
    {

        private ISPContext db = new ISPContext();
        // GET: AccountListVsAmountTransfer
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_AccountListVsAmountTransfer)]
        public ActionResult Index()
        {
            List<SelectListItem> Currency = new List<SelectListItem>();
            Currency.Add(new SelectListItem() { Text = "BDT", Value = "1" });
            Currency.Add(new SelectListItem() { Text = "USD", Value = "2" });
            ViewBag.CurrencyID = new SelectList(Currency, "Value", "Text");
            ViewBag.PaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
            ViewBag.FromAccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
            ViewBag.ToAccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllAmountTransferAjaxData()
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
                var Transfer = db.AccountListVsAmountTransfer.Where(x => x.Status == AppUtils.TableStatusIsActive && x.BreakDownAccountListID != 0).AsQueryable();

                Transfer = Transfer.OrderByDescending(x => x.TransferDate).AsQueryable();
                int ifSearch = 0;
                List<AccountListVSAmountTransferViewModel> data = new List<AccountListVSAmountTransferViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (Transfer.Any()) ? Transfer.Where(p => p.AccountListVsAmountTransferID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Amount.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Description.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    Transfer = Transfer.Where(p =>
                    p.AccountListVsAmountTransferID.ToString().ToLower().Contains(search.ToLower())
                    || p.Amount.ToString().ToLower().Contains(search.ToLower())
                    || p.Description.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = Transfer.Any() ? Transfer.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x =>
                            new AccountListVSAmountTransferViewModel
                            {
                                AccountListVsAmountTransferID = x.AccountListVsAmountTransferID,
                                Description = x.Description,
                                FromAccount = db.AccountList.Find(x.FromAccountID).AccountTitle,
                                ToAccount = db.AccountList.Where(s => s.AccountListID == x.ToAccountID).FirstOrDefault().AccountTitle,
                                TransferDate = x.TransferDate,
                                Amount = x.Amount,
                                UpdateTansfer = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_AccountListVsAmountTransfer) ? true : false
                            })
                        .ToList() : new List<AccountListVSAmountTransferViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = Transfer.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : Transfer.AsEnumerable().Count();

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

        private List<AccountListVSAmountTransferViewModel> SortByColumnWithOrder(string order, string orderDir, List<AccountListVSAmountTransferViewModel> data)
        {
            // Initialization.   
            List<AccountListVSAmountTransferViewModel> lst = new List<AccountListVSAmountTransferViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AccountListVsAmountTransferID).ToList() : data.OrderBy(p => p.AccountListVsAmountTransferID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FromAccount).ToList() : data.OrderBy(p => p.FromAccount).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ToAccount).ToList() : data.OrderBy(p => p.ToAccount).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransferDate).ToList() : data.OrderBy(p => p.TransferDate).ToList();
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
        public JsonResult InsertAccountListVsAmountTransfer(AccountListVsAmountTransfer AccountListVsAmountTransferDetails)
        {

            try
            {
                AccountListVsAmountTransferDetails.CreateBy = AppUtils.GetLoginUserID();
                AccountListVsAmountTransferDetails.CreateDate = AppUtils.GetDateTimeNow();
                AccountListVsAmountTransferDetails.Status = AppUtils.TableStatusIsActive;
                AccountListVsAmountTransferDetails.CurrencyID = 1;

                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == AccountListVsAmountTransferDetails.FromAccountID).FirstOrDefault();
                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == AccountListVsAmountTransferDetails.ToAccountID).FirstOrDefault();

                FromAccountForAmountChange.InitialBalance = (FromAccountForAmountChange.InitialBalance - AccountListVsAmountTransferDetails.Amount);
                ToAccountForAmountChange.InitialBalance = (ToAccountForAmountChange.InitialBalance + AccountListVsAmountTransferDetails.Amount);

                db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                db.AccountListVsAmountTransfer.Add(AccountListVsAmountTransferDetails);
                db.SaveChanges();

                if (AccountListVsAmountTransferDetails.AccountListVsAmountTransferID > 0)
                {
                    AccountListVsAmountTransfer AccountListVsAmountTransferIn = new AccountListVsAmountTransfer();
                    AccountListVsAmountTransfer AccountListVsAmountTransferOut = new AccountListVsAmountTransfer();
                    SetInOutDateDuringCreate(ref AccountListVsAmountTransferIn, ref AccountListVsAmountTransferOut, AccountListVsAmountTransferDetails);
                    db.AccountListVsAmountTransfer.Add(AccountListVsAmountTransferIn);
                    db.AccountListVsAmountTransfer.Add(AccountListVsAmountTransferOut);
                    db.SaveChanges();
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        private void SetInOutDateDuringCreate(ref AccountListVsAmountTransfer accountListVsAmountTransferIn, ref AccountListVsAmountTransfer accountListVsAmountTransferOut, AccountListVsAmountTransfer cl)
        {
            accountListVsAmountTransferIn.BreakDownAccountListID = cl.AccountListVsAmountTransferID;
            accountListVsAmountTransferIn.TransferType = "In";
            accountListVsAmountTransferIn.Amount = cl.Amount;
            accountListVsAmountTransferIn.CreateBy = cl.CreateBy;
            accountListVsAmountTransferIn.CreateDate = cl.CreateDate;
            accountListVsAmountTransferIn.CurrencyID = cl.CurrencyID;
            accountListVsAmountTransferIn.Description = cl.Description;
            accountListVsAmountTransferIn.ToAccountID = cl.ToAccountID;
            accountListVsAmountTransferIn.PaymentByID = cl.PaymentByID;
            accountListVsAmountTransferIn.References = cl.References;
            accountListVsAmountTransferIn.Status = cl.Status;
            accountListVsAmountTransferIn.Tags = cl.Tags;
            accountListVsAmountTransferIn.TransferDate = cl.TransferDate;


            accountListVsAmountTransferOut.BreakDownAccountListID = cl.AccountListVsAmountTransferID;
            accountListVsAmountTransferOut.TransferType = "Out";
            accountListVsAmountTransferOut.Amount = cl.Amount;
            accountListVsAmountTransferOut.CreateBy = cl.CreateBy;
            accountListVsAmountTransferOut.CreateDate = cl.CreateDate;
            accountListVsAmountTransferOut.CurrencyID = cl.CurrencyID;
            accountListVsAmountTransferOut.Description = cl.Description;
            accountListVsAmountTransferOut.FromAccountID = cl.FromAccountID;
            accountListVsAmountTransferOut.PaymentByID = cl.PaymentByID;
            accountListVsAmountTransferOut.References = cl.References;
            accountListVsAmountTransferOut.Status = cl.Status;
            accountListVsAmountTransferOut.Tags = cl.Tags;
            accountListVsAmountTransferOut.TransferDate = cl.TransferDate;
        }

        [HttpGet]
        public ActionResult Manage(int id)
        {
            var TransferDetails = db.AccountListVsAmountTransfer.Where(x => x.AccountListVsAmountTransferID == id && x.Status == AppUtils.TableStatusIsActive).FirstOrDefault();
            List<SelectListItem> CurrencyID = new List<SelectListItem>();
            CurrencyID.Add(new SelectListItem() { Text = "BDT", Value = "1" });
            CurrencyID.Add(new SelectListItem() { Text = "USD", Value = "2" });
            ViewBag.CurrencyID = new SelectList(CurrencyID, "Value", "Text", TransferDetails.CurrencyID);
            ViewBag.PaymentByID = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName", TransferDetails.PaymentByID);
            ViewBag.AccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", TransferDetails.FromAccountID);
            ViewBag.ToAccountID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", TransferDetails.ToAccountID);
            return View(TransferDetails);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult InsertAmountCheck(AmountCheck amountforchecking)
        {
            var CheckAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.FromAccountID).FirstOrDefault();
            if (CheckAccount.InitialBalance >= amountforchecking.TransferAmount)
            {
                return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateAmountCheck(AmountCheck amountforchecking)
        {
            var CheckFromAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.ToAccountID).FirstOrDefault();
            var CheckToAccount = db.AccountList.Where(s => s.AccountListID == amountforchecking.ToAccountID).FirstOrDefault();
            var TransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == amountforchecking.TransferID).FirstOrDefault();
            if (TransferDetails.Amount >= amountforchecking.TransferAmount)
            {
                if (CheckToAccount.InitialBalance >= amountforchecking.TransferAmount)
                {
                    return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (TransferDetails.Amount <= amountforchecking.TransferAmount)
            {
                if (CheckFromAccount.InitialBalance >= amountforchecking.TransferAmount)
                {
                    return Json(new { InsufficientBalance = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { InsufficientBalance = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateAccountListVsAmountTransfer(AccountListVsAmountTransfer accountlistvsamounttransferdetails)
        {
            try
            {
                AccountListVsAmountTransfer dbTransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == accountlistvsamounttransferdetails.AccountListVsAmountTransferID).FirstOrDefault();

                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == accountlistvsamounttransferdetails.FromAccountID).FirstOrDefault();
                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == accountlistvsamounttransferdetails.ToAccountID).FirstOrDefault();

                SetTransferInformationDuringUpdate(ref dbTransferDetails, ref FromAccountForAmountChange, ref ToAccountForAmountChange, accountlistvsamounttransferdetails);

                db.Entry(dbTransferDetails).State = System.Data.Entity.EntityState.Modified;
                //db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                //db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetTransferInformationDuringUpdate(ref AccountListVsAmountTransfer dbTransferDetails, ref AccountList fromAccountForAmountChange, ref AccountList toAccountForAmountChange, AccountListVsAmountTransfer accountlistvsamounttransferdetails)
        {
            //dbTransferDetails.AccountListID = accountlistvsamounttransferdetails.AccountListID;
            //dbTransferDetails.ToAccountID = accountlistvsamounttransferdetails.ToAccountID;
            dbTransferDetails.TransferDate = accountlistvsamounttransferdetails.TransferDate;
            dbTransferDetails.Description = accountlistvsamounttransferdetails.Description;
            dbTransferDetails.PaymentByID = accountlistvsamounttransferdetails.PaymentByID;
            dbTransferDetails.References = accountlistvsamounttransferdetails.References;
            dbTransferDetails.UpdateBy = AppUtils.GetLoginUserID();
            dbTransferDetails.UpdateDate = AppUtils.GetDateNow();
            //if (dbTransferDetails.Amount > accountlistvsamounttransferdetails.Amount)
            //{
            //    var ReducedAmount = dbTransferDetails.Amount - accountlistvsamounttransferdetails.Amount;
            //    FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance + ReducedAmount;
            //    ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance - ReducedAmount;
            //}
            //else if (dbTransferDetails.Amount < accountlistvsamounttransferdetails.Amount)
            //{
            //    var AdditionalAmount = dbTransferDetails.Amount - accountlistvsamounttransferdetails.Amount;
            //    FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance - AdditionalAmount;
            //    ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance + AdditionalAmount;
            //}

            //dbTransferDetails.Amount = accountlistvsamounttransferdetails.Amount;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTransfer(int id)
        {
            try
            {
                AccountListVsAmountTransfer TransferDetails = db.AccountListVsAmountTransfer.Where(s => s.AccountListVsAmountTransferID == id).FirstOrDefault();

                var FromAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == TransferDetails.FromAccountID).FirstOrDefault();
                var ToAccountForAmountChange = db.AccountList.Where(s => s.AccountListID == TransferDetails.ToAccountID).FirstOrDefault();
                if (ToAccountForAmountChange.InitialBalance < TransferDetails.Amount)
                {
                    return Json(new { success = false, message = "Sorry Balance is not avialable for transfer." }, JsonRequestBehavior.AllowGet);
                }

                TransferDetails.DeleteBy = AppUtils.LoginUserID;
                TransferDetails.DeleteDate = AppUtils.GetDateNow();
                TransferDetails.Status = AppUtils.TableStatusIsDelete;
                FromAccountForAmountChange.InitialBalance = FromAccountForAmountChange.InitialBalance + TransferDetails.Amount;
                ToAccountForAmountChange.InitialBalance = ToAccountForAmountChange.InitialBalance - TransferDetails.Amount;


                db.Entry(TransferDetails).State = System.Data.Entity.EntityState.Modified;
                db.Entry(FromAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                db.Entry(ToAccountForAmountChange).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Information Removed Successfully";
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occourd. Please contact with adminstrator." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}