var AccountListVsAmountTransferManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("FromAccountListID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Account.", "FromAccountListID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("ToAccountListID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Account.", "ToAccountListID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Date") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Date.", "Date", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Descriptions") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Description.", "Descriptions", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("CurrencyID") === '') {
        //    AppUtil.ShowErrorOnControl("Please Select Currency.", "CurrencyID", "top center");
        //    return false;
        //}
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentBy") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payment Method.", "PaymentBy", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("References") === '') {
            AppUtil.ShowErrorOnControl("Please Insert References.", "References", "top center");
            return false;
        }
        return true;
    },
    UpdateValidation: function () {

        if (AppUtil.GetIdValue("AccountListID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Account.", "AccountListID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("ToAccountID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Account.", "ToAccountID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("TransferDate") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Date.", "TransferDate", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Description") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Description.", "Description", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("CurrencyID") === '') {
        //    AppUtil.ShowErrorOnControl("Please Select Currency.", "CurrencyID", "top center");
        //    return false;
        //}
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentByID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payment Method.", "PaymentByID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("References") === '') {
            AppUtil.ShowErrorOnControl("Please Insert References.", "References", "top center");
            return false;
        }
        return true;
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    InsertAccountListVsAmountTransfer: function () {
        var url = "/AccountListVsAmountTransfer/InsertAccountListVsAmountTransfer/";
        var AccountListID = AppUtil.GetIdValue("FromAccountListID");
        var ToAccountID = AppUtil.GetIdValue("ToAccountListID");
        var TransferDate = AppUtil.GetIdValue("Date");
        var Description = AppUtil.GetIdValue("Descriptions");
        var CurrencyID = AppUtil.GetIdValue("CurrencyID");
        var Amount = AppUtil.GetIdValue("Amount");
        var PaymentByID = AppUtil.GetIdValue("PaymentBy");
        var References = AppUtil.GetIdValue("References");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var AccountListVsAmountTransferDetails = { AccountListID: AccountListID, ToAccountID: ToAccountID, TransferDate: TransferDate, Description: Description, CurrencyID: CurrencyID, Amount: Amount, PaymentByID: PaymentByID, References: References };
        var data = JSON.stringify({ AccountListVsAmountTransferDetails: AccountListVsAmountTransferDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListVsAmountTransferManager.InsertAccountListVsAmountTransferSuccess, AccountListVsAmountTransferManager.InsertAccountListVsAmountTransferFail);

    },
    InsertAccountListVsAmountTransferSuccess: function (data) {

        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            table.draw();
        }
        else {
            AppUtil.ShowSuccess("Error Occoured!");
        }
        AccountListVsAmountTransferManager.clearForSaveInformation();

    },

    InsertAccountListVsAmountTransferFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },



    UpdateAccountListVsAmountTransfer: function () {
        var url = "/AccountListVsAmountTransfer/UpdateAccountListVsAmountTransfer/";
        var AccountListVsAmountTransferID = AppUtil.GetIdValue("AccountListVsAmountTransferID");
        var AccountListID = AppUtil.GetIdValue("AccountListID");
        var ToAccountID = AppUtil.GetIdValue("ToAccountID");
        var TransferDate = AppUtil.GetIdValue("TransferDate");
        var Description = AppUtil.GetIdValue("Description");
        var CurrencyID = AppUtil.GetIdValue("CurrencyID");
        var Amount = AppUtil.GetIdValue("Amount");
        var PaymentByID = AppUtil.GetIdValue("PaymentByID");
        var References = AppUtil.GetIdValue("References");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var AccountListVsAmountTransferDetails = { AccountListVsAmountTransferID: AccountListVsAmountTransferID, AccountListID: AccountListID, ToAccountID: ToAccountID, TransferDate: TransferDate, Description: Description, CurrencyID: CurrencyID, Amount: Amount, PaymentByID: PaymentByID, References: References };
        var data = JSON.stringify({ accountlistvsamounttransferdetails: AccountListVsAmountTransferDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListVsAmountTransferManager.UpdateAccountListVsAmountTransferSuccess, AccountListVsAmountTransferManager.UpdateAccountListVsAmountTransferFail);


    },
    UpdateAccountListVsAmountTransferSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Updated!");
            table.draw();
        }
        else {
            AppUtil.ShowError("Failed to Update! ");
        }
    },
    UpdateAccountListVsAmountTransferFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },


    DeleteByID: function (id) {

        var url = "/AccountListVsAmountTransfer/DeleteTransfer/";

        var id = AppUtil.GetIdValue("AccountListVsAmountTransferID");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ id: id });
        datas = AccountListVsAmountTransferManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountListVsAmountTransferManager.DeleteByIDsuccess, AccountListVsAmountTransferManager.DeleteByIDFail);

    },
    DeleteByIDsuccess: function (data) {
        if (data.success === true) {
            window.location.href = '/AccountListVsAmountTransfer/index';
        }
        else {
            AppUtil.ShowError(data.message)
        }
    },
    DeleteByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },

    clearForSaveInformation: function () {
        $("#FromAccountListID").val("");
        $("#ToAccountListID").val("");
        $("#Date").val("");
        $("#Descriptions").val("");
        $("#CurrencyID").val("");
        $("#Amount").val("");
        $("#PaymentBy").val("");
        $("#References").val("");
    },

}