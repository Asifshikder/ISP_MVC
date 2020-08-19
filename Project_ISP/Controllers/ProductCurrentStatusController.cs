//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Dynamic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using ISP_ManagementSystemModel;
//using ISP_ManagementSystemModel.Models;
//using ISP_ManagementSystemModel.ViewModel;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

//namespace Project_ISP.Controllers
//{
//    [SessionTimeout]
//    [AjaxAuthorizeAttribute]
//    public class ProductCurrentStatusController : Controller
//    {
//        public ProductCurrentStatusController()
//        {
//            AppUtils.dateTimeNow = DateTime.Now;
//        }
//        private ISPContext db = new ISPContext();


//        [HttpGet]
//        //[UserRIghtCheck(ControllerValue = AppUtils.ProductCurrentStatus_Ca)]
//        public ActionResult CablesHistory()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.WarrantySection).ToList(), "SectionID", "SectionName");
//            List<StockDetails> lstSoStockDetailse =
//                db.StockDetails.Where(
//                    s =>
//                        s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
//                    .ToList();
//            return View(lstSoStockDetailse);
//        }


//        // GET: StockDetails
//        [HttpGet]

//        // [UserRIghtCheck(ControllerValue = AppUtils.Warranty)]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Warrenty_List)]
//        public ActionResult WarrentyList()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.WarrantySection).ToList(), "SectionID", "SectionName");
//            //List<StockDetails> lstSoStockDetailse =
//            //    db.StockDetails.Where(
//            //        s =>
//            //            s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
//            //        .ToList();
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockWarrentyListSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                                      || p.Serial.Contains(search.ToLower())
//                                                                                      || p.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                   || p.Serial.Contains(search.ToLower())
//                                                                   || p.Section.SectionName.Contains(search.ToLower())
//                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            StockDetailsID = s.StockDetailsID,
//                            SectionID = s.SectionID,
//                            ProductStatusID = s.ProductStatusID,
//                            ItemName = s.Stock.Item.ItemName,
//                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                            Serial = s.Serial,
//                            SectionName = s.Section.SectionName,
//                            ProductStatusName = s.ProductStatus.ProductStatusName,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
//                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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



//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Dead_List)]
//        public ActionResult DeadList()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.DeadSection).ToList(), "SectionID", "SectionName");
//            //List<StockDetails> lstSoStockDetailse =
//            //    db.StockDetails.Where(
//            //        s =>
//            //            s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection)
//            //        .ToList();
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockDeadListSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                                      || p.Serial.Contains(search.ToLower())
//                                                                                      || p.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                   || p.Serial.Contains(search.ToLower())
//                                                                   || p.Section.SectionName.Contains(search.ToLower())
//                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            StockDetailsID = s.StockDetailsID,
//                            SectionID = s.SectionID,
//                            ProductStatusID = s.ProductStatusID,
//                            ItemName = s.Stock.Item.ItemName,
//                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                            Serial = s.Serial,
//                            SectionName = s.Section.SectionName,
//                            ProductStatusName = s.ProductStatus.ProductStatusName,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
//                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"
//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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



//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_avialable_List)]
//        public ActionResult AvialableList()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.StockSection).ToList(), "SectionID", "SectionName");
//            //List<StockDetails> lstSoStockDetailse =
//            //    db.StockDetails.Where(
//            //        s =>
//            //            s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection)
//            //        .ToList();
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockAvailableListSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Brand.BrandName.Contains(search.ToLower())
//                                                                                      || p.Serial.Contains(search.ToLower())
//                                                                                      || p.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Brand.BrandName.Contains(search.ToLower())
//                                                                   || p.Serial.Contains(search.ToLower())
//                                                                   || p.Section.SectionName.Contains(search.ToLower())
//                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            StockDetailsID = s.StockDetailsID,
//                            SectionID = s.SectionID,
//                            ProductStatusID = s.ProductStatusID,
//                            ItemName = s.Stock.Item.ItemName,
//                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                            Serial = s.Serial,
//                            SectionName = s.Section.SectionName,
//                            ProductStatusName = s.ProductStatus.ProductStatusName,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
//                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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



//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_repairing_List)]
//        public ActionResult RepairingList()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.RepairingSection).ToList(), "SectionID", "SectionName");
//            //List<StockDetails> lstSoStockDetailse =
//            //    db.StockDetails.Where(
//            //        s =>
//            //            s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection)
//            //        .ToList();
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockRepairingListSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                                      || p.Serial.Contains(search.ToLower())
//                                                                                      || p.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
//                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                   || p.Serial.Contains(search.ToLower())
//                                                                   || p.Section.SectionName.Contains(search.ToLower())
//                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            StockDetailsID = s.StockDetailsID,
//                            SectionID = s.SectionID,
//                            ProductStatusID = s.ProductStatusID,
//                            ItemName = s.Stock.Item.ItemName,
//                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                            Serial = s.Serial,
//                            SectionName = s.Section.SectionName,
//                            ProductStatusName = s.ProductStatus.ProductStatusName,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
//                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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




//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_running_List)]
//        public ActionResult RunningList()
//        {
//            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

//            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

//            ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
//            ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
//            ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
//            ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
//            ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");


//            ViewBag.StockID = new SelectList(db.Stock.Select(s => new { StockID = s.StockID, ItemName = s.Item.ItemName }), "StockID", "ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");


//            //List<StockDetails> lstStockDetailse =
//            //    db.StockDetails.Where(
//            //        s =>
//            //            s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//            //        .ToList();
//            //List<int> lstStockDetailsID = (lstStockDetailse != null) ? lstStockDetailse.Select(s => s.StockDetailsID).ToList() : new List<int>();

//            //List<Distribution> lstDistributions = db.Distribution.Where(s => lstStockDetailsID.Contains(s.StockDetailsID)).ToList();


//            //List<int> clientDetailsID = lstDistributions.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
//            //if (clientDetailsID.Count > 0)
//            //{
//            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
//            //                  .Select(s => new ClientSetByViewBag
//            //                  {
//            //                      ClientDetailsID = s.ClientDetailsID,
//            //                      TransactionID = s.TransactionID,
//            //                      PaymentAmount = s.PaymentAmount.Value,

//            //                  }).ToList();
//            //}
//            //else
//            //{
//            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
//            //                  .Select(s => new
//            //                  {
//            //                      ClientDetailsID = s.ClientDetailsID,
//            //                      TransactionID = s.TransactionID,
//            //                      PaymentAmount = s.PaymentAmount.Value,

//            //                  }).ToList();
//            //}



//            //VM_lstStockDetails_lstDistribution VM_lstStockDetails_lstDistribution = new VM_lstStockDetails_lstDistribution();
//            //VM_lstStockDetails_lstDistribution.lstDistribution = lstDistributions;
//            //VM_lstStockDetails_lstDistribution.lstStockDetails = lstStockDetailse;

//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockRunningSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection).AsEnumerable();

//                var secondPartOfQuery = firstPartOfQuery.GroupJoin(db.Distribution, StockDetails => StockDetails.StockDetailsID, Distribution => Distribution.StockDetailsID, (StockDetails, Distribution) => new
//                {
//                    StockDetails = StockDetails,
//                    Distribution = (Distribution.Count() == 0) ? new Distribution() : Distribution.FirstOrDefault()
//                }).AsEnumerable();

//                foreach (var VARIABLE in secondPartOfQuery)
//                {
//                    if (VARIABLE.Distribution != null)
//                    {
//                        if (VARIABLE.Distribution.ClientDetails == null)
//                        {
//                            VARIABLE.Distribution.ClientDetails = new ClientDetails() { Name = "" };

//                        }
//                        if (VARIABLE.Distribution.Employee == null)
//                        {
//                            VARIABLE.Distribution.Employee = new Employee() { Name = "" };

//                        }
//                        if (VARIABLE.Distribution.Pop == null)
//                        {
//                            VARIABLE.Distribution.Pop = new Pop() { PopName = "" };

//                        }
//                        if (VARIABLE.Distribution.Box == null)
//                        {
//                            VARIABLE.Distribution.Box = new Box() { BoxName = "" };

//                        }
//                    }
//                }

//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

//                                                                                        || p.Distribution.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                                        || p.Distribution.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                                        || p.Distribution.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
//                                                                                        || p.Distribution.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

//                                                                                        //|| p.StockDetails.Brand.BrandName.Contains(search.ToLower())
//                                                                                        || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                                        || p.StockDetails.Serial.Contains(search.ToLower())
//                                                                                      || p.StockDetails.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    secondPartOfQuery = secondPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

//                                                                    || p.Distribution.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.Distribution.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.Distribution.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.Distribution.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

//                                                                    //|| p.StockDetails.Brand.BrandName.Contains(search.ToLower())
//                                                                    || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                    || p.StockDetails.Serial.Contains(search.ToLower())
//                                                                    || p.StockDetails.Section.SectionName.Contains(search.ToLower())
//                                                                    || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))

//                        .AsEnumerable();
//                }
//                //var test = secondPartOfQuery.ToList();
//                var thirdPartOfquery = secondPartOfQuery
//                    .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
//                        Distribution => Distribution.Distribution.ClientDetails.ClientDetailsID,
//                        Transaction => Transaction.ClientDetailsID,
//                        (Distribution, Transaction) => new
//                        {
//                            StockDetails = Distribution.StockDetails,
//                            Distribution = Distribution.Distribution,
//                            Transaction = Transaction
//                        })
//                          .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), StockDetails => StockDetails.Distribution.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (StockDetails, ClientLineStatus) => new
//                          {
//                              StockDetails = StockDetails.Distribution.StockDetails,
//                              Distribution = StockDetails.Distribution,
//                              Transaction = StockDetails.Transaction,
//                              ClientLineStatus = ClientLineStatus.FirstOrDefault(),

//                          }).AsEnumerable();
//                //     var a = thirdPartOfquery.ToList();
//                if (thirdPartOfquery.Count() > 0)
//                {
//                    //var i = thirdPartOfquery.ToList();
//                    totalRecords = secondPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = thirdPartOfquery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
//                            StockDetailsID = s.StockDetails.StockDetailsID,
//                            SectionID = s.StockDetails.SectionID,
//                            ProductStatusID = s.StockDetails.ProductStatusID,
//                            ItemName = s.StockDetails.Stock.Item.ItemName,
//                            BrandName = s.StockDetails.Brand != null ? s.StockDetails.Brand.BrandName : "",
//                            Serial = s.StockDetails.Serial,
//                            ClientDetailsID = s.Distribution.ClientDetailsID == null ? 0 : s.Distribution.ClientDetails.ClientDetailsID,
//                            ClientName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.Name,
//                            ClientLoginName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.LoginName,
//                            EmployeeName = s.Distribution.EmployeeID == null ? "" : s.Distribution.Employee.Name,
//                            PopName = s.Distribution.Pop != null ? s.Distribution.Pop.PopName : "",
//                            BoxName = s.Distribution.Box != null ? s.Distribution.Box.BoxName : "",
//                            SectionName = s.StockDetails.Section.SectionName,
//                            ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.StockDetails.SectionID != AppUtils.WorkingSection) ? true : false,
//                            WarrentyProduct = s.StockDetails.WarrentyProduct ? "Yes" : "No",
//                            IsPriorityClient = s.Distribution.ClientDetails.IsPriorityClient,
//                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrderForRunningSection(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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

//        private List<CustomStockListSectionInformation> SortByColumnWithOrderForRunningSection(string order, string orderDir, List<CustomStockListSectionInformation> data)
//        {

//            // Initialization.   
//            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {

//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
//                        break;
//                    case "1":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
//                        break;
//                    case "2":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
//                        break;
//                    case "3":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
//                        break;
//                    case "4":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
//                        break;
//                    case "5":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
//                        break;
//                    case "6":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
//                        break;
//                    case "7":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeName).ToList() : data.OrderBy(p => p.EmployeeName).ToList();
//                        break;
//                    case "8":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PopName).ToList() : data.OrderBy(p => p.PopName).ToList();
//                        break;
//                    case "9":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BoxName).ToList() : data.OrderBy(p => p.BoxName).ToList();
//                        break;
//                    case "10":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
//                        break;
//                    case "11":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
//                        break;
//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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

//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
//        public ActionResult TotalList()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");
//            //List<StockDetails> lstSoStockDetailse = db.StockDetails.ToList();
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockTotalListSectionInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }

//                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert).AsEnumerable() : db.StockDetails.AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
//                                                                                      || p.Serial.Contains(search.ToLower())
//                                                                                      || p.Section.SectionName.Contains(search.ToLower())
//                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))//p.Brand.BrandName.Contains(search.ToLower())
//                                                                   || p.Serial.Contains(search.ToLower())
//                                                                   || p.Section.SectionName.Contains(search.ToLower())
//                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockListSectionInformation()
//                        {
//                            StockDetailsID = s.StockDetailsID,
//                            SectionID = s.SectionID,
//                            ProductStatusID = s.ProductStatusID,
//                            ItemName = s.Stock.Item.ItemName,
//                            BrandName = (s.Brand == null) ? "" : s.Brand.BrandName,
//                            Serial = s.Serial,
//                            SectionName = s.Section.SectionName,
//                            ProductStatusName = s.ProductStatus.ProductStatusName,
//                            //ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
//                            ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status)) ? true : false,
//                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"
//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockTotalListSectionInformation
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
//        public ActionResult CountNumberOfItemById(int StockId)
//        {
//            int NumberOfItems = db.StockDetails.Where(s => s.StockID == StockId).Count();
//            return Json(new { TotalItem = NumberOfItems }, JsonRequestBehavior.AllowGet);
//        }

//        private List<CustomStockListSectionInformation> SortByColumnWithOrder(string order, string orderDir, List<CustomStockListSectionInformation> data)
//        {

//            // Initialization.   
//            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {

//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
//                        break;
//                    case "1":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
//                        break;
//                    case "2":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
//                        break;
//                    case "3":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
//                        break;
//                    case "4":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
//                        break;
//                    case "5":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
//                        break;
//                    case "6":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
//                        break;
//                    case "7":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
//                        break;
//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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


//        private List<CustomStockOverview> SortByColumnWithOrderStockOverView(string order, string orderDir, List<CustomStockOverview> data)
//        {
//            // Initialization.   
//            List<CustomStockOverview> lst = new List<CustomStockOverview>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {
//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
//                        break;

//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
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

//        //[HttpGet]
//        //[UserRIghtCheck(ControllerValue = AppUtils.View_Product_Working_List)]
//        //public ActionResult TotalWorkingList()
//        //{
//        //    //  .Join(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new VM_Transaction_ClientDueBills { Transaction = Transaction, ClientDueBills = ClientDueBills })
//        //    //    .
//        //    ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.Select(s => new { ConnectionTypeID = s.ConnectionTypeID, ConnectionTypeName = s.ConnectionTypeName }), "ConnectionTypeID", "ConnectionTypeName");
//        //    ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
//        //    ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
//        //    ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsInActive || s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
//        //    ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");

//        //    ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//        //    ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");
//        //    //List<Distribution_Transaction> lstDistribution_Transaction = new List<Distribution_Transaction>();
//        //    //lstDistribution_Transaction = db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
//        //    //                                .Join(db.Transaction, Distribution => Distribution.ClientDetails)

//        //    //List<Distribution> lstDistribution = db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();
//        //    //List<int> clientDetails = lstDistribution.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
//        //    //ViewBag.lstTransaction = db.Transaction.Where(s => clientDetails.Contains(s.ClientDetailsID) && (s.PaymentTypeID == AppUtils.PaymentTypeIsConnection)).ToList();

//        //    return View(new List<CustomStockListSectionInformation>());
//        //}

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult CustomStockTotalWorkingSectionInformation()
//        //{
//        //    // Initialization.   
//        //    JsonResult result = new JsonResult();
//        //    try
//        //    {
//        //        // Initialization.   
//        //        int ifSearch = 0;
//        //        int totalRecords = 0;
//        //        int recFilter = 0;
//        //        // Initialization.   

//        //        int zoneFromDDL = 0;
//        //        var StockID = Request.Form.Get("StockID");
//        //        // Initialization.   
//        //        string search = Request.Form.GetValues("search[value]")[0];
//        //        string draw = Request.Form.GetValues("draw")[0];
//        //        string order = Request.Form.GetValues("order[0][column]")[0];
//        //        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//        //        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//        //        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//        //        List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
//        //        int itemIDConvert = 0;
//        //        if (StockID != "")
//        //        {
//        //            itemIDConvert = int.Parse(StockID);
//        //        }

//        //        //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
//        //        var firstPartOfQuery = (StockID != "") ? db.Distribution.Where(s => s.StockDetails.StockID == itemIDConvert && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).AsEnumerable() : db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();

//        //        ClientPopBoxEmpty(ref firstPartOfQuery);

//        //        // Verification.   
//        //        if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//        //        {
//        //            forSearch(ref ifSearch, ref firstPartOfQuery, search);
//        //        }
//        //        var thirdPartOfquery = firstPartOfQuery
//        //            .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
//        //                Distribution => Distribution.ClientDetails.ClientDetailsID,
//        //                Transaction => Transaction.ClientDetailsID,
//        //                (Distribution, Transaction) => new
//        //                {
//        //                    Distribution = Distribution,
//        //                    Transaction = Transaction
//        //                }).AsEnumerable();
//        //        if (firstPartOfQuery.Any())
//        //        {
//        //            // var i = thirdPartOfquery.ToList();
//        //            totalRecords = firstPartOfQuery.Count();
//        //            lstCustomStockTotalListSectionInformation = thirdPartOfquery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//        //                s => new CustomStockListSectionInformation()
//        //                {
//        //                    DistributionID = s.Distribution.DistributionID,
//        //                    TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
//        //                    StockDetailsID = s.Distribution.StockDetailsID,
//        //                    SectionID = s.Distribution.StockDetails.SectionID,
//        //                    ProductStatusID = s.Distribution.StockDetails.ProductStatusID,
//        //                    ItemName = s.Distribution.StockDetails.Stock.Item.ItemName,
//        //                    BrandName = s.Distribution.StockDetails.Brand.BrandName,
//        //                    Serial = s.Distribution.StockDetails.Serial,
//        //                    ClientDetailsID = s.Distribution.ClientDetailsID == null ? 0 : s.Distribution.ClientDetails.ClientDetailsID,
//        //                    ClientName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.Name,
//        //                    EmployeeName = s.Distribution.EmployeeID == null ? "" : s.Distribution.Employee.Name,
//        //                    PopName = s.Distribution.Pop != null ? s.Distribution.Pop.PopName : "",
//        //                    BoxName = s.Distribution.Box != null ? s.Distribution.Box.BoxName : "",
//        //                    SectionName = s.Distribution.StockDetails.Section.SectionName,
//        //                    ProductStatusName = s.Distribution.StockDetails.ProductStatus.ProductStatusName,
//        //                    ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) /*&& s.Distribution.StockDetails.SectionID != AppUtils.WorkingSection*/) ? true : false,

//        //                }).ToList();

//        //        }

//        //        // Sorting.   
//        //        lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrderForTotalWorkingSectionSection(order, orderDir, lstCustomStockTotalListSectionInformation);
//        //        // Total record count.   
//        //        // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//        //        // Filter record count.   
//        //        recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//        //        ////////////////////////////////////


//        //        // Loading drop down lists.   
//        //        result = this.Json(new
//        //        {
//        //            draw = Convert.ToInt32(draw),
//        //            recordsTotal = totalRecords,
//        //            recordsFiltered = recFilter,
//        //            data = lstCustomStockTotalListSectionInformation
//        //        }, JsonRequestBehavior.AllowGet);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // Info   
//        //        Console.Write(ex);
//        //    }
//        //    // Return info.   
//        //    return result;
//        //}

//        private List<CustomStockListSectionInformation> SortByColumnWithOrderForTotalWorkingSectionSection(string order, string orderDir, List<CustomStockListSectionInformation> data)
//        {

//            // Initialization.   
//            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {

//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
//                        break;
//                    case "1":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
//                        break;
//                    case "2":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
//                        break;
//                    case "3":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DistributionID).ToList() : data.OrderBy(p => p.DistributionID).ToList();
//                        break;
//                    case "4":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
//                        break;
//                    case "5":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
//                        break;
//                    case "6":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
//                        break;
//                    case "7":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
//                        break;
//                    case "8":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeName).ToList() : data.OrderBy(p => p.EmployeeName).ToList();
//                        break;

//                    case "9":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
//                        break;
//                    case "10":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
//                        break;
//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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

//        private void ClientPopBoxEmpty(ref IEnumerable<Distribution> firstPartOfQuery)
//        {
//            foreach (var VARIABLE in firstPartOfQuery)
//            {
//                if (VARIABLE.ClientDetails == null)
//                {
//                    VARIABLE.ClientDetails = new ClientDetails() { Name = "" };

//                }
//                if (VARIABLE.Employee == null)
//                {
//                    VARIABLE.Employee = new Employee() { Name = "" };

//                }
//                if (VARIABLE.Pop == null)
//                {
//                    VARIABLE.Pop = new Pop() { PopName = "" };

//                }
//                if (VARIABLE.Box == null)
//                {
//                    VARIABLE.Box = new Box() { BoxName = "" };

//                }
//            }
//        }

//        private void forSearch(ref int ifSearch, ref IEnumerable<Distribution> firstPartOfQuery, string search)
//        {
//            ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

//                                                                              || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
//                                                                              || p.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

//                                                                              || p.StockDetails.Brand.BrandName.Contains(search.ToLower())
//                                                                              || p.StockDetails.Serial.Contains(search.ToLower())
//                                                                              || p.StockDetails.Section.SectionName.Contains(search.ToLower())
//                                                                              || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
//                .Count() : 0;

//            // Apply search   
//            firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

//                                                           || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

//                                                           || p.StockDetails.Brand.BrandName.Contains(search.ToLower())
//                                                           || p.StockDetails.Serial.Contains(search.ToLower())
//                                                           || p.StockDetails.Section.SectionName.Contains(search.ToLower())
//                                                           || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))

//                .AsEnumerable();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForWarrenty(int StockID)
//        {

//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblWarrentyList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
//                .Select(s => new
//                {
//                    StockID = s.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.Stock.Item.ItemName,
//                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                    Serial = s.Serial,
//                    SectionID = s.Section.SectionID,
//                    ProductStatusID = s.ProductStatus.ProductStatusID,
//                    SectionName = s.Section.SectionName,
//                    ProductStatusName = s.ProductStatus.ProductStatusName,
//                    SupplierName = s.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblWarrentyList = tblWarrentyList }, JsonRequestBehavior.AllowGet);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForDead(int StockID)
//        {

//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblDeadList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection)
//                .Select(s => new
//                {
//                    StockID = s.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.Stock.Item.ItemName,
//                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                    Serial = s.Serial,
//                    SectionID = s.Section.SectionID,
//                    ProductStatusID = s.ProductStatus.ProductStatusID,
//                    SectionName = s.Section.SectionName,
//                    ProductStatusName = s.ProductStatus.ProductStatusName,
//                    SupplierName = s.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblDeadList = tblDeadList }, JsonRequestBehavior.AllowGet);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForRunning(int StockID)
//        {

//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblRunningList =
//                db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
//                Distribution => Distribution.StockDetailsID,
//                (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution })
//                .Select(s => new
//                {

//                    Name = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.Name : "",
//                    ClientDetailsID = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetailsID.ToString() : "",
//                    TransactionID = (s.Distribution.ClientDetails != null) ? db.Transaction.Where(ss => ss.ClientDetailsID == s.Distribution.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",
//                    clientLoginName = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.LoginName : "",
//                    employeeName = (s.Distribution.Employee != null) ? s.Distribution.Employee.Name : "",
//                    popName = (s.Distribution.Pop != null) ? s.Distribution.Pop.PopName : "",
//                    boxName = (s.Distribution.Box != null) ? s.Distribution.Box.BoxName : "",

//                    StockID = s.StockDetails.StockID,
//                    StockDetailsID = s.StockDetails.StockDetailsID,
//                    ItemName = s.StockDetails.Stock.Item.ItemName,
//                    BrandName = s.StockDetails.Brand.BrandName,
//                    Serial = s.StockDetails.Serial,
//                    SectionID = s.StockDetails.Section.SectionID,
//                    ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
//                    SectionName = s.StockDetails.Section.SectionName,
//                    ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
//                    SupplierName = s.StockDetails.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblRunningList = tblRunningList }, JsonRequestBehavior.AllowGet);
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult FindItemStatusByClientOrByStockDetailsID(int? StockID, int? StockDetailsID, int? ClientDetailsID)
//        {

//            List<dynamic> lstDynamic = new List<dynamic>();
//            IEnumerable<dynamic> tblRunningList = Enumerable.Empty<dynamic>(); ;
//            if (StockID != null && StockDetailsID != null && ClientDetailsID != null)
//            {
//                tblRunningList = db.StockDetails.Where(s => s.StockID == StockID.Value && s.StockDetailsID == StockDetailsID.Value && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                     .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive && s.ClientDetailsID == ClientDetailsID.Value), StockDetails => StockDetails.StockDetailsID,
//                     Distribution => Distribution.StockDetailsID,
//                     (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
//            }
//            else if (StockID != null && ClientDetailsID != null)
//            {
//                tblRunningList = db.StockDetails.Where(s => s.StockID == StockID.Value && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                     .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
//                     Distribution => Distribution.StockDetailsID,
//                     (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
//            }
//            else if (ClientDetailsID != null)
//            {
//                tblRunningList = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                    .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive && s.ClientDetailsID == ClientDetailsID.Value), StockDetails => StockDetails.StockDetailsID,
//                    Distribution => Distribution.StockDetailsID,
//                    (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
//            }
//            else if (StockID != null)
//            {
//                tblRunningList = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                    .Join(db.Distribution.Where(s => s.StockDetails.StockID == StockID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
//                    Distribution => Distribution.StockDetailsID,
//                    (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
//            }



//            var res = tblRunningList
//            .Select(s => new
//            {
//                clientLoginName = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.LoginName : "",
//                employeeName = (s.Distribution.Employee != null) ? s.Distribution.Employee.Name : "",
//                popName = (s.Distribution.Pop != null) ? s.Distribution.Pop.PopName : "",
//                boxName = (s.Distribution.Box != null) ? s.Distribution.Box.BoxName : "",
//                DistributionID = (s.Distribution != null) ? s.Distribution.DistributionID.ToString() : "",
//                StockID = s.StockDetails.StockID,
//                StockDetailsID = s.StockDetails.StockDetailsID,
//                ItemName = s.StockDetails.Stock.Item.ItemName,
//                BrandName = s.StockDetails.Brand.BrandName,
//                Serial = s.StockDetails.Serial,
//                SectionID = s.StockDetails.Section.SectionID,
//                ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
//                SectionName = s.StockDetails.Section.SectionName,
//                ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
//                SupplierName = s.StockDetails.Supplier.SupplierName,

//            }).ToList();

//            return Json(new { tblRunningList = res }, JsonRequestBehavior.AllowGet);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]//CableTypeID: cableTypeID, CableStockID: cableStockID, ClientDetailsID: clientDetailsID
//        public ActionResult FindCableDetailsByCableBoxOrDrumOrByClientDetailsID(int? CableTypeID, int? CableStockID, int? ClientDetailsID)
//        {

//            List<dynamic> lstDynamic = new List<dynamic>();
//            IEnumerable<dynamic> tblCableDetailsList = Enumerable.Empty<dynamic>(); ;
//            if (CableTypeID != null && CableStockID != null && ClientDetailsID != null)
//            {
//                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.CableStockID == CableStockID.Value && s.ClientDetailsID == ClientDetailsID.Value)
//                      .AsQueryable();
//            }
//            else if (CableTypeID != null && ClientDetailsID != null)
//            {
//                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.ClientDetailsID == ClientDetailsID.Value)
//                   .AsQueryable();
//            }
//            else if (CableTypeID != null && CableStockID != null)
//            {
//                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.CableStockID == CableStockID.Value)
//                   .AsQueryable();
//            }
//            else if (ClientDetailsID != null)
//            {
//                tblCableDetailsList = db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID.Value)
//                   .AsQueryable();
//            }
//            else if (CableTypeID != null)
//            {
//                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value)
//                    .AsQueryable();
//            }


//            var res = tblCableDetailsList.AsEnumerable()
//            .Select(s => new
//            {


//                Name = (s.ClientDetails != null) ? s.ClientDetails.Name : "",
//                ClientDetailsID = (s.ClientDetails != null) ? s.ClientDetailsID.ToString() : "",
//                //  TransactionI = (s.ClientDetails != null) ? db.Transaction.Where(ss => ss.ClientDetailsID == s.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",


//                CableDistributionID = s.CableDistributionID,
//                CableTypeName = s.CableStock.CableType.CableTypeName,
//                CableBoxName = s.CableStock.CableBoxName,
//                AmountOfCableUsed = s.AmountOfCableUsed,
//                Date = (s.UpdateDate != null) ? s.UpdateDate.ToString() : s.CreatedDate.ToString(),
//                LoginName = s.ClientDetails != null ? s.ClientDetails.LoginName : "",
//                AssignedEmployee = s.Employee != null ? s.Employee.Name : "",
//                CableForEmployee = s.CableForEmployeeID,
//                CreatedBy = (s.UpdateDate != null) ? s.UpdateBy.ToString() : s.CreatedBy.ToString(),
//                CableStatus = s.CableIndicatorStatus

//            }).ToList();
//            var returns = res.AsEnumerable().Select(s => new
//            {

//                Name = s.Name,
//                ClientDetailsID = s.ClientDetailsID,
//                TransactionID = (s.ClientDetailsID != "") ? fc(s.ClientDetailsID) : "",


//                CableDistributionID = s.CableDistributionID,
//                CableTypeName = s.CableTypeName,
//                CableBoxName = s.CableBoxName,
//                AmountOfCableUsed = s.AmountOfCableUsed,
//                Date = s.Date,
//                LoginName = s.LoginName,
//                AssignedEmployee = s.AssignedEmployee,
//                CableForEmployee = s.CableForEmployee == null ? "" : GetEmployeeName(s.CableForEmployee),
//                CreatedBy = s.CreatedBy.ToString(),
//                CableStatus = s.CableStatus
//            }).ToList();

//            return Json(new { tblCableAssignedList = returns }, JsonRequestBehavior.AllowGet);
//        }

//        private string GetEmployeeName(dynamic employeeID)
//        {
//            int i = Convert.ChangeType(employeeID, typeof(int));
//            if (db.Employee.Where(s => s.EmployeeID == i).Count() > 0)
//            {
//                return db.Employee.Where(s => s.EmployeeID == i).FirstOrDefault().Name;
//            }
//            return "";
//        }

//        private string fc(dynamic clientDetailsID)
//        {
//            int i = Convert.ChangeType(clientDetailsID, typeof(int));
//            var TransactionID = db.Transaction.Where(ss => ss.ClientDetailsID == i).FirstOrDefault().TransactionID.ToString();
//            return TransactionID;
//        }

//        private int GetClientDetailsIDFromDynamic(dynamic clientDetailsID)
//        {
//            return 1;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForAvailable(int StockID)
//        {
//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblAvailableList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection)
//                .Select(s => new
//                {
//                    StockID = s.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.Stock.Item.ItemName,
//                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                    Serial = s.Serial,
//                    SectionID = s.Section.SectionID,
//                    ProductStatusID = s.ProductStatus.ProductStatusID,
//                    SectionName = s.Section.SectionName,
//                    ProductStatusName = s.ProductStatus.ProductStatusName,
//                    SupplierName = s.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblAvailableList = tblAvailableList }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForRepair(int StockID)
//        {
//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblRepairList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection)
//                .Select(s => new
//                {
//                    StockID = s.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.Stock.Item.ItemName,
//                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                    Serial = s.Serial,
//                    SectionID = s.Section.SectionID,
//                    ProductStatusID = s.ProductStatus.ProductStatusID,
//                    SectionName = s.Section.SectionName,
//                    ProductStatusName = s.ProductStatus.ProductStatusName,
//                    SupplierName = s.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblRepairList = tblRepairList }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchStockDetailsListByCriteriaForTotal(int StockID)
//        {
//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblTotalList = db.StockDetails.Where(s => s.StockID == StockID)
//                .Select(s => new
//                {
//                    StockID = s.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.Stock.Item.ItemName,
//                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
//                    Serial = s.Serial,
//                    SectionID = s.Section.SectionID,
//                    ProductStatusID = s.ProductStatus.ProductStatusID,
//                    SectionName = s.Section.SectionName,
//                    ProductStatusName = s.ProductStatus.ProductStatusName,
//                    SupplierName = s.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblTotalList = tblTotalList }, JsonRequestBehavior.AllowGet);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SearchDistributionListByCriteriaForTotalWorkingSection(int StockID)
//        {
//            List<dynamic> lstDynamic = new List<dynamic>();
//            var tblTotalList = db.Distribution.Where(s => s.StockDetails.StockID == StockID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
//                .Select(s => new
//                {
//                    DistributionID = s.DistributionID,
//                    ClientDetailsID = s.ClientDetails != null ? s.ClientDetailsID.Value.ToString() : "",

//                    Name = s.ClientDetails != null ? s.ClientDetails.Name : "",
//                    TransactionID = s.ClientDetailsID != null ? db.Transaction.Where(ss => ss.ClientDetailsID == s.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",
//                    EmployeeName = s.Employee != null ? db.Employee.Where(ss => ss.EmployeeID == s.EmployeeID).FirstOrDefault().Name.ToString() : "",
//                    StockID = s.StockDetails.StockID,
//                    StockDetailsID = s.StockDetailsID,
//                    ItemName = s.StockDetails.Stock.Item.ItemName,
//                    BrandName = s.StockDetails.Brand.BrandName,
//                    Serial = s.StockDetails.Serial,
//                    SectionID = s.StockDetails.Section.SectionID,
//                    ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
//                    SectionName = s.StockDetails.Section.SectionName,
//                    ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
//                    SupplierName = s.StockDetails.Supplier.SupplierName,

//                }).ToList();

//            return Json(new { tblTotalList = tblTotalList }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult GetProductStatusBySectionID(int SectionID)
//        {
//            var lstSectionFromDB = db.Section.ToList().AsQueryable();
//            List<dynamic> lstDynamic = new List<dynamic>();
//            dynamic dynamic = new ExpandoObject();

//            if (SectionID == AppUtils.StockSection)
//            {
//                dynamic =
//                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList();
//            }

//            if (SectionID == AppUtils.RepairingSection)
//            {
//                dynamic =
//                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRepair).ToList();
//            }
//            if (SectionID == AppUtils.WarrantySection)
//            {
//                dynamic =
//                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsWarrenty).ToList();
//            }
//            if (SectionID == AppUtils.DeadSection)
//            {
//                dynamic =
//                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsDead).ToList();
//            }
//            //var lstProductStatus = db.StockDetailse.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
//            return Json(new { lstProductStatus = dynamic }, JsonRequestBehavior.AllowGet);
//            return Json(true);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult ChangeProductStatusAndSection(int StockDetailsID, int NewSectionID, int NewProductStatusID)
//        {
//            try
//            {
//                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();
//                if (stockDetails != null)
//                {
//                    //if (stockDetails.ProductStatusID == AppUtils.ProductStatusIsRunning && stockDetails.SectionID == AppUtils.WorkingSection)
//                    //{
//                    //    return Json(new { WorkingSectionRunning = true }, JsonRequestBehavior.AllowGet);
//                    //}

//                    if (stockDetails.SectionID == AppUtils.WorkingSection)
//                    {
//                        Distribution distribution =
//                            db.Distribution.Where(s => s.StockDetailsID == StockDetailsID &&
//                                                       s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();
//                        if (distribution != null)
//                        {
//                            //first save data to new table for record purpose
//                            DirectProductSectionChangeFromWorkingToOthers DirectProductSectionChangeFromWorkingToOthers = new DirectProductSectionChangeFromWorkingToOthers();
//                            SaveProductSectionChangeData(ref DirectProductSectionChangeFromWorkingToOthers, distribution, stockDetails, NewSectionID);
//                            //then change recovery table status to delete
//                            Recovery recovery = db.Recovery.Where(s => s.StockDetailsID == StockDetailsID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
//                                .FirstOrDefault();
//                            if (recovery != null)
//                            {
//                                recovery.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                                db.Entry(recovery).State = EntityState.Modified;
//                                db.SaveChanges();
//                            }

//                            //then chagne status of distribution to delete
//                            distribution.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                            distribution.UpdateBy = AppUtils.GetLoginEmployeeName();
//                            distribution.UpdateDate = AppUtils.GetDateTimeNow();
//                            db.Entry(distribution).State = EntityState.Modified;
//                            db.SaveChanges();

//                        }
//                    }


//                    stockDetails.SectionID = NewSectionID;
//                    stockDetails.ProductStatusID = NewProductStatusID;
//                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
//                    db.Entry(stockDetails).State = EntityState.Modified;
//                    db.SaveChanges();


//                }
//                return Json(new { Success = true, StockDetails = stockDetails }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Success = false, StockDetails = "" }, JsonRequestBehavior.AllowGet);
//            }


//        }

//        private void SaveProductSectionChangeData(ref DirectProductSectionChangeFromWorkingToOthers DirectProductSectionChangeFromWorkingToOthers, Distribution distribution, StockDetails stockDetails, int NewSectionID)
//        {
//            DirectProductSectionChangeFromWorkingToOthers.StockDetailsID = distribution.StockDetailsID;
//            DirectProductSectionChangeFromWorkingToOthers.ClientName = distribution.ClientDetails != null ? distribution.ClientDetails.Name : "";
//            DirectProductSectionChangeFromWorkingToOthers.TakenEmployee = distribution.Employee != null ? distribution.Employee.Name : "";
//            DirectProductSectionChangeFromWorkingToOthers.FromSection = stockDetails.SectionID;
//            DirectProductSectionChangeFromWorkingToOthers.ToSection = NewSectionID;
//            DirectProductSectionChangeFromWorkingToOthers.WhoChanged = AppUtils.GetLoginEmployeeName();
//            DirectProductSectionChangeFromWorkingToOthers.ChangeDateTime = AppUtils.GetDateTimeNow();
//            db.DirectProductSectionChangeFromWorkingToOthers.Add(DirectProductSectionChangeFromWorkingToOthers);
//            db.SaveChanges();
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult ChangeProductStatusAndSectionForWorkingList(int StockDetailsID, int DistributionID, int NewSectionID, int NewProductStatusID)
//        {
//            try
//            {
//                Recovery rc = db.Recovery.Where(s => s.DistributionID == DistributionID).FirstOrDefault();
//                if (rc != null)
//                {
//                    rc.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                    db.Entry(rc).State = EntityState.Modified;
//                    db.SaveChanges();
//                }
//                Distribution dt = db.Distribution.Where(s => s.DistributionID == DistributionID).FirstOrDefault();
//                if (dt != null)
//                {
//                    dt.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                    db.Entry(dt).State = EntityState.Modified;
//                    db.SaveChanges();
//                }

//                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();
//                if (stockDetails != null)
//                {
//                    //if (stockDetails.ProductStatusID == AppUtils.ProductStatusIsRunning && stockDetails.SectionID == AppUtils.WorkingSection)
//                    //{
//                    //    return Json(new { WorkingSectionRunning = true }, JsonRequestBehavior.AllowGet);
//                    //}

//                    stockDetails.SectionID = NewSectionID;
//                    stockDetails.ProductStatusID = NewProductStatusID;
//                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
//                    db.Entry(stockDetails).State = EntityState.Modified;
//                    db.SaveChanges();

//                    //Distribution distribution = db.Distribution.Where(s => s.StockDetailsID == stockDetails.StockDetailsID).FirstOrDefault();
//                    //if (distribution != null)
//                    //{
//                    //    Recovery recovery = db.Recovery.Where(s => s.DistributionID == distribution.DistributionID).FirstOrDefault();

//                    //    if (recovery != null)
//                    //    {
//                    //        db.Entry(recovery).State = EntityState.Modified;
//                    //        db.SaveChanges();
//                    //    }

//                    //    db.Entry(distribution).State = EntityState.Modified;
//                    //    db.SaveChanges();
//                    //}

//                }
//                return Json(new { Success = true, StockDetails = stockDetails }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Success = false, StockDetails = "" }, JsonRequestBehavior.AllowGet);
//            }


//        }

//        [HttpGet]
//        public ActionResult FindItemByItemOrClientID()
//        {
//            ViewBag.lstStockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");

//            List<StockDetails> lstStockDetailse =
//             db.StockDetails.Where(
//                 s =>
//                     s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
//                 .ToList();
//            List<int> lstStockDetailsID = (lstStockDetailse != null) ? lstStockDetailse.Select(s => s.StockDetailsID).ToList() : new List<int>();

//            List<Distribution> lstDistributions = db.Distribution.Where(s => lstStockDetailsID.Contains(s.StockDetailsID) && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();

//            VM_lstStockDetails_lstDistribution VM_lstStockDetails_lstDistribution = new VM_lstStockDetails_lstDistribution();
//            VM_lstStockDetails_lstDistribution.lstDistribution = lstDistributions;
//            VM_lstStockDetails_lstDistribution.lstStockDetails = lstStockDetailse;

//            return View(VM_lstStockDetails_lstDistribution);

//        }

//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Cable_Distributed_To_Client_Or_Employee)]
//        public ActionResult FindCableUsedByCableStockIDOrClientID()
//        {
//            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

//            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

//            ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
//            ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
//            ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
//            ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
//            ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");


//            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
//            ViewBag.CableTypePopUpID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
//            //ViewBag.lstCableStockID = new SelectList(db.CableStock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");
//            ViewBag.lstEmployee = db.Employee.ToList();

//            //List<CableDistribution> lstCableDistribution = db.CableDistribution.ToList();

//            //List<int> clientDetailsID = lstCableDistribution.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
//            //if (clientDetailsID.Count > 0)
//            //{
//            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
//            //                  .Select(s => new ClientSetByViewBag
//            //                  {
//            //                      ClientDetailsID = s.ClientDetailsID,
//            //                      TransactionID = s.TransactionID,
//            //                      PaymentAmount = s.PaymentAmount.Value,

//            //                  }).ToList();
//            //}
//            //else
//            //{
//            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
//            //                  .Select(s => new
//            //                  {
//            //                      ClientDetailsID = s.ClientDetailsID,
//            //                      TransactionID = s.TransactionID,
//            //                      PaymentAmount = s.PaymentAmount.Value,

//            //                  }).ToList();
//            //}

//            return View(new List<CableDistribution>());

//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CableUsedByCableStockIDOrClientIDInformation()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int CableTypeIDDDL = 0;
//                int CableStockIDDDL = 0;
//                int ClientDetailsDDL = 0;

//                var CableTypeID = Request.Form.Get("CableTypeID");
//                if (CableTypeID != "")
//                {
//                    CableTypeIDDDL = int.Parse(CableTypeID);
//                }
//                var CableStockID = Request.Form.Get("CableStockID");
//                if (CableStockID != "")
//                {
//                    CableStockIDDDL = int.Parse(CableStockID);
//                }
//                var lstClientDetailsID = Request.Form.Get("ClientDetailsID");
//                if (lstClientDetailsID != "")
//                {
//                    ClientDetailsDDL = int.Parse(lstClientDetailsID);
//                }

//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomCableUsedInformation> lstCustomSCustomCableUsedInformation = new List<CustomCableUsedInformation>();

//                //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
//                var firstPartOfQuery =
//                        (CableTypeID != "" && CableStockID != "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL && s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
//                            : (CableTypeID != "" && CableStockID != "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL).AsEnumerable()
//                             : (CableTypeID == "" && CableStockID != "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStockID == CableStockIDDDL).AsEnumerable()
//                               : (CableTypeID != "" && CableStockID == "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
//                                    : (CableTypeID != "" && CableStockID == "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL).AsEnumerable()
//                                        : (CableTypeID == "" && CableStockID == "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
//                                            :
//                                            db.CableDistribution.AsEnumerable()
//                    ;

//                //var a = firstPartOfQuery.ToList();
//                ClientPopBoxEmpty(ref firstPartOfQuery);

//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {
//                    forSearch(ref ifSearch, ref firstPartOfQuery, search);
//                }

//                var secondPartOfQuery = firstPartOfQuery
//                    .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
//                        CableDistribution => CableDistribution.ClientDetailsID,
//                        Transaction => Transaction.ClientDetailsID, (CableDistribution, Transaction) => new
//                        {
//                            CableDistribution = CableDistribution,
//                            Transaction = Transaction
//                        })
//                          .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), CableDistribution => CableDistribution.CableDistribution.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (CableDistribution, ClientLineStatus) => new
//                          {
//                              CableDistribution = CableDistribution.CableDistribution,
//                              Transaction = CableDistribution.Transaction,
//                              ClientLineStatus = ClientLineStatus.FirstOrDefault(),

//                          }).ToList();

//                if (secondPartOfQuery.Any())
//                {
//                    //var i = secondPartOfQuery.ToList();
//                    totalRecords = secondPartOfQuery.Count();
//                    lstCustomSCustomCableUsedInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomCableUsedInformation()
//                        {
//                            CableDistributionID = s.CableDistribution.CableDistributionID,
//                            ClientDetailsID = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetailsID.Value : 0,
//                            TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
//                            CableTypeName = s.CableDistribution.CableStock.CableType.CableTypeName,
//                            CableBoxName = s.CableDistribution.CableStock.CableBoxName,
//                            AmountOfCableUsed = s.CableDistribution.AmountOfCableUsed.ToString(),
//                            Date = s.CableDistribution.CreatedDate.Value,
//                            ClientName = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetails.Name : "",
//                            ClientLoginName = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetails.LoginName : "",
//                            AssignEmployeeName = s.CableDistribution.CableForEmployeeID != null ? db.Employee.Find(s.CableDistribution.CableForEmployeeID).Name : "",
//                            EmployeeTakenCable = s.CableDistribution.EmployeeID != null ? s.CableDistribution.Employee.Name : "",
//                            cableStatus = ChangeCableStatus(s.CableDistribution.CableIndicatorStatus),
//                            ChangeStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Change_Cable_Status_To_Other_Such_New_Or_Old_Box_Or_Dead) ? true : false,
//                            IsPriorityClient = s.CableDistribution.ClientDetails.IsPriorityClient,
//                            LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomSCustomCableUsedInformation = this.SortByColumnWithOrderForCableUsedByCableStockIDOrClientID(order, orderDir, lstCustomSCustomCableUsedInformation);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomSCustomCableUsedInformation
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

//        private string ChangeCableStatus(int cableIndicatorStatus)
//        {
//            if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsRunning)
//            {
//                return "Running";
//            }
//            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
//            {
//                return "Old Box";
//            }
//            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsStolen)
//            {
//                return "Stolen";
//            }
//            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsNotWorking)
//            {
//                return "Not Working";
//            }
//            else
//            {
//                return "";
//            }
//        }

//        private void forSearch(ref int ifSearch, ref IEnumerable<CableDistribution> firstPartOfQuery, string search)
//        {

//            ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.CableDistributionID.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.CableStock.CableBoxName.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                                                  || ChangeCableStatus(p.CableIndicatorStatus).ToString().ToLower().Contains(search.ToLower())
//                                                                                  ).Count() : 0;

//            // Apply search   
//            firstPartOfQuery = firstPartOfQuery.Where(p => p.CableDistributionID.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.CableStock.CableBoxName.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
//                                                           || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
//                                                           || ChangeCableStatus(p.CableIndicatorStatus).ToString().ToLower().Contains(search.ToLower()))


//                .AsEnumerable();
//        }

//        private object functionstatus(int cableIndicatorStatus)
//        {
//            throw new NotImplementedException();
//        }

//        private void ClientPopBoxEmpty(ref IEnumerable<CableDistribution> firstPartOfQuery)
//        {
//            foreach (var VARIABLE in firstPartOfQuery)
//            {
//                if (VARIABLE.ClientDetails == null)
//                {
//                    VARIABLE.ClientDetails = new ClientDetails() { Name = "" };

//                }
//                if (VARIABLE.Employee == null)
//                {
//                    VARIABLE.Employee = new Employee() { Name = "" };

//                }
//            }
//        }

//        private List<CustomCableUsedInformation> SortByColumnWithOrderForCableUsedByCableStockIDOrClientID(string order, string orderDir, List<CustomCableUsedInformation> data)
//        {

//            // Initialization.   
//            List<CustomCableUsedInformation> lst = new List<CustomCableUsedInformation>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {
//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableDistributionID).ToList() : data.OrderBy(p => p.CableDistributionID).ToList();
//                        break;
//                    case "1":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeName).ToList() : data.OrderBy(p => p.CableTypeName).ToList();
//                        break;
//                    case "2":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableBoxName).ToList() : data.OrderBy(p => p.CableBoxName).ToList();
//                        break;
//                    case "3":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AmountOfCableUsed).ToList() : data.OrderBy(p => p.AmountOfCableUsed).ToList();
//                        break;
//                    case "4":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
//                        break;
//                    case "5":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
//                        break;
//                    case "6":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignEmployeeName).ToList() : data.OrderBy(p => p.AssignEmployeeName).ToList();
//                        break;
//                    case "7":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeTakenCable).ToList() : data.OrderBy(p => p.EmployeeTakenCable).ToList();
//                        break;
//                    case "8":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.cableStatus).ToList() : data.OrderBy(p => p.cableStatus).ToList();
//                        break;


//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableDistributionID).ToList() : data.OrderBy(p => p.CableDistributionID).ToList();
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
//        [ValidateAntiForgeryToken]
//        public ActionResult GetStockDetailsItemListByStockID(int StockID)
//        {
//            var lstStockDetails = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRunning).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
//            return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsID(int StockDetailsID, int DistributionID)
//        {
//            try
//            {
//                if (DistributionID != null && DistributionID > 0)
//                {
//                    Recovery recovery =
//                        db.Recovery.Where(s => s.DistributionID == DistributionID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();

//                    if (recovery != null)
//                    {
//                        recovery.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                    }

//                    Distribution distribution =
//                        db.Distribution.Where(s => s.DistributionID == DistributionID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();

//                    if (distribution != null)
//                    {
//                        distribution.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
//                    }
//                    StockDetails stockDetails =
//                       db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();

//                    if (stockDetails != null)
//                    {
//                        stockDetails.SectionID = AppUtils.StockSection;
//                        stockDetails.ProductStatusID = AppUtils.ProductStatusIsAvialable;
//                    }
//                    db.SaveChanges();

//                    return Json(new { SuccessDeleteDistribution = true, StockDetailsID = StockDetailsID }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { SuccessDeleteDistribution = false, StockDetailsID = "" }, JsonRequestBehavior.AllowGet);
//            }

//            return Json(new { SuccessDeleteDistribution = false, StockDetailsID = "" }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult ChangeCableSection(int CableDistributionID, int NewCableStatus)
//        {
//            CableDistribution cableDistribution =
//                db.CableDistribution.Where(s => s.CableDistributionID == CableDistributionID).FirstOrDefault();
//            if (cableDistribution != null)
//            {
//                if (NewCableStatus == cableDistribution.CableIndicatorStatus && NewCableStatus != AppUtils.SelectedWhereToPassIsMainBox)
//                //we dont need to check for when we will pass to main box cause after change the section below it will delete the distribution details
//                //but for other it will keep the information only it will change the status.so think now we have cable in old box section. but again we are change 
//                //this cable to old section then what will be happen. it will automatically add quantity +. so we have have to check here giben type is same or not.
//                {
//                    return Json(new { NewCableTypeSameAsOldType = true }, JsonRequestBehavior.AllowGet);
//                }
//                if (NewCableStatus == AppUtils.SelectedWhereToPassIsMainBox)
//                {
//                    CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cableDistribution.CableStockID).FirstOrDefault();
//                    if (cableStock != null)
//                    {
//                        cableStock.UsedQuantityFromThisBox -= cableDistribution.AmountOfCableUsed;
//                        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
//                        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
//                        db.SaveChanges();
//                    }
//                    db.Entry(cableDistribution).State = EntityState.Deleted;
//                    db.SaveChanges();
//                    return Json(new { Success = true, DeleteStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus }, JsonRequestBehavior.AllowGet);
//                }
//                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsOCBox)
//                {
//                    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
//                    if (cableStock != null)
//                    {
//                        cableStock.CableQuantity += cableDistribution.AmountOfCableUsed;
//                        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
//                        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
//                        db.Entry(cableStock).State = EntityState.Modified;
//                        db.SaveChanges();
//                    }

//                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsOldBox;
//                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
//                    db.Entry(cableDistribution).State = EntityState.Modified;
//                    db.SaveChanges();

//                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
//                }
//                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsStolen)
//                {
//                    //if (cableDistribution.CableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
//                    //{
//                    //    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
//                    //    if (cableStock != null)
//                    //    {
//                    //        cableStock.CableQuantity -= cableDistribution.AmountOfCableUsed;
//                    //        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    //        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
//                    //        db.Entry(cableStock).State = EntityState.Modified;
//                    //        db.SaveChanges();
//                    //    }
//                    //}
//                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsStolen;
//                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
//                    db.Entry(cableDistribution).State = EntityState.Modified;
//                    db.SaveChanges();
//                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
//                }
//                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsNotWorking)
//                {
//                    //if (cableDistribution.CableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
//                    //{
//                    //    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
//                    //    if (cableStock != null)
//                    //    {
//                    //        cableStock.CableQuantity -= cableDistribution.AmountOfCableUsed;
//                    //        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    //        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
//                    //        db.Entry(cableStock).State = EntityState.Modified;
//                    //        db.SaveChanges();
//                    //    }
//                    //}
//                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsNotWorking;
//                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
//                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
//                    db.Entry(cableDistribution).State = EntityState.Modified;
//                    db.SaveChanges();
//                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
//                }
//            }
//            else
//            {
//                return Json(new { Success = false, ChangeStatus = "", CableDistributionID = "", NewCableStatus = "" },
//                    JsonRequestBehavior.AllowGet);
//            }
//            return Json(new { Success = false, ChangeStatus = "", CableDistributionID = "", NewCableStatus = "" },
//                    JsonRequestBehavior.AllowGet);
//        }



//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
//        public ActionResult TotalItemListOverview()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockTotalListTotalItemListOverview()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockOverview> lstCustomStockOverview = new List<CustomStockOverview>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }
//                List<int> lstWarrentyProductStockID = db.StockDetails.Where(s => s.WarrentyProduct == true).Select(s => s.StockID).ToList();

//                var firstPartOfQuery = (StockID != "") ? db.Stock.Where(s => s.StockID == itemIDConvert && lstWarrentyProductStockID.Contains(s.StockID)).AsQueryable() : db.Stock.Where(s => lstWarrentyProductStockID.Contains(s.StockID)).AsQueryable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      )
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where((p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())))
//                        .AsQueryable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockOverview()
//                        {
//                            StockID = s.StockID,
//                            ItemName = s.Item.ItemName,
//                            TotalItemCount = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.WarrentyProduct == true).Count(),
//                            ProductInStock = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.StockSection && ss.WarrentyProduct == true).Count(),
//                            ProductInRunning = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WorkingSection && ss.WarrentyProduct == true).Count(),
//                            ProductInDead = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.DeadSection && ss.WarrentyProduct == true).Count(),
//                            ProductInRepair = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.RepairingSection && ss.WarrentyProduct == true).Count(),
//                            ProductInWarrenty = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WarrantySection && ss.WarrentyProduct == true).Count(),

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockOverview = this.SortByColumnWithOrderStockOverView(order, orderDir, lstCustomStockOverview);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockOverview
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


//        [HttpGet]
//        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
//        public ActionResult TotalNonWarrentyProductListOverview()
//        {
//            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
//            return View(new List<CustomStockListSectionInformation>());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomStockTotalListTotalNonWarrentyProductListOverview()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int zoneFromDDL = 0;
//                var StockID = Request.Form.Get("StockID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomStockOverview> lstCustomStockOverview = new List<CustomStockOverview>();
//                int itemIDConvert = 0;
//                if (StockID != "")
//                {
//                    itemIDConvert = int.Parse(StockID);
//                }
//                List<int> lstNonWarrentyProductStockID = db.StockDetails.Where(s => s.WarrentyProduct == false).Select(s => s.StockID).ToList();
//                var firstPartOfQuery = (StockID != "") ? db.Stock.Where(s => s.StockID == itemIDConvert && lstNonWarrentyProductStockID.Contains(s.StockID)).AsEnumerable() : db.Stock.Where(s => lstNonWarrentyProductStockID.Contains(s.StockID)).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
//                                                                                      || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
//                                                                                      )
//                        .Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where((p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
//                                                                    || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())))
//                        .AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomStockOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomStockOverview()
//                        {
//                            StockID = s.StockID,
//                            ItemName = s.Item.ItemName,
//                            TotalItemCount = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.WarrentyProduct == false).Count(),
//                            ProductInStock = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.StockSection && ss.WarrentyProduct == false).Count(),
//                            ProductInRunning = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WorkingSection && ss.WarrentyProduct == false).Count(),
//                            ProductInDead = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.DeadSection && ss.WarrentyProduct == false).Count(),
//                            ProductInRepair = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.RepairingSection && ss.WarrentyProduct == false).Count(),
//                            ProductInWarrenty = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WarrantySection && ss.WarrentyProduct == false).Count(),

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomStockOverview = this.SortByColumnWithOrderStockOverView(order, orderDir, lstCustomStockOverview);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomStockOverview
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



//        public ActionResult CableOverView()
//        {
//            ViewBag.CableTypeID = new SelectList(db.CableType.Select(s => new { CableTypeID = s.CableTypeID, CableTypeName = s.CableTypeName }), "CableTypeID", "CableTypeName");
//            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

//            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

//            ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
//            ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
//            ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
//            ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
//            ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");


//            //ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
//            ViewBag.CableTypePopUpID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
//            //ViewBag.lstCableStockID = new SelectList(db.CableStock.ToList(), "StockID", "Item.ItemName");
//            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult CustomCableListOverview()
//        {
//            // Initialization.   
//            JsonResult result = new JsonResult();
//            try
//            {
//                // Initialization.   
//                int ifSearch = 0;
//                int totalRecords = 0;
//                int recFilter = 0;
//                // Initialization.   

//                int CableTypeIDFromDDL = 0;
//                var CableTypeID = Request.Form.Get("CableTypeID");
//                // Initialization.   
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



//                List<CustomCableTypeOverview> lstCustomCableTypeOverview = new List<CustomCableTypeOverview>();
//                int CableTypeIDConvert = 0;
//                if (CableTypeID != "")
//                {
//                    CableTypeIDConvert = int.Parse(CableTypeID);
//                }

//                var firstPartOfQuery = (CableTypeID != "") ? db.CableType.Where(s => s.CableTypeID == CableTypeIDConvert).OrderBy(s => s.CableTypeName).AsEnumerable() : db.CableType.OrderBy(s => s.CableTypeName).AsEnumerable();


//                // Verification.   
//                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
//                {

//                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.CableTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

//                    // Apply search   
//                    firstPartOfQuery = firstPartOfQuery.Where(p => p.CableTypeName.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
//                }
//                if (firstPartOfQuery.Any())
//                {
//                    totalRecords = firstPartOfQuery.Count();
//                    lstCustomCableTypeOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
//                        s => new CustomCableTypeOverview()
//                        {
//                            CableTypeID = s.CableTypeID,
//                            CableTypeName = s.CableTypeName,
//                            TotalCableTypeCount = db.CableStock.Where(ss => ss.CableTypeID == s.CableTypeID).Count()

//                        }).ToList();

//                }

//                // Sorting.   
//                lstCustomCableTypeOverview = this.SortByColumnWithOrderCableOverView(order, orderDir, lstCustomCableTypeOverview);
//                // Total record count.   
//                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
//                // Filter record count.   
//                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : totalRecords;

//                ////////////////////////////////////


//                // Loading drop down lists.   
//                result = this.Json(new
//                {
//                    draw = Convert.ToInt32(draw),
//                    recordsTotal = totalRecords,
//                    recordsFiltered = recFilter,
//                    data = lstCustomCableTypeOverview
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



//        private List<CustomCableTypeOverview> SortByColumnWithOrderCableOverView(string order, string orderDir, List<CustomCableTypeOverview> data)
//        {
//            // Initialization.   
//            List<CustomCableTypeOverview> lst = new List<CustomCableTypeOverview>();
//            try
//            {
//                // Sorting   
//                switch (order)
//                {
//                    case "0":
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeName).ToList() : data.OrderBy(p => p.CableTypeName).ToList();
//                        break;

//                    default:
//                        // Setting.   
//                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeID).ToList() : data.OrderBy(p => p.CableTypeID).ToList();
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


//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult GetAssetDetailsByAssetTypeID(int AssetTypeID)
//        //{

//        //    try
//        //    {
//        //        List<AssetCustomList> lstAssetCustomList = db.Asset.Where(s => s.AssetTypeID == AssetTypeID).Select(fp => new AssetCustomList
//        //        {
//        //            AssetID = fp.AssetID,
//        //            AssetTypeName = fp.AssetType.AssetTypeName,
//        //            AssetName = fp.AssetName,
//        //            AssetValue = fp.AssetValue,
//        //            PurchaseDate = fp.PurchaseDate,
//        //            SerialNumber = fp.SerialNumber,
//        //            WarrentyStartDate = fp.WarrentyStartDate,
//        //            WarrentyEndDate = fp.WarrentyEndDate,

//        //        }).ToList();
//        //        return Json(new { Success = true, lstAssetByAssetTypeID = lstAssetCustomList }, JsonRequestBehavior.AllowGet);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
//        //    }


//        //}
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult GetCableDetailsByCableTypeID(int CableTypeID)
//        {


//            try
//            {
//                List<CableCustomList> lstCableCustomList = db.CableStock.Where(s => s.CableTypeID == CableTypeID).Select(s => new CableCustomList
//                {
//                    CableStockID = s.CableStockID,
//                    CableTypeName = s.CableType.CableTypeName,
//                    BoxDrumName = s.CableBoxName,
//                    BrandName = s.Brand != null ? s.Brand.BrandName : "",
//                    SupplierName = s.Supplier != null ? s.Supplier.SupplierName : "",
//                    Invoice = s.SupplierInvoice == null ? "" : s.SupplierInvoice,
//                    ReadingFrom = s.FromReading,
//                    ReadingEnd = s.ToReading,
//                    Quantity = s.CableQuantity,
//                    Used = s.UsedQuantityFromThisBox,
//                    Remain = s.CableQuantity - s.UsedQuantityFromThisBox

//                }).ToList();
//                return Json(new { Success = true, lstCableByCableTypeID = lstCableCustomList }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
//            }


//        }

//    }
//}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ProductCurrentStatusController : Controller
    {
        public ProductCurrentStatusController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [HttpGet]
        //[UserRIghtCheck(ControllerValue = AppUtils.ProductCurrentStatus_Ca)]
        public ActionResult CablesHistory()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.WarrantySection).ToList(), "SectionID", "SectionName");
            List<StockDetails> lstSoStockDetailse =
                db.StockDetails.Where(
                    s =>
                        s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
                    .ToList();
            return View(lstSoStockDetailse);
        }


        // GET: StockDetails
        [HttpGet]

        // [UserRIghtCheck(ControllerValue = AppUtils.Warranty)]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Warrenty_List)]
        public ActionResult WarrentyList()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.WarrantySection).ToList(), "SectionID", "SectionName");
            //List<StockDetails> lstSoStockDetailse =
            //    db.StockDetails.Where(
            //        s =>
            //            s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
            //        .ToList();
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockWarrentyListSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                      || p.Serial.Contains(search.ToLower())
                                                                                      || p.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                   || p.Serial.Contains(search.ToLower())
                                                                   || p.Section.SectionName.Contains(search.ToLower())
                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            StockDetailsID = s.StockDetailsID,
                            SectionID = s.SectionID,
                            ProductStatusID = s.ProductStatusID,
                            ItemName = s.Stock.Item.ItemName,
                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                            Serial = s.Serial,
                            SectionName = s.Section.SectionName,
                            ProductStatusName = s.ProductStatus.ProductStatusName,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Dead_List)]
        public ActionResult DeadList()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.DeadSection).ToList(), "SectionID", "SectionName");
            //List<StockDetails> lstSoStockDetailse =
            //    db.StockDetails.Where(
            //        s =>
            //            s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection)
            //        .ToList();
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockDeadListSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                      || p.Serial.Contains(search.ToLower())
                                                                                      || p.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                   || p.Serial.Contains(search.ToLower())
                                                                   || p.Section.SectionName.Contains(search.ToLower())
                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            StockDetailsID = s.StockDetailsID,
                            SectionID = s.SectionID,
                            ProductStatusID = s.ProductStatusID,
                            ItemName = s.Stock.Item.ItemName,
                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                            Serial = s.Serial,
                            SectionName = s.Section.SectionName,
                            ProductStatusName = s.ProductStatus.ProductStatusName,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"
                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_avialable_List)]
        public ActionResult AvialableList()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.StockSection).ToList(), "SectionID", "SectionName");
            //List<StockDetails> lstSoStockDetailse =
            //    db.StockDetails.Where(
            //        s =>
            //            s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection)
            //        .ToList();
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockAvailableListSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Brand.BrandName.Contains(search.ToLower())
                                                                                      || p.Serial.Contains(search.ToLower())
                                                                                      || p.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.Brand.BrandName.Contains(search.ToLower())
                                                                   || p.Serial.Contains(search.ToLower())
                                                                   || p.Section.SectionName.Contains(search.ToLower())
                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            StockDetailsID = s.StockDetailsID,
                            SectionID = s.SectionID,
                            ProductStatusID = s.ProductStatusID,
                            ItemName = s.Stock.Item.ItemName,
                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                            Serial = s.Serial,
                            SectionName = s.Section.SectionName,
                            ProductStatusName = s.ProductStatus.ProductStatusName,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_repairing_List)]
        public ActionResult RepairingList()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection && s.SectionID != AppUtils.RepairingSection).ToList(), "SectionID", "SectionName");
            //List<StockDetails> lstSoStockDetailse =
            //    db.StockDetails.Where(
            //        s =>
            //            s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection)
            //        .ToList();
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockRepairingListSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                                      || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                      || p.Serial.Contains(search.ToLower())
                                                                                      || p.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                   //|| p.Brand.BrandName.Contains(search.ToLower())
                                                                   || ((p.Brand != null) ? p.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                   || p.Serial.Contains(search.ToLower())
                                                                   || p.Section.SectionName.Contains(search.ToLower())
                                                                   || p.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockTotalListSectionInformation = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            StockDetailsID = s.StockDetailsID,
                            SectionID = s.SectionID,
                            ProductStatusID = s.ProductStatusID,
                            ItemName = s.Stock.Item.ItemName,
                            BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                            Serial = s.Serial,
                            SectionName = s.Section.SectionName,
                            ProductStatusName = s.ProductStatus.ProductStatusName,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
                            WarrentyProduct = s.WarrentyProduct ? "Yes" : "No"

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_running_List)]
        public ActionResult RunningList()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
             


            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
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



            ViewBag.StockID = new SelectList(db.Stock.Select(s => new { StockID = s.StockID, ItemName = s.Item.ItemName }), "StockID", "ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");


            //List<StockDetails> lstStockDetailse =
            //    db.StockDetails.Where(
            //        s =>
            //            s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
            //        .ToList();
            //List<int> lstStockDetailsID = (lstStockDetailse != null) ? lstStockDetailse.Select(s => s.StockDetailsID).ToList() : new List<int>();

            //List<Distribution> lstDistributions = db.Distribution.Where(s => lstStockDetailsID.Contains(s.StockDetailsID)).ToList();


            //List<int> clientDetailsID = lstDistributions.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
            //if (clientDetailsID.Count > 0)
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
            //                  .Select(s => new ClientSetByViewBag
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}
            //else
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //                  .Select(s => new
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}



            //VM_lstStockDetails_lstDistribution VM_lstStockDetails_lstDistribution = new VM_lstStockDetails_lstDistribution();
            //VM_lstStockDetails_lstDistribution.lstDistribution = lstDistributions;
            //VM_lstStockDetails_lstDistribution.lstStockDetails = lstStockDetailse;

            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockRunningSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection).AsEnumerable() : db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection).AsEnumerable();
                //var a = firstPartOfQuery.ToList();
                var secondPartOfQuery = firstPartOfQuery.GroupJoin(db.Distribution, StockDetails => StockDetails.StockDetailsID, Distribution => Distribution.StockDetailsID, (StockDetails, Distribution) => new
                {
                    StockDetails = StockDetails,
                    Distribution = (Distribution.Count() == 0) ? new Distribution() : Distribution.FirstOrDefault()
                }).AsEnumerable();
                //var aa = secondPartOfQuery.ToList();
                foreach (var VARIABLE in secondPartOfQuery)
                {
                    if (VARIABLE.Distribution != null)
                    {
                        if (VARIABLE.Distribution.ClientDetails == null)
                        {
                            VARIABLE.Distribution.ClientDetails = new ClientDetails() { Name = "" };

                        }
                        if (VARIABLE.Distribution.Employee == null)
                        {
                            VARIABLE.Distribution.Employee = new Employee() { Name = "" };

                        }
                        if (VARIABLE.Distribution.Pop == null)
                        {
                            VARIABLE.Distribution.Pop = new Pop() { PopName = "" };

                        }
                        if (VARIABLE.Distribution.Box == null)
                        {
                            VARIABLE.Distribution.Box = new Box() { BoxName = "" };

                        }
                    }
                }

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    //var a = secondPartOfQuery.ToList();
                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

                                                                                        || p.Distribution.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Distribution.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Distribution.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
                                                                                        || p.Distribution.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

                                                                                        //|| p.StockDetails.Brand.BrandName.Contains(search.ToLower())
                                                                                        || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                        || p.StockDetails.Serial.ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

                                                                    || p.Distribution.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.Distribution.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.Distribution.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.Distribution.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

                                                                    //|| p.StockDetails.Brand.BrandName.Contains(search.ToLower())
                                                                    || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                    || p.StockDetails.Serial.ToLower().Contains(search.ToLower())
                                                                    || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                    || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))

                        .AsEnumerable();
                }
                //var test = secondPartOfQuery.ToList();
                var thirdPartOfquery = secondPartOfQuery
                    .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
                        Distribution => Distribution.Distribution.ClientDetails.ClientDetailsID,
                        Transaction => Transaction.ClientDetailsID,
                        (Distribution, Transaction) => new
                        {
                            StockDetails = Distribution.StockDetails,
                            Distribution = Distribution.Distribution,
                            Transaction = Transaction
                        })
                          .GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), StockDetails => StockDetails.Distribution.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (StockDetails, ClientLineStatus) => new
                          {
                              StockDetails = StockDetails.Distribution.StockDetails,
                              Distribution = StockDetails.Distribution,
                              Transaction = StockDetails.Transaction,
                              ClientLineStatus = ClientLineStatus.FirstOrDefault(),

                          }).AsEnumerable();

                //var aaa = thirdPartOfquery.ToList();
                //     var a = thirdPartOfquery.ToList();
                if (thirdPartOfquery.Count() > 0)
                {
                    //var i = thirdPartOfquery.ToList();
                    totalRecords = secondPartOfQuery.Count();
                    lstCustomStockTotalListSectionInformation = thirdPartOfquery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
                            StockDetailsID = s.StockDetails.StockDetailsID,
                            SectionID = s.StockDetails.SectionID,
                            ProductStatusID = s.StockDetails.ProductStatusID,
                            ItemName = s.StockDetails.Stock.Item.ItemName,
                            BrandName = s.StockDetails.Brand != null ? s.StockDetails.Brand.BrandName : "",
                            Serial = s.StockDetails.Serial,
                            ClientDetailsID = s.Distribution.ClientDetailsID == null ? 0 : s.Distribution.ClientDetails.ClientDetailsID,
                            ClientName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.Name,
                            ClientLoginName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.LoginName,
                            EmployeeName = s.Distribution.EmployeeID == null ? "" : s.Distribution.Employee.Name,
                            PopName = s.Distribution.Pop != null ? s.Distribution.Pop.PopName : "",
                            BoxName = s.Distribution.Box != null ? s.Distribution.Box.BoxName : "",
                            SectionName = s.StockDetails.Section.SectionName,
                            ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status) && s.StockDetails.SectionID != AppUtils.WorkingSection) ? true : false,
                            WarrentyProduct = s.StockDetails.WarrentyProduct ? "Yes" : "No",
                            IsPriorityClient = s.Distribution.ClientDetails.IsPriorityClient,
                            LineStatusActiveDate = s.ClientLineStatus != null ?
                                                    s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : ""
                            : "",
                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrderForRunningSection(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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

        private List<CustomStockListSectionInformation> SortByColumnWithOrderForRunningSection(string order, string orderDir, List<CustomStockListSectionInformation> data)
        {

            // Initialization.   
            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeName).ToList() : data.OrderBy(p => p.EmployeeName).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PopName).ToList() : data.OrderBy(p => p.PopName).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BoxName).ToList() : data.OrderBy(p => p.BoxName).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
                        break;
                    case "11":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
        public ActionResult TotalList()
        {


            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
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


            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");



            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");
            //List<StockDetails> lstSoStockDetailse = db.StockDetails.ToList();
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockTotalListSectionInformation()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }

                var firstPartOfQuery = (StockID != "") ? db.StockDetails.Where(s => s.StockID == itemIDConvert).AsEnumerable() : db.StockDetails.AsEnumerable();

                var secondPart = firstPartOfQuery
                .GroupJoin(db.Distribution, StockDetails => StockDetails.StockDetailsID, Distribution => Distribution.StockDetailsID, (StockDetails, Distribution) => new
                {
                    StockDetails = StockDetails,
                    Distribution = (Distribution.Count() == 0) ? new Distribution() : Distribution.FirstOrDefault()
                }).AsQueryable();
                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPart.Any()) ? secondPart.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))
                                                                                      || p.StockDetails.Serial.Contains(search.ToLower())
                                                                                      || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                                      || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .Count() : 0;

                    // Apply search   
                    secondPart = secondPart.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                   || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                   || ((p.StockDetails.Brand != null) ? p.StockDetails.Brand.BrandName.Contains(search.ToLower()) : "".Contains(search.ToLower()))//p.Brand.BrandName.Contains(search.ToLower())
                                                                   || p.StockDetails.Serial.Contains(search.ToLower())
                                                                   || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                   || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                        .AsQueryable();
                }
                if (secondPart.Any())
                {
                    totalRecords = secondPart.Count();
                    lstCustomStockTotalListSectionInformation = secondPart.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListSectionInformation()
                        {
                            StockDetailsID = s.StockDetails.StockDetailsID,
                            SectionID = s.StockDetails.SectionID,
                            ProductStatusID = s.StockDetails.ProductStatusID,
                            ItemName = s.StockDetails.Stock.Item.ItemName,
                            BrandName = (s.StockDetails.Brand == null) ? "" : s.StockDetails.Brand.BrandName,
                            Serial = s.StockDetails.Serial,
                            SectionName = s.StockDetails.Section.SectionName,
                            ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                            //ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,
                            ChangeSectionPermission = (AppUtils.HasAccessInTheList(AppUtils.Change_Product_Status)) ? true : false,
                            WarrentyProduct = s.StockDetails.WarrentyProduct ? "Yes" : "No",
                            EmployeeName = s.Distribution.Employee != null ? s.Distribution.Employee.Name : "",
                            ClientDetailsID = s.Distribution.ClientDetails != null ? s.Distribution.ClientDetails.ClientDetailsID : 0,
                            ClientLoginName = s.Distribution.ClientDetails != null ? s.Distribution.ClientDetails.LoginName : "",
                            Remarks = s.Distribution.Remarks
                        }).ToList();

                }

                // Sorting.   
                lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockTotalListSectionInformation);
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
                    data = lstCustomStockTotalListSectionInformation
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
        public ActionResult CountNumberOfItemById(int StockId)
        {
            int NumberOfItems = db.StockDetails.Where(s => s.StockID == StockId).Count();
            return Json(new { TotalItem = NumberOfItems }, JsonRequestBehavior.AllowGet);
        }

        private List<CustomStockListSectionInformation> SortByColumnWithOrder(string order, string orderDir, List<CustomStockListSectionInformation> data)
        {

            // Initialization.   
            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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


        private List<CustomStockOverview> SortByColumnWithOrderStockOverView(string order, string orderDir, List<CustomStockOverview> data)
        {
            // Initialization.   
            List<CustomStockOverview> lst = new List<CustomStockOverview>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
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

        //[HttpGet]
        //[UserRIghtCheck(ControllerValue = AppUtils.View_Product_Working_List)]
        //public ActionResult TotalWorkingList()
        //{
        //    //  .Join(db.ClientDueBills, Transaction => Transaction.ClientDetailsID, ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new VM_Transaction_ClientDueBills { Transaction = Transaction, ClientDueBills = ClientDueBills })
        //    //    .
        //    ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.Select(s => new { ConnectionTypeID = s.ConnectionTypeID, ConnectionTypeName = s.ConnectionTypeName }), "ConnectionTypeID", "ConnectionTypeName");
        //    ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
        //    ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
        //    ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsInActive || s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
        //    ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");

        //    ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
        //    ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");
        //    //List<Distribution_Transaction> lstDistribution_Transaction = new List<Distribution_Transaction>();
        //    //lstDistribution_Transaction = db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
        //    //                                .Join(db.Transaction, Distribution => Distribution.ClientDetails)

        //    //List<Distribution> lstDistribution = db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();
        //    //List<int> clientDetails = lstDistribution.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
        //    //ViewBag.lstTransaction = db.Transaction.Where(s => clientDetails.Contains(s.ClientDetailsID) && (s.PaymentTypeID == AppUtils.PaymentTypeIsConnection)).ToList();

        //    return View(new List<CustomStockListSectionInformation>());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CustomStockTotalWorkingSectionInformation()
        //{
        //    // Initialization.   
        //    JsonResult result = new JsonResult();
        //    try
        //    {
        //        // Initialization.   
        //        int ifSearch = 0;
        //        int totalRecords = 0;
        //        int recFilter = 0;
        //        // Initialization.   

        //        int zoneFromDDL = 0;
        //        var StockID = Request.Form.Get("StockID");
        //        // Initialization.   
        //        string search = Request.Form.GetValues("search[value]")[0];
        //        string draw = Request.Form.GetValues("draw")[0];
        //        string order = Request.Form.GetValues("order[0][column]")[0];
        //        string orderDir = Request.Form.GetValues("order[0][dir]")[0];
        //        int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
        //        int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



        //        List<CustomStockListSectionInformation> lstCustomStockTotalListSectionInformation = new List<CustomStockListSectionInformation>();
        //        int itemIDConvert = 0;
        //        if (StockID != "")
        //        {
        //            itemIDConvert = int.Parse(StockID);
        //        }

        //        //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
        //        var firstPartOfQuery = (StockID != "") ? db.Distribution.Where(s => s.StockDetails.StockID == itemIDConvert && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).AsEnumerable() : db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();

        //        ClientPopBoxEmpty(ref firstPartOfQuery);

        //        // Verification.   
        //        if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
        //        {
        //            forSearch(ref ifSearch, ref firstPartOfQuery, search);
        //        }
        //        var thirdPartOfquery = firstPartOfQuery
        //            .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
        //                Distribution => Distribution.ClientDetails.ClientDetailsID,
        //                Transaction => Transaction.ClientDetailsID,
        //                (Distribution, Transaction) => new
        //                {
        //                    Distribution = Distribution,
        //                    Transaction = Transaction
        //                }).AsEnumerable();
        //        if (firstPartOfQuery.Any())
        //        {
        //            // var i = thirdPartOfquery.ToList();
        //            totalRecords = firstPartOfQuery.Count();
        //            lstCustomStockTotalListSectionInformation = thirdPartOfquery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
        //                s => new CustomStockListSectionInformation()
        //                {
        //                    DistributionID = s.Distribution.DistributionID,
        //                    TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
        //                    StockDetailsID = s.Distribution.StockDetailsID,
        //                    SectionID = s.Distribution.StockDetails.SectionID,
        //                    ProductStatusID = s.Distribution.StockDetails.ProductStatusID,
        //                    ItemName = s.Distribution.StockDetails.Stock.Item.ItemName,
        //                    BrandName = s.Distribution.StockDetails.Brand.BrandName,
        //                    Serial = s.Distribution.StockDetails.Serial,
        //                    ClientDetailsID = s.Distribution.ClientDetailsID == null ? 0 : s.Distribution.ClientDetails.ClientDetailsID,
        //                    ClientName = s.Distribution.ClientDetailsID == null ? "" : s.Distribution.ClientDetails.Name,
        //                    EmployeeName = s.Distribution.EmployeeID == null ? "" : s.Distribution.Employee.Name,
        //                    PopName = s.Distribution.Pop != null ? s.Distribution.Pop.PopName : "",
        //                    BoxName = s.Distribution.Box != null ? s.Distribution.Box.BoxName : "",
        //                    SectionName = s.Distribution.StockDetails.Section.SectionName,
        //                    ProductStatusName = s.Distribution.StockDetails.ProductStatus.ProductStatusName,
        //                    ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) /*&& s.Distribution.StockDetails.SectionID != AppUtils.WorkingSection*/) ? true : false,

        //                }).ToList();

        //        }

        //        // Sorting.   
        //        lstCustomStockTotalListSectionInformation = this.SortByColumnWithOrderForTotalWorkingSectionSection(order, orderDir, lstCustomStockTotalListSectionInformation);
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
        //            data = lstCustomStockTotalListSectionInformation
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

        private List<CustomStockListSectionInformation> SortByColumnWithOrderForTotalWorkingSectionSection(string order, string orderDir, List<CustomStockListSectionInformation> data)
        {

            // Initialization.   
            List<CustomStockListSectionInformation> lst = new List<CustomStockListSectionInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionID).ToList() : data.OrderBy(p => p.SectionID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusID).ToList() : data.OrderBy(p => p.ProductStatusID).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DistributionID).ToList() : data.OrderBy(p => p.DistributionID).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeName).ToList() : data.OrderBy(p => p.EmployeeName).ToList();
                        break;

                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
                        break;
                    case "10":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
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

        private void ClientPopBoxEmpty(ref IEnumerable<Distribution> firstPartOfQuery)
        {
            foreach (var VARIABLE in firstPartOfQuery)
            {
                if (VARIABLE.ClientDetails == null)
                {
                    VARIABLE.ClientDetails = new ClientDetails() { Name = "" };

                }
                if (VARIABLE.Employee == null)
                {
                    VARIABLE.Employee = new Employee() { Name = "" };

                }
                if (VARIABLE.Pop == null)
                {
                    VARIABLE.Pop = new Pop() { PopName = "" };

                }
                if (VARIABLE.Box == null)
                {
                    VARIABLE.Box = new Box() { BoxName = "" };

                }
            }
        }

        private void forSearch(ref int ifSearch, ref IEnumerable<Distribution> firstPartOfQuery, string search)
        {
            ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

                                                                              || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
                                                                              || p.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

                                                                              || p.StockDetails.Brand.BrandName.Contains(search.ToLower())
                                                                              || p.StockDetails.Serial.Contains(search.ToLower())
                                                                              || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                              || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))
                .Count() : 0;

            // Apply search   
            firstPartOfQuery = firstPartOfQuery.Where(p => p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                           || p.StockDetails.SectionID.ToString().ToLower().Contains(search.ToLower())
                                                           || p.StockDetails.ProductStatusID.ToString().ToLower().Contains(search.ToLower())
                                                           || p.StockDetails.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())

                                                           || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                           || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                           || p.Pop.PopName.ToString().ToLower().Contains(search.ToLower())
                                                           || p.Box.BoxName.ToString().ToLower().Contains(search.ToLower())

                                                           || p.StockDetails.Brand.BrandName.Contains(search.ToLower())
                                                           || p.StockDetails.Serial.Contains(search.ToLower())
                                                           || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                           || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower()))

                .AsEnumerable();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForWarrenty(int StockID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            var tblWarrentyList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
                .Select(s => new
                {
                    StockID = s.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionID = s.Section.SectionID,
                    ProductStatusID = s.ProductStatus.ProductStatusID,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName,
                    SupplierName = s.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblWarrentyList = tblWarrentyList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForDead(int StockID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            var tblDeadList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection)
                .Select(s => new
                {
                    StockID = s.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionID = s.Section.SectionID,
                    ProductStatusID = s.ProductStatus.ProductStatusID,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName,
                    SupplierName = s.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblDeadList = tblDeadList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForRunning(int StockID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            var tblRunningList =
                db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
                Distribution => Distribution.StockDetailsID,
                (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution })
                .Select(s => new
                {

                    Name = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.Name : "",
                    ClientDetailsID = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetailsID.ToString() : "",
                    TransactionID = (s.Distribution.ClientDetails != null) ? db.Transaction.Where(ss => ss.ClientDetailsID == s.Distribution.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",
                    clientLoginName = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.LoginName : "",
                    employeeName = (s.Distribution.Employee != null) ? s.Distribution.Employee.Name : "",
                    popName = (s.Distribution.Pop != null) ? s.Distribution.Pop.PopName : "",
                    boxName = (s.Distribution.Box != null) ? s.Distribution.Box.BoxName : "",

                    StockID = s.StockDetails.StockID,
                    StockDetailsID = s.StockDetails.StockDetailsID,
                    ItemName = s.StockDetails.Stock.Item.ItemName,
                    BrandName = s.StockDetails.Brand.BrandName,
                    Serial = s.StockDetails.Serial,
                    SectionID = s.StockDetails.Section.SectionID,
                    ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
                    SectionName = s.StockDetails.Section.SectionName,
                    ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                    SupplierName = s.StockDetails.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblRunningList = tblRunningList }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindItemStatusByClientOrByStockDetailsID(int? StockID, int? StockDetailsID, int? ClientDetailsID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            IEnumerable<dynamic> tblRunningList = Enumerable.Empty<dynamic>(); ;
            if (StockID != null && StockDetailsID != null && ClientDetailsID != null)
            {
                tblRunningList = db.StockDetails.Where(s => s.StockID == StockID.Value && s.StockDetailsID == StockDetailsID.Value && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                     .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive && s.ClientDetailsID == ClientDetailsID.Value), StockDetails => StockDetails.StockDetailsID,
                     Distribution => Distribution.StockDetailsID,
                     (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
            }
            else if (StockID != null && ClientDetailsID != null)
            {
                tblRunningList = db.StockDetails.Where(s => s.StockID == StockID.Value && s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                     .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
                     Distribution => Distribution.StockDetailsID,
                     (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
            }
            else if (ClientDetailsID != null)
            {
                tblRunningList = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                    .Join(db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive && s.ClientDetailsID == ClientDetailsID.Value), StockDetails => StockDetails.StockDetailsID,
                    Distribution => Distribution.StockDetailsID,
                    (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
            }
            else if (StockID != null)
            {
                tblRunningList = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                    .Join(db.Distribution.Where(s => s.StockDetails.StockID == StockID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive), StockDetails => StockDetails.StockDetailsID,
                    Distribution => Distribution.StockDetailsID,
                    (StockDetails, Distribution) => new { StockDetails = StockDetails, Distribution = Distribution }).AsQueryable();
            }



            var res = tblRunningList
            .Select(s => new
            {
                clientLoginName = (s.Distribution.ClientDetails != null) ? s.Distribution.ClientDetails.LoginName : "",
                employeeName = (s.Distribution.Employee != null) ? s.Distribution.Employee.Name : "",
                popName = (s.Distribution.Pop != null) ? s.Distribution.Pop.PopName : "",
                boxName = (s.Distribution.Box != null) ? s.Distribution.Box.BoxName : "",
                DistributionID = (s.Distribution != null) ? s.Distribution.DistributionID.ToString() : "",
                StockID = s.StockDetails.StockID,
                StockDetailsID = s.StockDetails.StockDetailsID,
                ItemName = s.StockDetails.Stock.Item.ItemName,
                BrandName = s.StockDetails.Brand.BrandName,
                Serial = s.StockDetails.Serial,
                SectionID = s.StockDetails.Section.SectionID,
                ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
                SectionName = s.StockDetails.Section.SectionName,
                ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                SupplierName = s.StockDetails.Supplier.SupplierName,

            }).ToList();

            return Json(new { tblRunningList = res }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//CableTypeID: cableTypeID, CableStockID: cableStockID, ClientDetailsID: clientDetailsID
        public ActionResult FindCableDetailsByCableBoxOrDrumOrByClientDetailsID(int? CableTypeID, int? CableStockID, int? ClientDetailsID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            IEnumerable<dynamic> tblCableDetailsList = Enumerable.Empty<dynamic>(); ;
            if (CableTypeID != null && CableStockID != null && ClientDetailsID != null)
            {
                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.CableStockID == CableStockID.Value && s.ClientDetailsID == ClientDetailsID.Value)
                      .AsQueryable();
            }
            else if (CableTypeID != null && ClientDetailsID != null)
            {
                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.ClientDetailsID == ClientDetailsID.Value)
                   .AsQueryable();
            }
            else if (CableTypeID != null && CableStockID != null)
            {
                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value && s.CableStockID == CableStockID.Value)
                   .AsQueryable();
            }
            else if (ClientDetailsID != null)
            {
                tblCableDetailsList = db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID.Value)
                   .AsQueryable();
            }
            else if (CableTypeID != null)
            {
                tblCableDetailsList = db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeID.Value)
                    .AsQueryable();
            }


            var res = tblCableDetailsList.AsEnumerable()
            .Select(s => new
            {


                Name = (s.ClientDetails != null) ? s.ClientDetails.Name : "",
                ClientDetailsID = (s.ClientDetails != null) ? s.ClientDetailsID.ToString() : "",
                //  TransactionI = (s.ClientDetails != null) ? db.Transaction.Where(ss => ss.ClientDetailsID == s.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",


                CableDistributionID = s.CableDistributionID,
                CableTypeName = s.CableStock.CableType.CableTypeName,
                CableBoxName = s.CableStock.CableBoxName,
                AmountOfCableUsed = s.AmountOfCableUsed,
                Date = (s.UpdateDate != null) ? s.UpdateDate.ToString() : s.CreatedDate.ToString(),
                LoginName = s.ClientDetails != null ? s.ClientDetails.LoginName : "",
                AssignedEmployee = s.Employee != null ? s.Employee.Name : "",
                CableForEmployee = s.CableForEmployeeID,
                CreatedBy = (s.UpdateDate != null) ? s.UpdateBy.ToString() : s.CreatedBy.ToString(),
                CableStatus = s.CableIndicatorStatus

            }).ToList();
            var returns = res.AsEnumerable().Select(s => new
            {

                Name = s.Name,
                ClientDetailsID = s.ClientDetailsID,
                TransactionID = (s.ClientDetailsID != "") ? fc(s.ClientDetailsID) : "",


                CableDistributionID = s.CableDistributionID,
                CableTypeName = s.CableTypeName,
                CableBoxName = s.CableBoxName,
                AmountOfCableUsed = s.AmountOfCableUsed,
                Date = s.Date,
                LoginName = s.LoginName,
                AssignedEmployee = s.AssignedEmployee,
                CableForEmployee = s.CableForEmployee == null ? "" : GetEmployeeName(s.CableForEmployee),
                CreatedBy = s.CreatedBy.ToString(),
                CableStatus = s.CableStatus
            }).ToList();

            return Json(new { tblCableAssignedList = returns }, JsonRequestBehavior.AllowGet);
        }

        private string GetEmployeeName(dynamic employeeID)
        {
            int i = Convert.ChangeType(employeeID, typeof(int));
            if (db.Employee.Where(s => s.EmployeeID == i).Count() > 0)
            {
                return db.Employee.Where(s => s.EmployeeID == i).FirstOrDefault().Name;
            }
            return "";
        }

        private string fc(dynamic clientDetailsID)
        {
            int i = Convert.ChangeType(clientDetailsID, typeof(int));
            var TransactionID = db.Transaction.Where(ss => ss.ClientDetailsID == i).FirstOrDefault().TransactionID.ToString();
            return TransactionID;
        }

        private int GetClientDetailsIDFromDynamic(dynamic clientDetailsID)
        {
            return 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForAvailable(int StockID)
        {
            List<dynamic> lstDynamic = new List<dynamic>();
            var tblAvailableList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection)
                .Select(s => new
                {
                    StockID = s.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionID = s.Section.SectionID,
                    ProductStatusID = s.ProductStatus.ProductStatusID,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName,
                    SupplierName = s.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblAvailableList = tblAvailableList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForRepair(int StockID)
        {
            List<dynamic> lstDynamic = new List<dynamic>();
            var tblRepairList = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection)
                .Select(s => new
                {
                    StockID = s.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionID = s.Section.SectionID,
                    ProductStatusID = s.ProductStatus.ProductStatusID,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName,
                    SupplierName = s.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblRepairList = tblRepairList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockDetailsListByCriteriaForTotal(int StockID)
        {
            List<dynamic> lstDynamic = new List<dynamic>();
            var tblTotalList = db.StockDetails.Where(s => s.StockID == StockID)
                .Select(s => new
                {
                    StockID = s.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand == null ? "" : s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionID = s.Section.SectionID,
                    ProductStatusID = s.ProductStatus.ProductStatusID,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName,
                    SupplierName = s.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblTotalList = tblTotalList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchDistributionListByCriteriaForTotalWorkingSection(int StockID)
        {
            List<dynamic> lstDynamic = new List<dynamic>();
            var tblTotalList = db.Distribution.Where(s => s.StockDetails.StockID == StockID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
                .Select(s => new
                {
                    DistributionID = s.DistributionID,
                    ClientDetailsID = s.ClientDetails != null ? s.ClientDetailsID.Value.ToString() : "",

                    Name = s.ClientDetails != null ? s.ClientDetails.Name : "",
                    TransactionID = s.ClientDetailsID != null ? db.Transaction.Where(ss => ss.ClientDetailsID == s.ClientDetailsID).FirstOrDefault().TransactionID.ToString() : "",
                    EmployeeName = s.Employee != null ? db.Employee.Where(ss => ss.EmployeeID == s.EmployeeID).FirstOrDefault().Name.ToString() : "",
                    StockID = s.StockDetails.StockID,
                    StockDetailsID = s.StockDetailsID,
                    ItemName = s.StockDetails.Stock.Item.ItemName,
                    BrandName = s.StockDetails.Brand.BrandName,
                    Serial = s.StockDetails.Serial,
                    SectionID = s.StockDetails.Section.SectionID,
                    ProductStatusID = s.StockDetails.ProductStatus.ProductStatusID,
                    SectionName = s.StockDetails.Section.SectionName,
                    ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                    SupplierName = s.StockDetails.Supplier.SupplierName,

                }).ToList();

            return Json(new { tblTotalList = tblTotalList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProductStatusBySectionID(int SectionID)
        {
            var lstSectionFromDB = db.Section.ToList().AsQueryable();
            List<dynamic> lstDynamic = new List<dynamic>();
            dynamic dynamic = new ExpandoObject();

            if (SectionID == AppUtils.StockSection)
            {
                dynamic =
                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList();
            }

            if (SectionID == AppUtils.RepairingSection)
            {
                dynamic =
                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRepair).ToList();
            }
            if (SectionID == AppUtils.WarrantySection)
            {
                dynamic =
                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsWarrenty).ToList();
            }
            if (SectionID == AppUtils.DeadSection)
            {
                dynamic =
                    db.ProductStatus.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsDead).ToList();
            }
            //var lstProductStatus = db.StockDetailse.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
            return Json(new { lstProductStatus = dynamic }, JsonRequestBehavior.AllowGet);
            return Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProductStatusAndSection(int StockDetailsID, int NewSectionID, int NewProductStatusID)
        {
            try
            {
                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();
                if (stockDetails != null)
                {
                    //if (stockDetails.ProductStatusID == AppUtils.ProductStatusIsRunning && stockDetails.SectionID == AppUtils.WorkingSection)
                    //{
                    //    return Json(new { WorkingSectionRunning = true }, JsonRequestBehavior.AllowGet);
                    //}

                    if (stockDetails.SectionID == AppUtils.WorkingSection)
                    {
                        Distribution distribution =
                            db.Distribution.Where(s => s.StockDetailsID == StockDetailsID &&
                                                       s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();
                        if (distribution != null)
                        {
                            //first save data to new table for record purpose
                            DirectProductSectionChangeFromWorkingToOthers DirectProductSectionChangeFromWorkingToOthers = new DirectProductSectionChangeFromWorkingToOthers();
                            SaveProductSectionChangeData(ref DirectProductSectionChangeFromWorkingToOthers, distribution, stockDetails, NewSectionID);
                            //then change recovery table status to delete
                            Recovery recovery = db.Recovery.Where(s => s.StockDetailsID == StockDetailsID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
                                .FirstOrDefault();
                            if (recovery != null)
                            {
                                recovery.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                                db.Entry(recovery).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            //then chagne status of distribution to delete
                            distribution.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                            distribution.UpdateBy = AppUtils.GetLoginEmployeeName();
                            distribution.UpdateDate = AppUtils.GetDateTimeNow();
                            db.Entry(distribution).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }


                    stockDetails.SectionID = NewSectionID;
                    stockDetails.ProductStatusID = NewProductStatusID;
                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(stockDetails).State = EntityState.Modified;
                    db.SaveChanges();


                }
                return Json(new { Success = true, StockDetails = stockDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, StockDetails = "" }, JsonRequestBehavior.AllowGet);
            }


        }

        private void SaveProductSectionChangeData(ref DirectProductSectionChangeFromWorkingToOthers DirectProductSectionChangeFromWorkingToOthers, Distribution distribution, StockDetails stockDetails, int NewSectionID)
        {
            DirectProductSectionChangeFromWorkingToOthers.StockDetailsID = distribution.StockDetailsID;
            DirectProductSectionChangeFromWorkingToOthers.ClientName = distribution.ClientDetails != null ? distribution.ClientDetails.Name : "";
            DirectProductSectionChangeFromWorkingToOthers.TakenEmployee = distribution.Employee != null ? distribution.Employee.Name : "";
            DirectProductSectionChangeFromWorkingToOthers.FromSection = stockDetails.SectionID;
            DirectProductSectionChangeFromWorkingToOthers.ToSection = NewSectionID;
            DirectProductSectionChangeFromWorkingToOthers.WhoChanged = AppUtils.GetLoginEmployeeName();
            DirectProductSectionChangeFromWorkingToOthers.ChangeDateTime = AppUtils.GetDateTimeNow();
            db.DirectProductSectionChangeFromWorkingToOthers.Add(DirectProductSectionChangeFromWorkingToOthers);
            db.SaveChanges();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProductStatusAndSectionForWorkingList(int StockDetailsID, int DistributionID, int NewSectionID, int NewProductStatusID)
        {
            try
            {
                Recovery rc = db.Recovery.Where(s => s.DistributionID == DistributionID).FirstOrDefault();
                if (rc != null)
                {
                    rc.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                    db.Entry(rc).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Distribution dt = db.Distribution.Where(s => s.DistributionID == DistributionID).FirstOrDefault();
                if (dt != null)
                {
                    dt.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                    db.Entry(dt).State = EntityState.Modified;
                    db.SaveChanges();
                }

                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();
                if (stockDetails != null)
                {
                    //if (stockDetails.ProductStatusID == AppUtils.ProductStatusIsRunning && stockDetails.SectionID == AppUtils.WorkingSection)
                    //{
                    //    return Json(new { WorkingSectionRunning = true }, JsonRequestBehavior.AllowGet);
                    //}

                    stockDetails.SectionID = NewSectionID;
                    stockDetails.ProductStatusID = NewProductStatusID;
                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(stockDetails).State = EntityState.Modified;
                    db.SaveChanges();

                    //Distribution distribution = db.Distribution.Where(s => s.StockDetailsID == stockDetails.StockDetailsID).FirstOrDefault();
                    //if (distribution != null)
                    //{
                    //    Recovery recovery = db.Recovery.Where(s => s.DistributionID == distribution.DistributionID).FirstOrDefault();

                    //    if (recovery != null)
                    //    {
                    //        db.Entry(recovery).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }

                    //    db.Entry(distribution).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}

                }
                return Json(new { Success = true, StockDetails = stockDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, StockDetails = "" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public ActionResult FindItemByItemOrClientID()
        {
            ViewBag.lstStockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");

            List<StockDetails> lstStockDetailse =
             db.StockDetails.Where(
                 s =>
                     s.ProductStatusID == AppUtils.ProductStatusIsRunning && s.SectionID == AppUtils.WorkingSection)
                 .ToList();
            List<int> lstStockDetailsID = (lstStockDetailse != null) ? lstStockDetailse.Select(s => s.StockDetailsID).ToList() : new List<int>();

            List<Distribution> lstDistributions = db.Distribution.Where(s => lstStockDetailsID.Contains(s.StockDetailsID) && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).ToList();

            VM_lstStockDetails_lstDistribution VM_lstStockDetails_lstDistribution = new VM_lstStockDetails_lstDistribution();
            VM_lstStockDetails_lstDistribution.lstDistribution = lstDistributions;
            VM_lstStockDetails_lstDistribution.lstStockDetails = lstStockDetailse;

            return View(VM_lstStockDetails_lstDistribution);

        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Cable_Distributed_To_Client_Or_Employee)]
        public ActionResult FindCableUsedByCableStockIDOrClientID()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
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


            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            ViewBag.CableTypePopUpID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            //ViewBag.lstCableStockID = new SelectList(db.CableStock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");
            ViewBag.lstEmployee = db.Employee.ToList();

            //List<CableDistribution> lstCableDistribution = db.CableDistribution.ToList();

            //List<int> clientDetailsID = lstCableDistribution.Where(s => s.ClientDetails != null).Select(s => s.ClientDetailsID.Value).Distinct().ToList();
            //if (clientDetailsID.Count > 0)
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && clientDetailsID.Contains(s.ClientDetailsID))
            //                  .Select(s => new ClientSetByViewBag
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}
            //else
            //{
            //    ViewData["lstTransaction"] = db.Transaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
            //                  .Select(s => new
            //                  {
            //                      ClientDetailsID = s.ClientDetailsID,
            //                      TransactionID = s.TransactionID,
            //                      PaymentAmount = s.PaymentAmount.Value,

            //                  }).ToList();
            //}

            return View(new List<CableDistribution>());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CableUsedByCableStockIDOrClientIDInformation()
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

                int CableTypeIDDDL = 0;
                int CableStockIDDDL = 0;
                int ClientDetailsDDL = 0;
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();

                var CableTypeID = Request.Form.Get("CableTypeID");
                if (CableTypeID != "")
                {
                    CableTypeIDDDL = int.Parse(CableTypeID);
                }
                var CableStockID = Request.Form.Get("CableStockID");
                if (CableStockID != "")
                {
                    CableStockIDDDL = int.Parse(CableStockID);
                }
                var lstClientDetailsID = Request.Form.Get("ClientDetailsID");
                if (lstClientDetailsID != "")
                {
                    ClientDetailsDDL = int.Parse(lstClientDetailsID);
                }

                var FromDate = Request.Form.Get("FromDate");
                if (FromDate != "")
                {
                    fromDate = Convert.ToDateTime(FromDate);
                }
                var ToDate = Request.Form.Get("ToDate");
                if (ToDate != "")
                {
                    toDate = Convert.ToDateTime(ToDate);
                }

                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomCableUsedInformation> lstCustomSCustomCableUsedInformation = new List<CustomCableUsedInformation>();

                //  IEnumerable<StockDetails> firstPartOfQuery = Enumerable.Empty<StockDetails>();
                var firstPartOfQuery =
                        (CableTypeID != "" && CableStockID != "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL && s.ClientDetailsID == ClientDetailsDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                            : (CableTypeID != "" && CableStockID != "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                             : (CableTypeID == "" && CableStockID != "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStockID == CableStockIDDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                               : (CableTypeID != "" && CableStockID == "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.ClientDetailsID == ClientDetailsDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                                    : (CableTypeID != "" && CableStockID == "" && lstClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                                        : (CableTypeID == "" && CableStockID == "" && lstClientDetailsID != "") ? db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsDDL).OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                                            :
                                            db.CableDistribution.OrderBy(x => x.CableStock.CableTypeID).ThenBy(x => x.CableStock.CableBoxName).AsEnumerable()
                    ;
                if (FromDate != "" && ToDate != "")
                {
                    firstPartOfQuery = firstPartOfQuery.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= AppUtils.GetLastDayWithHrMinSecMsByMyDate(toDate)).AsEnumerable();
                }

                //var a = firstPartOfQuery.ToList();
                ClientPopBoxEmpty(ref firstPartOfQuery);

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    forSearch(ref ifSearch, ref firstPartOfQuery, search);
                }

                var secondPartOfQuery = firstPartOfQuery
                    .GroupJoin(db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsConnection),
                        CableDistribution => CableDistribution.ClientDetailsID,
                        Transaction => Transaction.ClientDetailsID, (CableDistribution, Transaction) => new
                        {
                            CableDistribution = CableDistribution,
                            Transaction = Transaction
                        })
                          //.GroupJoin(db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()), CableDistribution => CableDistribution.CableDistribution.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (CableDistribution, ClientLineStatus) => new
                          //{
                          //    CableDistribution = CableDistribution.CableDistribution,
                          //    Transaction = CableDistribution.Transaction,
                          //    ClientLineStatus = ClientLineStatus.FirstOrDefault(),

                          //})
                          .AsEnumerable();

                //var b = firstPartOfQuery.ToList();
                if (secondPartOfQuery.Any())
                {
                    //var i = secondPartOfQuery.ToList();
                    totalRecords = secondPartOfQuery.Count();
                    var cableDistributionAfterSkipAndTakeInGroup = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).AsEnumerable().GroupBy(x => new { x.CableDistribution.CableStock.CableTypeID, x.CableDistribution.CableStock.CableBoxName });
                    //lstCustomSCustomCableUsedInformation = 
                    double length = 0;
                    double lengthUsed = 0;
                    string cableBoxName = "";
                    foreach (var Group in cableDistributionAfterSkipAndTakeInGroup)
                    {
                        cableBoxName = Group.FirstOrDefault().CableDistribution.CableStock.CableBoxName;
                        length = Group.FirstOrDefault().CableDistribution.CableStock.CableQuantity;
                        //lengthUsed = length;
                        foreach (var OneByOneFromGroup in Group)
                        {
                            CustomCableUsedInformation CustomCableUsedInformation = new CustomCableUsedInformation();
                            CustomCableUsedInformation.CableDistributionID = OneByOneFromGroup.CableDistribution.CableDistributionID;
                            CustomCableUsedInformation.ClientDetailsID = OneByOneFromGroup.CableDistribution.ClientDetailsID != null ? OneByOneFromGroup.CableDistribution.ClientDetailsID.Value : 0;
                            CustomCableUsedInformation.TransactionID = OneByOneFromGroup.Transaction.Any() ? OneByOneFromGroup.Transaction.FirstOrDefault().TransactionID : 0;
                            CustomCableUsedInformation.CableTypeName = OneByOneFromGroup.CableDistribution.CableStock.CableType.CableTypeName;
                            CustomCableUsedInformation.CableBoxName = OneByOneFromGroup.CableDistribution.CableStock.CableBoxName;
                            CustomCableUsedInformation.AmountOfCableUsed = OneByOneFromGroup.CableDistribution.AmountOfCableUsed.ToString();
                            CustomCableUsedInformation.Date = OneByOneFromGroup.CableDistribution.CreatedDate.Value;
                            CustomCableUsedInformation.ClientName = OneByOneFromGroup.CableDistribution.ClientDetailsID != null ? OneByOneFromGroup.CableDistribution.ClientDetails.Name : "";
                            CustomCableUsedInformation.ClientLoginName = OneByOneFromGroup.CableDistribution.ClientDetailsID != null ? OneByOneFromGroup.CableDistribution.ClientDetails.LoginName : "";
                            CustomCableUsedInformation.AssignEmployeeName = OneByOneFromGroup.CableDistribution.CableForEmployeeID != null ? db.Employee.Find(OneByOneFromGroup.CableDistribution.CableForEmployeeID).Name
                                               : OneByOneFromGroup.CableDistribution.EmployeeID != null ? OneByOneFromGroup.CableDistribution.Employee.Name
                                               : "";

                            //EmployeeTakenCable = OneByOneFromGroup.CableDistribution.EmployeeID != null ? OneByOneFromGroup.CableDistribution.Employee.Name : "";
                            CustomCableUsedInformation.cableStatus = ChangeCableStatus(OneByOneFromGroup.CableDistribution.CableIndicatorStatus);
                            CustomCableUsedInformation.ChangeStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Change_Cable_Status_To_Other_Such_New_Or_Old_Box_Or_Dead) ? true : false;
                            CustomCableUsedInformation.IsPriorityClient = OneByOneFromGroup.CableDistribution.ClientDetails.IsPriorityClient;
                            //LineStatusActiveDate = OneByOneFromGroup.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? OneByOneFromGroup.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(OneByOneFromGroup.ClientLineStatus.LineStatusID) : "";
                            //LineStatusActiveDate = OneByOneFromGroup.ClientLineStatus != null ?
                            //                        OneByOneFromGroup.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? OneByOneFromGroup.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(OneByOneFromGroup.ClientLineStatus.LineStatusID) : ""
                            //: "";
                            CustomCableUsedInformation.Remarks = OneByOneFromGroup.CableDistribution.Remarks;
                            CustomCableUsedInformation.CableFinishMinusView = length.ToString() + " - " + OneByOneFromGroup.CableDistribution.AmountOfCableUsed.ToString() + " = " + (length - OneByOneFromGroup.CableDistribution.AmountOfCableUsed);
                            length = length - OneByOneFromGroup.CableDistribution.AmountOfCableUsed;
                            lstCustomSCustomCableUsedInformation.Add(CustomCableUsedInformation);
                        }
                    }
                    //.Select(
                    //    s => new CustomCableUsedInformation()
                    //    {
                    //        CableDistributionID = s.CableDistribution.CableDistributionID,
                    //        ClientDetailsID = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetailsID.Value : 0,
                    //        TransactionID = s.Transaction.Any() ? s.Transaction.FirstOrDefault().TransactionID : 0,
                    //        CableTypeName = s.CableDistribution.CableStock.CableType.CableTypeName,
                    //        CableBoxName = s.CableDistribution.CableStock.CableBoxName,
                    //        AmountOfCableUsed = s.CableDistribution.AmountOfCableUsed.ToString(),
                    //        Date = s.CableDistribution.CreatedDate.Value,
                    //        ClientName = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetails.Name : "",
                    //        ClientLoginName = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetails.LoginName : "",
                    //        AssignEmployeeName = s.CableDistribution.CableForEmployeeID != null ? db.Employee.Find(s.CableDistribution.CableForEmployeeID).Name
                    //                           : s.CableDistribution.EmployeeID != null ? s.CableDistribution.Employee.Name
                    //                           : "",

                    //        //EmployeeTakenCable = s.CableDistribution.EmployeeID != null ? s.CableDistribution.Employee.Name : "",
                    //        cableStatus = ChangeCableStatus(s.CableDistribution.CableIndicatorStatus),
                    //        ChangeStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Change_Cable_Status_To_Other_Such_New_Or_Old_Box_Or_Dead) ? true : false,
                    //        IsPriorityClient = s.CableDistribution.ClientDetails.IsPriorityClient,
                    //        //LineStatusActiveDate = s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : "",
                    //        //LineStatusActiveDate = s.ClientLineStatus != null ?
                    //        //                        s.ClientLineStatus.LineStatusWillActiveInThisDate.HasValue ? s.ClientLineStatus.LineStatusWillActiveInThisDate.Value.Date.ToString("MM/dd/yyyy") + " " + AppUtils.GetStatusDivByStatusID(s.ClientLineStatus.LineStatusID) : ""
                    //        //: "",
                    //        Remarks = s.CableDistribution.Remarks
                    //    }).ToList();

                }

                // Sorting.   
                lstCustomSCustomCableUsedInformation = this.SortByColumnWithOrderForCableUsedByCableStockIDOrClientID(order, orderDir, lstCustomSCustomCableUsedInformation);
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
                    data = lstCustomSCustomCableUsedInformation
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

        private string ChangeCableStatus(int cableIndicatorStatus)
        {
            if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsRunning)
            {
                return "Running";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
            {
                return "Old Box";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsStolen)
            {
                return "Stolen";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsNotWorking)
            {
                return "Not Working";
            }
            else
            {
                return "";
            }
        }

        private void forSearch(ref int ifSearch, ref IEnumerable<CableDistribution> firstPartOfQuery, string search)
        {

            ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.CableDistributionID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.CableStock.CableBoxName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                                                  || ChangeCableStatus(p.CableIndicatorStatus).ToString().ToLower().Contains(search.ToLower())
                                                                                  ).Count() : 0;

            // Apply search   
            firstPartOfQuery = firstPartOfQuery.Where(p => p.CableDistributionID.ToString().ToLower().Contains(search.ToLower())
                                                           || p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                           || p.CableStock.CableBoxName.ToString().ToLower().Contains(search.ToLower())
                                                           || p.ClientDetails.Name.ToString().ToLower().Contains(search.ToLower())
                                                           || p.Employee.Name.ToString().ToLower().Contains(search.ToLower())
                                                           || ChangeCableStatus(p.CableIndicatorStatus).ToString().ToLower().Contains(search.ToLower()))


                .AsEnumerable();
        }

        private object functionstatus(int cableIndicatorStatus)
        {
            throw new NotImplementedException();
        }

        private void ClientPopBoxEmpty(ref IEnumerable<CableDistribution> firstPartOfQuery)
        {
            foreach (var VARIABLE in firstPartOfQuery)
            {
                if (VARIABLE.ClientDetails == null)
                {
                    VARIABLE.ClientDetails = new ClientDetails() { Name = "" };

                }
                if (VARIABLE.Employee == null)
                {
                    VARIABLE.Employee = new Employee() { Name = "" };

                }
            }
        }

        private List<CustomCableUsedInformation> SortByColumnWithOrderForCableUsedByCableStockIDOrClientID(string order, string orderDir, List<CustomCableUsedInformation> data)
        {

            // Initialization.   
            List<CustomCableUsedInformation> lst = new List<CustomCableUsedInformation>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeName).ThenBy(x => x.CableBoxName).ToList() : data.OrderBy(p => p.CableTypeName).ThenBy(x => x.CableBoxName).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeName).ToList() : data.OrderBy(p => p.CableTypeName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableBoxName).ToList() : data.OrderBy(p => p.CableBoxName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AmountOfCableUsed).ToList() : data.OrderBy(p => p.AmountOfCableUsed).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignEmployeeName).ToList() : data.OrderBy(p => p.AssignEmployeeName).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmployeeTakenCable).ToList() : data.OrderBy(p => p.EmployeeTakenCable).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.cableStatus).ToList() : data.OrderBy(p => p.cableStatus).ToList();
                        break;


                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableDistributionID).ToList() : data.OrderBy(p => p.CableDistributionID).ToList();
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
        [ValidateAntiForgeryToken]
        public ActionResult GetStockDetailsItemListByStockID(int StockID)
        {
            var lstStockDetails = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsRunning).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
            return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsID(int StockDetailsID, int DistributionID)
        {
            try
            {
                if (DistributionID != null && DistributionID > 0)
                {
                    Recovery recovery =
                        db.Recovery.Where(s => s.DistributionID == DistributionID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();

                    if (recovery != null)
                    {
                        recovery.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                    }

                    Distribution distribution =
                        db.Distribution.Where(s => s.DistributionID == DistributionID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).FirstOrDefault();

                    if (distribution != null)
                    {
                        distribution.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                    }
                    StockDetails stockDetails =
                       db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();

                    if (stockDetails != null)
                    {
                        stockDetails.SectionID = AppUtils.StockSection;
                        stockDetails.ProductStatusID = AppUtils.ProductStatusIsAvialable;
                    }
                    db.SaveChanges();

                    return Json(new { SuccessDeleteDistribution = true, StockDetailsID = StockDetailsID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessDeleteDistribution = false, StockDetailsID = "" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { SuccessDeleteDistribution = false, StockDetailsID = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeCableSection(int CableDistributionID, int NewCableStatus)
        {
            CableDistribution cableDistribution =
                db.CableDistribution.Where(s => s.CableDistributionID == CableDistributionID).FirstOrDefault();
            if (cableDistribution != null)
            {
                if (NewCableStatus == cableDistribution.CableIndicatorStatus && NewCableStatus != AppUtils.SelectedWhereToPassIsMainBox)
                //we dont need to check for when we will pass to main box cause after change the section below it will delete the distribution details
                //but for other it will keep the information only it will change the status.so think now we have cable in old box section. but again we are change 
                //this cable to old section then what will be happen. it will automatically add quantity +. so we have have to check here giben type is same or not.
                {
                    return Json(new { NewCableTypeSameAsOldType = true }, JsonRequestBehavior.AllowGet);
                }
                if (NewCableStatus == AppUtils.SelectedWhereToPassIsMainBox)
                {
                    CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cableDistribution.CableStockID).FirstOrDefault();
                    if (cableStock != null)
                    {
                        cableStock.UsedQuantityFromThisBox -= cableDistribution.AmountOfCableUsed;
                        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
                        db.SaveChanges();
                    }
                    db.Entry(cableDistribution).State = EntityState.Deleted;
                    db.SaveChanges();
                    return Json(new { Success = true, DeleteStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus }, JsonRequestBehavior.AllowGet);
                }
                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsOCBox)
                {
                    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
                    if (cableStock != null)
                    {
                        cableStock.CableQuantity += cableDistribution.AmountOfCableUsed;
                        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
                        db.Entry(cableStock).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsOldBox;
                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(cableDistribution).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
                }
                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsStolen)
                {
                    //if (cableDistribution.CableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
                    //{
                    //    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
                    //    if (cableStock != null)
                    //    {
                    //        cableStock.CableQuantity -= cableDistribution.AmountOfCableUsed;
                    //        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                    //        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
                    //        db.Entry(cableStock).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //}
                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsStolen;
                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(cableDistribution).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
                }
                else if (NewCableStatus == AppUtils.SelectedWhereToPassIsNotWorking)
                {
                    //if (cableDistribution.CableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
                    //{
                    //    CableStock cableStock = db.CableStock.Where(s => s.CableTypeID == AppUtils.CableTypeIsOldBox).FirstOrDefault();
                    //    if (cableStock != null)
                    //    {
                    //        cableStock.CableQuantity -= cableDistribution.AmountOfCableUsed;
                    //        cableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                    //        cableStock.UpdateDate = AppUtils.GetDateTimeNow();
                    //        db.Entry(cableStock).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //}
                    cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsNotWorking;
                    cableDistribution.UpdateBy = AppUtils.GetLoginEmployeeName();
                    cableDistribution.UpdateDate = AppUtils.GetDateTimeNow();
                    db.Entry(cableDistribution).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus, Date = cableDistribution.UpdateDate }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, ChangeStatus = "", CableDistributionID = "", NewCableStatus = "" },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, ChangeStatus = "", CableDistributionID = "", NewCableStatus = "" },
                    JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
        public ActionResult TotalItemListOverview()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockTotalListTotalItemListOverview()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockOverview> lstCustomStockOverview = new List<CustomStockOverview>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }
                List<int> lstWarrentyProductStockID = db.StockDetails.Where(s => s.WarrentyProduct == true).Select(s => s.StockID).ToList();

                var firstPartOfQuery = (StockID != "") ? db.Stock.Where(s => s.StockID == itemIDConvert && lstWarrentyProductStockID.Contains(s.StockID)).AsQueryable() : db.Stock.Where(s => lstWarrentyProductStockID.Contains(s.StockID)).AsQueryable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      )
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where((p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())))
                        .AsQueryable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockOverview()
                        {
                            StockID = s.StockID,
                            ItemName = s.Item.ItemName,
                            TotalItemCount = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.WarrentyProduct == true).Count(),
                            ProductInStock = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.StockSection && ss.WarrentyProduct == true).Count(),
                            ProductInRunning = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WorkingSection && ss.WarrentyProduct == true).Count(),
                            ProductInDead = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.DeadSection && ss.WarrentyProduct == true).Count(),
                            ProductInRepair = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.RepairingSection && ss.WarrentyProduct == true).Count(),
                            ProductInWarrenty = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WarrantySection && ss.WarrentyProduct == true).Count(),

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockOverview = this.SortByColumnWithOrderStockOverView(order, orderDir, lstCustomStockOverview);
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
                    data = lstCustomStockOverview
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
        [UserRIghtCheck(ControllerValue = AppUtils.View_Product_Total_List)]
        public ActionResult TotalNonWarrentyProductListOverview()
        {
            ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            return View(new List<CustomStockListSectionInformation>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomStockTotalListTotalNonWarrentyProductListOverview()
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
                var StockID = Request.Form.Get("StockID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockOverview> lstCustomStockOverview = new List<CustomStockOverview>();
                int itemIDConvert = 0;
                if (StockID != "")
                {
                    itemIDConvert = int.Parse(StockID);
                }
                List<int> lstNonWarrentyProductStockID = db.StockDetails.Where(s => s.WarrentyProduct == false).Select(s => s.StockID).ToList();
                var firstPartOfQuery = (StockID != "") ? db.Stock.Where(s => s.StockID == itemIDConvert && lstNonWarrentyProductStockID.Contains(s.StockID)).AsEnumerable() : db.Stock.Where(s => lstNonWarrentyProductStockID.Contains(s.StockID)).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
                                                                                      || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                      )
                        .Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where((p => p.ItemID.ToString().ToLower().Contains(search.ToLower())
                                                                    || p.Item.ItemName.ToString().ToLower().Contains(search.ToLower())))
                        .AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomStockOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockOverview()
                        {
                            StockID = s.StockID,
                            ItemName = s.Item.ItemName,
                            TotalItemCount = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.WarrentyProduct == false).Count(),
                            ProductInStock = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.StockSection && ss.WarrentyProduct == false).Count(),
                            ProductInRunning = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WorkingSection && ss.WarrentyProduct == false).Count(),
                            ProductInDead = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.DeadSection && ss.WarrentyProduct == false).Count(),
                            ProductInRepair = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.RepairingSection && ss.WarrentyProduct == false).Count(),
                            ProductInWarrenty = db.StockDetails.Where(ss => ss.StockID == s.StockID && ss.SectionID == AppUtils.WarrantySection && ss.WarrentyProduct == false).Count(),

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockOverview = this.SortByColumnWithOrderStockOverView(order, orderDir, lstCustomStockOverview);
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
                    data = lstCustomStockOverview
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



        public ActionResult CableOverView()
        {
            ViewBag.CableTypeID = new SelectList(db.CableType.Select(s => new { CableTypeID = s.CableTypeID, CableTypeName = s.CableTypeName }), "CableTypeID", "CableTypeName");
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");


            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
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

            //ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            ViewBag.CableTypePopUpID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            //ViewBag.lstCableStockID = new SelectList(db.CableStock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomCableListOverview()
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

                int CableTypeIDFromDDL = 0;
                var CableTypeID = Request.Form.Get("CableTypeID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomCableTypeOverview> lstCustomCableTypeOverview = new List<CustomCableTypeOverview>();
                int CableTypeIDConvert = 0;
                if (CableTypeID != "")
                {
                    CableTypeIDConvert = int.Parse(CableTypeID);
                }

                var firstPartOfQuery = (CableTypeID != "") ? db.CableType.Where(s => s.CableTypeID == CableTypeIDConvert).OrderBy(s => s.CableTypeName).AsEnumerable() : db.CableType.OrderBy(s => s.CableTypeName).AsEnumerable();


                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (firstPartOfQuery.Any()) ? firstPartOfQuery.Where(p => p.CableTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    firstPartOfQuery = firstPartOfQuery.Where(p => p.CableTypeName.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                }
                if (firstPartOfQuery.Any())
                {
                    totalRecords = firstPartOfQuery.Count();
                    lstCustomCableTypeOverview = firstPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomCableTypeOverview()
                        {
                            CableTypeID = s.CableTypeID,
                            CableTypeName = s.CableTypeName,
                            TotalCableTypeCount = db.CableStock.Where(ss => ss.CableTypeID == s.CableTypeID).Count()

                        }).ToList();

                }

                // Sorting.   
                lstCustomCableTypeOverview = this.SortByColumnWithOrderCableOverView(order, orderDir, lstCustomCableTypeOverview);
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
                    data = lstCustomCableTypeOverview
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



        private List<CustomCableTypeOverview> SortByColumnWithOrderCableOverView(string order, string orderDir, List<CustomCableTypeOverview> data)
        {
            // Initialization.   
            List<CustomCableTypeOverview> lst = new List<CustomCableTypeOverview>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeName).ToList() : data.OrderBy(p => p.CableTypeName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CableTypeID).ToList() : data.OrderBy(p => p.CableTypeID).ToList();
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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetAssetDetailsByAssetTypeID(int AssetTypeID)
        //{

        //    try
        //    {
        //        List<AssetCustomList> lstAssetCustomList = db.Asset.Where(s => s.AssetTypeID == AssetTypeID).Select(fp => new AssetCustomList
        //        {
        //            AssetID = fp.AssetID,
        //            AssetTypeName = fp.AssetType.AssetTypeName,
        //            AssetName = fp.AssetName,
        //            AssetValue = fp.AssetValue,
        //            PurchaseDate = fp.PurchaseDate,
        //            SerialNumber = fp.SerialNumber,
        //            WarrentyStartDate = fp.WarrentyStartDate,
        //            WarrentyEndDate = fp.WarrentyEndDate,

        //        }).ToList();
        //        return Json(new { Success = true, lstAssetByAssetTypeID = lstAssetCustomList }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //    }


        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCableDetailsByCableTypeID(int CableTypeID)
        {


            try
            {
                List<CableCustomList> lstCableCustomList = db.CableStock.Where(s => s.CableTypeID == CableTypeID).Select(s => new CableCustomList
                {
                    CableStockID = s.CableStockID,
                    CableTypeName = s.CableType.CableTypeName,
                    BoxDrumName = s.CableBoxName,
                    BrandName = s.Brand != null ? s.Brand.BrandName : "",
                    SupplierName = s.Supplier != null ? s.Supplier.SupplierName : "",
                    Invoice = s.SupplierInvoice == null ? "" : s.SupplierInvoice,
                    ReadingFrom = s.FromReading,
                    ReadingEnd = s.ToReading,
                    Quantity = s.CableQuantity,
                    Used = s.UsedQuantityFromThisBox,
                    Remain = s.CableQuantity - s.UsedQuantityFromThisBox

                }).ToList();
                return Json(new { Success = true, lstCableByCableTypeID = lstCableCustomList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }


        }

    }
}