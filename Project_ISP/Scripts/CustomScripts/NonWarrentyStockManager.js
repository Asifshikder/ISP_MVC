var NonWarrentyStockManager = {
    

    AddInListForNonWarrentyProductValidation: function () {
        
        if (AppUtil.GetIdValue("ItemID") === '') {
            AppUtil.ShowSuccess("Please Select Item.");
            return false;
        }
        //if (AppUtil.GetIdValue("BrandID") === '') {
        //    AppUtil.ShowSuccess("Please Select Brand.");
        //    return false;
        //}
        if (AppUtil.GetIdValue("txtQuantity") === '') {
            AppUtil.ShowSuccess("Please add quantity.");
            return false;
        }
        //if (AppUtil.GetIdValue("SupplierID") === '') {
        //    AppUtil.ShowSuccess("Please Select Supplier.");
        //    return false;
        //}
        //if (AppUtil.GetIdValue("txtSupplierInvoice") === '') {
        //    AppUtil.ShowSuccess("Please add supplier invoice number.");
        //    return false;
        //}

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
    
    AddItemInListForNonWarrentyProduct: function (serialNumber) {
        
        var ItemID = AppUtil.GetIdValue("ItemID");
        var ItemText = $("#ItemID option:selected").text();;
        var SupplierID = AppUtil.GetIdValue("SupplierID");
        var SupplierText = $("#SupplierID option:selected").text();;
        var BarndID = AppUtil.GetIdValue("BrandID");
        var BarndText = $("#BrandID option:selected").text();
        //var Serial = AppUtil.GetIdValue("txtSerialNumber");
        var Serial = serialNumber;
        var SupplierInvoice = AppUtil.GetIdValue("txtSupplierInvoice");
        var WarrentyProduct = false;
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
            NonWarrentyStockManager.SerialIDExistInDatabase(Serial);
            
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
    
    SerialIDExistInDatabase: function (serial) {
        

        var url = "/Stock/CheckSerialNumberIsExistOrNot/";
        //int ClientDetailsID, int Amount, string remarks
        var data = ({ serial: serial });
        //  data = AdvancePaymentManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, NonWarrentyStockManager.SerialIDExistInDatabaseSuccess, NonWarrentyStockManager.SerialIDExistInDatabaseFail);
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
    

    InsertStockItem: function (ItemList) {
        
        var url = "/Stock/InsertStockItem/";
        var data = JSON.stringify({ ItemList: ItemList });
        AppUtil.MakeAjaxCall(url, "POST", data, NonWarrentyStockManager.InsertStockItemSuccess, NonWarrentyStockManager.InsertStockItemFail);
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
                AppUtil.ShowSuccess("Item Stored Successfully.");
            }
            if (data.SavedSuccessfully === false) {
                AppUtil.ShowSuccess("Something Wrond When Item Stored in System.");
            }
            $("#tblStock>tbody>tr").remove();
        }
    },
    InsertStockItemFail: function () {
        itemArray = [];
        
        return "error";
    },

   
}