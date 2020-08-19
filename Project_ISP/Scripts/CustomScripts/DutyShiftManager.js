var DutyShiftManager = {

    Validation: function () {

        if (AppUtil.GetIdValue("StartHour") === '') {
            AppUtil.ShowSuccess("Please Select Start Hour.");
            return false;
        }
        if (AppUtil.GetIdValue("StartMinute") === '') {
            AppUtil.ShowSuccess("Please Select Start Minute.");
            return false;
        }
        if (AppUtil.GetIdValue("EndHour") === '') {
            AppUtil.ShowSuccess("Please Select End Hour.");
            return false;
        }
        if (AppUtil.GetIdValue("EndMinute") === '') {
            AppUtil.ShowSuccess("Please Select End Minute.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreateStartHour") === '') {
            AppUtil.ShowSuccess("Please Select Start Hour.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateStartMinute") === '') {
            AppUtil.ShowSuccess("Please Select Start Minute.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateEndHour") === '') {
            AppUtil.ShowSuccess("Please Select End Hour.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateEndMinute") === '') {
            AppUtil.ShowSuccess("Please Select End Minute.");
            return false;
        }
        return true;
    },

   

    InsertDutyShiftFromPopUp: function () {
        var url = "/DutyShift/InsertDutyShift/";
        var StartHour = AppUtil.GetIdValue("StartHour");
        var StartMinute = AppUtil.GetIdValue("StartMinute");
        var EndHour = AppUtil.GetIdValue("EndHour");
        var EndMinute = AppUtil.GetIdValue("EndMinute");

        var ShiftForInsert = { StartHour: StartHour, StartMinute: StartMinute, EndHour: EndHour, EndMinute: EndMinute };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = JSON.stringify({ ShiftForInsert: ShiftForInsert });
        //data = DutyShiftManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCall(url, "POST", data, DutyShiftManager.InsertDutyShiftFromPopUpSuccess, DutyShiftManager.InsertDutyShiftFromPopUpFail);
  

    },
    InsertDutyShiftFromPopUpSuccess: function (data) {
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            var ShiftInformation = (data.ShiftInformation);
            $('#tblDutyShift>tbody>tr:last').after('<tr><td hidden><input type="hidden" id="" value=' + ShiftInformation.DutyShiftID + '></td><td>' + ShiftInformation.StartHour + ' </td><td>' + ShiftInformation.StartMinute + ' </td><td>' + ShiftInformation.EndHour + '</td><td>' + ShiftInformation.EndMinute + '</td><td><a class="glyphicon glyphicon-edit btn btn-primary" value=' + ShiftInformation.DutyShiftID + '  onclick="ShowForEdit(' + ShiftInformation.DutyShiftID + ')"></a> <a class="glyphicon glyphicon-remove btn btn-danger" value=' + ShiftInformation.DutyShiftID + '  onclick="ShowForDelete(' + ShiftInformation.DutyShiftID +')"></a></td></tr>');
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }

        DutyShiftManager.clearForSaveInformation();
    },
    InsertDutyShiftFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowDutyShiftDetailsByIDForUpdate: function (DutyDhiftID) {
        var url = "/DutyShift/GetDutyShiftDetailsByID/";

        var data = JSON.stringify({ DutyShiftID: DutyDhiftID });
        AppUtil.MakeAjaxCall(url, "POST", data, DutyShiftManager.ShowDutyShiftDetailsByIDForUpdateSuccess, DutyShiftManager.ShowDutyShiftDetailsByIDForUpdateError);

    },
    ShowDutyShiftDetailsByIDForUpdateSuccess: function (data) {
        if (data.success === true) {
            var PackageJSONParse = data.DutyShiftDetails;

            $("#DutyShiftID").val(PackageJSONParse.DutyShiftID);
            $("#StartHourUpdate").val(PackageJSONParse.StartHour);
            $("#StartMinuteUpdate").val(PackageJSONParse.StartMinute);
            $("#EndHourUpdate").val(PackageJSONParse.EndHour);
            $("#EndMinuteUpdate").val(PackageJSONParse.EndMinute);

            $("#mdlDutyShiftUpdate").modal("show");
        }
        
    },
    ShowDutyShiftDetailsByIDForUpdateError: function (data) {
        AppUtil.ShowError("Unable to Update. Please Contact with Administrator!")
    },

    UpdateDutyShiftInformation: function () {

        var DutyShiftID = $("#DutyShiftID").val();
        var StartHour = $("#StartHourUpdate").val();
        var StartMinute = $("#StartMinuteUpdate").val();
        var EndHour = $("#EndHourUpdate").val();
        var EndMinute = $("#EndMinuteUpdate").val();

        var url = "/DutyShift/UpdateDutyShift";
        var DutyShiftForUpdate =
            ({ DutyDhiftID: DutyShiftID, StartHour: StartHour, StartMinute: StartMinute, EndHour: EndHour, EndMinute: EndMinute });
        var data = JSON.stringify({ DutyShiftForUpdate: DutyShiftForUpdate });
        AppUtil.MakeAjaxCall(url, "POST", data, DutyShiftManager.UpdateDutyShiftInformationSuccess, DutyShiftManager.UpdateDutyShiftInformationFail);
    },
    UpdateDutyShiftInformationSuccess: function (data) {

        

        if (data.UpdateSuccess === true) {
            var Shift = (data.Shift);

            $('#tblDutyShift tbody>tr:eq(' + rowIndex + ')').find("td:eq(1)").text(Shift.StartHour);
            $('#tblDutyShift tbody>tr:eq(' + rowIndex + ')').find("td:eq(2)").text(Shift.StartMinute);
            $('#tblDutyShift tbody>tr:eq(' + rowIndex + ')').find("td:eq(3)").text(Shift.EndHour);
            $('#tblDutyShift tbody>tr:eq(' + rowIndex + ')').find("td:eq(4)").text(Shift.EndMinute);
            AppUtil.ShowSuccess("Successfully Edited!");
        }
        DutyShiftManager.clearForUpdateInformation();
        $("#mdlDutyShiftUpdate").modal('hide');
    },
    UpdateDutyShiftInformationFail: function () {
        console.log(data);
    },

    ShowDutyShiftForDelete: function (DutyDhiftID) {
        var url = "/DutyShift/GetDutyShiftDetailsByID/";
        var data = JSON.stringify({ DutyShiftID: DutyDhiftID });

        AppUtil.MakeAjaxCall(url, "POST", data, DutyShiftManager.ShowDutyShiftForDeleteSuccess, DutyShiftManager.ShowDutyShiftForDeleteError);


    },
    ShowDutyShiftForDeleteSuccess: function (data) {

        if (data.success === true) {
            var PackageJSONParse = data.DutyShiftDetails;

            $("#DutyShiftIDDelete").val(PackageJSONParse.DutyShiftID);


            $("#mdlDutyShiftDelete").modal("show");
        }

    },
    ShowDutyShiftForDeleteError: function (data) {
        AppUtil.ShowError("Unable to Update. Please Contact with Administrator!")
    },

    DeleteDutyShiftInformation: function () {
        var DutyShiftID = $("#DutyShiftIDDelete").val();
        

        var url = "/DutyShift/DeleteDutyShift";
        var data = JSON.stringify({ id: DutyShiftID });
        AppUtil.MakeAjaxCall(url, "POST", data, DutyShiftManager.DeleteDutyShiftInformationSuccess, DutyShiftManager.DeleteDutyShiftInformationFail);
    },
    DeleteDutyShiftInformationSuccess: function (data) {

        if (data.DeleteSuccess === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");

            $('#tblDutyShift tbody>tr:eq(' + rowIndex + ')').remove();
            $("#mdlDutyShiftDelete").modal('hide');
        }
        else {
            AppUtil.ShowError("Error Occured!");
            $("#mdlDutyShiftUpdate").modal('hide');
        }
       
    },
    DeleteDutyShiftInformationFail: function () {
        AppUtil.ShowError("Failed to delete")
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


 

    clearForSaveInformation: function () {
        $("#CreateStartHour").val("");
        $("#CreateStartMinute").val("");
        $("#CreateEndHour").val("");
        $("#CreateEndMinute").val("");
    },
    clearForUpdateInformation: function () {
        $("#StartHour").val("");
        $("#StartMinute").val("");
        $("#EndHour").val("");
        $("#EndMinute").val("");
    }
}