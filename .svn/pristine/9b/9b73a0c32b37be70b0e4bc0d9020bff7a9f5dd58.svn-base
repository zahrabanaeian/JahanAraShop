using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace JahanAraShop.Areas.Admin.Models
{
    public class DicountCodeViewModel
    {
        public tblSiteDiscountCode SiteDiscountCode { get; set; }

        [Display(Name = "CustomerName", ResourceType = typeof(Labels))]
        public System.Web.Mvc.SelectListItem[] CustomerList { get; set; }
        

    }

    public class DisCodlist
    {

        public string expiredate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountValue { get; set; }
        public int orgid { get; set; }
        public string orgname { get; set; }
        public string discription { get; set; }
    }
   
}