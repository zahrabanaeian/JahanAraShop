using JahanAraShop.Data.Context;
using JahanAraShop.Domain.Constants;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Helper;
using JahanAraShop.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class ProductController : Controller
    {

        private static string _basketCookie = AppConstants.Basket;
        private static string _basketCountCookie = AppConstants.BasketCount;
        [ActionName("Products")]
        public virtual ActionResult Index()
        {
            return View();
        }

        private class TempGroup
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private class TempBarcodeList
        {
            public string Barcode { get; set; }

        }
        public virtual ActionResult Categories(int groupid)
        {


            try
            {
                ViewBag.GroupID = groupid;
                using (var db = new DataBaseContext())
                {
                    Stack<TempGroup> GoodGroupList = new Stack<TempGroup>();
                    tblGoodGroup GoodGroupParentParent = new tblGoodGroup();


                    var GooodGroupName = db.tblGoodGroups.Where(x => x.ID == groupid).Select(x => x.Name).FirstOrDefault();
                    var GoodGroup = db.tblGoodGroups.Where(x => x.ID == groupid).FirstOrDefault();
                    var GoodGroupParent = db.tblGoodGroups.Where(x => x.ID == GoodGroup.ParentID).FirstOrDefault();
                    if (GoodGroupParent != null)
                    {

                        GoodGroupParentParent = db.tblGoodGroups.Where(x => x.ID == GoodGroupParent.ParentID).FirstOrDefault();

                    }


                    GoodGroupList.Push(new TempGroup()
                    {
                        Name = GoodGroup.Name,
                        Value = GoodGroup.ID,
                    });
                    if (GoodGroupParent != null)
                    {
                        GoodGroupList.Push(new TempGroup()
                        {
                            Name = GoodGroupParent.Name,
                            Value = GoodGroupParent.ID,
                        });

                        if (GoodGroupParentParent != null)
                        {
                            GoodGroupList.Push(new TempGroup()
                            {
                                Name = GoodGroupParentParent.Name,
                                Value = GoodGroupParentParent.ID,
                            });
                        }
                    }

                    var tempList = new SelectListItem[GoodGroupList.Count];
                    var i = 0;
                    foreach (var t in GoodGroupList)
                    {
                        tempList[i] = new SelectListItem
                        {
                            Value = t.Value.ToString(),
                            Text = t.Name.ToString()
                        };
                        i++;
                    }
                    ViewBag.GoodGroup = tempList;
                    ViewBag.Name = GooodGroupName;

                }

                return View();
            }
            catch (Exception error)
            {
                const string title = "صفحه محصولات";
                TempData["Message"] = error.Message + "ارتباط با سرور قطع می باشد لطفا مجددا تلاش کنید. ";
                var description = (string)TempData["Message"];
                TempData = new TempDataDictionary() {
                            {"title", title},
                            {"description", description},
                            { "currentpage", "/Shop"},
                            { "type", MessageViewModel.MessageTypes.Error}
                        };
             
                return RedirectToAction(MVC.Message.Index());
            }


        }

        public virtual IEnumerable<Goods> Sorting(int pageNumber, int pageSize, int GroupID, List<string> ChekedBrands, decimal? MinPrice, decimal MaxPrice, string Sort)
        {
            using (var db = new DataBaseContext())
            {
                List<int> SelectBrand = new List<int>();
                var model = new GoodViewModel();
                model.CategoriesGoods = new List<Goods>();
                List<int> Grouplist = db.tblGoodGroups.Where(x => x.ParentID == GroupID || x.ID == GroupID).Select(x => x.ID).ToList();
                List<int> GrouplistParent = db.tblGoodGroups.Where(x => Grouplist.Contains(x.ParentID ?? 0)).Select(x => x.ID).ToList();
                List<int> GrouplistAll = Grouplist.Union(GrouplistParent).ToList();
                if (ChekedBrands != null)
                {
                    SelectBrand = ChekedBrands.Select(int.Parse).ToList();
                    if (Sort == "Newest")
                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.FarsiCreateDate descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                  && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.FarsiCreateDate descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        return model.CategoriesGoods;
                    }
                    if (Sort == "Cheapest")
                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.RetailPrice ascending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,

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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {


                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.RetailPrice ascending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }


                        }
                        return model.CategoriesGoods;

                    }

                    else

                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.RetailPrice descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }

                        else

                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                  && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  && SelectBrand.Contains(good.TypeID ?? 0)
                                                  orderby good.RetailPrice descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        return model.CategoriesGoods;

                    }
                }
                //////
                else
                {

                    if (Sort == "Newest")
                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                  && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice


                                                  orderby good.FarsiCreateDate descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {

                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                  && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  orderby good.FarsiCreateDate descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }


                        }
                        return model.CategoriesGoods;
                    }
                    if (Sort == "Cheapest")
                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                  && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice

                                                  orderby good.RetailPrice ascending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {


                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  orderby good.RetailPrice ascending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }



                        }
                        return model.CategoriesGoods;

                    }
                    else

                    {
                        if (MinPrice != 0 || MaxPrice != 0)
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice

                                                  orderby good.RetailPrice descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {
                            var CategoriesGood = (from good in db.tblGoods
                                                  join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                                  where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                                   && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                                  orderby good.RetailPrice descending
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
                                                      goodsale.DiscountPercent,
                                                      goodsale.DiscountValue,
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }


                        }
                        return model.CategoriesGoods;

                    }

                }
            }
        }

        public virtual ActionResult GetPagingCategories(int pageNumber, int pageSize, int GroupID, List<string> ChekedBrands, decimal? MinPrice, decimal MaxPrice, string Sort)
        {
            List<int> SelectBrand = new List<int>();
            var model = new GoodViewModel();
            model.CategoriesGoods = new List<Goods>();
            IEnumerable<Goods> SortingTemp = new List<Goods>();

            using (var db = new DataBaseContext())
            {
                List<int> Grouplist = db.tblGoodGroups.Where(x => x.ParentID == GroupID || x.ID == GroupID).Select(x => x.ID).ToList();
                List<int> GrouplistParent = db.tblGoodGroups.Where(x => Grouplist.Contains(x.ParentID ?? 0)).Select(x => x.ID).ToList();
                List<int> GrouplistAll = Grouplist.Union(GrouplistParent).ToList();
                //filter Price
                if (MinPrice != 0 || MaxPrice != 0)
                {
                    if (ChekedBrands != null)
                    {
                        SelectBrand = ChekedBrands.Select(int.Parse).ToList();
                        if (Sort != "")
                        {
                            SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                        }
                        var CategoriesGood = (from good in db.tblGoods
                                              join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                              where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                              && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                              && SelectBrand.Contains(good.TypeID ?? 0)
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
                                                  goodsale.DiscountPercent,
                                                  goodsale.DiscountValue,
                                              }).ToList();

                        if (SortingTemp.Count() == 0)
                        {
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {

                            model.CategoriesGoods = SortingTemp.ToList();

                        }

                        var Goodtypes = (from good in db.tblGoods
                                         join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                         join type in db.tblGoodTypes on good.TypeID equals type.ID
                                         where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                          && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                         orderby type.ID
                                         select type

                                       ).Distinct().ToList();

                        var Minprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                         && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                        && SelectBrand.Contains(good.TypeID ?? 0)
                                        select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                        var Maxprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                         && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                        && SelectBrand.Contains(good.TypeID ?? 0)
                                        select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                        var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                        return Json(PagedData, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {

                        if (Sort != "" || Sort != null)
                        {

                            SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                        }


                        var CategoriesGood = (from good in db.tblGoods
                                              join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                              where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                               && GrouplistAll.Contains(good.GoodGroupID ?? 0) && goodsale.RetailPrice >= MinPrice && goodsale.RetailPrice <= MaxPrice

                                              orderby good.FarsiCreateDate descending
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
                                                  goodsale.DiscountPercent,
                                                  goodsale.DiscountValue,
                                              }).ToList();


                        if (SortingTemp.Count() == 0)
                        {
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
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountValue = item.DiscountValue,
                                });
                            }
                        }
                        else
                        {

                            model.CategoriesGoods = SortingTemp.ToList();

                        }


                        var Goodtypes = (from good in db.tblGoods
                                         join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                         join type in db.tblGoodTypes on good.TypeID equals type.ID
                                         where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                          && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                         orderby type.ID
                                         select type

                                   ).Distinct().ToList();

                        var Minprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                         && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                        select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                        var Maxprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                         && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                        select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                        var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                        return Json(PagedData, JsonRequestBehavior.AllowGet);
                    }


                }


                if (ChekedBrands != null)
                {
                    if (Sort != "")
                    {

                        SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                    }
                    SelectBrand = ChekedBrands.Select(int.Parse).ToList();

                    var CategoriesGood = (from good in db.tblGoods
                                          join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                          where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                           && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                         && SelectBrand.Contains(good.TypeID ?? 0)
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
                                              goodsale.DiscountPercent,
                                              goodsale.DiscountValue,
                                          }).ToList();

                    if (SortingTemp.Count() == 0)
                    {
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
                                DiscountPercent = item.DiscountPercent,
                                DiscountValue = item.DiscountValue,
                            });
                        }
                    }
                    else
                    {

                        model.CategoriesGoods = SortingTemp.ToList();

                    }

                    var Goodtypes = (from good in db.tblGoods
                                     join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                     join type in db.tblGoodTypes on good.TypeID equals type.ID
                                     where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                      && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                     orderby type.ID
                                     select type

                                   ).Distinct().ToList();

                    var Minprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                    && SelectBrand.Contains(good.TypeID ?? 0)
                                    select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                    var Maxprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                    && SelectBrand.Contains(good.TypeID ?? 0)
                                    select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                    var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                    return Json(PagedData, JsonRequestBehavior.AllowGet);
                }

                else
                {


                    if (Sort != "")
                    {

                        SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                    }

                    var CategoriesGood = (from good in db.tblGoods
                                          join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                          where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                           && GrouplistAll.Contains(good.GoodGroupID ?? 0)
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
                                              goodsale.DiscountPercent,
                                              goodsale.DiscountValue,
                                          }).ToList();

                    if (SortingTemp.Count() == 0)
                    {
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
                                DiscountPercent = item.DiscountPercent,
                                DiscountValue = item.DiscountValue,
                            });
                        }
                    }
                    else
                    {

                        model.CategoriesGoods = SortingTemp.ToList();

                    }

                    var Goodtypes = (from good in db.tblGoods
                                     join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                     join type in db.tblGoodTypes on good.TypeID equals type.ID
                                     where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                     orderby type.ID
                                     select type

                               ).Distinct().ToList();

                    var Minprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                    && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                    select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                    var Maxprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && GrouplistAll.Contains(good.GoodGroupID ?? 0)
                                    select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                    var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                    return Json(PagedData, JsonRequestBehavior.AllowGet);

                }

            }


        }

        public virtual async System.Threading.Tasks.Task<ActionResult> Product(int GoodID)
        {


            using (var db = new DataBaseContext())
            {

                //fill site map
                Stack<TempGroup> GoodGroupList = new Stack<TempGroup>();
                tblGoodGroup GoodGroupParentParent = new tblGoodGroup();
                var GroupID = db.tblGoods.Where(x => x.ID == GoodID).Select(x => x.GoodGroupID).SingleOrDefault();
                var GroupName = db.tblGoods.Where(x => x.ID == GoodID).Select(x => x.Name).SingleOrDefault();

                var GooodGroupName = db.tblGoodGroups.Where(x => x.ID == GroupID).Select(x => x.Name).FirstOrDefault();
                var GoodGroup = db.tblGoodGroups.Where(x => x.ID == GroupID).FirstOrDefault();
                var GoodGroupParent = db.tblGoodGroups.Where(x => x.ID == GoodGroup.ParentID).FirstOrDefault();
                if (GoodGroupParent != null)
                {

                    GoodGroupParentParent = db.tblGoodGroups.Where(x => x.ID == GoodGroupParent.ParentID).FirstOrDefault();

                }


                GoodGroupList.Push(new TempGroup()
                {
                    Name = GoodGroup.Name,
                    Value = GoodGroup.ID,
                });
                if (GoodGroupParent != null)
                {
                    GoodGroupList.Push(new TempGroup()
                    {
                        Name = GoodGroupParent.Name,
                        Value = GoodGroupParent.ID,
                    });

                    if (GoodGroupParentParent != null)
                    {
                        GoodGroupList.Push(new TempGroup()
                        {
                            Name = GoodGroupParentParent.Name,
                            Value = GoodGroupParentParent.ID,
                        });
                    }
                }

                var tempList = new SelectListItem[GoodGroupList.Count];
                var i = 0;
                foreach (var t in GoodGroupList)
                {
                    tempList[i] = new SelectListItem
                    {
                        Value = t.Value.ToString(),
                        Text = t.Name.ToString()
                    };
                    i++;
                }
                ViewBag.GoodGroup = tempList;
                ViewBag.Name = GroupName;


                var model = new GoodViewModel();
                model.Good = new List<Goods>();


                using (var client = new HttpClient())
                {
                    var product = (from good in db.tblGoods
                                   join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                   where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                   where good.ID == GoodID
                                   select new
                                   {
                                       good.ID,
                                       good.Name,
                                       good.Name3,
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
                                       goodsale.DiscountPercent,
                                       goodsale.DiscountValue,
                                   }).SingleOrDefault();

                    //دریافت موجودی
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    dynamic Sales = new ExpandoObject();
                    var jsonGetGoodGoal = JsonConvert.SerializeObject(Sales);
                    var content = new StringContent(jsonGetGoodGoal, Encoding.UTF8, AppConstants.JsonType);
                    var url = AppConstants.InventoryQuantity + "/" + AppConstants.ApiKey + "/" + product.BarCode;
                    var result = await client.GetAsync(url);
                    var response = result.Content.ReadAsStringAsync().Result;
                    var res = new SpInventoryQuantityWebService();
                    if (response != "")
                    {
                        res = JsonConvert.DeserializeObject<SpInventoryQuantityWebService>(response);
                    }
                    else
                    {

                        res.Remained = 0;
                    }
                    model.Good.Add(new Goods
                    {
                        ID = product.ID,
                        BarCode = product.BarCode,
                        GoodGroupID = product.GoodGroupID,
                        Code = product.Code,
                        Name = product.Name,
                        Name3 = product.Name3,
                        RetailPrice = product.RetailPrice,
                        Notes = product.Notes,
                        TypeID = product.TypeID,
                        SiteVisible = product.SiteVisible,
                        TabletVisible = product.TabletVisible,
                        Sequence = product.Sequence,
                        DiscountPercent = product.DiscountPercent,
                        DiscountValue = product.DiscountValue,
                        HasRemiand = (res.Remained != 0) ? true : false,
                    });

                }
                model.ReviewsList = db.TblSiteReview.Where(x => x.GoodID == GoodID && x.IsVisible == true).ToList();
                return View(model);
            }



        }

        public virtual ActionResult ShowCartItems()
        {


            if (Request.Cookies.AllKeys.Contains(_basketCountCookie) && int.Parse(Request.Cookies[_basketCountCookie].Value) > 0)
            {
                using (var db = new DataBaseContext())
                {
                    var model = new CartViewModel();
                    model.Items = new List<CartItems>();
                    var st = Request.Cookies[_basketCookie].Value;
                    var selectedGoods = st.Replace("%2C", ",").Replace("%22", "").Split(new char[3] { ',', ']', '[' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (var sg in selectedGoods)
                    {
                        var item = sg.Split(':');
                        var Barcode = item[0];

                        var Goods = (from good in db.tblGoods
                                     join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                     where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && good.BarCode == Barcode
                                     select new
                                     {
                                         Barcode = good.BarCode,
                                         Name = good.Name,
                                         RetailPrice = goodsale.RetailPrice,
                                         DiscountValue = goodsale.DiscountValue

                                     }).SingleOrDefault();
                        if (Goods != null)
                        {
                            model.Items.Add(new CartItems
                            {
                                Name = Goods.Name,
                                Price = Goods.RetailPrice - Goods.DiscountValue,
                                Barcode = Goods.Barcode,
                                Count = int.Parse(item[1])


                            });
                        }
                    }
                    var result = model.Items;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }


        }
        public virtual JsonResult AddWishList(int GoodID)
        {

            //if user is not login,first log in
            if (!Request.IsAuthenticated)
            {
                return Json(new
                {
                    redirectUrl = Url.Action(MVC.Account.Login(GoodID.ToString())),
                    isRedirect = true

                });

            }

            else
            {


                using (var db = new DataBaseContext())
                {
                    int exitg = 0;
                    var userId = User.Identity.GetUserId();
                    tblSiteWishList wish = new tblSiteWishList();
                    var wishlist = db.tblSiteWishLists.Where(current => current.SiteUser_Id == userId).ToList();
                    //if wishlist of user zero
                    if (wishlist.Count == 0)
                    {
                        wish.Good_Id = GoodID;
                        wish.SiteUser_Id = userId;
                        db.tblSiteWishLists.Add(wish);
                        db.SaveChanges();
                        return Json(new { result = true });

                    }
                    else
                        foreach (var item in wishlist)
                        {


                            if (item.Good_Id == GoodID) { exitg = GoodID; break; }

                        }

                    //if product does not exit in wishlist
                    if (exitg != GoodID)
                    {
                        wish.Good_Id = GoodID;
                        wish.SiteUser_Id = userId;
                        db.tblSiteWishLists.Add(wish);
                        db.SaveChanges();
                        return Json(new { result = true });
                    }

                    //if exit in wishlist
                    else
                    {
                        return Json(new { result = false });
                    }

                }

            }

        }

        //getWish
        public virtual JsonResult GetWishList(int GoodId)
        {

            //if user is not login,first log in
            if (Request.IsAuthenticated)
            {
                using (var db = new DataBaseContext())
                {
                    var userId = User.Identity.GetUserId();
                    tblSiteWishList wish = new tblSiteWishList();
                    var wishlist = db.tblSiteWishLists.Where(current => current.SiteUser_Id == userId && current.Good_Id == GoodId).ToList();
                    return Json(wishlist);

                }

            }
            else
            {

                return Json(false);
            }

        }



        [HttpPost]
        public virtual ActionResult AddReview(GoodViewModel model)
        {

            using (var db = new DataBaseContext())
            {
                var userId = User.Identity.GetUserId();
                var review = new tblSiteReview
                {
                    CreateDate = DateTime.Now,
                    FarsiCreateDate = AppConstants.GetPersianDate(),
                    GoodID = model.Reviews.GoodID,
                    Description = model.Reviews.Description,
                    Name = model.Reviews.Name,
                    SiteUser_Id = userId,
                    Rating = model.Reviews.Rating


                };
                db.TblSiteReview.Add(review);
                db.SaveChanges();

            }

            return RedirectToAction(MVC.Product.Product(model.Reviews.GoodID));

        }





    }
}