var SearchClientInformationManager = {

    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },
    GetClientInformationBySearchKeyWord: function (searchKey) {

        var url = "/Client/GetClientListBySearchKeyWord/";
        var data = ({ KeyWord: searchKey });
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SearchClientInformationManager.GetClientInformationBySearchKeyWordSuccess, SearchClientInformationManager.GetClientInformationBySearchKeyWordFailed);
    },
    GetClientInformationBySearchKeyWordSuccess: function (data) {
        
        console.log(data);
        if (data.Success === true) {
            $.each((data.ClientList), function (index, item) {
                var StatusThisMonth = "";
                if (item.StatusThisMonth == 3)
                {
                    StatusThisMonth = '<div id="Status" class="label label-success" >Active</div>';
                }

                if (item.StatusThisMonth == 4) {
                    StatusThisMonth = '<div id="Status" class="label label-warning" >In Active</div>';
                }
                if (item.StatusThisMonth == 5) {
                    StatusThisMonth = '<div id="Status" class="label label-danger" >Lock</div>';
                }
                var StatusNextMonth = "";
                if (item.StatusNextMonth == 3) {
                    StatusNextMonth = '<div id="Status" class="label label-success" >Active</div>';
                }

                if (item.StatusNextMonth == 4) {
                    StatusNextMonth = '<div id="Status" class="label label-warning" >In Active</div>';
                }
                if (item.StatusNextMonth == 5) {
                    StatusNextMonth = '<div id="Status" class="label label-danger" >Lock</div>';
                }
                

                $("#tblAllClientsDashBoard>tbody").append("<tr><td hidden><input type='hidden' value='" + item.ClientDetailsID+"'></td><td hidden>" + item.ClientLineStatusID + "</td>\
                    <td>" + item.ClientName + "</td><td>" + item.LoginID + "</td><td>" + item.PackageThisMonth + "</td><td>" + item.PackageNextMonth + "</td><td>" + item.Address + "</td>\
                    <td>" + item.Zone + "</td><td>" + item.Contact + "</td><td>" + StatusThisMonth + "</td><td>" + StatusNextMonth + "</td>\
                    <td>" + "<a href='#' onclick='navigate(this.href," + item.ClientDetailsID + ");'><input type='button' value='Show Complain List' /></a>" + "</td>\
                    <td>" + "<a href='/client/GetSpecificClientDetailsFromDashBoard?CID=" + item.ClientDetailsID + "'>Show Client Details</a>" + "</td>\
                    <td>" + "<a href='/Transaction/GetClientPaymentHistoryByCID?CID=" + item.ClientDetailsID + "'>Show Bill History</a>" + "</td>\
                    <td>" + "<a href='#' id='layoutClientStatusUpdateHistory'>Show Line History</a>" + "</td></tr>");

                // $("#lstSubject").append($("<option></option>").val(item.SubjectAllocationID).text(item.Subject.SubjectName));
                //$.each(data, function (index, itemData) {
                //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
                //});
            });
        }



    },
    GetClientInformationBySearchKeyWordFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },
}