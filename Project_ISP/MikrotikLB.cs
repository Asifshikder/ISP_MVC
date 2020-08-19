using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;
using Project_ISP.ViewModel.CustomClass;
using tik4net;
using static ISP_ManagementSystemModel.Controllers.MikrotikUserController;

namespace ISP_ManagementSystemModel
{
    internal static class MikrotikLB
    {
        public const string MikrotikService = "pppoe";
        private static ISPContext db = new ISPContext();
        static ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api);
        internal static ITikConnection CreateInstanceOfMikrotik(TikConnectionType Api, string RealIP, int Port, string MikUserName, string MikPassword)
        {
            connection = ConnectionFactory.OpenConnection(TikConnectionType.Api, RealIP, Port, MikUserName, MikPassword);
            return connection;
        }
        internal static void CreateUserInMikrotikDuringUpdate(ITikConnection connection, ClientDetails clientDetails, ClientLineStatus clientLineStatus)
        {
            connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(clientLineStatus.PackageID).PackageName.Trim()/*,"disabled", "yes"*/).ExecuteNonQuery();
        }
        internal static void CreateUserInMikrotikDuringUpdate(ITikConnection connectionForGivenByClientMK, ClientDetails ClientClientDetails, ClientLineStatus ClientClientLineStatus, bool? chkStatusFromRunningMonth, Transaction checkForExistingTransaction)
        {
            //here i am searching for the status
            int status = 0;
            if (chkStatusFromRunningMonth == true)
            {
                status = ClientClientLineStatus.LineStatusID;
            }
            else if (checkForExistingTransaction != null)
            {
                status = checkForExistingTransaction.LineStatusID.Value;
            }
            else
            {
                status = db.ClientLineStatus.Where(s => s.ClientDetailsID == ClientClientDetails.ClientDetailsID).OrderByDescending(s => s.LineStatusChangeDate).FirstOrDefault().LineStatusID;
            }
            //string statusForMKT = (status == 0) ? 
            connection.CreateCommandAndParameters("/ppp/secret/add", "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(ClientClientLineStatus.PackageID).PackageName.Trim(), "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();

        }
        internal static void CreateUserInMikrotikWithPackageAndStatus(ITikConnection connectionForGivenByClientMK, ClientDetails ClientClientDetails, int packageID, int status)
        {
            connection.CreateCommandAndParameters("/ppp/secret/add", "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(packageID).PackageName.Trim(), "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        }
        internal static void UpdateUserInMikrotikWithPackageAndStatus(ITikConnection connectionForGivenByClientMK, ClientDetails OldClientDetails, ClientDetails ClientClientDetails, int packageID, int status)
        {
            string prf = db.Package.Find(packageID).PackageName.Trim();
            connection.CreateCommandAndParameters("/ppp/secret/set", ".id", OldClientDetails.LoginName, "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", MikrotikService, "profile", prf, "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        }

        //internal static void CreateUserInMikrotikWithPackageAndStatusOnlyForPaymentDuePackage(ITikConnection connectionForGivenByClientMK, ClientDetails ClientClientDetails, int packageID, int status)
        //{
        //    connection.CreateCommandAndParameters("/ppp/secret/add", "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(packageID).PackageName.Trim(), "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        //}
        //internal static void UpdateUserInMikrotikWithPackageAndStatusOnlyForPaymentDuePackage(ITikConnection connectionForGivenByClientMK, ClientDetails OldClientDetails, ClientDetails ClientClientDetails, int packageID, int status)
        //{
        //    string prf = db.Package.Find(packageID).PackageName.Trim();
        //    connection.CreateCommandAndParameters("/ppp/secret/set", ".id", OldClientDetails.LoginName, "name", ClientClientDetails.LoginName, "password", ClientClientDetails.Password, "service", MikrotikService, "profile", prf, "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        //}

        internal static void CreateUserInMikrotikDuringCreate(ITikConnection connection, ClientDetails clientDetails)
        {
            connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(clientDetails.PackageID).PackageName.Trim()).ExecuteNonQuery();
        }
        internal static void CreateUserInMikrotikByPackageID(ITikConnection connection, ClientDetails clientDetails, int packageID)
        {
            connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(packageID).PackageName.Trim()).ExecuteNonQuery();
        }
        //internal static void CreateUserInMikrotikByPackageAndStatus(ITikConnection connection, ClientDetails clientDetails, int packageID, int status)
        //{
        //    connection.CreateCommandAndParameters("/ppp/secret/add", "name", clientDetails.LoginName, "password", clientDetails.Password, "service", MikrotikService, "profile", db.Package.Find(packageID).PackageName.Trim(), "disabled", status == 5 ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        //}
        internal static void UpdateUserInMikrotikWithPackageInformation(ITikConnection connection, ClientDetails oldClientDetails, ClientDetails newClientDetails, ClientLineStatus clientLineStatus)
        {
            connection.CreateCommandAndParameters("/ppp/secret/set", ".id", oldClientDetails.LoginName, "name", newClientDetails.LoginName, "password", newClientDetails.Password/*, "service", MikrotikService*/, "profile", db.Package.Find(clientLineStatus.PackageID).PackageName.Trim()).ExecuteNonQuery();
        }
        internal static void UpdateUserInMikrotikWithPackageAndStatusInformation(ITikConnection connection, ClientDetails oldClientDetails, ClientDetails newClientDetails, ClientLineStatus ClientClientLineStatus, bool? chkStatusFromRunningMonth, bool? chkPackageFromRunningMonth)
        {
            connection.CreateCommandAndParameters("/ppp/secret/set", ".id", oldClientDetails.LoginName, "name", newClientDetails.LoginName, "password", newClientDetails.Password/*, "service", MikrotikService*/, "profile", db.Package.Find(ClientClientLineStatus.PackageID).PackageName.Trim(), "disabled", ClientClientLineStatus.LineStatusID == AppUtils.LineIsLock ? AppUtils.MakeUserDisabledInMikrotik : AppUtils.MakeUserEnableInMikrotik).ExecuteNonQuery();
        }
        internal static void UpdateUserInMikrotikWithOutPackageInformation(ITikConnection connection, ClientDetails oldClientDetails, ClientDetails newClientDetails)
        {
            connection.CreateCommandAndParameters("/ppp/secret/set", ".id", oldClientDetails.LoginName, "name", newClientDetails.LoginName, "password", newClientDetails.Password/*, "service", MikrotikService*/).ExecuteNonQuery();
        }
        internal static void RemoveUserInMikrotik(ITikConnection connection, ClientDetails clientDetails)
        {
            connection.CreateCommandAndParameters("/ppp/secret/remove", ".id", clientDetails.LoginName).ExecuteNonQuery();

        }
        internal static void RemoveUserInMikrotik(ITikConnection connection, string UserName)
        {
            connection.CreateCommandAndParameters("/ppp/secret/remove", ".id", UserName).ExecuteNonQuery();

        }
        internal static /*ITikReSentence*/ int CountNumbeOfUserInMikrotik(ITikConnection connection, ClientDetails clientDetails)
        {
            var countLoginName = connection.CreateCommandAndParameters("/ppp/secret/print", "name", clientDetails.LoginName).ExecuteList();
            return countLoginName.Count() > 0 ? countLoginName.Count() : 0;
        }

        internal static void SetStatusOfUserInMikrotik(ITikConnection connection, string ClientLoginName, string status)
        {
            connection.CreateCommandAndParameters("/ppp/secret/set", "disabled", status.Trim(), TikSpecialProperties.Id, ClientLoginName).ExecuteNonQuery();
        }

        internal static void UpdateMikrotikUserBySingleSingleData(ITikConnection connection, string LoginName, string password, int PackageID)
        {
            connection.CreateCommandAndParameters("/ppp/secret/set", ".id", LoginName, "name", LoginName, "password", password,/*"service", MikrotikService,*/  "profile", db.Package.Find(PackageID).PackageName.Trim()).ExecuteNonQuery();

        }


        internal static void RemoveUserInActiveConenction(ITikConnection connection, ClientDetails clientDetails)
        {
            //            $arrID =$API->comm("/ppp/active/getall",
            //                  array(
            //                 ".proplist"=> ".id",
            //                 "?name" => $user,

            //                 ));

            //$API->comm("/ppp/active/remove",
            //        array(
            //            ".id" => $arrID[0][".id"],
            //            )
            //        );


            /// interface pppoe-server remove [find user=ali]
            //connection.CreateCommandAndParameters("/ppp/active/remove[id=" + clientDetails.LoginName + "]").ExecuteNonQuery();
            var a = connection.CreateCommandAndParameters("/ppp/active/getall", "?name", clientDetails.LoginName).ExecuteList();
            var aa = a.FirstOrDefault().Words[".id"]/*.Replace("*", "")*/;
            connection.CreateCommandAndParameters("/ppp/active/remove", ".id", aa).ExecuteNonQuery();
        }
        internal static void RemoveUserInActiveConenctionByLoginName(ITikConnection connection, string loginName)
        {
            //            $arrID =$API->comm("/ppp/active/getall",
            //                  array(
            //                 ".proplist"=> ".id",
            //                 "?name" => $user,

            //                 ));

            //$API->comm("/ppp/active/remove",
            //        array(
            //            ".id" => $arrID[0][".id"],
            //            )
            //        );


            /// interface pppoe-server remove [find user=ali]
            //connection.CreateCommandAndParameters("/ppp/active/remove[id=" + clientDetails.LoginName + "]").ExecuteNonQuery();
            var a = connection.CreateCommandAndParameters("/ppp/active/getall", "?name", loginName).ExecuteList();
            var aa = a.FirstOrDefault().Words[".id"]/*.Replace("*", "")*/;
            connection.CreateCommandAndParameters("/ppp/active/remove", ".id", aa).ExecuteNonQuery();
        }
        internal static /*ITikReSentence*/ int CountNumbeOfUserInActive(ITikConnection connection, ClientDetails clientDetails)
        {
            var countLoginName = connection.CreateCommandAndParameters("/ppp/active/print", "name", clientDetails.LoginName).ExecuteList();
            return countLoginName.Count() > 0 ? countLoginName.Count() : 0;
        }

        internal static /*ITikReSentence*/ bool UserIDExistOrNotInMikrotik(ITikConnection connection, ClientDetails clientDetails)
        {
            var checkUserExistOrNot = connection.CreateCommandAndParameters("/ppp/secret/print", "name", clientDetails.LoginName).ExecuteList();
            return checkUserExistOrNot.Count() > 0 ? true : false;
        }

        internal static List<MikrotikUserList> GetUserListFromMikrotik(ITikConnection connection, Mikrotik mikrotik)
        {
            List<MikrotikUserList> lstMikrotikUserList = new List<MikrotikUserList>();
            ITikCommand userCmd = connection.CreateCommand("/ppp/secret/print");
            var lstUserFromMikrotik = userCmd.ExecuteList();
            foreach (var user in lstUserFromMikrotik)
            {
                var name = user.Words["name"];
                var password = user.Words["password"];
                var package = user.Words["profile"];
                var active = user.Words["disabled"];
                var profile = user.Words["profile"];
                lstMikrotikUserList.Add(new MikrotikUserList()
                {
                    UserName = name,
                    Password = password,
                    MikrotikID = mikrotik.MikrotikID,
                    MikrotikName = mikrotik.MikName,
                    ProfileName = profile,
                    active = active
                });
            }
            return lstMikrotikUserList;
        }
        internal static List<MikrotikUserList> GetActiveUserListFromMikrotik(ITikConnection connection, Mikrotik mikrotik)
        {
            List<MikrotikUserList> lstMikrotikUserList = new List<MikrotikUserList>();
            ITikCommand userCmd = connection.CreateCommand("/ppp/active/print");
            var lstUserFromMikrotik = userCmd.ExecuteList();
            foreach (var user in lstUserFromMikrotik)
            {
                var name = user.Words["name"];
                lstMikrotikUserList.Add(new MikrotikUserList() { UserName = name });
            }
            return lstMikrotikUserList;
        }

        internal static IEnumerable<ITikReSentence> GetPackageListFromMikrotik(ITikConnection connection)
        {

            ITikCommand PackageCmd = connection.CreateCommand("/ppp/profile/print");
            return PackageCmd.ExecuteList();
        }

        internal static ITikConnection CreateConnectionType(TikConnectionType api)
        {
            ITikConnection conn = ConnectionFactory.CreateConnection(api);
            return conn;
        }


    }
}