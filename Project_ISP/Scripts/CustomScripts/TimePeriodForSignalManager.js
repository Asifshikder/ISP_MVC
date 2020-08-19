var TimePeriodForSignalManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    

    PrintTimePeriodForSignalList: function () {
        
        var url = "/Excel/CreateReportForTimePeriodForSignalList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var SearchByTimePeriodForSignalTypeID = AppUtil.GetIdValue("SearchByTimePeriodForSignalTypeID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({ TimePeriodForSignalTypeID: SearchByTimePeriodForSignalTypeID });
        data = TimePeriodForSignalManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TimePeriodForSignalManager.PrintTimePeriodForSignalListSuccess, TimePeriodForSignalManager.PrintTimePeriodForSignalListFail);
    },
    PrintTimePeriodForSignalListSuccess: function (data) {
        
        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;

        //if (data.Success === true) {
        //    AppUtil.ShowSuccess("TimePeriodForSignal List Print Successfully.");
        //}
        //if (data.Success === false) {
        //    AppUtil.ShowSuccess("Failed To Print TimePeriodForSignal List.");
        //}
    },
    PrintTimePeriodForSignalListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("UpToHours") === '') {
            AppUtil.ShowSuccess("Please Add Hours.");
            return false;
        }

        var radioButtonGroupValuelength = $('input[name=SignalSign]:checked').length;
        if (radioButtonGroupValuelength === 0) {
            AppUtil.ShowSuccess("Please check a Sign.");
            return false;
        }
        //alert(radioButtonGroupValue +" "+ radioButtonGroupValue.length);
        return true;
    },

    UpdateValidation: function () {
        

        if (AppUtil.GetIdValue("UpToHourss") === '') {
            AppUtil.ShowSuccess("Please Add Hours.");
            return false;
        }
        if (!$("input:radio[name='SignalSigns']").is(":checked")) {
            AppUtil.ShowSuccess("Please check a Sign.");
            return false;
        }
       // var radioButtonGroupValue = $("input[name=SignalSigns]:checked").val();
       //// alert(radioButtonGroupValue + " " + radioButtonGroupValue.length);
        return true;
        return true;
    },

    InsertTimePeriodForSignalFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/TimePeriodForSignal/InsertTimePeriodForSignalFromPopUp";
        var UpToHours = AppUtil.GetIdValue("UpToHours");
        var SignalSign = $("input[name=SignalSign]:checked").val();

        //  setTimeout(function () {
        var TimePeriodForSignal = {
            SignalSign:SignalSign,UpToHours: UpToHours
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ TimePeriodForSignal_Client: TimePeriodForSignal });
        AppUtil.MakeAjaxCall(url, "POST", datas, TimePeriodForSignalManager.InsertTimePeriodForSignalFromPopUpSuccess, TimePeriodForSignalManager.InsertTimePeriodForSignalFromPopUpFail);
        // }, 500);
    },
    InsertTimePeriodForSignalFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.reload();
           // if (data.TimePeriodForSignal) {
              //  
                //var TimePeriodForSignal = (data.TimePeriodForSignal);

                //var WarrentyEndDate = TimePeriodForSignal.WarrentyEndDate != null ? AppUtil.ParseDateTime(TimePeriodForSignal.WarrentyEndDate) : "";
                //var PurchaseDate = TimePeriodForSignal.PurchaseDate != null ? AppUtil.ParseDateTime(TimePeriodForSignal.PurchaseDate) : "";
                //var WarrentyStartDate = TimePeriodForSignal.WarrentyStartDate != null ? AppUtil.ParseDateTime(TimePeriodForSignal.WarrentyStartDate) : "";
                //$("#tblTimePeriodForSignal>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + TimePeriodForSignal.TimePeriodForSignalID + '></td><td>' + TimePeriodForSignal.TimePeriodForSignalTypeName + '</td><td>' + TimePeriodForSignal.TimePeriodForSignalName + '</td><td>' + TimePeriodForSignal.TimePeriodForSignalValue + '</td><td>' + PurchaseDate + '</td><td>' + TimePeriodForSignal.SerialNumber + '</td><td>' + WarrentyStartDate + '</td><td>' + WarrentyEndDate + '</td><td> <button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"> <span class="glyphicon glyphicon-remove"></span> </button> <button id="btnUpdate" type="button" class="btn btn-success btn-sm padding" data-placement="top" data-toggle="modal" > <span class="glyphicon glyphicon-pencil"></span> </button>    </td></tr> ');
            //}
        }
        if (data.SuccessInsert === false) {
            
            if (data.SuccessInsert === false) {
                AppUtil.ShowSuccess("Sorry This type of signal sign is already added. Please choose different one.");
            }
            else {
            AppUtil.ShowSuccess("Save Failed.");
            }
        }
        //  window.location.href = "/TimePeriodForSignal/Index";
        TimePeriodForSignalManager.ClearForSaveInformation();
        $("#mdlTimePeriodForSignalInsert").modal('hide');

    },
    InsertTimePeriodForSignalFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertTimePeriodForSignal: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/TimePeriodForSignal/InsertTimePeriodForSignal";
        var TimePeriodForSignalType = AppUtil.GetIdValue("lstTimePeriodForSignalType");
        var TimePeriodForSignalName = AppUtil.GetIdValue("TimePeriodForSignalName");
        var TimePeriodForSignalValue = AppUtil.GetIdValue("TimePeriodForSignalValue");
        var PurchaseDate = $("#PurchaseDate").datepicker("getDate");
        var SerialNumber = AppUtil.GetIdValue("SerialNumber");
        var WarrentyStartDate = AppUtil.getDateTime("WarrentyStartDate");
        var WarrentyEndDate = AppUtil.getDateTime("WarrentyEndDate");

        //setTimeout(function () {
        var TimePeriodForSignal = {
            TimePeriodForSignalTypeID: TimePeriodForSignalType, TimePeriodForSignalName: TimePeriodForSignalName, TimePeriodForSignalValue: TimePeriodForSignalValue, PurchaseDate: PurchaseDate, SerialNumber: SerialNumber,
            WarrentyStartDate: WarrentyStartDate, WarrentyEndDate: WarrentyEndDate
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ TimePeriodForSignal_Client: TimePeriodForSignal });
        AppUtil.MakeAjaxCall(url, "POST", datas, TimePeriodForSignalManager.InsertTimePeriodForSignalSuccess, TimePeriodForSignalManager.InsertTimePeriodForSignalUpFail);
        // }, 500);
    },
    InsertTimePeriodForSignalSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.TimePeriodForSignal) {
            // 
            // var TimePeriodForSignal = (data.TimePeriodForSignal);
            // $("#tblTimePeriodForSignal>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + TimePeriodForSignal.TimePeriodForSignalID + '/></td><td>' + TimePeriodForSignal.TimePeriodForSignalName + '</td><td><a href="" id="showTimePeriodForSignalForUpdate">Show</a></td></tr>');
            // }
            TimePeriodForSignalManager.ClearForSaveInformation();
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("TimePeriodForSignal Already Added. Choose different TimePeriodForSignal Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/TimePeriodForSignal/Index";
        $("#mdlTimePeriodForSignalInsert").modal('hide');

    },
    InsertTimePeriodForSignalUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowTimePeriodForSignalDetailsByIDForUpdate: function (TimePeriodForSignalID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/TimePeriodForSignal/GetTimePeriodForSignalDetailsByID/";
        var data = { TimePeriodForSignalID: TimePeriodForSignalID };
        data = TimePeriodForSignalManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TimePeriodForSignalManager.ShowTimePeriodForSignalDetailsByIDForUpdateSuccess, TimePeriodForSignalManager.ShowTimePeriodForSignalDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowTimePeriodForSignalDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var TimePeriodForSignalDetailsJsonParse = (data.TimePeriodForSignalDetails);
        $("#UpToHourss").val(TimePeriodForSignalDetailsJsonParse.UpToHours);
        $("input[name=SignalSigns][value=" + TimePeriodForSignalDetailsJsonParse.SignalSign+ "]").prop("checked", true);
        

        $("#mdlTimePeriodForSignalUpdate").modal("show");
    },
    ShowTimePeriodForSignalDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateTimePeriodForSignalInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var UpToHours = $("#UpToHourss").val();
        var SignalSign = $("input[name=SignalSigns]:checked").val() ;
       
        var url = "/TimePeriodForSignal/UpdateTimePeriodForSignal";
        var TimePeriodForSignalInfomation = ({
            TimePeriodForSignalID: TimePeriodForSignalID, UpToHours: UpToHours, SignalSign: SignalSign
        });
        var data = JSON.stringify({ TimePeriodForSignalInfoForUpdate: TimePeriodForSignalInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, TimePeriodForSignalManager.UpdateTimePeriodForSignalInformationSuccess, TimePeriodForSignalManager.UpdateTimePeriodForSignalInformationFail);
    },
    UpdateTimePeriodForSignalInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            window.location.reload();
            //var TimePeriodForSignalInformation = (data.TimePeriodForSignalUpdateInformation);

            //$("#tblTimePeriodForSignal tbody>tr").each(function () {
            //    
            //    var TimePeriodForSignalID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    if (TimePeriodForSignalInformation[0].TimePeriodForSignalID == TimePeriodForSignalID) {
            //        
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(1)").text(TimePeriodForSignalInformation[0].TimePeriodForSignalTypeName);
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(2)").text(TimePeriodForSignalInformation[0].TimePeriodForSignalName);
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(3)").text(TimePeriodForSignalInformation[0].TimePeriodForSignalValue);
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(4)").text(TimePeriodForSignalInformation[0].PurchaseDate != null ? AppUtil.ParseDateTime(TimePeriodForSignalInformation[0].PurchaseDate) : "");
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(5)").text(TimePeriodForSignalInformation[0].SerialNumber);
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(6)").text(TimePeriodForSignalInformation[0].WarrentyStartDate != null ? AppUtil.ParseDateTime(TimePeriodForSignalInformation[0].WarrentyStartDate) : "");
            //        $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').find("td:eq(7)").text(TimePeriodForSignalInformation[0].WarrentyEndDate != null ? AppUtil.ParseDateTime(TimePeriodForSignalInformation[0].WarrentyEndDate) : "");

            //    }
            //});
            //AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            // if (AlreadyInsert = true) {
            if (data.DuplicateSignExist) {
                AppUtil.ShowSuccess("Sorry Change the sign. It is already exist. ");
            }
            else {
                AppUtil.ShowSuccess("Time Period For Signal can not update. Please contact with administrator. ");
            }
            // }
        }

        $("#mdlTimePeriodForSignalUpdate").modal('hide');

        TimePeriodForSignalManager.ClearForUpdateInformation();
        console.log(data);
    },
    UpdateTimePeriodForSignalInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },
    
    DeleteTimePeriodForSignal: function () {

        
        var url = "/TimePeriodForSignal/DeleteTimePeriodForSignal/";

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = ({ TimePeriodForSignalID: TimePeriodForSignalID });
        datas = TimePeriodForSignalManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, TimePeriodForSignalManager.DeleteTimePeriodForSignalSuccess, TimePeriodForSignalManager.DeleteTimePeriodForSignalFail);
        // }, 50);
    },
    DeleteTimePeriodForSignalSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();

        if (data.DeleteStatus === true) {
            $("#tblTimePeriodForSignal>tbody>tr").each(function () {
                

                var index = $(this).index();
                var TimePeriodForSignalID = $(this).find("td:eq(0) input").val();
                if (TimePeriodForSignalID == data.TimePeriodForSignalID) {
                    
                    $('#tblTimePeriodForSignal tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
        }

        if (data.DeleteStatus === false) {
            AppUtil.ShowSuccess("Some Information Can not removed.");
        }


        console.log(data);
    },
    DeleteTimePeriodForSignalFail: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },
    
    ClearForSaveInformation: function () {
        $("#UpToHours").val("");
    },
    ClearForUpdateInformation: function () { 
        $("#UpToHourss").val("");
    }
}