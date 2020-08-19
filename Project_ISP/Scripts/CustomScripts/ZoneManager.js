var ZoneManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("ZoneName") === '') {
            AppUtil.ShowSuccess("Please Insert Zone ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("ZoneNames") === '') {
            AppUtil.ShowSuccess("Please Insert Zone ");
            return false;
        }
        return true;
    },

    InsertZoneFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Zone/InsertZoneFromPopUp";
        var ZoneName = AppUtil.GetIdValue("ZoneName");


        //setTimeout(function () {
        var resellerID = $("#ddlCreateReseller").val();
        var Zone = { ZoneName: ZoneName, ResellerID: resellerID };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Zone_Client: Zone });
            AppUtil.MakeAjaxCall(url, "POST", datas, ZoneManager.InsertZoneFromPopUpSuccess, ZoneManager.InsertZoneFromPopUpFail);
      //  }, 500);
    },
    InsertZoneFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Zone) {

                table.draw();
                //window.location.reload();
                //var Zone = (data.Zone);
                //$("#tblZone>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Zone.ZoneID + '></td><td>' + Zone.ZoneName + '</td><td><a href="" id="showZoneForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Zone Already Added. Choose different Zone.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Zone/Index";
        $("#ZoneName").val("");
        $("#mdlZoneInsert").modal('hide');

    },
    InsertZoneFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertZone: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Zone/InsertZone";
        var ZoneName = AppUtil.GetIdValue("ZoneName");


        //setTimeout(function () {
            var Zone = { ZoneName: ZoneName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Zone_Client: Zone });
            AppUtil.MakeAjaxCall(url, "POST", datas, ZoneManager.InsertZoneSuccess, ZoneManager.InsertZoneUpFail);
       // }, 500);
    },
    InsertZoneSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Zone) {
            // 
            // var Zone = (data.Zone);
            // $("#tblZone>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Zone.ZoneID + '/></td><td>' + Zone.ZoneName + '</td><td><a href="" id="showZoneForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Zone Already Added. Choose different Zone.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Zone/Index";
        $("#mdlZoneInsert").modal('hide');

    },
    InsertZoneUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowZoneDetailsByIDForUpdate: function (ZoneID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
            
            var url = "/Zone/GetZoneDetailsByID/";
            var data = { ZoneID: ZoneID };
            data = ZoneManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ZoneManager.ShowZoneDetailsByIDForUpdateSuccess, ZoneManager.ShowZoneDetailsByIDForUpdateError);

       // }, 500);

    },
    ShowZoneDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var ZoneDetailsJsonParse = (data.ZoneDetails);
        $("#ZoneNames").val(ZoneDetailsJsonParse.ZoneName);
        $("#ddlUpdateReseller").val(ZoneDetailsJsonParse.ResellerID);


        $("#mdlZoneUpdate").modal("show");
    },
    ShowZoneDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    UpdatePackageInformation: function () { 

        var ZoneName = $("#ZoneNames").val();
        var resellerID = $("#ddlUpdateReseller").val();


        var url = "/Zone/UpdateZone";
        var ZoneInfomation = ({ ZoneID: ZoneID, ZoneName: ZoneName, ResellerID: resellerID });
        var data = JSON.stringify({ ZoneInfoForUpdate: ZoneInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, ZoneManager.UpdatePackageInformationSuccess, ZoneManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            //var ZoneInformation = (data.ZoneUpdateInformation);

            //$("#tblZone tbody>tr").each(function () {
                
            //    var ZoneID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    if (ZoneInformation[0].ZoneID == ZoneID) {
                    
            //        $('#tblZone tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ZoneInformation[0].PackageName);

            //    }
            //});
            table.draw();
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Zone Already Added. Choose different Zone. ");
            }
        }

        $("#mdlZoneUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintZoneList: function () {
        
        var url = "/Excel/CreateReportForZoneList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = ZoneManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ZoneManager.PrintZoneListSuccess, ZoneManager.PrintZoneListFail);
    },
    PrintZoneListSuccess: function (data) {
        
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
        $("#ZoneName").val("");
        $("#ddlCreateReseller").val("");
    },
    clearForUpdateInformation: function () {
        $("#ZoneNames").val("");
        $("#ddlUpdateReseller").val("");
    }
}