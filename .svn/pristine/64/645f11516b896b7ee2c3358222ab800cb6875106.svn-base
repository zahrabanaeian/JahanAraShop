using JahanAraShop.Data.Context;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class HomeController : Controller
    {
      
        // GET: Home
        public virtual ActionResult Index()
        {


           
            using (var db=new DataBaseContext())
            {

                //fill MenuSite from GoodGroup
                var MenuGroup = new MenuModel();
                MenuGroup.FirstLevel = db.tblGoodGroups.Where(x => x.ParentID == null && x.TabletVisible == true).ToList();
                MenuGroup.SecondLevel = db.tblGoodGroups.Where(x => x.ParentID != null && x.TabletVisible == true ).ToList();
                var list = MenuGroup.SecondLevel.Select(c => c.ID).ToList();
                MenuGroup.ThirdLevel = db.tblGoodGroups.Where(x =>list.Contains((x.ID))).ToList();
                TempData["GoodMenu"] = MenuGroup;

                //fill GoodViewModel For MainPage
                var model = new GoodViewModel();
                model.NewestGoods= new List<Goods>();
                 var NewGood = (from good in db.tblGoods
                                             join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                             where good.TabletVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID==14
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
                                             }).Take(8).ToList();
                foreach(var item in NewGood)
                {
                    model.NewestGoods.Add(new Goods
                    {
                        ID = item.ID,
                        BarCode=item.BarCode,
                        GoodGroupID=item.GoodGroupID,
                        Code=item.Code,
                        Name=item.Name,
                        RetailPrice=item.RetailPrice,
                        Notes=item.Notes,
                        TypeID=item.TypeID,
                        SiteVisible=item.SiteVisible,
                        TabletVisible=item.TabletVisible,
                        Sequence=item.Sequence,
                    });
                }


                var SalesGood = (from good in db.tblGoods
                                 join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                 join detail in db.tblSaleInvoiceDetails on good.BarCode equals detail.BarCode
                                 where good.TabletVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
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

                return View(model);
            }

           

        }
    }
}