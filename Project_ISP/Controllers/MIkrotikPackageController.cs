using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using Project_ISP;
using tik4net;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]

    public class MIkrotikPackageController : Controller
    {
        ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
        public MIkrotikPackageController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        // GET: /Package/

        [UserRIghtCheck(ControllerValue = AppUtils.VIew_Package_List)]
        public ActionResult Index()
        {

            //{
            //    "data": "PackageID"
            //},
            //{
            //    "data": "PackageName"
            //},
            //{
            //    "data": "BandWith"
            //},
            //{
            //    "data": "PackagePrice"
            //},
            //{
            //    "data": "Client"
            //},
            //{
            //    "data": ""
            //}

            List<SelectListItem> lstSelectListItem = new List<SelectListItem>();
            lstSelectListItem.Add(new SelectListItem() { Text = "Package For My User", Value = AppUtils.PackageForMyUser });
            lstSelectListItem.Add(new SelectListItem() { Text = "Package For My Reseller", Value = AppUtils.PackageForResellerUser });
            ViewBag.ddlCreatePackageFor = new SelectList(lstSelectListItem, "Value", "Text");
            ViewBag.ddlUpdatePackageFor = new SelectList(lstSelectListItem, "Value", "Text");

            ViewBag.CreateIPPoolID = new SelectList(db.IPPool.Select(s => new { s.IPPoolID, s.PoolName }), "IPPoolID", "PoolName");
            ViewBag.CreateMikrotikID = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.IPPoolIDs = new SelectList(db.IPPool.Select(s => new { s.IPPoolID, s.PoolName }), "IPPoolID", "PoolName");
            ViewBag.MikrotikIDs = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            VM_Package_ClientDetails VM_Package_ClientDetails = new VM_Package_ClientDetails();
            //   VM_Package_ClientDetails.lstClientDetails = db.ClientDetails.ToList();
            // VM_Package_ClientDetails.lstPackage = db.Package.ToList();

            return View(VM_Package_ClientDetails);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllPackageAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                int SearchByPackageTypID = 0;
                //int zoneFromDDL = 0;
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                var PackageType = Request.Form.Get("PackageType");
                if (!string.IsNullOrEmpty(PackageType))
                {
                    SearchByPackageTypID = int.Parse(PackageType);
                }
                // Loading.   

                // Apply pagination.   
                //data = data.Skip(startRec).Take(pageSize).ToList();
                int packageForMyUser = int.Parse(AppUtils.PackageForMyUser);
                int packageForResellerUser = int.Parse(AppUtils.PackageForResellerUser);
                var package = (SearchByPackageTypID == packageForMyUser) ? db.Package.Where(x => x.PackageForMyOrResellerUser == packageForMyUser).AsEnumerable() : db.Package.Where(x => x.PackageForMyOrResellerUser == packageForResellerUser).AsEnumerable();


                int ifSearch = 0;
                List<CustomPackage> data =
                    package.Any() ? package.Skip(startRec).Take(pageSize).AsEnumerable()
                        //.Select new {ss=>ss. }
                        .Select(
                            s => new CustomPackage
                            {
                                PackageID = s.PackageID,
                                PackageName = s.PackageName,
                                BandWith = s.BandWith,
                                PackagePrice = s.PackagePrice.ToString(),
                                Client = db.ClientDetails.Where(ss => ss.PackageID == s.PackageID).Count(),
                                IPPoolName = (s.IPPoolID != null) ? s.IpPool.PoolName : "",
                                LocalAddress = s.LocalAddress,
                                MikrotikName = s.Mikrotik != null ? s.Mikrotik.MikName: "",
                                PackageUpdate = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Package) ? true : false,
                            })
                        .ToList() : new List<CustomPackage>();
                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (data.Any()) ? data.Where(p => p.PackageID.ToString().ToLower().Contains(search.ToLower()) || p.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                p.BandWith.ToString().ToLower().Contains(search.ToLower()) || p.PackagePrice.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    data = data.Where(p => p.PackageID.ToString().ToLower().Contains(search.ToLower()) || p.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.BandWith.ToString().ToLower().Contains(search.ToLower()) || p.PackagePrice.ToString().ToLower().Contains(search.ToLower())
                    ).ToList();
                }
                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = package.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : package.AsEnumerable().Count();

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


        private List<CustomPackage> SortByColumnWithOrder(string order, string orderDir, List<CustomPackage> data)
        {
            // Initialization.   
            List<CustomPackage> lst = new List<CustomPackage>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageID).ToList() : data.OrderBy(p => p.PackageID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.BandWith).ToList() : data.OrderBy(p => p.BandWith).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackagePrice).ToList() : data.OrderBy(p => p.PackagePrice).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageID).ToList() : data.OrderBy(p => p.PackageID).ToList();
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
        public ActionResult GetPackageDetailsByID(int packageID)
        {
            var Package = db.Package.Where(s => s.PackageID == packageID).Select(s => new { PackageForMyOrResellerUser = s.PackageForMyOrResellerUser, PackageName = s.PackageName, PackagePrice = s.PackagePrice, BandWith = s.BandWith, IPPoolID = s.IPPoolID, MikrotikID = s.MikrotikID, LocalAddress = s.LocalAddress }).FirstOrDefault();
            //    $("#PackageName").val(PackageJSONParse.PackageName);
            //$("#PackagePrice").val(PackageJSONParse.PackagePrice);
            //$("#BandWith").val(PackageJSONParse.BandWith);
            // var PackageCircularLoopIgnored = AppUtils.IgnoreCircularLoop(Package);

            var JSON = Json(new { PackageDetails = Package }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatePackage(Package PackageInfoForUpdate)
        {

            Package package_Check = db.Package.Where(s => s.PackageID != PackageInfoForUpdate.PackageID && s.PackageName.ToLower() == PackageInfoForUpdate.PackageName.Trim().ToLower() && s.PackageForMyOrResellerUser == PackageInfoForUpdate.PackageForMyOrResellerUser).FirstOrDefault();
            if (package_Check != null)
            {
                return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            if ((bool)Session["MikrotikOptionEnable"])
            {
                Mikrotik mikrotik = db.Mikrotik.Find(PackageInfoForUpdate.MikrotikID);
                var ipPoolName = db.IPPool.Find(PackageInfoForUpdate.IPPoolID).PoolName;
                var oldPackageInfoForUpdate = db.Package.Find(PackageInfoForUpdate.PackageID);

                PackageInfoForUpdate.OldPackageName = oldPackageInfoForUpdate.PackageName;
                try
                {//ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
                    ITikConnection connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
                    connection.CreateCommandAndParameters("/ppp/profile/set",".id", oldPackageInfoForUpdate.PackageName, "name", PackageInfoForUpdate.PackageName.Trim(), "local-address", PackageInfoForUpdate.LocalAddress, "remote-address", ipPoolName, "incoming-filter", "mypppclients").ExecuteNonQuery();

                    //add profile
                    //var profileadd = connection.CreateCommandAndParameters("ppp/profile/add", "name", "test1", "local-address", "10.0.0.1", "remote-address", "sp", "incoming-filter", "mypppclients");
                    //profileadd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            try
            {
                var Package = db.Package.Where(s => s.PackageID == PackageInfoForUpdate.PackageID);
                PackageInfoForUpdate.CreatedBy = Package.FirstOrDefault().CreatedBy;
                PackageInfoForUpdate.CreatedDate = Package.FirstOrDefault().CreatedDate;
                PackageInfoForUpdate.UpdateBy = "Hasan";
                PackageInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(Package.SingleOrDefault()).CurrentValues.SetValues(PackageInfoForUpdate);
                db.SaveChanges();

                var package =
                  new CustomPackage()
                  {
                      PackageID = PackageInfoForUpdate.PackageID,
                      PackageName = PackageInfoForUpdate.PackageName,
                      BandWith = PackageInfoForUpdate.BandWith,
                      PackagePrice = PackageInfoForUpdate.PackagePrice.ToString(),
                      Client = db.ClientDetails.Where(ss => ss.PackageID == PackageInfoForUpdate.PackageID).Count(),
                      IPPoolName = (PackageInfoForUpdate.IPPoolID != null) ? db.IPPool.Find(PackageInfoForUpdate.IPPoolID).PoolName : "",
                      LocalAddress = PackageInfoForUpdate.LocalAddress,
                      MikrotikName = db.Mikrotik.Find(PackageInfoForUpdate.MikrotikID).MikName,
                      PackageUpdate = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Package) ? true : false,
                  };
                var JSON = Json(new { UpdateSuccess = true, PackageUpdateInformation = package }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, PackageUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }



        [UserRIghtCheck(ControllerValue = AppUtils.Create_Package)]
        public ActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public JsonResult InsertPackage(Package Package)
        {

            Package package_Check = package_Check = db.Package.Where(s => s.PackageName.ToLower() == Package.PackageName.Trim().ToLower() && s.PackageForMyOrResellerUser == Package.PackageForMyOrResellerUser).FirstOrDefault();

            if (package_Check != null)
            {
                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            if ((bool)Session["MikrotikOptionEnable"])
            {
                Mikrotik mikrotik = db.Mikrotik.Find(Package.MikrotikID);
                var ipPoolName = db.IPPool.Find(Package.IPPoolID).PoolName;

                try
                {//ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
                ITikConnection connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
                    connection.CreateCommandAndParameters("/ppp/profile/add", "name", Package.PackageName.Trim(), "local-address", Package.LocalAddress, "remote-address", ipPoolName, "incoming-filter", "mypppclients").ExecuteNonQuery();

                    //add profile
                    //var profileadd = connection.CreateCommandAndParameters("ppp/profile/add", "name", "test1", "local-address", "10.0.0.1", "remote-address", "sp", "incoming-filter", "mypppclients");
                    //profileadd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                
            }
            int packageCount = db.Package.Count();

            try
            {
                Package.CreatedBy = AppUtils.GetLoginEmployeeName();
                Package.CreatedDate = AppUtils.GetDateTimeNow();

                db.Package.Add(Package);
                db.SaveChanges();
                var PoolName = db.IPPool.Where(s => s.IPPoolID == Package.IPPoolID).FirstOrDefault().PoolName;
                //var packageInsertInformation = AppUtils.IgnoreCircularLoop(PackageInfo);

                CustomPackage PackageInfo = new CustomPackage
                {
                    PackageID = Package.PackageID,
                    PackageName = Package.PackageName,
                    BandWith = Package.BandWith,
                    PackagePrice = Package.PackagePrice.ToString(),
                    Client = db.ClientDetails.Where(ss => ss.PackageID == Package.PackageID).Count(),
                    IPPoolName = (Package.IPPoolID != null) ? Package.IpPool.PoolName : "",
                    LocalAddress = Package.LocalAddress,
                    MikrotikName = db.Mikrotik.Find(Package.MikrotikID).MikName,
                    PackageUpdate = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Package) ? true : false,
                };


                return Json(new { SuccessInsert = true, PackageInformation = PackageInfo, packageCount = packageCount }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }



            //return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
