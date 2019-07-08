using JahanAraShop.Data.Context;
using JahanAraShop.Domain.Constants;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Models;
using JahanAraShop.Services;
using Microsoft.AspNet.Identity;
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
    public partial class UserController : Controller
    {
        // GET: User
        public virtual async System.Threading.Tasks.Task<ActionResult> Index(string Cellphone)
        {
            using (var db = new DataBaseContext())
            {
                using (var client = new HttpClient())
                {
                    var Model = new InvoiceViewModel();
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    dynamic Sales = new ExpandoObject();
                    var jsonGetGoodGoal = JsonConvert.SerializeObject(Sales);
                    var content = new StringContent(jsonGetGoodGoal, Encoding.UTF8, AppConstants.JsonType);
                    var url = AppConstants.GetOrganizationSite + "/" + Cellphone + "/" + AppConstants.ApiKey;
                    var result = await client.GetAsync(url);
                    var response = result.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<tblOrganization>(response);
                    Model.Buyer = res;
                    var states = db.tblSiteStates.ToDictionary(x => x.IdTbl_State, x => x.StateName);
                    SelectListItem[] tempList = new SelectListItem[states.Count];
                    int i = 0;
                    foreach (var item in states)
                    {
                        tempList[i] = new SelectListItem
                        {
                            Value = item.Key.ToString(),
                            Text = item.Value,
                            //Selected  = item.Key!=null? true:false,
                        };
                        i++;
                    }
                    Model.States = tempList;

                    return View(Model);
                }
            }

        }

        public virtual async System.Threading.Tasks.Task<ActionResult> EditOrg(InvoiceViewModel Model)
        {

            var SaveOrgModel = new SaveOrganizationModel();
            SaveOrgModel.ApiKey = AppConstants.ApiKey;
            SaveOrgModel.TblOrganization = Model.Buyer;
            var res = new Result();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConstants.ApiAddress);
                var url = AppConstants.EditOrganizationSite;
                var jsonEditOrganization = JsonConvert.SerializeObject(SaveOrgModel);
                var content = new StringContent(jsonEditOrganization, Encoding.UTF8, AppConstants.JsonType);
                var result = await client.PostAsync(url, content);
                var response = result.Content.ReadAsStringAsync().Result;
                res = JsonConvert.DeserializeObject<Result>(response);

                if (res.Message == "Done")
                {
                    return RedirectToAction(MVC.User.Index(Model.Buyer.CellPhone));
                }


            }


            return RedirectToAction(MVC.User.Index(Model.Buyer.CellPhone));

            // return View();


        }

        public virtual async System.Threading.Tasks.Task<ActionResult> WishList(string cellphone)
        {
            ViewBag.cellphone = cellphone;
            using (var db = new DataBaseContext())
            {
                using (var client = new HttpClient())
                {
                    var model = new GoodViewModel();
                    model.Good = new List<Goods>();
                    var user = User.Identity.GetUserId();
                    var product = (from good in db.tblGoods
                                   join list in db.tblSiteWishLists on good.ID equals list.Good_Id
                                   join goodsale in db.tblGoodSaleInfoes on good.BarCode equals goodsale.Barcode
                                   where good.SiteVisible == true && goodsale.RetailPrice != 0 && goodsale.PricingMethodID == 14
                                   where list.SiteUser_Id == user
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
                                   }).ToList();
                    //دریافت موجودی
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    dynamic Sales = new ExpandoObject();
                    var jsonGetGoodGoal = JsonConvert.SerializeObject(Sales);
                    var content = new StringContent(jsonGetGoodGoal, Encoding.UTF8, AppConstants.JsonType);
                    foreach (var item in product)
                    {

                        var url = AppConstants.InventoryQuantity + "/" + AppConstants.ApiKey + "/" + item.BarCode;
                        var result = await client.GetAsync(url);
                        var response = result.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<SpInventoryQuantityWebService>(response);
                        model.Good.Add(new Goods
                        {
                            ID = item.ID,
                            BarCode = item.BarCode,
                            GoodGroupID = item.GoodGroupID,
                            Code = item.Code,
                            Name = item.Name,
                            Name3 = item.Name3,
                            RetailPrice = item.RetailPrice,
                            Notes = item.Notes,
                            TypeID = item.TypeID,
                            SiteVisible = item.SiteVisible,
                            TabletVisible = item.TabletVisible,
                            Sequence = item.Sequence,
                            DiscountPercent = item.DiscountPercent,
                            DiscountValue = item.DiscountValue,
                            HasRemiand = (res.Remained) != 0 ? true : false,
                        });
                    }
                    return View(model);
                }
            }
        }
        public virtual ActionResult DeleteWishes(int goodid)
        {
            using (var db = new DataBaseContext())
            {
                var user = User.Identity.GetUserId();
                var wish = db.tblSiteWishLists.Where(x => x.Good_Id == goodid && x.SiteUser_Id == user).SingleOrDefault();
                db.tblSiteWishLists.Remove(wish);
                db.SaveChanges();
                return Json(new { result = true });
            }
        }

        public virtual ActionResult Orders(string cellphone)
        {

            try
            {
                List<tblSaleInvoice> invoices = new List<tblSaleInvoice>();
                ViewBag.cellphone = cellphone;
                using (var db = new DataBaseContext())
                {

                    var model = (from sale in db.tblSaleInvoices
                                 join org in db.tblOrganizations on sale.CustomerId equals org.ID
                                 where org.CellPhone == cellphone
                                 select new { sale }
                               ).ToList();

                    foreach(var item in model)
                    {

                        invoices.Add(new tblSaleInvoice
                        {
                            Id=item.sale.Id,
                            FarsiCreateDate = item.sale.FarsiCreateDate,
                            ManualCode=item.sale.ManualCode,
                            TotalValue=item.sale.TotalValue,


                        });

                    }
                    return View(invoices);
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

        public virtual ActionResult OrdersDetail( int saleid)
        {

            // InvoiceViewModel saledetail = new InvoiceViewModel();
                using(var db=new DataBaseContext())
                {

                    var model = (from detail in db.tblSaleInvoiceDetails
                                 join good in db.tblGoods on detail.BarCode equals good.BarCode
                                 where detail.InvoiceID==saleid
                                 select new
                                 {
                                     detail.BarCode,
                                     detail.Qty,
                                     detail.RetailPrice,
                                     detail.DiscountValue,
                                     detail.DiscountPercent,
                                     good.Name
                                 }).ToList();




                    return Json(model, JsonRequestBehavior.AllowGet);
                }


        

           

        }

    }
}