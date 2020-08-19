var ClientUpdateManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    UpdateClientDetailsValidation: function () {
         
        if (AppUtil.GetIdValue("Name") === '') {
            //AppUtil.ShowSuccess("Please Insert Name.");
            AppUtil.ShowErrorOnControl("Please Insert Name.", "Name", "top center");
            return false;
        }

        //if (AppUtil.GetIdValue("Email") === '') {
        //    AppUtil.ShowSuccess("Please Insert Email.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("LoginName") === '') {
            //AppUtil.ShowSuccess("Please Insert LoginName.");
            AppUtil.ShowErrorOnControl("Please Insert Login Name.", "LoginName", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("UserID") === '') {
            //AppUtil.ShowSuccess("Please Insert UserID.");
            AppUtil.ShowErrorOnControl("Please Insert UserID.", "UserID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Password") === '') {
            //AppUtil.ShowSuccess("Please Insert Password.");
            AppUtil.ShowErrorOnControl("Please Insert Password.", "Password", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("Address") === '') {
            //AppUtil.ShowSuccess("Please Insert Address.");
            AppUtil.ShowErrorOnControl("Please Insert Address.", "Address", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("LatitudeLongitude") === '') {
            //AppUtil.ShowSuccess("Please Insert Address.");
            AppUtil.ShowErrorOnControl("Please Insert LatitudeLongitude.", "LatitudeLongitude", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("ContactNumber") === '') {
            //AppUtil.ShowSuccess("Please Insert Contact Number.");
            AppUtil.ShowErrorOnControl("Please Insert Contact Number.", "ContactNumber", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("ZoneID") === '') {
            //AppUtil.ShowSuccess("Please Select Zone.");
            AppUtil.ShowErrorOnControl("Please Select Zone.", "ZoneID", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("SMSCommunication") === '') {
            //AppUtil.ShowSuccess("Please Insert SMS Communication.");
            AppUtil.ShowErrorOnControl("Please Insert SMS Communication.", "SMSCommunication", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("Occupation") === '') {
        //    AppUtil.ShowSuccess("Please Insert Occupation.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
        //    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("Remarks") === '') {
        //    AppUtil.ShowSuccess("Please Insert Remarks.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
            //AppUtil.ShowSuccess("Please Select Connection Type.");
            AppUtil.ShowErrorOnControl("Please Select Connection Type.", "ConnectionTypeID", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("BoxNumber") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Box Number.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("PopDetails") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Pop Details.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("RequireCable") === '') {
            //AppUtil.ShowSuccess("Please Insert Require Cable.");
            AppUtil.ShowErrorOnControl("Please Insert Require Cable.", "RequireCable", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("CableTypeID") === '') {
        //    //AppUtil.ShowSuccess("Please Select Cable Type.");
        //    AppUtil.ShowErrorOnControl("Please Select Cable Type.", "CableTypeID", "top center");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("Reference") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Reference.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("NationalID") === '') {
        //    //AppUtil.ShowSuccess("Please Select National Id.");
        //    return false;
        //}


        ////if (AppUtil.GetIdValue("PackageID") === '') {
        ////    //AppUtil.ShowSuccess("Please Select Package.");
        ////    AppUtil.ShowErrorOnControl("Please Select Package.", "PackageID", "top center");
        ////    return false;
        ////}


        if (AppUtil.GetIdValue("PackageThisMonth") === '' || AppUtil.GetIdValue("PackageThisMonth") === null) {
            //AppUtil.ShowSuccess("Please Select Package.");
            AppUtil.ShowErrorOnControl("Please Select This Month Package.", "PackageThisMonth", "top center");
            return false;
        }
        //var a = AppUtil.GetIdValue("PackageNextMonth");
        if (AppUtil.GetIdValue("PackageNextMonth") === '' || AppUtil.GetIdValue("PackageNextMonth") === null) {
            //AppUtil.ShowSuccess("Please Select Package.");
            AppUtil.ShowErrorOnControl("Please Select Next Month Package.", "PackageNextMonth", "top center");
            return false;
        }


        if (AppUtil.GetIdValue("SingUpFee") === '') {
            //AppUtil.ShowSuccess("Please Insert SignUp Fee.");
            AppUtil.ShowErrorOnControl("Please Insert SignUp Fee.", "SingUpFee", "top center");
            return false;
        }
        if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
            //AppUtil.ShowSuccess("Please Select Security Question.");
            AppUtil.ShowErrorOnControl("Please Select Security Question.", "SecurityQuestionID", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
        //    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("MacAddress") === '') {
        //    AppUtil.ShowSuccess("Please Insert Mac Address.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("BillPaymentDate") === '') {   
        if (AppUtil.GetIdValue("BillPaymentDate") < 1 || AppUtil.GetIdValue("BillPaymentDate") > 31) {
            //AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
            AppUtil.ShowErrorOnControl("Bill payment Date must be between 1 and 31.", "BillPaymentDate", "top center");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientSurvey") === '') {
        //    AppUtil.ShowSuccess("Please Insert Client Survey.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ConnectionDate") === '') {
            //AppUtil.ShowSuccess("Please Insert Connection Date.");
            AppUtil.ShowErrorOnControl("Please Insert Connection Date.", "ConnectionDate", "top center");
            return false;
        }

        if (AppUtil.GetIdValue("LineStatusID") === '') {
            //AppUtil.ShowSuccess("Please Select Line Status.");
            AppUtil.ShowErrorOnControl("Please Select Line Status.", "LineStatusID", "top center");
            return false;
        }
        //var a = AppUtil.GetIdValue("LineStatusActiveDate");
        if (AppUtil.GetIdValue("LineStatusActiveDate") === '' || AppUtil.GetIdValue("LineStatusActiveDate") == 'Invalid date') {
            //AppUtil.ShowSuccess("Please Insert Line Status Active Date.");
            AppUtil.ShowErrorOnControl("Please Insert Line Status Active Date.", "LineStatusActiveDate", "top center");
            return false;
        }


        return true;
    },

    GetClientDetailsByID: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/CLient/GetClientDetailsByID/";
        var Data = ({ ClientDetailsID: id });
        Data = ClientUpdateManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdateManager.GetClientDetailsByIDSuccess, ClientUpdateManager.GetClientDetailsByIDError);
        //  }, 500);



    },
    GetClientDetailsByIDSuccess: function (data) {


        //var ClientDetails = JSON.parse(data.ClientLineStatus);
        //var Transaction = (data.Transaction);

        var ClientDetails = (data.ClientLineStatus);
        var Transaction = (data.Transaction);
        var cableDetails = data.CableForThisClient;
        var itemsDetails = data.ItemForThisClient;

        console.log("Transaction: " + Transaction);
        console.log("ClientLineStatus: " + ClientDetails);

        ClientDetailsID = ClientDetails.ClientDetailsID;
        ClientLineStatusID = ClientDetails.ClientLineStatusID;
        // ClientBannedStatusID;
        ClientTransactionID = Transaction[0].TransactionID;

        var itemList = "";
        $.each(cableDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Cable:&nbsp;&nbsp;" + item.CableType + " " + item.AmountOfCableGiven + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /> ";
            });
        });
        if (itemList.length > 0) {
            itemList += "<br />";
        }
        $.each(itemsDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Item Name:&nbsp;&nbsp;" + item.ItemName + " &nbsp;&nbsp;&nbsp;&nbsp;Serial:&nbsp;&nbsp;" + item.ItemSerial + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b> ";
            });
        });
        //itemList += "</div>";

        //if (ClientDetails.ClientOWNImageBytesPaths != null && ClientDetails.ClientOWNImageBytesPaths != "") {

        $("#PreviewClientOWNImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientOWNImageBytesPaths + '#' + new Date().getTime()).show(0);//.attr("src", ClientDetails.ClientOWNImageBytesPaths);
        $("#PreviewClientOWNImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientOWNImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientOWNImageBytesPaths").attr("value", "" + ClientDetails.ClientOWNImageBytesPaths + "");
        //$("#ClientOwnImagePath").val(ClientDetails.ClientOwnImageBytesPaths);
        //}

        $("#divPreviewClientNIDImageBytesPaths").html("");
        $("#divPreviewClientNIDImageBytesPaths").html('<img id="PreviewClientNIDImageBytesPaths" src="" width="100" height="90" onclick="">');
        //$("#PreviewClientNIDImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientNIDImageBytesPaths + '').show(0);
        $("#PreviewClientNIDImageBytesPaths").hide(0).attr("src", "" + ClientDetails.ClientNIDImageBytesPaths + "#" + new Date().getTime()).show(0);
        $("#PreviewClientNIDImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientNIDImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientNIDImageBytesPaths").attr("value", "" + ClientDetails.ClientNIDImageBytesPaths + "");
        //}

        $("#txtItemAndCablesAssign").html(itemList);
        $("#Name").val(ClientDetails.Name);
        $("#Email").val(ClientDetails.Email);
        $("#LoginName").val(ClientDetails.LoginName);
        $("#Password").val(ClientDetails.Password);
        $("#Address").val(ClientDetails.Address);
        $("#LatitudeLongitude").val(ClientDetails.LatitudeLongitude);
        $("#ContactNumber").val(ClientDetails.ContactNumber);
        $("#ZoneID").val(ClientDetails.ZoneID);
        $("#SMSCommunication").val(ClientDetails.SMSCommunication);
        $("#Occupation").val(ClientDetails.Occupation);
        $("#SocialCommunicationURL").val(ClientDetails.SocialCommunicationURL);
        $("#Remarks").val(ClientDetails.Remarks);
        $("#ConnectionTypeID").val(ClientDetails.ConnectionTypeID);
        $("#BoxID").val(ClientDetails.BoxNumber);
        $("#PopDetails").val(ClientDetails.PopDetails);
        $("#RequireCable").val(ClientDetails.RequireCable);
        $("#CableTypeID").val(ClientDetails.CableTypeID);
        $("#Reference").val(ClientDetails.Reference);
        $("#NationalID").val(ClientDetails.NationalID);

        //$("#PackageID").val(ClientDetails.PackageID);
        $("#PackageThisMonth").val(ClientDetails.PackageThisMonth);
        $("#PackageNextMonth").val(ClientDetails.PackageNextMonth);
        if (data.ControlNeedToDisable) {
            $("#SingUpFee").val(Transaction[0].PaymentAmount).prop("disabled",true);
            //$("#PermanentDiscount").val(ClientDetails.PermanentDiscount).prop("disabled", true);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate).prop("disabled", true);;
        }
        else {
            $("#SingUpFee").val(Transaction[0].PaymentAmount);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate);
            
        }
        $("#PermanentDiscount").val(ClientDetails.PermanentDiscount);
        $("#SecurityQuestionID").val(ClientDetails.SecurityQuestionID);
        $("#SecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
        $("#MacAddress").val(ClientDetails.MacAddress);
        //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
        $("#ClientSurvey").val(ClientDetails.ClientSurvey);
        $("#ConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
        $("#LineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

        // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#ThisMonthLineStatusID").val(ClientDetails.ThisMonthLineStatusID);
        $("#NextMonthLineStatusID").val(ClientDetails.NextMonthLineStatusID);
        //$("#LineStatusID").val(ClientDetails.LineStatusID);
        $("#Reason").val(ClientDetails.StatusChangeReason);

        $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

        $("#lstMikrotik").val(ClientDetails.MikrotikID);
        $("#IP").val(ClientDetails.IP);
        $("#Mac").val(ClientDetails.Mac);

        $("#ResellerID").val(ClientDetails.ResellerID);
        $("#divProfileUpdatePercentage").html(ClientDetails.ProfileStatusUpdateInPercent);
        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },



    GetNewClientDetailsByID: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/NewClient/GetClientDetailsByID/";
        var Data = ({ ClientDetailsID: id });
        Data = ClientUpdateManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdateManager.GetNewClientDetailsByIDSuccess, ClientUpdateManager.GetNewClientDetailsByIDError);
        //  }, 500);



    },
    GetNewClientDetailsByIDSuccess: function (data) {


        //var ClientDetails = JSON.parse(data.ClientLineStatus);
        //var Transaction = (data.Transaction);

        var ClientDetails = (data.ClientLineStatus);
        var Transaction = (data.Transaction);
        var cableDetails = data.CableForThisClient;
        var itemsDetails = data.ItemForThisClient;

        console.log("Transaction: " + Transaction);
        console.log("ClientLineStatus: " + ClientDetails);

        ClientDetailsID = ClientDetails.ClientDetailsID;
        ClientLineStatusID = ClientDetails.ClientLineStatusID;
        // ClientBannedStatusID;
        ClientTransactionID = Transaction[0].TransactionID;

        var itemList = "";
        $.each(cableDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Cable:&nbsp;&nbsp;" + item.CableType + " " + item.AmountOfCableGiven + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /> ";
            });
        });
        if (itemList.length > 0) {
            itemList += "<br />";
        }
        $.each(itemsDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Item Name:&nbsp;&nbsp;" + item.ItemName + " &nbsp;&nbsp;&nbsp;&nbsp;Serial:&nbsp;&nbsp;" + item.ItemSerial + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b> ";
            });
        });
        //itemList += "</div>";

        //if (ClientDetails.ClientOWNImageBytesPaths != null && ClientDetails.ClientOWNImageBytesPaths != "") {

        $("#PreviewClientOWNImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientOWNImageBytesPaths + '#' + new Date().getTime()).show(0);//.attr("src", ClientDetails.ClientOWNImageBytesPaths);
        $("#PreviewClientOWNImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientOWNImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientOWNImageBytesPaths").attr("value", "" + ClientDetails.ClientOWNImageBytesPaths + "");
        //$("#ClientOwnImagePath").val(ClientDetails.ClientOwnImageBytesPaths);
        //}

        $("#divPreviewClientNIDImageBytesPaths").html("");
        $("#divPreviewClientNIDImageBytesPaths").html('<img id="PreviewClientNIDImageBytesPaths" src="" width="100" height="90" onclick="">');
        //$("#PreviewClientNIDImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientNIDImageBytesPaths + '').show(0);
        $("#PreviewClientNIDImageBytesPaths").hide(0).attr("src", "" + ClientDetails.ClientNIDImageBytesPaths + "#" + new Date().getTime()).show(0);
        $("#PreviewClientNIDImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientNIDImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientNIDImageBytesPaths").attr("value", "" + ClientDetails.ClientNIDImageBytesPaths + "");
        //}

        $("#txtItemAndCablesAssign").html(itemList);
        $("#Name").val(ClientDetails.Name);
        $("#Email").val(ClientDetails.Email);
        $("#LoginName").val(ClientDetails.LoginName);
        $("#Password").val(ClientDetails.Password);
        $("#Address").val(ClientDetails.Address);
        $("#ContactNumber").val(ClientDetails.ContactNumber);
        $("#ZoneID").val(ClientDetails.ZoneID);
        $("#SMSCommunication").val(ClientDetails.SMSCommunication);
        $("#Occupation").val(ClientDetails.Occupation);
        $("#SocialCommunicationURL").val(ClientDetails.SocialCommunicationURL);
        $("#Remarks").val(ClientDetails.Remarks);
        $("#ConnectionTypeID").val(ClientDetails.ConnectionTypeID);
        $("#BoxID").val(ClientDetails.BoxNumber);
        $("#PopDetails").val(ClientDetails.PopDetails);
        $("#RequireCable").val(ClientDetails.RequireCable);
        $("#CableTypeID").val(ClientDetails.CableTypeID);
        $("#Reference").val(ClientDetails.Reference);
        $("#NationalID").val(ClientDetails.NationalID);

        //$("#PackageID").val(ClientDetails.PackageID);
        $("#PackageThisMonth").val(ClientDetails.PackageThisMonth);
        $("#PackageNextMonth").val(ClientDetails.PackageNextMonth);
        if (data.ControlNeedToDisable) {
            $("#SingUpFee").val(Transaction[0].PaymentAmount).prop("disabled", true);
            //$("#PermanentDiscount").val(ClientDetails.PermanentDiscount).prop("disabled", true);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate).prop("disabled", true);;
        }
        else {
            $("#SingUpFee").val(Transaction[0].PaymentAmount);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate);

        }
        $("#PermanentDiscount").val(ClientDetails.PermanentDiscount);
        $("#SecurityQuestionID").val(ClientDetails.SecurityQuestionID);
        $("#SecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
        $("#MacAddress").val(ClientDetails.MacAddress);
        //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
        $("#ClientSurvey").val(ClientDetails.ClientSurvey);
        $("#ConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
        $("#LineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

        // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#ThisMonthLineStatusID").val(ClientDetails.ThisMonthLineStatusID);
        $("#NextMonthLineStatusID").val(ClientDetails.NextMonthLineStatusID);
        //$("#LineStatusID").val(ClientDetails.LineStatusID);
        $("#Reason").val(ClientDetails.StatusChangeReason);

        $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

        $("#lstMikrotik").val(ClientDetails.MikrotikID);
        $("#IP").val(ClientDetails.IP);
        $("#Mac").val(ClientDetails.Mac);

        $("#ResellerID").val(ClientDetails.ResellerID);
        $("#divProfileUpdatePercentage").html(ClientDetails.ProfileStatusUpdateInPercent);
        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetNewClientDetailsByIDError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    GetClientDetailsByIDForResellerUserByAdmin: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/CLient/GetClientDetailsByIDForResellerUserByAdmin/";
        var Data = ({ ClientDetailsID: id });
        Data = ClientUpdateManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdateManager.GetClientDetailsByIDForResellerUserByAdminSuccess, ClientUpdateManager.GetClientDetailsByIDForResellerUserByAdminError);
        //  }, 500);



    },
    GetClientDetailsByIDForResellerUserByAdminSuccess: function (data) {


        //var ClientDetails = JSON.parse(data.ClientLineStatus);
        //var Transaction = (data.Transaction);

         
        $("#PackageThisMonth,#PackageNextMonth").find("option").not(":first").remove(); 
        $("#ZoneID").find("option").not(":first").remove();
        $("#BoxID").find("option").not(":first").remove();
        $("#lstMikrotik").find("option").not(":first").remove();
         

        var zoneList = (data.resellerzone);
        var packageList = (data.resellerpackage);
        var boxList = (data.resellerbox);
        var mikrotikList = (data.resellermikrotik); 

        $.each(zoneList, function (index, element) {
            $("#ZoneID").append($("<option></option>").val(element.zoneid).text(element.zonename));
        });
        $.each(packageList, function (index, element) {
            $("#PackageThisMonth,#PackageNextMonth").append($("<option></option>").val(element.packageid).text(element.packagename));
        });
        $.each(boxList, function (index, element) {
            $("#BoxID").append($("<option></option>").val(element.boxid).text(element.boxname));
        });
        $.each(mikrotikList, function (index, element) {
            $("#lstMikrotik").append($("<option></option>").val(element.mikid).text(element.mikname));
        });

        var ClientDetails = (data.ClientLineStatus); 
        var Transaction = (data.Transaction);
        var cableDetails = data.CableForThisClient;
        var itemsDetails = data.ItemForThisClient; 

        ClientDetailsID = ClientDetails.ClientDetailsID;
        ClientLineStatusID = ClientDetails.ClientLineStatusID; 
        ClientTransactionID = Transaction[0].TransactionID;

        var itemList = "";
        $.each(cableDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Cable:&nbsp;&nbsp;" + item.CableType + " " + item.AmountOfCableGiven + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /> ";
            });
        });
        if (itemList.length > 0) {
            itemList += "<br />";
        }
        $.each(itemsDetails, function (index, item) {
            $.each(item, function (index, item) {
                itemList += "<b> Item Name:&nbsp;&nbsp;" + item.ItemName + " &nbsp;&nbsp;&nbsp;&nbsp;Serial:&nbsp;&nbsp;" + item.ItemSerial + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b> ";
            });
        });
        //itemList += "</div>";

        //if (ClientDetails.ClientOWNImageBytesPaths != null && ClientDetails.ClientOWNImageBytesPaths != "") {

        $("#PreviewClientOWNImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientOWNImageBytesPaths + '#' + new Date().getTime()).show(0);//.attr("src", ClientDetails.ClientOWNImageBytesPaths);
        $("#PreviewClientOWNImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientOWNImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientOWNImageBytesPaths").attr("value", "" + ClientDetails.ClientOWNImageBytesPaths + "");
        //$("#ClientOwnImagePath").val(ClientDetails.ClientOwnImageBytesPaths);
        //}

        $("#divPreviewClientNIDImageBytesPaths").html("");
        $("#divPreviewClientNIDImageBytesPaths").html('<img id="PreviewClientNIDImageBytesPaths" src="" width="100" height="90" onclick="">');
        //$("#PreviewClientNIDImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientNIDImageBytesPaths + '').show(0);
        $("#PreviewClientNIDImageBytesPaths").hide(0).attr("src", "" + ClientDetails.ClientNIDImageBytesPaths + "#" + new Date().getTime()).show(0);
        $("#PreviewClientNIDImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientNIDImageBytesPaths')");
        //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
        $("#ClientNIDImageBytesPaths").attr("value", "" + ClientDetails.ClientNIDImageBytesPaths + "");
        //}

        $("#txtItemAndCablesAssign").html(itemList);
        $("#Name").val(ClientDetails.Name);
        $("#Email").val(ClientDetails.Email);
        $("#LoginName").val(ClientDetails.LoginName);
        $("#Password").val(ClientDetails.Password);
        $("#Address").val(ClientDetails.Address);
        $("#ContactNumber").val(ClientDetails.ContactNumber);
        $("#ZoneID").val(ClientDetails.ZoneID);
        $("#SMSCommunication").val(ClientDetails.SMSCommunication);
        $("#Occupation").val(ClientDetails.Occupation);
        $("#SocialCommunicationURL").val(ClientDetails.SocialCommunicationURL);
        $("#Remarks").val(ClientDetails.Remarks);
        $("#ConnectionTypeID").val(ClientDetails.ConnectionTypeID);
        $("#BoxID").val(ClientDetails.BoxNumber);
        $("#PopDetails").val(ClientDetails.PopDetails);
        $("#RequireCable").val(ClientDetails.RequireCable);
        $("#CableTypeID").val(ClientDetails.CableTypeID);
        $("#Reference").val(ClientDetails.Reference);
        $("#NationalID").val(ClientDetails.NationalID);

        //$("#PackageID").val(ClientDetails.PackageID);
        $("#PackageThisMonth").val(ClientDetails.PackageThisMonth);
        $("#PackageNextMonth").val(ClientDetails.PackageNextMonth);
        if (data.ControlNeedToDisable) {
            $("#SingUpFee").val(Transaction[0].PaymentAmount).prop("disabled",true);
            //$("#PermanentDiscount").val(ClientDetails.PermanentDiscount).prop("disabled", true);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate).prop("disabled", true);;
        }
        else {
            $("#SingUpFee").val(Transaction[0].PaymentAmount);
            $("#BillPaymentDate").val(ClientDetails.PaymentDate);
            
        }
        $("#PermanentDiscount").val(ClientDetails.PermanentDiscount);
        $("#SecurityQuestionID").val(ClientDetails.SecurityQuestionID);
        $("#SecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
        $("#MacAddress").val(ClientDetails.MacAddress);
        //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
        $("#ClientSurvey").val(ClientDetails.ClientSurvey);
        $("#ConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
        $("#LineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

        // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#ThisMonthLineStatusID").val(ClientDetails.ThisMonthLineStatusID);
        $("#NextMonthLineStatusID").val(ClientDetails.NextMonthLineStatusID);
        //$("#LineStatusID").val(ClientDetails.LineStatusID);
        $("#Reason").val(ClientDetails.StatusChangeReason);

        $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

        $("#lstMikrotik").val(ClientDetails.MikrotikID);
        $("#IP").val(ClientDetails.IP);
        $("#Mac").val(ClientDetails.Mac);

        $("#ResellerID").val(ClientDetails.ResellerID);
        $("#divProfileUpdatePercentage").html(ClientDetails.ProfileStatusUpdateInPercent);
        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDForResellerUserByAdminError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    GetLastID: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/CLient/GetLastID/";
        var Data = ({ ClientDetailsID: id });
        Data = ClientUpdateManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdateManager.GetLastIDSuccess, ClientUpdateManager.GetLastIDError);
        //  }, 500);



    },
    GetLastIDSuccess: function (data) { 
        var resellerid = $("#lstResellerID").val();
        var Name = "User";
        if (resellerid != '') {
            Name += $("#lstResellerID option:selected").text();
        }
        Name += data.id;

        $("#txtItemAndCablesAssign").html("");
        $("#Name").val(Name);
        $("#Email").val(Name+"@Gmail.com");
        $("#LoginName").val(Name);
        $("#Password").val("1");
        $("#Address").val("Dhaka Bangladesh");
        $("#ContactNumber").val("01553138099");
        $("#ZoneID").val(5);
        $("#SMSCommunication").val("01553138099");
        $("#Occupation").val("DOctor");
        $("#SocialCommunicationURL").val("");
        $("#Remarks").val("Remarks");
        $("#ConnectionTypeID").val(1);
        $("#BoxID").val("5");
        $("#PopDetails").val("5");
        $("#RequireCable").val("");
        $("#CableTypeID").val("");
        $("#Reference").val("Khabir Uddin ");
        $("#NationalID").val("1111111111111111");

        //$("#PackageID").val( PackageID);
        $("#PackageThisMonth").val(5);
        $("#PackageNextMonth").val(5);

        $("#SingUpFee").val("500");
        $("#SecurityQuestionID").val("1");
        $("#SecurityQuestionAnswer").val("");
        $("#MacAddress").val("M102030104055");
        //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
        $("#BillPaymentDate").val("5");
        $("#ClientSurvey").val("Client Servey Done.");
        $("#ConnectionDate").val("11/03/2019");
        $("#LineStatusActiveDate").val("2019-11-03");

        // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#ThisMonthLineStatusID").val("1");
        $("#NextMonthLineStatusID").val("1");
        //$("#LineStatusID").val( LineStatusID);
        $("#Reason").val("Test change");

        $("#IsPriorityClient").prop("checked",  false);


    },
    GetLastIDError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    UpdateClientDetails: function () {

        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";

        var Name = AppUtil.GetIdValue("Name");
        var Email = AppUtil.GetIdValue("Email");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("Address");
        var LatitudeLongitude = AppUtil.GetIdValue("LatitudeLongitude");
        var ContactNumber = AppUtil.GetIdValue("ContactNumber");
        var ZoneID = AppUtil.GetIdValue("ZoneID");
        var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
        var Occupation = AppUtil.GetIdValue("Occupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("Remarks");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("BoxID");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        //var PackageID = AppUtil.GetIdValue("PackageID");
        var PackageThisMonth = AppUtil.GetIdValue("PackageThisMonth");
        var PackageNextMonth = AppUtil.GetIdValue("PackageNextMonth");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee");       
        var PermanentDiscount = AppUtil.GetIdValue("PermanentDiscount");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        var BillPaymentDate = $("#BillPaymentDate").val();//AppUtil.getDateTime("BillPaymentDate");//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
        var LineStatusWillActiveInThisDate = $("#LineStatusActiveDate").val();

        //var LineStatusID = AppUtil.GetIdValue("LineStatusID");
        var ThisMonthLineStatusID = AppUtil.GetIdValue("ThisMonthLineStatusID");
        var NextMonthLineStatusID = AppUtil.GetIdValue("NextMonthLineStatusID");
        var Reason = AppUtil.GetIdValue("Reason");

        var IsPriorityClient = $("#IsPriorityClient").is(':checked');

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");



        //var StatusThisMonth = AppUtil.GetIdValue("LineStatusID");
        //var StatusNextMonth = AppUtil.GetIdValue("LineStatusID");

        //var PackageThisMonth = AppUtil.GetIdValue("PackageID");
        //var PackageNextMonth = AppUtil.GetIdValue("PackageID");
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, /*PackageID: PackageID,*/ SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient, ApproxPaymentDate: BillPaymentDate
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac
            , ResellerID: ResellerID
            , StatusThisMonth: ThisMonthLineStatusID
            , StatusNextMonth: NextMonthLineStatusID
            , LatitudeLongitude: LatitudeLongitude

            , PackageThisMonth: PackageThisMonth
            , PackageNextMonth: PackageNextMonth
            , LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate
            , PermanentDiscount: PermanentDiscount
        };
        var Transaction = { TransactionID: ClientTransactionID, PaymentAmount: SingUpFee, PermanentDiscount: PermanentDiscount };
        var ClientLineStatus = {
            ClientLineStatusID: ClientLineStatusID, ClientDetailsID: ClientDetailsID, LineStatusID: /*LineStatusID*/NextMonthLineStatusID, StatusChangeReason: Reason, /*PackageID: PackageID,*/ LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate
            , PackageThisMonth: PackageThisMonth
            , PackageNextMonth: PackageNextMonth
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var a = $('#ClientOWNImagePath').val();
        var formData = new FormData();
        formData.append('ClientOWNImageBytes', $('#ClientOWNImageBytes')[0].files[0]);
        formData.append('ClientOWNImagePath', $('#ClientOWNImageBytesPaths').val());
        formData.append('ClientNIDImage', $('#ClientNIDImage')[0].files[0]);
        formData.append('ClientNIDImagePath', $('#ClientNIDImageBytesPaths').val());
        formData.append('ClientClientDetails', JSON.stringify(ClientDetails));
        formData.append('ClientTransaction', JSON.stringify(Transaction));
        formData.append('ClientClientLineStatus', JSON.stringify(ClientLineStatus));
        formData.append('chkPackageFromRunningMonth', JSON.stringify(chkPackageFromRunningMonth));
        formData.append('chkStatusFromRunningMonth', JSON.stringify(chkStatusFromRunningMonth));


        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdateManager.UpdateClientDetailsSuccess, ClientUpdateManager.UpdateClientDetailsFail);
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientUpdateManager.UpdateClientDetailsSuccess, ClientUpdateManager.UpdateClientDetailsFail);
        // }, 50);
    },
    UpdateClientDetailsSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }

        if (data.Success === false) {
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
        }
        if (data.StatusIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError("Sorry you checked running month but the Status is remaing same. please chage the Status.");
        }
        if (data.PackageIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError("Sorry you checked running month but the package is remaing same. please chage the package.");
        }

        if (data.BothChecked === true) {
            AppUtil.ShowError("Sorry Running Month Checkbox and LockFromRunning Month Cant Check at a time.");
        }
        if (data.BillAlreadyGenerate === true) {
            AppUtil.ShowError("Sorry You Can Not Change Package. Cause Bill Already Generate.");
        }
        if (data.BillNotGenerate === true) {
            AppUtil.ShowError("Bill Not Generate.");
        }
        if (data.BillAlreadyPaid === true) {
            AppUtil.ShowError("Bill Already paid you cant change package.");
        }
        if (data.PackageCantChangeInThisMonthLimitExist === true) {
            AppUtil.ShowError("Package Cant change cause already its changed in this month. wait till next month.");
        }
        if (data.CantChangePackageCauseStatusIsLock === true) {
            AppUtil.ShowError("Sorry You Can Not Change Package. Cause This Client Is Lock.");
        }

        if (data.LoginNameExist === true) {
            AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        }
        if (data.LoginNameExist === false && data.UpdateStatus === true) {

            var packageName = "";
            var ClientStatusThisMonthHtml = "";
            var ClientStatusNextMonthHtml = "";
            var needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = "";


            var StatusThisMonth = (data.StatusThisMonth);
            if (StatusThisMonth === 3) {
                ClientStatusThisMonthHtml = '<div style="color:green;font-weight:bold;">Active</div>';
            }
            else if (StatusThisMonth === 4) {
                ClientStatusThisMonthHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
            }
            else {
                ClientStatusThisMonthHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
            } 

            var StatusNexrMonth = (data.StatusNexrMonth);
            if (StatusNexrMonth === 3) {
                ClientStatusNextMonthHtml = '<div style="color:green;font-weight:bold;">Active</div>';
            }
            else if (StatusNexrMonth === 4) {
                ClientStatusNextMonthHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
            }
            else {
                ClientStatusNextMonthHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
            } 


            if (_InformationUpdateForWhichPage == "allclients") {
                if (data.ClientDetails) {
                    var parseClient = (data.ClientDetails);
                    $("#tblUsers>tbody>tr").each(function () {


                        var index = $(this).index();
                        var employeeID = $(this).find("td:eq(0) input").val();
                        if (employeeID == parseClient.ClientDetailsID) {


                            if (parseClient.IsPriorityClient) {
                                $('#tblUsers tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                            }
                            else {
                                $('#tblUsers tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                            }

                            needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = index;
                            $('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseClient.Name);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseClient.LoginName);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(parseClient.PermanentDiscount);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(data.ThisMonthPackage);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(data.NextMonthPackage);

                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseClient.Address);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(parseClient.Email);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(9)").text(parseClient.ZoneName);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(10)").text(parseClient.ContactNumber);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(11)").html(parseClient.ProfileStatusUpdateInPercent);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(12)").html(ClientStatusThisMonthHtml);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(13)").html(ClientStatusNextMonthHtml);

                            var split = data.LineStatusActiveDate.split('<div');
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(14)").html(split[0]);

                        }
                    });
                }
                if (data.UpdateStatus === true) {

                    AppUtil.ShowSuccess("Successfully Edited.");
                    var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
                    var LineStatusID = AppUtil.GetIdValue("LineStatusID");
                    if (chkStatusFromRunningMonth == true && LineStatusID == 5) {
                        $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(16)").html('<button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"> <span class="glyphicon glyphicon-remove"></span> </button>');
                    }
                    if (chkStatusFromRunningMonth == true && LineStatusID == 3) {
                        $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(16)").html('');
                    }
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }
            } 

            if (_InformationUpdateForWhichPage == "accounts") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        if (data.TransactionID > 0) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(0)").html('<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(' + data.TransactionID + ')" type="checkbox" value="true"></a>');

                        }
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(3)").html(parseClientDetails.Address);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(4)").html(parseClientDetails.ContactNumber);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(5)").html(parseClientDetails.ZoneName);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(6)").html(data.ThisMonthPackage); 
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(data.packageChangeAmountCalculation);
                        } 
                        if (data.chkStatusFromRunningMonth === true) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(11)").html(ClientStatusThisMonthHtml);
                        } 

                        //$('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(17)").html(data.LineStatusActiveDate);
                    });
                     
                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "unpaidbill") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(4)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(5)").html(parseClientDetails.Address);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(6)").html(parseClientDetails.ContactNumber);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(7)").html(parseClientDetails.ZoneName);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(8)").html(data.ThisMonthPackage); 
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(9)").text(data.packageChangeAmountCalculation);
                        } 
                        if (data.chkStatusFromRunningMonth === true) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(13)").html(ClientStatusThisMonthHtml);
                        } 

                        //$('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(17)").html(data.LineStatusActiveDate);
                    });
                     
                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "signupbill") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(2)").html(parseClientDetails.Address);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(3)").html(parseClientDetails.ContactNumber);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(4)").html(parseClientDetails.ZoneName);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(5)").html(data.ThisMonthPackage);
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(data.packageChangeAmountCalculation);
                        }

                        //$('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(17)").html(data.LineStatusActiveDate);
                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "advancepayment") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');
                         
                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "complainlist") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(3)").html(parseClientDetails.Address);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(4)").html(parseClientDetails.ZoneName);
                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(5)").html(parseClientDetails.ContactNumber); 
                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "cabledistributionHistory") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(7)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');

                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }

            if (_InformationUpdateForWhichPage == "runninglist") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(7)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');

                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }
             
            if (_InformationUpdateForWhichPage == "totallist") {
                if (data.ClientDetails) {

                    var parseClientDetails = (data.ClientDetails);
                    $("#" + _tblName + ">tbody>tr").each(function () {

                        var index = _rowIndex;

                        if (parseClientDetails.IsPriorityClient) {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#' + _tblName + '>tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        $('#' + _tblName + '>tbody>tr:eq(' + index + ')').find("td:eq(10)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseClientDetails.ClientDetailsID + ',' + 0 + ')">' + parseClientDetails.LoginName + '</a>');

                    });

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Information Update Successfully.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when During Client Information Update. Please Contact with administrator.");
                }
            }
             // need to fix for total list

        }


        ClientUpdateManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");
        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);

        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    ClearClientDetailsModalInformation: function () {

        $("#Name").val("");
        $("#Email").val("");
        $("#LoginName").val("");
        $("#Password").val("");
        $("#Address").val("");
        $("#ContactNumber").val("");
        $("#ZoneID").val("");
        $("#SMSCommunication").val("");
        $("#Occupation").val("");
        $("#SocialCommunicationURL").val("");
        $("#Remarks").val("");
        $("#ConnectionTypeID").val("");
        $("#BoxNumber").val("");
        $("#PopDetails").val("");
        $("#RequireCable").val("");
        $("#CableTypeID").val("");
        $("#Reference").val("");
        $("#NationalID").val(""); 
        $("#PackageThisMonth").val("");
        $("#PackageNextMonth").val("");
        $("#SingUpFee").val("");                              //////
        $("#SecurityQuestionID").val("");
        $("#SecurityQuestionAnswer").val("");
        $("#MacAddress").val("");
        $("#BillPaymentDate").val("")//$('#BillPaymentDate').val("").datepicker('getDate').val("");
        $("#ClientSurvey").val("");
        $("#ConnectionDate").val(""); //('#ConnectionDate').val("").datepicker('getDate').val("");
         
        $("#ThisMonthLineStatusID").val("");
        $("#NextMonthLineStatusID").val("");
        $("#Reason").val("");


        $("#PreviewClientOWNImageBytesPaths").attr("src", "");
        $("#ClientOWNImageBytes").wrap('<form>').closest('form').get(0).reset();
        $("#ClientOWNImageBytes").unwrap();

        // Prevent form submission
        //$("#ClientOWNImageBytes").stopPropagation();
        //$("#ClientOWNImageBytes").preventDefault();

        $("#PreviewClientNIDImageBytesPaths").attr("src", "");
        $("#ClientNIDImage").wrap('<form>').closest('form').get(0).reset();
        $("#ClientNIDImage").unwrap();

        // Prevent form submission
        //$("#ClientNIDImage").stopPropagation();
        //$("#ClientNIDImage").preventDefault();

        //$("#ClientOWNImageBytes").val("");
        //$("#ClientNIDImage").val("");
    },
}