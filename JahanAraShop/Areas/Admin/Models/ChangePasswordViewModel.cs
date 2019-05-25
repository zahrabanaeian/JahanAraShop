using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JahanAraShop.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
       

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "LimitCharacterRequired", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", ResourceType = typeof(Labels))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "LimitCharacterRequired", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Labels))]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Labels))]
        [Compare("NewPassword", ErrorMessageResourceName = "IsNotSame", ErrorMessageResourceType = typeof(ErrorMessages))]
        public string ConfirmPassword { get; set; }

    }
}