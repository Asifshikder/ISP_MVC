using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
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

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class CompanyController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: Company
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Company)]
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCompanyAjaxData()
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
                var company = db.Company.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<CompanyViewModel> data = new List<CompanyViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (company.Any()) ? company.Where(p => p.CompanyID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.CompanyEmail.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.CompanyAddress.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Phone.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.ContactPerson.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    company = company.Where(p =>
                    p.CompanyID.ToString().ToLower().Contains(search.ToLower())
                    || p.CompanyName.ToString().ToLower().Contains(search.ToLower())
                    || p.CompanyEmail.ToString().ToLower().Contains(search.ToLower())
                    || p.CompanyAddress.ToString().ToLower().Contains(search.ToLower())
                    || p.Phone.ToString().ToLower().Contains(search.ToLower())
                    || p.ContactPerson.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = company.Any() ? company.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new CompanyViewModel
                            {
                                CompanyID = x.CompanyID,
                                CompanyName = x.CompanyName,
                                CompanyEmail = x.CompanyEmail,
                                ContactPerson = x.ContactPerson,
                                Phone = x.Phone,
                                CompanyAddress = x.CompanyAddress,
                                CompanyLogoPath = x.CompanyLogoPath,
                                UpdateCompany = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Company) ? true : false
                            })
                        .ToList() : new List<CompanyViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = company.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : company.AsEnumerable().Count();

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

        private List<CompanyViewModel> SortByColumnWithOrder(string order, string orderDir, List<CompanyViewModel> data)
        {
            // Initialization.   
            List<CompanyViewModel> lst = new List<CompanyViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyID).ToList() : data.OrderBy(p => p.CompanyID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyName).ToList() : data.OrderBy(p => p.CompanyName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyEmail).ToList() : data.OrderBy(p => p.CompanyEmail).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyAddress).ToList() : data.OrderBy(p => p.CompanyAddress).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactPerson).ToList() : data.OrderBy(p => p.ContactPerson).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Phone).ToList() : data.OrderBy(p => p.Phone).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyID).ToList() : data.OrderBy(p => p.CompanyID).ToList();
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

        public ActionResult InsertCompanyFromPopUp(FormCollection form, HttpPostedFileBase CompanyImage)
        {

            Company Company_info = JsonConvert.DeserializeObject<Company>(form["CompanyDetails"]);
            Company Company_Check = db.Company.Where(s => s.CompanyName == Company_info.CompanyName.Trim() && s.Status != AppUtils.TableStatusIsDelete).FirstOrDefault();

            if (Company_Check != null)
            {
                return Json(new { success = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Company CompanyReturn = new Company();

            try
            {

                Company_info.Status = AppUtils.TableStatusIsActive;
                Company_info.CreateBy = AppUtils.GetLoginUserID();
                Company_info.CreateDate = AppUtils.GetDateTimeNow();
                CompanyReturn = db.Company.Add(Company_info);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInCompanyTable(ref Company_info, "LOGO", CompanyImage);
                if (CompanyReturn.CompanyID > 0)
                {
                    db.SaveChanges();
                    return Json(new { success = true, }, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDetailsByID(int CompanyID)
        {
            var Company = db.Company.Where(s => s.CompanyID == CompanyID).Select(s => new
            {
                CompanyID = s.CompanyID,
                CompanyName = s.CompanyName,
                CompanyEmail = s.CompanyEmail,
                CompanyAddress = s.CompanyAddress,
                ContactPerson = s.ContactPerson,
                Phone = s.Phone,
                CompanyLogoPath = s.CompanyLogoPath
            }).ToList().Select(
                s =>
                new
                {
                    CompanyID = s.CompanyID,
                    CompanyName = s.CompanyName,
                    CompanyEmail = s.CompanyEmail,
                    CompanyAddress = s.CompanyAddress,
                    ContactPerson = s.ContactPerson,
                    Phone = s.Phone,
                    CompanyLogoPath = s.CompanyLogoPath
                }).FirstOrDefault();

            var JSON = Json(new { Company = Company, success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateCompanyFromPopUp(FormCollection form, HttpPostedFileBase CompanyImageUpdate)
        {

            Company Company_details = JsonConvert.DeserializeObject<Company>(form["Company_details"]);
            Company Company_DB = db.Company.Where(s => s.CompanyID == Company_details.CompanyID).FirstOrDefault();
            Company Company_Check = db.Company.Where(s => s.CompanyName == Company_details.CompanyName.Trim()).FirstOrDefault();
            if (Company_details.CompanyName != Company_DB.CompanyName)
            {
                if (Company_Check != null)
                {
                    return Json(new { success = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }
            }


            try
            {
                if (CompanyImageUpdate != null)
                {
                    AddGivenImageInCurrentRow(ref Company_DB, Company_details, "LOGO", CompanyImageUpdate, Company_details.CompanyLogoPath);

                }
                if (Company_DB.CompanyID > 0)
                {
                    Company_DB.CompanyName = Company_details.CompanyName;
                    Company_DB.CompanyEmail = Company_details.CompanyEmail;
                    Company_DB.CompanyAddress = Company_details.CompanyAddress;
                    Company_DB.ContactPerson = Company_details.ContactPerson;
                    Company_DB.Phone = Company_details.Phone;
                    Company_DB.UpdateBy = AppUtils.GetLoginUserID();
                    Company_DB.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(Company_DB).State = System.Data.Entity.EntityState.Modified;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDelete(int CompanyID)
        {
            var company = db.Company.Where(s => s.CompanyID == CompanyID).FirstOrDefault();
            company.DeleteBy = AppUtils.GetLoginUserID();
            company.DeleteDate = AppUtils.GetDateTimeNow();
            company.Status = AppUtils.TableStatusIsDelete;
            db.Entry(company).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        private void AddGivenImageInCurrentRow(ref Company company_DB, Company company_details, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "LOGO")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref company_DB, company_details, "LOGO", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    company_DB.CompanyLogoPath = company_details.CompanyLogoPath;
                }
                else
                {
                    RemoveImageFromServerFolder(type, company_DB);
                    company_DB.CompanyLogoPath = null;
                }
            }
        }

        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Company company_DB, Company company_details, string type, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(type, company_DB);
            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = company_DB.CompanyID + "_" + type + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (type == "LOGO")
            {
                company_DB.CompanyLogoPath = "/images/CompanyImage/" + fileName;

            }
        }

        private void RemoveImageFromServerFolder(string type, Company company_DB)
        {
            string removeImageName = "";
            if (type == "LOGO")
            {
                removeImageName = !string.IsNullOrEmpty(company_DB.CompanyLogoPath) ? company_DB.CompanyLogoPath.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/images/CompanyImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void SaveImageInFolderAndAddInformationInCompanyTable(ref Company company_info, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(image.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = company_info.CompanyID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/images/CompanyImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            company_info.CompanyLogo = imagebyte;
            company_info.CompanyLogoPath = "/images/CompanyImage/" + fileName;


        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
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