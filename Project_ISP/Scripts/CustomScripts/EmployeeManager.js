//var EmployeeManager = {


//    addRequestVerificationToken: function (data) {
        
//        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
//        return data;
//    },

//    Validation: function () {

//        if (AppUtil.GetIdValue("Name") === '') {
//            AppUtil.ShowSuccess("Please Insert Employee Name.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("LoginName") === '') {
//            AppUtil.ShowSuccess("Please Insert Employee Login Name.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("Password") === '') {
//            AppUtil.ShowSuccess("Please Insert Password.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("Phone") === '') {
//            AppUtil.ShowSuccess("Please Insert Phone Number.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("Address") === '') {
//            AppUtil.ShowSuccess("Please Insert Address.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("Email") === '') {
//            AppUtil.ShowSuccess("Please Insert Email.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("DepartmentID") === '') {
//            AppUtil.ShowSuccess("Please Insert DepartmentID.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("RoleID") === '') {
//            AppUtil.ShowSuccess("Please Insert RoleID.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("UserRightPermissionID") === '') {
//            AppUtil.ShowSuccess("Please Select User Right Permission Name.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("EmployeeStatus") === '') {
//            AppUtil.ShowSuccess("Please Insert Status.");
//            return false;
//        }
//        return true;
//    },

//    InsertEmployeeFromPopUp: function (Employee) {
        
//        //AppUtil.ShowWaitingDialog();

//        var url = "/Employee/InsertEmployee/";
//        var EmployeeID = AppUtil.GetIdValue("EmployeeID");
//        var Name = AppUtil.GetIdValue("Name");
//        var LoginName = AppUtil.GetIdValue("LoginName");
//        var Password = AppUtil.GetIdValue("Password");
//        var Phone = AppUtil.GetIdValue("Phone");
//        var Address = AppUtil.GetIdValue("Address");
//        var Email = AppUtil.GetIdValue("Email");
//        var DepartmentID = AppUtil.GetIdValue("DepartmentID");
//        var RoleID = AppUtil.GetIdValue("RoleID");
//        var UserRightPermissionID = $("#UserRightPermissionID").val();
//        //var EmployeeStatus = AppUtil.GetIdValue("EmployeeStatus");
//        var EmployeeStatus = AppUtil.GetIdValue("EmployeeStatusDD");

//        // setTimeout(function () {
//        var Employee = { EmployeeID: EmployeeID, Name: Name, LoginName: LoginName, Password: Password, Phone: Phone, Address: Address, Email: Email, DepartmentID: DepartmentID, RoleID: RoleID, UserRightPermissionID: UserRightPermissionID, EmployeeStatus: EmployeeStatus };

//        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
//        var datas = JSON.stringify({ Employee: Employee });
//        AppUtil.MakeAjaxCall(url, "POST", datas, EmployeeManager.EmployeeManagerFromPopUpSuccess, EmployeeManager.EmployeeManagerFromPopUpFail);
//        //    }, 500);


//    },
//    EmployeeManagerFromPopUpSuccess: function (data) {
        
//        //AppUtil.HideWaitingDialog();
//        if (data.SuccessInsert === true) {
            
//            AppUtil.ShowSuccess("Successfully Insert Employee.");
//            window.location.href = "/Employee/Index";
//            //AppUtil.ShowSuccess("Successfully Insert ");
//        }
//        if (data.SuccessInsert === false) {

//            if (data.LoginNameExist === true) {
//                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
//            }
//            else {
//                alert("Employe Insert fail ");
//            }
            
//            //AppUtil.ShowError("Insert fail");
//        }
//    },
//    EmployeeManagerFromPopUpFail: function (data) {
//        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
//        console.log(data);
//    },

//    ShowEmployeeDetailsByIDForUpdate: function (EmployeeID) {
        
//        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';
        
//        //AppUtil.ShowWaitingDialog();
//        //  setTimeout(function () {
        
//        var url = "/Employee/GetEmployeeDetailsByID/";
//        // var data = { EmployeeID: EmployeeID };
//        var data = ({ EmployeeID: EmployeeID });
//        //  data = PackageManager.addRequestVerificationToken(data);

//        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.ShowEmployeeDetailsByIDForUpdateSuccess, EmployeeManager.ShowEmployeeDetailsByIDForUpdateError);

//        //   }, 500);

//    },
//    ShowEmployeeDetailsByIDForUpdateSuccess: function (data) {
//        // console.log(data);
        
//        var EmployeeJSONParse = (data.EmployeeDetails);

//        $("#NewName").val(EmployeeJSONParse.Name);
//        $("#NewLoginName").val(EmployeeJSONParse.LoginName);
//        $("#NewPassword").val(EmployeeJSONParse.Password);
//        $("#NewPhone").val(EmployeeJSONParse.Phone);
//        $("#NewAddress").val(EmployeeJSONParse.Address);
//        $("#NewEmail").val(EmployeeJSONParse.Email);
//        $("#NewDepartmentID").val(EmployeeJSONParse.DepartmentID);
//        $("#NewRoleID").val(EmployeeJSONParse.RoleID);
//        $("#NewUserRightPermissionID").val(EmployeeJSONParse.UserRightPermissionID);
//        $("#NewEmployeeStatus").val(EmployeeJSONParse.EmployeeStatus);
//        $("#NewEmployeeStatusDD").val(EmployeeJSONParse.EmployeeStatus);


//        $("#mdlEmployeeUpdate").modal("show");
//    },
//    ShowEmployeeDetailsByIDForUpdateError: function () {
//        console.log(data);
//        alert("error");
//    },

//    EmployeeUpdateValidation: function () {
//        if (AppUtil.GetIdValue("NewName") === '') {
//            AppUtil.ShowSuccess("Please Insert Employee Name.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("NewLoginName") === '') {
//            AppUtil.ShowSuccess("Please Insert Employee Login Name.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("NewPassword") === '') {
//            AppUtil.ShowSuccess("Please Insert Password.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("NewPhone") === '') {
//            AppUtil.ShowSuccess("Please Insert Phone Number.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("NewAddress") === '') {
//            AppUtil.ShowSuccess("Please Insert Address.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("NewEmail") === '') {
//            AppUtil.ShowSuccess("Please Insert Email.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("NewDepartmentID") === '') {
//            AppUtil.ShowSuccess("Please Insert DepartmentID.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("NewRoleID") === '') {
//            AppUtil.ShowSuccess("Please Insert RoleID.");
//            return false;
//        }

//        if (AppUtil.GetIdValue("NewUserRightPermissionID") === '') {
//            AppUtil.ShowSuccess("Please Select User Right Permission Name.");
//            return false;
//        }
//        if (AppUtil.GetIdValue("NewEmployeeStatus") === '') {
//            AppUtil.ShowSuccess("Please Insert Status.");
//            return false;
//        }
//        return true;
//    },

//    UpdateEmployeeInformation: function () {
        
//        var employeeID = EmployeeID;

//        var NewName = $("#NewName").val();
//        var NewLoginName = $("#NewLoginName").val();
//        var Password = $("#NewPassword").val();
//        var NewPhone = $("#NewPhone").val();
//        var NewAddress = $("#NewAddress").val();
//        var NewEmail = $("#NewEmail").val();
//        var NewDepartmentID = $("#NewDepartmentID").val();
//        var NewRoleID = $("#NewRoleID").val();
//        var NewUserRightPermissionID = $("#NewUserRightPermissionID").val();
//        //var NewEmployeeStatus = $("#NewEmployeeStatus").val();
//        var NewEmployeeStatus = $("#NewEmployeeStatusDD").val();
//        var url = "/Employee/UpdateEmployeeInformation/";
        
//        var employeeInfo = ({ EmployeeID: employeeID, Name: NewName, LoginName: NewLoginName, Password: Password, Phone: NewPhone, Address: NewAddress, Email: NewEmail, DepartmentID: NewDepartmentID, RoleID: NewRoleID, UserRightPermissionID: NewUserRightPermissionID, EmployeeStatus: NewEmployeeStatus });
//        var data = JSON.stringify({ Employee: employeeInfo });
//        AppUtil.MakeAjaxCall(url, "POST", data, EmployeeManager.UpdateEmployeeSuccess, EmployeeManager.UpdateEmployeeError);
//    },
//    UpdateEmployeeSuccess: function (data) {
        
//        if (data.UpdateSuccess === false) {
//            if (data.LoginNameExist === true) {
//                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
//            }
//            else {
//                AppUtil.ShowError("Update Fail. Contact With Administrator.");
//            }
//        }

//        if (data.UpdateSuccess === true) {
//            var EmployeeInformation = (data.employees);

//            $("#tblEmployee tbody>tr").each(function () {
                
//                var EmployeeID = $(this).find("td:eq(0) input").val();
//                var index = $(this).index();
//                if (EmployeeInformation[0].EmployeeID == EmployeeID) {
                    
//                    var status = "";
//                    if (EmployeeInformation[0].EmployeeStatus == 1) {
//                        status = "Active";
//                    }
//                    if (EmployeeInformation[0].EmployeeStatus == 2) {
//                        status = "Lock";
//                    }
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(1)").text(EmployeeInformation[0].Name);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(2)").text(EmployeeInformation[0].LoginName);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(3)").text(EmployeeInformation[0].Phone);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(4)").text(EmployeeInformation[0].Address);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(5)").text(EmployeeInformation[0].Email);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(6)").text(EmployeeInformation[0].DepartmentName);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(7)").text(EmployeeInformation[0].RoleName);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(8)").text(EmployeeInformation[0].UserRightPermissionName);
//                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(9)").text(status);
//                    //$('#tblExpense tbody>tr:eq(' + index + ')').find("td:eq(6)").text(AppUtil.ParseDateTime(PackageInformation[0].Date));
//                }
//            });

//        }
//        $("#mdlEmployeeUpdate").modal('hide');
//        console.log(data);
//    },
//    UpdateEmployeeError: function (data) {
//        console.log(data);
//    },


//    PrintEmployeeList: function () {
        
//        var url = "/Excel/CreateReportForEmployee";
//        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

//        //('#ConnectionDate').datepicker('getDate');

//        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
//        var data = ({});
//        data = EmployeeManager.addRequestVerificationToken(data);
//        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.PrintEmployeeListSuccess, EmployeeManager.PrintEmployeeListFail);
//    },
//    PrintEmployeeListSuccess: function (data) {
        
//        console.log(data);
//        var response = (data);
//        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
//            + '&filename=' + response.FileName;

//        //if (data.Success === true) {
//        //    AppUtil.ShowSuccess("Employee List Print Successfully.");
//        //}
//        //if (data.Success === false) {
//        //    AppUtil.ShowSuccess("Failed To Print Employee List.");
//        //}
//    },
//    PrintEmployeeListFail: function (data) {
        
//        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
//        console.log(data);
//    },
//}

var EmployeeManager = {


    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    Validation: function () {

        if (AppUtil.GetIdValue("Name") === '') {
            AppUtil.ShowSuccess("Please Insert Employee Name.");
            return false;
        }
        if (AppUtil.GetIdValue("LoginName") === '') {
            AppUtil.ShowSuccess("Please Insert Employee Login Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Password") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("Phone") === '') {
            AppUtil.ShowSuccess("Please Insert Phone Number.");
            return false;
        }

        if (AppUtil.GetIdValue("Address") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }

        if (AppUtil.GetIdValue("Email") === '') {
            AppUtil.ShowSuccess("Please Insert Email.");
            return false;
        }

        if (AppUtil.GetIdValue("DepartmentID") === '') {
            AppUtil.ShowSuccess("Please Insert DepartmentID.");
            return false;
        }

        if (AppUtil.GetIdValue("RoleID") === '') {
            AppUtil.ShowSuccess("Please Insert RoleID.");
            return false;
        }
        if (AppUtil.GetIdValue("UserRightPermissionID") === '') {
            AppUtil.ShowSuccess("Please Select User Right Permission Name.");
            return false;
        }

        if (AppUtil.GetIdValue("EmployeeStatus") === '') {
            AppUtil.ShowSuccess("Please Insert Status.");
            return false;
        }
        if (AppUtil.GetIdValue("EmployeeStatus") === '') {
            AppUtil.ShowSuccess("Please Insert Device ID.");
            return false;
        }
        if (AppUtil.GetIdValue("EmployeeOwnCreateImage") === '') {
            AppUtil.ShowSuccess("Please Upload Employee Own Image.");
            return false;
        }

        if (AppUtil.GetIdValue("EmployeeNIDCreateImage") === '') {
            AppUtil.ShowSuccess("Please Please Upload Employee NID Image.");
            return false;
        }
        return true;
    },

    InsertEmployeeFromPopUp: function (Employee) {
        var url = "/Employee/InsertEmployee/";
        //var EmployeeID = AppUtil.GetIdValue("EmployeeID");
        var Name = AppUtil.GetIdValue("Name");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Phone = AppUtil.GetIdValue("Phone");
        var Address = AppUtil.GetIdValue("Address");
        var Email = AppUtil.GetIdValue("Email");
        var DepartmentID = AppUtil.GetIdValue("DepartmentID");
        var RoleID = AppUtil.GetIdValue("RoleID");
        var UserRightPermissionID = $("#UserRightPermissionID").val();
        var DOB = AppUtil.GetIdValue("DateOfBirth");
        var DutyShiftID = AppUtil.GetIdValue("DutyShift");
        var EmployeeStatus = AppUtil.GetIdValue("EmployeeStatusDD");
        var Salary = AppUtil.GetIdValue("Salary");
        var OffDay = AppUtil.GetIdValue("OffDay");
        var DeviceID = $("#DeviceID").val();
        var BreakHour = AppUtil.GetIdValue("BreakHour");
        var BreakMin = AppUtil.GetIdValue("BreakMinute");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Employee = { Name: Name, LoginName: LoginName, Password: Password, Phone: Phone, DeviceID: DeviceID, Address: Address, Email: Email, DOB: DOB, DepartmentID: DepartmentID, RoleID: RoleID, UserRightPermissionID: UserRightPermissionID, EmployeeStatus: EmployeeStatus, DutyShiftID: DutyShiftID, Salary: Salary, DayId: OffDay, BreakHour: BreakHour, BreakMinute: BreakMin };

        var formData = new FormData();
        formData.append('EmployeeOwnCreateImage', $('#EmployeeOwnCreateImage')[0].files[0]);
        formData.append('EmployeeNIDCreateImage', $('#EmployeeNIDCreateImage')[0].files[0]);
        formData.append('Employee_details', JSON.stringify(Employee));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, EmployeeManager.EmployeeInsertFromPopUpSuccess, EmployeeManager.EmployeeInsertFromPopUpFail);

    },
    EmployeeInsertFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        if (data.SuccessInsert === true) {

            AppUtil.ShowSuccess("Successfully Insert Employee.");
            window.location.href = "/Employee/Index";
        }
        if (data.SuccessInsert === false) {

            if (data.LoginNameExist === true) {
                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
            }
            else {
                alert("Employe Insert fail ");
            }

            //AppUtil.ShowError("Insert fail");
        }
    },
    EmployeeInsertFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowEmployeeDetailsByIDForUpdate: function (EmployeeID) {

        var url = "/Employee/GetEmployeeDetailsByID/";
        var data = ({ EmployeeID: EmployeeID });

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.ShowEmployeeDetailsByIDForUpdateSuccess, EmployeeManager.ShowEmployeeDetailsByIDForUpdateError);

    },
    ShowEmployeeDetailsByIDForUpdateSuccess: function (data) {
        var EmployeeJSONParse = (data.EmployeeDetails);

        $("#NewName").val(EmployeeJSONParse.Name);
        $("#NewLoginName").val(EmployeeJSONParse.LoginName);
        $("#NewPassword").val(EmployeeJSONParse.Password);
        $("#NewPhone").val(EmployeeJSONParse.Phone);
        $("#NewAddress").val(EmployeeJSONParse.Address);
        $("#NewEmail").val(EmployeeJSONParse.Email);
        $("#NewDepartmentID").val(EmployeeJSONParse.DepartmentID);
        $("#NewDutyShift").val(EmployeeJSONParse.DutyShiftID);
        $("#NewRoleID").val(EmployeeJSONParse.RoleID);
        $("#NewSalary").val(EmployeeJSONParse.Salary);
        $("#NewOffDay").val(EmployeeJSONParse.OffDay);
        $("#NewBreakHour").val(EmployeeJSONParse.BreakHour);
        $("#NewDutyShift").val(data.ShiftTime);
        $("#NewBreakMinute").val(EmployeeJSONParse.BreakMinute);
        $("#NewUserRightPermissionID").val(EmployeeJSONParse.UserRightPermissionID);
        $("#NewEmployeeStatusDD").val(EmployeeJSONParse.EmployeeStatus);
        $("#NewDateofBirth").val(AppUtil.ParseDateINMMDDYYYY(EmployeeJSONParse.DOB));

        var a = '' + EmployeeJSONParse.OwnImagePath + '#' + new Date().getTime();

        $("#PreviewEmployeeOwnUpdateImage").hide(0).attr('src', '' + EmployeeJSONParse.OwnImagePath + '#' + new Date().getTime()).show(0);
        $("#PreviewEmployeeOwnUpdateImage").prop("src", EmployeeJSONParse.OwnImagePath);

        var a = '' + EmployeeJSONParse.NIDImagePath + '#' + new Date().getTime();

        $("#PreviewEmployeeNIDUpdateImage").hide(0).attr('src', '' + EmployeeJSONParse.NIDImagePath + '#' + new Date().getTime()).show(0);
        $("#PreviewEmployeeNIDUpdateImage").prop("src", EmployeeJSONParse.NIDImagePath);

        $("#mdlEmployeeUpdate").modal("show");
    },
    ShowEmployeeDetailsByIDForUpdateError: function () {
        console.log(data);
        alert("error");
    },

    EmployeeUpdateValidation: function () {
        if (AppUtil.GetIdValue("NewName") === '') {
            AppUtil.ShowSuccess("Please Insert Employee Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewLoginName") === '') {
            AppUtil.ShowSuccess("Please Insert Employee Login Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPassword") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPhone") === '') {
            AppUtil.ShowSuccess("Please Insert Phone Number.");
            return false;
        }

        if (AppUtil.GetIdValue("NewAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }

        if (AppUtil.GetIdValue("NewEmail") === '') {
            AppUtil.ShowSuccess("Please Insert Email.");
            return false;
        }

        if (AppUtil.GetIdValue("NewDepartmentID") === '') {
            AppUtil.ShowSuccess("Please Insert DepartmentID.");
            return false;
        }

        if (AppUtil.GetIdValue("NewRoleID") === '') {
            AppUtil.ShowSuccess("Please Insert RoleID.");
            return false;
        }

        if (AppUtil.GetIdValue("NewUserRightPermissionID") === '') {
            AppUtil.ShowSuccess("Please Select User Right Permission Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewEmployeeStatus") === '') {
            AppUtil.ShowSuccess("Please Insert Status.");
            return false;
        }
        return true;
    },

    UpdateEmployeeInformation: function () {
        var employeeID = EmployeeID;

        var NewName = $("#NewName").val();
        var NewLoginName = $("#NewLoginName").val();
        var Password = $("#NewPassword").val();
        var NewPhone = $("#NewPhone").val();
        var NewAddress = $("#NewAddress").val();
        var NewEmail = $("#NewEmail").val();
        var NewDepartmentID = $("#NewDepartmentID").val();
        var NewRoleID = $("#NewRoleID").val();
        var Salary = $("#NewSalary").val();
        var NewUserRightPermissionID = $("#NewUserRightPermissionID").val();
        var DOB = $("#NewDateofBirth").val();
        var DutyShift = $("#NewDutyShift").val();
        var OffDay = $("#NewOffDay").val();
        var DeviceID = $("#NewDevieID").val();
        var BreakHour = $("#NewBreakHour").val();
        var BreakMin = $("#NewBreakMinute").val();
        var NewEmployeeStatus = $("#NewEmployeeStatusDD").val();
        var EmployeeOwnImagePath = $("#EmployeeOwnUpdateImage").val();
        var EmployeeNIDImagePath = $("#EmployeeNIDUpdateImage").val();
        var url = "/Employee/UpdateEmployeeInformation/";


        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var employeeInfo = ({ EmployeeID: employeeID, Name: NewName, LoginName: NewLoginName, DOB: DOB, DutyShiftID: DutyShift, Password: Password, Phone: NewPhone, Address: NewAddress, Email: NewEmail, DepartmentID: NewDepartmentID, RoleID: NewRoleID, UserRightPermissionID: NewUserRightPermissionID, EmployeeStatus: NewEmployeeStatus, Salary: Salary, DayID: OffDay, BreakHour: BreakHour, BreakMinute: BreakMin, EmployeeOwnImageBytesPaths: EmployeeOwnImagePath, EmployeeNIDImageBytesPaths: EmployeeNIDImagePath, DeviceID: DeviceID });

        var formData = new FormData();
        formData.append('EmployeeOwnUpdateImage', $('#EmployeeOwnUpdateImage')[0].files[0]);
        formData.append('EmployeeNIDUpdateImage', $('#EmployeeNIDUpdateImage')[0].files[0]);
        formData.append('Employee_details', JSON.stringify(employeeInfo));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, EmployeeManager.UpdateEmployeeSuccess, EmployeeManager.UpdateEmployeeError);
    },
    UpdateEmployeeSuccess: function (data) {

        if (data.Success === false) {
            if (data.LoginNameExist === true) {
                AppUtil.ShowError("Sorry Login Already Exist. Choose Another One.");
            }
            else {
                AppUtil.ShowError("Update Fail. Contact With Administrator.");
            }
        }

        if (data.UpdateSuccess === true) {
            var EmployeeInformation = (data.employees);

            $("#tblEmployee tbody>tr").each(function () {

                var EmployeeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (EmployeeInformation.EmployeeID == EmployeeID) {

                    var status = "";
                    if (EmployeeInformation.EmployeeStatus == 1) {
                        status = "Active";
                    }
                    if (EmployeeInformation.EmployeeStatus == 2) {
                        status = "Lock";
                    }
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(1)").text(EmployeeInformation.Name);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(2)").text(EmployeeInformation.LoginName);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(3)").text(EmployeeInformation.Phone);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(4)").text(EmployeeInformation.Address);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(5)").text(EmployeeInformation.Email);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(6)").text(AppUtil.ParseDateINMMDDYYYY(EmployeeInformation.DOB));
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(7)").text(EmployeeInformation.DepartmentName);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(8)").text(EmployeeInformation.Salary);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(9)").text(EmployeeInformation.RoleName);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(10)").text(EmployeeInformation.OffDay);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(11)").text(EmployeeInformation.UserRightPermissionName);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(12)").text(data.BreakRunTime);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(13)").text(data.ShiftTime);
                    $('#tblEmployee tbody>tr:eq(' + index + ')').find("td:eq(15)").text(status);
                }
            });

        }
        $("#mdlEmployeeUpdate").modal('hide');
        console.log(data);
    },
    UpdateEmployeeError: function (data) {
        console.log(data);
    },


    PrintEmployeeList: function () {

        var url = "/Excel/CreateReportForEmployee";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({});
        data = EmployeeManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.PrintEmployeeListSuccess, EmployeeManager.PrintEmployeeListFail);
    },
    PrintEmployeeListSuccess: function (data) {

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
    PrintEmployeeListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowEmployeeWorkSchedule: function (EmployeeID) {

        var url = "/Employee/InsertWorkScheduleForEmployee/";
        var data = ({ empID: EmployeeID });

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.ShowEmployeeWorkScheduleSuccess, EmployeeManager.ShowEmployeeWorkScheduleError);

    },
    ShowEmployeeWorkScheduleSuccess: function (data) {

        if (data.success === true) {
            if (jQuery.isEmptyObject(data.lstEmployeeSchedule)) {
                day1 = data.dayList[0];
                day2 = data.dayList[1];
                day3 = data.dayList[2];
                day4 = data.dayList[3];
                day5 = data.dayList[4];
                day6 = data.dayList[5];
                day7 = data.dayList[6];

                $("#Empid").val(data.EmployeeID);
                $("#DayID1").val(day1.DayName);
                $("#DayID2").val(day2.DayName);
                $("#DayID3").val(day3.DayName);
                $("#DayID4").val(day4.DayName);
                $("#DayID5").val(day5.DayName);
                $("#DayID6").val(day6.DayName);
                $("#DayID7").val(day7.DayName);

                $("#mdlEmployeeWorkSchedule").modal("show");
            }
            else {

                $("#Empid").val(data.lstEmployeeSchedule[0].EmployeeID);
                $("#EmployeeWorkScheduleID1").val(data.lstEmployeeSchedule[0].EmployeeVsWorkScheduleID);
                //day1 = data.lstEmployeeSchedule[0].Days.DayName;
                $("#DayID1").val(data.lstEmployeeSchedule[0].Days.DayName);

                $("#StartH1").val(data.lstEmployeeSchedule[0].StartHour);
                $("#StartM1").val(data.lstEmployeeSchedule[0].StartMinute);
                $("#RunH1").val(data.lstEmployeeSchedule[0].RunHour);
                $("#RunM1").val(data.lstEmployeeSchedule[0].RunMinute);
                $("#BreakSH1").val(data.lstEmployeeSchedule[0].BreakStartHour);
                $("#BreakSM1").val(data.lstEmployeeSchedule[0].BreakStartMinute);
                $("#BreakEH1").val(data.lstEmployeeSchedule[0].BreakEndHour);
                $("#BreakEM1").val(data.lstEmployeeSchedule[0].BreakEndMinute);


                $("#EmployeeWorkScheduleID2").val(data.lstEmployeeSchedule[1].EmployeeVsWorkScheduleID);
                $("#DayID2").val(data.lstEmployeeSchedule[1].Days.DayName);

                $("#StartH12").val(data.lstEmployeeSchedule[1].StartHour);
                $("#StartM2").val(data.lstEmployeeSchedule[1].StartMinute);
                $("#RunH2").val(data.lstEmployeeSchedule[1].RunHour);
                $("#RunM2").val(data.lstEmployeeSchedule[1].RunMinute);
                $("#BreakSH2").val(data.lstEmployeeSchedule[1].BreakStartHour);
                $("#BreakSM2").val(data.lstEmployeeSchedule[1].BreakStartMinute);
                $("#BreakEH2").val(data.lstEmployeeSchedule[1].BreakEndHour);
                $("#BreakEM2").val(data.lstEmployeeSchedule[1].BreakEndMinute);


                $("#EmployeeWorkScheduleID3").val(data.lstEmployeeSchedule[2].EmployeeVsWorkScheduleID);
                $("#DayID3").val(data.lstEmployeeSchedule[2].Days.DayName);

                $("#StartH3").val(data.lstEmployeeSchedule[2].StartHour);
                $("#StartM3").val(data.lstEmployeeSchedule[2].StartMinute);
                $("#RunH3").val(data.lstEmployeeSchedule[2].RunHour);
                $("#RunM3").val(data.lstEmployeeSchedule[2].RunMinute);
                $("#BreakSH3").val(data.lstEmployeeSchedule[2].BreakStartHour);
                $("#BreakSM3").val(data.lstEmployeeSchedule[2].BreakStartMinute);
                $("#BreakEH3").val(data.lstEmployeeSchedule[2].BreakEndHour);
                $("#BreakEM3").val(data.lstEmployeeSchedule[2].BreakEndMinute);


                $("#EmployeeWorkScheduleID4").val(data.lstEmployeeSchedule[3].EmployeeVsWorkScheduleID);
                $("#DayID4").val(data.lstEmployeeSchedule[3].Days.DayName);

                $("#StartH4").val(data.lstEmployeeSchedule[3].StartHour);
                $("#StartM4").val(data.lstEmployeeSchedule[3].StartMinute);
                $("#RunH4").val(data.lstEmployeeSchedule[3].RunHour);
                $("#RunM4").val(data.lstEmployeeSchedule[3].RunMinute);
                $("#BreakSH4").val(data.lstEmployeeSchedule[3].BreakStartHour);
                $("#BreakSM4").val(data.lstEmployeeSchedule[3].BreakStartMinute);
                $("#BreakEH4").val(data.lstEmployeeSchedule[3].BreakEndHour);
                $("#BreakEM4").val(data.lstEmployeeSchedule[3].BreakEndMinute);


                $("#EmployeeWorkScheduleID5").val(data.lstEmployeeSchedule[4].EmployeeVsWorkScheduleID);
                $("#DayID5").val(data.lstEmployeeSchedule[4].Days.DayName);

                $("#StartH5").val(data.lstEmployeeSchedule[4].StartHour);
                $("#StartM5").val(data.lstEmployeeSchedule[4].StartMinute);
                $("#RunH5").val(data.lstEmployeeSchedule[4].RunHour);
                $("#RunM5").val(data.lstEmployeeSchedule[4].RunMinute);
                $("#BreakSH5").val(data.lstEmployeeSchedule[4].BreakStartHour);
                $("#BreakSM5").val(data.lstEmployeeSchedule[4].BreakStartMinute);
                $("#BreakEH5").val(data.lstEmployeeSchedule[4].BreakEndHour);
                $("#BreakEM5").val(data.lstEmployeeSchedule[4].BreakEndMinute);


                $("#EmployeeWorkScheduleID6").val(data.lstEmployeeSchedule[5].EmployeeVsWorkScheduleID);
                $("#DayID6").val(data.lstEmployeeSchedule[5].Days.DayName);

                $("#StartH6").val(data.lstEmployeeSchedule[5].StartHour);
                $("#StartM6").val(data.lstEmployeeSchedule[5].StartMinute);
                $("#RunH6").val(data.lstEmployeeSchedule[5].RunHour);
                $("#RunM6").val(data.lstEmployeeSchedule[5].RunMinute);
                $("#BreakSH6").val(data.lstEmployeeSchedule[5].BreakStartHour);
                $("#BreakSM6").val(data.lstEmployeeSchedule[5].BreakStartMinute);
                $("#BreakEH6").val(data.lstEmployeeSchedule[5].BreakEndHour);
                $("#BreakEM6").val(data.lstEmployeeSchedule[5].BreakEndMinute);


                $("#EmployeeWorkScheduleID7").val(data.lstEmployeeSchedule[6].EmployeeVsWorkScheduleID);
                $("#DayID7").val(data.lstEmployeeSchedule[6].Days.DayName);

                $("#StartH7").val(data.lstEmployeeSchedule[6].StartHour);
                $("#StartM7").val(data.lstEmployeeSchedule[6].StartMinute);
                $("#RunH7").val(data.lstEmployeeSchedule[6].RunHour);
                $("#RunM7").val(data.lstEmployeeSchedule[6].RunMinute);
                $("#BreakSH7").val(data.lstEmployeeSchedule[6].BreakStartHour);
                $("#BreakSM7").val(data.lstEmployeeSchedule[6].BreakStartMinute);
                $("#BreakEH7").val(data.lstEmployeeSchedule[6].BreakEndHour);
                $("#BreakEM7").val(data.lstEmployeeSchedule[6].BreakEndMinute);

                $("#mdlEmployeeWorkSchedule").modal("show");


            }

        }
        else {
            AppUtil.ShowError("Couldn't show employee work schedule!");
        }
    },
    ShowEmployeeWorkScheduleError: function () {
        alert("error");
    },

    SaveEmployeeWorkSchedule: function (EmployeeID) {

        var LeaveInfoForSpecific = new Array();

        var EmpID = $("#Empid").val();

        var url = "/Employee/SaveEmployeeWorkSchedule/";


        var EmpWorkSchedule1 = $("#EmployeeWorkScheduleID1").val();
        var Day1 = 1;
        var StartH1 = $("#StartH1").val();
        var StartM1 = $("#StartM1").val();
        var RunH1 = $("#RunH1").val();
        var RunM1 = $("#RunM1").val();
        var BreakSH1 = $("#BreakSH1").val();
        var BreakSM1 = $("#BreakSM1").val();
        var BreakEH1 = $("#BreakEH1").val();
        var BreakEM1 = $("#BreakEM1").val();


        var EmpWorkSchedule2 = $("#EmployeeWorkScheduleID2").val();
        var Day2 = 2;
        var StartH2 = $("#StartH2").val();
        var StartM2 = $("#StartM2").val();
        var RunH2 = $("#RunH2").val();
        var RunM2 = $("#RunM2").val();
        var BreakSH2 = $("#BreakSH2").val();
        var BreakSM2 = $("#BreakSM2").val();
        var BreakEH2 = $("#BreakEH2").val();
        var BreakEM2 = $("#BreakEM2").val();


        var EmpWorkSchedule3 = $("#EmployeeWorkScheduleID3").val();
        var Day3 = 3;
        var StartH3 = $("#StartH3").val();
        var StartM3 = $("#StartM3").val();
        var RunH3 = $("#RunH3").val();
        var RunM3 = $("#RunM3").val();
        var BreakSH3 = $("#BreakSH3").val();
        var BreakSM3 = $("#BreakSM3").val();
        var BreakEH3 = $("#BreakEH3").val();
        var BreakEM3 = $("#BreakEM3").val();


        var EmpWorkSchedule4 = $("#EmployeeWorkScheduleID4").val();
        var Day4 = 4;
        var StartH4 = $("#StartH4").val();
        var StartM4 = $("#StartM4").val();
        var RunH4 = $("#RunH4").val();
        var RunM4 = $("#RunM4").val();
        var BreakSH4 = $("#BreakSH4").val();
        var BreakSM4 = $("#BreakSM4").val();
        var BreakEH4 = $("#BreakEH4").val();
        var BreakEM4 = $("#BreakEM4").val();


        var EmpWorkSchedule5 = $("#EmployeeWorkScheduleID5").val();
        var Day5 = 7;
        var StartH5 = $("#StartH5").val();
        var StartM5 = $("#StartM5").val();
        var RunH5 = $("#RunH5").val();
        var RunM5 = $("#RunM5").val();
        var BreakSH5 = $("#BreakSH5").val();
        var BreakSM5 = $("#BreakSM5").val();
        var BreakEH5 = $("#BreakEH5").val();
        var BreakEM5 = $("#BreakEM5").val();


        var EmpWorkSchedule6 = $("#EmployeeWorkScheduleID6").val();
        var Day6 = 8;
        var StartH6 = $("#StartH6").val();
        var StartM6 = $("#StartM6").val();
        var RunH6 = $("#RunH6").val();
        var RunM6 = $("#RunM6").val();
        var BreakSH6 = $("#BreakSH6").val();
        var BreakSM6 = $("#BreakSM6").val();
        var BreakEH6 = $("#BreakEH6").val();
        var BreakEM6 = $("#BreakEM6").val();


        var EmpWorkSchedule7 = $("#EmployeeWorkScheduleID7").val();
        var Day7 = 9;
        var StartH7 = $("#StartH7").val();
        var StartM7 = $("#StartM7").val();
        var RunH7 = $("#RunH7").val();
        var RunM7 = $("#RunM7").val();
        var BreakSH7 = $("#BreakSH7").val();
        var BreakSM7 = $("#BreakSM7").val();
        var BreakEH7 = $("#BreakEH7").val();
        var BreakEM7 = $("#BreakEM7").val();


        var Day1 = { EmployeeVsWorkScheduleID: EmpWorkSchedule1, EmployeeID: EmpID, DayID: Day1, StartHour: StartH1, StartMinute: StartM1, RunHour: RunH1, RunMinute: RunM1, BreakStartHour: BreakSH1, BreakStartMinute: BreakSM1, BreakEndHour: BreakEH1, BreakEndMinute: BreakEM1 }
        var Day2 = { EmployeeVsWorkScheduleID: EmpWorkSchedule2, EmployeeID: EmpID, DayID: Day2, StartHour: StartH2, StartMinute: StartM2, RunHour: RunH2, RunMinute: RunM2, BreakStartHour: BreakSH2, BreakStartMinute: BreakSM2, BreakEndHour: BreakEH2, BreakEndMinute: BreakEM2 }
        var Day3 = { EmployeeVsWorkScheduleID: EmpWorkSchedule3, EmployeeID: EmpID, DayID: Day3, StartHour: StartH3, StartMinute: StartM3, RunHour: RunH3, RunMinute: RunM3, BreakStartHour: BreakSH3, BreakStartMinute: BreakSM3, BreakEndHour: BreakEH3, BreakEndMinute: BreakEM3 }
        var Day4 = { EmployeeVsWorkScheduleID: EmpWorkSchedule4, EmployeeID: EmpID, DayID: Day4, StartHour: StartH4, StartMinute: StartM4, RunHour: RunH4, RunMinute: RunM4, BreakStartHour: BreakSH4, BreakStartMinute: BreakSM4, BreakEndHour: BreakEH4, BreakEndMinute: BreakEM4 }
        var Day5 = { EmployeeVsWorkScheduleID: EmpWorkSchedule5, EmployeeID: EmpID, DayID: Day5, StartHour: StartH5, StartMinute: StartM5, RunHour: RunH5, RunMinute: RunM5, BreakStartHour: BreakSH5, BreakStartMinute: BreakSM5, BreakEndHour: BreakEH5, BreakEndMinute: BreakEM5 }
        var Day6 = { EmployeeVsWorkScheduleID: EmpWorkSchedule6, EmployeeID: EmpID, DayID: Day6, StartHour: StartH6, StartMinute: StartM6, RunHour: RunH6, RunMinute: RunM6, BreakStartHour: BreakSH6, BreakStartMinute: BreakSM6, BreakEndHour: BreakEH6, BreakEndMinute: BreakEM6 }
        var Day7 = { EmployeeVsWorkScheduleID: EmpWorkSchedule7, EmployeeID: EmpID, DayID: Day7, StartHour: StartH7, StartMinute: StartM7, RunHour: RunH7, RunMinute: RunM7, BreakStartHour: BreakSH7, BreakStartMinute: BreakSM7, BreakEndHour: BreakEH7, BreakEndMinute: BreakEM7 }

        LeaveInfoForSpecific.push(Day1);
        LeaveInfoForSpecific.push(Day2);
        LeaveInfoForSpecific.push(Day3);
        LeaveInfoForSpecific.push(Day4);
        LeaveInfoForSpecific.push(Day5);
        LeaveInfoForSpecific.push(Day6);
        LeaveInfoForSpecific.push(Day7);

        //var data = ({ Day1st: Day1, Day2nd: Day2, Day3rd: Day3, Day4th: Day4, Day5th: Day5, Day6th: Day6, Day7th: Day7 });
        var data = ({ lstEmployeeVsSchedules: LeaveInfoForSpecific })
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.SaveEmployeeWorkScheduleSuccess, EmployeeManager.SaveEmployeeWorkScheduleError);



    },
    SaveEmployeeWorkScheduleSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess(data.message);
            $("#mdlEmployeeWorkSchedule").modal("hide");
        }
        else {
            AppUtil.ShowError("Error");
        }
    },
    SaveEmployeeWorkScheduleError: function () {
        alert("error");
    },


    ShowEmployeeDutyShift: function (EmployeeID) {

        var url = "/Employee/DutyShiftForEmployee/";
        var data = ({ empID: EmployeeID });

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, EmployeeManager.ShowEmployeeDutyShiftSuccess, EmployeeManager.ShowEmployeeDutyShiftError);

    },

    ShowEmployeeDutyShiftSuccess: function (data) {
        if (data.success === true) {
            var html = "<div>"
            html += "<table class='table table-striped table-bordered table-responsive'>";
            html += "<thead>";
            html += "<tr>";
            html += "<th>" + "Start Time" + "</th>";
            // html += "<th>" + "Start Minute" + "</th>";
            html += "<th>" + "End Time" + "</th>";
            // html += "<th>" + "End Minute" + "</th>";
            html += "</tr>";
            html += "</thead>";

            html += "<tbody>";
            html += "<tr>";
            html += "<td>" + data.shift.StartTime + "</td>";
            //html += "<td>" + data.shift.StartMinute + "</td>";
            html += "<td>" + data.shift.EndTime + "</td>";
            //html += "<td>" + data.shift.EndMinute + "</td>";
            html += "</tr>";
            html += "</tbody>";
            html += "</table>";
            html += "</div>";
            $("#div").html(html);
            //$("#StartH1").val(data.shift.StartHour);
            //$("#StartM1").val(data.shift.StartMinute);
            //$("#EndH1").val(data.shift.EndHour);
            //$("#EndM1").val(data.shift.EndMinute);

            $("#mdlEmployeeDutyShift").modal("show");

        }
        else {
            AppUtil.ShowError("Error while showing duty shift!");
        }
    },
    ShowEmployeeDutyShiftError: function () {
        alert("error");
    },
}