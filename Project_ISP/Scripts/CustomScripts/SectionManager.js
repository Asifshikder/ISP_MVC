var SectionManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("SectionName") === '') {
            AppUtil.ShowSuccess("Please Insert Section ");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("SectionNames") === '') {
            AppUtil.ShowSuccess("Please Insert Section ");
            return false;
        }
        return true;
    },

    InsertSectionFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Section/InsertSectionFromPopUp";
        var SectionName = AppUtil.GetIdValue("SectionName");


        //setTimeout(function () {
            var Section = { SectionName: SectionName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Section_Client: Section });
            AppUtil.MakeAjaxCall(url, "POST", datas, SectionManager.InsertSectionFromPopUpSuccess, SectionManager.InsertSectionFromPopUpFail);
       // }, 500);
    },
    InsertSectionFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Section) {
                
                var Section = (data.Section);
                $("#tblSection").dataTable().fnDestroy();
                $("#tblSection>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Section.SectionID + '></td><td>' + Section.SectionName + '</td><td><a href="" id="showSectionForUpdate">Show</a></td></tr>');
                var mytable = $('#tblSection').DataTable({
                    "paging": true,
                    "lengthChange": false,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": true,
                    "sDom": 'lfrtip'
                });
                mytable.draw();

            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Section Already Added. Choose different Section.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //  window.location.href = "/Section/Index";
        $("#SectionName").val("");
        $("#mdlSectionInsert").modal('hide');

    },
    InsertSectionFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    InsertSection: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/Section/InsertSection";
        var SectionName = AppUtil.GetIdValue("SectionName");


        //setTimeout(function () {
            var Section = { SectionName: SectionName };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = JSON.stringify({ Section_Client: Section });
            AppUtil.MakeAjaxCall(url, "POST", datas, SectionManager.InsertSectionSuccess, SectionManager.InsertSectionUpFail);
       // }, 500);
    },
    InsertSectionSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            // if (data.Section) {
            // 
            // var Section = (data.Section);
            // $("#tblSection>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Section.SectionID + '/></td><td>' + Section.SectionName + '</td><td><a href="" id="showSectionForUpdate">Show</a></td></tr>');
            // }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Section Already Added. Choose different Section.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }
        //window.location.href = "/Section/Index";
        $("#mdlSectionInsert").modal('hide');

    },
    InsertSectionUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowSectionDetailsByIDForUpdate: function (SectionID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
            
            var url = "/Section/GetSectionDetailsByID/";
            var data = { SectionID: SectionID };
            data = SectionManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SectionManager.ShowSectionDetailsByIDForUpdateSuccess, SectionManager.ShowSectionDetailsByIDForUpdateError);

      //  }, 500);

    },
    ShowSectionDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var SectionDetailsJsonParse = (data.SectionDetails);
        $("#SectionNames").val(SectionDetailsJsonParse.SectionName);


        $("#mdlSectionUpdate").modal("show");
    },
    ShowSectionDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateSectionInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var SectionID = SectionID;
        var SectionName = $("#SectionNames").val();


        var url = "/Section/UpdateSection";
        var SectionInfomation = ({ SectionID: SectionID, SectionName: SectionName });
        var data = JSON.stringify({ SectionInfoForUpdate: SectionInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, SectionManager.UpdateSectionInformationSuccess, SectionManager.UpdateSectionInformationFail);
    },
    UpdateSectionInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var SectionInformation = (data.SectionUpdateInformation);

            $("#tblSection tbody>tr").each(function () {
                
                var SectionID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (SectionInformation[0].SectionID == SectionID) {
                    
                    $('#tblSection tbody>tr:eq(' + index + ')').find("td:eq(1)").text(SectionInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Section Already Added. Choose different Section. ");
            }
        }

        $("#mdlSectionUpdate").modal('hide');
        console.log(data);
    },
    UpdateSectionInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    PrintSectionList: function () {
        
        var url = "/Excel/CreateReportForSectionList";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        //('#ConnectionDate').datepicker('getDate');

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        var data = ({});
        data = SectionManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, SectionManager.PrintSectionListSuccess, SectionManager.PrintSectionListFail);
    },
    PrintSectionListSuccess: function (data) {
        
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
    PrintSectionListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#SectionName").val("");
    },
    clearForUpdateInformation: function () {
        $("#SectionNames").val("");
    }
}