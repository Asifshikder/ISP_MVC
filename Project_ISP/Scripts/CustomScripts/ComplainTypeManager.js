var ComplainTypeManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("ComplainTypeName") === '') {
            AppUtil.ShowSuccess("Please Insert ComplainType ");
            return false;
        }
        return true;
    },
    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("ComplainTypeNames") === '') {
            AppUtil.ShowSuccess("Please Insert ComplainType ");
            return false;
        }
        return true;
    },

    InsertComplainTypeFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/ComplainType/InsertComplainTypeFromPopUp";
        var ComplainTypeName = AppUtil.GetIdValue("ComplainTypeName");
        var showMessageBox = $("#chkMessageBox").is(":checked") ? true: false;

        //setTimeout(function () {
        var ComplainType = { ShowMessageBox: showMessageBox, ComplainTypeName: ComplainTypeName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({  ComplainType_Client: ComplainType });
        AppUtil.MakeAjaxCall(url, "POST", datas, ComplainTypeManager.InsertComplainTypeFromPopUpSuccess, ComplainTypeManager.InsertComplainTypeFromPopUpFail);
        //  }, 500);
    },
    InsertComplainTypeFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.ComplainType) {
                
                window.location.reload();
                //var ComplainType = (data.ComplainType);
                //$("#tblComplainType>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + ComplainType.ComplainTypeID + '></td><td>' + ComplainType.ComplainTypeName + '</td><td><a href="" id="showComplainTypeForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("ComplainType Already Added. Choose different ComplainType.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/ComplainType/Index";
        $("#ComplainTypeName").val("");
        $("#mdlComplainTypeInsert").modal('hide');

    },
    InsertComplainTypeFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertComplainType: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/ComplainType/InsertComplainType";
        var ComplainTypeName = AppUtil.GetIdValue("ComplainTypeName");


        //setTimeout(function () {
        var ComplainType = { ComplainTypeName: ComplainTypeName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ComplainType_Client: ComplainType });
        AppUtil.MakeAjaxCall(url, "POST", datas, ComplainTypeManager.InsertComplainTypeSuccess, ComplainTypeManager.InsertComplainTypeUpFail);
        // }, 500);
    },
    InsertComplainTypeSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.ComplainType) {
            // 
            // var ComplainType = (data.ComplainType);
            // $("#tblComplainType>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + ComplainType.ComplainTypeID + '/></td><td>' + ComplainType.ComplainTypeName + '</td><td><a href="" id="showComplainTypeForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("ComplainType Already Added. Choose different ComplainType.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/ComplainType/Index";
        $("#mdlComplainTypeInsert").modal('hide');

    },
    InsertComplainTypeUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowComplainTypeDetailsByIDForUpdate: function (ComplainTypeID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/ComplainType/GetComplainTypeDetailsByID/";
        var data = { ComplainTypeID: ComplainTypeID };
        data = ComplainTypeManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainTypeManager.ShowComplainTypeDetailsByIDForUpdateSuccess, ComplainTypeManager.ShowComplainTypeDetailsByIDForUpdateError);

        // }, 500);

    },
    ShowComplainTypeDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var ComplainTypeDetailsJsonParse = (data.ComplainTypeDetails);
        $("#ComplainTypeNames").val(ComplainTypeDetailsJsonParse.ComplainTypeName);
        $("#chkMessageBoxUpdate").prop("checked", ComplainTypeDetailsJsonParse.ShowMessageBox)


        $("#mdlComplainTypeUpdate").modal("show");
    },
    ShowComplainTypeDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },
    
    UpdatePackageInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var ComplainTypeID = ComplainTypeID;
        var ComplainTypeName = $("#ComplainTypeNames").val();
        var ShowMessageBox = $("#chkMessageBoxUpdate").is(":checked") ? true : false; 

        var url = "/ComplainType/UpdateComplainType";
        var ComplainTypeInfomation = ({ ComplainTypeID: ComplainTypeID, ComplainTypeName: ComplainTypeName, ShowMessageBox : ShowMessageBox });
        var data = JSON.stringify({ ComplainTypeInfoForUpdate: ComplainTypeInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainTypeManager.UpdatePackageInformationSuccess, ComplainTypeManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var ComplainTypeInformation = (data.ComplainTypeUpdateInformation);

            $("#tblComplainType tbody>tr").each(function () {
                
                var ComplainTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ComplainTypeInformation[0].ComplainTypeID == ComplainTypeID) {
                    
                    $('#tblComplainType tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ComplainTypeInformation[0].PackageName);
                    $('#tblComplainType tbody>tr:eq(' + index + ')').find("td:eq(2)").text(ComplainTypeInformation[0].ShowMessageBox);
                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("ComplainType Already Added. Choose different ComplainType. ");
            }
        }

        $("#mdlComplainTypeUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },
    
    PrintComplainTypeList: function () {
        
        var url = "/Excel/CreateReportForComplainTypeList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = ComplainTypeManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainTypeManager.PrintComplainTypeListSuccess, ComplainTypeManager.PrintComplainTypeListFail);
    },
    PrintComplainTypeListSuccess: function (data) {
        
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
    PrintMikrotikListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#ComplainTypeName").val("");
    },
    clearForUpdateInformation: function () {
        $("#ComplainTypeNames").val("");
    }
}