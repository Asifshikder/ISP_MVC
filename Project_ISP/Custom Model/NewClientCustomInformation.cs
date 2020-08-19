using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //<th hidden = "hidden" ></ th >
    //< th > Name </ th >
    //< th > Zone </ th >
    //< th > Address </ th >
    //< th > Contact </ th >
    //< th > Package </ th >
    //< th > Assigned To</th>
    //<th> Survey</th>
    public class NewClientCustomInformation
    {
        public int ClientDetailsID { get; set; }
        public string Name { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }
        public string LatitudeLongitude { get; set; }
        public string ContactNumber { get; set; }
        public string Package { get; set; }
        public string AssignedTo { get; set; }
        public string Survey { get; set; }
        public DateTime time { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public bool ShowNewClient { get; set; }
        public bool SignUp { get; set; }
        public bool DeleteButton { get; set; }
    }
}