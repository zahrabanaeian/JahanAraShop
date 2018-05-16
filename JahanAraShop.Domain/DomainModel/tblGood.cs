namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGood")]
    public partial class tblGood
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblGood()
        {
            tblGoodSaleInfoes = new HashSet<tblGoodSaleInfo>();
            tblSaleInvoiceDetails = new HashSet<tblSaleInvoiceDetail>();
        }

        [Key]
        [StringLength(13)]
        public string BarCode { get; set; }

        public int? GoodGroupID { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Name2 { get; set; }

        [StringLength(150)]
        public string Name3 { get; set; }

        [Column(TypeName = "money")]
        public decimal? RetailPrice { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public int? SaleScaleID { get; set; }

        public int? PurchaseScaleID { get; set; }

        public int? TypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrencyRetailPrice { get; set; }

        public int? CurrencyTypeID { get; set; }

        public double? StandardDiscountPercent { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "money")]
        public decimal? StandardDiscountValue { get; set; }

        public int? LoanExpireDuration { get; set; }

        public int? EsTreshold { get; set; }

        [Column(TypeName = "money")]
        public decimal? StandardPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? CustomerPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? OldRetailPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? PurchaseRetailPrice { get; set; }

        public bool? SiteVisible { get; set; }

        public bool? TabletVisible { get; set; }

        public int? Sequence { get; set; }

        public bool? IsTraceable { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(10)]
        public string FarsiCreateDate { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblGoodSaleInfo> tblGoodSaleInfoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSaleInvoiceDetail> tblSaleInvoiceDetails { get; set; }
    }
}
