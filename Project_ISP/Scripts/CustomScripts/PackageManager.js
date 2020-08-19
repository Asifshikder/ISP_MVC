var PackageManager = {

    Validation: function () {

        if (AppUtil.GetIdValue("PackageName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        if (AppUtil.GetIdValue("PackagePrice") === '') {
            AppUtil.ShowSuccess("Please Insert Package Price.");
            return false;
        }
        if (AppUtil.GetIdValue("BandWith") === '') {
            AppUtil.ShowSuccess("Please Insert BandWith.");
            return false;
        }
        if (AppUtil.GetIdValue("IPPoolID") === '') {
            AppUtil.ShowSuccess("Please Select IP Pool.");
            return false;
        }
        if (AppUtil.GetIdValue("LocalAddress") === '') {
            AppUtil.ShowSuccess("Please Select Local Address.");
            return false;
        }
        return true;
    },
    CreateValidation: function () {

        if (AppUtil.GetIdValue("CreatePackageName") === '') {
            AppUtil.ShowSuccess("Please Insert Package Name.");
            return false;
        }
        if (AppUtil.GetIdValue("CreatePackagePrice") === '') {
            AppUtil.ShowSuccess("Please Insert Package Price.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateBandWith") === '') {
            AppUtil.ShowSuccess("Please Insert BandWith.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateIPPoolID") === '') {
            AppUtil.ShowSuccess("Please Select IP Pool.");
            return false;
        }
        if (AppUtil.GetIdValue("CreateLocalAddress") === '') {
            AppUtil.ShowSuccess("Please Select Local Address.");
            return false;
        }
        return true;
    },
    UpdateResellerPackageUpdateValidation: function () {

        if (AppUtil.GetIdValue("MyGivenPrice") === '') {
            AppUtil.ShowSuccess("Please Add Package Price.");
            return false;
        }
        return true;
    },

    InsertPackage: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Package/InsertPackage/";
        var PackageName = AppUtil.GetIdValue("PackageName");
        var PackagePrice = AppUtil.GetIdValue("PackagePrice");
        var BandWith = AppUtil.GetIdValue("BandWith");

        //   setTimeout(function () {
        var Package = { PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Package: Package });
        AppUtil.MakeAjaxCall(url, "POST", datas, PackageManager.InsertPackageSuccess, PackageManager.InsertPackageFail);
        //  }, 500);


    },
    InsertPackageSuccess: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        window.location.href = "Package/Index";
        //window.location.href = '/Client/GetAllCLients';
    },
    InsertPackageFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    InsertPackageFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Package/InsertPackage/";
        var PackageForMyOrResellerUser = AppUtil.GetIdValue("ddlCreatePackageFor");
        var PackageName = AppUtil.GetIdValue("CreatePackageName");
        var PackagePrice = AppUtil.GetIdValue("CreatePackagePrice");
        var BandWith = AppUtil.GetIdValue("CreateBandWith");

        //     setTimeout(function () {
        var Package = { PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith, PackageForMyOrResellerUser: PackageForMyOrResellerUser };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Package: Package });
        AppUtil.MakeAjaxCall(url, "POST", datas, PackageManager.InsertPackageFromPopUpSuccess, PackageManager.InsertPackageFromPopUpFail);
        //      }, 500);


    },
    InsertPackageFromPopUpSuccess: function (data) {
        //AppUtil.HideWaitingDialog();


        if (data.Success === false) {
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
        }
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            //if (data.PackageInformation) { 
            //    var PackageInformtion = (data.PackageInformation);
            //    $("#tblPackage>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + PackageInformtion.PackageID + '></td><td>' + PackageInformtion.PackageName + '</td><td>' + PackageInformtion.BandWith + '</td><td>' + PackageInformtion.PackagePrice + '</td><td>' + PackageInformtion.Client + '</td><td><a href="" id="showPackageForUpdate">Show</a></td></tr>');
            //}

            table.draw();
        }

        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Package Already Added. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }
        }
        if (data.packageCount < 1) {
            window.location.href = "Package/Index";
        }
        PackageManager.clearForSaveInformation();
        $("#mdlPackageInsert").modal('hide');
        table.draw();
    },
    InsertPackageFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowPackageDetailsByIDForUpdate: function (PackageID) {
        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';

        //AppUtil.ShowWaitingDialog();
        //    setTimeout(function () {

        var url = "/Package/GetPackageDetailsByID/";
        var data = { packageID: PackageID };
        data = PackageManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PackageManager.ShowPackageDetailsByIDForUpdateSuccess, PackageManager.ShowPackageDetailsByIDForUpdateError);

        //   }, 500);

    },
    ShowPackageDetailsByIDForUpdateSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        var PackageJSONParse = (data.PackageDetails);
        $("#ddlUpdatePackageFor").val(PackageJSONParse.PackageForMyOrResellerUser);
        $("#PackageName").val(PackageJSONParse.PackageName);
        $("#PackagePrice").val(PackageJSONParse.PackagePrice);
        $("#BandWith").val(PackageJSONParse.BandWith);

        $("#mdlPackageUpdate").modal("show");
    },
    ShowPackageDetailsByIDForUpdateError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    ShowPackageDetailsByIDForUpdate: function (ResellerID) {
        var url = "/Reseller/GetMacResellerPackageAndZoneDetailsByResellerID/";
        var data = { resellerid: ResellerID };
        data = PackageManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PackageManager.ShowPackageDetailsByIDForUpdateSuccess, PackageManager.ShowPackageDetailsByIDForUpdateError);
    },
    ShowPackageDetailsByIDForUpdateSuccess: function (data) {
        var zoneList = (data.resellerzone);
        var packageList = (data.resellerpackage);
        var boxList = (data.resellerbox);
        var mikrotikList = (data.resellermikrotik);

        $("#ZoneID").find("option").not(":first").remove();
        $("#PackageID").find("option").not(":first").remove();
        $("#BoxID").find("option").not(":first").remove();
        $("#lstMikrotik").find("option").not(":first").remove();

        $.each(zoneList, function (index, element) {
            $("#ZoneID").append($("<option></option>").val(element.zoneid).text(element.zonename));
        });
        $.each(packageList, function (index, element) {
            $("#PackageID").append($("<option></option>").val(element.packageid).text(element.packagename));
        }); 
        $.each(boxList, function (index, element) {
            $("#BoxID").append($("<option></option>").val(element.boxid).text(element.boxname));
        });
        $.each(mikrotikList, function (index, element) {
            $("#lstMikrotik").append($("<option></option>").val(element.mikid).text(element.mikname));
        });
    },
    ShowPackageDetailsByIDForUpdateError: function (data) {
        console.log(data);
    },

    UpdatePackageInformation: function () {
        //AppUtil.ShowWaitingDialog();
        var PackageID = packageID;
        var PackageForMyOrResellerUser = AppUtil.GetIdValue("ddlUpdatePackageFor");
        var PackageName = $("#PackageName").val();
        var PackagePrice = $("#PackagePrice").val();
        var BandWith = $("#BandWith").val();

        var url = "/Package/UpdateMacResellerPackagePrice";
        var PackageInfomation =
            ({ PackageID: PackageID, PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith, PackageForMyOrResellerUser: PackageForMyOrResellerUser });
        var data = JSON.stringify({ PackageInfoForUpdate: PackageInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, PackageManager.UpdatePackageInformationSuccess, PackageManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        if (data.Success === false) {
            AppUtil.ShowError(data.Message);
        }

        if (data.Success === true) {
            $("#tblResellerPackagePriceSet tbody>tr").each(function () {
                var PackageID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.pid == PackageID) {
                    $('#tblResellerPackagePriceSet tbody>tr:eq(' + index + ')').find("td:eq(1)").text(data.price);
                }
            });
            //table.draw();
            AppUtil.ShowSuccess("Package Update Successfully.");
        }

        if (data.UpdateSuccess === false) {

            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Package Already Added. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Update Failed.");
            }
        }
        PackageManager.clearForUpdateInformation();
        $("#mdlPackageUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    ShowResellerPackageDetailsByIDForUpdate: function (PackageID) {
        var url = "/Package/GetResellerPackageDetailsByID/";
        var data = { packageid: PackageID };
        data = PackageManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PackageManager.ShowResellerPackageDetailsByIDForUpdateSuccess, PackageManager.ShowResellerPackageDetailsByIDForUpdateError);
    },
    ShowResellerPackageDetailsByIDForUpdateSuccess: function (data) {
        var PackageJSONParse = (data.PackageDetails);
        $("#PackageNames").val(PackageJSONParse.PName);
        $("#AdminPrice").val(PackageJSONParse.PPAdmin);
        $("#MyGivenPrice").val(PackageJSONParse.PPFromRS);

        $("#mdlResellerPackagePriceUpdate").modal("show");
    },
    ShowResellerPackageDetailsByIDForUpdateError: function (data) {
        console.log(data);
    },

    UpdateMacResellerPackagePrice: function () {
        //AppUtil.ShowWaitingDialog();
        var PackageID = _PackageID;
        var MyGivenPrice = $("#MyGivenPrice").val();
        var url = "/Package/UpdateMacResellerPackagePrice";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = JSON.stringify({ PID: PackageID, PPFromRS: MyGivenPrice });
        //data = PackageManager.addRequestVerificationToken(data);
        //AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PackageManager.UpdateMacResellerPackagePriceSuccess, PackageManager.UpdateMacResellerPackagePriceFail);
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, PackageManager.UpdateMacResellerPackagePriceSuccess, PackageManager.UpdateMacResellerPackagePriceFail);
    },
    UpdateMacResellerPackagePriceSuccess: function (data) {

        if (data.Success === false) {
            AppUtil.ShowError(data.Message);
        }

        if (data.Success === true) {
            $("#tblResellerPackagePriceSet tbody>tr").each(function () {
                var PackageID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (data.pid == PackageID) {
                    $('#tblResellerPackagePriceSet tbody>tr:eq(' + index + ')').find("td:eq(3)").text(data.price);
                }
            });
            //table.draw();
            AppUtil.ShowSuccess("Package Update Successfully.");
        }

        if (data.Success === false) {
            AppUtil.ShowSuccess("Package Price Update Failed.");
        }
        PackageManager.clearMacResellerPackageUpdateInformation();
        $("#mdlResellerPackagePriceUpdate").modal('hide');
    },
    UpdateMacResellerPackagePriceFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    PrintPackageList: function () {

        var url = "/Excel/CreateReportForPackage";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({});
        data = PackageManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PackageManager.PrintPackageListSuccess, PackageManager.PrintPackageListFail);
    },
    PrintPackageListSuccess: function (data) {

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
    PrintPackageListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#ddlCreatePackageFor").val("");
        $("#CreatePackageName").val("");
        $("#CreatePackagePrice").val("");
        $("#CreateBandWith").val("");
    },
    clearForUpdateInformation: function () {
        $("#ddlUpdatePackageFor").val("");
        $("#PackageName").val("");
        $("#PackagePrice").val("");
        $("#BandWith").val("");
    },
    clearMacResellerPackageUpdateInformation: function () {
        _PackageID = "";

        $("#PackageNames").val("");
        $("#AdminPrice").val("");
        $("#MyGivenPrice").val("");
    }
}