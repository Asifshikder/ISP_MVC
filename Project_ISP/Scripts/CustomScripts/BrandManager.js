var BrandManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("BrandName") === '') {
            AppUtil.ShowSuccess("Please Insert Brand ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("BrandNames") === '') {
            AppUtil.ShowSuccess("Please Insert Brand ");
            return false;
        }
        return true;
    },

    InsertBrandFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Brand/InsertBrandFromPopUp";
        var BrandName = AppUtil.GetIdValue("BrandName");


   //     setTimeout(function () {
            var Brand = { BrandName: BrandName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Brand_Client: Brand });
            AppUtil.MakeAjaxCall(url, "POST", datas, BrandManager.InsertBrandFromPopUpSuccess, BrandManager.InsertBrandFromPopUpFail);
      //  }, 500);
    },
    InsertBrandFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Brand) {
                
                window.location.reload();
                //var Brand = (data.Brand);
                //$("#tblBrand>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Brand.BrandID + '></td><td>' + Brand.BrandName + '</td><td><a href="" id="showBrandForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Brand Already Added. Choose different Brand.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Brand/Index";
        $("#BrandName").val("");
        $("#mdlBrandInsert").modal('hide');

    },
    InsertBrandFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertBrand: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Brand/InsertBrand";
        var BrandName = AppUtil.GetIdValue("BrandName");


   //     setTimeout(function () {
            var Brand = { BrandName: BrandName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Brand_Client: Brand });
            AppUtil.MakeAjaxCall(url, "POST", datas, BrandManager.InsertBrandSuccess, BrandManager.InsertBrandUpFail);
       // }, 500);
    },
    InsertBrandSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
           // if (data.Brand) {
               // 
               // var Brand = (data.Brand);
               // $("#tblBrand>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Brand.BrandID + '/></td><td>' + Brand.BrandName + '</td><td><a href="" id="showBrandForUpdate">Show</a></td></tr>');
           // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Brand Already Added. Choose different Brand.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Brand/Index";
        $("#mdlBrandInsert").modal('hide');

    },
    InsertBrandUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowBrandDetailsByIDForUpdate: function (BrandID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
            
            var url = "/Brand/GetBrandDetailsByID/";
            var data = { BrandID: BrandID };
            data = BrandManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BrandManager.ShowBrandDetailsByIDForUpdateSuccess, BrandManager.ShowBrandDetailsByIDForUpdateError);

    //    }, 500);

    },
    ShowBrandDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var BrandDetailsJsonParse = (data.BrandDetails);
        $("#BrandNames").val(BrandDetailsJsonParse.BrandName);


        $("#mdlBrandUpdate").modal("show");
    },
    ShowBrandDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    UpdatePackageInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var BrandID = BrandID;
        var BrandName = $("#BrandNames").val();


        var url = "/Brand/UpdateBrand";
        var BrandInfomation = ({ BrandID: BrandID, BrandName: BrandName });
        var data = JSON.stringify({ BrandInfoForUpdate: BrandInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, BrandManager.UpdatePackageInformationSuccess, BrandManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var BrandInformation = (data.BrandUpdateInformation);

            $("#tblBrand tbody>tr").each(function () {
                
                var BrandID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (BrandInformation[0].BrandID == BrandID) {
                    
                    $('#tblBrand tbody>tr:eq(' + index + ')').find("td:eq(1)").text(BrandInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Brand Already Added. Choose different Brand. ");
            }
        }

        $("#mdlBrandUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintBrandList: function () {
        
        var url = "/Excel/CreateReportForBrandList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = BrandManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BrandManager.PrintBrandListSuccess, BrandManager.PrintBrandListFail);
    },
    PrintBrandListSuccess: function (data) {
        
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
    PrintBrandListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#BrandName").val("");
    },
    clearForUpdateInformation: function () {
        $("#BrandNames").val("");
    }
}