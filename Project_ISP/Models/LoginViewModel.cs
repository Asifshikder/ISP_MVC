using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("16")]
    public class LoginViewModel
    {

            [Required]
            [Display(Name = "User name")]
            //[Column("A")]
        public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            //[Column("B")]
        public string Password { get; set; }

            [Display(Name = "Remember me?")]
            //[Column("C")]
        public bool RememberMe { get; set; }
        
    }
}