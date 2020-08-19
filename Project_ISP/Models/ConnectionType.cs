using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("y")]
    public class ConnectionType
    {
        [Key]
        //[Column("AG")]
        public int ConnectionTypeID { get; set; }
        //[Column("AR")]
        public string ConnectionTypeName { get; set; }
        //[Column("AE")]
        public string CreatedBy { get; set; }
        //[Column("AW")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AQ")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<ClientDetails> ClientDetails { get; set; }
    }
}