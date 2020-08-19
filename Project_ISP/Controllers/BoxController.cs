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

    [SessionTimeout][AjaxAuthorizeAttribute]
    public class BoxController : Controller
    {
        public BoxController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }

        private ISPContext db = new ISPContext();

        [UserRIghtCheck(ControllerValue = AppUtils.View_Box_List)]
        public ActionResult Index()
        {
            string macResellerType = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString())); ;
            var lstReseller = db.Reseller.Where(x => x.ResellerTypeListID == macResellerType).Select(x => new { x.ResellerID, x.ResellerLoginName });
            ViewBag.ddlReseller = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            ViewBag.ddlUpdateReseller = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            ViewBag.SearchByResellerID = new SelectList(lstReseller, "ResellerID", "ResellerLoginName");
            //List<Box> lstBox = db.Box.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetBoxAJAXData()
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

                IEnumerable<Box> lstBox = Enumerable.Empty<Box>();
                IEnumerable<dynamic> finalItem = Enumerable.Empty<dynamic>();
                lstBox = db.Box.AsQueryable();


                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int loginResellerID = AppUtils.GetLoginUserID();
                    lstBox = db.Box.Where(x => x.ResellerID == loginResellerID).AsQueryable();
                }
                else if (AppUtils.GetLoginRoleID() != AppUtils.ResellerRole && SearchByResellerID > 0)
                // mean data is loaded already and now admin is searching by reseller id for resller Box list
                {
                    lstBox = db.Box.Where(x => x.ResellerID == SearchByResellerID).AsQueryable();
                }
                else
                {
                    lstBox = db.Box.Where(x => x.ResellerID == null).AsQueryable();
                }

                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    ifSearch = (lstBox.Any()) ? lstBox.Where(p => p.BoxName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;
                    // Apply search   
                    lstBox = lstBox.Where(p => p.BoxName.ToString().ToLower().Contains(search.ToLower())).AsQueryable();
                }

                if (lstBox.Any())
                {
                    totalRecords = lstBox.Count();
                    finalItem = lstBox.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            s => new
                            {
                                BoxID = s.BoxID,
                                BoxName = s.BoxName,
                                BoxLocation = s.BoxLocation,
                                LatitudeLongitude = s.LatitudeLongitude,
                                UpdateStatus = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Box) ? true : false
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.BoxID).ToList() : finalItem.OrderBy(p => p.BoxID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.BoxName).ToList() : finalItem.OrderBy(p => p.BoxName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.BoxLocation).ToList() : finalItem.OrderBy(p => p.BoxName).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? finalItem.OrderByDescending(p => p.BoxID).ToList() : finalItem.OrderBy(p => p.BoxID).ToList();
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

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Box)]
        public ActionResult InsertBox()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertBox(Box Box_Client)
        {
            Box Box_Check = db.Box.Where(s => s.BoxName == Box_Client.BoxName.Trim()).FirstOrDefault();

            if (Box_Check != null)
            {
                TempData["AlreadyInsert"] = "Box Already Added. Choose different Box. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Box Box_Return = new Box();

            try
            {
                Box_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Box_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Box_Return = db.Box.Add(Box_Client);
                db.SaveChanges();

                if (Box_Return.BoxID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Box = Box_Return }, JsonRequestBehavior.AllowGet);
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
        public ActionResult InsertBoxFromPopUp(Box Box_Client)
        {
            Box Box_Check = db.Box.Where(s => s.BoxName == Box_Client.BoxName.Trim()).FirstOrDefault();

            if (Box_Check != null)
            {
                //  TempData["AlreadyInsert"] = "Box Already Added. Choose different Box. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            Box Box_Return = new Box();

            try
            {
                Box_Client.CreatedBy = AppUtils.GetLoginEmployeeName();
                Box_Client.CreatedDate = AppUtils.GetDateTimeNow();

                Box_Return = db.Box.Add(Box_Client);
                db.SaveChanges();

                if (Box_Return.BoxID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, Box = Box_Return }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetBoxDetailsByID(int BoxID)
        {
            var Box = db.Box.Where(s => s.BoxID == BoxID).Select(s => new { BoxName = s.BoxName, BoxLocations = s.BoxLocation, ResellerID = s.ResellerID, LatitudeLongitude = s.LatitudeLongitude }).FirstOrDefault();


            var JSON = Json(new { BoxDetails = Box }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateBox(Box BoxInfoForUpdate)
        {

            try
            {

                Box Box_Check = db.Box.Where(s => s.BoxID != BoxInfoForUpdate.BoxID && s.BoxName == BoxInfoForUpdate.BoxName.Trim()).FirstOrDefault();

                if (Box_Check != null)
                {
                    //TempData["AlreadyInsert"] = "Box Already Added. Choose different Box. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var Box_db = db.Box.Where(s => s.BoxID == BoxInfoForUpdate.BoxID);
                BoxInfoForUpdate.CreatedBy = Box_db.FirstOrDefault().CreatedBy;
                BoxInfoForUpdate.CreatedDate = Box_db.FirstOrDefault().CreatedDate;
                BoxInfoForUpdate.UpdateBy = AppUtils.GetLoginEmployeeName();
                BoxInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Box_db.SingleOrDefault()).CurrentValues.SetValues(BoxInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var Box_Return = Box_db.Select(s => new { BoxID = s.BoxID, PackageName = s.BoxName, BoxLocation = s.BoxLocation });
                var JSON = Json(new { UpdateSuccess = true, BoxUpdateInformation = Box_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, BoxUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}