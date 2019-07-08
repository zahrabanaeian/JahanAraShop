﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{
    public class CartViewModel
    {
        public IList<CartItems> Items { get; set; }
    }
    public class CartItems
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }

        public int GoodID { get; set; }


        public int GoodAttach { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal Qauntity { get; set; }



    }
}