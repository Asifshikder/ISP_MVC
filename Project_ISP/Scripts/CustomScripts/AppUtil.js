var AppUtil = {
    ShowErrors: function (message, id, pos) {
        alert(message + " " + id + " " + pos);
        //$("#login-pinno").notify("Please enter pin no", { position: "top center" });
        //$("#" + id).notify("asasdf", { position: pos });
        $("#login-pinno").notify("idoot", { position: "top center" });
        //$("#nid").notify("Please enter National ID", { position: "top center" });
    },

    MakeAjaxCall: function (url, type, data, SuccessCall, ErrorCall) {

        $.ajax({
            type: type,
            //async: false,
            url: url,
            dataType: "json",
            cache: false,
             //headers: header,
            contentType: 'application/json; charset=utf-8',
            data: data,
            success: SuccessCall,
            error: ErrorCall

        });


    },
    //MakeAjaxCallJSONAntifergery: function (url, type, data, header, SuccessCall, ErrorCall) {

    //    $.ajax({
    //        type: type,
    //        //async: false,
    //        url: url,
    //        dataType: "json",
    //        cache: false,
    //        headers: header,
    //        contentType: 'application/json; charset=utf-8',
    //        data: data,
    //        success: SuccessCall,
    //        error: ErrorCall 
    //    }); 
    //},

    MakeAjaxCallJSONAntifergeryNotFormCollection: function (url, type, data, header, SuccessCall, ErrorCall) { 
        $.ajax({
            type: type,
            //async: false,
            url: url,
            dataType: "json",
            cache: false,
            headers: header,
            contentType: 'application/json; charset=utf-8',
            data: data,
            success: SuccessCall,
            error: ErrorCall 
        }); 
    },

    MakeAjaxCallJSONAntifergery: function (url, type, data, header, SuccessCall, ErrorCall) {//this start when we start pass image using jquery from client create page.

        $.ajax({ 
            type: type,
            //async: false,
            url: url,
            //dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            headers: header,
            //contentType: 'application/json; charset=utf-8',
            data: data,
            success: SuccessCall,
            error: ErrorCall

        });


    },
    MakeAjaxCallJSONAntifergeryJSON: function (url, type, data, header, SuccessCall, ErrorCall) {//this start when we start pass image using jquery from client create page.

        $.ajax({
            

            type: type,
            async: false,
            url: url,
            dataType: "json",
            cache: false,
            headers: header,
            contentType: 'application/json; charset=utf-8',
            data: data,
            success: SuccessCall,
            error: ErrorCall

        });


    },
    MakeAjaxCallsForAntiForgery: function (url, type, data, SuccessCall, ErrorCall) {

        $.ajax({
            async: false,
            type: type,
            //headers:headers,
            url: url,
            dataType: "json",
            //contentType: "'application/x-www-form-urlencoded; charset=utf-8'",
            cache: false,
            data: data,
            success: SuccessCall,
            error: ErrorCall

        });
    },


    GetIdValue: function (id) {
        //
        if (($("#" + id)).value == "") {
            return "";
        }
        else
            return $("#" + id).val();

    },

    getDateTime: function (id) {
        return $('#' + id).datepicker('getDate');
    },
    ParseDateTime: function (dateString) {

        var myDate = moment(dateString);
        return myDate.format("DD/MM/YYYY hh:mm A");
    },
    ParseDate: function (dateString) {

        var myDate = moment(dateString);
        return myDate.format("DD/MM/YYYY");
    },
    ParseDateINMMDDYYYY: function (dateString) {
        
        var myDate = moment(dateString);
        return myDate.format("MM/DD/YYYY");
    },
    ShowSuccess: function (message) {

        $.notify(message, "success");
    },

    ShowError: function (message) {
        $.notify(message, "error");
    },

    ShowSuccessOnControl: function (message, controlID, position) {
        $("#" + controlID).notify(message, {
            position: position,
            autoHideDelay: 5000,
            showAnimation: "fadeIn",
            hideAnimation: "fadeOut",
            hideDuration: 700,
            //arrowShow: false,
            className: "success"
        });
    },

    ShowErrorOnControl: function (message, controlID, position) {
        $("#" + controlID).notify(message, {
            position: position,
            autoHideDelay: 5000,
            showAnimation: "fadeIn",
            hideAnimation: "fadeOut",
            hideDuration: 700,
            //arrowShow: false,
            className: "error",
            clickToHide: true,
            //autoHide: false
        });
    },
    //ShowErrorOnControl: function (message, controlID, position) {

        
    //    $("#" + controlID).notify(message, { position: position });
    //},
    ShowLoadingModal: function () {
        $("#loadingModal").modal({ backdrop: 'static' });
    },

    HideLoadingModal: function () {
        $("#loadingModal").modal('hide');
    },

    ShowWaitingDialog: function (message) {
        waitingDialogManager.show(message, { dialogSize: 'sm', progressType: 'success' });
        //waitingDialogManager.show();
    },

    HideWaitingDialog: function () {
        waitingDialogManager.hide()
    },
}