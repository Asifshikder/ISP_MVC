var ProductCurrentStatusManager = {
    DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsID: function (StockDetailsID, DistributionID) {
        var url = "/ProductCurrentStatus/DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockDetailsID: StockDetailsID, DistributionID: DistributionID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsIDSuccess, ProductCurrentStatusManager.DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsIDFailed);
    },
    DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsIDSuccess: function (data) {
        
        if (data.SuccessDeleteDistribution === true) {
            $("#tblRunningList>tbody>tr").each(function () {
                
                var index = $(this).index();
                var stockDetailsFromTable = $(this).find("td:eq(0) input").val();
                if (stockDetailsFromTable == data.StockDetailsID) {
                    $("#tblRunningList>tbody>tr:eq(" + index + ")").remove();
                }
            });
            AppUtil.ShowSuccess("Data Removed Successfully.");
        }
        if (data.SuccessDeleteDistribution === false) {
            AppUtil.ShowError("Data Can Not remove. Contact With Administrator.");
        }

    },
    DeleteDistributionSearchByClientOrStockDetailsOrStockDetailsIDFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },


    FindItemStatusByClientOrByStockDetailsID: function (stockID, stockDetailsID, clientDetailsID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/FindItemStatusByClientOrByStockDetailsID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: stockID, StockDetailsID: stockDetailsID, ClientDetailsID: clientDetailsID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.FindItemStatusByClientOrByStockDetailsIDSuccess, ProductCurrentStatusManager.FindItemStatusByClientOrByStockDetailsIDError);
    },
    FindItemStatusByClientOrByStockDetailsIDSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblRunningList').dataTable().fnDestroy();
        $("#tblRunningList>tbody").empty();

        var lstRunningItem = (data.tblRunningList);

        $.each(lstRunningItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);
            $('#tblRunningList tbody').append('<tr><td hidden="hidden" style="padding:0px"><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="" name="" value=' + item.DistributionID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.clientLoginName + '</td><td>' + item.employeeName + '</td><td>' + item.popName + '</td><td>' + item.boxName + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"><button id="btnDelete" type="button" class="btn btn-danger btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-remove"></span></button></td></tr>');
            //<td align="center"><button id="btnDelete" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td>
        });

        var mytable = $('#tblRunningList').DataTable({
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
    FindItemStatusByClientOrByStockDetailsIDError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    ValidationOnlySearch: function () {
        
        if (AppUtil.GetIdValue("StockID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }

        return true;

    },

    FindItemByItemOrClientIDSearch: function () {
        
        if (AppUtil.GetIdValue("lstStockID") === '' && AppUtil.GetIdValue("lstStockDetailsID") === '' && AppUtil.GetIdValue("lstClientDetailsID") === '') {
            AppUtil.ShowSuccess("Please Select some option for search.");
            return false;
        }

        return true;

    },

    FindCableDetailsByCableTypeOrStockOrClientIDSearch: function () {
        
        if (AppUtil.GetIdValue("CableTypeID") === '' && AppUtil.GetIdValue("CableStockID") === '' && AppUtil.GetIdValue("lstClientDetailsID") === '') {
            AppUtil.ShowSuccess("Please Select some option for search.");
            return false;
        }

        return true;

    },

    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    SearchStockDetailsListByStockIDForWarrenty: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForWarrenty/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForWarrentySuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForWarrentyError);
    },
    SearchStockDetailsListByStockIDForWarrentySuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblWarrentyList').dataTable().fnDestroy();
        $("#tblWarrentyList>tbody").empty();

        var lstWarrentyItem = (data.tblWarrentyList);

        $.each(lstWarrentyItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);
            $('#tblWarrentyList tbody').append('<tr><td  hidden="hidden" style="padding:0px"><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td  hidden="hidden" style="padding:0px"><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td  hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"><button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td></tr>');

        });

        var mytable = $('#tblWarrentyList').DataTable({
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
    SearchStockDetailsListByStockIDForWarrentyError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchStockDetailsListByStockIDForDead: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForDead/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForDeadSuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForDeadError);
    },
    SearchStockDetailsListByStockIDForDeadSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblDeadList').dataTable().fnDestroy();
        $("#tblDeadList>tbody").empty();

        var lstDeadItem = (data.tblDeadList);

        $.each(lstDeadItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);
            $('#tblDeadList tbody').append('<tr><td hidden><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"><button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td></tr>');

        });

        var mytable = $('#tblDeadList').DataTable({
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
    SearchStockDetailsListByStockIDForDeadError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchStockDetailsListByStockIDForRunning: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForRunning/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForRunningSuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForRunningError);
    },
    SearchStockDetailsListByStockIDForRunningSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblRunningList').dataTable().fnDestroy();
        $("#tblRunningList>tbody").empty();

        var lstRunningItem = (data.tblRunningList);

        $.each(lstRunningItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);


            var link = "";
            if (item.ClientDetailsID && item.TransactionID) {
                link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
            }
            else {
                link = item.Name;
            }

            $('#tblRunningList tbody').append('<tr><td hidden="hidden" style="padding:0px"><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + link + '</td><td>' + item.employeeName + '</td><td>' + item.popName + '</td><td>' + item.boxName + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td></tr>');
            //<td align="center"><button id="btnDelete" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td>
        });

        var mytable = $('#tblRunningList').DataTable({
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
    SearchStockDetailsListByStockIDForRunningError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchStockDetailsListByStockIDForAvailable: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForAvailable/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForAvailableSuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForAvailableError);
    },
    SearchStockDetailsListByStockIDForAvailableSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblAvailableList').dataTable().fnDestroy();
        $("#tblAvailableList>tbody").empty();

        var lstAvailableItem = (data.tblAvailableList);

        $.each(lstAvailableItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);
            $('#tblAvailableList tbody').append('<tr><td hidden><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"><button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td></tr>');

        });

        var mytable = $('#tblAvailableList').DataTable({
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
    SearchStockDetailsListByStockIDForAvailableError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchStockDetailsListByStockIDForRepair: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForRepair/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForRepairSuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForRepairError);
    },
    SearchStockDetailsListByStockIDForRepairSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblRepairList').dataTable().fnDestroy();
        $("#tblRepairList>tbody").empty();

        var lstRepairItem = (data.tblRepairList);

        $.each(lstRepairItem, function (index, item) {
            
            console.log(item);
            
            console.log(item);
            $('#tblRepairList tbody').append('<tr><td hidden><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"><button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td></tr>');

        });

        var mytable = $('#tblRepairList').DataTable({
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
    SearchStockDetailsListByStockIDForRepairError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchStockDetailsListByStockIDForTotal: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchStockDetailsListByCriteriaForTotal/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForTotalSuccess, ProductCurrentStatusManager.SearchStockDetailsListByStockIDForTotalError);
    },
    SearchStockDetailsListByStockIDForTotalSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblTotalList').dataTable().fnDestroy();
        $("#tblTotalList>tbody").empty();

        var lstTotalItem = (data.tblTotalList);

        $.each(lstTotalItem, function (index, item) {
            var showStatusChangeButton = "";
            if (item.SectionID != 17 && item.ProductStatusID != 2) {
                showStatusChangeButton = '<button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button>';
            }
            
            console.log(item);
            
            console.log(item);
            $('#tblTotalList tbody').append('<tr><td hidden="hidden" style="padding:0px"><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center">' + showStatusChangeButton + '</td></tr>');

        });

        var mytable = $('#tblTotalList').DataTable({
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
    SearchStockDetailsListByStockIDForTotalError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    SearchDistributionListByCriteriaForTotalWorkingSection: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/SearchDistributionListByCriteriaForTotalWorkingSection/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchDistributionListByCriteriaForTotalWorkingSectionSuccess, ProductCurrentStatusManager.SearchDistributionListByCriteriaForTotalWorkingSectionError);
    },
    SearchDistributionListByCriteriaForTotalWorkingSectionSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblTotalList').dataTable().fnDestroy();
        $("#tblTotalList>tbody").empty();

        var lstTotalItem = (data.tblTotalList);

        $.each(lstTotalItem, function (index, item) {
            var link = "";
            if (item.ClientDetailsID && item.TransactionID) {
                link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
            }
            else {
                link = item.Name;
            }

            var showStatusChangeButton = "";
            //if (item.SectionID != 17 && item.ProductStatusID != 2) {
                showStatusChangeButton = '<button id="btnEditSectionProductStatus" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button>';
            //}
            
            console.log(item);
            
            console.log(item);
            $('#tblTotalList tbody').append('<tr><td hidden="hidden" style="padding:0px"><input type="hidden" id="StockIDD" name="StockDetailsIDD" value=' + item.StockDetailsID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="SectionID" name="SectionID" value=' + item.SectionID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.ProductStatusID + '></td><td hidden="hidden" style="padding:0px"><input type="hidden" id="ProductStatusID" name="ProductStatusID" value=' + item.DistributionID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + link + '</td><td>' + item.EmployeeName + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center">' + showStatusChangeButton + '</td></tr>');

        });

        var mytable = $('#tblTotalList').DataTable({
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
    SearchDistributionListByCriteriaForTotalWorkingSectionError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    GetProductStatusBySectionID: function (SectionID) {

        var url = "/ProductCurrentStatus/GetProductStatusBySectionID/";
        var data = ({ SectionID: SectionID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.GetProductStatusBySectionIDForWarrentySuccess, ProductCurrentStatusManager.GetProductStatusBySectionIDForWarrentyFailed);
    },
    GetProductStatusBySectionIDForWarrentySuccess: function (data) {
        
        console.log(data);
        $("#lstProductStatusID").find("option").not(":first").remove();
        $.each((data.lstProductStatus), function (index, item) {
            $("#lstProductStatusID").append($("<option></option>").val(item.ProductStatusID).text(item.ProductStatusName));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    GetProductStatusBySectionIDForWarrentyFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },

    resetPopUpSectinoAndProductStatus: function () {
        $("#lstSectionID").prop("selectedIndex", 0);
        $("#lstProductStatusID").find("option").not(":first").remove();
        // PopManager.clearForUpdateInformation();
    },

    ChangeProductStatusAndSection: function (StockDetailsID, NewSectionID, NewProductStatusID) {
        
        var url = "/ProductCurrentStatus/ChangeProductStatusAndSection/";
        var data = ({ StockDetailsID: StockDetailsID, NewSectionID: NewSectionID, NewProductStatusID: NewProductStatusID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.ChangeProductStatusAndSectionSuccess, ProductCurrentStatusManager.ChangeProductStatusAndSectionFail);
    },
    ChangeProductStatusAndSectionSuccess: function (data) {
        if (data.WorkingSectionRunning === true) {
            AppUtil.ShowSuccess("Sorry this product status can not change.");
        }

        // ProductCurrentStatusManager.RemoveStockDistributionInformation();
        
        if (data.Success === false) {
            AppUtil.ShowError("Something is wrong when Updating the Section And Status. Contact with administrator.");
        }

        //var SectionID;
        //var ProductStatusID;
        if (data.Success === true) {
            if (SectionID == data.StockDetails.SectionID && ProductStatusID == data.StockDetails.ProductStatusID) {
                AppUtil.ShowSuccess("Update Successfully.");
            }
            if (SectionID != data.StockDetails.SectionID && ProductStatusID != data.StockDetails.ProductStatusID) {

                $("#" + tblName + ">tbody>tr").each(function (index, item) {
                    
                    var index = $(this).index();

                    var stockDetailsID = $(this).find("td:eq(0) input").val();
                    if (stockDetailsID == StockDetailsID) {

                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").remove();
                        //$("#tblWarrentyList>tbody>tr:eq(" + index + ")").find("td:eq(1) input").val(data.StockDetails.SectionID);
                        //$("#tblWarrentyList>tbody>tr:eq(" + index + ")").find("td:eq(2) input").val(data.StockDetails.ProductStatusID);
                        //$("#tblWarrentyList>tbody>tr:eq(" + index + ")").find("td:eq(6) ").text(data.StockDetails.Section.SectionName);
                        //$("#tblWarrentyList>tbody>tr:eq(" + index + ")").find("td:eq(7) ").text(data.StockDetails.ProductStatus.ProductStatusName);
                    }
                });
            }
        }
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
    },
    ChangeProductStatusAndSectionFail: function (data) {
        //ProductCurrentStatusManager.RemoveStockDistributionInformation();
        
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
        console.log(data);
        return "error";
    },


    ChangeProductStatusAndSectionForAll: function (StockDetailsID, NewSectionID, NewProductStatusID) {
        
        var url = "/ProductCurrentStatus/ChangeProductStatusAndSection/";
        var data = ({ StockDetailsID: StockDetailsID, NewSectionID: NewSectionID, NewProductStatusID: NewProductStatusID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.ChangeProductStatusAndSectionForAllSuccess, ProductCurrentStatusManager.ChangeProductStatusAndSectionForAllFail);
    },
    ChangeProductStatusAndSectionForAllSuccess: function (data) {

        // ProductCurrentStatusManager.RemoveStockDistributionInformation();
        if (data.WorkingSectionRunning === true) {
            AppUtil.ShowSuccess("Sorry this product status can not change.");
        }

        
        if (data.Success === false) {
            AppUtil.ShowError("Something is wrong when Updating the Section And Status. Contact with administrator.");
        }

        //var SectionID;
        //var ProductStatusID;
        if (data.Success === true) {
            if (SectionID == data.StockDetails.SectionID && ProductStatusID == data.StockDetails.ProductStatusID) {
                AppUtil.ShowSuccess("Update Successfully.");
            }
            if (SectionID != data.StockDetails.SectionID && ProductStatusID != data.StockDetails.ProductStatusID) {

                $("#" + tblName + ">tbody>tr").each(function (index, item) {
                    
                    var index = $(this).index();

                    var stockDetailsID = $(this).find("td:eq(0) input").val();
                    if (stockDetailsID == StockDetailsID) {

                        // $("#" + tblName + ">tbody>tr:eq(" + index + ")").remove();
                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(1) input").val(data.StockDetails.SectionID);
                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(2) input").val(data.StockDetails.ProductStatusID);
                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(7) ").text(data.StockDetails.Section.SectionName);
                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(8) ").text(data.StockDetails.ProductStatus.ProductStatusName);
                    }
                });
            }
        }
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
    },
    ChangeProductStatusAndSectionForAllFail: function (data) {
        //ProductCurrentStatusManager.RemoveStockDistributionInformation();
        
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
        console.log(data);
        return "error";
    },


    ChangeProductStatusAndSectionForAllWorkingItem: function (StockDetailsID,DistributionID, NewSectionID, NewProductStatusID) {
        
        var url = "/ProductCurrentStatus/ChangeProductStatusAndSectionForWorkingList/";
        var data = ({ StockDetailsID: StockDetailsID, DistributionID: DistributionID, NewSectionID: NewSectionID, NewProductStatusID: NewProductStatusID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.ChangeProductStatusAndSectionForAllWorkingItemSuccess, ProductCurrentStatusManager.ChangeProductStatusAndSectionForAllWorkingItemFail);
    },
    ChangeProductStatusAndSectionForAllWorkingItemSuccess: function (data) {

        // ProductCurrentStatusManager.RemoveStockDistributionInformation();
        if (data.WorkingSectionRunning === true) {
            AppUtil.ShowSuccess("Sorry this product status can not change.");
        }

        
        if (data.Success === false) {
            AppUtil.ShowError("Something is wrong when Updating the Section And Status. Contact with administrator.");
        }

        //var SectionID;
        //var ProductStatusID;
        if (data.Success === true) {
            if (SectionID == data.StockDetails.SectionID && ProductStatusID == data.StockDetails.ProductStatusID) {
                AppUtil.ShowSuccess("Update Successfully.");
            }
            if (SectionID != data.StockDetails.SectionID && ProductStatusID != data.StockDetails.ProductStatusID) {

                $("#" + tblName + ">tbody>tr").each(function (index, item) {
                    
                    var index = $(this).index();

                    var stockDetailsID = $(this).find("td:eq(0) input").val();
                    if (stockDetailsID == StockDetailsID) {
                        $("#" + tblName + ">tbody>tr:eq(" + index + ")").remove();
                        // $("#" + tblName + ">tbody>tr:eq(" + index + ")").remove();
                        //$("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(1) input").val(data.StockDetails.SectionID);
                        //$("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(2) input").val(data.StockDetails.ProductStatusID);
                        //$("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(6) ").text(data.StockDetails.Section.SectionName);
                        //$("#" + tblName + ">tbody>tr:eq(" + index + ")").find("td:eq(7) ").text(data.StockDetails.ProductStatus.ProductStatusName);
                    }
                });
            }
        }
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
    },
    ChangeProductStatusAndSectionForAllWorkingItemFail: function (data) {
        //ProductCurrentStatusManager.RemoveStockDistributionInformation();
        
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
        console.log(data);
        return "error";
    },

    GetStockDetailsItemListByStockID: function (StockID) {

        var url = "/ProductCurrentStatus/GetStockDetailsItemListByStockID/";
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.GetStockDetailsItemListByStockIDSuccess, ProductCurrentStatusManager.GetStockDetailsItemListByStockIDFailed);
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


    SearchCableBoxOrDrumNameByCableTypeID: function (CableTypeID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchCableBoxOrDrumNameByCableTypeID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableTypeID: CableTypeID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.SearchCableBoxOrDrumNameByCableTypeIDSuccess, ProductCurrentStatusManager.SearchCableBoxOrDrumNameByCableTypeIDError);
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

    ChangeCableStatus: function (_cableDistributionID, popRdbCableStatus) {
        
        var url = "/ProductCurrentStatus/ChangeCableSection/";
        var data = ({ CableDistributionID: _cableDistributionID, NewCableStatus: popRdbCableStatus });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.ChangeCableStatusSuccess, ProductCurrentStatusManager.ChangeCableStatusFail);
    },
    ChangeCableStatusSuccess: function (data) {
        if (!data.NewCableTypeSameAsOldType === true) {
            $("#mdlPopUp_Specific_Cable_Details").modal("hide");
        }
        
        if (data.NewCableTypeSameAsOldType === true) {
            AppUtil.ShowError("New Status Matched With The Old Cable Status. Please Change The Type And Save Again.");
        }
        //if (data.WorkingSectionRunning === false) {
        //    AppUtil.ShowSuccess("Sorry Cable Status Can not change. Contact with administrator.");
        //}
        if (data.Success === true) {
            //   return Json(new { Success = true, ChangeStatus = true, CableDistributionID = CableDistributionID, NewCableStatus = NewCableStatus }, JsonRequestBehavior.AllowGet);
        //    $('#tblCableAssignedList').dataTable().fnDestroy();
            if (data.DeleteStatus === true) {
                $("#tblCableAssignedList>tbody>tr").each(function (index, item) {
                    
                    var index = $(this).index();
                    var cableDistributionID = $(this).find("td:eq(0) input").val();
                    if (cableDistributionID == data.CableDistributionID) {
                        $("#tblCableAssignedList>tbody>tr:eq(" + index + ")").remove();
                    }
                });
            }
            if (data.ChangeStatus === true) {
                $("#tblCableAssignedList>tbody>tr").each(function (index, item) {
                    
                    var changeText = "";
                    if (data.NewCableStatus == 2) {
                        changeText = "Old Box";
                    }
                    if (data.NewCableStatus == 3) {
                        changeText = "Stolen";
                    }
                    if (data.NewCableStatus == 4) {
                        changeText = "Not Working";
                    }

                    var index = $(this).index();
                    var cableDistributionID = $(this).find("td:eq(0) input").val();
                    if (cableDistributionID == data.CableDistributionID) {
                        $("#tblCableAssignedList>tbody>tr:eq(" + index + ")").find("td:eq(4)").text(AppUtil.ParseDateTime(data.Date));
                        $("#tblCableAssignedList>tbody>tr:eq(" + index + ")").find("td:eq(8)").text(changeText);
                    }
                });
            }
            //var mytable = $('#tblCableAssignedList').DataTable({
            //    "paging": true,
            //    "lengthChange": false,
            //    "searching": true,
            //    "ordering": true,
            //    "info": true,
            //    "autoWidth": true,
            //    "sDom": 'lfrtip'
            //});
            //mytable.draw();
        }
        if (data.Success === false) {
            AppUtil.ShowError("Sorry Cable Status Can Not Update. Contact With Administrator.");
        }

    },
    ChangeCableStatusFail: function (data) {
        //ProductCurrentStatusManager.RemoveStockDistributionInformation();
        
        ProductCurrentStatusManager.resetPopUpSectinoAndProductStatus();
        SectionID = "";
        ProductStatusID = "";
        StockDetailsID = '';
        console.log(data);
        return "error";
    },


    FindCableDetailsByCableBoxOrDrumOrByClientDetailsID: function (cableTypeID, cableStockID, clientDetailsID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/ProductCurrentStatus/FindCableDetailsByCableBoxOrDrumOrByClientDetailsID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableTypeID: cableTypeID, CableStockID: cableStockID, ClientDetailsID: clientDetailsID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.FindCableDetailsByCableBoxOrDrumOrByClientDetailsIDSuccess, ProductCurrentStatusManager.FindCableDetailsByCableBoxOrDrumOrByClientDetailsIDError);
    },
    FindCableDetailsByCableBoxOrDrumOrByClientDetailsIDSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;

        $('#tblCableAssignedList').dataTable().fnDestroy();
        $("#tblCableAssignedList>tbody").empty();

        var lstCableAssignedList = (data.tblCableAssignedList);

        $.each(lstCableAssignedList, function (index, item) {
            var cableStatus = "";

            var link = "";
            if (item.ClientDetailsID && item.TransactionID) {
                link = "<a href='#' onclick='GetClientDetailsByClientDetailsID(" + item.ClientDetailsID + "," + item.TransactionID + ")'>" + item.Name + "</a>";
            }
            else {
                link = item.Name;
            }

            if (item.CableStatus == 1) {
                cableStatus = "Running";
            }
            if (item.CableStatus == 2) {
                cableStatus = "Old Box";
            }
            if (item.CableStatus == 3) {
                cableStatus = "Stolen";
            }
            if (item.CableStatus == 4) {
                cableStatus = "Not Working";
            }
            
            console.log(item);



            $('#tblCableAssignedList tbody').append('<tr><td hidden="hidden" style="padding:0px"><input type="hidden" id="" name="" value=' + item.CableDistributionID + '></td>\
            <td>' + item.CableTypeName + '</td><td>' + item.CableBoxName + '</td><td>' + item.AmountOfCableUsed + "M" + '</td>\
            <td>' + AppUtil.ParseDateTime(item.Date) + '</td><td>' + link + '</td><td>' + item.AssignedEmployee + '</td>\
            <td>' + item.CableForEmployee + '</td><td>' + cableStatus + '</td><td align="center"><button id="btnEdit" type="button" class="btn btn-success btn-sm padding" data-toggle="confirmation" data-placement="top"><span class="glyphicon glyphicon-edit"></span></button></td></tr>');

        });

        var mytable = $('#tblCableAssignedList').DataTable({
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
    FindCableDetailsByCableBoxOrDrumOrByClientDetailsIDError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },


    PrintProductListOverView: function () {
        
        var url = "/Excel/CreateReportForItemOverView";

        var StockID = AppUtil.GetIdValue("StockID");

        
        //var data = ({});
        var data = ({ StockID: StockID});
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.PrintProductListOverViewSuccess, ProductCurrentStatusManager.PrintProductListOverViewFail);
    },
    PrintProductListOverViewSuccess: function (data) {
        
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
    PrintProductListOverViewFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    PrintProductByStatusAndItemNameList: function (status) {
        
        var url = "/Excel/CreateReportForProductByStatus";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var StockID = AppUtil.GetIdValue("StockID"); //('#ConnectionDate').datepicker('getDate');

        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        //var data = ({});
        var data = ({ StockID: StockID,ProductStatus:status });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.PrintProductByStatusAndItemNameListSuccess, ProductCurrentStatusManager.PrintProductByStatusAndItemNameListFail);
    },
    PrintProductByStatusAndItemNameListSuccess: function (data) {
        
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
    PrintProductByStatusAndItemNameListFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    GetProductTotalCountByStockID: function (status) {
        
        var url = "/ProductCurrentStatus/CountNumberOfItemById";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var StockID = AppUtil.GetIdValue("StockID"); //('#ConnectionDate').datepicker('getDate');

        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        //var data = ({});
        var data = ({ StockID: StockID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.GetProductTotalCountByStockIDSuccess, ProductCurrentStatusManager.GetProductTotalCountByStockIDFail);
    },
    GetProductTotalCountByStockIDSuccess: function (data) {
        

        $("#ShowCountLabel").prop("hidden", false);
        $("#TotalItem").html(data.TotalItem+" Piece");
        console.log(data);
    },
    GetProductTotalCountByStockIDFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    PrintCableUsed: function (status) {
        
        var url = "/Excel/CreateReportForCableDetailsHistory";

        var CableTypeID = AppUtil.GetIdValue("CableTypeID"); //('#ConnectionDate').datepicker('getDate');
        var CableStockID = AppUtil.GetIdValue("CableStockID");
        var ClientDetailsID = AppUtil.GetIdValue("lstClientDetailsID");

        var data = ({ CableTypeID: CableTypeID, CableStockID: CableStockID, ClientDetailsID: ClientDetailsID });
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.PrintCableUsedSuccess, ProductCurrentStatusManager.PrintCableUsedFail);
    },
    PrintCableUsedSuccess: function (data) {
        
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
    PrintCableUsedFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    PrintProductByWorkingStatus: function (status) {
        
        var url = "/Excel/CreateReportForProductForWorkingStatus";
        // var url = '@Url.Action("PrintAllClientInExcel","Excel")';

        var StockID = AppUtil.GetIdValue("StockID"); //('#ConnectionDate').datepicker('getDate');

        // var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        
        //var data = ({});
        var data = ({ StockID: StockID});
        data = ProductCurrentStatusManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ProductCurrentStatusManager.PrintProductByWorkingStatusSuccess, ProductCurrentStatusManager.PrintProductByWorkingStatusFail);
    },
    PrintProductByWorkingStatusSuccess: function (data) {
        
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
    PrintProductByWorkingStatusFail: function (data) {
        
        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },
}