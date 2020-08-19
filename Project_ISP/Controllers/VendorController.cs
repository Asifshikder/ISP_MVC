using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using System.Dynamic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Project_ISP.Custom_Model;
using static ISP_ManagementSystemModel.AppUtils;
using static Project_ISP.JSON_Antiforgery_Token_Validation;
using Project_ISP.Models;
using ISP_ManagementSystemModel.ViewModel.CustomClass;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.Script.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using Image = System.Drawing.Image;
using Project_ISP.ViewModel;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class VendorController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: Vendor
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_vendor)]
        public ActionResult Index()
        {
            ViewBag.VendorType = new SelectList(db.VendorTypes.Where(s => s.Status == AppUtils.TableStatusIsActive), "VendorTypeID", "VendorTypeName");
            ViewBag.VendorTypeForUpdate = new SelectList(db.VendorTypes.Where(s => s.Status == AppUtils.TableStatusIsActive), "VendorTypeID", "VendorTypeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllVendorAjaxData()
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
                var vendor = db.Vendor.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<VendorViewModel> data = new List<VendorViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (vendor.Any()) ? vendor.Where(p => p.VendorTypeID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorEmail.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorAddress.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorContactPerson.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorType.VendorTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    vendor = vendor.Where(p =>
                    p.VendorID.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorName.ToString().ToLower().Contains(search.ToLower())
                    || p.CompanyName.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorEmail.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorAddress.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorType.VendorTypeName.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorContactPerson.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = vendor.Any() ? vendor.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new VendorViewModel
                            {
                                VendorID = x.VendorID,
                                VendorName = x.VendorName,
                                CompanyName = x.CompanyName,
                                VendorEmail = x.VendorEmail,
                                VendorAddress = x.VendorAddress,
                                VendorTypeName = x.VendorType.VendorTypeName,
                                VendorContactPerson = x.VendorContactPerson,
                                VendorUpdate = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Vendor) ? true : false
                            })
                        .ToList() : new List<VendorViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = vendor.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : vendor.AsEnumerable().Count();

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

        private List<VendorViewModel> SortByColumnWithOrder(string order, string orderDir, List<VendorViewModel> data)
        {
            // Initialization.   
            List<VendorViewModel> lst = new List<VendorViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorID).ToList() : data.OrderBy(p => p.VendorID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorName).ToList() : data.OrderBy(p => p.VendorName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyName).ToList() : data.OrderBy(p => p.CompanyName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorAddress).ToList() : data.OrderBy(p => p.VendorAddress).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorTypeName).ToList() : data.OrderBy(p => p.VendorTypeName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorEmail).ToList() : data.OrderBy(p => p.VendorEmail).ToList();
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

        public ActionResult InsertVendorFromPopUp(FormCollection form, HttpPostedFileBase VendorCreateImage)
        {

            Vendor Vendor_info = JsonConvert.DeserializeObject<Vendor>(form["VendorInformationForInsert"]);
            Vendor Vendor_Check = db.Vendor.Where(s => s.VendorName == Vendor_info.VendorName.Trim()).FirstOrDefault();

            if (Vendor_Check != null)
            {
                return Json(new { success = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Vendor Vendor_Return = new Vendor();

            try
            {

                Vendor_info.Status = AppUtils.TableStatusIsActive;
                Vendor_info.CreateBy = AppUtils.GetLoginUserID();
                Vendor_info.CreateDate = AppUtils.GetDateTimeNow();
                var vendorType = db.VendorTypes.Find(Vendor_info.VendorTypeID).VendorTypeName;
                Vendor_Return = db.Vendor.Add(Vendor_info);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInVendorTable(ref Vendor_info, AppUtils.ImageIsVendorLogo, VendorCreateImage);
                if (Vendor_Return.VendorID > 0)
                {
                    db.SaveChanges();
                    return Json(new { SuccessInsert = true, Vendor = Vendor_Return, VendorType = vendorType }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetVendorDetailsByID(int VendorID)
        {
            var vendor = db.Vendor.Where(s => s.VendorID == VendorID).Select(s => new
            {
                VendorID = s.VendorID,
                VendorName = s.VendorName,
                VendorEmail = s.VendorEmail,
                VendorAddress = s.VendorAddress,
                VendorContactPerson = s.VendorContactPerson,
                CompanyName = s.CompanyName,
                VendorTypeName = s.VendorType.VendorTypeName,
                VendorImagePath = s.VendorImagePath,
                VendorTypeID = s.VendorTypeID
            }).ToList().Select(
                s =>
                new
                {
                    VendorID = s.VendorID,
                    VendorName = s.VendorName,
                    VendorEmail = s.VendorEmail,
                    VendorAddress = s.VendorAddress,
                    VendorContactPerson = s.VendorContactPerson,
                    CompanyName = s.CompanyName,
                    VendorTypeName = s.VendorTypeName,
                    VendorImagePath = s.VendorImagePath,
                    VendorTypeID = s.VendorTypeID
                }).FirstOrDefault();

            var JSON = Json(new { Vendor = vendor, Success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateVendorFromPopUp(FormCollection form, HttpPostedFileBase VendorUpdateImage)
        {

            Vendor Vendor_details = JsonConvert.DeserializeObject<Vendor>(form["Vendor_details"]);
            Vendor vendor_DB = db.Vendor.Where(s => s.VendorID == Vendor_details.VendorID).FirstOrDefault();

            try
            {
                
                //AddGivenImageInCurrentRow(ref vendor_DB, Vendor_details, "Vendor_Image", VendorUpdateImage, Vendor_details.VendorImagePath);
                AddGivenImageInCurrentRow(ref vendor_DB, Vendor_details, "Vendor_Image", VendorUpdateImage, form["VendorImagePath"]);
                if (vendor_DB.VendorID > 0)
                {
                    vendor_DB.VendorName = Vendor_details.VendorName;
                    vendor_DB.VendorEmail = Vendor_details.VendorEmail;
                    vendor_DB.VendorAddress = Vendor_details.VendorAddress;
                    vendor_DB.VendorContactPerson = Vendor_details.VendorContactPerson;
                    vendor_DB.VendorTypeID = Vendor_details.VendorTypeID;
                    vendor_DB.CompanyName = Vendor_details.CompanyName;
                    vendor_DB.UpdateBy = AppUtils.GetLoginUserID();
                    vendor_DB.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(vendor_DB).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { UpdateSuccess = true, vendor = vendor_DB, vendorTypeName = db.VendorTypes.Find(vendor_DB.VendorTypeID).VendorTypeName }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteVendor(int VendorID)
        {
            var vendor = db.Vendor.Where(s => s.VendorID == VendorID).FirstOrDefault();
            vendor.DeleteBy = AppUtils.GetLoginUserID();
            vendor.DeleteDate = AppUtils.GetDateTimeNow();
            vendor.Status = AppUtils.TableStatusIsDelete;
            db.Entry(vendor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { Success = true, VendorID = vendor.VendorID }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }




        private void SaveImageInFolderAndAddInformationInVendorTable(ref Vendor vendor_info, string WhichPic, HttpPostedFileBase image)
        {

            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }

            byte[] imagebyte = null;

            string fileNameWithExtension = Path.GetFileName(image.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = vendor_info.VendorID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/VendorImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == AppUtils.ImageIsVendorLogo)
            {
                vendor_info.VendorLogoName = fileNameWithExtension;
                vendor_info.VendorImageOriginalName = imagebyte;
                vendor_info.VendorImagePath = "/Images/VendorImage/" + fileName;

            }
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



        private void AddGivenImageInCurrentRow(ref Vendor VendorUpdate, Vendor VendorDetails, string type, HttpPostedFileBase image, string imagePath)
        {
            if (type == "Vendor_Image")
            {
                if (image != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref VendorUpdate, VendorDetails, "Vendor_Image", image);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    VendorDetails.VendorImagePath = VendorUpdate.VendorImagePath;
                }
                else
                {
                    RemoveImageFromServerFolder(type, VendorUpdate);
                    VendorUpdate.VendorImagePath = null;
                    VendorUpdate.VendorImageOriginalName = null;
                }
            }

        }

        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Vendor VendorUpdate, Vendor VendorDetails, string WhichPic, HttpPostedFileBase image)
        {
            RemoveImageFromServerFolder(WhichPic, VendorUpdate);



            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = VendorUpdate.VendorID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/VendorImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "Vendor_Image")
            {
                VendorUpdate.VendorImagePath = "/Images/VendorImage/" + fileName;
                VendorUpdate.VendorImageOriginalName = imagebyte;
            }
        }


        private void RemoveImageFromServerFolder(string WhichPic, Vendor vendor)
        {
            string removeImageName = "";
            if (WhichPic == "Vendor_Image")
            {
                removeImageName = !string.IsNullOrEmpty(vendor.VendorImagePath) ? vendor.VendorImagePath.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/Images/VendorImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

    }
}