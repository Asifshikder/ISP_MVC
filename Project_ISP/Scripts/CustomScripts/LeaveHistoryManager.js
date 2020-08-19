var LeaveHistoryManager = {
    Validation: function () {
        if (AppUtil.GetIdValue("EditEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Emplyee.");
            return false;
        }
        if (AppUtil.GetIdValue("EditReason") === '') {
            AppUtil.ShowSuccess("Please Type Reason.");
            return false;
        }
        if (AppUtil.GetIdValue("EditLeaveType") === '') {
            AppUtil.ShowSuccess("Please Select Leave Type.");
            return false;
        }
        if (AppUtil.GetIdValue("EditStartDate") === '') {
            AppUtil.ShowSuccess("Please Provide Start Date.");
            return false;
        }
        if (AppUtil.GetIdValue("EditEndDate") === '') {
            AppUtil.ShowSuccess("Please Provide End Date.");
            return false;
        }
        
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("InsertEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Emplyee.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertReason") === '') {
            AppUtil.ShowSuccess("Please Type Reason.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertLeaveType") === '') {
            AppUtil.ShowSuccess("Please Select Leave Type.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertStartDate") === '') {
            AppUtil.ShowSuccess("Please Provide Start Date.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertEndDate") === '') {
            AppUtil.ShowSuccess("Please Provide End Date.");
            return false;
        }
        return true;
    },

    
    InsertLeaveHistoryFromPopUp: function () {


        var url = "/LeaveSalary/InsertEmployeLeaveHistory/";
        var Employee = AppUtil.GetIdValue("AddEmployeeID");
        var Reason = AppUtil.GetIdValue("InsertReason");
        var LeaveType = AppUtil.GetIdValue("AddLeaveType");
        var StartDate = AppUtil.GetIdValue("InsertStartDate");
        var EndDate = AppUtil.GetIdValue("InsertEndDate");
        var LeaveStory = { EmployeeID: Employee, Reason: Reason, LeaveType: LeaveType, StartDate: StartDate, EndDate: EndDate};

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = JSON.stringify({ LeaveStory: LeaveStory });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, LeaveHistoryManager.InsertLeaveHistoryFromPopUpSuccess, LeaveHistoryManager.InsertLeaveHistoryFromPopUpFail);
        
    },
    InsertLeaveHistoryFromPopUpSuccess: function (data) {

        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            table.draw();
            
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        
    },
    InsertLeaveHistoryFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowLeaveHistoryByIDForUpdate: function (ID) {

        var url = "/LeaveSalary/GetLeaveHistoryByID/";
        var data = { id: ID };
        data = LeaveHistoryManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, LeaveHistoryManager.ShowLeaveHistoryByIDForUpdateSuccess, LeaveHistoryManager.ShowLeaveHistoryByIDForUpdateError);

    },
    ShowLeaveHistoryByIDForUpdateSuccess: function (data) {

        console.log(data);
        var PackageJSONParse = (data.leaveHistoryDetails);
        $("#ID").val(PackageJSONParse.EmployeeLeaveHistoryID);
        $("#EditEmployeeID").val(PackageJSONParse.EmployeeID);
        $("#EditReason").val(PackageJSONParse.reason);
        $("#EditLeaveType").val(PackageJSONParse.LeaveType);
        $("#EditStartDate").val(AppUtil.ParseDateINMMDDYYYY(PackageJSONParse.StartDate));
        $("#EditEndDate").val(AppUtil.ParseDateINMMDDYYYY(PackageJSONParse.EndDate));

        $("#mdlLeaveHistoryUpdate").modal("show");
    },
    ShowLeaveHistoryByIDForUpdateError: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


    UpdateLeaveHistoryInformation: function () {
        var EmployeeLeaveHistoryID = $("#ID").val();
        var EmployeeID = $("#EditEmployeeID").val();
        var Reason = $("#EditReason").val();
        var LeaveType = $("#EditLeaveType").val();
        var StartDate = $("#EditStartDate").val();
        var EndDate = $("#EditEndDate").val();
        var url = "/LeaveSalary/UpdateLeaveHistoryType";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var LeaveHistoryIformation =
            ({ EmployeeLeaveHistoryID: EmployeeLeaveHistoryID, EmployeeID: EmployeeID, Reason: Reason, LeaveType: LeaveType, StartDate: StartDate, EndDate: EndDate });
        var data = JSON.stringify({ LeaveHistoryIformation: LeaveHistoryIformation });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, LeaveHistoryManager.UpdateLeaveHistoryInformationSuccess, LeaveHistoryManager.UpdateLeaveHistoryInformationFail);
    },
    UpdateLeaveHistoryInformationSuccess: function (data) {

        if (data.UpdateSuccess === true) {
            var EMPLH = (data.EMPLH);

            $("#tblEmployeeLeaveHistory tbody>tr").each(function () {

                var ID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (EMPLH.ID == ID) {

                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').find("td:eq(1)").text(EMPLH.LoginName);
                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').find("td:eq(2)").text(EMPLH.Reason);
                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').find("td:eq(3)").text(EMPLH.LeaveTypeName);
                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').find("td:eq(4)").text(AppUtil.ParseDateINMMDDYYYY(EMPLH.StartDate));
                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').find("td:eq(5)").text(AppUtil.ParseDateINMMDDYYYY(EMPLH.EndDate));
                }
            });

        }

        LeaveHistoryManager.clearForUpdateInformation();
        $("#mdlLeaveHistoryUpdate").modal('hide');
    },
    UpdateLeaveHistoryInformationFail: function () {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    DeleteLeaveHistory: function (id) {


        var url = "/LeaveSalary/DelecteLeaveHistory/";

        var Id = ({ id: ID });
        data = LeaveHistoryManager.addRequestVerificationToken(Id);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, LeaveHistoryManager.DeleteLeaveHistorySuccess, LeaveHistoryManager.DeleteLeaveHistoryFail);
        // }, 50);
    },
    DeleteLeaveHistorySuccess: function (data) {
        if (data.success === true) {
            $("#tblEmployeeLeaveHistory>tbody>tr").each(function () {


                var index = $(this).index();
                var ID = $(this).find("td:eq(0) input").val();
                if (ID == data.leaveHistoryID) {

                    $('#tblEmployeeLeaveHistory tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
        }

            else {
                AppUtil.ShowSuccess("Some Information Can not removed.");
            }
        


        console.log(data);
    },
    DeleteLeaveHistoryFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    LeaveHistorySearchValidation: function () {
        if (AppUtil.GetIdValue("EmployeeID") === '' && AppUtil.GetIdValue("StartDate") === '' && AppUtil.GetIdValue("EndDate") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        return true;

    },

    clearForSaveInformation: function () {
        $("#AddEmployeeID").val("");
        $("#InsertReason").val("");
        $("#AddLeaveType").val("");
        $("#InsertStartDate").val("");
        $("#InsertEndDate").val("");
    },
    clearForUpdateInformation: function () {
        $("#EditEmployeeID").val("");
        $("#EditReason").val("");
        $("#EditLeaveType").val("");
        $("#EditStartDate").val("");
        $("#EditEndDate").val("");
    }
}