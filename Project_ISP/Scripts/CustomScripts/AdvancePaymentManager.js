var AdvancePaymentManager = {

    Validation: function() {
        if (AppUtil.GetIdValue("txtClientName") === '') {
            AppUtil.ShowSuccess("Please Select Client Name.");
            return false;
        }

        if (AppUtil.GetIdValue("txtCredit") === '') {
            AppUtil.ShowSuccess("Please Insert Credit.");
            return false;
        }
        if (AppUtil.GetIdValue("txtRemarks") === '') {
            AppUtil.ShowSuccess("Please Insert Remarks.");
            return false;
        }
        if (AppUtil.GetIdValue("txtMobile") === '') {
            AppUtil.ShowSuccess("Please Insert Mobile Number.");
            return false;
        }
        if (AppUtil.GetIdValue("txtClientAdress") === '') {
            AppUtil.ShowSuccess("Please Insert Client Address.");
            return false;
        }
        return true;
    },

    AddAdvancePayment: function () {
        
        var clientDetailsID = _ClientDetailsID;
        var amount =  $("#txtCredit").val();
        var remarks = $("#txtRemarks").val();
        var url = "/AdvancePayment/SaveAdvanceAmount/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({ ClientDetailsID: clientDetailsID, Amount: amount, Remarks: remarks });
        data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url,"POST",data,AdvancePaymentManager.AddAdvancePaymentSuccess,AdvancePaymentManager.AddAdvancePaymentError);
    },
    AddAdvancePaymentSuccess : function(data){
        if(data.Success === true)
        {
            AppUtil.ShowSuccess("Advance Payment Added Successfully.");
            $("#txtClientName").val("");
            _ClientDetailsID = "";
            $("#txtMobile").val("");
            $("#txtClientAdress").val("");
            $("#txtCredit").val("");
            $("#txtRemarks").val("");
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Advance Payment Fail.");
        }
    },
    AddAdvancePaymentError: function(data)
    {
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    
    },

    getAutoCompleateDetailsInformation: function (val) {
        
        var url = "/AdvancePayment/getAutoCompleateDetailsInformation/";
        var data = ({ ClientDetsilsID: val });
        data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AdvancePaymentManager.getAutoCompleateDetailsInformationSuccess, AdvancePaymentManager.getAutoCompleateDetailsInformationError);
    },
    getAutoCompleateDetailsInformationSuccess: function (data) {
        
       // AppUtil.ShowSuccess("S");
        console.log(data);
        var data = data.ClientDetails;
        $("#txtMobile").val(data.Mobile);
        $("#txtClientAdress").val(data.ClientAdress);

    },
    getAutoCompleateDetailsInformationError: function (data) {
        
       // AppUtil.ShowSuccess("F");
        console.log(data);
    }
    ,
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },



    ViewAdvancePaymentIDForUpdate: function (AdvancePaymentID) {
        
        //AppUtil.ShowWaitingDialog();

      //  setTimeout(function () {
            
            var url = "/AdvancePayment/ViewAdvancePaymentIDForUpdate/";
            var data = JSON.stringify({ AdvancePaymentID: AdvancePaymentID });
            AppUtil.MakeAjaxCall(url, "POST", data, AdvancePaymentManager.ViewAdvancePaymentIDForUpdateSuccess, AdvancePaymentManager.ViewAdvancePaymentIDForUpdateError);
     //   },500);
    },
    ViewAdvancePaymentIDForUpdateSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var PackageJSONParse = (data.ViewAdvancePayment);
        $("#AdvanceLoginName").val(PackageJSONParse.LoginName);
        $("#AdvanceAmount").val(PackageJSONParse.AdvanceAmount);
        $("#AdvanceRemarks").val(PackageJSONParse.Remarks);
        $("#AdvanceContactNumber").val(PackageJSONParse.ContactNumber);
        $("#AdvanceAddress").val(PackageJSONParse.Address);


        
        $("#mdlViewAdvancePaymentForUpdate").modal("show");


    },
    ViewAdvancePaymentIDForUpdateError: function (data) {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    UpdateAdvancePaymentManagerValidation: function() {
        if (AppUtil.GetIdValue("AdvanceRemarks") === '') {
            AppUtil.ShowSuccess("Please Insert Remarks.");
            return false;
        }
        if (AppUtil.GetIdValue("AdvanceAmount") === '') {
            AppUtil.ShowSuccess("Please Insert Advance Amount.");
            return false;
        }
      
        return true;
    },

    UpdateAdvancePayment: function () {
        
        //AppUtil.ShowWaitingDialog();
        //var AdvancePaymentID = AdvancePaymentID;
        var AdvanceAmount = $("#AdvanceAmount").val();
        var Remarks = $("#AdvanceRemarks").val();
        
        var url = "/AdvancePayment/UpdateAdvancePayment/";
        var UpdateAdvancePaymentInformation = ({ AdvancePaymentID: AdvancePaymentID, AdvanceAmount: AdvanceAmount, Remarks: Remarks });
        var data = JSON.stringify({ UpdateAdvancePaymentInformation: UpdateAdvancePaymentInformation });
        AppUtil.MakeAjaxCall(url, "POST", data, AdvancePaymentManager.UpdateAdvancePaymentSuccess, AdvancePaymentManager.UpdateAdvancePaymentError);


    }
    ,UpdateAdvancePaymentSuccess:function(data) {
        
        //AppUtil.HideWaitingDialog();
        if (data.UpdateSuccess === true) {
            
            var UpdateAdvancePaymentInfo = (data.UpdateAdvancePaymentInformation);

            $("#tblViewAdvancePayment tbody>tr").each(function() {
                
                var AdvancePaymentID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (UpdateAdvancePaymentInfo[0].AdvancePaymentID == AdvancePaymentID) {
                    
                    $('#tblViewAdvancePayment tbody>tr:eq(' + index + ')').find("td:eq(3)").text(UpdateAdvancePaymentInfo[0].AdvanceAmount);
                    //$('#tblViewAdvancePayment tbody>tr:eq(' + index + ')').find("td:eq(3)").text(UpdateAdvancePaymentInfo[0].Remarks);
                }
            });
        }
        else {
            alert("success not work");
        }
        $("#mdlViewAdvancePaymentForUpdate").modal('hide');
        console.log(data);

    },
    UpdateAdvancePaymentError: function(data) {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    }

    
    ,PrintAdvancePayment: function () {
        
        var url = "/Excel/CreateReportForAdvancePayment";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({  });
        data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AdvancePaymentManager.PrintAdvancePaymentSuccess, AdvancePaymentManager.PrintAdvancePaymentFail);
    },
    PrintAdvancePaymentSuccess: function (data) {
        
        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;

        //if (data.Success === true) {
        //    AppUtil.ShowSuccess("Employee List Print Successfully.");
        //}
        //if (data.Success === false) {
        //    AppUtil.ShowSuccess("Failed To Print Employee List.");
        //}
    },
    PrintAdvancePaymentFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },
}