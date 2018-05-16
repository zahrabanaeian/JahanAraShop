namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSaleDeliverType")]
    public partial class tblSaleDeliverType
    {
        public int id { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(250)]
        public string SubTitle { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }
    }
}
