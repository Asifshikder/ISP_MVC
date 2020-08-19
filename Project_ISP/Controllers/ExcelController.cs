using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using Month = OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Month;

namespace Project_ISP.Controllers
{
    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class ExcelController : Controller
    {
        public ExcelController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForAssetList(int? AssetTypeID)
        {

            IEnumerable<Asset> lstAsset;
            if (AssetTypeID == null || AssetTypeID == 0)
            {
                lstAsset = db.Asset.AsEnumerable();
            }
            else
            {lstAsset = db.Asset.Where(s=>s.AssetTypeID == AssetTypeID).AsEnumerable();
                

            }

            var myCustomFormatAssetInformation = lstAsset.Select(
                s => new
                {
                    AssetTypeName = s.AssetType.AssetTypeName,
                    AssetName = s.AssetName,
                    AssetValue = s.AssetValue,
                    PurchaseDate = "'"+s.PurchaseDate,
                    Serial = s.SerialNumber,
                    WarrentyStart = "'" + s.WarrentyStartDate,
                    WarrentyEnd = "'"+s.WarrentyEndDate

                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAllAsset(ref workbook, myCustomFormatAssetInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = (AssetTypeID == null) ? "AllAssetList" + AppUtils.GetDateTimeNow() + ".xlsx" : "AllAssetList_" + db.AssetType.Find(AssetTypeID).AssetTypeName + "_" + AppUtils.GetDateTimeNow() + ".xlsx" }
            };
        }



        private void LoadWorkBookForAllAsset(ref ExcelPackage workbook, dynamic lstAsset)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "SL_No";
            workSheet.Cells[1, 2].Value = "Asset Type";
            workSheet.Cells[1, 3].Value = "Asset Name";
            workSheet.Cells[1, 4].Value = "Asset Value";
            workSheet.Cells[1, 5].Value = "Purchase Date";
            workSheet.Cells[1, 6].Value = "Serial";
            workSheet.Cells[1, 7].Value = "Warrenty Start";
            workSheet.Cells[1, 8].Value = "Warrenty End";
            int recordIndex = 2;
            foreach (var info in lstAsset)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.AssetTypeName;
                workSheet.Cells[recordIndex, 3].Value = info.AssetName;
                workSheet.Cells[recordIndex, 4].Value = info.AssetValue;
                workSheet.Cells[recordIndex, 5].Value = info.PurchaseDate;
                workSheet.Cells[recordIndex, 6].Value = info.Serial;
                workSheet.Cells[recordIndex, 7].Value = info.WarrentyStart;
                workSheet.Cells[recordIndex, 8].Value = info.WarrentyEnd;

                //workSheet.Cells[recordIndex, 19].Style.Font.Color.SetColor(Color.White);
                //workSheet.Cells[recordIndex, 19].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //if ((info.LineStatusInThisMonth == "'Active"))
                //{
                //    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                //}
                //else if (info.LineStatusInThisMonth == "'InActive")
                //{
                //    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}
                //else if (info.LineStatusInThisMonth == "'Lock")
                //{
                //    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                //}
                //else
                //{
                //    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                //}

                //(info.LineStatusInThisMonth == AppUtils.LineIsActive) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green)
                //    : (info.LineStatusInThisMonth == AppUtils.LineIsInActive) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
                //    : (info.LineStatusInThisMonth == AppUtils.LineIsLock) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red)
                //    : workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

              //  workSheet.Cells[recordIndex, 19].Value = info.LineStatusInThisMonth;
                recordIndex++;
            }
        }
        // GET: Excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForAllClient(int? ZoneID)
        {

            IEnumerable<ClientLineStatus> lstClientLineStatus;
            if (ZoneID == null || ZoneID == 0)
            {
                lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).AsEnumerable();
            }
            else
            {
                lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.ClientDetails.ZoneID == ZoneID).AsEnumerable();

            }

            var myCustomFormatClientInformation = lstClientLineStatus.Select(
                s => new
                {
                    ClientID = s.ClientDetailsID,
                    Name = s.ClientDetails.Name,
                    Login_Name = s.ClientDetails.LoginName,
                    Email = s.ClientDetails.Email,
                    Zone = s.ClientDetails.Zone.ZoneName,
                    Address = s.ClientDetails.Address,
                    Connection_Date = "'" + s.LineStatusChangeDate,
                    MacAddress = "'" + s.ClientDetails.MacAddress,
                    Bill_Payment_Date = "'" + s.LineStatusChangeDate,
                    Contact_Number = "'" + s.ClientDetails.ContactNumber,
                    SMS_Communication = "'" + s.ClientDetails.SMSCommunication,
                    Package = s.Package.PackageName,
                    BandWidth = s.Package.PackagePrice,
                    Occupation = s.ClientDetails.Occupation,
                    Social_Communication_URL = s.ClientDetails.SocialCommunicationURL,
                    Connection_Type = (s.ClientDetails.ConnectionType != null) ? s.ClientDetails.ConnectionType.ConnectionTypeName : "",
                    Cable_Type = (s.ClientDetails.CableType != null) ? s.ClientDetails.CableType.CableTypeName : "",
                    NationalID = "'" + s.ClientDetails.NationalID,
                    LineStatusInThisMonth = "'" + s.LineStatus.LineStatusName
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAllClient(ref workbook, myCustomFormatClientInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = (ZoneID == null) ? "AllClientList_AllZone_" + AppUtils.GetDateTimeNow() + ".xlsx" : "AllClientList_" + db.Zone.Find(ZoneID).ZoneName + "_" + AppUtils.GetDateTimeNow() + ".xlsx" }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForActiveClient(int? ZoneID)
        {

            //List<ClientLineStatus> lstClientLineStatus = new List<ClientLineStatus>();
            //if (ZoneID == null || ZoneID == 0)
            //{
            //    lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsActive).ToList();
            //}
            //else
            //{
            //    lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.ClientDetails.ZoneID == ZoneID && s.LineStatusID == AppUtils.LineIsActive).ToList();

            //}

            //var myCustomFormatClientInformation = lstClientLineStatus
            //    .Select(s => new
            //{
            //    ClientID = s.ClientDetailsID,
            //    Name = s.ClientDetails.Name,
            //    Login_Name = s.ClientDetails.LoginName,
            //    Email = s.ClientDetails.LoginName,
            //    Zone = s.ClientDetails.Zone.ZoneName,
            //    Address = s.ClientDetails.Address,
            //    Connection_Date = "'" + s.LineStatusChangeDate,
            //    MacAddress = "'" + s.ClientDetails.MacAddress,
            //    Bill_Payment_Date = "'" + s.LineStatusChangeDate,
            //    Contact_Number = "'" + s.ClientDetails.ContactNumber,
            //    SMS_Communication = "'" + s.ClientDetails.SMSCommunication,
            //    Package = s.Package.PackageName,
            //    BandWidth = s.Package.PackagePrice,
            //    Occupation = s.ClientDetails.Occupation,
            //    Social_Communication_URL = s.ClientDetails.SocialCommunicationURL,
            //    Connection_Type = s.ClientDetails.ConnectionType != null ? s.ClientDetails.ConnectionType.ConnectionTypeName : "",
            //    Cable_Type = s.ClientDetails.CableType != null ?  s.ClientDetails.CableType.CableTypeName : "",
            //    NationalID = "'" + s.ClientDetails.NationalID,
            //    LineStatusInThisMonth = "'" + s.LineStatus.LineStatusName
            //}).ToList();

            int zoneFromDDL = 0;
            if (ZoneID != null)
            {
                zoneFromDDL = int.Parse(ZoneID.Value.ToString());
            }

            List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
            /// this are the clients from transaction who is lock
            List<int> lstActiveClients = (ZoneID == null) ?
                db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList()
                : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsLock).Select(s => s.ClientDetailsID).ToList();


            //// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
            var firstPartOfQuery =
                (ZoneID != null)
                    ? db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
                        .GroupBy(s => new { s.ClientDetailsID },
                            (key, g) => g.OrderByDescending(s => new { s.PaymentYear, s.PaymentMonth })
                                .FirstOrDefault()).Where(s => !lstActiveClients.Contains(s.ClientDetailsID))
                        .AsQueryable()
                    : db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth)
                        .GroupBy(s => new { s.ClientDetailsID },
                            (key, g) => g.OrderByDescending(s => new { s.PaymentYear, s.PaymentMonth })
                                .FirstOrDefault()).Where(s => !lstActiveClients.Contains(s.ClientDetailsID))
                        .AsQueryable();
            //var firstPartOfQuery =
            //                   (ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstActiveClients.Contains(s.ClientDetailsID)).AsQueryable()
            //                       : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstActiveClients.Contains(s.ClientDetailsID)).AsQueryable();


            var secondPartOfQuery = firstPartOfQuery
                .GroupJoin(
                    db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                        (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
                    Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                    (Transaction, ClientLineStatus) => new
                    {
                        Transaction = Transaction,
                        ClientLineStatus = ClientLineStatus.FirstOrDefault()
                    }).AsEnumerable();


            var myCustomFormatClientInformation = secondPartOfQuery.Select(s => new
            {
                ClientID = s.Transaction.ClientDetailsID,
                Name = s.Transaction.ClientDetails.Name,
                Login_Name = s.Transaction.ClientDetails.LoginName,
                Email = s.Transaction.ClientDetails.LoginName,
                Zone = s.Transaction.ClientDetails.Zone.ZoneName,
                Address = s.Transaction.ClientDetails.Address,
                Connection_Date = "'" + s.ClientLineStatus.LineStatusChangeDate,
                MacAddress = "'" + s.Transaction.ClientDetails.MacAddress,
                Bill_Payment_Date = "'" + s.ClientLineStatus.LineStatusChangeDate,
                Contact_Number = "'" + s.Transaction.ClientDetails.ContactNumber,
                SMS_Communication = "'" + s.Transaction.ClientDetails.SMSCommunication,
                Package = s.Transaction.Package.PackageName,
                BandWidth = s.Transaction.Package.PackagePrice,
                Occupation = s.Transaction.ClientDetails.Occupation,
                Social_Communication_URL = s.Transaction.ClientDetails.SocialCommunicationURL,
                Connection_Type = s.Transaction.ClientDetails.ConnectionType != null ? s.Transaction.ClientDetails.ConnectionType.ConnectionTypeName : "",
                Cable_Type = s.Transaction.ClientDetails.CableType != null ? s.Transaction.ClientDetails.CableType.CableTypeName : "",
                NationalID = "'" + s.Transaction.ClientDetails.NationalID,
                LineStatusInThisMonth = "'Active"
            }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAllClient(ref workbook, myCustomFormatClientInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = (ZoneID == null) ? "AllActiveClient_AllZone_" + AppUtils.GetDateTimeNow() + ".xlsx" : "AllActiveClient_" + db.Zone.Find(ZoneID).ZoneName + "_" + AppUtils.GetDateTimeNow() + ".xlsx" }
            };
        }

        private void LoadWorkBookForAllClient(ref ExcelPackage workbook, dynamic lstClientLineStatus)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "SL_No";
            workSheet.Cells[1, 2].Value = "ClientID";
            workSheet.Cells[1, 3].Value = "Name";
            workSheet.Cells[1, 4].Value = "Login_Name";
            workSheet.Cells[1, 5].Value = "Email";
            workSheet.Cells[1, 6].Value = "Zone";
            workSheet.Cells[1, 7].Value = "Connection_Date";
            workSheet.Cells[1, 8].Value = "MacAddress";
            workSheet.Cells[1, 9].Value = "Bill_Payment_Date";
            workSheet.Cells[1, 10].Value = "Contact_Number";
            workSheet.Cells[1, 11].Value = "SMS_Communication";
            workSheet.Cells[1, 12].Value = "Package";
            workSheet.Cells[1, 13].Value = "BandWidth";
            workSheet.Cells[1, 14].Value = "Occupation";
            workSheet.Cells[1, 15].Value = "Social_Communication_URL";
            workSheet.Cells[1, 16].Value = "Connection_Type";
            workSheet.Cells[1, 17].Value = "Cable_Type";
            workSheet.Cells[1, 18].Value = "NationalID";
            workSheet.Cells[1, 19].Value = "LineStatusInThisMonth";

            int recordIndex = 2;
            foreach (var info in lstClientLineStatus)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.ClientID;
                workSheet.Cells[recordIndex, 3].Value = info.Name;
                workSheet.Cells[recordIndex, 4].Value = info.Login_Name;
                workSheet.Cells[recordIndex, 5].Value = info.Email;
                workSheet.Cells[recordIndex, 6].Value = info.Zone;
                workSheet.Cells[recordIndex, 7].Value = info.Connection_Date;
                workSheet.Cells[recordIndex, 8].Value = info.MacAddress;
                workSheet.Cells[recordIndex, 9].Value = info.Bill_Payment_Date;
                workSheet.Cells[recordIndex, 10].Value = info.Contact_Number;
                workSheet.Cells[recordIndex, 11].Value = info.SMS_Communication;
                workSheet.Cells[recordIndex, 12].Value = info.Package;
                workSheet.Cells[recordIndex, 13].Value = info.BandWidth;
                workSheet.Cells[recordIndex, 14].Value = info.Occupation;
                workSheet.Cells[recordIndex, 15].Value = info.Social_Communication_URL;
                workSheet.Cells[recordIndex, 16].Value = info.Connection_Type;
                workSheet.Cells[recordIndex, 17].Value = info.Cable_Type;
                workSheet.Cells[recordIndex, 18].Value = info.NationalID;

                workSheet.Cells[recordIndex, 19].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells[recordIndex, 19].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                if ((info.LineStatusInThisMonth == "'Active"))
                {
                    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                }
                else if (info.LineStatusInThisMonth == "'InActive")
                {
                    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                }
                else if (info.LineStatusInThisMonth == "'Lock")
                {
                    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                }
                else
                {
                    workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                }

                //(info.LineStatusInThisMonth == AppUtils.LineIsActive) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green)
                //    : (info.LineStatusInThisMonth == AppUtils.LineIsInActive) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow)
                //    : (info.LineStatusInThisMonth == AppUtils.LineIsLock) ? workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red)
                //    : workSheet.Cells[recordIndex, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);

                workSheet.Cells[recordIndex, 19].Value = info.LineStatusInThisMonth;
                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForLockClient(int? ZoneID)
        {

            //List<ClientLineStatus> lstClientLineStatus = new List<ClientLineStatus>();
            //if (ZoneID == null || ZoneID == 0)
            //{
            //    lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.LineStatusID == AppUtils.LineIsLock).ToList();
            //}
            //else
            //{
            //    lstClientLineStatus = db.ClientLineStatus.GroupBy(s => s.ClientDetailsID, (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()).Where(s => s.ClientDetails.ZoneID == ZoneID && s.LineStatusID == AppUtils.LineIsLock).ToList();

            //}
            int zoneFromDDL = 0;
            if (ZoneID != null)
            {
                zoneFromDDL = int.Parse(ZoneID.Value.ToString());
            }

            List<ClientCustomInformation> lstClientCustomInformation = new List<ClientCustomInformation>();
            /// this are the clients from transaction who is lock
            List<int> lstActiveClients = (ZoneID == null) ?
                db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList()
                : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.LineStatusID == AppUtils.LineIsActive).Select(s => s.ClientDetailsID).ToList();


            //// Now i am searching for the active client. this are the final active clients cause bill is generate or sign up in this month
            var firstPartOfQuery =
                (ZoneID != null)
                    ? db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL)
                        .GroupBy(s => new { s.ClientDetailsID },
                            (key, g) => g.OrderByDescending(s => new { s.PaymentYear, s.PaymentMonth })
                                .FirstOrDefault()).Where(s => !lstActiveClients.Contains(s.ClientDetailsID))
                        .AsQueryable()
                    : db.Transaction
                        .GroupBy(s => new { s.ClientDetailsID },
                            (key, g) => g.OrderByDescending(s => new { s.PaymentYear, s.PaymentMonth })
                                .FirstOrDefault()).Where(s => !lstActiveClients.Contains(s.ClientDetailsID))
                        .AsQueryable();
            //var firstPartOfQuery =
            //                   (ZoneID == "") ? db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstActiveClients.Contains(s.ClientDetailsID)).AsQueryable()
            //                       : db.Transaction.Where(s => s.ClientDetails.ZoneID == zoneFromDDL && s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && !lstActiveClients.Contains(s.ClientDetailsID)).AsQueryable();


            var secondPartOfQuery = firstPartOfQuery
                .GroupJoin(
                    db.ClientLineStatus.GroupBy(s => s.ClientDetailsID,
                        (key, g) => g.OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault()),
                    Transaction => Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID,
                    (Transaction, ClientLineStatus) => new
                    {
                        Transaction = Transaction,
                        ClientLineStatus = ClientLineStatus.FirstOrDefault()
                    }).AsEnumerable();


            var myCustomFormatClientInformation = secondPartOfQuery.Select(s => new
            {
                ClientID = s.Transaction.ClientDetailsID,
                Name = s.Transaction.ClientDetails.Name,
                Login_Name = s.Transaction.ClientDetails.LoginName,
                Email = s.Transaction.ClientDetails.LoginName,
                Zone = s.Transaction.ClientDetails.Zone.ZoneName,
                Address = s.Transaction.ClientDetails.Address,
                Connection_Date = "'" + s.ClientLineStatus.LineStatusChangeDate,
                MacAddress = "'" + s.Transaction.ClientDetails.MacAddress,
                Bill_Payment_Date = "'" + s.ClientLineStatus.LineStatusChangeDate,
                Contact_Number = "'" + s.Transaction.ClientDetails.ContactNumber,
                SMS_Communication = "'" + s.Transaction.ClientDetails.SMSCommunication,
                Package = s.Transaction.Package.PackageName,
                BandWidth = s.Transaction.Package.PackagePrice,
                Occupation = s.Transaction.ClientDetails.Occupation,
                Social_Communication_URL = s.Transaction.ClientDetails.SocialCommunicationURL,
                Connection_Type = s.Transaction.ClientDetails.ConnectionType != null ? s.Transaction.ClientDetails.ConnectionType.ConnectionTypeName : "",
                Cable_Type = s.Transaction.ClientDetails.CableType != null ? s.Transaction.ClientDetails.CableType.CableTypeName : "",
                NationalID = "'" + s.Transaction.ClientDetails.NationalID,
                LineStatusInThisMonth = "'Lock"
            }).ToList();
            //var myCustomFormatClientInformation = lstClientLineStatus.Select(s => new
            //{
            //    ClientID = s.ClientDetailsID,
            //    Name = s.ClientDetails.Name,
            //    Login_Name = s.ClientDetails.LoginName,
            //    Email = s.ClientDetails.LoginName,
            //    Zone = s.ClientDetails.Zone.ZoneName,
            //    Address = s.ClientDetails.Address,
            //    Connection_Date = "'" + s.LineStatusChangeDate,
            //    MacAddress = "'" + s.ClientDetails.MacAddress,
            //    Bill_Payment_Date = "'" + s.LineStatusChangeDate,
            //    Contact_Number = "'" + s.ClientDetails.ContactNumber,
            //    SMS_Communication = "'" + s.ClientDetails.SMSCommunication,
            //    Package = s.Package.PackageName,
            //    BandWidth = s.Package.PackagePrice,
            //    Occupation = s.ClientDetails.Occupation,
            //    Social_Communication_URL = s.ClientDetails.SocialCommunicationURL,
            //    Connection_Type = s.ClientDetails.ConnectionType.ConnectionTypeName,
            //    Cable_Type = s.ClientDetails.CableType != null? s.ClientDetails.CableType.CableTypeName:"",
            //    NationalID = "'" + s.ClientDetails.NationalID,
            //    LineStatusInThisMonth = "'" + s.LineStatus.LineStatusName
            //}).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAllClient(ref workbook, myCustomFormatClientInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = (ZoneID == null) ? "AllLockClient_AllZone_" + AppUtils.GetDateTimeNow() + ".xlsx" : "AllLockClient_" + db.Zone.Find(ZoneID).ZoneName + "_" + AppUtils.GetDateTimeNow() + ".xlsx" }
            };
        }
        //private void LoadWorkBookForAllClient(ref ExcelPackage workbook, dynamic lstClientLineStatus)
        //{



        //    var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
        //    workSheet.TabColor = System.Drawing.Color.Black;
        //    workSheet.DefaultRowHeight = 12;
        //    workSheet.Cells[1, 1].Value = "SL_No";
        //    workSheet.Cells[1, 2].Value = "ClientID";
        //    workSheet.Cells[1, 3].Value = "Name";
        //    workSheet.Cells[1, 4].Value = "Login_Name";
        //    workSheet.Cells[1, 5].Value = "Email";
        //    workSheet.Cells[1, 6].Value = "Zone";
        //    workSheet.Cells[1, 7].Value = "Connection_Date";
        //    workSheet.Cells[1, 8].Value = "MacAddress";
        //    workSheet.Cells[1, 9].Value = "Bill_Payment_Date";
        //    workSheet.Cells[1, 10].Value = "Contact_Number";
        //    workSheet.Cells[1, 11].Value = "SMS_Communication";
        //    workSheet.Cells[1, 12].Value = "Package";
        //    workSheet.Cells[1, 13].Value = "BandWidth";
        //    workSheet.Cells[1, 14].Value = "Occupation";
        //    workSheet.Cells[1, 15].Value = "Social_Communication_URL";
        //    workSheet.Cells[1, 16].Value = "Connection_Type";
        //    workSheet.Cells[1, 17].Value = "Cable_Type";
        //    workSheet.Cells[1, 18].Value = "NationalID";
        //    workSheet.Cells[1, 19].Value = "LineStatusInThisMonth";

        //    int recordIndex = 2;
        //    foreach (var info in lstClientLineStatus)
        //    {
        //        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
        //        workSheet.Cells[recordIndex, 2].Value = info.ClientID;
        //        workSheet.Cells[recordIndex, 3].Value = info.Name;
        //        workSheet.Cells[recordIndex, 4].Value = info.Login_Name;
        //        workSheet.Cells[recordIndex, 5].Value = info.Email;
        //        workSheet.Cells[recordIndex, 6].Value = info.Zone;
        //        workSheet.Cells[recordIndex, 7].Value = info.Connection_Date;
        //        workSheet.Cells[recordIndex, 8].Value = info.MacAddress;
        //        workSheet.Cells[recordIndex, 9].Value = info.Bill_Payment_Date;
        //        workSheet.Cells[recordIndex, 10].Value = info.Contact_Number;
        //        workSheet.Cells[recordIndex, 11].Value = info.SMS_Communication;
        //        workSheet.Cells[recordIndex, 12].Value = info.Package;
        //        workSheet.Cells[recordIndex, 13].Value = info.BandWidth;
        //        workSheet.Cells[recordIndex, 14].Value = info.Occupation;
        //        workSheet.Cells[recordIndex, 15].Value = info.Social_Communication_URL;
        //        workSheet.Cells[recordIndex, 16].Value = info.Connection_Type;
        //        workSheet.Cells[recordIndex, 17].Value = info.Cable_Type;
        //        workSheet.Cells[recordIndex, 18].Value = info.NationalID;
        //        workSheet.Cells[recordIndex, 19].Value = info.LineStatusInThisMonth;
        //        recordIndex++;
        //    }
        //}


        //private byte[] ConvertDataSetToByteArray(DataTable dataTable)
        //{
        //    byte[] binaryDataResult = null;
        //    using (MemoryStream memStream = new MemoryStream())
        //    {
        //        BinaryFormatter brFormatter = new BinaryFormatter();
        //        dataTable.RemotingFormat = SerializationFormat.Binary;
        //        brFormatter.Serialize(memStream, dataTable);
        //        binaryDataResult = memStream.ToArray();
        //    }
        //    return binaryDataResult;
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForExpense(DateTime? startDate, DateTime? EndDate, string expenseSubject)
        {
            string FileName = "";
            EndDate = (EndDate != null) ? AppUtils.GetLastDayWithHrMinSecMsByMyDate(EndDate.Value) : EndDate;
            List<Expense> lstExpense = new List<Expense>();
            //if (startDate != null && EndDate != null && !string.IsNullOrEmpty(expenseSubject))
            //{
            //    FileName = "ExpenseList_From_Date:_" + startDate + "_EndDate:_" + EndDate + "_Subject:_" + expenseSubject;
            //    lstExpense = db.Expenses.Where(s => s.PaymentDate >= startDate.Value && s.PaymentDate <= EndDate && s.Subject.Contains(expenseSubject)).ToList();
            //}
            //else if (startDate != null && EndDate != null)
            //{
            //    FileName = "ExpenseList_From_Date:_" + startDate + "_EndDate:_" + EndDate;
            //    lstExpense = db.Expenses.Where(s => s.PaymentDate >= startDate.Value && s.PaymentDate <= EndDate).ToList();
            //}
            //else if (!string.IsNullOrEmpty(expenseSubject))
            //{
            //    FileName = "ExpenseList_Subject:_" + expenseSubject;
            //    lstExpense = db.Expenses.Where(s => s.Subject.Contains(expenseSubject)).ToList();
            //}
            //else
            //{

            //    FileName = "ExpenseListOf_Year:" + AppUtils.ThisMonthStartDate().Year + "_Month:" + AppUtils.ThisMonthStartDate().Month;
            //    DateTime sDate = AppUtils.ThisMonthStartDate();
            //    DateTime eDate = AppUtils.ThisMonthLastDate();
            //    lstExpense = db.Expenses.Where(s => s.PaymentDate >= sDate && s.PaymentDate <= eDate).ToList();
            //}
            //var myExpenseList = lstExpense.Select(s => new
            //{
            //    Subject = s.Subject,
            //    Details = s.Details,
            //    Paid_To = s.PaidTo,
            //    Amount = s.Amount,
            //    Paid_By = db.Employee.Find(s.EmployeeID).Name,
            //    Payment_Date = "'" + s.PaymentDate,
            //}).ToList();

            ExcelPackage workbook = new ExcelPackage();
            //LoadWorkBookForExpense(ref workbook, myExpenseList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForArchiveBills(int? Year, int? Month, int? ZoneID)
        {

            int ifSearch = 0;
            int totalRecords = 0;
            int recFilter = 0;
            string FileName = "";
            if (Year != null)
            {
                Year = int.Parse(db.Year.Find(Year.Value).YearName);
            }

            //  List<VM_Transaction_ClientDueBills> VM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            List<Transaction> lstTransaction = new List<Transaction>();
            //var VM_Transaction_ClientDueBills = db.Transaction
            //    .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                             (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                             .SelectMany(
            //                                variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                                (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                             );
            //dynamic d = new ExpandoObject(); 
            object d = new object();
            dynamic dd = new ExpandoObject();
            IEnumerable<dynamic> topAgents = new List<dynamic>();
            if (Year != null && Month != null && ZoneID != null)
            {
                FileName = "ArchiveBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                // topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else if (Year != null && Month != null)
            {
                FileName = "ArchiveBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value;
                // topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value).AsQueryable();
            }
            else if (Year != null && ZoneID != null)
            {
                FileName = "ArchiveBillsList_Year:_" + Year.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else if (ZoneID != null)
            {
                FileName = "ArchiveBillsList_:ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else
            {
                DateTime sDate = AppUtils.ThisMonthStartDate();
                DateTime eDate = AppUtils.ThisMonthLastDate();
                FileName = "ArchiveBillsList_Year:_" + sDate.Year + "_Month:_" + sDate.Month + "_AllZone";
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == sDate.Year && s.PaymentMonth == sDate.Month).AsQueryable();
            }

            List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstArchiveBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

            var firstPartOfQuery =
                    (Year != null && Month != null && ZoneID != null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentMonth.ToString() == Month.Value.ToString() && s.ClientDetails.ZoneID.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                        : (Year != null && Month != null && ZoneID == null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentMonth.ToString() == Month.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                            : (Year != null && Month == null && ZoneID != null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.ClientDetails.ZoneID.Value.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                : (Year != null && Month == null && ZoneID == null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                    : (Year == null && Month == null && ZoneID != null) ? db.Transaction.Where(s => s.ClientDetails.ZoneID.Value.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                                        :
                                        db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly).AsQueryable()
                ;
            var secondPartOfQuery = firstPartOfQuery
                .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                    ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                    {
                        Transaction = Transaction,
                        ClientDueBills = ClientDueBills
                    })
                .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                    (Transaction, EmployeeTransactionLockUnlock) => new
                    {
                        Transaction = Transaction.Transaction,
                        ClientDueBills = Transaction.ClientDueBills,
                        EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                    })
                //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //{
                //      Transaction = Transaction.Transaction,

                //})
                .AsEnumerable();

            if (secondPartOfQuery.Count() > 0)
            {
                totalRecords = secondPartOfQuery.Count();
                lstArchiveBillsInformation = secondPartOfQuery.AsEnumerable().Select(
                    s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                    {
                        ClientName = s.Transaction.ClientDetails.Name,
                        Address = s.Transaction.ClientDetails.Address,
                        ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                        ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                        PackageID = s.Transaction.PackageID.Value,
                        PackageName = s.Transaction.Package.PackageName,
                        MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                        FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                        PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                        Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                        PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                        CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                        PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                        RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                        ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                        StatusThisMonthID = s.Transaction.LineStatusID.Value,
                        Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""
                    }).ToList();

            }

            //var ss = topAgents.GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                              (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                              .SelectMany(
            //                                 variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                                 (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                              )
            //    .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //    .Select(s => new
            //    {
            //        Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //        LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //        Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //        Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //        Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //        Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //        BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //        MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //        DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //    }).ToList(); ;


            //var transactionList = d.Select(s => new
            //{
            //    Name = s[0].Transaction.ClientDetails.Name,
            //    LoginName = s.Transaction.ClientDetails.LoginName,
            //    Contact = s.Transaction.ClientDetails.ContactNumber,
            //    Zone = s.Transaction.ClientDetails.Zone.ZoneName,
            //    Address = s.Transaction.ClientDetails.Address,
            //    Package = s.Transaction.Package.PackageName,
            //    BandWith = s.Transaction.Package.BandWith,
            //    MonthlyFee = s.Transaction.Package.PackagePrice,
            //    DueAmount = (s.ClientDueBills == null) ? "0" : SqlFunctions.StringConvert(s.ClientDueBills.DueAmount),

            //}).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForArchiveBillsOrDueBills(ref workbook, lstArchiveBillsInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }

        private void LoadWorkBookForArchiveBillsOrDueBills(ref ExcelPackage workbook, dynamic myExpenseList)
        {


            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "SL_No";

            workSheet.Cells[1, 2].Value = "Client Name";
            workSheet.Cells[1, 3].Value = "Address";
            workSheet.Cells[1, 4].Value = "Contact Number";
            workSheet.Cells[1, 5].Value = "Zone";
            workSheet.Cells[1, 6].Value = "Package";
            workSheet.Cells[1, 7].Value = "Monthly Fee";
            workSheet.Cells[1, 8].Value = "FeeForThisMonth";
            workSheet.Cells[1, 9].Value = "PaidAmount";
            workSheet.Cells[1, 10].Value = "Due";
            workSheet.Cells[1, 11].Value = "Status";
            workSheet.Cells[1, 12].Value = "PaidBy";
            workSheet.Cells[1, 13].Value = "PaidTime";
            workSheet.Cells[1, 14].Value = "RemarksNo";
            workSheet.Cells[1, 15].Value = "ReceiptNo";


            int recordIndex = 2;
            foreach (var info in myExpenseList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.ClientName;
                workSheet.Cells[recordIndex, 3].Value = info.Address;
                workSheet.Cells[recordIndex, 4].Value = info.ContactNumber;
                workSheet.Cells[recordIndex, 5].Value = info.ZoneName;
                workSheet.Cells[recordIndex, 6].Value = info.PackageName;
                workSheet.Cells[recordIndex, 7].Value = info.MonthlyFee;
                workSheet.Cells[recordIndex, 8].Value = info.FeeForThisMonth;
                workSheet.Cells[recordIndex, 9].Value = info.PaidAmount;
                workSheet.Cells[recordIndex, 10].Value = info.Due;
                //workSheet.Cells[recordIndex, 11].Value = info.StatusThisMonthID;
                workSheet.Cells[recordIndex, 12].Value = info.PaidBy;
                workSheet.Cells[recordIndex, 13].Value = info.PaidTime;
                workSheet.Cells[recordIndex, 14].Value = info.RemarksNo;
                workSheet.Cells[recordIndex, 15].Value = info.ReceiptNo;


                workSheet.Cells[recordIndex, 11].Style.Font.Color.SetColor(Color.White);
                workSheet.Cells[recordIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                if ((info.StatusThisMonthID == 3))
                {
                    workSheet.Cells[recordIndex, 11].Value = "Active";
                    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                }
                else if (info.StatusThisMonthID == 4)
                {
                    workSheet.Cells[recordIndex, 11].Value = "InActive";
                    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                }
                else if (info.StatusThisMonthID == 5)
                {
                    workSheet.Cells[recordIndex, 11].Value = "Lock";
                    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                }
                else
                {
                    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                }

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForDueBills(int? Year, int? Month, int? ZoneID)
        {
            //string FileName = "";
            //if (Year != null)
            //{
            //    Year = int.Parse(db.Year.Find(Year.Value).YearName);
            //}

            //List<Transaction> lstTransaction = new List<Transaction>();

            //object objects = new object();
            //if (Year != null && Month != null && ZoneID != null)
            //{
            //    FileName = "DueBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
            //    objects = db.Transaction
            //        .Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value && s.ClientDetails.ZoneID == ZoneID.Value && s.PaymentStatus == AppUtils.PaymentIsNotPaid)
            //    .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                             (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                             .SelectMany(
            //                                variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                                (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                             )

            //  .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //  .Select(s => new
            //  {
            //      Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //      LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //      Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //      Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //      Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //      Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //      BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //      MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //      DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //  }).ToList();
            //    //lstTransaction = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
            //}
            //else if (Year != null && Month != null)
            //{
            //    FileName = "DueBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value;
            //    //lstTransaction = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value).ToList();
            //    objects = db.Transaction
            //   .Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value && s.PaymentStatus == AppUtils.PaymentIsNotPaid)
            //   .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                            .SelectMany(
            //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                               (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                            )
            //   .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //  .Select(s => new
            //  {
            //      Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //      LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //      Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //      Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //      Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //      Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //      BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //      MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //      DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //  }).ToList();

            //}
            //else if (Year != null && ZoneID != null)
            //{
            //    FileName = "DueBillsList_Year:_" + Year.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
            //    //lstTransaction = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
            //    db.Transaction
            //   .Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.ClientDetails.ZoneID == ZoneID.Value && s.PaymentStatus == AppUtils.PaymentIsNotPaid)
            //   .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                            .SelectMany(
            //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                               (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                            )
            //   .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //  .Select(s => new
            //  {
            //      Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //      LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //      Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //      Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //      Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //      Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //      BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //      MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //      DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //  }).ToList();

            //}
            //else if (ZoneID != null)
            //{
            //    FileName = "DueBillsList_:ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
            //    //lstTransaction = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.ClientDetails.ZoneID == ZoneID.Value).ToList();
            //    objects = db.Transaction
            //    .Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.ClientDetails.ZoneID == ZoneID.Value && s.PaymentStatus == AppUtils.PaymentIsNotPaid)
            //   .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                            .SelectMany(
            //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                               (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                            )
            //   .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //  .Select(s => new
            //  {
            //      Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //      LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //      Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //      Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //      Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //      Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //      BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //      MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //      DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //  }).ToList();

            //}
            //else
            //{
            //    DateTime sDate = AppUtils.ThisMonthStartDate();
            //    DateTime eDate = AppUtils.ThisMonthLastDate();
            //    FileName = "DueBillsList_Year:_" + sDate.Year + "_Month:_" + sDate.Month + "_AllZone";

            //    objects = db.Transaction
            //       .Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == sDate.Year && s.PaymentMonth == sDate.Month && s.PaymentStatus == AppUtils.PaymentIsNotPaid)
            //   .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                            .SelectMany(
            //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                               (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                            )
            //  .GroupBy(s => s.Transaction.PaymentStatus, (key, g) => g.OrderBy(s => s.Transaction.PaymentStatus))
            //  .Select(s => new
            //  {
            //      Name = s.FirstOrDefault().Transaction.ClientDetails.Name,
            //      LoginName = s.FirstOrDefault().Transaction.ClientDetails.LoginName,
            //      Contact = s.FirstOrDefault().Transaction.ClientDetails.ContactNumber,
            //      Zone = s.FirstOrDefault().Transaction.ClientDetails.Zone.ZoneName,
            //      Address = s.FirstOrDefault().Transaction.ClientDetails.Address,
            //      Package = s.FirstOrDefault().Transaction.ClientDetails.Package.PackageName,
            //      BandWith = s.FirstOrDefault().Transaction.ClientDetails.Package.BandWith,
            //      MonthlyFee = s.FirstOrDefault().Transaction.ClientDetails.Package.PackagePrice,
            //      DueAmount = s.FirstOrDefault().ClientDueBills == null ? 0 : s.Sum(y => ((double?)y.ClientDueBills.DueAmount) ?? 0)
            //  }).ToList(); ;

            //}


            int ifSearch = 0;
            int totalRecords = 0;
            int recFilter = 0;
            string FileName = "";
            if (Year != null)
            {
                Year = int.Parse(db.Year.Find(Year.Value).YearName);
            }

            //  List<VM_Transaction_ClientDueBills> VM_Transaction_ClientDueBills = new List<VM_Transaction_ClientDueBills>();
            List<Transaction> lstTransaction = new List<Transaction>();
            //var VM_Transaction_ClientDueBills = db.Transaction
            //    .GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
            //                             (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
            //                             .SelectMany(
            //                                variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                                (valTra, valClientDueBill) => new VM_Transaction_ClientDueBills { Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                             );
            //dynamic d = new ExpandoObject(); 
            object d = new object();
            dynamic dd = new ExpandoObject();
            IEnumerable<dynamic> topAgents = new List<dynamic>();
            if (Year != null && Month != null && ZoneID != null)
            {
                FileName = "DueBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                // topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else if (Year != null && Month != null)
            {
                FileName = "DueBillsList_Year:_" + Year.Value + "_Month:_" + Month.Value;
                // topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.PaymentMonth == Month.Value).AsQueryable();
            }
            else if (Year != null && ZoneID != null)
            {
                FileName = "ArchiveBillsList_Year:_" + Year.Value + "_ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == Year.Value && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else if (ZoneID != null)
            {
                FileName = "DueBillsList_:ZoneName:_" + db.Zone.Find(ZoneID).ZoneName;
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.ClientDetails.ZoneID == ZoneID.Value).AsQueryable();
            }
            else
            {
                DateTime sDate = AppUtils.ThisMonthStartDate();
                DateTime eDate = AppUtils.ThisMonthLastDate();
                FileName = "DueBillsList_Year:_" + sDate.Year + "_Month:_" + sDate.Month + "_AllZone";
                //  topAgents = db.Transaction.Where(s => s.PaymentTypeID == AppUtils.PaymentTypeIsMonthly && s.PaymentYear == sDate.Year && s.PaymentMonth == sDate.Month).AsQueryable();
            }

            List<VM_Transaction_EmpTraLockUnlock_ClientDueBills> lstDueBillsInformation = new List<VM_Transaction_EmpTraLockUnlock_ClientDueBills>();

            var firstPartOfQuery =
                    (Year != null && Month != null && ZoneID != null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentMonth.ToString() == Month.Value.ToString() && s.ClientDetails.ZoneID.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                        : (Year != null && Month != null && ZoneID == null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentMonth.ToString() == Month.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                            : (Year != null && Month == null && ZoneID != null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.ClientDetails.ZoneID.Value.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                : (Year != null && Month == null && ZoneID == null) ? db.Transaction.Where(s => s.PaymentYear.ToString() == Year.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                    : (Year == null && Month == null && ZoneID != null) ? db.Transaction.Where(s => s.ClientDetails.ZoneID.Value.ToString() == ZoneID.Value.ToString() && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                                        : db.Transaction.Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsMonthly && s.PaymentStatus == AppUtils.PaymentIsNotPaid).AsQueryable()
                ;
            var secondPartOfQuery = firstPartOfQuery
                .GroupJoin(db.ClientDueBills, Transaction => Transaction.ClientDetailsID,
                    ClientDueBills => ClientDueBills.ClientDetailsID, (Transaction, ClientDueBills) => new
                    {
                        Transaction = Transaction,
                        ClientDueBills = ClientDueBills
                    })
                .GroupJoin(db.EmployeeTransactionLockUnlock, Transaction => Transaction.Transaction.TransactionID, EmployeeTransactionLockUnlock => EmployeeTransactionLockUnlock.TransactionID,
                    (Transaction, EmployeeTransactionLockUnlock) => new
                    {
                        Transaction = Transaction.Transaction,
                        ClientDueBills = Transaction.ClientDueBills,
                        EmployeeTransactionLockUnlock = EmployeeTransactionLockUnlock
                    })
                //.GroupJoin(db.ClientLineStatus, Transaction => Transaction.Transaction.ClientDetailsID, ClientLineStatus => ClientLineStatus.ClientDetailsID, (Transaction, ClientLineStatus) => new
                //{
                //      Transaction = Transaction.Transaction,

                //})
                .AsEnumerable();

            if (secondPartOfQuery.Count() > 0)
            {
                totalRecords = secondPartOfQuery.Count();
                lstDueBillsInformation = secondPartOfQuery.AsEnumerable().Select(
                    s => new VM_Transaction_EmpTraLockUnlock_ClientDueBills()
                    {
                        ClientName = s.Transaction.ClientDetails.Name,
                        Address = s.Transaction.ClientDetails.Address,
                        ContactNumber = s.Transaction.ClientDetails.ContactNumber,
                        ZoneName = s.Transaction.ClientDetails.Zone.ZoneName,
                        PackageID = s.Transaction.PackageID.Value,
                        PackageName = s.Transaction.Package.PackageName,
                        MonthlyFee = Math.Round(s.Transaction.Package.PackagePrice, 2),
                        FeeForThisMonth = Math.Round(s.Transaction.PaymentAmount.Value, 2),
                        PaidAmount = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? Math.Round(s.Transaction.PaymentAmount.Value, 2) : 0,
                        Due = s.ClientDueBills.Any() ? s.ClientDueBills.Sum(ss => ss.DueAmount) : 0,
                        PaidBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : "",
                        CollectBy = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? db.Employee.Find(s.Transaction.BillCollectBy).Name : "",
                        PaidTime = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.PaymentDate.ToString() : "",
                        RemarksNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.RemarksNo.ToString() : "",
                        ReceiptNo = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.ResetNo.ToString() : "",
                        StatusThisMonthID = s.Transaction.LineStatusID.Value,
                        Employeename = s.Transaction.PaymentStatus == AppUtils.PaymentIsPaid ? s.Transaction.Employee.Name : ""
                    }).ToList();

            }


            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForArchiveBillsOrDueBills(ref workbook, lstDueBillsInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }


        private void LoadWorkBookForExpense(ref ExcelPackage workbook, dynamic myExpenseList)
        {

            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";
            workSheet.Cells[1, 2].Value = "Subject";
            workSheet.Cells[1, 3].Value = "Details";
            workSheet.Cells[1, 4].Value = "Paid_To";
            workSheet.Cells[1, 5].Value = "Amount";
            workSheet.Cells[1, 6].Value = "Paid_By";
            workSheet.Cells[1, 7].Value = "Payment_Date";


            int recordIndex = 2;
            foreach (var info in myExpenseList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.Subject;
                workSheet.Cells[recordIndex, 3].Value = info.Details;
                workSheet.Cells[recordIndex, 4].Value = info.Paid_To;
                workSheet.Cells[recordIndex, 5].Value = info.Amount;
                workSheet.Cells[recordIndex, 6].Value = info.Paid_By;
                workSheet.Cells[recordIndex, 7].Value = info.Payment_Date;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForMikrotikPackage(string MikrotikID)
        {
            string FileName = "";
            int MikrotikIDConvert = 0;
            IEnumerable<Package> lstPackages;

            if (!string.IsNullOrEmpty(MikrotikID))
            {
                MikrotikIDConvert = int.Parse(MikrotikID);
                FileName = "PackageList_MikrotikName: " + db.Mikrotik.Find(MikrotikID).MikName;
                lstPackages = db.Package.Where(s => s.MikrotikID == MikrotikIDConvert).ToList();
            }
            else
            {


                //MikrotikIDConvert = int.Parse(MikrotikID);
                FileName = "Mikrotik_PackageList: ";
                lstPackages = db.Package.AsEnumerable();
            }
            var lstMikrotikPackage = lstPackages.Select(s => new
            {
                PackageName = s.PackageName,
                BandWith = s.BandWith,
                PackagePrice = s.PackagePrice,
                Client = db.ClientDetails.Where(ss => ss.PackageID == s.PackageID).Count(),
                IPPoolName = s.IpPool != null ? s.IpPool.PoolName : "",
                LocalAddress = s.LocalAddress,
                MikrotikName = s.Mikrotik != null ? s.Mikrotik.MikName : "",
            }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForMikrotikPackageList(ref workbook, lstMikrotikPackage);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForMikrotikPackageList(ref ExcelPackage workbook, dynamic MikrotikPackageList)
        {

            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Package Name";
            workSheet.Cells[1, 3].Value = "Band With";
            workSheet.Cells[1, 4].Value = "Package Price";
            workSheet.Cells[1, 5].Value = "Client";
            workSheet.Cells[1, 6].Value = "IPPool Name";
            workSheet.Cells[1, 7].Value = "Local Address";
            workSheet.Cells[1, 8].Value = "Mikrotik Name";


            int recordIndex = 2;
            foreach (var info in MikrotikPackageList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.PackageName;
                workSheet.Cells[recordIndex, 3].Value = info.BandWith;
                workSheet.Cells[recordIndex, 4].Value = info.PackagePrice;
                workSheet.Cells[recordIndex, 5].Value = info.Client;
                workSheet.Cells[recordIndex, 6].Value = info.IPPoolName;
                workSheet.Cells[recordIndex, 7].Value = info.LocalAddress;
                workSheet.Cells[recordIndex, 8].Value = info.MikrotikName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForMikrotikUser(string MikrotikID)
        {
            string FileName = "";
            int MikrotikIDConvert = 0;
            IEnumerable<ClientDetails> lstClientDetails;

            if (!string.IsNullOrEmpty(MikrotikID))
            {
                MikrotikIDConvert = int.Parse(MikrotikID);
                FileName = "UserList_Mikrotik: " + db.Mikrotik.Find(MikrotikIDConvert).MikName;
                lstClientDetails = db.ClientDetails.Where(s => s.MikrotikID == MikrotikIDConvert).AsEnumerable();
            }
            else
            {
                //MikrotikIDConvert = int.Parse(MikrotikID);
                FileName = "Mikrotik_TotalUserList: ";
                lstClientDetails = db.ClientDetails.Where(s => s.MikrotikID != null).AsEnumerable();
            }
            var lstMikrotikUserCutomModel = lstClientDetails.Select(
                s => new MikrotikUserCutomModel()
                {
                    ClientName = s.Name,
                    LoginName = s.LoginName,
                    PackageName = s.Package != null ? s.Package.PackageName : "",
                    Zone = s.Zone != null ? s.Zone.ZoneName : "",
                    ContactNumber = s.ContactNumber,
                    MikrotikName = s.Mikrotik != null ? s.Mikrotik.MikName : "",
                    PoolName = s.Package != null && s.Package.IpPool != null ? s.Package.IpPool.PoolName : "",
                    LocalAddress = s.Package != null ? s.Package.LocalAddress : "",

                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForMikrotikUserList(ref workbook, lstMikrotikUserCutomModel);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForMikrotikUserList(ref ExcelPackage workbook, dynamic MikrotikPackageList)
        {

            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "ClientName";
            workSheet.Cells[1, 3].Value = "LoginName";
            workSheet.Cells[1, 4].Value = "PackageName";
            workSheet.Cells[1, 5].Value = "Zone";
            workSheet.Cells[1, 6].Value = "ContactNumber";
            workSheet.Cells[1, 7].Value = "MikrotikName";
            workSheet.Cells[1, 8].Value = "PoolName";
            workSheet.Cells[1, 9].Value = "LocalAddress";


            int recordIndex = 2;
            foreach (var info in MikrotikPackageList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.ClientName;
                workSheet.Cells[recordIndex, 3].Value = info.LoginName;
                workSheet.Cells[recordIndex, 4].Value = info.PackageName;
                workSheet.Cells[recordIndex, 5].Value = info.Zone;
                workSheet.Cells[recordIndex, 6].Value = info.ContactNumber;
                workSheet.Cells[recordIndex, 7].Value = info.MikrotikName;
                workSheet.Cells[recordIndex, 8].Value = info.PoolName;
                workSheet.Cells[recordIndex, 9].Value = info.LocalAddress;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForProductByStatus(string StockID, int ProductStatus)
        {
            //s => new CustomStockListSectionInformation()
            //{
            //    StockDetailsID = s.StockDetailsID,
            //    SectionID = s.SectionID,
            //    ProductStatusID = s.ProductStatusID,
            //    ItemName = s.Stock.Item.ItemName,
            //    BrandName = s.Brand.BrandName,
            //    Serial = s.Serial,
            //    SectionName = s.Section.SectionName,
            //    ProductStatusName = s.ProductStatus.ProductStatusName,
            //    ChangeSectionPermission = (AppUtils.lstAccessList.Contains(AppUtils.Change_Product_Status) && s.SectionID != AppUtils.WorkingSection) ? true : false,

            //}).ToList();

            string FileName = "";
            IEnumerable<StockDetails> lstStock;
            string ProductStatusName = "";
            int convertItemID = 0;
            //int convertStockID = int.Parse(StockID);




            if (!string.IsNullOrEmpty(StockID))
            {
                convertItemID = int.Parse(StockID);

                if (ProductStatus == AppUtils.ProductStatusIsAvialable)
                {
                    ProductStatusName = "Product_Avialable_List";
                    lstStock = db.StockDetails.Where(s => s.StockID == convertItemID && s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsWarrenty)
                {
                    ProductStatusName = "Product_Warrenty_List";
                    lstStock = db.StockDetails.Where(s => s.StockID == convertItemID && s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsRepair)
                {
                    ProductStatusName = "Product_Repairing_List";
                    lstStock = db.StockDetails.Where(s => s.StockID == convertItemID && s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsDead)
                {
                    ProductStatusName = "Product_Dead_List";
                    lstStock = db.StockDetails.Where(s => s.StockID == convertItemID && s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable();
                }
                else//mean all
                {
                    ProductStatusName = "Product_Total_List";
                    lstStock = lstStock = db.StockDetails.Where(s => s.StockID == convertItemID).AsEnumerable();
                }

                FileName = "Item_Name:" + db.Stock.Find(convertItemID).Item.ItemName + "_Status:" + ProductStatusName;

            }
            else
            {
                //MikrotikIDConvert = int.Parse(MikrotikID);
                FileName = "Mikrotik_TotalUserList: ";

                if (ProductStatus == AppUtils.ProductStatusIsAvialable)
                {
                    ProductStatusName = "Product_Avialable_List";
                    lstStock = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsAvialable && s.SectionID == AppUtils.StockSection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsWarrenty)
                {
                    ProductStatusName = "Product_Warrenty_List";
                    lstStock = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsWarrenty && s.SectionID == AppUtils.WarrantySection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsRepair)
                {
                    ProductStatusName = "Product_Repairing_List";
                    lstStock = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsRepair && s.SectionID == AppUtils.RepairingSection).AsEnumerable();
                }
                else if (ProductStatus == AppUtils.ProductStatusIsDead)
                {
                    ProductStatusName = "Product_Dead_List";
                    lstStock = db.StockDetails.Where(s => s.ProductStatusID == AppUtils.ProductStatusIsDead && s.SectionID == AppUtils.DeadSection).AsEnumerable();
                }
                else//mean all
                {
                    ProductStatusName = "Product_Total_List";
                    lstStock = lstStock = db.StockDetails.AsEnumerable();
                }
            }
            var lstCustomStockListSectionInformation = lstStock.Select(
                s => new CustomStockListSectionInformation()
                {
                    ItemName = s.Stock.Item.ItemName,
                    BrandName = s.Brand.BrandName,
                    Serial = s.Serial,
                    SectionName = s.Section.SectionName,
                    ProductStatusName = s.ProductStatus.ProductStatusName

                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForProductByStatusList(ref workbook, lstCustomStockListSectionInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForProductByStatusList(ref ExcelPackage workbook, dynamic lstCustomStockListSectionInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Item Name";
            workSheet.Cells[1, 3].Value = "Brand Name";
            workSheet.Cells[1, 4].Value = "Serial No";
            workSheet.Cells[1, 5].Value = "Section Name";
            workSheet.Cells[1, 6].Value = "Product Status";


            int recordIndex = 2;
            foreach (var info in lstCustomStockListSectionInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.ItemName;
                workSheet.Cells[recordIndex, 3].Value = info.BrandName;
                workSheet.Cells[recordIndex, 4].Value = info.Serial;
                workSheet.Cells[recordIndex, 5].Value = info.SectionName;
                workSheet.Cells[recordIndex, 6].Value = info.ProductStatusName;

                recordIndex++;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForItemOverView(string StockID)
        {

            string FileName = "";
            IEnumerable<Stock> lstStockOverview = new List<Stock>();
            string ProductStatusName = "";
            int convertItemID = 0;
            //int convertStockID = int.Parse(StockID);


            if (!string.IsNullOrEmpty(StockID))
            {
                convertItemID = int.Parse(StockID);
                lstStockOverview = db.Stock.Where(s => s.StockID == convertItemID).AsEnumerable();
                FileName = "Item_Name:" + db.Stock.Find(convertItemID).Item.ItemName;

            }
            else
            {
                FileName = "Total Item Summary:  ";
                lstStockOverview = lstStockOverview = db.Stock.AsEnumerable();
            }
            var lstItemOverView = lstStockOverview.Select(
                s => new CustomStockOverview()
                {
                    ItemName = s.Item.ItemName,
                    TotalItemCount = db.StockDetails.Where(ss => ss.StockID == s.StockID).Count()
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForItemOverView(ref workbook, lstItemOverView);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }


        private void LoadWorkBookForItemOverView(ref ExcelPackage workbook, dynamic lstItemOverView)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Item Name";
            workSheet.Cells[1, 3].Value = "Total";


            int recordIndex = 2;
            foreach (var info in lstItemOverView)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.ItemName;
                workSheet.Cells[recordIndex, 3].Value = info.TotalItemCount;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForProductForWorkingStatus(string StockID)
        {
            string FileName = "";
            IEnumerable<Distribution> lstDistributions;
            string ProductStatusName = "";
            int convertItemID = 0;

            if (!string.IsNullOrEmpty(StockID))
            {
                convertItemID = int.Parse(StockID);
                FileName = "Product_WorkingList: " + "Item_Name:" + db.Stock.Find(convertItemID).Item.ItemName;
                lstDistributions = db.Distribution.Where(s => s.StockDetails.StockID == convertItemID && s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).AsEnumerable();
            }
            else
            {
                FileName = "Product_WorkingList: ";
                lstDistributions = db.Distribution.Where(s => s.IndicatorStatus == AppUtils.IndicatorStatusIsActive).AsEnumerable();

            }
            var lstCustomWorkingListSectionInformation = lstDistributions.Select(
                s => new CustomStockListSectionInformation()
                {
                    ItemName = s.StockDetails.Stock.Item.ItemName,
                    BrandName = s.StockDetails.Brand.BrandName,
                    Serial = s.StockDetails.Serial,
                    ClientName = s.ClientDetailsID == null ? "" : s.ClientDetails.Name,
                    EmployeeName = s.EmployeeID == null ? "" : s.Employee.Name,
                    PopName = s.Pop != null ? s.Pop.PopName : "",
                    BoxName = s.Box != null ? s.Box.BoxName : "",
                    SectionName = s.StockDetails.Section.SectionName,
                    ProductStatusName = s.StockDetails.ProductStatus.ProductStatusName
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForProductForWorkingList(ref workbook, lstCustomWorkingListSectionInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForProductForWorkingList(ref ExcelPackage workbook, dynamic lstCustomStockListSectionInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Item Name";
            workSheet.Cells[1, 3].Value = "Brand Name";
            workSheet.Cells[1, 4].Value = "Serial No";
            workSheet.Cells[1, 5].Value = "Client Name";
            workSheet.Cells[1, 6].Value = "Employee Name";
            workSheet.Cells[1, 7].Value = "Pop Name";
            workSheet.Cells[1, 8].Value = "Box Name";
            workSheet.Cells[1, 9].Value = "Section";
            workSheet.Cells[1, 10].Value = "Product Status";



            int recordIndex = 2;
            foreach (var info in lstCustomStockListSectionInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.ItemName;
                workSheet.Cells[recordIndex, 3].Value = info.BrandName;
                workSheet.Cells[recordIndex, 4].Value = info.Serial;
                workSheet.Cells[recordIndex, 5].Value = info.ClientName;
                workSheet.Cells[recordIndex, 6].Value = info.EmployeeName;
                workSheet.Cells[recordIndex, 7].Value = info.PopName;
                workSheet.Cells[recordIndex, 8].Value = info.BoxName;
                workSheet.Cells[recordIndex, 9].Value = info.SectionName;
                workSheet.Cells[recordIndex, 10].Value = info.ProductStatusName;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForCableDetailsHistory(string CableTypeID, string CableStockID, string ClientDetailsID)
        {
            string FileName = "";

            List<CustomCableUsedInformation> lstCustomSCustomCableUsedInformation = new List<CustomCableUsedInformation>();
            int CableTypeIDDDL = 0;
            int CableStockIDDDL = 0;
            int ClientDetailsDDL = 0;
            FileName = "List_CableHistory";
            if (!string.IsNullOrEmpty(CableTypeID))
            {
                CableTypeIDDDL = int.Parse(CableTypeID);
                FileName += "_CableType:" + db.CableType.Find(CableTypeIDDDL).CableTypeName;
            }
            if (!string.IsNullOrEmpty(CableStockID))
            {
                CableStockIDDDL = int.Parse(CableStockID);
                FileName += "_CableBox:" + db.CableStock.Find(CableTypeIDDDL).CableBoxName;
            }
            if (!string.IsNullOrEmpty(ClientDetailsID))
            {
                ClientDetailsDDL = int.Parse(ClientDetailsID);
                FileName += "_ClientName:" + db.ClientDetails.Find(ClientDetailsDDL).Name;
            }


            var firstPartOfQuery =
                    (CableTypeID != "" && CableStockID != "" && ClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL && s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
                        : (CableTypeID != "" && CableStockID != "" && ClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.CableStockID == CableStockIDDDL).AsEnumerable()
                            : (CableTypeID != "" && CableStockID == "" && ClientDetailsID != "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL && s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
                                : (CableTypeID != "" && CableStockID == "" && ClientDetailsID == "") ? db.CableDistribution.Where(s => s.CableStock.CableTypeID == CableTypeIDDDL).AsEnumerable()
                                    : (CableTypeID == "" && CableStockID == "" && ClientDetailsID != "") ? db.CableDistribution.Where(s => s.ClientDetailsID == ClientDetailsDDL).AsEnumerable()
                                        :
                                        db.CableDistribution.AsEnumerable()
                ;
            var secondPartOfQuery = firstPartOfQuery
                .GroupJoin(db.Transaction,
                    CableDistribution => CableDistribution.ClientDetailsID,
                    Transaction => Transaction.ClientDetailsID, (CableDistribution, Transaction) => new
                    {
                        CableDistribution = CableDistribution,
                        Transaction = Transaction
                    }).AsEnumerable();

            if (secondPartOfQuery.Any())
            {
                // var i = thirdPartOfquery.ToList();
                lstCustomSCustomCableUsedInformation = secondPartOfQuery.AsEnumerable().Select(
                    s => new CustomCableUsedInformation()
                    {
                        CableTypeName = s.CableDistribution.CableStock.CableType.CableTypeName,
                        CableBoxName = s.CableDistribution.CableStock.CableBoxName,
                        AmountOfCableUsed = s.CableDistribution.AmountOfCableUsed.ToString(),
                        Date = s.CableDistribution.CreatedDate.Value,
                        ClientName = s.CableDistribution.ClientDetailsID != null ? s.CableDistribution.ClientDetails.Name : "",
                        AssignEmployeeName = s.CableDistribution.CableForEmployeeID != null ? db.Employee.Find(s.CableDistribution.CableForEmployeeID).Name : "",
                        EmployeeTakenCable = s.CableDistribution.EmployeeID != null ? s.CableDistribution.Employee.Name : "",
                        cableStatus = ChangeCableStatus(s.CableDistribution.CableIndicatorStatus),

                    }).ToList();

            }

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForCableHistory(ref workbook, lstCustomSCustomCableUsedInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private string ChangeCableStatus(int cableIndicatorStatus)
        {
            if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsRunning)
            {
                return "Running";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsOldBox)
            {
                return "Old Box";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsStolen)
            {
                return "Stolen";
            }
            else if (cableIndicatorStatus == AppUtils.CableIndicatorStatusIsNotWorking)
            {
                return "Not Working";
            }
            else
            {
                return "";
            }
        }
        private void LoadWorkBookForCableHistory(ref ExcelPackage workbook, dynamic lstCustomStockListSectionInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Cable Type";
            workSheet.Cells[1, 3].Value = "Box Name";
            workSheet.Cells[1, 4].Value = "AmountOfCableUsed";
            workSheet.Cells[1, 5].Value = "Date";
            workSheet.Cells[1, 6].Value = "Client Name";
            workSheet.Cells[1, 7].Value = "Assign Employee";
            workSheet.Cells[1, 8].Value = "Employee Taken Cable";
            workSheet.Cells[1, 9].Value = "Cable Status";



            int recordIndex = 2;
            foreach (var info in lstCustomStockListSectionInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.CableTypeName;
                workSheet.Cells[recordIndex, 3].Value = info.CableBoxName;
                workSheet.Cells[recordIndex, 4].Value = info.AmountOfCableUsed;
                workSheet.Cells[recordIndex, 5].Value = info.Date.ToString();
                workSheet.Cells[recordIndex, 6].Value = info.ClientName;
                workSheet.Cells[recordIndex, 7].Value = info.AssignEmployeeName;
                workSheet.Cells[recordIndex, 8].Value = info.EmployeeTakenCable;
                workSheet.Cells[recordIndex, 9].Value = info.cableStatus;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForItemList()
        {
            string FileName = "";
            IEnumerable<Item> lstItem;

            FileName = "Total_Item_List ";
            lstItem = db.Item.AsEnumerable();

            var lstItemListInformation = lstItem.Select(
                s => new
                {
                    ItemName = s.ItemName
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForItemList(ref workbook, lstItemListInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForItemList(ref ExcelPackage workbook, dynamic lstItemInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Item Name";

            int recordIndex = 2;
            foreach (var info in lstItemInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.ItemName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForAdvancePayment()
        {
            string FileName = "";
            IEnumerable<AdvancePayment> lstItem;

            FileName = "Total_AdvancePayment_List ";
            lstItem = db.AdvancePayment.AsEnumerable();

            var lstAdvancePaymentListInformation = lstItem.Select(
                s => new
                {
                    Name = s.ClientDetils.Name,
                    Credit = s.AdvanceAmount,
                    Remarks = s.Remarks,
                    AdedBy = (string.IsNullOrEmpty(s.UpdatePaymentBy)) ? s.CreatePaymentBy : s.UpdatePaymentBy,
                    Time = (string.IsNullOrEmpty(s.UpdatePaymentDate.ToString())) ? s.FirstPaymentDate.ToString() : s.UpdatePaymentDate.ToString()
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAdvancePayment(ref workbook, lstAdvancePaymentListInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForAdvancePayment(ref ExcelPackage workbook, dynamic lstItemInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Name";
            workSheet.Cells[1, 3].Value = "Credit";
            workSheet.Cells[1, 4].Value = "Remarks";
            workSheet.Cells[1, 5].Value = "Added/Update By";
            workSheet.Cells[1, 6].Value = "Time";


            int recordIndex = 2;
            foreach (var info in lstItemInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.Name;
                workSheet.Cells[recordIndex, 3].Value = info.Credit;
                workSheet.Cells[recordIndex, 4].Value = info.Remarks;
                workSheet.Cells[recordIndex, 5].Value = info.AdedBy;
                workSheet.Cells[recordIndex, 6].Value = info.Time;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForMikrotikList()
        {
            string FileName = "";
            IEnumerable<Mikrotik> lstItem;

            FileName = "Mikrotik_List ";
            lstItem = db.Mikrotik.AsEnumerable();

            var lstMikrotikList = lstItem.Select(
                s => new
                {
                    MikrotikName = s.MikName,
                    RealIP = s.RealIP,
                    MikrotikUserName = s.MikUserName,
                    Password = "", //s.MikPassword,
                    APIPort = s.APIPort,
                    WebPort = s.WebPort,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForMikrotikList(ref workbook, lstMikrotikList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForMikrotikList(ref ExcelPackage workbook, dynamic lstMikrotikInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Mikrotik Name";
            workSheet.Cells[1, 3].Value = "Real IP";
            workSheet.Cells[1, 4].Value = "Mikrotik User Name";
            workSheet.Cells[1, 5].Value = "Password";
            workSheet.Cells[1, 6].Value = "API Port";
            workSheet.Cells[1, 7].Value = "Web Port";


            int recordIndex = 2;
            foreach (var info in lstMikrotikInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.MikrotikName;
                workSheet.Cells[recordIndex, 3].Value = info.RealIP;
                workSheet.Cells[recordIndex, 4].Value = info.MikrotikUserName;
                workSheet.Cells[recordIndex, 5].Value = info.Password;
                workSheet.Cells[recordIndex, 6].Value = info.APIPort;
                workSheet.Cells[recordIndex, 7].Value = info.WebPort;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForZoneList()
        {
            string FileName = "";
            IEnumerable<Zone> lstZone;

            FileName = "Zone List ";
            lstZone = db.Zone.AsEnumerable();

            var lstMikrotikList = lstZone.Select(
                s => new
                {
                    ZoneName = s.ZoneName,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForZoneList(ref workbook, lstMikrotikList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForZoneList(ref ExcelPackage workbook, dynamic lstZoneInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Zone Name";


            int recordIndex = 2;
            foreach (var info in lstZoneInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.ZoneName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForComplainTypeList()
        {
            string FileName = "";
            IEnumerable<ComplainType> lstComplainType;

            FileName = "Complain Type List ";
            lstComplainType = db.ComplainType.AsEnumerable();

            var lstMikrotikList = lstComplainType.Select(
                s => new
                {
                    ComplainTypeName = s.ComplainTypeName,
                    ShowMessageBox = s.ShowMessageBox ? "Yes" : "No" 
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForComplainTypeList(ref workbook, lstMikrotikList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForComplainTypeList(ref ExcelPackage workbook, dynamic lstComplainTypeInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Complain Type Name";
            workSheet.Cells[1, 3].Value = "Show Message Box";


            int recordIndex = 2;
            foreach (var info in lstComplainTypeInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.ComplainTypeName;
                workSheet.Cells[recordIndex, 3].Value = info.ShowMessageBox;

                recordIndex++;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForAssetTypeList()
        {
            string FileName = "";
            IEnumerable<AssetType> lstAssetTypeType;

            FileName = "Asset Type List ";
            lstAssetTypeType = db.AssetType.AsEnumerable();

            var lstAssetTypeList = lstAssetTypeType.Select(
                s => new
                {
                    AssetTypeName = s.AssetTypeName
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForAssetTypeList(ref workbook, lstAssetTypeList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForAssetTypeList(ref ExcelPackage workbook, dynamic lstAssetTypeInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Asset Type Name";


            int recordIndex = 2;
            foreach (var info in lstAssetTypeInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.AssetTypeName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForDistributionReasonList()
        {
            string FileName = "";
            IEnumerable<DistributionReason> lstDistributionReasons;

            FileName = "Distribution_Reason_List ";
            lstDistributionReasons = db.DistributionReason.AsEnumerable();

            var lstDistributionReasonList = lstDistributionReasons.Select(
                s => new
                {
                    DistributionReasonName = s.DistributionReasonName,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForDistributionReasonList(ref workbook, lstDistributionReasonList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForDistributionReasonList(ref ExcelPackage workbook, dynamic lstDistrubutionReasonInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Distribution Reason Name";


            int recordIndex = 2;
            foreach (var info in lstDistrubutionReasonInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.DistributionReasonName;

                recordIndex++;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForBrandList()
        {
            string FileName = "";
            IEnumerable<Brand> lstBrand;

            FileName = "Brand List ";
            lstBrand = db.Brand.AsEnumerable();

            var lstZoneList = lstBrand.Select(
                s => new
                {
                    BrandName = s.BrandName,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForBrandList(ref workbook, lstZoneList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForBrandList(ref ExcelPackage workbook, dynamic lstBrandInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Brand Name";


            int recordIndex = 2;
            foreach (var info in lstBrandInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.BrandName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForSectionList()
        {
            string FileName = "";
            IEnumerable<Section> lstSection;

            FileName = "Section List ";
            lstSection = db.Section.AsEnumerable();

            var lstSectionList = lstSection.Select(
                s => new
                {
                    SectionName = s.SectionName,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForSectionList(ref workbook, lstSectionList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForSectionList(ref ExcelPackage workbook, dynamic lstSectionInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Section Name";


            int recordIndex = 2;
            foreach (var info in lstSectionInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.SectionName;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForProductStatusList()
        {
            string FileName = "";
            IEnumerable<ProductStatus> lstProductStatus;

            FileName = "Product_Status_List ";
            lstProductStatus = db.ProductStatus.AsEnumerable();

            var lstProductStatusList = lstProductStatus.Select(
                s => new
                {
                    ProductStatusName = s.ProductStatusName,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForProductStatusList(ref workbook, lstProductStatusList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForProductStatusList(ref ExcelPackage workbook, dynamic lstProductStatusInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Product Status Name";


            int recordIndex = 2;
            foreach (var info in lstProductStatusInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.ProductStatusName;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForPopList()
        {
            string FileName = "";
            IEnumerable<Pop> lstPop;

            FileName = "Pop_List ";
            lstPop = db.Pop.AsEnumerable();

            var lstPopList = lstPop.Select(
                s => new
                {
                    PopName = s.PopName,
                    PopLocation = s.PopLocation
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForPopList(ref workbook, lstPopList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForPopList(ref ExcelPackage workbook, dynamic lstPopInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Pop Name";
            workSheet.Cells[1, 3].Value = "Pop Location";


            int recordIndex = 2;
            foreach (var info in lstPopInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.PopName;
                workSheet.Cells[recordIndex, 3].Value = info.PopLocation;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForBoxList()
        {
            string FileName = "";
            IEnumerable<Box> lstBox;

            FileName = "Box_List ";
            lstBox = db.Box.AsEnumerable();

            var lstBoxList = lstBox.Select(
                s => new
                {
                    BoxName = s.BoxName,
                    BoxLocation = s.BoxLocation
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForBoxList(ref workbook, lstBoxList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForBoxList(ref ExcelPackage workbook, dynamic lstBoxInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Box Name";
            workSheet.Cells[1, 3].Value = "Box Location";


            int recordIndex = 2;
            foreach (var info in lstBoxInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.BoxName;
                workSheet.Cells[recordIndex, 3].Value = info.BoxLocation;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForSupplierList()
        {
            string FileName = "";
            IEnumerable<Supplier> lstSupplier;

            FileName = "Supplier_List ";
            lstSupplier = db.Supplier.AsEnumerable();

            var lstSupplierList = lstSupplier.Select(
                s => new
                {
                    SupplierName = s.SupplierName,
                    SupplierAddress = s.SupplierAddress,
                }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForSupplierList(ref workbook, lstSupplierList);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForSupplierList(ref ExcelPackage workbook, dynamic lstSupplierInformation)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Supplier Name";
            workSheet.Cells[1, 3].Value = "Supplier Address";


            int recordIndex = 2;
            foreach (var info in lstSupplierInformation)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.SupplierName;
                workSheet.Cells[recordIndex, 3].Value = info.SupplierAddress;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForPackage()
        {
            string FileName = "";
            int MikrotikIDConvert = 0;
            IEnumerable<Package> lstPackages;




            //MikrotikIDConvert = int.Parse(MikrotikID);
            FileName = "Package_List: ";
            lstPackages = db.Package.AsEnumerable();

            var lstMikrotikPackage = lstPackages.Select(s => new
            {
                PackageName = s.PackageName,
                BandWith = s.BandWith,
                PackagePrice = s.PackagePrice,
                Client = db.ClientDetails.Where(ss => ss.PackageID == s.PackageID).Count(),
            }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForPackageList(ref workbook, lstMikrotikPackage);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private void LoadWorkBookForPackageList(ref ExcelPackage workbook, dynamic PackageList)
        {

            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Package Name";
            workSheet.Cells[1, 3].Value = "Band With";
            workSheet.Cells[1, 4].Value = "Package Price";
            workSheet.Cells[1, 5].Value = "Client";


            int recordIndex = 2;
            foreach (var info in PackageList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = info.PackageName;
                workSheet.Cells[recordIndex, 3].Value = info.BandWith;
                workSheet.Cells[recordIndex, 4].Value = info.PackagePrice;
                workSheet.Cells[recordIndex, 5].Value = info.Client;

                recordIndex++;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForEmployee()
        {
            string FileName = "";
            int MikrotikIDConvert = 0;
            IEnumerable<Employee> lstEmployees;




            //MikrotikIDConvert = int.Parse(MikrotikID);
            FileName = "Employee_List: ";
            lstEmployees = db.Employee.AsEnumerable();

            var lstMikrotikPackage = lstEmployees.Where(s => (s.EmployeeID != AppUtils.EmployeeIDISKamrul && s.EmployeeID != AppUtils.EmployeeIDISTalent)).Select(s => new
            {
                EmployeeName = s.Name,
                LoginName = s.LoginName,
                Phone = s.Phone,
                Address = s.Address,
                Email = s.Email,
                Department = s.Department != null ? s.Department.DepartmentName : "",
                RightPermission = s.UserRightPermission != null ? s.UserRightPermission.UserRightPermissionName : "",
                Status = GetEmployeeStatus(s.EmployeeStatus)
            }).ToList();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForEmployeeList(ref workbook, lstMikrotikPackage);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }
        private string GetEmployeeStatus(int status)
        {
            if (status == ISP_ManagementSystemModel.AppUtils.EmployeeStatusIsActive)
            {
                return "Active";
            }
            else if (status == ISP_ManagementSystemModel.AppUtils.EmployeeStatusIsLock)
            {
                return "Lock";
            }
            else
            {
                return "";
            }
        }
        private void LoadWorkBookForEmployeeList(ref ExcelPackage workbook, dynamic EmployeeList)
        {

            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "Serial";

            workSheet.Cells[1, 2].Value = "Employee Name";
            workSheet.Cells[1, 3].Value = "Login Name";
            workSheet.Cells[1, 4].Value = "Phone";
            workSheet.Cells[1, 5].Value = "Address";
            workSheet.Cells[1, 6].Value = "Email";
            workSheet.Cells[1, 7].Value = "Department";
            workSheet.Cells[1, 8].Value = "Right Permission";
            workSheet.Cells[1, 9].Value = "Status";


            int recordIndex = 2;
            foreach (var info in EmployeeList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.EmployeeName;
                workSheet.Cells[recordIndex, 3].Value = info.LoginName;
                workSheet.Cells[recordIndex, 4].Value = info.Phone;
                workSheet.Cells[recordIndex, 5].Value = info.Address;
                workSheet.Cells[recordIndex, 6].Value = info.Email;
                workSheet.Cells[recordIndex, 7].Value = info.Department;
                workSheet.Cells[recordIndex, 8].Value = info.RightPermission;
                workSheet.Cells[recordIndex, 9].Value = info.Status;

                recordIndex++;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForSignUpClient(int? ZoneID)
        {
            int zoneFromDDL = 0;
            // Initialization.   

            if (ZoneID != null)
            {
                zoneFromDDL = int.Parse(ZoneID.Value.ToString());
            }
            var newCLientList = (ZoneID == null) ? db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient).AsEnumerable() : db.ClientDetails.Where(s => s.IsNewClient == AppUtils.isNewClient && s.ZoneID == zoneFromDDL).AsEnumerable();

            int ifSearch = 0;
            List<NewClientCustomInformation> data =
                newCLientList.Any() ? newCLientList.AsEnumerable()
                    .Select(
                        s => new NewClientCustomInformation
                        {
                            Name = s.Name,
                            Zone = s.Zone.ZoneName,
                            Address = s.Address,
                            ContactNumber = s.ContactNumber,
                            Package = s.Package.PackageName,
                            AssignedTo = s.Employee.Name,
                            Survey = s.ClientSurvey,
                            time = s.UpdateDate != null ? s.UpdateDate.Value : s.CreateDate.Value,
                            //CreateBy = db.Employee.Where(ss => ss.EmployeeID == Convert.ToInt32(s.CreateBy)).FirstOrDefault().Name,
                            CreateBy = s.CreateBy,
                            UpdateBy = s.UpdateBy != null ? s.UpdateBy : "",

                        })
                    .ToList() : new List<NewClientCustomInformation>();

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForSignUpClient(ref workbook, data);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = (ZoneID == null) ? "AllRequestClient_AllZone_" + AppUtils.GetDateTimeNow() + ".xlsx" : "AllRequestClient_" + db.Zone.Find(ZoneID).ZoneName + "_" + AppUtils.GetDateTimeNow() + ".xlsx" }
            };
        }

        private void LoadWorkBookForSignUpClient(ref ExcelPackage workbook, dynamic lstClientLineStatus)
        {
            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "SL_No";

            workSheet.Cells[1, 2].Value = "Name";
            workSheet.Cells[1, 3].Value = "Zone";
            workSheet.Cells[1, 4].Value = "Address";
            workSheet.Cells[1, 5].Value = "ContactNumber";
            workSheet.Cells[1, 6].Value = "Package";
            workSheet.Cells[1, 7].Value = "AssignedTo";
            workSheet.Cells[1, 8].Value = "Survey";
            workSheet.Cells[1, 9].Value = "time";
            workSheet.Cells[1, 10].Value = "CreateBy";
            workSheet.Cells[1, 11].Value = "UpdateBy";


            int recordIndex = 2;
            foreach (var info in lstClientLineStatus)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.Name;
                workSheet.Cells[recordIndex, 3].Value = info.Zone;
                workSheet.Cells[recordIndex, 4].Value = info.Address;
                workSheet.Cells[recordIndex, 5].Value = info.ContactNumber;
                workSheet.Cells[recordIndex, 6].Value = info.Package;
                workSheet.Cells[recordIndex, 7].Value = info.AssignedTo;
                workSheet.Cells[recordIndex, 8].Value = info.Survey;
                workSheet.Cells[recordIndex, 9].Value = info.time;
                workSheet.Cells[recordIndex, 10].Value = info.CreateBy;
                workSheet.Cells[recordIndex, 11].Value = info.UpdateBy;

                recordIndex++;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReportForSignUpBills(int? Year, int? Month, int? ZoneID)
        {

            int ifSearch = 0;
            int totalRecords = 0;
            int recFilter = 0;
            string zoneFromDDL = "";
            string convertYear = "";
            string convertMonth = "";

            string FileName = "";
            if (Year != null)
            {
                int year = int.Parse(Year.Value.ToString());
                convertYear = db.Year.Where(s => s.YearID == year).FirstOrDefault().YearName;
            }

            if (ZoneID != null)
            {
                zoneFromDDL = ZoneID.Value.ToString();
            }

            if (Month != null)
            {
                convertMonth = Month.Value.ToString();
            }



            DateTime startDate = AppUtils.ThisMonthStartDate();
            DateTime endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(AppUtils.ThisMonthLastDate());
            IEnumerable<Transaction> transactionEnumerable = Enumerable.Empty<Transaction>();
            setStartDateEndDateTransaction(ref FileName, convertYear, convertMonth, zoneFromDDL, ref startDate, ref endDate, ref transactionEnumerable);


            var secondPartOfQuery =
                transactionEnumerable
                    .GroupJoin(
                        db.Transaction.Where(ss => ss.PaymentYear == startDate.Year && ss.PaymentMonth == startDate.Month),
                        TCon => TCon.ClientDetailsID, TMon => TMon.ClientDetailsID,
                        (TCon, TMon) => new { TCon = TCon, TMon = TMon.FirstOrDefault() })
                    .AsEnumerable();

            // Verification.   
            List<CustomSignUpBills> lstSIgnUpBillsInformation = new List<CustomSignUpBills>();
            if (secondPartOfQuery.Count() > 0)
            {
                totalRecords = secondPartOfQuery.Count();
                lstSIgnUpBillsInformation = secondPartOfQuery.AsEnumerable()
                    .Select(
                        s => new CustomSignUpBills
                        {
                            TransactionID = s.TCon.TransactionID,
                            ClientDetailsID = s.TCon.ClientDetailsID,
                            Name = s.TCon.ClientDetails.Name,
                            Address = s.TCon.ClientDetails.Address,
                            ContactNumber = s.TCon.ClientDetails.ContactNumber,
                            ZoneName = s.TCon.ClientDetails.Zone.ZoneName,
                            PackageName = s.TMon.Package.PackageName,
                            PackagePrice = s.TMon.Package.PackagePrice.ToString(),
                            FeeForThisMonth = s.TMon.PaymentAmount.ToString(),
                            SignUpFee = s.TCon.PaymentAmount.ToString(),
                            PaymentDate = s.TCon.PaymentDate.Value,
                            RemarksNo = s.TCon.PaymentStatus == AppUtils.PaymentIsPaid ? s.TCon.RemarksNo : "",
                            ReceiptNo = s.TCon.PaymentStatus == AppUtils.PaymentIsPaid ? s.TCon.ResetNo : "",

                        }).ToList();

            }

            ExcelPackage workbook = new ExcelPackage();
            LoadWorkBookForSignUpBills(ref workbook, lstSIgnUpBillsInformation);

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {

                Data = new { FileGuid = handle, FileName = FileName + ".xlsx" }
            };
        }

        private void setStartDateEndDateTransaction(ref string FileName, string YearID, string MonthID, string ZoneID, ref DateTime startDate, ref DateTime endDate, ref IEnumerable<Transaction> transactionEnumerable)
        {
            if (YearID != "" && MonthID != "" && ZoneID != "")
            {
                int zone = int.Parse(ZoneID);
                FileName = "SignUpBills_Year:" + YearID + "_Month:" + MonthID + "_Zone:" + db.Zone.Find(zone).ZoneName;
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));
                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
            }
            else if (YearID != "" && MonthID != "" && ZoneID == "")
            {
                FileName = "SignUpBills_Year:" + YearID + "_Month:" + MonthID;
                startDate = new DateTime(int.Parse(YearID), int.Parse(MonthID), 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), int.Parse(MonthID), DateTime.DaysInMonth(int.Parse(YearID), int.Parse(MonthID))));

                DateTime sd = startDate;
                DateTime ed = endDate; transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
            else if (YearID != "" && MonthID == "" && ZoneID != "")
            {
                int zone = int.Parse(ZoneID);
                FileName = "SignUpBills_Year:" + YearID + "_Zone:" + db.Zone.Find(zone).ZoneName;
                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 1, DateTime.DaysInMonth(AppUtils.RunningYear, 1)));

                DateTime sd = startDate;
                DateTime ed = endDate; transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection && s.ClientDetails.ZoneID.ToString() == ZoneID).AsQueryable();
            }
            else if (YearID != "" && MonthID == "" && ZoneID == "")
            {
                FileName = "SignUpBills_Year:" + YearID;
                startDate = new DateTime(int.Parse(YearID), 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(int.Parse(YearID), 1, DateTime.DaysInMonth(AppUtils.RunningYear, 1)));

                DateTime sd = startDate;
                DateTime ed = endDate; transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
            else if (YearID == "" && MonthID == "" && ZoneID != "")
            {

                int zone = int.Parse(ZoneID);
                FileName = "SignUpBills_Zone:" + db.Zone.Find(zone).ZoneName;
                startDate = new DateTime(AppUtils.RunningYear, 1, 1);
                endDate = AppUtils.GetLastDayWithHrMinSecMsByMyDate(new DateTime(AppUtils.RunningYear, 1, DateTime.DaysInMonth(AppUtils.RunningYear, 1)));

                DateTime sd = startDate;
                DateTime ed = endDate; transactionEnumerable = db.Transaction.Where(s => s.ClientDetails.ZoneID.ToString() == ZoneID && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
            else
            {

                FileName = "SignUpBills";
                DateTime sd = startDate;
                DateTime ed = endDate;
                transactionEnumerable = db.Transaction.Where(s => s.PaymentDate >= sd && s.PaymentDate <= ed && s.PaymentStatus == AppUtils.PaymentIsPaid && s.PaymentTypeID == ISP_ManagementSystemModel.AppUtils.PaymentTypeIsConnection).AsQueryable();
            }
        }


        private void LoadWorkBookForSignUpBills(ref ExcelPackage workbook, dynamic myExpenseList)
        {


            var workSheet = workbook.Workbook.Worksheets.Add("Sheet1");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 1].Value = "SL_No";

            workSheet.Cells[1, 2].Value = "Client Name";
            workSheet.Cells[1, 3].Value = "Address";
            workSheet.Cells[1, 4].Value = "Contact Number";
            workSheet.Cells[1, 5].Value = "Zone";
            workSheet.Cells[1, 6].Value = "Package";
            workSheet.Cells[1, 7].Value = "Monthly Fee";
            workSheet.Cells[1, 8].Value = "Fee For This Month";
            workSheet.Cells[1, 9].Value = "Sign Up Fee";
            workSheet.Cells[1, 10].Value = "Payment Date";
            workSheet.Cells[1, 11].Value = "Remarks No";
            workSheet.Cells[1, 12].Value = "Receipt No";


            int recordIndex = 2;
            foreach (var info in myExpenseList)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();

                workSheet.Cells[recordIndex, 2].Value = info.Name;
                workSheet.Cells[recordIndex, 3].Value = info.Address;
                workSheet.Cells[recordIndex, 4].Value = info.ContactNumber;
                workSheet.Cells[recordIndex, 5].Value = info.ZoneName;
                workSheet.Cells[recordIndex, 6].Value = info.PackageName;
                workSheet.Cells[recordIndex, 7].Value = info.PackagePrice;
                workSheet.Cells[recordIndex, 8].Value = info.FeeForThisMonth;
                workSheet.Cells[recordIndex, 9].Value = info.SignUpFee;
                workSheet.Cells[recordIndex, 10].Value = "" + info.PaymentDate;
                workSheet.Cells[recordIndex, 11].Value = info.RemarksNo;
                workSheet.Cells[recordIndex, 12].Value = info.ReceiptNo;


                //workSheet.Cells[recordIndex, 11].Style.Font.Color.SetColor(Color.White);
                //workSheet.Cells[recordIndex, 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //if ((info.StatusThisMonthID == 3))
                //{
                //    workSheet.Cells[recordIndex, 11].Value = "Active";
                //    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                //}
                //else if (info.StatusThisMonthID == 4)
                //{
                //    workSheet.Cells[recordIndex, 11].Value = "InActive";
                //    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                //}
                //else if (info.StatusThisMonthID == 5)
                //{
                //    workSheet.Cells[recordIndex, 11].Value = "Lock";
                //    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                //}
                //else
                //{
                //    workSheet.Cells[recordIndex, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                //}

                recordIndex++;
            }
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        private byte[] ConvertDataSetToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter brFormatter = new BinaryFormatter();
                dataTable.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

    }
}