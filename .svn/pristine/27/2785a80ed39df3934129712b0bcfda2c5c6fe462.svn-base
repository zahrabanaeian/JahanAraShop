namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGoodSaleInfo")]
    public partial class tblGoodSaleInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(13)]
        public string Barcode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PricingMethodID { get; set; }

        [Column(TypeName = "money")]
        public decimal? RetailPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountPercent { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountValue { get; set; }

        public bool? IsDiscountInPercent { get; set; }

        public int? LoanExpireDuration { get; set; }

        public int? ComGroupID { get; set; }

        public virtual tblGood tblGood { get; set; }
    }
}
