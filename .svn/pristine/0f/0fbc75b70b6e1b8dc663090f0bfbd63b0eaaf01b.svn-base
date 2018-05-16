namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblSiteCounty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTbl_County { get; set; }

        public bool dsblTbl_County { get; set; }

        [StringLength(4)]
        public string CountyCode { get; set; }

        [StringLength(100)]
        public string CountyName { get; set; }

        public int? IdTbl_State { get; set; }

        [StringLength(100)]
        public string CountyEnglishName { get; set; }
    }
}
