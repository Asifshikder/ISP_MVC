var ResellerManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateMacResellerValidation: function () {


        var status = true;
        if (AppUtil.GetIdValue("txtCreateMacResellerName") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Name.", "txtCreateMacResellerName", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("txtCreateMacResellerBusinessName") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Business Name.", "txtCreateMacResellerBusinessName", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("txtCreateMacResellerLoginName") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Login Name.", "txtCreateMacResellerBusinessName", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("txtCreateMacResellerPassword") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Password.", "txtCreateMacResellerPassword", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("txtCreateMacResellerAddress") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Adress.", "txtCreateMacResellerAddress", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("txtCreateMacResellerContact") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Contact Number.", "txtCreateMacResellerAddress", "top center");
            status = false;
        }
        if ($("#ddlCreateMacResellerBillingCycle option:selected").text() == '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Billing Cycle.", "ddlCreateMacResellerBillingCycle", "top center");
            status = false;
        }
        var src = $("#PreviewMacResellerCreateImagePaths").attr('src');
        if (src == '#') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Image.", "MacResellerCreateImage", "top center");
            status = false;
        }
        var a = AppUtil.GetIdValue("ddlCreateMacResellerStatus");
        if (AppUtil.GetIdValue("ddlCreateMacResellerStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Status.", "ddlCreateMacResellerStatus", "top center");
            status = false;
        }
        if ($("#tblMacResellerPackageCreate>tbody>tr").length == 0) {
            AppUtil.ShowErrorOnControl("Please Add Reseller Package.", "ddlMacReselerPackageCreate", "top center");
            status = false;
        }
        return status;
    },

    UpdateMacResellerValidation: function () {

        var status = true;
        if (AppUtil.GetIdValue("txtUpdateMacResellerName") === '') { 
            AppUtil.ShowErrorOnControl("Please Add Reseller Name.", "txtUpdateMacResellerName", "top center");
            status = false; 
        }
        if (AppUtil.GetIdValue("txtUpdateMacResellerBusinessName") === '') { 
            AppUtil.ShowErrorOnControl("Please Add Reseller Business Name.", "txtUpdateMacResellerBusinessName", "top center");
            status = false;  
        }
        if (AppUtil.GetIdValue("txtUpdateMacResellerLoginName") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Login Name.", "txtUpdateMacResellerBusinessName", "top center");
            status = false;   
        }
        if (AppUtil.GetIdValue("txtUpdateMacResellerPassword") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Password.", "txtUpdateMacResellerPassword", "top center");
            status = false;    
        }
        if (AppUtil.GetIdValue("txtUpdateMacResellerAddress") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Adress.", "txtUpdateMacResellerAddress", "top center");
            status = false;    
        }
        if (AppUtil.GetIdValue("txtUpdateMacResellerContact") === '') { 
            AppUtil.ShowErrorOnControl("Please Add Reseller Contact Number.", "txtUpdateMacResellerAddress", "top center");
            status = false;     
        }
        if ($("#ddlUpdateMacResellerBillingCycle option:selected").text() == '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Billing Cycle.", "ddlUpdateMacResellerBillingCycle", "top center");
            status = false;    
        }
        var src = $("#PreviewMacResellerUpdateImagePaths").attr('src');
        if (src == '#') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Image.", "MacResellerUpdateImage", "top center");
            status = false;   
        }
        var a = AppUtil.GetIdValue("ddlUpdateMacResellerStatus");
        if (AppUtil.GetIdValue("ddlUpdateMacResellerStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Add Reseller Status.", "ddlUpdateMacResellerStatus", "top center");
            status = false;   
        }
        if ($("#tblMacResellerPackageUpdate>tbody>tr").length == 0) {
            AppUtil.ShowErrorOnControl("Please Add Reseller Package.", "ddlMacReselerPackageUpdate", "top center");
            status = false;   
        }
        return status;
    },

    CreateBandwithResellerValidation: function () {

        if (AppUtil.GetIdValue("txtInsertBandwithResellerName") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Name.");
            return false;
        }
        if (AppUtil.GetIdValue("txtInsertBandwithResellerBusinessName") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Business Name.");
            return false;
        }
        if (AppUtil.GetIdValue("txtInsertBandwithResellerLoginName") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Login Name.");
            return false;
        }
        if (AppUtil.GetIdValue("txtInsertBandwithResellerPassword") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Password.");
            return false;
        }
        if (AppUtil.GetIdValue("txtInsertBandwithResellerAddress") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Adress. ");
            return false;
        }
        if (AppUtil.GetIdValue("txtInsertBandwithResellerContact") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Contact Number. ");
            return false;
        }
        if (AppUtil.GetIdValue("BandwithResellerCreateImage") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Image. ");
            return false;
        }
        if (AppUtil.GetIdValue("ddlInsertBandwithResellerStatus") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Status. ");
            return false;
        }
        if ($("#tblBandwithResellerItemCreate>tbody>tr").length == 0) {
            AppUtil.ShowSuccess("Please Add Reseller Item In Table. ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {

        if (AppUtil.GetIdValue("ResellerNames") === '') {
            AppUtil.ShowSuccess("Please Add Reseller Name.");
            return false;
        }
        if (AppUtil.GetIdValue("ResellerAddresss") === '') {
            AppUtil.ShowSuccess("Please Insert Reseller Location ");
            return false;
        }
        if (AppUtil.GetIdValue("ResellerContacts") === '') {
            AppUtil.ShowSuccess("Please Insert Reseller Contact Number. ");
            return false;
        }
        return true;
    },


    AddMacResellerPackageInList: function () {
        var AddedInList = "";
        var PID = AppUtil.GetIdValue("ddlMacReselerPackageInsert");
        var PName = $("#ddlMacReselerPackageInsert option:selected").text();


        $("#tblMacResellerPackageInsert>tbody>tr").each(function () {
            debugger;
            var tblPID = $(this).find("td:eq(0) input").val();
            if (tblPID == PID) {
                AddedInList = true;
            }
        });

        if (AddedInList === true) {
            alert("This Package is Already added in list. Select Different Pacakge.");
        } else {
            
            $("#tblMacResellerPackageInsert>tbody").append("<tr><td hidden><input type='hidden' value=" + PID + "></td><td>" + PName + "</td><td><input type='text' class='form-group' value=''></td><td><input type='text' class='form-group' ></td><td align='center'><button id='btnDeleteMacUserPackageFromTableCreate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
        }
    },
    UpdateMacResellerPackageInList: function () {
        var AddedInList = "";
        var PID = AppUtil.GetIdValue("ddlMacReselerPackageUpdate");
        var PName = $("#ddlMacReselerPackageUpdate option:selected").text();


        $("#tblMacResellerPackageUpdate>tbody>tr").each(function () {
            debugger;
            var tblPID = $(this).find("td:eq(0) input").val();
            if (tblPID == PID) {
                AddedInList = true;
            }
        });

        if (AddedInList === true) {
            alert("This Package is Already added in list. Select Different Pacakge.");
        } else {
            var data = { pid: PID };
            data = ResellerManager.addRequestVerificationToken(data);
            var packagePrice = "";
            $.ajax({
                method: "POST",
                url: "/Package/GetPackagePriceByID",
                async:false,
                data: data,
                contentType: "application/x-www-form-urlencoded",
                success: function (data) {
                    packagePrice = data.PackagePrice;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
            $("#tblMacResellerPackageUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + PID + "></td><td>" + PName + "</td><td><input type='text' class='form-group' value=" + packagePrice + "></td><td><input type='text' class='form-group'></td><td align='center'><button id='btnDeleteMacUserPackageFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
        }
    },

    AddBandwithResellerItemInList: function () {
        var AddedInList = "";
        var itemID = AppUtil.GetIdValue("ddlAddItemsForBandwithReseller");
        var itemName = $("#ddlAddItemsForBandwithReseller option:selected").text();


        $("#tblBandwithResellerItemCreate>tbody>tr").each(function () {
            debugger;
            var tblItemID = $(this).find("td:eq(0) input").val();
            if (tblItemID == itemID) {
                AddedInList = true;
            }
        });

        if (AddedInList === true) {
            alert("This Package is Already added in list. Select Different Item.");
        } else {
            $("#tblBandwithResellerItemCreate>tbody").append("<tr><td hidden><input type='hidden' value=" + itemID + "></td><td>" + itemName + "</td><td><input type='text' class='form-group' onkeyup='SetValuesInOthersColtextForBandwithResellerCreate(this.parentNode.parentNode)'></td><td><input type='text' class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerCreate(this.parentNode.parentNode)'></td><td><input type='text' class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerCreate(this.parentNode.parentNode)'></td><td align='center'><button id='btnDeleteBandwithUserItemFromTableCreate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
        }
    },
    UpdateBandwithResellerItemInList: function () {
        var AddedInList = "";
        var itemID = AppUtil.GetIdValue("ddlUpdateItemsForBandwithReseller");
        var itemName = $("#ddlUpdateItemsForBandwithReseller option:selected").text();


        $("#tblBandwithResellerItemUpdate>tbody>tr").each(function () {
            debugger;
            var tblItemID = $(this).find("td:eq(0) input").val();
            if (tblItemID == itemID) {
                AddedInList = true;
            }
        });

        if (AddedInList === true) {
            alert("This Package is Already added in list. Select Different Item.");
        } else {
            $("#tblBandwithResellerItemUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + itemID + "></td><td>" + itemName + "</td><td><input type='text' class='form-group' onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text' class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text' class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td align='center'><button id='btnDeleteBandwithUserItemFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
        }
    },

    InsertResellerFromPopUp: function (MacOrBand) {

        //AppUtil.ShowWaitingDialog();

        var url = "/Reseller/InsertResellerFromPopUp";
        var ResellerName = AppUtil.GetIdValue("txtInsertResellerName");
        var ResellerLoginName = AppUtil.GetIdValue("txtInsertResellerLoginName");
        var ResellerBusinessName = AppUtil.GetIdValue("txtInsertResellerBusinessName");
        var ResellerPassword = AppUtil.GetIdValue("txtInsertResellerPassword");
        var ResellerTypeListID = $("input[name=rdbInsertResellerType]").val();
        var ResellerAddress = AppUtil.GetIdValue("txtInsertResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("txtInsertResellerContact");
        var ResellerBillingCycleList = "";
        $.each($("#ddlInsertResellerBillingCycle").val(), function (index, item) {
            ResellerBillingCycleList += item + ",";
        });
        ResellerBillingCycleList = ResellerBillingCycleList.trim();
        var ResellerStatus = $("#ddlInsertResellerStatus").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        //  setTimeout(function () {
        //var Reseller = [];
        //Reseller.push({
        //    ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerPassword: ResellerPassword,
        //    ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
        //    ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
        //});
        var Reseller = {
            ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerBusinessName: ResellerBusinessName, ResellerPassword: ResellerPassword,
            ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
            ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
        };

        var formData = new FormData();
        formData.append('ResellerLogoImageBytes', $('#ResellerImage')[0].files[0]);
        formData.append('Reseller_Client', JSON.stringify(Reseller));

        //var datas = JSON.stringify({ Reseller_Client: Reseller });
        //AppUtil.MakeAjaxCallJSONAntifergeryJSON(url, "POST", datas, header, ResellerManager.InsertResellerFromPopUpSuccess, ResellerManager.InsertResellerFromPopUpFail);

        //AppUtil.MakeAjaxCall(url, "POST", datas, ResellerManager.InsertResellerFromPopUpSuccess, ResellerManager.InsertResellerFromPopUpFail);
        AppUtil./*MakeAjaxCallJSONAntifergeryJSON*/MakeAjaxCallJSONAntifergery(url, "POST", /*datas*/formData, header, ResellerManager.InsertResellerFromPopUpSuccess, ResellerManager.InsertResellerFromPopUpFail);
        // }, 500);
    },
    InsertResellerFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            //if (data.Reseller) {
            //    
            //    var Reseller = (data.Reseller);
            //    $("#tblReseller>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Reseller.ResellerID + '></td><td>' + Reseller.ResellerName + '</td><td>' + Reseller.ResellerAddress + '</td><td>' + Reseller.ResellerContact + '</td><td><a href="" id="showResellerForUpdate">Show</a></td></tr>');
            //}
            // ResellerManager.clearForSaveInformation();
            $("#mdlResellerInsert").modal('hide');
            window.location.reload();
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Reseller Already Added. Choose different Reseller Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Reseller/Index";


    },
    InsertResellerFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },


    InsertResellerPaymentFromPopUp: function () {

        var url = "";

        if (_resellerID != '' && (_resellerPaymentID != '' && _resellerPaymentID != undefined)) {
            url = "/Reseller/UpdateResellerPayment/"
        }
        else {
            url = "/Reseller/InsertResellerPayment/"
        }

        var ResellerID = _resellerID;
        var ResellerPaymentID = _resellerPaymentID;
        var PaymentAmount = AppUtil.GetIdValue("txtResellerPaymentAmount");
        var CollectBy = AppUtil.GetIdValue("ddlResellerCollectBy");
        var CheckSerialOrAnyResetNo = AppUtil.GetIdValue("txtCheckSerialOrAnyResetNo");
        var PaymentBy = AppUtil.GetIdValue("ddlPaymentBy");
        var PaymentStatus = AppUtil.GetIdValue("ddlPaymentStatus");
        var GivenPaymentType = AppUtil.GetIdValue("ddlPaymentType");

        var resellerPayment = {
            ResellerID: ResellerID, ResellerPaymentID: ResellerPaymentID, PaymentAmount: PaymentAmount, CollectBy: CollectBy/*, CheckSerialOrAnyResetNo: CheckSerialOrAnyResetNo*/
            , PaymentCheckOrAnySerial: CheckSerialOrAnyResetNo, PaymentBy: PaymentBy, PaymentStatus: PaymentStatus, PaymentTypeID: GivenPaymentType
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;
        var datas = JSON.stringify({ ResellerPayment: resellerPayment });
        //  AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.InsertClientDetailsSuccess, ClientDetailsManager.InsertClientDetailsFail);

        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", datas, header, ResellerManager.InsertResellerPaymentFromPopUpSuccess, ResellerManager.InsertResellerPaymentFromPopUpFail);
    },
    InsertResellerPaymentFromPopUpSuccess: function (data) {

        if (data.SuccessInsert === false) {
            AppUtil.ShowError("Payment Unsecessfull. Please Contact With Administrator.");
        }

        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            //for reseller page not for reseller payment page
            $("#tblReseller>tbody>tr:eq(" + _rowIndex + ")").find("td:eq(6)").text(data.CurrentBalance);
            /////////////////////////////////////////////////
            $("#mdlPayment").modal("hide");
            //ResellerManager.ClearForPaymentInformation();
            tableRsellerPaymentHistory.draw();
        }
    },
    InsertResellerPaymentFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    ShowResellerPaymentDetailsByIDForUpdate: function (RPID) {
        var url = "/Reseller/ShowResellerPaymentDetailsByID/";
        var data = { RPID: RPID };
        data = ResellerManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.ShowResellerPaymentDetailsByIDForUpdateSuccess, ResellerManager.ShowResellerPaymentDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowResellerPaymentDetailsByIDForUpdateSuccess: function (data) {
        if (data.Success) {
            var RPMDetailsJsonParse = (data.RPD);

            $("#txtResellerLoginName").prop('disabled', true).val(RPMDetailsJsonParse.ResellerLoginName);
            $("#txtResellerType").val(RPMDetailsJsonParse.ResellerType);
            $("#txtResellerBusinessName").val(RPMDetailsJsonParse.ResellerBusinessName);
            $("#txtResellerAddress").val(RPMDetailsJsonParse.ResellerAddress);
            $("#txtResellerPhone").val(RPMDetailsJsonParse.ResellerPhone);
            $("#ResellerLogoPath").prop("src", RPMDetailsJsonParse.ResellerLogoPath);
            $("#txtResellerPaymentAmount").val(RPMDetailsJsonParse.PaymentAmount);
            $("#ddlResellerCollectBy").val(RPMDetailsJsonParse.Collectby).select2({ width: '100%' }).trigger('change');

            $("#txtCheckSerialOrAnyResetNo").val(RPMDetailsJsonParse.PaymentCheckOrAnySerial);

            $("#ddlPaymentBy").val(RPMDetailsJsonParse.PaymentBy).select2({ width: '100%' }).trigger('change');

            $("#ddlPaymentType").val(RPMDetailsJsonParse.PaymentTypeID);

            $("#ddlPaymentStatus").val(RPMDetailsJsonParse.PaymentStatus).select2({ width: '100%' }).trigger('change');

            $("#mdlPayment").modal("show");
        }
        else {
            AppUtil.ShowError("Something Wrong. Please Contact With Administrator.");
        }

    },
    ShowResellerPaymentDetailsByIDForUpdateError: function (data) {
        console.log(data);
    },


    ShowResellerDetailsByIDForCreatingPayment: function (RID) {
        var url = "/Reseller/ShowResellerDetailsByIDForCreatingPayment/";
        var data = { RID: RID };
        data = ResellerManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.ShowResellerDetailsByIDForCreatingPaymentSuccess, ResellerManager.ShowResellerDetailsByIDForCreatingPaymentError);
    },
    ShowResellerDetailsByIDForCreatingPaymentSuccess: function (data) {
        if (data.Success) {
            var RPMDetailsJsonParse = (data.RD);

            $("#txtResellerLoginName").prop('disabled', true).val(RPMDetailsJsonParse.ResellerLoginName);
            $("#txtResellerType").val(RPMDetailsJsonParse.ResellerType);
            $("#txtResellerBusinessName").val(RPMDetailsJsonParse.ResellerBusinessName);
            $("#txtResellerAddress").val(RPMDetailsJsonParse.ResellerAddress);
            $("#txtResellerPhone").val(RPMDetailsJsonParse.ResellerPhone);
            $("#ResellerLogoPath").prop("src", RPMDetailsJsonParse.ResellerLogoPath);

            $("#mdlPayment").modal("show");
        }
        else {
            AppUtil.ShowError("Something Wrong. Please Contact With Administrator.");
        }
    },
    ShowResellerDetailsByIDForCreatingPaymentError: function (data) {
        console.log(data);
    },


    DeleteResellerPaymentHistoryByID: function (ResellerPaymentID) {

        var url = "/Reseller/DeleteResellerPaymentByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var datas = ({ ResellerPaymentID: ResellerPaymentID });
        datas = ResellerManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, ResellerManager.DeleteResellerPaymentHistoryByIDSuccess, ResellerManager.DeleteResellerPaymentHistoryByIDFail);
        // }, 50);
    },
    DeleteResellerPaymentHistoryByIDSuccess: function (data) {

        if (data.DeleteStatus === true) {
            AppUtil.ShowSuccess("Payment removed successfully.");
            tableRsellerPaymentHistory.draw();
            $("#tblReseller>tbody>tr:eq(" + _rowIndex + ")").find("td:eq(6)").text(data.CurrentBalance);
            $("#popModalForDeletePermently").modal("hide")
        }

        if (data.DeleteStatus === false) {
            AppUtil.ShowError("Payment removed failed.");
        }

    },
    DeleteResellerPaymentHistoryByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    InsertMacResellerFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Reseller/InsertMacResellerFromPopUp";
        var ResellerName = AppUtil.GetIdValue("txtInsertMacResellerName");
        var ResellerLoginName = AppUtil.GetIdValue("txtInsertMacResellerLoginName");
        var ResellerBusinessName = AppUtil.GetIdValue("txtInsertMacResellerBusinessName");
        var ResellerPassword = AppUtil.GetIdValue("txtInsertMacResellerPassword");
        var ResellerTypeListID = 2;//$("input[name=rdbInsertMacResellerType]").val();
        var ResellerAddress = AppUtil.GetIdValue("txtInsertMacResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("txtInsertMacResellerContact");
        var ResellerBillingCycleList = "";
        $.each($("#ddlInsertMacResellerBillingCycle").val(), function (index, item) {
            ResellerBillingCycleList += item + ",";
        });
        ResellerBillingCycleList = ResellerBillingCycleList.trim();

        var ResellerGivenMikrotikList = "";
        $.each($("#ddlMacReselerMikrotikInsert").val(), function (index, item) {
            ResellerGivenMikrotikList += item + ",";
        });
        ResellerGivenMikrotikList = ResellerGivenMikrotikList.trim(',');

        var ResellerStatus = $("#ddlInsertMacResellerStatus").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        //  setTimeout(function () {
        //var Reseller = [];
        //Reseller.push({
        //    ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerPassword: ResellerPassword,
        //    ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
        //    ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
        //});
        var ResellerPackage = [];
        //var ResellerPackage = {
        //    PackageName
        //};
        $("#tblMacResellerPackageInsert>tbody>tr").each(function () { 
            //var pID = $(this).closest("tr").find("td:eq(0) input").val();
            //var pName = $(this).closest("tr").find("td:eq(1)").html();
            //var pp = $(this).closest("tr").find("td:eq(2) input").val();
            //ResellerPackage.push({ PID: pID, PName: pName, PP: pp });
            var pID = $(this).closest("tr").find("td:eq(0) input").val();
            var pName = $(this).closest("tr").find("td:eq(1)").html();
            var ppAdmin = $(this).closest("tr").find("td:eq(2) input").val();
            var ppFromRS = $(this).closest("tr").find("td:eq(3) input").val() == "" ? 0 : $(this).closest("tr").find("td:eq(3) input").val();
            ResellerPackage.push({ PID: pID, PName: pName, PPAdmin: ppAdmin, PPAdmin: ppAdmin, PPFromRS: ppFromRS });
        });
        var Reseller = {
            ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerBusinessName: ResellerBusinessName, ResellerPassword: ResellerPassword,
            ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
            ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
            , macResellerGivenPackagePriceModel: ResellerPackage, MacResellerAssignMikrotik: ResellerGivenMikrotikList
        };

        var formData = new FormData();
        formData.append('ResellerLogoImageBytes', $('#MacResellerCreateImage')[0].files[0]);
        formData.append('Reseller_Client', JSON.stringify(Reseller));

        //var datas = JSON.stringify({ Reseller_Client: Reseller });
        //AppUtil.MakeAjaxCallJSONAntifergeryJSON(url, "POST", datas, header, ResellerManager.InsertResellerFromPopUpSuccess, ResellerManager.InsertResellerFromPopUpFail);

        //AppUtil.MakeAjaxCall(url, "POST", datas, ResellerManager.InsertResellerFromPopUpSuccess, ResellerManager.InsertResellerFromPopUpFail);
        AppUtil./*MakeAjaxCallJSONAntifergeryJSON*/MakeAjaxCallJSONAntifergery(url, "POST", /*datas*/formData, header, ResellerManager.InsertMacResellerFromPopUpSuccess, ResellerManager.InsertMacResellerFromPopUpFail);
        // }, 500);
    },
    InsertMacResellerFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            //if (data.Reseller) {
            //    
            //    var Reseller = (data.Reseller);
            //    $("#tblReseller>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Reseller.ResellerID + '></td><td>' + Reseller.ResellerName + '</td><td>' + Reseller.ResellerAddress + '</td><td>' + Reseller.ResellerContact + '</td><td><a href="" id="showResellerForUpdate">Show</a></td></tr>');
            //}
            // ResellerManager.clearForSaveInformation();
            $("#mdlResellerInsert").modal('hide');
            window.location.reload();
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Reseller Already Added. Choose different Reseller Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Reseller/Index";


    },
    InsertMacResellerFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertBandwithResellerFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Reseller/InsertBandwithResellerFromPopUp";
        var ResellerName = AppUtil.GetIdValue("txtInsertBandwithResellerName");
        var ResellerLoginName = AppUtil.GetIdValue("txtInsertBandwithResellerLoginName");
        var ResellerBusinessName = AppUtil.GetIdValue("txtInsertBandwithResellerBusinessName");
        var ResellerPassword = AppUtil.GetIdValue("txtInsertBandwithResellerPassword");
        var ResellerTypeListID = 1;//$("input[name=rdbInsertBandwithResellerType]").val();
        var ResellerAddress = AppUtil.GetIdValue("txtInsertBandwithResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("txtInsertBandwithResellerContact");
        var ResellerStatus = $("#ddlInsertBandwithResellerStatus").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var BandwithResellerItem = [];
        $("#tblBandwithResellerItemCreate>tbody>tr").each(function () {
            var itemID = $(this).closest("tr").find("td:eq(0) input").val();
            var ItemName = $(this).closest("tr").find("td:eq(1)").text();
            var itemPrice = $(this).closest("tr").find("td:eq(2) input").val();
            var itemQuantity = $(this).closest("tr").find("td:eq(3) input").val();
            var itemTotal = $(this).closest("tr").find("td:eq(4) input").val();
            BandwithResellerItem.push({ ItemID: itemID, ItemName: ItemName, ItemPrice: itemPrice, ItemQuantity: itemQuantity, ItemTotalPrice: itemTotal });
        });
        var Reseller = {
            ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerBusinessName: ResellerBusinessName, ResellerPassword: ResellerPassword,
            ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact, ResellerStatus: ResellerStatus
            , bandwithReselleGivenItemWithPriceModel: BandwithResellerItem
        };

        var formData = new FormData();
        formData.append('ResellerLogoImageBytes', $('#BandwithResellerCreateImage')[0].files[0]);
        formData.append('Reseller_Client', JSON.stringify(Reseller));
        AppUtil./*MakeAjaxCallJSONAntifergeryJSON*/MakeAjaxCallJSONAntifergery(url, "POST", /*datas*/formData, header, ResellerManager.InsertBandwithResellerFromPopUpSuccess, ResellerManager.InsertBandwithResellerFromPopUpFail);
    },
    InsertBandwithResellerFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            //if (data.Reseller) {
            //    
            //    var Reseller = (data.Reseller);
            //    $("#tblReseller>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Reseller.ResellerID + '></td><td>' + Reseller.ResellerName + '</td><td>' + Reseller.ResellerAddress + '</td><td>' + Reseller.ResellerContact + '</td><td><a href="" id="showResellerForUpdate">Show</a></td></tr>');
            //}
            // ResellerManager.clearForSaveInformation();
            $("#mdlResellerInsert").modal('hide');
            window.location.reload();
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Reseller Already Added. Choose different Reseller Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Reseller/Index";


    },
    InsertBandwithResellerFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    InsertReseller: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Reseller/InsertReseller";
        var ResellerName = AppUtil.GetIdValue("ResellerName");
        var ResellerAddress = AppUtil.GetIdValue("ResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("ResellerContact");


        //setTimeout(function () {
        var Reseller = { ResellerName: ResellerName, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Reseller_Client: Reseller });
        AppUtil.MakeAjaxCall(url, "POST", datas, ResellerManager.InsertResellerSuccess, ResellerManager.InsertResellerUpFail);
        // }, 500);
    },
    InsertResellerSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Reseller) {
            // 
            // var Reseller = (data.Reseller);
            // $("#tblReseller>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Reseller.ResellerID + '/></td><td>' + Reseller.ResellerName + '</td><td><a href="" id="showResellerForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Reseller Already Added. Choose different Reseller Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Reseller/Index";
        $("#mdlResellerInsert").modal('hide');

    },
    InsertResellerUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    ClearBandwithResellerDuringUpdateShow: function () {
        $("#txtUpdateBandwithResellerName").val("");
        $("#txtUpdateBandwithResellerLoginName").val("");
        $("#txtUpdateBandwithResellerPassword").val("");
        $("#txtUpdateBandwithResellerAddress").val("");
        $("#txtUpdateBandwithResellerContact").val("");
        $("#PreviewBandwithResellerUpdateImagePaths").prop("src", "");
        $("#BandwithResellerUpdateImagePaths").val("");
        $("#BandwithResellerUpdateImage").val("");
        $("#ddlResellerStatus").val("");
    },
    ClearMacResellerDuringUpdateShow: function () {
        $("#txtUpdateMacResellerName").val("");
        $("#txtUpdateMacResellerLoginName").val("");
        $("#txtUpdateMacResellerPassword").val("");
        $("#txtUpdateMacResellerAddress").val("");
        $("#txtUpdateMacResellerContact").val("");
        $("#PreviewMacResellerUpdateImagePaths").prop("src", "");
        $("#MacResellerUpdateImagePaths").val("");
        $("#MacResellerUpdateImage").val("");
    },


    GetResellerDetailsByID: function (ResellerID) {

        var url = "/Reseller/GetResellerDetailsByID/";
        var data = ({ RID: ResellerID });
        data = ResellerManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.GetResellerDetailsByIDSuccess, ResellerManager.GetResellerDetailsByIDFailed);
    },
    GetResellerDetailsByIDSuccess: function (data) {
        debugger;
        console.log(data);
        if (data.Success === true) {
            debugger;
            //$("#tblClientListFromLineStatus>tbody").empty(); 

            $("#txtResellerLoginName").val(data.ResellerDetails.ResellerLoginName);
            $("#txtResellerType").val(data.ResellerDetails.ResellerType);
            $("#txtResellerBusinessName").val(data.ResellerDetails.ResellerBusinessName);
            $("#txtResellerAddress").val(data.ResellerDetails.ResellerAddress);
            $("#txtResellerPhone").val(data.ResellerDetails.ResellerPhone);
            $("#ResellerLogoPath").prop("src", data.ResellerDetails.ResellerLogoPath);
            $("#txtResellerPaymentAmount").val("");
            $("#txtCheckSerialOrAnyResetNo").val("");
            $("#ddlPaymentBy").val("").select2({ width: '100%' }).trigger('change');
            $("#ddlPaymentType").val("");
            $("#ddlPaymentStatus").val("2").select2({ width: '100%' }).trigger('change');
        }
    },
    GetResellerDetailsByIDFailed: function (data) {
        debugger;
        console.log(data);
        alert("Fail");
    },

    ShowResellerDetailsByIDForUpdate: function (ResellerID) {
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/Reseller/GetResellerDetailsByID/";
        var data = { RID: ResellerID };
        data = ResellerManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.ShowResellerDetailsByIDForUpdateSuccess, ResellerManager.ShowResellerDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowResellerDetailsByIDForUpdateSuccess: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
        var ResellerDetailsJsonParse = (data.ResellerDetails);

        if (ResellerDetailsJsonParse.ResellerTypeID == 1) {
            ResellerManager.ClearBandwithResellerDuringUpdateShow();

            $("#txtUpdateBandwithResellerName").val(ResellerDetailsJsonParse.ResellerName);
            $("#txtUpdateBandwithResellerBusinessName").val(ResellerDetailsJsonParse.ResellerBusinessName);
            $("#txtUpdateBandwithResellerLoginName").val(ResellerDetailsJsonParse.ResellerLoginName);
            $("#txtUpdateBandwithResellerPassword").val(ResellerDetailsJsonParse.ResellerPassword);
            //$("#rdbUpdateBandwithResellerType").val(ResellerDetailsJsonParse.ResellerType);
            $("input[name='rdbUpdateBandwithResellerType'][value='" + ResellerDetailsJsonParse.ResellerType + "']").prop("checked", true);
            $("#txtUpdateBandwithResellerAddress").val(ResellerDetailsJsonParse.ResellerAddress);
            $("#txtUpdateBandwithResellerContact").val(ResellerDetailsJsonParse.ResellerPhone);
            $("#PreviewBandwithResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#BandwithResellerUpdateImagePaths").val(ResellerDetailsJsonParse.ResellerLogoPath);
            $("#ddlUpdateBandwithResellerBillingCycle").selectpicker('refresh');

            var selectVal = [];
            $(ResellerDetailsJsonParse.ResellerBillingCycle.trim(",").split(",")).each(function (index, element) {
                selectVal.push(element);
            });
            //$('select[name=ddlUpdateBandwithResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlUpdateBandwithResellerBillingCycle").val(selectVal);
            $("#ddlUpdateBandwithResellerBillingCycle").selectpicker('refresh');


            //$("#ddlUpdateBandwithResellerBillingCycle").val(ResellerDetailsJsonParse.ResellerBillingCycle);
            $("#ddlResellerStatus").val(ResellerDetailsJsonParse.ResellerStatus);
            $("#tblBandwithResellerItemUpdate>tbody>tr").remove();
            $.each(ResellerDetailsJsonParse.bandwithReselleGivenItemWithPriceModel, function (index, item) {
                $("#tblBandwithResellerItemUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + item.ItemID + "></td><td>" + item.ItemName + "</td><td><input type='text' value=" + item.ItemPrice + " class='form-group' onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text'  value=" + item.ItemQuantity + "  class='form-group'   onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text'  value=" + item.ItemTotalPrice + "  class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td align='center'><button id='btnDeleteBandwithUserItemFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
            });

            $("#mdlBandwithResellerUpdate").modal("show");
        }
        if (ResellerDetailsJsonParse.ResellerTypeID == 2) {

            ResellerManager.ClearMacResellerDuringUpdateShow();

            $("#txtUpdateMacResellerName").val(ResellerDetailsJsonParse.ResellerName);
            $("#txtUpdateMacResellerBusinessName").val(ResellerDetailsJsonParse.ResellerBusinessName);
            $("#txtUpdateMacResellerLoginName").val(ResellerDetailsJsonParse.ResellerLoginName);
            $("#txtUpdateMacResellerPassword").val(ResellerDetailsJsonParse.ResellerPassword);
            //$("#rdbUpdateMacResellerType").val(ResellerDetailsJsonParse.ResellerType);
            $("input[name='rdbUpdateMacResellerType'][value='" + ResellerDetailsJsonParse.ResellerType + "']").prop("checked", true);
            $("#txtUpdateMacResellerAddress").val(ResellerDetailsJsonParse.ResellerAddress);
            $("#txtUpdateMacResellerContact").val(ResellerDetailsJsonParse.ResellerPhone);
            $("#PreviewMacResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#MacResellerUpdateImagePaths").val(ResellerDetailsJsonParse.ResellerLogoPath);

            $("#ddlUpdateMacResellerBillingCycle").selectpicker('refresh');
            var selectVal = [];
            $(ResellerDetailsJsonParse.ResellerBillingCycle.trim(",").split(",")).each(function (index, element) {
                selectVal.push(element);
            });
            //$('select[name=ddlUpdateMacResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlUpdateMacResellerBillingCycle").val(selectVal);
            $("#ddlUpdateMacResellerBillingCycle").selectpicker('refresh');


            $("#ddlMacReselerMikrotikUpdate").selectpicker('refresh');
            var selectMacMikrotikVal = [];
            $(ResellerDetailsJsonParse.MacResellerAssignMikrotik.trim(",").split(",")).each(function (index, element) {
                selectMacMikrotikVal.push(element);
            });
            //$('select[name=ddlUpdateBandwithResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlMacReselerMikrotikUpdate").val(selectMacMikrotikVal);
            $("#ddlMacReselerMikrotikUpdate").selectpicker('refresh');


            //$("#ddlUpdateMacResellerBillingCycle").val(ResellerDetailsJsonParse.ResellerBillingCycle);
            $("#ddlResellerStatus").val(ResellerDetailsJsonParse.ResellerStatus);

            $("#PreviewMacResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#tblMacResellerPackageUpdate>tbody>tr").remove();
            $.each(ResellerDetailsJsonParse.macResellerGivenPackagePriceModel, function (index, item) {
                $("#tblMacResellerPackageUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + item.PID + "></td><td>" + item.PName + "</td><td><input type='text' class='form-group' value=" + item.PPAdmin + "></td><td><input type='text' class='form-group' value=" + item.PPFromRS + "></td><td align='center'><button id='btnDeleteMacUserPackageFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
            });
            $("#mdlMacResellerUpdate").modal("show");
        }
    },
    ShowResellerDetailsByIDForUpdateError: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    ShowResellerPaymentDetailsHistoryByIDForUpdate: function (ResellerID) {
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/Reseller/GetResellerDetailsHistoryByID/";
        var data = { RID: RID };
        data = ResellerManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.ShowResellerPaymentDetailsHistoryByIDForUpdateSuccess, ResellerManager.ShowResellerPaymentDetailsHistoryByIDForUpdateError);

        //}, 500);

    },
    ShowResellerPaymentDetailsHistoryByIDForUpdateSuccess: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
        var ResellerDetailsJsonParse = (data.ResellerDetails);

        if (ResellerDetailsJsonParse.ResellerTypeID == 1) {
            ResellerManager.ClearBandwithResellerDuringUpdateShow();

            $("#txtUpdateBandwithResellerName").val(ResellerDetailsJsonParse.ResellerName);
            $("#txtUpdateBandwithResellerBusinessName").val(ResellerDetailsJsonParse.ResellerBusinessName);
            $("#txtUpdateBandwithResellerLoginName").val(ResellerDetailsJsonParse.ResellerLoginName);
            $("#txtUpdateBandwithResellerPassword").val(ResellerDetailsJsonParse.ResellerPassword);
            //$("#rdbUpdateBandwithResellerType").val(ResellerDetailsJsonParse.ResellerType);
            $("input[name='rdbUpdateBandwithResellerType'][value='" + ResellerDetailsJsonParse.ResellerType + "']").prop("checked", true);
            $("#txtUpdateBandwithResellerAddress").val(ResellerDetailsJsonParse.ResellerAddress);
            $("#txtUpdateBandwithResellerContact").val(ResellerDetailsJsonParse.ResellerPhone);
            $("#PreviewBandwithResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#BandwithResellerUpdateImagePaths").val(ResellerDetailsJsonParse.ResellerLogoPath);
            $("#ddlUpdateBandwithResellerBillingCycle").selectpicker('refresh');

            var selectVal = [];
            $(ResellerDetailsJsonParse.ResellerBillingCycle.trim(",").split(",")).each(function (index, element) {
                selectVal.push(element);
            });
            //$('select[name=ddlUpdateBandwithResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlUpdateBandwithResellerBillingCycle").val(selectVal);
            $("#ddlUpdateBandwithResellerBillingCycle").selectpicker('refresh');


            //$("#ddlUpdateBandwithResellerBillingCycle").val(ResellerDetailsJsonParse.ResellerBillingCycle);
            $("#ddlResellerStatus").val(ResellerDetailsJsonParse.ResellerStatus);
            $("#tblBandwithResellerItemUpdate>tbody>tr").remove();
            $.each(ResellerDetailsJsonParse.bandwithReselleGivenItemWithPriceModel, function (index, item) {
                $("#tblBandwithResellerItemUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + item.ItemID + "></td><td>" + item.ItemName + "</td><td><input type='text' value=" + item.ItemPrice + " class='form-group' onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text'  value=" + item.ItemQuantity + "  class='form-group'   onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td><input type='text'  value=" + item.ItemTotalPrice + "  class='form-group'  onkeyup='SetValuesInOthersColtextForBandwithResellerUpdate(this.parentNode.parentNode)'></td><td align='center'><button id='btnDeleteBandwithUserItemFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
            });

            $("#mdlBandwithResellerUpdate").modal("show");
        }
        if (ResellerDetailsJsonParse.ResellerTypeID == 2) {

            ResellerManager.ClearMacResellerDuringUpdateShow();

            $("#txtUpdateMacResellerName").val(ResellerDetailsJsonParse.ResellerName);
            $("#txtUpdateMacResellerBusinessName").val(ResellerDetailsJsonParse.ResellerBusinessName);
            $("#txtUpdateMacResellerLoginName").val(ResellerDetailsJsonParse.ResellerLoginName);
            $("#txtUpdateMacResellerPassword").val(ResellerDetailsJsonParse.ResellerPassword);
            //$("#rdbUpdateMacResellerType").val(ResellerDetailsJsonParse.ResellerType);
            $("input[name='rdbUpdateMacResellerType'][value='" + ResellerDetailsJsonParse.ResellerType + "']").prop("checked", true);
            $("#txtUpdateMacResellerAddress").val(ResellerDetailsJsonParse.ResellerAddress);
            $("#txtUpdateMacResellerContact").val(ResellerDetailsJsonParse.ResellerPhone);
            $("#PreviewMacResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#MacResellerUpdateImagePaths").val(ResellerDetailsJsonParse.ResellerLogoPath);

            $("#ddlUpdateMacResellerBillingCycle").selectpicker('refresh');
            var selectVal = [];
            $(ResellerDetailsJsonParse.ResellerBillingCycle.trim(",").split(",")).each(function (index, element) {
                selectVal.push(element);
            });
            //$('select[name=ddlUpdateMacResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlUpdateMacResellerBillingCycle").val(selectVal);
            $("#ddlUpdateMacResellerBillingCycle").selectpicker('refresh');


            $("#ddlMacReselerMikrotikUpdate").selectpicker('refresh');
            var selectMacMikrotikVal = [];
            $(ResellerDetailsJsonParse.MacResellerAssignMikrotik.trim(",").split(",")).each(function (index, element) {
                selectMacMikrotikVal.push(element);
            });
            //$('select[name=ddlUpdateBandwithResellerBillingCycle]').val(selectVal);
            //$('.selectpicker').selectpicker('refresh');
            $("#ddlMacReselerMikrotikUpdate").val(selectMacMikrotikVal);
            $("#ddlMacReselerMikrotikUpdate").selectpicker('refresh');


            //$("#ddlUpdateMacResellerBillingCycle").val(ResellerDetailsJsonParse.ResellerBillingCycle);
            $("#ddlResellerStatus").val(ResellerDetailsJsonParse.ResellerStatus);

            $("#PreviewMacResellerUpdateImagePaths").prop("src", ResellerDetailsJsonParse.ResellerLogoPath);
            $("#tblMacResellerPackageUpdate>tbody>tr").remove();
            $.each(ResellerDetailsJsonParse.macResellerGivenPackagePriceModel, function (index, item) {
                $("#tblMacResellerPackageUpdate>tbody").append("<tr><td hidden><input type='hidden' value=" + item.PID + "></td><td>" + item.PName + "</td><td><input type='text' class='form-group' value=" + item.PP + "></td><td align='center'><button id='btnDeleteMacUserPackageFromTableUpdate' type='button' class='btn btn-default btn-sm btn-circle padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td></tr>");
            });
            $("#mdlMacResellerUpdate").modal("show");
        }
    },
    ShowResellerPaymentDetailsHistoryByIDForUpdateError: function (data) {
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateMacResellerInformation: function () {

        ////AppUtil.ShowWaitingDialog();
        //var ResellerName = AppUtil.GetIdValue("txtUpdateResellerName");
        //var ResellerLoginName = AppUtil.GetIdValue("txtUpdateResellerLoginName");
        //var ResellerPassword = AppUtil.GetIdValue("txtUpdateResellerPassword");
        //var ResellerTypeListID = $("input[name=rdbUpdateResellerType]:checked").val();
        //var ResellerAddress = AppUtil.GetIdValue("txtUpdateResellerAddress");
        //var ResellerContact = AppUtil.GetIdValue("txtUpdateResellerContact");
        //var ResellerBillingCycleList = "";
        //$.each($("#ddlUpdateResellerBillingCycle").val(), function (index, item) {
        //    ResellerBillingCycleList += item + ",";
        //});
        //ResellerBillingCycleList = ResellerBillingCycleList.trim();
        //var ResellerStatus = $("#ddlResellerStatus").val();

        //var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        //var header = {};
        //header['__RequestVerificationToken'] = AntiForgeryToken;

        //var Reseller = {
        //    ResellerID: RID,
        //    ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerPassword: ResellerPassword,
        //    ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
        //    ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
        //};
        //var PackageWithPrice = [];
        //$("#tblResellerPackageUpdate>tbody>tr").each(function (index, item) {
        //    var pid = $(this).closest("tr").find("td:eq(0) input").val();
        //    var pprice = $(this).closest("tr").find("td:eq(2) input").val();
        //    PackageWithPrice.push({ ResellerPackageID: pid, PackagePrice: pprice });
        //});
        var url = "/Reseller/UpdateMacResellerFromPopUp";
        //var data = JSON.stringify({ ResellerInfoForUpdate: Reseller, CRP: PackageWithPrice });
        //AppUtil.MakeAjaxCallJSONAntifergeryJSON(url, "POST", data, header, ResellerManager.UpdateResellerInformationSuccess, ResellerManager.UpdateResellerInformationFail);

        var ResellerName = AppUtil.GetIdValue("txtUpdateMacResellerName");
        var ResellerLoginName = AppUtil.GetIdValue("txtUpdateMacResellerLoginName");
        var ResellerBusinessName = AppUtil.GetIdValue("txtUpdateMacResellerBusinessName");
        var ResellerPassword = AppUtil.GetIdValue("txtUpdateMacResellerPassword");
        var ResellerTypeListID = 2;//$("input[name=rdbUpdateMacResellerType]").val();
        var ResellerAddress = AppUtil.GetIdValue("txtUpdateMacResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("txtUpdateMacResellerContact");
        var ResellerBillingCycleList = "";
        var ResellerLogoPath = $("#MacResellerUpdateImagePaths").val();
        $.each($("#ddlUpdateMacResellerBillingCycle").val(), function (index, item) {
            ResellerBillingCycleList += item + ",";
        });
        ResellerBillingCycleList = ResellerBillingCycleList.trim();
        var ResellerStatus = $("#ddlUpdateMacResellerStatus").val();


        var MacResellerAssignMikrotik = "";
        $.each($("#ddlMacReselerMikrotikUpdate").val(), function (index, item) {
            MacResellerAssignMikrotik += item + ",";
        });
        MacResellerAssignMikrotik = MacResellerAssignMikrotik.trimLeft(",").trimRight(",");

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        //  setTimeout(function () {
        //var Reseller = [];
        //Reseller.push({
        //    ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerPassword: ResellerPassword,
        //    ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
        //    ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
        //});
        var ResellerPackage = [];
        //var ResellerPackage = {
        //    PackageName
        //};
        $("#tblMacResellerPackageUpdate>tbody>tr").each(function () {
            var pID = $(this).closest("tr").find("td:eq(0) input").val();
            var pName = $(this).closest("tr").find("td:eq(1)").html();
            var ppAdmin = $(this).closest("tr").find("td:eq(2) input").val();
            var ppFromRS = $(this).closest("tr").find("td:eq(3) input").val();
            ResellerPackage.push({ PID: pID, PName: pName, PPAdmin: ppAdmin, PPAdmin: ppAdmin, PPFromRS: ppFromRS });
        });
        var Reseller = {
            ResellerID: _resellerID, ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerBusinessName: ResellerBusinessName, ResellerPassword: ResellerPassword,
            ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact,
            ResellerBillingCycleList: ResellerBillingCycleList, ResellerStatus: ResellerStatus
            , macResellerGivenPackagePriceModel: ResellerPackage, ResellerLogoPath: ResellerLogoPath, MacResellerAssignMikrotik: MacResellerAssignMikrotik
        };

        var formData = new FormData();
        formData.append('ResellerLogoImageBytes', $('#MacResellerUpdateImage')[0].files[0]);
        formData.append('Reseller_Client', JSON.stringify(Reseller));

        //var datas = JSON.stringify({ Reseller_Client: Reseller });
        //AppUtil.MakeAjaxCallJSONAntifergeryJSON(url, "POST", datas, header, ResellerManager.UpdateResellerFromPopUpSuccess, ResellerManager.UpdateResellerFromPopUpFail);

        //AppUtil.MakeAjaxCall(url, "POST", datas, ResellerManager.UpdateResellerFromPopUpSuccess, ResellerManager.UpdateResellerFromPopUpFail);
        AppUtil./*MakeAjaxCallJSONAntifergeryJSON*/MakeAjaxCallJSONAntifergery(url, "POST", /*datas*/formData, header, ResellerManager.UpdateMacResellerFromPopUpSuccess, ResellerManager.UpdateMacResellerFromPopUpFail);
        // }, 500);

    },
    UpdateMacResellerFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.UpdateSuccess === true) {
            var ResellerInformation = (data.ResellerUpdateInformation);

            $("#tblReseller tbody>tr").each(function () {

                var ResellerID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ResellerInformation.ResellerID == ResellerID) {

                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ResellerInformation.ResellerName);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(2)").text(ResellerInformation.ResellerLoginName);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(3)").text(ResellerInformation.ResellerAddress);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(4)").text(ResellerInformation.ResellerContact);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(5)").text(ResellerInformation.ResellerTypeListID);
                    //$('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(5)").text(data.ResellerType);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(7)").text(ResellerInformation.ResellerBillingCycleList.trim(","));
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(8)").text(ResellerInformation.MacResellerAssignMikrotik.trim(","));
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(9)").html(ResellerInformation.macReselleGivenPackageWithPrice.trim(","));

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (data.CustomMessage != '') {
                AppUtil.ShowSuccess(data.CustomMessage);
            }
            if (data.AlreadyInsert == true) {
                AppUtil.ShowSuccess("Reseller Already Added.Choose different Reseller.");
            }
        }

        $("#mdlMacResellerUpdate").modal('hide');

        ResellerManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateMacResellerFromPopUpFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    UpdateBandwithResellerInformation: function () {

        var url = "/Reseller/UpdateBandwithResellerFromPopUp";
        var ResellerName = AppUtil.GetIdValue("txtUpdateBandwithResellerName");
        var ResellerLoginName = AppUtil.GetIdValue("txtUpdateBandwithResellerLoginName");
        var ResellerBusinessName = AppUtil.GetIdValue("txtUpdateBandwithResellerBusinessName");
        var ResellerPassword = AppUtil.GetIdValue("txtUpdateBandwithResellerPassword");
        var ResellerTypeListID = 1;//$("input[name=rdbUpdateBandwithResellerType]").val();
        var ResellerAddress = AppUtil.GetIdValue("txtUpdateBandwithResellerAddress");
        var ResellerContact = AppUtil.GetIdValue("txtUpdateBandwithResellerContact");
        var ResellerStatus = $("#ddlUpdateBandwithResellerStatus").val();
        var ResellerLogoPath = $("#BandwithResellerUpdateImagePaths").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var BandwithResellerItem = [];
        $("#tblBandwithResellerItemUpdate>tbody>tr").each(function () {
            var itemID = $(this).closest("tr").find("td:eq(0) input").val();
            var ItemName = $(this).closest("tr").find("td:eq(1)").text();
            var itemPrice = $(this).closest("tr").find("td:eq(2) input").val();
            var itemQuantity = $(this).closest("tr").find("td:eq(3) input").val();
            var itemTotal = $(this).closest("tr").find("td:eq(4) input").val();
            BandwithResellerItem.push({ ItemID: itemID, ItemName: ItemName, ItemPrice: itemPrice, ItemQuantity: itemQuantity, ItemTotalPrice: itemTotal });
        });
        var Reseller = {
            ResellerID: _resellerID, ResellerName: ResellerName, ResellerLoginName: ResellerLoginName, ResellerBusinessName: ResellerBusinessName, ResellerPassword: ResellerPassword,
            ResellerTypeListID: ResellerTypeListID, ResellerAddress: ResellerAddress, ResellerContact: ResellerContact, ResellerStatus: ResellerStatus
            , bandwithReselleGivenItemWithPriceModel: BandwithResellerItem, ResellerLogoPath: ResellerLogoPath
        };

        var formData = new FormData();
        formData.append('ResellerLogoImageBytes', $('#BandwithResellerUpdateImage')[0].files[0]);
        formData.append('Reseller_Client', JSON.stringify(Reseller));

        //var datas = JSON.stringify({ Reseller_Client: Reseller });
        //AppUtil.MakeAjaxCallJSONAntifergeryJSON(url, "POST", datas, header, ResellerManager.UpdateResellerFromPopUpSuccess, ResellerManager.UpdateResellerFromPopUpFail);

        //AppUtil.MakeAjaxCall(url, "POST", datas, ResellerManager.UpdateResellerFromPopUpSuccess, ResellerManager.UpdateResellerFromPopUpFail);
        AppUtil./*MakeAjaxCallJSONAntifergeryJSON*/MakeAjaxCallJSONAntifergery(url, "POST", /*datas*/formData, header, ResellerManager.UpdateBandwithResellerFromPopUpSuccess, ResellerManager.UpdateBandwithResellerFromPopUpFail);
        // }, 500);

    },
    UpdateBandwithResellerFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.UpdateSuccess === true) {
            var ResellerInformation = (data.ResellerUpdateInformation);

            $("#tblReseller tbody>tr").each(function () {

                var ResellerID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ResellerInformation.ResellerID == ResellerID) {

                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ResellerInformation.ResellerName);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(2)").text(ResellerInformation.ResellerLoginName);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(3)").text(ResellerInformation.ResellerAddress);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(4)").text(ResellerInformation.ResellerContact);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(5)").text(ResellerInformation.ResellerTypeListID);
                    //$('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(5)").text(data.ResellerType);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(7)").text(ResellerInformation.ResellerBillingCycleList);
                    $('#tblReseller tbody>tr:eq(' + index + ')').find("td:eq(10)").html(ResellerInformation.BandwithReselleItemGivenWithPrice);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Reseller Already Added. Choose different Reseller. ");
            }
        }

        $("#mdlBandwithResellerUpdate").modal('hide');

        ResellerManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateBandwithResellerFromPopUpFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },


    PrintResellerList: function () {

        var url = "/Excel/CreateReportForResellerList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({});
        data = ResellerManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerManager.PrintResellerListSuccess, ResellerManager.PrintResellerListFail);
    },
    PrintResellerListSuccess: function (data) {

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
    PrintResellerListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#ResellerName").val("");
        $("#ResellerAddress").val("");
        $("#ResellerContact").val("");
    },
    clearForUpdateInformation: function () {
        $("#ResellerNames").val("");
        $("#ResellerAddresss").val("");
        $("#ResellerContacts").val("");
    },
    ClearForPaymentInformation: function () {

        _resellerID = "";
        _resellerPaymentID = "";
        $("#txtResellerLoginName").prop('disabled', false).val("");
        $("#txtResellerType").val("");
        $("#txtResellerBusinessName").val("");
        $("#txtResellerAddress").val("");
        $("#txtResellerPhone").val("");
        $("#ResellerLogoPath").prop("src", "");
        $("#txtResellerPaymentAmount").val("");
        $("#ddlResellerCollectBy").val("").select2({ width: '100%' }).trigger('change');
        $("#txtCheckSerialOrAnyResetNo").val("");
        $("#ddlPaymentBy").val("").select2({ width: '100%' }).trigger('change');
        $("#ddlPaymentType").val("");
        $("#ddlPaymentStatus").val("2").select2({ width: '100%' }).trigger('change');
    },
    ClearForPaymentInformationFromResellerPage: function () {
        //$("#txtResellerLoginName").prop('disabled', false).val("");
        $("#txtResellerType").val("");
        $("#txtResellerBusinessName").val("");
        $("#txtResellerAddress").val("");
        $("#txtResellerPhone").val("");
        $("#ResellerLogoPath").prop("src", "");
        $("#txtResellerPaymentAmount").val("");
        $("#ddlResellerCollectBy").val("").select2({ width: '100%' }).trigger('change');
        $("#txtCheckSerialOrAnyResetNo").val("");
        $("#ddlPaymentBy").val("").select2({ width: '100%' }).trigger('change');
        $("#ddlPaymentType").val("");
        $("#ddlPaymentStatus").val("2").select2({ width: '100%' }).trigger('change');
    },
    ClearResellerRowIndexRPIDFromResellerPage: function () {
        _resellerID = "";
        _rowIndex = "";
        _resellerPaymentID = "";
    }
}