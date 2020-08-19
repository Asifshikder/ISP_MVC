using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("f")]
    public class BookViewModel
    {
        //[Column("A")]
        public long Id { get; set; }
        //[Column("AB")]
        public string Title { get; set; }
        //[Column("AC")]
        public bool IsWritten { get; set; }

    }
}