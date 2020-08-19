var ClientDetailsManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    Validation: function () {
        var status = true;
        if (AppUtil.GetIdValue("Name") === '') {

            //AppUtil.ShowErrorOnControl("Please Insert Name", "Name", "right");
            AppUtil.ShowErrorOnControl("Please Insert Name", "Name", "top center");
            //AppUtil.ShowSuccess("Please Insert Name.");
            //$("#Name").notify(
            //    "Please Insert Name.",20000,
            //    { position: "top" }
            //);
            status = false;
        }

        //if (AppUtil.GetIdValue("Email") === '') {
        //    AppUtil.ShowSuccess("Please Insert Email.");
        //    status =  false;
        //}
        if (AppUtil.GetIdValue("LoginName") === '') {
            //AppUtil.ShowSuccess("Please Insert LoginName.");
            AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("UserID") === '') {
            //AppUtil.ShowSuccess("Please Insert UserID.");  
            AppUtil.ShowErrorOnControl("Please Insert UserID.", "UserID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("Password") === '') {
            //AppUtil.ShowSuccess("Please Insert Password.");  
            AppUtil.ShowErrorOnControl("Please Insert Password.", "Password", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("Address") === '') {
            //AppUtil.ShowSuccess("Please Insert Address.");    
            AppUtil.ShowErrorOnControl("Please Insert Address.", "Address", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("LatitudeLongitude") === '') {
            //AppUtil.ShowSuccess("Please Insert Address.");    
            AppUtil.ShowErrorOnControl("Please Insert LatitudeLongitude.", "LatitudeLongitude", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("ContactNumber") === '') {
            //AppUtil.ShowSuccess("Please Insert Contact Number.");    
            AppUtil.ShowErrorOnControl("Please Insert Contact Number.", "ContactNumber", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("ZoneID") === '') {
            //AppUtil.ShowSuccess("Please Select Zone.");   
            AppUtil.ShowErrorOnControl("Please Insert Zone.", "ZoneID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("SMSCommunication") === '') {
            //AppUtil.ShowSuccess("Please Insert SMS Communication.");    
            AppUtil.ShowErrorOnControl("Please Insert SMS Communication.", "SMSCommunication", "top center");
            status = false;
        }
        //if (AppUtil.GetIdValue("Occupation") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Occupation.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Social Communication URL.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("Remarks") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Remarks.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
            //AppUtil.ShowSuccess("Please Select Connection Type.");    
            AppUtil.ShowErrorOnControl("Please Insert Connection Type.", "ConnectionTypeID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("BoxNumber") === '') {
            //AppUtil.ShowSuccess("Please Insert Box Number.");  
            AppUtil.ShowErrorOnControl("Please Insert Box Number.", "BoxNumber", "top center");
            status = false;
        }
        //if (AppUtil.GetIdValue("PopDetails") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Pop Details.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("RequireCable") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Require Cable.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("CableTypeID") === '') {
        //    //AppUtil.ShowSuccess("Please Select Cable Type.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("Reference") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Reference.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("NationalID") === '') {
        //    //AppUtil.ShowSuccess("Please Select National Id.");     AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        if (AppUtil.GetIdValue("PackageID") === '') {
            //AppUtil.ShowSuccess("Please Select Package.");     
            AppUtil.ShowErrorOnControl("Please Insert Package.", "PackageID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("SingUpFee") === '') {
            //AppUtil.ShowSuccess("Please Insert SignUp Fee.");    
            AppUtil.ShowErrorOnControl("Please Insert Sing Up Fee.", "SingUpFee", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("PermanentDiscount") === '') {
            //AppUtil.ShowSuccess("Please Insert SignUp Fee.");    
            AppUtil.ShowErrorOnControl("Please Add Permanent Discount.", "PermanentDiscount", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
            //AppUtil.ShowSuccess("Please Select Security Question.");  
            AppUtil.ShowErrorOnControl("Please Insert Security Question.", "SecurityQuestionID", "top center");
            status = false;
        }
        //if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Security Question Answer.");    AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        //if (AppUtil.GetIdValue("MacAddress") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Mac Address.");  AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        if (AppUtil.GetIdValue("BillPaymentDate") === '') {
            //AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");    
            AppUtil.ShowErrorOnControl("Bill payment Date must be between 1 and 31.", "BillPaymentDate", "top center");
            status = false;
        }
        //if (AppUtil.GetIdValue("ClientSurvey") === '') {
        //    //AppUtil.ShowSuccess("Please Insert Client Survey.");    AppUtil.ShowErrorOnControl("Please Insert LoginName.", "LoginName", "top center");
        //    status =  false;
        //}
        if (AppUtil.GetIdValue("ConnectionDate") === '') {
            //AppUtil.ShowSuccess("Please Insert Connection Date.");    
            AppUtil.ShowErrorOnControl("Please Insert Connection Date.", "ConnectionDate", "top center");
            status = false;
        }
        if (status == false) {
            return false;
        }
        else {
            return true;
        }



        ////if (AppUtil.GetIdValue("Name") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Name.");
        ////    return false;
        ////}

        //////if (AppUtil.GetIdValue("Email") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Email.");
        //////    return false;
        //////}
        ////if (AppUtil.GetIdValue("LoginName") === '') {
        ////    AppUtil.ShowSuccess("Please Insert LoginName.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("Password") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Password.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("Address") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Address.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("ContactNumber") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Contact Number.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SMSCommunication") === '') {
        ////    AppUtil.ShowSuccess("Please Insert SMS Communication Number.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("ZoneID") === '') {
        ////    AppUtil.ShowSuccess("Please Select Zone.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SMSCommunication") === '') {
        ////    AppUtil.ShowSuccess("Please Insert SMS Communication.");
        ////    return false;
        ////}
        //////if (AppUtil.GetIdValue("Occupation") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Occupation.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("Remarks") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Remarks.");
        //////    return false;
        //////}
        ////if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
        ////    AppUtil.ShowSuccess("Please Select Connection Type.");
        ////    return false;
        ////}
        //////if (AppUtil.GetIdValue("BoxNumber") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Box Number.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("PopDetails") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Pop Details.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("RequireCable") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Require Cable.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("CableTypeID") === '') {
        //////    AppUtil.ShowSuccess("Please Select Cable Type.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("Reference") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Reference.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("NationalID") === '') {
        //////    AppUtil.ShowSuccess("Please Select National Id.");
        //////    return false;
        //////}
        ////if (AppUtil.GetIdValue("PackageID") === '') {
        ////    AppUtil.ShowSuccess("Please Select Package.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SingUpFee") === '') {
        ////    AppUtil.ShowSuccess("Please Insert SignUp Fee.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
        ////    AppUtil.ShowSuccess("Please Select Security Question.");
        ////    return false;
        ////}
        //////if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
        //////    return false;
        //////}
        //////if (AppUtil.GetIdValue("MacAddress") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Mac Address.");
        //////    return false;
        //////}
        ////if (AppUtil.GetIdValue("BillPaymentDate") === '') {
        ////    AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
        ////    return false;
        ////}
        //////if (AppUtil.GetIdValue("ClientSurvey") === '') {
        //////    AppUtil.ShowSuccess("Please Insert Client Survey.");
        //////    return false;
        //////}
        ////if (AppUtil.GetIdValue("ConnectionDate") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Connection Date.");
        ////    return false;
        ////}

        ////return true;

    },
    CableAddInListValidation: function () {

        if (AppUtil.GetIdValue("CableTypePopUpID") === '') {
            AppUtil.ShowSuccess("Please Select Cable Type.");
            return false;
        }

        if (AppUtil.GetIdValue("CableStockID") === '') {
            AppUtil.ShowSuccess("Please Select Box/Drum Number.");
            return false;
        }
        if (AppUtil.GetIdValue("lstEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Employee Name.");
            return false;
        }
        if (AppUtil.GetIdValue("txtCableQuantity") === '') {
            AppUtil.ShowSuccess("Please Enter Cable Length.");
            return false;
        }
        else {
            var cableAssignToClient = AppUtil.GetIdValue("txtCableQuantity");
            if (cableAssignToClient > cableCanBeUseForThisClientFromDB) {
                AppUtil.ShowError(" Cable Length Can Not Greater than " + cableCanBeUseForThisClientFromDB + " M");
                return false;
            }
        }

        return true;
    },

    SMSSendValidations: function () {


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

    SendMessageToClient: function (isCheckAll, ifIsCheckAllThenNonCheckList, ifNotCheckAllThenCheckList) {


        var sms = $("#txtSMSText").val();
        var UserType = $("#SearchByClientType").val();
        var ZoneID = $("#SearchByZoneID").val();
        var url = "/SMS/SendSMSToClient/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ message: sms ,UserType : UserType , ZoneID : ZoneID, IsCheckAll: isCheckAll, IfIsCheckAllThenNonCheckList: ifIsCheckAllThenNonCheckList, IfNotCheckAllThenCheckList: ifNotCheckAllThenCheckList, __RequestVerificationToken: AntiForgeryToken, ForWhichTypeSMSIsThat: 1 });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SendMessageToClientSuccess, ClientDetailsManager.SendMessageToClientError);
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

    ValidationForAddItemInListForAssigningToCustomer: function () {

        if (AppUtil.GetIdValue("lstEmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Employee.");
            return false;
        }
        if (AppUtil.GetIdValue("lstStockID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }
        if (AppUtil.GetIdValue("StockDetailsID") === '') {
            AppUtil.ShowSuccess("Please Select Serial.");
            return false;
        }

        return true;

    },

    GetStockDetailsItemListByStockID: function (StockID) {

        var url = "/Stock/GetStockDetailsItemListByStockID/";
        var data = ({ StockID: StockID });
        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.GetStockDetailsItemListByStockIDSuccess, ClientDetailsManager.GetStockDetailsItemListByStockIDFailed);
    },
    GetStockDetailsItemListByStockIDSuccess: function (data) {

        console.log(data);
        $("#lstStockDetailsID").find("option").not(":first").remove();
        $.each((data.lstStockDetails), function (index, item) {
            $("#lstStockDetailsID").append($("<option></option>").val(item.StockDetailsID).text(item.Serial));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    GetStockDetailsItemListByStockIDFailed: function (data) {

        console.log(data);
        alert("Fail");
    },

    LineStatusChangeHistoryByClientDetailsID: function (SearchID) {
        var url = "/Client/LineStatusChangeHistoryByClientDetailsID/";
        var data = ({ SearchID: SearchID });
        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.LineStatusChangeHistoryByClientDetailsIDSuccess, ClientDetailsManager.LineStatusChangeHistoryByClientDetailsIDFailed);
    },
    LineStatusChangeHistoryByClientDetailsIDSuccess: function (data) {

        console.log(data);
        if (data.Success === true) {

            $("#tblClientListFromLineStatus>tbody").empty();

            $("#txtName").val(data.lstClientLineStatus[0].Name);
            $("#txtZone").val(data.lstClientLineStatus[0].Zone);
            $("#txtMobile").val(data.lstClientLineStatus[0].Contact);
            $("#txtAddress").val(data.lstClientLineStatus[0].Address);

            $.each(data.lstClientLineStatus, function (index, item) {


                //Name = s.ClientDetails.Name,
                //              LoginName = s.ClientDetails.LoginName,
                //              Package = s.Package.PackageName,
                //              Address = s.ClientDetails.Address,
                //              Zone = s.ClientDetails.Zone.ZoneName,
                //              Contact = s.ClientDetails.ContactNumber,
                //              Status = s.LineStatus.LineStatusName,
                //              Employee = s.Employee.Name,
                //              Time = s.LineStatusChangeDate

                var status = "";
                if (item.Status == 3) {
                    status = "<div id='status' class='label label-success'>Active</div>"
                }
                if (item.Status == 4) {
                    status = "<div id='status' class='label label-warning'>In Active</div>"
                }
                if (item.Status == 5) {
                    status = "<div id='status' class='label label-danger'>Lock</div>"
                }

                $("#tblClientListFromLineStatus>tbody").append("<tr><td>" + item.Name + "</td><td>" + item.LoginName + "</td><td>" + item.Package + "</td><td>" + item.Address + "</td>\
                                <td>" + item.Zone + "</td><td>" + item.Contact + "</td><td>" + status + "</td><td>" + item.Employee + "</td><td>" + item.Reseller + "</td>\
                                <td>" + AppUtil.ParseDateTime(item.Time) + "</td><td>" + item.Reason + "</td></tr>");
            });
        }
        $.each((data.lstStockDetails), function (index, item) {
            $("#lstStockDetailsID").append($("<option></option>").val(item.StockDetailsID).text(item.Serial));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    LineStatusChangeHistoryByClientDetailsIDFailed: function (data) {

        console.log(data);
        alert("Fail");
    },


    ValidationForSearchActiveToLockOrLockToActive: function () {

        if (AppUtil.GetIdValue("YearID") === '') {
            AppUtil.ShowSuccess("Please Select Year.");
            return false;
        }
        if (AppUtil.GetIdValue("MonthID") === '') {
            AppUtil.ShowSuccess("Please Select Month.");
            return false;
        }
        return true;

    },

    //UpdateClientDetailsValidation: function () {

    //    ////if (AppUtil.GetIdValue("Name") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Name.");
    //    ////    return false;
    //    ////}

    //    //////if (AppUtil.GetIdValue("Email") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Email.");
    //    //////    return false;
    //    //////}
    //    ////if (AppUtil.GetIdValue("LoginName") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert LoginName.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("Password") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Password.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("Address") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Address.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("ContactNumber") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Contact Number.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("ZoneID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Zone.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("SMSCommunication") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert SMS Communication.");
    //    ////    return false;
    //    ////}
    //    //////if (AppUtil.GetIdValue("Occupation") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Occupation.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("Remarks") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Remarks.");
    //    //////    return false;
    //    //////}
    //    ////if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Connection Type.");
    //    ////    return false;
    //    ////}
    //    //////if (AppUtil.GetIdValue("BoxNumber") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Box Number.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("PopDetails") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Pop Details.");
    //    //////    return false;
    //    //////}
    //    ////if (AppUtil.GetIdValue("RequireCable") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Require Cable.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("CableTypeID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Cable Type.");
    //    ////    return false;
    //    ////}
    //    //////if (AppUtil.GetIdValue("Reference") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Reference.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("NationalID") === '') {
    //    //////    AppUtil.ShowSuccess("Please Select National Id.");
    //    //////    return false;
    //    //////}
    //    ////if (AppUtil.GetIdValue("PackageID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Package.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("SingUpFee") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert SignUp Fee.");
    //    ////    return false;
    //    ////}
    //    ////if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Security Question.");
    //    ////    return false;
    //    ////}
    //    //////if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("MacAddress") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Mac Address.");
    //    //////    return false;
    //    //////}
    //    //////if (AppUtil.GetIdValue("BillPaymentDate") === '') {   
    //    ////if (AppUtil.GetIdValue("BillPaymentDate") < 1 || AppUtil.GetIdValue("BillPaymentDate") > 31) {
    //    ////    AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
    //    ////    return false;
    //    ////}
    //    //////if (AppUtil.GetIdValue("ClientSurvey") === '') {
    //    //////    AppUtil.ShowSuccess("Please Insert Client Survey.");
    //    //////    return false;
    //    //////}
    //    ////if (AppUtil.GetIdValue("ConnectionDate") === '') {
    //    ////    AppUtil.ShowSuccess("Please Insert Connection Date.");
    //    ////    return false;
    //    ////}

    //    ////if (AppUtil.GetIdValue("LineStatusID") === '') {
    //    ////    AppUtil.ShowSuccess("Please Select Line Status.");
    //    ////    return false;
    //    ////}
    //    //////var a = AppUtil.GetIdValue("LineStatusActiveDate");
    //    ////if (AppUtil.GetIdValue("LineStatusActiveDate") === '' || AppUtil.GetIdValue("LineStatusActiveDate") == 'Invalid date') {
    //    ////    AppUtil.ShowSuccess("Please Insert Line Status Active Date.");
    //    ////    return false;
    //    ////}



    //    ////return true;
    //    if (AppUtil.GetIdValue("Name") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Name.");
    //        AppUtil.ShowErrorOnControl("Please Insert Name.", "Name", "top center");
    //        return false;
    //    }

    //    //if (AppUtil.GetIdValue("Email") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Email.");
    //    //    return false;
    //    //}
    //    if (AppUtil.GetIdValue("LoginName") === '') {
    //        //AppUtil.ShowSuccess("Please Insert LoginName.");
    //        AppUtil.ShowErrorOnControl("Please Insert Login Name.", "LoginName", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("UserID") === '') {
    //        //AppUtil.ShowSuccess("Please Insert UserID.");
    //        AppUtil.ShowErrorOnControl("Please Insert UserID.", "UserID", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("Password") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Password.");
    //        AppUtil.ShowErrorOnControl("Please Insert Password.", "Password", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("Address") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Address.");
    //        AppUtil.ShowErrorOnControl("Please Insert Address.", "Address", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("ContactNumber") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Contact Number.");
    //        AppUtil.ShowErrorOnControl("Please Insert Contact Number.", "ContactNumber", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("ZoneID") === '') {
    //        //AppUtil.ShowSuccess("Please Select Zone.");
    //        AppUtil.ShowErrorOnControl("Please Select Zone.", "ZoneID", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("SMSCommunication") === '') {
    //        //AppUtil.ShowSuccess("Please Insert SMS Communication.");
    //        AppUtil.ShowErrorOnControl("Please Insert SMS Communication.", "SMSCommunication", "top center");
    //        return false;
    //    }
    //    //if (AppUtil.GetIdValue("Occupation") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Occupation.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("Remarks") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Remarks.");
    //    //    return false;
    //    //}
    //    if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
    //        //AppUtil.ShowSuccess("Please Select Connection Type.");
    //        AppUtil.ShowErrorOnControl("Please Select Connection Type.", "ConnectionTypeID", "top center");
    //        return false;
    //    }
    //    //if (AppUtil.GetIdValue("BoxNumber") === '') {
    //    //    //AppUtil.ShowSuccess("Please Insert Box Number.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("PopDetails") === '') {
    //    //    //AppUtil.ShowSuccess("Please Insert Pop Details.");
    //    //    return false;
    //    //}
    //    if (AppUtil.GetIdValue("RequireCable") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Require Cable.");
    //        AppUtil.ShowErrorOnControl("Please Insert Require Cable.", "RequireCable", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("CableTypeID") === '') {
    //        //AppUtil.ShowSuccess("Please Select Cable Type.");
    //        AppUtil.ShowErrorOnControl("Please Select Cable Type.", "CableTypeID", "top center");
    //        return false;
    //    }
    //    //if (AppUtil.GetIdValue("Reference") === '') {
    //    //    //AppUtil.ShowSuccess("Please Insert Reference.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("NationalID") === '') {
    //    //    //AppUtil.ShowSuccess("Please Select National Id.");
    //    //    return false;
    //    //}


    //    ////if (AppUtil.GetIdValue("PackageID") === '') {
    //    ////    //AppUtil.ShowSuccess("Please Select Package.");
    //    ////    AppUtil.ShowErrorOnControl("Please Select Package.", "PackageID", "top center");
    //    ////    return false;
    //    ////}


    //    if (AppUtil.GetIdValue("PackageThisMonth") === '' || AppUtil.GetIdValue("PackageThisMonth") === null) {
    //        //AppUtil.ShowSuccess("Please Select Package.");
    //        AppUtil.ShowErrorOnControl("Please Select This Month Package.", "PackageThisMonth", "top center");
    //        return false;
    //    }
    //    //var a = AppUtil.GetIdValue("PackageNextMonth");
    //    if (AppUtil.GetIdValue("PackageNextMonth") === '' || AppUtil.GetIdValue("PackageNextMonth") === null) {
    //        //AppUtil.ShowSuccess("Please Select Package.");
    //        AppUtil.ShowErrorOnControl("Please Select Next Month Package.", "PackageNextMonth", "top center");
    //        return false;
    //    }


    //    if (AppUtil.GetIdValue("SingUpFee") === '') {
    //        //AppUtil.ShowSuccess("Please Insert SignUp Fee.");
    //        AppUtil.ShowErrorOnControl("Please Insert SignUp Fee.", "SingUpFee", "top center");
    //        return false;
    //    }
    //    if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
    //        //AppUtil.ShowSuccess("Please Select Security Question.");
    //        AppUtil.ShowErrorOnControl("Please Select Security Question.", "SecurityQuestionID", "top center");
    //        return false;
    //    }
    //    //if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("MacAddress") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Mac Address.");
    //    //    return false;
    //    //}
    //    //if (AppUtil.GetIdValue("BillPaymentDate") === '') {   
    //    if (AppUtil.GetIdValue("BillPaymentDate") < 1 || AppUtil.GetIdValue("BillPaymentDate") > 31) {
    //        //AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
    //        AppUtil.ShowErrorOnControl("Bill payment Date must be between 1 and 31.", "BillPaymentDate", "top center");
    //        return false;
    //    }
    //    //if (AppUtil.GetIdValue("ClientSurvey") === '') {
    //    //    AppUtil.ShowSuccess("Please Insert Client Survey.");
    //    //    return false;
    //    //}
    //    if (AppUtil.GetIdValue("ConnectionDate") === '') {
    //        //AppUtil.ShowSuccess("Please Insert Connection Date.");
    //        AppUtil.ShowErrorOnControl("Please Insert Connection Date.", "ConnectionDate", "top center");
    //        return false;
    //    }

    //    if (AppUtil.GetIdValue("LineStatusID") === '') {
    //        //AppUtil.ShowSuccess("Please Select Line Status.");
    //        AppUtil.ShowErrorOnControl("Please Select Line Status.", "LineStatusID", "top center");
    //        return false;
    //    }
    //    //var a = AppUtil.GetIdValue("LineStatusActiveDate");
    //    if (AppUtil.GetIdValue("LineStatusActiveDate") === '' || AppUtil.GetIdValue("LineStatusActiveDate") == 'Invalid date') {
    //        //AppUtil.ShowSuccess("Please Insert Line Status Active Date.");
    //        AppUtil.ShowErrorOnControl("Please Insert Line Status Active Date.", "LineStatusActiveDate", "top center");
    //        return false;
    //    }


    //    return true;
    //},

    UpdateClientDetailsValidationDashBoard: function () {

        if (AppUtil.GetIdValue("ClientDetails_Name") === '') {
            AppUtil.ShowSuccess("Please Insert Name.");
            return false;
        }

        //if (AppUtil.GetIdValue("ClientDetails_Email") === '') {
        //    AppUtil.ShowSuccess("Please Insert Email.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ClientDetails_LoginName") === '') {
            AppUtil.ShowSuccess("Please Insert LoginName.");
            return false;
        }
        if (AppUtil.GetIdValue("Password") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("ClientDetails_Address") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("ClientDetails_ContactNumber") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Number.");
            return false;
        }
        if (AppUtil.GetIdValue("ZoneID") === '') {
            AppUtil.ShowSuccess("Please Select Zone.");
            return false;
        }
        if (AppUtil.GetIdValue("ClientDetails_SMSCommunication") === '') {
            AppUtil.ShowSuccess("Please Insert SMS Communication.");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientDetails_Occupation") === '') {
        //    AppUtil.ShowSuccess("Please Insert Occupation.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ClientDetails_SocialCommunicationURL") === '') {
        //    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ClientDetails_Remarks") === '') {
        //    AppUtil.ShowSuccess("Please Insert Remarks.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ClientDetails_ConnectionTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Connection Type.");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientDetails_BoxNumber") === '') {
        //    AppUtil.ShowSuccess("Please Insert Box Number.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ClientDetails_PopDetails") === '') {
        //    AppUtil.ShowSuccess("Please Insert Pop Details.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ClientDetails_RequireCable") === '') {
            AppUtil.ShowSuccess("Please Insert Require Cable.");
            return false;
        }
        if (AppUtil.GetIdValue("ClientDetails_CableTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Cable Type.");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientDetails_Reference") === '') {
        //    AppUtil.ShowSuccess("Please Insert Reference.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ClientDetails_NationalID") === '') {
        //    AppUtil.ShowSuccess("Please Select National Id.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("PackageID") === '') {
            AppUtil.ShowSuccess("Please Select Package.");
            return false;
        }
        if (AppUtil.GetIdValue("SingUpFee") === '') {
            AppUtil.ShowSuccess("Please Insert SignUp Fee.");
            return false;
        }
        if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
            AppUtil.ShowSuccess("Please Select Security Question.");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientDetails_SecurityQuestionAnswer") === '') {
        //    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ClientDetails_MacAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Mac Address.");
            return false;
        }
        if (AppUtil.GetIdValue("BillPaymentDate") === '') {
            AppUtil.ShowSuccess("Please Insert Payment Date.");
            return false;
        }
        //if (AppUtil.GetIdValue("ClientDetails_ClientSurvey") === '') {
        //    AppUtil.ShowSuccess("Please Insert Client Survey.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("ClientDetails_ConnectionDate") === '') {
            AppUtil.ShowSuccess("Please Insert Connection Date.");
            return false;
        }

        if (AppUtil.GetIdValue("LineStatusID") === '') {
            AppUtil.ShowSuccess("Please Select Line Status.");
            return false;
        }

        if (AppUtil.GetIdValue("StatusChangeReason") === '') {
            AppUtil.ShowSuccess("Please Select Line Status change reason.");
            return false;
        }


        return true;
    },

    PrintEmployeeList: function () {

        var url = "/Excel/CreateReportForAllClient";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var data = ({ ZoneID: ZoneID });
        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.PrintEmployeeListSuccess, ClientDetailsManager.PrintEmployeeListFail);
    },
    PrintEmployeeListSuccess: function (data) {

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
    PrintEmployeeListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    //GetClientDetailsByID: function (id) {
    //    //AppUtil.ShowWaitingDialog();
    //    //code before the pause
    //    //  setTimeout(function () {
    //    var url = "/CLient/GetClientDetailsByID/";
    //    var Data = ({ ClientDetailsID: id });
    //    Data = ClientDetailsManager.addRequestVerificationToken(Data);

    //    AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientDetailsManager.GetClientDetailsByIDSuccess, ClientDetailsManager.GetClientDetailsByIDError);
    //    //  }, 500);



    //},
    //GetClientDetailsByIDSuccess: function (data) {


    //    //var ClientDetails = JSON.parse(data.ClientLineStatus);
    //    //var Transaction = (data.Transaction);

    //    var ClientDetails = (data.ClientLineStatus);
    //    var Transaction = (data.Transaction);
    //    var cableDetails = data.CableForThisClient;
    //    var itemsDetails = data.ItemForThisClient;

    //    console.log("Transaction: " + Transaction);
    //    console.log("ClientLineStatus: " + ClientDetails);

    //    ClientDetailsID = ClientDetails.ClientDetailsID;
    //    ClientLineStatusID = ClientDetails.ClientLineStatusID;
    //    // ClientBannedStatusID;
    //    ClientTransactionID = Transaction[0].TransactionID;

    //    var itemList = "";
    //    $.each(cableDetails, function (index, item) {
    //        $.each(item, function (index, item) {
    //            itemList += "<b> Cable:&nbsp;&nbsp;" + item.CableType + " " + item.AmountOfCableGiven + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br /> ";
    //        });
    //    });
    //    if (itemList.length > 0) {
    //        itemList += "<br />";
    //    }
    //    $.each(itemsDetails, function (index, item) {
    //        $.each(item, function (index, item) {
    //            itemList += "<b> Item Name:&nbsp;&nbsp;" + item.ItemName + " &nbsp;&nbsp;&nbsp;&nbsp;Serial:&nbsp;&nbsp;" + item.ItemSerial + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</b> ";
    //        });
    //    });
    //    //itemList += "</div>";

    //    //if (ClientDetails.ClientOWNImageBytesPaths != null && ClientDetails.ClientOWNImageBytesPaths != "") {

    //    $("#PreviewClientOWNImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientOWNImageBytesPaths + '#' + new Date().getTime()).show(0);//.attr("src", ClientDetails.ClientOWNImageBytesPaths);
    //    $("#PreviewClientOWNImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientOWNImageBytesPaths')");
    //    //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
    //    $("#ClientOWNImageBytesPaths").attr("value", "" + ClientDetails.ClientOWNImageBytesPaths + "");
    //    //$("#ClientOwnImagePath").val(ClientDetails.ClientOwnImageBytesPaths);
    //    //}

    //    $("#divPreviewClientNIDImageBytesPaths").html("");
    //    $("#divPreviewClientNIDImageBytesPaths").html('<img id="PreviewClientNIDImageBytesPaths" src="" width="100" height="90" onclick="">');
    //    //$("#PreviewClientNIDImageBytesPaths").hide(0).attr('src', '' + ClientDetails.ClientNIDImageBytesPaths + '').show(0);
    //    $("#PreviewClientNIDImageBytesPaths").hide(0).attr("src", "" + ClientDetails.ClientNIDImageBytesPaths + "#" + new Date().getTime()).show(0);
    //    $("#PreviewClientNIDImageBytesPaths").attr("onclick", "ImageManager.ShowLargeImage('PreviewClientNIDImageBytesPaths')");
    //    //ImageManager.ShowLargeImage('PreviewClientOwnImageBytesPaths')
    //    $("#ClientNIDImageBytesPaths").attr("value", "" + ClientDetails.ClientNIDImageBytesPaths + "");
    //    //}

    //    $("#txtItemAndCablesAssign").html(itemList);
    //    $("#Name").val(ClientDetails.Name);
    //    $("#Email").val(ClientDetails.Email);
    //    $("#LoginName").val(ClientDetails.LoginName);
    //    $("#Password").val(ClientDetails.Password);
    //    $("#Address").val(ClientDetails.Address);
    //    $("#ContactNumber").val(ClientDetails.ContactNumber);
    //    $("#ZoneID").val(ClientDetails.ZoneID);
    //    $("#SMSCommunication").val(ClientDetails.SMSCommunication);
    //    $("#Occupation").val(ClientDetails.Occupation);
    //    $("#SocialCommunicationURL").val(ClientDetails.SocialCommunicationURL);
    //    $("#Remarks").val(ClientDetails.Remarks);
    //    $("#ConnectionTypeID").val(ClientDetails.ConnectionTypeID);
    //    $("#BoxID").val(ClientDetails.BoxNumber);
    //    $("#PopDetails").val(ClientDetails.PopDetails);
    //    $("#RequireCable").val(ClientDetails.RequireCable);
    //    $("#CableTypeID").val(ClientDetails.CableTypeID);
    //    $("#Reference").val(ClientDetails.Reference);
    //    $("#NationalID").val(ClientDetails.NationalID);

    //    //$("#PackageID").val(ClientDetails.PackageID);
    //    $("#PackageThisMonth").val(ClientDetails.PackageThisMonth);
    //    $("#PackageNextMonth").val(ClientDetails.PackageNextMonth);

    //    $("#SingUpFee").val(Transaction[0].PaymentAmount);
    //    $("#SecurityQuestionID").val(ClientDetails.SecurityQuestionID);
    //    $("#SecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
    //    $("#MacAddress").val(ClientDetails.MacAddress);
    //    //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
    //    //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
    //    $("#BillPaymentDate").val(ClientDetails.PaymentDate);
    //    $("#ClientSurvey").val(ClientDetails.ClientSurvey);
    //    $("#ConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
    //    $("#LineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

    //    // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
    //    $("#ThisMonthLineStatusID").val(ClientDetails.ThisMonthLineStatusID);
    //    $("#NextMonthLineStatusID").val(ClientDetails.NextMonthLineStatusID);
    //    //$("#LineStatusID").val(ClientDetails.LineStatusID);
    //    $("#Reason").val(ClientDetails.StatusChangeReason);

    //    $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

    //    $("#lstMikrotik").val(ClientDetails.MikrotikID);
    //    $("#IP").val(ClientDetails.IP);
    //    $("#Mac").val(ClientDetails.Mac);

    //    $("#ResellerID").val(ClientDetails.ResellerID);
    //    $("#divProfileUpdatePercentage").html(ClientDetails.ProfileStatusUpdateInPercent);
    //    //AppUtil.HideWaitingDialog();
    //    $("#tblEmployeeDetails").modal("show");

    //    //AppUtil.ShowSuccess("Success");
    //},
    //GetClientDetailsByIDError: function (data) {

    //    //AppUtil.HideWaitingDialog();

    //    AppUtil.ShowSuccess("Fail");
    //    console.log(data);
    //},

    ShowProfileListDoneOrNot: function (cdid) {
        var url = "/CLient/GetProfileUpdatePointsBycdid/";
        var Data = ({ cdid: cdid });
        Data = ClientDetailsManager.addRequestVerificationToken(Data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientDetailsManager.ShowProfileListDoneOrNotSuccess, ClientDetailsManager.ShowProfileListDoneOrNotError);
    },
    ShowProfileListDoneOrNotSuccess: function (data) {
        var lstFields = (data.lstCustomProfilePercentageFields);
        $.each(lstFields, function (index, item) {
            $("#tblShowProfileUpdatePoints>tbody").append("<tr><td>" + item.CheckBoxDoneOrNot + "</td><td>" + item.FieldsName + "</td></tr>")
        });
        $("#mdlShowProfileUpdatePoints").modal("show");
    },
    ShowProfileListDoneOrNotError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    InsertClientDetails: function (itemAssignArray, cableAssignArray) {

        var url = "/Client/InsertClientDetails/";

        //var formData = new FormData();
        //formData.append('file', $('#ClientNIDImage')[0].files[0]);

        //var NIDImage = $("#ClientNIDImage").get(0).files[0];
        //var formData = new FormData();
        ////formData.set("file", NIDImage, NIDImage.name);
        //formData.append('file', NIDImage);

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
        //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee");
        var PermanentDiscount = AppUtil.GetIdValue("PermanentDiscount");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        //var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');

        var ResellerID = AppUtil.GetIdValue("ResellerID");

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");


        var StatusThisMonth = 3;
        var StatusNextMonth = 3;

        var PackageThisMonth = AppUtil.GetIdValue("PackageID");
        var PackageNextMonth = AppUtil.GetIdValue("PackageID");

        var ClientDetails = [];
        ClientDetails.push({
            Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate,
            MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate,
            ResellerID: ResellerID
            , StatusThisMonth: StatusThisMonth, StatusNextMonth: StatusNextMonth
            , PackageThisMonth: PackageThisMonth, PackageNextMonth: PackageNextMonth
            , PermanentDiscount: PermanentDiscount,
            LatitudeLongitude: LatitudeLongitude
        });
        //   var Mikrotik = { MikrotikID: MikrotikID, IP: IP, Mac: Mac };
        var Transaction = { /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee, PermanentDiscount: PermanentDiscount };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        //var formData = new FormData();
        //formData.append('file', $('#f_UploadImage')[0].files[0]);

        //var file = $("#imguploader").get(0).files[0];
        //var formData = new FormData();
        //formData.set("file", file, file.name);

        var formData = new FormData();
        formData.append('ClientOwnImageBytes', $('#ClientOwnImageBytes')[0].files[0]);
        formData.append('ClientNIDImage', $('#ClientNIDImage')[0].files[0]);
        formData.append('ClientDetails', JSON.stringify(ClientDetails));
        formData.append('Transaction', JSON.stringify(Transaction));
        formData.append('ItemListForEmployee', JSON.stringify(itemAssignArray));
        formData.append('ClientCableAssign', JSON.stringify(cableAssignArray));
        //$.ajax({
        //    url: url,
        //    method: "post",
        //    data: formData,
        //    cache: false,
        //    contentType: false,
        //    processData: false
        //})
        //    .then(function (result) {

        //    });


        var datas = JSON.stringify({ ClientDetails: ClientDetails, Transaction: Transaction, ItemListForEmployee: itemAssignArray, ClientCableAssign: cableAssignArray });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.InsertClientDetailsSuccess, ClientDetailsManager.InsertClientDetailsFail);
    },
    InsertClientDetailsSuccess: function (data) {

        if (data.Success === false) {
            //   DuplicateCableStockID = , CableBoxName = , LenghtGreaterThanCableAmount = , GreaterBoxNameList = 
            if (data.MikrotikConnectionFailed === true) {
                AppUtil.ShowSuccess("Sorry Prolem Occoured in Mikrotik COnnection. Please Make Sure Mikrotik is Online and ID and Password is correct.");
            }
            if (data.AlreadyAddedLoginNameInMikrotik === true) {
                AppUtil.ShowSuccess("Login Name Already Used in Mikrotik. Please Choose Different One.");
            }
            if (data.LoginNameExist === true) {
                AppUtil.ShowSuccess("Login Name Already Used. Please Choose Different One.");
            }
            if (data.UserAddInMikrotik === false) {
                AppUtil.ShowError(data.Message);
            }

            if (data.SerialAlreadyAdded === true) {
                AppUtil.ShowError(data.SerialList + " Already Using Some Purpous.");
            }
            if (data.DuplicateCableStockID === true) {
                AppUtil.ShowError(data.CableBoxName + " are added in duplicate times.");
            }
            if (data.LenghtGreaterThanCableAmount === true) {
                AppUtil.ShowError(data.GreaterBoxNameList + " For Those Box you assign length of cable which is greater than the actual amount.");
            }
        }

        console.log(data);
        if (data.SuccessInsert === true) {
            itemAssignArray = [];
            cableAssignArray = [];
            $("#tblItemList>tbody>tr").remove();
            $("#tblCableList>tbody>tr").remove();
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.href = "/Client/GetAllCLients";
        }
        if (data.SuccessInsert === false) {


            AppUtil.ShowSuccess("Saved Failed.");

        }
    },
    InsertClientDetailsFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },


    InsertResellerClientDetails: function () {

        var url = "/Client/InsertResellerClientDetails/";
        var Name = AppUtil.GetIdValue("Name");
        var Email = AppUtil.GetIdValue("Email");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("Address");
        var ContactNumber = AppUtil.GetIdValue("ContactNumber");
        var ZoneID = AppUtil.GetIdValue("ZoneID");
        var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
        var Occupation = AppUtil.GetIdValue("Occupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("Remarks");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("BoxID");
        //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee");
        var PermanentDiscount = AppUtil.GetIdValue("PermanentDiscount");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        //var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');

        var ResellerID = AppUtil.GetIdValue("ResellerID");

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");


        var StatusThisMonth = 3;
        var StatusNextMonth = 3;

        var PackageThisMonth = AppUtil.GetIdValue("PackageID");
        var PackageNextMonth = AppUtil.GetIdValue("PackageID");

        var ClientDetails = [];
        ClientDetails.push({
            Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate,
            MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate,
            ResellerID: ResellerID
            , StatusThisMonth: StatusThisMonth, StatusNextMonth: StatusNextMonth
            , PackageThisMonth: PackageThisMonth, PackageNextMonth: PackageNextMonth, PermanentDiscount: PermanentDiscount
        });
        //   var Mikrotik = { MikrotikID: MikrotikID, IP: IP, Mac: Mac };
        var Transaction = { /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee, PermanentDiscount: PermanentDiscount };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var formData = new FormData();
        formData.append('ClientOwnImageBytes', $('#ClientOwnImageBytes')[0].files[0]);
        formData.append('ClientNIDImage', $('#ClientNIDImage')[0].files[0]);
        formData.append('ClientDetails', JSON.stringify(ClientDetails));
        formData.append('Transaction', JSON.stringify(Transaction));

        var datas = JSON.stringify({ ClientDetails: ClientDetails, Transaction: Transaction });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.InsertResellerClientDetailsSuccess, ClientDetailsManager.InsertResellerClientDetailsFail);
    },
    InsertResellerClientDetailsSuccess: function (data) {

        if (data.Success === false) {
            if (data.ResellerBalanceLow === true) {
                AppUtil.ShowSuccess("Sorry you have no balance. Recharge First.");
            }
            if (data.ResellerPermanentDiscountIsLessThenPackagepriceGivenByaAdmin) {
                AppUtil.ShowSuccess("Discount Amount Can Not Less Then Package Price.");
            }
            //   DuplicateCableStockID = , CableBoxName = , LenghtGreaterThanCableAmount = , GreaterBoxNameList = 
            if (data.MikrotikConnectionFailed === true) {
                AppUtil.ShowSuccess("Sorry Prolem Occoured in Mikrotik COnnection. Please Make Sure Mikrotik is Online and ID and Password is correct.");
            }
            if (data.AlreadyAddedLoginNameInMikrotik === true) {
                AppUtil.ShowSuccess("Login Name Already Used in Mikrotik. Please Choose Different One.");
            }
            if (data.LoginNameExist === true) {
                AppUtil.ShowSuccess("Login Name Already Used. Please Choose Different One.");
            }
            if (data.UserAddInMikrotik === false) {
                AppUtil.ShowError(data.Message);
            }

            if (data.SerialAlreadyAdded === true) {
                AppUtil.ShowError(data.SerialList + " Already Using Some Purpous.");
            }
            if (data.DuplicateCableStockID === true) {
                AppUtil.ShowError(data.CableBoxName + " are added in duplicate times.");
            }
            if (data.LenghtGreaterThanCableAmount === true) {
                AppUtil.ShowError(data.GreaterBoxNameList + " For Those Box you assign length of cable which is greater than the actual amount.");
            }
        }

        console.log(data);
        if (data.SuccessInsert === true) {
            itemAssignArray = [];
            cableAssignArray = [];
            $("#tblItemList>tbody>tr").remove();
            $("#tblCableList>tbody>tr").remove();
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.href = "/Client/GetAllResellerClients";
        }
        if (data.SuccessInsert === false) {


            AppUtil.ShowSuccess("Saved Failed.");

        }
    },
    InsertResellerClientDetailsFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },


    InsertResellerClientDetailsByAdmin: function () {

        var url = "/Client/InsertResellerClientDetailsByAdmin/"; 
        var Name = AppUtil.GetIdValue("Name");
        var Email = AppUtil.GetIdValue("Email");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("Address");
        var ContactNumber = AppUtil.GetIdValue("ContactNumber");
        var ZoneID = AppUtil.GetIdValue("ZoneID");
        var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
        var Occupation = AppUtil.GetIdValue("Occupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("Remarks");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("BoxID");
        //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee");
        var PermanentDiscount = AppUtil.GetIdValue("PermanentDiscount");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        //var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');

        var ResellerID = AppUtil.GetIdValue("lstResellerID");

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");


        var StatusThisMonth = 3;
        var StatusNextMonth = 3;

        var PackageThisMonth = AppUtil.GetIdValue("PackageID");
        var PackageNextMonth = AppUtil.GetIdValue("PackageID");

        var ClientDetails = [];
        ClientDetails.push({
            Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate,
            MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate,
            ResellerID: ResellerID
            , StatusThisMonth: StatusThisMonth, StatusNextMonth: StatusNextMonth
            , PackageThisMonth: PackageThisMonth, PackageNextMonth: PackageNextMonth, PermanentDiscount: PermanentDiscount
        });
        //   var Mikrotik = { MikrotikID: MikrotikID, IP: IP, Mac: Mac };
        var Transaction = { /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee, PermanentDiscount: PermanentDiscount };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;


        var formData = new FormData();
        formData.append('ClientOwnImageBytes', $('#ClientOwnImageBytes')[0].files[0]);
        formData.append('ClientNIDImage', $('#ClientNIDImage')[0].files[0]);
        formData.append('ClientDetails', JSON.stringify(ClientDetails));
        formData.append('Transaction', JSON.stringify(Transaction));

        var datas = JSON.stringify({ ClientDetails: ClientDetails, Transaction: Transaction });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.InsertResellerClientDetailsByAdminSuccess, ClientDetailsManager.InsertResellerClientDetailsByAdminFail);
    },
    InsertResellerClientDetailsByAdminSuccess: function (data) {
        if (data.Success === false) {
            if (data.ResellerBalanceLow === true) {
                AppUtil.ShowSuccess("Sorry you have no balance. Recharge First.");
            }
            if (data.ResellerPermanentDiscountIsLessThenPackagepriceGivenByaAdmin) {
                AppUtil.ShowSuccess("Discount Amount Can Not Less Then Package Price.");
            }
            //   DuplicateCableStockID = , CableBoxName = , LenghtGreaterThanCableAmount = , GreaterBoxNameList = 
            if (data.MikrotikConnectionFailed === true) {
                AppUtil.ShowSuccess("Sorry Prolem Occoured in Mikrotik COnnection. Please Make Sure Mikrotik is Online and ID and Password is correct.");
            }
            if (data.AlreadyAddedLoginNameInMikrotik === true) {
                AppUtil.ShowSuccess("Login Name Already Used in Mikrotik. Please Choose Different One.");
            }
            if (data.LoginNameExist === true) {
                AppUtil.ShowSuccess("Login Name Already Used. Please Choose Different One.");
            }
            if (data.UserAddInMikrotik === false) {
                AppUtil.ShowError(data.Message);
            }

            if (data.SerialAlreadyAdded === true) {
                AppUtil.ShowError(data.SerialList + " Already Using Some Purpous.");
            }
            if (data.DuplicateCableStockID === true) {
                AppUtil.ShowError(data.CableBoxName + " are added in duplicate times.");
            }
            if (data.LenghtGreaterThanCableAmount === true) {
                AppUtil.ShowError(data.GreaterBoxNameList + " For Those Box you assign length of cable which is greater than the actual amount.");
            }
        }

        console.log(data);
        if (data.SuccessInsert === true) {
            itemAssignArray = [];
            cableAssignArray = [];
            $("#tblItemList>tbody>tr").remove();
            $("#tblCableList>tbody>tr").remove();
            AppUtil.ShowSuccess("Saved Successfully.");
            window.location.href = "/Client/GetAllResellerClientsByAdmin";
        }
        if (data.SuccessInsert === false) { 
            AppUtil.ShowSuccess("Saved Failed."); 
        }
    },
    InsertResellerClientDetailsByAdminFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    //UpdateClientDetails: function () {

    //    var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
    //    var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
    //    var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

    //    var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";

    //    var Name = AppUtil.GetIdValue("Name");
    //    var Email = AppUtil.GetIdValue("Email");
    //    var LoginName = AppUtil.GetIdValue("LoginName");
    //    var Password = AppUtil.GetIdValue("Password");
    //    var Address = AppUtil.GetIdValue("Address");
    //    var ContactNumber = AppUtil.GetIdValue("ContactNumber");
    //    var ZoneID = AppUtil.GetIdValue("ZoneID");
    //    var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
    //    var Occupation = AppUtil.GetIdValue("Occupation");
    //    var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
    //    var Remarks = AppUtil.GetIdValue("Remarks");
    //    var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
    //    var BoxNumber = AppUtil.GetIdValue("BoxID");
    //    var PopDetails = AppUtil.GetIdValue("PopDetails");
    //    var RequireCable = AppUtil.GetIdValue("RequireCable");
    //    var CableTypeID = AppUtil.GetIdValue("CableTypeID");
    //    var Reference = AppUtil.GetIdValue("Reference");
    //    var NationalID = AppUtil.GetIdValue("NationalID");
    //    //var PackageID = AppUtil.GetIdValue("PackageID");
    //    var PackageThisMonth = AppUtil.GetIdValue("PackageThisMonth");
    //    var PackageNextMonth = AppUtil.GetIdValue("PackageNextMonth");
    //    var SingUpFee = AppUtil.GetIdValue("SingUpFee");                              //////
    //    var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
    //    var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
    //    var MacAddress = AppUtil.GetIdValue("MacAddress");
    //    var BillPaymentDate = $("#BillPaymentDate").val();//AppUtil.getDateTime("BillPaymentDate");//$('#BillPaymentDate').datepicker('getDate');
    //    var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
    //    var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
    //    var LineStatusWillActiveInThisDate = $("#LineStatusActiveDate").val();

    //    //var LineStatusID = AppUtil.GetIdValue("LineStatusID");
    //    var ThisMonthLineStatusID = AppUtil.GetIdValue("ThisMonthLineStatusID");
    //    var NextMonthLineStatusID = AppUtil.GetIdValue("NextMonthLineStatusID");
    //    var Reason = AppUtil.GetIdValue("Reason");

    //    var IsPriorityClient = $("#IsPriorityClient").is(':checked');

    //    var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
    //    var IP = AppUtil.GetIdValue("IP");
    //    var Mac = AppUtil.GetIdValue("Mac");

    //    var ResellerID = AppUtil.GetIdValue("ResellerID");



    //    //var StatusThisMonth = AppUtil.GetIdValue("LineStatusID");
    //    //var StatusNextMonth = AppUtil.GetIdValue("LineStatusID");

    //    //var PackageThisMonth = AppUtil.GetIdValue("PackageID");
    //    //var PackageNextMonth = AppUtil.GetIdValue("PackageID");
    //    //AppUtil.ShowWaitingDialog();
    //    //code before the pause
    //    //  setTimeout(function () {

    //    var ClientDetails = {
    //        ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
    //        , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
    //        , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, /*PackageID: PackageID,*/ SecurityQuestionID: SecurityQuestionID
    //        , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient, ApproxPaymentDate: BillPaymentDate
    //        , MikrotikID: MikrotikID, IP: IP, Mac: Mac
    //        , ResellerID: ResellerID
    //        , StatusThisMonth: ThisMonthLineStatusID
    //        , StatusNextMonth: NextMonthLineStatusID

    //        , PackageThisMonth: PackageThisMonth
    //        , PackageNextMonth: PackageNextMonth
    //        , LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate
    //    };
    //    var Transaction = { TransactionID: ClientTransactionID, PaymentAmount: SingUpFee };
    //    var ClientLineStatus = {
    //        ClientLineStatusID: ClientLineStatusID, LineStatusID: /*LineStatusID*/NextMonthLineStatusID, StatusChangeReason: Reason, /*PackageID: PackageID,*/ LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate
    //        , PackageThisMonth: PackageThisMonth
    //        , PackageNextMonth: PackageNextMonth };

    //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
    //    var header = {};
    //    header['__RequestVerificationToken'] = AntiForgeryToken;

    //    var a = $('#ClientOWNImagePath').val();
    //    var formData = new FormData();
    //    formData.append('ClientOWNImageBytes', $('#ClientOWNImageBytes')[0].files[0]);
    //    formData.append('ClientOWNImagePath', $('#ClientOWNImageBytesPaths').val());
    //    formData.append('ClientNIDImage', $('#ClientNIDImage')[0].files[0]);
    //    formData.append('ClientNIDImagePath', $('#ClientNIDImageBytesPaths').val());
    //    formData.append('ClientClientDetails', JSON.stringify(ClientDetails));
    //    formData.append('ClientTransaction', JSON.stringify(Transaction));
    //    formData.append('ClientClientLineStatus', JSON.stringify(ClientLineStatus));
    //    formData.append('chkPackageFromRunningMonth', JSON.stringify(chkPackageFromRunningMonth));
    //    formData.append('chkStatusFromRunningMonth', JSON.stringify(chkStatusFromRunningMonth));


    //    var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
    //    //AppUtil.MakeAjaxCall(url, "POST", datas, ClientDetailsManager.UpdateClientDetailsSuccess, ClientDetailsManager.UpdateClientDetailsFail);
    //    AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.UpdateClientDetailsSuccess, ClientDetailsManager.UpdateClientDetailsFail);
    //    // }, 50);
    //},
    //UpdateClientDetailsSuccess: function (data) {

    //    //AppUtil.HideWaitingDialog();

    //    if (data.MikrotikFailed === true) {
    //        AppUtil.ShowError(data.Message);
    //    }

    //    if (data.Success === false) {
    //        if (data.MikrotikFailed === true) {
    //            AppUtil.ShowError(data.Message);
    //        }
    //    }
    //    if (data.StatusIsSameButRunningMonthChecked === true) {
    //        AppUtil.ShowError("Sorry you checked running month but the Status is remaing same. please chage the Status.");
    //    }
    //    if (data.PackageIsSameButRunningMonthChecked === true) {
    //        AppUtil.ShowError("Sorry you checked running month but the package is remaing same. please chage the package.");
    //    }

    //    if (data.BothChecked === true) {
    //        AppUtil.ShowError("Sorry Running Month Checkbox and LockFromRunning Month Cant Check at a time.");
    //    }
    //    if (data.BillAlreadyGenerate === true) {
    //        AppUtil.ShowError("Sorry You Can Not Change Package. Cause Bill Already Generate.");
    //    }
    //    if (data.BillNotGenerate === true) {
    //        AppUtil.ShowError("Bill Not Generate.");
    //    }
    //    if (data.BillAlreadyPaid === true) {
    //        AppUtil.ShowError("Bill Already paid you cant change package.");
    //    }
    //    if (data.PackageCantChangeInThisMonthLimitExist === true) {
    //        AppUtil.ShowError("Package Cant change cause already its changed in this month. wait till next month.");
    //    }
    //    if (data.CantChangePackageCauseStatusIsLock === true) {
    //        AppUtil.ShowError("Sorry You Can Not Change Package. Cause This Client Is Lock.");
    //    }

    //    if (data.LoginNameExist === true) {
    //        AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
    //    }
    //    if (data.LoginNameExist === false && data.UpdateStatus === true) {

    //        var packageName = "";
    //        var ClientStatusThisMonthHtml = "";
    //        var ClientStatusNextMonthHtml = "";
    //        var needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = "";

    //        if (data.ClientLineStatus) {

    //            //StatusThisMonth = StatusThisMonth, StatusNexrMonth = StatusNexrMonth
    //            var StatusThisMonth = (data.StatusThisMonth);
    //            if (StatusThisMonth === 3) {
    //                ClientStatusThisMonthHtml = '<div style="color:green;font-weight:bold;">Active</div>';
    //            }
    //            else if (StatusThisMonth === 4) {
    //                ClientStatusThisMonthHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
    //            }
    //            else {
    //                ClientStatusThisMonthHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
    //            }

    //            var StatusNexrMonth = (data.StatusNexrMonth);
    //            if (StatusNexrMonth === 3) {
    //                ClientStatusNextMonthHtml = '<div style="color:green;font-weight:bold;">Active</div>';
    //            }
    //            else if (StatusNexrMonth === 4) {
    //                ClientStatusNextMonthHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
    //            }
    //            else {
    //                ClientStatusNextMonthHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
    //            }
    //        }

    //        if (data.ClientDetails) {
    //            var parseEmployee = (data.ClientDetails);
    //            $("#tblUsers>tbody>tr").each(function () {


    //                var index = $(this).index();
    //                var employeeID = $(this).find("td:eq(0) input").val();
    //                if (employeeID == parseEmployee.ClientDetailsID) {


    //                    if (parseEmployee.IsPriorityClient) {
    //                        $('#tblUsers tbody>tr:eq(' + index + ')').addClass('changetrbackground');
    //                    }
    //                    else {
    //                        $('#tblUsers tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
    //                    }

    //                    needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = index;
    //                    $('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee.Name);
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseEmployee.LoginName); 
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(data.ThisMonthPackage);  
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(data.NextMonthPackage);

    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee.Address);
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee.Email);
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(parseEmployee.ZoneName);
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(9)").text(parseEmployee.ContactNumber);
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(parseEmployee.ProfileStatusUpdateInPercent); 
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(11)").html(ClientStatusThisMonthHtml); 
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(12)").html(ClientStatusNextMonthHtml);

    //                    var split = data.LineStatusActiveDate.split('<div');
    //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(13)").html(split[0]);

    //                }
    //            });
    //        }

    //        if (data.UpdateStatus === true) {

    //            AppUtil.ShowSuccess("Successfully Edited.");
    //            var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
    //            var LineStatusID = AppUtil.GetIdValue("LineStatusID");
    //            if (chkStatusFromRunningMonth == true && LineStatusID == 5) {
    //                $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(15)").html('<button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"> <span class="glyphicon glyphicon-remove"></span> </button>');
    //            }
    //            if (chkStatusFromRunningMonth == true && LineStatusID == 3) {
    //                $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(15)").html('');
    //            }
    //        }
    //        if (data.UpdateStatus === false) {
    //            AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
    //        }

    //    }


    //    ClientDetailsManager.ClearClientDetailsModalInformation();
    //    //$("#Status").css("visibility", false);
    //    $("#Status").css("display", "none");
    //    $("#chkStatusFromRunningMonth").prop("checked", false);
    //    $("#chkPackageFromRunningMonth").prop("checked", false);

    //    $("#tblEmployeeDetails").modal("hide");
    //    console.log(data);
    //},
    //UpdateClientDetailsFail: function (data) {

    //    AppUtil.ShowSuccess("Error Occoured.");
    //    console.log(data);
    //},


    UpdateResellerClientDetails: function () {

        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        //var url = "/Client/UpdateResellerClientDetailsOnlyAllClientForMKT/";

        var url = "/Client/UpdateClientDetailsOnlyAllClientForMKTByResellerOrByAdminForReseller/";

        var Name = AppUtil.GetIdValue("Name");
        var Email = AppUtil.GetIdValue("Email");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("Address");
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
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient//, ApproxPaymentDate: BillPaymentDate
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac
            , ResellerID: ResellerID
            , StatusThisMonth: ThisMonthLineStatusID
            , StatusNextMonth: NextMonthLineStatusID

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

        //var ClientDetails = {
        //    ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
        //    , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
        //    , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, /*PackageID: PackageID,*/ SecurityQuestionID: SecurityQuestionID
        //    , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient//, ApproxPaymentDate: BillPaymentDate
        //    , MikrotikID: MikrotikID, IP: IP, Mac: Mac
        //    , ResellerID: ResellerID
        //    //, StatusThisMonth: StatusThisMonth
        //    , StatusNextMonth: LineStatusID

        //    , PackageThisMonth: PackageThisMonth
        //    , PackageNextMonth: PackageNextMonth
        //    , LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate, PermanentDiscount: PermanentDiscount
        //};
        //var Transaction = { TransactionID: ClientTransactionID, PaymentAmount: SingUpFee, PermanentDiscount: PermanentDiscount };
        //var ClientLineStatus = {
        //    ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, /*PackageID: PackageID,*/ LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate
        //    , PackageThisMonth: PackageThisMonth
        //    , PackageNextMonth: PackageNextMonth
        //};

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
        //AppUtil.MakeAjaxCall(url, "POST", datas, ClientDetailsManager.UpdateClientDetailsSuccess, ClientDetailsManager.UpdateClientDetailsFail);
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, ClientDetailsManager.UpdateResellerClientDetailsSuccess, ClientDetailsManager.UpdateResellerClientDetailsFail);
        // }, 50);
    },
    UpdateResellerClientDetailsSuccess: function (data) {

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }

        if (data.Success === false) {
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
            if (data.PackageChangeNumberIsFinish  === true) {
                AppUtil.ShowError("Package Can Not Change. Can Not Cross Number Of Package Change Limitation.");
            }
            if (data.ResellerHasBalance   === false) {
                AppUtil.ShowError("Package Can Not Change. You Do Not Have Sufficient Balance.");
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

            $("#Status").css("display", "none");
            $("#chkStatusFromRunningMonth").prop("checked", false);
            $("#chkPackageFromRunningMonth").prop("checked", false);

            $("#tblEmployeeDetails").modal("hide");
            console.log(data);
        }


        ////AppUtil.HideWaitingDialog();

        //if (data.MikrotikFailed === true) {
        //    AppUtil.ShowError(data.Message);
        //}

        //if (data.Success === false) {
        //    if (data.MikrotikFailed === true) {
        //        AppUtil.ShowError(data.Message);
        //    }
        //}
        //if (data.StatusIsSameButRunningMonthChecked === true) {
        //    AppUtil.ShowError("Sorry you checked running month but the Status is remaing same. please chage the Status.");
        //}
        //if (data.PackageIsSameButRunningMonthChecked === true) {
        //    AppUtil.ShowError("Sorry you checked running month but the package is remaing same. please chage the package.");
        //}

        //if (data.BothChecked === true) {
        //    AppUtil.ShowError("Sorry Running Month Checkbox and LockFromRunning Month Cant Check at a time.");
        //}
        //if (data.BillAlreadyGenerate === true) {
        //    AppUtil.ShowError("Sorry You Can Not Change Package. Cause Bill Already Generate.");
        //}
        //if (data.BillNotGenerate === true) {
        //    AppUtil.ShowError("Bill Not Generate.");
        //}
        //if (data.BillAlreadyPaid === true) {
        //    AppUtil.ShowError("Bill Already paid you cant change package.");
        //}
        //if (data.PackageCantChangeInThisMonthLimitExist === true) {
        //    AppUtil.ShowError("Package Cant change cause already its changed in this month. wait till next month.");
        //}
        //if (data.CantChangePackageCauseStatusIsLock === true) {
        //    AppUtil.ShowError("Sorry You Can Not Change Package. Cause This Client Is Lock.");
        //}

        //if (data.LoginNameExist === true) {
        //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        //}
        //if (data.LoginNameExist === false && data.UpdateStatus === true) {

        //    var packageName = "";
        //    var ClientStatusHtml = "";
        //    var needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = "";

        //    if (data.ClientLineStatus) {
        //        var ClientLineStatus = (data.ClientLineStatus);
        //        if (ClientLineStatus === 3) {
        //            ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
        //        }
        //        if (ClientLineStatus === 4) {
        //            ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
        //        }
        //        if (ClientLineStatus === 5) {
        //            ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
        //        }
        //    }

        //    if (data.ClientDetails) {
        //        var parseEmployee = (data.ClientDetails);
        //        $("#tblUsers>tbody>tr").each(function () {


        //            var index = $(this).index();
        //            var employeeID = $(this).find("td:eq(0) input").val();
        //            if (employeeID == parseEmployee[0].ClientDetailsID) {


        //                if (parseEmployee[0].IsPriorityClient) {
        //                    $('#tblUsers tbody>tr:eq(' + index + ')').addClass('changetrbackground');
        //                }
        //                else {
        //                    $('#tblUsers tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
        //                }

        //                needToAddDeleteButtonAfterUpdateClientLockFromThisMonth = index;
        //                $('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee[0].Name);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseEmployee[0].LoginName);
        //                if (data.chkPackageFromRunningMonth === true) {

        //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(data.ClientPackage);
        //                }

        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(data.ClientPackage);

        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee[0].Address);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee[0].Email);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(parseEmployee[0].ZoneName);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(9)").text(parseEmployee[0].ContactNumber);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(parseEmployee[0].ProfileStatusUpdateInPercent);
        //                if (data.chkStatusFromRunningMonth === true) {
        //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(11)").html(ClientStatusHtml);
        //                }
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(12)").html(ClientStatusHtml);

        //                var split = data.LineStatusActiveDate.split('<div');
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(13)").html(split[0]);

        //            }
        //        });
        //    }

        //    if (data.UpdateStatus === true) {

        //        AppUtil.ShowSuccess("Successfully Edited.");
        //        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        //        var LineStatusID = AppUtil.GetIdValue("LineStatusID");
        //        if (chkStatusFromRunningMonth == true && LineStatusID == 5) {
        //            $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(15)").html('<button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"> <span class="glyphicon glyphicon-remove"></span> </button>');
        //        }
        //        if (chkStatusFromRunningMonth == true && LineStatusID == 3) {
        //            $("#tblUsers>tbody>tr:eq(" + needToAddDeleteButtonAfterUpdateClientLockFromThisMonth + ")").find("td:eq(15)").html('');
        //        }
        //    }
        //    if (data.UpdateStatus === false) {
        //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
        //    }


        //}


        //ClientDetailsManager.ClearClientDetailsModalInformation();
        ////$("#Status").css("visibility", false);
        //$("#Status").css("display", "none");
        //$("#chkStatusFromRunningMonth").prop("checked", false);
        //$("#chkPackageFromRunningMonth").prop("checked", false);

        //$("#tblEmployeeDetails").modal("hide");
        //console.log(data);
    },
    UpdateResellerClientDetailsFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },


    UpdateClientDetailsDashBoard: function () {


        var packageFromThisMonth = false;


        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;



        var url = "/Client/UpdateClientDetails/";

        var Name = AppUtil.GetIdValue("ClientDetails_Name");
        var Email = AppUtil.GetIdValue("ClientDetails_Email");
        var LoginName = AppUtil.GetIdValue("ClientDetails_LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("ClientDetails_Address");
        var ContactNumber = AppUtil.GetIdValue("ClientDetails_ContactNumber");
        var ZoneID = AppUtil.GetIdValue("ZoneID");
        var SMSCommunication = AppUtil.GetIdValue("ClientDetails_SMSCommunication");
        var Occupation = AppUtil.GetIdValue("ClientDetails_Occupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("ClientDetails_SocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("ClientDetails_Remarks");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("ClientDetails_BoxNumber");
        var PopDetails = AppUtil.GetIdValue("ClientDetails_PopDetails");
        var RequireCable = AppUtil.GetIdValue("ClientDetails_RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("ClientDetails_Reference");
        var NationalID = AppUtil.GetIdValue("ClientDetails_NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("ClientDetails_SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("ClientDetails_MacAddress");
        //var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //var ClientSurvey = AppUtil.GetIdValue("ClientDetails_ClientSurvey");
        //var ConnectionDate = AppUtil.getDateTime("ClientDetails_ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var BillPaymentDate = $("#BillPaymentDate").val();//AppUtil.getDateTime("BillPaymentDate");//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientDetails_ClientSurvey");
        var ConnectionDate = $("#ClientDetails_ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
        var LineStatusWillActiveInThisDate = $("#LineStatusActiveDate").val();

        var LineStatusID = AppUtil.GetIdValue("LineStatusID");
        var Reason = AppUtil.GetIdValue("StatusChangeReason");

        var IsPriorityClient = $("#IsPriorityClient").is(':checked');

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: $("#ClientDetailsID").val(), Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate
            , IsPriorityClient: IsPriorityClient, ApproxPaymentDate: BillPaymentDate
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac
            , ResellerID: ResellerID
        };
        var Transaction = { TransactionID: $("#TransactionID").val(), /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: $("#ClientLineStatusID").val(), LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientDetailsManager.UpdateClientDetailsDashBoardSuccess, ClientDetailsManager.UpdateClientDetailsDashBoardFail);
        // }, 50);
    },
    UpdateClientDetailsDashBoardSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }
        if (data.RemoveMikrotikInformation === true) {
            AppUtil.ShowError("Remove Information from mikrotik for this Name : " + data.MKUserName);
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
        if (data.LoginNameExist === false) {

            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
                setTimeout(function () {
                    window.location.href = "/Client/GetSpecificClientDetailsFromDashBoard" + "?CID=" + data.ClientDetails[0].ClientDetailsID;
                }, 2000);


            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }

        }


        //ClientDetailsManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");

        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsDashBoardFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },


    DeleteClientDetails: function () {


        var url = "/Client/DeleteClientDetails/";

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = ({ ClientDetailsID: ClientDetailsID });
        datas = ClientDetailsManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, ClientDetailsManager.DeleteClientDetailsSuccess, ClientDetailsManager.DeleteClientDetailsFail);
        // }, 50);
    },
    DeleteClientDetailsSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.DeleteStatus === true) {
            $("#tblUsers>tbody>tr").each(function () {


                var index = $(this).index();
                var employeeID = $(this).find("td:eq(0) input").val();
                if (employeeID == data.ClientDetailsID) {

                    $('#tblUsers tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
        }

        if (data.DeleteStatus === false) {
            if (data.Reason.length > 0) {
                AppUtil.ShowSuccess(data.Reason);
            }
            else {
                AppUtil.ShowSuccess("Some Information Can not removed.");
            }
        }


        console.log(data);
    },
    DeleteClientDetailsFail: function (data) {

        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    UpdateClientInformationFromClient: function () {

        var url = "/Client/UpdateClientInformationFromClient/";


        //var LoginName = AppUtil.GetIdValue("LoginName");
        //var Password = AppUtil.GetIdValue("Password");
        //var PackageID = AppUtil.GetIdValue("PackageID");
        //var ZoneID = AppUtil.GetIdValue("ZoneID");

        var ClientDetailsID = AppUtil.GetIdValue("ClientDetailsID");
        var Name = AppUtil.GetIdValue("ClientDetails_Name");
        var Email = AppUtil.GetIdValue("ClientDetails_Email");
        var ContactNumber = AppUtil.GetIdValue("ClientDetails_ContactNumber");
        var Address = AppUtil.GetIdValue("ClientDetails_Address");
        var SMSCommunication = AppUtil.GetIdValue("ClientDetails_SMSCommunication");
        var Occupation = AppUtil.GetIdValue("ClientDetails_Occupation");
        var NationalID = AppUtil.GetIdValue("ClientDetails_NationalID");
        var Reference = AppUtil.GetIdValue("ClientDetails_Reference");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("ClientDetails_SecurityQuestionAnswer");
        var SocialCommunicationURL = AppUtil.GetIdValue("ClientDetails_SocialCommunicationURL");

        //var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        //var Remarks = AppUtil.GetIdValue("Remarks");
        //var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        //var PopDetails = AppUtil.GetIdValue("PopDetails");
        //var RequireCable = AppUtil.GetIdValue("RequireCable");
        //var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        //var SingUpFee = AppUtil.GetIdValue("SingUpFee");                              //////

        //var MacAddress = AppUtil.GetIdValue("MacAddress");
        //var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        //var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        //var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        //var LineStatusID = AppUtil.GetIdValue("LineStatusID");
        //var Reason = AppUtil.GetIdValue("Reason");

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //    setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, Address: Address, ContactNumber: ContactNumber, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL,
            Reference: Reference, NationalID: NationalID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer
        };


        //  var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ ClientClientDetails: ClientDetails });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientDetailsManager.UpdateClientInformationFromClientSuccess, ClientDetailsManager.UpdateClientInformationFromClientFail);
        //   }, 50);
    },
    UpdateClientInformationFromClientSuccess: function (data) {
        //AppUtil.HideWaitingDialog();
        if (data.UpdateStatus === true) {
            AppUtil.ShowSuccess("Client Updated Successfully.");
        }
        if (data.UpdateStatus === false) {
            AppUtil.ShowSuccess("Client Update Failed.");
        }

    },
    UpdateClientInformationFromClientFail: function (data) {
        AppUtil.ShowError("Error Found Contact With Administrator.");
    },

    SearchClientListByZone: function (SearchID, searchType) {

        if (SearchID === "") {
            SearchID = 0;
        }
        //AppUtil.ShowWaitingDialog();
        var url = "/Client/SearchClientListByZone/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ SearchID: SearchID, searchType: searchType, __RequestVerificationToken: AntiForgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SearchClientListByZoneSuccess, ClientDetailsManager.SearchClientListByZoneError);
    },
    SearchClientListByZoneSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;


        $('#tblUsers').dataTable().fnDestroy();
        $("#tblUsers>tbody").empty();

        //var listOfClient = JSON.parse(data.SearchClientList)
        var listOfClient = (data.SearchClientList);

        $.each(listOfClient, function (index, item) {

            console.log(item);
            // $.each(item, function (index, items) {

            console.log(item);
            var thisMonthStatus = "";
            if (item.StatusThisMonthID === "3") {
                thisMonthStatus = '<div style="color:green;font-weight:bold;">Active</div>';
            }
            if (item.StatusThisMonthID === "4") {
                thisMonthStatus = '<div style="color:red;font-weight:bold;">InActive</div>';
            }

            if (item.StatusThisMonthID === "5") {
                thisMonthStatus = '<div style="color:red;font-weight:bold;">Lock</div>';
            }

            var nextMonthStatus = "";
            if (item.StatusNextMonthID === "3") {
                nextMonthStatus = '<div style="color:green;font-weight:bold;">Active</div>';
            }
            if (item.StatusNextMonthID === "4") {
                nextMonthStatus = '<div style="color:red;font-weight:bold;">InActive</div>';
            }

            if (item.StatusNextMonthID === "5") {
                nextMonthStatus = '<div style="color:red;font-weight:bold;">Lock</div>';
            }

            //$('#tblUsers tbody').append('<tr><td style="padding:0px"><input type="hidden" id="EmployeeDetailsID" name="EmployeeDetailsID" value=' + item.ClientDetailsID + '></td><td>' + item.ClientDetails.Name + '</td><td>' + item.ClientDetails.LoginName + '</td><td>' + item.ClientDetails.Package.PackageName + '</td><td>' + item.ClientDetails.Address + '</td><td>' + item.ClientDetails.Email + '</td><td>' + item.ClientDetails.Zone.ZoneName + '</td><td>' + item.ClientDetails.ContactNumber + '</td><td>' + ClientStatusHtml + '</td><td><a href="" id="ShowPopUps">Show</a></td></tr>');
            $('#tblUsers tbody').append('<tr><td hidden="hidden"><input type="hidden" id="EmployeeDetailsID" name="EmployeeDetailsID" value=' + item.ClientDetailsID + '></td><td>' + item.Name + '</td><td>' + item.LoginName + '</td><td>' + item.PackageNameThisMonth + '</td><td>' + item.PackageNameNextMonth + '</td><td>' + item.Address + '</td><td>' + item.Email + '</td><td>' + item.ZoneName + '</td><td>' + item.ContactNumber + '</td><td>' + thisMonthStatus + '</td><td>' + nextMonthStatus + '</td><td><a href="" id="ShowPopUps">Show</a></td><td align="center"><button id="btnDelete" type="button" class="btn btn-danger btn-sm padding"  data-placement="top" data-toggle="modal" data-target="#popModalForDeletePermently"><span class="glyphicon glyphicon-remove"></span></button></td></tr>');
            //$('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(0)").html("<input type='hidden' id='EmployeeDetailsID' name='EmployeeDetailsID' value=" + item.EmployeeDetailsID + ">");
            //$('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(1)").text(item.Name);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(2)").text(item.LoginName);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(item.Package.PackageName);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(item.Address);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(item.Email);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(item.Zone.ZoneName);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(item.ContactNumber);
            //$('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").html(ClientStatusHtml);
            index = parseInt(index) + 1;
            // });
        });

        //var mytable =     $('#tblUsers').dataTable({
        //    //paging: false,
        //    //searching: false,
        //    //"aoColumns": [
        //    //{ "sType": "html",
        //    //    "sWidth": "200px"},
        //    //{ "bSearchable": false},
        //    //{ "bSearchable": false},
        //    //{ "bSearchable": false},
        //    //{ "bSearchable": false},
        //    //{ "bSearchable": false},
        //    //{ "sWidth": "140px" }
        //    //],
        //    //"bJQueryUI": false,
        //    //"sPaginationType": "full_numbers",
        //    //"iDisplayLength": 25,
        //    //"bAutoWidth": false,
        //    //"aaSorting": [ ] // Prevents initial sorting 


        //} );
        var mytable = $('#tblUsers').DataTable();


        //var mytable = $('#tblUsers')
        //.DataTable({
        //  //  "destroy": true, "filter": false,
        //    "deferRender": true,
        //    "paging": true,
        //    "lengthChange": false,
        //    "searching": false,
        //    "ordering": true,
        //    "info": true,
        //    "autoWidth": false,
        //    "sDom": 'lfrtip'
        //});
        mytable.draw();

    },
    SearchClientListByZoneError: function (data) {

        AppUtil.ShowError("Error");
        console.log(data);
    },

    GetPhoneListByZoneAndLineStatus: function () {
        var url = "/Client/ViewClientPhoneListDependOnCriteria/";
        //AppUtil.ShowWaitingDialog();
        // setTimeout(function () {
        var ZoneID = $("#ZoneID").val();
        var LineStatusID = $("#LineStatusID").val();
        var data = ({ ZoneID: ZoneID, LineStatusID: LineStatusID });
        data = ClientDetailsManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.GetPhoneListByZoneAndLineStatusSuccess, ClientDetailsManager.GetPhoneListByZoneAndLineStatusError);

        // }, 1000);

    },
    GetPhoneListByZoneAndLineStatusSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        $("#tblPhoneList>tbody").find("tr").remove();
        if (data.Success === true) {

            if (data.DataFound === true) {
                var phoneList = JSON.parse(data.lstPhoneNumber);
                $.each(phoneList, function (index, item) {


                    $("#tblPhoneList>tbody").append('<tr><td>' + item + '</td></tr>');
                });

                AppUtil.ShowSuccess("Information Found.");
            }
            if (data.DataFound === false) {
                AppUtil.ShowSuccess("No Information Found For this search criteria.");
            }

        }
    },
    GetPhoneListByZoneAndLineStatusError: function (data) {

        console.log(data);
        AppUtil.ShowError("Retrieve Information Error.");
    },

    ClientLoginExistOrNot: function (loginName) {

        $("#Status").css("display", "none");
        var url = "/Client/ClientLoginExistOrNot/";

        // //AppUtil.ShowWaitingDialog();
        //code before the pause
        //setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = ({ LoginName: loginName });
        datas = ClientDetailsManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, ClientDetailsManager.ClientLoginExistOrNotSuccess, ClientDetailsManager.ClientLoginExistOrNotError);
        //   }, 50);
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

    ClientLoginExistOrNotIncludeID: function (loginName, ClientDetailsID) {

        $("#Status").css("display", "none");
        var url = "/Client/ClientLoginExistOrNotIncludeID/";

        // //AppUtil.ShowWaitingDialog();
        //code before the pause
        //   setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = ({ LoginName: loginName, ClientDetailsID: ClientDetailsID });
        datas = ClientDetailsManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, ClientDetailsManager.ClientLoginExistOrNotIncludeIDSuccess, ClientDetailsManager.ClientLoginExistOrNotIncludeIDError);
        // }, 50);
    },
    ClientLoginExistOrNotIncludeIDSuccess: function (data) {

        //  //AppUtil.HideWaitingDialog();
        if (data.Exist === true) {
            $("#Status").css("display", "inline");
        }
    },
    ClientLoginExistOrNotIncludeIDError: function (data) {

        //AppUtil.HideWaitingDialog();
    },

    UpdateLoginNameIfGivenAndPassword: function () {

        //AppUtil.ShowWaitingDialog();
        $("#Status").css("display", "none"); 
        var OldPassword = $("#txtOldPassword").val();
        var NewPassword = $("#txtNewFirstPassword").val();
        var url = "/Client/UpdateLoginNameIfGivenAndPassword/";

        // //AppUtil.ShowWaitingDialog();
        //code before the pause
        //   setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = ({ OldPassword: OldPassword, NewPassword: NewPassword });
        datas = ClientDetailsManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, ClientDetailsManager.UpdateLoginNameIfGivenAndPasswordSuccess, ClientDetailsManager.UpdateLoginNameIfGivenAndPasswordFail);
        //    }, 50);
    },
    UpdateLoginNameIfGivenAndPasswordSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        $("#txtLoginName").val("");
        $("#txtOldPassword").val("");
        $("#txtNewFirstPassword").val("");
        $("#txtNewSecondPassword").val("");

        if (data.LoginNameExist === true) {
            AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        }

        if (data.UpdateStatus === true) {

            if (data.ReturnMessage) {
                AppUtil.ShowSuccess(data.ReturnMessage + " Update Successfully.");
            }
        }
        if (data.OldPasswordNotMatched === true) {

            AppUtil.ShowSuccess("Sorry Old Password is Not COrrect.");
        }
        if (data.LogInfirst === true) {
            AppUtil.ShowSuccess("Please Login First.");
        }
        if (data.Error === true) {
            AppUtil.ShowSuccess("Error Found Contact With Administrator.");
        }
    },
    UpdateLoginNameIfGivenAndPasswordFail: function (data) {

        AppUtil.HideWaitingDialog()
        AppUtil.ShowError("Error Found Contact With Administrator.");
    },

    SearchActiveToLockBySearchCriteria: function () {


        //AppUtil.ShowWaitingDialog();
        var url = "/Client/SearchActiveToLockBySearchCriteria";
        var YearID = $("#YearID").val();
        var MonthID = $("#MonthID").val();

        var data = ({ YearID: YearID, MonthID: MonthID });

        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SearchActiveToLockBySearchCriteriaSuccess, ClientDetailsManager.SearchActiveToLockBySearchCriteriaError);

    },
    SearchActiveToLockBySearchCriteriaSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {

            if (data.lstClientLineStatusesCount >= 0) {
                AppUtil.ShowSuccess("Total " + data.lstClientLineStatusesCount + " Information Found.");

                $('#tblClientMonthlyBill').dataTable().fnDestroy();
                $("#tblClientMonthlyBill>tbody").empty();

                $.each(data.lstClientLineStatusesList, function (index, item) {

                    var LineStatus = "";
                    var actionOption = "";


                    var link = "";
                    if (item.ClientDetailsID && item.TransactionID) {
                        link = "<div id='forRowID'><a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.LoginName + "</a></div>";
                    }
                    else {
                        link = item.Name;
                    }

                    var setPriority = false;
                    var classes;
                    if (item.IsPriorityClient) {
                        setPriority = true;
                        classes = 'changetrbackground';
                    }

                    $("#tblClientMonthlyBill>tbody").append("<tr  class='" + classes + "'><td hidden><input type='hidden' value=" + item.ClientLineStatusID + "></td><td hidden></td>\
                     <td>" + link + "</td><td>" + item.Address + "</td><td>" + item.ContactNumber + "</td><td>" + item.ZoneName + "</td>\
                     <td>" + item.PackageName + "</td><td>" + item.PackagePrice + "</td><td>" + item.EmployeeID + "</td><td>" + AppUtil.ParseDateTime(item.LineStatusChangeDate) + "</td><td>" + item.LineStatusActiveDate + "</td>\
                    </tr>");


                });
            }
            else {
                AppUtil.ShowError("Sorry No Information Found.");
            }
        }

        var mytable = $('#tblClientMonthlyBill').DataTable({
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
    SearchActiveToLockBySearchCriteriaError: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    SearchLockToActiveBySearchCriteria: function () {


        //AppUtil.ShowWaitingDialog();
        var url = "/Client/SearchLockToActiveBySearchCriteria";
        var YearID = $("#YearID").val();
        var MonthID = $("#MonthID").val();

        var data = ({ YearID: YearID, MonthID: MonthID });

        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SearchLockToActiveBySearchCriteriaSuccess, ClientDetailsManager.SearchLockToActiveBySearchCriteriaError);

    },
    SearchLockToActiveBySearchCriteriaSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {

            if (data.lstClientLineStatusesCount >= 0) {
                AppUtil.ShowSuccess("Total " + data.lstClientLineStatusesCount + " Information Found.");

                $('#tblClientMonthlyBill').dataTable().fnDestroy();
                $("#tblClientMonthlyBill>tbody").empty();

                //$("#tblClientMonthlyBill>tbody").refresh();

                $.each(data.lstClientLineStatusesList, function (index, item) {

                    var LineStatus = "";
                    var actionOption = "";


                    var link = "";
                    if (item.ClientDetailsID && item.TransactionID) {
                        link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.LoginName + "</a>";
                    }
                    else {
                        link = item.Name;
                    }


                    var setPriority = false;
                    var classes;
                    if (item.IsPriorityClient) {
                        setPriority = true;
                        classes = 'changetrbackground';
                    }

                    $("#tblClientMonthlyBill>tbody").append("<tr  class='" + classes + "'><td hidden><input type='hidden' value=" + item.ClientLineStatusID + "></td><td hidden></td>\
                     <td>" + link + "</td><td>" + item.Address + "</td><td>" + item.ContactNumber + "</td><td>" + item.ZoneName + "</td>\
                     <td>" + item.PackageName + "</td><td>" + item.PackagePrice + "</td><td>" + item.EmployeeID + "</td><td>" + AppUtil.ParseDateTime(item.LineStatusChangeDate) + "</td><td>" + item.LineStatusActiveDate + "</td>\
                    </tr>");


                });
            }
            else {
                AppUtil.ShowError("Sorry No Information Found.");
            }
        }
        var mytable = $('#tblClientMonthlyBill').DataTable({
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
    SearchLockToActiveBySearchCriteriaError: function (data) {

        //AppUtil.HideWaitingDialog();
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
        $("#PackageID").val("");
        $("#SingUpFee").val("");                              //////
        $("#SecurityQuestionID").val("");
        $("#SecurityQuestionAnswer").val("");
        $("#MacAddress").val("");
        $("#BillPaymentDate").val("")//$('#BillPaymentDate').val("").datepicker('getDate').val("");
        $("#ClientSurvey").val("");
        $("#ConnectionDate").val(""); //('#ConnectionDate').val("").datepicker('getDate').val("");

        $("#LineStatusID").val("");
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

    SearchCableBoxOrDrumNameByCableTypeID: function (cableTypePopUpID) {


        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchCableBoxOrDrumNameByCableTypeID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableTypeID: cableTypePopUpID });
        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SearchCableBoxOrDrumNameByCableTypeIDSuccess, ClientDetailsManager.SearchCableBoxOrDrumNameByCableTypeIDError);
    },
    SearchCableBoxOrDrumNameByCableTypeIDSuccess: function (data) {

        AppUtil.ShowSuccess("Success");
        console.log(data);

        $("#CableStockID").find("option").not(":first").remove();
        if (data.Success === true) {
            $.each(data.ListCableStock, function (index, item) {

                $("#CableStockID").append($("<option></option>").val(item.CableStockID).text(item.BoxOrDrumName));
            });
        }
        if (data.Success === false) {
            AppUtil.ShowError("Sorry Information Not Found.");
        }
    },
    SearchCableBoxOrDrumNameByCableTypeIDError: function (data) {

        AppUtil.ShowError("Error");
        console.log(data);
    },


    SearchCableQuantityStockedByCableBoxOrDrumName: function (CableStockID) {


        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchCableQuantityStockedByCableBoxOrDrumName/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableStockID: CableStockID });
        data = ClientDetailsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ClientDetailsManager.SearchCableQuantityStockedByCableBoxOrDrumNameSuccess, ClientDetailsManager.SearchCableQuantityStockedByCableBoxOrDrumNameError);
    },
    SearchCableQuantityStockedByCableBoxOrDrumNameSuccess: function (data) {

        AppUtil.ShowSuccess("Success");
        console.log(data);


        if (data.Success === true) {
            var a = data.CableStock[0].CableQuantity;
            var b = data.CableStock[0].UsedQuantityFromThisBox;


            cableLengthFromDB = a;
            cableUsedFromDB = b;
            cableCanBeUseForThisClientFromDB = a - b;


            $("#lblTotalCableLength").text(cableLengthFromDB).css("display", "block");
            $("#lblUsedCableLength").text(cableUsedFromDB);
            $("#lblDueCableLength").html(cableCanBeUseForThisClientFromDB);

            $("#lblUsedCableLength").css("display", "block");
            $("#lblDueCableLength").css("display", "block");

        }
        if (data.Success === false) {
            AppUtil.ShowError("Sorry Information Not Found.");
        }
    },
    SearchCableQuantityStockedByCableBoxOrDrumNameError: function (data) {

        AppUtil.ShowError("Error");
        console.log(data);
    },
}
