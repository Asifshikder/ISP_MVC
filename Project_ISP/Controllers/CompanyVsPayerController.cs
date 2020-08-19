using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Project_ISP.Migrations;
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
    public class CompanyVsPayerController : Controller
    {
        // GET: CompanyVsPayer
        private ISPContext db = new ISPContext();
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_CompanyVsPayer)]
        public ActionResult Index()
        {
            ViewBag.Company = new SelectList(db.Company.Where(x => x.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            ViewBag.NewCompany = new SelectList(db.Company.Where(x => x.Status == AppUtils.TableStatusIsActive), "CompanyID", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllCompanyVsPayer()
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
                var companyPayer = db.CompanyVSPayer.Where(x => x.Status == AppUtils.TableStatusIsActive).AsQueryable();

                int ifSearch = 0;
                List<CompanyVsPayerViewModel> data = new List<CompanyVsPayerViewModel>();
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (companyPayer.Any()) ? companyPayer.Where(p => p.PayerID.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                                                                                  || p.PayerName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;


                    companyPayer = companyPayer.Where(p =>
                    p.PayerID.ToString().ToLower().Contains(search.ToLower())
                    || p.PayerName.ToString().ToLower().Contains(search.ToLower())
                    || p.Company.CompanyName.ToString().ToLower().Contains(search.ToLower())
                    ).AsQueryable();
                }
                data = companyPayer.Any() ? companyPayer.AsEnumerable().Skip(startRec).Take(pageSize)
                        .Select(
                            x => new CompanyVsPayerViewModel
                            {
                                PayerID = x.PayerID,
                                PayerName = x.PayerName,
                                CompanyName = x.Company.CompanyName,
                                UpdateCompanyVsPayer = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_CompanyVsPayer) ? true : false
                            })
                        .ToList() : new List<CompanyVsPayerViewModel>();

                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = companyPayer.AsEnumerable().Count();
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : companyPayer.AsEnumerable().Count();

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


        private List<CompanyVsPayerViewModel> SortByColumnWithOrder(string order, string orderDir, List<CompanyVsPayerViewModel> data)
        {
            // Initialization.   
            List<CompanyVsPayerViewModel> lst = new List<CompanyVsPayerViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PayerID).ToList() : data.OrderBy(p => p.PayerID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PayerName).ToList() : data.OrderBy(p => p.PayerName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompanyName).ToList() : data.OrderBy(p => p.CompanyName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PayerID).ToList() : data.OrderBy(p => p.PayerID).ToList();
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
        public JsonResult InsertCompanyVsPayer(CompanyVSPayer CompanyVsPayer)
        {
            try
            {
                db.CompanyVSPayer.Add(CompanyVsPayer);
                CompanyVsPayer.CreateBy = AppUtils.GetLoginUserID();
                CompanyVsPayer.CreateDate = AppUtils.GetDateTimeNow();
                CompanyVsPayer.Status = AppUtils.TableStatusIsActive;
                db.SaveChanges();

                return Json(new { success = true}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDetailsByID(int ID)
        {
            var company = db.CompanyVSPayer.Where(s => s.PayerID == ID).Select(s => new { PayerID = s.PayerID, PayerName = s.PayerName, CompanyID = s.CompanyID }).FirstOrDefault();


            var JSON = Json(new { company = company }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdatePayerInformationFormPopUp(CompanyVSPayer PayerInfo)
        {

            try
            {
                CompanyVSPayer dbPayer = new CompanyVSPayer();
                dbPayer = db.CompanyVSPayer.Find(PayerInfo.PayerID);
                dbPayer.PayerName = PayerInfo.PayerName;
                dbPayer.CompanyID = PayerInfo.CompanyID;
                dbPayer.UpdateBy = AppUtils.GetLoginUserID();
                dbPayer.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbPayer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var JSON = Json(new { success = true}, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePayer(int ID)
        {
            CompanyVSPayer payer = new CompanyVSPayer();
            payer = db.CompanyVSPayer.Find(ID);
            payer.DeleteBy = AppUtils.GetLoginUserID();
            payer.DeleteDate = AppUtils.GetDateTimeNow();
            payer.Status = AppUtils.TableStatusIsDelete;


            db.Entry(payer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }
    }
}