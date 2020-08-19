var GetAllLockClientsManager = {
    Validation: function () {

        if (AppUtil.GetIdValue("Name") === '') {
            AppUtil.ShowSuccess("Please Insert Name.");
            return false;
        }
        if (AppUtil.GetIdValue("lstBatch") === '') {
            AppUtil.ShowSuccess("Please Select Batch.");
            return false;
        }
        if (AppUtil.GetIdValue("lstSubject") === '') {
            AppUtil.ShowSuccess("Please Select Subject.");
            return false;
        }
        return true;

    },

    InsertClientDetails: function () {
        
        var url = "/Client/InsertClientDetails/";

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
        var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee"); //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        var BillPaymentDate = AppUtil.getDateTime("BillPaymentDate") //$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = AppUtil.getDateTime("ConnectionDate"); //('#ConnectionDate').datepicker('getDate');

        var ClientDetails = {
            Name: Name,
            Email: Email,
            LoginName: LoginName,
            Password: Password,
            Address: Address,
            ContactNumber: ContactNumber,
            ZoneID: ZoneID,
            SMSCommunication: SMSCommunication,
            Occupation: Occupation,
            SocialCommunicationURL: SocialCommunicationURL,
            Remarks: Remarks,
            ConnectionTypeID: ConnectionTypeID,
            BoxNumber: BoxNumber,
            PopDetails: PopDetails,
            RequireCable: RequireCable,
            CableTypeID: CableTypeID,
            Reference: Reference,
            NationalID: NationalID,
            PackageID: PackageID,
            SecurityQuestionID: SecurityQuestionID,
            SecurityQuestionAnswer: SecurityQuestionAnswer,
            MacAddress: MacAddress,
            ClientSurvey: ClientSurvey,
            ConnectionDate: ConnectionDate
        };
        var Transaction = { PaymentDate: BillPaymentDate, PaymentAmount: SingUpFee };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientDetails: ClientDetails, Transaction: Transaction });
        AppUtil.MakeAjaxCall(url, "POST", datas, GetAllLockClientsManager.InsertClientDetailsSuccess, GetAllLockClientsManager.InsertClientDetailsFail);
    },
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    InsertClientDetailsSuccess: function (data) {

        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
        }
        if (data.SuccessInsert === false) {
            AppUtil.ShowSuccess("Saved Failed.");
        }
    },
    InsertClientDetailsFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    PrintAllLockClientList: function () {
        
        var url = "/Excel/CreateReportForLockClient";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({ ZoneID: ZoneID });
        data = GetAllLockClientsManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, GetAllLockClientsManager.PrintAllLockClientListSuccess, GetAllLockClientsManager.PrintAllLockClientListFail);
    },
    PrintAllLockClientListSuccess: function (data) {
        
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
    PrintAllLockClientListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
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
        var ContactNumber = AppUtil.GetIdValue("ContactNumber");
        var ZoneID = AppUtil.GetIdValue("ZoneID");
        var SMSCommunication = AppUtil.GetIdValue("SMSCommunication");
        var Occupation = AppUtil.GetIdValue("Occupation");
        var SocialCommunicationURL = AppUtil.GetIdValue("SocialCommunicationURL");
        var Remarks = AppUtil.GetIdValue("Remarks");
        var ConnectionTypeID = AppUtil.GetIdValue("ConnectionTypeID");
        var BoxNumber = AppUtil.GetIdValue("BoxNumber");
        var PopDetails = AppUtil.GetIdValue("PopDetails");
        var RequireCable = AppUtil.GetIdValue("RequireCable");
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var Reference = AppUtil.GetIdValue("Reference");
        var NationalID = AppUtil.GetIdValue("NationalID");
        var PackageID = AppUtil.GetIdValue("PackageID");
        var SingUpFee = AppUtil.GetIdValue("SingUpFee"); //////
        var SecurityQuestionID = AppUtil.GetIdValue("SecurityQuestionID");
        var SecurityQuestionAnswer = AppUtil.GetIdValue("SecurityQuestionAnswer");
        var MacAddress = AppUtil.GetIdValue("MacAddress");
        var BillPaymentDate = $("#BillPaymentDate").val(); //$('#BillPaymentDate').datepicker('getDate');
        var ClientSurvey = AppUtil.GetIdValue("ClientSurvey");
        var ConnectionDate = $("#ConnectionDate").val(); //('#ConnectionDate').datepicker('getDate');
        var LineStatusWillActiveInThisDate = $("#LineStatusActiveDate").val();

        var LineStatusID = AppUtil.GetIdValue("LineStatusID");
        var Reason = AppUtil.GetIdValue("Reason");


        var IsPriorityClient = $("#IsPriorityClient").is(':checked');

        var MikrotikID = AppUtil.GetIdValue("lstMikrotik");
        var IP = AppUtil.GetIdValue("IP");
        var Mac = AppUtil.GetIdValue("Mac");

        var ResellerID = AppUtil.GetIdValue("ResellerID");
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        // setTimeout(function() {

        var ClientDetails = {
            ClientDetailsID: ClientDetailsID,
            Name: Name,
            Email: Email,
            LoginName: LoginName,
            Password: Password,
            Address: Address,
            ContactNumber: ContactNumber,
            ZoneID: ZoneID,
            SMSCommunication: SMSCommunication,
            Occupation: Occupation,
            SocialCommunicationURL: SocialCommunicationURL,
            Remarks: Remarks,
            ConnectionTypeID: ConnectionTypeID,
            BoxNumber: BoxNumber,
            PopDetails: PopDetails,
            RequireCable: RequireCable,
            CableTypeID: CableTypeID,
            Reference: Reference,
            NationalID: NationalID,
            PackageID: PackageID,
            SecurityQuestionID: SecurityQuestionID,
            SecurityQuestionAnswer: SecurityQuestionAnswer,
            MacAddress: MacAddress,
            ClientSurvey: ClientSurvey,
            ConnectionDate: ConnectionDate,
            IsPriorityClient: IsPriorityClient,
            ApproxPaymentDate: BillPaymentDate,
            MikrotikID: MikrotikID, IP: IP, Mac: Mac,
            ResellerID: ResellerID
        };
        var Transaction = { TransactionID: ClientTransactionID, /*PaymentDate: BillPaymentDate,*/ PaymentAmount: SingUpFee };
        var ClientLineStatus = { ClientLineStatusID: ClientLineStatusID, LineStatusID: LineStatusID, StatusChangeReason: Reason, PackageID: PackageID, LineStatusWillActiveInThisDate: LineStatusWillActiveInThisDate };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus, chkPackageFromRunningMonth: chkPackageFromRunningMonth, chkStatusFromRunningMonth: chkStatusFromRunningMonth });
        //var datas = JSON.stringify({ ClientClientDetails: ClientDetails, ClientTransaction: Transaction, ClientClientLineStatus: ClientLineStatus});
        AppUtil.MakeAjaxCall(url, "POST", datas, GetAllLockClientsManager.UpdateClientDetailsSuccess, GetAllLockClientsManager.UpdateClientDetailsFail);
        //}, 500);
    },
    UpdateClientDetailsSuccess: function (data) {
        
        
        
        //AppUtil.HideWaitingDialog();

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
                $("#tblUsers>tbody>tr").each(function () {
                    

                    var index = $(this).index();
                    var employeeID = $(this).find("td:eq(0) input").val();
                    if (employeeID == parseEmployee[0].ClientDetailsID) {

                        if (data.chkStatusFromRunningMonth === true && data.ClientLineStatus == "3") {
                            $("#tblUsers>tbody>tr:eq(" + index + ")").remove();

                        } else {
                            if (parseEmployee[0].IsPriorityClient) {
                                $('#tblUsers tbody>tr:eq(' + index + ')').addClass('changetrbackground');
                            }
                            else {
                                $('#tblUsers tbody>tr:eq(' + index + ')').removeClass('changetrbackground');
                            }

                            $('#tblUsers tbody>tr:eq(' + index + ')').find("td:eq(1)").text(parseEmployee[0].Name);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(2)").text(parseEmployee[0].LoginName);
                            if (data.chkPackageFromRunningMonth === true) {

                                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)")
                                    .text(parseEmployee[0].PackageName);
                            }

                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(data.ClientPackage);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(parseEmployee[0].Address);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee[0].Email);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee[0].ZoneName);
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)")
                                .text(parseEmployee[0].ContactNumber);
                            if (data.chkStatusFromRunningMonth === true) {
                                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(9)").html(ClientStatusHtml);
                            }
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(10)").html(ClientStatusHtml);
                            var split = data.LineStatusActiveDate.split('<div');
                            $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(11)").html(split[0]);
                        }

                    }
                });
            }

            if (data.UpdateStatus === true) {
                AppUtil.ShowSuccess("Successfully Edited.");
            }
            if (data.UpdateStatus === false) {
                AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
            }

        }


        GetAllLockClientsManager.ClearClientDetailsModalInformation();
        //$("#Status").css("visibility", false);
        $("#Status").css("display", "none");
        $("#chkStatusFromRunningMonth").prop("checked", false);
        $("#chkPackageFromRunningMonth").prop("checked", false);
        $("#tblEmployeeDetails").modal("hide");
        console.log(data);

        ////AppUtil.HideWaitingDialog();
        //if (data.LoginNameExist === true) {
        //    AppUtil.ShowSuccess("Sorry Login Name Already Exist.");
        //}
        //if (data.LoginNameExist === false) {
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
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(3)").text(parseEmployee[0].PackageName);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(4)").text(parseEmployee[0].Address);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(5)").text(parseEmployee[0].Email);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(6)").text(parseEmployee[0].ZoneName);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(7)").text(parseEmployee[0].ContactNumber);
        //                $('#tblUsers>tbody>tr:eq(' + index + ')').find("td:eq(8)").html(ClientStatusHtml);
        //            }
        //        });
        //    }

        //    if (data.UpdateStatus === true) {
        //        AppUtil.ShowSuccess("Successfully Edited.");
        //    }
        //    if (data.UpdateStatus === false) {
        //        AppUtil.ShowSuccess("Something Is wrong when Update Client Details. Contact with administrator.");
        //    }

        //}


        //GetAllLockClientsManager.ClearClientDetailsModalInformation();
        ////$("#Status").css("visibility", false);
        //$("#Status").css("display", "none");
        //$("#tblEmployeeDetails").modal("hide");
        //// window.location.href = '/Client/GetAllCLients';
        ////.href = '
        //console.log(data);
    },
    UpdateClientDetailsFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured.");
        console.log(data);
    },

    GetClientDetailsByID: function (id) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //   setTimeout(function() {
        var url = "/CLient/GetClientDetailsByID/";
        var Data = ({ ClientDetailsID: id });
        Data = GetAllLockClientsManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, GetAllLockClientsManager.GetClientDetailsByIDSuccess, GetAllLockClientsManager.GetClientDetailsByIDError);
        //     }, 500);


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
        //ClientBannedStatusID;
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
        $("#BoxNumber").val(ClientDetails.BoxNumber);
        $("#PopDetails").val(ClientDetails.PopDetails);
        $("#RequireCable").val(ClientDetails.RequireCable);
        $("#CableTypeID").val(ClientDetails.CableTypeID);
        $("#Reference").val(ClientDetails.Reference);
        $("#NationalID").val(ClientDetails.NationalID);
        $("#PackageID").val(ClientDetails.PackageID);
        $("#SingUpFee").val(Transaction[0].PaymentAmount);
        $("#SecurityQuestionID").val(ClientDetails.SecurityQuestionID);
        $("#SecurityQuestionAnswer").val(ClientDetails.SecurityQuestionAnswer);
        $("#MacAddress").val(ClientDetails.MacAddress);
        //$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDateINMMDDYYYY(Transaction[0].PaymentDate));
        $("#BillPaymentDate").val(ClientDetails.PaymentDate);
        $("#ClientSurvey").val(ClientDetails.ClientSurvey);
        $("#ConnectionDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.ConnectionDate));
        $("#LineStatusActiveDate").val(AppUtil.ParseDateINMMDDYYYY(ClientDetails.LineStatusActiveDate));

        // $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        $("#LineStatusID").val(ClientDetails.LineStatusID);
        $("#Reason").val(ClientDetails.StatusChangeReason);


        $("#IsPriorityClient").prop("checked", ClientDetails.IsPriorityClient);

        $("#lstMikrotik").val(ClientDetails.MikrotikID);
        $("#IP").val(ClientDetails.IP);
        $("#Mac").val(ClientDetails.Mac);

        $("#ResellerID").val(ClientDetails.ResellerID);

        //AppUtil.HideWaitingDialog();
        $("#tblEmployeeDetails").modal("show");

        //
        //var ClientDetails = JSON.parse(data.ClientLineStatus);
        //var Transaction = (data.Transaction);
        //console.log("Transaction: " + Transaction);
        //console.log("ClientLineStatus: " + ClientDetails);

        //ClientDetailsID = ClientDetails.ClientDetails.ClientDetailsID;
        //ClientLineStatusID = ClientDetails.ClientLineStatusID;
        //ClientBannedStatusID;
        //ClientTransactionID = Transaction.TransactionID;

        //$("#Name").val(ClientDetails.ClientDetails.Name);
        //$("#Email").val(ClientDetails.ClientDetails.Email);
        //$("#LoginName").val(ClientDetails.ClientDetails.LoginName);
        //$("#Password").val(ClientDetails.ClientDetails.Password);
        //$("#Address").val(ClientDetails.ClientDetails.Address);
        //$("#ContactNumber").val(ClientDetails.ClientDetails.ContactNumber);
        //$("#ZoneID").val(ClientDetails.ClientDetails.Zone.ZoneID);
        //$("#SMSCommunication").val(ClientDetails.ClientDetails.SMSCommunication);
        //$("#Occupation").val(ClientDetails.ClientDetails.Occupation);
        //$("#SocialCommunicationURL").val(ClientDetails.ClientDetails.SocialCommunicationURL);
        //$("#Remarks").val(ClientDetails.ClientDetails.Remarks);
        //$("#ConnectionTypeID").val(ClientDetails.ClientDetails.ConnectionType.ConnectionTypeID);
        //$("#BoxNumber").val(ClientDetails.ClientDetails.BoxNumber);
        //$("#PopDetails").val(ClientDetails.ClientDetails.PopDetails);
        //$("#RequireCable").val(ClientDetails.ClientDetails.RequireCable);
        //$("#CableTypeID").val(ClientDetails.ClientDetails.CableType.CableTypeID);
        //$("#Reference").val(ClientDetails.ClientDetails.Reference);
        //$("#NationalID").val(ClientDetails.ClientDetails.NationalID);
        //$("#PackageID").val(ClientDetails.ClientDetails.Package.PackageID);
        //$("#SingUpFee").val(Transaction.PaymentAmount);
        //$("#SecurityQuestionID").val(ClientDetails.ClientDetails.SecurityQuestion.SecurityQuestionID);
        //$("#SecurityQuestionAnswer").val(ClientDetails.ClientDetails.SecurityQuestionAnswer);
        //$("#MacAddress").val(ClientDetails.ClientDetails.MacAddress);
        ////$("#BillPaymentDate").val(new Date(parseInt(Transaction.PaymentDate.substr(6))));
        //$("#BillPaymentDate").val(AppUtil.ParseDate(Transaction.PaymentDate));
        //$("#ClientSurvey").val(ClientDetails.ClientDetails.ClientSurvey);
        //$("#ConnectionDate").val(AppUtil.ParseDate(ClientDetails.ClientDetails.ConnectionDate));

        //// $("#BannedID").val(AppUtil.ParseDate(Transaction.PaymentDate));
        //$("#LineStatusID").val(ClientDetails.LineStatusID);
        //$("#Reason").val(ClientDetails.StatusChangeReason);

        ////AppUtil.HideWaitingDialog();
        //$("#tblEmployeeDetails").modal("show");

        ////AppUtil.ShowSuccess("Success");
    },
    GetClientDetailsByIDError: function (data) {

        //AppUtil.HideWaitingDialog();
        
        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    SearchClientListByZone: function (SearchID, searchType) {
        
        if (SearchID === "") {
            SearchID = 0;
        }
        //AppUtil.ShowWaitingDialog();
        var url = "/Client/SearchClientListByZone/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ SearchID: SearchID, searchType: searchType, __RequestVerificationToken: AntiForgeryToken });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, GetAllLockClientsManager.SearchClientListByZoneSuccess, GetAllLockClientsManager.SearchClientListByZoneError);
    },
    SearchClientListByZoneSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;


        //$('#tblUsers').dataTable().fnDestroy();
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
            $('#tblUsers tbody').append('<tr><td hidden="hidden"><input type="hidden" id="EmployeeDetailsID" name="EmployeeDetailsID" value=' + item.ClientDetailsID + '></td><td>' + item.Name + '</td><td>' + item.LoginName + '</td><td>' + item.PackageNameThisMonth + '</td><td>' + item.PackageNameNextMonth + '</td><td>' + item.Address + '</td><td>' + item.Email + '</td><td>' + item.ZoneName + '</td><td>' + item.ContactNumber + '</td><td>' + thisMonthStatus + '</td><td>' + nextMonthStatus + '</td><td><a href="" id="ShowPopUps">Show</a></td></tr>');
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

        var mytable = $('#tblUsers').DataTable();
        //var mytable = $('#tblUsers').DataTable({
        //    "paging": true,
        //    "lengthChange": false,
        //    "searching": true,
        //    "ordering": true,
        //    "info": true,
        //    "autoWidth": true,
        //    "sDom": 'lfrtip'
        //});
        mytable.draw();

    },
    SearchClientListByZoneError: function (data) {
        
        AppUtil.ShowError("Error");
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
    }


}