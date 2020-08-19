var PurchaseManager = {

    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    SavePurchaseValidation: function () {
        var status = true;
        if (AppUtil.GetIdValue("subject") === '') {
            AppUtil.ShowErrorOnControl("Please Add Purchase Subject", "subject", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("PublishStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Select Publish Status.", "PublishStatus", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("ddlSupplierID") === '') {
            AppUtil.ShowErrorOnControl("Please Select Supplier.", "ddlSupplierID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("InvoiceID") === '') {
            AppUtil.ShowErrorOnControl("Please Add Invoice No.", "InvoiceID", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("IssuedAt") === '') {
            AppUtil.ShowErrorOnControl("Please Add Issue Date.", "IssuedAt", "top center");
            status = false;
        }
        if (AppUtil.GetIdValue("ddlPurchaseStatus") === '') {
            AppUtil.ShowErrorOnControl("Please Select Purchase Status.", "ddlPurchaseStatus", "top center");
            status = false;
        }

        var rowsCount = $("#tblPurchaseItems>tbody>tr").length;
        if (rowsCount < 1) {
            status = false;
            AppUtil.ShowErrorOnControl("Please Add Item For Purchase", "tblPurchaseItems", "top center");
        }

        $("#tblPurchaseItems>tbody>tr").each(function () {
            var index = $(this).index();
            var ItemName = $(this).closest("tr").find("td:eq(2) textarea[name='ItemDescription']").val();
            if (ItemName == "") {
                AppUtil.ShowErrorOnControl("Please Add Item ", "tblPurchaseItems", "top center");
                return false;
            }

            var Warrenty = $(this).closest("tr").find("td:eq(3) input[name='chkHasWarrentyOrNot']").is(':checked');
            if (Warrenty) {
                var dtpWarrentyStart = $(this).closest("tr").find("td:eq(4) input[name='dtpWarrentyStart']").val();
                if (dtpWarrentyStart == "") {
                    AppUtil.ShowErrorOnControl("Please Add Warrenty Start Date ", "tblPurchaseItems", "top center");
                    return false;
                }

                var dtpWarrentyEnd = $(this).closest("tr").find("td:eq(5) input[name='dtpWarrentyEnd']").val();
                if (dtpWarrentyEnd == "") {
                    AppUtil.ShowErrorOnControl("Please Add Warrenty End Date ", "tblPurchaseItems", "top center");
                    return false;
                }
            }
            var
                serial = $(this).closest("tr").find("td:eq(6) input[name='serial']").val();
            if (serial == "") {
                AppUtil.ShowErrorOnControl("Please Add Serial Number. ", "tblPurchaseItems", "top center");
                return false;
            }

            var amount = $(this).closest("tr").find("td:eq(7) input[name='amount']").val();
            if (amount == "") {
                AppUtil.ShowErrorOnControl("Please Add Purchase Amount. ", "tblPurchaseItems", "top center");
                return false;
            }
        });

        if (status == false) {
            return false;
        }
        else {
            return true;
        }
    },

    ShowItemListForPurchase: function () {
        var url = "/Purchase/GetItemList/";
        var data = {};
        data = PurchaseManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PurchaseManager.ShowItemListForPurchaseSuccess, PurchaseManager.ShowItemListForPurchaseError);
    },
    ShowItemListForPurchaseSuccess: function (data) {
        var ItemDetails = (data.ItemList);
        $(ItemDetails).each(function (i, e) {
            $("#tblItemsTable>tbody").append('<tr><td><input type="checkbox" class="si"></td> <td>' + e.ItemID + '</td> <td>' + e.ItemName + '</td> <td><input type="text" name="Quantity" value="" placeholder="Quantity" /></td><td class="price"><input type="text" name="Price" value="" placeholder="Price" /></td></tr>');
        })
        $("#mdlItemDetails").modal("show");
    },
    ShowItemListForPurchaseError: function (data) {
        console.log(data);
    },

    SavePurchase: function () {
        var url = "/Purchase/SavePurchase/";

        var discountType = $("input[name='rdoDiscount']:checked").val();
        //purchase
        var PurchaseID = $("#purchase_PurchaseID").val();
        var Subject = $("#subject").val();
        var SupplierID = $("#ddlSupplierID").val();
        var PublishStatus = $("#PublishStatus").val();
        var InvoicePrefix = $("#txtInvoicePrefix").val();
        var InvoiceID = $("#InvoiceID").val();
        var IssuedAt = $("#IssuedAt").val();
        var SupplierNoted = $("#SupplierNotes").val();
        var rdoInputType = $("input[type='radio'][name='rdoDiscount']").val();
        var DiscountType = rdoInputType == "percent" ? 1 : 2;
        var Discount = $("#txtDiscountAmount").val();
        var PurchaseStatus = $("#ddlPurchaseStatus").val();

        var purchase = { PurchaseID: PurchaseID, Subject: Subject, SupplierID: SupplierID, PublishStatus: PublishStatus, InvoicePrefix: InvoicePrefix, InvoiceID: InvoiceID, IssuedAt: IssuedAt, SupplierNoted: SupplierNoted, DiscountType: DiscountType, Discount: Discount, PurchaseStatus: PurchaseStatus }
        var purchasedeatils = [];
        // purchase Details Items
        if ($("#tblPurchaseItems>tbody>tr").length > 0) {
            $("#tblPurchaseItems>tbody>tr").each(function () {
                var row = $(this);

                //var Quantity = ;
                //var Price = ;
                var HasWarrenty = false;
                var WarrentyStart = null;
                var WarrentyEnd = null;


                var PurchaseDeatilsID = row.find('td:eq(0) input').val();
                var ItemID = row.find('td:eq(1) input').val();
                var Quantity = 1;
                var HasWarrenty = row.find('td:eq(3) input[name="chkHasWarrentyOrNot"]').is(':checked');
                if (HasWarrenty) {
                    WarrentyStart = row.find('td:eq(4) input[name="dtpWarrentyStart"]').val();
                    WarrentyEnd = row.find('td:eq(5) input[name="dtpWarrentyEnd"]').val();;
                }
                var Serial = row.find('td:eq(6) input').val();
                var Price = row.find('td:eq(7) input').val();

                purchasedeatils.push({
                    "PurchaseDeatilsID": PurchaseDeatilsID,
                    "ItemID": ItemID,
                    "Quantity": Quantity,
                    "HasWarrenty": HasWarrenty,
                    "WarrentyStart": WarrentyStart,
                    "WarrentyEnd": WarrentyEnd,
                    "Price": Price,
                    "Serial": Serial
                });
            });

            if (purchasedeatils.length == 0) {
                AppUtil.ShowError("Please Add Purchase Item First.");
                return false;
            }

        } else {
            AppUtil.ShowError("Please Add Purchase Item.");
        }

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = JSON.stringify({ purchase: purchase, purchasedeatils: purchasedeatils });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, PurchaseManager.SavePurchaseSuccess, PurchaseManager.SavePurchaseError);
    },
    SavePurchaseSuccess: function (data) {
        if (data.Success === false) {
            AppUtil.ShowError(data.Message);
        }
        if (data.Success === true) {
            AppUtil.ShowSuccess(data.Message);

            PurchaseManager.ClearInformationAfterPurchase();

            window.location.href = "/purchase/purchasepayment?pid=" + $("#purchase_PurchaseID").val() + "";
        }
    },
    SavePurchaseError: function (data) {
        AppUtil.ShowSuccess("Something Wrong. Please Contact With Administrator.");
    },

    AddItemInListForNonWarrentyProduct: function (serialNumber) {

        var ItemID = AppUtil.GetIdValue("ItemID");
        var ItemText = $("#ItemID option:selected").text();;
        var SupplierID = AppUtil.GetIdValue("SupplierID");
        var SupplierText = $("#SupplierID option:selected").text();;
        var BarndID = AppUtil.GetIdValue("BrandID");
        var BarndText = $("#BrandID option:selected").text();
        var Serial = serialNumber;
        var SupplierInvoice = AppUtil.GetIdValue("txtSupplierInvoice");
        var WarrentyProduct = false;

        $("#tblStock>tbody").append("<tr><td style='padding:0px;'><input type='hidden' value=" + ItemID + "></td><td>" + ItemText + "</td><td>" + Serial + "</td>\
                                         <td style='padding:0px;'><input type='hidden' value=" + BarndID + "></td><td>" + BarndText + "</td>\
                                         <td style='padding:0px;'><input type='hidden' value=" + SupplierID + "></td><td>" + SupplierText + "</td><td>" + SupplierInvoice + "</td>\
                                         <td align='center'><button id='btnDelete' type='button' class='btn btn-danger btn-sm padding' data-toggle='confirmation' data-placement='top'><span class='glyphicon glyphicon-remove'></span></button></td>\
            </tr>");
    },

    GetPurchaseDuePaymentDetails: function () {
        var url = "/Purchase/GetPurchaseDuePaymentDetails/";

        var id = $("#pid").val();

        var data = JSON.stringify({ id: id });

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, PurchaseManager.GetPurchaseDuePaymentDetailsSuccess, PurchaseManager.GetPurchaseDuePaymentDetailsError);
    },
    GetPurchaseDuePaymentDetailsSuccess: function (data) {
        if (data.success === true) {
            $("#spnShowTotalAmount").html(data.paymentamount.SubTotalAmount);
            $("#spnShowTotalDueAmount").html(data.paymentamount.DueAmount);
            $("#txtPayee").val(data.paymentamount.Payee);
            $("#mdlPurchasePayment").modal("show");
        }
    },
    GetPurchaseDuePaymentDetailsError: function (data) {
        AppUtil.ShowSuccess("Something Wrong. Please Contact With Administrator.");
    },

    InsertPurchasePayment: function () {
        var url = "/Purchase/SavePurchasePayment/";

        var PurchaseID = $("#pid").val();
        var AccountListID = $("#ddlAccount").val();
        var PurchasePaymentDate = $("#dtpPaynentDate").val();
        var Description = $("#txtDescription").val();
        var PaymentAmount = $("#txtAmount").val();
        var PaymentByID = $("#lstPaymentMethod").val();
        var CheckNo = $("#txtCheckNo").val();

        var purchasePayment = {
            PurchaseID: PurchaseID, AccountListID: AccountListID, PurchasePaymentDate: PurchasePaymentDate, Description: Description
            , PaymentAmount: PaymentAmount, PaymentByID: PaymentByID, CheckNo: CheckNo
        }

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var header = {};
        header['__RequestVerificationToken'] = AntiForgeryToken;

        var data = JSON.stringify({ pph: purchasePayment });
        AppUtil.MakeAjaxCallJSONAntifergeryNotFormCollection(url, "POST", data, header, PurchaseManager.InsertPurchasePaymentSuccess, PurchaseManager.InsertPurchasePaymentError);
    },
    InsertPurchasePaymentSuccess: function (data) {
        if (data.Success === false) {
            AppUtil.ShowError(data.Message);
        }
        if (data.Success === true) {
            AppUtil.ShowSuccess(data.Message);
            $("#spnDueTotal").html(data.DueAmount);
            PurchaseManager.ClearPurchasePaymentInfo();
            if (data.PaymentStatus == "Paid") {
                $("#divPaymentStatus").html(data.PaymentStatus);
                $("#spnMakePayment").remove();
                $("#mdlPurchasePayment").modal("hide");
                //$("#mdlPurchasePayment").remove();
            }
            else {
                $("#mdlPurchasePayment").modal("hide");
            }
        }
    },
    InsertPurchasePaymentError: function (data) {
        AppUtil.ShowSuccess("Something Wrong. Please Contact With Administrator.");
    },

    DeletePurchasePaymentHistoryByID: function (pphid) {

        var url = "/Purchase/DeletePurchasePaymentHistoryByID/";

        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var data = ({ id: pphid });
        data = PurchaseManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PurchaseManager.DeletePurchasePaymentHistoryByIDSuccess, PurchaseManager.DeletePurchasePaymentHistoryByIDFail);
        // }, 50);
    },
    DeletePurchasePaymentHistoryByIDSuccess: function (data) {

        if (data.DeleteStatus === true) {
            AppUtil.ShowSuccess("Payment removed successfully.");
            //tableRsellerPaymentHistory.draw();
            $("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(5)").html(data.PaymentStatus);
            $("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(8)").html(data.DeleteBy);
            $("#tblPurchasePaymentHistory>tbody>tr:eq(" + _rowIndexPaymentHistory + ")").find("td:eq(9)").html(data.DeleteDate);
            //$("#tblPurchaseList>tbody>tr:eq(" + _rowIndexPurchase + ")").find("td:eq()").text(data.CurrentBalance);
            table.draw();
            $("#popModalForDeletePermently").modal("hide");
            $("#mdlViewResellerPaymentHistory").modal("hide");
            //$("#tblPurchasePaymentHistory>tbody>tr").remove();
        }

        if (data.DeleteStatus === false) {
            AppUtil.ShowError("Payment removed failed.");
        }
    },
    DeletePurchasePaymentHistoryByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    DeletePurchaseByID: function (pid) {
        var url = "/Purchase/DeletePurchaseByID/";
        var AntiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var data = ({ id: pid });
        data = PurchaseManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, PurchaseManager.DeletePurchaseByIDSuccess, PurchaseManager.DeletePurchaseByIDFail);
        // }, 50);
    },
    DeletePurchaseByIDSuccess: function (data) {
        if (data.DeleteStatus === true) {
            AppUtil.ShowSuccess("Purchase removed successfully.");
            $("#popModalForDeletePerchasePermently").modal("hide");
            table.draw();
        }
        if (data.DeleteStatus === false) {
            AppUtil.ShowError("Purchase removed failed.");
        }
    },
    DeletePurchaseByIDFail: function (data) {
        AppUtil.ShowSuccess("Error Occoured. Contact With Administrator.");
        console.log(data);
    },

    ClearInformationAfterPurchase: function () {
        $("#tblPurchaseItems>tbody>tr").remove();
        $("#subject").val("");
        $("#ddlSupplierID").val("");
        $("#PublishStatus").val("");
        $("#txtInvoicePrefix").val("PA-");
        $("#address").val("");
        $("#InvoiceID").val("");
        $("#IssuedAt").val("");
        $("#ddlPurchaseStatus").val("");
        $("#SupplierNotes").val("");
        $("input[type='checkbox'][name='rdoDiscount'][value='Fixed']").prop("checked", true);
        $("#txtDiscountAmount").val("");
        $("#btnItemRemove").css("display", "none");
        $("#divInvoiceSerial").html("");
        $("#sub_total").html("0.00");
        $("#discount_amount_total").html("0.00");
        $("#total").html("0.00");
    },
    ClearPurchasePaymentInfo: function () {
        $("#ddlAccount").val("");
        $("#ddlAccount").val("");
        $("#dtpPaynentDate").val("");
        $("#txtDescription").val("");
        $("#txtAmount").val("");
        $("#lstPaymentMethod").val("");
        $("#txtCheckNo").val("");
        $("#spnShowTotalAmount").html("");
        $("#spnShowTotalDueAmount").html("");
    }
}