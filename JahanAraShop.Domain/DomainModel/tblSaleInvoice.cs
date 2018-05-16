namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSaleInvoice")]
    public partial class tblSaleInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSaleInvoice()
        {
            tblSaleInvoiceDetails = new HashSet<tblSaleInvoiceDetail>();
        }

        public int ID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(10)]
        public string FarsiCreateDate { get; set; }

        public short DiscountTypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountPercent { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountValue { get; set; }

        [Column(TypeName = "money")]
        public decimal? SuchargeValue { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalValue { get; set; }

        public int? Code { get; set; }

        public int? CreatorID { get; set; }

        public int? CustomerID { get; set; }

        [StringLength(200)]
        public string CustomerMessage { get; set; }

        public int? SaleOrderID { get; set; }

        [Column(TypeName = "money")]
        public decimal PureTotalValue { get; set; }

        public int? PromoterID { get; set; }

        public bool? IsPaidByCash { get; set; }

        public int? InventoryID { get; set; }

        public int? ContractID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ClearDate { get; set; }

        [StringLength(10)]
        public string FarsiClearDate { get; set; }

        [StringLength(50)]
        public string ManualCode { get; set; }

        public int? PricingMethodID { get; set; }

        public int? DistributionInvoiceID { get; set; }

        [Column(TypeName = "money")]
        public decimal? ShortageValue { get; set; }

        public int? TypeID { get; set; }

        public bool? IsFromDispatch { get; set; }

        public int? AchieverID { get; set; }

        public int? DistributerID { get; set; }

        public DateTime? DeliverDate { get; set; }

        [StringLength(10)]
        public string FarsiDeliverDate { get; set; }

        public int? PreConditionID { get; set; }

        public int? CreditDays { get; set; }

        [Column(TypeName = "money")]
        public decimal? RemCredit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UsedCredit { get; set; }

        public int? ObjectID1 { get; set; }

        public int? ObjectID2 { get; set; }

        public int? BranchID { get; set; }

        public bool? ShouldSMSSend { get; set; }

        public int? CreatorEmployeeID { get; set; }

        public int? UpdaterID { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(10)]
        public string FarsiUpdateDate { get; set; }

        public int? PrintCount { get; set; }

        public int? PromoterID2 { get; set; }

        public long? CardNo { get; set; }

        public int? LotteryPrintCount { get; set; }

        public int? LastLotteryPrintUserID { get; set; }

        public DateTime? LastLotteryPrintDate { get; set; }

        [StringLength(10)]
        public string FarsiLastLotteryPrintDate { get; set; }

        public int? DeliverTypeID { get; set; }

        public bool? IsFormal { get; set; }

        public bool? IsCheckEchantillon { get; set; }

        public int? IntroductionTypeID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSaleInvoiceDetail> tblSaleInvoiceDetails { get; set; }
    }
}
