using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using static ISP_ManagementSystemModel.AppUtils;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ItemController : Controller
    {
        public ItemController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Item_List)]
        public ActionResult GenralItemList()
        {
            ViewBag.ItemFor = new SelectList(Enum.GetValues(typeof(ItemFor)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(ItemFor), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return View(new List<Item>());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetGeneralItemsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.  
                int itemForFromDDL = 0;
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var ItemFor = Request.Form.Get("ItemFor");

                if (!string.IsNullOrEmpty(ItemFor))
                {
                    itemForFromDDL = int.Parse(ItemFor);
                }
                //IEnumerable<Item> items = Enumerable.Empty<Item>();
                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                var items = db.Item.Where(x=>x.ItemFor == (int)AppUtils.ItemFor.General).AsQueryable();
                if (itemForFromDDL > 0)
                {
                    items = items.Where(x => x.ItemFor == itemForFromDDL).AsQueryable();
                }

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (items.Any()) ? items.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower()) || p.ItemName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    // Apply search   
                    items = items.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower()) || p.ItemName.ToString().ToLower().Contains(search.ToLower())).AsQueryable();
                }

                //if (items.Any())
                //{
                    //totalRecords = items.Count();
                    finalItem = items.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            s => new
                            {
                                ItemID = s.ItemID,
                                ItemName = s.ItemName,
                                ItemFor = s.ItemFor.HasValue ? Enum.GetName(typeof(ItemFor), s.ItemFor.Value) : "",
                                UpdateStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Item) ? true : false

                            }).ToList(); 
                //}

                // Sorting.   
                finalItem = this.SortByColumnWithOrderForGeneralItem(order, orderDir, finalItem);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : items.Count();

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = finalItem
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { 
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }
        private IEnumerable<dynamic> SortByColumnWithOrderForGeneralItem(string order, string orderDir, IEnumerable<dynamic> finalItem)
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemID).ToList() : finalItem.OrderBy(p => p.ItemID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemFor).ToList() : finalItem.OrderBy(p => p.ItemFor).ToList();
                    //    break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemName).ToList() : finalItem.OrderBy(p => p.ItemName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemID).ToList() : finalItem.OrderBy(p => p.ItemID).ToList();
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
        //[UserRIghtCheck(ControllerValue = AppUtils.Add_Item)]
        //public ActionResult InsertItem()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult InsertItem(Item Item_Client)
        //{
        //    Item Item_Check = db.Item.Where(s => s.ItemName == Item_Client.ItemName.Trim()).FirstOrDefault();

        //    if (Item_Check != null)
        //    {
        //        TempData["AlreadyInsert"] = "Item Already Added. Choose different Item. ";

        //        return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
        //    }

        //    Item Item_Return = new Item();

        //    try
        //    {
        //        Item_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
        //        Item_Client.CreatedDate = AppUtils.GetDateTimeNow();

        //        Item_Return = db.Item.Add(Item_Client);
        //        db.SaveChanges();

        //        if (Item_Return.ItemID > 0)
        //        {
        //            TempData["SaveSucessOrFail"] = "Save Successfully.";
        //            return Json(new { SuccessInsert = true, Item = Item_Return }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            TempData["SaveSucessOrFail"] = "Save Failed.";
        //            return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult InsertItemFromPopUp(Item Item_Client)
        {
            Item Item_Check = db.Item.Where(s => s.ItemName == Item_Client.ItemName.Trim()).FirstOrDefault();

            if (Item_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Item Already Added. Choose different Item. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Item Item_Return = new Item();

            try
            {
                Item_Client.ItemFor = (int)AppUtils.ItemFor.General;
                Item_Client.CreatedBy = AppUtils.GetLoginUserID().ToString()/*AppUtils.GetLoginEmployeeName()*/;
                Item_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Item_Return = db.Item.Add(Item_Client);
                db.SaveChanges();

                if (Item_Return.ItemID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Item = Item_Return, ItemFor = Enum.GetName(typeof(ItemFor), Item_Return.ItemFor.Value) }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetItemDetailsByID(int ItemID)
        {
            var Item = db.Item.Where(s => s.ItemID == ItemID).Select(s => new { ItemName = s.ItemName, ItemFor = s.ItemFor }).FirstOrDefault();


            var JSON = Json(new { ItemDetails = Item }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateItem(Item ItemInfoForUpdate)
        { 
            try
            { 
                Item Item_Check = db.Item.Where(s => s.ItemID != ItemInfoForUpdate.ItemID && s.ItemName == ItemInfoForUpdate.ItemName.Trim()).FirstOrDefault();

                if (Item_Check != null)
                { 
                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Item_db = db.Item.Where(s => s.ItemID == ItemInfoForUpdate.ItemID);
                ItemInfoForUpdate.ItemFor = Item_db.FirstOrDefault().ItemFor;
                ItemInfoForUpdate.CreatedBy = Item_db.FirstOrDefault().CreatedBy;
                ItemInfoForUpdate.CreatedDate = Item_db.FirstOrDefault().CreatedDate;
                ItemInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                ItemInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Item_db.SingleOrDefault()).CurrentValues.SetValues(ItemInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Item_Return = Item_db.Select(s => new { ItemID = s.ItemID, PackageName = s.ItemName,ItemFor = s.ItemFor });
                var JSON = Json(new { UpdateSuccess = true, ItemUpdateInformation = Item_Return, ItemFor = Enum.GetName(typeof(ItemFor), Item_Return.FirstOrDefault().ItemFor) }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ItemUpdateInformation = "" }, JsonRequestBehavior.AllowGet); 
            } 
        }






        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Item_List)]
        public ActionResult BandwithResellerItemList()
        {
            ViewBag.ItemFor = new SelectList(Enum.GetValues(typeof(ItemFor)).Cast<ItemFor>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(ItemFor), v),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return View(new List<Item>());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetBandwithResellerItemsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.  
                int itemForFromDDL = 0;
                int ifSearch = 0;
                int totalRecords = 0;
                int recFilter = 0;
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var ItemFor = Request.Form.Get("ItemFor");

                if (!string.IsNullOrEmpty(ItemFor))
                {
                    itemForFromDDL = int.Parse(ItemFor);
                }
                //IEnumerable<Item> items = Enumerable.Empty<Item>();
                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                var items = db.Item.Where(x => x.ItemFor == (int)AppUtils.ItemFor.BandwithReseller).AsQueryable();
                if (itemForFromDDL > 0)
                {
                    items = items.Where(x => x.ItemFor == itemForFromDDL).AsQueryable();
                }

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (items.Any()) ? items.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower()) || p.ItemName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    // Apply search   
                    items = items.Where(p => p.ItemID.ToString().ToLower().Contains(search.ToLower()) || p.ItemName.ToString().ToLower().Contains(search.ToLower())).AsQueryable();
                }

                //if (items.Any())
                //{
                //totalRecords = items.Count();
                finalItem = items.AsEnumerable().Skip(startRec).Take(pageSize)
                    .Select(
                        s => new
                        {
                            ItemID = s.ItemID,
                            ItemName = s.ItemName,
                            ItemFor = s.ItemFor.HasValue ? Enum.GetName(typeof(ItemFor), s.ItemFor.Value) : "",
                            UpdateStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Item) ? true : false
                        }).ToList();
                //}

                // Sorting.   
                finalItem = this.SortByColumnWithOrderForGeneralItem(order, orderDir, finalItem);
                // Total record count.   
                // totalRecords = secondpart.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : items.Count();

                ////////////////////////////////////


                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = finalItem
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }
        private IEnumerable<dynamic> SortByColumnWithOrderForBandwithResellerItem(string order, string orderDir, IEnumerable<dynamic> finalItem)
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemID).ToList() : finalItem.OrderBy(p => p.ItemID).ToList();
                        break;
                    //case "1":
                    //    // Setting.   
                    //    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemFor).ToList() : finalItem.OrderBy(p => p.ItemFor).ToList();
                    //    break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemName).ToList() : finalItem.OrderBy(p => p.ItemName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ItemID).ToList() : finalItem.OrderBy(p => p.ItemID).ToList();
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
        public ActionResult InsertBandwithResellerItemFromPopUp(Item Item_Client)
        {
            Item Item_Check = db.Item.Where(s => s.ItemName == Item_Client.ItemName.Trim()).FirstOrDefault();

            if (Item_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Item Already Added. Choose different Item. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Item Item_Return = new Item();

            try
            {
                Item_Client.ItemFor = (int)AppUtils.ItemFor.BandwithReseller;
                Item_Client.CreatedBy = AppUtils.GetLoginUserID().ToString()/*AppUtils.GetLoginEmployeeName()*/;
                Item_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Item_Return = db.Item.Add(Item_Client);
                db.SaveChanges();

                if (Item_Return.ItemID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Item = Item_Return, ItemFor = Enum.GetName(typeof(ItemFor), Item_Return.ItemFor.Value) }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetBandwithResellerItemDetailsByID(int ItemID)
        {
            var Item = db.Item.Where(s => s.ItemID == ItemID).Select(s => new { ItemName = s.ItemName, ItemFor = s.ItemFor }).FirstOrDefault();


            var JSON = Json(new { ItemDetails = Item }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateBandwithResellerItem(Item ItemInfoForUpdate)
        {
            try
            {
                Item Item_Check = db.Item.Where(s => s.ItemID != ItemInfoForUpdate.ItemID && s.ItemName == ItemInfoForUpdate.ItemName.Trim()).FirstOrDefault();

                if (Item_Check != null)
                {
                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Item_db = db.Item.Where(s => s.ItemID == ItemInfoForUpdate.ItemID);
                ItemInfoForUpdate.ItemFor = Item_db.FirstOrDefault().ItemFor;
                ItemInfoForUpdate.CreatedBy = Item_db.FirstOrDefault().CreatedBy;
                ItemInfoForUpdate.CreatedDate = Item_db.FirstOrDefault().CreatedDate;
                ItemInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                ItemInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Item_db.SingleOrDefault()).CurrentValues.SetValues(ItemInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Item_Return = Item_db.Select(s => new { ItemID = s.ItemID, PackageName = s.ItemName, ItemFor = s.ItemFor });
                var JSON = Json(new { UpdateSuccess = true, ItemUpdateInformation = Item_Return, ItemFor = Enum.GetName(typeof(ItemFor), Item_Return.FirstOrDefault().ItemFor) }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ItemUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}