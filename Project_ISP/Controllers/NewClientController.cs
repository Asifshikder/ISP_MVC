using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using Project_ISP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tik4net;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class NewClientController : Controller
    {
        public NewClientController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        //
        int newClient = 1;
        int newClientStatusChangeToOldClient = 0;
        ISPContext db = new ISPContext();
        // GET: /NewClient/


        [UserRIghtCheck(ControllerValue = AppUtils.View_Request__List_Of_Client)]
        public ActionResult GetAllNewClientList()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.CableTypePopUpID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");

            ViewBag.lstStockID = new SelectList(db.Stock.ToList(), "StockID", "Item.ItemName");

            //    ViewBag.lstPopID = new SelectList(db.StockDetails.Where(s => s.StockID == StockIdForPop && s.SectionID == AppUtils.StockSection && s.ProductStatusID == AppUtils.ProductStatusIsAvialable).ToList(), "StockID", "Serial");

            ViewBag.lstEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).ToList(), "EmployeeID", "Name");

            LoadViewBagNewConnecion();
            LoadViewBag();
            // List<ClientDetails> lstClientDetails = db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient).ToList();
            //return View(lstClientDetails);


            //ViewBag.CableTypePopUpID = new SelectList(db.CableType.Select(x => new { CableTypeID = x.CableTypeID, CableTypeName = x.CableTypeName }).ToList(), "CableTypeID", "CableTypeName");

            //ViewBag.lstStockID = new SelectList(db.Stock.Select(x => new { StockID = x.StockID, ItemName = x.Item.ItemName }).ToList(), "StockID", "ItemName");

            //ViewBag.lstMikrotik = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");

            //ViewBag.lstEmployeeID = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).Select(x => new { EmployeeID = x.EmployeeID, Name = x.Name }).ToList(), "EmployeeID", "Name");
            //ViewBag.ZoneID = new SelectList(db.Zone.Select(x => new { ZoneID = x.ZoneID, ZoneName = x.ZoneName }).ToList(), "ZoneID", "ZoneName");
            //ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.Select(x => new { ConnectionTypeID = x.ConnectionTypeID, ConnectionTypeName = x.ConnectionTypeName }).ToList(), "ConnectionTypeID", "ConnectionTypeName");
            //ViewBag.CableTypeID = new SelectList(db.CableType.Select(x => new { CableTypeID = x.CableTypeID, CableTypeName = x.CableTypeName }).ToList(), "CableTypeID", "CableTypeName");

            //int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            //var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            //ViewBag.PackageID = new SelectList(lstPackage, "PackageID", "PackageName");
            //ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.Select(x => new { SecurityQuestionID = x.SecurityQuestionID, SecurityQuestionName = x.SecurityQuestionName }).ToList(), "SecurityQuestionID", "SecurityQuestionName");
            //ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");

            //ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

            //if ((int)Session["role_id"] == AppUtils.ResellerRole)
            //{
            //    Reseller reseller = db.Reseller.Find((int)Session["LoggedUserID"]);
            //    List<int> lstResellerBillingCycle = string.IsNullOrEmpty(reseller.ResellerBillingCycleList) ? new List<int>() : reseller.ResellerBillingCycleList.ToString().Trim(',').Split(',').Select(Int32.Parse).ToList();
            //    ViewBag.BillPaymentDate = new SelectList(lstResellerBillingCycle.Select(x => new { BillPaymentDateID = x, BillPaymentDateName = x }).ToList(), "BillPaymentDateID", "BillPaymentDateName");
            //}
            //ClientDetails cd = new ClientDetails();
            //cd.ClientNIDImageBytesPaths = "/";
            //cd.ClientOwnImageBytesPaths = "/";

            TempData["ClientOwnImageBytesPaths"] = "/";
            TempData["ClientNIDImageBytesPaths"] = "/";

            return View(new List<NewClientCustomInformation>());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllNewClientsAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                int zoneFromDDL = 0;
                // Initialization.   
                var ZoneID = Request.Form.Get("ZoneID");
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                if (ZoneID != "")
                {
                    zoneFromDDL = int.Parse(ZoneID);
                }
                // Loading.   

                // Apply pagination.   
                //data = data.Skip(startRec).Take(pageSize).ToList();
                var newCLientList = (ZoneID == "") ? db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient && s.Status != AppUtils.TableStatusIsDelete).AsEnumerable() : db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient && s.ZoneID == zoneFromDDL && s.Status != AppUtils.TableStatusIsDelete).AsEnumerable();

                //db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient && s.RoleID == AppUtils.ClientRole && s.ZoneID == ((ZoneID == "")  ? 1: int.Parse(ZoneID)))


                int ifSearch = 0;
                List<NewClientCustomInformation> data =
                    newCLientList.Any() ? newCLientList.Skip(startRec).Take(pageSize).AsEnumerable()
                    //.Select new {ss=>ss. }
                    .Select(
                        s => new NewClientCustomInformation
                        {
                            ClientDetailsID = s.ClientDetailsID,
                            Name = s.Name,
                            Zone = s.Zone.ZoneName,
                            Address = s.Address,
                            LatitudeLongitude = s.LatitudeLongitude,
                            ContactNumber = s.ContactNumber,
                            Package = s.Package.PackageName,
                            AssignedTo = s.Employee.Name,
                            Survey = s.ClientSurvey,
                            time = s.UpdateDate != null ? s.UpdateDate.Value : s.CreateDate.Value,
                            //CreateBy = db.Employee.Where(ss => ss.EmployeeID == Convert.ToInt32(s.CreateBy)).FirstOrDefault().Name,
                            CreateBy = s.CreateBy,
                            UpdateBy = s.UpdateBy != null ? s.UpdateBy : "",
                            ShowNewClient = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Request_Client) ? true : false,
                            SignUp = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Sign_Up_New_Client) ? true : false,
                            DeleteButton = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Delete_Request_Client) ? true : false

                        })
                        .ToList() : new List<NewClientCustomInformation>();
                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {

                    ifSearch = (newCLientList.Any()) ? newCLientList.Where(p => p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) || p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                                                          p.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) || p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                                                                          p.ContactNumber.ToLower().Contains(search.ToLower()) || p.Package.PackageName.ToLower().Contains(search.ToLower()) ||
                                                                          p.Employee.Name.ToString().ToLower().Contains(search.ToLower()) || p.ClientSurvey.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                    // Apply search   
                    data = data.Where(p => p.ClientDetailsID.ToString().ToLower().Contains(search.ToLower()) || p.Name.ToString().ToLower().Contains(search.ToLower()) ||
                                                       p.Zone.ToString().ToLower().Contains(search.ToLower()) || p.Address.ToString().ToLower().Contains(search.ToLower()) ||
                                                       p.ContactNumber.ToLower().Contains(search.ToLower()) || p.Package.ToLower().Contains(search.ToLower()) ||
                                                       p.AssignedTo.ToString().ToLower().Contains(search.ToLower()) || p.Survey.ToString().ToLower().Contains(search.ToLower())
                    ).ToList();
                }
                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data);
                // Total record count.   
                int totalRecords = newCLientList.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                // Filter record count.   
                int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : newCLientList.AsEnumerable().Count();

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

        private List<NewClientCustomInformation> SortByColumnWithOrder(string order, string orderDir, List<NewClientCustomInformation> data)
        {
            // Initialization.   
            List<NewClientCustomInformation> lst = new List<NewClientCustomInformation>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientDetailsID).ToList() : data.OrderBy(p => p.ClientDetailsID).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Zone).ToList() : data.OrderBy(p => p.Zone).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList() : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Package).ToList() : data.OrderBy(p => p.Package).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssignedTo).ToList() : data.OrderBy(p => p.AssignedTo).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Survey).ToList() : data.OrderBy(p => p.Survey).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientDetailsID).ToList() : data.OrderBy(p => p.ClientDetailsID).ToList();
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

        private void LoadViewBag()
        {
            ViewBag.ZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.SearchByZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.ConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.CableTypeID = new SelectList(db.CableType.ToList(), "CableTypeID", "CableTypeName");
            ViewBag.PackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            ViewBag.SecurityQuestionID = new SelectList(db.SecurityQuestion.ToList(), "SecurityQuestionID", "SecurityQuestionName");
            ViewBag.LineStatusID = new SelectList(db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || /*s.LineStatusID == AppUtils.LineIsInActive ||*/ s.LineStatusID == AppUtils.LineIsLock).ToList(), "LineStatusID", "LineStatusName");
            ViewBag.BannedID = new SelectList(db.BannedStatus.ToList(), "BannedStatusID", "BannedStatusName");
            ViewBag.AssignToWhichEmployee = new SelectList(db.Employee.ToList(), "EmployeeID", "Name");

            ViewBag.BoxID = new SelectList(db.Box.Select(x => new { BoxID = x.BoxID, BoxName = x.BoxName }).ToList(), "BoxID", "BoxName");
            int PackageForMyUser = int.Parse(AppUtils.PackageForMyUser);
            var lstPackage = db.Package.Where(x => x.PackageForMyOrResellerUser == PackageForMyUser).Select(x => new { x.PackageID, x.PackageName }).ToList();
            ViewBag.PackageThisMonth = new SelectList(lstPackage, "PackageID", "PackageName");
            ViewBag.PackageNextMonth = new SelectList(lstPackage, "PackageID", "PackageName");

            var lstLineStatus = db.LineStatus.Where(s => s.LineStatusID == AppUtils.LineIsActive || s.LineStatusID == AppUtils.LineIsLock).Select(x => new { x.LineStatusID, x.LineStatusName }).ToList();
            ViewBag.ThisMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
            ViewBag.NextMonthLineStatusID = new SelectList(lstLineStatus, "LineStatusID", "LineStatusName");
        }

        private void LoadViewBagNewConnecion()
        {
            ViewBag.NewZoneID = new SelectList(db.Zone.ToList(), "ZoneID", "ZoneName");
            ViewBag.NewConnectionTypeID = new SelectList(db.ConnectionType.ToList(), "ConnectionTypeID", "ConnectionTypeName");
            ViewBag.NewPackageID = new SelectList(db.Package.ToList(), "PackageID", "PackageName");
            ViewBag.NewAssignToWhichEmployee = new SelectList(db.Employee.Where(s => s.EmployeeStatus == AppUtils.EmployeeStatusIsActive).ToList(), "EmployeeID", "Name");


        }

        [HttpGet]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_New_Request)]
        public ActionResult NewConnection()
        {
            LoadViewBagNewConnecion();
            return View();
        }

        [HttpPost]
        public JsonResult InsertNewClient(ClientDetails ClientDetails)
        {
            ClientDetails newClientSave = new ClientDetails();

            try
            {
                ClientDetails.CreateBy = AppUtils.GetLoginEmployeeName();
                ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
                ClientDetails.IsNewClient = newClient;
                ClientDetails.RoleID = AppUtils.ClientRole;
                ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;
                ClientDetails.LineStatusWillActiveInThisDate = DateTime.Now;
                newClientSave = db.ClientDetails.Add(ClientDetails);
                db.SaveChanges();
                if (newClientSave.ClientDetailsID > 0)
                {
                    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                    if ((bool)Session["SMSOptionEnable"])
                    {
                        try
                        {
                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Request && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                                if (sms != null)
                                {
                                    string smsMessage = sms.SendMessageText;
                                    smsMessage = smsMessage.Replace("[CompanyName]", smsSenderIdPass.CompanyName);

                                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass,
                                        ClientDetails.ContactNumber, smsMessage);
                                    if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                    {
                                        sms.SMSCounter += 1;
                                        db.Entry(sms).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        { }
                    }
                    return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetClientDetailsByID(int ClientDetailsID)
        {
            try
            {
                var tns = db.Transaction.Where(s => s.ClientDetailsID == ClientDetailsID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection)
                .Select(x => new { x.TransactionID, x.BillCollectBy, x.PaymentAmount, x.PaymentDate, x.PaymentFrom, x.PaymentType, x.PaymentTypeID });
                var Transaction = tns.Select(s => new { TransactionID = s.TransactionID, PaymentAmount = s.PaymentAmount/*, PaymentDate = s.PaymentDate*/ }).ToList();
                //var sa = Transaction.ToList();
                var cls = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID)
                    .GroupJoin(
                        db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientDetailsID),
                        ClientDetails => ClientDetails.ClientDetailsID, Transactions => Transactions.ClientDetailsID,
                        (ClientDetails, Transactions) => new { ClientDetails = ClientDetails, Transactions = Transactions })
                    .AsEnumerable()
                .Select(
                    s => new
                    {
                        TransactionID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().TransactionID : 0,
                        ClientDetailsID = s.ClientDetails.ClientDetailsID,
                        ThisMonthLineStatusID = s.ClientDetails.StatusThisMonth,
                        NextMonthLineStatusID = s.ClientDetails.StatusNextMonth,
                        Name = s.ClientDetails.Name,
                        Email = s.ClientDetails.Email,
                        LoginName = s.ClientDetails.LoginName,
                        Password = s.ClientDetails.Password,
                        Address = s.ClientDetails.Address,
                        ContactNumber = s.ClientDetails.ContactNumber,
                        ZoneID = s.ClientDetails.ZoneID,
                        SMSCommunication = s.ClientDetails.SMSCommunication,
                        Occupation = s.ClientDetails.Occupation,
                        IsPriorityClient = s.ClientDetails.IsPriorityClient,
                        SocialCommunicationURL = s.ClientDetails.SocialCommunicationURL,
                        Remarks = s.ClientDetails.Remarks,
                        ConnectionTypeID = s.ClientDetails.ConnectionTypeID,
                        BoxNumber = s.ClientDetails.BoxNumber,
                        PopDetails = s.ClientDetails.PopDetails,
                        RequireCable = s.ClientDetails.RequireCable,
                        CableTypeID = s.ClientDetails.CableTypeID,
                        Reference = s.ClientDetails.Reference,
                        NationalID = s.ClientDetails.NationalID,

                        PackageThisMonth = s.ClientDetails.PackageThisMonth,
                        PackageNextMonth = s.ClientDetails.PackageNextMonth,

                        SecurityQuestionID = s.ClientDetails.SecurityQuestionID,
                        SecurityQuestionAnswer = s.ClientDetails.SecurityQuestionAnswer,
                        MacAddress = s.ClientDetails.MacAddress,
                        ClientSurvey = s.ClientDetails.ClientSurvey,
                        ConnectionDate = s.ClientDetails.ConnectionDate,
                        //StatusChangeReason = db.ClientLineStatus.Where(x => x.ClientDetailsID == s.ClientDetails.ClientDetailsID).OrderByDescending(x => x.ClientLineStatusID).FirstOrDefault().StatusChangeReason,
                        LineStatusActiveDate = s.ClientDetails.LineStatusWillActiveInThisDate != null ? s.ClientDetails.LineStatusWillActiveInThisDate.ToString() : "",
                        ClientOWNImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientOwnImageBytesPaths) ? "" : s.ClientDetails.ClientOwnImageBytesPaths,
                        ClientNIDImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientNIDImageBytesPaths) ? "" : s.ClientDetails.ClientNIDImageBytesPaths,
                        ProfileStatusUpdateInPercent = GetProfileUpdatePercent(s.ClientDetails.ProfileUpdatePercentage, s.ClientDetails.ClientDetailsID),

                        MikrotikID = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.MikrotikID.Value : 0,
                        IP = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.IP : "",
                        Mac = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.MikrotikID != null ? s.ClientDetails.Mac : "",

                        ResellerID = s.ClientDetails.ResellerID != null ? s.ClientDetails.ResellerID.Value : 0,

                        PaymentDate = s.ClientDetails.ApproxPaymentDate,

                        //PermanentDiscount = s.ClientDetails.PermanentDiscount
                    }).FirstOrDefault();

                //var cls = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientDetailsID)
                //            .GroupBy(s => s.ClientDetailsID == ClientDetailsID, (key, g) => g.OrderByDescending(e => e.LineStatusChangeDate).FirstOrDefault())
                //            .GroupJoin(
                //                db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.ClientDetailsID == ClientDetailsID),
                //                ClientLineStatus => ClientLineStatus.ClientDetailsID, Transactions => Transactions.ClientDetailsID,
                //                (ClientLineStatus, Transactions) => new { ClientDetails = ClientLineStatus, Transactions = Transactions })
                //            .AsEnumerable()
                //        .Select(
                //            s => new
                //            {
                //                TransactionID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().TransactionID : 0,
                //                ClientDetailsID = s.ClientDetails.ClientDetailsID,
                //                ClientLineStatusID = s.ClientDetails.ClientLineStatusID,
                //                Name = s.ClientDetails.ClientDetails.Name,
                //                Email = s.ClientDetails.ClientDetails.Email,
                //                LoginName = s.ClientDetails.ClientDetails.LoginName,
                //                Password = s.ClientDetails.ClientDetails.Password,
                //                Address = s.ClientDetails.ClientDetails.Address,
                //                ContactNumber = s.ClientDetails.ClientDetails.ContactNumber,
                //                ZoneID = s.ClientDetails.ClientDetails.ZoneID,
                //                SMSCommunication = s.ClientDetails.ClientDetails.SMSCommunication,
                //                Occupation = s.ClientDetails.ClientDetails.Occupation,
                //                IsPriorityClient = s.ClientDetails.ClientDetails.IsPriorityClient,
                //                SocialCommunicationURL = s.ClientDetails.ClientDetails.SocialCommunicationURL,
                //                Remarks = s.ClientDetails.ClientDetails.Remarks,
                //                ConnectionTypeID = s.ClientDetails.ClientDetails.ConnectionTypeID,
                //                BoxNumber = s.ClientDetails.ClientDetails.BoxNumber,
                //                PopDetails = s.ClientDetails.ClientDetails.PopDetails,
                //                RequireCable = s.ClientDetails.ClientDetails.RequireCable,
                //                CableTypeID = s.ClientDetails.ClientDetails.CableTypeID,
                //                Reference = s.ClientDetails.ClientDetails.Reference,
                //                NationalID = s.ClientDetails.ClientDetails.NationalID,

                //                //PackageID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().PackageID : s.ClientDetails.PackageID,//PackageNameNextMonth
                //                PackageThisMonth = s.ClientDetails.ClientDetails.PackageThisMonth,
                //                PackageNextMonth = s.ClientDetails.ClientDetails.PackageNextMonth,

                //                SecurityQuestionID = s.ClientDetails.ClientDetails.SecurityQuestionID,
                //                SecurityQuestionAnswer = s.ClientDetails.ClientDetails.SecurityQuestionAnswer,
                //                MacAddress = s.ClientDetails.ClientDetails.MacAddress,
                //                ClientSurvey = s.ClientDetails.ClientDetails.ClientSurvey,
                //                ConnectionDate = s.ClientDetails.ClientDetails.ConnectionDate,
                //                //LineStatusID = s.Transactions.Count() > 0 ? s.Transactions.FirstOrDefault().LineStatusID : s.ClientDetails.LineStatusID,
                //                LineStatusID = s.ClientDetails.LineStatusID,
                //                StatusChangeReason = s.ClientDetails.StatusChangeReason,
                //                LineStatusActiveDate = s.ClientDetails.LineStatusWillActiveInThisDate.HasValue ? s.ClientDetails.LineStatusWillActiveInThisDate.Value.ToString() : "",
                //                ClientOWNImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientDetails.ClientOwnImageBytesPaths) ? "" : s.ClientDetails.ClientDetails.ClientOwnImageBytesPaths,
                //                ClientNIDImageBytesPaths = string.IsNullOrEmpty(s.ClientDetails.ClientDetails.ClientNIDImageBytesPaths) ? "" : s.ClientDetails.ClientDetails.ClientNIDImageBytesPaths,
                //                ProfileStatusUpdateInPercent = GetProfileUpdatePercent(s.ClientDetails.ClientDetails.ProfileUpdatePercentage, s.ClientDetails.ClientDetailsID),

                //                MikrotikID = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.MikrotikID.Value : 0,
                //                IP = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.IP : "",
                //                Mac = (bool)Session["MikrotikOptionEnable"] && s.ClientDetails.ClientDetails.MikrotikID != null ? s.ClientDetails.ClientDetails.Mac : "",

                //                ResellerID = s.ClientDetails.ClientDetails.ResellerID != null ? s.ClientDetails.ClientDetails.ResellerID.Value : 0,

                //                PaymentDate = s.ClientDetails.ClientDetails.ApproxPaymentDate
                //            }).FirstOrDefault();

                var cableForThisClient = (from cbldis in db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                                          join cblsck in db.CableStock on cbldis.CableStockID equals cblsck.CableStockID into a
                                          from cblscka in a.DefaultIfEmpty()
                                          join cblType in db.CableType on cblscka.CableTypeID equals cblType.CableTypeID into b
                                          from cblTypeb in b.DefaultIfEmpty()

                                          select new
                                          {
                                              CableTypeID = cblTypeb != null ? cblTypeb.CableTypeID : 0,
                                              CableType = cblTypeb != null ? cblTypeb.CableTypeName : "",
                                              AmountOfCableGiven = cbldis.AmountOfCableUsed + " M",
                                          }).GroupBy(s => s.CableTypeID).ToList();

                var itemForThisClient =
                    (from proddis in db.Distribution.Where(s => s.ClientDetailsID == ClientDetailsID)
                     join prod in db.StockDetails on proddis.StockDetailsID equals prod.StockDetailsID into a
                     from clnproddistribution in a.DefaultIfEmpty()

                     join stock in db.Stock on clnproddistribution.StockID equals stock.StockID into b
                     from clnstock in b.DefaultIfEmpty()

                     join items in db.Item on clnstock.ItemID equals items.ItemID into c
                     from clnItems in c.DefaultIfEmpty()

                     select new
                     {
                         ItemID = clnItems.ItemID,
                         ItemName = clnItems != null ? clnItems.ItemName.ToString() : "",
                         ItemSerial = clnproddistribution != null ? clnproddistribution.Serial : "",
                     }).GroupBy(s => s.ItemID).ToList();
                //var ignoreCircularLineStatusResults = AppUtils.IgnoreCircularLoop(cls);
                //var ignoreCircularTnsResults = AppUtils.IgnoreCircularLoop(tns);

                bool ControlNeedToDisable = false;
                if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                {
                    ControlNeedToDisable = true;
                }

                var JSON = Json(new { ControlNeedToDisable = ControlNeedToDisable, Transaction = Transaction.ToList(), ClientLineStatus = cls, CableForThisClient = cableForThisClient, ItemForThisClient = itemForThisClient }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch (Exception ex)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }


        private string GetProfileUpdatePercent(double profileUpdatePercentage, int ClientDetailsID)
        {
            if (profileUpdatePercentage <= 30)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-danger'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else if (profileUpdatePercentage >= 30 && profileUpdatePercentage <= 75)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-warning'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else if (profileUpdatePercentage >= 75 && profileUpdatePercentage < 100)
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-success'> " + profileUpdatePercentage + " %</span>   </a>";
            }
            else
            {
                return "<a href = '#' onclick='ShowProfileListDoneOrNot(" + ClientDetailsID + ")' id = 'ShowProfilePercentUpdate' > <span class='label label-primary'> " + profileUpdatePercentage + " %</span>   </a>";
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetNewClientByID(int ClientDetailsID)
        {

            var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID).Select(s => new
            {
                ClientDetailsID = s.ClientDetailsID,
                Name = s.Name,
                ZoneID = s.ZoneID,
                Address = s.Address,
                LatitudeLongitude = s.LatitudeLongitude,
                PackageID = s.PackageID,
                ConnectionTypeID = s.ConnectionTypeID,
                EmployeeID = s.EmployeeID,
                ContactNumber = s.ContactNumber,
                ClientSurvey = s.ClientSurvey,

                Email = s.Email,
                LoginName = s.LoginName,
                Password = s.Password,
                SMSCommunication = s.SMSCommunication,
                Occupation = s.Occupation,
                SocialCommunicationURL = s.SocialCommunicationURL,
                Remarks = s.Remarks,
                BoxNumber = s.BoxNumber,
                PopDetails = s.PopDetails,
                RequireCable = s.RequireCable,
                Reference = s.Reference,
                NationalID = s.NationalID,
                SecurityQuestionAnswer = s.SecurityQuestionAnswer,
                MacAddress = s.MacAddress,
            });

            //var ClientDetailsIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientDetails);
            var JSON = Json(new { newClient = ClientDetails }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        public ActionResult UpdateClientDetails(Models.ClientDetails ClientClientDetails)
        {

            //$('#tblAllNewClient tbody>tr:eq(' + index + ')').find("td:eq(1)").text(parseEmployee.Name);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee.Zone.ZoneName);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseEmployee.Address);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(parseEmployee.ContactNumber);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(parseEmployee.Package.PackageName);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee.Employee.Name);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee.ClientSurvey);
            //       $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(8)").html(parseEmployee.CreatedTime);


            ClientDetails ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();
            try
            {
                setDBNewClientToBrowserClient(ref ClientClientDetails, ClientDetailsForUpdate);
                ClientClientDetails.IsNewClient = newClient;
                ClientClientDetails.RoleID = AppUtils.ClientRole;
                ClientClientDetails.LineStatusWillActiveInThisDate = ClientDetailsForUpdate.LineStatusWillActiveInThisDate;
                ClientClientDetails.ApproxPaymentDate = ClientDetailsForUpdate.ApproxPaymentDate;

                if (ClientClientDetails.ClientDetailsID > 0)
                {
                    db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
                    // db.Entry(ClientDetailsForUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    var cTemp = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsForUpdate.ClientDetailsID).Select(s => new
                    {
                        ClientDetailsID = s.ClientDetailsID,
                        Name = s.Name,
                        ZoneName = s.Zone.ZoneName,
                        Address = s.Address,
                        ContactNumber = s.ContactNumber,
                        PackageName = s.Package.PackageName,
                        ClientSurvey = s.ClientSurvey,
                        EmployeeName = s.Employee.Name,
                        ConnectionTypeID = s.ConnectionTypeID,
                        EmployeeID = s.EmployeeID,
                        CreatedTime = s.CreateDate,
                        UpdateTime = s.UpdateDate,
                        UpdateBy = s.UpdateBy
                    }); ;

                    // var ClientDetailsIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientDetailsForUpdate);

                    var JSON = Json(new { UpdateStatus = true, ClientDetails = cTemp }, JsonRequestBehavior.AllowGet);
                    JSON.MaxJsonLength = int.MaxValue;
                    return JSON;
                }
                else
                {
                    return Json(new { UpdateStatus = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { UpdateStatus = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult UpdateNewClientSignUpDetails(Models.ClientDetails ClientClientDetails)
        //{
        //    ClientDetails ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();
        //    try
        //    {
        //        setDBNewClientToBrowserClient(ref ClientClientDetails, ClientDetailsForUpdate);
        //        ClientClientDetails.IsNewClient = newClientStatusChangeToOldClient;

        //        if (ClientClientDetails.ClientDetailsID > 0)
        //        {
        //            db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
        //            // db.Entry(ClientDetailsForUpdate).State = EntityState.Modified;
        //            db.SaveChanges();

        //            var ClientDetailsIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientDetailsForUpdate);

        //            var JSON = Json(new { UpdateStatus = true, ClientDetails = ClientDetailsIgnoreLoop }, JsonRequestBehavior.AllowGet);
        //            JSON.MaxJsonLength = int.MaxValue;
        //            return JSON;
        //        }
        //        else
        //        {
        //            return Json(new { UpdateStatus = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { UpdateStatus = false, ClientDetails = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        private double GetProfileUpdateInPercentage(ClientDetails clientClientDetails, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage)
        {
            var lstOfProfilePercentCalculationRow = db.ProfilePercentageFields.AsEnumerable();
            double perPointValue = 100 / lstOfProfilePercentCalculationRow.Count();
            double totalPoint = 0;
            foreach (var item in lstOfProfilePercentCalculationRow)
            {
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsClientPhoto)
                {
                    if (ClientOwnImageBytes != null || !string.IsNullOrEmpty(clientClientDetails.ClientOwnImageBytesPaths))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalID)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.NationalID))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsNationalIDImage)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.ClientNIDImageBytesPaths) || ClientNIDImage != null)
                    {
                        totalPoint += perPointValue;
                    }

                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsAddress)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.Address))
                    {
                        totalPoint += perPointValue;
                    }
                }
                if (item.FieldsName == AppUtils.ProfileUpdateInPercentagePointIsMobileNo)
                {
                    if (!string.IsNullOrEmpty(clientClientDetails.ContactNumber))
                    {
                        totalPoint += perPointValue;
                    }
                }
            }
            return totalPoint;
        }

        private void SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails clientDetails, string WhichPic, HttpPostedFileBase image)
        {
            if (!IsValidContentType(image.ContentType))
            {
                ViewBag.Error = "Only PNG image are allowed";
            }
            else if (!IsValidContentLength(image.ContentLength))
            {
                ViewBag.ErrorFileTooLarge = "Your file is too large.";
            }

            byte[] imagebyte = null;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(image.FileName);
            string extension = Path.GetExtension(image.FileName);
            var fileName = clientDetails.ClientDetailsID + "_" + WhichPic + "" + extension;

            string fileSaveInFolder = Path.Combine(Server.MapPath("~/Images/ClientsImage"), fileName);
            image.SaveAs(fileSaveInFolder);


            BinaryReader reader = new BinaryReader(image.InputStream);
            imagebyte = reader.ReadBytes(image.ContentLength);

            Image returnImage = byteArrayToImage(imagebyte);
            Bitmap bp = ResizeImage(returnImage, 200, 200);
            imagebyte = imageToByteArray(bp);

            if (WhichPic == "NID")
            {
                //clientDetails.image = fileName;
                clientDetails.ClientNIDImageBytes = imagebyte;
                clientDetails.ClientNIDImageBytesPaths = "/Images/ClientsImage/" + fileName;

            }
            else if (WhichPic == "OWN")
            {
                //clientDetails.image = fileName;
                clientDetails.ClientOwnImageBytes = imagebyte;
                clientDetails.ClientOwnImageBytesPaths = "/Images/ClientsImage/" + fileName;
            }
        }

        private void GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction Transaction, ClientDetails ClientDetailsSave, ClientDetails ClientDetails, int ResellerID)
        {
            Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
            Transaction.IsNewClient = AppUtils.isNewClient;
            if (ResellerID > 0)
            {
                Transaction.ResellerID = ResellerID;

            }
            else
            {
                Transaction.EmployeeID = AppUtils.GetLoginUserID();
            }

            //Transaction.RemarksNo = "RNEW" + RemarksNo();
            //Transaction.ResetNo = "RNEW" + SerialNo();


            Transaction.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
            Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow();
            Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
            Transaction.PackageID = ClientDetails.PackageID;
            Transaction.BillCollectBy = AppUtils.GetLoginUserID();
            Transaction.PaymentFromWhichPage = "Create";
            //Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
            Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
                                                                //Transaction.PaymentGenerateDate = AppUtils.GetDateTimeNow();
                                                                //Transaction.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);
        }


        private PaymentHistory UpdatePaymentIntoPaymentHistoryForClientCreate(string resetNo, Transaction ts)
        {
            PaymentHistory ph = new PaymentHistory();
            ph.TransactionID = ts.TransactionID;
            ph.ClientDetailsID = ts.ClientDetailsID;
            ph.EmployeeID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.CollectByID = int.Parse(Session["LoggedUserID"].ToString())/*AppUtils.LoginUserID*/;
            ph.PaymentDate = ts.PaymentDate.Value;
            ph.PaidAmount = ts.PaymentAmount.Value;
            ph.ResetNo = resetNo;
            ph.Status = AppUtils.TableStatusIsActive;
            db.PaymentHistory.Add(ph);
            db.SaveChanges();
            return ph;
        }

        private void GetRegularMonthlyBillDuringClientCreate(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave)
        {
            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
            int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);
            bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
            int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0;//AppUtils.ChangePackageHowMuchTimes;
            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            //if (!AppUtils.BillIsCycleWise)
            //{
            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();
            //}
            //else
            //{
            //    forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);
            //}

            double packagePricePerday = 0;
            int DaysRemains = 0;
            double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;

            // here we are taking from config that we have a day settings or not. like how much day for a month? like 30 or any other day like 28 Or We will continue with the regular month value.
            if (regularMonthlyBase == true)
            {
                //packagePricePerday = (MainPackagePrice / totalDaysInThisMonth);
                packagePricePerday = (MainPackagePrice / Totaldays);
                DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            }
            else
            {
                int getDayForBillGenerate = int.Parse(ConfigurationManager.AppSettings["CountDate"]);
                packagePricePerday = (MainPackagePrice / getDayForBillGenerate);
                DaysRemains = Convert.ToInt32(getDayForBillGenerate - currenDateTime.Day);
            }
            // done for package price and day remains depend on day settings.

            string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
                            : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
                            : "0";
            float tmp = 0;
            float.TryParse(amount, out tmp);
            //////forMonthlyBill.PaymentAmount = tmp;
            //////thisMonthFee = tmp;
            forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp) - (float?)Transaction.PermanentDiscount;
            forMonthlyBill.PermanentDiscount = Transaction.PermanentDiscount;

            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = tmp - Transaction.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            //if (AppUtils.BillIsCycleWise)
            //{
            //    forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            //}
        }

        private void SetClientLineStatusDuringClientCreate(ref ClientLineStatus ClientLineStatus, ClientDetails ClientDetailsSave, int ResellerID = 0)
        {
            ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            ClientLineStatus.ClientDetailsID = ClientDetailsSave.ClientDetailsID;
            ClientLineStatus.PackageID = ClientDetailsSave.PackageID;
            ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
            if (ResellerID > 0)
            {
                ClientLineStatus.ResellerID = ResellerID;
            }
            else
            {
                ClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
            }
            ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
            ClientLineStatus.StatusChangeReason = "New Connection";

            DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

            ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
        }

        private void GetRegularMonthlyBillDuringClientCreateIfBillIsCycleWise(ref Transaction forMonthlyBill, ref double thisMonthFee, Transaction Transaction, Transaction TransactonSave, ClientDetails ClientDetails, ClientDetails ClientDetailsSave)
        {
            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            forMonthlyBill.ChangePackageHowMuchTimes = 0; //AppUtils.ChangePackageHowMuchTimes; ;

            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            forMonthlyBill.EmployeeID = AppUtils.GetLoginUserID();
            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            forMonthlyBill.PackageID = Transaction.PackageID;
            //// forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;

            forMonthlyBill.AmountCountDate = new DateTime(AppUtils.dateTimeNow.Year, AppUtils.dateTimeNow.Month, ClientDetails.ApproxPaymentDate);

            double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;

            forMonthlyBill.PaymentAmount = (float)MainPackagePrice - (float)Transaction.PermanentDiscount;
            forMonthlyBill.PermanentDiscount = Transaction.PermanentDiscount;


            int paidDirect = int.Parse(ConfigurationManager.AppSettings["AutoBillPayDuringCreate"]);
            if (paidDirect == 1)
            {
                forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                forMonthlyBill.BillCollectBy = AppUtils.GetLoginUserID();
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                forMonthlyBill.PaidAmount = forMonthlyBill.PaymentAmount;
                forMonthlyBill.DueAmount = 0;
            }
            else
            {
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.PaidAmount = 0;
                forMonthlyBill.DueAmount = forMonthlyBill.PaymentAmount;
            }

            thisMonthFee = MainPackagePrice - Transaction.PermanentDiscount;
            //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

            //forMonthlyBill.PaymentGenerateDate = AppUtils.GetDateTimeNow();
            //forMonthlyBill.NextGenerateDate = AppUtils.GetDateTimeNow().AddMonths(1);

            if (AppUtils.BillIsCycleWise)
            {
                forMonthlyBill.TransactionForWhichCycle = ClientDetailsSave.RunningCycle;
            }
        }



        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/jpeg");
        }

        private bool IsValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1 MB
        }

        [HttpPost]
        //public ActionResult InsertNewClientSignUpDetails(ClientDetails ClientDetails, Transaction Transaction, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign)
        public ActionResult InsertNewClientSignUpDetails(FormCollection file, HttpPostedFileBase ClientOwnImageBytes, HttpPostedFileBase ClientNIDImage/*, ClientDetails ClientDetails, Transaction Transaction, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign*/)
        {
            #region old New client Entry
            //ITikConnection connection;
            //if ((bool)Session["MikrotikOptionEnable"])
            //{/////////////////////////////////// need to set message in manager for new client 
            //    try
            //    {
            //        Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
            //        connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

            //        var checkUserExistOrNot = connection.CreateCommandAndParameters("/ppp/secret/print", "name", ClientDetails.LoginName).ExecuteList();
            //        if (checkUserExistOrNot.Count() > 0)
            //        {
            //            return Json(new { MikrotikSuccess = false, AlreadyAddedLoginNameInMikrotik = true }, JsonRequestBehavior.AllowGet);
            //        }

            //        Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
            //        InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
            //    }
            //    catch (Exception e)
            //    {
            //        var code = e.HResult;
            //        //-2146233088
            //        return Json(new { MikrotikSuccess = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            //        throw;
            //    }
            //}



            //double thisMonthFee = 0;
            //ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            //if (clientLoginNameExistOrNot != null)
            //{
            //    return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            //}

            //ClientDetails ClientDetailsSave = new ClientDetails();
            //Transaction TransactonSave = new Transaction();
            //ClientLineStatus ClientLineStatusSave = new ClientLineStatus();

            //if (ItemListForEmployee != null)
            //{
            //    List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

            //    List<string> lstStockDetails =
            //        db.StockDetails.Where(
            //            s =>
            //                lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
            //                (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
            //                s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

            //    if (lstStockDetails.Count > 0)
            //    {
            //        return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //if (ClientCableAssign != null)
            //{
            //    bool duplicateCableStockID = false;
            //    string cableBoxName = "";
            //    var lenghtGreaterThanCableAmount = false;
            //    var greaterBoxNameList = "";
            //    int cSID = 0;

            //    List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
            //    foreach (var item in cableStockID)
            //    {
            //        List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
            //        if (duplicateCableStockIDExistOrNot.Count > 1)
            //        {
            //            cSID = duplicateCableStockIDExistOrNot[0];
            //            duplicateCableStockID = true;
            //            cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
            //        }
            //    }
            //    foreach (var cable in ClientCableAssign)
            //    {
            //        CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
            //        if (cableStock != null)
            //        {
            //            if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
            //            {
            //                lenghtGreaterThanCableAmount = true;
            //                greaterBoxNameList += " " + cableStock.CableBoxName;
            //            }
            //        }
            //    }
            //    if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
            //    {
            //        return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            //try
            //{
            //    ClientDetails.IsNewClient = newClientStatusChangeToOldClient;
            //    ClientDetails.RoleID = AppUtils.ClientRole;
            //    ClientDetails.CreateBy = AppUtils.GetLoginEmployeeName();
            //    ClientDetails.CreateDate = AppUtils.GetDateTimeNow();


            //    ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
            //    ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

            //    ClientDetails.PackageNextMonth = ClientDetails.PackageID.Value;
            //    ClientDetails.PackageNextMonth = ClientDetails.PackageID.Value;
            //    ClientDetails.StatusThisMonth = AppUtils.LineIsActive;
            //    ClientDetails.StatusNextMonth = AppUtils.LineIsActive;

            //    if (AppUtils.BillIsCycleWise)
            //    {
            //        ClientDetails.RunningCycle = "1";
            //    }

            //    ClientDetails forUpdateClientFromNewClient = db.ClientDetails
            //        .Where(s => s.ClientDetailsID == ClientDetails.ClientDetailsID).FirstOrDefault();
            //    db.Entry(forUpdateClientFromNewClient).CurrentValues.SetValues(ClientDetails);
            //    db.SaveChanges();


            //    //ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
            //    //db.SaveChanges();
            //    if (forUpdateClientFromNewClient.ClientDetailsID > 0)
            //    {

            //        //Transaction.RemarksNo = "RNEW" + RemarksNo();
            //        //Transaction.ResetNo = "RNEW" + SerialNo();


            //        Transaction.PaymentDate = AppUtils.GetDateTimeNow();
            //        Transaction.EmployeeID = AppUtils.GetLoginUserID();
            //        //Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //        Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
            //        Transaction.IsNewClient = AppUtils.isNewClient;
            //        Transaction.BillCollectBy = AppUtils.GetLoginUserID();
            //        Transaction.ClientDetailsID = forUpdateClientFromNewClient.ClientDetailsID;
            //        //Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
            //        Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //        Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
            //        Transaction.PackageID = ClientDetails.PackageID;
            //        Transaction.PaymentFromWhichPage = AppUtils.PamentIsOccouringFromNewClientSignUpPage.ToString();

            //        ////Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
            //        Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
            //        TransactonSave = db.Transaction.Add(Transaction);
            //        db.SaveChanges();
            //        if (Transaction.TransactionID > 0)
            //        {
            //            DateTime currenDateTime = AppUtils.GetDateTimeNow();
            //            int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);

            //            Transaction forMonthlyBill = new Transaction();

            //            //forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
            //            //forMonthlyBill.ResetNo = "RNEW" + SerialNo();
            //            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
            //            //         forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
            //            //     forMonthlyBill.EmployeeID = AppUtils.LoginUserID;
            //            //forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
            //            forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
            //            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
            //            //      forMonthlyBill.BillCollectBy = AppUtils.LoginUserID;
            //            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
            //            //forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
            //            forMonthlyBill.WhoGenerateTheBill = AppUtils.GetLoginUserID();
            //            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
            //            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
            //            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
            //            forMonthlyBill.PackageID = Transaction.PackageID;
            //            ////forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
            //            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
            //            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();
            //            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;


            //            double packagePricePerday = (db.Package.Find(Transaction.PackageID).PackagePrice / totalDaysInThisMonth);
            //            int DaysRemains = Convert.ToInt32((AppUtils.ThisMonthLastDate().Date - currenDateTime.Date).TotalDays) + 1;
            //            string amount = (packagePricePerday * DaysRemains).ToString();
            //            float tmp = 0;
            //            float.TryParse(amount, out tmp);
            //            forMonthlyBill.PaymentAmount = tmp;
            //            thisMonthFee = tmp;
            //            db.Transaction.Add(forMonthlyBill);
            //            db.SaveChanges();

            //            ClientLineStatus ClientLineStatus = new ClientLineStatus();
            //            //ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
            //            ClientLineStatus.ClientDetailsID = ClientDetails.ClientDetailsID;
            //            ClientLineStatus.PackageID = ClientDetails.PackageID;
            //            ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
            //            ClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
            //            ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
            //            ClientLineStatus.StatusChangeReason = "New Connection";


            //            DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

            //            ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
            //            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
            //            db.SaveChanges();
            //        }
            //    }
            //    //******************************** Assigning Item For EMployee****************************
            //    if (ClientDetails.ClientDetailsID > 0)
            //    {
            //        if (ItemListForEmployee != null)
            //        {
            //            foreach (var item in ItemListForEmployee)
            //            {
            //                StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

            //                if (stockDetails != null)
            //                {
            //                    stockDetails.SectionID = AppUtils.WorkingSection;
            //                    stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
            //                    stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
            //                    stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
            //                    db.Entry(stockDetails).State = EntityState.Modified;
            //                    db.SaveChanges();

            //                    Distribution distribution = new Distribution();
            //                    SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
            //                    db.Distribution.Add(distribution);
            //                    db.SaveChanges();
            //                }
            //            }

            //        }
            //        if (ClientCableAssign != null)
            //        {
            //            foreach (var cableUsedFromClient in ClientCableAssign)
            //            {
            //                CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

            //                if (CableStock != null)
            //                {
            //                    CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
            //                    CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
            //                    CableStock.UpdateDate = AppUtils.GetDateTimeNow();
            //                    db.Entry(CableStock).State = EntityState.Modified;
            //                    db.SaveChanges();

            //                    CableDistribution CableDistribution = new CableDistribution();
            //                    SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
            //                    CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
            //                    CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            //                    CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
            //                    db.CableDistribution.Add(CableDistribution);
            //                    db.SaveChanges();
            //                }
            //            }

            //            // List<>
            //        }
            //    }

            //    //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //    if ((bool)Session["SMSOptionEnable"])
            //    {
            //        try
            //        {
            //            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
            //            if (smsSenderIdPass != null)
            //            {
            //                SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
            //                if (sms != null)
            //                {
            //                    var message = sms.SendMessageText;
            //                    Package package = db.Package.Find(ClientDetails.PackageID);
            //                    message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
            //                    message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
            //                    message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
            //                    message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

            //                    SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
            //                    if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
            //                    {
            //                        sms.SMSCounter += 1;
            //                        db.Entry(sms).State = EntityState.Modified;
            //                        db.SaveChanges();
            //                    }
            //                }
            //            }

            //        }
            //        catch (Exception ex)
            //        { }
            //    }
            //    /////*****************************************************************************************************
            //    return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave);
            //    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            //}
            #endregion



            #region Initialization And Client Given Information
            ClientDetails ClientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(file["ClientDetails"]).FirstOrDefault();
            Transaction Transaction = JsonConvert.DeserializeObject<Transaction>(file["Transaction"]);
            List<ClientStockAssign> ItemListForEmployee = JsonConvert.DeserializeObject<List<ClientStockAssign>>(file["ItemListForEmployee"]);
            List<ClientCableAssign> ClientCableAssign = JsonConvert.DeserializeObject<List<ClientCableAssign>>(file["ClientCableAssign"]);
            PaymentHistory ph = new PaymentHistory();
            ClientDetails ClientDetailsSave = new ClientDetails();
            Transaction TransactonSave = new Transaction();
            ClientLineStatus ClientLineStatusSave = new ClientLineStatus();
            double thisMonthFee = 0;
            int ResellerID = 0;
            #endregion

            #region Check Pre Condition And Return
            ClientDetails clientLoginNameExistOrNot = db.ClientDetails.Where(s => s.LoginName == ClientDetails.LoginName).FirstOrDefault();
            if (clientLoginNameExistOrNot != null)
            {
                return Json(new { Success = false, LoginNameExist = true }, JsonRequestBehavior.AllowGet);
            }

            ClientDetails clientDetailsForStatusDeleteCauseOfNewInformation = db.ClientDetails.Find(ClientDetails.ClientDetailsID);
            clientDetailsForStatusDeleteCauseOfNewInformation.Status = AppUtils.TableStatusIsDelete;
            db.Entry(clientDetailsForStatusDeleteCauseOfNewInformation).State = EntityState.Modified;
            db.SaveChanges();

            //ClientDetails.ApproxPaymentDate = AppUtils.ApproxPaymentDate;

            if (ItemListForEmployee != null)
            {
                List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

                List<string> lstStockDetails =
                    db.StockDetails.Where(
                        s =>
                            lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
                            (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
                            s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

                if (lstStockDetails.Count > 0)
                {
                    return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
                }
            }

            if (ClientCableAssign != null)
            {
                bool duplicateCableStockID = false;
                string cableBoxName = "";
                var lenghtGreaterThanCableAmount = false;
                var greaterBoxNameList = "";
                int cSID = 0;

                List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
                foreach (var item in cableStockID)
                {
                    List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
                    if (duplicateCableStockIDExistOrNot.Count > 1)
                    {
                        cSID = duplicateCableStockIDExistOrNot[0];
                        duplicateCableStockID = true;
                        cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
                    }
                }
                foreach (var cable in ClientCableAssign)
                {
                    CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
                    if (cableStock != null)
                    {
                        if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
                        {
                            lenghtGreaterThanCableAmount = true;
                            greaterBoxNameList += " " + cableStock.CableBoxName;
                        }
                    }
                }
                if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
                {
                    return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
                }
            }
            #endregion

            #region First Mikrotik Issues Solved Like Client Insert Or Update Client In Mikrotik
            ITikConnection connection;
            if ((bool)Session["MikrotikOptionEnable"])
            {
                try
                {
                    Mikrotik mikrotik = db.Mikrotik.Where(s => s.MikrotikID == ClientDetails.MikrotikID.Value).FirstOrDefault();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    if (MikrotikLB.UserIDExistOrNotInMikrotik(connection, ClientDetails))
                    {
                        MikrotikLB.UpdateMikrotikUserBySingleSingleData(connection, ClientDetails.LoginName, ClientDetails.Password, ClientDetails.PackageID.Value);
                    }
                    else
                    {
                        Package packageSearch = db.Package.Where(s => s.PackageID == ClientDetails.PackageID).FirstOrDefault();
                        InsertClientDetailsInMikrotik(connection, ClientDetails, packageSearch);
                    }

                }
                catch (Exception e)
                {
                    var code = e.HResult;
                    //-2146233088
                    return Json(new { Success = false, UserAddInMikrotik = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
                    throw;
                }
            }
            #endregion 

            try
            {
                #region adding client in client details table and image in image folder
                ClientDetails.CreateBy = AppUtils.GetLoginEmployeeName();
                ClientDetails.CreateDate = AppUtils.GetDateTimeNow();
                ClientDetails.RoleID = AppUtils.ClientRole;
                ////newreseller//
                //if (AppUtils.GetLoginRoleID() == AppUtils.ResellerRole)
                //{
                //    ClientDetails.ResellerID = AppUtils.GetLoginUserID();
                //}
                ////newreseller//

                double profileUpdateInPercentage = GetProfileUpdateInPercentage(ClientDetails, ClientOwnImageBytes, ClientNIDImage);
                ClientDetails.ProfileUpdatePercentage = profileUpdateInPercentage;

                ClientDetails.LineStatusWillActiveInThisDate = AppUtils.GetDateNow().AddMonths(1);
                ClientDetails.NextApproxPaymentFullDate = AppUtils.GetDateNow().AddMonths(1);

                if (AppUtils.BillIsCycleWise)
                {
                    ClientDetails.RunningCycle = "1";
                }

                ClientDetailsSave = db.ClientDetails.Add(ClientDetails);
                db.SaveChanges();

                ////// Now Saving The Image Data Into Folder And Path
                if (ClientNIDImage != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsNID, ClientNIDImage);
                }
                if (ClientOwnImageBytes != null)
                {
                    SaveImageInFolderAndAddInformationInDVDSTable(ref ClientDetails, AppUtils.ImageIsOWN, ClientOwnImageBytes);
                }
                db.SaveChanges();
                ////////////////////////////////////////////////////
                #endregion

                #region Finishing Add Or Update package in Transaction Table 
                if (!AppUtils.BillIsCycleWise)
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreate(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (ClientDetailsSave.ClientDetailsID > 0)
                    {
                        // saving information for sign up bill in transaction table
                        GetTransactionInformationForSignUpMoneyDuringClientCreate(ref Transaction, ClientDetailsSave, ClientDetails, ResellerID);
                        TransactonSave = db.Transaction.Add(Transaction);
                        db.SaveChanges();

                        // saving payment information in Payment History Table
                        //if (Transaction.PaymentAmount > 0)
                        {
                            ph = UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
                        }

                        if (Transaction.TransactionID > 0)
                        {
                            Transaction forMonthlyBill = new Transaction();
                            GetRegularMonthlyBillDuringClientCreateIfBillIsCycleWise(ref forMonthlyBill, ref thisMonthFee, Transaction, TransactonSave, ClientDetails, ClientDetailsSave);

                            db.Transaction.Add(forMonthlyBill);
                            db.SaveChanges();

                            ClientLineStatus ClientLineStatus = new ClientLineStatus();
                            SetClientLineStatusDuringClientCreate(ref ClientLineStatus, ClientDetailsSave, ResellerID);
                            ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Update Client Details table And Client Line Status And Send SMS And Return
                //******************************** Assigning Item For EMployee****************************
                if (ClientDetails.ClientDetailsID > 0)
                {
                    if (ItemListForEmployee != null)
                    {
                        foreach (var item in ItemListForEmployee)
                        {
                            StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

                            if (stockDetails != null)
                            {
                                stockDetails.SectionID = AppUtils.WorkingSection;
                                stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
                                stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
                                stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
                                db.Entry(stockDetails).State = EntityState.Modified;
                                db.SaveChanges();

                                Distribution distribution = new Distribution();
                                SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetails);
                                db.Distribution.Add(distribution);
                                db.SaveChanges();
                            }
                        }

                    }
                    if (ClientCableAssign != null)
                    {
                        foreach (var cableUsedFromClient in ClientCableAssign)
                        {
                            CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

                            if (CableStock != null)
                            {
                                CableStock.UsedQuantityFromThisBox += cableUsedFromClient.CableQuantity;
                                CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
                                CableStock.UpdateDate = AppUtils.GetDateTimeNow();
                                db.Entry(CableStock).State = EntityState.Modified;
                                db.SaveChanges();

                                CableDistribution CableDistribution = new CableDistribution();
                                SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetails);
                                CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
                                CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
                                CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
                                db.CableDistribution.Add(CableDistribution);
                                db.SaveChanges();
                            }
                        }

                        // List<>
                    }
                }
                #endregion

                #region SMS send Regionif ((bool)Session["SMSOptionEnable"])
                {
                    try
                    {
                        SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                        if (smsSenderIdPass != null)
                        {
                            SMS sms = db.SMS.Where(s => s.SMSCode == AppUtils.New_Client_Signup && s.SMSStatus == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                            if (sms != null)
                            {
                                var message = sms.SendMessageText;
                                Package package = db.Package.Find(ClientDetails.PackageID);
                                message = message.Replace("[NAME]", ClientDetails.Name); message = message.Replace("[LOGIN-NAME]", ClientDetails.LoginName);
                                message = message.Replace("[LOGIN-PASSWORD]", ClientDetails.Password); message = message.Replace("[PACKAGE]", package.PackageName);
                                message = message.Replace("[BANDWITH]", package.BandWith); message = message.Replace("[MONTHLY-FEE]", Math.Round(thisMonthFee, 2).ToString());
                                message = message.Replace("[SIGNUP-FEE]", Transaction.PaymentAmount.ToString()); message = message.Replace("[SUPPORT-1]", smsSenderIdPass.HelpLine);

                                SMSReturnDetails SMSReturnDetails = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, ClientDetails.ContactNumber, message);
                                if (SMSReturnDetails.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {
                                    sms.SMSCounter += 1;
                                    db.Entry(sms).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                #endregion

                /////*****************************************************************************************************
                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();

                return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave, ph);
                return Json(new { SuccessInsert = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        private void InsertClientDetailsInMikrotik(ITikConnection connection, ClientDetails clientDetails, Package packageSearch)
        {
            //// add user 
            var userCerate = connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", "pppoe", "profile", packageSearch.PackageName.Trim());
            userCerate.ExecuteNonQuery();

        }


        public int RemarksNo()
        {
            Remarks remarks = db.Remarks.Find(1);
            remarks.RemarksNo += 1;
            db.SaveChanges();
            return remarks.RemarksNo - 1;
        }

        public int SerialNo()
        {
            Serial serial = db.Serial.Find(1);
            serial.SerialNo += 1;
            db.SaveChanges();
            return serial.SerialNo - 1;
        }
        private void DeleteClientDetails_Transaction_Status(ClientLineStatus ClientLineStatusSave, Transaction TransactonSave, ClientDetails ClientDetailsSave)
        {
            if (ClientLineStatusSave.ClientLineStatusID > 0)
            {
                ClientLineStatus ClienLineStatusDelete = db.ClientLineStatus.Find(ClientLineStatusSave.ClientLineStatusID);
                db.ClientLineStatus.Remove(ClienLineStatusDelete);
                db.SaveChanges();
            }
            if (TransactonSave.TransactionID > 0)
            {
                Transaction TransactionDelete = db.Transaction.Find(TransactonSave.TransactionID);
                db.Transaction.Remove(TransactionDelete);
                db.SaveChanges();
            }
            if (ClientDetailsSave.ClientDetailsID > 0)
            {
                ClientDetails ClientDetailsDelete = db.ClientDetails.Find(ClientDetailsSave.ClientDetailsID);
                db.ClientDetails.Remove(ClientDetailsDelete);
                db.SaveChanges();
            }
        }

        private void DeleteClientDetails_Transaction_Status(ClientLineStatus ClientLineStatusSave, Transaction TransactonSave, ClientDetails ClientDetailsSave, PaymentHistory ph)
        {
            if (ClientLineStatusSave.ClientLineStatusID > 0)
            {
                ClientLineStatus ClienLineStatusDelete = db.ClientLineStatus.Find(ClientLineStatusSave.ClientLineStatusID);
                db.ClientLineStatus.Remove(ClienLineStatusDelete);
                db.SaveChanges();
            }
            if (ph.PaymentHistoryID > 0)
            {
                PaymentHistory paymentHistory = db.PaymentHistory.Find(ph.PaymentHistoryID);
                db.PaymentHistory.Remove(paymentHistory);
                db.SaveChanges();
            }
            if (TransactonSave.TransactionID > 0)
            {
                Transaction TransactionDelete = db.Transaction.Find(TransactonSave.TransactionID);
                db.Transaction.Remove(TransactionDelete);
                db.SaveChanges();
            }
            if (ClientDetailsSave.ClientDetailsID > 0)
            {
                ClientDetails ClientDetailsDelete = db.ClientDetails.Find(ClientDetailsSave.ClientDetailsID);
                db.ClientDetails.Remove(ClientDetailsDelete);
                db.SaveChanges();
            }
        }


        //[HttpPost]
        //public ActionResult InsertNewClientSignUpDetails(Models.ClientDetails ClientClientDetails, Transaction ClientTransaction, ClientLineStatus ClientClientLineStatus, List<ClientStockAssign> ItemListForEmployee, List<ClientCableAssign> ClientCableAssign)
        //{
        //    ClientDetails ClientDetailsForUpdate = db.ClientDetails.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).FirstOrDefault();
        //    Transaction TransactonSave = new Transaction();
        //    ClientLineStatus ClientLineStatusSave = new ClientLineStatus();

        //    try
        //    {

        //        //*****************************************************************************
        //        if (ItemListForEmployee != null)
        //        {
        //            List<int> lstStockDetailsListFromClient = ItemListForEmployee.Select(s => s.StockDetailsID).ToList();

        //            List<string> lstStockDetails =
        //                db.StockDetails.Where(
        //                    s =>
        //                        lstStockDetailsListFromClient.Contains(s.StockDetailsID) &&
        //                        (s.ProductStatusID != AppUtils.ProductStatusIsAvialable ||
        //                        s.SectionID != AppUtils.StockSection)).Select(s => s.Serial).ToList();

        //            if (lstStockDetails.Count > 0)
        //            {
        //                return Json(new { Success = false, SerialAlreadyAdded = true, SerialList = lstStockDetails }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        if (ClientCableAssign != null)
        //        {
        //            bool duplicateCableStockID = false;
        //            string cableBoxName = "";
        //            var lenghtGreaterThanCableAmount = false;
        //            var greaterBoxNameList = "";
        //            int cSID = 0;

        //            List<int> cableStockID = ClientCableAssign.Select(s => s.CableStockID).Distinct().ToList();
        //            foreach (var item in cableStockID)
        //            {
        //                List<int> duplicateCableStockIDExistOrNot = ClientCableAssign.Where(s => s.CableStockID == item).Select(s => s.CableStockID).ToList();
        //                if (duplicateCableStockIDExistOrNot.Count > 1)
        //                {
        //                    cSID = duplicateCableStockIDExistOrNot[0];
        //                    duplicateCableStockID = true;
        //                    cableBoxName += " " + db.CableStock.Where(s => s.CableStockID == cSID).Select(s => s.CableBoxName).FirstOrDefault();
        //                }
        //            }
        //            foreach (var cable in ClientCableAssign)
        //            {
        //                CableStock cableStock = db.CableStock.Where(s => s.CableStockID == cable.CableStockID).FirstOrDefault();
        //                if (cableStock != null)
        //                {
        //                    if (cable.CableQuantity > (cableStock.CableQuantity - cableStock.UsedQuantityFromThisBox))
        //                    {
        //                        lenghtGreaterThanCableAmount = true;
        //                        greaterBoxNameList += " " + cableStock.CableBoxName;
        //                    }
        //                }
        //            }
        //            if (duplicateCableStockID == true || lenghtGreaterThanCableAmount == true)
        //            {
        //                return Json(new { Success = false, DuplicateCableStockID = duplicateCableStockID, CableBoxName = cableBoxName, LenghtGreaterThanCableAmount = lenghtGreaterThanCableAmount, GreaterBoxNameList = greaterBoxNameList }, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //        //**************************************************************

        //        setDBNewClientToBrowserClient(ref ClientClientDetails, ClientDetailsForUpdate);
        //        ClientClientDetails.IsNewClient = newClientStatusChangeToOldClient;
        //        ClientClientDetails.RoleID = AppUtils.ClientRole;

        //        if (ClientClientDetails.ClientDetailsID > 0)
        //        {
        //            db.Entry(ClientDetailsForUpdate).CurrentValues.SetValues(ClientClientDetails);
        //            db.SaveChanges();
        //        }
        //        if (ClientDetailsForUpdate.ClientDetailsID > 0)
        //        {
        //            ClientTransaction.ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID;
        //            ClientTransaction.PaymentFrom = AppUtils.PaymentByHandCash;
        //            ClientTransaction.EmployeeID = AppUtils.LoginUserID ;
        //            ClientTransaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;

        //            TransactonSave = db.Transaction.Add(ClientTransaction);
        //            db.SaveChanges();
        //            if (ClientTransaction.TransactionID > 0)
        //            {
        //                ClientLineStatus ClientLineStatus = new ClientLineStatus();
        //                ClientLineStatus.ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID;
        //                ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
        //                ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
        //                ClientLineStatus.PackageID = ClientClientDetails.PackageID;
        //                ClientLineStatus.EmployeeID = AppUtils.LoginUserID;

        //                ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
        //                db.SaveChanges();
        //            }
        //        }




        //        if (ClientDetailsForUpdate.ClientDetailsID > 0)
        //        {
        //            if (ItemListForEmployee != null)
        //            {
        //                foreach (var item in ItemListForEmployee)
        //                {
        //                    StockDetails stockDetails = db.StockDetails.Where(s => s.StockDetailsID == item.StockDetailsID).FirstOrDefault();

        //                    if (stockDetails != null)
        //                    {
        //                        stockDetails.SectionID = AppUtils.WorkingSection;
        //                        stockDetails.ProductStatusID = AppUtils.ProductStatusIsRunning;
        //                        stockDetails.UpdateBy = AppUtils.GetLoginEmployeeName();
        //                        stockDetails.UpdateDate = AppUtils.GetDateTimeNow();
        //                        db.Entry(stockDetails).State = EntityState.Modified;
        //                        db.SaveChanges();

        //                        Distribution distribution = new Distribution();
        //                        SetNewStockDistribution(ref distribution, stockDetails, ItemListForEmployee, ClientDetailsForUpdate);
        //                        db.Distribution.Add(distribution);
        //                        db.SaveChanges();
        //                    }
        //                }

        //            }
        //            if (ClientCableAssign != null)
        //            {
        //                foreach (var cableUsedFromClient in ClientCableAssign)
        //                {
        //                    CableStock CableStock = db.CableStock.Where(s => s.CableStockID == cableUsedFromClient.CableStockID).FirstOrDefault();

        //                    if (CableStock != null)
        //                    {
        //                        CableStock.UsedQuantityFromThisBox +=  cableUsedFromClient.CableQuantity;
        //                        CableStock.UpdateBy = AppUtils.GetLoginEmployeeName();
        //                        CableStock.UpdateDate = AppUtils.GetDateTimeNow();
        //                        db.Entry(CableStock).State = EntityState.Modified;
        //                        db.SaveChanges();

        //                        CableDistribution CableDistribution = new CableDistribution();
        //                        SetCableStockDistribution(ref CableDistribution, CableStock, cableUsedFromClient, ClientDetailsForUpdate);
        //                        CableDistribution.CableIndicatorStatus = AppUtils.CableIndicatorStatusIsRunning;
        //                        CableDistribution.CreatedBy = AppUtils.GetLoginEmployeeName();
        //                        CableDistribution.CreatedDate = AppUtils.GetDateTimeNow();
        //                        db.CableDistribution.Add(CableDistribution);
        //                        db.SaveChanges();
        //                    }
        //                }

        //                // List<>
        //            }
        //        }
        //        /////*****************************************************************************************************
        //       // return Json(new { SuccessInsert = true }, JsonRequestBehavior.AllowGet);



        //        //   var ClientDetailsIgnoreLoop = AppUtils.IgnoreCircularLoop(ClientDetailsForUpdate);

        //        var JSON = Json(new { UpdateStatus = true, ClientDetailsID = ClientDetailsForUpdate.ClientDetailsID }, JsonRequestBehavior.AllowGet);
        //        JSON.MaxJsonLength = int.MaxValue;
        //        return JSON;
        //    }
        //    catch(Exception ex)
        //    {
        //        DeleteClientTransaction_Status(ClientLineStatusSave, TransactonSave);
        //        return Json(new { UpdateStatus = false, ClientDetailsID = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        private void SetCableStockDistribution(ref CableDistribution cableDistribution, CableStock cableStock, ClientCableAssign clientCableAssign, ClientDetails clientDetails)
        {
            cableDistribution.AmountOfCableUsed = clientCableAssign.CableQuantity;
            cableDistribution.CableStockID = cableStock.CableStockID;
            cableDistribution.CableAssignFromWhere = AppUtils.CableAssignFromNewClient;
            cableDistribution.ClientDetailsID = clientDetails.ClientDetailsID;
            cableDistribution.CableForEmployeeID = clientCableAssign.EmployeeID;
            cableDistribution.Purpose = "Client Purpose";
        }
        private void SetNewStockDistribution(ref Distribution distribution, StockDetails stockDetails, List<ClientStockAssign> itemListForEmployee, ClientDetails ClientDetails)
        {
            distribution.EmployeeID = itemListForEmployee[0].EmployeeID;
            distribution.StockDetailsID = stockDetails.StockDetailsID;
            distribution.ClientDetailsID = ClientDetails.ClientDetailsID;
            distribution.DistributionReasonID = AppUtils.DistributionReasonIsNewConenction;
            distribution.CreatedBy = AppUtils.GetLoginEmployeeName();
            distribution.CreatedDate = AppUtils.GetDateTimeNow();
        }

        private void DeleteClientTransaction_Status(ClientLineStatus ClientLineStatusSave, Transaction TransactonSave)
        {
            if (ClientLineStatusSave.ClientLineStatusID > 0)
            {
                ClientLineStatus ClienLineStatusDelete = db.ClientLineStatus.Find(ClientLineStatusSave.ClientLineStatusID);
                db.ClientLineStatus.Remove(ClienLineStatusDelete);
                db.SaveChanges();
            }
            if (TransactonSave.TransactionID > 0)
            {
                Transaction TransactionDelete = db.Transaction.Find(TransactonSave.TransactionID);
                db.Transaction.Remove(TransactionDelete);
                db.SaveChanges();
            }

        }

        private void setDBNewClientToBrowserClient(ref ClientDetails ClientClientDetails, ClientDetails ClientDetailsForUpdate)
        {
            ClientClientDetails.UpdateBy = "Hasan";
            ClientClientDetails.UpdateDate = AppUtils.GetDateTimeNow();
            ClientClientDetails.CreateBy = ClientDetailsForUpdate.CreateBy;
            ClientClientDetails.CreateDate = ClientDetailsForUpdate.CreateDate;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchClientListByZone(int searchID, int searchType)
        {
            List<ClientDetails> lstClientDetailsDB = (searchID == 0) ? db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient).ToList() :
                                                                                       db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient && s.ZoneID == searchID).ToList();


            var sssw = lstClientDetailsDB.Select(s => new
            {
                ClientDetailsID = s.ClientDetailsID,
                Name = s.Name,
                ZoneName = s.Zone.ZoneName,
                Address = s.Address,
                ContactNumber = s.ContactNumber,
                PackageName = s.Package.PackageName,
                LoginName = s.LoginName,
                ClientSurvey = s.ClientSurvey,
                ConnectionDate = s.ConnectionDate,
                CreateBy = s.CreateBy,
                UpdateBy = s.UpdateBy,

            }).ToList();


            return Json(new { SearchClientList = sssw }, JsonRequestBehavior.AllowGet);
        }

    }
}