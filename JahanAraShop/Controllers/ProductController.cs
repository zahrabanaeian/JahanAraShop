using JahanAraShop.Data.Context;
using JahanAraShop.Helper;
using JahanAraShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class ProductController : Controller
    {
        [ActionName("Products")]
        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult Categories(int page,int size,int GroupID)
        {
           
            var model = new GoodViewModel();
            model.CategoriesGoods = new List<Goods>();
            using (var db = new DataBaseContext())
            {
                var CategoriesGood = (from good in db.tblGoods
                            join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                            where good.TabletVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                            && good.GoodGroupID==GroupID 
                            select new
                            {
                                good.ID,
                                good.Name,
                                good.BarCode,
                                good.GoodGroupID,
                                good.Code,
                                goodsale.RetailPrice,
                                good.Notes,
                                good.TypeID,
                                good.SiteVisible,
                                good.TabletVisible,
                                good.CreateDate,
                                good.FarsiCreateDate,
                                good.Sequence,
                            }).ToList();

                foreach (var item in CategoriesGood)
                {
                    model.CategoriesGoods.Add(new Goods
                    {
                        ID = item.ID,
                        BarCode = item.BarCode,
                        GoodGroupID = item.GoodGroupID,
                        Code = item.Code,
                        Name = item.Name,
                        RetailPrice = item.RetailPrice,
                        Notes = item.Notes,
                        TypeID = item.TypeID,
                        SiteVisible = item.SiteVisible,
                        TabletVisible = item.TabletVisible,
                        Sequence = item.Sequence,
                    });
                }
                var PagedData= Pagination.PagedResult(model.CategoriesGoods, 1, 2);
              //  return Json(PagedData, JsonRequestBehavior.AllowGet);
                  return View(model);

            }


        }
    }
}