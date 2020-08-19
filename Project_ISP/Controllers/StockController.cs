using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
//using CrystalDecisions.Shared.Json;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace Project_ISP.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]
    public class StockController : Controller
    {
        public StockController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();
        // GET: Stock


        [UserRIghtCheck(ControllerValue = AppUtils.Add_ProductORItem_In_Stock)]
        public ActionResult AddStock()
        {
            ViewBag.ItemID = new SelectList(db.Item.ToList(), "ItemID", "ItemName");
            ViewBag.BrandID = new SelectList(db.Brand.ToList(), "BrandID", "BrandName");
            ViewBag.SupplierID = new SelectList(db.Supplier.ToList(), "SupplierID", "SupplierName");

            return View();
        }

        [UserRIghtCheck(ControllerValue = AppUtils.Add_ProductORItem_In_Stock)]
        public ActionResult AddNonWarrentyStock()
        {
            ViewBag.ItemID = new SelectList(db.Item.ToList(), "ItemID", "ItemName");
            ViewBag.BrandID = new SelectList(db.Brand.ToList(), "BrandID", "BrandName");
            ViewBag.SupplierID = new SelectList(db.Supplier.ToList(), "SupplierID", "SupplierName");

            return View();
        }


        [UserRIghtCheck(ControllerValue = AppUtils.Add_Cable_In_Stock)]
        public ActionResult AddStockForCable()
        {
            ViewBag.BrandID = new SelectList(db.Brand.ToList(), "BrandID", "BrandName");
            ViewBag.SupplierID = new SelectList(db.Supplier.ToList(), "SupplierID", "SupplierName");

            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");

            List<CableStock> lstCableStock = db.CableStock.ToList();

            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult InsertStockDistributionForCable(List<CableStock> lstCableStock)
        {
            try
            {
                List<string> lstCableBoxName = lstCableStock.ToList().Select(s => s.CableBoxName).ToList();
                List<string> lstCableBoxNameFromDbMatchWithClientList = db.CableStock.Where(s => lstCableBoxName.Contains(s.CableBoxName.Trim()) && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).Select(s => s.CableBoxName).ToList();

                if (lstCableBoxNameFromDbMatchWithClientList.Count > 0)
                {
                    return Json(new { Success = false, BoxNameAlreadyAdded = true, BoxNameList = lstCableBoxNameFromDbMatchWithClientList }, JsonRequestBehavior.AllowGet);
                }

                List<CableStock> lstCableStocks = new List<CableStock>();
                foreach (var item in lstCableStock)
                {
                    item.CableBoxName = item.CableBoxName.Trim().Replace(" ", "");
                    item.CableUnitID = AppUtils.CableUnitIsMeater;
                    item.EmployeeID = AppUtils.GetLoginUserID();
                    item.CreatedBy = AppUtils.GetLoginEmployeeName();
                    item.CreatedDate = AppUtils.GetDateTimeNow();
                    item.IndicatorStatus = AppUtils.IndicatorStatusIsActive;
                    item.CableQuantity = item.ToReading - item.FromReading;
                    lstCableStocks.Add(item);
                }
                if (lstCableStocks.Count > 0)
                {
                    db.CableStock.AddRange(lstCableStocks);
                    db.SaveChanges();
                    return Json(new { Success = true, SavedSuccessfully = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = true, SavedSuccessfully = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, SavedSuccessfully = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CheckSerialNumberIsExistOrNot(string serial)
        {
            StockDetails stockDetails = db.StockDetails.Where(s => s.Serial == serial.Trim()).FirstOrDefault();

            bool serialExistOrNot = (stockDetails == null) ? false : true;

            return Json(new { SerialExistOrNot = serialExistOrNot }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckBoxNumberIsExistOrNot(string BoxName)
        {
            CableStock cableStock = db.CableStock.Where(s => s.CableBoxName.Trim() == BoxName.Trim().Replace(" ", "")).FirstOrDefault();

            bool BoxNameExistOrNot = (cableStock == null) ? false : true;

            return Json(new { BoxNameExistOrNot = BoxNameExistOrNot }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertStockDistribution(List<Client_Stock_StockDetails_ForDistribution> lstClientStockStockDetailsForDistributions)
        {

            List<int> lstSertialFromClientSide = lstClientStockStockDetailsForDistributions.ToList().Select(s => s.StockDetailsID).ToList();
            List<string> lstStockDetailsSerialSerial = db.Distribution.Where(s => lstSertialFromClientSide.Contains(s.StockDetailsID) && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).Select(s => s.StockDetails.Serial).ToList();

            if (lstStockDetailsSerialSerial.Count > 0)
            {
                return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetailsSerialSerial }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    foreach (var item in lstClientStockStockDetailsForDistributions)
                    {

                        /********************************************************************************************/
                        //Inserting New Distribution
                        Distribution distribution = new Distribution();
                        SetNewStockDistribution(ref distribution, item);
                        db.Distribution.Add(distribution);
                        db.SaveChanges();
                        /********************************************************************************************/
                        /********************************************************************************************/
                        //Updating Product status and section which is newly assign now
                        if (distribution.DistributionID > 0)
                        {
                            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == distribution.StockDetailsID).FirstOrDefault();
                            stockDetails.SectionID = AppUtils.WorkingSection;
                            stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
                            db.Entry(stockDetails).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        /********************************************************************************************/
                        /********************************************************************************************/
                        //for updateing the old item for which we will update/dead/warenty/notworkable.............
                        if (item.OldProductStatusID != null && item.OldSectionID != null && item.OldStockDetailsID != null && item.OldStockID != null &&
                            item.OldProductStatusID > 0 && item.OldSectionID > 0 && item.OldStockDetailsID > 0 && item.OldStockID > 0)
                        //if (item.OldProductStatusID > 0 && item.OldSectionID > 0 && item.OldStockDetailsID > 0 && item.OldStockID > 0)
                        {
                            StockDetails oldStockDetailsStatusAndSectionUpdate = db.StockDetails.Where(s => s.StockDetailsID == item.OldStockDetailsID).FirstOrDefault();
                            if (oldStockDetailsStatusAndSectionUpdate != null)
                            {
                                oldStockDetailsStatusAndSectionUpdate.ProductStatusID = item.OldProductStatusID.Value;
                                oldStockDetailsStatusAndSectionUpdate.SectionID = item.OldSectionID.Value;
                                oldStockDetailsStatusAndSectionUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                                oldStockDetailsStatusAndSectionUpdate.UpdateDate = AppUtils.GetDateTimeNow();
                            }
                            db.Entry(oldStockDetailsStatusAndSectionUpdate).State = EntityState.Modified;
                            /********************************************************************************************/
                            //removing the old item information from the Distribution list
                            Distribution removeDistributionForOldItem = db.Distribution.Where(s => s.StockDetailsID == item.OldStockDetailsID).FirstOrDefault();
                            if (removeDistributionForOldItem != null)
                            {
                                removeDistributionForOldItem.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                                removeDistributionForOldItem.UpdateBy = AppUtils.GetLoginEmployeeName();
                                removeDistributionForOldItem.UpdateDate = AppUtils.GetDateTimeNow();
                                db.Entry(removeDistributionForOldItem).State = EntityState.Modified;
                            }
                            /********************************************************************************************/
                            db.SaveChanges();
                            /************* when press Not Workable/Upgrade then we need to insert information into recovery table for who is recovered and where?************/
                            Recovery recovery = new Recovery();
                            SetRecoveryInformationForUpgradeOrNotWorkableReason(ref recovery, item, distribution);
                            db.Recovery.Add(recovery);
                            db.SaveChanges();
                            /********************************************************************************************/
                            /**************suppose distribuitonid is used in Recovery table mean already it was recovered some other distribution. 
                             * but now this distribution is not working/something else then we need to search in recovery table for this id is exist not. if yes then change the status to deleted ***************/
                            Recovery recoveryExistForThisRemoveDistributionForOldItem =
                                db.Recovery.Where(s => s.DistributionID == removeDistributionForOldItem.DistributionID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive)
                                    .FirstOrDefault();
                            if (recoveryExistForThisRemoveDistributionForOldItem != null)
                            {
                                recoveryExistForThisRemoveDistributionForOldItem.IndicatorStatus = AppUtils.IndicatorStatusIsDelete;
                                recoveryExistForThisRemoveDistributionForOldItem.UpdateDate = AppUtils.GetDateTimeNow();
                                recoveryExistForThisRemoveDistributionForOldItem.UpdateBy = AppUtils.GetLoginEmployeeName();
                                db.Entry(recoveryExistForThisRemoveDistributionForOldItem).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            /********************************************************************************************/
                            ///
                        }
                        /********************************************************************************************/
                    }
                    return Json(new { Success = true, SavedSuccessfully = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = true, SavedSuccessfully = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void SetRecoveryInformationForUpgradeOrNotWorkableReason(ref Recovery recovery, Client_Stock_StockDetails_ForDistribution item, Distribution distribution)
        {
            recovery.EmployeeID = item.EmployeeID;
            recovery.DistributionReasonID = item.DistributionReasonID;
            recovery.DistributionID = distribution.DistributionID;
            recovery.StockDetailsID = item.OldStockDetailsID.Value;
            // recovery.BoxID = (item.BoxID != null) ? item.BoxID.Value : Null;
            if ((item.BoxID != null))
                recovery.BoxID = item.BoxID.Value;
            else
                recovery.BoxID = null;

            if ((item.PopID != null))
                recovery.PopID = item.PopID.Value;
            if ((item.CustomerID != null))
                recovery.ClientDetailsID = item.CustomerID.Value;

            //recovery.BoxID = (item.BoxID != null) ? item.BoxID.Value : 0;
            //recovery.PopID = (item.PopID != null) ? item.PopID.Value : 0;
            //recovery.ClientDetailsID = (item.CustomerID != null) ? item.CustomerID.Value : 0;

            recovery.IndicatorStatus = AppUtils.IndicatorStatusIsActive;
            recovery.RecoveryDate = AppUtils.GetDateTimeNow();
            recovery.CreatedBy = AppUtils.GetLoginEmployeeName();
            recovery.CreatedDate = AppUtils.GetDateTimeNow();
        }

        private void SetNewStockDistribution(ref Distribution distribution, Client_Stock_StockDetails_ForDistribution item)
        {
            distribution.EmployeeID = item.EmployeeID;
            distribution.StockDetailsID = item.StockDetailsID;
            distribution.PopID = item.PopID;
            distribution.BoxID = item.BoxID;
            distribution.ClientDetailsID = item.CustomerID;
            distribution.DistributionReasonID = item.DistributionReasonID;
            distribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            distribution.CreatedDate = AppUtils.GetDateTimeNow();
            distribution.IndicatorStatus = AppUtils.IndicatorStatusIsActive;
            distribution.Remarks = item.Remarks;
        }

        [HttpPost]
        public ActionResult InsertStockItem(List<Client_Stock_StockDetails> ItemList)
        {
            //foreach (var item in ItemList)
            //{

            //}

            List<string> lstSertialFromClientSide = ItemList.ToList().Select(s => s.Serial.Trim()).ToList();
            List<string> lstStockDetails =
                db.StockDetails.Where(s => lstSertialFromClientSide.Contains(s.Serial)).Select(s => s.Serial).ToList();

            if (lstStockDetails.Count > 0)
            {
                return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    foreach (var item in ItemList)
                    {
                        Stock stock = db.Stock.Where(s => s.ItemID == item.ItemID).FirstOrDefault();
                        StockDetails stockDetails = new StockDetails();
                        if (stock != null)
                        {
                            SetStockDetailsInformation(ref stockDetails, item, stock);

                            db.StockDetails.Add(stockDetails);
                            db.SaveChanges();
                            if (stock.Quantity == null)
                            {
                                stock.Quantity = 0;
                            }
                            stock.Quantity += 1;

                            db.Entry(stock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            Stock st = new Stock();
                            st.ItemID = item.ItemID;
                            db.Stock.Add(st);
                            db.SaveChanges();

                            if (st.StockID > 0)
                            {
                                SetStockDetailsInformation(ref stockDetails, item, st);
                                db.StockDetails.Add(stockDetails);
                                db.SaveChanges();
                                if (st.Quantity == null)
                                {
                                    st.Quantity = 0;
                                }
                                st.Quantity += 1;

                                db.Entry(st).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    return Json(new { Success = true, SavedSuccessfully = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = true, SavedSuccessfully = false }, JsonRequestBehavior.AllowGet);
                }

            }
        }


        [HttpPost]
        public ActionResult InsertCableInStock(List<CableStock> insertCable)
        {

            //List<string> lstSertialFromClientSide = ItemList.ToList().Select(s => s.Serial.Trim()).ToList();
            //List<string> lstStockDetails =
            //    db.StockDetails.Where(s => lstSertialFromClientSide.Contains(s.Serial)).Select(s => s.Serial).ToList();

            //if (lstStockDetails.Count > 0)
            //{
            //    return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails },
            //        JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    try
            //    {
            //        foreach (var item in ItemList)
            //        {
            //            Stock stock = db.Stock.Where(s => s.ItemID == item.ItemID).FirstOrDefault();
            //            StockDetails stockDetails = new StockDetails();
            //            if (stock != null)
            //            {
            //                SetStockDetailsInformation(ref stockDetails, item, stock);

            //                db.StockDetails.Add(stockDetails);
            //                db.SaveChanges();
            //                stock.Quantity += 1;

            //                db.Entry(stock).State = EntityState.Modified;
            //                db.SaveChanges();
            //            }
            //            else
            //            {
            //                Stock st = new Stock();
            //                st.ItemID = item.ItemID;
            //                db.Stock.Add(st);
            //                db.SaveChanges();

            //                if (st.StockID > 0)
            //                {
            //                    SetStockDetailsInformation(ref stockDetails, item, st);
            //                    db.StockDetails.Add(stockDetails);
            //                    db.SaveChanges();
            //                    if (st.Quantity == null)
            //                    {
            //                        st.Quantity = 0;
            //                    }
            //                    st.Quantity += 1;

            //                    db.Entry(st).State = EntityState.Modified;
            //                    db.SaveChanges();
            //                }
            //            }
            //        }
            //        return Json(new { Success = true, SavedSuccessfully = true }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { Success = true, SavedSuccessfully = false }, JsonRequestBehavior.AllowGet);
            //    }

            //}
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }



        [UserRIghtCheck(ControllerValue = AppUtils.Delete_Product_if_Distributed_by_mistake)]
        public ActionResult StockList()
        {
            ViewBag.ItemID = new SelectList(db.Item.ToList(), "ItemID", "ItemName");

            //List<VM_Stock_StockDetails> lstVM_Stock_StockDetails = new List<VM_Stock_StockDetails>();
            //lstVM_Stock_StockDetails = db.Stock.GroupJoin(db.StockDetails, Stock => Stock.StockID, StockDetails => StockDetails.StockID,
            //    (Stock, StockDetails) => new VM_Stock_StockDetails { Stock = Stock, LstStockDetails = StockDetails.ToList() }).ToList();

            return View(new List<CustomStockListInformation>());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockListInformation()
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
                var ItemID = Request.Form.Get("ItemID");
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                List<CustomStockListInformation> lstCustomStockListInformation = new List<CustomStockListInformation>();
                int itemIDConvert = 0;
                if (ItemID != "")
                {
                    itemIDConvert = int.Parse(ItemID);
                }

                var firstPartOfQuery = (ItemID != "") ? db.Stock.Where(s => s.ItemID == itemIDConvert).AsEnumerable() : db.Stock.AsEnumerable();
                var secondPartOfQuery = firstPartOfQuery
                    .Join(db.StockDetails, Stock => Stock.StockID, StockDetails => StockDetails.StockID,
                    (Stock, StockDetails) => new
                    {
                        Stock = Stock,
                        StockDetails = StockDetails
                    })
                    .AsEnumerable();

                // Verification.   
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.Stock.StockID.ToString().ToLower().Contains(search.ToLower())
                                                                                                    || p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                                                    || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                                                    || p.StockDetails.Brand.BrandName.ToString().ToLower().Contains(search.ToLower())
                                                                                                    || p.StockDetails.Serial.Contains(search.ToLower())
                                                                                                    || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                                                    || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower())
                                                                                                   ).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.Stock.StockID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.StockDetails.StockDetailsID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.Stock.Item.ItemName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.StockDetails.Brand.BrandName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.StockDetails.Serial.Contains(search.ToLower())
                                                                     || p.StockDetails.Section.SectionName.Contains(search.ToLower())
                                                                     || p.StockDetails.ProductStatus.ProductStatusName.ToLower().Contains(search.ToLower())
                    ).AsEnumerable();
                }
                if (secondPartOfQuery.Any())
                {
                    totalRecords = secondPartOfQuery.Count();
                    lstCustomStockListInformation = secondPartOfQuery.AsEnumerable().Skip(startRec).Take(pageSize).Select(
                        s => new CustomStockListInformation()
                        {
                            StockID = s.Stock.StockID,
                            StockDetailsID = s.StockDetails.StockDetailsID,
                            ItemName = s.Stock.Item.ItemName,
                            BrandName = s.StockDetails.Brand.BrandName,
                            Serial = s.StockDetails.Serial,
                            SectionName = s.StockDetails.Section.SectionName,
                            ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                            DeleteStockList = true,

                        }).ToList();

                }

                // Sorting.   
                lstCustomStockListInformation = this.SortByColumnWithOrder(order, orderDir, lstCustomStockListInformation);
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
                    data = lstCustomStockListInformation
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

        private List<CustomStockListInformation> SortByColumnWithOrder(string order, string orderDir, List<CustomStockListInformation> data)
        {

            // Initialization.   
            List<CustomStockListInformation> lst = new List<CustomStockListInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockID).ToList() : data.OrderBy(p => p.StockID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockDetailsID).ToList() : data.OrderBy(p => p.StockDetailsID).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ItemName).ToList() : data.OrderBy(p => p.ItemName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BrandName).ToList() : data.OrderBy(p => p.BrandName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Serial).ToList() : data.OrderBy(p => p.Serial).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SectionName).ToList() : data.OrderBy(p => p.SectionName).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProductStatusName).ToList() : data.OrderBy(p => p.ProductStatusName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockID).ToList() : data.OrderBy(p => p.StockID).ToList();
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
        public ActionResult DeleteItemFromStockList(int StockDetailsID)
        {
            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == StockDetailsID).FirstOrDefault();
            if (stockDetails == null)
            {
                return Json(new { DeleteStatus = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Stock stock = db.Stock.Where(s => s.StockID == stockDetails.StockID).FirstOrDefault();
              //  if (stockDetails.ProductStatusID == AppUtils.ProductStatusIsAvialable && stockDetails.SectionID == AppUtils.StockSection)
              //  {
                    /// first removing from Recovery table 
                   // List<int> lstDistribution = db.Distribution.Where(s => s.StockDetailsID == StockDetailsID) .Select(s => s.DistributionID).ToList();
                    db.Recovery.RemoveRange(db.Recovery.Where(s => s.StockDetailsID == StockDetailsID));
                    db.SaveChanges();

                    //then remove from distribution table 
                    db.Distribution.RemoveRange(db.Distribution.Where(s => s.StockDetailsID == StockDetailsID));
                    db.SaveChanges();

                    //now  remove from stock details table
                    db.StockDetails.Remove(stockDetails);
                    db.SaveChanges();
                    
                    //decrease quantity from stock quantity table 
                    stock.Quantity -= 1;
                    db.SaveChanges();
                    return Json(new { DeleteStatus = true, ProductStatus = false, StockDetailsID = StockDetailsID }, JsonRequestBehavior.AllowGet);
              //  }
                //else
                //{
                //    return Json(new { DeleteStatus = false, ProductStatus = true }, JsonRequestBehavior.AllowGet);
                //}
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchStockListByCriteria(int ItemID)
        {

            List<dynamic> lstDynamic = new List<dynamic>();
            var lst = db.Stock.Where(s => s.ItemID == ItemID)
                .Join(db.StockDetails, Stock => Stock.StockID, StockDetails => StockDetails.StockID,
                    (Stock, StockDetails) => new { Stock = Stock, StockDetails = StockDetails }).Select(s => new
                    {
                        StockID = s.Stock.StockID,
                        StockDetailsID = s.StockDetails.StockDetailsID,
                        ItemName = s.Stock.Item.ItemName,
                        BrandName = s.StockDetails.Brand.BrandName,
                        Serial = s.StockDetails.Serial,
                        SectionName = s.StockDetails.Section.SectionName,
                        ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName,
                        SupplierName = s.Stock.StockID,

                    }).ToList();

            int i = 0;
            dynamic d = new ExpandoObject();
            foreach (var item in lst)
            {

                d.StockID = item.StockID;
                d.StockIDs = item.StockID;
                lstDynamic.Add(d);
            }
            return Json(new { lstDynamic = lst }, JsonRequestBehavior.AllowGet);
        }
        private void SetStockDetailsInformation(ref StockDetails stockDetails, Client_Stock_StockDetails item, Stock stock)
        {
            stockDetails.WarrentyProduct = item.WarrentyProduct ? true : false;
            stockDetails.BrandID = item.BrandID == 0 ? null : (int?)item.BrandID;
            stockDetails.StockID = stock.StockID;
            stockDetails.SupplierID = item.SupplierID  == 0 ? null : (int?)item.SupplierID;
            stockDetails.ProductStatusID = AppUtils.ProductStatusIsAvialable;
            stockDetails.SectionID = AppUtils.StockSection;
            stockDetails.Serial = item.Serial;
            stockDetails.BarCode = "";
            stockDetails.SupplierInvoice = item.SupplierInvoice;
            stockDetails.CreatedBy = AppUtils.GetLoginEmployeeName();
            stockDetails.CreatedDate = AppUtils.GetDateTimeNow();
        }


        [UserRIghtCheck(ControllerValue = AppUtils.Stock_Distribution)]
        public ActionResult StockDistribution()
        {
            //ViewBag.popsConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            //ViewBag.popsPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            //ViewBag.popsSecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            //ViewBag.popsLineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            //ViewBag.popsZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");


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


            ViewBag.lstStockIDForPopUp = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.DistributionReasonIDPopUp = new SelectList(db.DistributionReason.ToList(), "DistributionReasonID", "DistributionReasonName");
            // ViewBag.ProductStatusID = new SelectList(db.ProductStatus.ToList(), "ProductStatusID", "ProductStatusName");
            //we will give working section here if we need to use this item then we need to start from stock assign.
            ViewBag.SectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");

            ViewBag.lstStockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.EmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).ToList(), "EmployeeID", "Name");
            ViewBag.PopID = new SelectList(db.Pop.ToList(), "PopID", "PopName");
            //ViewBag.BoxID = new SelectList(db.Box.ToList(), "BoxID", "BoxName");
            ViewBag.CustomerID = new SelectList(db.ClientDetails.ToList(), "ClientDetailsID", "Name");
            ViewBag.DistributionReasonID = new SelectList(db.DistributionReason.ToList(), "DistributionReasonID", "DistributionReasonName");
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");
            return View();
        }



        [UserRIghtCheck(ControllerValue = AppUtils.Cable_Distribution)]
        public ActionResult CableStockDistributionToEmployeeOrClient()
        {

            ViewBag.lstEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).ToList(), "EmployeeID", "Name");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            ViewBag.lstStockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
            ViewBag.lstClientDetailsID = new SelectList(db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).ToList(), "ClientDetailsID", "LoginName");
            ViewBag.lstAssignEmployee = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).ToList(), "EmployeeID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStockDetailsItemListByStockID(int StockID)
        {
            var lstStockDetails = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
            return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetStockDetailsItemListByStockIDForPopUp(int StockID, List<int?> lstStockDetailsIDForRemoveWhenPassedByStockID)
        {
            //we will pass only those id which is distributed
            List<int> lstStockDetailsByStockIDForShowingInPopUpStockDetailsItemSerial =
                db.StockDetails.Where(s => s.StockID == StockID).Select(s => s.StockDetailsID).ToList();
            List<int> lstStockDetailsIDFromDistribution =
                db.Distribution.Where(s => lstStockDetailsByStockIDForShowingInPopUpStockDetailsItemSerial.Contains(s.StockDetailsID) && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).Select(s => s.StockDetailsID).ToList();

            if (lstStockDetailsIDForRemoveWhenPassedByStockID != null)
            {
                var lstStockDetails =
                    db.StockDetails.Where(
                        s => lstStockDetailsIDFromDistribution.Contains(s.StockDetailsID) && !lstStockDetailsIDForRemoveWhenPassedByStockID.Contains(s.StockDetailsID))
                        .ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
                return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lstStockDetails = db.StockDetails.Where(s => lstStockDetailsIDFromDistribution.Contains(s.StockDetailsID)).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
                return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
            }

            //if (lstStockDetailsIDForRemoveWhenPassedByStockID.Count > 0)
            //{
            //    var lstStockDetails =
            //        db.StockDetails.Where(
            //            s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && !lstStockDetailsIDForRemoveWhenPassedByStockID.Contains(s.StockDetailsID))
            //            .ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
            //    return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    var lstStockDetails = db.StockDetails.Where(s => s.StockID == StockID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList().Select(s => new { StockDetailsID = s.StockDetailsID, Serial = s.Serial });
            //    return Json(new { lstStockDetails = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //}

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
        public ActionResult SearchCableBoxOrDrumNameByCableTypeID(int CableTypeID)
        {
            var lstCableStocks = db.CableStock.Where(s => s.CableTypeID == CableTypeID).ToList().Select(s => new
            {
                CableStockID = s.CableStockID,
                BoxOrDrumName = s.CableBoxName
            });
            if (lstCableStocks.Any())
            {
                return Json(new { Success = true, ListCableStock = lstCableStocks }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, ListCableStock = "" }, JsonRequestBehavior.AllowGet);
            }


            return Json(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchCableQuantityStockedByCableBoxOrDrumName(int CableStockID)
        {
            var cableStock = db.CableStock.Where(s => s.CableStockID == CableStockID).AsQueryable();
            if (cableStock != null)
            {
                var stock = cableStock.Select(s => new
                {
                    CableStockID = s.CableStockID,
                    CableQuantity = s.CableQuantity,
                    UsedQuantityFromThisBox = s.UsedQuantityFromThisBox
                });
                return Json(new { Success = true, CableStock = stock }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, CableStock = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertCableStockDistributionForClientOrEmployee(List<ClientCableDistribution> ClientCableDistribution)
        {
            bool cableAvialableInDB = true;
            string boxNameListWhichIsNotAvialable = "";
            if (ClientCableDistribution.Count > 0)
            {
                List<int> lstCableDistributionID = ClientCableDistribution.Select(s => s.CableStokID).Distinct().ToList();
                foreach (var item in lstCableDistributionID)
                {
                    int CableQuantitySummmationFromClient = ClientCableDistribution.Where(s => s.CableStokID == item).Sum(s => s.CableQuantity);

                    CableStock cableStock = db.CableStock.Where(s => s.CableStockID == item).FirstOrDefault();
                    if (cableStock != null)
                    {
                        int totalAvialableInDB = cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox;

                        if (totalAvialableInDB < CableQuantitySummmationFromClient)
                        {
                            cableAvialableInDB = false;
                            boxNameListWhichIsNotAvialable += " " + cableStock.CableBoxName;
                        }
                    }
                }

                if (cableAvialableInDB == false)
                {
                    return Json(new { CableAvialableInDB = cableAvialableInDB, BoxNameListWhichIsNotAvialable = boxNameListWhichIsNotAvialable }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        List<CableDistribution> lstCableDistribution = new List<CableDistribution>();
                        foreach (var item in ClientCableDistribution)
                        {
                            CableDistribution cableDistribution = new CableDistribution();
                            AssignCableDetailsFromClient(ref cableDistribution, item);
                            lstCableDistribution.Add(cableDistribution);
                            db.CableDistribution.Add(cableDistribution);
                            db.SaveChanges();
                            CableStock cableStock = db.CableStock.Where(s => s.CableStockID == item.CableStokID).FirstOrDefault();
                            cableStock.UsedQuantityFromThisBox += item.CableQuantity;
                            db.Entry(cableStock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        //db.CableDistribution.AddRange(lstCableDistribution);
                        //db.SaveChanges();
                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new { Success = false, ItemListIsEmpty = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);


        }

        private void AssignCableDetailsFromClient(ref CableDistribution cableDistribution, ClientCableDistribution item)
        {
            cableDistribution.CableStockID = item.CableStokID;
            cableDistribution.AmountOfCableUsed = item.CableQuantity;
            cableDistribution.CableAssignFromWhere = AppUtils.CableAssignFromViewList;
            cableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
            if (item.ClientID != null)
            {
                cableDistribution.ClientDetailsID = item.ClientID.Value;
                cableDistribution.Purpose = "Client purpous";
            }
            if (item.EmployeeID != null)
            {
                cableDistribution.EmployeeID = item.EmployeeID.Value;
            }
            if (item.AssignEmployee != null)
            {
                cableDistribution.Purpose = "Employee purpous";
                cableDistribution.CableForEmployeeID = item.AssignEmployee.Value;
            }
            cableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            cableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
        }

        //[HttpGet]
        //public ActionResult ItemInWarrentyList()
        //{
        //    ViewBag.StockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");
        //    ViewBag.lstSectionID = new SelectList(db.Section.Where(s => s.SectionID != AppUtils.WorkingSection).ToList(), "SectionID", "SectionName");
        //    List<StockDetails> lstSoStockDetailse =
        //        db.StockDetails.Where(
        //            s =>
        //                s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection)
        //            .ToList();
        //    return View(lstSoStockDetailse);
        //}

    }
}