using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_ISP.Controllers
{
    public class AssetController : Controller
    {
        public AssetController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAssetDetailsByAssetTypeID(int AssetTypeID)
        {

            try
            {
                List<AssetCustomList> lstAssetCustomList = db.Asset.Where(s=>s.AssetTypeID == AssetTypeID).Select(fp => new AssetCustomList
                {
                    AssetID = fp.AssetID,
                    AssetTypeName = fp.AssetType.AssetTypeName,
                    AssetName = fp.AssetName,
                    AssetValue = fp.AssetValue,
                    PurchaseDate = fp.PurchaseDate,
                    SerialNumber = fp.SerialNumber,
                    WarrentyStartDate = fp.WarrentyStartDate,
                    WarrentyEndDate = fp.WarrentyEndDate,

                }).ToList();
                return Json(new { Success = true, lstAssetByAssetTypeID = lstAssetCustomList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_Asset_List)]
        public ActionResult Index()
        {
            //List<Asset> lstAsset = db.Asset.ToList();
            var lstAssetType = new SelectList(db.AssetType.Select(s => new { AssetTypeID = s.AssetTypeID, AssetTypeName = s.AssetTypeName }), "AssetTypeID", "AssetTypeName");
            ViewBag.SearchByAssetTypeID = lstAssetType;
            ViewBag.lstAssetTypeUpdate = lstAssetType;
            ViewBag.lstAssetType = lstAssetType;
            return View(new List<Asset>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAssetAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                int AssetTypeIDFromDDL = 0;
                // Initialization.   
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                string AssetTypeID = Request.Form.Get("AssetTypeID");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (AssetTypeID != "")
                {
                    AssetTypeIDFromDDL = int.Parse(AssetTypeID);
                }
                // Loading.   

                // Apply pagination.   
                //data = data.Skip(startRec).Take(pageSize).ToList();
                var firstPart = (AssetTypeID != "") ? db.Asset.Where(s => s.AssetTypeID == AssetTypeIDFromDDL).AsEnumerable() : db.Asset.AsEnumerable();

                var assetList = new List<Asset>();


                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (firstPart.Any()) ? firstPart.Where(p => p.AssetType.AssetTypeName.ToLower().Contains(search.ToLower())
                                                                     || p.AssetName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.AssetValue.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.SerialNumber.ToString().ToLower().Contains(search.ToLower())
                    ).Count() : 0;

                    // Apply search   
                    firstPart = firstPart.Where(p => p.AssetType.AssetTypeName.ToLower().Contains(search.ToLower())
                                                                     || p.AssetName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.AssetValue.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.SerialNumber.ToString().ToLower().Contains(search.ToLower())
                    ).AsEnumerable();
                }


                List<AssetCustomList> data =
                        firstPart.Any() ? (from fp in firstPart.Skip(startRec).Take(pageSize)
                                           join aT in db.AssetType on fp.AssetTypeID equals aT.AssetTypeID into a
                                           from fpap in a.DefaultIfEmpty()
                                           select new AssetCustomList
                                           {
                                               AssetID = fp.AssetID,
                                               AssetTypeName = fpap.AssetTypeName,
                                               AssetName = fp.AssetName,
                                               AssetValue = fp.AssetValue,
                                               PurchaseDate = fp.PurchaseDate,
                                               SerialNumber = fp.SerialNumber,
                                               WarrentyStartDate = fp.WarrentyStartDate,
                                               WarrentyEndDate = fp.WarrentyEndDate,
                                               Button = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Asset) ? true : false,
                                           }).ToList() : new List<AssetCustomList>();
                // Verification.   

                // Sorting.   
                finalItem = SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                totalRecords = firstPart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                                                                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPart.AsEnumerable().Count();

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

        private IEnumerable<dynamic> SortByColumnWithOrder(string order, string orderDir, IEnumerable<dynamic> finalItem)
        {
            // Initialization.   
            List<dynamic> lst = new List<dynamic>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.AssetID).ToList() : finalItem.OrderBy(p => p.AssetID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.AssetTypeName).ToList() : finalItem.OrderBy(p => p.AssetTypeName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.AssetName).ToList() : finalItem.OrderBy(p => p.AssetName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.AssetValue).ToList() : finalItem.OrderBy(p => p.AssetValue).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.PurchaseDate).ToList() : finalItem.OrderBy(p => p.PurchaseDate).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.SerialNumber).ToList() : finalItem.OrderBy(p => p.SerialNumber).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.WarrentyStartDate).ToList() : finalItem.OrderBy(p => p.WarrentyStartDate).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.WarrentyEndDate).ToList() : finalItem.OrderBy(p => p.WarrentyEndDate).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.AssetID).ToList() : finalItem.OrderBy(p => p.AssetID).ToList();
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

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Asset)]
        public ActionResult InsertAsset()
        {
            ViewBag.lstAssetType = new SelectList(db.AssetType.Select(s => new { AssetTypeID = s.AssetTypeID, AssetTypeName = s.AssetTypeName }), "AssetTypeID", "AssetTypeName");

            return View();
        }


        [HttpPost]
        public ActionResult InsertAsset(Asset Asset_Client)
        {
            //Asset Asset_Check = db.Asset.Where(s => s.AssetName == Asset_Client.AssetName.Trim()).FirstOrDefault();

            //if (Asset_Check != null)
            //{
            //    TempData["AlreadyInsert"] = "Asset Already Added. Choose different Asset. ";

            //    return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            //}

            Asset Asset_Return = new Asset();

            try
            {
                Asset_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Asset_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Asset_Return = db.Asset.Add(Asset_Client);
                db.SaveChanges();

                if (Asset_Return.AssetID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Asset = Asset_Return }, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public ActionResult InsertAssetFromPopUp(Asset Asset_Client)
        {
            //Asset Asset_Check = db.Asset.Where(s => s.AssetName == Asset_Client.AssetName.Trim()).FirstOrDefault();

            //if (Asset_Check != null)
            //{
            //    //  TempData["AlreadyInsert"] = "Asset Already Added. Choose different Asset. ";

            //    return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            //}

            Asset Asset_Return = new Asset();

            try
            {
                Asset_Client.PurchaseDate = Asset_Client.PurchaseDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                Asset_Client.WarrentyStartDate = (Asset_Client.WarrentyStartDate != null) ? Asset_Client.WarrentyStartDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second) : Asset_Client.WarrentyStartDate;
                Asset_Client.WarrentyEndDate = Asset_Client.WarrentyEndDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                Asset_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Asset_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Asset_Return = db.Asset.Add(Asset_Client);
                db.SaveChanges();

                if (Asset_Return.AssetID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Asset = Asset_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //   TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAssetDetailsByID(int AssetID)
        {
            var Asset = db.Asset.Where(s => s.AssetID == AssetID).Select(s => new { AssetTypeID = s.AssetTypeID, AssetName = s.AssetName, AssetValue = s.AssetValue, PurchaseDate = s.PurchaseDate, SerialNumber = s.SerialNumber, WarrentyStartDate = s.WarrentyStartDate, WarrentyEndDate = s.WarrentyEndDate }).FirstOrDefault();


            var JSON = Json(new { AssetDetails = Asset }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateAsset(Asset AssetInfoForUpdate)
        {

            try
            {

                Asset Asset_Check = db.Asset.Where(s => s.AssetID == AssetInfoForUpdate.AssetID).FirstOrDefault();

                if (Asset_Check == null)
                {
                    //TempData["AlreadyInsert"] = "Asset Already Added. Choose different Asset. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Asset_db = db.Asset.Where(s => s.AssetID == AssetInfoForUpdate.AssetID);
                //AssetInfoForUpdate.PurchaseDate = AssetInfoForUpdate.PurchaseDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                //AssetInfoForUpdate.WarrentyStartDate = (AssetInfoForUpdate.WarrentyStartDate != null) ? AssetInfoForUpdate.WarrentyStartDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second) : AssetInfoForUpdate.WarrentyStartDate;
                //AssetInfoForUpdate.WarrentyEndDate = AssetInfoForUpdate.WarrentyEndDate.Value.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);

                AssetInfoForUpdate.CreatedBy = Asset_db.FirstOrDefault().CreatedBy;
                AssetInfoForUpdate.CreatedDate = Asset_db.FirstOrDefault().CreatedDate;
                AssetInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                AssetInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Asset_db.SingleOrDefault()).CurrentValues.SetValues(AssetInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var temp = Asset_db.ToList();
                var Asset_Return = Asset_db.Select(s =>
                new
                {
                    AssetID = s.AssetID,
                    AssetTypeName = s.AssetType.AssetTypeName,
                    AssetName = s.AssetName,
                    AssetValue = s.AssetValue,
                    PurchaseDate = s.PurchaseDate,
                    SerialNumber = s.SerialNumber,
                    WarrentyStartDate = s.WarrentyStartDate,
                    WarrentyEndDate = s.WarrentyEndDate
                });
                var JSON = Json(new { UpdateSuccess = true, AssetUpdateInformation = Asset_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, AssetUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Asset)]
        public ActionResult DeleteAsset(int AssetID)
        {
            try
            {
                Asset Asset = db.Asset.Find(AssetID);
                db.SaveChanges();
                return Json(new { DeleteStatus = true, AssetID = AssetID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { DeleteStatus = false, AssetID = AssetID }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult AssetOverView()
        {
            ViewBag.AssetTypeID = new SelectList(db.AssetType.Select(s => new { AssetTypeID = s.AssetTypeID, AssetTypeName = s.AssetTypeName }), "AssetTypeID", "AssetTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomAssetListOverview()
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

                int assetTypeIDFromDDL = 0;
                var AssetTypeID = Request.Form.Get("AssetTypeID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomAssetTypeOverview> lstCustomAssetTypeOverview = new List<CustomAssetTypeOverview>();
                int AssetTypeIDConvert = 0;
                if (AssetTypeID != "")
                {
                    AssetTypeIDConvert = int.Parse(AssetTypeID);
                }

                var firstPartOfQuery = (AssetTypeID != "") ? db.AssetType.Where(s => s.AssetTypeID == AssetTypeIDConvert).OrderBy(s => s.AssetTypeName).AsEnumerable() : db.AssetType.OrderBy(s => s.AssetTypeName).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.AssetTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.AssetTypeName.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomAssetTypeOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomAssetTypeOverview()
                        {
                            AssetTypeID = s.AssetTypeID,
                            AssetTypeName = s.AssetTypeName,
                            TotalAssetTypeCount = db.Asset.Where(ss => ss.AssetTypeID == s.AssetTypeID).Count()

                        }).ToList();

                }

                // Sorting.   
                lstCustomAssetTypeOverview = this.SortByColumnWithOrderAssetOverView(order, orderDir, lstCustomAssetTypeOverview);
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
                    data = lstCustomAssetTypeOverview
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



        private List<CustomAssetTypeOverview> SortByColumnWithOrderAssetOverView(string order, string orderDir, List<CustomAssetTypeOverview> data)
        {
            // Initialization.   
            List<CustomAssetTypeOverview> lst = new List<CustomAssetTypeOverview>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetTypeName).ToList() : data.OrderBy(p => p.AssetTypeName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetTypeID).ToList() : data.OrderBy(p => p.AssetTypeID).ToList();
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
    }
}