var BPManager = {
    addRequestVerificationToken: function (data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    MonthNames: function (m) {

        var res;
        switch (m) {
            case 1:
                res = "Jan";
                break;
            case 2:
                res = "Feb";
                break;
            case 3:
                res = "Mar";
                break;
            case 4:
                res = "Apr";
                break;
            case 5:
                res = "May";
                break;
            case 6:
                res = "Jun";
                break;
            case 7:
                res = "Jul";
                break;
            case 8:
                res = "Agu";
                break;
            case 9:
                res = "Sep";
                break;
            case 10:
                res = "Oct";
                break;
            case 11:
                res = "Nov";
                break;
            case 12:
                res = "Dec";
                break;
            default:
                res = "Empty";
                break;
        }
        return res;
    },

    PayValidation: function () {
        var status = true;
        if (AppUtil.GetIdValue("txtPay") === '') {
            AppUtil.ShowErrorOnControl("Please Add Amount For Payment.", "txtPay", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Who Collect The Bill.", "EmployeeID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("PaymentFrom") === '') {
            AppUtil.ShowErrorOnControl("Please Select From Where Client Is Making Payment", "PaymentFrom", "top center");
            status = false;
        }
        return status == false ? false : true;
    },

    GetClientPaymentAmountAndRemarksAndSleepNoForDuePayment: function (id) {
        var url = "/Transaction/GetRemarksAndSleepNoForPaymentForAllPayment/";
        //var url = "/Transaction/GetRemarksAndSleepNoForPaymentForLineMan/";
        var data = { TransactionID: id };
        data = BPManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BPManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess, BPManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError);
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess: function (data) {
        console.log(data);
        if (data.success === true) {
            var Transaction = (data.transaction);

            $("#txtMonthName").val(BPManager.MonthNames(Transaction.MonthName));
            $("#txtSpecificLoginName").val(Transaction.LoginName);
            $("#txtUserID").val(Transaction.UserID);
            $("#txtTotalAmount").val(Transaction.PaymentAmount);
            $("#txtPaidAmount").val(Transaction.PaidAmount);
            $("#txtAmount").val(Transaction.DueAmount);
            $("#txtDiscount").val(Transaction.DiscountAmount);
            $("#txtMoneyResetNo").val("");
            $("#txtRemarks").val(data.RemarksNo);
            $("#txtPermanentDiscount").val(Transaction.PermanentDiscount);


            if (data.OneTypePaymentOrMultiplePayment === false) {
                $("#txtPay").val(Transaction.PaymentAmount).prop("disabled", "true");
                $("#txtNewDiscount").val(0).prop("disabled", "true");
                $("#EmployeeID").val(data.lID).prop("disabled", "true"); 
            }
        }
        $("#mdlMakePayment").modal("show");
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError: function (data) {
        $("#txtTotalAmount").val("");
        $("#txtPaidAmount").val("");
        $("#txtAmount").val("");
        $("#txtDiscount").val("");
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
        $("#mdlMakePayment").modal("show");

        // for accounts and unpaid bills page not for billpaybyadmin Or me specific month
        if (_nmlPaymentEnable == true) {
            _nmlPaymentEnable = false;
        }
        if (_duePaymentEnable == true) {
            _duePaymentEnable = false;
        }
        // end accounts and unpaid bills page
    },

    PayBill: function (T_ID, PaymentFromWhichPage) {
        var url = "/Transaction/PayBillByAdminOrEmployeeOrReseller/";//"";
        //if (PaymentFromWhichPage == 1) {//account page
        //    url = "/Transaction/PayMonthlyBill/";
        //}
        //else if (PaymentFromWhichPage == 2) {//unpaid page
        //    url = "/Transaction/PayMonthlyBill/";
        //}
        //else if (PaymentFromWhichPage == 8) {
        //    url = "/Transaction/PayBillByBillManSpecificMonth/";
        //}
        //else if (PaymentFromWhichPage == 9) {
        //    url = "/Transaction/PayBillByEmployeeOrAdminSpecificMonth/"; 
        //}

        //var tid = T_ID;
        var PaidAmount = $("#txtPay").val();
        var Discount = $("#txtNewDiscount").val();
        var ResetNo = $("#txtMoneyResetNo").val();
        var RemarksNo = $("#txtRemarks").val();
        var PaymentBy = $("#PaymentFrom").val();
        var BillCollectBy = $("#EmployeeID").val();
        var AnotherMobileNo = $("#txtAnotherMobileNo").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var Transaction = { ClientDetailsID: _ClientDetailsID, TransactionID: T_ID, PaidAmount: PaidAmount, BillCollectBy: BillCollectBy, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo, PaymentBy: PaymentBy, AnotherMobileNo: AnotherMobileNo, PaymentFromWhichPage: PaymentFromWhichPage };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, BPManager.PayBillSuccess, BPManager.PayBillError);


        //var OwnerDetails = { OwnerName: "xyz" };
        //var data = JSON.stringify({ OwnerDetails: OwnerDetails });
        //AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, BPManager.PayBillSuccess, BPManager.PayBillError);

        //var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        //var header = {};
        //header['__RequestVerificationToken'] = AntiForgeryToken;

        //var Transaction = { ClientDetailsID: ClientDetailsID, TransactionID: T_ID, PaidAmount: PaidAmount, BillCollectBy: BillCollectBy, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo, PaymentBy: PaymentBy, AnotherMobileNo: AnotherMobileNo, PaymentFromWhichPage: PaymentFromWhichPage };
        //var data = JSON.stringify({ Transaction: Transaction });
        //AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, BPManager.PayBillSuccess, BPManager.PayBillError); 
    },
    PayBillSuccess: function (data) {
        //if (data.message) {
        //    console.log(data.message);
        //}


        if (data.paymentfromwhichpage == 1) {//acc nml bill
            if (data.success === true) {
                AppUtil.ShowSuccess(data.message);
                BPManager.ClearPaymentDetails();
                $("#mdlMakePayment").modal('hide');
                table.draw();
            }
            if (data.success === false) {
                if (data.resetnoalreadyexist === true) {
                    AppUtil.ShowError(data.message);
                }
            }
            if (_tempTrxID != "") {//meaning this is call for due bill and we have to load the due bill list after due bil payment.
                                   //cause we load the temptrxid when click on main table to load the due table.
                BPManager.ShowDueBillDetailssByTransactionID(_tempTrxID);
            }
        }
        else if (data.paymentfromwhichpage == 2) {//unp nml bill
            if (data.success === true) {
                AppUtil.ShowSuccess(data.message);
                BPManager.ClearPaymentDetails();
                $("#mdlMakePayment").modal('hide');
                table.draw();
            }
            if (data.success === false) {
                if (data.resetnoalreadyexist === true) {
                    AppUtil.ShowError(data.message);
                }
            }
            if (_tempTrxID != "") {//meaning this is call for due bill and we have to load the due bill list after due bil payment.
                //cause we load the temptrxid when click on main table to load the due table.
                BPManager.ShowDueBillDetailssByTransactionID(_tempTrxID);
            }
        }
        else if (data.paymentfromwhichpage == 8) {//submit bill me specific month
            if (data.success === true) {
                AppUtil.ShowSuccess(data.message);
                BPManager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(_ClientDetailsID)
                BPManager.ClearPaymentDetails();
            }
            if (data.success === false) {
                AppUtil.ShowError(data.message);
            }
            $("#mdlMakePayment").modal("hide");
        }
        else if (data.paymentfromwhichpage == 9) { //submit bill me admin specific month
            if (data.success === true) {
                AppUtil.ShowSuccess(" Payment Successfully done.");
                BPManager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(_ClientDetailsID)
                BPManager.ClearPaymentDetails();
            }
            if (data.success === false) {
                AppUtil.ShowError(data.message);
            }
            $("#mdlMakePayment").modal("hide");
        }


        //if (data.PaymentFromWhichPage == 1) {//acc nml bill
        //    if (data.Success === true) {
        //        AppUtil.ShowSuccess("Payment Successfully done."); 
        //        BPManager.ClearPaymentDetails();
        //        $("#mdlMakePayment").modal('hide');
        //        table.draw();
        //    }
        //    if (data.Success === false) {
        //        if (data.ResetNoAlreadyExist === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        } else {
        //            AppUtil.ShowError("Payment Not done.");
        //        }
        //    }
        //}
        //else if (data.PaymentFromWhichPage == 2) {//unp nml bill
        //    if (data.Success === true) {
        //        AppUtil.ShowSuccess("Payment Successfully done."); 
        //        BPManager.ClearPaymentDetails();
        //        $("#mdlMakePayment").modal('hide');
        //        table.draw();
        //    }
        //    if (data.Success === false) {
        //        if (data.ResetNoAlreadyExist === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        } else {
        //            AppUtil.ShowError("Payment  Not done.");
        //        }
        //    }
        //}
        //else if (data.PaymentFromWhichPage == 8) {//submit bill me specific month
        //    if (data.Success === true) {
        //        AppUtil.ShowSuccess(" Payment Successfully done.");
        //        BPManager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(_ClientDetailsID)
        //        BPManager.ClearPaymentDetails();
        //    }
        //    if (data.Success === false) {
        //        if (data.ResetNoAlreadyExist === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        }
        //        else if (data.DiscounGreterThanZero === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        }
        //        else {
        //            AppUtil.ShowError("Payment UnSuccessfull.");
        //        }
        //    }
        //    $("#mdlMakePayment").modal("hide");
        //}
        //else if (data.PaymentFromWhichPage == 9) { //submit bill me admin specific month
        //    if (data.Success === true) {
        //        AppUtil.ShowSuccess(" Payment Successfully done.");
        //        BPManager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(_ClientDetailsID)
        //        BPManager.ClearPaymentDetails();
        //    }
        //    if (data.Success === false) {
        //        if (data.ResetNoAlreadyExist === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        } if (data.DiscounGreterThanZero === true) {
        //            AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //        } else {
        //            AppUtil.ShowError("Payment UnSuccessfull.");
        //        }
        //    }
        //    $("#mdlMakePayment").modal("hide");
        //}
    },
    PayBillError: function (data) {
        //$("#mdlMakePayment").modal("hide");
        AppUtil.ShowError("Update Information Failed. Please Contact With Administrator.");
        console.log(data);
        //window.location.reload();
        BPManager.ClearPaymentDetails();
        $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
    },


    ShowDueBillDetailssByTransactionID: function (transactionID) {
        var url = "/Transaction/ShowDueBillDetailssByTransactionID/";
        var data = { TransactionID: transactionID };
        data = BPManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, BPManager.ShowDueBillDetailssByTransactionIDSuccess, BPManager.ShowDueBillDetailssByTransactionIDFail);
    },
    ShowDueBillDetailssByTransactionIDSuccess: function (data) {

        console.log(data);
        $("#tblShowDueBillsList>tbody").empty();
        $.each(data.DueTransactionList, function (index, item) {

            $("#tblShowDueBillsList>tbody").append('<tr><td><input type="radio" name="chkPayDueBills" /></td><td><input type="hidden" id="hdnTransactionID" value=' + item.TransactionID + '></td><td>' + BPManager.MonthNames(item.MonthName) + '</td><td>' + item.PackageName + '</td><td>' + item.PackagePrice + '</td><td>' + item.PaidAmount + '</td><td>' + item.Discount + '</td><td>' + item.Due + '</td><td>' + item.Status + '</td></tr>');
            console.log(index, item);
        });

        $("#dueBillName").html(data.ClientDetails.ClientName);
        $("#dueBillLoginName").html(data.ClientDetails.ClientLoginName);
        $("#dueBillClientZone").html(data.ClientDetails.ClientZoneName);
        $("#dueBillClientAddress").html(data.ClientDetails.ClientAddress);
        $("#dueBillConnectionType").html(data.ClientDetails.ClientConnectionType);
        $("#dueBillContactNumber").html(data.ClientDetails.ClientContactNumber);


        $("#mdlShowDueBillsList").modal("show");
    },
    ShowDueBillDetailssByTransactionIDFail: function () {

        console.log(data);
        AppUtil.ShowError("Fail");
    },

    ShowDueBillDetailssByClientDetailsIDSpecificMonth: function (ClientDetailsID) {
        var url = "/Transaction/ShowDueBillDetailssByClientDetailsID/";
        var data = { ClientDetailsID: ClientDetailsID };
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.ShowDueBillDetailssByClientDetailsIDSpecificMonthSuccess, B_P_Manager.ShowDueBillDetailssByClientDetailsIDSpecificMonthFail);
    },
    ShowDueBillDetailssByClientDetailsIDSpecificMonthSuccess: function (data) {
        $("#tblClientDueBillsList>tbody>tr").remove();
        $.each(data.DueTransactionList, function (index, item) {
            $("#tblClientDueBillsList>tbody").append('<tr><td hidden><input type="hidden" id="hdnTransactionID" value=' + item.TransactionID + '></td><td><input type="radio" name="chkPayDueBills" /></td><td>' + B_P_Manager.MonthNames(item.MonthName) + '</td><td>' + item.PackageName + '</td><td>' + item.PaymentAmount + '</td><td>' + item.PaidAmount + '</td><td>' + item.Discount + '</td><td>' + item.DueAmount + '</td><td>' + item.Status + '</td></tr>');
        });
    },
    ShowDueBillDetailssByClientDetailsIDSpecificMonthFail: function () {
        console.log(data);
        AppUtil.ShowError("Fail");
    },

    ClearPaymentDetails: function () {
        T_ID = "";

        $("#txtMonthName").val("");
        $("#txtSpecificLoginName").val("");
        $("#txtUserID").val("");
        $("#txtTotalAmount").val("");
        $("#txtPaidAmount").val("");
        $("#txtDiscount").val("");
        $("#txtAmount").val("");
        $("#txtPay").val("");
        $("#txtNewDiscount").val("");
        $("#txtPermanentDiscount").val("");
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
        $("#PaymentFrom").prop("selectedIndex", 0);
        $("#txtAnotherMobileNo").val("");
    },

    ClearModalDueBillPageInformation: function (pageName) {
        if (pageName == "submitbillbymespecificmonth") {

            BPManager.ClearCommonInfo();

            $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
            $("#mdlMakePayment").modal("hide");
        }
        else if (pageName == "submitbillbymeadminspecificmonth") {

            BPManager.ClearCommonInfo();

            $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
            $("#mdlMakePayment").modal("hide");
        }
        else if (pageName == "accounts") {
            if (_nmlPaymentEnable) {
                T_ID = ""; _nmlPaymentEnable = false;
                $("#btnShowBillPaymentWindow").css("visibility", "hidden");
                BPManager.ClearCommonInfo();
                $("input[name='chkBillPay']").prop('checked', false);
                $("#mdlMakePayment").modal("hide");
            }
            else {// mean due bill enabled
                T_ID = ""; _duePaymentEnable = false;
                $("#btnShowBillPaymentWindow").css("visibility", "hidden");
                BPManager.ClearCommonInfo();
                $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
                $("#mdlMakePayment").modal("hide");
            }

        }
        else if (pageName == "unpaidbill") { 
            if (_nmlPaymentEnable) {
                T_ID = ""; _nmlPaymentEnable = false;
                $("#btnShowBillPaymentWindow").css("visibility", "hidden");
                BPManager.ClearCommonInfo();
                $("input[name='chkBillPay']").prop('checked', false);
                $("#mdlMakePayment").modal("hide");
            }
            else {// mean due bill enabled
                T_ID = ""; _duePaymentEnable = false;
                $("#btnShowBillPaymentWindow").css("visibility", "hidden");
                BPManager.ClearCommonInfo();
                $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
                $("#mdlMakePayment").modal("hide");
            }
        }
        else {
        }
    },

    ClearCommonInfo: function () {
        $("#txtMonthName").val("");
        $("#txtSpecificLoginName").val("");
        $("#txtUserID").val("");
        $("#txtTotalAmount").val("");
        $("#txtPaidAmount").val("");
        $("#txtDiscount").val("");
        $("#txtAmount").val("");
        $("#txtPay").val("");
        $("#txtNewDiscount").val("");
        $("#txtPermanentDiscount").val("");
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
        $("#PaymentFrom").val("");
        $("#txtAnotherMobileNo").val("");
    }
}