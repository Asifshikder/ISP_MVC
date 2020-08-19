 

var UnpaidBillsManager = {

    SMSSendValidation: function () {


        if (AppUtil.GetIdValue("txtSMSText") === '') {
            AppUtil.ShowSuccess("Please Add message.");
            return false;
        }

        if ($("input[name='chkSendSMSForAll']").prop("checked") == true) {
            return true;
        }

        else {
            if (ifNotCheckAllThenCheckList.length > 0) {
                return true;
            }
            else {
                AppUtil.ShowError("Please Select client for send message.");
                return false;
            }
        }

        return true;

    },

    PayMultipleBillsValidations: function () {
        if ($("input[name='chkPaymentForAll']").prop("checked") == true) {
            return true;
        }

        else {
            if (ifNotCheckAllThenCheckList.length > 0) {
                return true;
            }
            else {
                AppUtil.ShowError("Please Select client for Bill Payment.");
                return false;
            }
        }

        return true;

    },

    PayMultipleBills: function (isCheckAll, ifIsCheckAllThenNonCheckList, ifNotCheckAllThenCheckList) {

        var url = "/Transaction/PayMultipleBill/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ IsCheckAll: isCheckAll, IfIsCheckAllThenNonCheckList: ifIsCheckAllThenNonCheckList, IfNotCheckAllThenCheckList: ifNotCheckAllThenCheckList, __RequestVerificationToken: AntiForgeryToken, ForWhichTypeSMSIsThat: 1 });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.PayMultipleBillsSuccess, UnpaidBillsManager.PayMultipleBillsError);
    },
    PayMultipleBillsSuccess: function (data) {

        if (data.payMultipleBillSuccess == true) {
            AppUtil.ShowSuccess("Payment Done Successfully.");
        }
        if (data.payMultipleBillFail == true) {
            AppUtil.ShowError("Some Payment Can Not Done. Please Try Again Or Contact With Administrator.");
        }
        isCheckAll = false;
        ifNotCheckAllThenCheckList = [];
        ifIsCheckAllThenNonCheckList = [];

        $("input:checkbox").prop("checked", false);
        $("#SMSDiv").hide();
        $("#popModalForPayMultipleTS").modal("hide");

        table.draw();
        //AppUtil.ShowSuccess("Success");

    },
    PayMultipleBillsError: function (data) {

        AppUtil.ShowError("Error Occourd. Contact with administrator.");
        console.log(data);

        $("#popModalForPayMultipleTS").modal("hide");
    },

    SendMessageToClient: function (isCheckAll, ifIsCheckAllThenNonCheckList, ifNotCheckAllThenCheckList) {


        var sms = $("#txtSMSText").val();
        var url = "/sms/SendSMSToClient/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ message: sms, IsCheckAll: isCheckAll, IfIsCheckAllThenNonCheckList: ifIsCheckAllThenNonCheckList, IfNotCheckAllThenCheckList: ifNotCheckAllThenCheckList, __RequestVerificationToken: AntiForgeryToken, ForWhichTypeSMSIsThat: 2 });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.SendMessageToClientSuccess, UnpaidBillsManager.SendMessageToClientError);
    },
    SendMessageToClientSuccess: function (data) {

        var smsFailedList = "";
        if (data.sendSMSSuccess == true) {
            if (data.lstMobileUnableToSendSMS.length > 0) {
                $.each(data.lstMobileUnableToSendSMS, function (index, item) {
                    smsFailedList += item + ",";
                });
                AppUtil.ShowError("Unable to Send messsage to these number." + smsFailedList);
            }
            else {
                AppUtil.ShowSuccess("SMS Send Successfully.");
            }
            //$("input:checkbox").prop("checked", false);

        }
        if (data.sendSMSFail == true) {
            AppUtil.ShowError("Sorry Error Occourd. Contact with administrator.");
        }
        isCheckAll = false;
        ifNotCheckAllThenCheckList = [];
        ifIsCheckAllThenNonCheckList = [];
        $("input:checkbox").prop("checked", false);
        $("#txtSMSText").val("");
        $("#SMSDiv").hide();
        //AppUtil.ShowSuccess("Success");

    },
    SendMessageToClientError: function (data) {

        AppUtil.ShowError("Error Occourd. Contact with administrator.");
        console.log(data);
    },

    PayMonthlyBillValidation: function () {


        //if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        if (AppUtil.GetIdValue("txtRegularBillPay") === '') {
            AppUtil.ShowSuccess("Please Pay Amount. ");
            return false;
        }
        if (AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
            return false;
        }


        return true;
    },
    PayResellerMonthlyBillValidation: function () {


        //if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        //if (AppUtil.GetIdValue("txtRegularBillPay") === '') {
        //    AppUtil.ShowSuccess("Please Pay Amount. ");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("EmployeeID") === '') {
        //    AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
        //    return false;
        //}

        if (AppUtil.GetIdValue("txtRegularBillPay") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }

        return true;
    },
    PayDueBillValidation: function () {
        if (AppUtil.GetIdValue("txtDuePay") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }
        if (AppUtil.GetIdValue("DueEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
            return false;
        }
        //if (AppUtil.GetIdValue("txtDueMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        return true;
    },
    PayResellerDueBillValidation: function () {

        if (AppUtil.GetIdValue("txtDuePay") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }

        return true;
    },
    ValidationForSignUpBillsSearch: function () {


        if (byAdmin) {
            if (AppUtil.GetIdValue("ResellerID") == '') {
                AppUtil.ShowSuccess("Please Select Reseller. ");
                return false;
            }
        }

        if (AppUtil.GetIdValue("ResellerID") == '' && AppUtil.GetIdValue("YearID") == '' && AppUtil.GetIdValue("MonthID") == '' && AppUtil.GetIdValue("SearchByZoneID") == '') {
            AppUtil.ShowSuccess("Please Select Zone or Year. ");
            return false;
        }

        if (AppUtil.GetIdValue("MonthID") !== '') {
            if (AppUtil.GetIdValue("YearID") === '') {
                AppUtil.ShowSuccess("Please Select Year. ");
                return false;
            }
        }

        return true;
    },
    ValidationForUnPaidBillsSearch: function (byAdmin) {


        if (byAdmin) {
            if (AppUtil.GetIdValue("ResellerID") == '') {
                AppUtil.ShowSuccess("Please Select Reseller. ");
                return false;
            }
        }

        if (AppUtil.GetIdValue("ResellerID") == '' && AppUtil.GetIdValue("YearID") == '' && AppUtil.GetIdValue("MonthID") == '' && AppUtil.GetIdValue("SearchByZoneID") == '') {
            AppUtil.ShowSuccess("Please Select Zone or Year. ");
            return false;
        }

        if (AppUtil.GetIdValue("MonthID") !== '') {
            if (AppUtil.GetIdValue("YearID") === '') {
                AppUtil.ShowSuccess("Please Select Year. ");
                return false;
            }
        }
         
        return true;
    },


    ValidationForPayMultiplePaymentSearch: function () {
        if (AppUtil.GetIdValue("YearID") === '' && AppUtil.GetIdValue("MonthID") === '' && AppUtil.GetIdValue("ZoneID") === '') {
            AppUtil.ShowSuccess("Please Select Some Search Criteria. ");
            return false;
        }
        if (AppUtil.GetIdValue("MonthID") !== '') {
            if (AppUtil.GetIdValue("YearID") === '') {
                AppUtil.ShowSuccess("Please Select Year. ");
                return false;
            }
        }

        return true;
    },

    PrintDueBillsList: function () {

        var url = "/Excel/CreateReportForDueBills";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var YearID = AppUtil.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("ZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({ Year: YearID, Month: MonthID, ZoneID: ZoneID });
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.PrintDueBillsListSuccess, UnpaidBillsManager.PrintDueBillsListFail);
    },
    PrintDueBillsListSuccess: function (data) {

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
    PrintDueBillsListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    PrintSignUpBillsList: function () {

        var url = "/Excel/CreateReportForSignUpBills";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var YearID = AppUtil.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("ZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({ Year: YearID, Month: MonthID, ZoneID: ZoneID });
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.PrintSignUpBillsListSuccess, UnpaidBillsManager.PrintSignUpBillsListFail);
    },
    PrintSignUpBillsListSuccess: function (data) {

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
    PrintSignUpBillsListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    GetClientPaymentAmountAndRemarksAndSleepNoForPayment: function (id) { 
        var url = "/Transaction/GetRemarksAndSleepNoForPayment/";
        var data = { TransactionID: id };
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.GetClientPaymentAmountAndRemarksAndSleepNoForPaymentSuccess, UnpaidBillsManager.GetClientPaymentAmountAndRemarksAndSleepNoForPaymentError);
         
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForPaymentSuccess: function (data) {
         
        if (data.Success === true) {
            var Transaction = (data.Transaction);

            //$("#txtTotalAmount").val(Transaction.PaymentAmount);
            //$("#txtRegularBillPaidAmount").val(Transaction.PaidAmount);
            //$("#txtAmount").val(Transaction.DueAmount);
            //$("#txtRegularDiscount").val(Transaction.DiscountAmount);
            //$("#txtMoneyResetNo").val(/*Transaction.SerialNo*/);
            //$("#txtRemarks").val(data.RemarksNo);
            //$("#txtPermanentDiscount").val(Transaction.PermanentDiscount);


            $("#txtMonthName").val(UnpaidBillsManager.GetPaymentMonthName(Transaction.MonthName));
            $("#txtSpecificLoginName").val(Transaction.LoginName);
            $("#txtUserID").val(Transaction.UserID);
            $("#txtTotalAmount").val(Transaction.PaymentAmount);
            $("#txtPaidAmount").val(Transaction.PaidAmount);
            $("#txtAmount").val(Transaction.DueAmount);
            $("#txtDiscount").val(Transaction.DiscountAmount);
            $("#txtMoneyResetNo").val("");
            $("#txtRemarks").val(data.RemarksNo);
            $("#txtPermanentDiscount").val(Transaction.PermanentDiscount);
        }
         
        $("#mdlMakePayment").modal("show");
         
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForPaymentError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    GetClientPaymentAmountAndRemarksAndSleepNoForDuePayment: function (id) {
        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';

        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        var url = "/Transaction/GetRemarksAndSleepNoForPayment/";
        var data = { TransactionID: id };
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess, UnpaidBillsManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError);

        // }, 500);

    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {
            var Transaction = (data.Transaction);

            $("#txtDueTotalAmount").val(Transaction.PaymentAmount);
            $("#txtPaidAmount").val(Transaction.PaidAmount);
            $("#txtDueAmount").val(Transaction.DueAmount);
            $("#txtDueDiscount").val(Transaction.DiscountAmount);
            $("#txtDueMoneyResetNo").val(/*Transaction.SerialNo*/);
            $("#txtDueRemarks").val(data.RemarksNo);
            $("#txtDuePermanentDiscount").val(Transaction.PermanentDiscount);
        }


        $("#mdlMakeDuePayment").modal("show");

        //$("#mdlPackageUpdate").modal("show");
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    GenerateBillForThisMonth: function () {
        var url = "/Transaction/GenerateBillForThisMonth/";
        var data = {};
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.GenerateBillForThisMonthSuccess, UnpaidBillsManager.GenerateBillForThisMonthFail);
    },
    GenerateBillForThisMonthSuccess: function (data) {

        console.log(data);
        if (data.BillAlreadyGenerate === true) {
            AppUtil.ShowError("Sorry Bill Already generated for this month.");
        }
        if (data.Success === true) {
            if (data.Count > 0) {
                AppUtil.ShowSuccess('Total ' + data.Count + ' bills generated. ');
                window.location.href = "/Transaction/Accounts";
            }
            if (data.Count === 0) {
                AppUtil.ShowSuccess('Total 0 bill generate. ');
            }
        }
        if (data.Success === false) {
            AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ')
        }

    },
    GenerateBillForThisMonthFail: function (data) {

        console.log(data);
        AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ')
    },

    PayMonthlyBill: function (TransactionID, FromWhere) {
        var discount = $("#txtRegularDiscount").val();
        var resetNo = $("#txtMoneyResetNo").val();
        var remarksNo = $("#txtRemarks").val();
        var collectBy = $("#EmployeeID").val();
        var PaidAmount = $("#txtRegularBillPay").val();
        var url = "/Transaction/PayMonthlyBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, UnpaidBillsManager.PayMonthlyBillSuccess, UnpaidBillsManager.PayMonthlyBillError);
    },
    PayMonthlyBillSuccess: function (data) {
        //
        //AppUtil.ShowSuccess("Payment Successfully done.");
        //console.log(data);
        //window.location.href = "/Transaction/Accounts";

        if (data.Success === true) {
            AppUtil.ShowSuccess("Payment Successfully done.");
            //window.location.href = "/Transaction/Accounts";
            UnpaidBillsManager.ClearPaymentModalDuringPaymentFromUnpaidPage();
            $("#mdlMakePayment").modal('hide');
            table.draw();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment  Not done.");
            }
        }
    },
    PayMonthlyBillError: function (data) { 
        alert("Fail");
        console.log(data);
    },


    PayMonthlyBillForReseller: function (TransactionID, FromWhere) {
        var discount = $("#txtRegularDiscount").val();
        var resetNo = $("#txtMoneyResetNo").val();
        var remarksNo = $("#txtRemarks").val();
        var collectBy = $("#EmployeeID").val();
        var PaidAmount = $("#txtRegularBillPay").val();
        var url = "/Transaction/PayRSClientMonthlyBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, UnpaidBillsManager.PayMonthlyBillForResellerSuccess, UnpaidBillsManager.PayMonthlyBillForResellerError);
    },
    PayMonthlyBillForResellerSuccess: function (data) {

        if (data.Success === true) {
            AppUtil.ShowSuccess("Payment Successfully done.");
            //window.location.href = "/Transaction/Accounts";
            UnpaidBillsManager.ClearPaymentModalDuringPaymentFromUnpaidPage();
            $("#mdlMakePayment").modal('hide');
            table.draw();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Payment  Not done.");
            }
        }

        //console.log(data);
        //if (data.AddAdvancePaymentSuccess === true) {
        //    AppUtil.ShowSuccess("Payment Successfully done.");
        //    window.location.href = "/Transaction/Accounts";
        //}
        //if (data.AddAdvancePaymentSuccess === false) {
        //    if (data.ResetNoAlreadyExist === true) {
        //        AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
        //    } else {
        //        AppUtil.ShowError("Payment  Not done.");
        //    }
        //}

        //console.log(data);

    },
    PayMonthlyBillForResellerError: function (data) {


        alert("Fail");
        console.log(data);
    },

    PayDueBill: function (TransactionID, FromWhere) {
        var discount = $("#txtDueDiscount").val();
        var resetNo = $("#txtDueMoneyResetNo").val();
        var remarksNo = $("#txtDueRemarks").val();
        var collectBy = $("#DueEmployeeID").val();
        var PaidAmount = $("#txtDuePay").val();
        var url = "/Transaction/PayDueBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, UnpaidBillsManager.PayDueBillSuccess, UnpaidBillsManager.PayDueBillError);
    },
    PayDueBillSuccess: function (data) {

        if (data.Success === true) {
            var ts = data.duets;
            AppUtil.ShowSuccess("Due Payment Successfully done.");
            UnpaidBillsManager.ClearDuePaymentModalDuringPaymentFromUnpaidPage();
            $("#mdlMakeDuePayment").modal('hide');
            $("#tblShowDueBillsList>tbody>tr").each(function () {
                var rowIndex = $(this).index();
                var index = $(this).index();
                var T_DueID = $(this).find("td:eq(1) input").val();
                if (T_DueID == ts[0].Value) {
                    $('#tblShowDueBillsList tbody>tr:eq(' + index + ')').find("td:eq(5)").text(ts[1].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(ts[2].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(ts[3].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(ts[4].Value);
                }
            });
            table.draw();

        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Due Payment Not done.");
            }
        }
    },
    PayDueBillError: function () {


        alert("Fail");
        console.log(data);
    },


    PayDueBillForReseller: function (TransactionID, FromWhere) {
        var discount = $("#txtDueDiscount").val();
        var resetNo = $("#txtDueMoneyResetNo").val();
        var remarksNo = $("#txtDueRemarks").val();
        var collectBy = $("#DueEmployeeID").val();
        var PaidAmount = $("#txtDuePay").val();
        var url = "/Transaction/PayResellerDueBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, UnpaidBillsManager.PayDueBillForResellerSuccess, UnpaidBillsManager.PayDueBillForResellerError);
    },
    PayDueBillForResellerSuccess: function (data) {
        if (data.Success === true) {
            var ts = data.duets;
            AppUtil.ShowSuccess("Due Payment Successfully Paid.");
            UnpaidBillsManager.ClearDuePaymentModalDuringPaymentFromUnpaidPage();
            $("#mdlMakeDuePayment").modal('hide');
            $("#tblShowDueBillsList>tbody>tr").each(function () {
                var rowIndex = $(this).index();
                var index = $(this).index();
                var T_DueID = $(this).find("td:eq(1) input").val();
                if (T_DueID == ts[0].Value) {
                    $('#tblShowDueBillsList tbody>tr:eq(' + index + ')').find("td:eq(5)").text(ts[1].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(ts[2].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(ts[3].Value);
                    $('#tblShowDueBillsList>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(ts[4].Value);
                }
            });
            table.draw();
        }
        if (data.Success === false) {
            if (data.ResetNoAlreadyExist === true) {
                AppUtil.ShowError("Sorry ResetNo Already Exist. Choose Another one.");
            } else {
                AppUtil.ShowError("Due Payment Not done.");
            }
        }
    },
    PayDueBillForResellerError: function (data) { 
        alert("Fail");
        console.log(data);
    },

    ClearPaymentModalDuringPaymentFromUnpaidPage: function () {
        $("#txtTotalAmount").val("");
        $("#txtRegularBillPaidAmount").val("");
        $("#txtAmount").val("");
        $("#txtRegularBillPay").val("");
        $("#txtRegularDiscount").val(0);
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
        $("#txtPermanentDiscount").val("");
    },

    ClearDuePaymentModalDuringPaymentFromUnpaidPage: function () {
        $("#txtDueTotalAmount").val("");
        $("#txtPaidAmount").val("");
        $("#txtDueAmount").val("");
        $("#txtDuePay").val("");
        $("#txtDueDiscount").val(0);
        $("#DueEmployeeID").prop("selectedIndex", 0);
        $("#txtDueMoneyResetNo").val("");
        $("#txtDueRemarks").val("");
        $("#txtDuePermanentDiscount").val("");
    },
    //AdjustDueBill: function () {

    //    var url = "/Transaction/AdjustDueBill/";
    //    var data = {};
    //    data = UnpaidBillsManager.addRequestVerificationToken(data);
    //    AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.AdjustDueBillSuccess, UnpaidBillsManager.AdjustDueBillError);
    //},
    //AdjustDueBillSuccess: function (data) {
    //    console.log(data);
    //    if (data.DueBillAlreadyGenerate === true) {
    //        AppUtil.ShowError("Sorry Due Bill Already generated .");
    //    }
    //    if (data.Success === true) {
    //        if (data.NoDueBill > 0) {
    //            AppUtil.ShowSuccess('Total ' + data.NoDueBill + ' Due bills generated. ');
    //            window.location.href = "/Transaction/Accounts";
    //        }
    //        if (data.NoDueBill === 0) {
    //            AppUtil.ShowSuccess('Total 0 bill generate. ');
    //        }
    //    }
    //    if (data.Success === false) {
    //        AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ')
    //    }
    //},
    //AdjustDueBillError: function () {

    //},

    ShowDueBillDetailssByTransactionID: function (transactionID) {
        var url = "/Transaction/ShowDueBillDetailssByTransactionID/";
        var data = { TransactionID: transactionID };
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.ShowDueBillDetailssByTransactionIDSuccess, UnpaidBillsManager.ShowDueBillDetailssByTransactionIDFail);
    },
    ShowDueBillDetailssByTransactionIDSuccess: function (data) {

        console.log(data);
        $("#tblShowDueBillsList>tbody").empty();
        $.each(data.DueTransactionList, function (index, item) {

            $("#tblShowDueBillsList>tbody").append('<tr><td><input type="radio" name="chkPayDueBills" /></td><td><input type="hidden" id="hdnTransactionID" value=' + item.TransactionID + '></td><td>' + UnpaidBillsManager.MonthNames(item.MonthName) + '</td><td>' + item.PackageName + '</td><td>' + item.PackagePrice + '</td><td>' + item.PaidAmount + '</td><td>' + item.Discount + '</td><td>' + item.Due + '</td><td>' + item.Status + '</td></tr>');
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

    GetDueBillsListBySearchCriteria: function (byAdmin) {
        //AppUtil.ShowWaitingDialog();
        var url = "";
        var YearID = $("#YearID option:selected").text();//.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("SearchByZoneID");
        var ResellerID = AppUtil.GetIdValue("ResellerID");

        if (byAdmin) {
            url = "/Transaction/GetDueBillsListBySearchCriteriaByAdmin/";
        }
        else {
            url = "/Transaction/GetDueBillsListBySearchCriteria/";
        }
        var data = ({ YearID: YearID, MonthID: MonthID, ZoneID: ZoneID, ResellerID: ResellerID });
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.GetDueBillsListBySearchCriteriaSuccess, UnpaidBillsManager.GetDueBillsListBySearchCriteriaFail);
    },
    GetDueBillsListBySearchCriteriaSuccess: function (data) {

        if (data.Date) {
            $("#dateArchiveBills").html(data.Date);
            $("#dateBillSummary").html(data.Date);
        }
        //AppUtil.HideWaitingDialog();
        console.log(data.lstTransaction);
        //AppUtil.ShowSuccess("S");
        if (data.Success === true) {
            searchBySearchButton = 1;
            //$('#tblClientMonthlyBill').dataTable().fnDestroy();
            //$("#tblClientMonthlyBill>tbody").empty();
            // console.log(data.lstTransaction.length);
            // if (data.lstTransaction.length > 1) {
            //$.each(data.lstTransaction, function (index, items) {
            //    
            //    $.each(items, function (index, item) {

            //        
            //        var checkBox = "";
            //        if (item.PaymentStatus === 0) {
            //            checkBox = '<input type="checkbox" id="TransactionID" onclick = "setCompleteStatus(' + item.TransactionID + ')" >';
            //        }

            //        var PaidAmount = (item.PaidAmount === null ? "" : item.PaidAmount);
            //        // var DueAmount = (item.DueAmount === 0 ? "" : item.DueAmount);

            //        var dueAmountLink = "";
            //        var x = item.DueAmount;
            //        if (x > 0) {
            //            if (item.DueBillStatus === true) {
            //                dueAmountLink = '<a href="" id="linkOfShowingDueAmount">' + item.DueAmount + '</a>';
            //            }
            //            if (item.DueBillStatus === false) {
            //                dueAmountLink = '0';
            //            }

            //        }
            //        if (x === 0) {
            //            dueAmountLink = '0';
            //        }

            //        var PaidBy = (item.PaidBy === null ? "" : item.PaidBy);
            //        var CollectBy = (item.CollectBy === null ? "" : item.CollectBy);
            //        var PaidTime = (item.PaidTime === null ? "" : AppUtil.ParseDate(item.PaidTime));
            //        var RemarksNo = (item.RemarksNo === null ? "" : item.RemarksNo);
            //        var ReceiptNo = (item.ReceiptNo === null ? "" : item.ReceiptNo);

            //        var link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
            //        $("#tblClientMonthlyBill>tbody").append('<tr><td>' + checkBox + '</td><td hidden><input type="hidden" id="TransactionID" value=' + item.TransactionID + '></td><td>' + link + '</td>\
            //                                                 <td>' + item.Address + '</td><td>' + item.Mobile + '</td><td>' + item.Zone + '</td>\
            //                                                 <td>' + item.Package + '</td><td>' + item.MonthlyFee + '</td><td>' + PaidAmount + '</td><td style="color:blue"> ' + dueAmountLink + ' </td>\
            //                                                 <td>' + PaidBy + '</td><td>' + CollectBy + '</td><td>' + PaidTime + '</td><td>' + RemarksNo + '</td>\
            //                                                 <td>' + ReceiptNo + '</td> <td align="center" style="padding: 8px 3px;"><div style="float:left"><button type="button" id="btnPrint" class="btn btn-success  btn-sm"  title="Not Paid"><span class="glyphicon glyphicon-print"></span></button></div><div style="float:right"><button type="button" id="" class="btn btn-danger btn-sm"><span class="glyphicon glyphicon-remove"></span></button></div></td> </tr>');
            //    });
            //});

            UnpaidBillsManager.MakeEmptyBillSummary();

            var billSummaryDetails = data.billSummaryDetails;
            $.each(billSummaryDetails, function (index, item) {

                $('#' + item.Key + '').text(item.Value);
            });


            //var mytable = $('#tblClientMonthlyBill').DataTable({
            //    "paging": true,
            //    "lengthChange": false,
            //    "searching": true,
            //    "ordering": true,
            //    "info": true,
            //    "autoWidth": true,
            //    "sDom": 'lfrtip'
            //});
            //mytable.draw();


            // }
            //else {
            //    $.each(data.lstTransaction, function (index, item) {
            //        

            //        var checkBox = "";
            //        if (item.PaymentStatus === 0) {
            //            checkBox = '<input type="checkbox" id="TransactionID" onclick = "setCompleteStatus(' + item.TransactionID + ')" >';
            //        }

            //        var PaidAmount = (item.PaidAmount === null ? "" : item.PaidAmount);
            //        var DueAmount = (item.DueAmount === null ? "" : item.DueAmount);
            //        var PaidBy = (item.PaidBy === null ? "" : item.PaidBy);
            //        var CollectBy = (item.CollectBy === null ? "" : item.CollectBy);
            //        var PaidTime = (item.PaidTime === null ? "" : item.PaidTime);
            //        var RemarksNo = (item.RemarksNo === null ? "" : item.RemarksNo);
            //        var ReceiptNo = (item.ReceiptNo === null ? "" : item.ReceiptNo);

            //        $("#tblClientMonthlyBill>tbody").append('<tr><td>' + checkBox + '</td><td style="padding:0px"><input type="hidden" id="TransactionID" value=' + item.TransactionID + '></td><td>' + item.LoginName + '</td>\
            //                                                 <td>' + item.Address + '</td><td>' + item.Mobile + '</td><td>' + item.Zone + '</td>\
            //                                                 <td>' + item.Package + '</td><td>' + item.MonthlyFee + '</td><td>' + PaidAmount + '</td><td><a href="" id="linkOfShowingDueAmount">' + DueAmount + '</a></td>\
            //                                                 <td>' + PaidBy + '</td><td>' + CollectBy + '</td><td>' + PaidTime + '</td><td>' + RemarksNo + '</td>\
            //                                                 <td>' + ReceiptNo + '</td><td>' + ' <div style="width: 30%; float: left"><button type="button" id="btnEdit" class="btn btn-success btn-block" style="width: 40px;"><span class="glyphicon glyphicon-ok"></span></button></div><div style="width: 30%; float: left"><button type="button" id="btnDelete" class="btn btn-danger btn-block" style="width: 40px;"><span class="glyphicon glyphicon-remove"></span></button></div>' + '</td> </tr>');

            //    });
            // }

        }

    },
    GetDueBillsListBySearchCriteriaFail: function (data) {

        console.log(data);
        AppUtil.ShowSuccess("F");

    },

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


    MakeEmptyBillSummary: function () {
        $("#clnPayableAmount").text('');
        $("#clnCollectedAmount").text('');
        $("#clnDiscountAmount").text('');
        $("#clnCollectedAmountBIll").text('');
        $("#clnOnlinePayment").text('');
        $("#clnInstallationAmount").text('');
        $("#clnDueAmount").text('');
        $("#clnTotalExpense").text('');
        $("#clnRestOfAmount").text('');
        $("#clnTotalClient").text('');
        $("#clnPaidClient").text('');
        $("#clnUnpaidClient").text('');
        $("#clnPreviousBillCollection").text('');
    },

    PayRunningMonthBillFromAdvanceAmount: function (FromWhere) {
        var url = "/Transaction/PayRunningMonthBillFromAdvanceAmount/";
        var data = ({ PaymentFromWhichPage: FromWhere });
        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.PayRunningMonthBillFromAdvanceAmountSuccess, UnpaidBillsManager.PayRunningMonthBillFromAdvanceAmountError);
    },
    PayRunningMonthBillFromAdvanceAmountSuccess: function (data) {

        if (data.Success === true) {

            if (parseInt(data.AdvancePaymentCount) > 0) {
                AppUtil.ShowSuccess('Total ' + data.AdvancePaymentCount + ' advance payment generated.');
                window.location.href = "/Transaction/UnpaidBills/";
            }
            if (parseInt(data.AdvancePaymentCount) === 0) {
                AppUtil.ShowError('Total ' + data.AdvancePaymentCount + ' advance payment generated.');
            }
        }
        if (data.Success === false) {
            AppUtil.ShowError('Something is wrong contact with administrator.');
            window.location.href = "/Transaction/UnpaidBills/";
        }
    },
    PayRunningMonthBillFromAdvanceAmountError: function (data) {

        console.log(data);

    },

    SearchSignUpBillBySearchCriteria: function () {


        //AppUtil.ShowWaitingDialog();
        var url = "/Transaction/SearchSignUpBillBySearchCriteria";
        var YearID = $("#YearID").val();
        var MonthID = $("#MonthID").val();
        var ZoneID = $("#ZoneID").val();
        var data = ({ YearID: YearID, MonthID: MonthID, ZoneID: ZoneID });

        data = UnpaidBillsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, UnpaidBillsManager.SearchSignUpBillBySearchCriteriaSuccess, UnpaidBillsManager.SearchSignUpBillBySearchCriteriaError);

    },
    SearchSignUpBillBySearchCriteriaSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {

            if (data.TransactionsCount >= 0) {
                AppUtil.ShowSuccess("Total " + data.TransactionsCount + " Information Found.");
                $("#tblClientMonthlyBill>tbody").empty();

                //public string Name { get; set; }
                //public string Address { get; set; }
                //public string ContactNumber { get; set; }
                //public string ZoneName { get; set; }
                //public string PackageName { get; set; }
                //public string PackagePrice { get; set; }
                //public string SignUpFee { get; set; }
                //public DateTime PaymentDate { get; set; }
                //public string RemarksNo { get; set; }

                $.each(data.TransactionsList, function (index, item) {

                    var LineStatus = "";
                    var actionOption = "";

                    var link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
                    $("#tblClientMonthlyBill>tbody").append("<tr role='row' class='odd'><td hidden></td><td hidden><input type='hidden' value=" + item.TransactionID + "></td>\
                     <td>" + link + "</td><td>" + item.Address + "</td><td>" + item.ContactNumber + "</td><td>" + item.ZoneName + "</td>\
                     <td>" + item.PackageName + "</td><td>" + item.PackagePrice + "</td><td>" + item.SignUpFee + "</td><td>" + AppUtil.ParseDateTime(item.PaymentDate) + "</td><td>" + item.RemarksNo + "</td>\
                       <td><div style=''>  </div> </td>\
                    </tr>");//<button type='button' id='btnEdit' class='btn btn-success btn-block btn-sm' ><span class='glyphicon glyphicon-ok'></span></button> </div>  <div style=''>  <button type='button' id='btnDelete' class='btn btn-danger btn-block btn-sm' style='width: 40px;'><span class='glyphicon glyphicon-remove'></span></button>


                });
            }
            else {
                AppUtil.ShowError("Sorry No Information Found.");
            }
        }

    },
    SearchSignUpBillBySearchCriteriaError: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
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
    }
    ,
}