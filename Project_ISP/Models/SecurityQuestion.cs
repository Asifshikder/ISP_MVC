using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("31")]
    public class SecurityQuestion
    {
        [Key]
        //[Column("AQWQWQ")]
        public int SecurityQuestionID { get; set; }
        //[Column("ARR")]
        public string SecurityQuestionName { get; set; }
        //  public string SecurityQuestionAnswer { get; set; }
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