var HeadManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },




    Validation: function () {

        if (AppUtil.GetIdValue("EditHeadName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        if (AppUtil.GetIdValue("EditHeadTypeID") === '') {
            AppUtil.ShowSuccess("Please Insert Package Price.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("HeadName") === '') {
            AppUtil.ShowSuccess("Please Insert Head Name.");
            return false;
        }
        if (AppUtil.GetIdValue("HeadType") === '') {
            AppUtil.ShowSuccess("Please Select Head Type.");
            return false;
        }
        return true;
    },

    InsertHeadFromPopUp: function () {

        var url = "/Head/InsertHeadFromPopUp/";
        var HeadName = AppUtil.GetIdValue("HeadNameInsert");
        var HeadTypeID = AppUtil.GetIdValue("HeadTypeInsert");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Head = { HeadeName: HeadName, HeadTypeID: HeadTypeID };
        var data = JSON.stringify({ Head: Head });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, HeadManager.InsertHeadFromPopUpSuccess, HeadManager.InsertHeadFromPopUpFail);

    },
    InsertHeadFromPopUpSuccess: function (data) {
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            var HeadType;
            if (data.HeadTypeId == 1) {
                HeadType = "Expense";
            }
            else {
                HeadType = "Income";
            }
                if (data.HeadInfo) {

                //    var PoolName = data.PoolName;

                    var HeadInfo = (data.HeadInfo);
                    $("#tblHead>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + HeadInfo.HeadID + '></td><td>' + HeadInfo.HeadName + '</td><td>' + HeadType + '</td><td> <a href="" id="showHeadForUpdate" class="glyphicon glyphicon-edit btn-circle btn-default"></a><a href="" id="showHeadForDelete" class="glyphicon glyphicon-remove btn-circle btn-default"></a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        HeadManager.clearForSaveInformation();
        $("#mdlInsertHead").modal("hide");
        table.draw();
    },
    InsertHeadFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    EditHeadGet: function (HeadID) {

        var url = "/Head/GetHeadDetailsByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ HeadID: HeadID });
        datas = HeadManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, HeadManager.EditHeadGetsuccess, HeadManager.EditHeadGetFail);

    },
    EditHeadGetsuccess: function (data) {
        $("#EditHeadID").val(data.HeadInfo.HeadID);
        $("#EditHeadName").val(data.HeadInfo.HeadeName);
        $("#EditHeadTypeID").val(data.HeadInfo.HeadTypeID);
        $("#mdlHeadUpdate").modal("show");

    },
    EditHeadGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    EditHeadFromPopUp: function (HeadID) {
        var url = "/Head/UpdateHead/";
        var HeadID = AppUtil.GetIdValue("EditHeadID");
        var HeadName = AppUtil.GetIdValue("EditHeadName");
        var HeadTypeID = AppUtil.GetIdValue("EditHeadTypeID");
        var Head = { HeadID:HeadID,HeadeName: HeadName, HeadTypeID: HeadTypeID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ Head: Head });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, HeadManager.EditHeadFromPopUpSuccess, HeadManager.EditHeadFromPopUpFail);


    },
    EditHeadFromPopUpSuccess: function (data) {
        var HeadInfo = data.HeadInfo;
        var HeadType;
        if (data.HeadTypeId == 1) {
            HeadType = "Expense";
        }
        else {
            HeadType = "Income";
        }
        if (data.success === true) {

            $("#tblHead tbody>tr").each(function () {

                var HeadID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                $("#tblHead tbody>tr:eq(" + index + ")").find("td:eq(1)").text(HeadInfo.HeadName);
                $("#tblHead tbody>tr:eq(" + index + ")").find("td:eq(2)").text(HeadType);
                
            });

            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError(data.message);
        }
        HeadManager.clearForUpdateInformation();
        $("#mdlHeadUpdate").modal("hide");
        table.draw();
    },
    EditHeadFromPopUpFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },

    DeleteHead: function (HeadID) {

        var url = "/Head/DeleteHead/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ HeadID: HeadID });
        datas = HeadManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, HeadManager.DeleteHeadsuccess, HeadManager.DeleteHeadFail);

    },
    DeleteHeadsuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            window.location.reload();
        }
        $("#mdlHeadDelete").modal("hide");
    },
    DeleteHeadFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },



    clearForSaveInformation: function () {
        $("#HeadNameInsert").val("");
        $("#HeadTypeInsert").val("");
    },
    clearForUpdateInformation: function () {
        $("#EditHeadName").val("");
        $("#EditHeadTypeID").val("");
    },
    SearchValidation: function () {
        if (AppUtil.GetIdValue("SearchHeadTypeID") === '') {
            AppUtil.ShowSuccess("Please Head Type for Search.");
            return false;
        }
        return true;

    },
}