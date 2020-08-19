var SubmittedBillByEmployeeManager = {

    addRequestVerificationToken: function (data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    RemovePaymentFromPH: function (txID, rstID) {
        var url = "/Transaction/DeleteTheBill/";
        var data = ({ TxID: txID, rstNO: rstID });
        data = SubmittedBillByEmployeeManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SubmittedBillByEmployeeManager.RemovePaymentFromPHSuccess, SubmittedBillByEmployeeManager.RemovePaymentFromPHError);
    },
    RemovePaymentFromPHSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Payment Remove Successfully.");
            table.draw();
        }
        $("#popModalForDeletePermently").modal("hide");
    },
    RemovePaymentFromPHError: function (data) {
        alert("Fail");
        console.log(data);
        table.draw();
        $("#popModalForDeletePermently").modal("hide");
    },
}