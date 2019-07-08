﻿using JahanAraShop.Data.Context;
using JahanAraShop.Domain.Constants;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Helper;
using JahanAraShop.Models;
using JahanAraShop.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class HomeController : Controller
    {

        // GET: Home
        public virtual async System.Threading.Tasks.Task<ActionResult> Index()
        {
            using (var db = new DataBaseContext())
            {

                //fill GoodViewModel For MainPage
                //براساس sequence  جدیدترین کالای دلخواه واسه سایت گذاشته میشه
                var model = new GoodViewModel();
                model.NewestGoods = new List<Goods>();
                model.BestSellingGoods = new List<Goods>();
                var NewGood = (from good in db.tblGoods
                               join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                               where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14 && good.Sequence!=null
                               //&& good.Name3!=null
                               orderby good.Sequence descending
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
                               }).Take(8).ToList();
                foreach (var item in NewGood)
                {
                    model.NewestGoods.Add(new Goods
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


                var SalesGood = (from good in db.tblGoods
                                 join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                 join detail in db.tblSaleInvoiceDetails on good.BarCode equals detail.BarCode
                                 where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                 //&& good.Name3 != null
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
                                 }).Count();

                //news
                model.TblSiteInformations = db.tblSiteInformations.OrderByDescending(x => x.FarsiCreateDate).Take(4).ToList();
                model.SlideShows = db.TblSiteSlideShow.Where(x=>x.IsEnabled==true).OrderBy(x=>x.Priority).ToList();
                var bestsale = db.Database.SqlQuery<spBestSaleGoods>("spBestSaleGoods").ToList<spBestSaleGoods>().Take(8);
                foreach (var item in bestsale)
                {
                    model.BestSellingGoods.Add(new Goods
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
                return View(model);
                //using (var client = new HttpClient())
                //{

                //    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                //    dynamic Sales = new ExpandoObject();
                //    var jsonGetGoodGoal = JsonConvert.SerializeObject(Sales);
                //    var content = new StringContent(jsonGetGoodGoal, Encoding.UTF8, AppConstants.JsonType);
                //    var url = AppConstants.BestSellingGoods + "/" + AppConstants.ApiKey;
                //    var result = await client.GetAsync(url);
                //    var response = result.Content.ReadAsStringAsync().Result;
                //    var res = JsonConvert.DeserializeObject<List<BestSellingGoods>>(response);

                //    foreach (var item in res)
                //    {
                //        model.BestSellingGoods.Add(new Goods
                //        {
                //            ID = item.ID,
                //            BarCode = item.BarCode,
                //            GoodGroupID = item.GoodGroupID,
                //            Code = item.Code,
                //            Name = item.Name,
                //            RetailPrice = item.RetailPrice,
                //            Notes = item.Notes,
                //            TypeID = item.TypeID,
                //            SiteVisible = item.SiteVisible,
                //            TabletVisible = item.TabletVisible,
                //            Sequence = item.Sequence,
                //        });
                //    }

                //    return View(model);
                //}



            }
        }


        public virtual ActionResult Aboutus()
        {

            return View();
        }

        public virtual ActionResult Contactus()
        {

            return View();
        }
        public virtual ActionResult ShopStores()
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    List<tblAccountBranch> model = new List<tblAccountBranch>();
                    model = db.tblAccountBranches.Where(x => x.TabletVisible == true).ToList();

                    return View(model);
                }
            }
            catch(Exception ex)
            {


                TempData = new TempDataDictionary()
                                {
                                    {"title", "خطا"},
                                    {"description", ex.Message},
                                    {"currentpage", Request.Url.ToString()},
                                    {"type", ""}
                                };
                return RedirectToAction(MVC.Message.Index());
            }

        }
        public virtual ActionResult OutLet()
        {
                return View();
            
        }

        public virtual async System.Threading.Tasks.Task<ActionResult> GetPagingOutLets(int pageNumber, int pageSize, List<string> ChekedBrands, decimal? MinPrice, decimal MaxPrice, string Sort)
        {
            List<int> SelectBrand = new List<int>();
            var model = new GoodViewModel();
            model.CategoriesGoods = new List<Goods>();
            IEnumerable<Goods> SortingTemp = new List<Goods>();
            var SettingFarsiExpireDate = await WebApi.GetSiteParameters(Utilities.Parameter.OutLet);
            var FarsiDateNow = AppConstants.GetPersianDate();
            using (var db = new DataBaseContext())
            {

                //filter Price
                if (MinPrice != 0 || MaxPrice != 0)
                {
                    if (ChekedBrands != null)
                    {
                        SelectBrand = ChekedBrands.Select(int.Parse).ToList();
                        if (Sort != "")
                        {
                            //SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                        }
                        var OutletGoods = (from good in db.tblGoods
                                           join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                           where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                          && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) >0)
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
                                               good.ExpireDate
                                           }).ToList();

                        if (SortingTemp.Count() == 0)
                        {
                            foreach (var item in OutletGoods)
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
                                    ExpireDate=item.ExpireDate
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
                                         && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                         orderby type.ID
                                         select type

                                       ).Distinct().ToList();

                        var Minprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                        && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                        && SelectBrand.Contains(good.TypeID ?? 0)
                                        select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                        var Maxprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                        && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                        && SelectBrand.Contains(good.TypeID ?? 0)
                                        select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                        var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                        return Json(PagedData, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {

                        if (Sort != "" || Sort != null)
                        {

                            // SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                        }


                        var CategoriesGood = (from good in db.tblGoods
                                              join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                              where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                              && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
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
                                                  good.ExpireDate
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
                                    ExpireDate = item.ExpireDate
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
                                        && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                         orderby type.ID
                                         select type

                                   ).Distinct().ToList();

                        var Minprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                       && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                        select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                        var Maxprice = (from good in db.tblGoods
                                        join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                        join type in db.tblGoodTypes on good.TypeID equals type.ID
                                        where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                       && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                        select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                        var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                        return Json(PagedData, JsonRequestBehavior.AllowGet);
                    }


                }


                if (ChekedBrands != null)
                {
                    if (Sort != "")
                    {

                        // SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                    }
                    SelectBrand = ChekedBrands.Select(int.Parse).ToList();

                    var CategoriesGood = (from good in db.tblGoods
                                          join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                          where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                          && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
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
                                              good.ExpireDate
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
                                ExpireDate = item.ExpireDate
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
                                      && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                     orderby type.ID
                                     select type

                                   ).Distinct().ToList();

                    var Minprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                    && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                    && SelectBrand.Contains(good.TypeID ?? 0)
                                    select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                    var Maxprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                    && SelectBrand.Contains(good.TypeID ?? 0)
                                    select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                    var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                    return Json(PagedData, JsonRequestBehavior.AllowGet);
                }

                else
                {


                    if (Sort != "")
                    {

                        // SortingTemp = Sorting(pageNumber, pageSize, GroupID, ChekedBrands, MinPrice, MaxPrice, Sort);
                    }

                   
                    var CategoriesGood = (from good in db.tblGoods
                                          join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                          where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                           && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
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
                                              good.ExpireDate
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
                                ExpireDate = item.ExpireDate
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
                                    && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                     orderby type.ID
                                     select type

                               ).Distinct().ToList();

                    var Minprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                    && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                    select new { goodsale.RetailPrice }).Min(x => x.RetailPrice);

                    var Maxprice = (from good in db.tblGoods
                                    join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                    join type in db.tblGoodTypes on good.TypeID equals type.ID
                                    where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                    && (good.FarsiExpireDate.CompareTo(SettingFarsiExpireDate) < 0 && good.FarsiExpireDate.CompareTo(FarsiDateNow) > 0)
                                    select new { goodsale.RetailPrice }).Max(x => x.RetailPrice);

                    var PagedData = Pagination.PagedResult(model.CategoriesGoods, Goodtypes, SelectBrand, pageNumber, pageSize, Minprice, Maxprice);
                    return Json(PagedData, JsonRequestBehavior.AllowGet);

                }

            }


        }
    }
}