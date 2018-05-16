namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblSiteState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTbl_State { get; set; }

        public byte dplyTbl_State { get; set; }

        [StringLength(2)]
        public string StateCode { get; set; }

        [StringLength(100)]
        public string StateName { get; set; }

        [StringLength(100)]
        public string StateEnglishName { get; set; }
    }
}
