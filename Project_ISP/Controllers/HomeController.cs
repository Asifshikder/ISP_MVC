using ISP_ManagementSystemModel.Models;
using Project_ISP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ISP_ManagementSystemModel.AppUtils;

namespace ISP_ManagementSystemModel.Controllers
{

    [Authorize]
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class HomeController : Controller
    {
        public HomeController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();
        // [UserRIghtCheck(ControllerValue = AppUtils.AdvancePayment_AddAdvancePayment)]
        //public ActionResult demoData()
        //{
        //    var demoNewPermissionData =
        //           db.ISPAccessList
        //           .AsEnumerable().Select(
        //               s => new
        //               {
        //                   MyPermissionInfo = "public const string " + s.AccessName .Trim().Replace(" ", "_") + " = \"" + s.AccessValue + "\";"

        //                // Action = 
        //            }
        //               );

        //    var newAccessNameList = "";
        //    foreach (var item in demoNewPermissionData)
        //    {
        //        newAccessNameList += item.MyPermissionInfo;
        //    }

        //    var demoActionData =
        //        db.Form.
        //        Join(db.Action.Where(s => s.ShowingStatus == 1 && s.ActionDescription != ""),
        //            Form => Form.FormID,
        //            Action => Action.FormID,
        //            (Form, Action) => new { Form = Form, Action = Action })
        //       .Join(
        //            db.ControllerName,
        //            Forms => Forms.Form.ControllerNameID,
        //            Controller => Controller.ControllerNameID,
        //            (Forms, Controller) => new { Forms = Forms, Action = Forms.Action, Controller = Controller })

        //            .OrderBy(s => s.Controller.ControllerNameID).ThenBy(s=>s.Forms.Form.FormID).AsEnumerable().Select(
        //            s => new
        //            {
        //                MyButtonInfo  = "public const string " + s.Controller.ControllerNames+"_"+s.Forms.Form.FormName+"_"+s.Action.ActionDescription.Trim().Replace(" ", "__") + " = \""+s.Action.ActionValue+"\";"

        //                // Action = 
        //            }
        //            );

        //    var actionNameList = "";
        //    foreach (var item in demoActionData)
        //    {
        //        actionNameList += item.MyButtonInfo;
        //        Console.WriteLine(item.MyButtonInfo);
        //    }

        //    var demoFormNameData =
        //        db.ControllerName.Join(db.Form.Where(s => s.ShowingStatus == 1), Controller => Controller.ControllerNameID, Form => Form.ControllerNameID,
        //            (Controller, Form) => new
        //            {
        //                Controller = Controller,
        //                Form = Form
        //            }).OrderBy(s => s.Controller.ControllerNameID).ThenBy(s => s.Form.ControllerNameID).AsEnumerable().
        //            Select(s => new
        //            {
        //                FormName = "public const string " + s.Controller.ControllerNames + "_" + s.Form.FormName.Trim() + " = \"" + s.Form.FormValue + "\";"
        //            });
        //    string formNameList = "";
        //    foreach (var item in demoFormNameData)
        //    {
        //        formNameList += item.FormName;
        //    }



        //    var demoControllerNameData =
        //        db.ControllerName.OrderBy(s => s.ControllerNameID).AsEnumerable().
        //            Select(s => new
        //            {
        //                ControllerName = "public const string " + s.ControllerNames + " = \"" + s.ControllerValue + "\";"
        //            });
        //    string controllerNameList = "";
        //    foreach (var item in demoControllerNameData)
        //    {
        //        controllerNameList += item.ControllerName;
        //    }
        //    return Json(demoActionData.ToList().Select(s=>s.MyButtonInfo),JsonRequestBehavior.AllowGet);
        //}

        ////public  Bitmap ResizeImage(Image image, int width, int height)
        ////{
        ////    var destRect = new Rectangle(0, 0, width, height);
        ////    var destImage = new Bitmap(width, height);

        ////    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        ////    using (var graphics = Graphics.FromImage(destImage))
        ////    {
        ////        graphics.CompositingMode = CompositingMode.SourceCopy;
        ////        graphics.CompositingQuality = CompositingQuality.HighQuality;
        ////        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        ////        graphics.SmoothingMode = SmoothingMode.HighQuality;
        ////        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        ////        using (var wrapMode = new ImageAttributes())
        ////        {
        ////            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        ////            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        ////        }
        ////    }
        ////    imageToByteArray(destImage);

        ////    return destImage;
        ////}

        ////public byte[] imageToByteArray(System.Drawing.Image imageIn)
        ////{
        ////    MemoryStream ms = new MemoryStream();
        ////    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        ////    return ms.ToArray();
        ////}


        [Authorize(Roles = "1,2")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "2")]
        public ActionResult IndexRole2()
        {
            return View();
        }

        //[AllowAnonymous]
        //public ActionResult About()
        //{
        //    return View();
        //}
        [Authorize(Roles = "1")]
        public ActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "1,2,3,4")]

        [SetUserRightManual()]
        public ActionResult Dashboard()
        {


            if (Session["role_id"].ToString() == AppUtils.AdminRole.ToString() /*|| Session["role_id"].ToString() == AppUtils.EmployeeRole.ToString()*/ || Session["role_id"].ToString() == AppUtils.SuperUserRole.ToString())
            {
                if (AppUtils.HasAccessInTheList(AppUtils.ViewDashBoard))
                {
                    ViewDashBoardWithData();
                }
                else
                {
                    ViewDashBoardWithoutData();
                }


            }
            if (Session["role_id"].ToString() == AppUtils.ClientRole.ToString())
            {

                var loginID = AppUtils.GetLoginUserID();
                List<Complain> myComplainList = db.Complain.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/).ToList();

                ViewBag.BillHistoryNumber = db.Transaction.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/ && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).Count();
                ViewBag.MyTotalComplain = myComplainList.Count();
                ViewBag.MyPendingComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
                ViewBag.MySolveComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
                //ViewBag.TotalComplains = db.Complain.Count();
            }



            //DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            //DateTime endDateOfThisMonth = AppUtils.ThisMonthLastDate();
            //DateTime getDateTime = AppUtils.GetDateTimeNow();

            //if (Session["role_id"].ToString() == AppUtils.AdminRole.ToString() || Session["role_id"].ToString() == AppUtils.EmployeeRole.ToString() || Session["role_id"].ToString() == AppUtils.SuperUserRole.ToString())
            //{
            //    List<ClientDetails> lstClientDetails = db.ClientDetails.ToList();
            //    List<Transaction> lstTransaction = db.Transaction.ToList();
            //    List<ClientLineStatus> lstClientLineStatus = db.ClientLineStatus.ToList();
            //    List<ClientDueBills> lstClientDueBills = db.ClientDueBills.ToList();

            //    ViewBag.TotalEmployee = lstClientLineStatus.GroupBy(s => s.ClientDetailsID).Count();
            //    ViewBag.ActiveClient = lstClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 3).Count();
            //    ViewBag.LockClient = lstClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 5).Count(); ;
            //    ViewBag.ConnectionRequest = lstClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient).Count();
            //    ViewBag.TotalBill = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => ((s.PaymentAmount != null) ? s.PaymentAmount : 0) + ((s.PackageID.Value != null) ? s.Package.PackagePrice : 0));
            //    ViewBag.PaidBill = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.PaymentAmount);
            //    ViewBag.Discount = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == AppUtils.PaymentTypeIsConnection && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.Discount);
            //    ViewBag.UnpaidBill = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.Package.PackagePrice);
            //    ViewBag.UnpaidMember = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Count();
            //    ViewBag.NewSignUp = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate == endDateOfThisMonth)).Count();
            //    ViewBag.NewSignUpBill = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate == endDateOfThisMonth)).Sum(s => s.PaymentAmount);

            //    List<int> lstLockClientOnPreviousMonthall = lstClientLineStatus
            //        .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
            //        .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        .Where(s => s.LineStatusID == 5).Select(s => s.ClientDetailsID).ToList();
            //    var lstActiveClientOnThisMonthAndLockOnPreviousMonth = db.ClientLineStatus
            //                                                                .Where(s => s.LineStatusID == 3 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
            //                                                                .Select(s => s.ClientLineStatusID).ToList();
            //    var lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount = db.ClientLineStatus
            //                                                                .Where(s => s.LineStatusID == 3 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
            //                                                                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList().Sum(s => s.PackagePrice);

            //    List<int> lstActiveClientOnPreviousMonthall = lstClientLineStatus
            //        .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
            //        .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
            //        .Where(s => s.LineStatusID == 3).Select(s => s.ClientDetailsID).ToList();

            //    List<int> lstLockClientOnThisMonthAndActiveOnPreviousMonth = lstClientLineStatus
            //                                                                .Where(s => s.LineStatusID == 5 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
            //                                                                .Select(s => s.ClientLineStatusID).ToList();
            //    var lstLockClientOnThisMonthAndActiveOnPreviousMonthAmount = db.ClientLineStatus
            //                                                                .Where(s => s.LineStatusID == 5 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
            //                                                                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList()
            //                                                                .Sum(s => s.PackagePrice);

            //    ViewBag.LockToActive = lstActiveClientOnThisMonthAndLockOnPreviousMonth.Count();
            //    ViewBag.LockToActiveTotalBill = lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount;
            //    ViewBag.ActiveClientPhone = lstClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 3).Count();
            //    ViewBag.LockClientPhone = lstClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 5).Count();
            //    ViewBag.UnpaidClientPhone = lstTransaction.Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).GroupBy(s => s.ClientDetailsID).ToList().Count();
            //    ViewBag.ActiveToLock = lstLockClientOnThisMonthAndActiveOnPreviousMonth.Count();
            //    ViewBag.ActiveToLockTotalBill = lstLockClientOnThisMonthAndActiveOnPreviousMonthAmount;
            //    ViewBag.TotalComplains = db.Complain.Count();
            //    ViewBag.PendingComplains = db.Complain.Where(s => s.LineStatusID == 6).Count();
            //    ViewBag.SolvedComplains = db.Complain.Where(s => s.LineStatusID == 8).Count();

            //}
            //if (Session["role_id"].ToString() == AppUtils.ClientRole.ToString())
            //{
            //    List<Complain> myComplainList = db.Complain.Where(s => s.ClientDetailsID == AppUtils.LoginUserID).ToList();

            //    ViewBag.BillHistoryNumber = db.Transaction.Where(s => s.ClientDetailsID == AppUtils.LoginUserID && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).Count();
            //    ViewBag.MyTotalComplain = myComplainList.Count();
            //    ViewBag.MyPendingComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
            //    ViewBag.MySolveComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
            //    //ViewBag.TotalComplains = db.Complain.Count();
            //}


            return View();

        }


        [Authorize(Roles = "2")]
        //[SetUserRightManual()]
        public ActionResult UserDashboard()
        {
            var loginID = AppUtils.GetLoginUserID();
            var myComplainList = db.Complain.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/).Select(x => new { x.LineStatusID, x.ClientDetailsID }).ToList();

            ViewBag.BillHistoryNumber = db.Transaction.Where(s => s.ClientDetailsID == loginID/*AppUtils.LoginUserID*/ && s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly).Count();
            ViewBag.MyTotalComplain = myComplainList.Count();
            ViewBag.MyPendingComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
            ViewBag.MySolveComplain = myComplainList.Where(s => s.LineStatusID == AppUtils.ComplainPendingStatus).Count();

            return View();
        }

        [Authorize(Roles = "1,4,5")]

        [SetUserRightManual()]
        public ActionResult ResellerDashboard(int rid = 0)
        {

            string macResellerTypeId = Convert.ToString((int)Enum.Parse(typeof(ResellerTypeEnum), ResellerTypeEnum.MacBasedReseller.ToString()));
            var resellerList = db.Reseller.Where(x => x.ResellerTypeListID == macResellerTypeId).Select(x => new { x.ResellerID, x.ResellerLoginName }).ToList();
            ViewBag.ResellerID = new SelectList(resellerList, "ResellerID", "ResellerLoginName", rid);
            if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
            {
                rid = AppUtils.GetLoginUserID();
            }

            if (Session["role_id"].ToString() == AppUtils.AdminRole.ToString() /*|| Session["role_id"].ToString() == AppUtils.EmployeeRole.ToString()*/ || Session["role_id"].ToString() == AppUtils.SuperUserRole.ToString() || Session["role_id"].ToString() == AppUtils.ResellerRole.ToString())
            {
                if (AppUtils.HasAccessInTheList(AppUtils.ViewDashBoard))
                {
                    ViewDashBoardWithDataForReseller(rid);
                }
                else
                {
                    ViewDashBoardWithoutData();
                }
            }
            //ViewDashBoardWithoutData(); 
            return View();

        }
        private void ViewDashBoardWithoutData()
        {

            ViewBag.TotalEmployee = "";
            ViewBag.ActiveClient = "";
            ViewBag.LockClient = "";
            ViewBag.ConnectionRequest = "";
            ViewBag.TotalBill = "";
            ViewBag.PaidBill = "";
            ViewBag.UnpaidBill = "";
            ViewBag.Discount = "";
            ViewBag.UnpaidMember = "";
            ViewBag.NewSignUp = "";
            ViewBag.NewSignUpBill = "";


            ViewBag.LockToActive = "";
            ViewBag.LockToActiveTotalBill = "";
            ViewBag.ActiveClientPhone = "";
            ViewBag.LockClientPhone = "";
            ViewBag.UnpaidClientPhone = "";
            ViewBag.ActiveToLock = "";
            ViewBag.ActiveToLockTotalBill = "";
            ViewBag.TotalComplains = "";
            ViewBag.PendingComplains = "";
            ViewBag.SolvedComplains = "";

            ViewBag.TotalMikrotikCount = "";
            ViewBag.TotalMikrotikUserCount = "";
            ViewBag.TotalMikrotikPackageCount = "";

            ViewBag.TotalSMSSendCount = "";


            ViewBag.TotalExpenseInThisMonth = "";

        }

        private void ViewDashBoardWithData()
        {
            int? resellerID = null;
            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            DateTime getDateTime = AppUtils.GetDateTimeNow();

            IEnumerable<Transaction> lstTransaction = db.Transaction.Where(s => s.ResellerID == null && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).AsEnumerable();
            var lstClientDetails = db.ClientDetails.Where(x => x.ResellerID == null).AsQueryable();


            ViewBag.TotalEmployee = lstClientDetails.Where(x => x.IsNewClient != AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count();//db.ClientLineStatus.GroupBy(s => s.ClientDetailsID).Count();
            ViewBag.ActiveClient = lstClientDetails.Where(x => x.StatusThisMonth == AppUtils.LineIsActive && x.IsNewClient != AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count(); //lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsActive).Count();
            ViewBag.LockClient = lstClientDetails.Where(x => x.StatusThisMonth == AppUtils.LineIsLock && x.IsNewClient != AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count(); //lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock).Count(); ;
            ViewBag.DeleteClient = lstClientDetails.Where(x => x.Status == AppUtils.LineIsDelete).Count();
            ViewBag.ConnectionRequest = lstClientDetails.Where(x => x.IsNewClient == AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count();
            ViewBag.TotalBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => ((s.PaymentAmount != null) ? s.PaymentAmount : 0) /*+ ((s.PackageID.Value != null) ? s.Package.PackagePrice : 0)*/);
            ViewBag.PaidBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => ((s.PaidAmount == null ? 0 : s.PaidAmount.Value) + (s.Discount == null ? 0 : s.Discount.Value)));
            //var a = lstTransaction.ToList();
            ViewBag.UnpaidBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => s.PaymentAmount - ((s.Discount == null ? 0 : s.Discount.Value) + (s.PaidAmount == null ? 0 : s.PaidAmount.Value)));
            ViewBag.Discount = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.Discount);
            //var a = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).ToList();
            ViewBag.UnpaidMember = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Count();
            ViewBag.NewSignUp = db.Transaction.Where(s => s.ResellerID == null && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth)).Count();
            ViewBag.NewSignUpBill = db.Transaction.Where(s => s.ResellerID == null && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth)).Sum(s => s.PaymentAmount);

            List<int> lstLockClientOnPreviousMonthall = db.ClientLineStatus
                .Where(s => s.ResellerID == null && s.LineStatusChangeDate < startDateOfThisMonth)
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                .Where(s => s.LineStatusID == 5).Select(s => s.ClientDetailsID).ToList();
            var lstActiveClientOnThisMonthAndLockOnPreviousMonth = db.ClientLineStatus
                .Where(s => s.ResellerID == null && s.LineStatusID == 3 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => s.ClientLineStatusID).ToList();
            var lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount = db.ClientLineStatus
                .Where(s => s.ResellerID == null && s.LineStatusID == 3 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList().Sum(s => s.PackagePrice);

            List<int> lstActiveClientOnPreviousMonthall = db.ClientLineStatus
                .Where(s => s.LineStatusChangeDate < startDateOfThisMonth)
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                .Where(s => s.LineStatusID == 3).Select(s => s.ClientDetailsID).ToList();

            List<int> lstLockClientOnThisMonthAndActiveOnPreviousMonth = db.ClientLineStatus
                .Where(s => s.ResellerID == null && s.LineStatusID == 5 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => s.ClientLineStatusID).ToList();
            var lstLockClientOnThisMonthAndActiveOnPreviousMonthAmount = db.ClientLineStatus
                .Where(s => s.ResellerID == null && s.LineStatusID == 5 && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList()
                .Sum(s => s.PackagePrice);

            ViewBag.LockToActive = lstActiveClientOnThisMonthAndLockOnPreviousMonth.Count();
            ViewBag.LockToActiveTotalBill = lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount;
            ViewBag.ActiveClientPhone = db.ClientLineStatus.Where(s => s.ResellerID == null).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 3).Count();
            ViewBag.LockClientPhone = db.ClientLineStatus.Where(s => s.ResellerID == null).GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == 5).Count();
            ViewBag.UnpaidClientPhone = lstTransaction.Where(s => s.ResellerID == null && s.PaymentStatus == AppUtils.PaymentIsNotPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).GroupBy(s => s.ClientDetailsID).ToList().Count();
            ViewBag.ActiveToLock = lstLockClientOnThisMonthAndActiveOnPreviousMonth.Count();
            ViewBag.ActiveToLockTotalBill = lstLockClientOnThisMonthAndActiveOnPreviousMonthAmount;
            ViewBag.TotalComplains = db.Complain.Where(s => s.ResellerID == null).Count();
            ViewBag.PendingComplains = db.Complain.Where(s => s.ResellerID == null && s.LineStatusID == AppUtils.ComplainPendingStatus).Count();
            ViewBag.SolvedComplains = db.Complain.Where(s => s.ResellerID == null && s.LineStatusID == AppUtils.ComplainSolveStatus).Count();

            ViewBag.TotalMikrotikCount = (bool)Session["MikrotikOptionEnable"] ? db.Mikrotik.Count() : 0;
            ViewBag.TotalMikrotikUserCount = (bool)Session["MikrotikOptionEnable"] ? db.ClientDetails.Where(s => s.ResellerID == null && s.Mikrotik != null).Count() : 0;
            int packageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            ViewBag.TotalMikrotikPackageCount = (bool)Session["MikrotikOptionEnable"] ? db.Package.Where(s => s.PackageForMyOrResellerUser == packageForMyUser && s.Mikrotik != null).Count() : 0;

            ViewBag.TotalSMSSendCount = (bool)Session["SMSOptionEnable"] ? db.SMS.Sum(s => s.SMSCounter) : 0;


            ViewBag.TotalExpenseInThisMonth = db.Expenses.Where(s => s.ResellerID == null && s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth).Count() > 0 ? db.Expenses.Where(s => s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth).Sum(s => s.Amount) : 0;

        }
        private void ViewDashBoardWithDataForReseller(int resellerID)
        {

            DateTime startDateOfThisMonth = AppUtils.ThisMonthStartDate();
            DateTime endDateOfThisMonth = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            DateTime getDateTime = AppUtils.GetDateTimeNow();

            IEnumerable<Transaction> lstTransaction = db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ResellerID == resellerID).AsEnumerable();
            var lstClientDetails = db.ClientDetails.Where(x => x.ResellerID == resellerID).AsQueryable();

            //var a = lstTransaction.ToList();
            ViewBag.ActiveClient = lstClientDetails.Where(x => x.StatusThisMonth == AppUtils.LineIsActive && x.IsNewClient != AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count(); //lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsActive).Count();
            ViewBag.LockClient = lstClientDetails.Where(x => x.StatusThisMonth == AppUtils.LineIsLock && x.IsNewClient != AppUtils.isNewClient && x.Status != AppUtils.LineIsDelete).Count(); //lstTransaction.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.ClientDetailsID).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock).Count(); ;
            ViewBag.DeleteClient = lstClientDetails.Where(x => x.Status == AppUtils.LineIsDelete).Count();
            ViewBag.TotalBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => ((s.PaymentAmount != null) ? s.PaymentAmount : 0) /*+ ((s.PackageID.Value != null) ? s.Package.PackagePrice : 0)*/);
            ViewBag.PaidBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => ((s.PaidAmount == null ? 0 : s.PaidAmount.Value) + (s.Discount == null ? 0 : s.Discount.Value)));
            ViewBag.UnpaidBill = lstTransaction.Where(s => s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Sum(s => s.PaymentAmount - ((s.Discount == null ? 0 : s.Discount.Value) + (s.PaidAmount == null ? 0 : s.PaidAmount.Value)));
            ViewBag.Discount = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Sum(s => s.Discount);
            ViewBag.UnpaidMember = lstTransaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).Where(s => s.PaymentStatus == 0 && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).Count();
            ViewBag.NewSignUp = db.Transaction.Where(s => s.ResellerID == resellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth)).Count();
            ViewBag.NewSignUpBill = db.Transaction.Where(s => s.ResellerID == resellerID && s.PaymentTypeID == AppUtils.PaymentTypeIsConnection && (s.PaymentDate >= startDateOfThisMonth && s.PaymentDate <= endDateOfThisMonth)).Sum(s => s.PaymentAmount);

            List<int> lstLockClientOnPreviousMonthall = db.ClientLineStatus
                .Where(s => s.LineStatusChangeDate < startDateOfThisMonth && s.ResellerID == resellerID)
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                .Where(s => s.LineStatusID == 5).Select(s => s.ClientDetailsID).ToList();
            var lstActiveClientOnThisMonthAndLockOnPreviousMonth = db.ClientLineStatus
                .Where(s => s.LineStatusID == 3 && s.ResellerID == resellerID && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => s.ClientLineStatusID).ToList();
            var lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount = db.ClientLineStatus
                .Where(s => s.LineStatusID == 3 && s.ResellerID == resellerID && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstLockClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList().Sum(s => s.PackagePrice);

            List<int> lstActiveClientOnPreviousMonthall = db.ClientLineStatus
                .Where(s => s.LineStatusChangeDate < startDateOfThisMonth && s.ResellerID == resellerID)
                .GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault())
                .Where(s => s.LineStatusID == 3).Select(s => s.ClientDetailsID).ToList();

            List<int> lstLockClientOnThisMonthAndActiveOnPreviousMonth = db.ClientLineStatus
                .Where(s => s.LineStatusID == 5 && s.ResellerID == resellerID && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => s.ClientLineStatusID).ToList();
            var lstLockClientOnThisMonthAndActiveOnPreviousMonthAmount = db.ClientLineStatus
                .Where(s => s.LineStatusID == 5 && s.ResellerID == resellerID && (s.LineStatusChangeDate >= startDateOfThisMonth && s.LineStatusChangeDate <= endDateOfThisMonth) && lstActiveClientOnPreviousMonthall.Contains(s.ClientDetailsID))
                .Select(s => new { ClientID = s.ClientDetailsID, PackagePrice = (s.Package == null) ? 0 : s.Package.PackagePrice }).ToList()
                .Sum(s => s.PackagePrice);

            ViewBag.LockToActive = lstActiveClientOnThisMonthAndLockOnPreviousMonth.Count();
            ViewBag.LockToActiveTotalBill = lstActiveClientOnThisMonthAndLockOnPreviousMonthAmount;
            ViewBag.ActiveToLock = lstLockClientOnThisMonthAndActiveOnPreviousMonth.Count();

        }

        //[HttpGet]
        //public ActionResult tree()
        //{
        //    List<AuthorViewModel> model = new List<AuthorViewModel>();

        //    AuthorViewModel firstAuthor = new AuthorViewModel
        //    {
        //        Id = 1,
        //        Name = "John",
        //        IsAuthor = true,
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=1,
        //                Title = "JQuery",
        //                IsWritten = true
        //            }, new BookViewModel{
        //                Id=1,
        //                Title = "JavaScript",
        //                IsWritten = false
        //            }
        //        }
        //    };

        //    AuthorViewModel secondAuthor = new AuthorViewModel
        //    {
        //        Id = 2,
        //        Name = "Deo",
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=3,
        //                Title = "C#",
        //                IsWritten = false
        //            }, new BookViewModel{
        //                Id=4,
        //                Title = "Entity Framework",
        //                IsWritten = false
        //            }
        //        }
        //    };
        //    AuthorViewModel firstAuthor1 = new AuthorViewModel
        //    {
        //        Id = 1,
        //        Name = "John",
        //        IsAuthor = true,
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=1,
        //                Title = "JQuery",
        //                IsWritten = true
        //            }, new BookViewModel{
        //                Id=1,
        //                Title = "JavaScript",
        //                IsWritten = false
        //            }
        //        }
        //    };

        //    AuthorViewModel secondAuthor2 = new AuthorViewModel
        //    {
        //        Id = 2,
        //        Name = "Deo",
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=3,
        //                Title = "C#",
        //                IsWritten = false
        //            }, new BookViewModel{
        //                Id=4,
        //                Title = "Entity Framework",
        //                IsWritten = false
        //            }
        //        }
        //    };
        //    AuthorViewModel firstAuthor3 = new AuthorViewModel
        //    {
        //        Id = 1,
        //        Name = "John",
        //        IsAuthor = true,
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=1,
        //                Title = "JQuery",
        //                IsWritten = true
        //            }, new BookViewModel{
        //                Id=1,
        //                Title = "JavaScript",
        //                IsWritten = false
        //            }
        //        }
        //    };

        //    AuthorViewModel secondAuthor4 = new AuthorViewModel
        //    {
        //        Id = 2,
        //        Name = "Deo",
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=3,
        //                Title = "C#",
        //                IsWritten = false
        //            }, new BookViewModel{
        //                Id=4,
        //                Title = "Entity Framework",
        //                IsWritten = false
        //            }
        //        }
        //    };
        //    AuthorViewModel firstAuthor5 = new AuthorViewModel
        //    {
        //        Id = 1,
        //        Name = "John",
        //        IsAuthor = true,
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=1,
        //                Title = "JQuery",
        //                IsWritten = true
        //            }, new BookViewModel{
        //                Id=1,
        //                Title = "JavaScript",
        //                IsWritten = false
        //            }
        //        }
        //    };

        //    AuthorViewModel secondAuthor6 = new AuthorViewModel
        //    {
        //        Id = 2,
        //        Name = "Deo",
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=3,
        //                Title = "C#",
        //                IsWritten = false
        //            }, new BookViewModel{
        //                Id=4,
        //                Title = "Entity Framework",
        //                IsWritten = false
        //            }
        //        }
        //    };
        //    AuthorViewModel firstAuthor7 = new AuthorViewModel
        //    {
        //        Id = 1,
        //        Name = "John",
        //        IsAuthor = true,
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=1,
        //                Title = "JQuery",
        //                IsWritten = true
        //            }, new BookViewModel{
        //                Id=1,
        //                Title = "JavaScript",
        //                IsWritten = false
        //            }
        //        }
        //    };

        //    AuthorViewModel secondAuthor8 = new AuthorViewModel
        //    {
        //        Id = 2,
        //        Name = "Deo",
        //        BookViewModel = new List<BookViewModel>{
        //            new BookViewModel{
        //                Id=3,
        //                Title = "C#",
        //                IsWritten = false
        //            }, new BookViewModel{
        //                Id=4,
        //                Title = "Entity Framework",
        //                IsWritten = false
        //            }
        //        }
        //    };
        //    model.Add(firstAuthor);
        //    model.Add(secondAuthor);
        //    model.Add(firstAuthor1);
        //    model.Add(secondAuthor2);
        //    model.Add(firstAuthor3);
        //    model.Add(secondAuthor4);
        //    model.Add(firstAuthor5);
        //    model.Add(secondAuthor6);
        //    model.Add(firstAuthor7);
        //    model.Add(secondAuthor8);
        //    return View("tree", model);
        //}

        //[HttpPost]
        //public ActionResult tree(List<AuthorViewModel> model)
        //{
        //    List<AuthorViewModel> selectedAuthors = model.Where(a => a.IsAuthor).ToList();
        //    List<BookViewModel> selectedBooks = model.Where(a => a.IsAuthor)
        //                                        .SelectMany(a => a.BookViewModel.Where(b => b.IsWritten)).ToList();
        //    return RedirectToAction("tree");
        //}

        private string getValue(string advancePayment_AddAdvancePayment)
        {
            return "";
        }
    }
}