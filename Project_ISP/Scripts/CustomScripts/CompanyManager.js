var CompanyManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("CompanyName") === '') {
            AppUtil.ShowSuccess("Please write down company name.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyEmail") === '') {
            AppUtil.ShowSuccess("Please write down company Email.");
            return false;
        }
        if (AppUtil.GetIdValue("ContactPerson") === '') {
            AppUtil.ShowSuccess("Please write down Contact Person Information");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyAddress") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Address.");
            return false;
        }
        if (AppUtil.GetIdValue("Phone") === '') {
            AppUtil.ShowSuccess("Please Write Down Phone Number.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyCreateImage") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Address.");
            return false;
        }

        return true;
    },
    UpdateValidation: function () {

        if (AppUtil.GetIdValue("UpdateCompanyName") === '') {
            AppUtil.ShowSuccess("Please write down company name.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateCompanyEmail") === '') {
            AppUtil.ShowSuccess("Please write down company Email.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateContactPerson") === '') {
            AppUtil.ShowSuccess("Please write down Contact Person Information");
            return false;
        }
        if (AppUtil.GetIdValue("UpdateCompanyAddress") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Address.");
            return false;
        }
        if (AppUtil.GetIdValue("UpdatePhone") === '') {
            AppUtil.ShowSuccess("Please Write Down Phone Number.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyUpdateImage") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Address.");
            return false;
        }

        return true;
    },


    InsertCompanyFromPopUp: function () {

        var url = "/Company/InsertCompanyFromPopUp";
        var CompanyName = AppUtil.GetIdValue("CompanyName");
        var CompanyEmail = AppUtil.GetIdValue("CompanyEmail");
        var CompanyAddress = AppUtil.GetIdValue("CompanyAddress");
        var Phone = AppUtil.GetIdValue("Phone");
        var ContactPerson = AppUtil.GetIdValue("ContactPerson");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var CompanyInfo = {
            CompanyName: CompanyName, CompanyEmail: CompanyEmail, CompanyAddress: CompanyAddress, Phone: Phone, ContactPerson: ContactPerson
        };

        var formData = new FormData();
        formData.append('CompanyImage', $('#CompanyCreateImage')[0].files[0]);
        formData.append('CompanyDetails', JSON.stringify(CompanyInfo));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyManager.InsertCompanyFromPopUpSuccess, CompanyManager.InsertCompanyFromPopUpFail);

    },
    InsertCompanyFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            CompanyManager.clearForSaveVendorInformation();
            table.draw();
            $("#mdlCompanyInsert").modal('hide');
        }
        if (data.success === false) {

            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Company already exist. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        
    },
    InsertCompanyFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },



    GetCompanyDetailsByID: function (_CompanyID) {

        var url = "/Company/GetDetailsByID/";
        var data = ({ CompanyID: _CompanyID });
        data = CompanyManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyManager.GetCompanyDetailsByIDSuccess, CompanyManager.GetCompanyDetailsByIDFailed);
    },
    GetCompanyDetailsByIDSuccess: function (data) {
        if (data.success === true) {
            $("#UpdateCompanyName").val(data.Company.CompanyName);
            $("#UpdateCompanyEmail").val(data.Company.CompanyEmail);
            $("#UpdateCompanyAddress").val(data.Company.CompanyAddress);
            $("#UpdateContactPerson").val(data.Company.ContactPerson);
            $("#UpdatePhone").val(data.Company.Phone);
            var a = '' + data.Company.CompanyLogoPath + '#' + new Date().getTime();

            $("#PreviewCompanyUpdateImage").hide(0).attr('src', '' + data.Company.CompanyLogoPath + '#' + new Date().getTime()).show(0);
            $("#PreviewCompanyUpdateImage").prop("src", data.Company.CompanyLogoPath);

            $("#mdlCompanyUpdate").modal("show");
        }
    },
    GetCompanyDetailsByIDFailed: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },




    UpdateCompanyInformation: function (_CompanyID) {

        var url = "/Company/UpdateCompanyFromPopUp";
        var CompanyID = _CompanyID;
        var CompanyName = AppUtil.GetIdValue("UpdateCompanyName");
        var CompanyEmail = AppUtil.GetIdValue("UpdateCompanyEmail");
        var COmpanyAddress = AppUtil.GetIdValue("UpdateCompanyAddress");
        var ContactPerson = AppUtil.GetIdValue("UpdateContactPerson");
        var Phone = AppUtil.GetIdValue("UpdatePhone");
        var CompanyImagePath = $("#CompanyUpdateImage").val();
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var CompanyInfo = {
            CompanyID: CompanyID, CompanyName: CompanyName, CompanyEmail: CompanyEmail, CompanyAddress: COmpanyAddress, Phone: Phone, ContactPerson: ContactPerson, CompanyLogoPath:CompanyImagePath
        };

        var formData = new FormData();
        formData.append('CompanyImageUpdate', $('#CompanyUpdateImage')[0].files[0]);

        formData.append('Company_details', JSON.stringify(CompanyInfo));
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, CompanyManager.UpdateCompanyFromPopUpSuccess, CompanyManager.UpdateCompanyFromPopUpFail);


    },
    UpdateCompanyFromPopUpSuccess: function (data) {

        if (data.success === true) {
            AppUtil.ShowSuccess("Update Successfully. ");
            table.draw();
            $("#mdlCompanyUpdate").modal('hide');
        }
        if (data.success === false) {
            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Company already exist. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Update Failed.");
            }
        }
    },
    UpdateCompanyFromPopUpFail: function () {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },



    DeleteCompanyFromPopUp: function (_CompanyID) {

        var url = "/Company/CompanyDelete/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ CompanyID: _CompanyID });
        data = CompanyManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CompanyManager.DeleteCompanyFromPopUpSuccess, CompanyManager.DeleteCompanyFromPopUpFailed);
    },
    DeleteCompanyFromPopUpSuccess: function (data) {
        if (data.success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            table.draw();
        }
        $("#mdlCompanyDelete").modal("hide");

    },
    DeleteCompanyFromPopUpFailed: function (data) {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },

    clearForSaveVendorInformation: function () {
        $("#CompanyName").val("");
        $("#CompanyEmail").val("");
        $("#CompanyAddress").val("");
        $("#ContactPerson").val("");
        $("#Phone").val(""); 
        //$("#CompanyCreateImage").val("");
        //$("#PreviewCompanyCreateImage").wrap('<form>').closest('form').get(0).reset();
        //$("#CompanyCreateImage").wrap('<form>').closest('form').get(0).reset(); 

        $("#PreviewCompanyCreateImage").attr("src", "");
        $("#CompanyCreateImage").wrap('<form>').closest('form').get(0).reset();
        $("#CompanyCreateImage").unwrap();
    },
}