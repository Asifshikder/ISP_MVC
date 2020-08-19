var StockManager = {

    AddInListForWarrentyProductValidation: function () {
        
        if (AppUtil.GetIdValue("ItemID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }
        if (AppUtil.GetIdValue("BrandID") === '') {
            AppUtil.ShowSuccess("Please Select Brand.");
            return false;
        }
        if (AppUtil.GetIdValue("txtSerialNumber") === '') {
            AppUtil.ShowSuccess("Please add serial number.");
            return false;
        }
        if (AppUtil.GetIdValue("SupplierID") === '') {
            AppUtil.ShowSuccess("Please Select Supplier.");
            return false;
        }
        if (AppUtil.GetIdValue("txtSupplierInvoice") === '') {
            AppUtil.ShowSuccess("Please add supplier invoice number.");
            return false;
        }

        return true;
    },

    Validation: function () {
        
        if (AppUtil.GetIdValue("ItemID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }

        return true;

    },
    CableAddInListValidation: function (selectedRDB) {
        if (selectedRDB == 1) {
            if (AppUtil.GetIdValue("lstEmployeeID") === '') {
                AppUtil.ShowSuccess("Please Select For Which Employee you are distributing cable.");
                return false;
            }
        }
        if (selectedRDB == 2) {

            if (AppUtil.GetIdValue("lstAssignEmployee") === '') {
                AppUtil.ShowSuccess("Please Select Employee who will be assign for this client.");
                return false;
            }
            if (AppUtil.GetIdValue("lstClientDetailsID") === '') {
                AppUtil.ShowSuccess("Please Select Client.");
                return false;
            }
        }

        
        if (AppUtil.GetIdValue("CableTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Cable Type.");
            return false;
        }

        if (AppUtil.GetIdValue("CableStockID") === '') {
            AppUtil.ShowSuccess("Please Select Box/Drum Number.");
            return false;
        }
        if (AppUtil.GetIdValue("txtCableQuantity") === '') {
            AppUtil.ShowSuccess("Please Select Employee Name.");
            return false;
        }
        if (AppUtil.GetIdValue("txtCableQuantity") === '') {
            AppUtil.ShowSuccess("Please Enter Cable Length.");
            return false;
        }
        else {
            var cableAssignToClient = AppUtil.GetIdValue("txtCableQuantity");
            if (cableAssignToClient > _actualAmountCanBeUsedAfterCalculatingWithTableWithTheDBResult) {
                AppUtil.ShowError(" Cable Length Can Not Greater than " + _actualAmountCanBeUsedAfterCalculatingWithTableWithTheDBResult + " M");
                return false;
            }
        }

        return true;
    },

    CableAddingValidation: function () {
        if (AppUtil.GetIdValue("CableTypeID") === '') {
            AppUtil.ShowSuccess("Please Select Cable Type.");
            return false;
        }

        if (AppUtil.GetIdValue("txtBoxNumber") === '') {
            AppUtil.ShowSuccess("Please Select Box Number.");
            return false;
        }

        if (AppUtil.GetIdValue("txtReadingFrom") === '') {
            AppUtil.ShowSuccess("Please Add Reading Start .");
            return false;
        }
        if (AppUtil.GetIdValue("txtReadingTo") === '') {
            AppUtil.ShowSuccess("Please Add Reading End .");
            return false;
        }

        if (AppUtil.GetIdValue("txtCableQuantity") === '') {
            AppUtil.ShowSuccess("Please Add Cable Quantity.");
            return false;
        }
        return true;
    },
    AddInListForStockDistributionValidation: function () {
        
        if (AppUtil.GetIdValue("lstStockID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }

        if (AppUtil.GetIdValue("StockDetailsID") === '') {
            AppUtil.ShowSuccess("Please Select Item Serial.");
            return false;
        }

        //if (AppUtil.GetIdValue("PopID") == '' && AppUtil.GetIdValue("BoxID") == '' ) {
        //    AppUtil.ShowSuccess("Please Select Pop Or Box.");
        //    return false;
        //}

        if (AppUtil.GetIdValue("EmployeeID") === '') {
            AppUtil.ShowSuccess("Please Select Employee .");
            return false;
        }

        if (AppUtil.GetIdValue("DistributionReasonID") === '') {
            AppUtil.ShowSuccess("Please Select Distribution Reason.");
            return false;
        }

        return true;
    },
    AddInArrayForAddingInListOfStockDistribution: function () {
        
        if (AppUtil.GetIdValue("lstStockIDForPopUp") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }

        if (AppUtil.GetIdValue("StockDetailsIDPopUp") === '') {
            AppUtil.ShowSuccess("Please Select Item Serial.");
            return false;
        }

        if (AppUtil.GetIdValue("SectionID") === '') {
            AppUtil.ShowSuccess("Please Select Section for this item .");
            return false;
        }

        if (AppUtil.GetIdValue("ProductStatusID") === '') {
            AppUtil.ShowSuccess("Please Select Product Status.");
            return false;
        }

        return true;
    },
    RemoveStockDistributionInformation: function () {
        $("#lstStockID").prop("selectedIndex", 0);
        $("#StockDetailsID").find("option").not(":first").remove();
        $("#PopID").prop("selectedIndex", 0);
        $("#BoxID").prop("selectedIndex", 0);
        $("#CustomerID").prop("selectedIndex", 0);
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#DistributionReasonID").prop("selectedIndex", 0);
    },


    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    SearchStockListByItemID: function (StockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchStockListByCriteria/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ ItemID: StockID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.SearchStockListByItemIDSuccess, StockManager.SearchStockListByItemIDError);
    },
    SearchStockListByItemIDSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Success");
        console.log(data);
        var index = 0;


        $('#tblItemList').dataTable().fnDestroy();
        $("#tblItemList>tbody").empty();

        //var listOfClient = JSON.parse(data.SearchClientList)
        var lstItem = (data.lstDynamic);

        $.each(lstItem, function (index, item) {
            
            console.log(item);
            // $.each(item, function (index, items) {
            
            console.log(item);
            $('#tblItemList tbody').append('<tr><td hidden><input type="hidden" id="StockID" name="StockID" value=' + item.StockID + '></td><td hidden><input type="hidden" id="StockID" name="StockID" value=' + item.StockDetailsID + '></td><td>' + item.ItemName + '</td><td>' + item.BrandName + '</td><td>' + item.Serial + '</td><td>' + item.SectionName + '</td><td>' + item.ProductStatusName + '</td><td align="center"> <button type="button" id="btnDelete" class="btn btn-success  btn-sm" data-toggle="modal" data-target="#popModalForDeletePermently"><span class="glyphicon glyphicon-remove"></span></button></td></tr>');

            //  index = parseInt(index) + 1;
            // });
        });


        var mytable = $('#tblItemList').DataTable({
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
    SearchStockListByItemIDError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    DeleteStockItem: function (StockDetailsID) {

        
        var url = "/Stock/DeleteItemFromStockList/";

        //AppUtil.ShowWaitingDialog();
        //code before the pause
        //setTimeout(function () {
            var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

            
            var datas = ({ StockDetailsID: StockDetailsID });
            datas = StockManager.addRequestVerificationToken(datas);
            AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", datas, StockManager.DeleteStockItemSuccess, StockManager.DeleteStockItemFail);
       // }, 50);
    },
    DeleteStockItemSuccess: function (data) {
        
        //AppUtil.HideWaitingDialog();

        if (data.DeleteStatus === true) {

            $('#tblItemList').dataTable().fnDestroy();
            $("#tblItemList>tbody>tr").each(function () {
                

                var index = $(this).index();
                var stockDetailsID = $(this).find("td:eq(1) input").val();
                if (stockDetailsID == data.StockDetailsID) {
                    
                    $('#tblItemList tbody>tr:eq(' + index + ')').remove();
                }
            });
            AppUtil.ShowSuccess("Successfully removed.");
            var mytable = $('#tblItemList').DataTable({
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

        if (data.DeleteStatus === false) {
            if (data.ProductStatus === true) {
                AppUtil.ShowSuccess("Sorry this item already used. Contact with administrator.");
            } else {
                AppUtil.ShowSuccess("Some Information Can not removed.");
            }
        }


        console.log(data);
    },
    DeleteStockItemFail: function (data) {
        
        //AppUtil.HideWaitingDialog();
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    AddItemInList: function () {
        
        var ItemID = AppUtil.GetIdValue("ItemID");
        var ItemText = $("#ItemID option:selected").text();;
        var SupplierID = AppUtil.GetIdValue("SupplierID");
        var SupplierText = $("#SupplierID option:selected").text();;
        var BarndID = AppUtil.GetIdValue("BrandID");
        var BarndText = $("#BrandID option:selected").text();
        var Serial = AppUtil.GetIdValue("txtSerialNumber");
        var SupplierInvoice = AppUtil.GetIdValue("txtSupplierInvoice");
        //  var length = $("#tblStock>tbody>tr>td:eq(2):contains(" + Serial + ")").length;
        // var length = $('#tblStock').DataTable().columns(2).search(Serial).draw();

        $("#tblStock>tbody>tr").each(function () {
            
            var serialNumber = $(this).find("td:eq(2)").text();
            if (serialNumber == Serial) {
                AddedInList = true;
            }

        });

        if (AddedInList === true) {
            alert("Already added in list.");
        } else {
            StockManager.SerialIDExistInDatabase(Serial);
            
            if (SerialExist === false) {
                $("#tblStock>tbody").append("<tr><td style='padding:0px;'><input type='hidden' value=" + ItemID + "></td><td>" + ItemText + "</td><td>" + Serial + "</td>\
                                         <td style='padding:0px;'><input type='hidden' value=" + BarndID + "></td><td>" + BarndText + "</td>\
                                         <td style='padding:0px;'><input type='hidden' value=" + SupplierID + "></td><td>" + SupplierText + "</td><td>" + SupplierInvoice + "</td>\
                                         <td align='center'><button id='btnDelete' type='button' class='btn btn-danger btn-sm padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td>\
            </tr>");
            } if (SerialExist === true) {
                alert("Already added in database with this Serial Number.");
            }



        }

        AddedInList = "";
        SerialExist = "";
    },

    AddItemInListForCable: function () {
        
        var CableTypeID = AppUtil.GetIdValue("CableTypeID");
        var CableTypeText = $("#CableTypeID option:selected").text();;
        var BoxNumber = AppUtil.GetIdValue("txtBoxNumber");
        var readingStart = $("#txtReadingFrom").val();
        var readingEnd = $("#txtReadingTo").val();
        var CableQuantity = AppUtil.GetIdValue("txtCableQuantity");
        var BrandID = $("#BrandID").val();
        var BrandName = $("#BrandID").val() > 0 ? $("#BrandID option:selected").text() : "";
        var SupplierID = $("#SupplierID").val();
        var SupplierName = $("#SupplierID").val() > 0 ? $("#SupplierID option:selected").text() : "";
        var SupplierInvoice = $("#txtSupplierInvoice").val();
        $("#tblStockForCable>tbody>tr").each(function () {
            
            var BoxNumberFromTable = $(this).find("td:eq(2)").text();
            if (BoxNumber.trim() == BoxNumberFromTable.trim()) {
                AddedInList = true;
            }

        });

        if (AddedInList === true) {
            alert("Box Name Already added in list.");
        } else {
            StockManager.BoxNameExistInDatabase(BoxNumber.trim());
            
            if (BoxNameExist === false) {
                $("#tblStockForCable>tbody").append("<tr><td style='' hidden='hidden'><input type='hidden' value=" + CableTypeID + "></td><td>" + CableTypeText + "</td><td>" + BoxNumber + "</td>\
                                         <td>" + readingStart + "</td><td>" + readingEnd + "</td><td>" + CableQuantity + "</td>\
                    <td style='' hidden='hidden'><input type='hidden' value=" + BrandID + "></td><td>" + BrandName + "</td>\
                    <td style='' hidden='hidden'><input type='hidden' value=" + SupplierID + "></td><td>" + SupplierName + "</td><td>" + SupplierInvoice + "</td>\
                                         <td align='center'><button id='btnDelete' type='button' class='btn btn-danger btn-sm padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td>\
            </tr>");
            } if (BoxNameExist === true) {
                alert("Cable Stored in Database with this Box Name. Choose different one.");
            }

        }

        AddedInList = "";
        BoxNameExist = "";
    },


    BoxNameExistInDatabase: function (BoxName) {
        

        var url = "/Stock/CheckBoxNumberIsExistOrNot/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({ BoxName: BoxName });
        //  data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.BoxNameExistInDatabaseSuccess, StockManager.BoxIDExistInDatabaseFail);
    },
    BoxNameExistInDatabaseSuccess: function (data) {
        
        if (data.BoxNameExistOrNot === true) {
            BoxNameExist = true;
            return true;
        }
        if (data.BoxNameExistOrNot === false) {
            BoxNameExist = false;
            return false;
        }
    },
    BoxIDExistInDatabaseFail: function () {
        
        return "error";
    },


    SerialIDExistInDatabase: function (serial) {
        

        var url = "/Stock/CheckSerialNumberIsExistOrNot/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({ serial: serial });
        //  data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.SerialIDExistInDatabaseSuccess, StockManager.SerialIDExistInDatabaseFail);
    },
    SerialIDExistInDatabaseSuccess: function (data) {
        
        if (data.SerialExistOrNot === true) {
            SerialExist = true;
            return true;
        }
        if (data.SerialExistOrNot === false) {
            SerialExist = false;
            return false;
        }
    },
    SerialIDExistInDatabaseFail: function () {
        
        return "error";
    },


    //SerialIDExistInDatabase: function (serial) {
    //    
    //    var url = "/Stock/CheckSerialNumberIsExistOrNot/";
    //    //int ClientDetailsID, int Amount, string remarks
    //    var data = ({ serial: serial });
    //    //  data = AdvancePaymentManager.addRequestVerificationToken(data);
    //    AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.SerialIDExistInDatabaseSuccess, StockManager.SerialIDExistInDatabaseFail);
    //},
    //SerialIDExistInDatabaseSuccess: function (data) {
    //    
    //    if (data.SerialExistOrNot === true) {
    //        SerialExist = true;
    //        return true;
    //    }
    //    if (data.SerialExistOrNot === false) {
    //        SerialExist = false;
    //        return false;
    //    }
    //},
    //SerialIDExistInDatabaseFail: function () {
    //    
    //    return "error";
    //},


    InsertCableInStock: function (CableList) {
        
        var url = "/Stock/InsertStockDistributionForCable/";
        //  var json = JSON.stringify(CableList);
        var data = JSON.stringify({ "lstCableStock": CableList });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCall(url, "POST", data, StockManager.InsertCableInStockSuccess, StockManager.InsertCableInStockFail);
    },
    InsertCableInStockSuccess: function (data) {
        itemArray = [];
        
        if (data.Success === false) {
            if (data.BoxNameAlreadyAdded === true) {
                AppUtil.ShowError(data.BoxNameList + " Already Stored in DataBase.");
            }
        }
        if (data.Success === true) {
            if (data.SavedSuccessfully === true) {
                AppUtil.ShowSuccess("Cable add in stock successfully.");
            }
            if (data.SavedSuccessfully === false) {
                AppUtil.ShowSuccess("Something Wrond When Item Stored in System.");
            }
            $("#tblStockForCable>tbody>tr").remove();
        }
    },
    InsertCableInStockFail: function (data) {
        itemArray = [];
        console.log(data);
        
        return "error";
    },

    InsertStockItem: function (ItemList) {
        
        var url = "/Stock/InsertStockItem/";
        var data = JSON.stringify({ ItemList: ItemList });
        AppUtil.MakeAjaxCall(url, "POST", data, StockManager.InsertStockItemSuccess, StockManager.InsertStockItemFail);
    },
    InsertStockItemSuccess: function (data) {
        itemArray = [];
        
        if (data.Success === false) {
            if (data.SerialAlreadyAdded === true) {
                AppUtil.ShowError(data.SerialList + " Already Stored in DataBase.");
            }
        }
        if (data.Success === true) {
            if (data.SavedSuccessfully === true) {
                AppUtil.ShowSuccess("Warrenty Product Added Successfully.");
            }
            if (data.SavedSuccessfully === false) {
                AppUtil.ShowSuccess("Something Wrong When Add Warrenty Product in System.");
            }
            $("#tblStock>tbody>tr").remove();
        }
    },
    InsertStockItemFail: function () {
        itemArray = [];
        
        return "error";
    },

    InsertStockDistribution: function (ItemList) {
        
        var url = "/Stock/InsertStockDistribution/";
        var data = JSON.stringify({ lstClientStockStockDetailsForDistributions: ItemList });
        AppUtil.MakeAjaxCall(url, "POST", data, StockManager.InsertStockDistributionSuccess, StockManager.InsertStockDistributionFail);
    },
    InsertStockDistributionSuccess: function (data) {
        itemArray = [];
        lstStockDetailsIDForRemoveWhenPassedByStockID = [];
        StockManager.RemoveStockDistributionInformation();
        
        if (data.Success === false) {
            if (data.SerialAlreadyAdded === true) {
                AppUtil.ShowError(data.SerialList + " Already Using Some Purpous.");
            }
        }
        if (data.Success === true) {
            if (data.SavedSuccessfully === true) {
                AppUtil.ShowSuccess("Stock Distributed Successfully.");
            }
            if (data.SavedSuccessfully === false) {
                AppUtil.ShowSuccess("Something Wrong When Stock Distributed.");
            }
            $("#tblStockAssign>tbody>tr").remove();
        }
    },
    InsertStockDistributionFail: function () {
        itemArray = [];
        StockManager.RemoveStockDistributionInformation();
        
        return "error";
    },

    GetStockDetailsItemListByStockID: function (StockID) {

        var url = "/Stock/GetStockDetailsItemListByStockID/";
        var data = ({ StockID: StockID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.GetStockDetailsItemListByStockIDSuccess, StockManager.GetStockDetailsItemListByStockIDFailed);
    },
    GetStockDetailsItemListByStockIDSuccess: function (data) {
        
        console.log(data);
        $("#StockDetailsID").find("option").not(":first").remove();
        $.each((data.lstStockDetails), function (index, item) {
            $("#StockDetailsID").append($("<option></option>").val(item.StockDetailsID).text(item.Serial));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    GetStockDetailsItemListByStockIDFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },

    GetProductStatusBySectionID: function (SectionID) {

        var url = "/Stock/GetProductStatusBySectionID/";
        var data = ({ SectionID: SectionID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.GetProductStatusBySectionIDSuccess, StockManager.GetProductStatusBySectionIDFailed);
    },
    GetProductStatusBySectionIDSuccess: function (data) {
        
        console.log(data);
        $("#ProductStatusID").find("option").not(":first").remove();
        $.each((data.lstProductStatus), function (index, item) {
            $("#ProductStatusID").append($("<option></option>").val(item.ProductStatusID).text(item.ProductStatusName));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });


    },
    GetProductStatusBySectionIDFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },

    GetStockDetailsItemListByStockIDForPopUp: function (StockID) {

        var url = "/Stock/GetStockDetailsItemListByStockIDForPopUp/";
        var data = ({ StockID: StockID, lstStockDetailsIDForRemoveWhenPassedByStockID: lstStockDetailsIDForRemoveWhenPassedByStockID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.GetStockDetailsItemListByStockIDForPopUpSuccess, StockManager.GetStockDetailsItemListByStockIDForPopUpFailed);
    },
    GetStockDetailsItemListByStockIDForPopUpSuccess: function (data) {
        
        console.log(data);
        $("#StockDetailsIDPopUp").find("option").not(":first").remove();
        $.each((data.lstStockDetails), function (index, item) {
            $("#StockDetailsIDPopUp").append($("<option></option>").val(item.StockDetailsID).text(item.Serial));
            //$.each(data, function (index, itemData) {
            //    $("#BatchID").append($('<option></option>').val(itemData.BatchID).html(itemData.BatchName))
            //});
        });

    },
    GetStockDetailsItemListByStockIDForPopUpFailed: function (data) {
        
        console.log(data);
        alert("Fail");
    },


    SearchCableBoxOrDrumNameByCableTypeID: function (cableTypeID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchCableBoxOrDrumNameByCableTypeID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableTypeID: cableTypeID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.SearchCableBoxOrDrumNameByCableTypeIDSuccess, StockManager.SearchCableBoxOrDrumNameByCableTypeIDError);
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

    SearchCableQuantityStockedByCableBoxOrDrumName: function (CableStockID) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/SearchCableQuantityStockedByCableBoxOrDrumName/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ CableStockID: CableStockID });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.SearchCableQuantityStockedByCableBoxOrDrumNameSuccess, StockManager.SearchCableQuantityStockedByCableBoxOrDrumNameError);
    },
    SearchCableQuantityStockedByCableBoxOrDrumNameSuccess: function (data) {
        
        AppUtil.ShowSuccess("Success");
        console.log(data);

        _afterCalculatingTheCableFromTable = 0;
        _actualAmountCanBeUsedAfterCalculatingWithTableWithTheDBResult = 0;
        cableLengthFromDB = 0;
        cableUsedFromDB = 0;
        cableCanBeUseForThisClientFromDB = 0;

        if (data.Success === true) {
            //var a = data.CableStock[0].CableQuantity;
            //var b = data.CableStock[0].UsedQuantityFromThisBox;

            cableLengthFromDB = data.CableStock[0].CableQuantity;
            cableUsedFromDB = data.CableStock[0].UsedQuantityFromThisBox;
            cableCanBeUseForThisClientFromDB = cableLengthFromDB - cableUsedFromDB;

            var CableStockID = data.CableStock[0].CableStockID;

            $("#tblCableList>tbody>tr").each(function (index, item) {
                
                var index = $(this).index();
                var cableStockID = $(this).find("td:eq(0) input").val();
                if (cableStockID == CableStockID) {
                    var cableQuantity = $(this).find("td:eq(1) input").val();
                    _afterCalculatingTheCableFromTable += parseInt(cableQuantity);
                }
            });


            _actualAmountCanBeUsedAfterCalculatingWithTableWithTheDBResult = cableCanBeUseForThisClientFromDB - _afterCalculatingTheCableFromTable;

            $("#lblTotalCableLength").text(cableLengthFromDB).css("display", "block");
            $("#lblUsedCableLength").text(cableUsedFromDB + parseInt(_afterCalculatingTheCableFromTable));
            $("#lblDueCableLength").html(_actualAmountCanBeUsedAfterCalculatingWithTableWithTheDBResult);

            $("#lblUsedCableLength").css("display", "block");
            $("#lblDueCableLength").css("display", "block");
        }
        if (data.Success === false) {
            AppUtil.ShowError("Sorry Information Not Found.");
        }
    },
    SearchCableQuantityStockedByCableBoxOrDrumNameError: function (data) {
        
        AppUtil.ShowError("Error");
        console.log(data);
    },

    AddStockItemInAssignList: function () {
        
        var StockID = AppUtil.GetIdValue("lstStockID");
        var StockDetailsID = AppUtil.GetIdValue("StockDetailsID");
        lstStockDetailsIDForRemoveWhenPassedByStockID.push(StockDetailsID);
        var PopID = AppUtil.GetIdValue("PopID");
        var BoxID = AppUtil.GetIdValue("BoxID");
        var CustomerID = AppUtil.GetIdValue("CustomerID");
        var EmployeeID = AppUtil.GetIdValue("EmployeeID");
        var DistributionReasonID = AppUtil.GetIdValue("DistributionReasonID");

        var OldStockID = singleOldInformation[0].lstStockIDForPopUp;
        var OldStockDetailsID = singleOldInformation[0].StockDetailsIDPopUp;
        var OldSectionID = singleOldInformation[0].SectionID;
        var OldProductStatusID = singleOldInformation[0].ProductStatusID;

        var ItemName = $("#lstStockID option:selected").text();
        var ItemSerial = $("#StockDetailsID option:selected").text();
        var PopName = (PopID == "") ? "" : $("#PopID option:selected").text();
        var BoxName = (BoxID == "") ? "" : $("#BoxID option:selected").text();
        var CustomerLoginName = (CustomerID == "") ? "" : $("#CustomerID option:selected").text();
        var EmployeeName = $("#EmployeeID option:selected").text();
        var DistributionReasonName = $("#DistributionReasonID option:selected").text();

        var Remarks = $("#txtAreaRemarks").val();

        //  var length = $("#tblStock>tbody>tr>td:eq(2):contains(" + Serial + ")").length;
        // var length = $('#tblStock').DataTable().columns(2).search(Serial).draw();

        $("#tblStockAssign>tbody>tr").each(function () {
            debugger;
            var stockDetailsID = $(this).find("td:eq(2) input").val();
            if (stockDetailsID == StockDetailsID) {
                AddedInList = true;
            }

        });

        if (AddedInList === true) {
            alert("This Item Already added in list. Choose Different Serial.");
        } else {
            // StockManager.SerialIDExistInDatabase(Serial);

            //   if (SerialExist === false) {
            $("#tblStockAssign>tbody").append("<tr><td style='padding:0px;'><input type='hidden' value=" + StockID + "></td><td>" + ItemName + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + StockDetailsID + "></td><td>" + ItemSerial + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + PopID + "></td><td>" + PopName + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + BoxID + "></td><td>" + BoxName + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + CustomerID + "></td><td>" + CustomerLoginName + "</td><td>" + Remarks + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + EmployeeID + "></td><td>" + EmployeeName + "</td>\
            <td style='padding:0px;'><input type='hidden' value=" + DistributionReasonID + "></td><td>" + DistributionReasonName + "</td>\
            <td align='center'><button id='btnDelete' type='button' class='btn btn-danger btn-sm padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td>\
            <td style='padding:0px;'><input type='hidden' value=" + OldStockID + "></td>\
            <td style='padding:0px;'><input type='hidden' value=" + OldStockDetailsID + "></td>\
            <td style='padding:0px;'><input type='hidden' value=" + OldSectionID + "></td>\
            <td style='padding:0px;'><input type='hidden' value=" + OldProductStatusID + "></td>\
            </tr>");
            //} if (SerialExist === true) {
            //    alert("Already added in database with this Serial Number.");
            //}



        }

        AddedInList = "";
        SerialExist = "";
        singleOldInformation = [];

        $("#lstStockID").prop("selectedIndex", 0);
        $("#StockDetailsID").find("option").not(":first").remove();
        $("#PopID").prop("selectedIndex", 0);
        $("#BoxID").prop("selectedIndex", 0);
        $("#CustomerID").prop("selectedIndex", 0);
        $("#EmployeeID").prop("selectedIndex", 0);
        $("#DistributionReasonID").prop("selectedIndex", 0);
        $("#txtAreaRemarks").val("");
    },

    resetPopUpStockDistribution: function () {
        $("#lstStockIDForPopUp").prop("selectedIndex", 0);
        $("#StockDetailsIDPopUp").find("option").not(":first").remove();
        $("#DistributionReasonIDPopUp").prop("selectedIndex", 0);
        $("#SectionID").prop("selectedIndex", 0);
        $("#ProductStatusID").prop("selectedIndex", 0);
        // PopManager.clearForUpdateInformation();
    },

    InsertCableStockDistributionForClientOrEmployee: function (cableAssignArray) {
        

        //AppUtil.ShowWaitingDialog();
        var url = "/Stock/InsertCableStockDistributionForClientOrEmployee/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ ClientCableDistribution: cableAssignArray });
        data = StockManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, StockManager.InsertCableStockDistributionForClientOrEmployeeSuccess, StockManager.InsertCableStockDistributionForClientOrEmployeeFail);
    },
    InsertCableStockDistributionForClientOrEmployeeSuccess: function (data) {
        
        console.log(data);
        cableAssignArray = [];
        $("#tblCableList>tbody>tr").remove();
        ClearInformationToDefault();

        if (data.CableAvialableInDB === false) {
            AppUtil.ShowError("Sorry Given Quantiry is greater than the available quantity.Box Name: " + data.BoxNameListWhichIsNotAvialable + "");
        }
        if (data.Success === false) {
            AppUtil.ShowError("Sorry Data Can Not Save. Contact With Administrator");
        }
        if (data.Success === true) {
            AppUtil.ShowSuccess("Cable Distributed Successfully.");
        }
    },
    InsertCableStockDistributionForClientOrEmployeeFail: function(data) {
        
        console.log(data);
        cableAssignArray = [];
        AppUtil.ShowError("Sorry Data Can Not Save. Contact With Administrator");
    }
}