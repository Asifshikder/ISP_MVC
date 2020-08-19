var DistributionReasonManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("DistributionReasonName") === '') {
            AppUtil.ShowSuccess("Please Insert DistributionReason ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("DistributionReasonNames") === '') {
            AppUtil.ShowSuccess("Please Insert DistributionReason ");
            return false;
        }
        return true;
    },

    InsertDistributionReasonFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/DistributionReason/InsertDistributionReasonFromPopUp";
        var DistributionReasonName = AppUtil.GetIdValue("DistributionReasonName");


     //   setTimeout(function () {
            var DistributionReason = { DistributionReasonName: DistributionReasonName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ DistributionReason_Client: DistributionReason });
            AppUtil.MakeAjaxCall(url, "POST", datas, DistributionReasonManager.InsertDistributionReasonFromPopUpSuccess, DistributionReasonManager.InsertDistributionReasonFromPopUpFail);
      //  }, 500);
    },
    InsertDistributionReasonFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.DistributionReason) {
                
                window.location.reload();
                //var DistributionReason = (data.DistributionReason);
                //$("#tblDistributionReason>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + DistributionReason.DistributionReasonID + '></td><td>' + DistributionReason.DistributionReasonName + '</td><td><a href="" id="showDistributionReasonForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("DistributionReason Already Added. Choose different DistributionReason.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/DistributionReason/Index";
        $("#DistributionReasonName").val("");
        $("#mdlDistributionReasonInsert").modal('hide');

    },
    InsertDistributionReasonFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertDistributionReason: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/DistributionReason/InsertDistributionReason";
        var DistributionReasonName = AppUtil.GetIdValue("DistributionReasonName");


        //setTimeout(function () {
            var DistributionReason = { DistributionReasonName: DistributionReasonName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ DistributionReason_Client: DistributionReason });
            AppUtil.MakeAjaxCall(url, "POST", datas, DistributionReasonManager.InsertDistributionReasonSuccess, DistributionReasonManager.InsertDistributionReasonUpFail);
      //  }, 500);
    },
    InsertDistributionReasonSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
           // if (data.DistributionReason) {
               // 
               // var DistributionReason = (data.DistributionReason);
               // $("#tblDistributionReason>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + DistributionReason.DistributionReasonID + '/></td><td>' + DistributionReason.DistributionReasonName + '</td><td><a href="" id="showDistributionReasonForUpdate">Show</a></td></tr>');
           // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("DistributionReason Already Added. Choose different DistributionReason.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/DistributionReason/Index";
        $("#mdlDistributionReasonInsert").modal('hide');

    },
    InsertDistributionReasonUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowDistributionReasonDetailsByIDForUpdate: function (DistributionReasonID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
            
            var url = "/DistributionReason/GetDistributionReasonDetailsByID/";
            var data = { DistributionReasonID: DistributionReasonID };
            data = DistributionReasonManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, DistributionReasonManager.ShowDistributionReasonDetailsByIDForUpdateSuccess, DistributionReasonManager.ShowDistributionReasonDetailsByIDForUpdateError);

   //     }, 500);

    },
    ShowDistributionReasonDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var DistributionReasonDetailsJsonParse = (data.DistributionReasonDetails);
        $("#DistributionReasonNames").val(DistributionReasonDetailsJsonParse.DistributionReasonName);


        $("#mdlDistributionReasonUpdate").modal("show");
    },
    ShowDistributionReasonDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    UpdateDistributionReasonInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var DistributionReasonID = DistributionReasonID;
        var DistributionReasonName = $("#DistributionReasonNames").val();


        var url = "/DistributionReason/UpdateDistributionReason";
        var DistributionReasonInfomation = ({ DistributionReasonID: DistributionReasonID, DistributionReasonName: DistributionReasonName });
        var data = JSON.stringify({ DistributionReasonInfoForUpdate: DistributionReasonInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, DistributionReasonManager.UpdateDistributionReasonInformationSuccess, DistributionReasonManager.UpdateDistributionReasonInformationFail);
    },
    UpdateDistributionReasonInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var DistributionReasonInformation = (data.DistributionReasonUpdateInformation);

            $("#tblDistributionReason tbody>tr").each(function () {
                
                var DistributionReasonID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (DistributionReasonInformation[0].DistributionReasonID == DistributionReasonID) {
                    
                    $('#tblDistributionReason tbody>tr:eq(' + index + ')').find("td:eq(1)").text(DistributionReasonInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("DistributionReason Already Added. Choose different DistributionReason. ");
            }
        }

        $("#mdlDistributionReasonUpdate").modal('hide');
        console.log(data);
    },
    UpdateDistributionReasonInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    
    PrintDistributionReasonList: function () {
        
        var url = "/Excel/CreateReportForDistributionReasonList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = DistributionReasonManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, DistributionReasonManager.PrintDistributionReasonListSuccess, DistributionReasonManager.PrintDistributionReasonListFail);
    },
    PrintDistributionReasonListSuccess: function (data) {
        
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
    PrintDistributionReasonListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#DistributionReasonName").val("");
    },
clearForUpdateInformation: function () {
    $("#DistributionReasonNames").val("");
}
}