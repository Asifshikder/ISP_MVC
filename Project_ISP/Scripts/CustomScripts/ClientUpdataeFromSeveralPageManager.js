var ClientUpdataeFromSeveralPageManager = {


    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    UpdateClientDetailsValidation: function () {
        
        //if (AppUtil.GetIdValue("popsOccupation") === '') {
        //    AppUtil.ShowSuccess("Please Insert Occupation.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsRemarks") === '') {
        //    AppUtil.ShowSuccess("Please Insert Remarks.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsReference") === '') {
        //    AppUtil.ShowSuccess("Please Insert Reference.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsSecurityQuestionAnswer") === '') {
        //    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsClientSurvey") === '') {
        //    AppUtil.ShowSuccess("Please Insert Client Survey.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsNationalID") === '') {
        //    AppUtil.ShowSuccess("Please Select National Id.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsSocialCommunicationURL") === '') {
        //    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        //    return false;
        //}


        if (AppUtil.GetIdValue("popsName") === '') {
            AppUtil.ShowSuccess("Please Insert Name.");
            return false;
        }

        //if (AppUtil.GetIdValue("popsEmail") === '') {
        //    AppUtil.ShowSuccess("Please Insert Email.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("popsLoginName") === '') {
            AppUtil.ShowSuccess("Please Insert LoginName.");
            return false;
        }
        if (AppUtil.GetIdValue("popsPassword") === '') {
            AppUtil.ShowSuccess("Please Insert Password.");
            return false;
        }
        if (AppUtil.GetIdValue("popsAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("popsContactNumber") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Number.");
            return false;
        }
        if (AppUtil.GetIdValue("popsZoneID") === '') {
            AppUtil.ShowSuccess("Please Select Zone.");
            return false;
        }
        if (AppUtil.GetIdValue("popsSMSCommunication") === '') {
            AppUtil.ShowSuccess("Please Insert SMS Communication.");
            return false;
        }
        if (AppUtil.GetIdValue("popsConnectionTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Connection Type.");
            return false;
        }
        //if (AppUtil.GetIdValue("popsBoxNumber") === '') {
        //    AppUtil.ShowSuccess("Please Insert Box Number.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("popsPopDetails") === '') {
        //    AppUtil.ShowSuccess("Please Insert Pop Details.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("popsRequireCable") === '') {
            AppUtil.ShowSuccess("Please Insert Require Cable.");
            return false;
        }
        if (AppUtil.GetIdValue("popsCableTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Cable Type.");
            return false;
        }
        if (AppUtil.GetIdValue("popsPackageID") === '') {
            AppUtil.ShowSuccess("Please Select Package.");
            return false;
        }
        if (AppUtil.GetIdValue("popsSingUpFee") === '') {
            AppUtil.ShowSuccess("Please Insert SignUp Fee.");
            return false;
        }
        if (AppUtil.GetIdValue("popsSecurityQuestionID") === '') {
            AppUtil.ShowSuccess("Please Select Security Question.");
            return false;
        }
        if (AppUtil.GetIdValue("popsMacAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Mac Address.");
            return false;
        }
        if (AppUtil.GetIdValue("popsBillPaymentDate") === '') {
            AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
            return false;
        }
        if (AppUtil.GetIdValue("popsConnectionDate") === '') {
            AppUtil.ShowSuccess("Please Insert Connection Date.");
            return false;
        }

        if (AppUtil.GetIdValue("popsLineStatusID") === '') {
            AppUtil.ShowSuccess("Please Select Line Status.");
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
        Data = ClientUpdataeFromSeveralPageManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdataeFromSeveralPageManager.GetClientDetailsByIDSuccess, ClientUpdataeFromSeveralPageManager.GetClientDetailsByIDError);
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


        $("#txtItemAndCablesAssign").html(itemList);
        $("#popsName").val(ClientDetails.Name);
        $("#popsEmail").val(ClientDetails.Email);
        $("#popsLoginName").val(ClientDetails.LoginName);
        $("#popsPassword").val(ClientDetails.Password);
        $("#popsAddress").val(ClientDetails.Address);
        $("#popsContactNumber").val(ClientDetails.ContactNumber);
        $("#popsZoneID").val(ClientDetails.ZoneID);
        $("#popsSMSCommunication").val(ClientDetails.SMSCommunication);
        $("#popsOccupation").val(ClientDetails.Occupation);
        $("#popsSocialCommunicationURL").val(ClientDetails.SocialCommunicationURL);
        $("#popsRemarks").val(ClientDetails.Remarks);
        $("#popsConnectionTypeID").val(ClientDetails.ConnectionTypeID);
        $("#popsBoxNumber").val(ClientDetails.BoxNumber);
        $("#popsPopDetails").val(ClientDetails.PopDetails);
        $("#popsRequireCable").val(ClientDetails.RequireCable);
        $("#popsCableTypeID").val(ClientDetails.CableTypeID);
        $("#popsReference").val(ClientDetails.Reference);
        $("#popsNationalID").val(ClientDetails.NationalID);
        $("#popsPackageID").val(ClientDetails.PackageID);
        $("#popsSingUpFee").val(Transaction[0].PaymentAmount);
        $("#popsSecurityQuestionID").val(ClientDetails.SecurityQuestionID);
        $("#popsSecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
        $("#popsMacAddress").val(ClientDetails.MacAddress);
        //$("#popsBillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        $("#popsBillPaymentDate").val(ClientDetails.PaymentDate);
        $("#popsClientSurvey").val(ClientDetails.ClientSurvey);
        $("#popsConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
        $("#PopsLineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

        // $("#popsBannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#popsLineStatusID").val(ClientDetails.LineStatusID);
        $("#popsReason").val(ClientDetails.StatusChangeReason);

        $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

        $("#lstMikrotik").val(ClientDetails.MikrotikID);
        $("#IP").val(ClientDetails.IP);
        $("#Mac").val(ClientDetails.Mac);

        $("#ResellerID").val(ClientDetails.ResellerID);

        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDError: function (data) {

        //AppUtil.HideWaitingDialog();
        
        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },



    UpdateClientDetailsPopUpFromSignUpOtherPages: function () {

        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        
        //var url = "/Client/UpdateClientDetails/";
        var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";


        var Name = AppUtil.GetIdValue("popsName");

        var Email = AppUtil.GetIdValue("popsEmail");
        var LoginName = AppUtil.GetIdValue("popsLoginName");
        var Password = AppUtil.GetIdValue("popsPassword");
        var Address = AppUtil.GetIdValue("popsAddress");
        var ContactNumber = AppUtil.GetIdValue("popsContactNumber");
        var ZoneID = AppUtil.GetIdValue("popsZoneID");
        var SMSCommunication = AppUtil.GetIdValue("popsSMSCommunication");
        var Occupation = AppUtil.GetIdValue("popsOccupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("popsSocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("popsRemarks");
        var ConnectionTypeID = AppUtil.GetIdValue("popsConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("popsBoxNumber");
        var PopDetails = AppUtil.GetIdValue("popsPopDetails");
        var RequireCable = AppUtil.GetIdValue("popsRequireCable");
        var CableTypeID = AppUtil.GetIdValue("popsCableTypeID");
        var Reference = AppUtil.GetIdValue("popsReference");
        var NationalID = AppUtil.GetIdValue("popsNationalID");
        var PackageID = AppUtil.GetIdValue("popsPackageID");
        var SingUpFee = AppUtil.GetIdValue("popsSingUpFee");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("popsSecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("popsSecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("popsMacAddress");
        var BillPaymentDate = $('#popsBillPaymentDate').val();;//AppUtil.getDateTime("popsBillPaymentDate");
        var ClientSurvey = AppUtil.GetIdValue("popsClientSurvey");
        var ConnectionDate = $('#popsConnectionDate').val();//AppUtil.getDateTime("popsConnectionDate"); //('#ConnectionDate').datepicker('getDate');
        //var ConnectionDate = AppUtil.getDateTime("popsConnectionDate");


        var LineStatusWillActiveInThisDate = $("#PopsLineStatusActiveDate").val();

        var LineStatusID = AppUtil.GetIdValue("popsLineStatusID");
        var Reason = AppUtil.GetIdValue("popsReason");

        var IsPriorityClient = $("#IsPriorityClient").is(':checked');


        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");

        _ClientName = Name;
        _ClientLoginName = LoginName;
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate
            , ResellerID: ResellerID
        };
        var Transaction = { TransactionID: ClientTransactionID/*, PaymentDate: BillPaymentDate*/, PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromSignUpOtherPagesSuccess, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromSignUpOtherPagesFail);
        //    }, 50);
    },
    UpdateClientDetailsPopUpFromSignUpOtherPagesSuccess: function (data) {
        

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }
        if (data.RemoveMikrotikInformation === true) {
            AppUtil.ShowError("Remove Information from mikrotik for this Name : " + data.MKUserName);
        }

        if (data.ThisIsSignUpBill === true) {
            AppUtil.ShowError(
                "Sorry You cant change client information from here. Change From all client list.");
        }
        if (data.StatusIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the Status is remaing same. please chage the Status.");
        }
        if (data.PackageIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the package is remaing same. please chage the package.");
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

            var ClientStatusHtml = "";

            if (data.ClientLineStatus) {
                var ClientLineStatus = (data.ClientLineStatus);
                if (ClientLineStatus === 3) {
                    ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                }
                if (ClientLineStatus === 4) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                }
                if (ClientLineStatus === 5) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                }
            }

            if (data.ClientDetails) {
                var parseEmployee = (data.ClientDetails);
                $("#tblClientMonthlyBill>tbody>tr").each(function () {
                    
                    var index = $(this).index();
                    var transactionID = $(this).find("td:eq(0) input").val();
                    if (transactionID == T_ID) {
                        //<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(3119)" type="checkbox" value="true">


                        if (parseEmployee[0].IsPriorityClient) {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }
                        //if (data.TransactionID > 0) {
                        //    $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(0)").html('<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(' + data.TransactionID + ')" type="checkbox" value="true"></a>');
                        //}
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');

                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(2)").html(parseEmployee[0].Address);
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(3)").html(parseEmployee[0].ContactNumber);;
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(5)").html(data.ClientPackage);
                        }
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(data.packageChangeAmountCalculation);
                        }

                        $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(16)").html(data.LineStatusActiveDate);
                    }
                });
            }

            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }

            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //  $("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";

        }

        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);
        ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");

        _ClientName = "";
        _ClientLoginName = "";
        T_ID = "";
        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsPopUpFromSignUpOtherPagesFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },


    UpdateClientDetailsPopUpFromUnpaidOtherPages: function () {

        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        
        //var url = "/Client/UpdateClientDetails/";
        var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";


        var Name = AppUtil.GetIdValue("popsName");

        var Email = AppUtil.GetIdValue("popsEmail");
        var LoginName = AppUtil.GetIdValue("popsLoginName");
        var Password = AppUtil.GetIdValue("popsPassword");
        var Address = AppUtil.GetIdValue("popsAddress");
        var ContactNumber = AppUtil.GetIdValue("popsContactNumber");
        var ZoneID = AppUtil.GetIdValue("popsZoneID");
        var SMSCommunication = AppUtil.GetIdValue("popsSMSCommunication");
        var Occupation = AppUtil.GetIdValue("popsOccupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("popsSocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("popsRemarks");
        var ConnectionTypeID = AppUtil.GetIdValue("popsConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("popsBoxNumber");
        var PopDetails = AppUtil.GetIdValue("popsPopDetails");
        var RequireCable = AppUtil.GetIdValue("popsRequireCable");
        var CableTypeID = AppUtil.GetIdValue("popsCableTypeID");
        var Reference = AppUtil.GetIdValue("popsReference");
        var NationalID = AppUtil.GetIdValue("popsNationalID");
        var PackageID = AppUtil.GetIdValue("popsPackageID");
        var SingUpFee = AppUtil.GetIdValue("popsSingUpFee");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("popsSecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("popsSecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("popsMacAddress");
        var BillPaymentDate = $('#popsBillPaymentDate').val();;//AppUtil.getDateTime("popsBillPaymentDate");
        var ClientSurvey = AppUtil.GetIdValue("popsClientSurvey");
        var ConnectionDate = $('#popsConnectionDate').val();//AppUtil.getDateTime("popsConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var LineStatusWillActiveInThisDate = $("#PopsLineStatusActiveDate").val();
        //var ConnectionDate = AppUtil.getDateTime("popsConnectionDate");
        var LineStatusID = AppUtil.GetIdValue("popsLineStatusID");
        var Reason = AppUtil.GetIdValue("popsReason");

        var IsPriorityClient = $("#IsPriorityClient").is(':checked');


        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");

        _ClientName = Name;
        _ClientLoginName = LoginName;
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate
            , ResellerID: ResellerID
        };
        var Transaction = { TransactionID: ClientTransactionID, /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromUnpaidOtherPagesSuccess, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromUnpaidOtherPagesFail);
        //    }, 50);
    },
    UpdateClientDetailsPopUpFromUnpaidOtherPagesSuccess: function (data) {
        

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }
        if (data.RemoveMikrotikInformation === true) {
            AppUtil.ShowError("Remove Information from mikrotik for this Name : " + data.MKUserName);
        }

        if (data.ThisIsSignUpBill === true) {
            AppUtil.ShowError(
                "Sorry You cant change client information from here. Change From all client list.");
        }
        if (data.StatusIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the Status is remaing same. please chage the Status.");
        }
        if (data.PackageIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the package is remaing same. please chage the package.");
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

            var ClientStatusHtml = "";

            if (data.ClientLineStatus) {
                var ClientLineStatus = (data.ClientLineStatus);
                if (ClientLineStatus === 3) {
                    ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                }
                if (ClientLineStatus === 4) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                }
                if (ClientLineStatus === 5) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                }
            }

            if (data.ClientDetails) {
                var parseEmployee = (data.ClientDetails);
                $("#tblClientMonthlyBill>tbody>tr").each(function () {
                    
                    var index = $(this).index();
                    var transactionID = $(this).find("td:eq(3) input").val();
                    if (transactionID == T_ID) {
                        //<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(3119)" type="checkbox" value="true">


                        if (parseEmployee[0].IsPriorityClient) {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }
                        //if (data.TransactionID > 0) {
                        //    $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(0)").html('<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(' + data.TransactionID + ')" type="checkbox" value="true"></a>');
                        //}
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(4)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');

                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(5)").html(parseEmployee[0].Address);
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(6)").html(parseEmployee[0].ContactNumber);;
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(7)").html(parseEmployee[0].ZoneName);
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(8)").html(data.ClientPackage);
                        }
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(9)").text(data.packageChangeAmountCalculation);
                        }

                        $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(18)").html(data.LineStatusActiveDate);

                    }
                });
            }

            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }
            
            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //  $("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";

        }

        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);

        ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");

        _ClientName = "";
        _ClientLoginName = "";
        T_ID = "";
        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsPopUpFromUnpaidOtherPagesFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    UpdateClientDetailsPopUpFromOtherPages: function () {

        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        
        //var url = "/Client/UpdateClientDetails/";

        var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";


        var Name = AppUtil.GetIdValue("popsName");

        var Email = AppUtil.GetIdValue("popsEmail");
        var LoginName = AppUtil.GetIdValue("popsLoginName");
        var Password = AppUtil.GetIdValue("popsPassword");
        var Address = AppUtil.GetIdValue("popsAddress");
        var ContactNumber = AppUtil.GetIdValue("popsContactNumber");
        var ZoneID = AppUtil.GetIdValue("popsZoneID");
        var SMSCommunication = AppUtil.GetIdValue("popsSMSCommunication");
        var Occupation = AppUtil.GetIdValue("popsOccupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("popsSocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("popsRemarks");
        var ConnectionTypeID = AppUtil.GetIdValue("popsConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("popsBoxNumber");
        var PopDetails = AppUtil.GetIdValue("popsPopDetails");
        var RequireCable = AppUtil.GetIdValue("popsRequireCable");
        var CableTypeID = AppUtil.GetIdValue("popsCableTypeID");
        var Reference = AppUtil.GetIdValue("popsReference");
        var NationalID = AppUtil.GetIdValue("popsNationalID");
        var PackageID = AppUtil.GetIdValue("popsPackageID");
        var SingUpFee = AppUtil.GetIdValue("popsSingUpFee");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("popsSecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("popsSecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("popsMacAddress");
        var BillPaymentDate = $('#popsBillPaymentDate').val();;//AppUtil.getDateTime("popsBillPaymentDate");
        var ClientSurvey = AppUtil.GetIdValue("popsClientSurvey");
        var ConnectionDate = $('#popsConnectionDate').val();//AppUtil.getDateTime("popsConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var LineStatusWillActiveInThisDate = $("#PopsLineStatusActiveDate").val();

        //var ConnectionDate = AppUtil.getDateTime("popsConnectionDate");
        var LineStatusID = AppUtil.GetIdValue("popsLineStatusID");
        var Reason = AppUtil.GetIdValue("popsReason");
        var IsPriorityClient = $("#IsPriorityClient").is(':checked');


        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");

        _ClientName = Name;
        _ClientLoginName = LoginName;
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient, ApproxPaymentDate: BillPaymentDate
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac
            , ResellerID: ResellerID
        };
        var Transaction = { TransactionID: ClientTransactionID, /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee };
        var ClientLineStatus = { MikrotikID: MikrotikID, ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromOtherPagesSuccess, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromOtherPagesFail);
        //    }, 50);
    },
    /////////////////////////////////////////////////////////////////////
    UpdateClientDetailsPopUpFromOtherPagesSuccess: function (data) {
        
        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }
        if (data.RemoveMikrotikInformation === true) {
            AppUtil.ShowError("Remove Information from mikrotik for this Name : " + data.MKUserName);
        }

        if (data.ThisIsSignUpBill === true) {
            AppUtil.ShowError(
                "Sorry You cant change client information from here. Change From all client list.");
        }
        if (data.StatusIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the Status is remaing same. please chage the Status.");
        }
        if (data.PackageIsSameButRunningMonthChecked === true) {
            AppUtil.ShowError(
                "Sorry you checked running month but the package is remaing same. please chage the package.");
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

            var ClientStatusHtml = "";

            if (data.ClientLineStatus) {
                var ClientLineStatus = (data.ClientLineStatus);
                if (ClientLineStatus === 3) {
                    ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                }
                if (ClientLineStatus === 4) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                }
                if (ClientLineStatus === 5) {
                    ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                }
            }

            if (data.ClientDetails) {
                var parseEmployee = (data.ClientDetails);
                $("#tblClientMonthlyBill>tbody>tr").each(function () {
                    
                    var index = $(this).index();
                    var transactionID = $(this).find("td:eq(1) input").val();
                    if (transactionID == T_ID) {
                        //<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(3119)" type="checkbox" value="true">


                        if (parseEmployee[0].IsPriorityClient) {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                        }
                        else {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                        }

                        if (data.TransactionID > 0) {
                            $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(0)").html('<input class="checkGroup1" id="chkBillPay" name="chkBillPay" onclick="setCompleteStatus(' + data.TransactionID + ')" type="checkbox" value="true"></a>');

                        }
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
                        $('#tblClientMonthlyBill tbody>tr:eq(' + index + ')').find("td:eq(5)").html(parseEmployee[0].ContactNumber);;

                        //var parseEmployee = (data.ClientDetails);
                        ////    $("#tblFilterBill>tbody>tr").each(function () {
                        //

                        //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                        //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                        //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                        //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);
                        if (data.chkStatusFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(ClientStatusHtml);
                        }
                        if (data.MikrotikUserInsert === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(6)").html(data.ClientPackage);
                        }
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(6)").html(data.ClientPackage);
                        }
                        if (data.chkPackageFromRunningMonth === true) {
                            $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(data.packageChangeAmountCalculation);
                        }

                        $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(16)").html(data.LineStatusActiveDate);
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(11)").html("");
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(12)").html("");
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(13)").html("");
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(14)").html("");
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(15)").html("");
                        //$('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(16)").html('<div style="float: left"><button type="button" id="btnPrint" class="btn btn-success  btn-sm"><span class="glyphicon glyphicon-print"></span></button>\
                        //                                                                              <div style="float: right"><button type="button" id="" class="btn btn-danger btn-sm" title="Not Paid"><span class="glyphicon glyphicon-remove"></span></button></div>\
                        //    </div>');
                    }
                });
            }

            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }

            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //  $("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";

        }

        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);

        ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");

        _ClientName = "";
        _ClientLoginName = "";
        T_ID = "";
        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsPopUpFromOtherPagesFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    UpdateClientDetailsPopUpFromOtherPagesWithPageNumber: function () {



        var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        
        var url = "/Client/UpdateClientDetails/";


        var Name = AppUtil.GetIdValue("popsName");

        var Email = AppUtil.GetIdValue("popsEmail");
        var LoginName = AppUtil.GetIdValue("popsLoginName");
        var Password = AppUtil.GetIdValue("popsPassword");
        var Address = AppUtil.GetIdValue("popsAddress");
        var ContactNumber = AppUtil.GetIdValue("popsContactNumber");
        var ZoneID = AppUtil.GetIdValue("popsZoneID");
        var SMSCommunication = AppUtil.GetIdValue("popsSMSCommunication");
        var Occupation = AppUtil.GetIdValue("popsOccupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("popsSocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("popsRemarks");
        var ConnectionTypeID = AppUtil.GetIdValue("popsConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("popsBoxNumber");
        var PopDetails = AppUtil.GetIdValue("popsPopDetails");
        var RequireCable = AppUtil.GetIdValue("popsRequireCable");
        var CableTypeID = AppUtil.GetIdValue("popsCableTypeID");
        var Reference = AppUtil.GetIdValue("popsReference");
        var NationalID = AppUtil.GetIdValue("popsNationalID");
        var PackageID = AppUtil.GetIdValue("popsPackageID");
        var SingUpFee = AppUtil.GetIdValue("popsSingUpFee");                              //////
        var SecurityQuestionID = AppUtil.GetIdValue("popsSecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("popsSecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("popsMacAddress");
        var BillPaymentDate = $("#popsBillPaymentDate").val(); 
        var ClientSurvey = AppUtil.GetIdValue("popsClientSurvey");
        var ConnectionDate = $("#popsConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
        var LineStatusWillActiveInThisDate = $("#PopsLineStatusActiveDate").val();

        var LineStatusID = AppUtil.GetIdValue("popsLineStatusID");
        var Reason = AppUtil.GetIdValue("popsReason");


        var IsPriorityClient = $("#IsPriorityClient").is(':checked');

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");


        var ResellerID = AppUtil.GetIdValue("ResellerID");

        _ClientName = Name;
        _ClientLoginName = LoginName;
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate, IsPriorityClient: IsPriorityClient
            , MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate
            , ResellerID: ResellerID

        };
        var Transaction = { TransactionID: ClientTransactionID/*, PaymentDate: BillPaymentDate*/, PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromOtherPagesWithPageNumberSuccess, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsPopUpFromOtherPagesWithPageNumberFail);
        //    }, 50);
    },
    UpdateClientDetailsPopUpFromOtherPagesWithPageNumberSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();

        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        }
        if (data.RemoveMikrotikInformation === true) {
            AppUtil.ShowError("Remove Information from mikrotik for this Name : " + data.MKUserName);
        }

        if (pageID == "69")//F48
        {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblRunningList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblRunningList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblRunningList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                }
                $('#tblRunningList>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(8)").html(data.LineStatusActiveDate);

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }
            }




            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //$("#Status").css("visibility", false);
            $("#Status").css("display", "none");

            _ClientName = "";
            _ClientLoginName = "";
            T_ID = "";
        }


        if (pageID == "166") {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);
                    $('#tblCableAssignedList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');

                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ContactNumber);;


                    $('#tblCableAssignedList>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html(data.LineStatusActiveDate);

                }
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }



            }



            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //$("#Status").css("visibility", false);
            $("#Status").css("display", "none");

            _ClientName = "";
            _ClientLoginName = "";
            T_ID = "";
        }

        if (pageID == "66") {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblCableAssignedList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblCableAssignedList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblCableAssignedList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');

                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
                    //$('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ContactNumber);;


                    $('#tblCableAssignedList>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(9)").html(data.LineStatusActiveDate);
                }

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }



            }



            ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            //$("#Status").css("visibility", false);
            $("#Status").css("display", "none");

            _ClientName = "";
            _ClientLoginName = "";
            T_ID = "";
        }//F52

        if (pageID == "24") {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                    $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
                    $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ContactNumber);;

                    //var parseEmployee = (data.ClientDetails);
                    ////    $("#tblFilterBill>tbody>tr").each(function () {
                    //

                    //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                    //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                    //$('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);
                    //if (data.chkPackageFromRunningMonth === true) {
                    //    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html(data.ClientPackage);
                    //}
                    //if (data.chkPackageFromRunningMonth === true) {
                    //    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").text(data.packageChangeAmountCalculation);
                    //}

                }

                $('#tblComplainList>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(14)").html(data.LineStatusActiveDate);
                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }

                ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                //$("#Status").css("display", "none");

                _ClientName = "";
                _ClientLoginName = "";
                T_ID = "";

            }


            //if (data.LoginNameExist === true) {
            //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
            //}
            //if (data.LoginNameExist === false) {
            //    var ClientStatusHtml = "";
            //    if (data.ClientLineStatus) {
            //        var ClientLineStatus = (data.ClientLineStatus);
            //    }

            //    if (data.ClientDetails) {
            //        var parseEmployee = (data.ClientDetails);

            //        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
            //        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
            //        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);
            //        $('#tblComplainList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ContactNumber);;

            //    }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            //    }
            //}

            //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            ////$("#Status").css("visibility", false);
            //$("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";
        }//F19

        if (pageID == "16") {
            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);
                    //    $("#tblFilterBill>tbody>tr").each(function () {
                    

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblClientMonthlyBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html(data.ClientPackage);
                    }
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblClientMonthlyBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").text(data.packageChangeAmountCalculation);
                    }

                    $('#tblClientMonthlyBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(10)").html(data.LineStatusActiveDate);
                }

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }

                ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                //$("#Status").css("visibility", false);
                $("#Status").css("display", "none");

                _ClientName = "";
                _ClientLoginName = "";
                T_ID = "";

                //if (data.LoginNameExist === true) {
                //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
                //}
                //if (data.LoginNameExist === false) {
                //    var ClientStatusHtml = "";
                //    if (data.ClientLineStatus) {
                //        var ClientLineStatus = (data.ClientLineStatus);
                //    }

                //    if (data.ClientDetails) {
                //        var parseEmployee = (data.ClientDetails);

                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);

            }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            //    }
            //}

            //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            ////$("#Status").css("visibility", false);
            //$("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";
        }//F12

        if (pageID == "17") {
            //if (data.LoginNameExist === true) {
            //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
            //}
            //if (data.LoginNameExist === false) {
            //    var ClientStatusHtml = "";
            //    if (data.ClientLineStatus) {
            //        var ClientLineStatus = (data.ClientLineStatus);
            //    }

            //    if (data.ClientDetails) {
            //        var parseEmployee = (data.ClientDetails);

            //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
            //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
            //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
            //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);

            //    }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            //    }
            //}

            //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            ////$("#Status").css("visibility", false);
            //$("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);
                    //    $("#tblFilterBill>tbody>tr").each(function () {
                    

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                    $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblClientMonthlyBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html(data.ClientPackage);
                    }
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblClientMonthlyBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").text(data.packageChangeAmountCalculation);
                    }

                    $('#tblClientMonthlyBill>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(data.LineStatusActiveDate);
                }

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }

                ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                //$("#Status").css("visibility", false);
                $("#Status").css("display", "none");

                _ClientName = "";
                _ClientLoginName = "";
                T_ID = "";

                //if (data.LoginNameExist === true) {
                //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
                //}
                //if (data.LoginNameExist === false) {
                //    var ClientStatusHtml = "";
                //    if (data.ClientLineStatus) {
                //        var ClientLineStatus = (data.ClientLineStatus);
                //    }

                //    if (data.ClientDetails) {
                //        var parseEmployee = (data.ClientDetails);

                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].Address);
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ContactNumber);;
                //        $('#tblClientMonthlyBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(parseEmployee[0].ZoneName);

            }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            //    }
            //}

            //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            ////$("#Status").css("visibility", false);
            //$("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";

        }//f13

        if (pageID == "18") {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    var parseEmployee = (data.ClientDetails);
                    //    $("#tblFilterBill>tbody>tr").each(function () {
                    

                    //var index = $(this).index();
                    //var employeeID = $(this).find("td:eq(0) input").val();
                    //if (employeeID == parseEmployee[0].ClientDetailsID) {
                    

                    if (parseEmployee[0].IsPriorityClient) {
                        //$("#tblUsers>tbody>tr").each(function () {
                        //    

                        //    var index = $(this).index();
                        //    var employeeID = $(this).find("td:eq(0) input").val();
                        //    if (employeeID == data.ClientDetailsID) {
                        //        
                        //        $('#tblUsers tbody>tr:eq(' + index + ')').remove();
                        //    }
                        //});
                        $("#tblFilterBill>tbody>tr").each(function () {
                            
                            var index = $(this).index();
                            var cid = $(this).closest("tr").find("td:eq(0) input").val();
                            if (cid == _CID) {
                                $('#tblFilterBill tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                            }
                        });


                    }
                    else {

                        $("#tblFilterBill>tbody>tr").each(function () {
                            
                            var index = $(this).index();
                            var cid = $(this).closest("tr").find("td:eq(0) input").val();
                            if (cid == _CID) {
                                $('#tblFilterBill tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                            }
                        });
                    }
                    $('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
                    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").text(parseEmployee[0].Address);
                    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").text(parseEmployee[0].ContactNumber);
                    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").text(parseEmployee[0].ZoneName);
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").html(data.ClientPackage);
                    }
                    if (data.chkPackageFromRunningMonth === true) {
                        $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(10)").text(data.packageChangeAmountCalculation);
                    }

                    $("#tblFilterBill>tbody>tr").each(function () {
                        
                        var index = $(this).index();
                        var cid = $(this).closest("tr").find("td:eq(0) input").val();
                        if (cid == _CID) { 
                            $('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(3)").html(data.LineStatusActiveDate);
                        }
                    });


                    //$('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(5)")
                    //    .text(parseEmployee[0].Address);
                    //$('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(6)")
                    //    .text(parseEmployee[0].Email);
                    //$('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(7)")
                    //    .text(parseEmployee[0].ZoneName);
                    //$('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(8)")
                    //    .text(parseEmployee[0].ContactNumber);
                    //if (data.chkStatusFromRunningMonth === true) {
                    //    $('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(9)").html(ClientStatusHtml);
                    //}
                    //$('#tblFilterBill>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(ClientStatusHtml);

                    // }
                    // });

                }

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }


                ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                //$("#Status").css("visibility", false);
                $("#Status").css("display", "none");

                _ClientName = "";
                _ClientLoginName = "";
                T_ID = "";
                _CID = "";

                //if (data.LoginNameExist === true) {
                //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
                //}
                //if (data.LoginNameExist === false) {
                //    var ClientStatusHtml = "";
                //    if (data.ClientLineStatus) {
                //        var ClientLineStatus = (data.ClientLineStatus);
                //    }

                //    if (data.ClientDetails) {
                //        var parseEmployee = (data.ClientDetails);

                //        $('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                //        $('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").html(parseEmployee[0].Address);
                //        $('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").html(parseEmployee[0].ContactNumber);;
                //        $('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").html(parseEmployee[0].ZoneName);

                //    }

                //    if (data.UpdateStatus === true) {
                //        AppUtil.ShowSuccess("Successfully Edited.");
                //    }
                //    if (data.UpdateStatus === false) {
                //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                //    }
                //}

                //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                ////$("#Status").css("visibility", false);
                //$("#Status").css("display", "none");

                //_ClientName = "";
                //_ClientLoginName = "";
                //T_ID = "";
            }
        } //F14

        if (pageID == "20") {

            if (data.ThisIsSignUpBill === true) {
                AppUtil.ShowError(
                    "Sorry You cant change client information from here. Change From all client list.");
            }
            if (data.StatusIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the Status is remaing same. please chage the Status.");
            }
            if (data.PackageIsSameButRunningMonthChecked === true) {
                AppUtil.ShowError(
                    "Sorry you checked running month but the package is remaing same. please chage the package.");
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

                var ClientStatusHtml = "";

                if (data.ClientLineStatus) {
                    var ClientLineStatus = (data.ClientLineStatus);
                    if (ClientLineStatus === 3) {
                        ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
                    }
                    if (ClientLineStatus === 4) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
                    }
                    if (ClientLineStatus === 5) {
                        ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
                    }
                }

                if (data.ClientDetails) {
                    
                    var parseEmployee = (data.ClientDetails);

                    if (parseEmployee[0].IsPriorityClient) {
                        $('#tblViewAdvancePayment tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').addClass('changetrbackground');
                    }
                    else {
                        $('#tblViewAdvancePayment tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').removeClass('changetrbackground');
                    }
                    $('#tblViewAdvancePayment tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');


                $('#tblViewAdvancePayment>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html(data.LineStatusActiveDate);

                    //$('#tblFilterBill tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');
                    //$('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(2)").text(parseEmployee[0].Address);
                    //$('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(3)").text(parseEmployee[0].ContactNumber);
                    //$('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(4)").text(parseEmployee[0].ZoneName);
                    //if (data.chkPackageFromRunningMonth === true) {
                    //    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(5)").html(data.ClientPackage);
                    //}
                    //if (data.chkPackageFromRunningMonth === true) {
                    //    $('#tblFilterBill>tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(8)").text(data.packageChangeAmountCalculation);
                    //}

                }

                if (data.UpdateStatus === true) {
                    AppUtil.ShowSuccess("Successfully Edited.");
                }
                if (data.UpdateStatus === false) {
                    AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
                }


                ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
                //$("#Status").css("visibility", false);
                $("#Status").css("display", "none");

                _ClientName = "";
                _ClientLoginName = "";
                T_ID = "";

            }
            //if (data.LoginNameExist === true) {
            //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
            //}
            //if (data.LoginNameExist === false) {
            //    var ClientStatusHtml = "";
            //    if (data.ClientLineStatus) {
            //        var ClientLineStatus = (data.ClientLineStatus);
            //    }

            //    if (data.ClientDetails) {
            //        var parseEmployee = (data.ClientDetails);

            //        $('#tblViewAdvancePayment tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');


            //    }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
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
            //            

            //            var index = $(this).index();
            //            var employeeID = $(this).find("td:eq(0) input").val();
            //            if (employeeID == parseEmployee[0].ClientDetailsID) {
            //                $('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(1)").text(parseEmployee[0].Name);
            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee[0].LoginName);
            //                if (data.chkPackageFromRunningMonth === true) {

            //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(data.ClientPackage);
            //                }

            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(data.ClientPackage);

            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(parseEmployee[0].Address);
            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee[0].Email);
            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee[0].ZoneName);
            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").text(parseEmployee[0].ContactNumber);
            //                if (data.chkStatusFromRunningMonth === true) {
            //                    $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(9)").html(ClientStatusHtml);
            //                }
            //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(ClientStatusHtml);

            //            }
            //        });
            //    }

            //    if (data.UpdateStatus === true) {
            //        AppUtil.ShowSuccess("Successfully Edited.");
            //    }
            //    if (data.UpdateStatus === false) {
            //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            //    }


            //ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
            ////$("#Status").css("visibility", false);
            //$("#Status").css("display", "none");

            //_ClientName = "";
            //_ClientLoginName = "";
            //T_ID = "";
        }//f17

        //if (pageID == "80") {
        //    if (data.ThisIsSignUpBill === true) {
        //        AppUtil.ShowError(
        //            "Sorry You cant change client information from here. Change From all client list.");
        //    }
        //    if (data.StatusIsSameButRunningMonthChecked === true) {
        //        AppUtil.ShowError(
        //            "Sorry you checked running month but the Status is remaing same. please chage the Status.");
        //    }
        //    if (data.PackageIsSameButRunningMonthChecked === true) {
        //        AppUtil.ShowError(
        //            "Sorry you checked running month but the package is remaing same. please chage the package.");
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

        //        var ClientStatusHtml = "";

        //        if (data.ClientLineStatus) {
        //            var ClientLineStatus = (data.ClientLineStatus);
        //            if (ClientLineStatus === 3) {
        //                ClientStatusHtml = '<div style="color:green;font-weight:bold;">Active</div>';
        //            }
        //            if (ClientLineStatus === 4) {
        //                ClientStatusHtml = '<div style="color:red;font-weight:bold;">InActive</div>';
        //            }
        //            if (ClientLineStatus === 5) {
        //                ClientStatusHtml = '<div style="color:red;font-weight:bold;">Lock</div>';
        //            }
        //        }

        //        if (data.ClientDetails) {
        //            var parseEmployee = (data.ClientDetails);
        //            $('#tblTotalList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(7)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].LoginName + '</a>');
        //        }

        //        if (data.UpdateStatus === true) {
        //            AppUtil.ShowSuccess("Successfully Edited.");
        //        }
        //        if (data.UpdateStatus === false) {
        //            AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
        //        }
        //    }

        //    //if (data.LoginNameExist === true) {
        //    //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        //    //}
        //    //if (data.LoginNameExist === false) {
        //    //    var ClientStatusHtml = "";
        //    //    if (data.ClientLineStatus) {
        //    //        var ClientLineStatus = (data.ClientLineStatus);
        //    //    }

        //    //    if (data.ClientDetails) {
        //    //        var parseEmployee = (data.ClientDetails);
        //    //        $('#tblTotalList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(6)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');

        //    //        // $('#tblTotalList tbody>tr:eq(' + tableRowIndexForUpdateClientName + ')').find("td:eq(1)").html('<a href="#" onclick="GetClientDetailsByClientDetailsID(' + parseEmployee[0].ClientDetailsID + ',' + T_ID + ')">' + parseEmployee[0].Name + '</a>');


        //    //    }

        //    //    if (data.UpdateStatus === true) {
        //    //        AppUtil.ShowSuccess("Successfully Edited.");
        //    //    }
        //    //    if (data.UpdateStatus === false) {
        //    //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
        //    //    }
        //    //}

        //    ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
        //    //$("#Status").css("visibility", false);
        //    $("#Status").css("display", "none");

        //    _ClientName = "";
        //    _ClientLoginName = "";
        //    T_ID = "";
        //}
        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);

        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsPopUpFromOtherPagesWithPageNumberFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },


    ClearClientDetailsModalInformation: function () {
        
        $("#popsName").val("");
        $("#popsEmail").val("");
        $("#popsLoginName").val("");
        $("#popsPassword").val("");
        $("#popsAddress").val("");
        $("#popsContactNumber").val("");
        $("#popsZoneID").val("");
        $("#popsSMSCommunication").val("");
        $("#popsOccupation").val("");
        $("#popsSocialCommunicationURL").val("");
        $("#popsRemarks").val("");
        $("#popsConnectionTypeID").val("");
        $("#popsBoxNumber").val("");
        $("#popsPopDetails").val("");
        $("#popsRequireCable").val("");
        $("#popsCableTypeID").val("");
        $("#popsReference").val("");
        $("#popsNationalID").val("");
        $("#popsPackageID").val("");
        $("#popsSingUpFee").val("");                              //////
        $("#popsSecurityQuestionID").val("");
        $("#popsSecurityQuestionAnswer").val("");
        $("#popsMacAddress").val("");
        $("#popsBillPaymentDate").val("")//$('#BillPaymentDate').val("").datepicker('getDate').val("");
        $("#popsClientSurvey").val("");
        $("#popsConnectionDate").val(""); //('#ConnectionDate').val("").datepicker('getDate').val("");

        $("#popsLineStatusID").val("");
        $("#popsReason").val("");
    },
}