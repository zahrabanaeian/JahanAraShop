﻿using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Notes { get; set; }



        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "MobileEnter")]
        [Display(Name = "UserName", ResourceType = typeof(Labels))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Labels))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Labels))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [StringLength(250)]
        [Display(Name = "FirsName", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }


        [Key]
        [StringLength(500)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [Display(Name = "Name", ResourceType = typeof(Labels))]
        public string Name { get; set; }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "LimitCharacterRequired", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Labels))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Labels))]
        [Compare("Password", ErrorMessageResourceName = "IsNotSame", ErrorMessageResourceType = typeof(ErrorMessages))]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "MobileEnter", ErrorMessageResourceType = typeof(ErrorMessages))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Labels))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "JustNumber")]
        [StringLength(11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "LimitCharacterRequired")]
        public string PhoneNumber { get; set; }







    }

    public class ResetPasswordViewModel
    {


        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "MobileEnter", ErrorMessageResourceType = typeof(ErrorMessages))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Labels))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "JustNumber")]
        [StringLength(11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "LimitCharacterRequired")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "LimitCharacterRequired", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Labels))]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Labels))]
        [Compare("Password", ErrorMessageResourceName = "IsNotSame", ErrorMessageResourceType = typeof(ErrorMessages))]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [Display(Name = "AuthenticationCode", ResourceType = typeof(Labels))]
        [Range(1000, 9999, ErrorMessageResourceName = "WrongCode", ErrorMessageResourceType = typeof(ErrorMessages))]
        public string Code { get; set; }


        //public string Smscode { get; set; }



    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceName = "MobileEnter", ErrorMessageResourceType = typeof(ErrorMessages))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Labels))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "JustNumber")]
        [StringLength(11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "LimitCharacterRequired")]
        public string PhoneNumber { get; set; }
    }

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