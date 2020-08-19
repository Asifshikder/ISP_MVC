using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("b")]
    public class ActionNameAuthentication
    {
        //[Column("A")]
        public string ActionNameID { get; set; }
        //[Column("Aa")]
        public string ActionName { get; set; }
        //[Column("As")]
        public bool IsGranted { get; set; }
        //[Column("Ad")]
        public string id { get; set; }
        //[Column("Ae")]
        public string text { get; set; }
        //[Column("Aw")]
        public bool @checked { get; set; }
    }
}