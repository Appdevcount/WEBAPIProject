using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBAPIProject.DBEntity_Layer
{
    public  class CustomerMetadata
    {
        [Key]// Added by me as the Entity class is missing to add primary key attribute
        
        [Display(Name ="CustomerID" )]
        public long CustID { get; set; }
        [Display(Name = "Name")]
        [Required,StringLength(maximumLength:20,MinimumLength =2,ErrorMessage = "Name length not as per expected standard length")]
        public string Name { get; set; }
        [Display(Name = "EmailID")]
        public string Email { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "MobileNo")]
        [RegularExpression(@"^9\d{9}$",ErrorMessage ="Mobile Number should start with 9 and 10 digits in total")]
        public string Contact { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Tran Date")]
        public DateTime? TranDate { get; set; }

    }
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {

    }
}