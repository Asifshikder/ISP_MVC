using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("10")]
    public class FormNameForAuth
    {
        //[Column("AF")]
        public string FormNameID { get; set; }
        //[Column("AE")]
        public string FormName { get; set; }
        //[Column("AD")]
        public bool IsGranted { get; set; }
        //[Column("AC")]
        public string id { get; set; }
        //[Column("AB")]
        public string text { get; set; }
        //[Column("A")]
        public bool @checked { get; set; }

        public virtual List<ActionNameAuthentication> children { get; set; }
    }
}