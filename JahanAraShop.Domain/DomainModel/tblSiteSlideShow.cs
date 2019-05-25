using JahanAraShop.Resourecs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JahanAraShop.Domain.DomainModel
{
    [Table("tblSiteSlideShow")]
    public class TblSiteSlideShow
    {
        [Key]
        public int IdtblSlideShow { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        [Display(Name = "Priority", ResourceType = typeof(Labels))]
        public int? Priority { get; set; }

        [StringLength(500)]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public string Picture { get; set; }

        [StringLength(200)]
        [Display(Name = "FarsiTitle", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public string FarsiTitle { get; set; }

        [StringLength(200)]
        public string EnglishTitle { get; set; }
        [StringLength(200)]
        public string ArabicTitle { get; set; }
        public string Title
        {
            get
            {
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    return EnglishTitle;
                if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
                    return FarsiTitle;
                if (Thread.CurrentThread.CurrentCulture.Name == "ar-SA")
                    return ArabicTitle;
                return "";
            }
        }

        [StringLength(500)]
         [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public string FarsiDescription { get; set; }

        [StringLength(500)]
        public string EnglishDescription { get; set; }

        [StringLength(500)]
        public string ArabicDescription { get; set; }
     
        [Display(Name = "Description", ResourceType = typeof(Labels))]
        public string Description
        {
            get
            {
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    return EnglishDescription;
                if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR")
                    return FarsiDescription;
                if (Thread.CurrentThread.CurrentCulture.Name == "ar-SA")
                    return ArabicDescription;
                return "";
            }
        }
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Required")]
        public bool? IsEnabled { get; set; }

        [StringLength(500)]
        public string PictureEn { get; set; }
    }
}
