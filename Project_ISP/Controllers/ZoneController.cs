using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using Project_ISP;
using static ISP_ManagementSystemModel.AppUtils;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ZoneController : Controller
    {
        public ZoneController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [UserRIghtCheck(ControllerValue = AppUtils.View_Zone_List)]
        public ActionResult Index()
        {
            string macResellerType = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString())); ;
            var lstReseller = db.Reseller.Where(x => x.ResellerTypeListID == macResellerType).Select(x => new { x.ResellerID, x.ResellerLoginName });
            ViewBag.ddlCreateReseller = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            ViewBag.ddlUpdateReseller = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            ViewBag.SearchByResellerID = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetZoneAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                // Initialization.  

                int SearchByResellerID = 0;
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

                var ResellerID = Request.Form.Get("ResellerID");

                if (!string.IsNullOrEmpty(ResellerID))
                {
                    SearchByResellerID = int.Parse(ResellerID);
                }

                IEnumerable<Zone> lstZone = Enumerable.Empty<Zone>();
                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                lstZone = db.Zone.AsQueryable();


                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int loginResellerID = AppUtils.GetLoginUserID();
                    lstZone = db.Zone.Where(x => x.ResellerID == loginResellerID).AsQueryable();
                }
                else if (AppUtils.GetLoginRoleID() != AppUtils.ResellerRole && SearchByResellerID > 0)
                // mean data is loaded already and now admin is searching by reseller id for resller zone list
                {
                    lstZone = db.Zone.Where(x => x.ResellerID == SearchByResellerID).AsQueryable();
                }
                else
                {
                    lstZone = db.Zone.Where(x => x.ResellerID == null).AsQueryable();
                }

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (lstZone.Any()) ? lstZone.Where(p => p.ZoneName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    // Apply search   
                    lstZone = lstZone.Where(p => p.ZoneName.ToString().ToLower().Contains(search.ToLower())).AsQueryable();
                }

                if (lstZone.Any())
                {
                    totalRecords = lstZone.Count();
                    finalItem = lstZone.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            s => new
                            {
                                ZoneID = s.ZoneID,
                                ZoneName = s.ZoneName,
                                UpdateStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Zone) ? true : false
                            }).ToList();

                }

                // Sorting.   
                finalItem = this.SortByColumnWithOrder(order, orderDir, finalItem);
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
                    data = finalItem
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ZoneID).ToList() : finalItem.OrderBy(p => p.ZoneID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ZoneName).ToList() : finalItem.OrderBy(p => p.ZoneName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.ZoneID).ToList() : finalItem.OrderBy(p => p.ZoneID).ToList();
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
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Zone)]
        public ActionResult InsertZone()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertZone(Zone Zone_Client)
        {
            Zone Zone_Check = db.Zone.Where(s => s.ZoneName == Zone_Client.ZoneName.Trim()).FirstOrDefault();

            if (Zone_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Zone Already Added. Choose different Zone. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Zone Zone_Return = new Zone();

            try
            {
                Zone_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Zone_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Zone_Return = db.Zone.Add(Zone_Client);
                db.SaveChanges();

                if (Zone_Return.ZoneID > 0)
                {
                    //   TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Zone = Zone_Return }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //     TempData["SaveSucessOrFail"] = "Save Failed.";
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertZoneFromPopUp(Zone Zone_Client)
        {
            Zone zone_Check = new Zone();
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                int resellerID = AppUtils.GetLoginUserID();
                zone_Check = db.Zone.Where(s => s.ZoneName.ToLower() == Zone_Client.ZoneName.Trim().ToLower() && s.ResellerID == resellerID).FirstOrDefault();
                Zone_Client.ResellerID = resellerID;
            }
            else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole && Zone_Client.ResellerID.HasValue)
            { 
                zone_Check = db.Zone.Where(s => s.ZoneName.ToLower() == Zone_Client.ZoneName.Trim().ToLower() && s.ResellerID == Zone_Client.ResellerID).FirstOrDefault();
            }
            else
            {
                zone_Check = db.Zone.Where(s => s.ZoneName.ToLower() == Zone_Client.ZoneName.Trim().ToLower() && s.ResellerID == Zone_Client.ResellerID).FirstOrDefault();
            } 

            if (zone_Check != null)
            { 
                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Zone Zone_Return = new Zone();

            try
            {
                Zone_Client.CreatedBy = AppUtils.GetLoginUserID().ToString();
                Zone_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Zone_Return = db.Zone.Add(Zone_Client);
                db.SaveChanges();

                if (Zone_Return.ZoneID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Zone = Zone_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetZoneDetailsByID(int ZoneID)
        {
            var Zone = db.Zone.Where(s => s.ZoneID == ZoneID).Select(s => new { ZoneName = s.ZoneName, ResellerID = s.ResellerID }).FirstOrDefault();


            var JSON = Json(new { ZoneDetails = Zone }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateZone(Zone ZoneInfoForUpdate)
        {

            try
            {
                Zone band_Check = new Zone();
                if (AppUtils.GetLoginUserID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                    band_Check = db.Zone.Where(s => s.ZoneID != ZoneInfoForUpdate.ZoneID && s.ResellerID == resellerID && s.ZoneName.ToLower() == ZoneInfoForUpdate.ZoneName.Trim().ToLower()).FirstOrDefault();
                    ZoneInfoForUpdate.ResellerID = resellerID;
                }
                else if (AppUtils.GetLoginRoleID() == AppUtils.AdminRole && ZoneInfoForUpdate.ResellerID.HasValue)
                {
                    ZoneInfoForUpdate = db.Zone.Where(s => s.ZoneID != ZoneInfoForUpdate.ZoneID && s.ResellerID == ZoneInfoForUpdate.ResellerID && s.ZoneName.ToLower() == ZoneInfoForUpdate.ZoneName.Trim().ToLower()).FirstOrDefault();
                }
                else
                {
                    band_Check = db.Zone.Where(s => s.ZoneID != ZoneInfoForUpdate.ZoneID && s.ResellerID == ZoneInfoForUpdate.ResellerID && s.ZoneName.ToLower() == ZoneInfoForUpdate.ZoneName.Trim().ToLower()).FirstOrDefault();
                }


                if (band_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Zone Already Added. Choose different Zone. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Zone_db = db.Zone.Where(s => s.ZoneID == ZoneInfoForUpdate.ZoneID);
                ZoneInfoForUpdate.CreatedBy = Zone_db.FirstOrDefault().CreatedBy;
                ZoneInfoForUpdate.CreatedDate = Zone_db.FirstOrDefault().CreatedDate;
                ZoneInfoForUpdate.UpdateBy = AppUtils.GetLoginUserID().ToString();
                ZoneInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Zone_db.SingleOrDefault()).CurrentValues.SetValues(ZoneInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Zones = Zone_db.Select(s => new { ZoneID = s.ZoneID, PackageName = s.ZoneName });
                var JSON = Json(new { UpdateSuccess = true, ZoneUpdateInformation = Zones }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, ZoneUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}