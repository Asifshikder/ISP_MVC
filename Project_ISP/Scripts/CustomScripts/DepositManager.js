var DepositManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    DepositCreateValidation: function () {
        if (AppUtil.GetIdValue("AccountList") === '') {
            AppUtil.ShowErrorOnControl("Please Select An Account.", "AccountList","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Date") === '') {
            AppUtil.ShowErrorOnControl("Please Provide Deposit Date.", "Date","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Descriptions") === '') {
            AppUtil.ShowErrorOnControl("Please Wirte Down Description.", "Descriptions","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Head") === '') {
            AppUtil.ShowErrorOnControl("Please Select Company", "Head","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Company") === '') {
            AppUtil.ShowErrorOnControl("Please Select Vendor Type.", "Company","top center");
            return false;
        }
        if (AppUtil.GetIdValue("PayerID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payer", "PayerID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentBy") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payement Method", "PaymentBy","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Status") === '') {
            AppUtil.ShowErrorOnControl("Please Select Status.", "Status","top center");
            return false;
        }

        return true;
    },

    DepositUpdateValidation: function () {
        if (AppUtil.GetIdValue("AccountListID") === '') {
            AppUtil.ShowErrorOnControl("Please Select An Account.", "AccountListID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("DepositDate") === '') {
            AppUtil.ShowErrorOnControl("Please Provide Deposit Date.", "DepositDate","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Description") === '') {
            AppUtil.ShowErrorOnControl("Please Wirte Down Description.", "Description","top center");
            return false;
        }
        if (AppUtil.GetIdValue("Amount") === '') {
            AppUtil.ShowErrorOnControl("Please Insert Amount.", "Amount","top center");
            return false;
        }
        if (AppUtil.GetIdValue("HeadID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Company", "HeadID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Vendor Type.", "CompanyID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("PayerID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payer", "PayerID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("PaymentByID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Payement Method", "PaymentByID","top center");
            return false;
        }
        if (AppUtil.GetIdValue("DepositStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Select Status.", "DepositStatus","top center");
            return false;
        }

        return true;
    },


    InsertDeposit: function () {

        var url = "/Deposit/InsertNewDeposit";
        var AccountListID = AppUtil.GetIdValue("AccountList");
        var DepositDate = AppUtil.GetIdValue("Date");
        var Description = AppUtil.GetIdValue("Descriptions");
        var Amount = AppUtil.GetIdValue("Amount");
        var HeadID = AppUtil.GetIdValue("Head");
        var CompanyID = AppUtil.GetIdValue("Company");
        var PayerID = AppUtil.GetIdValue("PayerID");
        var PaymentByID = AppUtil.GetIdValue("PaymentBy");
        var DepositStatus = AppUtil.GetIdValue("StatusType");
        var References = AppUtil.GetIdValue("References");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Deposit = {
            AccountListID: AccountListID, DepositDate: DepositDate, Description: Description, Amount: Amount,
            HeadID: HeadID, CompanyID: CompanyID, PayerID: PayerID, PaymentByID: PaymentByID, DepositStatus: DepositStatus, References: References
        };

        var formData = new FormData();
        formData.append('DescriptionImage', $('#DescriptionFile')[0].files[0]);
        formData.append('NewDepositInformation', JSON.stringify(Deposit));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, DepositManager.InsertDepositSuccess, DepositManager.InsertDepositFail);

    },
    InsertDepositSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            table.draw();
        }
        if (data.success === false) {

            AppUtil.ShowSuccess("Saved Failed.");

        }
        DepositManager.ClearCreateInformation();
    },
    InsertDepositFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },

    UpdateDeposit: function () {

        var url = "/Deposit/UpdateDeposit";
        var DepositID = AppUtil.GetIdValue("DepositID");
        var AccountListID = AppUtil.GetIdValue("AccountListID");
        var DepositDate = AppUtil.GetIdValue("DepositDate");
        var Description = AppUtil.GetIdValue("Description");
        var Amount = AppUtil.GetIdValue("Amount");
        var HeadID = AppUtil.GetIdValue("HeadID");
        var CompanyID = AppUtil.GetIdValue("CompanyID");
        var PayerID = AppUtil.GetIdValue("PayerID");
        var PaymentByID = AppUtil.GetIdValue("PaymentByID");
        var DepositStatus = AppUtil.GetIdValue("DepositStatus");
        var References = AppUtil.GetIdValue("References");
        var DescriptionFilePath = $("#DescriptionFile").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var Deposit = {
            DepositID: DepositID, AccountListID: AccountListID, DepositDate: DepositDate, Description: Description, Amount: Amount,
            HeadID: HeadID, CompanyID: CompanyID, PayerID: PayerID, PaymentByID: PaymentByID, DepositStatus: DepositStatus, References: References, DescriptionFilePath: DescriptionFilePath
        };

        var formData = new FormData();
        formData.append('DepositUpdateImage', $('#DescriptionFile')[0].files[0]);
        formData.append('DescriptionFilePath', $('#DescriptionFilePath').val());
        formData.append('Deposit_details', JSON.stringify(Deposit));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, DepositManager.UpdateDepositSuccess, DepositManager.UpdateDepositFail);

    },
    UpdateDepositSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Update Successfully.");
            table.draw();
        }
        if (data.success === false) {

            AppUtil.ShowSuccess("Edit Failed.");

        }
        DepositManager.ClearCreateInformation();
    },
    UpdateDepositFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },




    DeleteDeposit: function () {

        var url = "/Deposit/DeleteDeposit/";
        var id = AppUtil.GetIdValue("DepositID");
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ ID: id });
        data = DepositManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, DepositManager.DeleteDepositSuccess, DepositManager.DeleteDepositFailed);
    },
    DeleteDepositSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            window.location.href = '/Deposit/index';
            
        }
        else{
            AppUtil.ShowSuccess("Failed to delete!");
        }

    },
    DeleteDepositFailed: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },


    ClearCreateInformation: function () {
        $("#AccountList").val("");
        $("#Date").val("");
        $("#Descriptions").val("");
        $("#Amount").val("");
        $("#Company").val("");
        $("#PaymentBy").val("");
        $("#StatusType").val("");
        $("#References").val("");
        $("#Head").val("");
        $("#PayerID").val("");
        $("#References").val("");
        $("#PreviewDescriptionFile").attr('src', "#");
        $("#DescriptionFile").wrap('<form>').closest('form').get(0).reset();

    },

}