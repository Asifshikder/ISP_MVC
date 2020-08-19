
var AttendanceManager = {
    AttendanceInOutValidation: function () {

        if (AppUtil.GetIdValue("UpdateTitle") === '') {
            AppUtil.ShowSuccess("Please Insert Title.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateStart") === '') {
            AppUtil.ShowSuccess("Please Insert Start Time .");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateEnd") === '') {
            AppUtil.ShowSuccess("Please Insert End Time.");
            return false;
        } if (AppUtil.GetIdValue("UpdateInSalayCut") === '') {
            AppUtil.ShowSuccess("Please Insert In Salary Cut.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateOutSalaryCut") === '') {
            AppUtil.ShowSuccess("Please Insert Out Salary Cut.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateAttenDanceType") === '') {
            AppUtil.ShowSuccess("Please Slect Attendance Type.");
            return false;
        }
        return true;
    },
    AttendanceInOutCreateValidation: function () {


        if (AppUtil.GetIdValue("insertTitle") === '') {
            AppUtil.ShowSuccess("Please Insert Title.");
            return false;
        }
        if (AppUtil.GetIdValue("insertStart") === '') {
            AppUtil.ShowSuccess("Please Insert Start Time .");
            return false;
        }
        if (AppUtil.GetIdValue("insertEnd") === '') {
            AppUtil.ShowSuccess("Please Insert End Time.");
            return false;
        } if (AppUtil.GetIdValue("insertInSalayCut") === '') {
            AppUtil.ShowSuccess("Please Insert In Salary Cut.");
            return false;
        }
        if (AppUtil.GetIdValue("insertOutSalaryCut") === '') {
            AppUtil.ShowSuccess("Please Insert Out Salary Cut.");
            return false;
        }
        if (AppUtil.GetIdValue("InsertAttenDanceType") === '') {
            AppUtil.ShowSuccess("Please Slect Attendance Type.");
            return false;
        }
        return true;
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    CreateAttendanceTypeValidation: function () {

        if (AppUtil.GetIdValue("insertAttenanceType") === '') {
            AppUtil.ShowSuccess("Please Insert Attendance Type Name.");
            return false;
        }
        return true;
    },

    InsertAttendanceType: function (type) {
      
        var url = "/Attedance/InsertAttendanceType/";
        var AttendanceName = AppUtil.GetIdValue("InsertAttendanceTypeName")
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var type = { AttendanceName: AttendanceName };
        var data = JSON.stringify({ type: type });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AttendanceManager.InsertAttendanceTypeSuccess, AttendanceManager.InsertAttendanceTypeFail);

    },
    InsertAttendanceTypeSuccess: function (data) {
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            $('#myTable >tbody > tr:last').after('<tr> <td>' + data.AttendanceTypeInfo.AttendanceName + '</td><td><a href="" id="showAttendanceTypeForUpdate">Show</a></td> </tr>');

        }
        else {
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        }
        AttendanceManager.clearForSaveInformation();
        $("#mdlAttendanceTypeInsert").modal('hide');
        table.draw();
    },
    InsertAttendanceTypeFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    EditAttenanceTypeGet: function (AttendanceTypeID) {

        var url = "/Attedance/GetAttendanceTypeID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ AttendanceTypeID: AttendanceTypeID });
        datas = AttendanceManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, AttendanceManager.EditAttenanceTypeGetsuccess, AttendanceManager.EditAttenanceTypeGetFail);

    },
    EditAttenanceTypeGetsuccess: function (data) {
        $("#EditAttendanceTypeID").val(data.AttendanceType.AttendanceTypeID);
        $("#EditAttenanceType").val(data.AttendanceType.AttendanceName);
        $("#mdlAttendanceTypeUpdate").modal("show");

    },
    EditAttenanceTypeGetFail: function (data) {
      
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");



    },


    EditAttendanceTypePopUp: function () {
      

        var url = "/Attedance/UpdateAttendanceType/";
        var AttendanceId = AppUtil.GetIdValue("EditAttendanceTypeID");
        var AttendanceName = AppUtil.GetIdValue("EditAttenanceType");
        var type = { AttendanceName: AttendanceName, AttendanceTypeID: AttendanceId };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ type: type });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AttendanceManager.EditAttendanceTypePopUpSuccess, AttendanceManager.EditAttendanceTypePopUpFail);


    },
    EditAttendanceTypePopUpSuccess: function (data) {
        var At = data.AttenDanceTypeInformation;
        if (data.UpdateSuccess === true) {

            $("#tbleAttendanceType tbody>tr").each(function () {

                var AttendanceTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (At.AttendanceTypeID == AttendanceTypeID) {
                    $("#tbleAttendanceType >tbody>tr:eq(" + index + ")").find("td:eq(1)").text(At.AttendanceName);
                }
            });
        

            
            AppUtil.ShowSuccess("Attendance Type Successfully Edited");
        }
        else {
            AppUtil.ShowError("Attendance Type Edit fail ");
        }
        AttendanceManager.clearForUpdateInformation();
        $("#mdlAttendanceTypeUpdate").modal("hide");

    },
    EditAttendanceTypePopUpFail: function (data) {
        alert("Employe edit fail ");
    },


    DeleteAttendacneType: function (VendorTypeID) {

        var url = "/Attedance/DeleteAttendanceType/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ AttendanceTypeID: AttendanceTypeID });
        data = AttendanceManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AttendanceManager.DeleteAttendacneTypesuccess, AttendanceManager.DeleteAttendacneTypeFail);

    },
    DeleteAttendacneTypesuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            $("#tbleAttendanceType tbody>tr").each(function () {

                var AttendanceTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.AttendanceTypeID == AttendanceTypeID) {
                    $("#tbleAttendanceType >tbody>tr:eq(" + index + ")").remove();
                }
            });
        }
        $("#mdlAttendanceTypeDelete").modal("hide");
    },
    DeleteAttendacneTypeFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    InsertAttendanceInOut: function (atendaceInOut) {
        var url = "/Attedance/InsertAttendanceInOut/";
        var Title = AppUtil.GetIdValue("insertTitle");
        var Start = AppUtil.GetIdValue("insertStart");
        var End = AppUtil.GetIdValue("insertEnd");
        var InSalaryCut = AppUtil.GetIdValue("insertInSalayCut");
        var OutSalaryCut = AppUtil.GetIdValue("insertOutSalaryCut");
        var AttendanceType = AppUtil.GetIdValue("InsertAttenDanceType");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var atendaceInOut = { Title: Title, start: Start, end: End, InsalaryCut: InSalaryCut, OutSalaryCut: OutSalaryCut, typeId: AttendanceType };
        var data = JSON.stringify({ atendaceInOut: atendaceInOut });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AttendanceManager.InsertAttendanceInOutSuccess, AttendanceManager.InsertAttendanceInOutFail);

    },
    InsertAttendanceInOutSuccess: function (data) {
        if (data.success === true) {
            if (data.AtendanceINOUT.AttendanceTypeID == 40) {
                $('#myTable >tbody > tr:last').after('<tr> <td>' + data.AtendanceINOUT.Title + '</td><td>' + data.AtendanceINOUT.start + '</td><td>' + data.AtendanceINOUT.end + '</td><td>' + data.AtendanceINOUT.InSalaryCut + '</td><td>' + data.AtendanceINOUT.OutSalaryCut + '</td><td> <a value="' + data.AtendanceINOUT.AttendaceInOutID + '"  class="glyphicon glyphicon-edit btn-primary btn-circle" onclick="ShowForEdit(' + data.AtendanceINOUT.AttendaceInOutID + ')"></a ><a value="' + data.AtendanceINOUT.AttendaceInOutID + '"  class="glyphicon glyphicon-remove btn-primary btn-circle" onclick="ShowForDelete(' + data.AtendanceINOUT.AttendaceInOutID + ')"></a ></td> </tr>');

            }
            else if (data.AtendanceINOUT.AttendanceTypeID == 41) {
                $('#MyTable >tbody > tr:last').after('<tr> <td>' + data.AtendanceINOUT.Title + '</td><td>' + data.AtendanceINOUT.start + '</td><td>' + data.AtendanceINOUT.end + '</td><td>' + data.AtendanceINOUT.InSalaryCut + '</td><td>' + data.AtendanceINOUT.OutSalaryCut + '</td><td> <a value="' + data.AtendanceINOUT.AttendaceInOutID + '"  class="glyphicon glyphicon-edit btn-primary btn-circle" onclick="ShowForEdit(' + data.AtendanceINOUT.AttendaceInOutID + ')"></a ><a value="' + data.AtendanceINOUT.AttendaceInOutID + '"  class="glyphicon glyphicon-remove btn-primary btn-circle" onclick="ShowForDelete(' + data.AtendanceINOUT.AttendaceInOutID + ')"></a ></td> </tr>');

            }

            AttendanceManager.clearForAttendanceSaveInformation();
            $("#AddAttendanceInOutModal").modal("hide");
            AppUtil.ShowSuccess("Successfully Inserted");

        }
        else {
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        }
    },
    InsertAttendanceInOutFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    EditAttenanceInOutGet: function (id) {
        var url = "/Attedance/GetAttendanceInOut/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        
        var data = ({ id: id });
        data = AttendanceManager.addRequestVerificationToken(data);
        
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AttendanceManager.EditAttenanceInOutGetsuccess, AttendanceManager.EditAttenanceInOutGetFail);
    },
    EditAttenanceInOutGetsuccess: function (data) {
        if (data.success === true) {
            $("#AttendaceInOutID").val(data.attendanceInOut.AttendaceInOutID);
            $("#UpdateTitle").val(data.attendanceInOut.Title);
            $("#UpdateStart").val(data.attendanceInOut.start);
            $("#UpdateEnd").val(data.attendanceInOut.end);
            $("#UpdateInSalayCut").val(data.attendanceInOut.InSalaryCut);
            $("#UpdateOutSalaryCut").val(data.attendanceInOut.OutSalaryCut);
            $("#UpdateAttenDanceType").val(data.attendanceInOut.AttendanceTypeID);
            $("#AttendanceTypeID").val(data.attendanceInOut.AttendanceTypeID);
            $("#UpdateAttendanceInOutModal").modal("show");
        }
    },
    EditAttenanceInOutGetFail: function (data) {
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },

    EditAttendanceInOut: function () {

        var url = "/Attedance/UpdateAttendanceInOut/";
        var id = AppUtil.GetIdValue("AttendaceInOutID");
        var title = AppUtil.GetIdValue("UpdateTitle");
        var start = AppUtil.GetIdValue("UpdateStart");
        var end = AppUtil.GetIdValue("UpdateEnd");
        var inSalaryCut = AppUtil.GetIdValue("UpdateInSalayCut");
        var outSalaryCut = AppUtil.GetIdValue("UpdateOutSalaryCut");
        var attendanceType = AppUtil.GetIdValue("UpdateAttenDanceType");
        var TypeID = AppUtil.GetIdValue("AttendanceTypeID");
        // setTimeout(function () {
        var attendaceInOut = { id: id, Title: title, start: start, end: end, InsalaryCut: inSalaryCut, OutSalaryCut: outSalaryCut, AtendanceType: attendanceType, typeId: TypeID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = JSON.stringify({ attendaceInOut: attendaceInOut });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AttendanceManager.EditAttendanceInOutSuccess, AttendanceManager.EditAttendanceInOutFail);

    },
    EditAttendanceInOutSuccess: function (data) {
        
        var atendaceInOut = data.AtendaceInOut;
        var id = atendaceInOut.typeId;
        if (data.success === true) {
            if (id == atendaceInOut.AtendanceType) {
                if (atendaceInOut.AtendanceType == 40) {
                    $("#myTable >tbody>tr:eq(" + rowIndex + ")").find("td:eq(0)").text(atendaceInOut.Title);
                    $("#myTable >tbody>tr:eq(" + rowIndex + ")").find("td:eq(1)").text(atendaceInOut.start);
                    $("#myTable >tbody>tr:eq(" + rowIndex + ")").find("td:eq(2)").text(atendaceInOut.end);
                    $("#myTable >tbody>tr:eq(" + rowIndex + ")").find("td:eq(3)").text(atendaceInOut.InsalaryCut);
                    $("#myTable >tbody>tr:eq(" + rowIndex + ")").find("td:eq(4)").text(atendaceInOut.OutSalaryCut);
                }
                else if (atendaceInOut.AtendanceType == 41) {
                    $("#MyTable >tbody>tr:eq(" + RowIndex + ")").find("td:eq(0)").text(atendaceInOut.Title);
                    $("#MyTable >tbody>tr:eq(" + RowIndex + ")").find("td:eq(1)").text(atendaceInOut.start);
                    $("#MyTable >tbody>tr:eq(" + RowIndex + ")").find("td:eq(2)").text(atendaceInOut.end);
                    $("#MyTable >tbody>tr:eq(" + RowIndex + ")").find("td:eq(3)").text(atendaceInOut.InSalaryCut);
                    $("#MyTable >tbody>tr:eq(" + RowIndex + ")").find("td:eq(4)").text(atendaceInOut.OutSalaryCut);
                }
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            else {
                window.location.reload();
            }
            AttendanceManager.clearForAttendanceUpdateInformation();
            $("#UpdateAttendanceInOutModal").modal("hide");
        }
        else {
            AppUtil.ShowError("Failed to Edit");
        }

    },
    EditAttendanceInOutFail: function (data) {

        AppUtil.ShowError("Attendance In Out Edit fail");


    },


    DeleteAttenanceInOutGet: function (id) {
        var url = "/Attedance/DeleteAttendanceInOut/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = ({ id: id });
        data = AttendanceManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AttendanceManager.DeleteAttenanceInOutGetsuccess, AttendanceManager.DeleteAttenanceInOutGetFail);
    },
    DeleteAttenanceInOutGetsuccess: function (data) {
        if (data.success === true) {
            $("#AttendaceInOutID").val(data.attendanceInOut.AttendaceInOutID);
            $("#DeleteAttendanceInOutModal").modal("show");
        }
    },
    DeleteAttenanceInOutGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },

    DeleteAttendanceInOut: function () {
        var url = "/Attedance/DeleteAttendanceInOutConfirm/";
        var id = AppUtil.GetIdValue("AttendaceInOutID");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = ({ id: id });
        data = AttendanceManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AttendanceManager.DeleteAttendanceInOutSuccess, AttendanceManager.DeleteAttendanceInOutFail);

    },
    DeleteAttendanceInOutSuccess: function (data) {
        var atendaceInOut = data.at;
        if (data.success === true) {
            if (atendaceInOut.AttendanceTypeID == 40) {
                $("#myTable >tbody>tr:eq(" + rowIndex + ")").remove();
            }
            else if (atendaceInOut.AttendanceTypeID == 41) {
                $("#MyTable >tbody>tr:eq(" + RowIndex + ")").remove();
            }
            AppUtil.ShowSuccess("Successfully Deleted.");
            $("#DeleteAttendanceInOutModal").modal("hide");
        }
        else {
            AppUtil.ShowError("Failed to Delete")
        }

    },
    DeleteAttendanceInOutFail: function (data) {
        
        AppUtil.ShowError("Attendance In Out Edit fail");


    },

    clearForSaveInformation: function () {
        $("#InsertAttendanceTypeName").val("");
    },

    clearForUpdateInformation: function () {
        $("#EditAttenanceType").val("");
    },

    clearForAttendanceSaveInformation: function () {
        $("#insertTitle").val("");
        $("#insertStart").val("");
        $("#insertEnd").val("");
        $("#insertInSalayCut").val("");
        $("#insertOutSalaryCut").val("");
        $("#InsertAttenDanceType").val("");
    },

    clearForAttendanceUpdateInformation: function () {
        $("#UpdateTitle").val("");
        $("#UpdateStart").val("");
        $("#UpdateEnd").val("");
        $("#UpdateInSalayCut").val("");
        $("#UpdateOutSalaryCut").val("");
        $("#UpdateAttenDanceType").val("");
    }

}