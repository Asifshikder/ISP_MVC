var MeasurementUnitManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    Validation: function () {
        if (AppUtil.GetIdValue("EditUnitName") === '') {
            AppUtil.ShowSuccess("Please Unit Name.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("InsertMeasureUnitName") === '') {
            AppUtil.ShowSuccess("Please Insert Measurement Unit Name.");
            return false;
        }
        return true;
    },

    clearForSaveInformation: function () {
        $("#InsertMeasureUnitName").val("");
    },
    clearForUpdateInformation: function () {
        $("#EditMeasurementUnitID").val("");
        $("#EditUnitName").val("");

    },

    InsertMeasurementUnit: function (measureUnit) {
        
        var url = "/MeasurementUnit/InsertMeasurementUnit/";
        var UnitName = AppUtil.GetIdValue("InsertMeasureUnitName")
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var measureUnit = { UnitName: UnitName };
        var data = JSON.stringify({ measureUnit: measureUnit });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, MeasurementUnitManager.InsertMeasurementUnitSuccess, MeasurementUnitManager.InsertMeasurementUnitFail);

    },
    InsertMeasurementUnitSuccess: function (data) {
        debugger;
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            $('#tblMeasurementUnit >tbody > tr:last').after('<tr> <td>' + data.UnitInfo.UnitName + '</td><td><a href="" id="showMeasurementUnitForUpdate" class="glyphicon glyphicon-edit btn-circle btn-default"></a><a href="" id="showMeasurementUnitForDelete" class="glyphicon glyphicon-remove btn-circle btn-default"></a></td> </tr>');

        }
        else {
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        }
        MeasurementUnitManager.clearForSaveInformation();
        $("#mdlMeasurementUnitInsert").modal('hide');
        table.draw();
    },
    InsertMeasurementUnitFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    EditMeasurementUnitGet: function (MeasurementUnitID) {

        var url = "/MeasurementUnit/GetMeasurementUnitID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ MeasurementUnitID: MeasurementUnitID });
        datas = MeasurementUnitManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, MeasurementUnitManager.EditMeasurementUnitGetsuccess, MeasurementUnitManager.EditMeasurementUnitGetFail);

    },
    EditMeasurementUnitGetsuccess: function (data) {
        $("#EditMeasurementUnitID").val(data.measureUnit.MeasurementUnitID);
        $("#EditUnitName").val(data.measureUnit.UnitName);
        $("#mdlMeasurementUnitUpdate").modal("show");

    },
    EditMeasurementUnitGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },

    EditMeasurementUnit: function () {
        var url = "/MeasurementUnit/UpdateMeasurementUnit/";
        var MeasurementUnitID = AppUtil.GetIdValue("EditMeasurementUnitID");
        var UnitName = AppUtil.GetIdValue("EditUnitName");
        var UNitDetails = { MeasurementUnitID: MeasurementUnitID, UnitName: UnitName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ UNitDetails: UNitDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, MeasurementUnitManager.EditMeasurementUnitSuccess, MeasurementUnitManager.EditMeasurementUnitFail);


    },
    EditMeasurementUnitSuccess: function (data) {
        var At = data.Units;
        if (data.UpdateSuccess === true) {

            $("#tblMeasurementUnit tbody>tr").each(function () {

                var MeasurementUnitID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (At.MeasurementUnitID == MeasurementUnitID) {
                    $("#tblMeasurementUnit >tbody>tr:eq(" + index + ")").find("td:eq(1)").text(At.UnitName);
                }
            });

            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError("Measurement Unit  Edit fail ");
        }
        MeasurementUnitManager.clearForUpdateInformation();
        $("#mdlMeasurementUnitUpdate").modal("hide");

    },
    EditMeasurementUnitFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },

    DeleteMeasurementUnit: function (MeasurementUnitID) {

        var url = "/MeasurementUnit/DeleteMeasurement/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ MeasurementUnitID: MeasurementUnitID });
        datas = MeasurementUnitManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, MeasurementUnitManager.DeleteMeasurementUnitsuccess, MeasurementUnitManager.DeleteMeasurementUnitFail);

    },
    DeleteMeasurementUnitsuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");


            $("#tblMeasurementUnit tbody>tr").each(function () {

                var MeasurementUnitID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.measureUnitID == MeasurementUnitID) {
                    $("#tblMeasurementUnit >tbody>tr:eq(" + index + ")").remove();
                }
            });
        }
        $("#mdlMeasurementUnitDelete").modal("hide");
    },
    DeleteMeasurementUnitFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

}