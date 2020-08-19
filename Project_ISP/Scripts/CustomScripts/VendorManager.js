
var VendorManager = {

    VerndorTypeValidation: function () {

        if (AppUtil.GetIdValue("PackageName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        return true;
    },
    CreateVerndorTypeValidation: function () {

        if (AppUtil.GetIdValue("InsertVendorTypeName") === '') {
            AppUtil.ShowSuccess("Please Insert Vendor Type Name.");
            return false;
        }
        return true;
    },
    VerndorVendorValidation: function () {

        if (AppUtil.GetIdValue("PackageName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        return true;
    },
    CreateVerndorValidation: function () {

        if (AppUtil.GetIdValue("Name") === '') {
            AppUtil.ShowSuccess("Please Insert Vendor Name.");
            return false;
        }
        if (AppUtil.GetIdValue("Email") === '') {
            AppUtil.ShowSuccess("Please Insert Vendor Email.");
            return false;
        }
        if (AppUtil.GetIdValue("Address") === '') {
            AppUtil.ShowSuccess("Please Insert Vendor Address.");
            return false;
        }
        if (AppUtil.GetIdValue("VendorCreateImage") === '') {
            AppUtil.ShowSuccess("Please Upload an Image.");
            return false;
        }
        if (AppUtil.GetIdValue("ContactPerson") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Person Information");
            return false;
        }
        if (AppUtil.GetIdValue("VendorType") === '') {
            AppUtil.ShowSuccess("Please Select Vendor Type.");
            return false;
        }
        if (AppUtil.GetIdValue("CompanyName") === '') {
            AppUtil.ShowSuccess("Please Write Down Company Name.");
            return false;
        }

        return true;
    },

    InsertVendorType: function () {
        var url = "/VendorType/InsertVendorType/";
        var VendorTypeName = AppUtil.GetIdValue("InsertVendorTypeName")
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var vendorType = { VendorTypeName: VendorTypeName };
        var data = JSON.stringify({ vendorType: vendorType });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, VendorManager.InsertVendorTypeSuccess, VendorManager.InsertEditVendorTypeGetFail);

    },
    InsertVendorTypeSuccess: function (data) {
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Successfully Inserted");
            $('#tblVendorType >tbody > tr:last').after('<tr> <td>' + data.vendorTypeView.VendorTypeName + '</td><td><a href="" id="showVendorTypeForUpdate" class="glyphicon glyphicon-edit btn-circle btn-default"></a><a href="" id="showVendorTypeForDelete" class="glyphicon glyphicon-remove btn-circle btn-default"></a></td> </tr>');

        }
        else {
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        }
        VendorManager.clearForSaveInformation();
        $("#mdlVendorTypeInsert").modal('hide');
        table.draw();
    },
    InsertEditVendorTypeGetFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    EditVendorTypeGet: function (VendorTypeID) {

        var url = "/VendorType/GetVendorTypeID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ VendorTypeID: VendorTypeID });
        datas = VendorManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, VendorManager.EditVendorTypeGetsuccess, VendorManager.EditVendorTypeFail);

    },
    EditVendorTypeGetsuccess: function (data) {
        $("#EditVendorTypeID").val(data.vendorType.VendorTypeID);
        $("#EditVendorTypeName").val(data.vendorType.VendorTypeName);
        $("#mdlVendorTypeUpdate").modal("show");

    },
    EditVendorTypeFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");

    },


    EditVendorType: function (VendorTypeID) {
        var url = "/VendorType/UpdateVendorType/";
        var VendorTypeID = AppUtil.GetIdValue("EditVendorTypeID");
        var VendorTypeName = AppUtil.GetIdValue("EditVendorTypeName");
        var VendorType = { VendorTypeID: VendorTypeID, VendorTypeName: VendorTypeName };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        data = JSON.stringify({ VendorType: VendorType });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, VendorManager.EditVendorTypeSuccess, VendorManager.EditVendorTypeFail);


    },
    EditVendorTypeSuccess: function (data) {
        var At = data.vendor;
        if (data.UpdateSuccess === true) {

            $("#tblVendorType tbody>tr").each(function () {

                var VendorTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (At.VendorTypeID == VendorTypeID) {
                    $("#tblVendorType >tbody>tr:eq(" + index + ")").find("td:eq(1)").text(At.VendorTypeName);
                }
            });

            AppUtil.ShowSuccess("Successfully Edited");
        }
        else {
            AppUtil.ShowError("Vendor Type Edit fail ");
        }
        VendorManager.clearForUpdateInformation();
        $("#mdlVendorTypeUpdate").modal("hide");

    },
    EditVendorTypeFail: function (data) {
        alert("Error Occured. Contact With Admninstrator. ");
    },

    DeleteVendorType: function (VendorTypeID) {

        var url = "/VendorType/DeleteVendorType/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = ({ VendorTypeID: VendorTypeID });
        datas = VendorManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, VendorManager.DeleteVendorTypesuccess, VendorManager.DeleteVendorTypeFail);

    },
    DeleteVendorTypesuccess: function (data) {
        if (data.DeleteSuccess == true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            $("#tblVendorType tbody>tr").each(function () {

                var VendorTypeID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.VendorTypeID == VendorTypeID) {
                    $("#tblVendorType >tbody>tr:eq(" + index + ")").remove();
                }
            });
        }
        $("#mdlVendorTypeDelete").modal("hide");
    },
    DeleteVendorTypeFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },



    clearForSaveInformation: function () {
        $("#InsertVendorTypeName").val("");
    },
    clearForUpdateInformation: function () {
        $("#EditMeasurementUnitID").val("");
        $("#EditUnitName").val("");

    },
    clearForVendorUpdateInformation: function () {
        _VendorID = "";
        $("#UpdateName").val("");
        $("#UpdateEmail").val("");
        $("#UpdateAddress").val("");
        $("#UpdateCompanyName").val("");
        $("#UpdateContactPerson").val("");
        $("#VendorTypeForUpdate").val("");
        $("#PreviewVendorUpdateImage").attr("src", "");
        $("#VendorUpdateImage").wrap('<form>').closest('form').get(0).reset();
        $("#VendorUpdateImage").unwrap();


        //$("#PreviewVendorUpdateImage").prop("src", "");
        //$("#VendorUpdateImage").val("");
        $("#VendorUpdateImagePath").val("");
    },
    clearForSaveVendorInformation: function () {
        $("#Name").val("");
        $("#Email").val("");
        $("#Address").val("");
        $("#CompanyName").val("");
        $("#ContactPerson").val("");
        $("#VendorType").val("");
        $("#VendorCreateImage").val("");
        $("#VendorCreateImage").wrap('<form>').closest('form').get(0).reset();

    },



    InsertVendorFromPopUp: function () {

        var url = "/Vendor/InsertVendorFromPopUp";
        var VendorName = AppUtil.GetIdValue("Name");
        var VendorEmail = AppUtil.GetIdValue("Email");
        var VendorAddress = AppUtil.GetIdValue("Address");
        var CompanyName = AppUtil.GetIdValue("CompanyName");
        var VendorContactPerson = AppUtil.GetIdValue("ContactPerson");
        var VendorTypeID = AppUtil.GetIdValue("VendorType");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var VendorInformationForInsert = {
            VendorName: VendorName, VendorEmail: VendorEmail, VendorAddress: VendorAddress, CompanyName: CompanyName,
            VendorContactPerson: VendorContactPerson, VendorTypeID: VendorTypeID
        };

        var formData = new FormData();
        formData.append('VendorCreateImage', $('#VendorCreateImage')[0].files[0]);
        formData.append('VendorInformationForInsert', JSON.stringify(VendorInformationForInsert));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, VendorManager.InsertVendorFromPopUpSuccess, VendorManager.InsertVendorFromPopUpFail);

    },
    InsertVendorFromPopUpSuccess: function (data) {
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            table.draw();
            $("#mdlVendorInsert").modal('hide');
        }
        if (data.success === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Vendor Already Added. Choose different Vendor Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        VendorManager.clearForSaveVendorInformation();
    },
    InsertVendorFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    },

    GetVendorDetailsByID: function (VendorID) {

        var url = "/Vendor/GetVendorDetailsByID/";
        var data = ({ VendorID: VendorID });
        data = VendorManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, VendorManager.GetRVendorDetailsByIDSuccess, VendorManager.GetVendorDetailsByIDFailed);
    },
    GetRVendorDetailsByIDSuccess: function (data) {
        if (data.Success === true) {
            $("#EditVendorID").val(data.Vendor.VendorID);
            $("#UpdateName").val(data.Vendor.VendorName);
            $("#UpdateEmail").val(data.Vendor.VendorEmail);
            $("#UpdateAddress").val(data.Vendor.VendorAddress);
            $("#UpdateCompanyName").val(data.Vendor.CompanyName);
            $("#UpdateContactPerson").val(data.Vendor.VendorContactPerson);
            $("#VendorTypeForUpdate").val(data.Vendor.VendorTypeID);
            $("#VendorUpdateImagePath").val(data.Vendor.VendorImagePath);
            //$("#VendorUpdateImage").val(data.Vendor.VendorImagePath);
            $("#PreviewVendorUpdateImage").prop("src", data.Vendor.VendorImagePath);

            $("#mdlVendorUpdate").modal("show");
        }
    },
    GetVendorDetailsByIDFailed: function (data) {

        alert("Fail");
    },




    UpdateVendorInformation: function () {

        var url = "/Vendor/UpdateVendorFromPopUp";
        var VendorID = AppUtil.GetIdValue("EditVendorID");
        var VendorName = AppUtil.GetIdValue("UpdateName");
        var VendorEmail = AppUtil.GetIdValue("UpdateEmail");
        var VendorAddress = AppUtil.GetIdValue("UpdateAddress");
        var CompanyName = AppUtil.GetIdValue("UpdateCompanyName");
        var VendorTypeID = AppUtil.GetIdValue("VendorTypeForUpdate");
        var VendorContactPerson = AppUtil.GetIdValue("UpdateContactPerson");
        var VendorImagePath = $("#VendorUpdateImagePath").val();
        var VendorImages = $("#VendorUpdateImage").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var Vendor = {
            VendorID: VendorID, VendorName: VendorName, VendorEmail: VendorEmail, VendorAddress: VendorAddress, CompanyName: CompanyName,
            VendorContactPerson: VendorContactPerson, VendorImagePath: VendorImagePath, VendorTypeID: VendorTypeID
        };

        var formData = new FormData();
        formData.append('VendorUpdateImage', $('#VendorUpdateImage')[0].files[0]);
        formData.append('VendorImagePath', $('#VendorUpdateImagePath').val());
        formData.append('Vendor_details', JSON.stringify(Vendor));

        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, VendorManager.UpdateVendorFromPopUpSuccess, VendorManager.UpdateVendorFromPopUpFail);


    },
    UpdateVendorFromPopUpSuccess: function (data) {


        if (data.UpdateSuccess === true) {
            var vendor = (data.vendor);
            table.draw();
            //$("#tblVendor tbody>tr").each(function () {

            //    var VendorID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    if (vendor.VendorID == VendorID) {

            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(1)").text(vendor.VendorName);
            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(2)").text(vendor.CompanyName);
            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(3)").text(vendor.VendorEmail);
            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(4)").text(vendor.VendorAddress);
            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(5)").text(vendor.vendorTypeName);
            //        $('#tblVendor tbody>tr:eq(' + index + ')').find("td:eq(6)").text(vendor.VendorContactPerson);

            //    }
            //});
            AppUtil.ShowSuccess("Update Successfully. ");
        }

        VendorManager.clearForVendorUpdateInformation();
        $("#mdlVendorUpdate").modal('hide');
    },
    UpdateVendorFromPopUpFail: function () {
        AppUtil.ShowSuccess("Error! Contact with Administator")
    },



    DeleteVendorFromPopUp: function (VendorID) {
        var url = "/Vendor/DeleteVendor/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var data = ({ VendorID: VendorID });
        data = VendorManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, VendorManager.DeleteVendorFromPopUpSuccess, VendorManager.DeleteVendorFromPopUpFailed);
    },
    DeleteVendorFromPopUpSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Successfully Deleted!");
            $("#tblVendor tbody>tr").each(function () {

                var VendorID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.VendorID == VendorID) {
                    $("#tblVendor >tbody>tr:eq(" + index + ")").remove();
                }
            });
        }
        $("#mdlVendorDelete").modal("hide");

    },
    DeleteVendorFromPopUpFailed: function (data) {

        alert("Fail");
    },
}