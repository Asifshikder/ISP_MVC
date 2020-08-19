using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using Project_ISP.Migrations;
using Project_ISP.Models;
using Project_ISP.ViewModel;
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
using AccountingHistory = Project_ISP.Models.AccountingHistory;
using Deposit = Project_ISP.Models.Deposit;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class DepositController : Controller
    {
        // GET: Deposit
        private ISPContext db = new ISPContext();
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Deposit)]
        public ActionResult Index()
        {
            List<SelectListItem> StatusType = new List<SelectListItem>();
            StatusType.Add(new SelectListItem() { Text = "Cleared", Value = "1" });
            StatusType.Add(new SelectListItem() { Text = "Uncleared", Value = "2" });
            ViewBag.StatusType = new SelectList(StatusType, "Value", "Text");
            ViewBag.PaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
            ViewBag.NewPaymentBy = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName");
            ViewBag.Head = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive && s.HeadTypeID == 2), "HeadID", "HeadeName");
            ViewBag.AccountList = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle");
            ViewBag.Company = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            ViewBag.NewCompany = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllDepositAjaxData()
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
                var Deposit = db.Deposit.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

                Deposit = Deposit.OrderByDescending(x => x.DepositDate).AsQueryable();
                int ifSearch = 0;
                List<DepositViewModel> data = new List<DepositViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (Deposit.Any()) ? Deposit.Where(p => p.DepositID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.DepositDate.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Amount.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Description.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.DepositStatus.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    Deposit = Deposit.Where(p =>
                    p.DepositID.ToString().ToLower().Contains(search.ToLower())
                    || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                    || p.DepositDate.ToString().ToLower().Contains(search.ToLower())
                    || p.Amount.ToString().ToLower().Contains(search.ToLower())
                    || p.DepositStatus.ToString().ToLower().Contains(search.ToLower())
                    || p.Description.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = Deposit.Any() ? Deposit.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new DepositViewModel
                            {
                                DepositID = x.DepositID,
                                Description = x.Description,
                                CompanyName = x.Company.CompanyName,
                                Amount = x.Amount,
                                DepositDate = x.DepositDate,
                                Status = x.DepositStatus,
                                UpdateDeposit = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Deposit) ? true : false
                            })
                        .ToList() : new List<DepositViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = Deposit.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : Deposit.AsEnumerable().Count();

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
        private List<DepositViewModel> SortByColumnWithOrder(string order, string orderDir, List<DepositViewModel> data)
        {
            // Initialization.   
            List<DepositViewModel> lst = new List<DepositViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DepositID).ToList() : data.OrderBy(p => p.DepositID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DepositDate).ToList() : data.OrderBy(p => p.DepositDate).ToList();
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DepositDate).ToList() : data.OrderBy(p => p.DepositDate).ToList();
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
        public ActionResult GetPayerList(int CompanyID)
        {
            ViewBag.PayerList = new SelectList(db.CompanyVSPayer.Where(x => x.CompanyID == CompanyID && x.Status == AppUtils.TableStatusIsActive), "PayerID", "PayerName");
            return PartialView("GetPayerList");
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Create_Deposit)]
        public ActionResult InsertNewDeposit(FormCollection form, HttpPostedFileBase DescriptionImage)
        {
            Deposit DepositInfo = JsonConvert.DeserializeObject<Deposit>(form["NewDepositInformation"]);
            Deposit DepositReturn = new Deposit();

            try
            {
                DepositInfo.Status = AppUtils.TableStatusIsActive;
                DepositInfo.CreateBy = AppUtils.GetLoginUserID();
                DepositInfo.CreateDate = AppUtils.GetDateTimeNow();
                DepositReturn = db.Deposit.Add(DepositInfo);
                db.SaveChanges();
                if (DescriptionImage != null)
                {
                    SaveImageInFolderAndAddInformationInVendorTable(ref DepositInfo, "Description", DescriptionImage);
                }
                if (DepositReturn.DepositID > 0)
                {
                    db.SaveChanges();

                    AccountingHistory accountingHistory = new AccountingHistory();
                    //Mode 1 mean Create 2 mean Update
                    SetInformationForAccountHistory(ref accountingHistory, DepositReturn, 1);
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
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Deposit)]
        public ActionResult Manage(int id)
        {
            var Deposit = db.Deposit.Where(x => x.DepositID == id && x.Status == AppUtils.TableStatusIsActive).FirstOrDefault();
            List<SelectListItem> StatusType = new List<SelectListItem>();
            StatusType.Add(new SelectListItem() { Text = "Cleared", Value = "1" });
            StatusType.Add(new SelectListItem() { Text = "Uncleared", Value = "2" });
            ViewBag.DepositStatus = new SelectList(StatusType, "Value", "Text", Deposit.DepositStatus);
            ViewBag.HeadID = new SelectList(db.Head.Where(s => s.Status == AppUtils.TableStatusIsActive && s.HeadTypeID == 2), "HeadID", "HeadeName", Deposit.HeadID);
            ViewBag.CompanyID = new SelectList(db.Company.Where(s => s.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName", Deposit.CompanyID);
            ViewBag.AccountListID = new SelectList(db.AccountList.Where(s => s.Status == AppUtils.TableStatusIsActive), "AccountListID", "AccountTitle", Deposit.AccountListID);
            var a = db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Deposit.CompanyID).ToList();
            ViewBag.PayerID = new SelectList(db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Deposit.CompanyID), "PayerID", "PayerName", Deposit.PayerID);
            ViewBag.Payer = new SelectList(db.CompanyVSPayer.Where(s => s.Status == AppUtils.TableStatusIsActive && s.CompanyID == Deposit.CompanyID), "PayerID", "PayerName", Deposit.PayerID);
            ViewBag.PaymentByID = new SelectList(db.PaymentBy.Where(s => s.Status == AppUtils.TableStatusIsActive), "PaymentByID", "PaymentByName", Deposit.PaymentByID); ;
            return View(Deposit);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_Deposit)]
        public ActionResult UpdateDeposit(FormCollection form, HttpPostedFileBase DepositUpdateImage)
        {
            Deposit Deposit_Details = JsonConvert.DeserializeObject<Deposit>(form["Deposit_details"]);
            Deposit Deposit_Db = db.Deposit.Where(s => s.DepositID == Deposit_Details.DepositID).FirstOrDefault();

            try
            {
                AddGivenImageInCurrentRow(ref Deposit_Db, Deposit_Details, "DescriptionFile", DepositUpdateImage, form["DescriptionFilePath"]);

                if (Deposit_Db.DepositID > 0)
                {
                    SetClientDepositToDatabaseDB(ref Deposit_Db, Deposit_Details);

                    db.Entry(Deposit_Db).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.DepositID == Deposit_Db.DepositID).FirstOrDefault();
                    //Mode 1 mean Create 2 mean Update
                    SetInformationForAccountHistory(ref accountingHistory, Deposit_Db, 2);
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

        private void SetClientDepositToDatabaseDB(ref Deposit Deposit_Db, Deposit Deposit_Details)
        {
            Deposit_Db.AccountListID = Deposit_Details.AccountListID;
            Deposit_Db.DepositDate = Deposit_Details.DepositDate;
            Deposit_Db.Description = Deposit_Details.Description;
            Deposit_Db.Amount = Deposit_Details.Amount;
            Deposit_Db.HeadID = Deposit_Details.HeadID;
            Deposit_Db.CompanyID = Deposit_Details.CompanyID;
            Deposit_Db.PayerID = Deposit_Details.PayerID;
            Deposit_Db.PaymentByID = Deposit_Details.PaymentByID;
            Deposit_Db.DepositStatus = Deposit_Details.DepositStatus;
            Deposit_Db.References = Deposit_Details.References;
            Deposit_Db.UpdateBy = AppUtils.GetLoginUserID();
            Deposit_Db.UpdateDate = AppUtils.GetDateTimeNow();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Deposit)]
        public ActionResult DeleteDeposit(int ID)
        {
            try
            {
                var deposit = db.Deposit.Where(s => s.DepositID == ID).FirstOrDefault();
                deposit.DeleteBy = AppUtils.GetLoginUserID();
                deposit.DeleteDate = AppUtils.GetDateTimeNow();
                deposit.Status = AppUtils.TableStatusIsDelete;
                db.Entry(deposit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                AccountingHistory accountingHistory = db.AccountingHistory.Where(x => x.DepositID == deposit.DepositID).FirstOrDefault();
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

        #region Add image 
        private void AddGivenImageInCurrentRow(ref Deposit deposit_Db, Deposit deposit_Details, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "DescriptionFile")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref deposit_Db, deposit_Details, "DescriptionFile", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    deposit_Details.DescriptionFilePath = deposit_Db.DescriptionFilePath;
                }
                else
                {
                    RemoveImageFromServerFolder(type, deposit_Db);
                    deposit_Db.DescriptionFilePath = null;
                    deposit_Db.DescriptionFileByte = null;
                }
            }
        }
        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Deposit deposit_Db, Deposit deposit_Details, string type, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(type, deposit_Db);



            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = deposit_Db.DepositID + "_" + type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/Deposit"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (type == "DescriptionFile")
            {
                deposit_Db.DescriptionFilePath = "/images/Deposit/" + fileName;

            }
        }
        private void RemoveImageFromServerFolder(string type, Deposit deposit_Db)
        {

            string removeImageName = "";
            if (type == "DescriptionFile")
            {
                removeImageName = !string.IsNullOrEmpty(deposit_Db.DescriptionFilePath) ? deposit_Db.DescriptionFilePath.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/images/Deposit/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        private void SaveImageInFolderAndAddInformationInVendorTable(ref Deposit depositInfo, string Type, HttpPostedFileBase descriptionImage)
        {
            if (!IsValidContentType(descriptionImage.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(descriptionImage.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(descriptionImage.FileName);
            string extension = Path.GetExtension(descriptionImage.FileName);
            var fileName = depositInfo.DepositID + "_" + Type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/Deposit"), fileName);
            descriptionImage.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(descriptionImage.InputStream);
            imagebyte = reader.ReadBytes(descriptionImage.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            depositInfo.DescriptionFileByte = imagebyte;
            depositInfo.DescriptionFilePath = "/images/Deposit/" + fileName;
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
         
        private void SetInformationForAccountHistory(ref Models.AccountingHistory accountingHistory, Deposit Deposit, int CreateOrUpdate)
        {
            DateTime dt = AppUtils.GetDateTimeNow();
            accountingHistory.Amount = Convert.ToDouble(Deposit.Amount);
            if (CreateOrUpdate == 1)//mean create
            {
                accountingHistory.DepositID = Deposit.DepositID;
                accountingHistory.ActionTypeID = (int)AppUtils.AccountingHistoryType.Deposit;
                accountingHistory.Date = AppUtils.GetDateTimeNow();
                accountingHistory.DRCRTypeID = (int)AppUtils.AccountTransactionType.CR;
                accountingHistory.Description = !string.IsNullOrEmpty(Deposit.Description) ? Deposit.Description : db.Head.Find(Deposit.HeadID).HeadeName;
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