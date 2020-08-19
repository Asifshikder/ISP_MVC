var ItemManager = {
    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    CreateValidation: function () {
        var status = true;
        //if (AppUtil.GetIdValue("ItemFor") === '') {
        //    AppUtil.ShowSuccessOnControl("Please Select Item For. ", "ItemFor", "top center");
        //    status = false;
        //}
        if (AppUtil.GetIdValue("ItemName") === '') {
            AppUtil.ShowSuccessOnControl("Please Insert Item ", "ItemName", "top center");
            status = false;
        }
        return status;
    },
    UpdateValidation: function () {
        var status = true;
        //if (AppUtil.GetIdValue("ItemFors") === '') {
        //    AppUtil.ShowSuccessOnControl("Please Select Item For. ", "ItemFors", "top center");
        //    status = false;
        //}
        if (AppUtil.GetIdValue("ItemNames") === '') {
            AppUtil.ShowSuccessOnControl("Please Insert Item ", "ItemNames", "top center");
            status = false;
        }
        return status;
    },

    InsertItemFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Item/InsertItemFromPopUp";
        var ItemName = AppUtil.GetIdValue("ItemName");
        //var ItemFor = AppUtil.GetIdValue("ItemFor");
         
        //  setTimeout(function () {
        //var Item = { ItemName: ItemName, ItemFor: ItemFor };
        var Item = { ItemName: ItemName/*, ItemFor: ItemFor*/ };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Item_Client: Item });
        AppUtil.MakeAjaxCall(url, "POST", datas, ItemManager.InsertItemFromPopUpSuccess, ItemManager.InsertItemFromPopUpFail);
        //  }, 500);
    },
    InsertItemFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Item) {
                table.draw();
                //window.location.reload();
                //var Item = (data.Item);
                //$("#tblItem>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Item.ItemID + '></td><td>' + data.ItemFor + '</td><td>' + Item.ItemName + '</td><td><a href="" id="showItemForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Item Already Added. Choose different Item.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }
        }
        ItemManager.clearForSaveInformation();
        $("#mdlItemInsert").modal('hide');
    },
    InsertItemFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    //InsertItem: function () {

    //    //AppUtil.ShowWaitingDialog();

    //    var url = "/Item/InsertItem";
    //    var ItemName = AppUtil.GetIdValue("ItemName");


    //    //  setTimeout(function () {
    //    var Item = { ItemName: ItemName };

    //    var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


    //    var datas = JSON.stringify({ Item_Client: Item });
    //    AppUtil.MakeAjaxCall(url, "POST", datas, ItemManager.InsertItemSuccess, ItemManager.InsertItemUpFail);
    //    //  }, 500);
    //},
    //InsertItemSuccess: function (data) {

    //    //AppUtil.HideWaitingDialog();
    //    console.log(data);
    //    if (data.SuccessInsert === true) {
    //        AppUtil.ShowSuccess("Saved Successfully.");
    //        // if (data.Item) {
    //        // 
    //        // var Item = (data.Item);
    //        // $("#tblItem>tbody").append('<tr><td style="padding:0px"><input type="hidden" id="" value=' + Item.ItemID + '/></td><td>' + Item.ItemName + '</td><td><a href="" id="showItemForUpdate">Show</a></td></tr>');
    //        // }
    //    }
    //    if (data.SuccessInsert === false) {

    //        if (data.AlreadyInsert = true) {
    //            AppUtil.ShowSuccess("Item Already Added. Choose different Item.");
    //        } else {
    //            AppUtil.ShowSuccess("Saved Failed.");
    //        }

    //    }
    //    //window.location.href = "/Item/Index";
    //    $("#mdlItemInsert").modal('hide');

    //},
    //InsertItemUpFail: function (data) {
    //    AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
    //    console.log(data);
    //},

    ShowItemDetailsByIDForUpdate: function (ItemID) {


        //AppUtil.ShowWaitingDialog();
        //  setTimeout(function () {

        var url = "/Item/GetItemDetailsByID/";
        var data = { ItemID: ItemID };
        data = ItemManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ItemManager.ShowItemDetailsByIDForUpdateSuccess, ItemManager.ShowItemDetailsByIDForUpdateError);

        //  }, 500);

    },
    ShowItemDetailsByIDForUpdateSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        var ItemDetailsJsonParse = (data.ItemDetails);
        $("#ItemNames").val(ItemDetailsJsonParse.ItemName);
        //$("#ItemFors").val(ItemDetailsJsonParse.ItemFor);
        $("#mdlItemUpdate").modal("show");
    },
    ShowItemDetailsByIDForUpdateError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateItemInformation: function () {

        //AppUtil.ShowWaitingDialog();
        // var ItemID = ItemID;
        var ItemName = $("#ItemNames").val();
        //var ItemFor = $("#ItemFors").val();


        var url = "/Item/UpdateItem";
        var ItemInfomation = ({ ItemID: ItemID, ItemName: ItemName });
        //var ItemInfomation = ({ ItemID: ItemID, ItemName: ItemName, ItemFor: ItemFor });
        var data = JSON.stringify({ ItemInfoForUpdate: ItemInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, ItemManager.UpdateItemInformationSuccess, ItemManager.UpdateItemInformationFail);
    },
    UpdateItemInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.UpdateSuccess === true) {
            var ItemInformation = (data.ItemUpdateInformation);

            $("#tblItem tbody>tr").each(function () {

                var ItemID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ItemInformation[0].ItemID == ItemID) { 
                    //$('#tblItem tbody>tr:eq(' + index + ')').find("td:eq(1)").text(data.ItemFor);
                    $('#tblItem tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ItemInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Item Already Added. Choose different Item. ");
            }
        }

        ItemManager.clearForUpdateInformation();
        $("#mdlItemUpdate").modal('hide');
        console.log(data);
    },
    UpdateItemInformationFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    InsertBandwithResellerItemFromPopUp: function () {

        //AppUtil.ShowWaitingDialog();

        var url = "/Item/InsertBandwithResellerItemFromPopUp";
        var ItemName = AppUtil.GetIdValue("ItemName");
        //var ItemFor = AppUtil.GetIdValue("ItemFor");

        //  setTimeout(function () {
        //var Item = { ItemName: ItemName, ItemFor: ItemFor };
        var Item = { ItemName: ItemName/*, ItemFor: ItemFor*/ };

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();


        var datas = JSON.stringify({ Item_Client: Item });
        AppUtil.MakeAjaxCall(url, "POST", datas, ItemManager.InsertBandwithResellerItemFromPopUpSuccess, ItemManager.InsertBandwithResellerItemFromPopUpFail);
        //  }, 500);
    },
    InsertBandwithResellerItemFromPopUpSuccess: function (data) {

        //AppUtil.HideWaitingDialog();
        console.log(data);
        if (data.SuccessInsert === true) {
            AppUtil.ShowSuccess("Saved Successfully.");
            if (data.Item) {
                table.draw();
                //window.location.reload();
                //var Item = (data.Item);
                //$("#tblItem>tbody").append('<tr><td hidden><input type="hidden" id="" value=' + Item.ItemID + '></td><td>' + data.ItemFor + '</td><td>' + Item.ItemName + '</td><td><a href="" id="showItemForUpdate">Show</a></td></tr>');
            }
        }
        if (data.SuccessInsert === false) {

            if (data.AlreadyInsert = true) {
                AppUtil.ShowSuccess("Item Already Added. Choose different Item.");
            } else {
                AppUtil.ShowSuccess("Saved Failed.");
            }
        }
        ItemManager.clearForSaveInformation();
        $("#mdlItemInsert").modal('hide');
    },
    InsertBandwithResellerItemFromPopUpFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact bbb  Administrator.");
        console.log(data);
    },

    ShowBandwithResellerItemDetailsByIDForUpdate: function (ItemID) {


        //AppUtil.ShowWaitingDialog();
        //  setTimeout(function () {

        var url = "/Item/GetBandwithResellerItemDetailsByID/";
        var data = { ItemID: ItemID };
        data = ItemManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ItemManager.ShowBandwithResellerItemDetailsByIDForUpdateSuccess, ItemManager.ShowBandwithResellerItemDetailsByIDForUpdateError);

        //  }, 500);

    },
    ShowBandwithResellerItemDetailsByIDForUpdateSuccess: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);

        var ItemDetailsJsonParse = (data.ItemDetails);
        $("#ItemNames").val(ItemDetailsJsonParse.ItemName);
        //$("#ItemFors").val(ItemDetailsJsonParse.ItemFor);
        $("#mdlItemUpdate").modal("show");
    },
    ShowBandwithResellerItemDetailsByIDForUpdateError: function (data) {


        //AppUtil.HideWaitingDialog();
        console.log(data);
    },

    UpdateBandwithResellerItemInformation: function () {

        //AppUtil.ShowWaitingDialog();
        // var ItemID = ItemID;
        var ItemName = $("#ItemNames").val();
        //var ItemFor = $("#ItemFors").val();


        var url = "/Item/UpdateBandwithResellerItem";
        var ItemInfomation = ({ ItemID: ItemID, ItemName: ItemName });
        //var ItemInfomation = ({ ItemID: ItemID, ItemName: ItemName, ItemFor: ItemFor });
        var data = JSON.stringify({ ItemInfoForUpdate: ItemInfomation });
        AppUtil.MakeAjaxCall(url, "POST", data, ItemManager.UpdateBandwithResellerItemInformationSuccess, ItemManager.UpdateBandwithResellerItemInformationFail);
    },
    UpdateBandwithResellerItemInformationSuccess: function (data) {

        //AppUtil.HideWaitingDialog();

        if (data.UpdateSuccess === true) {
            var ItemInformation = (data.ItemUpdateInformation);

            $("#tblItem tbody>tr").each(function () {

                var ItemID = $(this).find("td:eq(0) input").val();
                var index = $(this).index();
                if (ItemInformation[0].ItemID == ItemID) {
                    //$('#tblItem tbody>tr:eq(' + index + ')').find("td:eq(1)").text(data.ItemFor);
                    $('#tblItem tbody>tr:eq(' + index + ')').find("td:eq(1)").text(ItemInformation[0].PackageName);

                }
            });
            AppUtil.ShowSuccess("Update Successfully. ");
        }
        if (data.UpdateSuccess === false) {
            if (AlreadyInsert = true) {
                AppUtil.ShowSuccess("Item Already Added. Choose different Item. ");
            }
        }

        ItemManager.clearForUpdateInformation();
        $("#mdlItemUpdate").modal('hide');
        console.log(data);
    },
    UpdateBandwithResellerItemInformationFail: function () {

        console.log(data);
        //AppUtil.HideWaitingDialog();
    },

    PrintItemList: function (status) {

        var url = "/Excel/CreateReportForItemList";


        var data = ({});
        data = ItemManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, ItemManager.PrintItemListSuccess, ItemManager.PrintItemListFail);
    },
    PrintItemListSuccess: function (data) {

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
    PrintItemListFail: function (data) {

        AppUtil.ShowSuccess("Error Occoured. Contact with Administrator.");
        console.log(data);
    },

    clearForSaveInformation: function () {
        $("#ItemName").val("");
        $("#ItemFor").val("");

    },
    clearForUpdateInformation: function () {
        $("#ItemNames").val("");
        $("#ItemFors").val("");
    }
}