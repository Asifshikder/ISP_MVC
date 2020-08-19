using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Custom_Model;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class VendorTypeController : Controller
    {

        
        // GET: VendorType
        private ISPContext db = new ISPContext();
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_vendor)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllVendorType()
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
                var vendorType = db.VendorTypes.Where(x=>x.Status==AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<VendorTypeViewModel> data = new List<VendorTypeViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (vendorType.Any()) ? vendorType.Where(p => p.VendorTypeID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorTypeID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorTypeName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.VendorTypeName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    
                    vendorType = vendorType.Where(p =>
                    p.VendorTypeID.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorTypeID.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorTypeName.ToString().ToLower().Contains(search.ToLower())
                    || p.VendorTypeName.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = vendorType.Any() ? vendorType.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new VendorTypeViewModel
                            {
                                VendorTypeID = x.VendorTypeID,
                                VendorTypeName = x.VendorTypeName,
                                UpdateVendorType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Vendor_Type) ? true : false
                            })
                        .ToList() : new List<VendorTypeViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = vendorType.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : vendorType.AsEnumerable().Count();

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


        private List<VendorTypeViewModel> SortByColumnWithOrder(string order, string orderDir, List<VendorTypeViewModel> data)
        {
            // Initialization.   
            List<VendorTypeViewModel> lst = new List<VendorTypeViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorTypeID).ToList() : data.OrderBy(p => p.VendorTypeID).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VendorTypeName).ToList() : data.OrderBy(p => p.VendorTypeName).ToList();
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
        public JsonResult InsertVendorType(VendorType vendorType)
        {
            try
            {
                db.VendorTypes.Add(vendorType);
                vendorType.CreateBy = AppUtils.GetLoginUserID();
                vendorType.CreateDate = AppUtils.GetDateTimeNow();
                vendorType.Status = AppUtils.TableStatusIsActive;
                db.SaveChanges();
                VendorTypeViewModel vendorTypeView = new VendorTypeViewModel
                {
                    VendorTypeID = vendorType.VendorTypeID,
                    VendorTypeName = vendorType.VendorTypeName,
                    UpdateVendorType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Vendor_Type) ? true : false,
                };


                return Json(new { SuccessInsert = true, vendorTypeView = vendorTypeView }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetVendorTypeID(int VendorTypeID)
        {
            var vendorType = db.VendorTypes.Where(s => s.VendorTypeID == VendorTypeID).Select(s => new { VendorTypeID = s.VendorTypeID, VendorTypeName = s.VendorTypeName }).FirstOrDefault();


            var JSON = Json(new { vendorType = vendorType }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateVendorType(VendorType VendorType)
        {

            try
            {
                VendorType dbVendorType = new VendorType();
                dbVendorType = db.VendorTypes.Find(VendorType.VendorTypeID);
                dbVendorType.VendorTypeName = VendorType.VendorTypeName;
                dbVendorType.UpdateBy = AppUtils.GetLoginUserID();
                dbVendorType.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbVendorType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var vendor =
                    new VendorTypeViewModel()
                    {
                        VendorTypeID = VendorType.VendorTypeID,
                        VendorTypeName = VendorType.VendorTypeName,
                        UpdateVendorType = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Vendor_Type) ? true : false,
                    };
                var JSON = Json(new { UpdateSuccess = true, vendor = vendor }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch(Exception ex)
            {
                return Json(new { UpdateSuccess = false}, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVendorType(int VendorTypeID)
        {
            VendorType vendor = new VendorType();
            vendor = db.VendorTypes.Find(VendorTypeID);
            vendor.DeleteBy = AppUtils.GetLoginUserID();
            vendor.DeleteDate = AppUtils.GetDateTimeNow();
            vendor.Status = AppUtils.TableStatusIsDelete;


            db.Entry(vendor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { DeleteSuccess = true, VendorTypeID = vendor.VendorTypeID }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }
    }
}