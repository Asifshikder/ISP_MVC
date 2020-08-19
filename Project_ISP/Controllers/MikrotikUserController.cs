using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using Project_ISP;
using Project_ISP.ViewModel.CustomClass;
using tik4net;
using static Project_ISP.JSON_Antiforgery_Token_Validation;

namespace ISP_ManagementSystemModel.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]

    public class MikrotikUserController : Controller
    {
        ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
        public MikrotikUserController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        // GET: /Package/

        [UserRIghtCheck(ControllerValue = AppUtils.View_Client_List)]
        public ActionResult Index()
        {
            ViewBag.ResellerID = new SelectList(db.Reseller.Select(s => new { ResellerID = s.ResellerID, ResellerName = s.ResellerName }).ToList(), "ResellerID", "ResellerName");

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

            //ViewBag.CreateIPPoolID = new SelectList(db.IPPool.Select(s => new { s.IPPoolID, s.PoolName }), "IPPoolID", "PoolName");
            //ViewBag.CreateMikrotikID = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            //ViewBag.IPPoolID = new SelectList(db.IPPool.Select(s => new { s.IPPoolID, s.PoolName }), "IPPoolID", "PoolName");
            //ViewBag.MikrotikID = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.lstMikrotiks = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            ViewBag.lstMikrotikUpdate = new SelectList(db.Mikrotik.Select(s => new { s.MikrotikID, s.MikName }), "MikrotikID", "MikName");
            VM_Package_ClientDetails VM_Package_ClientDetails = new VM_Package_ClientDetails();
            //   VM_Package_ClientDetails.lstClientDetails = db.ClientDetails.ToList();
            // VM_Package_ClientDetails.lstPackage = db.Package.ToList();

            return View(VM_Package_ClientDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllMikrotikUserAJAXData()
        {
            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {
                int zoneFromDDL = 0;
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var Mikrotik = Request.Form.Get("lstMikrotiks");
                int MikrotikID = 0;
                if (!String.IsNullOrEmpty(Mikrotik))
                {
                    MikrotikID = int.Parse(Mikrotik);
                }
                // Loading.   

                Mikrotik mikrotik = db.Mikrotik.Find(MikrotikID);

                ITikConnection connection;
                ITikCommand userCmd;
                ITikCommand PackageCmd;
                try
                {
                    List<MikrotikUserList> lstMikrotikUserList = new List<MikrotikUserList>();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,

                    List<MikrotikUserList> lstUserFromMikrotik = MikrotikLB.GetUserListFromMikrotik(connection, mikrotik);
                    List<MikrotikUserList> lstActiveUserList = MikrotikLB.GetActiveUserListFromMikrotik(connection, mikrotik);
                    IEnumerable<ITikReSentence> lstPackageFromMikrotik = MikrotikLB.GetPackageListFromMikrotik(connection);

                    //userCmd.ExecuteList();
                    //var a = MikrotikLB.GetUserListFromMikrotik(userCmd);

                    //PackageCmd = connection.CreateCommand("/ppp/profile/print");
                    //IEnumerable<ITikReSentence> lstPackageFromMikrotik = PackageCmd.ExecuteList();


                    List<string> lstUserFromSystem = db.ClientDetails.Select(x => x.LoginName).ToList();
                    lstUserFromMikrotik = lstUserFromMikrotik.Where(z => !lstUserFromSystem.Contains(z.UserName)).ToList();
                    //s => lstStockDetailsListFromClient.Contains(s.StockDetailsID)
                    return Json(new { Success = true, lstUserFromMikrotik = lstUserFromMikrotik, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }


                //// Apply pagination.   
                ////data = data.Skip(startRec).Take(pageSize).ToList();
                //var clientDetails = !string.IsNullOrEmpty(Mikrotik) ? db.ClientDetails.Where(s => s.MikrotikID.Value == MikrotikID && s.IsNewClient != AppUtils.isNewClient).AsEnumerable() : db.ClientDetails.Where(s => s.IsNewClient != AppUtils.isNewClient).AsEnumerable();

                //int ifSearch = 0;
                //if (!string.IsNullOrEmpty(search) &&
                //    !string.IsNullOrWhiteSpace(search))
                //{

                //    ifSearch = (clientDetails.Any()) ? clientDetails.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) || p.Package.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                //                                              p.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) || p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //                                              p.LoginName.ToString().ToLower().Contains(search.ToLower())).Count() : 0;

                //    //Apply search
                //    clientDetails = clientDetails.Where(p => p.Name.ToString().ToLower().Contains(search.ToLower()) || p.Package.PackageName.ToString().ToLower().Contains(search.ToLower()) ||
                //                                             p.Zone.ZoneName.ToString().ToLower().Contains(search.ToLower()) || p.ContactNumber.ToString().ToLower().Contains(search.ToLower()) ||
                //                                             p.LoginName.ToString().ToLower().Contains(search.ToLower())

                //    ).AsEnumerable();
                //}
                //List<MikrotikUserCutomModel> data =
                //    clientDetails.Any() ? clientDetails.Skip(startRec).Take(pageSize).AsEnumerable()
                //        //.Select new {ss=>ss. }
                //        .Select(
                //            s => new MikrotikUserCutomModel
                //            {
                //                ClientDetailsID = s.ClientDetailsID,
                //                MikrotikID = (s.Mikrotik != null) ? s.MikrotikID.Value : 0,
                //                ClientName = s.Name,
                //                LoginName = s.LoginName.ToString(),
                //                PackageName = s.Package.PackageName,
                //                Zone = s.Zone.ZoneName,
                //                ContactNumber = s.ContactNumber,
                //                MikrotikName = (s.Mikrotik != null) ? s.Mikrotik.MikName : "",
                //                LocalAddress = s.Package.LocalAddress,
                //                PoolName = s.Package.IpPool != null ? s.Package.IpPool.PoolName : "",
                //                UpdateMikrotikUserInformation = ISP_ManagementSystemModel.AppUtils.HasAccessInTheList(ISP_ManagementSystemModel.AppUtils.Update_Client) ? true : false,
                //            })
                //        .ToList() : new List<MikrotikUserCutomModel>();
                //// Verification.   

                //// Sorting.   
                //data = this.SortByColumnWithOrder(order, orderDir, data);
                //// Total record count.   
                //int totalRecords = clientDetails.AsEnumerable().Count();//(!string.IsNullOrEmpty(search) &&  !string.IsNullOrWhiteSpace(search))? data.AsEnumerable().Count(): 
                //// Filter record count.   
                //int recFilter = (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search)) ? ifSearch : clientDetails.AsEnumerable().Count();

                //////////////////////////////////////


                //// Loading drop down lists.   
                //result = this.Json(new
                //{
                //    draw = Convert.ToInt32(draw),
                //    recordsTotal = totalRecords,
                //    recordsFiltered = recFilter,
                //    data = data
                //}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowMikrotikUserByMikrotikIDForSynchronize(int MikrotikID)
        {

            // Initialization.   
            JsonResult result = new JsonResult();
            try
            {

                Mikrotik mikrotik = db.Mikrotik.Find(MikrotikID);

                ITikConnection connection;
                ITikCommand userCmd;
                ITikCommand PackageCmd;
                try
                {
                    List<MikrotikUserList> lstMikrotikUserList = new List<MikrotikUserList>();
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, mikrotik.MikUserName, mikrotik.MikPassword);//mikrotik.APIPort,

                    List<MikrotikUserList> lstUserFromMikrotik = MikrotikLB.GetUserListFromMikrotik(connection, mikrotik);
                    List<MikrotikUserList> lstActiveUserList = MikrotikLB.GetActiveUserListFromMikrotik(connection, mikrotik);
                    IEnumerable<ITikReSentence> lstPackageFromMikrotik = MikrotikLB.GetPackageListFromMikrotik(connection);

                    List<string> lstUserFromSystem = db.ClientDetails.Select(x => x.LoginName).ToList();
                    lstUserFromMikrotik = lstUserFromMikrotik.Where(z => !lstUserFromSystem.Contains(z.UserName)).ToList();
                    return Json(new { Success = true, lstUserFromMikrotik = lstUserFromMikrotik, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, MikrotikConnectionFailed = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                // Info   
                Console.Write(ex);
            }
            // Return info.   
            return result;
        }


        private List<MikrotikUserCutomModel> SortByColumnWithOrder(string order, string orderDir, List<MikrotikUserCutomModel> data)
        {
            // Initialization.   
            List<MikrotikUserCutomModel> lst = new List<MikrotikUserCutomModel>();
            try
            {
                // Sorting   
                switch (order)
                {

                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientName).ToList() : data.OrderBy(p => p.ClientName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LoginName).ToList() : data.OrderBy(p => p.LoginName).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PackageName).ToList() : data.OrderBy(p => p.PackageName).ToList();
                        break;
                    case "5":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Zone).ToList() : data.OrderBy(p => p.Zone).ToList();
                        break;
                    case "6":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactNumber).ToList() : data.OrderBy(p => p.ContactNumber).ToList();
                        break;
                    case "7":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MikrotikName).ToList() : data.OrderBy(p => p.MikrotikName).ToList();
                        break;
                    case "8":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PoolName).ToList() : data.OrderBy(p => p.PoolName).ToList();
                        break;
                    case "9":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LocalAddress).ToList() : data.OrderBy(p => p.LocalAddress).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowMikrotikUserByIDForUpdate(int ClientDetailsID)
        {
            var ClientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID).AsEnumerable();
            int MikrotikID = ClientDetails.FirstOrDefault().Mikrotik != null ? ClientDetails.FirstOrDefault().MikrotikID.Value : 0;
            //    $("#PackageName").val(PackageJSONParse.PackageName);
            //$("#PackagePrice").val(PackageJSONParse.PackagePrice);
            //$("#BandWith").val(PackageJSONParse.BandWith);
            // var PackageCircularLoopIgnored = AppUtils.IgnoreCircularLoop(Package);

            var JSON = Json(new { MikrotikID = MikrotikID }, JsonRequestBehavior.AllowGet);
            JSON.MaxJsonLength = int.MaxValue;
            return JSON;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMikrotikUser(int ClientDetailsID, int MikrotikID)
        {
            ///first check mikrotik is active or not *******************************************************
            ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);

            Mikrotik mk = db.Mikrotik.Find(MikrotikID);
            try
            {
                if ((bool)Session["MikrotikOptionEnable"])
                {
                    var clientDetails = db.ClientDetails.Where(s => s.ClientDetailsID == ClientDetailsID).FirstOrDefault();
                    //checking new mikrotik is enable or not
                    connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mk.RealIP, 8728, mk.MikUserName, mk.MikPassword);

                    // now checking if old client details mikrotik is null then direcet try to insert into mikrotik.
                    if (clientDetails.Mikrotik == null)
                    {
                        clientDetails.MikrotikID = MikrotikID;
                        db.Entry(clientDetails).State = EntityState.Modified;
                        db.SaveChanges();

                        connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "profile", clientDetails.Package.PackageName).ExecuteNonQuery();
                    }
                    else
                    {
                        connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, clientDetails.Mikrotik.RealIP, 8728, clientDetails.Mikrotik.MikUserName, clientDetails.Mikrotik.MikPassword);
                        if (clientDetails.MikrotikID == mk.MikrotikID)
                        {
                            // connection.CreateCommandAndParameters("/ppp/secret/set", ".id",clientDetails.LoginName,).ExecuteNonQuery();
                        }
                        else if (clientDetails.MikrotikID != mk.MikrotikID)
                        {
                            //check if new mikrotik has this name then return
                            connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mk.RealIP, 8728, mk.MikUserName, mk.MikPassword);
                            var checkClientLoginNameExistInNewMikrotik = connection.CreateCommandAndParameters("/ppp/secret/print", "name", clientDetails.LoginName).ExecuteList();
                            if (checkClientLoginNameExistInNewMikrotik.Count() > 0)
                            {
                                return Json(new { Success = false, MikrotikFailed = true, Message = "secret name already exist in " + mk.MikName + " mikrotik." }, JsonRequestBehavior.AllowGet);
                            }

                            connection.CreateCommandAndParameters("/ppp/secret/remove", ".id", clientDetails.LoginName).ExecuteNonQuery();
                            connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", "pppoe", "profile", clientDetails.Package.PackageName).ExecuteNonQuery();
                        }

                    }



                    //string loginName = db.ClientDetails.Find(ClientClientDetails.ClientDetailsID).LoginName;

                    //var oldIDExistOrNot = connection.CreateCommandAndParameters("/ppp/secret/print", "name", loginName).ExecuteList();
                    ////var oldIDExistOrNot1 = connection.CreateCommandAndParameters("/ppp/secret/print", "name", loginName).ExecuteScalar();
                    ////var oldIDExistOrNot2 = connection.CreateCommandAndParameters("/ppp/secret/print", "name", loginName).ExecuteSingleRow();
                    //if (oldIDExistOrNot.Count() < 1)
                    //{
                    //    return Json(new { Success = false, MikrotikFailed = true, Message = "Old login name: " + loginName + " was not exist in Mikrotik. Please Assign First." },
                    //        JsonRequestBehavior.AllowGet);
                    //}

                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    MikrotikFailed = true,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }


            try
            {
                var JSON = Json(new { UpdateSuccess = true, ClientDetailsID = ClientDetailsID, MikrotikName = mk.MikName }, JsonRequestBehavior.AllowGet);
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
            //if (AppUtils.lstAccessList.Contains(AppUtils.MikrotikOptionEnable))
            //{
            //    Mikrotik mikrotik = db.Mikrotik.Find(Package.MikrotikID);
            //    var ipPoolName = db.IPPool.Find(Package.IPPoolID).PoolName;

            //    try
            //    {//ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
            //        ITikConnection connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);
            //        connection.CreateCommandAndParameters("/ppp/profile/add", "name", Package.PackageName.Trim(), "local-address", Package.LocalAddress, "remote-address", ipPoolName, "incoming-filter", "mypppclients").ExecuteNonQuery();

            //        //add profile
            //        //var profileadd = connection.CreateCommandAndParameters("ppp/profile/add", "name", "test1", "local-address", "10.0.0.1", "remote-address", "sp", "incoming-filter", "mypppclients");
            //        //profileadd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { Success = false, MikrotikFailed = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            //    }

            //}
            int packageCount = db.Package.Count();

            try
            {
                Package.CreatedBy = AppUtils.GetLoginEmployeeName();
                Package.CreatedDate = AppUtils.GetDateTimeNow();

                db.Package.Add(Package);
                db.SaveChanges();
                //  var PoolName = db.IPPool.Where(s => s.IPPoolID == Package.IPPoolID).FirstOrDefault().PoolName;
                //var packageInsertInformation = AppUtils.IgnoreCircularLoop(PackageInfo);

                CustomPackage PackageInfo = new CustomPackage
                {
                    PackageID = Package.PackageID,
                    PackageName = Package.PackageName,
                    BandWith = Package.BandWith,
                    PackagePrice = Package.PackagePrice.ToString(),
                    Client = db.ClientDetails.Where(ss => ss.PackageID == Package.PackageID).Count(),
                    //IPPoolName = (Package.IPPoolID != null) ? Package.IpPool.PoolName : "",
                    //LocalAddress = Package.LocalAddress,
                    //MikrotikName = db.Mikrotik.Find(Package.MikrotikID).MikName,
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

        public class MikrotikUser
        {
            public string LoginName { get; set; }
            public string Password { get; set; }
            public int MikrotikID { get; set; }
            public string PackageName { get; set; }
        }

        [HttpPost]
        //[ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult InsertClientDetailsIntoSystemFromMikrotik(List<MikrotikUser> MikrotikUser)
        {
            List<Package> lstPackage = db.Package.ToList();
            int questionID = db.SecurityQuestion.FirstOrDefault().SecurityQuestionID;
            bool success = true;
            List<string> successArray = new List<string>();
            foreach (var user in MikrotikUser)
            {
                try
                {
                    ClientDetails ClientDetails = new ClientDetails();
                    ClientLineStatus ClientLineStatusSave = new ClientLineStatus();
                    Transaction TransactonSave = new Transaction();
                    Transaction Transaction = new Transaction();


                    double thisMonthFee = 0;
                    ClientDetails clientDetails = SetClientDetailsForMikrotikUser(user, lstPackage);
                    clientDetails.SecurityQuestionID = questionID;
                    db.ClientDetails.Add(clientDetails);
                    db.SaveChanges();

                    if (clientDetails.ClientDetailsID > 0)
                    {

                        ClientLineStatus ClientLineStatus = new ClientLineStatus();
                        ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
                        ClientLineStatus.ClientDetailsID = clientDetails.ClientDetailsID;
                        ClientLineStatus.PackageID = clientDetails.PackageID;
                        ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
                        ClientLineStatus.EmployeeID = AppUtils.GetLoginUserID();
                        ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow();
                        ClientLineStatus.StatusChangeReason = "New Connection";

                        DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

                        ClientLineStatus.LineStatusWillActiveInThisDate = dayone;
                        ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                        db.SaveChanges();
                    }

                    Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
                    //Transaction.PaymentStatus = AppUtils.PaymentIsNotPaid;
                    Transaction.IsNewClient = AppUtils.isNewClient;
                    Transaction.EmployeeID = AppUtils.GetLoginUserID();

                    //Transaction.RemarksNo = "RNEW" + RemarksNo();
                    //Transaction.ResetNo = "RNEW" + SerialNo();


                    Transaction.ClientDetailsID = clientDetails.ClientDetailsID;
                    Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
                    Transaction.WhoGenerateTheBill = AppUtils.GetLoginUserID();
                    Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
                    Transaction.PackageID = clientDetails.PackageID;
                    Transaction.BillCollectBy = AppUtils.GetLoginUserID();
                    Transaction.PaymentFromWhichPage = "Create";
                    Transaction.PaymentAmount = 0;
                    //Transaction.PaymentDate = Transaction.PaymentDate.Value.AddHours(AppUtils.GetDateTimeNow().Hour).AddMinutes(AppUtils.GetDateTimeNow().Minute).AddSeconds(AppUtils.GetDateTimeNow().Second).AddMilliseconds(AppUtils.GetDateTimeNow().Millisecond);
                    Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date

                    TransactonSave = db.Transaction.Add(Transaction);
                    db.SaveChanges();
                    //if (Transaction.PaymentAmount > 0)
                    {
                        UpdatePaymentIntoPaymentHistoryForClientCreate("SignUp:" + SerialNo(), Transaction);
                    }
                    if (Transaction.TransactionID > 0)
                    {
                        int BillRemainingSameUptoWhichDate = int.Parse(ConfigurationManager.AppSettings["BillRemainingSameUptoWhichDate"]);
                        int BillWillNotEffectAfterWhichDate = int.Parse(ConfigurationManager.AppSettings["BillWillNotEffectAfterWhichDate"]);


                        DateTime currenDateTime = AppUtils.GetDateTimeNow();
                        bool regularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);
                        int totalDaysInThisMonth = DateTime.DaysInMonth(currenDateTime.Year, currenDateTime.Month);
                        int Totaldays = regularMonthlyBase == false ? int.Parse(ConfigurationManager.AppSettings["CountDate"]) : totalDaysInThisMonth;

                        Transaction forMonthlyBill = new Transaction();
                        forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
                        forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes; ;

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
                        forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow().Day <= BillRemainingSameUptoWhichDate ? AppUtils.ThisMonthStartDate() : AppUtils.GetDateTimeNow();

                        double packagePricePerday = 0;
                        int DaysRemains = 0;
                        double MainPackagePrice = db.Package.Find(Transaction.PackageID).PackagePrice;
                        bool CountRegularMonthlyBase = bool.Parse(ConfigurationManager.AppSettings["CountRegularMonthlyBase"]);

                        if (CountRegularMonthlyBase == true)
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


                        string amount = (currenDateTime.Day <= BillRemainingSameUptoWhichDate) ? MainPackagePrice.ToString()//taking full package if date<=10
                                        : (currenDateTime.Day > BillRemainingSameUptoWhichDate && currenDateTime.Day <= BillWillNotEffectAfterWhichDate) ? (packagePricePerday * DaysRemains).ToString()
                                        : "0";
                        float tmp = 0;
                        float.TryParse(amount, out tmp);
                        //////forMonthlyBill.PaymentAmount = tmp;
                        //////thisMonthFee = tmp;
                        forMonthlyBill.PaymentAmount = (float?)Math.Round(tmp);


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

                        thisMonthFee = tmp;
                        //forMonthlyBill.PaymentAmount = db.Package.Find(Transaction.PackageID).PackagePrice;
                        //thisMonthFee = db.Package.Find(Transaction.PackageID).PackagePrice; ;

                        db.Transaction.Add(forMonthlyBill);
                        db.SaveChanges();

                    }
                    successArray.Add(user.LoginName);
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }

            //catch (Exception ex)
            //{
            //    DeleteClientDetails_Transaction_Status(ClientLineStatusSave, TransactonSave, ClientDetailsSave);
            //    return Json(new { SuccessInsert = false }, JsonRequestBehavior.AllowGet);
            //}
            return Json(new { Success = success, SuccessArray = successArray }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //[ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult RemoveClientPermentlyFromMikrotik(List<MikrotikUser> MikrotikUser)
        {
            List<Package> lstPackage = db.Package.ToList();
            Mikrotik mikrotik = db.Mikrotik.Find(MikrotikUser.FirstOrDefault().MikrotikID);

            int questionID = db.SecurityQuestion.FirstOrDefault().SecurityQuestionID;
            bool success = true;
            List<string> successArray = new List<string>();
            ITikConnection connMik = MikrotikLB.CreateInstanceOfMikrotik(TikConnectionType.Api, mikrotik.RealIP, 8728, mikrotik.MikUserName, mikrotik.MikPassword);

            foreach (var user in MikrotikUser)
            {
                try
                {
                    MikrotikLB.RemoveUserInMikrotik(connMik, user.LoginName);
                    successArray.Add(user.LoginName);
                }
                catch (Exception ex)
                {
                    success = false;
                }
            } 
            return Json(new { Success = success, SuccessArray = successArray }, JsonRequestBehavior.AllowGet);
        }

        private void UpdatePaymentIntoPaymentHistoryForClientCreate(string resetNo, Transaction ts)
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

        private ClientDetails SetClientDetailsForMikrotikUser(MikrotikUser user, List<Package> lstPackage)
        {
            ClientDetails cd = new ClientDetails();
            cd.Name = user.LoginName;
            cd.LoginName = user.LoginName;
            cd.Password = user.Password;
            cd.Address = "Address";
            cd.ContactNumber = "0101010101";
            cd.ZoneID = 10;
            cd.SMSCommunication = cd.ContactNumber;
            cd.Occupation = "Ocupation";
            cd.SocialCommunicationURL = "SocialCommunicationURL";
            cd.Remarks = "Remarks";
            cd.ConnectionTypeID = 1;
            cd.BoxNumber = "BoxNumber";
            cd.PopDetails = "PopDetails";
            cd.Reference = "Reference";
            cd.NationalID = "NationalID";
            cd.PackageID = lstPackage.Where(x => x.PackageName.ToLower().Trim() == user.PackageName.ToLower().Trim()).Select(x => x.PackageID).FirstOrDefault();
            cd.SecurityQuestionID = 0;
            cd.SecurityQuestionAnswer = "SecurityQuestionAnswer";
            cd.MacAddress = "MacAddress";
            cd.ClientSurvey = "ClientSurvey";
            cd.ConnectionDate = AppUtils.dateTimeNow;
            cd.MikrotikID = user.MikrotikID;
            cd.IP = "IP";
            cd.Mac = "Mac";
            cd.ApproxPaymentDate = 10;

            return cd;
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