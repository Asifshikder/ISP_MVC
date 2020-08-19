


var ResellerTransactionManager = {


    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    PayMonthlyBillValidation: function () {

        
        if (AppUtil.GetIdValue("txtMoneyResetNo") === '') {
            AppUtil.ShowSuccess("Please Add Money Reset No. ");
            return false;
        }

        if (AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
            return false;
        }

        return true;
    },
    PayDueBillValidation: function () {

        
        if (AppUtil.GetIdValue("txtDueMoneyResetNo") === '') {
            AppUtil.ShowSuccess("Please Add Money Reset No. ");
            return false;
        }

        if (AppUtil.GetIdValue("DueEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Who Collect the bill. ");
            return false;
        }

        return true;
    },

    ValidationForArchiveBillsSearch: function () {

        
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

    GetBillsListBySearchCriteria: function () {
        //AppUtil.ShowWaitingDialog();
        var YearID = $("#YearID option:selected").text();//.GetIdValue("YearID");
        var MonthID = AppUtil.GetIdValue("MonthID");
        var ZoneID = AppUtil.GetIdValue("ZoneID");

        var url = "/Reseller/GetBillsListBySearchCriteria/";
        var data = ({ YearID: YearID, MonthID: MonthID, ZoneID: ZoneID });
        data = ResellerTransactionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ResellerTransactionManager.GetBillsListBySearchCriteriaSuccess, ResellerTransactionManager.GetBillsListBySearchCriteriaFail);
    },
    GetBillsListBySearchCriteriaSuccess: function (data) {
        if (data.Date) {
            $("#dateArchiveBills").html(data.Date);
            $("#dateBillSummary").html(data.Date);
        }

        
       
        if (data.Success === true) {
            searchBySearchButton = 1;
            
            
            ResellerTransactionManager.MakeEmptyBillSummary();

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
}