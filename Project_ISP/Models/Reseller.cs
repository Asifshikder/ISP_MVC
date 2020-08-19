using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("28")]
    public class Reseller
    {
        [Key]
        //[Column("RI")]
        public int ResellerID { get; set; }
        //[Column("RN")]
        public string ResellerName { get; set; }
        //[Column("RLN")]
        public string ResellerLoginName { get; set; }
        //[Column("RBN")]
        public string ResellerBusinessName { get; set; }
        //[Column("RP")]
        public string ResellerPassword { get; set; }
        //[Column("RTLID")]
        public string ResellerTypeListID { get; set; }//cause there is a possibilities that a reseller has a mac and bandwidth client
        //[Column("RA")]
        public string ResellerAddress { get; set; }
        //[Column("RC")]
        public string ResellerContact { get; set; }
        //[Column("RBCL")]
        public string ResellerBillingCycleList { get; set; }
        //[Column("RS")]
        public int ResellerStatus { get; set; }

        //[Column("RL")]
        public byte[] ResellerLogo { get; set; }
        //[Column("RLP")]
        public string ResellerLogoPath { get; set; }
        //[NotMapped]
        //public virtual List<BandwithReselleItemGivenWithPriceList> OrderDetails { get; set; }
        [NotMapped]
        public virtual List<bandwithReselleGivenItemWithPriceModel> bandwithReselleGivenItemWithPriceModel { get; set; }
        //[Column("BRIGWP")]
        public string BandwithReselleItemGivenWithPrice { get; set; }
        [NotMapped]
        public virtual List<macReselleGivenPackageWithPriceModel> macResellerGivenPackagePriceModel { get; set; }
        //[Column("MRGPWP")]
        public string macReselleGivenPackageWithPrice { get; set; }
        public double ResellerBalance { get; set; }
        //[Column("RlID")]
        public int? RoleID { get; set; }
        public virtual Role Role { get; set; }
        //[Column("URPI")]
        public int? UserRightPermissionID { get; set; }
        public virtual UserRightPermission UserRightPermission { get; set; }
        public string MacResellerAssignMikrotik { get; set; }
        //[Column("CB")]
        public string CreatedBy { get; set; }
        //[Column("CD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("UB")]
        public string UpdateBy { get; set; }
        //[Column("UD")]
        public DateTime? UpdateDate { get; set; }
    }
}