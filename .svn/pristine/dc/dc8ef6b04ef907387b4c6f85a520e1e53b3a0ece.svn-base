using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JahanAraShop.Areas.Admin.Models
{
    public class InvoiceReportModel
    {

        [Display(Name = "FromDate", ResourceType = typeof(Labels))]
        public string FromDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Labels))]
        public string ToDate { get; set; }
        [Display(Name = "CellPhone", ResourceType = typeof(Labels))]
        [RegularExpression(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "WrongCellPhoneEntered")]
        public string CustomerPhone { get; set; }
    }
}