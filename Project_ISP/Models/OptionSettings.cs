using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("20")]
    public class OptionSettings
    {
        [Key]
        //[Column("AOD")]
        public int OptionSettingsID { get; set; }
        //[Column("AO")]
        public string OptionSettingsName { get; set; }
        //[Column("AS")]
        public int Status { get; set; }
    }
}