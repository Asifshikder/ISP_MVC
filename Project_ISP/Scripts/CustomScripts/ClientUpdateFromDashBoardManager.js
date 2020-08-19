var ClientUpdateFromDashBoardManager = {
    
    UpdateClientDetailsValidationFromDashBoard: function () {
        
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
        if (AppUtil.GetIdValue("popsOccupation") === '') {
            AppUtil.ShowSuccess("Please Insert Occupation.");
            return false;
        }
        //if (AppUtil.GetIdValue("popsSocialCommunicationURL") === '') {
        //    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("popsRemarks") === '') {
            AppUtil.ShowSuccess("Please Insert Remarks.");
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
        //if (AppUtil.GetIdValue("popsReference") === '') {
        //    AppUtil.ShowSuccess("Please Insert Reference.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("popsNationalID") === '') {
            AppUtil.ShowSuccess("Please Select National Id.");
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
        if (AppUtil.GetIdValue("popsSecurityQuestionAnswer") === '') {
            AppUtil.ShowSuccess("Please Insert Security Question Answer.");
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
        if (AppUtil.GetIdValue("popsClientSurvey") === '') {
            AppUtil.ShowSuccess("Please Insert Client Survey.");
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

    GetClientDetailsByIDDashBoard: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/CLient/GetClientDetailsByID/";
        var Data = ({ ClientDetailsID: id });
        Data = ClientUpdataeFromSeveralPageManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ClientUpdataeFromSeveralPageManager.GetClientDetailsByIDSuccessDashBoard, ClientUpdataeFromSeveralPageManager.GetClientDetailsByIDErrorDashBoard);
        //  }, 500);



    },
    GetClientDetailsByIDSuccessDashBoard: function (data) {

        
        //var ClientDetails = JSON.parse(data.ClientLineStatus);
        //var Transaction = (data.Transaction);

        var ClientDetails = (data.ClientLineStatus);
        var Transaction = (data.Transaction);

        console.log("Transaction: " + Transaction);
        console.log("ClientLineStatus: " + ClientDetails);

        ClientDetailsID = ClientDetails[0].ClientDetailsID;
        ClientLineStatusID = ClientDetails[0].ClientLineStatusID;
        // ClientBannedStatusID;
        ClientTransactionID = Transaction[0].TransactionID;

        $("#popsName").val(ClientDetails[0].Name);
        $("#popsEmail").val(ClientDetails[0].Email);
        $("#popsLoginName").val(ClientDetails[0].LoginName);
        $("#popsPassword").val(ClientDetails[0].Password);
        $("#popsAddress").val(ClientDetails[0].Address);
        $("#popsContactNumber").val(ClientDetails[0].ContactNumber);
        $("#popsZoneID").val(ClientDetails[0].ZoneID);
        $("#popsSMSCommunication").val(ClientDetails[0].SMSCommunication);
        $("#popsOccupation").val(ClientDetails[0].Occupation);
        $("#popsSocialCommunicationURL").val(ClientDetails[0].SocialCommunicationURL);
        $("#popsRemarks").val(ClientDetails[0].Remarks);
        $("#popsConnectionTypeID").val(ClientDetails[0].ConnectionTypeID);
        $("#popsBoxNumber").val(ClientDetails[0].BoxNumber);
        $("#popsPopDetails").val(ClientDetails[0].PopDetails);
        $("#popsRequireCable").val(ClientDetails[0].RequireCable);
        $("#popsCableTypeID").val(ClientDetails[0].CableTypeID);
        $("#popsReference").val(ClientDetails[0].Reference);
        $("#popsNationalID").val(ClientDetails[0].NationalID);
        $("#popsPackageID").val(ClientDetails[0].PackageID);
        $("#popsSingUpFee").val(Transaction[0].PaymentAmount);
        $("#popsSecurityQuestionID").val(ClientDetails[0].SecurityQuestionID);
        $("#popsSecurityQuestionAnswer").val(ClientDetails[0].SecurityQuestionAnswer);
        $("#popsMacAddress").val(ClientDetails[0].MacAddress);
        //$("#popsBillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        $("#popsBillPaymentDate").val(AppUtil.ParseDate(Transaction[0].PaymentDate));
        $("#popsClientSurvey").val(ClientDetails[0].ClientSurvey);
        $("#popsConnectionDate").val(AppUtil.ParseDate(ClientDetails[0].ConnectionDate));

        // $("#popsBannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#popsLineStatusID").val(ClientDetails[0].LineStatusID);
        $("#popsReason").val(ClientDetails[0].StatusChangeReason);


        $("#ResellerID").val(ClientDetails.ResellerID);
        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDErrorDashBoard: function (data) {

        //AppUtil.HideWaitingDialog();
        
        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    UpdateClientDetailsFromDashBoard: function () {

        
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
        var BillPaymentDate = AppUtil.getDateTime("popBillPaymentDate")//$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("popsClientSurvey");
        var ConnectionDate = AppUtil.getDateTime("popsConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var LineStatusID = AppUtil.GetIdValue("popsLineStatusID");
        var Reason = AppUtil.GetIdValue("popsReason");

        _ClientName = Name;
        _ClientLoginName = LoginName;
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
           , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
           , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
           , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate
        };
        var Transaction = { TransactionID: ClientTransactionID, PaymentDate: BillPaymentDate, PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus });
        AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsFromDashBoardSuccess, ClientUpdataeFromSeveralPageManager.UpdateClientDetailsFromDashBoardFail);
        //    }, 50);
    },
    UpdateClientDetailsFromDashBoardSuccess: function (data) {
        

        if (data.LoginNameExist === true) {
            AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        }
        if (data.LoginNameExist === false) {
            var ClientStatusHtml = "";

            var ClientLineStatus = (data.ClientLineStatus);
            var parseEmployee = (data.ClientDetails);

            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(2)").html(parseEmployee[0].Name);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(3)").html(parseEmployee[0].LoginName);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(4)").html(ClientLineStatus[0].Package.PackageName);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(5)").html(parseEmployee[0].Address);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(6)").html(parseEmployee[0].Zone.ZoneName);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(7)").html(parseEmployee[0].ContactNumber);
            $('#tblAllClientsDashBoard tbody>tr:eq(' + rowIndexDB + ')').find("td:eq(8)").html(ClientLineStatus[0].LineStatus.LineStatusName);



            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }
        }

        ClientUpdataeFromSeveralPageManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        //$("#Status").css("display", "none");

        //_ClientName = "";
        //_ClientLoginName = "";
        //T_ID = "";

        $("#tblEmployeeDetailsDashBoard").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsFromDashBoardFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },
}