var AssetTypeManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("AssetTypeName") === '') {
            AppUtil.ShowSuccess("Please Insert AssetType ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("AssetTypeNames") === '') {
            AppUtil.ShowSuccess("Please Insert AssetType ");
            return false;
        }
        return true;
    },

    InsertAssetTypeFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/AssetType/InsertAssetTypeFromPopUp";
        var AssetTypeName = AppUtil.GetIdValue("AssetTypeName");


        //setTimeout(function () {
        var AssetType = { AssetTypeName: AssetTypeName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ AssetType_Client: AssetType });
        AppUtil.MakeAjaxCall(url, "POST", datas, AssetTypeManager.InsertAssetTypeFromPopUpSuccess, AssetTypeManager.InsertAssetTypeFromPopUpFail);
        //  }, 500);
    },
    InsertAssetTypeFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.AssetType) {
                
                window.location.reload();
                //var AssetType = (data.AssetType);
                //$("#tblAssetType>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + AssetType.AssetTypeID + '></td><td>' + AssetType.AssetTypeName + '</td><td><a href="" id="showAssetTypeForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("AssetType Already Added. Choose different AssetType.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/AssetType/Index";
        $("#AssetTypeName").val("");
        $("#mdlAssetTypeInsert").modal('hide');

    },
    InsertAssetTypeFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertAssetType: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/AssetType/InsertAssetType";
        var AssetTypeName = AppUtil.GetIdValue("AssetTypeName");


        //setTimeout(function () {
        var AssetType = { AssetTypeName: AssetTypeName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ AssetType_Client: AssetType });
        AppUtil.MakeAjaxCall(url, "POST", datas, AssetTypeManager.InsertAssetTypeSuccess, AssetTypeManager.InsertAssetTypeUpFail);
        // }, 500);
    },
    InsertAssetTypeSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.AssetType) {
            // 
            // var AssetType = (data.AssetType);
            // $("#tblAssetType>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + AssetType.AssetTypeID + '/></td><td>' + AssetType.AssetTypeName + '</td><td><a href="" id="showAssetTypeForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("AssetType Already Added. Choose different AssetType.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/AssetType/Index";
        $("#mdlAssetTypeInsert").modal('hide');

    },
    InsertAssetTypeUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowAssetTypeDetailsByIDForUpdate: function (AssetTypeID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/AssetType/GetAssetTypeDetailsByID/";
        var data = { AssetTypeID: AssetTypeID };
        data = AssetTypeManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssetTypeManager.ShowAssetTypeDetailsByIDForUpdateSuccess, AssetTypeManager.ShowAssetTypeDetailsByIDForUpdateError);

        // }, 500);

    },
    ShowAssetTypeDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var AssetTypeDetailsJsonParse = (data.AssetTypeDetails);
        $("#AssetTypeNames").val(AssetTypeDetailsJsonParse.AssetTypeName);


        $("#mdlAssetTypeUpdate").modal("show");
    },
    ShowAssetTypeDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    UpdatePackageInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var AssetTypeID = AssetTypeID;
        var AssetTypeName = $("#AssetTypeNames").val();


        var url = "/AssetType/UpdateAssetType";
        var AssetTypeInfomation = ({ AssetTypeID: AssetTypeID, AssetTypeName: AssetTypeName });
        var data = JSON.stringify({ AssetTypeInfoForUpdate: AssetTypeInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, AssetTypeManager.UpdatePackageInformationSuccess, AssetTypeManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var AssetTypeInformation = (data.AssetTypeUpdateInformation);

            $("#tblAssetType tbody>tr").each(function () {
                
                var AssetTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (AssetTypeInformation[0].AssetTypeID == AssetTypeID) {
                    
                    $('#tblAssetType tbody>tr:eq(' + index + ')').find("td:eq(1)").text(AssetTypeInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("AssetType Already Added. Choose different AssetType. ");
            }
        }

        $("#mdlAssetTypeUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintAssetTypeList: function () {
        
        var url = "/Excel/CreateReportForAssetTypeList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = AssetTypeManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssetTypeManager.PrintAssetTypeListSuccess, AssetTypeManager.PrintAssetTypeListFail);
    },
    PrintAssetTypeListSuccess: function (data) {
        
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
        $("#AssetTypeName").val("");
    },
    clearForUpdateInformation: function () {
        $("#AssetTypeNames").val("");
    }
}