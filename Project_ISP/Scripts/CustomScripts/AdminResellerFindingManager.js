var AdminResellerFindingManager = {


    addRequestVerificationToken: function (data) {

        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    },

    ShowPackageDetailsByIDForUpdate: function (ResellerID) {
        var url = "/Reseller/GetMacResellerPackageAndZoneDetailsByResellerID/";
        var data = { resellerid: ResellerID };
        data = AdminResellerFindingManager.addRequestVerificationToken(data);
        AppUtil.MakeAjaxCallsForAntiForgery(url, "POST", data, AdminResellerFindingManager.ShowPackageDetailsByIDForUpdateSuccess, AdminResellerFindingManager.ShowPackageDetailsByIDForUpdateError);
    },
    ShowPackageDetailsByIDForUpdateSuccess: function (data) {
        var zoneList = (data.resellerzone);
        var packageList = (data.resellerpackage);
        var boxList = (data.resellerbox);
        var mikrotikList = (data.resellermikrotik);

        $("#PackageID,#PackageThisMonth,#PackageNextMonth,#ZoneID,#BoxID,#lstMikrotik").find("option").not(":first").remove(); 

        $.each(zoneList, function (index, element) {
            $("#SearchByZoneID,#ZoneID").append($("<option></option>").val(element.zoneid).text(element.zonename));
        });
        $.each(packageList, function (index, element) {
            $("#PackageID,#PackageThisMonth,#PackageNextMonth").append($("<option></option>").val(element.packageid).text(element.packagename));
        });
        $.each(boxList, function (index, element) {
            $("#BoxID").append($("<option></option>").val(element.boxid).text(element.boxname));
        });
        $.each(mikrotikList, function (index, element) {
            $("#lstMikrotik").append($("<option></option>").val(element.mikid).text(element.mikname));
        });
    },
    ShowPackageDetailsByIDForUpdateError: function (data) {
        console.log(data);
    },
}