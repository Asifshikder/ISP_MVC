var AccountListManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    Validation: function () {

        if (AppUtil.GetIdValue("InsertOwnerName") === '') {
            AppUtil.ShowSuccess("Please Insert Owner Name.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("account") === '') {
            AppUtil.ShowSuccess("Please Insert Account Title.");
            return false;
        }
        if (AppUtil.GetIdValue("description") === '') {
            AppUtil.ShowSuccess("Please Insert Description.");
            return false;
        }
        if (AppUtil.GetIdValue("balance_USD") === '') {
            AppUtil.ShowSuccess("Please Insert Initial Balance.");
            return false;
        }
        if (AppUtil.GetIdValue("account_number") === '') {
            AppUtil.ShowSuccess("Please Insert Account Number.");
            return false;
        }
        if (AppUtil.GetIdValue("contact_person") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Person.");
            return false;
        }
        if (AppUtil.GetIdValue("contact_phone") === '') {
            AppUtil.ShowSuccess("Please Insert Phone Number.");
            return false;
        }
        if (AppUtil.GetIdValue("ib_url") === '') {
            AppUtil.ShowSuccess("Please Insert Bank URL.");
            return false;
        }
        if (AppUtil.GetIdValue("Owner") === '') {
            AppUtil.ShowSuccess("Please Select Owner.");
            return false;
        }
        return true;
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    InsertAccountList: function () {
        var url = "/AccountList/CreateConfirm/";
        var AccountTitle = AppUtil.GetIdValue("account");
        var Description = AppUtil.GetIdValue("description");
        var InitialBalance = AppUtil.GetIdValue("balance_USD");
        var AccountNumber = AppUtil.GetIdValue("account_number");
        var ContactPerson = AppUtil.GetIdValue("contact_person");
        var Phone = AppUtil.GetIdValue("contact_phone");
        var BankUrl = AppUtil.GetIdValue("ib_url");
        var OwnerID = AppUtil.GetIdValue("Owner");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var AccountDetails = { AccountTitle: AccountTitle, Description: Description, InitialBalance: InitialBalance, AccountNumber: AccountNumber, ContactPerson: ContactPerson, Phone: Phone, BankUrl: BankUrl, OwnerID: OwnerID };
        var data = JSON.stringify({ accountlist: AccountDetails });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListManager.InsertAccountListSuccess, AccountListManager.InsertAccountListFail);

    },
    InsertAccountListSuccess: function (data) {
        if (data.nameExist === true) {
            AppUtil.ShowSuccess("Account already exist. Try Different account!");
        }
        else {
            if (data.success === true) {
                AppUtil.ShowSuccess("Successfully Inserted");
                window.location.href = "/accountlist/index/";
            }
            else {
                AppUtil.ShowSuccess("Error Occoured!");
            }
            AccountListManager.clearForSaveInformation();
        }
       
    },
    InsertAccountListFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },


    GetInitialBalanceByID: function (id) {

        var url = "/AccountList/GetInitialBalanceByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: id });
        datas = AccountListManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountListManager.GetInitialBalanceByIDsuccess, AccountListManager.GetInitialBalanceByIDFail);

    },
    GetInitialBalanceByIDsuccess: function (data) {
        $("#accountTile").html("<h4 class='modal-title'>" + data.Accounts.AccountTitle + "</h4>");
        $("#AccountId").val(data.Accounts.AccountListID);
        $("#Initialbalance_USD").val(data.Accounts.InitialBalance);


    },
    GetInitialBalanceByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    UpdateInitialBalance: function () {
        var url = "/AccountList/UpdateInitialBalance/";
        var AccountListID = AppUtil.GetIdValue("AccountId");
        var InitialBalance = AppUtil.GetIdValue("Initialbalance_USD");
        var InitBalance = { AccountListID: AccountListID, InitialBalance: InitialBalance };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ accountlist: InitBalance });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListManager.UpdateInitialBalanceSuccess, AccountListManager.UpdateInitialBalanceFail);


    },
    UpdateInitialBalanceSuccess: function (data) {
        if (data.success === true) {

            window.location.reload();
        }
        else {
            AppUtil.ShowError("Failed to Update! ");
        }
        $("#mdlInitialBalance").modal("hide");
    },
    UpdateInitialBalanceFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },
    
    GetInitialBalanceByID: function (id) {

        var url = "/AccountList/GetInitialBalanceByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: id });
        datas = AccountListManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountListManager.GetInitialBalanceByIDsuccess, AccountListManager.GetInitialBalanceByIDFail);

    },
    GetInitialBalanceByIDsuccess: function (data) {
        $("#accountTile").html("<h4 class='modal-title'>" + data.Accounts.AccountTitle + "</h4>");
        $("#AccountId").val(data.Accounts.AccountListID);
        $("#Initialbalance_USD").val(data.Accounts.InitialBalance);


    },
    GetInitialBalanceByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    //UpdateInitialBalance: function () {
    //    var url = "/AccountList/UpdateInitialBalance/";
    //    var AccountListID = AppUtil.GetIdValue("AccountId");
    //    var InitialBalance = AppUtil.GetIdValue("Initialbalance_USD");
    //    var InitBalance = { AccountListID: AccountListID, InitialBalance: InitialBalance };

    //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
    //    var header = {};
    //    header['__RequestVerificationToken'] = AntiForgeryToken;

    //    data = JSON.stringify({ InitBalance: InitBalance });//accountlist
    //    AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListManager.UpdateInitialBalanceSuccess, AccountListManager.UpdateInitialBalanceFail);


    //},
    //UpdateInitialBalanceSuccess: function (data) {
    //    if (data.success === true) {

    //        window.location.reload();
    //    }
    //    else {
    //        AppUtil.ShowError("Failed to Update! ");
    //    }
    //    $("#mdlInitialBalance").modal("hide");
    //},
    //UpdateInitialBalanceFail: function (data) {
    //    alert("Error Occured. Contact With Admninstrator. ");
    //},
    
    GetDetailsByID: function (id) {

        var url = "/AccountList/GetDetailsByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: id });
        datas = AccountListManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountListManager.GetDetailsByIDsuccess, AccountListManager.GetDetailsByIDFail);

    },
    GetDetailsByIDsuccess: function (data) {
        $("#AccountId").val(data.Accounts.AccountListID);
        $("#account").val(data.Accounts.AccountTitle);
        $("#description").val(data.Accounts.Description);
        $("#balance_USD").val(data.Accounts.InitialBalance);
        $("#account_number").val(data.Accounts.AccountNumber);
        $("#contact_person").val(data.Accounts.ContactPerson);
        $("#contact_phone").val(data.Accounts.Phone);
        $("#ib_url").val(data.Accounts.BankUrl);
        $("#Owner").val(data.Accounts.OwnerID);


    },
    GetDetailsByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    UpdateAccountList: function () {
        var url = "/AccountList/UpdateAccountList/";
        var AccountListID = AppUtil.GetIdValue("AccountId");
        var AccountTitle = AppUtil.GetIdValue("account");
        var Description = AppUtil.GetIdValue("description");
        var InitialBalance = AppUtil.GetIdValue("balance_USD");
        var AccountNumber = AppUtil.GetIdValue("account_number");
        var ContactPerson = AppUtil.GetIdValue("contact_person");
        var Phone = AppUtil.GetIdValue("contact_phone");
        var BankUrl = AppUtil.GetIdValue("ib_url");
        var OwnerID = AppUtil.GetIdValue("Owner");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var AccountInfo = { AccountListID:AccountListID,AccountTitle: AccountTitle, Description: Description, InitialBalance: InitialBalance, AccountNumber: AccountNumber, ContactPerson: ContactPerson, Phone: Phone, BankUrl: BankUrl, OwnerID: OwnerID };

        data = JSON.stringify({ AccountInfo: AccountInfo });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, AccountListManager.UpdateAccountListSuccess, AccountListManager.UpdateAccountListFail);


    },
    UpdateAccountListSuccess: function (data) {
        if (data.success === true) {

            window.location.reload();
        }
        else {
            AppUtil.ShowError("Failed to Update! ");
        }
        $("#mdlUpdateAccountList").modal("hide");
    },
    UpdateAccountListFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },


    DeleteByID: function (id) {

        var url = "/AccountList/DeleteAccount/";

        var ID = AppUtil.GetIdValue("AccountId");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: ID });
        datas = AccountListManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AccountListManager.DeleteByIDsuccess, AccountListManager.DeleteByIDFail);

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
        $("#account").val("");
        $("#description").val("");
        $("#balance_USD").val("");
        $("#account_number").val("");
        $("#contact_person").val("");
        $("#contact_phone").val("");
        $("#ib_url").val("");
        $("#Owner").val("");
    },

}