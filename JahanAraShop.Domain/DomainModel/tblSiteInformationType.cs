namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSiteInformationType")]
    public partial class tblSiteInformationType
    {
        [Key]
        public int IdtblInformationType { get; set; }

        [StringLength(50)]
        public string FarsiTitle { get; set; }

        [StringLength(50)]
        public string EnglishTitle { get; set; }

        [StringLength(50)]
        public string ArabicTitle { get; set; }

        public bool? Visible { get; set; }
    }
}
