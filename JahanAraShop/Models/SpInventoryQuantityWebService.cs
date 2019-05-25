using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{
    public class SpInventoryQuantityWebService
    {
        public string Barcode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal ReceiptCount { get; set; }
        public decimal DispatchCount { get; set; }
        public decimal Remained { get; set; }
    }
}