using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Domain.DomainModel
{
    [Table("vwGoodGroupSite")]
    public partial class vwGoodGroupSite
    {
        public long? Row { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FirstID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string FirstName { get; set; }

        public int? FirstParentID { get; set; }

        public int? SecondID { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        public int? SecondParentID { get; set; }

        public int? ThirdID { get; set; }

        [StringLength(50)]
        public string ThirdName { get; set; }

        public int? ThirdParentID { get; set; }
    }
}
