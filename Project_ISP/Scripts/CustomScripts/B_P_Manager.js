var B_P_Manager = {

    //// Unnecessary////
    ShowDueBillDetailssByClientDetailsID: function (ClientDetailsID) {
        var url = "/Transaction/ShowDueBillDetailssByClientDetailsID/";
        var data = { ClientDetailsID: ClientDetailsID };
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.ShowDueBillDetailssByClientDetailsIDSuccess, B_P_Manager.ShowDueBillDetailssByClientDetailsIDFail);
    },
    ShowDueBillDetailssByClientDetailsIDSuccess: function (data) {

        //console.log(data);
        //$("#tblClientDueBillsList>tbody").empty();

        //$("#txtName").val(data.ClientDetails.Name);
        //$("#txtZone").val(data.ClientDetails.Zone);
        //$("#txtMobile").val(data.ClientDetails.Contact);
        //$("#txtAddress").val(data.ClientDetails.Address);


        $.each(data.DueTransactionList, function (index, item) {
            /*<td><input type="radio" name="chkPayDueBills" /></td>*/
            $("#tblClientDueBillsList>tbody").append('<tr><td hidden><input type="hidden" id="hdnTransactionID" value=' + item.TransactionID + '></td><td>' + B_P_Manager.MonthNames(item.MonthName) + '</td><td>' + item.PackageName + '</td><td>' + item.PaymentAmount + '</td><td>' + item.PaidAmount + '</td><td>' + item.Discount + '</td><td>' + item.DueAmount + '</td><td>' + item.Status + '</td></tr>');
            console.log(index, item);
        });
        if (data.DueTransactionList.length > 0) {
            $("#mdlClientDueBillsList").modal("show");
        }
    },
    ShowDueBillDetailssByClientDetailsIDFail: function () {

        console.log(data);
        AppUtil.ShowError("Fail");
    },

    GetPaymentMonthName: function (paymentMonth) {
        switch (paymentMonth) {
            case 1:
                return "January";
            case 2:
                return "February";
            case 3:
                return "March";
            case 4:
                return "April";
            case 5:
                return "May";
            case 6:
                return "June";
            case 7:
                return "July";
            case 8:
                return "August";
            case 9:
                return "September";
            case 10:
                return "October";
            case 11:
                return "November";
            case 12:
                return "December";
            default:
                return "";
        }
    },

    PayDueBillValidation: function () {


        if (AppUtil.GetIdValue("txtDueMoneyResetNo") === '') {
            AppUtil.ShowSuccess("Please Add Money Reset No. ");
            return false;
        }

        return true;
    },

    PayBillValidation: function () {


        if (AppUtil.GetIdValue("txtRegularBillPay") === '') {
            AppUtil.ShowSuccess("Please Add Pay Amount. ");
            return false;
        }
        //if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        return true;
    },
    PayValidation: function () {
        if (AppUtil.GetIdValue("txtResetNo") === '') {
            AppUtil.ShowSuccess("Please Add Reset Number. ");
            return false;
        }
        if (AppUtil.GetIdValue("txtPaymentAmount") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }
        return true;
    },
    PayValidationSpecificMonthByAdmin: function () {
        if (AppUtil.GetIdValue("txtResetNo") === '') {
            AppUtil.ShowSuccess("Please Add Reset Number. ");
            return false;
        }
        if (AppUtil.GetIdValue("txtPay") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }
        return true;
    },

    PayDueBill: function (TransactionID, FromWhere) {
        var discount = $("#txtDiscount").val();
        var resetNo = $("#txtMoneyResetNo").val();
        var remarksNo = $("#txtRemarks").val();
        var paidAmount = $("#txtRegularBillPay").val();
        var url = "/Transaction/PayDueBillByEmployeeHimself/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: paidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, B_P_Manager.PayDueBillSuccess, B_P_Manager.PayDueBillError);
    },
    PayDueBillSuccess: function (data) {


        if (data.Success === true) {
            AppUtil.ShowSuccess("Due Payment Successfully done.");
            //FullBillPaid = ts.PaymentStatus, 
            //PaidAmount = (ts.PaidAmount == null) ? 0 : ts.PaidAmount,
            //    Discount = ts.Discount == null ? 0 : ts.Discount, 
            //DueAmount = ts.DueAmount == null ? 0 : ts.DueAmount
            if (data.FullBillPaid == 1) {
                $("#tblClientDueBillsList>tbody>tr").each(function () {
                    var index = $(this).index();
                    var id = $(this).find("td:eq(0) input").val();
                    if (id == data.TransactionID) {
                        $("#tblClientDueBillsList>tbody>tr:eq(" + index + ")").remove();
                    }
                });
            }
            if (data.FullBillPaid == 0) {
                $("#tblClientDueBillsList>tbody>tr").each(function () {
                    var index = $(this).index();
                    var id = $(this).find("td:eq(0) input").val();
                    if (id == data.TransactionID) {
                        $("#tblClientDueBillsList>tbody>tr:eq(" + index + ")").find("td:eq(4)").html(data.PaidAmount);
                        $("#tblClientDueBillsList>tbody>tr:eq(" + index + ")").find("td:eq(5)").html(data.Discount);
                        $("#tblClientDueBillsList>tbody>tr:eq(" + index + ")").find("td:eq(6)").html(data.DueAmount);
                        $("#tblClientDueBillsList>tbody>tr:eq(" + index + ")").find("td:eq(7)").html("Not Paid");
                    }
                });
            }
            B_P_Manager.ClearModalDueBillPageInformation();

            $("#mdlMakeDuePayment").modal("hide");
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment UnSuccessfull.");
            }
        }
        //
        //AppUtil.ShowSuccess("Payment Successfully done.");
        //console.log(data);
        //window.location.href = "/Transaction/Accounts";
        $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
    },
    PayDueBillError: function (data) {


        alert("Fail");
        console.log(data);
        $('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
        window.location.reload();
    },

    ClearPaymentDetails: function () {
        //ClientDetailsID = "";
        T_DuePayment_ID = "";
        //$("#btnPayBill").css("visibility", "hidden");
        //$("#txtLoginName").val("");
        //$("#txtName").val("");
        //$("#txtZone").val("");
        //$("#txtMobile").val("");
        //$("#txtAddress").val("");
        //$("#txtTotalBillNeedToPay").val("");
        //$("#txtPaymentAmount").val("");
        //$("#txtDicountByAdmin").val("");
        //$("#txtDue").val("");
        //$("#txtAdvance").val("");
        //$("#tblClientDueBillsList>tbody>tr").empty();
        //$("#txtResetNo").val("");
        //$("#txtRemarksNo").val("");
        //$("#txtLoginName").val("");


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
    // Unnecessary end //





    // Necessary Start..///
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

    ShowClientInfoWIthSumOfTotalDueByClientDetailsID: function (ClientDetailsID) {
        debugger;
        var url = "/Transaction/ShowClientInfoWIthSumOfTotalDueByClientDetailsID/";
        var data = { ClientDetailsID: ClientDetailsID };
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.ShowClientInfoWIthSumOfTotalDueByClientDetailsIDSuccess, B_P_Manager.ShowClientInfoWIthSumOfTotalDueByClientDetailsIDFail);
    },
    ShowClientInfoWIthSumOfTotalDueByClientDetailsIDSuccess: function (data) {
        $("#txtName").val(data.ClientDetails.Name);
        $("#txtZone").val(data.ClientDetails.Zone);
        $("#txtMobile").val(data.ClientDetails.Contact);
        $("#txtAddress").val(data.ClientDetails.Address);

        $("#txtTotalBillNeedToPay").val(data.TotalAmountAfterDiscount);
        //$("#").val(data.PaidAmount);
        //$("#").val(data.Discount);
        console.log(data);
    },
    ShowClientInfoWIthSumOfTotalDueByClientDetailsIDFail: function (data) {

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
            //console.log(index, item);
        });
        //if (data.DueTransactionList.length > 0) {
        //    $("#mdlClientDueBillsList").modal("show");
        //}
    },
    ShowDueBillDetailssByClientDetailsIDSpecificMonthFail: function () {
        console.log(data);
        AppUtil.ShowError("Fail");
    },

    GetClientPaymentAmountAndRemarksAndSleepNoForDuePayment: function (id) {
        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';

        //AppUtil.ShowWaitingDialog();
        //  setTimeout(function () {
        var url = "/Transaction/GetRemarksAndSleepNoForPaymentForLineMan/";
        var data = { TransactionID: id };
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess, B_P_Manager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError);

        //   }, 500);

    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess: function (data) {
        console.log(data);
        if (data.Success === true) {
            var Transaction = (data.Transaction);

            $("#txtMonthName").val(B_P_Manager.GetPaymentMonthName(Transaction.MonthName));
            $("#txtSpecificLoginName").val(Transaction.LoginName);
            $("#txtUserID").val(Transaction.UserID);
            $("#txtTotalAmount").val(Transaction.PaymentAmount);
            $("#txtPaidAmount").val(Transaction.PaidAmount);
            $("#txtAmount").val(Transaction.DueAmount);
            $("#txtDiscount").val(Transaction.DiscountAmount);
            $("#txtMoneyResetNo").val("");
            $("#txtRemarks").val(data.RemarksNo);

        }


        $("#mdlMakePayment").modal("show");

        //$("#mdlPackageUpdate").modal("show");
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
        $("#txtTotalAmount").val("");
        $("#txtPaidAmount").val("");
        $("#txtAmount").val("");
        $("#txtDiscount").val("");
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
        $("#mdlMakePayment").modal("show");
    },

    PayBillByBillMan: function (ClientDetailsID, PaymentAmount, Discount, ResetNo, RemarksNo, PaymentFrom) {
        var url = "/Transaction/PayBillByBillMan/";

        var data = ({ ClientDetailsID: ClientDetailsID, PaymentAmount: PaymentAmount, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo, PaymentFrom: PaymentFrom });
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.PayBillByBillManSuccess, B_P_Manager.PayBillByBillManError);
    },
    PayBillByBillManSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess(" Payment Successfully done.");
            B_P_Manager.ClearPaymentDetails();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } if (data.DiscounGreterThanZero === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment UnSuccessfull.");
            }
        }
        $("#popModalForPayment").modal("hide");
    },
    PayBillByBillManError: function (data) {

        $("#popModalForPayment").modal("hide");
        alert("Fail");
        console.log(data);
        //$('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
        window.location.reload();
    },

    PayBillByBillManSpecificMonth: function () {
        var url = "/Transaction/PayBillByBillManSpecificMonth/";
        var tid = T_DuePayment_ID;
        var PaidAmount = $("#txtPay").val();
        var Discount = $("#txtNewDiscount").val();
        var ResetNo = $("#txtMoneyResetNo").val();
        var RemarksNo = $("#txtRemarks").val();
        var PaymentBy = $("#PaymentFrom").val();
        var BillCollectBy = $("#EmployeeID").val();
        var AnotherMobileNo = $("#txtAnotherMobileNo").val();
        var PaymentFromWhichPage = "Submit Bill By Me Specific Month";
        var data = ({ ClientDetailsID: ClientDetailsID, TransactionID: tid, PaidAmount: PaidAmount, BillCollectBy: BillCollectBy, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo, PaymentBy: PaymentBy, AnotherMobileNo: AnotherMobileNo, PaymentFromWhichPage: PaymentFromWhichPage });
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.PayBillByBillManSpecificMonthSuccess, B_P_Manager.PayBillByBillManSpecificMonthError);
    },
    PayBillByBillManSpecificMonthSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess(" Payment Successfully done.");
            B_P_Manager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(ClientDetailsID)
            B_P_Manager.ClearPaymentDetails();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } if (data.DiscounGreterThanZero === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment UnSuccessfull.");
            }
        }
        $("#mdlMakePayment").modal("hide");
    },
    PayBillByBillManSpecificMonthError: function (data) {

        $("#popModalForPayment").modal("hide");
        alert("Fail");
        console.log(data);
        //$('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
        window.location.reload();
    },

    PayBill: function (ClientDetailsID, PaymentAmount, Discount, ResetNo, RemarksNo, PaymentFrom) {
        var url = "/Transaction/PayBillByEmployeeOrAdmin/";

        var data = ({ ClientDetailsID: ClientDetailsID, PaymentAmount: PaymentAmount, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo });
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.PayBillSuccess, B_P_Manager.PayBillError);
    },
    PayBillSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess(" Payment Successfully done.");
            B_P_Manager.ClearPaymentDetails();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment UnSuccessfull.");
            }
        }
        $("#popModalForPayment").modal("hide");
    },
    PayBillError: function (data) {

        $("#popModalForPayment").modal("hide");
        alert("Fail");
        console.log(data);
        //$('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
        window.location.reload();
    },

    PayBillSpecificMonth: function () {
        var url = "/Transaction/PayBillByEmployeeOrAdminSpecificMonth/";
        var tid = T_DuePayment_ID;
        var PaidAmount = $("#txtPay").val();
        var Discount = $("#txtNewDiscount").val();
        var ResetNo = $("#txtMoneyResetNo").val();
        var RemarksNo = $("#txtRemarks").val();
        var PaymentBy = $("#PaymentFrom").val();
        var BillCollectBy = $("#EmployeeID").val();
        var AnotherMobileNo = $("#txtAnotherMobileNo").val();
        var PaymentFromWhichPage = "Submit Bill By Me Specific Month";
        var data = ({ ClientDetailsID: ClientDetailsID, TransactionID: tid, PaidAmount: PaidAmount, BillCollectBy: BillCollectBy, Discount: Discount, ResetNo: ResetNo, RemarksNo: RemarksNo, PaymentBy: PaymentBy, AnotherMobileNo: AnotherMobileNo, PaymentFromWhichPage: PaymentFromWhichPage });
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.PayBillSpecificMonthSuccess, B_P_Manager.PayBillSpecificMonthError);
    },
    PayBillSpecificMonthSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess(" Payment Successfully done.");
            B_P_Manager.ShowDueBillDetailssByClientDetailsIDSpecificMonth(ClientDetailsID)
            B_P_Manager.ClearPaymentDetails();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } if (data.DiscounGreterThanZero === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment UnSuccessfull.");
            }
        }
        $("#mdlMakePayment").modal("hide");
    },
    PayBillSpecificMonthError: function (data) {
        $("#popModalForPayment").modal("hide");
        alert("Fail");
        console.log(data);
        //$('input:radio[name=chkPayDueBills]').each(function () { $(this).prop('checked', false); });
        window.location.reload();
    },


    CollectedBillAccept: function () {
        var url = "/Transaction/AcceptBill";


        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var data = {
            acceptFor: _CollectBy, fromDate: _StartDate, toDate: _EndDate
        };//JSON.stringify({ Asset_Client: Asset });
        data = B_P_Manager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, B_P_Manager.CollectedBillAcceptSuccess, B_P_Manager.CollectedBillAcceptFail);
    },
    CollectedBillAcceptSuccess: function (data) {
        console.log(data);
        if (data.success === true) {
            AppUtil.ShowSuccess(data.message);
        }
        if (data.success === false) {
            AppUtil.ShowSuccess(data.message);
        }
        _CollectBy = "";
        _StartDate = "";
        _EndDate = "";
        table.draw();
        $("#popModalForAcceptPermently").modal('hide');

    },
    CollectedBillAcceptFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
        _CollectBy = "";
        _StartDate = "";
        _EndDate = "";
        table.draw();
        $("#popModalForAcceptPermently").modal('hide');
    }
}