var ProductStatusManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("ProductStatusName") === '') {
            AppUtil.ShowSuccess("Please Insert ProductStatus ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("ProductStatusNames") === '') {
            AppUtil.ShowSuccess("Please Insert ProductStatus ");
            return false;
        }
        return true;
    },

    InsertProductStatusFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/ProductStatus/InsertProductStatusFromPopUp";
        var ProductStatusName = AppUtil.GetIdValue("ProductStatusName");


        //setTimeout(function () {
            var ProductStatus = { ProductStatusName: ProductStatusName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ ProductStatus_Client: ProductStatus });
            AppUtil.MakeAjaxCall(url, "POST", datas, ProductStatusManager.InsertProductStatusFromPopUpSuccess, ProductStatusManager.InsertProductStatusFromPopUpFail);
      //  }, 500);
    },
    InsertProductStatusFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.ProductStatus) {
                
                window.location.reload();
                //var ProductStatus = (data.ProductStatus);
                //$("#tblProductStatus>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + ProductStatus.ProductStatusID + '></td><td>' + ProductStatus.ProductStatusName + '</td><td><a href="" id="showProductStatusForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("ProductStatus Already Added. Choose different ProductStatus.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/ProductStatus/Index";
        $("#ProductStatusName").val("");
        $("#mdlProductStatusInsert").modal('hide');

    },
    InsertProductStatusFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertProductStatus: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/ProductStatus/InsertProductStatus";
        var ProductStatusName = AppUtil.GetIdValue("ProductStatusName");


        //setTimeout(function () {
            var ProductStatus = { ProductStatusName: ProductStatusName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ ProductStatus_Client: ProductStatus });
            AppUtil.MakeAjaxCall(url, "POST", datas, ProductStatusManager.InsertProductStatusSuccess, ProductStatusManager.InsertProductStatusUpFail);
       // }, 500);
    },
    InsertProductStatusSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.ProductStatus) {
            // 
            // var ProductStatus = (data.ProductStatus);
            // $("#tblProductStatus>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + ProductStatus.ProductStatusID + '/></td><td>' + ProductStatus.ProductStatusName + '</td><td><a href="" id="showProductStatusForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("ProductStatus Already Added. Choose different ProductStatus.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/ProductStatus/Index";
        $("#mdlProductStatusInsert").modal('hide');

    },
    InsertProductStatusUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowProductStatusDetailsByIDForUpdate: function (ProductStatusID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
            
            var url = "/ProductStatus/GetProductStatusDetailsByID/";
            var data = { ProductStatusID: ProductStatusID };
            data = ProductStatusManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductStatusManager.ShowProductStatusDetailsByIDForUpdateSuccess, ProductStatusManager.ShowProductStatusDetailsByIDForUpdateError);

      //  }, 500);

    },
    ShowProductStatusDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var ProductStatusDetailsJsonParse = (data.ProductStatusDetails);
        $("#ProductStatusNames").val(ProductStatusDetailsJsonParse.ProductStatusName);


        $("#mdlProductStatusUpdate").modal("show");
    },
    ShowProductStatusDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateProductStatusInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var ProductStatusID = ProductStatusID;
        var ProductStatusName = $("#ProductStatusNames").val();


        var url = "/ProductStatus/UpdateProductStatus";
        var ProductStatusInfomation = ({ ProductStatusID: ProductStatusID, ProductStatusName: ProductStatusName });
        var data = JSON.stringify({ ProductStatusInfoForUpdate: ProductStatusInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, ProductStatusManager.UpdateProductStatusInformationSuccess, ProductStatusManager.UpdateProductStatusInformationFail);
    },
    UpdateProductStatusInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var ProductStatusInformation = (data.ProductStatusUpdateInformation);

            $("#tblProductStatus tbody>tr").each(function () {
                
                var ProductStatusID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ProductStatusInformation[0].ProductStatusID == ProductStatusID) {
                    
                    $('#tblProductStatus tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ProductStatusInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("ProductStatus Already Added. Choose different ProductStatus. ");
            }
        }

        $("#mdlProductStatusUpdate").modal('hide');
        console.log(data);
    },
    UpdateProductStatusInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    UpdateItemInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintProductStatusList: function () {
        
        var url = "/Excel/CreateReportForProductStatusList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = ProductStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductStatusManager.PrintProductStatusListSuccess, ProductStatusManager.PrintProductStatusListFail);
    },
    PrintProductStatusListSuccess: function (data) {
        
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
    PrintProductStatusListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#ProductStatusName").val("");
    },
    clearForUpdateInformation: function () {
        $("#ProductStatusNames").val("");
    }
}