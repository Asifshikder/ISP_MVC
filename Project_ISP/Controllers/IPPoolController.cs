using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace Project_ISP.Controllers
{
    [SessionTimeout][AjaxAuthorizeAttribute]
    public class IPPoolController : Controller
    {
        //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        //public class ValidateJsonAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
        //{
        //    public void OnAuthorization(AuthorizationContext filterContext)
        //    {
        //        if (filterContext == null)
        //        {
        //            throw new ArgumentNullException("filterContext");
        //        }

        //        var httpContext = filterContext.HttpContext;
        //        var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
        //        AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
        //    }
        //}

        public IPPoolController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();


        [UserRIghtCheck(ControllerValue = AppUtils.View_IPPool_List)]
        public ActionResult Index()
        {
            List<IPPool> lstIPPool = db.IPPool.ToList();
            return View(lstIPPool);
        }

        [HttpGet]

        [UserRIghtCheck(ControllerValue = AppUtils.Add_IPPool)]
        public ActionResult InsertIPPool()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InsertIPPool(IPPool IPPool_Client)
        {
            IPPool IPPool_Check = db.IPPool.Where(s => s.PoolName == IPPool_Client.PoolName.Trim()).FirstOrDefault();

            if (IPPool_Check != null)
            {
                TempData["AlreadyInsert"] = "IPPool Already Added. Choose different IPPool. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            IPPool IPPool_Return = new IPPool();

            try
            {
                IPPool_Client.CreatedBy = int.Parse(Session["LoggedUserID"].ToString());//AppUtils.LoginUserID;
                IPPool_Client.CreatedDate = AppUtils.GetDateTimeNow();

                IPPool_Return = db.IPPool.Add(IPPool_Client);
                db.SaveChanges();

                if (IPPool_Return.IPPoolID > 0)
                {
                    TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, IPPool = IPPool_Return }, JsonRequestBehavior.AllowGet);
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
        [ValidateJsonAntiForgeryToken]
        public ActionResult InsertIPPoolFromPopUp(IPPool IPPool_Client)
        {
            int IPPoolCount = db.IPPool.Count();
            IPPool IPPool_Check = db.IPPool.Where(s => s.PoolName == IPPool_Client.PoolName.Trim()).FirstOrDefault();

            if (IPPool_Check != null)
            {
                //  TempData["AlreadyInsert"] = "IPPool Already Added. Choose different IPPool. ";

                return Json(new { SuccessInsert = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
            }

            IPPool IPPool_Return = new IPPool();

            try
            {
                IPPool_Client.CreatedBy = int.Parse(Session["LoggedUserID"].ToString());//AppUtils.LoginUserID;
                IPPool_Client.CreatedDate = AppUtils.GetDateTimeNow();

                IPPool_Return = db.IPPool.Add(IPPool_Client);
                db.SaveChanges();

                if (IPPool_Return.IPPoolID > 0)
                {
                    //  TempData["SaveSucessOrFail"] = "Save Successfully.";
                    return Json(new { SuccessInsert = true, IPPool = IPPool_Return, IPPoolCount = IPPoolCount }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetIPPoolDetailsByID(int IPPoolID)
        {
            var IPPool = db.IPPool.Where(s => s.IPPoolID == IPPoolID).FirstOrDefault();


            var JSON = Json(new { IPPoolDetails = IPPool }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult UpdateIPPool(IPPool IPPoolInfoForUpdate)
        {

            try
            {

                IPPool IPPool_Check = db.IPPool.Where(s => s.IPPoolID != IPPoolInfoForUpdate.IPPoolID && s.PoolName == IPPoolInfoForUpdate.PoolName.Trim()).FirstOrDefault();

                if (IPPool_Check != null)
                {
                    //TempData["AlreadyInsert"] = "IPPool Already Added. Choose different IPPool. ";

                    return Json(new { UpdateSuccess = false, AlreadyInsert = true }, JsonRequestBehavior.AllowGet);
                }

                var IPPool_db = db.IPPool.Where(s => s.IPPoolID == IPPoolInfoForUpdate.IPPoolID);
                IPPoolInfoForUpdate.CreatedBy = IPPool_db.FirstOrDefault().CreatedBy;
                IPPoolInfoForUpdate.CreatedDate = IPPool_db.FirstOrDefault().CreatedDate;
                IPPoolInfoForUpdate.UpdateBy = AppUtils.GetLoginUserID();
                IPPoolInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(IPPool_db.SingleOrDefault()).CurrentValues.SetValues(IPPoolInfoForUpdate);
                db.SaveChanges();

                TempData["UpdateSucessOrFail"] = "Update Successfully.";
                var IPPool_Return = IPPool_db;
                var JSON = Json(new { UpdateSuccess = true, IPPoolUpdateInformation = IPPool_Return }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                TempData["UpdateSucessOrFail"] = "Update Fail.";
                return Json(new { UpdateSuccess = false, IPPoolUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}