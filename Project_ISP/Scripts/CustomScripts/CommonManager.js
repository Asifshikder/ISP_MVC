var CommonManager = {
    ClientLoginExistOrNot: function (loginName) {
        
        $("#Status").css("display", "none");
        var url = "/Client/ClientLoginExistOrNot/";

        // //AppUtil.ShowWaitingDialog();
        //code before the pause
        //setTimeout(function () {
            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = ({ LoginName: loginName });
            datas = CommonManager.addRequestVerificationToken(datas);
            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, CommonManager.ClientLoginExistOrNotSuccess, CommonManager.ClientLoginExistOrNotError);
      //  }, 50);
    },
    ClientLoginExistOrNotSuccess: function (data) {
        
        //  //AppUtil.HideWaitingDialog();
        if (data.Exist === true) {
            $("#Status").css("display", "inline");
        }
    },
    ClientLoginExistOrNotError: function (data) {
        
        //AppUtil.HideWaitingDialog();
    },


    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
}