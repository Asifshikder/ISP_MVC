
var LeaveManager = {

    Validation: function () {

        if (AppUtil.GetIdValue("LeaveTypeName") === '') {
            AppUtil.ShowSuccess("Please Insert Type Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Percent") === '') {
            AppUtil.ShowSuccess("Please Insert Percent.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreateLeaveType") === '') {
            AppUtil.ShowSuccess("Please Insert Leave Type Name.");
            return false;
        }
        if (AppUtil.GetIdValue("CreatePercent") === '') {
            AppUtil.ShowSuccess("Please Insert Percent.");
            return false;
        }
        return true;
    },

    InsertLeaveTypeFromPopUp: function () {

        var url = "/LeaveSalary/InsertLeaveType/";
        var LeaveTypeName = AppUtil.GetIdValue("CreateLeaveType");
        var Persent = AppUtil.GetIdValue("CreatePercent");
        var leaveType = { LeaveTypeName: LeaveTypeName, Persent: Persent };
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = JSON.stringify({ leaveType: leaveType });

        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, LeaveManager.InsertLeaveTypeFromPopUpSuccess, LeaveManager.InsertLeaveTypeFromPopUpFail);



    },
    InsertLeaveTypeFromPopUpSuccess: function (data) {
       
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.TypeInfos) {

                var TypeInfos = (data.TypeInfos);
                $("#tblLeaveType>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + TypeInfos.LeaveTypeId + '></td><td>' + TypeInfos.LeaveTypeName + '</td><td>' + TypeInfos.Persent + '</td><td><a href="" id="ShowUpdateForType">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        LeaveManager.clearForSaveInformation();
        table.draw();
    },
    InsertLeaveTypeFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


    ShowTypeDetailsByIDForUpdate: function (LeaveTypeId) {

        var url = "/LeaveSalary/GetTypeDetailsByID/";
        var data = { TypeID: LeaveTypeId };
        data = LeaveManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, LeaveManager.ShowTypeDetailsByIDForUpdateSuccess, LeaveManager.ShowTypeDetailsByIDForUpdateError);

    },
    ShowTypeDetailsByIDForUpdateSuccess: function (data) {

        var PackageJSONParse = (data.leaveTypeDetails);
        $("#TypeId").val(PackageJSONParse.LeaveTypeId);
        $("#LeaveTypeName").val(PackageJSONParse.LeaveTypeName);
        $("#Percent").val(PackageJSONParse.Persent);

        $("#mdlTypeUpdate").modal("show");
    },
    ShowTypeDetailsByIDForUpdateError: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


    UpdateLeaveTypeInformation: function () {
        var LeaveTypeId = $("#TypeId").val(); 
        var LeaveTypeName = $("#LeaveTypeName").val();
        var Percent = $("#Percent").val();
        var url = "/LeaveSalary/UpdateType";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var TypeInfoForUpdate = ({ LeaveTypeId: LeaveTypeId, LeaveTypeName: LeaveTypeName, Persent: Percent });
        var data = JSON.stringify({ TypeInfoForUpdate: TypeInfoForUpdate });

        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, LeaveManager.UpdateLeaveTypeInformationSuccess, LeaveManager.UpdateLeaveTypeInformationFail);
    },
    UpdateLeaveTypeInformationSuccess: function (data) {

        if (data.UpdateSuccess === true) {
            var TypeUpdateInformation = (data.TypeUpdateInformation);

            $("#tblLeaveType tbody>tr").each(function () {

                var LeaveTypeId = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (TypeUpdateInformation.LeaveTypeId == LeaveTypeId) {

                    $('#tblLeaveType tbody>tr:eq(' + index + ')').find("td:eq(1)").text(TypeUpdateInformation.LeaveTypeName);
                    $('#tblLeaveType tbody>tr:eq(' + index + ')').find("td:eq(2)").text(TypeUpdateInformation.Persent);
                }
            });

        }
        AppUtil.ShowSuccess("Successfully Updated.");
        LeaveManager.clearForUpdateInformation();
        $("#mdlTypeUpdate").modal('hide');
    },
    UpdateLeaveTypeInformationFail: function () {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    clearForSaveInformation: function () {
        $("#CreatePackageName").val("");
        $("#CreatePercent").val("");
    },
    clearForUpdateInformation: function () {
        $("#LeaveTypeName").val("");
        $("#Percent").val("");
    }
}