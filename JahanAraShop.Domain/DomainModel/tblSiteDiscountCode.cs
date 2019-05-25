using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Domain.DomainModel
{
    [Table("tblSiteDiscountCode")]
    public class tblSiteDiscountCode
    {
        public int ID { get; set; }

        public int OrganizationID { get; set; }


        [Display(Name = "DiscountCode", ResourceType = typeof(Labels))]
        [StringLength(50)]
        public string DiscountCode { get; set; }

        public DateTime? CreateDate { get; set; }

     
        [Display(Name = "FarsiCreateDate", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(10)]
        public string FarsiCreateDate { get; set; }
        public DateTime ExpireDate { get; set; }

        
        [Display(Name = "FarsiExpiredate", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [StringLength(10)]
        public string FarsiExpiredate { get; set; }
        public bool IsUsed { get; set; }

        [Display(Name = "DiscountPercent", ResourceType = typeof(Labels))]
        [Column(TypeName = "money")]
        public decimal? DiscountPercent { get; set; }

        [Display(Name = "DiscountValue", ResourceType = typeof(Labels))]
        [Column(TypeName = "money")]
        public decimal? DiscountValue { get; set; }

        
        [Display(Name = "Descriptions", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }
    }
}
