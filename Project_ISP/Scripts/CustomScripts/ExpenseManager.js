var ExpenseManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    ExpenseCreateValidation: function () {
        if (AppUtil.GetIdValue("AccountList") === '') {
            AppUtil.ShowErrorOnControl("Please Select An Account.", "AccountList", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Date") === '') {
            AppUtil.ShowErrorOnControl("Please Provide Expense Date.", "Date", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Descriptions") === '') {
            AppUtil.ShowErrorOnControl("Please Wirte Down Description.", "Descriptions", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Head") === '') {
            AppUtil.ShowErrorOnControl("Please Select Company", "Head", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Company") === '') {
            AppUtil.ShowErrorOnControl("Please Select Vendor Type.", "Company", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PayerID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payer", "PayerID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentBy") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payement Method", "PaymentBy", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("StatusType") === '') {
            AppUtil.ShowErrorOnControl("Please Select Status.", "StatusType", "top center");
            return false;
        }

        return true;
    },

    ExpenseUpdateValidation: function () {
        if (AppUtil.GetIdValue("AccountListID") === '') {
            AppUtil.ShowErrorOnControl("Please Select An Account.", "AccountListID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentDate") === '') {
            AppUtil.ShowErrorOnControl("Please Provide Expense Date.", "PaymentDate", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Descriptions") === '') {
            AppUtil.ShowErrorOnControl("Please Wirte Down Description.", "Descriptions", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("HeadID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Company", "HeadID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Vendor Type.", "CompanyID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PayerID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payer", "PayerID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentByID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payement Method", "PaymentByID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("ExpenseStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Select Status.", "ExpenseStatus", "top center");
            return false;
        }

        return true;
    },


    InsertExpense: function () {

        var url = "/Expense/InsertNewExpense";
        var AccountListID = AppUtil.GetIdValue("AccountList");
        var ExpenseDate = AppUtil.GetIdValue("Date");
        var Description = AppUtil.GetIdValue("Description");
        var Amount = AppUtil.GetIdValue("Amount");
        var HeadID = AppUtil.GetIdValue("Head");
        var CompanyID = AppUtil.GetIdValue("Company");
        var PayerID = AppUtil.GetIdValue("PayerID");
        var PaymentByID = AppUtil.GetIdValue("PaymentBy");
        var ExpenseStatus = AppUtil.GetIdValue("StatusType");
        var References = AppUtil.GetIdValue("References");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Expense = {
            AccountListID: AccountListID,PaymentDate :ExpenseDate , Descriptions: Description, Amount: Amount,
            HeadID: HeadID, CompanyID: CompanyID, PayerID: PayerID, PaymentByID: PaymentByID, ExpenseStatus: ExpenseStatus, References: References
        };

        var formData = new FormData();
        formData.append('DescriptionImage', $('#DescriptionFile')[0].files[0]);
        formData.append('NewExpenseInformation', JSON.stringify(Expense));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ExpenseManager.InsertExpenseSuccess, ExpenseManager.InsertExpenseFail);

    },
    InsertExpenseSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            table.draw();
        }
        if (data.success === false) {

            AppUtil.ShowSuccess("Saved Failed.");

        }
        ExpenseManager.ClearCreateInformation();
    },
    InsertExpenseFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },

    UpdateExpense: function () {

        var url = "/Expense/UpdateExpense";
        var ExpenseID = AppUtil.GetIdValue("ExpenseID");
        var AccountListID = AppUtil.GetIdValue("AccountListID");
        var PaymentDate = AppUtil.GetIdValue("PaymentDate");
        var Descriptions = AppUtil.GetIdValue("Descriptions");
        var Amount = AppUtil.GetIdValue("Amount");
        var HeadID = AppUtil.GetIdValue("HeadID");
        var CompanyID = AppUtil.GetIdValue("CompanyID");
        var PayerID = AppUtil.GetIdValue("PayerID");
        var PaymentByID = AppUtil.GetIdValue("PaymentByID");
        var ExpenseStatus = AppUtil.GetIdValue("ExpenseStatus");
        var References = AppUtil.GetIdValue("References");
        var DescriptionFilePath = $("#DescriptionFile").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Expense = {
            ExpenseID: ExpenseID, AccountListID: AccountListID, PaymentDate: PaymentDate, Descriptions: Descriptions, Amount: Amount,
            HeadID: HeadID, CompanyID: CompanyID, PayerID: PayerID, PaymentByID: PaymentByID, ExpenseStatus: ExpenseStatus, References: References, DescriptionFilePath: DescriptionFilePath
        };

        var formData = new FormData();
        formData.append('ExpenseUpdateImage', $('#DescriptionFile')[0].files[0]);
        formData.append('DescriptionFilePath', $('#DescriptionFilePath').val());
        formData.append('Expense_details', JSON.stringify(Expense));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ExpenseManager.UpdateExpenseSuccess, ExpenseManager.UpdateExpenseFail);

    },
    UpdateExpenseSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Update Successfully.");
            table.draw();
        }
        if (data.success === false) {

            AppUtil.ShowSuccess("Edit Failed.");

        }
        ExpenseManager.ClearCreateInformation();
    },
    UpdateExpenseFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },




    DeleteExpense: function () {

        var url = "/Expense/DeleteExpense/";
        var id = AppUtil.GetIdValue("ExpenseID");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: id });
        data = ExpenseManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ExpenseManager.DeleteExpenseSuccess, ExpenseManager.DeleteExpenseFailed);
    },
    DeleteExpenseSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            window.location.href='/expense/index';
        }
        else {
            AppUtil.ShowSuccess("Failed to delete!");
        }

    },
    DeleteExpenseFailed: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },



    ClearCreateInformation: function () {
        $("#AccountList").val("");
        $("#Date").val("");
        $("#Description").val("");
        $("#Amount").val("");
        $("#Company").val("");
        $("#PaymentBy").val("");
        $("#StatusType").val("");
        $("#References").val("");
        $("#Head").val("");
        $("#PayerID").val("");
        $("#References").val("");
        $("#PreviewDescriptioFile").attr('src', "#");
        $("#DescriptionFile").wrap('<form>').closest('form').get(0).reset();

    },

}