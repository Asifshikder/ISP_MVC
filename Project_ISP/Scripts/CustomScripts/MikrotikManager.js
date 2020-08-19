
var MikrotikManager =
    {
        addRequestVerificationToken: function (data) {
            
            data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
            return data;
        },

        CreateValidation: function () {
            
            if (AppUtil.GetIdValue("MikrotikName") === '') {
                AppUtil.ShowSuccess("Please Insert Mikrotik Name.");
                return false;
            }
            if (AppUtil.GetIdValue("RealIP") === '') {
                AppUtil.ShowSuccess("Please Insert RealIP.");
                return false;
            }
            if (AppUtil.GetIdValue("MikUserName") === '') {
                AppUtil.ShowSuccess("Please Insert Mikrotik User Name. ");
                return false;
            }
            if (AppUtil.GetIdValue("MikPassword") === '') {
                AppUtil.ShowSuccess("Please Add Mikrotik Password.");
                return false;
            }
            if (AppUtil.GetIdValue("APIPort") === '') {
                AppUtil.ShowSuccess("Please Insert API Port. ");
                return false;
            }
            if (AppUtil.GetIdValue("WebPort") === '') {
                AppUtil.ShowSuccess("Please Insert Web Port.");
                return false;
            }
            return true;
        },
        UpdateValidation: function () {
            
            if (AppUtil.GetIdValue("MikrotikNames") === '') {
                AppUtil.ShowSuccess("Please Insert Mikrotik Name.");
                return false;
            }
            if (AppUtil.GetIdValue("RealIPs") === '') {
                AppUtil.ShowSuccess("Please Insert RealIP.");
                return false;
            }
            if (AppUtil.GetIdValue("MikUserNames") === '') {
                AppUtil.ShowSuccess("Please Insert Mikrotik User Name. ");
                return false;
            }
            if (AppUtil.GetIdValue("MikPasswords") === '') {
                AppUtil.ShowSuccess("Please Add Mikrotik Password.");
                return false;
            }
            if (AppUtil.GetIdValue("APIPorts") === '') {
                AppUtil.ShowSuccess("Please Insert API Port. ");
                return false;
            }
            if (AppUtil.GetIdValue("WebPorts") === '') {
                AppUtil.ShowSuccess("Please Insert Web Port.");
                return false;
            }
            return true;
        },


        PrintMikrotikList: function () {
            
            var url = "/Excel/CreateReportForMikrotikList";
            // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

          //  var ZoneID = AppUtil.GetIdValue("SearchByZoneID"); //('#ConnectionDate').datepicker('getDate');

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var data = ({  });
            data = MikrotikManager.addRequestVerificationToken(data);
            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MikrotikManager.PrintMikrotikListSuccess, MikrotikManager.PrintMikrotikListFail);
        },
        PrintMikrotikListSuccess: function (data) {
            
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
        PrintMikrotikListFail: function (data) {
            
            AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
            console.log(data);
        },

        InsertMikrotikFromPopUp: function () {
            
            //AppUtil.ShowWaitingDialog();
            
            var url = "/Mikrotik/InsertMikrotikFromPopUp";
            var MikrotikName = AppUtil.GetIdValue("MikrotikName");
            var RealIP = AppUtil.GetIdValue("RealIP");
            var MikUserName = AppUtil.GetIdValue("MikUserName");
            var MikPassword = AppUtil.GetIdValue("MikPassword");
            var APIPort = AppUtil.GetIdValue("APIPort");
            var WebPort = AppUtil.GetIdValue("WebPort");
          //  var IPPoolID = AppUtil.GetIdValue("IPPoolID");


            //  setTimeout(function () {
            var Mikrotik = { RealIP: RealIP, MikUserName: MikUserName, MikPassword: MikPassword, APIPort: APIPort, WebPort: WebPort, MikName: MikrotikName };
            //var Mikrotik = { RealIP: RealIP, MikUserName: MikUserName, MikPassword: MikPassword, APIPort: APIPort, WebPort: WebPort, IPPoolID: IPPoolID };

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
            var headers = {};
            headers['__RequestVerificationToken'] = AntiForgeryToken;

            
            var datas = JSON.stringify({Mikrotik_Client:Mikrotik});
            AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", datas, headers, MikrotikManager.InsertMikrotikFromPopUpSuccess, MikrotikManager.InsertMikrotikFromPopUpFail);
            // }, 500);
        },
        InsertMikrotikFromPopUpSuccess: function (data) {
            
            //AppUtil.HideWaitingDialog();
            console.log(data);

            //if (data.MikrotikCount <1) {
            //    $('#tblMikrotik').dataTable().fnDestroy();
            //    $("#tblMikrotik>tbody").empty();
            //}
            if (data.SuccessInsert === true) {
                AppUtil.ShowSuccess("Saved Successfully.");
                if (data.Mikrotik) {
                    
                    var Mikrotik = (data.Mikrotik);
                    //$("#tblMikrotik>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Mikrotik.MikrotikID + '></td><td>' + Mikrotik.RealIP + '</td><td>' + Mikrotik.MikUserName + '</td><td>' + Mikrotik.MikPassword + '</td><td>' + Mikrotik.APIPort + '</td><td>' + Mikrotik.WebPort + '</td><td style="color:darkblue">' + $("#IPPoolID option[value=" + Mikrotik.IPPoolID + "]").text() + '</td><td><a href="" id="showMikrotikForUpdate">Show</a></td></tr>');
                    $("#tblMikrotik>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Mikrotik.MikrotikID + '></td><td>' + Mikrotik.MikName + '</td><td>' + Mikrotik.RealIP + '</td><td>' + Mikrotik.MikUserName + '</td><td>' + Mikrotik.MikPassword + '</td><td>' + Mikrotik.APIPort + '</td><td>' + Mikrotik.WebPort + '</td><td><a href="" id="showMikrotikForUpdate">Show</a></td></tr>');
                }
            }
            if (data.SuccessInsert === false) {
                
                if (data.AlreadyInsert = true) {
                    AppUtil.ShowSuccess("Mikrotik Already Added. Choose different Mikrotik Name.");
                } else {
                    AppUtil.ShowSuccess("Saved Failed.");
                }

            }

            if (data.MikrotikCount < 1) {
                window.location.href = "/mikrotik/index";
            }

            //if (data.MikrotikCount < 1) {
            //    var mytable = $('#tblMikrotik')
            //        .DataTable({
            //            //  "destroy": true, "filter": false,
            //            "deferRender": true,
            //            "paging": true,
            //            "lengthChange": false,
            //            "searching": false,
            //            "ordering": true,
            //            "info": true,
            //            "autoWidth": false,
            //            "sDom": 'lfrtip'
            //        });
            //    mytable.draw();
            //}
            MikrotikManager.clearForSaveInformation();
            $("#mdlMikrotikInsert").modal('hide');

        },
        InsertMikrotikFromPopUpFail: function (data) {
            AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
            console.log(data);
        },

        //InsertMikrotik: function () {
        //    
        //    //AppUtil.ShowWaitingDialog();
        //    
        //    var url = "/Mikrotik/InsertMikrotik";
        //    var MikrotikName = AppUtil.GetIdValue("MikrotikName");
        //    var MikrotikAddress = AppUtil.GetIdValue("MikrotikAddress");


        //    //setTimeout(function () {
        //    var Mikrotik = { MikrotikName: MikrotikName, MikrotikAddress: MikrotikAddress };

        //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        //    
        //    var datas = JSON.stringify({ Mikrotik_Client: Mikrotik });
        //    AppUtil.MakeAjaxCall(url, "POST", datas, MikrotikManager.InsertMikrotikSuccess, MikrotikManager.InsertMikrotikUpFail);
        //    // }, 500);
        //},
        //InsertMikrotikSuccess: function (data) {
        //    
        //    //AppUtil.HideWaitingDialog();
        //    console.log(data);
        //    if (data.SuccessInsert === true) {
        //        AppUtil.ShowSuccess("Saved Successfully.");
        //        // if (data.Mikrotik) {
        //        // 
        //        // var Mikrotik = (data.Mikrotik);
        //        // $("#tblMikrotik>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Mikrotik.MikrotikID + '/></td><td>' + Mikrotik.MikrotikName + '</td><td><a href="" id="showMikrotikForUpdate">Show</a></td></tr>');
        //        // }
        //    }
        //    if (data.SuccessInsert === false) {
        //        
        //        if (data.AlreadyInsert = true) {
        //            AppUtil.ShowSuccess("Mikrotik Already Added. Choose different Mikrotik Name.");
        //        } else {
        //            AppUtil.ShowSuccess("Saved Failed.");
        //        }

        //    }
        //    //window.location.href = "/Mikrotik/Index";
        //    $("#mdlMikrotikInsert").modal('hide');

        //},
        //InsertMikrotikUpFail: function (data) {
        //    AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        //    console.log(data);
        //},

        ShowMikrotikDetailsByIDForUpdate: function (MikrotikID) {

            
            //AppUtil.ShowWaitingDialog();
            //setTimeout(function () {
            
            var url = "/Mikrotik/GetMikrotikDetailsByID/";
            var data = { MikrotikID: MikrotikID };
            data = MikrotikManager.addRequestVerificationToken(data);

            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, MikrotikManager.ShowMikrotikDetailsByIDForUpdateSuccess, MikrotikManager.ShowMikrotikDetailsByIDForUpdateError);

            //}, 500);

        },
        ShowMikrotikDetailsByIDForUpdateSuccess: function (data) {
            

            //AppUtil.HideWaitingDialog();
            console.log(data);
            
            var MikrotikDetailsJsonParse = (data.MikrotikDetails);
            $("#MikrotikNames").val(MikrotikDetailsJsonParse.MikName);
            $("#RealIPs").val(MikrotikDetailsJsonParse.RealIP);
            $("#MikUserNames").val(MikrotikDetailsJsonParse.MikUserName);
            $("#MikPasswords").val(MikrotikDetailsJsonParse.MikPassword);
            $("#APIPorts").val(MikrotikDetailsJsonParse.APIPort);
            $("#WebPorts").val(MikrotikDetailsJsonParse.WebPort);
           // $("#IPPoolIDs").val(MikrotikDetailsJsonParse.IPPoolID);


            $("#mdlMikrotikUpdate").modal("show");
        },
        ShowMikrotikDetailsByIDForUpdateError: function (data) {

            
            //AppUtil.HideWaitingDialog();
            console.log(data);
        },

        UpdateMikrotikInformation: function () {
            
            //AppUtil.ShowWaitingDialog();
            // var MikrotikID = MikrotikID;
          //  var IPPoolID = $("#IPPoolIDs").val();
            var MikrotikName = $("#MikrotikNames").val();
            var RealIP = $("#RealIPs").val();
            var MikUserName = $("#MikUserNames").val();
            var MikPassword = $("#MikPasswords").val();
            var APIPort = $("#APIPorts").val();
            var WebPort = $("#WebPorts").val();

            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
            var headers = {};
            headers['__RequestVerificationToken'] = AntiForgeryToken;
            var url = "/Mikrotik/UpdateMikrotik";
            //var MikrotikInfomation = ({ MikrotikID: MikrotikID, IPPoolID: IPPoolID, RealIP: RealIP, MikUserName: MikUserName, MikPassword: MikPassword, APIPort: APIPort, WebPort: WebPort });
            var MikrotikInfomation = ({ MikrotikID: MikrotikID,MikName:MikrotikName,  RealIP: RealIP, MikUserName: MikUserName, MikPassword: MikPassword, APIPort: APIPort, WebPort: WebPort });
            var data = JSON.stringify({ MikrotikInfoForUpdate: MikrotikInfomation });
            AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", data, headers, MikrotikManager.UpdateMikrotikInformationSuccess, MikrotikManager.UpdateMikrotikInformationFail);
        },
        UpdateMikrotikInformationSuccess: function (data) {

            //AppUtil.HideWaitingDialog();
            
            if (data.UpdateSuccess === true) {
                var MikrotikInformation = (data.MikrotikUpdateInformation);

                $("#tblMikrotik tbody>tr").each(function () {
                    
                    var MikrotikID = $(this).find("td:eq(0) input").val();
                    var index = $(this).index();
                    if (MikrotikInformation[0].MikrotikID == MikrotikID) {
                        
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(1)").text(MikrotikInformation[0].MikName);
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(2)").text(MikrotikInformation[0].RealIP);
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(3)").text(MikrotikInformation[0].MikUserName);
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(4)").text(MikrotikInformation[0].MikPassword);
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(5)").text(MikrotikInformation[0].APIPort);
                        $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(6)").text(MikrotikInformation[0].WebPort);
                      //  $('#tblMikrotik tbody>tr:eq(' + index + ')').find("td:eq(6)").text(MikrotikInformation[0].IPPool.PoolName);

                    }
                });
                AppUtil.ShowSuccess("Update Successfully. ");
            }
            if (data.UpdateSuccess === false) {
                if (AlreadyInsert = true) {
                    AppUtil.ShowSuccess("Mikrotik Already Added. Choose different Mikrotik. ");
                }
            }

            $("#mdlMikrotikUpdate").modal('hide');

            MikrotikManager.clearForUpdateInformation();
            console.log(data);
        },
        UpdateMikrotikInformationFail: function () {
            
            console.log(data);
            //AppUtil.HideWaitingDialog();
        },

        clearForSaveInformation: function () {
            $("#MikrotikName").val("");
            $("#RealIP").val("");
            $("#MikUserName").val("");
            $("#MikPassword").val("");
            $("#APIPort").val("");
            $("#WebPort").val("");
        },
        clearForUpdateInformation: function () {
            $("#MikrotikNames").val("");
            $("#RealIP").val("");
            $("#MikUserName").val("");
            $("#MikPassword").val("");
            $("#APIPort").val("");
            $("#WebPort").val("");
        }
    }

