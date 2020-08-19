var MIkrotikPackageManager = {

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

    InsertPackage: function () {
        
        //AppUtil.ShowWaitingDialog();

        var url = "/MIkrotikPackage/InsertPackage/";
        var PackageName = AppUtil.GetIdValue("PackageName");
        var PackagePrice = AppUtil.GetIdValue("PackagePrice");
        var BandWith = AppUtil.GetIdValue("BandWith");
        var PackagePrice = AppUtil.GetIdValue("PackagePrice");
        var BandWith = AppUtil.GetIdValue("BandWith");

        //   setTimeout(function () {
        var Package = { PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ Package: Package });
        AppUtil.MakeAjaxCall(url, "POST", datas, MIkrotikPackageManager.InsertPackageSuccess, MIkrotikPackageManager.InsertPackageFail);
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
        window.location.href = "MIkrotikPackage/Index";
        //window.location.href = '/Client/GetAllCLients';
    },
    InsertPackageFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    InsertPackageFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();

        var url = "/MIkrotikPackage/InsertPackage/";
        var PackageName = AppUtil.GetIdValue("CreatePackageName");
        var PackagePrice = AppUtil.GetIdValue("CreatePackagePrice");
        var BandWith = AppUtil.GetIdValue("CreateBandWith");
        var IPPoolID = AppUtil.GetIdValue("CreateIPPoolID");
        var LocalAddress = AppUtil.GetIdValue("CreateLocalAddress");
        var PackageForMyOrResellerUser = AppUtil.GetIdValue("ddlCreatePackageFor");

        var MikrotikID = AppUtil.GetIdValue("CreateMikrotikID");

        //     setTimeout(function () {
        var Package = { PackageForMyOrResellerUser: PackageForMyOrResellerUser, PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith, IPPoolID: IPPoolID, LocalAddress: LocalAddress, MikrotikID: MikrotikID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ Package: Package });
        AppUtil.MakeAjaxCall(url, "POST", datas, MIkrotikPackageManager.InsertPackageFromPopUpSuccess, MIkrotikPackageManager.InsertPackageFromPopUpFail);
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
            if (data.PackageInformation) {
                
                //    var PoolName = data.PoolName;

                //var PackageInformtion = (data.PackageInformation);
                //$("#tblPackage>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + PackageInformtion.PackageID + '></td><td>' + PackageInformtion.PackageName + '</td><td>' + PackageInformtion.BandWith + '</td><td>' + PackageInformtion.PackagePrice + '</td><td>' + PackageInformtion.Client + '</td><td style="color:darkblue">' + PackageInformtion.IPPoolName + '</td><td style="color:darkblue">' + PackageInformtion.LocalAddress + '</td><td style="color:darkblue">' + PackageInformtion.MikrotikName + '</td><td><a href="" id="showPackageForUpdate">Show</a></td></tr>');

                table.draw();
            }
        }

        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Package Already Added. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }
        }
        if (data.packageCount < 1) {
            window.location.href = "MIkrotikPackage/Index";
        }
        MIkrotikPackageManager.clearForSaveInformation();
        $("#mdlPackageCreate").modal('hide');
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
        
        var url = "/MIkrotikPackage/GetPackageDetailsByID/";
        var data = { packageID: PackageID };
        data = MIkrotikPackageManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikPackageManager.ShowPackageDetailsByIDForUpdateSuccess, MIkrotikPackageManager.ShowPackageDetailsByIDForUpdateError);

        //   }, 500);

    },
    ShowPackageDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);

        var PackageJSONParse = (data.PackageDetails);
        $("#PackageNames").val(PackageJSONParse.PackageName);
        $("#PackagePrices").val(PackageJSONParse.PackagePrice);
        $("#BandWiths").val(PackageJSONParse.BandWith);
        $("#IPPoolIDs").val(PackageJSONParse.IPPoolID);
        $("#LocalAddresss").val(PackageJSONParse.LocalAddress);
        $("#MikrotikIDs").val(PackageJSONParse.MikrotikID);

        $("#ddlUpdatePackageFor").val(PackageJSONParse.PackageForMyOrResellerUser);

        $("#mdlPackageUpdate").modal("show");
    },
    ShowPackageDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdatePackageInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        var PackageID = packageID;
        var PackageName = $("#PackageNames").val();
        var PackagePrice = $("#PackagePrices").val();
        var BandWith = $("#BandWiths").val();
        var IPPoolID = $("#IPPoolIDs").val();
        var LocalAddress = $("#LocalAddresss").val();
        var MikrotikID = $("#MikrotikIDs").val();
        var PackageForMyOrResellerUser = AppUtil.GetIdValue("ddlUpdatePackageFor");

        var url = "/MIkrotikPackage/UpdatePackage";
        var PackageInfomation =
            ({ PackageForMyOrResellerUser: PackageForMyOrResellerUser, PackageID: PackageID, PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith, IPPoolID: IPPoolID, LocalAddress: LocalAddress, MikrotikID: MikrotikID });
        var data = JSON.stringify({ PackageInfoForUpdate: PackageInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, MIkrotikPackageManager.UpdatePackageInformationSuccess, MIkrotikPackageManager.UpdatePackageInformationFail);
    },
    UpdatePackageInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        
        if (data.Success === false) {
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
        }

        if (data.UpdateSuccess === true) {
            var PackageInfoForUpdate = (data.PackageUpdateInformation);

            //$("#tblPackage tbody>tr").each(function () {
                
            //    var PackageID = $(this).find("td:eq(0) input").val();
            //    var index = $(this).index();
            //    if (PackageInfoForUpdate.PackageID == PackageID) {
                    
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(1)").text(PackageInfoForUpdate.PackageName);
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(2)").text(PackageInfoForUpdate.BandWith);
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(3)").text(PackageInfoForUpdate.PackagePrice);
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(5)").html("<div style='color:darkblue'>" + PackageInfoForUpdate.IPPoolName + "</div>");
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(6)").html("<div style='color:darkblue'>" + PackageInfoForUpdate.LocalAddress + "</div>");
            //        $('#tblPackage tbody>tr:eq(' + index + ')').find("td:eq(7)").html(PackageInfoForUpdate.MikrotikName);
            //    }
            //});

            table.draw();
            AppUtil.ShowSuccess("Package Update Successfully.");
        }

        if (data.UpdateSuccess === false) {

            if (data.AlreadyInsert === true) {
                AppUtil.ShowSuccess("Package Already Added. Choose different Name.");
            } else {
                AppUtil.ShowSuccess("Update Failed.");
            }
        }
        MIkrotikPackageManager.clearForUpdateInformation();
        $("#mdlPackageUpdate").modal('hide');
        console.log(data);
    },
    UpdatePackageInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    clearForSaveInformation: function () {
        $("#CreatePackageName").val("");
        $("#CreatePackagePrice").val("");
        $("#CreateBandWith").val("");
        $("#CreateIPPoolID").val("");
        $("#CreateLocalAddress").val("");
    },
    clearForUpdateInformation: function () {
        $("#PackageName").val("");
        $("#PackagePrice").val("");
        $("#BandWith").val("");
        $("#IPPoolID").val("");
        $("#LocalAddress").val("");
    }

    ,
    PrintMikrotikPackageList: function () {
        
        var url = "/Excel/CreateReportForMikrotikPackage";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

       // var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

       // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({  });
        //var data = ({ ZoneID: ZoneID });
        data = MIkrotikPackageManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikPackageManager.PrintMikrotikPackageListSuccess, MIkrotikPackageManager.PrintMikrotikPackageListFail);
    },
    PrintMikrotikPackageListSuccess: function (data) {
        
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
    PrintMikrotikPackageListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },
}