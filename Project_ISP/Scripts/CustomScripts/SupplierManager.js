var SupplierManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {

        if (AppUtil.GetIdValue("SupplierName") === '') {
            AppUtil.ShowSuccess("Please Add Supplier Name.");
            return false;
        }
        if (AppUtil.GetIdValue("SupplierAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Supplier Location. ");
            return false;
        }
        if (AppUtil.GetIdValue("SupplierContact") === '') {
            AppUtil.ShowSuccess("Please Insert Supplier Contact. ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {

        if (AppUtil.GetIdValue("SupplierNames") === '') {
            AppUtil.ShowSuccess("Please Add Supplier Name.");
            return false;
        }
        if (AppUtil.GetIdValue("SupplierAddresss") === '') {
            AppUtil.ShowSuccess("Please Insert Supplier Location ");
            return false;
        }
        if (AppUtil.GetIdValue("SupplierContacts") === '') {
            AppUtil.ShowSuccess("Please Insert Supplier Contact. ");
            return false;
        }
        return true;
    },

    InsertSupplierFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Supplier/InsertSupplierFromPopUp";
        var SupplierName = AppUtil.GetIdValue("SupplierName");
        var SupplierAddress = AppUtil.GetIdValue("SupplierAddress");
        var SupplierContact = AppUtil.GetIdValue("SupplierContact");


        //  setTimeout(function () {
        var Supplier = { SupplierName: SupplierName, SupplierAddress: SupplierAddress, SupplierContact: SupplierContact };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Supplier_Client: Supplier });
        AppUtil.MakeAjaxCall(url, "POST", datas, SupplierManager.InsertSupplierFromPopUpSuccess, SupplierManager.InsertSupplierFromPopUpFail);
        // }, 500);
    },
    InsertSupplierFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Supplier) {

                var Supplier = (data.Supplier);
                $("#tblSupplier>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Supplier.SupplierID + '></td><td>' + Supplier.SupplierName + '</td><td>' + Supplier.SupplierAddress + '</td><td><a href="" id="showSupplierForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Supplier Already Added. Choose different Supplier Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Supplier/Index";

    },
    InsertSupplierFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertSupplierFromPurchasePopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Supplier/InsertSupplierFromPopUp";
        var SupplierName = AppUtil.GetIdValue("SupplierName");
        var SupplierAddress = AppUtil.GetIdValue("SupplierAddress");
        var SupplierContact = AppUtil.GetIdValue("SupplierContact");


        //  setTimeout(function () {
        var Supplier = { SupplierName: SupplierName, SupplierAddress: SupplierAddress, SupplierContact: SupplierContact };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Supplier_Client: Supplier });
        AppUtil.MakeAjaxCall(url, "POST", datas, SupplierManager.InsertSupplierFromPurchasePopUpSuccess, SupplierManager.InsertSupplierFromPurchasePopUpFail);
        // }, 500);
    },
    InsertSupplierFromPurchasePopUpSuccess: function (data) {

        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Supplier) {
                var Supplier = (data.Supplier);
                $("#ddlSupplierID").append('<option value="' + Supplier.SupplierID + '">' + Supplier.SupplierName + '</option>');
                $("#ddlSupplierID").val(Supplier.SupplierID);
                $("#address").val(Supplier.SupplierAddress);
            }

            SupplierManager.clearForSaveInformation();
            $("#mdlSupplierInsert").modal('hide');
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Supplier Already Added. Choose different Supplier Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }
        } 
    },
    InsertSupplierFromPurchasePopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertSupplier: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Supplier/InsertSupplier";
        var SupplierName = AppUtil.GetIdValue("SupplierName");
        var SupplierAddress = AppUtil.GetIdValue("SupplierAddress");


        //setTimeout(function () {
        var Supplier = { SupplierName: SupplierName, SupplierAddress: SupplierAddress };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Supplier_Client: Supplier });
        AppUtil.MakeAjaxCall(url, "POST", datas, SupplierManager.InsertSupplierSuccess, SupplierManager.InsertSupplierUpFail);
        // }, 500);
    },
    InsertSupplierSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Supplier) {
            // 
            // var Supplier = (data.Supplier);
            // $("#tblSupplier>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Supplier.SupplierID + '/></td><td>' + Supplier.SupplierName + '</td><td><a href="" id="showSupplierForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Supplier Already Added. Choose different Supplier Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Supplier/Index";
        $("#mdlSupplierInsert").modal('hide');

    },
    InsertSupplierUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ShowSupplierDetailsByIDForUpdate: function (SupplierID) {


        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/Supplier/GetSupplierDetailsByID/";
        var data = { SupplierID: SupplierID };
        data = SupplierManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SupplierManager.ShowSupplierDetailsByIDForUpdateSuccess, SupplierManager.ShowSupplierDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowSupplierDetailsByIDForUpdateSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        var SupplierDetailsJsonParse = (data.SupplierDetails);
        $("#SupplierNames").val(SupplierDetailsJsonParse.SupplierName);
        $("#SupplierAddresss").val(SupplierDetailsJsonParse.SupplierAddress);


        $("#mdlSupplierUpdate").modal("show");
    },
    ShowSupplierDetailsByIDForUpdateError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    ShowSupplierAddresssFromPurchaseByID: function (SupplierID) {


        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/Supplier/GetSupplierDetailsByID/";
        var data = { SupplierID: SupplierID };
        data = SupplierManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SupplierManager.ShowSupplierAddresssFromPurchaseByIDSuccess, SupplierManager.ShowSupplierAddresssFromPurchaseByIDError);

        //}, 500);

    },
    ShowSupplierAddresssFromPurchaseByIDSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        var SupplierDetailsJsonParse = (data.SupplierDetails); 
        $("#address").val(SupplierDetailsJsonParse.SupplierAddress); 
    },
    ShowSupplierAddresssFromPurchaseByIDError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },
    UpdateSupplierInformation: function () {

        //AppUtil.ShowWaitingDialog();
        // var SupplierID = SupplierID;
        var SupplierName = $("#SupplierNames").val();
        var SupplierAddress = $("#SupplierAddresss").val();


        var url = "/Supplier/UpdateSupplier";
        var SupplierInfomation = ({ SupplierID: SupplierID, SupplierName: SupplierName, SupplierAddress: SupplierAddress });
        var data = JSON.stringify({ SupplierInfoForUpdate: SupplierInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, SupplierManager.UpdateSupplierInformationSuccess, SupplierManager.UpdateSupplierInformationFail);
    },
    UpdateSupplierInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.UpdateSuccess === true) {
            var SupplierInformation = (data.SupplierUpdateInformation);

            $("#tblSupplier tbody>tr").each(function () {

                var SupplierID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (SupplierInformation[0].SupplierID == SupplierID) {

                    $('#tblSupplier tbody>tr:eq(' + index + ')').find("td:eq(1)").text(SupplierInformation[0].PackageName);
                    $('#tblSupplier tbody>tr:eq(' + index + ')').find("td:eq(2)").text(SupplierInformation[0].SupplierAddress);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Supplier Already Added. Choose different Supplier. ");
            }
        }

        $("#mdlSupplierUpdate").modal('hide');

        SupplierManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateSupplierInformationFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintSupplierList: function () {

        var url = "/Excel/CreateReportForSupplierList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({});
        data = SupplierManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SupplierManager.PrintSupplierListSuccess, SupplierManager.PrintSupplierListFail);
    },
    PrintSupplierListSuccess: function (data) {

        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;

        //if (data.Success === true) {
        //    AppUtil.ShowSuccess("Employee List Print Successfully.");
        //}
        //if (data.Success === false) {
        //    AppUtil.ShowSuccess("Failed To Print Employee List.");
        //}
    },
    PrintSupplierListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#SupplierName").val("");
        $("#SupplierAddress").val("");
        $("#SupplierContact").val("");
    },
    clearForUpdateInformation: function () {
        $("#SupplierNames").val("");
        $("#SupplierAddresss").val("");
        $("#SupplierContacts").val("");
    }
}