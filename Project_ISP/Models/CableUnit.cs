using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using ISP_ManagementSystemModel.Models;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("l")]
    public class CableUnit
    {
        [Key]
        //[Column("A")]
        public int CableUnitID { get; set; }
        //[Column("As")]
        public string CableUnitName { get; set; }
        //[Column("Ad")]
        public string CreatedBy { get; set; }
        //[Column("Af")]
        public DateTime? CreatedDate { get; set; }
        //[Column("Ag")]
        public string UpdateBy { get; set; }
        //[Column("Ah")]
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<ClientDetails> ClientDetails { get; set; }
    }
}