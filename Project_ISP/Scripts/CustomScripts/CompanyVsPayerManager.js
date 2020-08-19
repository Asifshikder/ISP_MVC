var CompanyVsPayerManager = {
    UpdateValidation: function () {

        if (AppUtil.GetIdValue("PackageName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("InsertPayerName") === '') {
            AppUtil.ShowSuccess("Please Insert Payer Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Company") === '') {
            AppUtil.ShowSuccess("Please Select Company.");
            return false;
        }
        return true;
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    InsertCompanyVsPayer: function () {
        var url = "/CompanyVsPayer/InsertCompanyVsPayer/";
        var PayerName = AppUtil.GetIdValue("InsertPayerName");
        var CompanyID = AppUtil.GetIdValue("Company");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var CompanyVsPayer = { PayerName: PayerName, CompanyID: CompanyID };
        var data = JSON.stringify({ CompanyVsPayer: CompanyVsPayer });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, CompanyVsPayerManager.InsertCompanyVsPayerSuccess, CompanyVsPayerManager.InsertCompanyVsPayerFail);

    },
    InsertCompanyVsPayerSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            table.draw();
        }
        else {
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        }
        CompanyVsPayerManager.clearForSaveInformation();
        $("#mdlInsert").modal('hide');

    },
    InsertCompanyVsPayerFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    GetDetailsByID: function (_PayerID) {

        var url = "/CompanyVsPayer/GetDetailsByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ ID: _PayerID });
        datas = CompanyVsPayerManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CompanyVsPayerManager.GetDetailsByIDsuccess, CompanyVsPayerManager.GetDetailsByIDFail);

    },
    GetDetailsByIDsuccess: function (data) {
        $("#UpdatePayerName").val(data.company.PayerName);
        $("#NewCompany").val(data.company.CompanyID);
        $("#mdlUpdate").modal("show");

    },
    GetDetailsByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    EditCompanyVsPayer: function (_PayerID) {
        var url = "/CompanyVsPayer/UpdatePayerInformationFormPopUp/";
        var PayerID = _PayerID;
        var PayerName = AppUtil.GetIdValue("UpdatePayerName");
        var ComanyID = AppUtil.GetIdValue("NewCompany");
        var PayerInfo = { PayerID: PayerID, PayerName: PayerName, CompanyID: ComanyID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ PayerInfo: PayerInfo });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, CompanyVsPayerManager.EditCompanyVsPayerSuccess, CompanyVsPayerManager.EditCompanyVsPayerFail);


    },
    EditCompanyVsPayerSuccess: function (data) {
        if (data.success === true) {
            table.draw();
            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError("Edit fail ");
        }
        $("#mdlUpdate").modal("hide");

    },
    EditCompanyVsPayerFail: function (data) {
        AppUtil.ShowError("Error Occured. Contact With Admninstrator. ");
    },

    DeletePayer: function (_PayerID) {

        var url = "/CompanyVsPayer/DeletePayer/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ ID: _PayerID });
        datas = CompanyVsPayerManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CompanyVsPayerManager.DeletePayersuccess, CompanyVsPayerManager.DeletePayerFail);

    },
    DeletePayersuccess: function (data) {
        if (data.success == true) {
            table.draw();
        }
        $("#mdlDelete").modal("hide");
    },
    DeletePayerFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    clearForSaveInformation: function () {
        $("#InsertPayerName").val("");
        $("#Company").val("");
    },

}