var AssetManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    PrintAssetList: function () {
        
        var url = "/Excel/CreateReportForAssetList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var SearchByAssetTypeID = AppUtil.GetIdValue("SearchByAssetTypeID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({ AssetTypeID: SearchByAssetTypeID });
        data = AssetManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssetManager.PrintAssetListSuccess, AssetManager.PrintAssetListFail);
    },
    PrintAssetListSuccess: function (data) {
        
        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;

        //if (data.Success === true) {
        //    AppUtil.ShowSuccess("Asset List Print Successfully.");
        //}
        //if (data.Success === false) {
        //    AppUtil.ShowSuccess("Failed To Print Asset List.");
        //}
    },
    PrintAssetListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("lstAssetType") === '') {
            AppUtil.ShowSuccess("Please Add Asset Type Name.");
            return false;
        }
        if (AppUtil.GetIdValue("AssetName") === '') {
            AppUtil.ShowSuccess("Please Add Asset Name.");
            return false;
        }
        if (AppUtil.GetIdValue("AssetValue") === '') {
            AppUtil.ShowSuccess("Please Add Asset Value.");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        

        if (AppUtil.GetIdValue("lstAssetTypeUpdate") === '') {
            AppUtil.ShowSuccess("Please Add Asset Type Name.");
            return false;
        }
        if (AppUtil.GetIdValue("AssetNames") === '') {
            AppUtil.ShowSuccess("Please Add Asset Name.");
            return false;
        }
        if (AppUtil.GetIdValue("AssetsValue") === '') {
            AppUtil.ShowSuccess("Please Add Asset Value.");
            return false;
        }
        return true;
    },

    InsertAssetFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Asset/InsertAssetFromPopUp";
        var AssetType = AppUtil.GetIdValue("lstAssetType");
        var AssetName = AppUtil.GetIdValue("AssetName");
        var AssetValue = AppUtil.GetIdValue("AssetValue");
        var PurchaseDate = $("#PurchaseDate").datepicker("getDate");
        var SerialNumber = AppUtil.GetIdValue("SerialNumber");
        var WarrentyStartDate = AppUtil.getDateTime("WarrentyStartDate");
        var WarrentyEndDate = AppUtil.getDateTime("WarrentyEndDate");

        //  setTimeout(function () {
        var Asset = {
            AssetTypeID: AssetType, AssetName: AssetName, AssetValue: AssetValue, PurchaseDate: PurchaseDate, SerialNumber: SerialNumber,
            WarrentyStartDate: WarrentyStartDate, WarrentyEndDate: WarrentyEndDate
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ Asset_Client: Asset });
        AppUtil.MakeAjaxCall(url, "POST", datas, AssetManager.InsertAssetFromPopUpSuccess, AssetManager.InsertAssetFromPopUpFail);
        // }, 500);
    },
    InsertAssetFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Asset) {
                
                var Asset = (data.Asset);

                var WarrentyEndDate = Asset.WarrentyEndDate != null ? AppUtil.ParseDateTime(Asset.WarrentyEndDate) : "";
                var PurchaseDate = Asset.PurchaseDate != null ? AppUtil.ParseDateTime(Asset.PurchaseDate) : "";
                var WarrentyStartDate = Asset.WarrentyStartDate != null ? AppUtil.ParseDateTime(Asset.WarrentyStartDate) : "";
                $("#tblAsset>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Asset.AssetID + '></td><td>' + Asset.AssetTypeName + '</td><td>' + Asset.AssetName + '</td><td>' + Asset.AssetValue + '</td><td>' + PurchaseDate + '</td><td>' + Asset.SerialNumber + '</td><td>' + WarrentyStartDate + '</td><td>' + WarrentyEndDate + '</td><td> <button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"> <span class="glyphicon glyphicon-remove"></span> </button> <button id="btnUpdate" type="button" class="btn btn-success btn-sm padding" data-placement="top" data-toggle="modal" > <span class="glyphicon glyphicon-pencil"></span> </button>    </td></tr> ');
            }
        }
        if (data.SuccessInsert === false) {
            
            AppUtil.ShowSuccess("Save Failed.");
        }
        //  window.location.href = "/Asset/Index";
        AssetManager.ClearForSaveInformation();
        $("#mdlAssetInsert").modal('hide');

    },
    InsertAssetFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertAsset: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Asset/InsertAsset";
        var AssetType = AppUtil.GetIdValue("lstAssetType");
        var AssetName = AppUtil.GetIdValue("AssetName");
        var AssetValue = AppUtil.GetIdValue("AssetValue");
        var PurchaseDate = $("#PurchaseDate").datepicker("getDate");
        var SerialNumber = AppUtil.GetIdValue("SerialNumber");
        var WarrentyStartDate = AppUtil.getDateTime("WarrentyStartDate");
        var WarrentyEndDate = AppUtil.getDateTime("WarrentyEndDate");

        //setTimeout(function () {
        var Asset = {
            AssetTypeID: AssetType, AssetName: AssetName, AssetValue: AssetValue, PurchaseDate: PurchaseDate, SerialNumber: SerialNumber,
            WarrentyStartDate: WarrentyStartDate, WarrentyEndDate: WarrentyEndDate
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ Asset_Client: Asset });
        AppUtil.MakeAjaxCall(url, "POST", datas, AssetManager.InsertAssetSuccess, AssetManager.InsertAssetUpFail);
        // }, 500);
    },
    InsertAssetSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Asset) {
            // 
            // var Asset = (data.Asset);
            // $("#tblAsset>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Asset.AssetID + '/></td><td>' + Asset.AssetName + '</td><td><a href="" id="showAssetForUpdate">Show</a></td></tr>');
            // }
            AssetManager.ClearForSaveInformation();
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Asset Already Added. Choose different Asset Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Asset/Index";
        $("#mdlAssetInsert").modal('hide');

    },
    InsertAssetUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowAssetDetailsByIDForUpdate: function (AssetID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/Asset/GetAssetDetailsByID/";
        var data = { AssetID: AssetID };
        data = AssetManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssetManager.ShowAssetDetailsByIDForUpdateSuccess, AssetManager.ShowAssetDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowAssetDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var AssetDetailsJsonParse = (data.AssetDetails);
        $("#lstAssetTypeUpdate").val(AssetDetailsJsonParse.AssetTypeID);
        $("#AssetNames").val(AssetDetailsJsonParse.AssetName);
        $("#AssetValues").val(AssetDetailsJsonParse.AssetValue);
        $("#PurchaseDates").val(AssetDetailsJsonParse.PurchaseDate != null ? AppUtil.ParseDateTime(AssetDetailsJsonParse.PurchaseDate) : "");
        $("#SerialNumbers").val(AssetDetailsJsonParse.SerialNumber);
        $("#WarrentyStartDates").val(AssetDetailsJsonParse.WarrentyStartDate != null ? AppUtil.ParseDateTime(AssetDetailsJsonParse.WarrentyStartDate) : "");
        $("#WarrentyEndDates").val(AssetDetailsJsonParse.WarrentyEndDate != null ? AppUtil.ParseDateTime(AssetDetailsJsonParse.WarrentyEndDate) : "");


        $("#mdlAssetUpdate").modal("show");
    },
    ShowAssetDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateAssetInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var AssetID = AssetID;
        var AssetTypeID = $("#lstAssetTypeUpdate").val();
        var AssetName = $("#AssetNames").val();
        var AssetAddress = $("#AssetAddresss").val();

        var AssetValue = AppUtil.GetIdValue("AssetValues");
        var PurchaseDate = $("#PurchaseDates").datepicker('getDate');
        var SerialNumber = $("#SerialNumbers").val();
        //  var WarrentyStartDate = AppUtil.ParseDateTime("WarrentyStartDates")
        var WarrentyStartDate = AppUtil.getDateTime("WarrentyStartDates");//$("#WarrentyStartDates").val().toString("mm/dd/yyyy hh:mm:ss");
        var WarrentyEndDate = AppUtil.getDateTime("WarrentyEndDates");


        var url = "/Asset/UpdateAsset";
        var AssetInfomation = ({
            AssetID: AssetID, AssetTypeID: AssetTypeID, AssetName: AssetName, AssetValue: AssetValue, PurchaseDate: PurchaseDate,
            SerialNumber: SerialNumber, WarrentyStartDate: WarrentyStartDate, WarrentyEndDate: WarrentyEndDate
        });
        var data = JSON.stringify({ AssetInfoForUpdate: AssetInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, AssetManager.UpdateAssetInformationSuccess, AssetManager.UpdateAssetInformationFail);
    },
    UpdateAssetInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var AssetInformation = (data.AssetUpdateInformation);

            $("#tblAsset tbody>tr").each(function () {
                
                var AssetID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (AssetInformation[0].AssetID == AssetID) {
                    
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(1)").text(AssetInformation[0].AssetTypeName);
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(2)").text(AssetInformation[0].AssetName);
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(3)").text(AssetInformation[0].AssetValue);
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(4)").text(AssetInformation[0].PurchaseDate != null ? AppUtil.ParseDateTime(AssetInformation[0].PurchaseDate) : "");
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(5)").text(AssetInformation[0].SerialNumber);
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(6)").text(AssetInformation[0].WarrentyStartDate != null ? AppUtil.ParseDateTime(AssetInformation[0].WarrentyStartDate) : "");
                    $('#tblAsset tbody>tr:eq(' + index + ')').find("td:eq(7)").text(AssetInformation[0].WarrentyEndDate != null ? AppUtil.ParseDateTime(AssetInformation[0].WarrentyEndDate) : "");

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            // if (AlreadyInsert = true) {
            AppUtil.ShowSuccess("Asset can not update. Please contact with administrator. ");
            // }
        }

        $("#mdlAssetUpdate").modal('hide');

        AssetManager.ClearForUpdateInformation();
        console.log(data);
    },
    UpdateAssetInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    DeleteAsset: function () {

        
        var url = "/Asset/DeleteAsset/";

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = ({ AssetID: AssetID });
        datas = AssetManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, AssetManager.DeleteAssetSuccess, AssetManager.DeleteAssetFail);
        // }, 50);
    },
    DeleteAssetSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();

        if (data.DeleteStatus === true) {
            $("#tblAsset>tbody>tr").each(function () {
                

                var index = $(this).index();
                var assetID = $(this).find("td:eq(0) input").val();
                if (assetID == data.AssetID) {
                    
                    $('#tblAsset tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
        }

        if (data.DeleteStatus === false) {
            AppUtil.ShowSuccess("Some Information Can not removed.");
        }


        console.log(data);
    },
    DeleteAssetFail: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },


    ShowAssetDetailsByAssetTypeIDForDiv: function (AssetTypeID) {
        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/Asset/GetAssetDetailsByAssetTypeID/";
        var data = { AssetTypeID: AssetTypeID };
        data = AssetManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AssetManager.ShowAssetDetailsByAssetTypeIDForDivSuccess, AssetManager.ShowAssetDetailsByAssetTypeIDForDivError);

        //}, 500);

    },
    ShowAssetDetailsByAssetTypeIDForDivSuccess: function (data) {
        
       // console.log(data);
        //AssetID = fp.AssetID,
        //    AssetTypeName = fp.AssetType.AssetTypeName,
        //    AssetName = fp.AssetName,
        //    AssetValue = fp.AssetValue,
        //    PurchaseDate = fp.PurchaseDate,
        //    SerialNumber = fp.SerialNumber,
        //    WarrentyStartDate = fp.WarrentyStartDate,
        //    WarrentyEndDate = fp.WarrentyEndDate,
        $.each(data.lstAssetByAssetTypeID, function (index, item) {
            $("#tblAssetList>tbody").append("<tr><td>" + item.AssetTypeName + "</td><td>" + item.AssetName + "</td><td>" + item.AssetValue + "</td><td>" + AppUtil.ParseDateTime(item.PurchaseDate) + "</td><td>" + (item.SerialNumber != null ? item.SerialNumber : "") + "</td><td>" + (item.WarrentyStartDate != null ? AppUtil.ParseDateTime(item.WarrentyStartDate) : "") + "</td><td>" + (item.WarrentyEndDate != null ? AppUtil.ParseDateTime(item.WarrentyEndDate):"") + "</td></tr>");
            // alert(item);
        });
        if (data.lstAssetByAssetTypeID.length > 0) {
            $("#divAssetList").show();
        }
    },
    ShowAssetDetailsByAssetTypeIDForDivError: function (data) {

        
        console.log(data);
        $("#divAssetList").hide();
    },

    ClearForSaveInformation: function () {
        //AssetID , AssetName , AssetValue , PurchaseDate = , SerialNumber , WarrentyStartDate , WarrentyEndDate
        $("#lstAssetType").prop("selectedIndex", 0);
        $("#AssetName").val("");
        $("#AssetValue").val("");
        $("#PurchaseDate").val("");
        $("#SerialNumber").val("");
        $("#WarrentyStartDate").val("");
        $("#WarrentyEndDate").val("");
    },
    ClearForUpdateInformation: function () {
        $("#lstAssetTypeUpdate").prop("selectedIndex", 0)
        $("#AssetNames").val("");
        $("#AssetValues").val("");
        $("#PurchaseDates").val("");
        $("#SerialNumbers").val("");
        $("#WarrentyStartDates").val("");
        $("#WarrentyEndDates").val("");
    }
}