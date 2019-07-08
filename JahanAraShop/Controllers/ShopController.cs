﻿using JahanAraShop.Data.Context;
using JahanAraShop.Domain.Constants;
using JahanAraShop.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class ShopController : Controller
    {

        private static string _basketCookie = AppConstants.Basket;
        private static string _basketCountCookie = AppConstants.BasketCount;
        // GET: Shop
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult ShoppingCart()
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
                        decimal quantity=Convert.ToInt32(item[2]);

                        var Goods = (from good in db.tblGoods
                                     join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                     where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                     && good.BarCode == Barcode
                                     select new
                                     {
                                         Barcode = good.BarCode,
                                         Name = good.Name,
                                         RetailPrice = goodsale.RetailPrice,
                                         DiscountValue = goodsale.DiscountValue,
                                         DiscoutPercent=goodsale.DiscountPercent,
                                         Quantity= quantity



                                     }).SingleOrDefault();
                        if (Goods != null)
                        {
                            model.Items.Add(new CartItems
                            {
                                Name = Goods.Name,
                                Price = Goods.RetailPrice,
                                Barcode = Goods.Barcode,
                                Count = int.Parse(item[1]),
                                DiscountPercent=Goods.DiscoutPercent,
                                DiscountValue=Goods.DiscountValue,
                                Qauntity = Goods.Quantity

                            });
                        }
                    }
                   
                    return View(model);
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public virtual ActionResult Authenticate()
        {

            if (Request.IsAuthenticated)
                return RedirectToAction(MVC.Shop.Invoice());
          
            return View();
        }


        [HttpGet]
        public virtual ActionResult Invoice()
        {
           
                var model = new InvoiceViewModel { Items = new List<CartItems>() };

                #region لود کردن لیست اقلام محصولات

                if (Request.Cookies.AllKeys.Contains(_basketCountCookie) && int.Parse(Request.Cookies[_basketCountCookie].Value) > 0)
                {
                    using (var db = new DataBaseContext())
                    {
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
                                             DiscountValue = goodsale.DiscountValue,
                                             DiscountPercent=goodsale.DiscountPercent,

                                         }).SingleOrDefault();
                            if (Goods != null)
                            {
                                model.Items.Add(new CartItems
                                {
                                    Name = Goods.Name,
                                    Price = Goods.RetailPrice,
                                    Barcode = Goods.Barcode,
                                    Count = int.Parse(item[1]),
                                    DiscountValue=Goods.DiscountValue,
                                    DiscountPercent = Goods.DiscountPercent,



                                });
                            }
                        }

                        model.DeliverType = db.tblSaleDeliverTypes.ToList();
                        var states = db.tblSiteStates.ToDictionary(x => x.IdTbl_State, x => x.StateName);
                        SelectListItem[] tempList = new SelectListItem[states.Count];
                        int i = 0;
                        foreach (var item in states)
                        {
                            tempList[i] = new SelectListItem
                            {
                                Value = item.Key.ToString(),
                                Text = item.Value
                            };
                            i++;
                        }
                        model.States = tempList;

                        if (Request.IsAuthenticated)
                        {
                            var userId = User.Identity.GetUserId();
                            model.Buyer = db.tblOrganizations.FirstOrDefault(q => q.User_Id == userId);
                            Session["Buyer"] = model.Buyer;
                        }
                    }
                    Session["CartItems"] = model.Items;
                    Session["Invoice"] = model;
                    Session.Timeout = 30;
                }
                else
                {
                    return View();
                    //TempData = new TempDataDictionary()
                    //            {
                    //                {"title", "سبد خرید خالی است"},
                    //                {"description", "کاربر گرامی لطفا قبل از تلاش برای دیدن فاکتور حداقل یک محصول را به سبد خرید خود اضافه کنید."},
                    //                {"currentpage", Request.Url.ToString()},
                    //                {"type", ViewModels.MessageViewModel.MessageTypes.Warning}
                    //            };
                    //return RedirectToAction(MVC.Message.Index());
                }
                #endregion
                model.Buyer = new Domain.DomainModel.tblOrganization();
                model.DiscountPrecent = 0;

            return View(model);
            
            
        }

        public virtual ActionResult GetCounty(string id)
        {
            var stateid = Convert.ToInt32(id);
            using (var db = new DataBaseContext())
            {
                var counties = db.tblSiteCounties.Where(x => x.IdTbl_State == stateid).ToDictionary(x => x.IdTbl_County, x => x.CountyName);
                SelectListItem[] tempList = new SelectListItem[counties.Count];
                int i = 0;
                foreach (var item in counties)
                {
                    tempList[i] = new SelectListItem
                    {
                        Value = item.Key.ToString(),
                        Text = item.Value
                    };
                    i++;
                }
                return Json(tempList, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual ActionResult DownloadRules()
        { 
       
            var fname = "~/App_Data/JahanArarules.pdf";
            return new FilePathResult(fname, "APPLICATION / pdf") { FileDownloadName = "قوانین فروشگاه جهان آرا طوس.pdf" };
        }

        [HttpPost]
        public virtual bool SaveInvoice(string post, decimal totalvalue,decimal discountvalue,decimal discountpercent, string CoupnCode)
        {
            using (var db = new DataBaseContext())
            {
                var model = new InvoiceViewModel();
                model = (InvoiceViewModel)Session["Invoice"];
                model.PostCost = post;
                model.TotalValue = totalvalue;
                if (CoupnCode != "")
                {
                    model.DiscountPrecent = Math.Round(discountpercent, 2);
                    model.DiscountValue = discountvalue;
                    model.DiscountCode = CoupnCode;
                    model.TotalValue = (totalvalue- discountvalue);
                }
                //model.PostID = PostID;
                Session["Invoice"] = model;


            }
            return true;

        }


        [HttpPost]
        public virtual JsonResult CheckCoupnCode(string CoupnCode)
        {
            using (var db = new DataBaseContext())
            {

                var date = AppConstants.GetPersianDate();
                var model = db.tblSiteDiscountCodes.Where(x => x.DiscountCode == CoupnCode && 

                x.IsUsed == false  && x.FarsiExpiredate.CompareTo(date)>=0).SingleOrDefault();
                

                if (model != null)
                {
                    if (model.DiscountPercent != null && model.DiscountPercent!=0)
                    {
                        decimal discountpercent = model.DiscountPercent??0;
                        return Json(new {  res = 1 ,DisPercent= discountpercent, DisValue=0});

                    }
                    else
                    {
                        decimal discountvalue = model.DiscountValue ?? 0;
                        return Json(new { res=1, DisValue = discountvalue, DisPercent = 0 });

                    }
                   
                    
                }
                else
                {
                     var discount = "کد تخفیف معتبر نیست.";
                    return Json(new { result = discount, res = 0});
                    
                }

            }

        }


    }
}