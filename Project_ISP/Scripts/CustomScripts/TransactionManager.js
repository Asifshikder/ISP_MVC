  var TransactionManager = {

    PayMonthlyBillValidation: function () {


        //if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        if (AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
            return false;
        }

        return true;
    },
    PayResellerMonthlyBillValidation: function () {


        if (AppUtil.GetIdValue("txtRegularBillPay") === '') {
            AppUtil.ShowSuccess("Please Add Amount. ");
            return false;
        }
        //if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
        //    AppUtil.ShowSuccess("Please Add Money Reset No. ");
        //    return false;
        //}

        //if (AppUtil.GetIdValue("EmployeeID") === '') {
        //    AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
        //    return false;
        //}

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

    ValidationForFilterBillsSearch: function () {


        if (AppUtil.GetIdValue("YearID") === '' && AppUtil.GetIdValue("MonthID") === '' && AppUtil.GetIdValue("EmployeeID") === '') {
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
    PrintAchiveBillIndividually: function () {

        var url = "/Excel/CreateReportForArchiveBills";

        var YearID = AppUtil.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("ZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({ Year: YearID, Month: MonthID, ZoneID: ZoneID });
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.PrintArchiveBillsListSuccess, TransactionManager.PrintArchiveBillsListFail);

    },
    PrintArchiveBillsListSuccess: function (data) {

        console.log(data);
        var response = (data);
        window.location = '/Excel/Download?fileGuid=' + response.FileGuid
            + '&filename=' + response.FileName;
    },
    PrintArchiveBillsListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
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
        AppUtil.MakeAjaxCall(url, "POST", data, TransactionManager.PayMonthlyBillForResellerSuccess, TransactionManager.PayMonthlyBillForResellerError);
    },
    PayMonthlyBillForResellerSuccess: function (data) {

        if (data.Success === true) {
            AppUtil.ShowSuccess("Payment Successfully done.");
            //window.location.href = "/Transaction/Accounts";
            TransactionManager.ClearPaymentModalDuringPaymentFromAccoutsPage();
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

    ClearPaymentModalDuringPaymentFromAccoutsPage: function () {
        $("#txtTotalAmount").val("");
        $("#txtRegularBillPaidAmount").val("");
        $("#txtAmount").val("");
        $("#txtRegularBillPay").val("");
        $("#txtRegularDiscount").val(0);
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#txtMoneyResetNo").val("");
        $("#txtRemarks").val("");
    },
    ClearDuePaymentModalDuringPaymentFromAccoutsPage: function () {
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
     
    PayDueBillForReseller: function (TransactionID, FromWhere) {
        var discount = $("#txtDueDiscount").val();
        var resetNo = $("#txtDueMoneyResetNo").val();
        var remarksNo = $("#txtDueRemarks").val();
        var collectBy = $("#DueEmployeeID").val();
        var PaidAmount = $("#txtDuePay").val();
        var url = "/Transaction/PayResellerDueBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, TransactionManager.PayDueBillForResellerSuccess, TransactionManager.PayDueBillForResellerError);
    },
    PayDueBillForResellerSuccess: function (data) {
        if (data.Success === true) {
            var ts = data.duets;
            AppUtil.ShowSuccess("Due Payment Successfully Paid.");
            TransactionManager.ClearDuePaymentModalDuringPaymentFromAccoutsPage();
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

    //AdjustDueBill: function () {

    //    var url = "/Transaction/AdjustDueBill/";
    //    var data = {};
    //    data = TransactionManager.addRequestVerificationToken(data);
    //    AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.AdjustDueBillSuccess, TransactionManager.AdjustDueBillError);
    //},
    //AdjustDueBillSuccess: function (data) {

    //    console.log(data);

    //    AppUtil.ShowSuccess("S");
    //    if (data.DueBillAlreadyGenerate === true) {
    //        AppUtil.ShowError("Sorry Due Bill Already generated .");
    //    }
    //    else if (data.Success === true) {
    //        if (data.GenerateBillEmpty === true) {
    //            AppUtil.ShowSuccess('Please Generate Bill First.');

    //        }
    //        if (data.NoDueBillFound === true) {
    //            AppUtil.ShowSuccess('No Due Bill Found.');
    //        }
    //        if (data.NoDueBill > 0) {
    //            AppUtil.ShowSuccess('Total ' + data.NoDueBill + ' Due bills generated. ');
    //            window.location.href = "/Transaction/Accounts";
    //        }
    //        if (data.NoDueBill === 0) {
    //            AppUtil.ShowSuccess('Total 0 bill generate. ');
    //        }
    //    }
    //    else if (data.Success === false) {
    //        AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ')
    //    }
    //    else {

    //    }
    //},
    //AdjustDueBillError: function (data) {

    //    console.log(data);
    //    AppUtil.ShowError("F");
    //},

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

    GetFilterBillsListBySearchCriteria: function () {
        ////AppUtil.ShowWaitingDialog();
        var YearID = $("#YearID option:selected").text();//.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var EmployeeID = AppUtil.GetIdValue("EmployeeID");

        var url = "/Transaction/GetFilterBillsListBySearchCriteria/";
        var data = ({ YearID: YearID, MonthID: MonthID, EmployeeID: EmployeeID });
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.GetFilterBillsListBySearchCriteriaSuccess, TransactionManager.GetFilterBillsListBySearchCriteriaFail);
    },
    GetFilterBillsListBySearchCriteriaSuccess: function (data) {

        // //AppUtil.HideWaitingDialog();
        console.log(data.lstTransaction);
        AppUtil.ShowSuccess("Total " + data.TotalCount + " Information Found.");
        if (data.Success === true) {
            $('#tblFilterBill').dataTable().fnDestroy();
            $("#tblFilterBill>tbody").empty();
            console.log(data.lstTransaction.length);
            // if (data.lstTransaction.length > 1) {
            $.each(data.lstTransaction, function (index, item) {


                var typeMonthlyOrConnection = "";
                var link = "";
                if (item.Type === true) {
                    link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
                    typeMonthlyOrConnection = "Monthly";
                }
                else {
                    link = item.Name;
                    typeMonthlyOrConnection = "Connection Fee";
                }


                var setPriority = false;
                var classes;
                if (item.IsPriorityClient) {
                    setPriority = true;
                    classes = 'changetrbackground';
                }

                $("#tblFilterBill>tbody").append('<tr  class="' + classes + '"><td hidden><input type="hidden" value=' + item.ClientDetailsID + '></td><td hidden><input type="hidden" value=' + item.TransactionID + '></td><td>' + link + '</td><td>' + item.LineStatusActiveDate + '</td><td>' + item.Address + '</td><td>' + item.Mobile + '</td><td>' + item.Zone + '</td>\
                                                             <td>' + item.Package + '</td><td>' + item.Year + '</td><td>' + item.Month + '</td><td style="color:blue"> ' + item.Amount + ' </td>\
                                                             <td>' + typeMonthlyOrConnection + '</td><td>' + item.Paid_By + '</td><td>' + AppUtil.ParseDateTime(item.Paid_Time) + '</td><td align="center" style="padding: 4px 15px;"><div style="float:left"><button type="button" id="btnPrint" class="btn btn-success  padding"><span class="glyphicon glyphicon-print"></span></button></div><div style="float:right"><button type="button" id="" class="btn btn-danger padding"><span class="glyphicon glyphicon-remove"></span></button></div></td> </tr>');

            });

        }


        var mytable = $('#tblFilterBill').DataTable({
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": true,
            "sDom": 'lfrtip'
        });
        mytable.draw();

    },
    GetFilterBillsListBySearchCriteriaFail: function (data) {

        console.log(data);
        AppUtil.ShowSuccess("F");

    },




    // Necessary For Accounts Page

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
    ValidationForArchiveBillsSearch: function (byAdmin) {
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

    GetBillsListBySearchCriteria: function (byAdmin) {
        //AppUtil.ShowWaitingDialog();
        var YearID = $("#YearID option:selected").text();//.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("SearchByZoneID");
        var ResellerID = AppUtil.GetIdValue("ResellerID");
        var url = "";
        if (byAdmin) {
            url = "/Transaction/GetBillsListBySearchCriteriaByAdmin/";
        }
        else {
            url = "/Transaction/GetBillsListBySearchCriteria/";
        }
        var data = ({ YearID: YearID, MonthID: MonthID, ZoneID: ZoneID, ResellerID: ResellerID });
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.GetBillsListBySearchCriteriaSuccess, TransactionManager.GetBillsListBySearchCriteriaFail);
    },
    GetBillsListBySearchCriteriaSuccess: function (data) {
        if (data.Date) {
            $("#dateArchiveBills").html(data.Date);
            $("#dateBillSummary").html(data.Date);
        }

        if (data.Success === true) {
            searchBySearchButton = 1;

            TransactionManager.MakeEmptyBillSummary();

            var billSummaryDetails = data.billSummaryDetails;
            $.each(billSummaryDetails, function (index, item) {

                $('#' + item.Key + '').text(item.Value);
            });

        }
        if (data.Success === false) {
            AppUtil.ShowError("F");
        }

    },
    GetBillsListBySearchCriteriaFail: function (data) {
        console.log(data);
        AppUtil.ShowSuccess("F");
    },
     
    GetClientPaymentAmountAndRemarksAndSleepNoForPayment: function (id) {
        var url = "/Transaction/GetRemarksAndSleepNoForPayment/";
        var data = { TransactionID: id };
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.GetClientPaymentAmountAndRemarksAndSleepNoForPaymentSuccess, TransactionManager.GetClientPaymentAmountAndRemarksAndSleepNoForPaymentError);

    },
    GetClientPaymentAmountAndRemarksAndSleepNoForPaymentSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {
            //var Transaction = (data.Transaction);
            //$("#txtMonthName").val(TransactionManager.GetPaymentMonthName(Transaction.MonthName));
            //$("#txtLoginName").val(Transaction.LoginName);
            //$("#txtUserID").val(Transaction.UserID);
            //$("#txtTotalAmount").val(Transaction.PaymentAmount);
            //$("#txtRegularBillPaidAmount").val(Transaction.PaidAmount);
            //$("#txtAmount").val(Transaction.DueAmount);
            //$("#txtRegularDiscount").val(Transaction.DiscountAmount);
            //$("#txtMoneyResetNo").val(/*Transaction.SerialNo*/);
            //$("#txtRemarks").val(data.RemarksNo);
            //$("#txtPermanentDiscount").val(Transaction.PermanentDiscount);


            var Transaction = (data.Transaction);

            $("#txtMonthName").val(TransactionManager.GetPaymentMonthName(Transaction.MonthName));
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

        //$("#mdlPackageUpdate").modal("show");
    },
    GetClientPaymentAmountAndRemarksAndSleepNoForPaymentError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    PayMonthlyBill: function (TransactionID, FromWhere) {
        var discount = $("#txtRegularDiscount").val();
        var resetNo = $("#txtMoneyResetNo").val();
        var remarksNo = $("#txtRemarks").val();
        var collectBy = $("#EmployeeID").val();
        var PaidAmount = $("#txtRegularBillPay").val();
        var AnotherMobileNo = $("#txtAnotherMobileNo").val();
        var url = "/Transaction/PayMonthlyBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere, AnotherMobileNo: AnotherMobileNo };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, TransactionManager.PayMonthlyBillSuccess, TransactionManager.PayMonthlyBillError);
    },
    PayMonthlyBillSuccess: function (data) {

        if (data.Success === true) {
            AppUtil.ShowSuccess("Payment Successfully done.");
            //window.location.href = "/Transaction/Accounts";
            TransactionManager.ClearPaymentModalDuringPaymentFromAccoutsPage();
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
     
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePayment: function (id) {
        //var url = '@Url.Action("GetPackageDetailsByID", "Package")';

        //AppUtil.ShowWaitingDialog();
        //  setTimeout(function () {
        var url = "/Transaction/GetRemarksAndSleepNoForPayment/";
        var data = { TransactionID: id };
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess, TransactionManager.GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentError);

        //   }, 500);

    },
    GetClientPaymentAmountAndRemarksAndSleepNoForDuePaymentSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {
            var Transaction = (data.Transaction);

            $("#txtDueMonthName").val(TransactionManager.GetPaymentMonthName(Transaction.MonthName));
            $("#txtDueLoginName").val(Transaction.LoginName);
            $("#txtDueUserID").val(Transaction.UserID);
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

    PayDueBill: function (TransactionID, FromWhere) {
        var discount = $("#txtDueDiscount").val();
        var resetNo = $("#txtDueMoneyResetNo").val();
        var remarksNo = $("#txtDueRemarks").val();
        var collectBy = $("#DueEmployeeID").val();
        var PaidAmount = $("#txtDuePay").val();
        var AnotherMobileNo = $("#txtDueAnotherMobileNo").val();
        var url = "/Transaction/PayDueBill/";
        var Transaction = { TransactionID: TransactionID, PaidAmount: PaidAmount, Discount: discount, ResetNo: resetNo, RemarksNo: remarksNo, BillCollectBy: collectBy, PaymentFromWhichPage: FromWhere, AnotherMobileNo: AnotherMobileNo };
        var data = JSON.stringify({ Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", data, TransactionManager.PayDueBillSuccess, TransactionManager.PayDueBillError);
    },
    PayDueBillSuccess: function (data) {
        if (data.Success === true) {
            var ts = data.duets;
            AppUtil.ShowSuccess("Due Payment Successfully Paid.");
            TransactionManager.ClearDuePaymentModalDuringPaymentFromAccoutsPage();
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
    PayDueBillError: function (data) {


        alert("Fail");
        console.log(data);
    },

    GenerateBillForThisMonth: function () {
        var url = "/Transaction/GenerateBillForThisMonth/";
        var data = {};
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.GenerateBillForThisMonthSuccess, TransactionManager.GenerateBillForThisMonthFail);
    },
    GenerateBillForThisMonthSuccess: function (data) {

        console.log(data);
        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message)
        }
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
            AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ');
        }

    },
    GenerateBillForThisMonthFail: function (data) {

        console.log(data);
        AppUtil.ShowSuccess('Something is wrong. Contact with administrator. ');
    },

    PayRunningMonthBillFromAdvanceAmount: function (FromWhere) {
        var url = "/Transaction/PayRunningMonthBillFromAdvanceAmount/";
        var data = ({ PaymentFromWhichPage: FromWhere });
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.PayRunningMonthBillFromAdvanceAmountSuccess, TransactionManager.PayRunningMonthBillFromAdvanceAmountError);
    },
    PayRunningMonthBillFromAdvanceAmountSuccess: function (data) {

        console.log(data);
        if (data.Success === true) {

            if (parseInt(data.AdvancePaymentCount) > 0) {
                AppUtil.ShowSuccess('Total ' + data.AdvancePaymentCount + ' advance payment generated.');
                window.location.href = "/Transaction/Accounts/";
            }
            if (parseInt(data.AdvancePaymentCount) === 0) {
                AppUtil.ShowError('Total ' + data.AdvancePaymentCount + ' advance payment generated.');
            }
        }
        if (data.Success === false) {
            AppUtil.ShowError('Something is wrong contact with administrator.');
            window.location.href = "/Transaction/Accounts/";
        }
    },
    PayRunningMonthBillFromAdvanceAmountError: function (data) {

        console.log(data);

    },

    PrintArchiveBillsList: function () {

        var url = "/Excel/CreateReportForArchiveBills";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var YearID = AppUtil.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("ZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({ Year: YearID, Month: MonthID, ZoneID: ZoneID });
        data = TransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, TransactionManager.PrintArchiveBillsListSuccess, TransactionManager.PrintArchiveBillsListFail);
    },
    PrintArchiveBillsListSuccess: function (data) {

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
    PrintArchiveBillsListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
      },

    // Accounts End //
}