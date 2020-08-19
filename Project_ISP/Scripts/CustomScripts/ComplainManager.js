

var ComplainManager = {
    ComplainSearchValidation: function () {
        if (AppUtil.GetIdValue("YearID") === '' && AppUtil.GetIdValue("MonthID") === '' && AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Some Criteria for Search.");
            return false;
        }
        if (AppUtil.GetIdValue("MonthID") !== '') {
            if (AppUtil.GetIdValue("YearID") === '') {
                AppUtil.ShowSuccess("Please Select Year.");
                return false;
            }
        }
        return true;

    },

    CreateComplainValidation: function (isforreseller, employeeRequired) {
        if (isforreseller) {
            if (AppUtil.GetIdValue("ResellerID") === '') {
                AppUtil.ShowSuccess("Please Select Reseller.");
                return false;
            }
        }

        if (AppUtil.GetIdValue("txtClientName") === '') {
            AppUtil.ShowSuccess("Please Select Client Name.");
            return false;
        }

        if (employeeRequired) {
            if (AppUtil.GetIdValue("EmployeeID") === '') {
                AppUtil.ShowSuccess("Please Select Assign Employee.");
                return false;
            }
        }
        if (complainMessage == true) {
            if (AppUtil.GetIdValue("txtComplain") === '') {
                AppUtil.ShowSuccess("Please Add your complain.");
                return false;
            }
        }
        if (AppUtil.GetIdValue("ComplainType") === '') {
            AppUtil.ShowSuccess("Please Select Complain Type.");
            return false;
        }
        if (whichOrWhere == true) {
            if (AppUtil.GetIdValue("txtWhichWhere") === '') {
                AppUtil.ShowSuccess("Please add message in Which/Where.");
                return false;
            }
        }

        //if (AppUtil.GetIdValue("txtComplain") === '') {
        //    AppUtil.ShowSuccess("Please Insert Your Complain.");
        //    return false;
        //}

        if (AppUtil.GetIdValue("txtMobile") === '') {
            AppUtil.ShowSuccess("Please Insert Mobile.");
            return false;
        }

        if (AppUtil.GetIdValue("txtClientAdress") === '') {
            AppUtil.ShowSuccess("PleaseInsert  Client Adress.");
            return false;
        }
        return true;
    },

    UpdateComplainValidation: function (isforreseller, employeeRequired) {
        if (isforreseller) {
            if (AppUtil.GetIdValue("ResellerIDForUpdate") === '') {
                AppUtil.ShowSuccess("Please Select Reseller.");
                return false;
            }
        }
        if (employeeRequired) {
            if (AppUtil.GetIdValue("EmployeeIDForUpdate") === '') {
                AppUtil.ShowSuccess("Please Select Assign Employee.");
                return false;
            }
        }


        if (complainMessage == true) {
            if (AppUtil.GetIdValue("ComplainDetails") === '') {
                AppUtil.ShowSuccess("Please Add your complain.");
                return false;
            }
        }
        if (whichOrWhere == true) {
            if (AppUtil.GetIdValue("WhichOrWhereUpdate") === '') {
                AppUtil.ShowSuccess("Please add message in Which/Where.");
                return false;
            }
        }
        return true;
    },

    ShowMessageBoxOrNotByComplainTypeID: function (ComplainTypeID) {


        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/ComplainType/MessageBoxShowOrHide/";
        var data = { ComplainTypeID: ComplainTypeID };
        data = ComplainManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.ShowMessageBoxOrNotByComplainTypeIDSuccess, ComplainManager.ShowMessageBoxOrNotByComplainTypeIDError);

        // }, 500);
    },
    ShowMessageBoxOrNotByComplainTypeIDSuccess: function (data) {
        console.log(data);

        if (data.ShowMesssageBox) {
            $("#txtWhichWhere").prop("disabled", false);
            //complainMessage = false;
            whichOrWhere = true;
        }
        else {
            whichOrWhere = false;
            //complainMessage = true;
            //$("#txtWhichWhere").prop("disabled", true);
            //$("#txtWhichWhere").val('');
            //$("#txtComplain").val('');
        }
    },
    ShowMessageBoxOrNotByComplainTypeIDError: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    ShowMessageBoxOrNotByComplainTypeIDForUpdate: function (ComplainTypeID) {


        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {

        var url = "/ComplainType/MessageBoxShowOrHide/";
        var data = { ComplainTypeID: ComplainTypeID };
        data = ComplainManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.ShowMessageBoxOrNotByComplainTypeIDSuccessForUpdate, ComplainManager.ShowMessageBoxOrNotByComplainTypeIDErrorForUpdate);

        // }, 500);
    },
    ShowMessageBoxOrNotByComplainTypeIDSuccessForUpdate: function (data) {
        console.log(data);

        if (data.ShowMesssageBox) {
            $("#divWhichOrWhereUpdate").prop("hidden", false);
            whichOrWhere = true;
        }
        else {
            $("#divWhichOrWhereUpdate").prop("hidden", true);
            whichOrWhere = false;
        }
    },
    ShowMessageBoxOrNotByComplainTypeIDErrorForUpdate: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    SearchComplainBySearchCriteria: function () {


        //AppUtil.ShowWaitingDialog();
        var url = "/Complain/SearchComplainBySearchCriteria";
        var YearID = $("#YearID").val();
        var MonthID = $("#MonthID").val();
        var EmployeeID = $("#EmployeeID").val();
        var data = ({ YearID: YearID, MonthID: MonthID, EmployeeID: EmployeeID });

        data = ComplainManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.SearchComplainBySearchCriteriaSuccess, ComplainManager.SearchComplainBySearchCriteriaError);

    },
    SearchComplainBySearchCriteriaSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);

        if (data.Success === true) {
            if (data.ComplainCount >= 0) {
                AppUtil.ShowSuccess("Total " + data.ComplainCount + " Information Found.");

                $('#tblComplainList').dataTable().fnDestroy();
                $("#tblComplainList>tbody").empty();

                $.each(data.ComplainList, function (index, item) {

                    var LineStatus = "";
                    var actionOption = "";
                    if (item.LineStatusID == 7 || item.LineStatusID == 8) {
                        actionOption = "";
                    }
                    else if (item.LineStatusID == 6) {
                        actionOption = "<button type='button' id='btnEdit' class='btn btn-danger btn-sm'><span class='glyphicon glyphicon-edit'></span></button>\
                                        <button type='button' id='btnDelete' class='btn btn-danger btn-sm '><span class='glyphicon glyphicon-remove'></span></button>\
                                        <button type='button' id='btnSolve' class='btn btn-success btn-sm  '><span class='glyphicon glyphicon-ok'></span></button>";
                    }

                    if (item.LineStatusID == 6) {
                        LineStatus = '<div id="Status" class="label label-warning">' + item.LineStatusName + '</div>';
                    }
                    else if (item.LineStatusID == 7) {
                        LineStatus = '<div id="Status" class="label label-danger">' + item.LineStatusName + '</div>';
                    }
                    else if (item.LineStatusID == 8) {
                        LineStatus = '<div id="Status" class="label label-success">' + item.LineStatusName + '</div>';
                    }

                    var link = "";
                    if (item.ClientDetailsID && item.TransactionID) {
                        link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.CName + "</a>";
                    }
                    else {
                        link = item.CName;
                    }


                    $("#tblComplainList>tbody").append("<tr role='row' class='odd'><td hidden><input type='hidden' value=" + item.ComplainID + "></td>\
                     <td>"+ item.TokenNo + "</td><td>" + link + "</td><td>" + item.Address + "</td><td>" + item.ZoneName + "</td>\
                     <td>" + item.ContactNumber + "</td><td>" + item.ComplainDetails + "</td><td>" + AppUtil.ParseDateTime(item.ComplainTime) + "</td><td hidden></td><td>" + item.Name + "</td><td>" + item.ComplainOpenBy + "</td><td>" + LineStatus + "</td>\
                     <td align='center' style='width:9%'> " + actionOption + " </td>\
                    </tr>");

                    //$("#tblComplainList>tbody").append("<div><tr role='row' class='odd'><td><input type='hidden' value=" + item.ComplainID + "></td>\
                    // <td>"+ item.TokenNo + "</td><td>" + item.ClientDetails.LoginName + "</td><td>" + item.ClientDetails.Address + "</td><td>" + item.ClientDetails.Zone.ZoneName + "</td>\
                    // <td>"+ item.ClientDetails.ContactNumber + "</td><td>" + item.ComplainDetails + "</td><td></td><td>" + item.Employee.Name + "</td><td></td><td>" + item.LineStatus.LineStatusName + "</td>\
                    // <td <div style='width: 20%; float: left'><button type='button' id='btnEdit' class='btn btn-success btn-block' style='width: 20px;'>Edit@*<span class='glyphicon glyphicon-ok'></span>*@</button></div>\
                    //     <div style='width: 20%; float: right'><button type='button' id='btnDelete' class='btn btn-danger btn-block' style='width: 20px;'>Delete@*<span class='glyphicon glyphicon-remove'></span>*@</button></div>></td>\
                    //</tr></div>");
                });
            }
            else {
                AppUtil.ShowError("Sorry No Information Found.");
            }
        }

        var mytable = $('#tblComplainList').DataTable({
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
    SearchComplainBySearchCriteriaError: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
    },


    AddComplain: function () {

        var clientDetailsID = _ClientDetailsID;
        var EmployeeID = $("#EmployeeID").val();
        var Complain = $("#txtComplain").val();
        var ComplainTypeId = $("#ComplainType").val();
        var WhichOrWhere = $("#txtWhichWhere").val();
        var sendSMS;

        if ($("input[name='chkSwitch']").is(":checked")) {
            sendSMS = true;
        }
        else {
            sendSMS = false;
        }

        var url = "/Complain/SaveComplain/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({
            ClientDetailsID: clientDetailsID, EmployeeID: EmployeeID, Complain: Complain, ComplainTypeID: ComplainTypeId
            , WhichOrWhere: WhichOrWhere, SendSMS: sendSMS
        });
        data = ComplainManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.AddComplainSuccess, ComplainManager.AddComplainError);
    },
    AddComplainSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Complain Added Successfully.");
            $("#txtClientName").val("");
            _ClientDetailsID = "";
            $("#txtMobile").val("");
            $("#txtClientAdress").val("");
            $("#EmployeeID").val("");
            $("#txtComplain").val("");
            window.location.href = "/Complain/GetAllComplainList/";

        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Compalin Fail.");
        }
    },
    AddComplainError: function (data) {
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");

    },

    getAutoCompleateDetailsInformation: function (val) {

        var url = "/Complain/getAutoCompleateDetailsInformation/";
        var data = ({ ClientDetsilsID: val });
        data = ComplainManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.getAutoCompleateDetailsInformationSuccess, ComplainManager.getAutoCompleateDetailsInformationError);
    },
    getAutoCompleateDetailsInformationSuccess: function (data) {

        // AppUtil.ShowSuccess("S");
        console.log(data);
        var dataClient = data.ClientDetails;
        $("#txtMobile").val(dataClient.Mobile);
        $("#txtClientAdress").val(dataClient.ClientAdress);
        $("#txtTotalItemGivenForThisClient").val(data.itemgiven.replace("</br>","                        "));

    },
    getAutoCompleateDetailsInformationError: function (data) {

        // AppUtil.ShowSuccess("F");
        console.log(data);
    },
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },


    UpdateClientComplainStatusToPending: function (ComplainID) {

        var url = "/Complain/UpdateComplainStatusToPending/";
        var data = JSON.stringify({ ComplainID: ComplainID });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainManager.UpdateClientComplainStatusToPendingSuccess, ComplainManager.UpdateClientComplainStatusToPendingError)
    },
    UpdateClientComplainStatusToPendingSuccess: function (data) {

        if (data.Success === true) {
            $("#tblComplainList>tbody>tr").each(function (i, item) {

                var index = $(this).index();
                var val = $(this).find("td:eq(0) input").val();
                if (val == data.ComplainID) {
                    $('#tblComplainList>tbody>tr:eq(' + index + ')').remove();
                }

            });
        }
        if (data.Success === false) {

            alert("F");
        }

    },
    UpdateClientComplainStatusToPendingError: function (data) {

        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },

    UpdateStatusByDelete: function (ComplainID) {

        var url = "/Complain/UpdateStatusByDelete/";
        var data = JSON.stringify({ ComplainID: ComplainID });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainManager.UpdateStatusByDeleteSuccess, ComplainManager.UpdateStatusByDeleteError)

        // alert(ComplainID);
    },
    UpdateStatusByDeleteSuccess: function (data) {

        if (data.Success === true) {
            $("#tblComplainList>tbody>tr").each(function (i, item) {

                var index = $(this).index();
                var val = $(this).find("td:eq(0) input").val();
                if (val == data.ComplainID) {
                    $('#tblComplainList>tbody>tr:eq(' + index + ')').remove();
                }

            });
        }
        if (data.Success === false) {

            alert("F");
        }

    },
    UpdateStatusByDeleteError: function (data) {

        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },


    UpdateStatusBySolve: function (ComplainID) {

        var url = "/Complain/UpdateStatusBySolve/";
        var data = JSON.stringify({ ComplainID: ComplainID });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainManager.UpdateStatusBySolveSuccess, ComplainManager.UpdateStatusBySolveError)

        // alert(ComplainID);
    },
    UpdateStatusBySolveSuccess: function (data) {

        if (data.Success === true) {
            $("#tblComplainList>tbody>tr").each(function (i, item) {

                var index = $(this).index();
                var val = $(this).find("td:eq(0) input").val();
                if (val == data.ComplainID) {
                    $('#tblComplainList>tbody>tr:eq(' + index + ')').remove();
                }

            });
        }
        if (data.Success === false) {

            alert("F");
        }

    },
    UpdateStatusBySolveError: function (data) {

        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");
    },

    ShowComplainByIdForUpdate: function (ComplainID) {

        //AppUtil.ShowWaitingDialog();

        //  setTimeout(function () {

        var url = "/Complain/ShowComplainByIdForUpdate/";
        var data = JSON.stringify({ ComplainID: ComplainID });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainManager.ShowComplainByIdForUpdateSuccess, ComplainManager.ShowComplainByIdForUpdateError);
        //  });

    },
    ShowComplainByIdForUpdateSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        var complainJsonParse = (data.result);

        ComplainManager.ClearComplainAndWhichOrWhere();
        //$("#divComplainDetails").prop("hidden", true);
        //$("#divWhichOrWhereUpdate").prop("hidden", true);

        if (complainJsonParse.ShowComplainDiv) {
            complainMessage = true;
            $("#divComplainDetails").prop("hidden", false);
            $("#ComplainDetails").val(complainJsonParse.ComplainDetails);
        }
        if (complainJsonParse.ShowWhichOrWhereDiv) {
            whichOrWhere = true;
            $("#divWhichOrWhereUpdate").prop("hidden", false);
            $("#WhichOrWhereUpdate").val(complainJsonParse.WhichOrWhere);
        }

        //if (complainJsonParse.HasComplain) {
        //    $("#divComplainDetails").prop("hidden", false);
        //}
        //if (complainJsonParse.HasWhichOrWhere) {
        //    $("#divWhichOrWhereUpdate").prop("hidden", false);
        //}
        $("#EmployeeID").val(complainJsonParse.Name);

        $("#txtMobile").val(complainJsonParse.ContactNumber);
        $("#txtClientAdress").val(complainJsonParse.Address);
        $("#txtClientName").val(complainJsonParse.LoginName);
        $("#EmployeeIDForUpdate").val(complainJsonParse.EmployeeID);
        $("#ComplainTypeUpdate").val(complainJsonParse.ComplainTypeID);

        $("#mdlComplainUpdate").modal("show");
    },
    ShowComplainByIdForUpdateError: function (data) {

    },


    UpdateComplain: function () {

        //AppUtil.ShowWaitingDialog();

        // var ComplainID = complain;
        var ComplainDetails = $("#ComplainDetails").val();
        var EmployeeID = $("#EmployeeIDForUpdate").val();
        var WhichOrWhere = $("#WhichOrWhereUpdate").val();
        var ComplainTypeID = $("#ComplainTypeUpdate").val();

        //var Mobile = $("#txtMobile").val();
        //var ClientAdress = $("#txtClientAdress").val();
        //var ClientName = $("#txtClientName").val();
        // , ContactNumber: Mobile, Address: ClientAdress, LoginName: ClientName

        var url = "/Complain/UpdateComplain/";
        var complainInfo = ({ ComplainID: ComplainID, ComplainTypeID: ComplainTypeID, ComplainDetails: ComplainDetails, EmployeeID: EmployeeID, WhichOrWhere: WhichOrWhere });
        var data = JSON.stringify({ complain: complainInfo });
        AppUtil.MakeAjaxCall(url, "POST", data, ComplainManager.UpdateComplainSuccess, ComplainManager.UpdateComplainError);
    },
    UpdateComplainSuccess: function (data) {

        //AppUtil.HideWaitingDialog();



        if (data.UpdateSuccess === true) {
            var complainsInfo = (data.complains);

            $("#tblComplainList tbody>tr").each(function () {

                var ComplaiID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (complainsInfo[0].ComplainID == ComplaiID) {

                    $('#tblComplainList tbody>tr:eq(' + index + ')').find("td:eq(6)").text(complainsInfo[0].ComplainTypeName);
                    $('#tblComplainList tbody>tr:eq(' + index + ')').find("td:eq(7)").text(complainsInfo[0].ComplainDetails);
                    $('#tblComplainList tbody>tr:eq(' + index + ')').find("td:eq(8)").text(complainsInfo[0].WhichOrWhere);
                    $('#tblComplainList tbody>tr:eq(' + index + ')').find("td:eq(10)").text(complainsInfo[0].EmployeeID);

                }
            });

        }
        $("#mdlComplainUpdate").modal('hide');
        console.log(data);


    },
    UpdateComplainError: function (data) {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    ClearComplainAndWhichOrWhere: function () {
        complainMessage = false;
        whichOrWhere = false;
        $("#divComplainDetails").prop("hidden", true);
        $("#divWhichOrWhereUpdate").prop("hidden", true);
        $("#ComplainDetails").val('');;
        $("#WhichOrWhereUpdate").val('');
    },
    ClearComplainAndWhichOrWhereCreate: function () {
        complainMessage = false;
        whichOrWhere = false;
        $("#txtWhichWhere").prop("disabled", true);
        $("#txtComplain").prop("disabled", true);
        $("#txtWhichWhere").val('');;
        $("#txtComplain").val('');
    },


    ChangeCheckboxValue: function (ComplainID, Status) {
        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //  setTimeout(function () {
        var url = "/Complain/OnRequestChange/";
        var Data = ({ ComplainID: ComplainID, Status: Status });
        Data = ClientUpdataeFromSeveralPageManager.addRequestVerificationToken(Data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", Data, ComplainManager.ChangeCheckboxValueSuccess, ComplainManager.ChangeCheckboxValueError);
        //  }, 500);

    },
    ChangeCheckboxValueSuccess: function (data) {

        if (data.UpdateSuccess == true) {
            //$('input[id=' + checkboxID + ']').prop("checked", data.Status);
            $('#' + checkboxID + '').prop("checked", data.Status);
            AppUtil.ShowSuccess("On Request " + onRequestStatus + " Success.");
        }
        if (data.UpdateSuccess == false) {
            $('input[name=' + checkboxID + ']').prop("checked", data.Status);
            AppUtil.ShowSuccess("On Request " + onRequestStatus + " Failed.");
        }

    },
    ChangeCheckboxValueError: function (data) {

        //AppUtil.HideWaitingDialog();

        AppUtil.ShowSuccess("Fail");
        console.log(data);
    },

    AddComplainbyClient: function () {
        debugger;
        var Complain = $("#txtComplainByClient").val();
        var url = "/Complain/SaveComplainByClient/";
        var data = ({ Complain: Complain });
        data = ComplainManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ComplainManager.AddComplainByClientSuccess, ComplainManager.AddComplainByClientError);
    },
    AddComplainByClientSuccess: function (data) {
        if (data.Success === true) {
            AppUtil.ShowSuccess("Complain Added Successfully.");
            _ClientDetailsID = "";
            $("#txtComplainByClient").val("");
            window.location.href = "/Complain/GetAllComplainListForSpecificUser/";

        }
        if (data.Success === false) {
            AppUtil.ShowSuccess("Complaiin Fail.");
        }
    },
    AddComplainByClientError: function (data) {
        console.log(data);
        AppUtil.ShowSuccess("Something is wrong contact with administratore.");

    }
}