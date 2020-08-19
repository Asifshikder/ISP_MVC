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
using System.Data.Entity;

namespace Project_ISP.Controllers
{

    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ResellerController : Controller
    {
        public ResellerController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_Reseller_List)]
        public ActionResult ResellerList()
        {
            List<SelectListItem> lstSelectListItem = new List<SelectListItem>();
            lstSelectListItem.Add(new SelectListItem() { Value = "1", Text = "Active" });
            lstSelectListItem.Add(new SelectListItem() { Value = "2", Text = "Lock" });
            var lstResellerBillingCycle = db.ResellerBillingCycle.Select(x => new { ResellerBillingCycleID = x.ResellerBillingCycleID, Day = x.Day }).ToList();
            //var lstBandwithResellerItems = db.BandwithResellerGivenItem.Select(x => new { id = x.BandwithResellerGivenItemID, ItemName = x.ItemName }).ToList();
            var lstBandwithResellerItems = db.Item.Where(x=>x.ItemFor == (int)AppUtils.ItemFor.BandwithReseller).Select(x => new { id = x.ItemID, ItemName = x.ItemName }).ToList();

            ViewBag.ddlResellerCollectBy = new SelectList(db.Employee.Where(x => x.EmployeeStatus != AppUtils.EmployeeStatusIsLock).Select(x => new { EmployeeID = x.EmployeeID, EmployeeName = x.Name + "_" + x.LoginName }).ToList(), "EmployeeID", "EmployeeName");
            ViewBag.ddlPaymentBy = new SelectList(db.PaymentBy.Where(x => x.Status != AppUtils.TableStatusIsDelete).Select(x => new { PaymentByID = x.PaymentByID, PaymentByName = x.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");
            ViewBag.ddlPaymentType = new SelectList(db.GivenPaymentType.Select(x => new { ID = x.GivenPaymentTypeID, Name = x.GivenPaymentTypeName }).ToList(), "ID", "Name");

            List<SelectListItem> ddlPaymentStatusSelectListItem = new List<SelectListItem>();
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Receive", Value = "1" });
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status OnProcess", Value = "2" });
            //ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Pending", Value = "3" });
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Delete", Value = "3" });
            ViewBag.ddlPaymentStatus = new SelectList(ddlPaymentStatusSelectListItem, "Value", "Text", AppUtils.PaymentStatusIsOnProcess);

            ViewBag.ddlInsertMacResellerBillingCycle = new SelectList(lstResellerBillingCycle, "ResellerBillingCycleID", "Day");
            ViewBag.ddlUpdateMacResellerBillingCycle = new SelectList(lstResellerBillingCycle, "ResellerBillingCycleID", "Day");
            ViewBag.ddlInsertMacResellerStatus = new SelectList(lstSelectListItem, "Value", "Text");
            ViewBag.ddlUpdateMacResellerStatus = new SelectList(lstSelectListItem, "Value", "Text");

            ViewBag.ddlAddItemsForBandwithReseller = new SelectList(lstBandwithResellerItems, "id", "ItemName");
            ViewBag.ddlUpdateItemsForBandwithReseller = new SelectList(lstBandwithResellerItems, "id", "ItemName");
            ViewBag.ddlInsertBandwithResellerStatus = new SelectList(lstSelectListItem, "Value", "Text");
            ViewBag.ddlUpdateBandwithResellerStatus = new SelectList(lstSelectListItem, "Value", "Text");

            int packageForResellerUser = int.Parse(AppUtils.PackageForResellerUser);
            var resellerPackageList = db.Package.Where(x => x.PackageForMyOrResellerUser == packageForResellerUser).Select(x => new { PackageID = x.PackageID, PackageName = x.PackageName }).ToList();
            ViewBag.ddlMacReselerPackageUpdate = new SelectList(resellerPackageList, "PackageID", "PackageName");
            ViewBag.ddlMacReselerPackageInsert = new SelectList(resellerPackageList, "PackageID", "PackageName");

            var mikrotikList = db.Mikrotik.Select(x => new { ID = x.MikrotikID, Name = x.MikName }).ToList();
            ViewBag.ddlMacReselerMikrotikInsert = new SelectList(mikrotikList, "ID", "Name");
            ViewBag.ddlMacReselerMikrotikUpdate = new SelectList(mikrotikList, "ID", "Name");
            //List<Reseller> lstReseller = db.Reseller.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllResellerAJAXData(string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList)
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int totalRecords = 0;
                int recFilter = 0;
                int zoneFromDDL = 0;
                bool isCheckAllFromCln = false;
                bool IsCheckedRealIpUser = false;
                int[] IfIsCheckAllThenNonCheckLists = new int[] { };
                int[] IfNotCheckAllThenCheckLists = new int[] { };
                int[] SMSSendAry = new int[] { };
                // Initialization.   
                var ResellerStatusID = 1;//Request.Form.Get("ZoneID");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);


                List<ResellerCustomInforamation> data = new List<ResellerCustomInforamation>();

                using (SqlConnection sqlConn = new SqlConnection(AppUtils.connectionStringForQuery()))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SP_SearchReseller", sqlConn))
                    {
                        sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add("@searchString", search.Trim().ToLower());
                        sqlCmd.Parameters.Add("@Skip", SqlDbType.Int).Value = startRec;
                        sqlCmd.Parameters.Add("@Take", SqlDbType.Int).Value = pageSize;

                        sqlConn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(sqlCmd))
                        {
                            DataSet ds = new DataSet();
                            Stopwatch watch = new Stopwatch();
                            watch.Start();
                            adp.Fill(ds); //get select list from temp table
                            List<ResellerBillingCycle> resellerBillingCycles = db.ResellerBillingCycle.ToList();
                            List<Mikrotik> lstMikrotik = db.Mikrotik.ToList();
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                //Console.WriteLine(ds.Tables[0].Rows[i][0].ToString());
                                ResellerCustomInforamation tec = new ResellerCustomInforamation();
                                SetClientOneByOneInTheList(ds, ref tec, i, resellerBillingCycles, lstMikrotik);
                                data.Add(tec);
                            }
                            watch.Stop();
                            var totalmsRequred = watch.ElapsedMilliseconds + " ms";
                            var totalsecRequred = watch.Elapsed.Seconds + " s";

                            recFilter = (ds.Tables[1].Rows.Count - 1 >= 0) ? (int)ds.Tables[1].Rows[0]["totalSupplierCount"] : 0;
                            totalRecords = ds.Tables[1].Rows.Count - 1 >= 0 ? (int)ds.Tables[1].Rows[0]["totalSupplierCount"] : 0;


                            //Session["IdListForSMSSend"] = secondPartOfQuery.Select(s => new SendSMSCustomInformation
                            //{
                            //    ClientID = s.Transaction.ClientDetails.ClientDetailsID,
                            //    Phone = s.Transaction.ClientDetails.ContactNumber
                            //}).ToList();
                        }

                        sqlConn.Close();
                    }
                }



                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                //int totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : secondpart.AsEnumerable().Count();
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? totalRecords : totalRecords;
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


        private void SetClientOneByOneInTheList(DataSet ds, ref ResellerCustomInforamation tec, int i, List<ResellerBillingCycle> resellerBillingCycles, List<Mikrotik> lstMikrotik)
        {
            tec.ResellerID = (int)ds.Tables[0].Rows[i]["ResellerID"];
            tec.ResellerLoginName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerLoginName"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerLoginName"].ToString();
            tec.ResellerName = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerName"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerName"].ToString();
            tec.ResellerPassword = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerPassword"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerPassword"].ToString();
            tec.ResellerTypeNameList = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerTypeListID"].ToString()) ? "" : SetResellerTypeList(ds.Tables[0].Rows[i]["ResellerTypeListID"].ToString());
            tec.ResellerAddress = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerAddress"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerAddress"].ToString();
            tec.ResellerContact = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerContact"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerContact"].ToString();

            tec.CurrentBalance = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CurrentBalance"].ToString()) ? "" : ds.Tables[0].Rows[i]["CurrentBalance"].ToString();

            tec.ResellerBillingCycleList = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerBillingCycleList"].ToString()) ? "" : GetBillingCycleName(ds.Tables[0].Rows[i]["ResellerBillingCycleList"].ToString(), resellerBillingCycles);

            tec.ResellerStatus = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerStatus"].ToString()) ? 0 : int.Parse(ds.Tables[0].Rows[i]["ResellerStatus"].ToString());
            tec.ShowButton = true;
            tec.ResellerTypeID = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ResellerTypeListID"].ToString()) ? "" : ds.Tables[0].Rows[i]["ResellerTypeListID"].ToString(); ;

            var a = ds.Tables[0].Rows[i]["MacResellerAssignMikrotik"].ToString();
            List<int> lstMikrotikForUser = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["MacResellerAssignMikrotik"].ToString()) ? new List<int>() : ds.Tables[0].Rows[i]["MacResellerAssignMikrotik"].ToString().Trim(',').Split(',').Select(int.Parse).ToList();
            tec.MacResellerAssignMikrotik = lstMikrotikForUser.Count() > 0 ? GetMikrotikNameList(lstMikrotikForUser, lstMikrotik) : "";
            var resellertype = (int)ResellerTypeEnum.BandwidthReseller;
            tec.BandwithResellerPackage = ds.Tables[0].Rows[i]["ResellerTypeListID"].ToString() == (resellertype).ToString() ? !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["BandwithReselleItemGivenWithPrice"].ToString()) ? GetBandwithResellerPacakgeListFromJsonDeserialized(ds.Tables[0].Rows[i]["BandwithReselleItemGivenWithPrice"].ToString()) : "" : "";
            tec.MacResellerPackage = ds.Tables[0].Rows[i]["MacReselleGivenPackageWithPrice"].ToString() != (resellertype).ToString() ? !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["MacReselleGivenPackageWithPrice"].ToString()) ? GetMacResellerPacakgeListFromJsonDeserialized(ds.Tables[0].Rows[i]["MacReselleGivenPackageWithPrice"].ToString()) : "" : ""; ;//!string.IsNullOrEmpty(reseller_DB.macReselleGivenPackageWithPrice) ? GetMacResellerPacakgeListFromJsonDeserialized(reseller_DB.macReselleGivenPackageWithPrice) : "";

        }
        private string GetBandwithResellerPacakgeListFromJsonDeserialized(string bandwithResellerPackage)
        {
            List<bandwithReselleGivenItemWithPriceModel> lstBandwithReselleGivenItemWithPriceModel = JsonConvert.DeserializeObject<List<bandwithReselleGivenItemWithPriceModel>>(bandwithResellerPackage);
            string packageWithPrice = "";
            foreach (var bandwithPackage in lstBandwithReselleGivenItemWithPriceModel)
            {
                packageWithPrice += bandwithPackage.ItemName + "-" + bandwithPackage.ItemPrice + " / ";
            }
            return "<div style='width:200px; overflow-y:auto;' >" + packageWithPrice.Trim().TrimEnd('/') + "</div>"; ;
        }
        private string GetMacResellerPacakgeListFromJsonDeserialized(string macResellerPackage)
        {
            List<macReselleGivenPackageWithPriceModel> lstMacReselleGivenItemWithPriceModel = JsonConvert.DeserializeObject<List<macReselleGivenPackageWithPriceModel>>(macResellerPackage);
            string packageWithPrice = "";
            foreach (var macPackage in lstMacReselleGivenItemWithPriceModel)
            {
                packageWithPrice += macPackage.PName + "-" + macPackage.PPAdmin + " / ";
            }
            return "<div style='width:200px; overflow-y:auto;' >" + packageWithPrice.Trim().TrimEnd('/') + "</div>"; ;
        }
        private string GetMikrotikNameList(List<int> lstMikrotikForUser, List<Mikrotik> lstDBMikrotik)
        {
            string mikrotikName = "";
            foreach (var item in lstMikrotikForUser)
            {
                mikrotikName += lstDBMikrotik.Where(x => x.MikrotikID == item).FirstOrDefault().MikName + ", ";
            }
            return mikrotikName.Trim(',');
        }
        private string GetBillingCycleName(string v, List<ResellerBillingCycle> lstResellerBillingCycle)
        {
            v = v.Trim(',');
            var finalString = "";
            List<int> lstSplit = v.Split(',').Select(int.Parse).ToList();
            var lstRBCS = lstResellerBillingCycle.Where(s => lstSplit.Contains(s.ResellerBillingCycleID)).Select(s => s.Day).ToList();
            foreach (var item in lstRBCS)
            {
                finalString += item + ",";
            }
            return finalString.Trim(',').Replace(",", ", ");
        }
        private string SetResellerTypeList(string v)
        {
            List<int> lstResellerType = v.Split(',').Select(int.Parse).ToList();
            string resellerTypeList = "";
            foreach (var item in lstResellerType)
            {
                ResellerTypeEnum ResellerTypeEnum = (ResellerTypeEnum)item;
                resellerTypeList += (string)ResellerTypeEnum.ToString() + ",";
            }
            return resellerTypeList.Trim(',').ToLower().Replace(",", ", ").Replace("reseller", "");
        }
        private List<ResellerCustomInforamation> SortByColumnWithOrder(string order, string orderDir, List<ResellerCustomInforamation> data)
        {
            // Initialization.   
            List<ResellerCustomInforamation> lst = new List<ResellerCustomInforamation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerID).ToList() : data.OrderBy(p => p.ResellerID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerName).ToList() : data.OrderBy(p => p.ResellerName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerLoginName).ToList() : data.OrderBy(p => p.ResellerLoginName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerAddress).ToList() : data.OrderBy(p => p.ResellerAddress).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerContact).ToList() : data.OrderBy(p => p.ResellerContact).ToList();
                        break;


                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResellerID).ToList() : data.OrderBy(p => p.ResellerID).ToList();
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
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)]
        public ActionResult InsertReseller()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertReseller(Reseller Reseller_Client)
        {
            Reseller Reseller_Check = db.Reseller.Where(s => s.ResellerName == Reseller_Client.ResellerName.Trim()).FirstOrDefault();

            if (Reseller_Check != null)
            {
                TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Reseller Reseller_Return = new Reseller();

            try
            {
                Reseller_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Reseller_Return = db.Reseller.Add(Reseller_Client);
                db.SaveChanges();

                if (Reseller_Return.ResellerID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Reseller = Reseller_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }


        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
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
        private bool IsValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1 MB
        }
        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }
        private void RemoveImageFromServerFolder(string WhichPic, Reseller Reseller)
        {
            string removeImageName = "";
            if (WhichPic == "LOGO")
            {
                removeImageName = !string.IsNullOrEmpty(Reseller.ResellerLogoPath) ? Reseller.ResellerLogoPath.Split('/')[3] : "";

            }

            var filePath = Server.MapPath("~/Images/ResellerImage/" + removeImageName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        private void RemoveOldImageAndThenSaveImageDuringClientUpdate(ref Reseller ResellerUpdate, Reseller Reseller, string WhichPic, HttpPostedFileBase clientImageBytes)
        {
            RemoveImageFromServerFolder(WhichPic, ResellerUpdate);

            //if (!IsValidContentType(clientImageBytes.ContentType))
            //{
            //    ViewBag.Error = "Only PNG image are allowed";
            //}
            //else if (!IsValidContentLength(clientImageBytes.ContentLength))
            //{
            //    ViewBag.ErrorFileTooLarge = "Your file is too large.";
            //}

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clientImageBytes.FileName);
            string extension = Path.GetExtension(clientImageBytes.FileName);
            var fileName = ResellerUpdate.ResellerID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ResellerImage"), fileName);
            clientImageBytes.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(clientImageBytes.InputStream);
            imagebyte = reader.ReadBytes(clientImageBytes.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "LOGO")
            {
                ResellerUpdate.ResellerLogoPath = "/Images/ResellerImage/" + fileName;

            }
        }
        private void SaveImageInFolderAndAddInformationInResellerTable(ref Reseller reseller, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            //else if (!IsValidContentLength(image.ContentLength))
            //{
            //    ViewBag.ErrorFileTooLarge = "Your file is too large.";
            //}

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = reseller.ResellerID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ResellerImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == AppUtils.ImageIsResellerLogo)
            {
                //clientDetails.image = fileName;
                reseller.ResellerLogo = imagebyte;
                reseller.ResellerLogoPath = "/Images/ResellerImage/" + fileName;

            }
        }
        private void AddGivenImageInCurrentRow(ref Reseller ResellerUpdate, Reseller ResellerClient, string type, HttpPostedFileBase ClientImageBytes, string imagePath)
        {
            if (type == "LOGO")
            {
                if (ClientImageBytes != null && imagePath != null)
                {
                    RemoveOldImageAndThenSaveImageDuringClientUpdate(ref ResellerUpdate, ResellerClient, "LOGO", ClientImageBytes);
                }
                else if (!string.IsNullOrEmpty(imagePath))
                {
                    ResellerUpdate.ResellerLogoPath = ResellerClient.ResellerLogoPath;
                }
                else
                {
                    RemoveImageFromServerFolder(type, ResellerUpdate);
                    ResellerUpdate.ResellerLogoPath = null;
                }
            }

        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)]
        public ActionResult InsertResellerFromPopUp(FormCollection form, HttpPostedFileBase ResellerLogoImageBytes/*, Reseller Reseller_Client*/)
        {

            Reseller Reseller_Client = JsonConvert.DeserializeObject<Reseller>(form["Reseller_Client"]);
            Reseller Reseller_Check = db.Reseller.Where(s => s.ResellerLoginName == Reseller_Client.ResellerLoginName.Trim()).FirstOrDefault();

            if (Reseller_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Reseller Reseller_Return = new Reseller();

            try
            {
                Reseller_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.CreatedDate = AppUtils.GetDateTimeNow();
                Reseller_Return = db.Reseller.Add(Reseller_Client);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInResellerTable(ref Reseller_Client, AppUtils.ImageIsResellerLogo, ResellerLogoImageBytes);
                if (Reseller_Return.ResellerID > 0)
                {
                    db.SaveChanges();
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Reseller = Reseller_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)]
        public ActionResult InsertMacResellerFromPopUp(FormCollection form, HttpPostedFileBase ResellerLogoImageBytes/*, Reseller Reseller_Client*/)
        {

            Reseller Reseller_Client = JsonConvert.DeserializeObject<Reseller>(form["Reseller_Client"]);
            Reseller Reseller_Check = db.Reseller.Where(s => s.ResellerLoginName == Reseller_Client.ResellerLoginName.Trim()).FirstOrDefault();

            //List<ResellerPackageInJson> lstResellerGivenPackage = new List<ResellerPackageInJson>();
            //lstResellerGivenPackage = (Reseller_Client.macReselleGivenPackageWithPrice != null) ? new JavaScriptSerializer().Deserialize<List<ResellerPackageInJson>>(Reseller_Client.macReselleGivenPackageWithPrice) : new List<ResellerPackageInJson>();

            //List<macReselleGivenPackageWithPriceModel> lstmacReselleGivenPackageWithPriceModel = new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(json);

            if (Reseller_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Reseller Reseller_Return = new Reseller();

            try
            {

                string json = JsonConvert.SerializeObject(Reseller_Client.macResellerGivenPackagePriceModel);
                Reseller_Client.macReselleGivenPackageWithPrice = json;
                Reseller_Client.ResellerBalance = 0;
                Reseller_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.CreatedDate = AppUtils.GetDateTimeNow();
                Reseller_Client.UserRightPermissionID = AppUtils.UserRightPermissionIDIsReseller;
                Reseller_Client.RoleID = AppUtils.ResellerRole;
                Reseller_Client.MacResellerAssignMikrotik = !string.IsNullOrEmpty(Reseller_Client.MacResellerAssignMikrotik) ? Reseller_Client.MacResellerAssignMikrotik.Trim(',') : "";
                Reseller_Client.ResellerBillingCycleList = !string.IsNullOrEmpty(Reseller_Client.ResellerBillingCycleList) ? Reseller_Client.ResellerBillingCycleList.Trim(',') : "";

                Reseller_Return = db.Reseller.Add(Reseller_Client);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInResellerTable(ref Reseller_Client, AppUtils.ImageIsResellerLogo, ResellerLogoImageBytes);
                if (Reseller_Return.ResellerID > 0)
                {
                    db.SaveChanges();
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Reseller = Reseller_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)]
        public ActionResult InsertBandwithResellerFromPopUp(FormCollection form, HttpPostedFileBase ResellerLogoImageBytes/*, Reseller Reseller_Client*/)
        {

            Reseller Reseller_Client = JsonConvert.DeserializeObject<Reseller>(form["Reseller_Client"]);
            Reseller Reseller_Check = db.Reseller.Where(s => s.ResellerLoginName == Reseller_Client.ResellerLoginName.Trim()).FirstOrDefault();

            //List<ResellerPackageInJson> lstResellerGivenPackage = new List<ResellerPackageInJson>();
            //lstResellerGivenPackage = (Reseller_Client.macReselleGivenPackageWithPrice != null) ? new JavaScriptSerializer().Deserialize<List<ResellerPackageInJson>>(Reseller_Client.macReselleGivenPackageWithPrice) : new List<ResellerPackageInJson>();

            //List<macReselleGivenPackageWithPriceModel> lstmacReselleGivenPackageWithPriceModel = new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(json);

            if (Reseller_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Reseller Reseller_Return = new Reseller();

            try
            {

                string json = JsonConvert.SerializeObject(Reseller_Client.bandwithReselleGivenItemWithPriceModel);
                Reseller_Client.BandwithReselleItemGivenWithPrice = json;
                Reseller_Client.ResellerBalance = 0;
                Reseller_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.CreatedDate = AppUtils.GetDateTimeNow();
                Reseller_Client.UserRightPermissionID = AppUtils.UserRightPermissionIDIsReseller;
                Reseller_Client.RoleID = AppUtils.ResellerRole;

                Reseller_Return = db.Reseller.Add(Reseller_Client);
                db.SaveChanges();
                SaveImageInFolderAndAddInformationInResellerTable(ref Reseller_Client, AppUtils.ImageIsResellerLogo, ResellerLogoImageBytes);
                if (Reseller_Return.ResellerID > 0)
                {
                    db.SaveChanges();
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Reseller = Reseller_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)]
        public ActionResult UpdateMacResellerFromPopUp(FormCollection form, HttpPostedFileBase ResellerLogoImageBytes/*, Reseller Reseller_Client*/)
        {

            Reseller Reseller_Client = JsonConvert.DeserializeObject<Reseller>(form["Reseller_Client"]);
            Reseller Reseller_DB = db.Reseller.Where(s => s.ResellerID == Reseller_Client.ResellerID).FirstOrDefault();

            string message = "";
            if (Reseller_DB != null)
            {
                List<macReselleGivenPackageWithPriceModel> lstDBPackage = string.IsNullOrEmpty(Reseller_DB.macReselleGivenPackageWithPrice) ? new List<macReselleGivenPackageWithPriceModel>()
                    : new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(Reseller_DB.macReselleGivenPackageWithPrice);

                List<int> lstPackageIDFromClient = Reseller_Client.macResellerGivenPackagePriceModel != null ? Reseller_Client.macResellerGivenPackagePriceModel.Select(x => x.PID).ToList() : new List<int>();
                foreach (var item in lstDBPackage)
                {
                    if (!lstPackageIDFromClient.Contains(item.PID))
                    {
                        int count = db.ClientDetails.Where(x => x.ResellerID == Reseller_DB.ResellerID && x.PackageThisMonth == item.PID).Count();
                        if (count > 0)
                        {
                            message += db.Package.Find(item.PID).PackageName + ",";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(message))
                {
                    message = message.Trim(',') + ". Those Package has already has user. Please Change the user in different Package first.";
                    return Json(new { UpdateSuccess = false, CustomMessage = message }, JsonRequestBehavior.AllowGet);
                }


                List<int> lstDBmikrotik = string.IsNullOrEmpty(Reseller_DB.MacResellerAssignMikrotik) ? new List<int>()//.Trim().Trim(',')
                    : Reseller_DB.MacResellerAssignMikrotik.Trim(',').Split(',').Select(int.Parse).ToList();

                List<int> lstMikrotikIDFromClient = !string.IsNullOrEmpty(Reseller_Client.MacResellerAssignMikrotik.Trim().Trim(',')) ? Reseller_Client.MacResellerAssignMikrotik.Trim(',').Split(',').Select(int.Parse).ToList() : new List<int>();
                foreach (var dbMikrotik in lstDBmikrotik)
                {
                    if (!lstMikrotikIDFromClient.Contains(dbMikrotik))
                    {
                        int count = db.ClientDetails.Where(x => x.ResellerID == Reseller_DB.ResellerID && x.MikrotikID == dbMikrotik).Count();
                        if (count > 0)
                        {
                            message += db.Mikrotik.Find(dbMikrotik).MikName + ",";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(message))
                {
                    message = message.Trim(',') + ". Those Mikrotik has already has user. Please Change the user in different Mikrotik first.";
                    return Json(new { UpdateSuccess = false, CustomMessage = message }, JsonRequestBehavior.AllowGet);
                }


                //  TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";
                Reseller resellerLoginNameExistOrNot = db.Reseller.Where(x => x.ResellerID != Reseller_DB.ResellerID && x.ResellerLoginName == Reseller_DB.ResellerLoginName).FirstOrDefault();
                if (resellerLoginNameExistOrNot != null)
                {
                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                string json = JsonConvert.SerializeObject(Reseller_Client.macResellerGivenPackagePriceModel);
                Reseller_Client.macReselleGivenPackageWithPrice = json;
                Reseller_Client.CreatedBy = Reseller_DB.CreatedBy;
                Reseller_Client.CreatedDate = Reseller_DB.CreatedDate;
                Reseller_Client.UpdateBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.UpdateDate = AppUtils.GetDateTimeNow();
                Reseller_Client.UserRightPermissionID = Reseller_DB.UserRightPermissionID;
                Reseller_Client.RoleID = Reseller_DB.RoleID;
                Reseller_Client.ResellerBalance = Reseller_DB.ResellerBalance; 
                Reseller_Client.MacResellerAssignMikrotik = !string.IsNullOrEmpty(Reseller_Client.MacResellerAssignMikrotik) ? Reseller_Client.MacResellerAssignMikrotik.Trim(',') : "";
                Reseller_Client.ResellerBillingCycleList = !string.IsNullOrEmpty(Reseller_Client.ResellerBillingCycleList) ? Reseller_Client.ResellerBillingCycleList.Trim(',') : "";

            }
            else
            {
                return Json(new { UpdateSuccess = false, DbResellerIsNull = true }, JsonRequestBehavior.AllowGet);
            }

            //Reseller Reseller_Return = new Reseller();

            try
            {

                //Reseller_Return = db.Reseller.Add(Reseller_Client);
                //db.SaveChanges();

                AddGivenImageInCurrentRow(ref Reseller_DB, Reseller_Client, "LOGO", ResellerLogoImageBytes, Reseller_Client.ResellerLogoPath);

                //SaveImageInFolderAndAddInformationInResellerTable(ref Reseller_Client, AppUtils.ImageIsResellerLogo, ResellerLogoImageBytes);
                if (Reseller_Client.ResellerID > 0)
                {
                    Reseller_Client.ResellerLogoPath = Reseller_DB.ResellerLogoPath;
                    db.Entry(Reseller_DB).CurrentValues.SetValues(Reseller_Client);
                    db.SaveChanges();
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { UpdateSuccess = true, ResellerUpdateInformation = ModifyResellerForShowingInListAfterUpdate(Reseller_DB) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private object ModifyResellerForShowingInListAfterUpdate(Reseller reseller_DB)
        {
            var lstResellerBillingCycle = string.IsNullOrEmpty(reseller_DB.ResellerBillingCycleList) ? new List<string>() : reseller_DB.ResellerBillingCycleList.Trim(',').Split(',').ToList();
            var billingCycleInString = "";

            if (lstResellerBillingCycle.Count() > 0)
            {
                foreach (var item in lstResellerBillingCycle)
                {
                    var billingCycle = db.ResellerBillingCycle.Find(int.Parse(item));
                    billingCycleInString += billingCycle.Day + ",";
                }
            }
            reseller_DB.ResellerBillingCycleList = string.IsNullOrEmpty(billingCycleInString) ? "" : billingCycleInString.Trim(',');



            var lstMacResellerAssignMikrotik = string.IsNullOrEmpty(reseller_DB.MacResellerAssignMikrotik) ? new List<string>() : reseller_DB.MacResellerAssignMikrotik.Trim(',').Split(',').ToList();
            var MacResellerAssignMikrotikInString = "";

            if (lstMacResellerAssignMikrotik.Count() > 0)
            {
                foreach (var item in lstMacResellerAssignMikrotik)
                {
                    var mikrotik = db.Mikrotik.Find(int.Parse(item));
                    MacResellerAssignMikrotikInString += mikrotik.MikName + ",";
                }
            }

            reseller_DB.MacResellerAssignMikrotik = string.IsNullOrEmpty(MacResellerAssignMikrotikInString) ? "" : MacResellerAssignMikrotikInString.Trim(',');
            reseller_DB.ResellerTypeListID = reseller_DB.ResellerTypeListID == "1" ? "BandwidthReseller" : "MacBasedReseller";
            reseller_DB.BandwithReselleItemGivenWithPrice = !string.IsNullOrEmpty(reseller_DB.BandwithReselleItemGivenWithPrice) ? GetBandwithResellerPacakgeListFromJsonDeserialized(reseller_DB.BandwithReselleItemGivenWithPrice) : "";
            reseller_DB.macReselleGivenPackageWithPrice = !string.IsNullOrEmpty(reseller_DB.macReselleGivenPackageWithPrice) ? GetMacResellerPacakgeListFromJsonDeserialized(reseller_DB.macReselleGivenPackageWithPrice) : "";
            return reseller_DB;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Reseller)] 
        public ActionResult UpdateBandwithResellerFromPopUp(FormCollection form, HttpPostedFileBase ResellerLogoImageBytes/*, Reseller Reseller_Client*/)
        {

            Reseller Reseller_Client = JsonConvert.DeserializeObject<Reseller>(form["Reseller_Client"]);
            Reseller Reseller_DB = db.Reseller.Where(s => s.ResellerID == Reseller_Client.ResellerID).FirstOrDefault();

            if (Reseller_DB != null)
            {
                //  TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";
                Reseller resellerLoginNameExistOrNot = db.Reseller.Where(x => x.ResellerID != Reseller_DB.ResellerID && x.ResellerLoginName == Reseller_DB.ResellerLoginName).FirstOrDefault();
                if (resellerLoginNameExistOrNot != null)
                {
                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                string json = JsonConvert.SerializeObject(Reseller_Client.bandwithReselleGivenItemWithPriceModel);
                Reseller_Client.BandwithReselleItemGivenWithPrice = json;
                Reseller_Client.ResellerBalance = Reseller_DB.ResellerBalance;
                //Reseller_Client.macReselleGivenPackageWithPrice = json;
                Reseller_Client.CreatedBy = Reseller_DB.CreatedBy;
                Reseller_Client.CreatedDate = Reseller_DB.CreatedDate;
                Reseller_Client.UpdateBy = AppUtils.GetLoginEmployeeName();
                Reseller_Client.UpdateDate = AppUtils.GetDateTimeNow();
                Reseller_Client.UserRightPermissionID = Reseller_DB.UserRightPermissionID;
                Reseller_Client.RoleID = Reseller_DB.RoleID;
            }
            else
            {
                return Json(new { UpdateSuccess = false, DbResellerIsNull = true }, JsonRequestBehavior.AllowGet);
            }


            try
            {

                AddGivenImageInCurrentRow(ref Reseller_DB, Reseller_Client, "LOGO", ResellerLogoImageBytes, Reseller_Client.ResellerLogoPath);

                //SaveImageInFolderAndAddInformationInResellerTable(ref Reseller_Client, AppUtils.ImageIsResellerLogo, ResellerLogoImageBytes);
                if (Reseller_Client.ResellerID > 0)
                {
                    Reseller_Client.ResellerLogoPath = Reseller_DB.ResellerLogoPath;
                    db.Entry(Reseller_DB).CurrentValues.SetValues(Reseller_Client);
                    db.SaveChanges();
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { UpdateSuccess = true, ResellerUpdateInformation = ModifyResellerForShowingInListAfterUpdate(Reseller_DB) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }


        private object SetItemName(List<bandwithReselleGivenItemWithPriceModel> bandwithReselleGivenItemWithPriceModel)
        {
            //List<BandwithResellerGivenItem> lstBandwithResellerGivenItem = db.BandwithResellerGivenItem.ToList();
            //foreach (var item in bandwithReselleGivenItemWithPriceModel)
            //{
            //    item.ItemName = lstBandwithResellerGivenItem.Where(x => x.BandwithResellerGivenItemID == item.ItemID).FirstOrDefault().ItemName;
            //}
            List<Item> lstBandwithResellerGivenItem = db.Item.Where(x=>x.ItemFor == (int)AppUtils.ItemFor.BandwithReseller).ToList();
            foreach (var item in bandwithReselleGivenItemWithPriceModel)
            {
                item.ItemName = lstBandwithResellerGivenItem.Where(x => x.ItemID == item.ItemID).FirstOrDefault().ItemName;
            }
            return bandwithReselleGivenItemWithPriceModel;
        }

        private object SetPackageName(List<macReselleGivenPackageWithPriceModel> macResellerGivenPackagePriceModel)
        {
            if (macResellerGivenPackagePriceModel != null)
            {
                int resellerUserPackage = int.Parse(AppUtils.PackageForResellerUser);
                List<Package> lstResellerPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == resellerUserPackage).ToList();
                foreach (var item in macResellerGivenPackagePriceModel)
                {
                    item.PName = lstResellerPackage.Where(x => x.PackageID == item.PID).FirstOrDefault().PackageName;
                }
            }
            return macResellerGivenPackagePriceModel;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateReseller(Reseller ResellerInfoForUpdate, List<CustomResellerPackage> CRP)
        {

            try
            {

                Reseller Reseller_Check = db.Reseller.Where(s => s.ResellerID != ResellerInfoForUpdate.ResellerID && s.ResellerName == ResellerInfoForUpdate.ResellerName.Trim()).FirstOrDefault();

                if (Reseller_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Reseller Already Added. Choose different Reseller. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Reseller_db = db.Reseller.Where(s => s.ResellerID == ResellerInfoForUpdate.ResellerID);
                ResellerInfoForUpdate.CreatedBy = Reseller_db.FirstOrDefault().CreatedBy;
                ResellerInfoForUpdate.CreatedDate = Reseller_db.FirstOrDefault().CreatedDate;
                ResellerInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                ResellerInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Reseller_db.SingleOrDefault()).CurrentValues.SetValues(ResellerInfoForUpdate);
                db.SaveChanges();
                //List<int> lstDeleteList = db.ClientDetails.Where(x=>x.ResellerID == AppUtils.GetLoginUserID()).Select(x=>x.)
                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Reseller_Return = Reseller_db.ToList()
                    .Select(s => new
                    {
                        ResellerID = s.ResellerID,
                        ResellerName = s.ResellerName,
                        ResellerLoginName = s.ResellerLoginName,
                        ResellerAddress = s.ResellerAddress,
                        ResellerContact = s.ResellerContact,
                        ResellerTypeListID = SetResellerTypeList(s.ResellerTypeListID),
                        ResellerBillingCycleList = s.ResellerBillingCycleList.Trim(',').Replace(",", ", "),
                    }).FirstOrDefault();
                var JSON = Json(new { UpdateSuccess = true, ResellerUpdateInformation = Reseller_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ResellerUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_Account_Archive_Bills)]
        public ActionResult ResellerAccounts()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            DateTime dtNow = AppUtils.GetDateTimeNow();

            ViewBag.Title = "Archive Bills";
            ViewBag.Date = AppUtils.RunningYear + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(AppUtils.RunningMonth);
            setViewBagList();


            //List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstVM_Transaction_EmpTraLockUnlock_ClientDueBillsJoin =
            //  db.Transaction.Where(s =>
            //          s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
            //      .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
            //          ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
            //          {
            //              Transaction = Transaction,
            //              ClientDueBills = ClientDueBills
            //          })
            //          .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
            //          (Transaction, EmployeeTransactionLockUnlock) => new
            //          {
            //              Transaction = Transaction.Transaction,
            //              ClientDueBills = Transaction.ClientDueBills,
            //              EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
            //          })
            //          //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
            //          //{
            //          //      Transaction = Transaction.Transaction,

            //          //})
            //          .AsEnumerable()
            //          .Select(
            //      s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
            //      {
            //          TransactionID = s.Transaction.TransactionID,
            //          Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
            //          ClientDetailsID = s.Transaction.ClientDetailsID,
            //          ClientName = s.Transaction.ClientDetails.Name,
            //          Address = s.Transaction.ClientDetails.Address,
            //          ContactNumber = s.Transaction.ClientDetails.ContactNumber,
            //          ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
            //          PackageID = s.Transaction.PackageID.Value,
            //          PackageName = s.Transaction.Package.PackageName,
            //          MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
            //          FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
            //          PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentAmount.Value : 0,
            //          //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
            //          //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
            //          Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
            //          PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
            //          CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
            //          PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
            //          RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
            //          ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
            //          StatusThisMonthID = s.Transaction.LineStatusID.Value,
            //          //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
            //          //StatusNextMonthID = ,
            //          Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""

            //      }).ToList();


            //SetBillSummary();


            return View(new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>());


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCurrentMonthArchiveBillsAJAXData()
        {
            // Initialization.   
            List<int> lstClientDetailsID = new List<int>();
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   

                int zoneFromDDL = 0;
                var YearID = Request.Form.Get("YearID");
                var MonthID = Request.Form.Get("MonthID");
                var ZoneID = Request.Form.Get("ZoneID");
                var ResellerID = Request.Form.Get("ResellerID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (YearID != "")
                {
                    YearID = db.Year.Find(int.Parse(YearID)).YearName;
                }
                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                if (ResellerID != "")
                {
                    int resellerIDCon = int.Parse(ResellerID);
                    lstClientDetailsID = db.ClientDetails.Where(s => s.ResellerID == resellerIDCon)
                        .Select(s => s.ClientDetailsID).ToList();
                }
                List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

                var firstPartOfQuery =
                        (ResellerID != "" && YearID != "" && MonthID != "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                            : (ResellerID != "" && YearID != "" && MonthID != "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentMonth.ToString() == MonthID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                : (ResellerID != "" && YearID != "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                    : (ResellerID != "" && YearID != "" && MonthID == "" && ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear.ToString() == YearID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                        : (ResellerID != "" && YearID == "" && MonthID == "" && ZoneID != "") ? db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                            :
                                            db.Transaction.Where(s => s.TransactionID == 0).AsQueryable()
                    ;
                var secondPartOfQuery = firstPartOfQuery
                    .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                        ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                        {
                            Transaction = Transaction,
                            ClientDueBills = ClientDueBills
                        })
                    .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                        (Transaction, EmployeeTransactionLockUnlock) => new
                        {
                            Transaction = Transaction.Transaction,
                            ClientDueBills = Transaction.ClientDueBills,
                            EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                        })
                    //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                    //{
                    //      Transaction = Transaction.Transaction,

                    //})
                    .AsEnumerable();

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.Transaction.TransactionID.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                                        || p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.Transaction.TransactionID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ClientDetails.Address.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ClientDetails.ContactNumber.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ClientDetails.Zone.ZoneName.Contains(search.ToLower())
                                                                     || p.Transaction.Package.PackageName.ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.RemarksNo.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Transaction.ResetNo.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                        {
                            TransactionID = s.Transaction.TransactionID,
                            Paid = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? true : false,
                            ClientDetailsID = s.Transaction.ClientDetailsID,
                            ClientName = s.Transaction.ClientDetails.Name,
                            Address = s.Transaction.ClientDetails.Address,
                            ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                            ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                            PackageID = s.Transaction.PackageID.Value,
                            PackageName = s.Transaction.Package.PackageName,
                            MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                            FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                            PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                            //Due = db.ClientDueBills.Where(ss => ss.ClientDetailsID == s.Transaction.ClientDetailsID).ToList().Count > 0
                            //            ? CalculationForShowingDueBills(s.Transaction.ClientDetailsID) : 0,
                            Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                            PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                            CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                            PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                            RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                            ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                            StatusThisMonthID = s.Transaction.LineStatusID.Value,
                            //StatusThisMonthID = Function(s.Transaction.LineStatusID.Value, s.Transaction.TransactionID, s.Transaction.PackageID.Value, dtNow),
                            //StatusNextMonthID = ,
                            Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""
                        }).ToList();

                }

                // Sorting.   
                lstArchiveBillsInformation = this.SortByColumnWithOrder(order, orderDir, lstArchiveBillsInformation);
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
                    data = lstArchiveBillsInformation
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
        private List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> SortByColumnWithOrder(string order, string orderDir, List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> data)
        {
            // Initialization.   
            List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lst = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLineStatusID).ToList() : data.OrderBy(p => p.ClientLineStatusID).ToList();
                    //    break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ZoneName).ToList() : data.OrderBy(p => p.ZoneName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "14":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RemarksNo).ToList() : data.OrderBy(p => p.RemarksNo).ToList();
                        break;
                    case "15":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReceiptNo).ToList() : data.OrderBy(p => p.ReceiptNo).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionID).ToList() : data.OrderBy(p => p.TransactionID).ToList();
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

        private void setViewBagList()
        {
            //ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.Select(s => new { ConnectionTypeID = s.ConnectionTypeID, ConnectionTypeName = s.ConnectionTypeName }), "ConnectionTypeID", "ConnectionTypeName");
            //ViewBag.PackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            //ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            //ViewBag.LineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive /*|| s.LineStatusID == AppUtils.LineIsInActive*/ || s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            //ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { s.ResellerID, s.ResellerName }), "ResellerID", "ResellerName");

            //ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            //ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");
            //ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            //ViewBag.EmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            //ViewBag.DueEmployeeID = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");
            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { s.ResellerID, s.ResellerName }), "ResellerID", "ResellerName"); ;


            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");
            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");

            ViewBag.YearID = new SelectList(db.Year.ToList(), "YearID", "YearName");
            ViewBag.MonthID = new SelectList(db.Month.ToList(), "MonthID", "MonthName");

            var lstzone = db.Zone.Select(x => new { x.ZoneID, x.ZoneName }).ToList();
            ViewBag.ZoneID = new SelectList(lstzone, "ZoneID", "ZoneName");
            ViewBag.SearchByZoneID = new SelectList(lstzone, "ZoneID", "ZoneName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");
            ViewBag.DueEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive && s.EmployeeID != AppUtils.EmployeeIDISKamrul).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).Select(s => new { EmployeeID = s.EmployeeID, Name = s.Name }).ToList(), "EmployeeID", "Name");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetBillsListBySearchCriteria(int? YearID, int? MonthID, int? ZoneID)
        {
            string date = "";
            try
            {
                IEnumerable<VM_Transaction_ClientDueBills> lstTransactMonthlyBills = new List<VM_Transaction_ClientDueBills>();

                List<Transaction> lstTransactOnlySignUpBill = new List<Transaction>();
                List<Expense> lstExpenses = new List<Expense>();


                if (YearID != null && MonthID != null && ZoneID != null)
                {
                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value) + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction
                       .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                       .Where(s => s.Transaction.PaymentYear == YearID && s.Transaction.PaymentMonth == MonthID && s.Transaction.ClientDetails.ZoneID == ZoneID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                       .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();
                    DateTime dtStart = new DateTime(YearID.Value, MonthID.Value, 1);
                    DateTime dtEnd = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= dtStart &&
                        s.PaymentDate <= dtEnd && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
                }

                else if (YearID != null && MonthID != null && ZoneID == null)
                {

                    date = YearID + "-" + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(MonthID.Value);
                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction
                        .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                        .Where(s => s.Transaction.PaymentYear == YearID && s.Transaction.PaymentMonth == MonthID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                        .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();
                    //var sss = lstTransactMonthlyBills.ToList();
                    DateTime dtStart = new DateTime(YearID.Value, MonthID.Value, 1);
                    DateTime dtEnd = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= dtStart &&
                        s.PaymentDate <= dtEnd).ToList();
                }
                else if (YearID != null && ZoneID != null && MonthID == null)
                {

                    date = YearID.Value.ToString() + "_ Zone:" + db.Zone.Find(ZoneID).ZoneName;
                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction
                        .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                        .Where(s => s.Transaction.PaymentYear == YearID && s.Transaction.ClientDetails.ZoneID == ZoneID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                        .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();

                    DateTime dtStart = new DateTime(YearID.Value, 1, 1);
                    DateTime dtEnd = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= dtStart &&
                        s.PaymentDate <= dtEnd && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
                }

                else if (YearID != null && MonthID == null && ZoneID == null)
                {

                    date = YearID.Value.ToString();
                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction
                        .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                        .Where(s => s.Transaction.PaymentYear == YearID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                        .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();

                    DateTime dtStart = new DateTime(YearID.Value, 1, 1);
                    DateTime dtEnd = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));
                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= dtStart &&
                        s.PaymentDate <= dtEnd).ToList();
                }

                else if (YearID == null && MonthID == null && ZoneID != null)
                {

                    date = "All Year and Month Based On Zone : " + db.Zone.Find(ZoneID).ZoneName;
                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction.GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                        .Where(s => s.Transaction.ClientDetails.ZoneID == ZoneID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                        .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();

                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
                }
                else
                {

                    YearID = AppUtils.RunningYear;
                    MonthID = AppUtils.RunningMonth;

                    GetExpense(ref lstExpenses, YearID, MonthID, ZoneID);
                    lstTransactMonthlyBills = db.Transaction
                        .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new { Transaction = Transaction, ClientDueBills = ClientDueBills })
                        .Where(s => s.Transaction.PaymentYear == YearID && s.Transaction.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly)
                        .Select(s => new VM_Transaction_ClientDueBills { Transaction = s.Transaction, ClientDueBills = s.ClientDueBills.FirstOrDefault() }).AsQueryable();

                    DateTime dtStart = new DateTime(YearID.Value, MonthID.Value, 1);
                    DateTime dtEnd = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));
                    lstTransactOnlySignUpBill = db.Transaction.Where(s =>
                        s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= dtStart &&
                        s.PaymentDate <= dtEnd).ToList();
                }

                var ss = lstTransactMonthlyBills.Select(s => new
                {
                    LineStatusID = s.Transaction.LineStatusID,
                    TransactionID = s.Transaction.TransactionID,
                    Name = s.Transaction.ClientDetails.Name,
                    ClientDetailsID = s.Transaction.ClientDetails.ClientDetailsID,
                    LoginName = s.Transaction.ClientDetails.LoginName,
                    Address = s.Transaction.ClientDetails.Address,
                    Mobile = s.Transaction.ClientDetails.ContactNumber,
                    Zone = s.Transaction.ClientDetails.Zone.ZoneName,
                    Package = s.Transaction.Package.PackageName,
                    MonthlyFee = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                    PaidAmount = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                    DueAmount = (s.ClientDueBills == null) ? "0" : s.ClientDueBills.DueAmount.ToString(),
                    PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                    CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name.ToString() : "",
                    PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                    RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo : "",
                    ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo : "",
                    PaymentStatus = s.Transaction.PaymentStatus,
                    DueBillStatus = (s.ClientDueBills == null) ? false : s.ClientDueBills.Status
                })
                       .GroupBy(s => s.PaymentStatus, (Key, g) => g.OrderBy(s => s.PaymentStatus).ToList()).ToList();

                dynamic billSummaryDetailss = new ExpandoObject();
                if (ss.Count > 0)
                {
                    SetBillSummaryForAjaxCall(lstTransactMonthlyBills, lstTransactOnlySignUpBill, lstExpenses, ref billSummaryDetailss, YearID, MonthID, ZoneID);
                }
                return Json(new { Success = true, lstTransaction = ss, billSummaryDetails = billSummaryDetailss, Date = date }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Date = date }, JsonRequestBehavior.AllowGet);
            }
        }
        private List<Expense> GetExpense(ref List<Expense> lstExpenses, int? YearID, int? MonthID, int? ZoneID)
        {
            int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
            int month = MonthID != null ? MonthID.Value : 0;
            lstExpenses = new List<Expense>();
            if (year > 0 && month > 0)
            {
                DateTime expenseStartDateTime = new DateTime(year, month, 1);
                DateTime expenseEndDateTime = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(year, month, Convert.ToInt32(DateTime.DaysInMonth(year, month))));
                lstExpenses = db.Expenses.Where(s =>
                    s.PaymentDate >= expenseStartDateTime && s.PaymentDate <= expenseEndDateTime).ToList();
                return lstExpenses;
            }
            else if (YearID != null)
            {
                DateTime expenseStartDateTime = new DateTime(year, 1, 1);
                DateTime daysInMonth = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                DateTime expenseEndDateTime = AppUtils.GetLastDayWithHrMinSecMsByMyDate(daysInMonth);
                lstExpenses = db.Expenses.Where(s => s.PaymentDate >= expenseStartDateTime && s.PaymentDate <= expenseEndDateTime).ToList();
                return lstExpenses;
            }
            else
            {
                return lstExpenses;
            }
        }
        private void SetBillSummaryForAjaxCall(IEnumerable<VM_Transaction_ClientDueBills> lstTransaction, List<Transaction> lstSignUpBill, List<Expense> lstExpenses, ref dynamic billSummaryDetails, int? YearID, int? MonthID, int? ZoneID)
        {// is this for all?
            double expense = 0;
            List<RegularSignUpBill> lstRegularSignUpBill = new List<RegularSignUpBill>();

            int year = YearID != null ? YearID.Value : AppUtils.RunningYear;
            int month = MonthID != null ? MonthID.Value : AppUtils.RunningMonth;
            int zone = ZoneID != null ? ZoneID.Value : 0;
            // DateTime customDate = new DateTime(AppUtils.RunningYear, AppUtils.RunningMonth, 01);
            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            List<TransactionTemp> lstTransactionForBillSummary = new List<TransactionTemp>();
            IEnumerable<TransactionTemp> lstRegularMonthlyBill = new List<TransactionTemp>();


            //var lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Select(s => new { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount }).ToList();
            //           var lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
            //           var lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new { PaymentAmount = s.PaymentAmount }).ToList();
            //           var expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);


            if (YearID != null && MonthID != null && zone > 0)
            {
                startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month && s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;

            }
            else if (YearID != null && MonthID != null && zone == 0)
            {
                startDate = new DateTime(YearID.Value, MonthID.Value, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, MonthID.Value, DateTime.DaysInMonth(YearID.Value, MonthID.Value)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == year && s.PaymentMonth == month).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == year && s.PaymentMonth == month) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;
            }
            else if (YearID != null && MonthID == null && zone > 0)
            {
                startDate = new DateTime(YearID.Value, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == year && s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else if (YearID != null && MonthID == null && zone == 0)
            {
                startDate = new DateTime(YearID.Value, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(YearID.Value, 12, DateTime.DaysInMonth(YearID.Value, 12)));

                lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == year).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else if (YearID == null && MonthID == null && zone > 0)
            {
                lstTransactionForBillSummary = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.ClientDetails.ZoneID == zone && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = "Calculation Not Possible";
            }
            else
            {//date time is default 
                lstTransactionForBillSummary = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Select(s => new TransactionTemp { ClientDetailsID = s.ClientDetailsID, Discount = s.Discount != null ? s.Discount.Value : 0, PaymentTypeID = s.PaymentTypeID, Package = s.Package, PaymentStatus = s.PaymentStatus, PaymentAmount = s.PaymentAmount.Value }).ToList();
                lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).AsEnumerable();
                lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).Select(s => new RegularSignUpBill { PaymentAmount = (s.PaymentAmount != null) ? s.PaymentAmount.Value : 0 }).ToList();
                expense = !db.Expenses.Any() ? 0 : db.Expenses.Where(s => s.PaymentDate >= startDate && s.PaymentDate <= endDate).Select(s => new { Amount = s.Amount }).ToList().Sum(s => s.Amount);

                billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= endDate).Sum(ss => ss.PaymentAmount);
                billSummaryDetails.clnPreviousBillCollection = db.Transaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount); ;
            }


            //List<Transaction> lstTransaction = db.Transaction.ToList();
            //List<Transaction> lstTransactionForBillSummary = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();
            //List<Transaction> lstRegularMonthlyBill = lstTransactionForBillSummary.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
            //List<Transaction> lstRegularSignUpBill = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDate && s.PaymentDate <= endDate)).ToList();

            billSummaryDetails.clnPayableAmount = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
            billSummaryDetails.clnCollectedAmount = Math.Round(lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount), 2);
            billSummaryDetails.clnDiscountAmount = lstTransactionForBillSummary.Sum(s => s.Discount);
            billSummaryDetails.clnCollectedAmountBIll = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Sum(s => s.PaymentAmount);
            billSummaryDetails.clnOnlinePayment = 0;
            DateTime lstDateForLinq = AppUtils.GetLastDayWithHrMinSecMsByMyDate(endDate);
            //billSummaryDetails.clnInstallationAmount = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && s.PaymentDate >= startDate && s.PaymentDate <= lstDateForLinq).Sum(ss => ss.PaymentAmount);
            //ViewBag.clnDueAmount = lstRegularMonthlyBill.Sum(s => s.Package.PackagePrice) - ((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) + lstTransactionForBillSummary.Sum(s => s.Discount));
            double a = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount);
            double b = lstRegularMonthlyBill.Sum(s => s.PaymentAmount);
            billSummaryDetails.clnDueAmount = a/*(a - b) < 0 ? 0 : -1*(b - a)*/;
            //ViewBag.clnDueAmount = (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount)) < 0 ? 0 : (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsNotPaid).Sum(s => s.PaymentAmount) - lstRegularMonthlyBill.Sum(s => s.PaymentAmount));
            billSummaryDetails.clnTotalExpense = expense;
            billSummaryDetails.clnRestOfAmount = Math.Round((lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection).Sum(s => s.PaymentAmount) + lstRegularSignUpBill.Sum(s => s.PaymentAmount)) - expense, 2);
            billSummaryDetails.clnTotalClient = lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count();
            billSummaryDetails.clnPaidClient = lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count();
            billSummaryDetails.clnUnpaidClient = (lstRegularMonthlyBill.Select(s => s.ClientDetailsID).Distinct().Count()) - (lstRegularMonthlyBill.Where(s => s.PaymentStatus == AppUtils.PaymentIsPaid).Count());
            //billSummaryDetails.clnPreviousBillCollection = "";//db.Transaction.Where(s => !(s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth) && (s.PaymentDate >= startDate && s.PaymentDate <= endDate) && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Select(s => new { PaymentAmount = s.PaymentAmount }).Sum(s => s.PaymentAmount);


        }
        class TransactionTemp
        {
            public int ClientDetailsID { get; set; }
            public float Discount { get; set; }
            public int PaymentTypeID { get; set; }
            public Package Package { get; set; }
            public int PaymentStatus { get; set; }
            public float PaymentAmount { get; set; }
        }
        class RegularSignUpBill
        {
            public float PaymentAmount { get; set; }
        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_ResellerPayment_List)]
        public ActionResult ResellerPaymentHistory()
        {

            ViewBag.ResellerLoginID = new SelectList(db.Reseller.Select(x => new { ResellerID = x.ResellerID, ResellerLoginName = x.ResellerBusinessName + "_" + x.ResellerLoginName + "_" + x.ResellerAddress }).ToList(), "ResellerID", "ResellerLoginName");
            ViewBag.ddlResellerCollectBy = new SelectList(db.Employee.Where(x => x.EmployeeStatus != AppUtils.EmployeeStatusIsLock).Select(x => new { EmployeeID = x.EmployeeID, EmployeeName = x.Name + "_" + x.LoginName }).ToList(), "EmployeeID", "EmployeeName");
            ViewBag.ddlPaymentBy = new SelectList(db.PaymentBy.Where(x => x.Status != AppUtils.TableStatusIsDelete).Select(x => new { PaymentByID = x.PaymentByID, PaymentByName = x.PaymentByName }).ToList(), "PaymentByID", "PaymentByName");

            List<SelectListItem> ddlPaymentStatusSelectListItem = new List<SelectListItem>();
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Receive", Value = "1" });
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status OnProcess", Value = "2" });
            //ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Pending", Value = "3" });
            ddlPaymentStatusSelectListItem.Add(new SelectListItem { Text = "Payment Status Delete", Value = "3" });
            ViewBag.ddlPaymentStatus = new SelectList(ddlPaymentStatusSelectListItem, "Value", "Text", AppUtils.PaymentStatusIsOnProcess);

            ViewBag.ddlPaymentType = new SelectList(db.GivenPaymentType.Select(x => new { ID = x.GivenPaymentTypeID, Name = x.GivenPaymentTypeName }).ToList(), "ID", "Name");

            return View();
        }


        //public class ResellerCustomPaymentInformation
        //{
        //    public int ResellerPaymentID { get; set; }
        //    public int ResellerID { get; set; }
        //    public string RID { get; set; }
        //    public string ResellerName { get; set; }
        //    public string ResellerLoginName { get; set; }
        //    public string ResellerAddress { get; set; }
        //    public string ResellerStatus { get; set; }
        //    public string PaymentTypeID { get; set; } //cash or check or some other.
        //    public string ActionTypeID { get; set; } //cash  purchase or cash purchase return
        //    public double PaymentAmount { get; set; }
        //    public double PaymentYear { get; set; }
        //    public double PaymentMonth { get; set; }
        //    public string PaymentStatus { get; set; }
        //    public string PaymentCheckOrAnySerial { get; set; }
        //    public string Status { get; set; }
        //    public string ActiveBy { get; set; }
        //    public string CreatedBy { get; set; }
        //    public string DeleteBy { get; set; }
        //    public string Collectby { get; set; }
        //    public string PaymentBy { get; set; }
        //    public string Button { get; set; }

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllResellerPaymentAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                int resellerIDFromDDL = 0;
                DateTime resellerPaymentStartDateFromDDL;
                DateTime resellerPaymentEndDateFromDDL;
                // Initialization.   
                var ResellerID = Request.Form.Get("ResellerID");
                var PaymentStartDate = Request.Form.Get("PaymentStartDate");
                var PaymentEndDate = Request.Form.Get("PaymentEndDate");

                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (!string.IsNullOrEmpty(ResellerID))
                {
                    resellerIDFromDDL = int.Parse(ResellerID);
                }
                if (!string.IsNullOrEmpty(PaymentStartDate))
                {
                    resellerPaymentStartDateFromDDL = Convert.ToDateTime(PaymentStartDate);
                }
                else
                {
                    resellerPaymentStartDateFromDDL = AppUtils.GetDateNow();
                }
                if (!string.IsNullOrEmpty(PaymentEndDate))
                {
                    resellerPaymentEndDateFromDDL = Convert.ToDateTime(PaymentEndDate).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                }
                else
                {
                    resellerPaymentEndDateFromDDL = AppUtils.GetDateNow().AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                }

                List<ResellerCustomPaymentInformation> data = new List<ResellerCustomPaymentInformation>();

                //var a = db.ResellerPaymentDetailsHistory.ToList();
                var firstPartOfQuery = db.ResellerPaymentDetailsHistory.AsQueryable();
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                    firstPartOfQuery = firstPartOfQuery.Where(s => s.ResellerID == resellerID).Include("Reseller").OrderByDescending(x => x.CreatedDate)/*.ThenBy(x => x.ResellerID)*/.AsQueryable();
                    //firstPartOfQuery = db.ClientDetails.Where(s => s.ResellerID == resellerID && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole && resellerIDFromDDL > 0)
                {
                    firstPartOfQuery = firstPartOfQuery.Where(s => s.ResellerID == resellerIDFromDDL).Include("Reseller").OrderByDescending(x => x.CreatedDate)/*.ThenBy(x => x.ResellerID)*/.AsQueryable();
                    //firstPartOfQuery = db.ClientDetails.Where(s => s.ResellerID == resellerIDFromDDL && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }
                else
                {
                    firstPartOfQuery = db.ResellerPaymentDetailsHistory.Include("Reseller").OrderByDescending(x => x.CreatedDate)/*.ThenBy(x => x.ResellerID)*/.AsQueryable();
                    //firstPartOfQuery = db.ClientDetails.Where(s => s.ResellerID == null && (s.IsNewClient == null || s.IsNewClient == 0) && s.LoginName.Contains(SearchText)).Select(s => new AutoSearchBoxModel { label = s.LoginName, val = s.ClientDetailsID }).ToList();
                }
                //var firstPartOfQuery = db.ResellerPaymentDetailsHistory.Include("Reseller").OrderByDescending(x => x.CreatedDate)/*.ThenBy(x => x.ResellerID)*/.AsQueryable();
                //if (resellerIDFromDDL > 0)
                //{
                //    firstPartOfQuery = firstPartOfQuery.Where(x => x.ResellerID == resellerIDFromDDL).AsQueryable();
                //}
                var secontPartOrQuery =
                            firstPartOfQuery
                            .Where(s =>
                                //s.Status == AppUtils.TableStatusIsActive
                                //&& (s.PaymentStatus == AppUtils.PaymentStatusIsReceive || s.PaymentStatus == AppUtils.PaymentStatusIsOnProcess)
                                (s.CreatedDate >= resellerPaymentStartDateFromDDL && s.CreatedDate <= resellerPaymentEndDateFromDDL)
                                ).AsEnumerable(); //AsQueryable();
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
                    ifSearch = (secontPartOrQuery.Any()) ? secontPartOrQuery.Where(p =>
                                                                           p.Reseller.ResellerName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.Reseller.ResellerLoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.Reseller.ResellerBusinessName.ToString().ToLower().Contains(search.ToLower())
                                                                          ).Count() : 0;

                    // Apply search   
                    secontPartOrQuery = secontPartOrQuery.Where(p => p.Reseller.ResellerName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.Reseller.ResellerLoginName.ToString().ToLower().Contains(search.ToLower())
                                                                          || p.Reseller.ResellerBusinessName.ToString().ToLower().Contains(search.ToLower())
                                                                          ).AsEnumerable();
                }
                //var a = secontPartOrQuery.ToList();
                if (secontPartOrQuery.Count() > 0)
                {
                    bool hasUpdateAccess = false;
                    bool hasDeleteAccess = false;
                    HasAccessOnWhichAction(ref hasUpdateAccess, ref hasDeleteAccess);

                    totalRecords = secontPartOrQuery.Count();
                    data = secontPartOrQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s =>
                        new ResellerCustomPaymentInformation()
                        {
                            ResellerID = s.ResellerID,
                            ResellerPaymentID = s.ResellerPaymentID,
                            PaymentYear = s.PaymentYear,
                            PaymentMonth = s.PaymentMonth,
                            ResellerLoginName = s.Reseller.ResellerLoginName,
                            ResellerName = s.Reseller.ResellerName,
                            ResellerAddress = s.Reseller.ResellerAddress,
                            PaymentAmount = s.PaymentAmount,
                            PaymentCheckOrAnySerial = s.PaymentCheckOrAnySerial,
                            PaymentStatus = GetPaymentStatus(s.PaymentStatus),
                            PaymentTypeID = GetPaymentType(s.ResellerPaymentGivenTypeID),
                            ResellerStatus = s.Reseller.ResellerStatus == AppUtils.ResellerStatusIsActive ? "<span class='label  label-success'>Active</span>" : "<span class='label  label-danger'>Lock</span>",
                            Collectby = s.CollectBy > 0 ? db.Employee.Find(s.CollectBy).Name : "",
                            CreatedBy = s.CreatedBy > 0 ? db.Employee.Find(s.CreatedBy).Name : "",
                            ActiveBy = s.ActiveBy > 0 ? db.Employee.Find(s.ActiveBy).Name : "",
                            ActionTypeID = "",
                            DeleteBy = s.PaymentStatus == AppUtils.PaymentStatusIsDelete ? db.Employee.Find(s.DeleteBy).Name + "_" + s.DeleteDate : "",
                            PaymentTime = s.CreatedDate.HasValue ? s.CreatedDate.Value.ToString() : "",
                            PaymentBy = s.PaymentByID > 0 ? db.PaymentBy.Find(s.PaymentByID).PaymentByName : "",
                            LastAmount = s.LastAmount,
                            Button = GetButton(s.PaymentStatus, hasUpdateAccess, hasDeleteAccess)
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

        //[UserRIghtCheck(ControllerValue = AppUtils.View_ResellerPayment_List)]
        //public ActionResult SpecificResellerPaymentHistory()
        //{

        //    return View();
        //}

        //[HttpPost]
        //[UserRIghtCheck(ControllerValue = AppUtils.View_ResellerPayment_List)]
        //[ValidateAntiForgeryToken]
        //public JsonResult GetSpecificResellerPaymentHistoryAJAXData()
        //{
        //    // Initialization.   
        //    JsonResult result = new JsonResult();
        //    try
        //    {

        //        int ifSearch = 0;
        //        int totalRecords = 0;
        //        int recFilter = 0;
        //        int resellerIDFromDDL = 0;
        //        DateTime resellerPaymentStartDateFromDDL;
        //        DateTime resellerPaymentEndDateFromDDL;
        //        // Initialization.   
        //        var ResellerID = Request.Form.Get("ResellerID");
        //        var PaymentStartDate = Request.Form.Get("PaymentStartDate");
        //        var PaymentEndDate = Request.Form.Get("PaymentEndDate");

        //        string search = Request.Form.GetValues("search[value]")[0];
        //        string draw = Request.Form.GetValues("draw")[0];
        //        string order = Request.Form.GetValues("order[0][column]")[0];
        //        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
        //        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
        //        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

        //        if (ResellerID != "")
        //        {
        //            resellerIDFromDDL = int.Parse(ResellerID);
        //        }
        //        if (!string.IsNullOrEmpty(PaymentStartDate))
        //        {
        //            resellerPaymentStartDateFromDDL = Convert.ToDateTime(PaymentStartDate);
        //        }
        //        else
        //        {
        //            resellerPaymentStartDateFromDDL = AppUtils.GetDateNow();
        //        }
        //        if (!string.IsNullOrEmpty(PaymentEndDate))
        //        {
        //            resellerPaymentEndDateFromDDL = Convert.ToDateTime(PaymentEndDate).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
        //        }
        //        else
        //        {
        //            resellerPaymentEndDateFromDDL = AppUtils.GetDateNow().AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
        //        }

        //        List<ResellerCustomPaymentInformation> data = new List<ResellerCustomPaymentInformation>();

        //        //var a = db.ResellerPaymentDetailsHistory.ToList();
        //        var firstPartOfQuery = db.ResellerPaymentDetailsHistory.Include("Reseller")/*.OrderByDescending(x => new { x.PaymenReceivedDate })*/.OrderByDescending(x => x.CreatedDate)/*.ThenBy(x => x.ResellerID)*/.AsQueryable();
        //        if (resellerIDFromDDL > 0)
        //        {
        //            firstPartOfQuery = firstPartOfQuery.Where(x => x.ResellerID == resellerIDFromDDL).AsQueryable();
        //        }
        //        var secontPartOrQuery =
        //                    firstPartOfQuery
        //                    .Where(s =>
        //                        //s.Status == AppUtils.TableStatusIsActive
        //                        //&& (s.PaymentStatus == AppUtils.PaymentStatusIsReceive || s.PaymentStatus == AppUtils.PaymentStatusIsOnProcess)
        //                        (s.CreatedDate >= resellerPaymentStartDateFromDDL && s.CreatedDate <= resellerPaymentEndDateFromDDL)
        //                        ).AsEnumerable(); //AsQueryable();
        //        //var secondPartOfQuery = firstPartOfQuery
        //        //          .GroupJoin(db.ClientDetails, ph => ph.ClientDetailsID, ClientDetails => ClientDetails.ClientDetailsID, (PH, ClientDetails) => new
        //        //          {
        //        //              Transaction = PH,
        //        //              ClientDetails = ClientDetails.FirstOrDefault()

        //        //          })
        //        //          .AsEnumerable();

        //        // Verification.   
        //        //a = firstPartOfQuery.ToList();
        //        if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
        //        {
        //            //        var a = secondPartOfQuery.ToList();
        //            ifSearch = (secontPartOrQuery.Any()) ? secontPartOrQuery.Where(p =>
        //                                                                   p.Reseller.ResellerName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  || p.Reseller.ResellerLoginName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  || p.Reseller.ResellerBusinessName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  ).Count() : 0;

        //            // Apply search   
        //            secontPartOrQuery = secontPartOrQuery.Where(p => p.Reseller.ResellerName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  || p.Reseller.ResellerLoginName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  || p.Reseller.ResellerBusinessName.ToString().ToLower().Contains(search.ToLower())
        //                                                                  ).AsEnumerable();
        //        }
        //        //var a = secontPartOrQuery.ToList();
        //        if (secontPartOrQuery.Count() > 0)
        //        {
        //            bool hasUpdateAccess = false;
        //            bool hasDeleteAccess = false;
        //            HasAccessOnWhichAction(ref hasUpdateAccess, ref hasDeleteAccess);

        //            totalRecords = secontPartOrQuery.Count();
        //            data = secontPartOrQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
        //                s =>
        //                new ResellerCustomPaymentInformation()
        //                {
        //                    ResellerID = s.ResellerID,
        //                    ResellerPaymentID = s.ResellerPaymentID,
        //                    PaymentYear = s.PaymentYear,
        //                    PaymentMonth = s.PaymentMonth,
        //                    ResellerLoginName = s.Reseller.ResellerLoginName,
        //                    ResellerName = s.Reseller.ResellerName,
        //                    ResellerAddress = s.Reseller.ResellerAddress,
        //                    PaymentAmount = s.PaymentAmount,
        //                    PaymentCheckOrAnySerial = s.PaymentCheckOrAnySerial,
        //                    PaymentStatus = GetPaymentStatus(s.PaymentStatus),
        //                    PaymentTypeID = GetPaymentType(s.ResellerPaymentGivenTypeID),
        //                    ResellerStatus = s.Reseller.ResellerStatus == AppUtils.ResellerStatusIsActive ? "<span class='label  label-success'>Active</span>" : "<span class='label  label-danger'>Lock</span>",
        //                    Collectby = s.CollectBy > 0 ? db.Employee.Find(s.CollectBy).Name : "",
        //                    CreatedBy = s.CreatedBy > 0 ? db.Employee.Find(s.CreatedBy).Name : "",
        //                    ActiveBy = s.ActiveBy > 0 ? db.Employee.Find(s.ActiveBy).Name : "",
        //                    ActionTypeID = "",
        //                    DeleteBy = s.PaymentStatus == AppUtils.PaymentStatusIsDelete ? db.Employee.Find(s.DeleteBy).Name + "_" + s.DeleteDate : "",
        //                    PaymentTime = s.CreatedDate.HasValue ? s.CreatedDate.Value.ToString() : "",
        //                    PaymentBy = s.PaymentByID > 0 ? db.PaymentBy.Find(s.PaymentByID).PaymentByName : "",
        //                    LastAmount = s.LastAmount,
        //                    Button = GetButton(s.PaymentStatus, hasUpdateAccess, hasDeleteAccess)
        //                }).ToList();

        //        }

        //        // Sorting.   
        //        //lstVM_Paid_History_Employee = this.SortByColumnWithOrderForShowingMyBill(order, orderDir, lstVM_Paid_History_Employee);
        //        // Total record count.   
        //        // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
        //        // Filter record count.   
        //        recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

        //        ////////////////////////////////////


        //        // Loading drop down lists.   
        //        result = this.Json(new
        //        {
        //            draw = Convert.ToInt32(draw),
        //            recordsTotal = totalRecords,
        //            recordsFiltered = recFilter,
        //            data = data
        //        }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Info   
        //        Console.Write(ex);
        //    }
        //    // Return info.   
        //    return result;
        //}
        private string GetButton(int PaymentStatus, bool hasUpdateAccess, bool hasDeleteAccess)
        {//s.DeleteBy.HasValue ? "" : 
            string button = "";
            if (PaymentStatus != AppUtils.PaymentStatusIsReceive && PaymentStatus != AppUtils.PaymentStatusIsDelete)
            {
                if (hasUpdateAccess)
                {
                    button += "<button id='btnPaymentHistoryEdit'  type='button' class='btn btn-default btn-circle'><i class='glyphicon glyphicon-pencil'></i></button>";
                }

            }
            if (PaymentStatus != AppUtils.PaymentStatusIsDelete)
            {
                if (hasDeleteAccess)
                {
                    button += "<button id='btnPaymentHistoryDelete' type='button' class='btn btn-default btn-circle' data-toggle='modal' data-target='#popModalForDeletePermently'><i class='glyphicon glyphicon-remove'></i></button>";
                }
            }
            return button;
        }
        private void HasAccessOnWhichAction(ref bool hasUpdateAccess, ref bool hasDeleteAccess)
        {
            int CurrentUserRightPermission = AppUtils.GetLoginUserRightPermissionID();
            UserRightPermission userRightPermission = db.UserRightPermission.Where(s => s.UserRightPermissionID == CurrentUserRightPermission).FirstOrDefault();

            List<int> lstRightPerssion = new List<int>();
            lstRightPerssion = userRightPermission.UserRightPermissionDetails.Trim(',').Split(',').Select(int.Parse).ToList();
            if (lstRightPerssion.Contains(int.Parse(AppUtils.Delete_ResellerPayment)))
            {
                hasDeleteAccess = true;
            }
            if (lstRightPerssion.Contains(int.Parse(AppUtils.Update_ResellerPayment)))
            {
                hasUpdateAccess = true;
            }
        }
        private string GetPaymentType(int ResellerPaymentGivenTypeID)
        {
            if (ResellerPaymentGivenTypeID == AppUtils.ResellerPaymentGivenTypeIsCash)
            {
                return "<span class='badge badge-primary'>Cash</span>";
            }
            else
            {
                return "<span class='badge badge-primary'>Check</span>";
            }
        }
        private string GetPaymentStatus(int paymentStatus)
        {
            if (paymentStatus == AppUtils.PaymentStatusIsReceive)
            {
                return "<span class='label  label-success'>Received</span>";
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


        [HttpPost]
        public ActionResult GetAutoCompleateInformationForSearchCriteria(string SearchText)
        {
            try
            {
                var iquerableReseller = db.Reseller.AsQueryable();

                var lstReviewerDetails = iquerableReseller.Where(s => (s.ResellerStatus != AppUtils.ResellerStatusIsDelete) && (s.ResellerName.Contains(SearchText) || s.ResellerLoginName.Contains(SearchText) || s.ResellerBusinessName.Contains(SearchText) || s.ResellerContact.Contains(SearchText))).Select(s => new { label = s.ResellerLoginName, val = s.ResellerID }).ToList();
                return Json(new { lstReviewerDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, lstReviewerDetails = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_ResellerPayment)]
        public ActionResult InsertResellerPayment(ResellerCustomPaymentInformation ResellerPayment)
        {
            try
            {

                if (int.Parse(ResellerPayment.PaymentStatus) == AppUtils.PaymentStatusIsDelete)
                {
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }

                Reseller reseller = db.Reseller.Find(ResellerPayment.ResellerID);
                if (ResellerPayment.ResellerPaymentID == 0)//reseller payment Create
                {
                    ResellerPaymentDetailsHistory resellerPaymentDetailsHistory = new ResellerPaymentDetailsHistory();
                    resellerPaymentDetailsHistory.ResellerID = ResellerPayment.ResellerID;
                    resellerPaymentDetailsHistory.LastAmount = reseller.ResellerBalance;
                    resellerPaymentDetailsHistory.PaymentAmount = ResellerPayment.PaymentAmount;
                    resellerPaymentDetailsHistory.CollectBy = !string.IsNullOrEmpty(ResellerPayment.Collectby) ? int.Parse(ResellerPayment.Collectby) : 0;
                    resellerPaymentDetailsHistory.PaymentCheckOrAnySerial = ResellerPayment.PaymentCheckOrAnySerial;
                    resellerPaymentDetailsHistory.PaymentByID = !string.IsNullOrEmpty(ResellerPayment.PaymentBy) ? int.Parse(ResellerPayment.PaymentBy) : 0;
                    resellerPaymentDetailsHistory.PaymentStatus = !string.IsNullOrEmpty(ResellerPayment.PaymentStatus) ? int.Parse(ResellerPayment.PaymentStatus) : 0;
                    resellerPaymentDetailsHistory.PaymentYear = AppUtils.GetDateNow().Year;
                    resellerPaymentDetailsHistory.PaymentMonth = AppUtils.GetDateNow().Month;
                    resellerPaymentDetailsHistory.CreatedBy = AppUtils.GetLoginUserID();
                    resellerPaymentDetailsHistory.CreatedDate = AppUtils.GetDateTimeNow();
                    resellerPaymentDetailsHistory.ResellerPaymentGivenTypeID = !string.IsNullOrEmpty(ResellerPayment.PaymentTypeID) ? int.Parse(ResellerPayment.PaymentTypeID) : 0;
                    resellerPaymentDetailsHistory.ActiveBy = AppUtils.GetLoginUserID();
                    resellerPaymentDetailsHistory.Status = AppUtils.TableStatusIsActive;

                    if (ResellerPayment.PaymentStatus == AppUtils.PaymentStatusIsReceive.ToString())
                    {
                        resellerPaymentDetailsHistory.PaymenReceivedDate = AppUtils.GetDateTimeNow();
                    }
                    db.ResellerPaymentDetailsHistory.Add(resellerPaymentDetailsHistory);
                    db.SaveChanges();

                    if (resellerPaymentDetailsHistory.PaymentStatus == AppUtils.PaymentStatusIsReceive)
                    {
                        int enumVal = (int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString());
                        if (reseller.ResellerTypeListID == enumVal.ToString())
                        {
                            reseller.ResellerBalance += ResellerPayment.PaymentAmount;
                            reseller.UpdateBy = AppUtils.GetLoginUserID().ToString();
                            reseller.UpdateDate = AppUtils.GetDateTimeNow();
                            db.SaveChanges();
                        }
                    }

                }

                return Json(new { SuccessInsert = true, CurrentBalance = reseller.ResellerBalance }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Update_ResellerPayment)]
        public ActionResult UpdateResellerPayment(ResellerCustomPaymentInformation ResellerPayment)
        {

            Reseller reseller = db.Reseller.Find(ResellerPayment.ResellerID);
            double resellerOldPaymentAmount = reseller.ResellerBalance;

            try
            {
                ResellerPaymentDetailsHistory resellerPaymentDetailsHistory = db.ResellerPaymentDetailsHistory.Include("Reseller")
                                            .Where(s => s.ResellerPaymentID == ResellerPayment.ResellerPaymentID).FirstOrDefault();

                if (resellerPaymentDetailsHistory == null)
                {
                    return Json(new { UpdateSuccess = false }, JsonRequestBehavior.AllowGet);
                }


                resellerPaymentDetailsHistory.LastAmount = reseller.ResellerBalance;

                if (ResellerPayment.PaymentStatus == AppUtils.PaymentStatusIsReceive.ToString())
                {
                    int enumVal = (int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString());
                    if (reseller.ResellerTypeListID == enumVal.ToString())
                    {
                        reseller.ResellerBalance += ResellerPayment.PaymentAmount;
                        reseller.UpdateBy = AppUtils.GetLoginUserID().ToString();
                        reseller.UpdateDate = AppUtils.GetDateTimeNow();
                        db.SaveChanges();
                    }
                    resellerPaymentDetailsHistory.PaymenReceivedDate = AppUtils.GetDateTimeNow();
                }
                if (ResellerPayment.PaymentStatus == AppUtils.PaymentStatusIsDelete.ToString())
                {
                    int enumVal = (int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString());
                    if (reseller.ResellerTypeListID == enumVal.ToString())
                    {
                        reseller.ResellerBalance += ResellerPayment.PaymentAmount;
                        reseller.UpdateBy = AppUtils.GetLoginUserID().ToString();
                        reseller.UpdateDate = AppUtils.GetDateTimeNow();
                        db.SaveChanges();
                    }

                    resellerPaymentDetailsHistory.DeleteTimeResellerAmount = reseller.ResellerBalance;
                    resellerPaymentDetailsHistory.DeleteBy = AppUtils.GetLoginUserID();
                    resellerPaymentDetailsHistory.DeleteDate = AppUtils.GetDateTimeNow();
                }

                //resellerPaymentDetailsHistory.ResellerID = ResellerPayment.ResellerID;
                resellerPaymentDetailsHistory.PaymentAmount = ResellerPayment.PaymentAmount;
                resellerPaymentDetailsHistory.CollectBy = !string.IsNullOrEmpty(ResellerPayment.Collectby) ? int.Parse(ResellerPayment.Collectby) : 0;
                resellerPaymentDetailsHistory.PaymentCheckOrAnySerial = ResellerPayment.PaymentCheckOrAnySerial;
                resellerPaymentDetailsHistory.PaymentByID = !string.IsNullOrEmpty(ResellerPayment.PaymentBy) ? int.Parse(ResellerPayment.PaymentBy) : 0;
                resellerPaymentDetailsHistory.PaymentStatus = !string.IsNullOrEmpty(ResellerPayment.PaymentStatus) ? int.Parse(ResellerPayment.PaymentStatus) : 0;
                //resellerPaymentDetailsHistory.PaymentYear = AppUtils.GetDateNow().Year;
                //resellerPaymentDetailsHistory.PaymentMonth = AppUtils.GetDateNow().Month;
                //resellerPaymentDetailsHistory.CreatedBy = AppUtils.GetLoginUserID();
                //resellerPaymentDetailsHistory.CreatedDate = AppUtils.GetDateTimeNow();
                resellerPaymentDetailsHistory.ResellerPaymentGivenTypeID = !string.IsNullOrEmpty(ResellerPayment.PaymentTypeID) ? int.Parse(ResellerPayment.PaymentTypeID) : 0;
                //resellerPaymentDetailsHistory.ActiveBy = AppUtils.GetLoginUserID();
                //resellerPaymentDetailsHistory.Status = AppUtils.TableStatusIsActive;
                resellerPaymentDetailsHistory.UpdateBy = AppUtils.GetLoginUserID();
                resellerPaymentDetailsHistory.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(resellerPaymentDetailsHistory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                return Json(new { SuccessInsert = true, CurrentBalance = reseller.ResellerBalance }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //[ValidateJsonAntiForgeryTokenAttribute]
        //[UserRIghtCheck(ControllerValue = AppUtils.Update_ResellerPayment)]
        //public ActionResult UpdateResellerPayment(ResellerCustomPaymentInformation ResellerPayment)
        //{
        //    try
        //    {
        //        if (ResellerPayment.ResellerPaymentID == 0)//reseller payment Create
        //        {
        //            ResellerPaymentDetailsHistory resellerPaymentDetailsHistory = new ResellerPaymentDetailsHistory();
        //            resellerPaymentDetailsHistory.ResellerID = ResellerPayment.ResellerID;
        //            resellerPaymentDetailsHistory.PaymentAmount = ResellerPayment.PaymentAmount;
        //            resellerPaymentDetailsHistory.CollectBy = !string.IsNullOrEmpty(ResellerPayment.Collectby) ? int.Parse(ResellerPayment.Collectby) : 0;
        //            resellerPaymentDetailsHistory.PaymentCheckOrAnySerial = ResellerPayment.PaymentCheckOrAnySerial;
        //            resellerPaymentDetailsHistory.PaymentByID = !string.IsNullOrEmpty(ResellerPayment.PaymentBy) ? int.Parse(ResellerPayment.PaymentBy) : 0;
        //            resellerPaymentDetailsHistory.PaymentStatus = !string.IsNullOrEmpty(ResellerPayment.PaymentStatus) ? int.Parse(ResellerPayment.PaymentStatus) : 0;
        //            resellerPaymentDetailsHistory.PaymentYear = AppUtils.GetDateNow().Year;
        //            resellerPaymentDetailsHistory.PaymentMonth = AppUtils.GetDateNow().Month;
        //            resellerPaymentDetailsHistory.CreatedBy = AppUtils.GetLoginUserID();
        //            resellerPaymentDetailsHistory.CreatedDate = AppUtils.GetDateTimeNow();
        //            resellerPaymentDetailsHistory.ResellerPaymentGivenTypeID = !string.IsNullOrEmpty(ResellerPayment.PaymentTypeID) ? int.Parse(ResellerPayment.PaymentTypeID) : 0;
        //            resellerPaymentDetailsHistory.ActiveBy = AppUtils.GetLoginUserID();
        //            resellerPaymentDetailsHistory.Status = AppUtils.TableStatusIsActive;
        //            db.ResellerPaymentDetailsHistory.Add(resellerPaymentDetailsHistory);
        //            db.SaveChanges();
        //        }
        //        return Json(new { SuccessUpdate = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { SuccessUpdate = false }, JsonRequestBehavior.AllowGet);
        //    }

        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetResellerDetailsByID(int RID)
        {
            var Reseller = db.Reseller.Where(s => s.ResellerID == RID).Select(s => new
            {
                RID = s.ResellerID,
                ResellerName = s.ResellerName,
                ResellerLoginName = s.ResellerLoginName,
                ResellerPassword = s.ResellerPassword,
                ResellerType = s.ResellerTypeListID,
                ResellerTypeID = s.ResellerTypeListID,
                ResellerAddress = s.ResellerAddress,
                ResellerPhone = s.ResellerContact,
                ResellerBusinessName = s.ResellerBusinessName,
                ResellerBillingCycle = s.ResellerBillingCycleList,
                macResellerGivenPackagePrice = s.macReselleGivenPackageWithPrice,
                bandwithReselleGivenItemWithPrice = s.BandwithReselleItemGivenWithPrice,
                ResellerStatus = s.ResellerStatus,
                ResellerLogoPath = s.ResellerLogoPath,
                MacResellerAssignMikrotik = s.MacResellerAssignMikrotik
            }).ToList().Select(
                s =>
                new
                {
                    RID = s.RID,
                    ResellerName = s.ResellerName,
                    ResellerLoginName = s.ResellerLoginName,
                    ResellerBusinessName = s.ResellerBusinessName,
                    ResellerPassword = s.ResellerPassword,
                    ResellerTypeID = s.ResellerTypeID,
                    ResellerType = Enum.GetName(typeof(ResellerTypeEnum), int.Parse(s.ResellerType)),
                    ResellerAddress = s.ResellerAddress,
                    ResellerPhone = s.ResellerPhone,
                    ResellerBillingCycle = s.ResellerBillingCycle != null ? s.ResellerBillingCycle.Trim(',') : "",
                    macResellerGivenPackagePriceModel = SetPackageName((s.macResellerGivenPackagePrice != null) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(s.macResellerGivenPackagePrice) : new List<macReselleGivenPackageWithPriceModel>()),
                    bandwithReselleGivenItemWithPriceModel = SetItemName((s.bandwithReselleGivenItemWithPrice != null) ? new JavaScriptSerializer().Deserialize<List<bandwithReselleGivenItemWithPriceModel>>(s.bandwithReselleGivenItemWithPrice) : new List<bandwithReselleGivenItemWithPriceModel>()),
                    ResellerStatus = s.ResellerStatus,
                    ResellerLogoPath = s.ResellerLogoPath,
                    MacResellerAssignMikrotik = !string.IsNullOrEmpty(s.MacResellerAssignMikrotik) ? s.MacResellerAssignMikrotik.Trim(',') : ""
                }).FirstOrDefault();

            var JSON = Json(new { ResellerDetails = Reseller, Success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowResellerPaymentDetailsByID(int RPID)
        {
            try
            {
                var ResellerPaymentDetailsForUpdate = db.ResellerPaymentDetailsHistory.Include("Reseller")
                    .Where(s => s.ResellerPaymentID == RPID)
                    .Select(s => new
                    {
                        ResellerPaymentID = s.ResellerPaymentID,
                        ResellerLoginName = s.Reseller.ResellerLoginName,
                        ResellerType = s.Reseller.ResellerTypeListID,
                        ResellerBusinessName = s.Reseller.ResellerBusinessName,
                        ResellerAddress = s.Reseller.ResellerAddress,
                        ResellerPhone = s.Reseller.ResellerContact,
                        ResellerLogoPath = s.Reseller.ResellerLogoPath,
                        PaymentAmount = s.PaymentAmount,
                        CollectBy = s.CollectBy,
                        PaymentCheckOrAnySerial = s.PaymentCheckOrAnySerial,
                        PaymentBy = s.PaymentByID,
                        PaymentStatus = s.PaymentStatus.ToString(),
                        PaymentTypeID = s.ResellerPaymentGivenTypeID
                    }).ToList().Select(
                s =>
                new ResellerCustomPaymentInformation
                {

                    ResellerPaymentID = s.ResellerPaymentID,
                    ResellerLoginName = s.ResellerLoginName,
                    ResellerType = Enum.GetName(typeof(ResellerTypeEnum), int.Parse(s.ResellerType)),
                    ResellerBusinessName = s.ResellerBusinessName,
                    ResellerAddress = s.ResellerAddress,
                    ResellerPhone = s.ResellerPhone,
                    ResellerLogoPath = s.ResellerLogoPath,
                    PaymentAmount = s.PaymentAmount,
                    Collectby = s.CollectBy.ToString(),
                    PaymentCheckOrAnySerial = s.PaymentCheckOrAnySerial,
                    PaymentBy = s.PaymentBy.ToString(),
                    PaymentTypeID = s.PaymentTypeID.ToString(),
                    PaymentStatus = s.PaymentStatus
                }).FirstOrDefault();
                return Json(new { RPD = ResellerPaymentDetailsForUpdate, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            //    $("#txtResellerLoginName").val(data.ResellerLoginName);
            //$("#txtResellerType").val(data.ResellerType);
            //$("#txtResellerBusinessName").val(data.ResellerBusinessName);
            //$("#txtResellerAddress").val(data.ResellerAddress);
            //$("#txtResellerPhone").val(data.ResellerPhone);
            //$("#ResellerLogoPath").prop("src", data.ResellerLogoPath);
            //$("#txtResellerPaymentAmount").val(data.PaymentAmount);
            //$("#ddlResellerCollectBy").val(data.CollectBy);
            //$("#txtCheckSerialOrAnyResetNo").val(data.PaymentCheckOrAnySerial);
            //$("#ddlPaymentBy").val(data.CreatedBy);
            //$("#ddlPaymentStatus").val(data.PaymentStatus);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowResellerDetailsByIDForCreatingPayment(int RID)
        {
            try
            {
                var ResellerDetailsForCreatePayment = db.Reseller
                    .Where(s => s.ResellerID == RID)
                    .Select(s => new
                    {
                        ResellerID = s.ResellerID,
                        ResellerLoginName = s.ResellerLoginName,
                        ResellerType = s.ResellerTypeListID,
                        ResellerBusinessName = s.ResellerBusinessName,
                        ResellerAddress = s.ResellerAddress,
                        ResellerPhone = s.ResellerContact,
                        ResellerLogoPath = s.ResellerLogoPath,
                        PaymentAmount = s
                    }).ToList().Select(
                s =>
                new ResellerCustomPaymentInformation
                {
                    ResellerPaymentID = s.ResellerID,
                    ResellerLoginName = s.ResellerLoginName,
                    ResellerType = Enum.GetName(typeof(ResellerTypeEnum), int.Parse(s.ResellerType)),
                    ResellerBusinessName = s.ResellerBusinessName,
                    ResellerAddress = s.ResellerAddress,
                    ResellerPhone = s.ResellerPhone,
                    ResellerLogoPath = s.ResellerLogoPath,

                }).FirstOrDefault();
                return Json(new { RD = ResellerDetailsForCreatePayment, Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_ResellerPayment)]
        public ActionResult DeleteResellerPaymentByID(int ResellerPaymentID)
        {
            try
            {
                ResellerPaymentDetailsHistory ResellerPaymentDetailsHistoryDelete = db.ResellerPaymentDetailsHistory.Find(ResellerPaymentID);
                Reseller reseller = db.Reseller.Find(ResellerPaymentDetailsHistoryDelete.ResellerID);
                ResellerPaymentDetailsHistoryDelete.DeleteTimeResellerAmount = reseller.ResellerBalance;
                ResellerPaymentDetailsHistoryDelete.DeleteBy = AppUtils.GetLoginUserID();
                ResellerPaymentDetailsHistoryDelete.DeleteDate = AppUtils.GetDateTimeNow();
                ResellerPaymentDetailsHistoryDelete.Status = AppUtils.TableStatusIsDelete;
                ResellerPaymentDetailsHistoryDelete.PaymentStatus = AppUtils.PaymentStatusIsDelete;
                db.SaveChanges();

                int enumVal = (int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString());
                if (ResellerPaymentDetailsHistoryDelete.Reseller.ResellerTypeListID == enumVal.ToString())
                {
                    reseller.ResellerBalance -= ResellerPaymentDetailsHistoryDelete.PaymentAmount;
                    reseller.UpdateBy = AppUtils.GetLoginUserID().ToString();
                    reseller.UpdateDate = AppUtils.GetDateTimeNow();
                    db.SaveChanges();
                }

                return Json(new { DeleteStatus = true, CurrentBalance = reseller.ResellerBalance }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, }, JsonRequestBehavior.AllowGet);
            }

        }

        //[HttpGet]
        //[UserRIghtCheck(ControllerValue = AppUtils.Change_Reseller_PackagePrice)]
        //public ActionResult ResellerPackagePriceChange()
        //{
        //    int resellerID = AppUtils.GetLoginUserID();
        //    Reseller reseller = db.Reseller.Find(resellerID);
        //    List<macReselleGivenPackageWithPriceModel> macReselleGivenPackage = (List<macReselleGivenPackageWithPriceModel>)SetPackageName((reseller.macReselleGivenPackageWithPrice != null) ? new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>());
        //    return View(macReselleGivenPackage);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMacResellerPackageAndZoneDetailsByResellerID(int resellerid)
        {
            Reseller reseller = db.Reseller.Find(resellerid);
            List<macReselleGivenPackageWithPriceModel> lstMacResellerPackage = reseller != null ? (List<macReselleGivenPackageWithPriceModel>)new JavaScriptSerializer().Deserialize<List<macReselleGivenPackageWithPriceModel>>(reseller.macReselleGivenPackageWithPrice) : new List<macReselleGivenPackageWithPriceModel>();
            List<int> lstMirkotik = string.IsNullOrEmpty(reseller.MacResellerAssignMikrotik) ? new List<int>() : reseller.MacResellerAssignMikrotik.Trim(',').Split(',').Select(x=>int.Parse(x)).ToList();
            var lstMacResellerMikrotik = db.Mikrotik.Where(x=> lstMirkotik.Contains(x.MikrotikID)).Select(x=>new { mikid = x.MikrotikID, mikname = x.MikName });

            var lstZone = db.Zone.Where(x=>x.ResellerID == reseller.ResellerID).Select(x => new { zoneid = x.ZoneID, zonename = x.ZoneName }).ToList();
            var lstPackage = lstMacResellerPackage.Select(x => new { packageid = x.PID, packagename = x.PName }).ToList();
            var lstBox = db.Box.Where(x=>x.ResellerID == reseller.ResellerID).Select(x => new { boxid = x.BoxID, boxname = x.BoxName }).ToList();
            var lstMIkrotik = lstMacResellerMikrotik;
            return Json(new { resellerzone = lstZone,resellerpackage = lstPackage,resellerbox = lstBox,resellerMikrotik = lstMacResellerMikrotik, Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}