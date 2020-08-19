using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("r")]
    public class ClientDetails
    {
        [Key]
        //[Column("QAZSDA")]
        public int ClientDetailsID { get; set; }

        //[Column("QA")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        //[Column("WA")]
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }

        //[Column("AE")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Column("AR")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Column("AT")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string LatitudeLongitude { get; set; }

        //[Column("AAA")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        //[Column("SSA")]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }
        //[Column("DDA")]
        [Display(Name = "Social Communication URL")]
        public string SocialCommunicationURL { get; set; }

        //[Column("DDDA")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        //[Column("ZA")]
        [Display(Name = "Box Number")]
        public string BoxNumber { get; set; }

        //[Column("AZ")]
        [Display(Name = "Pop Details")]
        public string PopDetails { get; set; }

        //[Column("ZZZA")]
        [Display(Name = "Require Cable")]
        public string RequireCable { get; set; }

        //[Column("AXXX")]
        [Display(Name = "Reference")]
        public string Reference { get; set; }

        //[Column("ASA")]
        [Display(Name = "National ID")]
        public string NationalID { get; set; }

        //[Column("ADA")]
        [Display(Name = "Connection Fees Provided Date")]
        //public float SignUpFee { get; set; } [Display(Name = "Box Number")]
        public DateTime? ConnectionFeesProvidedDate { get; set; }

        //[Column("QQQA")]
        [Display(Name = "Client Survey")]
        public string ClientSurvey { get; set; }

        //[Column("AWW")]
        [Display(Name = "Connection Date")]
        public DateTime? ConnectionDate { get; set; }

        //[Column("AQWQW")]
        [Display(Name = "Client Priority")]
        public int? ClientPriority { get; set; }

        //[Column("AH")]
        [Display(Name = "Mac Address")]
        public string MacAddress { get; set; }

        //[Column("AO")]
        [Display(Name = "SMS Communication")]
        public string SMSCommunication { get; set; }

        //[Column("AOO")]
        [Display(Name = "Is New Client?")]
        public int? IsNewClient { get; set; }

        //[Column("AP")]
        [Display(Name = "New Client Request Date")]
        public DateTime? NewClientRequestDate { get; set; }

        //[Column("APP")]
        [Display(Name = "New Client Approximate Connection Start Date")]
        public DateTime? NewClientApproximateConnectionStartDate { get; set; }

        //[Column("A")]
        [Display(Name = "Zone")]
        public int? ZoneID { get; set; }
        public virtual Zone Zone { get; set; }

        //[Column("APPLK")]
        [Display(Name = "Connection Type")]
        public int? ConnectionTypeID { get; set; }
        public virtual ConnectionType ConnectionType { get; set; }


        //[Column("AYTR")]
        [Display(Name = "Cable Type")]
        public int? CableTypeID { get; set; }
        public virtual CableType CableType { get; set; }

        //[Column("ARTY")]
        [Display(Name = "Package")]
        public int? PackageID { get; set; }
        public virtual Package Package { get; set; }

        //public int? ResellerPackageID { get; set; }
        //public virtual ResellerPackage ResellerPackage { get; set; }

        //[Column("AERER")]
        [Display(Name = "Security Question")]
        public int? SecurityQuestionID { get; set; }
        public virtual SecurityQuestion SecurityQuestion { get; set; }

        //[Column("ATRTR")]
        [Display(Name = "Security Question Answer")]
        public string SecurityQuestionAnswer { get; set; }

        //[Column("AGHGH")]
        [Display(Name = "Assign To")]
        public int? EmployeeID { get; set; }/// <summary>
                                            /// this is only for new client and the field is assign to which employee...:D
                                            /// </summary>
        public virtual Employee Employee { get; set; }

        //[Column("AHGHG")]
        public int? RoleID { get; set; }
        public virtual Role Role { get; set; }

        //[Column("AJH")]
        public int? UserRightPermissionID { get; set; }
        public virtual UserRightPermission UserRightPermission { get; set; }

        //[Column("AHJJ")]
        public int? MikrotikID { get; set; }
        public virtual Mikrotik Mikrotik { get; set; }

        //[Column("AKKKK")]
        public string IP { get; set; }

        //[Column("ALKLK")]
        public string Mac { get; set; }


        //[Column("AKLKL")]
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }

        //[Column("IsPriorityClient")]
        public bool IsPriorityClient { get; set; }

        //[Column("APPMN")]
        public string CreateBy { get; set; }

        //[Column("ACV")]
        public DateTime? CreateDate { get; set; }

        //[Column("AVVV")]
        public string UpdateBy { get; set; }

        //[Column("ACBV")]
        public DateTime? UpdateDate { get; set; }

        //[Display(Name = "Client Line Status")]
        //public int ?  ClientLineStatusID { get; set; } 
        //public virtual ClientLineStatus ClientLineStatus { get; set; } 

        //[Display(Name = "Client Banned Status")]
        //public int ?  ClientBannedStatusID { get; set; } 
        //public virtual ClientBannedStatus ClientBannedStatus { get; set; } 
        [Range(1, 31)]
        public int ApproxPaymentDate { get; set; }

        public double ProfileUpdatePercentage { get; set; }
        public int StatusThisMonth { get; set; }
        public int StatusNextMonth { get; set; }
        public DateTime LineStatusWillActiveInThisDate { get; set; }

        public byte[] ClientOwnImageBytes { get; set; }
        public string ClientOwnImageBytesPaths { get; set; }
        public byte[] ClientNIDImageBytes { get; set; }
        public string ClientNIDImageBytesPaths { get; set; }


        public int PackageThisMonth { get; set; }
        public int PackageNextMonth { get; set; }


        public DateTime? NextApproxPaymentFullDate { get; set; } //this the Client Payment Full Date
        //public DateTime NextGenerateDate { get; set; }    //this is the next bill generate time

        public string RunningCycle { get; set; }
        public int ClientLineStatusID { get; set; }
        public int Status { get; set; }
        public double PermanentDiscount { get; set; }
    } 
} 