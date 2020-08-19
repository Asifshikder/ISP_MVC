using ISP_ManagementSystemModel.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using Project_ISP.Controllers;
using System.ComponentModel;

namespace ISP_ManagementSystemModel
{
    public static class AppUtils
    {
        public static string GetEnumDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return null;
        }
        protected internal enum ResellerTypeEnum
        {
            [Description("Bandwidth Reseller")]
            BandwidthReseller = 1,
            [Description("Mac Based Reseller")]
            MacBasedReseller = 2
        }
        protected internal enum PublishStatus
        {
            [Description("Published")]
            Published = 1,
            [Description("Draft")]
            Draft = 2
        }
        protected internal enum ClientTypeEnum
        {
            //[Description("All Client")]
            //AllClient = 1,
            [Description("Active Client")]
            ActiveClient = 3,
            [Description("Lock Client")]
            LockClient = 5,
            [Description("Delete Client")]
            DeleteClient = 7
        }
        protected internal enum ItemFor
        { 
            [Description("For All")]
            ForAll = 1,
            [Description("General")]
            General = 2,
            [Description("BandwithReseller")]
            BandwithReseller = 3
        }
        protected internal enum PurchaseStatus
        { 
            [Description("On Process")]
            OnProcess = 1,
            [Description("Shipping")]
            Shipping = 2,
            [Description("Received")]
            Received = 3
        }
        protected internal enum PaymentMethod
        { 
            [Description("Cash")]
            Cash = 1,
            [Description("Check")]
            Check = 2,
            [Description("ATMWithDraw")]
            ATMWithDraw = 3,
            [Description("ElectronicTransfer")]
            ElectronicTransfer = 4
        }
        protected internal enum DepositStatus
        {
            [Description("Cleared")]
            Cleared = 1,
            [Description("Uncleared")]
            Uncleared = 2
        }
        protected internal enum HeadType
        {
            [Description("Expense")]
            Expense = 1,
            [Description("Income")]
            Income = 2
        }
        protected internal enum AccountingHistoryType
        {
            [Description("AccountList")]
            AccountList = 1,
            [Description("Purchase")]
            Purchase = 2,
            [Description("Sales")]
            Sales = 3,
            [Description("Deposit")]
            Deposit = 4,
            [Description("Expense")]
            Expense = 5
        }
        protected internal enum AccountTransactionType
        {
            [Description("DR")]
            DR = 1,
            [Description("CR")]
            CR = 2 
        }
        protected internal enum AdminBillPaymentType
        {
            [Description("FixMonth")]
            FixMonth = 1,
            [Description("AnyMonth")]
            AnyMonth = 0 
        }
        protected internal enum EmployeeBillPaymentType
        {
            [Description("FixMonth")]
            FixMonth = 1,
            [Description("AnyMonth")]
            AnyMonth = 0 
        }
        protected internal enum CanGiveDiscount
        {
            [Description("AdminCan")]
            AdminCan = 1,
            [Description("EmploeeCan")]
            EmploeeCan = 0 ,
            [Description("ResellerCan")]
            ResellerCan = 0 
        }
        protected internal enum CanSelectBillCollectBy
        {
            [Description("AdminCan")]
            AdminCan = 1,
            [Description("EmploeeCan")]
            EmploeeCan = 0 ,
            [Description("ResellerCan")]
            ResellerCan = 1 
        }

        protected internal enum TransactionType
        {
            [Description("Deposit")]
            Deposit = 1,
            [Description("Expense")]
            Expense = 2,
            [Description("Transfer")]
            Transfer = 3,
            [Description("Purchase")]
            Purchase = 4
        }
        //static AppUtils()
        //{
        //    dateTimeNow = dateTimeNow.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);
        //}

        //public  static  DateTime dateTimeNow  = new DateTime(2017,3,30); 
        //.AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);

        //this is for when employee will show in employee page but not give an option for update.
        public static int connFromLiveOrLocal = 0;//0 mean local 1 meanlive. 
        private static string ISPConnectionStringLocal = "Data Source=localhost; Initial Catalog = ISPCommonSpeedTech09042020; integrated security=true; MultipleActiveResultSets=true;";
        private static string ISPConnectionStringLive = "Data Source=localhost; Initial Catalog = ispspeed;                   User Id=hasan;      Password=Hn)9POWER!@#$POWER;MultipleActiveResultSets=true";
        //private static string ISPConnectionStringLocal = "Data Source=localhost; Initial Catalog = ISPCommonSpeedTech09042020;        User Id=hasan;      Password=Hn)9POWER!@#$POWER;MultipleActiveResultSets=true;";
        //private static string ISPConnectionStringLive =  "Data Source=localhost; Initial Catalog = ISPCommonSpeedTech09042020;        User Id=hasan;      Password=Hn)9POWER!@#$POWER;MultipleActiveResultSets=true";
        public static string connectionStringForQuery()
        {
            if (connFromLiveOrLocal == 1)//mean live string
            {
                return ISPConnectionStringLive;
            }
            else
            {
                return ISPConnectionStringLocal;
            }
        }
        public static List<string> GetTempNotUpdateEmployee { get; set; }

        public static DateTime dateTimeNow = DateTime.Now;

        //public static void CallingMethod()
        //{
        //    UserRightPermission userRightPermission = db.UserRightPermission
        //        .Where(s => s.UserRightPermissionID == AppUtils.CurrentUserRightPermission).FirstOrDefault();
        //    if (!string.IsNullOrEmpty(userRightPermission.UserRightPermissionDetails))
        //    {
        //        AppUtils.lstAccessList = db.UserRightPermission
        //            .Where(s => s.UserRightPermissionID ==
        //                        AppUtils.CurrentUserRightPermission) //AppUtils.UserRightPermissionIDIsAdminUser)
        //            .Select(s => s.UserRightPermissionDetails)
        //            .FirstOrDefault().Split(',').ToList();
        //    }
        //}

        public static SMSReturnDetails SendSMS(string Sender, string ID, string Pass, string destination, string message)
        {
            string html = string.Empty;
            string url = @"http://sms.walletmix.biz/sms-api/apiAccess?username=" + ID + "&password=" + Pass + "&type=0&destination=" + destination + "&source=" + Sender + "&message=" + message + "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            SMSReturnDetails SMSReturnDetails = JsonConvert.DeserializeObject<SMSReturnDetails>(html);

            return SMSReturnDetails;
        }


        public static bool IsGranted(string buttonVal)
        {
            List<string> lstAppUtils = HttpContext.Current.Session["lstAccessList"] != null ? HttpContext.Current.Session["lstAccessList"] as List<string> : new List<string>();
            //int countButtonNames = AppUtils.lstAccessList.Where(s => s == buttonVal).Count();
            int countButtonNames = lstAppUtils.Where(s => s == buttonVal).Count();
            if (countButtonNames > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static List<string> lstAccessList = new List<string>();
        public static bool HasAccessInTheList(string PID)
        {
            List<string> lstAccessList = HttpContext.Current.Session["lstAccessList"] != null ? HttpContext.Current.Session["lstAccessList"] as List<string> : new List<string>();
            if (lstAccessList.Contains(PID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int LstAccessCount()
        {
            List<string> lstAccessList = HttpContext.Current.Session["lstAccessList"] != null ? HttpContext.Current.Session["lstAccessList"] as List<string> : new List<string>();
            return lstAccessList.Count();
        }

        private static ISPContext db = new ISPContext();

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        //public class SessionKeyConstants
        //{
        //    public const string LOGGEDIN_USER_KEY = "__loggedinuser";
        //    public const string CURRENT_FORM_KEY = "__loggedinuser";
        //    public const string PUBLIC_KEY = "__publicKey";
        //}

        //public static ClientDetails LoggedinUser
        //{
        //    get
        //    {
        //        ClientDetails loggedinUser = null;
        //        if (HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] != null)
        //        {
        //            loggedinUser = HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] as Users;
        //        }

        //        return loggedinUser;
        //    }
        //    set { HttpContext.Current.Session[SessionKeyConstants.LOGGEDIN_USER_KEY] = value; }
        //}

        //public static string IgnoreCircularLoop(object data)
        //{
        //    var circularIgnoreResult = JsonConvert.SerializeObject(data, Formatting.None,
        //          new JsonSerializerSettings
        //          {
        //              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //          });
        //    return circularIgnoreResult;/* code to convert the setting to T... */
        //}

        public static string IgnoreCircularLoop(object data)
        {
            var circularIgnoreResult = JsonConvert.SerializeObject(data, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return circularIgnoreResult;
        }

        public static int LoginUserID { get; set; }
        public static int GetLoginUserID()
        {
            int LoggedInUserID = (int)HttpContext.Current.Session["LoggedUserID"];
            return LoggedInUserID;
        }
        public static int GetLoginRoleID()
        {
            int LoggedInUserID = (int)HttpContext.Current.Session["role_id"];
            return LoggedInUserID;
        }
        public static int GetLoginUserRightPermissionID()
        {
            int UserRightPermission = (int)HttpContext.Current.Session["CurrentUserRightPermission"];
            return UserRightPermission;
        }

        public static string GetLoginEmployeeName()
        {
            int LoggedInUserID = (int)HttpContext.Current.Session["LoggedUserID"];
            return db.Employee.Where(s => s.EmployeeID == LoggedInUserID).FirstOrDefault().Name;
        }
        //public static string GetLoginEmployeeName()
        //{
        //    return db.Employee.Where(s => s.EmployeeID == AppUtils.LoginUserID).FirstOrDefault().Name;
        //}

        public static DateTime ThisMonthStartDate()
        {
            DateTime firstDateOfThisMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month, 01);
            return firstDateOfThisMonth;
        }

        public static DateTime ThisMonthLastDate()
        {
            DateTime lastDateOfThisMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month,
                DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month));
            return lastDateOfThisMonth;
        }

        public static DateTime GetDateTimeNow()
        {
            return dateTimeNow
            //return dateTimeNow.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute)
            //    .AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);
            ;
        }

        public static DateTime GetDateNow()
        {
            return dateTimeNow.Date;
            //return dateTimeNow.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute)
            //    .AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);
            ;
        }

        public static DateTime GetLastDayWithHrMinSecMsByMyDate(DateTime dt)
        {
            return dt.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
        }


        //protected static bool AdminBillIsMonthly = true;
        //protected static bool AdminBillIsMonthly = true;
        //protected static bool AdminBillIsMonthly = true;
        //protected static bool AdminBillIsMonthly = true;


        public const bool BillIsCycleWise = false;
        public const int BillWillBeGenerateInThisDate = 1;


        public const int AllUser = 1;
        public const int MyUser = 2;
        public const int ResellerUser = 3;

        public const string PackageForMyUser = "1";
        public const string PackageForResellerUser = "2";

        public const string ProfileUpdateInPercentagePointIsClientPhoto = "ClientPhoto";
        public const string ProfileUpdateInPercentagePointIsNationalID = "NationalID";
        public const string ProfileUpdateInPercentagePointIsNationalIDImage = "NationalIDImage";
        public const string ProfileUpdateInPercentagePointIsAddress = "Address";
        public const string ProfileUpdateInPercentagePointIsMobileNo = "MobileNo";

        public const string ImageIsNID = "NID";
        public const string ImageIsOWN = "OWN";
        public const string ImageIsResellerLogo = "LOGO";
        public const string ImageIsVendorLogo = "Vendor_Image";

        public const bool EmployeeHasRightToGiveDiscount = false;

        public const int ApproxPaymentDate = 5;


        public const int PamentIsOccouringFromAccountsPage = 1;
        public const int PamentIsOccouringFromUnpaidPage = 2;
        public const int PamentIsOccouringFromNewClientSignUpPage = 3;
        public const int PamentIsOccouringFromAdvancePaymentAccountPage = 4;
        public const int PamentIsOccouringFromAdvancePaymentUnpaidPage = 5;
        public const int PamentIsOccouringFromSubmitBillByMe = 6;
        public const int PamentIsOccouringFromPayBulkBill = 7;
        public const int PamentIsOccouringFromSubmitBillByMeSpecificMonth = 8;
        public const int PamentIsOccouringFromSubmitBillByMeAdminSpecificMonth = 9;

        public const int SMSFromAllClientPage = 1;
        public const int SMSFromUnpaidPage = 2;

        public const string FreePackageName = "Free";

        public const int ChangePackageHowMuchTimes = 1;
        public const int ChangePackageHowMuchTimesForReseller = 2;
        public const string MakeUserDisabledInMikrotik = "yes";
        public const string MakeUserEnableInMikrotik = "no";

        public static string[] lstMikrotikReleated = { "91", "92", "93", "94", "95", "96" };
        public static string[] lstSMSReleated = { "88", "89", "90" };

        public static int EmployeeIDISKamrul = 1;
        public static int EmployeeIDISTalent = 14;

        public static int RunningYear = dateTimeNow.Year;
        public static int RunningMonth = dateTimeNow.Month;

        public static int ClientRole = 2;
        public static int EmployeeRole = 3;
        public static int AdminRole = 1;
        public static int SuperUserRole = 4;
        public static int ResellerRole = 5;

        public static int ComplainPendingStatus = 6;
        public static int ComplainDeleteStatus = 7;
        public static int ComplainSolveStatus = 8;
        public static int ComplainOnRequestPendingStatus = 9;
        public static int ComplainByClientPendingStatus = 10;

        public static int PandingStatus = 6;
        public static int DeleteStatus = 7;
        public static int SolveStatus = 8;

        public static int SMSGlobalStatusIsTrue = 1;

        public static int SendSMSStatusTrue = 1;
        public static int SendSMSStatusFalse = 0;

        public static int MaxPackageChangeHowMuchTimes = 2;

        public static int ReportShowForAll = 1;

        public static int PaymentIsNotPaid = 0;
        public static int PaymentIsPaid = 1;
        public static int PaymentIsPartiallyPaid = 2;

        public static int NoDueAmount = 0;



        public const int TableStatusIsActive = 1;
        public const int TableStatusIsDelete = 7;

        public static int LineIsActive = 3;
        public static int LineIsInActive = 4;
        public static int LineIsLock = 5;
        public static int LineIsDelete = 7;

        public static int RunningMonthBillIndicator = 2;
        public static int SignUpBillIndicator = 1;

        public static int isNewClient = 1; //1 mean true/yes
        public static int isNotNewClient = 0; //0 mean false/no
        public static int PaymentTypeIsConnection = 1;
        public static int PaymentTypeIsMonthly = 2;

        public static int EmployeeID = 2;


        public static int ProductStatusIsAvialable = 1;
        public static int ProductStatusIsRunning = 2;
        public static int ProductStatusIsLost = 3;
        public static int ProductStatusIsRepair = 4;
        public static int ProductStatusIsWarrenty = 5;
        public static int ProductStatusIsDead = 6;

        public static int StockSection = 1;
        public static int RepairingSection = 2;
        public static int WarrantySection = 3;
        // public static int NotWorkingSection = 15;
        public static int DeadSection = 4;
        public static int WorkingSection = 5;

        public static int DistributionReasonIsNewConenction = 1;
        public static int DistributionReasonIsUpgrade = 2;
        public static int DistributionReasonIsNotWorkable = 3;
        public static int DistributionReasonIsLost = 4;


        public static int ItemIsPop = 18;

        public static int IndicatorStatusIsActive = 1;
        public static int IndicatorStatusIsDelete = 2;

        public static int CableUnitIsCentiMeater = 1;
        public static int CableUnitIsMeater = 2;

        public static int CableAssignFromNewClient = 1;
        public static int CableAssignFromViewList = 2;


        public static int CableIndicatorStatusIsRunning = 1;
        public static int CableIndicatorStatusIsDelete = 2;
        public static int CableIndicatorStatusIsOldBox = 2;
        public static int CableIndicatorStatusIsStolen = 3;
        public static int CableIndicatorStatusIsNotWorking = 4;


        public static int SelectedWhereToPassIsMainBox = 1;
        public static int SelectedWhereToPassIsOCBox = 2;
        public static int SelectedWhereToPassIsStolen = 3;
        public static int SelectedWhereToPassIsNotWorking = 4;

        public static int CableTypeIsOldBox = 4;
        // public static int LoginUserID { get; set; }



        public static int UserRightPermissionIDIsSuperUser = 1;
        public static int UserRightPermissionIDIsReseller = 5;
        public static int UserRightPermissionIDIsSuperTallentUser = 3;
        public static int UserRightPermissionIDIsAdminUser = 2;
        public static int CurrentUserRightPermission;

        public static int EmployeeStatusIsActive = 1;
        public static int EmployeeStatusIsLock = 2;
        public static int EmployeeStatusIsDelete = 3;

        public static int ResellerStatusIsActive = 1;
        public static int ResellerStatusIsLock = 2;
        public static int ResellerStatusIsDelete = 3;

        public static int ResellerPaymentGivenTypeIsCash = 1;
        public static int ResellerPaymentGivenTypeIsCheck = 2;

        public static int PaymentStatusIsReceive = 1;
        public static int PaymentStatusIsOnProcess = 2;
        public static int PaymentStatusIsDelete = 3;

        public static int PaymentByHandCash = 1;
        public static int PaymentByPaymentGateWay = 2;

        public static int StatusChangeFromThisMonth = 1;
        public static int StatusChangeFromNextMonth = 2;


        public static string Bill_Pay_Code = "0000";
        public static string Member_Locked_This_Month = "0001";
        public static string Member_Active_This_Month = "0002";
        public static string SMS_Member_Complain_Open = "0003";
        public static string SMS_User_Complain_Open = "0004";
        public static string SMS_Member_Complain_Close = "0005";
        public static string SMS_User_Complain_Close = "0006";
        public static string New_Client_Request = "0007";
        public static string New_Client_Signup = "0008";
        public static string Member_Locked_Next_Month = "0009";
        public static string Member_Active_Next_Month = "0010";
        public static string Package_Change_This_Month = "0011";


        public static string ReturnMessageStatusCodeIsSuccess = "1000";
        public static string ReturnMessageStatusCodeIsFail = "";


        public const string Add_New_Client = "1";
        public const string Update_Client = "2";
        public const string View_Client_List = "3";
        public const string View_Active_Client_List = "4";
        public const string View_Lock_CLient = "5";
        public const string View_Request__List_Of_Client = "6";
        public const string Sign_Up_New_Client = "7";
        public const string Create_Package = "8";
        public const string VIew_Package_List = "9";
        public const string Update_Package = "10";
        public const string View_Account_Archive_Bills = "11";
        public const string Pay__Bill = "12";
        public const string View_Unpaid_Bills_List = "13";
        public const string Adjust_Due_Bills = "14";
        public const string View_Sign_Up_Bills_List = "15";
        public const string View_Active_To_Lock_List = "16";
        public const string View_Lock_To_Active_List = "17";
        public const string View_Filter_Bills_List = "18";
        public const string Add_Advance_Payment = "19";
        public const string View_Advance_Payment_List = "20";
        public const string Update_Advance_Payment = "21";
        public const string View_Client_Phone_List = "22";
        public const string Create_Complain = "23";
        public const string View_Complain_List = "24";
        public const string Update_Complain_Status = "25";
        public const string Add_Expense = "26";
        public const string View_Expense_List = "27";
        public const string Update_Expense = "28";
        public const string Delete_Expense = "29";
        public const string Delete_Client = "30";
        public const string Add_Zone = "31";
        public const string View_Zone_List = "32";
        public const string Update_Zone = "33";
        public const string Add_Brand = "34";
        public const string View_Brand_List = "35";
        public const string Update_Brand = "36";
        public const string Add_Distribution_Reason = "37";
        public const string View_Distribution_Reason_List = "38";
        public const string Update_Distribution_Reason = "39";
        public const string Add_Section = "40";
        public const string View_Section_List = "41";
        public const string Update_Section = "42";
        public const string Add_Product_Status = "43";
        public const string View_Product_Status_List = "44";
        public const string Update_Product_Status = "45";
        public const string Add_Item = "46";
        public const string View_Item_List = "47";
        public const string Update_Item = "48";
        public const string Add_Pop = "49";
        public const string View_Pop_List = "50";
        public const string Update_Pop = "51";
        public const string Add_Box = "52";
        public const string View_Box_List = "53";
        public const string Update_Box = "54";
        public const string Add_Supplier = "55";
        public const string View_Supplier_List = "56";
        public const string Update_Supplier = "57";
        public const string Add_ProductORItem_In_Stock = "58";
        public const string Stock_Distribution = "59";
        public const string Add_Cable_In_Stock = "60";
        public const string Cable_Distribution = "61";
        public const string View_Product_Stock_List_Already_Distributed = "62";
        public const string Delete_Product_if_Distributed_by_mistake = "63";
        public const string View_Product_Total_List = "64";
        public const string Change_Product_Status = "65";
        public const string View_Cable_Distributed_To_Client_Or_Employee = "66";
        public const string Change_Cable_Status_To_Other_Such_New_Or_Old_Box_Or_Dead = "67";
        public const string View_Product_avialable_List = "68";
        public const string View_Product_running_List = "69";
        public const string View_Product_repairing_List = "70";
        public const string View_Product_Dead_List = "71";
        public const string Assign_Item_Or_Cable_To_Employee = "72";
        public const string Update_Request_Client = "73";
        public const string Delete_Request_Client = "74";
        public const string Add_New_Request = "75";
        public const string Pay_Due_Bill = "76";
        public const string Bill_Generate = "77";
        public const string View_Product_Warrenty_List = "78";
        public const string Change_Product_Status_If_Working_Section = "79";
        public const string View_Product_Working_List = "80";
        public const string Set_User_Right = "81";
        public const string Assign_Employee_User_Right = "82";
        public const string View_Employee_List = "83";
        public const string Add_Employee = "84";
        public const string Update_Employee = "85";
        public const string Delete_Employee = "86";
        public const string View_Line_Status_History = "87";


        public static bool SMSOptionEnable = false;
        public const string SMSOptionName = "SMSOptionName";

        public const string View_SMS_Settings_List = "88";
        public const string Add_SMS_Title_Message = "89";
        public const string Update_SMS_Settings_List = "90";

        public static bool MikrotikOptionEnable = false;

        public const string View_Mikrotik_List = "91";
        public const string Add_Mikrotik = "92";
        public const string Update_Mikrotik = "93";

        //public const string View_Package_Mikrotik = "87";
        //public const string Add_Package_Mikrotik = "87";
        // public const string Update_Package_Mikrotik = "87";

        public const string View_IPPool_List = "94";
        public const string Add_IPPool = "95";
        public const string Update_IPPool = "96";


        public const string ViewDashBoard = "97";

        public const string Add_Update_SMS_Sender_IDPass = "98";

        public const string Add_Reseller = "99";
        public const string View_Reseller_List = "100";
        public const string Update_Reseller = "101";

        public const string Add_ComplainType = "102";
        public const string View_ComplainType_List = "103";
        public const string Update_ComplainType = "104";


        public const string Add_Asset = "105";
        public const string View_Asset_List = "106";
        public const string Update_Asset = "107";
        public const string Delete_Asset = "108";

        public const string Add_AssetType = "109";
        public const string View_AssetType_List = "110";
        public const string Update_AssetType = "111";
        public const string Delete_AssetType = "112";


        public const string Add_TimePeriodForSignal = "113";
        public const string View_TimePeriodForSignal_List = "114";
        public const string Update_TimePeriodForSignal = "115";
        public const string Delete_TimePeriodForSignal = "116";


        public const string AssetOverView = "117";

        public const string CableOverView = "118";//166

        public const string Bill_Submit_By_Me = "119";
        public const string View_Paid_Bills_By_Me = "120";
        public const string View_Paid_Bills_By_Employee = "121";

        public const string Delete_Payment_From_Employee_Payment_For_Mistake = "122";

        public const string Pay_Multiple_Bill = "123";
        public const string Bill_Submit_By_Admin = "124";

        public const string Add_ResellerPayment = "125";
        public const string View_ResellerPayment_List = "126";
        public const string Update_ResellerPayment = "127";
        public const string Delete_ResellerPayment = "128";

        public const string Add_New_Client_By_Reseller = "129";
        public const string Update_Client_By_Reseller = "130";
        public const string View_Client_List_By_Reseller = "131";

        public const string Add_New_Client_For_Reseller_By_Admin = "132";
        public const string Update_Client_For_Reseller_By_Admin = "133";
        public const string View_Client_List_For_Reseller_By_Admin = "134";

        public const string Add_Reseller_Zone = "135";
        public const string View_Reseller_Zone_List = "136";
        public const string Update_Reseller_Zone = "137";

        public const string Create_Reseller_Package = "138";
        public const string VIew_Reseller_Package_List_By_Himself = "139";
        public const string Update_Reseller_Package_By_Himself = "140";
        //public const string Change_Reseller_PackagePrice_By_Himself = "141";

        public const string Add_User_In_Mikrotik = "142";

        public const string View_LeaveSalary = "143";

        public const string VIew_Attendance = "144";

        public const string View_Attendance_Type = "145";
        public const string Create_Attendance_Type = "146";
        public const string Update_Attendance_Type = "147";
        public const string Delete_Attendance_Type = "148";

        public const string View_Measurement_Unit = "149";
        public const string Create_Measurement_Unit = "150";
        public const string Update_Measurement_Unit = "151";
        public const string Delete_Measurement_Unit = "152";

        public const string View_Vendor_Type = "153";
        public const string Create_Vendor_Type = "154";
        public const string Update_Vendor_Type = "155";
        public const string Delete_Vendor_Type = "156";

        public const string View_vendor = "157";
        public const string Create_vendor = "158";
        public const string Update_Vendor = "159";
        public const string Delete_Vendor = "160";

        public const string Add_Reseller_Box = "161";
        public const string View_Reseller_Box_List = "162";
        public const string Update_Reseller_Box = "163";

        public const string View_Reseller_Line_Status_History = "164";
        public const string View_All_ResellerPayment_List = "165";

        public const string View_Reseller_Accounts = "166";
        public const string View_Reseller_Accounts_By_Admin = "167";

        public const string Pay__Bill_Reseller_Clients = "168";
        public const string Pay__Bill_Reseller_Clients_By_Admin = "169";

        public const string View_Unpaid_Bills_List_Reseller_Clients = "170";
        public const string View_Unpaid_Bills_List_Reseller_Clients_By_Admin = "171";

        public const string View_Sign_Up_Bills_List_Reseller_Clients = "172";
        public const string View_Sign_Up_Bills_List_Reseller_Clients_By_Admin = "173";

        public const string Pay_Due_Bill_By_Reseller = "174";
        public const string Pay_Due_Bill_By_Admin_For_Reseller = "175";

        public const string View_Advance_Payment_Reseller_Clients = "176";
        public const string Add_Advance_Payment_Reseller_Clients = "177";
        public const string Update_Advance_Payment_Reseller_Clients = "178";

        public const string View_Advance_Payment_Reseller_Clients_By_Admin = "179";
        public const string Add_Advance_Payment_Reseller_Clients_By_Admin = "180";
        public const string Update_Advance_Payment_Reseller_Clients_By_Admin = "181";

        public const string View_Account_Owner_List = "10";
        public const string Create_Acocunt_Owner = "10";
        public const string Update_Account_Owner = "10";
        public const string Delete_Account_Owner = "10";


        public const string View_AccountList = "10";
        public const string Insert_AccountList = "10";
        public const string Record_InitialBalance = "10";
        public const string Update_AccountList = "10";
        public const string Delete_AccountList = "10";

        public const string View_Purchase = "10";
        public const string Add_Purchase = "10";
        public const string Update_Purchase = "10";
        public const string Delete_Purchase = "10";

        public const string View_Purchase_Payment = "10";
        public const string Add_Purchase_Payment = "10";
        public const string Update_Purchase_Payment = "10";
        public const string Delete_Purchase_Payment = "10";

        public const string View_Head = "10";
        public const string Create_Head = "10";
        public const string Update_Head = "10";
        public const string Delete_Head = "10";

        public const string View_Company = "10";
        public const string Create_Company = "10";
        public const string Update_Company = "10";
        public const string Delete_Company = "10";

        public const string View_CompanyVsPayer = "10";
        public const string Create_CompanyVsPayer = "10";
        public const string Update_CompanyVsPayer = "10";
        public const string Delete_CompanyVsPayer = "10";


        public const string View_Deposit = "10";
        public const string Create_Deposit = "10";
        public const string Update_Deposit = "10";
        public const string Delete_Deposit = "10";

        public const string View_AccountListVsAmountTransfer = "10";
        public const string Create_AccountListVsAmountTransfer = "10";
        public const string Update_AccountListVsAmountTransfer = "10";
        public const string Delete_AccountListVsAmountTransfer = "10";

        public const string View_AccountReport = "10";
        public const string Received_Collected_Bill = "182";

        internal static string GetStatusDivByStatusID(int statusID)
        {

            //return statusID == AppUtils.LineIsActive ? "<div style='color: green; font-weight:bold'>Active</td>"
            //    : statusID == AppUtils.LineIsLock ? "<div style='color: red; font-weight:bold'>Lock</td>"
            //    : "<div style='color: yellow; font-weight:bold'>Nothing</td>";

            return statusID == AppUtils.LineIsActive ? "<div style='color: green; font-weight:bold'>Active</div>"
                : statusID == AppUtils.LineIsLock ? "<div style='color: red; font-weight:bold'>Lock</div>"
                : "<div style='color: yellow; font-weight:bold'>Nothing</div>";
        }
        internal static string GetTableStatusDivByStatusID(int statusID)
        {

            return statusID == AppUtils.TableStatusIsActive ? "<span class='label  label-success'>Active</span>"
                : statusID == AppUtils.TableStatusIsDelete ? "<span class='label  label-danger'>Delete</span>"
                : "<span class='label  label-success'>Nothing</span>";
            // "<div style='color: green; font-weight:bold'>Active</td>" : "<div style='color: red; font-weight:bold'>Lock</td>"
        }
        //    public const string Update_User_Mikrotik = "87";
        //  public const string View_User_Mikrotik = "87";

        // public const string Set_User_Right = "";



        //public const string Account = "C1";
        //public const string AdvancePayment = "C2";
        //public const string Box = "C3";
        //public const string Brand = "C4";
        //public const string Client = "C5";
        //public const string Complain = "C6";
        //public const string DistributionReason = "C7";
        //public const string Employee = "C8";
        //public const string Excel = "C9";
        //public const string Expense = "C10";
        //public const string Hone = "C11";
        //public const string Item = "C12";
        //public const string NewClient = "C13";
        //public const string Package = "C14";
        //public const string Pop = "C15";
        //public const string ProductCurrentStatus = "C16";
        //public const string ProductStatus = "C17";
        //public const string Report = "C18";
        //public const string Section = "C19";
        //public const string Stock = "C20";
        //public const string Supplier = "C21";
        //public const string Transaction = "C22";
        //public const string Zone = "C23";




        //public const string AdvancePayment_AddAdvancePayment = "F16";
        //public const string AdvancePayment_ViewAdvancePayment = "F17";
        //public const string Box_Index = "F37";
        //public const string Box_InsertBox = "F38";
        //public const string Brand_Index = "F25";
        //public const string Brand_InsertBrand = "F26";
        //public const string Client_ActiveToLock = "F12";
        //public const string Client_LockToActive = "F13";
        //public const string Client_ViewClientPhoneList = "F18";
        //public const string Client_GetAllClients = "F1";
        //public const string Client_Create = "F2";
        //public const string Client_GetAllActiveClient = "F3";
        //public const string Client_GetAllLockCLients = "F4";
        //public const string Complain_GetAllComplainList = "F19";
        //public const string Complain_CreateComplain = "F20";
        //public const string DistributionReason_Index = "F27";
        //public const string DistributionReason_InsertDistributionReason = "F28";
        //public const string Expense_Index = "F21";
        //public const string Expense_Create = "F22";
        //public const string Item_Index = "F33";
        //public const string Item_InsertItem = "F34";
        //public const string NewClient_GetAllNewClientList = "F5";
        //public const string NewClient_NewConnection = "F6";
        //public const string Package_Index = "F7";
        //public const string Package_Create = "F8";
        //public const string Pop_Index = "F35";
        //public const string Pop_InsertPop = "F36";
        //public const string ProductCurrentStatus_TotalList_IFMistake = "F46";
        //public const string ProductCurrentStatus_avialableList = "F47";
        //public const string ProductCurrentStatus_runninglist = "F48";
        //public const string ProductCurrentStatus_TotalList = "F49";
        //public const string ProductCurrentStatus_RepairingList = "F50";
        //public const string ProductCurrentStatus_DeadList = "F51";
        //public const string ProductCurrentStatus_FindCableUsedByCableStockIDOrClientID = "F52";
        //public const string ProductStatus_Index = "F31";
        //public const string ProductStatus_InsertProductStatus = "F32";
        //public const string Section_Index = "F29";
        //public const string Section_InsertSection = "F30";
        //public const string Stock_addstock = "F41";
        //public const string Stock_StockDistribution = "F42";
        //public const string Stock_AddStockForCable = "F43";
        //public const string Stock_CableStockDistributionToEmployeeOrClient = "F44";
        //public const string Stock_stocklist = "F45";
        //public const string Supplier_Index = "F39";
        //public const string Supplier_InsertSupplier = "F40";
        //public const string Transaction_FilterBills = "F14";
        //public const string Transaction_Accounts = "F9";
        //public const string Transaction_UnpaidBills = "F10";
        //public const string Transaction_SignUpBills = "F11";
        //public const string Zone_Index = "F23";
        //public const string Zone_InsertZone = "F24";

        //public const string Client_GetAllActiveClient_New__Client = "B12";
        //public const string Client_GetAllActiveClient_Lock__Client = "B13";
        //public const string Client_GetAllActiveClient_Export__Excel = "B14";
        //public const string Client_GetAllActiveClient_Update__Client = "B15";
        //public const string Client_GetAllLockCLients_New__Client = "B16";
        //public const string Client_GetAllLockCLients_Active__Client = "B17";
        //public const string Client_GetAllLockCLients_Export__Excel = "B18";
        //public const string Client_GetAllLockCLients_Update__Client = "B19";
        //public const string Complain_GetAllComplainList_New__Complain = "B66";
        //public const string Complain_GetAllComplainList_Edit__Complain = "B67";
        //public const string Complain_GetAllComplainList_Delete__Complain = "B68";
        //public const string Complain_GetAllComplainList_Solve__Complain = "B69";
        //public const string Complain_CreateComplain_Save__Complain = "B65";
        //public const string DistributionReason_Index_Create__New = "B86";
        //public const string DistributionReason_Index_Save__Distribution = "B87";
        //public const string DistributionReason_Index_Update__Distribution = "B88";
        //public const string DistributionReason_InsertDistributionReason_Save__Distribution = "B89";
        //public const string Expense_Index_New__Expense = "B70";
        //public const string Expense_Index_Delete__Expense = "B71";
        //public const string Expense_Index_Save__Expense = "B72";
        //public const string Expense_Index_Export__Excel = "B73";
        //public const string Expense_Create_Save__New__Expense = "B74";
        //public const string Item_Index_Create__New = "B98";
        //public const string Item_Index_Save__Item = "B99";
        //public const string Item_Index_Update__Item = "B100";
        //public const string Item_InsertItem_Save__Item = "B101";
        //public const string NewClient_GetAllNewClientList_New__Conection__Setup = "B26";
        //public const string NewClient_GetAllNewClientList_Sign__Up = "B27";
        //public const string NewClient_GetAllNewClientList_Delete__New__Client = "B28";
        //public const string NewClient_GetAllNewClientList_View__For__Update = "B29";
        //public const string NewClient_GetAllNewClientList_Update__New__Client__Info = "B30";
        //public const string NewClient_GetAllNewClientList_Item__Assign__To__Client = "B31";
        //public const string NewClient_NewConnection_Save__New__Connection = "B32";
        //public const string Package_Index_Save__Package = "B75";
        //public const string Package_Index_Update__Package = "B76";
        //public const string Package_Create_Save__Package = "B77";
        //public const string Pop_Index_Create__New = "B102";
        //public const string Pop_Index_Save__Pop = "B103";
        //public const string Pop_Index_Update__Pop = "B104";
        //public const string Pop_InsertPop_Save__Pop = "B105";
        //public const string ProductCurrentStatus_TotalList_Update_IFMistake = "B123";
        //public const string ProductCurrentStatus_avialableList_Update = "B124";
        //public const string ProductCurrentStatus_TotalList_Update = "B125";
        //public const string ProductCurrentStatus_RepairingList_Update = "B126";
        //public const string ProductCurrentStatus_DeadList_Update = "B127";
        //public const string ProductCurrentStatus_FindCableUsedByCableStockIDOrClientID_EditORUpdate = "B128";
        //public const string ProductStatus_Index_Create__New = "B94";
        //public const string ProductStatus_Index_Save__Product = "B95";
        //public const string ProductStatus_Index_Update__Product = "B96";
        //public const string ProductStatus_InsertProductStatus_Save__Product = "B97";
        //public const string Section_Index_Create__New = "B90";
        //public const string Section_Index_Save__Section = "B91";
        //public const string Section_Index_Update__Section = "B92";
        //public const string Section_InsertSection_Search = "B93";
        //public const string Stock_addstock_Add__In__List = "B114";
        //public const string Stock_addstock_Stock__Save = "B115";
        //public const string Stock_StockDistribution_Add__In__List = "B116";
        //public const string Stock_StockDistribution_Save = "B117";
        //public const string Stock_AddStockForCable_Add__In__List = "B118";
        //public const string Stock_AddStockForCable_Cable__Stock__Save = "B119";
        //public const string Stock_CableStockDistributionToEmployeeOrClient_Add__In__List = "B120";
        //public const string Stock_CableStockDistributionToEmployeeOrClient_Cable__Distribution__Save = "B121";
        //public const string Stock_stocklist_Delete = "B122";
        //public const string Supplier_Index_Create__New = "B110";
        //public const string Supplier_Index_Save__Supplier = "B111";
        //public const string Supplier_Index_Update__Supplier = "B112";
        //public const string Supplier_InsertSupplier_Save__Supplier = "B113";
        //public const string Transaction_Accounts_Bill__Pay__Check__Box = "B33";
        //public const string Transaction_Accounts_Pay__Bill = "B34";
        //public const string Transaction_Accounts_Generate__Bill = "B35";
        //public const string Transaction_Accounts_Adjust__Due__Bill = "B36";
        //public const string Transaction_Accounts_Collect__Advance__Payment = "B37";
        //public const string Transaction_Accounts_Unpaid__Bill = "B38";
        //public const string Transaction_Accounts_Sign__Up__Bill = "B39";
        //public const string Transaction_Accounts_Bill__Print = "B40";
        //public const string Transaction_Accounts_Export__Excel = "B41";
        //public const string Transaction_Accounts_Print__Report = "B42";
        //public const string Transaction_UnpaidBills_Bill__Pay__Check__Box = "B43";
        //public const string Transaction_UnpaidBills_Pay__Bill = "B44";
        //public const string Transaction_UnpaidBills_Generate__Bill = "B45";
        //public const string Transaction_UnpaidBills_Adjust__Due__Bill = "B46";
        //public const string Transaction_UnpaidBills_Collect__Advance__Payment = "B47";
        //public const string Transaction_UnpaidBills_Unpaid__Bill = "B48";
        //public const string Transaction_UnpaidBills_Sign__Up__Bill = "B49";
        //public const string Transaction_UnpaidBills_Bill__Print = "B50";
        //public const string Transaction_UnpaidBills_Export__Excel = "B51";
        //public const string Transaction_UnpaidBills_Print__Report = "B52";
        //public const string Zone_Index_Create__New = "B78";
        //public const string Zone_Index_Save__Zone = "B79";
        //public const string Zone_Index_Update__Zone = "B80";
        //public const string Zone_InsertZone_Save__Zone = "B81";


    }
}


