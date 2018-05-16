namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSiteInformation")]
    public partial class tblSiteInformation
    {
        [Key]
        public int IdtblInformation { get; set; }

        public int? IdtblInformationType { get; set; }

        [StringLength(100)]
        public string SmallPicture { get; set; }

        [Required]
        [StringLength(100)]
        public string LargePicture { get; set; }

        [StringLength(200)]
        public string FarsiTitle { get; set; }

        [StringLength(500)]
        public string FarsiSmallDescription { get; set; }

        public string FarsiDescription { get; set; }

        [StringLength(10)]
        public string FarsiCreateDate { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(200)]
        public string EnglishTitle { get; set; }

        [StringLength(500)]
        public string EnglishSmallDescription { get; set; }

        public string EnglishDescription { get; set; }

        [StringLength(200)]
        public string ArabicTitle { get; set; }

        [StringLength(500)]
        public string ArabicSmallDescription { get; set; }

        public string ArabicDescription { get; set; }

        [StringLength(100)]
        public string PictureEn { get; set; }
    }
}
