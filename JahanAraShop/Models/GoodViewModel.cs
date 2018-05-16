using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{

    public class GoodViewModel
    {
        public List<Goods> NewestGoods { get; set; }
        public List<Goods> BestSellingGoods { get; set; }
        public List<Goods> CategoriesGoods { get; set; }
    }
    public class Goods
    {

        public int ID { get; set; }
        public string BarCode { get; set; }
        public int? GoodGroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? RetailPrice { get; set; }
        public string Notes { get; set; }
        public int? TypeID { get; set; }  
        public bool? SiteVisible { get; set; }
        public bool? TabletVisible { get; set; }
        public int? Sequence { get; set; }
        public DateTime? CreateDate { get; set; }
        public string FarsiCreateDate { get; set; }

       
    }

  
}