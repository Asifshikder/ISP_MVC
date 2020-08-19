using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using ISP_ManagementSystemModel;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ISP_ManagementSystemModel.Controllers;

namespace Project_ISP.Controllers
{
    public class Status
    {
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
    }

    [SessionTimeout]
    [AjaxAuthorizeAttribute]
    public class SMSController : Controller
    {
        public SMSController()
        {
            AppUtils.dateTimeNow = DateTime.Now;
        }
        private ISPContext db = new ISPContext();
        // GET: SMS

        [UserRIghtCheck(ControllerValue = AppUtils.Add_Update_SMS_Sender_IDPass)]
        public ActionResult SMSIDPass()
        {
            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.FirstOrDefault();
            // ViewBag.ID = smsSenderIdPass!= null ? smsSenderIdPass.ID : "";
            //ViewBag.pass = smsSenderIdPass!= null ? smsSenderIdPass.Pass : "";
            //ViewBag.Sender = smsSenderIdPass!= null ? smsSenderIdPass.Sender : "";
            //  ViewBag.CompanyName = smsSenderIdPass!= null ? smsSenderIdPass.CompanyName : "";
            //  ViewBag.HelpLine = smsSenderIdPass != null ? smsSenderIdPass.HelpLine : "";
            return View(smsSenderIdPass);
        }

        [HttpPost]
        [JSON_Antiforgery_Token_Validation.ValidateJsonAntiForgeryTokenAttribute]
        [UserRIghtCheck(ControllerValue = AppUtils.Add_Update_SMS_Sender_IDPass)]
        public ActionResult UpdateSMSIDPass(SMSSenderIDPass smsSenderIdPass)
        {
            SMSSenderIDPass SMSSenderIDPass = db.SMSSenderIDPass.FirstOrDefault();
            try
            {
                if (SMSSenderIDPass == null)
                {
                    smsSenderIdPass.CreateBy = AppUtils.GetLoginEmployeeName();
                    smsSenderIdPass.CreateDate = AppUtils.dateTimeNow;
                    db.SMSSenderIDPass.Add(smsSenderIdPass);
                    db.SaveChanges();
                }
                else
                {
                    smsSenderIdPass.Status = SMSSenderIDPass.Status;
                    smsSenderIdPass.SMSSenderIDPassID = SMSSenderIDPass.SMSSenderIDPassID;
                    smsSenderIdPass.CreateBy = AppUtils.GetLoginEmployeeName();
                    smsSenderIdPass.CreateDate = AppUtils.dateTimeNow;
                    smsSenderIdPass.UpdateBy = AppUtils.GetLoginEmployeeName();
                    smsSenderIdPass.UpdateDate = AppUtils.dateTimeNow;
                    db.Entry(SMSSenderIDPass).CurrentValues.SetValues(smsSenderIdPass);
                    db.SaveChanges();
                }

                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false });
            }
        }

        [UserRIghtCheck(ControllerValue = AppUtils.View_SMS_Settings_List)]
        public ActionResult SMSConfiguration()
        {
            List<Status> lstStatuses = new List<Status>();
            lstStatuses.Add(new Status { StatusID = 0, StatusCode = "Disabaled" });
            lstStatuses.Add(new Status { StatusID = 1, StatusCode = "Enable" });

            ViewBag.ddlSMSStatus = new SelectList(lstStatuses, "StatusID", "StatusCode");
            List<SMS> lstSMS = db.SMS.ToList();

            return View(lstSMS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSMSDetailsByID(int SMSID)
        {
            List<SMS> lstSms = db.SMS.Where(s => s.SMSID == SMSID).ToList();
            if (lstSms.Count > 0)
            {
                return Json(new { Success = true, SMSInfo = lstSms[0] }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, SMSInfo = new List<SMS>() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateSMS(SMS SMSInfoForUpdate)
        {


            try
            {
                var SMS_db = db.SMS.Where(s => s.SMSID == SMSInfoForUpdate.SMSID);
                SMSInfoForUpdate.CreateBy = SMS_db.FirstOrDefault().CreateBy;
                SMSInfoForUpdate.CreateDate = SMS_db.FirstOrDefault().CreateDate;
                SMSInfoForUpdate.UpdateBy = AppUtils.GetLoginUserID();
                SMSInfoForUpdate.UpdateDate = AppUtils.GetDateTimeNow();

                db.Entry(SMS_db.SingleOrDefault()).CurrentValues.SetValues(SMSInfoForUpdate);
                db.SaveChanges();

                //Response.Redirect("/page/pagename.aspx", true);

                var JSON = Json(new { UpdateSuccess = true, SMSUpdateInformation = SMS_db }, JsonRequestBehavior.AllowGet);
                JSON.MaxJsonLength = int.MaxValue;
                return JSON;
            }
            catch
            {
                return Json(new { UpdateSuccess = false, SMSUpdateInformation = "" }, JsonRequestBehavior.AllowGet);

            }

            // SendSMS();

        }

        //private void SendSMS()
        //{

        //    Response.RedirectLocation("");
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendSMSToClient(string message, bool IsCheckAll, string[] IfIsCheckAllThenNonCheckList, string[] IfNotCheckAllThenCheckList, int ForWhichTypeSMSIsThat, int UserType = 0, int ZoneID = 0)
        {
            try
            {
                List<ClientDetails> lstClientDetails = new List<ClientDetails>();
                if (IsCheckAll)
                {
                    var clientDetails = db.ClientDetails.AsQueryable();
                    if (UserType != 0 && ZoneID != 0)
                    {
                        clientDetails = clientDetails.Where(x => x.Status == UserType && x.ZoneID == ZoneID).AsQueryable();
                    }
                    else if (UserType != 0)
                    {
                        clientDetails = clientDetails.Where(x => x.Status == UserType).AsQueryable();
                    }
                    else if (ZoneID != 0)
                    {
                        clientDetails = clientDetails.Where(x => x.ZoneID == ZoneID).AsQueryable();
                    }
                    else
                    {
                        var test = "";
                    }

                    if (IfIsCheckAllThenNonCheckList != null)
                    {
                        List<int> lstClientDetailsInInt = IfIsCheckAllThenNonCheckList.Select(int.Parse).ToList();
                        //lstClientDetails = clientDetails/*.AsEnumerable().Where(x => !lstClientDetailsInInt.Contains(x.ClientDetailsID))*/.ToList();
                        var lstClientPhoneNumberWithID = clientDetails.Where(x => !lstClientDetailsInInt.Contains(x.ClientDetailsID)).Select(x => new SendSMSCustomInformation { ClientID = x.ClientDetailsID, Phone = x.ContactNumber }).ToList();
                        //var lstClientDetails2 = clientDetails.ToList().Where(x => !lstClientDetailsInInt.Contains(x.ClientDetailsID)).ToList();
                        //lstClientDetails = clientDetails.ToList();
                        Session["IdListForSMSSend"] = lstClientPhoneNumberWithID;
                    }
                    else
                    {
                        Session["IdListForSMSSend"] = clientDetails.Select(x => new SendSMSCustomInformation { ClientID = x.ClientDetailsID, Phone = x.ContactNumber }).ToList();
                    }
                }
                else
                {
                    if (IfNotCheckAllThenCheckList != null)
                    {
                        //var clientList = db.ClientDetails.Where(x => IfNotCheckAllThenCheckList.Any(o => o == x.ClientDetailsID.ToString()));

                        List<int> lstClientDetailsInInt = IfNotCheckAllThenCheckList.Select(int.Parse).ToList();
                        var clientList = db.ClientDetails.Where(x => lstClientDetailsInInt.Contains(x.ClientDetailsID));
                        var lstCLientPhoneNumberWithID = clientList.Select(x => new SendSMSCustomInformation { ClientID = x.ClientDetailsID, Phone = x.ContactNumber }).ToList();
                        
                        Session["IdListForSMSSend"] = lstCLientPhoneNumberWithID;
                    }
                }
                //var aa = clientDetails.ToList();

                List<string> lstMobileNumber = new List<string>();
                List<SendSMSCustomInformation> clnIdList = Session["IdListForSMSSend"] as List<SendSMSCustomInformation>;
                List<SendSMSCustomInformation> lsttemp = new List<SendSMSCustomInformation>();
                //if (IsCheckAll)
                //{
                //    if (IfIsCheckAllThenNonCheckList != null)
                //    {
                //        lsttemp = clnIdList.Where(x => !IfIsCheckAllThenNonCheckList.Contains(x.ClientID.ToString())).ToList();
                //    }
                //    else
                //    {
                //        lsttemp = clnIdList;
                //    }
                //}
                //else
                //{
                //    if (IfNotCheckAllThenCheckList != null)
                //    {
                //        lsttemp = clnIdList.Where(x => IfNotCheckAllThenCheckList.Contains(x.ClientID.ToString())).ToList(); ;
                //    }
                //    //else
                //    //{
                //    //    lsttemp = clnIdList;
                //    //}
                //}

                //if (IsCheckAll)
                //{
                //    if (IfIsCheckAllThenNonCheckList != null)
                //    {
                //        clnIdList.RemoveAll(x => IfIsCheckAllThenNonCheckList.Contains(x.ClientID.ToString()));
                //    }
                //}
                //else
                //{
                //    if (IfNotCheckAllThenCheckList != null)
                //    {
                //        clnIdList.RemoveAll(x => !IfNotCheckAllThenCheckList.Contains(Convert.ToString(x.ClientID)));
                //    }
                //}
                //List<SendSMSCustomInformation> lsttemp2 = TempData["IdListForSMSSendTemp"] as List<SendSMSCustomInformation>;
                //OptionSettings os = db.OptionSettings.Where(s => s.OptionSettingsName == AppUtils.SMSOptionName && s.Status == AppUtils.SendSMSStatusTrue).FirstOrDefault();
                if ((bool)Session["SMSOptionEnable"])
                {
                    foreach (var client in clnIdList)
                    {
                        try
                        {
                            //send sms here

                            SMSSenderIDPass smsSenderIdPass = db.SMSSenderIDPass.Where(s => s.Status == AppUtils.SMSGlobalStatusIsTrue).FirstOrDefault();
                            if (smsSenderIdPass != null)
                            {
                                SMSReturnDetails SMSReturnDetailsClient = AppUtils.SendSMS(smsSenderIdPass.Sender, smsSenderIdPass.ID, smsSenderIdPass.Pass, client.Phone, message);
                                if (SMSReturnDetailsClient.statusCode == AppUtils.ReturnMessageStatusCodeIsSuccess)
                                {

                                }
                                else
                                {
                                    lstMobileNumber.Add(client.Phone);
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            lstMobileNumber.Add(client.Phone);
                        }
                    }
                }
                return Json(new { sendSMSSuccess = true, lstMobileUnableToSendSMS = lstMobileNumber }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { sendSMSFail = true }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}