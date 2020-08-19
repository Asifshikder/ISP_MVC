using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("34")]
    public class SMS
    {
        //[Column("AQSD")]
        public int SMSID { get; set; }
        //[Column("AWWWWWWWW")]
        public string SMSTitle { get; set; }
        //[Column("QQWQA")]
        public string SendMessageText { get; set; }
        //[Column("AWWW")]
        public string SMSCode { get; set; }
        //[Column("AFG")]
        public string Sender { get; set; }
        //[Column("DDA")]
        public int SMSStatus { get; set; }
        //[Column("DA")]
        public int SMSCounter { get; set; }
        //[Column("AR")]
        public int CreateBy { get; set; }
        //[Column("AE")]
        public  DateTime ? CreateDate { get; set; }
        //[Column("AW")]
        public int UpdateBy { get; set; }
        //[Column("AQ")]
        public DateTime ? UpdateDate { get; set; }
    }
}