using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JahanAraShop.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "CorrectField", ErrorMessageResourceType = typeof(ErrorMessages))]
        [Display(Name = "UserName", ResourceType = typeof(Labels))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Labels))]
        public string Password { get; set; }
    }
}