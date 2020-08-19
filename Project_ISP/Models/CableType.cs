using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("k")]
    public class CableType
    {
        [Key]
        //[Column("Aq")]
        public int CableTypeID { get; set; }
        //[Column("Aqq")]
        public string CableTypeName { get; set; }
        //[Column("Aqqqq")]
        public string CreatedBy { get; set; }
        //[Column("Aqqqqq")]
        public DateTime? CreatedDate { get; set; }
        //[Column("Aa")]
        public string UpdateBy { get; set; }
        //[Column("Asd")]
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<ClientDetails> ClientDetails { get; set; }
    }
}