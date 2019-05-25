using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Domain.DomainModel
{
    [Table("tblAccountBranch")]
    public partial class tblAccountBranch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAccountBranch()
        {
            tblAccountBranch1 = new HashSet<tblAccountBranch>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(1000)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Name2 { get; set; }

        [StringLength(1000)]
        public string Name3 { get; set; }

        public int? ParentID { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? MinID { get; set; }

        public int? MaxID { get; set; }

        [StringLength(250)]
        public string LinkedServerName { get; set; }

        [StringLength(250)]
        public string DBName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(250)]
        public string Phone1 { get; set; }

        [StringLength(250)]
        public string CodePerfix { get; set; }

        [StringLength(250)]
        public string Phone2 { get; set; }

        [StringLength(250)]
        public string WebSite { get; set; }

        [StringLength(250)]
        public string Fax { get; set; }

        public int? IncomingSalePercent { get; set; }

        public decimal? longitude { get; set; }

        public decimal? latitude { get; set; }

        [StringLength(100)]
        public string ImageThumb { get; set; }

        [StringLength(100)]
        public string ImageBig { get; set; }

        public int? BusinessPartnerSalePercent { get; set; }

        public bool? TabletVisible { get; set; }

        public int? Sequence { get; set; }

        [StringLength(250)]
        public string EnAddress { get; set; }

        public string BeaconUUID { get; set; }

        public int? BeaconMajor { get; set; }

        public int? BeaconMinor { get; set; }

        public int? TabletRadius { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAccountBranch> tblAccountBranch1 { get; set; }

        public virtual tblAccountBranch tblAccountBranch2 { get; set; }
    }
}
