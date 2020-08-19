using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;
using Project_ISP.Models;
using Project_ISP.ViewModel;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ExpenseController : Controller
    {
        private ISPContext db = new ISPContext();

        // GET: Expense
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Expense_List)]
        public ActionResult Index()
        {
            List<SelectListItem> StatusType = new List<SelectListItem>();
            StatusType.Add(new SelectListItem() { Text = "Cleared", Value = "1" });
            StatusType.Add(new SelectListItem() { Text = "Uncleared", Value = "2" });
            ViewBag.StatusType = new SelectList(StatusType, "Value", "Text");
            ViewBag.PaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
            ViewBag.NewPaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
            ViewBag.Head = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive && s.HeadTypeID == 1), "HeadID", "HeadeName");
            ViewBag.NewHead = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive), "HeadID", "HeadeName");
            ViewBag.AccountList = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
            ViewBag.Company = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            ViewBag.NewCompany = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllExpenseAjaxData()
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
                var Expense = db.Expenses.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

                Expense = Expense.OrderByDescending(x => x.PaymentDate).AsQueryable();
                int ifSearch = 0;
                List<ExpenseViewModel> data = new List<ExpenseViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (Expense.Any()) ? Expense.Where(p => p.ExpenseID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.PaymentDate.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Amount.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Descriptions.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.ExpenseStatus.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    Expense = Expense.Where(p =>
                    p.ExpenseID.ToString().ToLower().Contains(search.ToLower())
                    || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                    || p.PaymentDate.ToString().ToLower().Contains(search.ToLower())
                    || p.Amount.ToString().ToLower().Contains(search.ToLower())
                    || p.ExpenseStatus.ToString().ToLower().Contains(search.ToLower())
                    || p.Descriptions.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = Expense.Any() ? Expense.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new ExpenseViewModel
                            {
                                ExpenseID = x.ExpenseID,
                                Description = x.Descriptions,
                                CompanyName = x.Company.CompanyName,
                                Amount = x.Amount,
                                ExpenseDate = x.PaymentDate,
                                Status = x.ExpenseStatus,
                                UpdateExpense = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Expense) ? true : false
                            })
                        .ToList() : new List<ExpenseViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = Expense.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : Expense.AsEnumerable().Count();

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
        private List<ExpenseViewModel> SortByColumnWithOrder(string order, string orderDir, List<ExpenseViewModel> data)
        {
            // Initialization.   
            List<ExpenseViewModel> lst = new List<ExpenseViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpenseID).ToList() : data.OrderBy(p => p.ExpenseID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpenseDate).ToList() : data.OrderBy(p => p.ExpenseDate).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyName).ToList() : data.OrderBy(p => p.CompanyName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpenseDate).ToList() : data.OrderBy(p => p.ExpenseDate).ToList();
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
        public ActionResult GetRecieverList(int CompanyID)
        {
            ViewBag.RecieverList = new SelectList(db.CompanyVSPayer.Where(x => x.CompanyID == CompanyID && x.Status == AppUtils.TableStatusIsActive), "PayerID", "PayerName");
            return PartialView("GetRecieverList");
        } 

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Expense)]
        public ActionResult InsertNewExpense(FormCollection form, HttpPostedFileBase DescriptionImage)
        { 
            Expense ExpenseInfo = JsonConvert.DeserializeObject<Expense>(form["NewExpenseInformation"]); 
            Expense ExpenseReturn = new Expense();

            try
            { 
                ExpenseInfo.Status = AppUtils.TableStatusIsActive;
                ExpenseInfo.CreateBy = AppUtils.GetLoginUserID();
                ExpenseInfo.CreateDate = AppUtils.GetDateTimeNow();
                ExpenseReturn = db.Expenses.Add(ExpenseInfo);
                db.SaveChanges();
                if (DescriptionImage != null)
                {
                    SaveImageInFolderAndAddInformationInVendorTable(ref ExpenseInfo, "DescriptionFile", DescriptionImage);
                }
                if (ExpenseReturn.ExpenseID > 0)
                {
                    db.SaveChanges();

                    AccountingHistory accountingHistory = new AccountingHistory();
                    //Mode 1 mean Create 2 mean Update
                    SetInformationForAccountHistory(ref accountingHistory, ExpenseReturn, 1);
                    db.AccountingHistory.Add(accountingHistory);
                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Expense)]
        public ActionResult Manage(int id)
        {
            var Expense = db.Expenses.Where(x => x.ExpenseID == id && x.Status != AppUtils.TableStatusIsDelete).FirstOrDefault();
            List<SelectListItem> StatusType = new List<SelectListItem>();
            StatusType.Add(new SelectListItem() { Text = "Cleared", Value = "1" });
            StatusType.Add(new SelectListItem() { Text = "Uncleared", Value = "2" });
            ViewBag.ExpenseStatus = new SelectList(StatusType, "Value", "Text", Expense.ExpenseStatus);
            ViewBag.HeadID = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive && s.HeadTypeID == 1), "HeadID", "HeadeName", Expense.HeadID);
            ViewBag.CompanyID = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName", Expense.CompanyID);
            ViewBag.AccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", Expense.AccountListID);
            var a = db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Expense.CompanyID).ToList();
            ViewBag.PayerID = new SelectList(db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Expense.CompanyID), "PayerID", "PayerName", Expense.PayerID);
            ViewBag.Payer = new SelectList(db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Expense.CompanyID), "PayerID", "PayerName", Expense.PayerID);
            ViewBag.PaymentByID = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName", Expense.PaymentByID); ;
            return View(Expense);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Expense)]
        public ActionResult UpdateExpense(FormCollection form, HttpPostedFileBase ExpenseUpdateImage)
        { 
            Expense Expense_Details = JsonConvert.DeserializeObject<Expense>(form["Expense_details"]);
            Expense Expense_Db = db.Expenses.Where(s => s.ExpenseID == Expense_Details.ExpenseID).FirstOrDefault();

            try
            {
                AddGivenImageInCurrentRow(ref Expense_Db, Expense_Details, "DescriptionFile", ExpenseUpdateImage, form["DescriptionFilePath"]);
                 
                if (Expense_Db.ExpenseID > 0)
                {
                    SetClientExpenseToDatabaseDB(ref Expense_Db, Expense_Details); 
                    db.Entry(Expense_Db).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                     
                    AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.DepositID == Expense_Db.ExpenseID).FirstOrDefault();
                    //Mode 1 mean Create 2 mean Update
                    SetInformationForAccountHistory(ref accountingHistory, Expense_Db, 2);
                    db.AccountingHistory.Add(accountingHistory);
                    db.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetClientExpenseToDatabaseDB(ref Expense Expense_Db, Expense Expense_Details)
        {
            Expense_Db.AccountListID = Expense_Details.AccountListID;
            Expense_Db.PaymentDate = Expense_Details.PaymentDate;
            Expense_Db.Descriptions = Expense_Details.Descriptions;
            Expense_Db.Amount = Expense_Details.Amount;
            Expense_Db.HeadID = Expense_Details.HeadID;
            Expense_Db.CompanyID = Expense_Details.CompanyID;
            Expense_Db.PayerID = Expense_Details.PayerID;
            Expense_Db.PaymentByID = Expense_Details.PaymentByID;
            Expense_Db.ExpenseStatus = Expense_Details.ExpenseStatus;
            Expense_Db.References = Expense_Details.References;
            Expense_Db.UpdateBy = AppUtils.GetLoginUserID();
            Expense_Db.UpdateDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Expense)]
        public ActionResult DeleteExpense(int ID)
        {
            try
            {
                var Expense = db.Expenses.Where(s => s.ExpenseID == ID).FirstOrDefault();
                Expense.DeleteBy = AppUtils.GetLoginUserID();
                Expense.DeleteDate = AppUtils.GetDateTimeNow();
                Expense.Status = AppUtils.TableStatusIsDelete;
                db.Entry(Expense).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(); 

                AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.ExpenseID == Expense.ExpenseID).FirstOrDefault();
                accountingHistory.Status = AppUtils.TableStatusIsDelete;
                db.SaveChanges();

                var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                var JSON = Json(new { success = false }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            } 
        }

        #region image
        private void AddGivenImageInCurrentRow(ref Expense Expense_Db, Expense Expense_Details, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "DescriptionFile")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Expense_Db, Expense_Details, "DescriptionFile", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    Expense_Details.DescriptionFilePath = Expense_Db.DescriptionFilePath;
                }
                else
                {
                    RemoveImageFromServerFolder(type, Expense_Db);
                    Expense_Db.DescriptionFilePath = null;
                    Expense_Db.DescriptionFileByte = null;
                }
            }
        }
        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Expense Expense_Db, Expense Expense_Details, string type, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(type, Expense_Db);



            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = Expense_Db.ExpenseID + "_" + type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/Expense"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (type == "DescriptionFile")
            {
                Expense_Db.DescriptionFilePath = "/images/Expense/" + fileName;

            }
        }
        private void RemoveImageFromServerFolder(string type, Expense Expense_Db)
        {

            string removeImageName = "";
            if (type == "DescriptionFile")
            {
                removeImageName = !string.IsNullOrEmpty(Expense_Db.DescriptionFilePath) ? Expense_Db.DescriptionFilePath.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/images/Expense/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        private void SaveImageInFolderAndAddInformationInVendorTable(ref Expense ExpenseInfo, string Type, HttpPostedFileBase descriptionImage)
        {
            if (!IsValidContentType(descriptionImage.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(descriptionImage.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(descriptionImage.FileName);
            string extension = Path.GetExtension(descriptionImage.FileName);
            var fileName = ExpenseInfo.ExpenseID + "_" + Type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/Expense"), fileName);
            descriptionImage.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(descriptionImage.InputStream);
            imagebyte = reader.ReadBytes(descriptionImage.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            ExpenseInfo.DescriptionFileByte = imagebyte;
            ExpenseInfo.DescriptionFilePath = "/images/Expense/" + fileName;
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
        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }
        #endregion 

        private void SetInformationForAccountHistory(ref Models.AccountingHistory accountingHistory, Expense Expense, int CreateOrUpdate)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            accountingHistory.Amount = Convert.ToDouble(Expense.Amount);
            if (CreateOrUpdate == 1)//mean create
            {
                accountingHistory.ExpenseID = Expense.ExpenseID;
                accountingHistory.ActionTypeID = (int)AppUtils.AccountingHistoryType.Deposit;
                accountingHistory.Date = AppUtils.GetDateTimeNow();
                accountingHistory.DRCRTypeID = (int)AppUtils.AccountTransactionType.CR;
                accountingHistory.Description = !string.IsNullOrEmpty(Expense.Descriptions) ? Expense.Descriptions : db.Head.Find(Expense.HeadID).HeadeName;
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