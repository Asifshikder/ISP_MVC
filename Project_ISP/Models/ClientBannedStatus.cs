using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("o")]
    public class ClientBannedStatus
    {
        [Key]
        //[Column("Ad")]
        public int ClientBannedStatusID { get; set; }
        //[Column("As")]
        public int ClientDetailsID { get; set; }

        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("Aaa")]
        public int BannedStatusID { get; set; }
        public virtual BannedStatus BannedStatus { get; set; }
        //[Column("A")]
        public DateTime BannedDate { get; set; }
    }
}