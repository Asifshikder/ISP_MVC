var SMSManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("SMSName") === '') {
            AppUtil.ShowSuccess("Please Add SMS Name.");
            return false;
        }
        if (AppUtil.GetIdValue("SMSAddress") === '') {
            AppUtil.ShowSuccess("Please Insert SMS Location. ");
            return false;
        }
        return true;
    },

    CreateValidationForSaveSMSSenderInformation: function () {
        
        if (AppUtil.GetIdValue("ID") === '') {
            AppUtil.ShowSuccess("Please Insert User ID.");
            return false;
        }
        if (AppUtil.GetIdValue("Pass") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        //if (AppUtil.GetIdValue("Sender") === '') {
        //    AppUtil.ShowSuccess("Please Insert Masking Name. ");
        //    return false;
        //}
        if (AppUtil.GetIdValue("CompanyName") === '') {
            AppUtil.ShowSuccess("Please Insert Company Name.");
            return false;
        }
        if (AppUtil.GetIdValue("HelpLine") === '') {
            AppUtil.ShowSuccess("Please Insert Help Line Number. ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("SMSNames") === '') {
            AppUtil.ShowSuccess("Please Add SMS Name.");
            return false;
        }
        if (AppUtil.GetIdValue("SMSAddresss") === '') {
            AppUtil.ShowSuccess("Please Insert SMS Location ");
            return false;
        }
        return true;
    },


    InsertSmsSenderIdPass: function () {
        
        var url = "/SMS/UpdateSMSIDPass";
        var ID = AppUtil.GetIdValue("ID");
        var Pass = AppUtil.GetIdValue("Pass");
        var Sender = AppUtil.GetIdValue("Sender");
        var CompanyName = AppUtil.GetIdValue("CompanyName");
        var HelpLine = AppUtil.GetIdValue("HelpLine");


        var SMSIDPass = { ID: ID, Pass: Pass, Sender: Sender, CompanyName: CompanyName, HelpLine: HelpLine };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var headers = {};
        headers['__RequestVerificationToken'] = AntiForgeryToken;

        
        var datas = JSON.stringify({ smsSenderIdPass: SMSIDPass });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", datas, headers, SMSManager.InsertSmsSenderIdPassSuccess, SMSManager.InsertSmsSenderIdPassFail);

    },
    InsertSmsSenderIdPassSuccess: function (data) {
        
        console.log(data);

        if (data.Success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
        }
        if (data.Success === false) {
            
            AppUtil.ShowSuccess("Saved Failed.");
        }


    },
    InsertSmsSenderIdPassFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertSMSFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/SMS/InsertSMSFromPopUp";
        var SMSName = AppUtil.GetIdValue("SMSName");
        var SMSAddress = AppUtil.GetIdValue("SMSAddress");


        //  setTimeout(function () {
        var SMS = { SMSName: SMSName, SMSAddress: SMSAddress };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ SMS_Client: SMS });
        AppUtil.MakeAjaxCall(url, "POST", datas, SMSManager.InsertSMSFromPopUpSuccess, SMSManager.InsertSMSFromPopUpFail);
        // }, 500);
    },
    InsertSMSFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.SMS) {
                
                var SMS = (data.SMS);
                $("#tblSmsConfiguration>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + SMS.SMSID + '></td><td>' + SMS.SMSName + '</td><td>' + SMS.SMSAddress + '</td><td><a href="" id="showSMSForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("SMS Already Added. Choose different SMS Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/SMS/Index";
        SMSManager.clearForSaveInformation();
        $("#mdlSMSInsert").modal('hide');

    },
    InsertSMSFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertSMS: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/SMS/InsertSMS";
        var SMSName = AppUtil.GetIdValue("SMSName");
        var SMSAddress = AppUtil.GetIdValue("SMSAddress");


        //setTimeout(function () {
        var SMS = { SMSName: SMSName, SMSAddress: SMSAddress };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ SMS_Client: SMS });
        AppUtil.MakeAjaxCall(url, "POST", datas, SMSManager.InsertSMSSuccess, SMSManager.InsertSMSUpFail);
        // }, 500);
    },
    InsertSMSSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.SMS) {
            // 
            // var SMS = (data.SMS);
            // $("#tblSmsConfiguration>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + SMS.SMSID + '/></td><td>' + SMS.SMSName + '</td><td><a href="" id="showSMSForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("SMS Already Added. Choose different SMS Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/SMS/Index";
        $("#mdlSMSInsert").modal('hide');

    },
    InsertSMSUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowSMSDetailsByIDForUpdate: function (SMSID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/SMS/GetSMSDetailsByID/";
        var data = { SMSID: SMSID };
        data = SMSManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SMSManager.ShowSMSDetailsByIDForUpdateSuccess, SMSManager.ShowSMSDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowSMSDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var SMSInfo = (data.SMSInfo);
        $("#txtSMSTitle").val(SMSInfo.SMSTitle.trim());
        $("#txtMaskingName").val(SMSInfo.Sender);
        $("#txtSMSDetails").val(SMSInfo.SendMessageText.trim());
        $("#txtSMSCode").val(SMSInfo.SMSCode);
        $("#ddlSMSStatus").val(SMSInfo.SMSStatus);
        $("#txtSMSCounter").val(SMSInfo.SMSCounter);


        $("#mdlSMSSettingsUpdate").modal("show");
    },
    ShowSMSDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateSMSInformation: function () {
        
        var SMSTitle = $("#txtSMSTitle").val();
        var Sender = $("#txtMaskingName").val();
        var SendMessageText = $("#txtSMSDetails").val();
        var SMSCode = $("#txtSMSCode").val();
        var SMSStatus = $("#ddlSMSStatus").val();
        var SMSCounter = $("#txtSMSCounter").val();


        var url = "/SMS/UpdateSMS";
        var SMSInfomation = ({ SMSID: SMSID, SMSTitle: SMSTitle, Sender: Sender, SendMessageText: SendMessageText, SMSCode: SMSCode, SMSStatus: SMSStatus, SMSCounter: SMSCounter });
        var data = JSON.stringify({ SMSInfoForUpdate: SMSInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, SMSManager.UpdateSMSInformationSuccess, SMSManager.UpdateSMSInformationFail);
    },
    UpdateSMSInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var SMSInformation = (data.SMSUpdateInformation);

            $("#tblSmsConfiguration tbody>tr").each(function () {
                
                var SMSID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (SMSInformation[0].SMSID == SMSID) {
                    


                    $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(1)").text(SMSInformation[0].SMSTitle);
                    $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(2)").text(SMSInformation[0].SendMessageText);
                    $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(3)").text(SMSInformation[0].SMSCode);
                    $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(4)").text(SMSInformation[0].Sender);
                    if (SMSInformation[0].SMSStatus === 1) {
                        $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(5)").html("<div style='color:green; font-weight:bold'>Active</div>");
                    } else {
                        $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(5)").html("<div style='color:red; font-weight:bold'>Inactive</div>");
                    }
                    $('#tblSmsConfiguration tbody>tr:eq(' + index + ')').find("td:eq(6)").text(SMSInformation[0].SMSCounter);
                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            AppUtil.ShowSuccess("Update Fail. Contact With Administrator. ");
        }

        $("#mdlSMSSettingsUpdate").modal('hide');

        SMSManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateSMSInformationFail: function (data) {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    clearForSaveInformation: function () {
        $("#txtSMSTitle").val("");
        $("#txtMaskingName").val("");
        $("#txtSMSDetails").val("");
        $("#txtSMSCode").val("");
        $("#ddlSMSStatus").val("");
        $("#txtSMSCounter").val("");
    },
    clearForUpdateInformation: function () {
        $("#txtSMSTitle").val("");
        $("#txtMaskingName").val("");
        $("#txtSMSDetails").val("");
        $("#txtSMSCode").val("");
        $("#ddlSMSStatus").val("");
        $("#txtSMSCounter").val("");
    }
}