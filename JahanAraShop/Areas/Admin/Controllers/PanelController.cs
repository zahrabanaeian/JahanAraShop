using JahanAraShop.Areas.Admin.Models;
using JahanAraShop.Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JahanAraShop.Services;
using JahanAraShop.Domain.Constants;
using System.Text;
using JahanAraShop.Domain.DomainModel;

namespace JahanAraShop.Areas.Admin.Controllers
{
    public partial class PanelController : Controller
    {

        
        // GET: Admin/Panel
        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult SlidShow()
        {
            using (var db = new DataBaseContext())
            {
                SlideShowViewModel model = new SlideShowViewModel();
                if (TempData["message"] != null)
                {

                    TempData["message"] = TempData["message"];

                }
                else
                {
                    TempData["message"] = null;
                }
                model.ListSlideShow = db.TblSiteSlideShow.OrderBy(m => m.Priority).ToList();
                return View(model);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SlideShow(SlideShowViewModel model, HttpPostedFileBase imgFa)
        {
            var allowedExtensions = new[]
                {
                  ".Jpg", ".png", ".jpg", "jpeg",".bmp"
                };
            if (imgFa != null)
            {


                var fileNameIMG = Path.GetFileName(imgFa.FileName); //getting only file name(ex-ganesh.jpg)  
                var extIMG = Path.GetExtension(imgFa.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(extIMG)) //check what type of extension  
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(imgFa.InputStream, true, true))
                    {
                        if (image.Width == 1920 && image.Height == 995)
                        {
                            string finalFileNameIMG = Guid.NewGuid().ToString() + extIMG;
                            var pathIMG = Path.Combine(Server.MapPath("/Content/images/Slider"), finalFileNameIMG);


                            imgFa.SaveAs(pathIMG);
                            model.SlideShow.Picture = finalFileNameIMG;

                            using (var db = new DataBaseContext())
                            {
                                db.TblSiteSlideShow.Add(model.SlideShow);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            TempData["message"] = "لطفا تصویر را در اندازه 995*1920 و حجم کمتر از 100 کیلوبایت ارسال فرمایید";
                            return RedirectToAction(MVC.Admin.Panel.SlidShow());
                        }
                    }

                }
                else
                {
                    TempData["message"] = "لطفا تصویر را در قالب عکس ارسال فرمایید";
                    return RedirectToAction(MVC.Admin.Panel.SlidShow());
                }
            }
            TempData["message"] = " با موفقیت اپلود شد";
            return RedirectToAction(MVC.Admin.Panel.SlidShow());

        }

        public virtual ActionResult News()
        {
            using (var db = new DataBaseContext())
            {
                NewsViewModel model = new NewsViewModel();
                model.ListSiteInformations = db.tblSiteInformations.ToList();

                return View(model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SaveNews(NewsViewModel model, HttpPostedFileBase imgFa)
        {
            using (var db = new DataBaseContext())
            {


                var allowedExtensions = new[]
                {
                  ".Jpg", ".png", ".jpg", "jpeg",".bmp"
                };
                if (imgFa != null)
                {


                    var fileNameIMG = Path.GetFileName(imgFa.FileName); //getting only file name(ex-ganesh.jpg)  
                    var extIMG = Path.GetExtension(imgFa.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(extIMG)) //check what type of extension  
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(imgFa.InputStream, true, true))
                        {
                            if (image.Width <= 345 && image.Height <= 211)
                            {
                                string finalFileNameIMG = Guid.NewGuid().ToString() + extIMG;
                                var pathIMG = Path.Combine(Server.MapPath("/Content/images/News"), finalFileNameIMG);


                                imgFa.SaveAs(pathIMG);
                                model.SiteInformation.SmallPicture = finalFileNameIMG;
                                model.SiteInformation.LargePicture = finalFileNameIMG;
                                model.SiteInformation.IdtblInformationType = 1;
                                model.SiteInformation.CreateDate = Utilities.ParsToChrisDate(model.SiteInformation.FarsiCreateDate);
                                db.tblSiteInformations.Add(model.SiteInformation);
                                db.SaveChanges();

                            }
                            else
                            {
                                TempData["message"] = "لطفا تصویر را در اندازه 995*1920 و حجم کمتر از 100 کیلوبایت ارسال فرمایید";
                                return RedirectToAction(MVC.Admin.Panel.News());
                            }
                        }

                    }
                    else
                    {
                        TempData["message"] = "لطفا تصویر را در قالب عکس ارسال فرمایید";
                        return RedirectToAction(MVC.Admin.Panel.News());
                    }
                }
                return RedirectToAction(MVC.Admin.Panel.News());
            }


        }


        public virtual JsonResult DeleteNews(int Id)
        {
            using (var db = new DataBaseContext())
            {
                var news = db.tblSiteInformations.Where(x => x.IdtblInformation == Id).SingleOrDefault();
                if (news != null)
                {
                    db.tblSiteInformations.Remove(news);
                    db.SaveChanges();
                    return Json(new { result = true });


                }

                else
                {
                    return Json(new { result = false });

                }
            }


        }

        public virtual ActionResult InvoiceList()
        {
            return View();

        }

        [HttpPost]
        public virtual ActionResult GetInvoiceReport(InvoiceReportModel model)
        {
            using (var db = new DataBaseContext())
            {
                if (model.CustomerPhone != null)
                {
                    var query = (from invoice in db.tblSaleInvoices
                                 join org in db.tblOrganizations on invoice.CustomerId equals org.ID
                                 where invoice.FarsiCreateDate.CompareTo(model.FromDate) >= 0 && invoice.FarsiCreateDate.CompareTo(model.ToDate) <= 0
                                 && org.CellPhone == model.CustomerPhone
                                 select new
                                 {
                                     invoice.Id,
                                     invoice.FarsiCreateDate,
                                     invoice.TotalValue,
                                     invoice.Transfer,
                                     organame = org.FirstName + " " + org.Name,
                                     invoice.ManualCode,
                                     invoice.SuchargeValue


                                 }).ToList();


                    return Json(query, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    var query = (from invoice in db.tblSaleInvoices
                                 join org in db.tblOrganizations on invoice.CustomerId equals org.ID
                                 where invoice.FarsiCreateDate.CompareTo(model.FromDate) >= 0 && invoice.FarsiCreateDate.CompareTo(model.ToDate) <= 0
                                
                                 select new
                                 {
                                     invoice.Id,
                                     invoice.FarsiCreateDate,
                                     invoice.TotalValue,
                                     invoice.Transfer,
                                     organame = org.FirstName + " " + org.Name,
                                     invoice.ManualCode,
                                     invoice.SuchargeValue


                                 }).ToList();


                    return Json(query, JsonRequestBehavior.AllowGet);

                }
            }

        }

        public virtual ActionResult CreateDiscountcode()            
        {

            using (var db = new DataBaseContext())
            {
                DicountCodeViewModel model = new DicountCodeViewModel();
                var customer = db.tblOrganizations.ToDictionary(x => x.ID, x =>x.FirstName+' '+ x.Name+'-'+x.CellPhone);
                SelectListItem[] tempList = new SelectListItem[customer.Count];
                int i = 0;
                foreach (var item in customer)
                {
                    tempList[i] = new SelectListItem
                    {
                        Value = item.Key.ToString(),
                        Text = item.Value
                    };
                    i++;
                }
                

                model.CustomerList = tempList;

                return View(model);
            }

        }

        [HttpPost]
        public virtual async System.Threading.Tasks.Task<ActionResult> SaveDiscountcode(List<DisCodlist> model)
        {


            using (var db = new DataBaseContext())
            {


                foreach (var item in model)
                {

                    var DiscountCode = Utilities.RandomGenerator();

                    tblSiteDiscountCode arr = new tblSiteDiscountCode();
                    arr.CreateDate = DateTime.Now;
                    arr.FarsiCreateDate = AppConstants.GetPersianDate();
                    arr.OrganizationID = Convert.ToInt32(item.orgid);
                    arr.ExpireDate = Utilities.ParsToChrisDate(item.expiredate);
                    arr.FarsiExpiredate = item.expiredate;
                    arr.DiscountCode = DiscountCode;
                    arr.DiscountPercent = Convert.ToDecimal(item.DiscountPercent);
                    arr.DiscountValue = Convert.ToDecimal(item.DiscountValue);
                    arr.IsUsed = false;
                    if (item.discription == null) item.discription = "_";
                    arr.Description = item.discription;

                    db.tblSiteDiscountCodes.Add(arr);
                    db.SaveChanges();

                    //send sms
                    var discount = "";
                    var customer = db.tblOrganizations.Where(x => x.ID == item.orgid).SingleOrDefault();
                    var userName = await WebApi.GetSiteParameters(Utilities.Parameter.UserName);
                    var password = await WebApi.GetSiteParameters(Utilities.Parameter.PassWord);
                    var number = await WebApi.GetSiteParameters(Utilities.Parameter.Number);
                    var message = db.TblAtashTabletParameters.Where(x => x.Id == 40014).SingleOrDefault();
                    if (item.DiscountValue != 0) discount= String.Format("{0:0,0}", item.DiscountValue) + " ریال ";
                    if (item.DiscountPercent !=0) discount = item.DiscountPercent.ToString() + " درصد ";
                    

                    var mes= WebApi.SendSmsOfferCdeForCustomer(customer.CellPhone, customer.Name, DiscountCode,item.expiredate, discount, message.Value, userName, password, number);
                    //send sms

                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }





        }



    }


}