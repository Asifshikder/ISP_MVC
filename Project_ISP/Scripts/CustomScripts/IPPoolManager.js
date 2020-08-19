
var IPPoolManager =
{
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        
        if (AppUtil.GetIdValue("PoolName") === '') {
            AppUtil.ShowSuccess("Please Insert Pool Name.");
            return false;
        }
        if (AppUtil.GetIdValue("StartRange") === '') {
            AppUtil.ShowSuccess("Please Insert Start Ranges. ");
            return false;
        }
        if (AppUtil.GetIdValue("EndRange") === '') {
            AppUtil.ShowSuccess("Please Add End Range.");
            return false;
        }
        return true;
    },

    UpdateValidation: function () {
        
        if (AppUtil.GetIdValue("PoolNames") === '') {
            AppUtil.ShowSuccess("Please Insert Pool Name.");
            return false;
        }
        if (AppUtil.GetIdValue("StartRanges") === '') {
            AppUtil.ShowSuccess("Please Insert Start Range. ");
            return false;
        }
        if (AppUtil.GetIdValue("EndRanges") === '') {
            AppUtil.ShowSuccess("Please Add End Range.");
            return false;
        }
        return true;
    },

    InsertIPPoolFromPopUp: function () {
        
        //AppUtil.ShowWaitingDialog();
        
        var url = "/IPPool/InsertIPPoolFromPopUp";
        var PoolName = AppUtil.GetIdValue("PoolName");
        var StartRange = AppUtil.GetIdValue("StartRange");
        var EndRange = AppUtil.GetIdValue("EndRange");


        //  setTimeout(function () {
        var IPPool = { PoolName: PoolName, StartRange: StartRange, EndRange: EndRange };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var headers = {};
        headers['__RequestVerificationToken'] = AntiForgeryToken;

        
        var datas = JSON.stringify({ IPPool_Client: IPPool });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", datas, headers, IPPoolManager.InsertIPPoolFromPopUpSuccess, IPPoolManager.InsertIPPoolFromPopUpFail);
        // }, 500);
    },
    InsertIPPoolFromPopUpSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        console.log(data);

        //if (data.IPPoolCount <1) {
        //    $('#tblIPPool').dataTable().fnDestroy();
        //    $("#tblIPPool>tbody").empty();
        //}
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.IPPool) {
                
                var IPPool = (data.IPPool);
                $("#tblIPPool>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + IPPool.IPPoolID + '></td><td>' + IPPool.PoolName + '</td><td>' + IPPool.StartRange + '</td><td>' + IPPool.EndRange + '</td><td><a href="" id="showIPPoolForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {
            
            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("IPPool Already Added. Choose different IPPool Name.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }

        }

        if (data.IPPoolCount < 1) {
            window.location.href = "/IPPool/index";
        }

        //if (data.IPPoolCount < 1) {
        //    var mytable = $('#tblIPPool')
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
        IPPoolManager.clearForSaveInformation();
        $("#mdlIPPoolInsert").modal('hide');

    },
    InsertIPPoolFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
        console.log(data);
    },

    //InsertIPPool: function () {
    //    
    //    //AppUtil.ShowWaitingDialog();
    //    
    //    var url = "/IPPool/InsertIPPool";
    //    var IPPoolName = AppUtil.GetIdValue("IPPoolName");
    //    var IPPoolAddress = AppUtil.GetIdValue("IPPoolAddress");


    //    //setTimeout(function () {
    //    var IPPool = { IPPoolName: IPPoolName, IPPoolAddress: IPPoolAddress };

    //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

    //    
    //    var datas = JSON.stringify({ IPPool_Client: IPPool });
    //    AppUtil.MakeAjaxCall(url, "POST", datas, IPPoolManager.InsertIPPoolSuccess, IPPoolManager.InsertIPPoolUpFail);
    //    // }, 500);
    //},
    //InsertIPPoolSuccess: function (data) {
    //    
    //    //AppUtil.HideWaitingDialog();
    //    console.log(data);
    //    if (data.SuccessInsert === true) {
    //        AppUtil.ShowSuccess("Saved Successfully.");
    //        // if (data.IPPool) {
    //        // 
    //        // var IPPool = (data.IPPool);
    //        // $("#tblIPPool>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + IPPool.IPPoolID + '/></td><td>' + IPPool.IPPoolName + '</td><td><a href="" id="showIPPoolForUpdate">Show</a></td></tr>');
    //        // }
    //    }
    //    if (data.SuccessInsert === false) {
    //        
    //        if (data.AlreadyInsert = true) {
    //            AppUtil.ShowSuccess("IPPool Already Added. Choose different IPPool Name.");
    //        } else {
    //            AppUtil.ShowSuccess("Saved Failed.");
    //        }

    //    }
    //    //window.location.href = "/IPPool/Index";
    //    $("#mdlIPPoolInsert").modal('hide');

    //},
    //InsertIPPoolUpFail: function (data) {
    //    AppUtil.ShowSuccess("Error Occoured. Contact with  Administrator.");
    //    console.log(data);
    //},

    ShowIPPoolDetailsByIDForUpdate: function (IPPoolID) {

        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/IPPool/GetIPPoolDetailsByID/";
        var data = { IPPoolID: IPPoolID };
        data = IPPoolManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, IPPoolManager.ShowIPPoolDetailsByIDForUpdateSuccess, IPPoolManager.ShowIPPoolDetailsByIDForUpdateError);

        //}, 500);

    },
    ShowIPPoolDetailsByIDForUpdateSuccess: function (data) {
        

        //AppUtil.HideWaitingDialog();
        console.log(data);
        
        var IPPoolDetailsJsonParse = (data.IPPoolDetails);
        $("#PoolNames").val(IPPoolDetailsJsonParse.PoolName);
        $("#StartRanges").val(IPPoolDetailsJsonParse.StartRange);
        $("#EndRanges").val(IPPoolDetailsJsonParse.EndRange);


        $("#mdlIPPoolUpdate").modal("show");
    },
    ShowIPPoolDetailsByIDForUpdateError: function (data) {

        
        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateIPPoolInformation: function () {
        
        //AppUtil.ShowWaitingDialog();
        // var IPPoolID = IPPoolID;
        var PoolName = $("#PoolNames").val();
        var StartRange = $("#StartRanges").val();
        var EndRange = $("#EndRanges").val();

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var headers = {};
        headers['__RequestVerificationToken'] = AntiForgeryToken;
        var url = "/IPPool/UpdateIPPool";
        var IPPoolInfomation = ({ IPPoolID: IPPoolID, PoolName: PoolName, StartRange: StartRange, EndRange: EndRange });
        var data = JSON.stringify({ IPPoolInfoForUpdate: IPPoolInfomation });
        AppUtil.MakeAjaxCallJSONAntifergery(url, "POST", data, headers, IPPoolManager.UpdateIPPoolInformationSuccess, IPPoolManager.UpdateIPPoolInformationFail);
    },
    UpdateIPPoolInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        
        if (data.UpdateSuccess === true) {
            var IPPoolInformation = (data.IPPoolUpdateInformation);

            $("#tblIPPool tbody>tr").each(function () {
                
                var IPPoolID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (IPPoolInformation[0].IPPoolID == IPPoolID) {
                    
                    $('#tblIPPool tbody>tr:eq(' + index + ')').find("td:eq(1)").text(IPPoolInformation[0].PoolName);
                    $('#tblIPPool tbody>tr:eq(' + index + ')').find("td:eq(2)").text(IPPoolInformation[0].StartRange);
                    $('#tblIPPool tbody>tr:eq(' + index + ')').find("td:eq(3)").text(IPPoolInformation[0].EndRange);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("IPPool Already Added. Choose different IPPool. ");
            }
        }

        $("#mdlIPPoolUpdate").modal('hide');

        IPPoolManager.clearForUpdateInformation();
        console.log(data);
    },
    UpdateIPPoolInformationFail: function () {
        
        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    clearForSaveInformation: function () {
        $("#PoolName").val("");
        $("#StartRange").val("");
        $("#EndRange").val("");
    },
    clearForUpdateInformation: function () {
        $("#PoolNames").val("");
        $("#StartRanges").val("");
        $("#EndRanges").val("");
    }
}

