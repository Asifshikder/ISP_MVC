var AccountManager = {
    ValidationForInserUserRightPermissionName: function () {

        if (AppUtil.GetIdValue("txtUserRightName") === '') {
            alert("Please Add User Right Name.");
            return false;
        }

        return true;

    },
    Validation: function () {

        if (AppUtil.GetIdValue("lstCourse") === '') {
            alert("Please Select Course.");
            return false;
        }
        if (AppUtil.GetIdValue("lstBatch") === '') {
            alert("Please Select Batch.");
            return false;
        }
        if (AppUtil.GetIdValue("lstSubject") === '') {
            alert("Please Select Subject.");
            return false;
        }
        return true;

    },
    ValidationForLockOrActiveOrDeleteEmployee: function () {
        if (AppUtil.GetIdValue("lstEmployeeID") === '') {
            alert("Please Select Employee.");
            return false;
        }

        return true;
    },
    AssignPermissionValidation: function () {
        if (AppUtil.GetIdValue("lstEmployeeIDForSetUserPermission") === '') {
            alert("Please Select Employee.");
            return false;
        }
        if (AppUtil.GetIdValue("UserRightID") === '') {
            alert("Please Select User Permission.");
            return false;
        }
        return true;
    },

    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    GetUserPermissionDetailsByUserRightID: function (UserRightID) {
        
        var url = "/Account/GetPermissionDetailsByUserRightID/";
        var data = ({ UserRightID: UserRightID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.GetUserPermissionDetailsByUserRightIDSuccess, AccountManager.GetUserPermissionDetailsByUserRightIDFailed);
    },
    GetUserPermissionDetailsByUserRightIDSuccess: function (data) {
        
        console.log(data);
        $('input:checkbox').prop('checked', false);
        if (data.Success === true) {
            AppUtil.ShowSuccess("s");
            $.each(data.PermissionList, function (index, item) {
                
                $("#" + item + "").prop("checked", true);
            });
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("f");
        }
    },
    GetUserPermissionDetailsByUserRightIDFailed: function (data) {
        
        alert("Fail");
        console.log(data);
    },

    AddUserRightName: function (userRightName) {
        var url = "/Account/AddUserRightName/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({ UserRightName: userRightName });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.AddUserRightNameSuccess, AccountManager.AddUserRightNameError);
    },
    AddUserRightNameSuccess: function (data) {
        
        $("#mdlUserRightInsert").modal("hide");
        $("input:checkbox").prop("checked", false);
        if (data.Exist === true) {
            AppUtil.ShowSuccess("Sorry User Right Name Already Exist.");
        }

        if (data.Success === true) {
            AppUtil.ShowSuccess("S");
            $("#UserRightID").find("option").not(":first").remove();

            $.each(data.lstUserRight, function (index, item) {
                //$("#lstBatch").append($("<option></option>").val(item.BatchID).text(item.BatchName));
                $("#UserRightID").append($("<option></option)").val(item.UserRightPermissionID).text(item.UserRightPermissionName));
            });
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Sorry User Right Name Can Not Add.");
        }
    },
    AddUserRightNameError: function (data) {
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");

    },

    DeleteUserRightPermissionByID: function (userRightPermissionID) {
        
        var url = "/Account/DeletePermission/";
        var data = ({ UserRightPermissionID: userRightPermissionID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.DeleteUserRightPermissionByIDSuccess, AccountManager.DeleteUserRightPermissionByIDError)

        // alert(ComplainID);
    },
    DeleteUserRightPermissionByIDSuccess: function (data) {
        
        if (data.Success === true) {

            AppUtil.ShowError("Success.");
            $("#tblUserRightList>tbody>tr").each(function (i, item) {
                
                var index = $(this).index();
                var val = $(this).find("td:eq(0) input").val();
                if (val == data.UserRightPermissionID) {
                    $('#tblUserRightList>tbody>tr:eq(' + index + ')').remove();
                }

            });
            $("#UserRightID option[value = " + data.UserRightPermissionID + "]").remove();
        }
        if (data.Success === false) {

            if (data.UserRightUsed === true) {
                AppUtil.ShowError("Sorry User Right in use. Remove Permission Failed.");
            }

            else {
                AppUtil.ShowError("Remove Permission Failed.");
            }

        }

    },
    DeleteUserRightPermissionByIDError: function (data) {
        
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },

    UpdateEmployeeStatus: function (employeeID, statusID) {
        
        var url = "/Account/UpdateEmployeeStatus/";
        var data = ({ EmployeeID: employeeID, EmployeeStatusID: statusID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.UpdateEmployeeStatusSuccess, AccountManager.UpdateEmployeeStatusError)

        // alert(ComplainID);
    },
    UpdateEmployeeStatusSuccess: function (data) {
        
        if (data.Success === true) {
            AppUtil.ShowSuccess("Success.");
            employeeStatus = data.EmployeeStatusID;
            //$("#lstEmployeeID").prop("selectedIndex", 0);
            if (data.EmployeeStatusID == 1) {
                $("#chkEployeeLockOrActiveTemporary").val("Unlock");
            }
            if (data.EmployeeStatusID == 2) {
                $("#chkEployeeLockOrActiveTemporary").val("Lock");
            }
            //$("#tblUserRightList>tbody>tr").each(function (i, item) {
            //    
            //    var index = $(this).index();
            //    var val = $(this).find("td:eq(0) input").val();
            //    if (val == data.UserRightPermissionID) {
            //        $('#tblUserRightList>tbody>tr:eq(' + index + ')').remove();
            //    }

            //});
        }
        if (data.Success === false) {
            
            AppUtil.ShowError("Status Change Failed.");
        }

    },
    UpdateEmployeeStatusError: function (data) {
        
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },

    GetEmployeeStatusByEmployeeID: function (empoyeeID) {
        
        var url = "/Account/GetEmployeeStatusByEmployeeID/";
        var data = ({ EmployeeID: empoyeeID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.GetEmployeeStatusByEmployeeIDSuccess, AccountManager.GetEmployeeStatusByEmployeeIDFailed);
    },
    GetEmployeeStatusByEmployeeIDSuccess: function (data) {
        
        console.log(data);

        if (data.Success === true) {
            AppUtil.ShowSuccess("s");
            employeeStatus = data.EmployeeStatusID;
            if (data.EmployeeStatusID == 1) {
                //   $("#chkEployeeLockOrActiveTemporary").prop("disabled", false);
                $("#chkEployeeLockOrActiveTemporary").val("Unlock");
            }
            else if (data.EmployeeStatusID == 2) {

                $("#chkEployeeLockOrActiveTemporary").val("lock");
                //  $("#chkEployeeLockOrActiveTemporary").prop("disabled", true);
            }
            else {
                //  $("#chkEployeeLockOrActiveTemporary").prop("disabled", true);
            }
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("f");
        }
    },
    GetEmployeeStatusByEmployeeIDFailed: function (data) {
        
        alert("Fail");
        console.log(data);
    },

    DeletEmployeeByEmployeeID: function (employeeID) {
        
        var url = "/Account/DeleteEmployeeByEmployeeID/";
        var data = ({ EmployeeID: employeeID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.DeletEmployeeByEmployeeIDSuccess, AccountManager.DeletEmployeeByEmployeeIDError)

        // alert(ComplainID);
    },
    DeletEmployeeByEmployeeIDSuccess: function (data) {
        
        $("#popModalForDeletePermently").modal("hide");
        if (data.Success === true) {
            AppUtil.ShowError("Success.");

            $("#lstEmployeeID option[value=" + data.EmployeeID + "]").remove();
            $("#lstEmployeeIDForSetUserPermission option[value=" + data.EmployeeID + "]").remove();

            //$("#tblUserRightList>tbody>tr").each(function (i, item) {
            //    
            //    var index = $(this).index();
            //    var val = $(this).find("td:eq(0) input").val();
            //    if (val == data.UserRightPermissionID) {
            //        $('#tblUserRightList>tbody>tr:eq(' + index + ')').remove();
            //    }

            //});
        }
        if (data.Success === false) {
            
            AppUtil.ShowError("Remove Employee Failed.");
        }

    },
    DeletEmployeeByEmployeeIDError: function (data) {
        
        console.log(data);
        $("#popModalForDeletePermently").modal("hide");
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },

    SetPermissionForEmployee: function (employeeID, userRightID) {
        
        var url = "/Account/UpdateEmployeePermission/";
        var data = ({ EmployeeID: employeeID, UserRightID: userRightID });
        data = AccountManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountManager.SetPermissionForEmployeeSuccess, AccountManager.SetPermissionForEmployeeError)

        // alert(ComplainID);
    },
    SetPermissionForEmployeeSuccess: function (data) {
        
        if (data.Success === true) {
            AppUtil.ShowSuccess("Successfully Permission set.");
            //employeeStatus = data.EmployeeStatusID;
            ////$("#lstEmployeeID").prop("selectedIndex", 0);
            //if (data.EmployeeStatusID == 1) {
            //    $("#chkEployeeLockOrActiveTemporary").val("Unlock");
            //}
            //if (data.EmployeeStatusID == 2) {
            //    $("#chkEployeeLockOrActiveTemporary").val("Lock");
            //}
            //$("#tblUserRightList>tbody>tr").each(function (i, item) {
            //    
            //    var index = $(this).index();
            //    var val = $(this).find("td:eq(0) input").val();
            //    if (val == data.UserRightPermissionID) {
            //        $('#tblUserRightList>tbody>tr:eq(' + index + ')').remove();
            //    }

            //});
        }
        if (data.Success === false) {
            
            AppUtil.ShowError("Permission set Failed.");
        }

    },
    SetPermissionForEmployeeError: function (data) {
        
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administrator.");
    },
}