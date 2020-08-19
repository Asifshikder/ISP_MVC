using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using ISP_ManagementSystemModel.ViewModel.CustomClass;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ISP_ManagementSystemModel.AppUtils;
//using static ISP_ManagementSystemModel.JSON_Antiforgery_Token_Validation;
//using Project_ISP.JSON_Antiforgery_Token_Validation;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using static Project_ISP.JSON_Antiforgery_Token_Validation;
using Project_ISP;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    public class PurchaseController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: Purchase
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Purchase)]
        public ActionResult Add()
        {
            ViewBag.lstPurchaseStatus = new SelectList(Enum.GetValues(typeof(ItemFor)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PurchaseStatus), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag.lstSupplier = new SelectList(db.Supplier.Select(x => new { x.SupplierID, x.SupplierName }).ToList(), "SupplierID", "SupplierName");

            List<SelectListItem> lstSelectListItem = new List<SelectListItem>();
            lstSelectListItem.Add(new SelectListItem() { Text = "BDT", Value = "1" });
            lstSelectListItem.Add(new SelectListItem() { Text = "USD", Value = "2" });
            ViewBag.lstCurrencyID = new SelectList(lstSelectListItem, "Value", "Text", 1);

            ViewBag.PublishStatus = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PublishStatus), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return View(new VM_PurchaseAndDetails());
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Purchase)]
        public ActionResult Edit(int pid)
        {
            VM_PurchaseAndDetails vM_PurchaseAndDetails = new VM_PurchaseAndDetails();
            vM_PurchaseAndDetails.purchase = db.Purchase.Include("Supplier").Where(x => x.PurchaseID == pid).FirstOrDefault();
            vM_PurchaseAndDetails.purchasedeatils = db.PurchaseDeatils.Include("Item").Where(x => x.PurchaseID == pid && x.Status != AppUtils.TableStatusIsDelete).ToList();

            ViewBag.lstPurchaseStatus = new SelectList(Enum.GetValues(typeof(ItemFor)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PurchaseStatus), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text", vM_PurchaseAndDetails.purchase.PurchaseStatus);

            ViewBag.lstSupplier = new SelectList(db.Supplier.Select(x => new { x.SupplierID, x.SupplierName }).ToList(), "SupplierID", "SupplierName", vM_PurchaseAndDetails.purchase.SupplierID);

            List<SelectListItem> lstSelectListItem = new List<SelectListItem>();
            lstSelectListItem.Add(new SelectListItem() { Text = "BDT", Value = "1" });
            lstSelectListItem.Add(new SelectListItem() { Text = "USD", Value = "2" });
            ViewBag.lstCurrencyID = new SelectList(lstSelectListItem, "Value", "Text", 1);

            ViewBag.PublishStatus = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PublishStatus), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text", vM_PurchaseAndDetails.purchase.PublishStatus);

            return View(vM_PurchaseAndDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetItemList()
        {
            var lstItem = db.Item.Select(x => new { x.ItemID, x.ItemName }).ToList();
            return Json(new { ItemList = lstItem }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Purchase)]
        public ActionResult SavePurchase(VM_PurchaseAndDetails VM_PurchaseAndDetails)
        {
            if (VM_PurchaseAndDetails.purchase.PurchaseID == 0)
            {
                Purchase purchase = db.Purchase.Where(x => x.InvoiceID.ToLower().ToString() == VM_PurchaseAndDetails.purchase.InvoiceID.ToLower().ToString()).FirstOrDefault();
                if (purchase != null)
                {
                    return Json(new { Success = false, Message = "Sorry Invoice Id Already Exist. Please Add Different One." }, JsonRequestBehavior.AllowGet);
                }
                if (VM_PurchaseAndDetails.purchasedeatils.Count() == 0)
                {
                    return Json(new { Success = false, Message = "Add Item In Purchsase List." }, JsonRequestBehavior.AllowGet);
                }

                VM_PurchaseAndDetails.purchase.Total = VM_PurchaseAndDetails.purchasedeatils.Sum(x => x.Price);
                //if (VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount > 0)
                //{
                //    if (VM_PurchaseAndDetails.purchase.DiscountType == 1)
                //    {
                //        VM_PurchaseAndDetails.purchase.DiscountAmount = (VM_PurchaseAndDetails.purchase.Total / 100) * VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount;
                //    }
                //    else
                //    {
                //        VM_PurchaseAndDetails.purchase.DiscountAmount = VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount;
                //    }
                //}
                if (VM_PurchaseAndDetails.purchase.Discount > 0)
                {
                    if (VM_PurchaseAndDetails.purchase.DiscountType == 1)
                    {
                        VM_PurchaseAndDetails.purchase.DiscountAmount = (VM_PurchaseAndDetails.purchase.Total / 100) * VM_PurchaseAndDetails.purchase.Discount;
                    }
                    else
                    {
                        VM_PurchaseAndDetails.purchase.DiscountAmount = VM_PurchaseAndDetails.purchase.Discount;
                    }
                }
                VM_PurchaseAndDetails.purchase.SubTotal = VM_PurchaseAndDetails.purchase.Total - VM_PurchaseAndDetails.purchase.DiscountAmount;
                if (VM_PurchaseAndDetails.purchase.IssuedAt.Year == 1)
                {
                    VM_PurchaseAndDetails.purchase.IssuedAt = AppUtils.GetDateNow();
                }
                VM_PurchaseAndDetails.purchase.Status = AppUtils.TableStatusIsActive;
                VM_PurchaseAndDetails.purchase.CreateBy = AppUtils.GetLoginUserID();
                VM_PurchaseAndDetails.purchase.CreateDate = AppUtils.GetDateTimeNow();

                db.Purchase.Add(VM_PurchaseAndDetails.purchase);
                db.SaveChanges();

                if (VM_PurchaseAndDetails.purchase.PurchaseID > 0)
                {
                    foreach (var item in VM_PurchaseAndDetails.purchasedeatils)
                    {
                        item.PurchaseID = VM_PurchaseAndDetails.purchase.PurchaseID;
                        item.Status = AppUtils.TableStatusIsActive;
                        item.CreateBy = AppUtils.GetLoginUserID();
                        item.CreateDate = AppUtils.GetDateTimeNow();
                    }
                    db.PurchaseDeatils.AddRange(VM_PurchaseAndDetails.purchasedeatils);
                    db.SaveChanges();

                    AccountingHistory accountingHistory = new AccountingHistory();
                    //Mode 1 mean Create 2 mean Update
                    SetInformationForAccountHistory(ref accountingHistory, VM_PurchaseAndDetails.purchase, 1);
                    db.AccountingHistory.Add(accountingHistory);
                    db.SaveChanges();
                }
                return Json(new { Type = "create", Success = true, Message = "Purchase Saved Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Purchase purchase = db.Purchase.Where(x => x.InvoiceID.ToLower().ToString() == VM_PurchaseAndDetails.purchase.InvoiceID.ToLower().ToString() && x.PurchaseID != VM_PurchaseAndDetails.purchase.PurchaseID).FirstOrDefault();
                if (purchase != null)
                {
                    return Json(new { Success = false, Message = "Sorry Invoice Id Already Exist. Please Add Different One." }, JsonRequestBehavior.AllowGet);
                }
                if (VM_PurchaseAndDetails.purchasedeatils.Count() == 0)
                {
                    return Json(new { Success = false, Message = "Add Item In Purchsase List." }, JsonRequestBehavior.AllowGet);
                }

                Purchase purchaseDB = db.Purchase.Where(x => x.PurchaseID == VM_PurchaseAndDetails.purchase.PurchaseID).FirstOrDefault();
                List<PurchaseDeatils> lstNewPurchaseDetails = VM_PurchaseAndDetails.purchasedeatils.Where(x => x.PurchaseDeatilsID == 0).ToList();
                List<PurchaseDeatils> lstOldPurchaseDeatils = VM_PurchaseAndDetails.purchasedeatils.Where(x => x.PurchaseDeatilsID > 0).ToList();

                //deleting item if delete from client
                List<int> lstOldPurchaseDetails = lstOldPurchaseDeatils.Select(x => x.PurchaseDeatilsID).ToList();
                List<PurchaseDeatils> lstDeletePurchaseDetails = db.PurchaseDeatils
                .Where(x => !lstOldPurchaseDetails.Contains(x.PurchaseDeatilsID) && x.PurchaseID == VM_PurchaseAndDetails.purchase.PurchaseID && x.Status != AppUtils.TableStatusIsDelete).ToList();
                if (lstDeletePurchaseDetails.Count() > 0)
                {
                    lstDeletePurchaseDetails.ForEach(x => { x.Status = AppUtils.TableStatusIsDelete; x.DeleteBy = AppUtils.GetLoginUserID(); x.DeleteDate = AppUtils.GetDateTimeNow(); });
                    db.SaveChanges();
                }
                //delete done

                // Updating Part
                foreach (var item in lstOldPurchaseDeatils)
                {
                    var purchaseDetails = db.PurchaseDeatils.Where(x => x.PurchaseDeatilsID == item.PurchaseDeatilsID).FirstOrDefault();
                    purchaseDetails.Serial = item.Serial;
                    purchaseDetails.Price = item.Price;
                    purchaseDetails.HasWarrenty = item.HasWarrenty;
                    purchaseDetails.WarrentyStart = item.WarrentyStart;
                    purchaseDetails.WarrentyEnd = item.WarrentyEnd;
                    purchaseDetails.UpdateBy = AppUtils.GetLoginUserID();
                    purchaseDetails.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(purchaseDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                // Update Done

                //Create New Details In Purchase
                foreach (var item in lstNewPurchaseDetails)
                {
                    item.PurchaseID = VM_PurchaseAndDetails.purchase.PurchaseID;
                    item.CreateBy = AppUtils.GetLoginUserID();
                    item.CreateDate = AppUtils.GetDateTimeNow();
                    db.PurchaseDeatils.Add(item);
                    db.SaveChanges();
                }
                //Create Done

                //Updating Purchase
                purchaseDB.Total = VM_PurchaseAndDetails.purchasedeatils.Where(x => x.Status != AppUtils.TableStatusIsDelete).Sum(x => x.Price);

                //if (VM_PurchaseAndDetails.purchase.DiscountType == 1)
                //{
                //    purchaseDB.DiscountPercentOrFixedAmount = Math.Round(VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount);
                //    purchaseDB.DiscountAmount = Math.Round((purchaseDB.Total / 100) * VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount);
                //}
                //else
                //{
                //    purchaseDB.DiscountPercentOrFixedAmount = Math.Round(VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount);
                //    purchaseDB.DiscountAmount = Math.Round(VM_PurchaseAndDetails.purchase.DiscountPercentOrFixedAmount);
                //}

                if (VM_PurchaseAndDetails.purchase.Discount > 0)
                {
                    if (VM_PurchaseAndDetails.purchase.DiscountType == 1)
                    {
                        purchaseDB.DiscountAmount = Math.Round((purchaseDB.Total / 100) * VM_PurchaseAndDetails.purchase.Discount);
                    }
                    else
                    {
                        purchaseDB.DiscountAmount = Math.Round(VM_PurchaseAndDetails.purchase.Discount);
                    }
                }
                purchaseDB.SubTotal = Math.Round(purchaseDB.Total - purchaseDB.DiscountAmount);
                purchaseDB.DiscountType = VM_PurchaseAndDetails.purchase.DiscountType;
                purchaseDB.UpdateBy = AppUtils.GetLoginUserID();
                purchaseDB.UpdateDate = AppUtils.GetDateTimeNow();
                purchaseDB.SupplierNoted = VM_PurchaseAndDetails.purchase.SupplierNoted;
                db.Entry(purchaseDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //Updating Purchase Done


                AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.PurchaseID == purchaseDB.PurchaseID).FirstOrDefault();
                //Mode 1 mean Create 2 mean Update
                SetInformationForAccountHistory(ref accountingHistory, purchaseDB, 1);
                db.Entry(purchaseDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { Type = "update", Success = true, Message = "Purchase Update Successfully." }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Purchase)]
        public ActionResult PurchaseList()
        {
            ViewBag.lstPurchaseStatus = new SelectList(Enum.GetValues(typeof(ItemFor)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PurchaseStatus), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "Active", Value = AppUtils.TableStatusIsActive.ToString() });
            selectListItems.Add(new SelectListItem() { Text = "Delete", Value = AppUtils.TableStatusIsDelete.ToString() });
            ViewBag.lstTableStatus = new SelectList(selectListItems, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllPurchaseAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                VM_CustomPurchaseList_Percentage VM_CustomPurchaseList_Percentage = new VM_CustomPurchaseList_Percentage();
                DateTime StartDateTime = AppUtils.GetDateNow();
                DateTime EndDateTime = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.GetDateNow());
                int PurchaseStatusID = 0;
                int TableStatusID = AppUtils.TableStatusIsActive;

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);


                var clnStartDate = Request.Form.Get("StartDate");
                var clnEndDate = Request.Form.Get("EndDate");
                var clnPurchaseStatus = Request.Form.Get("PurchaseStatus");
                var clnTableStatus = Request.Form.Get("TableStatus");

                if (!string.IsNullOrEmpty(clnStartDate))
                {
                    StartDateTime = DateTime.Parse(clnStartDate);
                }
                if (!string.IsNullOrEmpty(clnEndDate))
                {
                    EndDateTime = DateTime.Parse(clnEndDate);
                }
                if (!string.IsNullOrEmpty(clnPurchaseStatus))
                {
                    PurchaseStatusID = int.Parse(clnPurchaseStatus);
                }
                if (!string.IsNullOrEmpty(clnTableStatus))
                {
                    TableStatusID = int.Parse(clnTableStatus);
                }

                var purchase = db.Purchase.Where(x => (x.IssuedAt >= StartDateTime && x.IssuedAt <= EndDateTime)).AsQueryable();

                if (!string.IsNullOrEmpty(clnPurchaseStatus) && !string.IsNullOrEmpty(clnTableStatus))
                {
                    purchase = purchase.Where(x => x.PurchaseStatus == PurchaseStatusID && x.Status == TableStatusID).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(clnPurchaseStatus))
                {
                    purchase = purchase.Where(x => x.PurchaseStatus == PurchaseStatusID && x.Status == AppUtils.TableStatusIsActive).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(clnTableStatus))
                {
                    purchase = purchase.Where(x => x.Status == TableStatusID).AsQueryable();
                }
                else
                {
                    purchase = purchase.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();
                }

                //var purchase = (!string.IsNullOrEmpty(clnStartDate) && !string.IsNullOrEmpty(clnEndDate) && !string.IsNullOrEmpty(clnPurchaseStatus) && !string.IsNullOrEmpty(clnTableStatus)) ? db.Purchase.Where(x => (x.IssuedAt >= StartDateTime && x.IssuedAt <= EndDateTime && x.PurchaseStatus == PurchaseStatusID && x.Status == TableStatusID)).AsQueryable()
                //    : (!string.IsNullOrEmpty(clnStartDate) && !string.IsNullOrEmpty(clnEndDate) && !string.IsNullOrEmpty(clnPurchaseStatus)) ? db.Purchase.Where(x => (x.IssuedAt >= StartDateTime && x.IssuedAt <= EndDateTime && x.PurchaseStatus == PurchaseStatusID)).AsQueryable()
                //: (!string.IsNullOrEmpty(clnStartDate) && !string.IsNullOrEmpty(clnEndDate)) ? db.Purchase.Where(x => (x.IssuedAt >= StartDateTime && x.IssuedAt <= EndDateTime) && x.Status == TableStatusID).AsQueryable()
                //: db.Purchase.Where(x => (x.IssuedAt >= StartDateTime && x.IssuedAt <= EndDateTime) && x.Status == TableStatusID).AsQueryable();
                double totalAmount = 0, partiallyPaidAmount = 0, DueAmount = 0;
                CalculateAmountPercentage(ref VM_CustomPurchaseList_Percentage, ref totalAmount, ref partiallyPaidAmount, ref DueAmount, purchase);

                int ifSearch = 0;
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (purchase.Any()) ? purchase.Where(p => p.InvoiceID.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    // Apply search   
                    purchase = purchase.Where(p => p.InvoiceID.ToString().ToLower().Contains(search.ToLower())).AsQueryable();
                }


                List<CustomPurchaseList> data =
                    purchase.Any() ? purchase.AsEnumerable().Skip(startRec).Take(pageSize)
                        //.Select new {ss=>ss. }
                        .Select(s =>
                            new CustomPurchaseList
                            {
                                PID = s.PurchaseID,
                                InvoiceID = s.InvoiceID,
                                AccountName = (s.Supplier == null) ? db.Supplier.Find(s.SupplierID).SupplierName : s.Supplier.SupplierName,
                                Amount = s.SubTotal,
                                PurchasePayment = s.PurchasePayment,
                                IssuedAt = s.IssuedAt.ToString(),
                                ProductStatus = Enum.GetName(typeof(PurchaseStatus), s.PurchaseStatus),
                                TableStatus = GetTableStatusDivByStatusID(s.Status),
                                Type = "",
                                PurchaseUpdate = true,
                                Button = GetButtonForPurchaseList(s)

                            })
                        .ToList() : new List<CustomPurchaseList>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = purchase.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : purchase.AsEnumerable().Count();

                ////////////////////////////////////
                bool setPercentage = false;

                if (data.Count() > 0)
                {
                    data[0].PaidPercent = VM_CustomPurchaseList_Percentage.PaidPercent;
                    data[0].UnPaidPercent = VM_CustomPurchaseList_Percentage.UnPaidPercent;
                    data[0].PartiallyPaidPercent = VM_CustomPurchaseList_Percentage.PartiallyPaidPercent;

                    data[0].TotalAmount = totalAmount;
                    data[0].TotalPaidAmount = partiallyPaidAmount;
                    data[0].TotalUnPaidAmount = totalAmount - partiallyPaidAmount;
                    data[0].TotalCancelAmount = 0;

                }
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

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Purchase_Payment)]
        public ActionResult PurchasePayment(int pid)
        {

            VM_PurchaseAndDetails vM_PurchaseAndDetails = new VM_PurchaseAndDetails();
            vM_PurchaseAndDetails.purchase = db.Purchase.Include("Supplier").Where(x => x.PurchaseID == pid).FirstOrDefault();
            vM_PurchaseAndDetails.purchasedeatils = db.PurchaseDeatils.Include("Item").Where(x => x.PurchaseID == pid && x.Status != AppUtils.TableStatusIsDelete).ToList();

            ViewBag.lstAccount = new SelectList(db.AccountList.Select(x => new { x.AccountListID, x.AccountTitle }).ToList(), "AccountListID", "AccountTitle");
            ViewBag.lstPaymentMethod = new SelectList(Enum.GetValues(typeof(PaymentMethod)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(PaymentMethod), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");//new SelectList(db.PaymentBy.Where(x => x.Status != AppUtils.TableStatusIsDelete).Select(x => new { x.PaymentByID, x.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            return View(vM_PurchaseAndDetails);
        }

        private void CalculateAmountPercentage(ref VM_CustomPurchaseList_Percentage vM_CustomPurchaseList_Percentage, ref double totalAmount, ref double partiallyPaidAmount, ref double DueAmount, IQueryable<Purchase> purchase)
        {
            if (purchase.Any())
            {
                totalAmount = purchase.Sum(x => x.SubTotal);
                partiallyPaidAmount = purchase.Sum(x => x.PurchasePayment);
                DueAmount = totalAmount - partiallyPaidAmount;

                vM_CustomPurchaseList_Percentage.PaidPercent = Math.Round((partiallyPaidAmount / totalAmount) * 100);
                vM_CustomPurchaseList_Percentage.UnPaidPercent = Math.Round((DueAmount / totalAmount) * 100);
                vM_CustomPurchaseList_Percentage.PartiallyPaidPercent = Math.Round((partiallyPaidAmount / totalAmount) * 100);
            }
            else
            {
                vM_CustomPurchaseList_Percentage.PaidPercent = 0;
                vM_CustomPurchaseList_Percentage.UnPaidPercent = 0;
                vM_CustomPurchaseList_Percentage.PartiallyPaidPercent = 0;
            }
        }

        private List<CustomPurchaseList> SortByColumnWithOrder(string order, string orderDir, List<CustomPurchaseList> data)
        {
            // Initialization.   
            List<CustomPurchaseList> lst = new List<CustomPurchaseList>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PID).ToList() : data.OrderBy(p => p.PID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.IssuedAt).ToList() : data.OrderBy(p => p.IssuedAt).ToList();
                        break;


                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PID).ToList() : data.OrderBy(p => p.PID).ToList();
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

        //public class SearchPurchasePercentageModel {
        //    public DateTime StartDate { get; set; } 
        //    public DateTime EndDate { get; set; } 
        //    public int PurchaseStatus { get; set; }  
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetPurcahsePercentageData(SearchPurchasePercentageModel model)
        //{
        //    var Package = db.Package.Where(s => s.PackageID == packageID).Select(s => new { PackageName = s.PackageName, PackagePrice = s.PackagePrice, BandWith = s.BandWith, IPPoolID = s.IPPoolID, MikrotikID = s.MikrotikID, LocalAddress = s.LocalAddress, PackageForMyOrResellerUser = s.PackageForMyOrResellerUser }).FirstOrDefault();
        //    //    $("#PackageName").val(PackageJSONParse.PackageName);
        //    //$("#PackagePrice").val(PackageJSONParse.PackagePrice);
        //    //$("#BandWith").val(PackageJSONParse.BandWith);
        //    // var PackageCircularLoopIgnored = AppUtils.IgnoreCircularLoop(Package);

        //    var JSON = Json(new { PackageDetails = Package }, JsonRequestBehavior.AllowGet);
        //    JSON.MaxJsonLength = int.MaxValue;
        //    return JSON;
        //}

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Purchase_Payment)]
        public ActionResult GetPurchaseDuePaymentDetails(GetDetailsByID getDetailsByID)
        {
            if (getDetailsByID != null)
            {
                Purchase purchase = db.Purchase.Find(getDetailsByID.id);
                VM_Purchase_Payment vM_Purchase_Payment = new VM_Purchase_Payment();
                vM_Purchase_Payment.PaidAmount = purchase.PurchasePayment;
                vM_Purchase_Payment.TotalAmount = purchase.Total;
                vM_Purchase_Payment.SubTotalAmount = purchase.SubTotal;
                vM_Purchase_Payment.DueAmount = vM_Purchase_Payment.SubTotalAmount - vM_Purchase_Payment.PaidAmount;
                vM_Purchase_Payment.Payee = db.Supplier.Find(purchase.SupplierID).SupplierName;
                return Json(new { success = true, paymentamount = vM_Purchase_Payment });
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Purchase_Payment)]
        public ActionResult SavePurchasePayment(PurchasePaymentHistory pph)
        {
            AccountList accountList = db.AccountList.Find(pph.AccountListID);
            if (Convert.ToDouble(accountList.InitialBalance.Value) < pph.PaymentAmount)
            {
                return Json(new { Success = false, Message = "Sorry This Account Does Not Have Suffiecient Balance ." }, JsonRequestBehavior.AllowGet);
            }
            accountList.InitialBalance -= Convert.ToDecimal(pph.PaymentAmount);
            db.SaveChanges();
            DateTime dt = new DateTime();
            pph.PurchasePaymentDate = pph.PurchasePaymentDate.AddHours(dt.Hour).AddMinutes(dt.Minute).AddSeconds(dt.Second);
            pph.Status = AppUtils.TableStatusIsActive;
            pph.CreateBy = AppUtils.GetLoginUserID();
            pph.CreateDate = AppUtils.GetDateTimeNow();

            db.PurchasePaymentHistory.Add(pph);
            db.SaveChanges();

            Purchase purchase = db.Purchase.Where(x => x.PurchaseID == pph.PurchaseID).FirstOrDefault();
            purchase.PurchasePayment += pph.PaymentAmount;
            //db.Entry(purchase).State = System.Data.Entity.EntityState.Modified; 
            db.SaveChanges();


            AccountingHistory accountingHistory = new AccountingHistory();
            //Mode 1 mean Create 2 mean Update
            SetInformationForAccountHistory(ref accountingHistory, pph, 1);
            db.AccountingHistory.Add(accountingHistory);
            db.SaveChanges();

            return Json(new { Success = true, Message = "Purchase Payment Added Successfully.", DueAmount = purchase.SubTotal - purchase.PurchasePayment, PaymentStatus = purchase.SubTotal > purchase.PurchasePayment ? "Pending" : "Paid" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllPurchasePaymentAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                int purchaseID = 0;
                DateTime purchasePaymentStartDateFromDDL;
                DateTime purchasePaymentEndDateFromDDL;
                // Initialization.   
                var PurchaseID = Request.Form.Get("pid");
                var PaymentStartDate = Request.Form.Get("PaymentStartDate");
                var PaymentEndDate = Request.Form.Get("PaymentEndDate");

                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (!string.IsNullOrEmpty(PurchaseID))
                {
                    purchaseID = int.Parse(PurchaseID);
                }
                if (!string.IsNullOrEmpty(PaymentStartDate))
                {
                    purchasePaymentStartDateFromDDL = Convert.ToDateTime(PaymentStartDate);
                }
                else
                {
                    purchasePaymentStartDateFromDDL = AppUtils.GetDateNow();
                }
                if (!string.IsNullOrEmpty(PaymentEndDate))
                {
                    purchasePaymentEndDateFromDDL = Convert.ToDateTime(PaymentEndDate).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                }
                else
                {
                    purchasePaymentEndDateFromDDL = AppUtils.GetDateNow().AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                }

                List<PurchaseCustomPaymentInformation> data = new List<PurchaseCustomPaymentInformation>();

                var firstPartOfQuery = db.PurchasePaymentHistory.Include("AccountList").OrderByDescending(x => x.CreateDate).AsQueryable();

                var secontPartOrQuery =
                            firstPartOfQuery
                            .Where(s =>
                                (s.CreateDate >= purchasePaymentStartDateFromDDL && s.CreateDate <= purchasePaymentEndDateFromDDL)
                                ).AsEnumerable();
                //var secondPartOfQuery = firstPartOfQuery
                //          .GroupJoin(db.ClientDetails, ph => ph.ClientDetailsID, ClientDetails => ClientDetails.ClientDetailsID, (PH, ClientDetails) => new
                //          {
                //              Transaction = PH,
                //              ClientDetails = ClientDetails.FirstOrDefault()

                //          })
                //          .AsEnumerable();

                // Verification.   
                //a = firstPartOfQuery.ToList();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    //        var a = secondPartOfQuery.ToList();
                    ifSearch = (secontPartOrQuery.Any()) ? secontPartOrQuery.Where(p => p.CheckNo.ToString().ToLower().Contains(search.ToLower())
                                                                          ).Count() : 0;

                    // Apply search   
                    secontPartOrQuery = secontPartOrQuery.Where(p => p.CheckNo.ToString().ToLower().Contains(search.ToLower())
                                                                          ).AsEnumerable();
                }
                //var a = secontPartOrQuery.ToList();
                if (secontPartOrQuery.Count() > 0)
                {
                    bool hasUpdateAccess = true;
                    bool hasDeleteAccess = true;
                    HasAccessOnPurchasePaymentAction(ref hasUpdateAccess, ref hasDeleteAccess);

                    totalRecords = secontPartOrQuery.Count();
                    data = secontPartOrQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new PurchaseCustomPaymentInformation()
                        {
                            pphid = s.PurchasePaymentHistoryID,
                            PaymentDate = s.PurchasePaymentDate.ToString(),
                            PaymentFrom = s.AccountList.AccountTitle.ToString(),
                            PaymentMethod = Enum.GetName(typeof(PaymentMethod), s.PaymentByID),
                            PaymentAmount = s.PaymentAmount,
                            Status = GetPaymentStatus(s.Status),
                            CheckOrResetNo = s.CheckNo,
                            EntryBy = db.Employee.Find(s.CreateBy).LoginName,
                            Delete_By = !string.IsNullOrEmpty(s.DeleteBy.ToString()) ? db.Employee.Find(s.DeleteBy).LoginName : "",
                            DeleteDate = !string.IsNullOrEmpty(s.DeleteDate.ToString()) ? s.DeleteDate.ToString() : "",
                            Button = GetButton(s.Status, hasUpdateAccess, hasDeleteAccess)
                        }).ToList();

                }

                // Sorting.   
                //lstVM_Paid_History_Employee = this.SortByColumnWithOrderForShowingMyBill(order, orderDir, lstVM_Paid_History_Employee);
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

        private string GetPaymentStatus(int paymentStatus)
        {
            if (paymentStatus == AppUtils.TableStatusIsActive)
            {
                return "<span class='label  label-success'>Active</span>";
            }
            else if (paymentStatus == AppUtils.PaymentStatusIsOnProcess)
            {
                return "<span class='label  label-warning'>Pending</span>";
            }
            else
            {
                return "<span class='label  label-danger'>Removed</span>";
            }
        }
        private void HasAccessOnPurchasePaymentAction(ref bool hasUpdateAccess, ref bool hasDeleteAccess)
        {
            int CurrentUserRightPermission = AppUtils.GetLoginUserRightPermissionID();
            UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();

            List<int> lstRightPerssion = new List<int>();
            lstRightPerssion = userRightPermission.UserRightPermissionDetails.Trim(',').Split(',').Select(int.Parse).ToList();
            if (lstRightPerssion.Contains(int.Parse(AppUtils.Delete_Purchase_Payment)))
            {
                hasDeleteAccess = true;
            }
            //if (lstRightPerssion.Contains(int.Parse(AppUtils.Update_ResellerPayment)))
            //{
            //    hasUpdateAccess = true;
            //}
        }
        private string GetButtonForPurchaseList(Purchase purchase)
        {
            string button = "";
            bool rowIsDeleted = purchase.Status == AppUtils.TableStatusIsDelete ? true : false;

            int CurrentUserRightPermission = AppUtils.GetLoginUserRightPermissionID();
            UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();

            List<int> lstRightPerssion = new List<int>();
            lstRightPerssion = userRightPermission.UserRightPermissionDetails.Trim(',').Split(',').Select(int.Parse).ToList();
            bool addPurchaePayment = false, updatePurchase = false, viewPurchasePayment = false, deletePurchasePayment = false;
            if (rowIsDeleted)
            {
                addPurchaePayment = true; updatePurchase = true; viewPurchasePayment = true; deletePurchasePayment = true;
                //button += "<a target='_blank' href='/purchase/PurchasePayment ? pid = " + purchase.PurchaseID + "' class='btn btn-primary btn-xs' data-original-title='View' id='ViewPurchase'><i class='fa fa-file-text-o'></i></a>   " +
                //    " <a target='_blank' href='/purchase/Edit?pid=" + purchase.PurchaseID + "' class='btn btn-info btn-xs' data-original-title='Edit' id='EditPurchase'><i class='fa fa-pencil'></i></a>" +
                //    " <a href='javascript:void(0)' class='btn btn-primary btn-xs' data-original-title='View' id='btnShowPurchasePaymentHistory'><i class='fa fa-lis'></i></a>         " +
                //    "<a href='#' class='btn btn-danger btn-xs cdelete' data-original-title='Delete' id='btnDeletePurchase' ><i class='fa fa-trash'></i></a>";
            }
            else
            {
                addPurchaePayment = true; updatePurchase = true; viewPurchasePayment = true; deletePurchasePayment = true;
            }

            if (addPurchaePayment)
            {
                if (lstRightPerssion.Contains(int.Parse(AppUtils.Add_Purchase_Payment)))
                {
                    button += "<a target='_blank' href='/purchase/PurchasePayment?pid=" + purchase.PurchaseID + "' class='btn btn-primary btn-xs' data-original-title='View' id='ViewPurchase'><i class='fa fa-file-text-o'></i></a>";
                }
            }
            if (updatePurchase)
            {
                if (lstRightPerssion.Contains(int.Parse(AppUtils.View_Purchase)))
                {
                    button += "<a target='_blank' href='/purchase/Edit?pid=" + purchase.PurchaseID + "' class='btn btn-info btn-xs' data-original-title='Edit' id='EditPurchase'><i class='fa fa-pencil'></i></a>";
                }
            }
            if (viewPurchasePayment)
            {
                if (lstRightPerssion.Contains(int.Parse(AppUtils.View_Purchase_Payment)))
                {
                    button += " <a href='javascript:void(0)' class='btn btn-primary btn-xs' data-original-title='View' id='btnShowPurchasePaymentHistory'><i class='fa fa-list'></i></a> ";
                }
            }
            if (deletePurchasePayment)
            {
                if (lstRightPerssion.Contains(int.Parse(AppUtils.Delete_Purchase_Payment)))
                {
                    button += "<a href='#' class='btn btn-danger btn-xs cdelete' data-original-title='Delete' id='btnDeletePurchase' ><i class='fa fa-trash'></i></a>";
                }
            }

            return button;
        }
        private string GetButton(int PaymentStatus, bool hasUpdateAccess, bool hasDeleteAccess)
        {//s.DeleteBy.HasValue ? "" : 
            string button = "";
            //if (PaymentStatus != AppUtils.PaymentStatusIsReceive && PaymentStatus != AppUtils.PaymentStatusIsDelete)
            //{
            //    if (hasUpdateAccess)
            //    {
            //        button += "<button id='btnPaymentHistoryEdit'  type='button' class='btn btn-default btn-circle'><i class='glyphicon glyphicon-pencil'></i></button>";
            //    }

            //}
            if (PaymentStatus != AppUtils.PaymentStatusIsDelete)
            {
                if (hasDeleteAccess)
                {
                    button += "<button id='btnPaymentHistoryDelete' type='button' class='btn btn-default btn-circle' data-toggle='modal' data-target='#popModalForDeletePermently'><i class='glyphicon glyphicon-remove'></i></button>";
                }
            }
            return button;
        }

        //$("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(5)").html(data.PaymentStatus);
        //    $("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(6)").html(data.DeleteBy);
        //    $("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(6)").html(data.DeleteDate);

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Purchase_Payment)]
        public ActionResult DeletePurchasePaymentHistoryByID(GetDetailsByID getByid)
        {
            try
            {
                PurchasePaymentHistory pphd = db.PurchasePaymentHistory.Find(getByid.id);
                AccountList AccountList = db.AccountList.Find(pphd.AccountListID);

                pphd.DeleteBy = AppUtils.GetLoginUserID();
                pphd.DeleteDate = AppUtils.GetDateTimeNow();
                pphd.Status = AppUtils.TableStatusIsDelete;
                db.SaveChanges();

                AccountList.InitialBalance += Convert.ToDecimal(pphd.PaymentAmount);
                AccountList.UpdateBy = AppUtils.GetLoginUserID();
                AccountList.UpdateDate = AppUtils.GetDateTimeNow();
                db.SaveChanges();

                Purchase purchase = db.Purchase.Find(pphd.PurchaseID);
                purchase.PurchasePayment -= pphd.PaymentAmount;
                db.SaveChanges();


                AccountingHistory accountingHistory = new AccountingHistory();
                //Mode 1 mean Create 2 mean Update
                SetInformationForAccountHistory(ref accountingHistory, pphd, 1);
                db.AccountingHistory.Add(accountingHistory);
                db.SaveChanges();

                return Json(new { DeleteStatus = true, PaymentStatus = GetPaymentStatus(pphd.Status), DeleteBy = db.Employee.Find(pphd.DeleteBy).LoginName, DeleteDate = pphd.DeleteDate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Purchase)]
        public ActionResult DeletePurchaseByID(GetDetailsByID getByid)
        {
            try
            {//we are deleting the purchase information but data will and remaining same for purchase and refunding the amount to his own account mean in AccountList Table

                Purchase purchase = db.Purchase.Find(getByid.id);
                purchase.DeleteBy = AppUtils.GetLoginUserID();
                purchase.DeleteDate = AppUtils.GetDateTimeNow();
                purchase.Status = AppUtils.TableStatusIsDelete;
                db.SaveChanges();
                var lstPurchasePaymentDetails = db.Purchase.Where(x => x.PurchaseID == purchase.PurchaseID)
                                         .GroupJoin(db.PurchasePaymentHistory.Where(x => x.Status != AppUtils.TableStatusIsDelete), ph => ph.PurchaseID, pph => pph.PurchaseID, (ph, pph) => new
                                         {
                                             ph = ph,
                                             pph = pph.GroupBy(x => x.AccountListID)
                                         })
                                         .ToList();

                foreach (var purchasePaymentInGroup in lstPurchasePaymentDetails)
                {
                    foreach (var purchasePament in purchasePaymentInGroup.pph)
                    {
                        AccountList accountList = db.AccountList.Find(purchasePament.FirstOrDefault().AccountListID);
                        accountList.InitialBalance += Convert.ToDecimal(purchasePament.ToList().Sum(x => x.PaymentAmount));
                        foreach (var paymentHistory in purchasePament)
                        {
                            paymentHistory.DeleteByParent = true;
                        }
                        db.SaveChanges();
                    }
                }

                return Json(new { DeleteStatus = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, }, JsonRequestBehavior.AllowGet);
            }
        }


        private void SetInformationForAccountHistory(ref AccountingHistory accountingHistory, Purchase Purchase, int CreateOrUpdate)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            accountingHistory.Amount = Convert.ToDouble(Purchase.SubTotal);
            if (CreateOrUpdate == 1)//mean create
            {
                accountingHistory.PurchaseID = Purchase.PurchaseID;
                accountingHistory.ActionTypeID = (int)AppUtils.AccountingHistoryType.Purchase;
                accountingHistory.Date = AppUtils.GetDateTimeNow();
                accountingHistory.DRCRTypeID = (int)AppUtils.AccountTransactionType.DR;
                accountingHistory.Description = Purchase.Subject;
                accountingHistory.Year = dt.Year;
                accountingHistory.Month = dt.Month;
                accountingHistory.Day = dt.Day;
                accountingHistory.CreateBy = AppUtils.GetLoginUserID();
                accountingHistory.CreateDate = dt;
                accountingHistory.Status = AppUtils.TableStatusIsActive;
            }
            else
            {
                accountingHistory.UpdateBy = AppUtils.GetLoginUserID();
                accountingHistory.UpdateDate = dt;
            }
        }
        private void SetInformationForAccountHistory(ref AccountingHistory accountingHistory, PurchasePaymentHistory PurchasePaymentHistory, int CreateOrUpdate)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            accountingHistory.Amount = Convert.ToDouble(PurchasePaymentHistory.PaymentAmount);
            if (CreateOrUpdate == 1)//mean create
            {
                accountingHistory.PurchaseID = PurchasePaymentHistory.PurchaseID;
                accountingHistory.ActionTypeID = (int)AppUtils.AccountingHistoryType.Purchase;
                accountingHistory.Date = AppUtils.GetDateTimeNow();
                accountingHistory.DRCRTypeID = (int)AppUtils.AccountTransactionType.DR;
                accountingHistory.Description = PurchasePaymentHistory.Description;
                accountingHistory.Year = dt.Year;
                accountingHistory.Month = dt.Month;
                accountingHistory.Day = dt.Day;
                accountingHistory.CreateBy = AppUtils.GetLoginUserID();
                accountingHistory.CreateDate = dt;
                accountingHistory.Status = AppUtils.TableStatusIsActive;
            }
            else
            {
                accountingHistory.UpdateBy = AppUtils.GetLoginUserID();
                accountingHistory.UpdateDate = dt;
            }
        }
    }
}