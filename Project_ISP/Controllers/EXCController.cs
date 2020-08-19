using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using tik4net;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel;
using System.Configuration;
using System.Data.Entity;

namespace Project_ISP.Controllers
{
    public class EXCController : Controller
    {
        private ISPContext db = new ISPContext();
        public class MyViewModel
        {
            [Required]
            public HttpPostedFileBase MyExcelFile { get; set; }

            public string MSExcelTable { get; set; }
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            //{
            //    connection.Open("103.121.105.34", "admin", "sbnk");
            //    //ITikCommand cmd = connection.CreateCommand("/system/identity/print");
            //    //Console.WriteLine(cmd.ExecuteScalar());
            //    ITikReSentence secret = connection.CreateCommandAndParameters("/ppp/secret/print", "name", "babu").ExecuteSingleRow();
            //    var profile = secret.Words["profile"];
            //    var password = secret.Words["password"];
            //}


            var model = new MyViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(MyViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            {
                
                var area = model.MyExcelFile.FileName.ToString().Split('_').ToList();
                List<string> onlyAreaCOde = area[0].Split('.').ToList();
                DataTable dt = GetDataTableFromSpreadsheet(model.MyExcelFile, false, 0/*int.Parse(onlyAreaCOde[0])*/);
                
                string strContent = "<p>Thanks for uploading the file</p>" + ConvertDataTableToHTMLTable(dt);
                model.MSExcelTable = strContent;
            }

            return View(model);
        }

        List<string> lstDeleteRow = new List<string>();
        List<string> nonMatchingPackage = new List<string>();
        List<string> dueIsLessthenPackage = new List<string>();
        List<string> lstAdvancePayment = new List<string>();
        List<string> hasSmallAmountCanNotSetAnywhere = new List<string>();
        //List<string> lstPackage = new List<string> { "BASNETVIP", "BRONZE", "DIAMOND", "ECONOMY", "GOLD", "GT-", "PROFILE-5MB", "VVIP", "VIP", "SUITE", "STANDARD", "SILVER" };
        

        public DataTable GetDataTableFromSpreadsheet(HttpPostedFileBase MyExcelStream, bool ReadOnly, int zoneID)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream.InputStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                    }

                    dt.Rows.Add(tempRow);
                }
            }
             dt.Rows.RemoveAt(0);
            //var Name = AppUtil.GetIdValue("Name");
            //var Email = AppUtil.GetIdValue("Email");
            //var LoginName = AppUtil.GetIdValue("LoginName");
            //var Password = AppUtil.GetIdValue("Password");
            //var Address = AppUtil.GetIdValue("Address");
            //var ContactNumber = AppUtil.GetIdValue("ContactNumber");
            //var ZoneID = AppUtil.GetIdValue("ZoneID");
            //var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
            //var Occupation = AppUtil.GetIdValue("Occupation");
            //var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
            //var Remarks = AppUtil.GetIdValue("Remarks");
            //var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
            //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
            //var PopDetails = AppUtil.GetIdValue("PopDetails");
            //var RequireCable = AppUtil.GetIdValue("RequireCable");
            //var CableTypeID = AppUtil.GetIdValue("CableTypeID");
            //var Reference = AppUtil.GetIdValue("Reference");
            //var NationalID = AppUtil.GetIdValue("NationalID");
            //var PackageID = AppUtil.GetIdValue("PackageID");
            //var SingUpFee = AppUtil.GetIdValue("SingUpFee");                              //////
            //var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
            //var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
            //var MacAddress = AppUtil.GetIdValue("MacAddress");
            ////var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
            ////var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
            ////var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

            //var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
            //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
            //var ConnectionDate = $("#ConnectionDate").val();

            int Serial = 1;
            bool error = false;

            List<string> lstSuccessList = new List<string>();
            List<string> lstErrorList = new List<string>();
            List<Box> lstBox = db.Box.ToList();
            List<Zone> lstZone = db.Zone.ToList();
            List<Package> lstPackage = db.Package.ToList();

            List<ClientDetails> lstClientDetails = new List<ClientDetails>();
            List<ClientLineStatus> lstClientLineStatus = new List<ClientLineStatus>();
            List<Transaction> lstTransaction = new List<Transaction>();
            List<PaymentHistory> lstPaymentHistory = new List<PaymentHistory>();

            int clientSerial = 1;
            int clientLineStatusSerial = 1;
            int clientTransactionSerial = 1;
            int clientPaymentHistorySerial = 1;

            foreach (DataRow row in dt.Rows)
            {
                var Name = row["Name"].ToString();
                var Mobile = row["Contact Number"].ToString();
                var SMSCommunication = row["SMS Communication"].ToString();
                var Email = row["Email"].ToString();
                var IPUsername = row["Login Name"].ToString().Trim();
                var ContactPerson = row["Reference"].ToString();
                var Address = row["Address"].ToString();
                var profile = row["Package"].ToString();
                var PopDetails = row["Pop Details"].ToString();
                var Remarks = row["Remarks"].ToString();
                var NationalID = row["National ID"].ToString();
                var ConnectionDate = row["Connection Date"].ToString();
                var Occupation = row["Occupation"].ToString();
                var BoxNumber = row["Box Number"].ToString();
                var Zone = row["Zone"].ToString();   ;
                int MonthlyRent = 0;//int.Parse(row["Monthly Rent"].ToString());
                int PreviousDue = 0;//int.Parse(row["Previous Due"].ToString());
                int TotalDue = 0;// int.Parse(row["Total Due"].ToString());
                //int Advance = int.Parse(row["Advance"].ToString());

                var BillPaymentDate = row["Bill Payment Date"].ToString() == " " ? "8" : row["Bill Payment Date"].ToString();
                if (/*BillPaymentDate != "8" ||*/ BillPaymentDate.Length > 2)
                {
                    List<string> billDateParse = row["Bill Payment Date"].ToString().Split(new string[] { "" }, StringSplitOptions.None).ToList();
                    if (billDateParse.Count() > 1)
                    {
                        BillPaymentDate = billDateParse[0];
                    }
                    else
                    {
                        BillPaymentDate = "8";
                    }
                }
                else {

                    if (int.Parse(BillPaymentDate) > 28)
                    {
                        BillPaymentDate = "8";
                    }
                }
                // if (row["Bill Payment Date"].ToString().Split("").ToList().Count() > 0) {
                //    BillPaymentDate = ;
                //}

                //ITikReSentence secret = null;
                try
                {
                    //secret = connection.CreateCommandAndParameters("/ppp/secret/print", "name", IPUsername).ExecuteSingleRow();

                    //var profile = secret.Words["profile"];
                    //var password = secret.Words["password"]; 
                    //if (!lstPackage.Contains(profile) && profile != "Payment-Due")
                    //{
                    //    nonMatchingPackage.Add(row["Customer ID"].ToString());

                    //} 
                    //else
                    //{
                    try
                    {

                        ClientDetails ClientDetails = new ClientDetails();

                        ClientDetails.ClientDetailsID = clientSerial;

                        ClientDetails.Name = Name;
                        ClientDetails.Email = Email;
                        ClientDetails.LoginName = IPUsername;
                        ClientDetails.Password = "123";
                        ClientDetails.Address = Address;
                        ClientDetails.ContactNumber = Mobile;
                        //ClientDetails.ZoneID = zoneID;
                        ClientDetails.ZoneID = GetZone(ref lstZone, Zone);
                        ClientDetails.SMSCommunication = SMSCommunication;
                        ClientDetails.Occupation = Occupation;
                        ClientDetails.Remarks = Remarks;
                        ClientDetails.ConnectionTypeID = 1;
                        ClientDetails.BoxNumber = GetBoxNumber(ref lstBox, BoxNumber);
                        ClientDetails.PopDetails = PopDetails;
                        ClientDetails.Reference = ContactPerson;
                        ClientDetails.NationalID = NationalID;
                        ClientDetails.PackageID = GetPackageName(lstPackage, profile, ref MonthlyRent);
                        ClientDetails.SecurityQuestionID = 1;
                        ClientDetails.SecurityQuestionAnswer = Name;
                        ClientDetails.ApproxPaymentDate = int.Parse(BillPaymentDate.ToString());
                        ClientDetails.ClientSurvey = "Survey By Speedtech";
                        ClientDetails.ConnectionDate = string.IsNullOrEmpty(ConnectionDate.Trim()) ? AppUtils.dateTimeNow : Convert.ToDateTime(ConnectionDate);
                        //ClientDetails.MikrotikID = 1;
                        ClientDetails.RoleID = AppUtils.ClientRole;
                        ClientDetails.StatusThisMonth = 3;
                        ClientDetails.StatusNextMonth = 3;
                        ClientDetails.PackageThisMonth = ClientDetails.PackageID.Value;
                        ClientDetails.PackageNextMonth = ClientDetails.PackageID.Value;
                        ClientDetails.NextApproxPaymentFullDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, ClientDetails.ApproxPaymentDate) ;
                        ClientDetails.LineStatusWillActiveInThisDate = ClientDetails.ConnectionDate.Value.AddMonths(1);
                        ClientDetails.ProfileUpdatePercentage = GetProfileUpdatePercentage(ClientDetails);
                        //var ClientDetailsAfterSave = db.ClientDetails.Add(ClientDetails);
                        //db.SaveChanges();
                        var ClientDetailsAfterSave = ClientDetails;
                        lstClientDetails.Add(ClientDetails); 
                        clientSerial++; 

                        ClientLineStatus ClientLineStatus = new ClientLineStatus();
                        ClientLineStatus.ClientLineStatusID = clientLineStatusSerial;
                        ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
                        ClientLineStatus.ClientDetailsID = ClientDetailsAfterSave.ClientDetailsID;
                        ClientLineStatus.PackageID = ClientDetailsAfterSave.PackageID;
                        ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
                        ClientLineStatus.EmployeeID = 6;
                        ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
                        ClientLineStatus.StatusChangeReason = "New Connection";

                        DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

                        ClientLineStatus.LineStatusWillActiveInThisDate = AppUtils.dateTimeNow;
                        //var ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
                        //db.SaveChanges();
                        var ClientLineStatusSave = ClientLineStatus;
                        lstClientLineStatus.Add(ClientLineStatus); 
                        clientLineStatusSerial++;


                        if (ClientLineStatusSave.ClientDetailsID > 0)
                        {
                            Transaction Transaction = new Transaction();
                            Transaction.TransactionID = clientTransactionSerial;
                            Transaction.PaymentAmount = 0;
                            Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
                            Transaction.IsNewClient = AppUtils.isNewClient;
                            Transaction.EmployeeID = 7;
                            Transaction.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
                            Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
                            Transaction.WhoGenerateTheBill = 7;
                            Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
                            Transaction.PackageID = ClientDetails.PackageID;
                            Transaction.BillCollectBy = 7;
                            Transaction.PaymentFromWhichPage = "Create";
                            Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
                            //var TransactonSave = db.Transaction.Add(Transaction);
                            //db.SaveChanges();
                            var TransactonSave = Transaction;
                            lstTransaction.Add(TransactonSave);
                            clientTransactionSerial++;


                            if (Transaction.TransactionID > 0)
                            {
                                PaymentHistory paymentHistory = new PaymentHistory();
                                paymentHistory.PaymentHistoryID = clientPaymentHistorySerial;
                                paymentHistory.TransactionID = Transaction.TransactionID;
                                paymentHistory.ClientDetailsID = Transaction.ClientDetailsID;
                                paymentHistory.CollectByID = 7;
                                paymentHistory.PaidAmount = Transaction.PaymentAmount.Value;
                                paymentHistory.PaymentByID = 6;
                                paymentHistory.PaymentDate = ClientDetails.ConnectionDate.Value;
                                paymentHistory.Status = AppUtils.TableStatusIsActive;
                                paymentHistory.ResetNo = "rst" + ClientDetails.ClientDetailsID;
                                //db.PaymentHistory.Add(paymentHistory);
                                //db.SaveChanges(); 
                                lstPaymentHistory.Add(paymentHistory);
                                clientPaymentHistorySerial++;
                            }

                            //////if (IPUsername == "ruman05@basnet")
                            //////{

                            //////}

                            //////Package runningPackage = db.Package.Find(Transaction.PackageID);
                            //////if (PreviousDue != 0 && PreviousDue >= runningPackage.PackagePrice)
                            //////{

                            //////    bool hasMisMatch = AddDueBillsAndOldTransaction(ClientLineStatusSave, runningPackage, PreviousDue);
                            //////    if (hasMisMatch)
                            //////    {
                            //////        hasSmallAmountCanNotSetAnywhere.Add(row["Customer ID"].ToString());
                            //////    }
                            //////}
                            //////else
                            //////{
                            //////    if (PreviousDue != 0)
                            //////    {
                            //////        dueIsLessthenPackage.Add(row["Customer ID"].ToString());
                            //////    }
                            //////}

                            if (Transaction.TransactionID > 0)
                            {
                                DateTime currenDateTime = AppUtils.GetDateTimeNow();

                                Transaction forMonthlyBill = new Transaction();
                                forMonthlyBill.TransactionID = clientTransactionSerial;
                                forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
                                forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;

                                //////if (TotalDue < 0)
                                //////{
                                //////    //////lstAdvancePayment.Add(Serial + ":" + row["Customer ID"].ToString());
                                //////    forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                                //////    forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                                //////    forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                                //////    forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                                //////    forMonthlyBill.BillCollectBy = 2;
                                //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                                //////}
                                //////else if (TotalDue == 0 && PreviousDue == 0)// meaning no due or monthly bill required
                                //////{
                                //////    forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
                                //////    forMonthlyBill.ResetNo = "RNEW" + SerialNo();
                                //////    forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
                                //////    forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
                                //////    forMonthlyBill.BillCollectBy = 2;
                                //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
                                //////}
                                //////else if (PreviousDue > 0 && PreviousDue != TotalDue)
                                //////{
                                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                                //////}
                                //////else
                                //////{
                                //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                                //////}

                                forMonthlyBill.IsNewClient = AppUtils.isNewClient;
                                forMonthlyBill.EmployeeID = 7;
                                forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
                                forMonthlyBill.WhoGenerateTheBill = 7;
                                forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
                                forMonthlyBill.PaymentYear = AppUtils.RunningYear;
                                forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
                                forMonthlyBill.PackageID = Transaction.PackageID;
                                forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
                                forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();
                                forMonthlyBill.PaymentAmount = MonthlyRent;
                                forMonthlyBill.PaidAmount = 0;
                                forMonthlyBill.DueAmount = MonthlyRent;
                                //db.Transaction.Add(forMonthlyBill);
                                //db.SaveChanges();

                                lstTransaction.Add(forMonthlyBill);
                                clientTransactionSerial++;
                            }
                        }

                        //////if (Advance > 0)
                        //////{
                        //////    AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientLineStatus.ClientDetailsID).FirstOrDefault();

                        //////    try
                        //////    {
                        //////        if (advancePayment != null)
                        //////        {
                        //////            advancePayment.UpdatePaymentBy = "Auni";
                        //////            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
                        //////            advancePayment.AdvanceAmount += Advance;
                        //////            advancePayment.Remarks = "Payment By Auni";
                        //////            db.Entry(advancePayment).State = EntityState.Modified;
                        //////            db.SaveChanges();
                        //////        }
                        //////        else
                        //////        {
                        //////            AdvancePayment insertAdvancePayment = new AdvancePayment();
                        //////            insertAdvancePayment.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
                        //////            insertAdvancePayment.AdvanceAmount = Advance;
                        //////            insertAdvancePayment.Remarks = "Payment By Auni";
                        //////            insertAdvancePayment.CreatePaymentBy = "Auni";
                        //////            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
                        //////            db.AdvancePayment.Add(insertAdvancePayment);
                        //////            db.SaveChanges();
                        //////        }

                        //////    }
                        //////    catch
                        //////    {
                        //////        lstErrorList.Add(Serial + ":" + row["Customer ID"].ToString());
                        //////        error = true;
                        //////    }
                        //////}



                    }
                    catch (Exception ex)
                    {
                        lstErrorList.Add(Serial + ":" + row["Customer ID"].ToString());
                        error = true;
                    }
                    //}
                }
                catch (Exception ex)
                {
                    lstErrorList.Add(Serial + ":" + row["No."].ToString());
                    lstDeleteRow.Add(Serial + ":" + row["Customer ID"].ToString());
                    error = true;
                }
                if (error != false)
                {
                    lstSuccessList.Add(Serial + ":" + row["Customer ID"].ToString());
                }
                Serial++;
            }


            db.ClientDetails.AddRange(lstClientDetails);
            db.SaveChanges();
            db.ClientLineStatus.AddRange(lstClientLineStatus);
            db.SaveChanges();
            db.Transaction.AddRange(lstTransaction);
            db.SaveChanges();
            db.PaymentHistory.AddRange(lstPaymentHistory);
            db.SaveChanges();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Status\" + MyExcelStream.FileName + ".txt"))
            {
                file.WriteLine("File Name: " + MyExcelStream.FileName);
                int TotalCount = lstErrorList.Distinct().Count() + lstSuccessList.Distinct().Count();
                int SuccessCount = lstSuccessList.Distinct().Count();
                int ErrorCount = lstErrorList.Distinct().Count();

                file.WriteLine("Total List: " + TotalCount + " Sucess : " + SuccessCount + " Error : " + ErrorCount);

                string errorList = "";
                foreach (string errList in lstErrorList)
                {
                    errorList += "/ " + errList;
                }
                file.WriteLine("error List: " + errorList);

                string SuccessList = "";
                foreach (string sucList in lstSuccessList)
                {
                    SuccessList += "/ " + sucList;
                }
                file.WriteLine("Success List: " + SuccessList);


                foreach (string line in lstDeleteRow)
                {
                    file.WriteLine("lstDeleteRow: " + line);
                }
                file.WriteLine("");
                foreach (string line in nonMatchingPackage)
                {

                    file.WriteLine("nonMatchingPackage " + line);
                }
                file.WriteLine("");
                foreach (string line in dueIsLessthenPackage)
                {

                    file.WriteLine("dueIsLessthenPackage: " + line);
                }
                file.WriteLine("");
                string advance = "";
                foreach (string adv in lstAdvancePayment)
                {
                    advance += "/ " + adv;
                }
                file.WriteLine("Payment From Advance: " + advance);

                file.WriteLine("");
                foreach (string line in hasSmallAmountCanNotSetAnywhere)
                {

                    file.WriteLine("hasSmallAmountCanNotSetAnywhereAfterAddingProperlyOtherDueBill: " + line);
                }
            }

            //var a  = dt
            return dt = new DataTable();


        }

        private int? GetZone(ref List<Zone> lstZone, string clientZone)
        {
            Zone zone = new Zone();
            zone = lstZone.Where(x => x.ZoneName.Trim().ToLower() == clientZone.Trim().ToLower()).FirstOrDefault();
            if (zone != null)
            {
                return zone.ZoneID;
            }
            else
            {
                zone = new Zone();
                zone.ZoneName = clientZone.Trim();
                zone.CreatedBy = "7";
                zone.CreatedDate = DateTime.Now;
                db.Zone.Add(zone);
                db.SaveChanges();
                lstZone.Add(zone);
                return zone.ZoneID;
            }
        }

        private string GetBoxNumber(ref List<Box> lstBox, string boxNumber)
        {
            Box box = new Box();
            box = lstBox.Where(x => x.BoxName.Trim().ToLower() == boxNumber.Trim().ToLower()).FirstOrDefault();
            if (box != null)
            {
                return box.BoxID.ToString();
            }
            else {
                box = new Box();
                box.BoxName = boxNumber.Trim();
                box.CreatedBy = "7";
                box.CreatedDate = DateTime.Now;
                db.Box.Add(box);
                db.SaveChanges();
                lstBox.Add(box);
                return box.BoxID.ToString();
            }
        }

        private double GetProfileUpdatePercentage(ClientDetails clientDetails)
        {
            double percent = 0;
            if (!string.IsNullOrEmpty(clientDetails.Address.Trim()))
            {
                percent += 20;
            }
            if (!string.IsNullOrEmpty(clientDetails.NationalID.Trim()))
            {
                percent += 20;
            }
            if (!string.IsNullOrEmpty(clientDetails.ContactNumber.Trim()))
            {
                percent += 20;
            }
            return percent;
        }

        //public DataTable GetDataTableFromSpreadsheet(HttpPostedFileBase MyExcelStream, bool ReadOnly, ITikConnection connection, int zoneID)
        //{
        //    DataTable dt = new DataTable();
        //    using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream.InputStream, ReadOnly))
        //    {
        //        WorkbookPart workbookPart = sDoc.WorkbookPart;
        //        IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        //        string relationshipId = sheets.First().Id.Value;
        //        WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
        //        Worksheet workSheet = worksheetPart.Worksheet;
        //        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        //        IEnumerable<Row> rows = sheetData.Descendants<Row>();

        //        foreach (Cell cell in rows.ElementAt(0))
        //        {
        //            dt.Columns.Add(GetCellValue(sDoc, cell));
        //        }

        //        foreach (Row row in rows) //this will also include your header row...
        //        {
        //            DataRow tempRow = dt.NewRow();

        //            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
        //            {
        //                tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
        //            }

        //            dt.Rows.Add(tempRow);
        //        }
        //    }
        //    dt.Rows.RemoveAt(0);
        //    //var Name = AppUtil.GetIdValue("Name");
        //    //var Email = AppUtil.GetIdValue("Email");
        //    //var LoginName = AppUtil.GetIdValue("LoginName");
        //    //var Password = AppUtil.GetIdValue("Password");
        //    //var Address = AppUtil.GetIdValue("Address");
        //    //var ContactNumber = AppUtil.GetIdValue("ContactNumber");
        //    //var ZoneID = AppUtil.GetIdValue("ZoneID");
        //    //var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
        //    //var Occupation = AppUtil.GetIdValue("Occupation");
        //    //var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        //    //var Remarks = AppUtil.GetIdValue("Remarks");
        //    //var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        //    //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        //    //var PopDetails = AppUtil.GetIdValue("PopDetails");
        //    //var RequireCable = AppUtil.GetIdValue("RequireCable");
        //    //var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        //    //var Reference = AppUtil.GetIdValue("Reference");
        //    //var NationalID = AppUtil.GetIdValue("NationalID");
        //    //var PackageID = AppUtil.GetIdValue("PackageID");
        //    //var SingUpFee = AppUtil.GetIdValue("SingUpFee");                              //////
        //    //var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        //    //var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        //    //var MacAddress = AppUtil.GetIdValue("MacAddress");
        //    ////var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //    ////var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //    ////var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        //    //var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
        //    //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //    //var ConnectionDate = $("#ConnectionDate").val();

        //    int Serial = 1;
        //    bool error = false;

        //    List<string> lstSuccessList = new List<string>();
        //    List<string> lstErrorList = new List<string>();

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        var Name = row["Name"].ToString();
        //        var Mobile = row["Mobile"].ToString();
        //        var IPUsername = row["IP / Username"].ToString().Trim();
        //        var ContactPerson = row["Contact Person"].ToString();
        //        List<string> AddressParse = row["Address"].ToString().Split(new string[] { ", 880" }, StringSplitOptions.None).ToList();
        //        int MonthlyRent = int.Parse(row["Monthly Rent"].ToString());
        //        int PreviousDue = int.Parse(row["Previous Due"].ToString());
        //        int TotalDue = int.Parse(row["Total Due"].ToString());
        //        int Advance = int.Parse(row["Advance"].ToString());

        //        ITikReSentence secret = null;
        //        try
        //        {
        //            secret = connection.CreateCommandAndParameters("/ppp/secret/print", "name", IPUsername).ExecuteSingleRow();

        //            var profile = secret.Words["profile"];
        //            var password = secret.Words["password"];
        //            //if (profile != "Payment-Due")
        //            //{
        //            if (!lstPackage.Contains(profile) && profile != "Payment-Due")
        //            {
        //                nonMatchingPackage.Add(row["Customer ID"].ToString());

        //            }

        //            //}
        //            else
        //            {
        //                try
        //                {

        //                    ClientDetails ClientDetails = new ClientDetails();
        //                    ClientDetails.Name = Name;
        //                    ClientDetails.Email = "Email";
        //                    ClientDetails.LoginName = IPUsername;
        //                    ClientDetails.Password = password;
        //                    ClientDetails.Address = AddressParse[0];
        //                    ClientDetails.ContactNumber = Mobile;
        //                    ClientDetails.ZoneID = zoneID;
        //                    ClientDetails.SMSCommunication = Mobile;
        //                    ClientDetails.Occupation = "Occupation";
        //                    ClientDetails.Remarks = "R";
        //                    ClientDetails.ConnectionTypeID = 1;
        //                    ClientDetails.BoxNumber = "B";
        //                    ClientDetails.PopDetails = "P";
        //                    ClientDetails.Reference = ContactPerson;
        //                    ClientDetails.NationalID = "0000000000000";
        //                    ClientDetails.PackageID = GetPackageName(profile, MonthlyRent);
        //                    ClientDetails.SecurityQuestionID = 1;
        //                    ClientDetails.SecurityQuestionAnswer = Name;
        //                    ClientDetails.ApproxPaymentDate = 8;
        //                    ClientDetails.ClientSurvey = "Survey By Auni";
        //                    ClientDetails.ConnectionDate = AppUtils.dateTimeNow;
        //                    ClientDetails.MikrotikID = 1;
        //                    ClientDetails.RoleID = AppUtils.ClientRole;

        //                    var ClientDetailsAfterSave = db.ClientDetails.Add(ClientDetails);
        //                    db.SaveChanges();


        //                    ClientLineStatus ClientLineStatus = new ClientLineStatus();
        //                    ////ClientLineStatus.LineStatusFromWhichMonth = AppUtils.StatusChangeFromThisMonth;
        //                    ClientLineStatus.ClientDetailsID = ClientDetailsAfterSave.ClientDetailsID;
        //                    ClientLineStatus.PackageID = ClientDetailsAfterSave.PackageID;
        //                    ClientLineStatus.LineStatusID = AppUtils.LineIsActive;
        //                    ClientLineStatus.EmployeeID = 2;
        //                    ClientLineStatus.LineStatusChangeDate = AppUtils.GetDateTimeNow(); ;
        //                    ClientLineStatus.StatusChangeReason = "New Connection";

        //                    DateTime dayone = new DateTime(AppUtils.dateTimeNow.AddMonths(1).Year, AppUtils.dateTimeNow.AddMonths(1).Month, 1);

        //                    ClientLineStatus.LineStatusWillActiveInThisDate = AppUtils.dateTimeNow;
        //                    var ClientLineStatusSave = db.ClientLineStatus.Add(ClientLineStatus);
        //                    db.SaveChanges();

        //                    if (ClientLineStatusSave.ClientDetailsID > 0)
        //                    {
        //                        Transaction Transaction = new Transaction();
        //                        Transaction.PaymentAmount = 0;
        //                        Transaction.PaymentStatus = AppUtils.PaymentIsPaid;
        //                        Transaction.IsNewClient = AppUtils.isNewClient;
        //                        Transaction.EmployeeID = 2;
        //                        Transaction.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
        //                        Transaction.PaymentFrom = AppUtils.PaymentByHandCash;
        //                        Transaction.WhoGenerateTheBill = 2;
        //                        Transaction.PaymentTypeID = AppUtils.PaymentTypeIsConnection;
        //                        Transaction.PackageID = ClientDetails.PackageID;
        //                        Transaction.BillCollectBy = 2;
        //                        Transaction.PaymentFromWhichPage = "Create";
        //                        Transaction.PaymentDate = AppUtils.GetDateTimeNow();//Payment Date will be sae from the system not the seected Date
        //                        var TransactonSave = db.Transaction.Add(Transaction);
        //                        db.SaveChanges();

        //                        //////if (IPUsername == "ruman05@basnet")
        //                        //////{

        //                        //////}

        //                        //////Package runningPackage = db.Package.Find(Transaction.PackageID);
        //                        //////if (PreviousDue != 0 && PreviousDue >= runningPackage.PackagePrice)
        //                        //////{

        //                        //////    bool hasMisMatch = AddDueBillsAndOldTransaction(ClientLineStatusSave, runningPackage, PreviousDue);
        //                        //////    if (hasMisMatch)
        //                        //////    {
        //                        //////        hasSmallAmountCanNotSetAnywhere.Add(row["Customer ID"].ToString());
        //                        //////    }
        //                        //////}
        //                        //////else
        //                        //////{
        //                        //////    if (PreviousDue != 0)
        //                        //////    {
        //                        //////        dueIsLessthenPackage.Add(row["Customer ID"].ToString());
        //                        //////    }
        //                        //////}

        //                        if (Transaction.TransactionID > 0)
        //                        {
        //                            DateTime currenDateTime = AppUtils.GetDateTimeNow();

        //                            Transaction forMonthlyBill = new Transaction();
        //                            forMonthlyBill.ForWhichSignUpBills = TransactonSave.TransactionID;
        //                            forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;

        //                            //////if (TotalDue < 0)
        //                            //////{
        //                            //////    //////lstAdvancePayment.Add(Serial + ":" + row["Customer ID"].ToString());
        //                            //////    forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
        //                            //////    forMonthlyBill.ResetNo = "RNEW" + SerialNo();
        //                            //////    forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
        //                            //////    forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
        //                            //////    forMonthlyBill.BillCollectBy = 2;
        //                            //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
        //                            //////}
        //                            //////else if (TotalDue == 0 && PreviousDue == 0)// meaning no due or monthly bill required
        //                            //////{
        //                            //////    forMonthlyBill.RemarksNo = "RNEW" + RemarksNo();
        //                            //////    forMonthlyBill.ResetNo = "RNEW" + SerialNo();
        //                            //////    forMonthlyBill.PaymentDate = AppUtils.GetDateTimeNow();
        //                            //////    forMonthlyBill.PaymentFrom = AppUtils.PaymentByHandCash;
        //                            //////    forMonthlyBill.BillCollectBy = 2;
        //                            //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsPaid;
        //                            //////}
        //                            //////else if (PreviousDue > 0 && PreviousDue != TotalDue)
        //                            //////{
        //                                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                            //////}
        //                            //////else
        //                            //////{
        //                            //////    forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
        //                            //////}

        //                            forMonthlyBill.IsNewClient = AppUtils.isNewClient;
        //                            forMonthlyBill.EmployeeID = 2;
        //                            forMonthlyBill.ClientDetailsID = Transaction.ClientDetailsID;
        //                            forMonthlyBill.WhoGenerateTheBill = 2;
        //                            forMonthlyBill.PaymentMonth = AppUtils.RunningMonth;
        //                            forMonthlyBill.PaymentYear = AppUtils.RunningYear;
        //                            forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
        //                            forMonthlyBill.PackageID = Transaction.PackageID;
        //                            forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
        //                            forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();
        //                            forMonthlyBill.PaymentAmount = MonthlyRent;
        //                            db.Transaction.Add(forMonthlyBill);
        //                            db.SaveChanges();
        //                        }
        //                    }

        //                    //////if (Advance > 0)
        //                    //////{
        //                    //////    AdvancePayment advancePayment = db.AdvancePayment.Where(s => s.ClientDetailsID == ClientLineStatus.ClientDetailsID).FirstOrDefault();

        //                    //////    try
        //                    //////    {
        //                    //////        if (advancePayment != null)
        //                    //////        {
        //                    //////            advancePayment.UpdatePaymentBy = "Auni";
        //                    //////            advancePayment.UpdatePaymentDate = AppUtils.GetDateTimeNow();
        //                    //////            advancePayment.AdvanceAmount += Advance;
        //                    //////            advancePayment.Remarks = "Payment By Auni";
        //                    //////            db.Entry(advancePayment).State = EntityState.Modified;
        //                    //////            db.SaveChanges();
        //                    //////        }
        //                    //////        else
        //                    //////        {
        //                    //////            AdvancePayment insertAdvancePayment = new AdvancePayment();
        //                    //////            insertAdvancePayment.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
        //                    //////            insertAdvancePayment.AdvanceAmount = Advance;
        //                    //////            insertAdvancePayment.Remarks = "Payment By Auni";
        //                    //////            insertAdvancePayment.CreatePaymentBy = "Auni";
        //                    //////            insertAdvancePayment.FirstPaymentDate = AppUtils.GetDateTimeNow();
        //                    //////            db.AdvancePayment.Add(insertAdvancePayment);
        //                    //////            db.SaveChanges();
        //                    //////        }

        //                    //////    }
        //                    //////    catch
        //                    //////    {
        //                    //////        lstErrorList.Add(Serial + ":" + row["Customer ID"].ToString());
        //                    //////        error = true;
        //                    //////    }
        //                    //////}

        //                }
        //                catch (Exception ex)
        //                {
        //                    lstErrorList.Add(Serial + ":" + row["Customer ID"].ToString());
        //                    error = true;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lstErrorList.Add(Serial + ":" + row["Customer ID"].ToString());
        //            lstDeleteRow.Add(Serial + ":" + row["Customer ID"].ToString());
        //            error = true;
        //        }
        //        if (error != false)
        //        {
        //            lstSuccessList.Add(Serial + ":" + row["Customer ID"].ToString());
        //        }
        //        Serial++;
        //    }

        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Status\" + MyExcelStream.FileName + ".txt"))
        //    {
        //        file.WriteLine("File Name: " + MyExcelStream.FileName);
        //        int TotalCount = lstErrorList.Distinct().Count() + lstSuccessList.Distinct().Count();
        //        int SuccessCount = lstSuccessList.Distinct().Count();
        //        int ErrorCount = lstErrorList.Distinct().Count();

        //        file.WriteLine("Total List: " + TotalCount + " Sucess : " + SuccessCount + " Error : " + ErrorCount);

        //        string errorList = "";
        //        foreach (string errList in lstErrorList)
        //        {
        //            errorList += "/ " + errList;
        //        }
        //        file.WriteLine("error List: " + errorList);

        //        string SuccessList = "";
        //        foreach (string sucList in lstSuccessList)
        //        {
        //            SuccessList += "/ " + sucList;
        //        }
        //        file.WriteLine("Success List: " + SuccessList);


        //        foreach (string line in lstDeleteRow)
        //        {
        //            file.WriteLine("lstDeleteRow: " + line);
        //        }
        //        file.WriteLine("");
        //        foreach (string line in nonMatchingPackage)
        //        {

        //            file.WriteLine("nonMatchingPackage " + line);
        //        }
        //        file.WriteLine("");
        //        foreach (string line in dueIsLessthenPackage)
        //        {

        //            file.WriteLine("dueIsLessthenPackage: " + line);
        //        }
        //        file.WriteLine("");
        //        string advance = "";
        //        foreach (string adv in lstAdvancePayment)
        //        {
        //            advance += "/ " + adv;
        //        }
        //        file.WriteLine("Payment From Advance: " + advance);

        //        file.WriteLine("");
        //        foreach (string line in hasSmallAmountCanNotSetAnywhere)
        //        {

        //            file.WriteLine("hasSmallAmountCanNotSetAnywhereAfterAddingProperlyOtherDueBill: " + line);
        //        }
        //    }

        //    //var a  = dt
        //    return dt;


        //}

        private bool AddDueBillsAndOldTransaction(ClientLineStatus ClientLineStatusSave, Package runningPackage, int previousDue)
        {
            double totalMonth = previousDue / runningPackage.PackagePrice;

            for (int i = int.Parse(Math.Truncate(totalMonth).ToString()); i > 0; i--)
            {
                Transaction forMonthlyBill = new Transaction();
                forMonthlyBill.ChangePackageHowMuchTimes = AppUtils.ChangePackageHowMuchTimes;
                forMonthlyBill.PaymentStatus = AppUtils.PaymentIsNotPaid;
                forMonthlyBill.IsNewClient = AppUtils.isNewClient;
                forMonthlyBill.EmployeeID = 2;
                forMonthlyBill.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
                forMonthlyBill.WhoGenerateTheBill = 2;
                forMonthlyBill.PaymentMonth = int.Parse(AppUtils.GetDateTimeNow().AddMonths(-i).Month.ToString());
                forMonthlyBill.PaymentYear = AppUtils.RunningYear;
                forMonthlyBill.PaymentTypeID = AppUtils.PaymentTypeIsMonthly;
                forMonthlyBill.PackageID = runningPackage.PackageID;
                forMonthlyBill.LineStatusID = AppUtils.LineIsActive;
                forMonthlyBill.AmountCountDate = AppUtils.GetDateTimeNow();
                forMonthlyBill.PaymentAmount = runningPackage.PackagePrice;
                db.Transaction.Add(forMonthlyBill);
                db.SaveChanges();

                ClientDueBills clientDueBills = new ClientDueBills();
                clientDueBills.ClientDetailsID = ClientLineStatusSave.ClientDetailsID;
                clientDueBills.DueAmount = forMonthlyBill.Package.PackagePrice;
                clientDueBills.Year = AppUtils.RunningYear;
                clientDueBills.Month = int.Parse(AppUtils.GetDateTimeNow().AddMonths(-i).Month.ToString());
                clientDueBills.Status = false;
                db.ClientDueBills.Add(clientDueBills);

            }

            double a = Math.Truncate(totalMonth);
            if (totalMonth - Math.Truncate(totalMonth) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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
        private int? GetPackageName(List<Package> lstPackage, string profile, ref int monthlyRate)
        {
            //if (profile == "Payment-Due")
            //{
            //    List<string> list400 = new List<string>() { "GT-", "GOLD" };
            //    var pc400 = list400[new Random().Next(list400.Count)];
            //    List<string> list500 = new List<string>() { "DIAMOND", "ECONOMY", "SILVER", "SUITE" };
            //    var pc500 = list500[new Random().Next(list500.Count)];
            //    List<string> list800 = new List<string>() { "PROFILE-5MB", "ECONOMY" };
            //    var pc800 = list800[new Random().Next(list800.Count)];

            //    profile = (monthlyRate <= 300) ? "BRONZE"
            //                     : (monthlyRate > 300 && monthlyRate <= 400) ? pc400
            //                     : (monthlyRate > 400 && monthlyRate <= 500) ? pc500
            //                     : (monthlyRate > 500 && monthlyRate <= 800) ? pc800
            //                     : (monthlyRate > 800 && monthlyRate <= 1200) ? "VIP"
            //                     : (monthlyRate > 1200 && monthlyRate <= 1600) ? "VVIP"
            //                     : (monthlyRate > 1600 && monthlyRate <= 2000) ? "BASNET-VIP"
            //                     : "TestPackage";
            //}
            //if (profile == "TestPackage")
            //{

            //}
            //Package p = db.Package.Where(s => s.PackageName == profile.Trim()).FirstOrDefault();
            //if (p != null)
            //{
            //    return p.PackageID;
            //}
            //else
            //{
            //    Package pcg = new Package();
            //    pcg.MikrotikID = 1;
            //    pcg.IPPoolID = 1;
            //    pcg.PackageName = profile;
            //    pcg.PackagePrice = (monthlyRate <= 300) ? 300
            //                     : (monthlyRate > 300 && monthlyRate <= 400) ? 400
            //                     : (monthlyRate > 400 && monthlyRate <= 500) ? 500
            //                     : (monthlyRate > 500 && monthlyRate <= 800) ? 800
            //                     : (monthlyRate > 800 && monthlyRate <= 1200) ? 1200
            //                     : (monthlyRate > 1200 && monthlyRate <= 1600) ? 1600
            //                     : (monthlyRate > 1600 && monthlyRate <= 2000) ? 2000
            //                     : 0;
            //    pcg.BandWith = "test mbps";
            //    pcg.CreatedBy = "Auni";
            //    pcg.CreatedDate = AppUtils.GetDateNow();
            //    db.Package.Add(pcg);
            //    db.SaveChanges();
            //    return pcg.PackageID;
            //}
            string profileName = profile.Trim();
            //Package p = db.Package.Where(s => s.PackageName == profileName).FirstOrDefault();
            Package p = lstPackage.Where(s => s.PackageName.ToLower().Trim() == profileName.ToLower().Trim()).FirstOrDefault();
            monthlyRate = int.Parse(p.PackagePrice.ToString());
            return p.PackageID;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static string ConvertDataTableToHTMLTable(DataTable dt)
        {
            string ret = "";
            ret = "<table id=" + (char)34 + "tblExcel" + (char)34 + ">";
            ret += "<tr>";
            foreach (DataColumn col in dt.Columns)
            {
                ret += "<td class=" + (char)34 + "tdColumnHeader" + (char)34 + ">" + col.ColumnName + "</td>";
            }
            ret += "</tr>";
            foreach (DataRow row in dt.Rows)
            {
                ret += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ret += "<td class=" + (char)34 + "tdCellData" + (char)34 + ">" + row[i].ToString() + "</td>";
                }
                ret += "</tr>";
            }
            ret += "</table>";
            return ret;
        }
    }
}