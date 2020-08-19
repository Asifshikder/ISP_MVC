var MIkrotikUserManager = {

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
        AppUtil.MakeAjaxCall(url, "POST", datas, MIkrotikUserManager.InsertPackageSuccess, MIkrotikUserManager.InsertPackageFail);
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

        var MikrotikID = AppUtil.GetIdValue("CreateMikrotikID");

        //     setTimeout(function () {
        var Package = { PackageName: PackageName, PackagePrice: PackagePrice, BandWith: BandWith, IPPoolID: IPPoolID, LocalAddress: LocalAddress, MikrotikID: MikrotikID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Package: Package });
        AppUtil.MakeAjaxCall(url, "POST", datas, MIkrotikUserManager.InsertPackageFromPopUpSuccess, MIkrotikUserManager.InsertPackageFromPopUpFail);
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

                var PackageInformtion = (data.PackageInformation);
                $("#tblPackage>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + PackageInformtion.PackageID + '></td><td>' + PackageInformtion.PackageName + '</td><td>' + PackageInformtion.BandWith + '</td><td>' + PackageInformtion.PackagePrice + '</td><td>' + PackageInformtion.Client + '</td><td style="color:darkblue">' + PackageInformtion.IPPoolName + '</td><td style="color:darkblue">' + PackageInformtion.LocalAddress + '</td><td style="color:darkblue">' + PackageInformtion.MikrotikName + '</td><td><a href="" id="showPackageForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
        if (data.packageCount < 1) {
            window.location.href = "MIkrotikPackage/Index";
        }
        MIkrotikUserManager.clearForSaveInformation();
        $("#mdlPackageInsert").modal('hide');
        table.draw();
    },
    InsertPackageFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowMikrotikUserByIDForUpdate: function () {

        var url = "/MIkrotikUser/ShowMikrotikUserByIDForUpdate/";
        var data = { ClientDetailsID: ClientDetailsID };
        data = MIkrotikUserManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.ShowMikrotikUserByIDForUpdateSuccess, MIkrotikUserManager.ShowMikrotikUserByIDForUpdateError);

        //   }, 500);

    },
    ShowMikrotikUserByIDForUpdateSuccess: function (data) {

        if (data.MikrotikID) {
            $("#lstMikrotikUpdate").val(data.MikrotikID);
        }
        $("#mdlMikrotikUserUpdate").modal("show");
    },
    ShowMikrotikUserByIDForUpdateError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateMikrotikUserInformation: function () {

        var MikrotikID = $("#lstMikrotikUpdate").val();

        var url = "/MIkrotikUser/UpdateMikrotikUser";
        var data = ({ ClientDetailsID: ClientDetailsID, MikrotikID: MikrotikID });
        data = MIkrotikUserManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.UpdateMikrotikUserInformationSuccess, MIkrotikUserManager.UpdateMikrotikUserInformationFail);
    },
    UpdateMikrotikUserInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();


        if (data.Success === false) {
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
        }

        if (data.UpdateSuccess === true) {

            $("#tblMikrotik_Client>tbody>tr").each(function () {

                var clientDetailsID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (clientDetailsID == data.ClientDetailsID) {

                    $('#tblMikrotik_Client>tbody>tr:eq(' + index + ')').find("td:eq(7)").html(data.MikrotikName);
                }
            });

        }

        MIkrotikUserManager.clearForUpdateInformation();
        $("#mdlMikrotikUserUpdate").modal('hide');
        console.log(data);
    },
    UpdateMikrotikUserInformationFail: function (data) {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintMikrotikUserList: function () {

        var url = "/Excel/CreateReportForMikrotikUser";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var MikrotikID = AppUtil.GetIdValue("lstMikrotiks"); //('#ConnectionDate').datepicker('getDate');

        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        //var data = ({});
        var data = ({ MikrotikID: MikrotikID });
        data = MIkrotikUserManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.PrintMikrotikUserListSuccess, MIkrotikUserManager.PrintMikrotikUserListFail);
    },
    PrintMikrotikUserListSuccess: function (data) {

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
    PrintMikrotikUserListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },


    ShowMikrotikUserByIDForSynchronize: function () {
        var url = "/MIkrotikUser/ShowMikrotikUserByMikrotikIDForSynchronize/";
        var MikrotikID = $("#lstMikrotiks").val();
        var data = { MikrotikID: MikrotikID };
        data = MIkrotikUserManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.ShowMikrotikUserByIDForSynchronizeSuccess, MIkrotikUserManager.ShowMikrotikUserByIDForSynchronizeError);
    },
    ShowMikrotikUserByIDForSynchronizeSuccess: function (data) {

        //$('#tblMikrotik_Client').dataTable().fnDestroy();
        $("#tblMikrotik_Client>tbody>tr").remove();
        //LoginName: loginName, Password: password, MikrotikID: mikrotikID, PackageName: packageName
        if (data.Success === true) {
            $.each(data.lstUserFromMikrotik, function (index, item) {
                //$("#tblMikrotik_Client>tbody").append("<tr> <td><div style='margin-left:1px' class='checkbox checkbox-danger'><input type='checkbox' id='chkMikrotikUser" + item.UserName + "' name='chkMikrotikUser" + item.UserName + "' onclick='enableDisableMikrotikOption(chkMikrotikUser'" + item.UserName + ",'" + item.UserName + "')'       > <label for= 'chkMikrotikUser" + item.UserName + "'> </label ></div></td>   <td hidden>" + item.MikrotikID + "</td><td hidden></td><td>" + item.UserName + "</td><td>" + item.Password + "</td><td>" + item.ProfileName + "</td><td>" + item.active + "</td><td>" + item.MikrotikName + "</td></tr>");/*<td></td>*/
                $("#tblMikrotik_Client>tbody").append('<tr> <td><div style="margin-left:1px" class="checkbox checkbox-danger"><input type="checkbox" id="chkMikrotikUser' + item.UserName + '" name="chkMikrotikUser' + item.UserName + '" onclick="enableDisableMikrotikOption(chkMikrotikUser' + item.UserName + ',\'' + item.UserName + '\',\'' + item.Password + '\',\'' + item.MikrotikID + '\',\'' + item.ProfileName + '\')"  > <label for= "chkMikrotikUser' + item.UserName + '"> </label ></div></td>   <td hidden>' + item.MikrotikID + '</td><td hidden></td><td>' + item.UserName + '</td><td>' + item.Password + '</td><td>' + item.ProfileName + '</td><td>' + item.active + '</td><td>' + item.MikrotikName + '</td></tr>');/*<td></td>*/
            });
        }

        //var mytable = $('#tblMikrotik_Client').DataTable();
        //mytable.draw();
    },
    ShowMikrotikUserByIDForSynchronizeError: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    ImportClientPermentlyFromMikrotik: function (userListFromMikrotik) {
        var url = "/MIkrotikUser/InsertClientDetailsIntoSystemFromMikrotik/";
        var MikrotikID = $("#lstMikrotiks").val();
        var data = ({ MikrotikUser: userListFromMikrotik });
        data = MIkrotikUserManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.ImportClientPermentlyFromMikrotikSuccess, MIkrotikUserManager.ImportClientPermentlyFromMikrotikError);
    },
    ImportClientPermentlyFromMikrotikSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Selected Client List Imported Successfully.");
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Some Clients List Can Not Imported Properly.");
        }
        $("#tblMikrotik_Client>tbody>tr").each(function () {
            var index = $(this).index();
            var userName = $(this).closest("tr").find("td:eq(3)").text();
            if (jQuery.inArray(userName, data.SuccessArray) !== -1) {
                $(this).closest("tr").remove();
            }
        });
        userListFromMikrotik = userListFromMikrotik.filter(function (el) {
            return !data.SuccessArray.includes(el.LoginName);
        });
        //userListFromMikrotik =  $.grep(userListFromMikrotik, function (v, i) {
        //    return v.LoginName != lName;
        //});
        $("#popModalForImportClientFromMikrotik").modal("hide");
    },
    ImportClientPermentlyFromMikrotikError: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    RemoveClientPermentlyFromMikrotik: function (userListFromMikrotik) {
        var url = "/MIkrotikUser/RemoveClientPermentlyFromMikrotik/";
        var MikrotikID = $("#lstMikrotiks").val();
        var data = ({ MikrotikUser: userListFromMikrotik });
        data = MIkrotikUserManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MIkrotikUserManager.RemoveClientPermentlyFromMikrotikSuccess, MIkrotikUserManager.RemoveClientPermentlyFromMikrotikError);
    },
    RemoveClientPermentlyFromMikrotikSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Selected Client List Removed Successfully From Mikrotik.");
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Some Clients List Can Not Removed From Mikrotik.");
        }
        $("#tblMikrotik_Client>tbody>tr").each(function () {
            var index = $(this).index();
            var userName = $(this).closest("tr").find("td:eq(3)").text();
            if (jQuery.inArray(userName, data.SuccessArray) !== -1) {
                $(this).closest("tr").remove();
            }
        });
        userListFromMikrotik = userListFromMikrotik.filter(function (el) {
            return !data.SuccessArray.includes(el.LoginName);
        });
        //userListFromMikrotik =  $.grep(userListFromMikrotik, function (v, i) {
        //    return v.LoginName != lName;
        //});
        $("#popModalForDeleteClientFromMikrotik").modal("hide");
    },
    RemoveClientPermentlyFromMikrotikError: function (data) { 
        console.log(data);
    },

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    clearForUpdateInformation: function () {
        $("#lstMikrotikUpdate").val("");
    }
}