namespace JahanAraShop.Domain.DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGoodGroup")]
    public partial class tblGoodGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblGoodGroup()
        {
            tblGoodGroup1 = new HashSet<tblGoodGroup>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        public int? ParentID { get; set; }

        public bool? TabletVisible { get; set; }

        public int? Sequence { get; set; }

        [StringLength(250)]
        public string Name3 { get; set; }

        [StringLength(250)]
        public string Name2 { get; set; }

        public bool? TableVisible { get; set; }

        public bool? SiteVisible { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblGoodGroup> tblGoodGroup1 { get; set; }

        public virtual tblGoodGroup tblGoodGroup2 { get; set; }
    }
}
