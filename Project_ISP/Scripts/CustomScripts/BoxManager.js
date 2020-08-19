var BoxManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("BoxName") === '') {
            AppUtil.ShowSuccess("Please Add Box Name.");
            return false;
        }
        if (AppUtil.GetIdValue("BoxLocation") === '') {
            AppUtil.ShowSuccess("Please Insert Box Location. ");
            return false;
        }
        if (AppUtil.GetIdValue("LatitudeLongitude") === '') {
            AppUtil.ShowSuccess("Please Insert Box Latitude & Longitude. ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("BoxNames") === '') {
            AppUtil.ShowSuccess("Please Add Box Name.");
            return false;
        }
        if (AppUtil.GetIdValue("BoxLocations") === '') {
            AppUtil.ShowSuccess("Please Insert Box Location ");
            return false;
        }
        if (AppUtil.GetIdValue("NewLatitudeLongitude") === '') {
            AppUtil.ShowSuccess("Please Insert Box Latitude & Longitude ");
            return false;
        }
        return true;
    },

    InsertBoxFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Box/InsertBoxFromPopUp";
        var ResellerID = AppUtil.GetIdValue("ddlReseller");
        var BoxName = AppUtil.GetIdValue("BoxName");
        var BoxLocation = AppUtil.GetIdValue("BoxLocation");
        var LatitudeLongitude = AppUtil.GetIdValue("LatitudeLongitude");


  //      setTimeout(function () {
        var Box = { BoxName: BoxName, BoxLocation: BoxLocation, ResellerID: ResellerID, LatitudeLongitude: LatitudeLongitude };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Box_Client: Box });
            AppUtil.MakeAjaxCall(url, "POST", datas, BoxManager.InsertBoxFromPopUpSuccess, BoxManager.InsertBoxFromPopUpFail);
     //   }, 500);
    },
    InsertBoxFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Box) {
                
                window.location.reload();
                //var Box = (data.Box);
                //$("#tblBox>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Box.BoxID + '></td><td>' + Box.BoxName + '</td><td>' + Box.BoxLocation + '</td><td><a href="" id="showBoxForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Box Already Added. Choose different Box Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Box/Index";
        BoxManager.clearForSaveInformation();
        $("#mdlBoxInsert").modal('hide');

    },
    InsertBoxFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertBox: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Box/InsertBox";
        var BoxName = AppUtil.GetIdValue("BoxName");
        var BoxLocation = AppUtil.GetIdValue("BoxLocation");
        var LatitudeLongitude = AppUtil.GetIdValue("LatitudeLongitude");


   //     setTimeout(function () {
        var Box = { BoxName: BoxName, BoxLocation: BoxLocation, LatitudeLongitude:LatitudeLongitude};

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Box_Client: Box });
            AppUtil.MakeAjaxCall(url, "POST", datas, BoxManager.InsertBoxSuccess, BoxManager.InsertBoxUpFail);
      //  }, 500);
    },
    InsertBoxSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Box) {
            // 
            // var Box = (data.Box);
            // $("#tblBox>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Box.BoxID + '/></td><td>' + Box.BoxName + '</td><td><a href="" id="showBoxForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Box Already Added. Choose different Box Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Box/Index";
        $("#mdlBoxInsert").modal('hide');

    },
    InsertBoxUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowBoxDetailsByIDForUpdate: function (BoxID) {

        
        //AppUtil.ShowWaitingDialog();
    //    setTimeout(function () {
            
            var url = "/Box/GetBoxDetailsByID/";
            var data = { BoxID: BoxID };
            data = BoxManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BoxManager.ShowBoxDetailsByIDForUpdateSuccess, BoxManager.ShowBoxDetailsByIDForUpdateError);

     //   }, 500);

    },
    ShowBoxDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var BoxDetailsJsonParse = (data.BoxDetails);
        $("#ddlUpdateReseller").val(BoxDetailsJsonParse.ResellerID);
        $("#BoxNames").val(BoxDetailsJsonParse.BoxName);
        $("#BoxLocations").val(BoxDetailsJsonParse.BoxLocations);
        $("#NewLatitudeLongitude").val(BoxDetailsJsonParse.LatitudeLongitude);


        $("#mdlBoxUpdate").modal("show");
    },
    ShowBoxDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateBoxInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var BoxID = BoxID;
        var ResellerID = $("#ddlUpdateReseller").val();
        var BoxName = $("#BoxNames").val();
        var BoxLocation = $("#BoxLocations").val();
        var LatitudeLongitude = $("#NewLatitudeLongitude").val();


        var url = "/Box/UpdateBox";
        var BoxInfomation = ({ BoxID: BoxID, BoxName: BoxName, BoxLocation: BoxLocation, ResellerID: ResellerID, LatitudeLongitude: LatitudeLongitude });
        var data = JSON.stringify({ BoxInfoForUpdate: BoxInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, BoxManager.UpdateBoxInformationSuccess, BoxManager.UpdateBoxInformationFail);
    },
    UpdateBoxInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            //var BoxInformation = (data.BoxUpdateInformation);

            //$("#tblBox tbody>tr").each(function () {
                
            //    var BoxID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    if (BoxInformation[0].BoxID == BoxID) {
                    
            //        $('#tblBox tbody>tr:eq(' + index + ')').find("td:eq(1)").text(BoxInformation[0].PackageName);
            //        $('#tblBox tbody>tr:eq(' + index + ')').find("td:eq(2)").text(BoxInformation[0].BoxLocation); 
            //    }
            //});
            table.draw();
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Box Already Added. Choose different Box. ");
            }
        }

        $("#mdlBoxUpdate").modal('hide');

        BoxManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateBoxInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintBoxList: function () {
        
        var url = "/Excel/CreateReportForBoxList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = BoxManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BoxManager.PrintBoxListSuccess, BoxManager.PrintBoxListFail);
    },
    PrintBoxListSuccess: function (data) {
        
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
    PrintBoxListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#BoxName").val("");
        $("#BoxLocation").val("");
        $("#LatitudeLongitude").val("");
    },
    clearForUpdateInformation: function () {
        $("#BoxNames").val("");
        $("#BoxLocations").val("");
        $("#NewLatitudeLongitude").val("");
    }
}