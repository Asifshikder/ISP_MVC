AccountOwnerManager = {
    Validation: function () {

        if (AppUtil.GetIdValue("InsertOwnerName") === '') {
            AppUtil.ShowSuccess("Please Insert Owner Name.");
            return false;
        }
        return true;
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    InsertAccountOwner: function () {
        var url = "/AccountOwner/InsertAccountOwner/";
        var OwnerName = AppUtil.GetIdValue("InsertOwnerName");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var OwnerDetails = { OwnerName: OwnerName };
        var data = JSON.stringify({ OwnerDetails: OwnerDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountOwnerManager.InsertAccountOwnerSuccess, AccountOwnerManager.InsertAccountOwnerFail);

    },
    InsertAccountOwnerSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            $('#tblOwner >tbody > tr:last').after('<tr> <td hidden>' + data.accountOwner.OwnerID + '</td><td>' + data.accountOwner.OwnerName + '</td><td><a href="" onclick="ShowForEdit(' + data.accountOwner.OwnerID + ')" class="glyphicon glyphicon-edit btn-circle btn-default"></a><a href = "" onclick ="ShowForDelete(' + data.accountOwner.OwnerID + ')" class="glyphicon glyphicon-remove btn-circle btn-default"></a></td> </tr>');

        }
        else {
            AppUtil.ShowSuccess("Error Occoured!");
        }
        AccountOwnerManager.clearForSaveInformation();
        $("#mdlOwnerInsert").modal('hide');
    },
    InsertAccountOwnerFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


    GetDetailsByID: function (id) {

        var url = "/AccountOwner/GetOwnerByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ OwnerID: id });
        datas = AccountOwnerManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountOwnerManager.GetDetailsByIDsuccess, AccountOwnerManager.GetDetailsByIDFail);

    },
    GetDetailsByIDsuccess: function (data) {
        $("#accountOwnerID").val(data.OwnerInfo.OwnerID);
        $("#EditOwnerName").val(data.OwnerInfo.OwnerName);


    },
    GetDetailsByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    UpdateOwner: function () {
        var url = "/AccountOwner/UpdateOwnerDetails/";
        var OwnerID = AppUtil.GetIdValue("accountOwnerID");
        var OwnerName = AppUtil.GetIdValue("EditOwnerName");
        var OwnerDetails = { OwnerID: OwnerID, OwnerName: OwnerName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ OwnerDetails: OwnerDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountOwnerManager.UpdateOwnerSuccess, AccountOwnerManager.UpdateOwnerFail);


    },
    UpdateOwnerSuccess: function (data) {
        var owner = data.owner;
        if (data.success === true) {

            window.location.reload();
        }
        else {
            AppUtil.ShowError("Failed to Update! ");
        }
        $("#mdlUpdateAccountOwner").modal("hide");
    },
    UpdateOwnerFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },


    DeleteByID: function (id) {

        var url = "/AccountOwner/DeleteOwner/";

        var ID = AppUtil.GetIdValue("accountOwnerID");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: ID });
        datas = AccountOwnerManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountOwnerManager.DeleteByIDsuccess, AccountOwnerManager.DeleteByIDFail);

    },
    DeleteByIDsuccess: function (data) {
        if (data.success === true) {
            window.location.reload();
        }
        else {
            AppUtil.ShowError("Error occured!")
        }
    },
    DeleteByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },

    clearForSaveInformation: function () {
        $("#InsertOwnerName").val("");
    },



}