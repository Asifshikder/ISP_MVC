using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class ProfilePercentageFields
    {
        [Key]
        public int ProfilePercentageFieldsID { get; set; }
        public string FieldsName { get; set; }
        public string TableName { get; set; }
        public string MappingField { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}