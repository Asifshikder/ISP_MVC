var PopManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("PopName") === '') {
            AppUtil.ShowSuccess("Please Insert Pop ");
            return false;
        }
        if (AppUtil.GetIdValue("PopLocation") === '') {
            AppUtil.ShowSuccess("Please Insert Pop Location ");
            return false;
        }
        if (AppUtil.GetIdValue("LatitudeLongitude") === '') {
            AppUtil.ShowSuccess("Please Insert Latitude & Longitude ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("PopNames") === '') {
            AppUtil.ShowSuccess("Please Insert Pop ");
            return false;
        }
        if (AppUtil.GetIdValue("PopLocations") === '') {
            AppUtil.ShowSuccess("Please Insert Pop Location ");
            return false;
        }
        if (AppUtil.GetIdValue("LatitudeLongitudes") === '') {
            AppUtil.ShowSuccess("Please Insert Latitude and longitude ");
            return false;
        }
        return true;
    },

    InsertPopFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Pop/InsertPopFromPopUp";
        var PopName = AppUtil.GetIdValue("PopName");
        var PopLocation = AppUtil.GetIdValue("PopLocation");
        var LatitudeLongitude = AppUtil.GetIdValue("LatitudeLongitude");


      //  setTimeout(function () {
        var Pop = { PopName: PopName, PopLocation: PopLocation, LatitudeLongitude: LatitudeLongitude };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Pop_Client: Pop });
            AppUtil.MakeAjaxCall(url, "POST", datas, PopManager.InsertPopFromPopUpSuccess, PopManager.InsertPopFromPopUpFail);
      //  }, 500);
    },
    InsertPopFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Pop) {
                
                window.location.reload();
                //var Pop = (data.Pop);
                //$("#tblPop>tbody").append('<tr><td style="padding:0px" hidden><input type="hidden" id="" value=' + Pop.PopID + '></td><td>' + Pop.PopName + '</td><td>' + Pop.PopLocation + '</td><td><a href="" id="showPopForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Pop Already Added. Choose different Pop Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Pop/Index";
        PopManager.clearForSaveInformation();
        $("#mdlPopInsert").modal('hide');

    },
    InsertPopFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertPop: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Pop/InsertPop";
        var PopName = AppUtil.GetIdValue("PopName");
        var PopLocation = AppUtil.GetIdValue("PopLocation");
        var LatitudeLongitude = AppUtil.GetIdValue("LatitudeLongitude");


      //  setTimeout(function () {
        var Pop = { PopName: PopName, PopLocation: PopLocation, LatitudeLongitude: LatitudeLongitude };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Pop_Client: Pop });
            AppUtil.MakeAjaxCall(url, "POST", datas, PopManager.InsertPopSuccess, PopManager.InsertPopUpFail);
      //  }, 500);
    },
    InsertPopSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Pop) {
            // 
            // var Pop = (data.Pop);
            // $("#tblPop>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Pop.PopID + '/></td><td>' + Pop.PopName + '</td><td><a href="" id="showPopForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Pop Already Added. Choose different Pop Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Pop/Index";
        $("#mdlPopInsert").modal('hide');

    },
    InsertPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowPopDetailsByIDForUpdate: function (PopID) {

        
        //AppUtil.ShowWaitingDialog();
     //   setTimeout(function () {
            
            var url = "/Pop/GetPopDetailsByID/";
            var data = { PopID: PopID };
            data = PopManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PopManager.ShowPopDetailsByIDForUpdateSuccess, PopManager.ShowPopDetailsByIDForUpdateError);

       // }, 500);

    },
    ShowPopDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var PopDetailsJsonParse = (data.PopDetails);
        $("#PopNames").val(PopDetailsJsonParse.PopName);
        $("#PopLocations").val(PopDetailsJsonParse.PopLocations);
        $("#LatitudeLongitudes").val(PopDetailsJsonParse.LatitudeLongitude);


        $("#mdlPopUpdate").modal("show");
    },
    ShowPopDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdatePopInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var PopID = PopID;
        var PopName = $("#PopNames").val();
        var PopLocation = $("#PopLocations").val();
        var LatitudeLongitude = $("#LatitudeLongitudes").val();


        var url = "/Pop/UpdatePop";
        var PopInfomation = ({ PopID: PopID, PopName: PopName, PopLocation: PopLocation, LatitudeLongitude: LatitudeLongitude});
        var data = JSON.stringify({ PopInfoForUpdate: PopInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, PopManager.UpdatePopInformationSuccess, PopManager.UpdatePopInformationFail);
    },
    UpdatePopInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var PopInformation = (data.PopUpdateInformation);

            $("#tblPop tbody>tr").each(function () {
                
                var PopID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (PopInformation[0].PopID == PopID) {
                    
                    $('#tblPop tbody>tr:eq(' + index + ')').find("td:eq(1)").text(PopInformation[0].PackageName);
                    $('#tblPop tbody>tr:eq(' + index + ')').find("td:eq(2)").text(PopInformation[0].PopLocation);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Pop Already Added. Choose different Pop. ");
            }
        }

        $("#mdlPopUpdate").modal('hide');

        PopManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdatePopInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },



    PrintPopList: function () {
        
        var url = "/Excel/CreateReportForPopList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = PopManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PopManager.PrintPopListSuccess, PopManager.PrintPopListFail);
    },
    PrintPopListSuccess: function (data) {
        
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
    PrintPopListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function() {
        $("#PopName").val("");
        $("#PopLocation").val("");
    },
    clearForUpdateInformation: function() {
        $("#PopNames").val("");
        $("#PopLocations").val("");
    }
}