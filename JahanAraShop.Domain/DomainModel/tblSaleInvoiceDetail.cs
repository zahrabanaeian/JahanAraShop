namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSaleInvoiceDetail")]
    public partial class tblSaleInvoiceDetail
    {
        public double? Qty { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(13)]
        public string BarCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Description { get; set; }

        public short? DiscountTypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountPercent { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountValue { get; set; }

        [Column(TypeName = "money")]
        public decimal RetailPrice { get; set; }

        public short? RowNumber { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ScaleID { get; set; }

        public double? InitialQty { get; set; }

        public int? InitialScaleID { get; set; }

        public int? ChangeReasonSequence { get; set; }

        public bool? IsEchantillon { get; set; }

        [StringLength(13)]
        public string ServicedBarCode { get; set; }

        public int? PricingMethodID { get; set; }

        public virtual tblGood tblGood { get; set; }

        public virtual tblSaleInvoice tblSaleInvoice { get; set; }
    }
}
