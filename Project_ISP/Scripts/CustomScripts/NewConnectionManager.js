var NewConnectionManager = {
    Validation: function () {

        if (AppUtil.GetIdValue("NewName") === '') {
            AppUtil.ShowSuccess("Please Insert Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewZoneID") === '') {
            AppUtil.ShowSuccess("Please Select Zone.");
            return false;
        }
        if (AppUtil.GetIdValue("NewAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("NewLatitudeLongitude") === '') {
            AppUtil.ShowSuccess("Please Insert LatitudeLongitude.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPackageID") === '') {
            AppUtil.ShowSuccess("Please Select Package.");
            return false;
        }
        if (AppUtil.GetIdValue("NewConnectionTypeID") === '') {
            AppUtil.ShowSuccess("Please Select  Connection Type.");
            return false;
        }
        if (AppUtil.GetIdValue("NewAssignToWhichEmployee") === '') {
            AppUtil.ShowSuccess("Please Select Assign To Which Employee.");
            return false;
        }
        if (AppUtil.GetIdValue("NewContactNumber") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Number.");
            return false;
        }
        if (AppUtil.GetIdValue("NewClientSurvey") === '') {
            AppUtil.ShowSuccess("Please Insert Client Survey.");
            return false;
        }
        return true;


    },

    UpdateClientDetailsValidation: function () {

        if (AppUtil.GetIdValue("NewName") === '') {
            AppUtil.ShowSuccess("Please Insert Name.");
            return false;
        }
        if (AppUtil.GetIdValue("NewZoneID") === '') {
            AppUtil.ShowSuccess("Please Select Zone.");
            return false;
        }
        if (AppUtil.GetIdValue("NewAddress") === '') {
            AppUtil.ShowSuccess("Please Insert Address.");
            return false;
        }
        if (AppUtil.GetIdValue("NewPackageID") === '') {
            AppUtil.ShowSuccess("Please Select Package.");
            return false;
        }
        if (AppUtil.GetIdValue("NewConnectionTypeID") === '') {
            AppUtil.ShowSuccess("Please Select  Connection Type.");
            return false;
        }
        if (AppUtil.GetIdValue("NewAssignToWhichEmployee") === '') {
            AppUtil.ShowSuccess("Please Select Assign To Which Employee.");
            return false;
        }
        if (AppUtil.GetIdValue("NewContactNumber") === '') {
            AppUtil.ShowSuccess("Please Insert Contact Number.");
            return false;
        }
        if (AppUtil.GetIdValue("NewClientSurvey") === '') {
            AppUtil.ShowSuccess("Please Insert Client Survey.");
            return false;
        }
        return true;
    },

    UpdateClientDetailsSignUpValidation: function () {
        
        ////if (AppUtil.GetIdValue("Occupation") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Occupation.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("Remarks") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Remarks.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("Reference") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Reference.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SecurityQuestionAnswer") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Security Question Answer.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("ClientSurvey") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Client Survey.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("NationalID") === '') {
        ////    AppUtil.ShowSuccess("Please Insert NationalID.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("SocialCommunicationURL") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Social Communication URL.");
        ////    return false;
        ////}
        //if (AppUtil.GetIdValue("Name") === '') {
        //    AppUtil.ShowSuccess("Please Insert Name.");
        //    return false;
        //}
        ////if (AppUtil.GetIdValue("Email") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Email.");
        ////    return false;
        ////}
        //if (AppUtil.GetIdValue("LoginName") === '') {
        //    AppUtil.ShowSuccess("Please Insert Login Name.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("Password") === '') {
        //    AppUtil.ShowSuccess("Please Insert Password.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("Address") === '') {
        //    AppUtil.ShowSuccess("Please Insert Address.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ContactNumber") === '') {
        //    AppUtil.ShowSuccess("Please Insert Contact Number.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ZoneID") === '') {
        //    AppUtil.ShowSuccess("Please Select Zone.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("SMSCommunication") === '') {
        //    AppUtil.ShowSuccess("Please Insert SMS Communication.");
        //    return false;
        //}

        //if (AppUtil.GetIdValue("ConnectionTypeID") === '') {
        //    AppUtil.ShowSuccess("Please Select Connection Type.");
        //    return false;
        //}
        ////if (AppUtil.GetIdValue("BoxNumber") === '') {
        ////    AppUtil.ShowSuccess("Please Insert Box Number.");
        ////    return false;
        ////}
        ////if (AppUtil.GetIdValue("PopDetails") === '') {
        ////    AppUtil.ShowSuccess("Please Insert PopDetails.");
        ////    return false;
        ////}
        //if (AppUtil.GetIdValue("RequireCable") === '') {
        //    AppUtil.ShowSuccess("Please Insert Require Cable.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("CableTypeID") === '') {
        //    AppUtil.ShowSuccess("Please Select Cable Type.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("PackageID") === '') {
        //    AppUtil.ShowSuccess("Please Select Package.");
        //    return false;
        //}

        //if (AppUtil.GetIdValue("SingUpFee") === '') {
        //    AppUtil.ShowSuccess("Please Insert Sing Up Fee.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("SecurityQuestionID") === '') {
        //    AppUtil.ShowSuccess("Please Select Security Question.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("MacAddress") === '') {
        //    AppUtil.ShowSuccess("Please Insert Mac Address.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("BillPaymentDate") === '') {
        //    AppUtil.ShowSuccess("Bill payment Date must be between 1 and 31.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("ConnectionDate") === '') {
        //    AppUtil.ShowSuccess("Please Insert Connection Date.");
        //    return false;
        //}
        ////if (AppUtil.GetIdValue("BannedID") === '') {
        ////    AppUtil.ShowSuccess("Please Select Banned.");
        ////    return false;
        ////}
        //if (AppUtil.GetIdValue("LineStatusID") === '') {
        //    AppUtil.ShowSuccess("Please Select Line Status.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("Reason") === '') {
        //    AppUtil.ShowSuccess("Please Insert Reason.");
        //    return false;
        //}

        //return true;

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
    },

    InsertNewConnection: function () {
        var Name = AppUtil.GetIdValue("NewName");
        var ZoneID = AppUtil.GetIdValue("NewZoneID");
        var Address = AppUtil.GetIdValue("NewAddress");
        var LatitudeLongitude = AppUtil.GetIdValue("NewLatitudeLongitude");
        var PackageID = AppUtil.GetIdValue("NewPackageID");
        var ConnectionTypeID = AppUtil.GetIdValue("NewConnectionTypeID");
        var AssignToWhichEmployee = AppUtil.GetIdValue("NewAssignToWhichEmployee");
        var ContactNumber = AppUtil.GetIdValue("NewContactNumber");
        var ClientSurvey = AppUtil.GetIdValue("NewClientSurvey");

        var url = "/NewClient/InsertNewClient/";
        var ClientDetails = {
            Name: Name, ZoneID: ZoneID, Address: Address, PackageID: PackageID, ConnectionTypeID: ConnectionTypeID, EmployeeID: AssignToWhichEmployee, ContactNumber: ContactNumber, ClientSurvey: ClientSurvey, LatitudeLongitude: LatitudeLongitude
            //Name: Name, ZoneID: ZoneID, Address: Address, PackageID: PackageID, ConnectionTypeID: ConnectionTypeID, EmployeeID: AssignToWhichEmployee, ContactNumber: ContactNumber, ClientSurvey: ClientSurvey
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = JSON.stringify({ ClientDetails: ClientDetails });
        AppUtil.MakeAjaxCall(url, "POST", data, NewConnectionManager.InsertNewConnectionSuccess, NewConnectionManager.InsertNewConnectionFail);
    },
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    InsertNewConnectionSuccess: function (data) {

        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("New Connection Information Saved Successfully.");
            window.location.href = '/NewClient/GetAllNewClientList';
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("New Connection Information Save Failed.");
        }
    },
    InsertNewConnectionFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured for saving new connection information. Contact with Administrator.");
        console.log(data);
    },

    GetClientDetailsByID: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //setTimeout(function () {
        var url = "/NewClient/GetNewClientByID/";
        var Data = ({ ClientDetailsID: id });
        Data = NewConnectionManager.addRequestVerificationToken(Data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, NewConnectionManager.GetClientDetailsByIDSuccess, NewConnectionManager.GetClientDetailsByIDError);
        // }, 500);
    },
    GetClientDetailsByIDSuccess: function (data) {

        
        var ClientDetails = (data.newClient);
        $("#ClientDetailsID").val(ClientDetails[0].ClientDetailsID);

        $("#NewName").val(ClientDetails[0].Name);
        $("#NewZoneID").val(ClientDetails[0].ZoneID);
        $("#NewAddress").val(ClientDetails[0].Address);
        $("#NewLatitudeLongitude").val(ClientDetails[0].LatitudeLongitude);
        $("#NewPackageID").val(ClientDetails[0].PackageID);
        $("#NewConnectionTypeID").val(ClientDetails[0].ConnectionTypeID);
        $("#NewAssignToWhichEmployee").val(ClientDetails[0].EmployeeID);
        $("#NewContactNumber").val(ClientDetails[0].ContactNumber);
        $("#NewClientSurvey").val(ClientDetails[0].ClientSurvey);

        //AppUtil.HideWaitingDialog();
        $("#tblUpdateNewConnectionInformation").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDError: function (data) {

        //AppUtil.HideWaitingDialog();
        
        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    GetClientDetailsByIDForSignUp: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //   setTimeout(function () {
        var url = "/NewClient/GetNewClientByID/";
        var Data = ({ ClientDetailsID: id });
        Data = NewConnectionManager.addRequestVerificationToken(Data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, NewConnectionManager.GetClientDetailsByIDForSignUpSuccess, NewConnectionManager.GetClientDetailsByIDForSignUpError);
        //  }, 500);
    },
    GetClientDetailsByIDForSignUpSuccess: function (data) {

        
        var ClientDetails = (data.newClient);
        console.log("ClientLineStatus: " + ClientDetails);


        $("#tblEmployeeDetails #Name").val(ClientDetails[0].Name);
        $("#tblEmployeeDetails #Email").val(ClientDetails[0].Email);
        $("#tblEmployeeDetails #LoginName").val(ClientDetails[0].LoginName);
        $("#tblEmployeeDetails #Password").val(ClientDetails[0].Password);
        $("#tblEmployeeDetails #Address").val(ClientDetails[0].Address);
        $("#tblEmployeeDetails #Latitudelongitude").val(ClientDetails[0].LatitudeLongitude);
        $("#tblEmployeeDetails #ContactNumber").val(ClientDetails[0].ContactNumber);
        $("#tblEmployeeDetails #ZoneID").val(ClientDetails[0].ZoneID);
        $("#tblEmployeeDetails #SMSCommunication").val(ClientDetails[0].SMSCommunication);
        $("#tblEmployeeDetails #Occupation").val(ClientDetails[0].Occupation);
        $("#tblEmployeeDetails #SocialCommunicationURL").val(ClientDetails[0].SocialCommunicationURL);
        $("#tblEmployeeDetails #Remarks").val(ClientDetails[0].Remarks);
        $("#tblEmployeeDetails #ConnectionTypeID").val(ClientDetails[0].ConnectionTypeID);
        $("#tblEmployeeDetails #BoxNumber").val(ClientDetails[0].BoxNumber);
        $("#tblEmployeeDetails #PopDetails").val(ClientDetails[0].PopDetails);
        $("#tblEmployeeDetails #RequireCable").val(ClientDetails[0].RequireCable);

        $("#tblEmployeeDetails #Reference").val(ClientDetails[0].Reference);
        $("#tblEmployeeDetails #NationalID").val(ClientDetails[0].NationalID);
        $("#tblEmployeeDetails #PackageID").val(ClientDetails[0].PackageID);

        $("#tblEmployeeDetails #SecurityQuestionAnswer").val(ClientDetails[0].SecurityQuestionAnswer);
        $("#tblEmployeeDetails #MacAddress").val(ClientDetails[0].MacAddress);
        $("#tblEmployeeDetails #ClientSurvey").val(ClientDetails[0].ClientSurvey);
        //$("#tblEmployeeDetails #ConnectionDate").val(AppUtil.ParseDate(ClientDetails[0].CreateDate));

        $("#ConnectionDate").datepicker("setDate", new Date());;

        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDForSignUpError: function (data) {

        //AppUtil.HideWaitingDialog();
        
        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    UpdateClientDetails: function () {

        
        var url = "/NewClient/UpdateClientDetails/";

        var ClientDetailsID = AppUtil.GetIdValue("ClientDetailsID");
        var Name = AppUtil.GetIdValue("NewName");
        var ZoneID = AppUtil.GetIdValue("NewZoneID");
        var Address = AppUtil.GetIdValue("NewAddress");
        var LatitudeLongitude = AppUtil.GetIdValue("NewLatitudeLongitude");
        var PackageID = AppUtil.GetIdValue("NewPackageID");
        var ConnectionTypeID = AppUtil.GetIdValue("NewConnectionTypeID");
        var AssignToWhichEmployee = AppUtil.GetIdValue("NewAssignToWhichEmployee");
        var ContactNumber = AppUtil.GetIdValue("NewContactNumber");
        var ClientSurvey = AppUtil.GetIdValue("NewClientSurvey");


        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID, Name: Name, ZoneID: ZoneID, Address: Address, PackageID: PackageID, ConnectionTypeID: ConnectionTypeID, EmployeeID: AssignToWhichEmployee, ContactNumber: ContactNumber, ClientSurvey: ClientSurvey, LatitudeLongitude: LatitudeLongitude
        };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = JSON.stringify({ ClientClientDetails: ClientDetails });
        AppUtil.MakeAjaxCall(url, "POST", data, NewConnectionManager.UpdateClientDetailsSuccess, NewConnectionManager.UpdateClientDetailsFail);
        //  }, 500);
    },
    UpdateClientDetailsSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();


        if (data.ClientDetails) {
            var parseEmployee = (data.ClientDetails);
            $("#tblAllNewClient>tbody>tr").each(function () {
                

                var index = $(this).index();
                var employeeID = $(this).find("td:eq(0) input").val();
                if (employeeID == parseEmployee[0].ClientDetailsID) {
                    $('#tblAllNewClient tbody>tr:eq(' + index + ')').find("td:eq(1)").text(parseEmployee[0].Name);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee[0].ZoneName);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseEmployee[0].Address);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(parseEmployee[0].ContactNumber);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(parseEmployee[0].PackageName);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee[0].EmployeeName);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee[0].ClientSurvey);
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(8)").html((parseEmployee[0].UpdateTime) ? AppUtil.ParseDateTime(parseEmployee[0].UpdateTime) : AppUtil.ParseDateTime(parseEmployee[0].CreatedTime));
                    $('#tblAllNewClient>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(parseEmployee[0].UpdateBy);
                }
            });
        }

        if (data.UpdateStatus === true) {
            AppUtil.ShowSuccess("New Connection Successfully Edited.");
        }
        if (data.UpdateStatus === false) {
            AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
        }

        $("#tblUpdateNewConnectionInformation").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsFail: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    UpdateClientDetailsSignUp: function (ClientDetailsID) {
        
        var url = "/NewClient/InsertNewClientSignUpDetails/";

    //    var Name = AppUtil.GetIdValue("tblEmployeeDetails #Name");//$("#tblEmployeeDetails 
    //    var Email = AppUtil.GetIdValue("tblEmployeeDetails #Email");
    //    var LoginName = AppUtil.GetIdValue("tblEmployeeDetails #LoginName");
    //    var Password = AppUtil.GetIdValue("tblEmployeeDetails #Password");
    //    var Address = AppUtil.GetIdValue("tblEmployeeDetails #Address");
    //    var ContactNumber = AppUtil.GetIdValue("tblEmployeeDetails #ContactNumber");
    //    var ZoneID = AppUtil.GetIdValue("tblEmployeeDetails #ZoneID");
    //    var SMSCommunication = AppUtil.GetIdValue("tblEmployeeDetails #SMSCommunication");
    //    var Occupation = AppUtil.GetIdValue("tblEmployeeDetails #Occupation");
    //    var SocialCommunicationURL = AppUtil.GetIdValue("tblEmployeeDetails #SocialCommunicationURL");
    //    var Remarks = AppUtil.GetIdValue("tblEmployeeDetails #Remarks");
    //    var ConnectionTypeID = AppUtil.GetIdValue("tblEmployeeDetails #ConnectionTypeID");
    //    var BoxNumber = AppUtil.GetIdValue("tblEmployeeDetails #BoxNumber");
    //    var PopDetails = AppUtil.GetIdValue("tblEmployeeDetails #PopDetails");
    //    var RequireCable = AppUtil.GetIdValue("tblEmployeeDetails #RequireCable");
    //    var CableTypeID = AppUtil.GetIdValue("tblEmployeeDetails #CableTypeID");
    //    var Reference = AppUtil.GetIdValue("tblEmployeeDetails #Reference");
    //    var NationalID = AppUtil.GetIdValue("tblEmployeeDetails #NationalID");
    //    var PackageID = AppUtil.GetIdValue("tblEmployeeDetails #PackageID");
    //    var SingUpFee = AppUtil.GetIdValue("tblEmployeeDetails #SingUpFee");                              //////
    //    var SecurityQuestionID = AppUtil.GetIdValue("tblEmployeeDetails #SecurityQuestionID");
    //    var SecurityQuestionAnswer = AppUtil.GetIdValue("tblEmployeeDetails #SecurityQuestionAnswer");
    //    var MacAddress = AppUtil.GetIdValue("tblEmployeeDetails #MacAddress");
    //    var BillPaymentDate = $("#BillPaymentDate").val();//$('#BillPaymentDate').datepicker('getDate');
    //    var ClientSurvey = AppUtil.GetIdValue("tblEmployeeDetails #ClientSurvey");
    //    var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
        

    //    var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
    //    var IP = AppUtil.GetIdValue("IP");
    //    var Mac = AppUtil.GetIdValue("Mac");


    //    var ResellerID = AppUtil.GetIdValue("ResellerID");

    //    var ClientDetails = {
    //        ClientDetailsID: ClientDetailsID, Name: Name, Email: Email, LoginName: LoginName, Password:
    //            Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication:
    //            SMSCommunication, Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks:
    //            Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails, RequireCable:
    //            RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID:
    //            PackageID, SecurityQuestionID: SecurityQuestionID, SecurityQuestionAnswer:
    //            SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate:
    //            ConnectionDate, MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate,
    //            ResellerID: ResellerID
    //};
    //    var Transaction = { /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee };
    //    //  var ClientLineStatus = {  LineStatusID: LineStatusID, StatusChangeReason: Reason };

    //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
    //    var datas = JSON.stringify({ ClientDetails: ClientDetails, Transaction: Transaction, ItemListForEmployee: itemAssignArray, ClientCableAssign: cableAssignArray });
    //    AppUtil.MakeAjaxCall(url, "POST", datas, NewConnectionManager.UpdateClientDetailsSignUpSuccess, NewConnectionManager.UpdateClientDetailsSignUpFail);


        //var chkPackageFromRunningMonth = $("#chkPackageFromRunningMonth").is(":checked");
        //var chkStatusFromRunningMonth = $("#chkStatusFromRunningMonth").is(":checked");
        //var check2 = $('input[name=chkStatusFromRunningMonth]:checked').length;

        //var url = "/Client/UpdateClientDetailsOnlyAllClientForMKT/";

        var Name = AppUtil.GetIdValue("Name");
        var Email = AppUtil.GetIdValue("Email");
        var LoginName = AppUtil.GetIdValue("LoginName");
        var Password = AppUtil.GetIdValue("Password");
        var Address = AppUtil.GetIdValue("Address");
        var LatitudeLongitude = AppUtil.GetIdValue("Latitudelongitude");
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
            ClientDetailsID: ClientDetailsID,Name: Name, Email: Email, LoginName: LoginName, Password: Password, Address: Address, ContactNumber: ContactNumber, ZoneID: ZoneID, SMSCommunication: SMSCommunication
            , Occupation: Occupation, SocialCommunicationURL: SocialCommunicationURL, Remarks: Remarks, ConnectionTypeID: ConnectionTypeID, BoxNumber: BoxNumber, PopDetails: PopDetails
            , RequireCable: RequireCable, CableTypeID: CableTypeID, Reference: Reference, NationalID: NationalID, PackageID: PackageID, SecurityQuestionID: SecurityQuestionID
            , SecurityQuestionAnswer: SecurityQuestionAnswer, MacAddress: MacAddress, ClientSurvey: ClientSurvey, ConnectionDate: ConnectionDate,
            MikrotikID: MikrotikID, IP: IP, Mac: Mac, ApproxPaymentDate: BillPaymentDate,
            ResellerID: ResellerID
            , StatusThisMonth: StatusThisMonth, StatusNextMonth: StatusNextMonth
            , PackageThisMonth: PackageThisMonth, PackageNextMonth: PackageNextMonth
            , PermanentDiscount: PermanentDiscount, LatitudeLongitude: LatitudeLongitude
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
        //AppUtil.MakeAjaxCall(url, "POST", datas, ClientUpdateManager.UpdateClientDetailsSuccess, ClientUpdateManager.UpdateClientDetailsFail);
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", formData, header, NewConnectionManager.UpdateClientDetailsSignUpSuccess, NewConnectionManager.UpdateClientDetailsSignUpFail);

    },
    UpdateClientDetailsSignUpSuccess: function (data) {
        
        if (data.MikrotikFailed === true) {
            AppUtil.ShowError(data.Message);
        } 

        if (data.MikrotikSuccess === false) {
            if (data.AlreadyAddedLoginNameInMikrotik === true) {
                AppUtil.ShowError("Login Name Already Exist in Mikrotik. Please Choose another.");
            } else {
                AppUtil.ShowError(data.Message);
            }
        }
        if (data.Success === false) {
            //   DuplicateCableStockID = , CableBoxName = , LenghtGreaterThanCableAmount = , GreaterBoxNameList = 
            if (data.MikrotikFailed === true) {
                AppUtil.ShowError(data.Message);
            }
            if (data.LoginNameExist === true) {
                AppUtil.ShowSuccess("Login Name Already Used. Please Choose Different One.");
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
            ClientDetailsID = "";
            NewConnectionManager.ClearClientDetailsModalInformation();

            AppUtil.ShowSuccess("Data Updated Successfully.");
            //window.location.href = "/NewClient/GetAllNewClientList";
            table.draw();
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }

           

        $("#tblEmployeeDetails").modal("hide");
        console.log(data);
    },
    UpdateClientDetailsSignUpFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    DeleteNewClientDetails: function () {

        
        var url = "/Client/DeleteClientDetails/";

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = ({ ClientDetailsID: ClientDetailsID });
        datas = NewConnectionManager.addRequestVerificationToken(datas);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, NewConnectionManager.DeleteNewClientDetailsSuccess, NewConnectionManager.DeleteNewClientDetailsFail);
        //   }, 50);
    },
    DeleteNewClientDetailsSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();

        if (data.DeleteStatus === true) {
            $("#tblAllNewClient>tbody>tr").each(function () {
                

                var index = $(this).index();
                var employeeID = $(this).find("td:eq(0) input").val();
                if (employeeID == data.ClientDetailsID) {
                    
                    $('#tblAllNewClient tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
        }

        if (data.DeleteStatus === false) {
            AppUtil.ShowSuccess("Some Information Can not removed.");
        }


        console.log(data);
    },
    DeleteNewClientDetailsFail: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    SearchClientListByZone: function (SearchID, searchType) {
        
        if (SearchID === "") {
            SearchID = 0;
        }
        //AppUtil.ShowWaitingDialog();
        var url = "/NewClient/SearchClientListByZone/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ SearchID: SearchID, searchType: searchType, __RequestVerificationToken: AntiForgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, NewConnectionManager.SearchClientListByZoneSuccess, NewConnectionManager.SearchClientListByZoneError);
    },
    SearchClientListByZoneSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $("#tblAllNewClient>tbody").empty();

        var listOfClient = (data.SearchClientList);
        //$('#tblAllNewClient tbody').remove();
        $.each(listOfClient, function (index, item) {
            
            console.log(item);
            $('#tblAllNewClient tbody').append('<tr><td hidden="hidden"><input type="hidden" id="EmployeeDetailsID" name="EmployeeDetailsID" value=' + item.ClientDetailsID + '></td><td>' + item.Name + '</td><td>' + item.ZoneName + '</td><td>' + item.Address + '</td><td>' + item.ContactNumber + '</td><td>' + item.PackageName + '</td><td>' + item.LoginName + '</td><td>' + item.ClientSurvey + '</td><td>' + item.ConnectionDate + '</td><td>' + item.CreateBy + '</td><td>' + item.UpdateBy + '</td><td><a href="" id="ShowNewClientInformationForUpdate">Show</a><a href="" id="ShowNewClientInformationForSignUp">SignUp</a></td><td> <button id="btnDelete" type="button" class="btn btn-danger btn-sm" data-toggle="confirmation" data-placement="top"> <span class="glyphicon glyphicon-remove"></span></button></td></tr>');

            index = parseInt(index) + 1;
        });

        var mytable = $('#tblAllNewClient').DataTable();


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


    PrintRequestClientList: function () {
        
        var url = "/Excel/CreateReportForSignUpClient";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({ ZoneID: ZoneID });
        data = NewConnectionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, NewConnectionManager.PrintRequestClientListSuccess, NewConnectionManager.PrintRequestClientListFail);
    },
    PrintRequestClientListSuccess: function (data) {
        
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
    PrintRequestClientListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
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


        $("#PreviewClientOwnImageBytesPaths").attr("src", "");
        $("#ClientOwnImageBytes").wrap('<form>').closest('form').get(0).reset();
        $("#ClientOwnImageBytes").unwrap();

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