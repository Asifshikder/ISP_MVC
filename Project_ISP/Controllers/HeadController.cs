using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using Project_ISP.Models;
using Project_ISP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ISP_ManagementSystemModel.AppUtils;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class HeadController : Controller
    {
        private ISPContext db = new ISPContext();
        // GET: Head
        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.View_Head)]
        public ActionResult Index()
        {
            List<SelectListItem> HeadType = new List<SelectListItem>();
            HeadType.Add(new SelectListItem() { Text = "Expense", Value = "1" });
            HeadType.Add(new SelectListItem() { Text = "Income", Value = "2" });
            ViewBag.HeadTypeInsert = new SelectList(HeadType, "Value", "Text");
            ViewBag.EditHeadTypeID = new SelectList(HeadType, "Value", "Text");
            ViewBag.SearchHeadTypeID = new SelectList(HeadType, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllHeadAJAXData()
        {
            int ExpenseTypeID = (int)Enum.Parse(typeof(HeadType), HeadType.Expense.ToString());
            JsonResult result = new JsonResult();
            try
            {
                int HeadType = 0;
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var HeadTypeID = Request.Form.Get("HeadTypeIDS");
                int totalRecords = 0;

                if (!string.IsNullOrEmpty(HeadTypeID))
                {
                    HeadType = int.Parse(HeadTypeID);
                }


                var firstPartOfQuery = db.Head.Where(a => a.Status == AppUtils.TableStatusIsActive).AsQueryable();
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                    firstPartOfQuery = firstPartOfQuery.Where(x => x.ResellerID == resellerID).AsQueryable();
                }
                else
                {
                    firstPartOfQuery = firstPartOfQuery.Where(x => x.ResellerID == null).AsQueryable();
                }

                int ifSearch = 0;
                List<HeadViewModel> data = new List<HeadViewModel>();
                firstPartOfQuery =
                    !string.IsNullOrEmpty(HeadTypeID) ? firstPartOfQuery.Where(s => s.HeadTypeID == HeadType).AsQueryable()
                                        : firstPartOfQuery.Where(a => a.HeadTypeID == ExpenseTypeID).AsQueryable();

                var secondPartOfQuery = firstPartOfQuery.AsEnumerable();
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (secondPartOfQuery.Any()) ? secondPartOfQuery.Where(p => p.HeadeName.ToString().ToLower().Contains(search.ToLower()) || p.HeadTypeID.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    secondPartOfQuery = secondPartOfQuery.Where(p => p.HeadID.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.HeadeName.ToString().ToLower().Contains(search.ToLower())
                                                                     || p.HeadTypeID.ToString().ToLower().Contains(search.ToLower())).AsEnumerable();
                } 
                if (secondPartOfQuery.Count() > 0)
                {
                    totalRecords = secondPartOfQuery.Count();
                    data = secondPartOfQuery.Skip(startRec).Take(pageSize).Select(

                            s => new HeadViewModel
                            {
                                HeadID = s.HeadID,
                                HeadName = s.HeadeName,
                                HeadTypeId = s.HeadTypeID,
                                UpdateHead = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Head) ? true : false,

                            })
                        .ToList();

                }

                data = this.SortByColumnWithOrder(order, orderDir, data);
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : firstPartOfQuery.AsEnumerable().Count();

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
                Console.Write(ex);
            }
            return result;
        }
        private List<HeadViewModel> SortByColumnWithOrder(string order, string orderDir, List<HeadViewModel> data)
        {
            // Initialization.   
            List<HeadViewModel> lst = new List<HeadViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.HeadID).ToList() : data.OrderBy(p => p.HeadID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.HeadName).ToList() : data.OrderBy(p => p.HeadName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.HeadTypeId).ToList() : data.OrderBy(p => p.HeadTypeId).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.HeadID).ToList() : data.OrderBy(p => p.HeadID).ToList();
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

        public ActionResult InsertHeadFromPopUp(Head Head)
        {
            try
            {
                int countHead = 0;
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                    countHead = db.Head.Where(x => x.ResellerID == resellerID  && x.Status != AppUtils.TableStatusIsDelete && x.HeadeName.ToLower() == Head.HeadeName).Count();
                }
                else
                {
                    countHead = db.Head.Where(x => x.ResellerID == null && x.Status != AppUtils.TableStatusIsDelete && x.HeadeName.ToLower() == Head.HeadeName).Count();
                }
                if (countHead > 0)
                {
                    return Json(new { success = false, message = "Sorry Head Name Alraeady Exist." }, JsonRequestBehavior.AllowGet);
                }
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    Head.ResellerID = AppUtils.GetLoginUserID();
                }
                db.Head.Add(Head);
                Head.CreateBy = AppUtils.GetLoginUserID();
                Head.CreateDate = AppUtils.GetDateTimeNow();
                Head.Status = AppUtils.TableStatusIsActive;
                db.SaveChanges();
                HeadViewModel HeadInfo = new HeadViewModel
                {
                    HeadID = Head.HeadID,
                    HeadName = Head.HeadeName,
                    HeadTypeId = Head.HeadTypeID,
                    UpdateHead = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Head) ? true : false,
                };


                return Json(new { SuccessInsert = true, HeadInfo = HeadInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetHeadDetailsByID(int HeadID)
        {
            var Headinfo = db.Head.Where(s => s.HeadID == HeadID).Select(s => new { HeadID = s.HeadID, HeadeName = s.HeadeName, HeadTypeID = s.HeadTypeID }).FirstOrDefault();


            var JSON = Json(new { HeadInfo = Headinfo }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryTokenAttribute]
        public ActionResult UpdateHead(Head Head)
        {

            try
            {
                int countHead = 0;
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    int resellerID = AppUtils.GetLoginUserID();
                     countHead = db.Head.Where(x => x.ResellerID == resellerID && x.HeadID != Head.HeadID && x.Status != AppUtils.TableStatusIsDelete && x.HeadeName.ToLower() == Head.HeadeName).Count();   
                }
                else
                { 
                     countHead = db.Head.Where(x => x.ResellerID == null && x.HeadID != Head.HeadID && x.Status != AppUtils.TableStatusIsDelete && x.HeadeName.ToLower() == Head.HeadeName).Count();
                }
                if (countHead > 0)
                {
                    return Json(new { success = false, message = "Sorry Head Name Alraeady Exist." }, JsonRequestBehavior.AllowGet);
                }
                Head dbHead = new Head();
                dbHead = db.Head.Find(Head.HeadID);
                dbHead.HeadeName = Head.HeadeName;
                dbHead.HeadTypeID = Head.HeadTypeID;
                dbHead.UpdateBy = AppUtils.GetLoginUserID();
                dbHead.UpdateDate = AppUtils.GetDateTimeNow();
                db.Entry(dbHead).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var HeadInfo =
                    new HeadViewModel()
                    {
                        HeadID = Head.HeadID,
                        HeadName = Head.HeadeName,
                        HeadTypeId = Head.HeadTypeID,
                        UpdateHead = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Head) ? true : false,
                    };
                var JSON = Json(new { success = true, HeadInfo = HeadInfo }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteHead(int HeadID)
        {
            Head head = new Head();
            head = db.Head.Find(HeadID);
            head.DeleteBy = AppUtils.GetLoginUserID();
            head.DeleteDate = AppUtils.GetDateTimeNow();
            head.Status = AppUtils.TableStatusIsDelete;


            db.Entry(head).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var JSON = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

    }
}