var CableManager = {
    addRequestVerificationToken: function (data) {
        
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    ShowCableDetailsByCableTypeIDForDiv: function (CableTypeID) {
        
        //AppUtil.ShowWaitingDialog();
        //setTimeout(function () {
        
        var url = "/ProductCurrentStatus/GetCableDetailsByCableTypeID/";
        var data = { CableTypeID: CableTypeID };
        data = CableManager.addRequestVerificationToken(data);

        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, CableManager.ShowCableDetailsByCableTypeIDForDivSuccess, CableManager.ShowCableDetailsByCableTypeIDForDivError);

        //}, 500);

    },
    ShowCableDetailsByCableTypeIDForDivSuccess: function (data) {
        
       //  { "targets": [1], "data": "", "render": function (data, type, row, meta) { return '<a href="#" onclick="GetCableDetailsByDrumOrBox(' + row.CableTypeID + ')">' + row. + '</a>' } },

                $.each(data.lstCableByCableTypeID, function (index, item) {//<input type='hidden' value='" + cableStockID + "'>
                $("#tblCableList>tbody").append("<tr><td hidden><input type='hidden' value=" + item.CableStockID + "></td><td>" + item.CableTypeName + "</td>\
<td><a href='#' onclick='GetCableDistributionDetailsByDrumOrBox(" + item.CableStockID + ")' >" + item.BoxDrumName + "</a></td><td>" + item.ReadingFrom + "</td><td>" + item.ReadingEnd + "</td><td>" + item.Quantity + "</td>\
<td>" + item.Used + "</td><td>" + item.Remain + "</td><td>" + item.BrandName + "</td><td>" + item.SupplierName + "</td><td>" + item.Invoice  + "</td></tr>");
            // alert(item);
        });
        if (data.lstCableByCableTypeID.length > 0) {
            $("#divCableList").show();
        }
    },
    ShowCableDetailsByCableTypeIDForDivError: function (data) {

        
        console.log(data);
        $("#divCableList").hide();
    },

    
}