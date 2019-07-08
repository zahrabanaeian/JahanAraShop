﻿using JahanAraShop.Data.Context;
using JahanAraShop.Domain.Constants;
using JahanAraShop.Domain.DomainModel;
using JahanAraShop.Getaway;
using JahanAraShop.Models;
using JahanAraShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers.Bank
{
    public partial class PaymentController : Controller
    {
        private static readonly string BasketCountCookie = AppConstants.BasketCount;
        private static readonly string BasketCookie = AppConstants.Basket;

        [HttpPost]
        public virtual async Task< ActionResult> Index(InvoiceViewModel invoice)
        {
            var organization = invoice.Buyer;
            invoice = (InvoiceViewModel)Session["Invoice"];
            invoice.Buyer = organization;

            try
            {
                var manualCode = DateTime.Now.Ticks.ToString();
                Session["manualCode"] = manualCode;
                Session["DeviceType"] = "Site";
                Session["SendSms"] = true;
                var orderId = Convert.ToInt64(manualCode); //شماره تراکنش که باید منحصر به فرد باشد
                var additionalText = $"{orderId},{DateTime.Now}"; // توضیحات شما برای این تراکنش
                long total;

                total = (long)invoice.TotalValue;
                //total=(total-(long) invoice.DiscountValue)+ long.Parse(invoice.PostCost); 
                Session["PostPrice"] = long.Parse(invoice.PostCost);
                Session["TotalPrice"] = total;
                Session["DiscountValue"] = invoice.DiscountValue;
                Session["DiscountPercent"] = invoice.DiscountPrecent;
                Session["CoupnCode"] = invoice.DiscountCode;



                using (var db = new DataBaseContext())
                {
                    var bankLog = new tblBankGatewayLog
                    {
                        CreateDate = DateTime.Now,
                        FarsiCreateDate = Utilities.FarsiDateTimeNow(),
                        Pamount = total.ToString(),
                        Porderid = orderId.ToString(),
                        Pstatus = 1000
                    };

                    db.TblBankGatewayLogs.Add(bankLog);
                    db.SaveChanges();
                    Session["logId"] = bankLog.Id;
                }
                if (total <= 0)
                {
                    var newInvoice = invoice;
                    newInvoice.Items = ((IList<CartItems>)Session["CartItems"]);
                    tblSaleInvoice invoice1 = null;
                    int deliver;
                    if (Convert.ToInt32(newInvoice.PostCost) == 0) deliver = 4;
                    else deliver = 5;

                    using (var db = new DataBaseContext())
                    {
                        invoice1 = new tblSaleInvoice
                        {
                            DiscountPercent = (decimal)Session["DiscountPercent"],
                            DiscountTypeId = 1,
                            DiscountValue = (decimal)Session["DiscountValue"],
                            Notes = Utilities.GetNote(newInvoice.Buyer.FirstName, newInvoice.Buyer.Name,
                                    newInvoice.Buyer.CellPhone, newInvoice.Buyer.Address,
                                    Utilities.FarsiDateTimeNow(), "وب سایت"),

                            TotalValue = (long)Session["TotalPrice"],
                            PureTotalValue = (long)Session["TotalPrice"],
                            ManualCode = (string)Session["manualCode"],
                            IsPaidByCash = true,
                            DeliverTypeId = deliver,
                            CustomerMessage = (string)Session["CoupnCode"],
                        };


                        var cartItems = (IList<CartItems>)Session["CartItems"];
                        short i = 1;
                        var items1 = cartItems.Select(items => new tblSaleInvoiceDetail
                        {
                            BarCode = items.Barcode,
                            Qty = items.Count,
                            RetailPrice = items.Price.Value,
                            DiscountPercent=items.DiscountPercent,
                            DiscountValue=items.DiscountValue,
                            RowNumber = i++
                        }).ToList();


                        var buyer = new tblOrganization();
                        if (Session["Buyer"] != null)
                            buyer = (tblOrganization)Session["Buyer"];
                        else
                        {
                            buyer.Address = newInvoice.Buyer.Address;
                            buyer.FirstName = newInvoice.Buyer.FirstName;
                            buyer.Name = newInvoice.Buyer.Name;
                            buyer.CellPhone = newInvoice.Buyer.CellPhone;
                            buyer.WorkActivityID = 2;
                            buyer.Phone1 = newInvoice.Buyer.Phone1;
                            buyer.Email = newInvoice.Buyer.Email;
                            buyer.PostalCode = newInvoice.Buyer.PostalCode;
                            buyer.Notes = newInvoice.Buyer.Notes;
                            buyer.CityId = newInvoice.Buyer.CityId;
                            buyer.ProvinceId = newInvoice.Buyer.ProvinceId;
                        }
                        var finilizeInvoice = new FinalizeInvoice()
                        {
                            ApiKey = AppConstants.ApiKey,
                            TblSaleInvoice = invoice1,
                            TblSaleInvoiceDetail = items1,
                            TblOrganization = buyer,
                            DeviceType = (string)Session["DeviceType"],
                            SendSms = (bool)Session["SendSms"],
                            Language = (string)Session["Language"]
                        };


                        var result = await WebApi.SaveLocalInvoice(finilizeInvoice);
                        if (result.Id != 0)
                        {
                            return RedirectToAction(MVC.Payment.SaveZeroInvoice());
                        }
                    }
                }
                var bankMellatImplement = new BankMellatImplement();
                var resultRequest = bankMellatImplement.BpPayRequest(orderId, total, additionalText);
                var statusSendRequest = resultRequest.Split(',');
                Session.Timeout = 15;
             if (int.Parse(statusSendRequest[0]) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
              {
                    using (var db = new DataBaseContext())
                    {
                        int id = (int)Session["logId"];
                        var bankLog = db.TblBankGatewayLogs.Find(id);
                        bankLog.Pstatus = 10001;
                        db.SaveChanges();
                        //------------------------
                        var provinceName = db.tblSiteStates.First(x => x.IdTbl_State == invoice.Buyer.ProvinceId).StateName;
                        var cityName = db.tblSiteCounties.First(x => x.IdTbl_County == invoice.Buyer.CityId).CountyName;
                        invoice.Buyer.Address = provinceName + "- " + cityName + "- " + invoice.Buyer.Address;
                        //-----------------------

                   }
                    Session["InvoiceModel"] = invoice;
                  // return RedirectToAction("RedirectVpos", "Payment", new { id = "1" });
                 return RedirectToAction("RedirectVpos", "Payment", new { id = statusSendRequest[1] });
              }



                const string title = "پرداخت آنلاین";
                var description =
                   bankMellatImplement.DescribtionStatusCode(int.Parse(statusSendRequest[0])).Replace("_", " ");

                TempData = new TempDataDictionary()
            {
                {"title", title},
                {"description", description},
                {"currentpage", "/Shop"},
                {"type", MessageViewModel.MessageTypes.Error}
            };
                return RedirectToAction(MVC.Message.Index());
            }
            catch (Exception ex)
            {

                const string title = "پرداخت آنلاین";
                var description = ex.Message + Environment.NewLine + "متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید";
                TempData = new TempDataDictionary()
                {
                    {"title", title},
                    {"description", description},
                    {"currentpage", "/Shop"},
                    {"type", MessageViewModel.MessageTypes.Error}
                };
                return RedirectToAction(MVC.Message.Index());
            }

        }

        public virtual ActionResult RedirectVpos(string id)
        {
            try
            {
                if (id == null)
                {

                    const string title = "پرداخت آنلاین";
                    const string description = "هیچ شماره پیگیری برای پرداخت از سمت بانک ارسال نشده است!";
                    TempData = new TempDataDictionary()
                    {
                        {"title", title},
                        {"description", description},
                        {"currentpage", "/Shop"},
                        {"type", MessageViewModel.MessageTypes.Error}
                    };
                    using (var db = new DataBaseContext())
                    {
                        var logId = (int)Session["logId"];
                        var bankLog = db.TblBankGatewayLogs.Find(logId);
                        bankLog.Pstatus = 0;
                        db.SaveChanges();
                    }
                    return RedirectToAction(MVC.Message.Index());
                }
                var invoice = (InvoiceViewModel)Session["InvoiceModel"];                
                invoice.Items = ((IList<CartItems>)Session["CartItems"]);                
                ViewBag.id = id;
                return View(invoice);
            }
            catch (Exception error)
            {
                const string title = "پرداخت آنلاین";
                var description = error.Message + Environment.NewLine + "متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید";
                TempData = new TempDataDictionary()
                {
                    {"title", title},
                    {"description", description},
                    {"currentpage", "/Shop"},
                    {"type", MessageViewModel.MessageTypes.Error}
                };
                return RedirectToAction(MVC.Message.Index());
            }
        }


        [HttpPost]
        public virtual async Task<ActionResult> SaveLocalInvoice()
        {

            var newInvoice = (InvoiceViewModel)Session["InvoiceModel"];
            newInvoice.Items = ((IList<CartItems>)Session["CartItems"]);
            tblSaleInvoice invoice = null;
            int deliver;
            if (Convert.ToInt32(newInvoice.PostCost) == 0) deliver = 4;
            else deliver = 5;
            decimal discountpercent = 0;
            decimal discountvalue = 0;

            using (var db = new DataBaseContext())
            {
                var test = (string)Session["CoupnCode"];
                if (test == "" || test==null)
                {
                    discountpercent = 0;
                    discountvalue = 0;

                }
                else
                {
                    discountpercent = (decimal)Session["DiscountPercent"];
                    discountvalue = (decimal)Session["DiscountValue"];


                }
                invoice = new tblSaleInvoice
                {

                    DiscountPercent = discountpercent,
                    DiscountTypeId = 1,
                    DiscountValue = discountvalue,
                    Notes = Utilities.GetNote(newInvoice.Buyer.FirstName, newInvoice.Buyer.Name,
                            newInvoice.Buyer.CellPhone, newInvoice.Buyer.Address,
                            Utilities.FarsiDateTimeNow(), "وب سایت"),

                    TotalValue = (long)Session["TotalPrice"],
                    PureTotalValue = (long)Session["TotalPrice"],
                    ManualCode = (string)Session["manualCode"],
                    IsPaidByCash = true,
                    DeliverTypeId = deliver,
                    CustomerMessage =(string) Session["CoupnCode"],
                };


                var cartItems = (IList<CartItems>)Session["CartItems"];
                short i = 1;
                var items1 = cartItems.Select(items => new tblSaleInvoiceDetail
                {
                    BarCode = items.Barcode,
                    Qty = items.Count,
                    RetailPrice = items.Price.Value,
                    DiscountValue=items.DiscountValue,
                    DiscountPercent=items.DiscountPercent,
                    RowNumber = i++
                }).ToList();


                var buyer = new tblOrganization();
                if (Session["Buyer"] != null)
                    buyer = (tblOrganization)Session["Buyer"];
                else
                {
                    buyer.Address = newInvoice.Buyer.Address;
                    buyer.FirstName = newInvoice.Buyer.FirstName;
                    buyer.Name = newInvoice.Buyer.Name;
                    buyer.CellPhone = newInvoice.Buyer.CellPhone;
                    buyer.WorkActivityID = 2;
                    buyer.Phone1 = newInvoice.Buyer.Phone1;
                    buyer.Email = newInvoice.Buyer.Email;
                    buyer.PostalCode = newInvoice.Buyer.PostalCode;
                    buyer.Notes = newInvoice.Buyer.Notes;
                    buyer.CityId = newInvoice.Buyer.CityId;
                    buyer.ProvinceId = newInvoice.Buyer.ProvinceId;
                }
                var finilizeInvoice = new FinalizeInvoice()
                {
                    ApiKey = AppConstants.ApiKey,
                    TblSaleInvoice = invoice,
                    TblSaleInvoiceDetail = items1,
                    TblOrganization = buyer,
                    DeviceType = (string)Session["DeviceType"],
                    SendSms = (bool)Session["SendSms"],
                    Language = (string)Session["Language"]
                };


                var result = await WebApi.SaveLocalInvoice(finilizeInvoice);

                if (result.Id != 0)
                {

                    return Json(true, JsonRequestBehavior.AllowGet);
                }


            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public virtual async Task<ActionResult> BankCallback()
        {
            var runBpReversalRequest = false;
            long saleReferenceId = -999;
            long saleOrderId = -999;
            var bankMellatImplement = new BankMellatImplement();

            string deviceType;
            if (Session["DeviceType"] != null)
                deviceType = (string)Session["DeviceType"];
            else
                deviceType = "Site";



            const string title = "پرداخت آنلاین";
            try
            {

                var resultCodeBpPayRequest = Request.Params["ResCode"];

                //Result Code
                var resultCodeBpinquiryRequest = "-9999";

                if (int.Parse(resultCodeBpPayRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                {
                    saleReferenceId = long.Parse(Request.Params["SaleReferenceId"]);
                    saleOrderId = long.Parse(Request.Params["SaleOrderId"]);
                    using (var db = new DataBaseContext())
                    {
                        var id = (int)Session["logId"];
                        var bankLog = db.TblBankGatewayLogs.Find(id);
                        bankLog.Pstatus = 10002;
                        db.SaveChanges();
                    }
                    #region Success
                    //تایید تراکنش خرید
                    var resultCodeBpVerifyRequest = bankMellatImplement.VerifyRequest(saleOrderId, saleOrderId, saleReferenceId);

                    if (string.IsNullOrEmpty(resultCodeBpVerifyRequest))
                    {
                        #region Inquiry Request

                        //استعلام وجه
                        resultCodeBpinquiryRequest = bankMellatImplement.InquiryRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if (int.Parse(resultCodeBpinquiryRequest) != (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                        {
                            //the transactrion faild

                            var description = bankMellatImplement.DescribtionStatusCode(int.Parse(resultCodeBpinquiryRequest)).Replace("_", " ");

                            TempData = new TempDataDictionary()
                            {
                                {"title", title},
                                {"description", description},
                                {"currentpage", "/Shop"},
                                {"type", MessageViewModel.MessageTypes.Error}
                            };
                            using (var db = new DataBaseContext())
                            {
                                int id = (int)Session["logId"];
                                var bankLog = db.TblBankGatewayLogs.Find(id);
                                bankLog.Pstatus = int.Parse(resultCodeBpinquiryRequest);
                                db.SaveChanges();
                            }

                            runBpReversalRequest = true;
                        }

                        #endregion
                    }

                    if (resultCodeBpVerifyRequest != null && ((int.Parse(resultCodeBpVerifyRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                                                              ||
                                                              (int.Parse(resultCodeBpinquiryRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)))
                    {

                        #region SettleRequest

                        var resultCodeBpSettleRequest = bankMellatImplement.SettleRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if ((int.Parse(resultCodeBpSettleRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                            || (int.Parse(resultCodeBpSettleRequest) == (int)BankMellatImplement.MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ))
                        {
                            TempData["Message"] = "تراکنش شما با موفقیت انجام شد." + Environment.NewLine;
                            TempData["Message"] += " لطفا شماره پیگیری را یادداشت نمایید: " + saleReferenceId + Environment.NewLine;


                            using (var db = new DataBaseContext())
                            {
                                var id = (int)Session["logId"];
                                var bankLog = db.TblBankGatewayLogs.Find(id);
                                bankLog.Pstatus = 10003;
                                bankLog.PTransaction = saleReferenceId.ToString();
                                db.SaveChanges();

                                var saleinvoice = db.tblSaleInvoices.Where(x => x.ManualCode == bankLog.Porderid).SingleOrDefault();
                                var saleinvoicedetail = db.tblSaleInvoiceDetails.Where(x => x.InvoiceID == saleinvoice.Id).ToList();
                                var maxcode = db.tblSaleInvoices.Max(x => x.Code);
                                if (maxcode == null) { maxcode = 1; }
                                else maxcode++;
                                saleinvoice.Code = maxcode;
                                db.SaveChanges();
                                tblSaleInvoice invoice = null;

                                invoice = new tblSaleInvoice
                                {

                                    DiscountPercent = saleinvoice.DiscountPercent,
                                    DiscountTypeId = saleinvoice.DiscountTypeId,
                                    DiscountValue = saleinvoice.DiscountValue,
                                    Notes = saleinvoice.Notes,
                                    TotalValue = saleinvoice.TotalValue,
                                    ManualCode = saleinvoice.ManualCode,
                                    IsPaidByCash = true,
                                    DeliverTypeId = saleinvoice.DeliverTypeId,
                                    CustomerId = saleinvoice.CustomerId,
                                };



                                var cartItems = saleinvoicedetail;
                                short i = 1;
                                var items1 = cartItems.Select(items => new tblSaleInvoiceDetail
                                {
                                    BarCode = items.BarCode,
                                    Qty = items.Qty,
                                    RetailPrice = items.RetailPrice,
                                    DiscountPercent = items.DiscountPercent,
                                    DiscountValue = items.DiscountValue,
                                    RowNumber = i++
                                }).ToList();

                                var buyer = new tblOrganization();
                                var finilizeInvoice = new FinalizeInvoice()
                                {
                                    ApiKey = AppConstants.ApiKey,
                                    TblSaleInvoice = invoice,
                                    TblSaleInvoiceDetail = items1,
                                    TblOrganization = buyer,
                                    DeviceType = (string)Session["DeviceType"],
                                    SendSms = (bool)Session["SendSms"],
                                    Language = (string)Session["Language"]
                                };


                                var result = await WebApi.SaveInvoice(finilizeInvoice);

                                if (result.Id != 0)
                                {
                                    saleinvoice.Transfer = true;
                                    bankLog.SaleInvoiceID = result.Id;
                                    //if use coupncode
                                    if (saleinvoice .CustomerMessage!= null)
                                    {
                                        var Coupn = db.tblSiteDiscountCodes.Where(x => x.DiscountCode == saleinvoice.CustomerMessage && x.IsUsed == false).SingleOrDefault();
                                        if(Coupn!=null) Coupn.IsUsed = true;
                                    }

                                    db.SaveChanges();
                                    if (saleinvoice.DeliverTypeId != 4)
                                        using (var client = new HttpClient())
                                        {
                                            client.BaseAddress = new Uri(AppConstants.ApiAddress);
                                            var url = $"SaveSucharge/{result.Id}/{AppConstants.ApiKey}";
                                            await client.GetStringAsync(url);
                                        }
                                    var description = TempData["Message"] + "\r\n" + "شماره فاکتور ثبت شده :" + result.Id;
                                    TempData = new TempDataDictionary()
                                                  {
                                                      {"title", "خرید"},
                                                      {"description", description},
                                                      {"currentpage", "/Shop"},
                                                      {"type", MessageViewModel.MessageTypes.Success}
                                                  };


                                    return RedirectToAction(MVC.Message.Index());

                                }
                            }

                            if (Request.Cookies[BasketCookie] != null)
                            {
                                var c = new HttpCookie(BasketCookie) { Expires = DateTime.Now.AddDays(-1) };
                                Response.Cookies.Add(c);
                            }
                            if (Request.Cookies[BasketCountCookie] != null)
                            {
                                var c = new HttpCookie(BasketCountCookie) { Expires = DateTime.Now.AddDays(-1) };
                                Response.Cookies.Add(c);
                            }
                            if (Request.Cookies["nininazShopSelectedPack"] != null)
                            {
                                var c = new HttpCookie("nininazShopSelectedPack")
                                {
                                    Expires = DateTime.Now.AddDays(-1)
                                };
                                Response.Cookies.Add(c);
                            }
                        }
                        else
                        {
                            TempData["Message"] = bankMellatImplement.DescribtionStatusCode(int.Parse(resultCodeBpSettleRequest)).Replace("_", " ");

                            var description = (string)TempData["Message"];
                            TempData = new TempDataDictionary()
                            {
                                {"title", title},
                                {"description", description},
                                {"currentpage", "/Shop"},
                                {"type", MessageViewModel.MessageTypes.Error}
                            };
                            using (var db = new DataBaseContext())
                            {
                                int id = (int)Session["logId"];
                                var bankLog = db.TblBankGatewayLogs.Find(id);
                                bankLog.Pstatus = int.Parse(resultCodeBpSettleRequest);
                                db.SaveChanges();
                            }

                            runBpReversalRequest = true;
                        }


                        #endregion
                    }

                    //end if first
                    else
                    {
                        if (resultCodeBpVerifyRequest != null)
                        {
                            TempData["Message"] = bankMellatImplement.DescribtionStatusCode(int.Parse(resultCodeBpVerifyRequest)).Replace("_", " ");
                            var description = (string)TempData["Message"];
                            TempData = new TempDataDictionary()
                            {
                                {"title", title},
                                {"description", description},
                                {"currentpage", "/Shop"},
                                {"type", MessageViewModel.MessageTypes.Error}
                            };
                            using (var db = new DataBaseContext())
                            {
                                int id = (int)Session["logId"];
                                var bankLog = db.TblBankGatewayLogs.Find(id);
                                bankLog.Pstatus = int.Parse(resultCodeBpVerifyRequest);
                                db.SaveChanges();
                            }

                        }
                        runBpReversalRequest = true;
                    }

                    #endregion
                }
                else
                {
                    TempData["Message"] = bankMellatImplement.DescribtionStatusCode(int.Parse(resultCodeBpPayRequest)).Replace("_", " ");
                    var description = (string)TempData["Message"];
                    TempData = new TempDataDictionary() {
                            {"title", title},
                            {"description", description},
                            { "currentpage", "/Shop"},
                            { "type", MessageViewModel.MessageTypes.Error}
                        };
                    using (var db = new DataBaseContext())
                    {
                        int id = (int)Session["logId"];
                        var bankLog = db.TblBankGatewayLogs.Find(id);
                        bankLog.Pstatus = int.Parse(resultCodeBpPayRequest);
                        db.SaveChanges();
                    }

                    runBpReversalRequest = true;
                }

                return RedirectToAction(MVC.Message.Index());
            }
            catch (Exception error)
            {
                TempData["Message"] = error.Message + "_ متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید_ ";
                var description = (string)TempData["Message"];
                TempData = new TempDataDictionary() {
                            {"title", title},
                            {"description", description},
                            { "currentpage", "/Shop"},
                            { "type", MessageViewModel.MessageTypes.Error}
                        };
                // Save and send Error for admin user
                using (var db = new DataBaseContext())
                {
                    int id = (int)Session["logId"];
                    var bankLog = db.TblBankGatewayLogs.Find(id);
                    bankLog.Pstatus = 0;
                    bankLog.FarsiCreateDate = error.Message + " | " + bankLog.FarsiCreateDate;
                    db.SaveChanges();
                }


                runBpReversalRequest = true;
                return RedirectToAction(MVC.Message.Index());
            }
            finally
            {
                if (runBpReversalRequest) //ReversalRequest
                {
                    if (saleOrderId != -999 && saleReferenceId != -999)
                        bankMellatImplement.BpReversalRequest(saleOrderId, saleOrderId, saleReferenceId);
                    // Save information to Database...
                    using (var db = new DataBaseContext())
                    {
                        int id = (int)Session["logId"];
                        var bankLog = db.TblBankGatewayLogs.Find(id);
                        bankLog.Pstatus = 10006;
                        db.SaveChanges();
                    }
                }
            }

        }

        public virtual ActionResult Errorpage()
        {
            const string title = "پرداخت آنلاین";
            TempData["Message"] = "متاسفانه در روند ثبت اطلاعات شمامشکلی پیش آمده است، لطفا مجددا تلاش نمایید.";
            var description = (string)TempData["Message"];
            TempData = new TempDataDictionary()
                       {
                        {"title", title},
                        {"description", description},
                        {"currentpage", "/Shop"},
                        {"type", MessageViewModel.MessageTypes.Error}
                       };
            return RedirectToAction(MVC.Message.Index());
        }



        //[HttpPost]
        //public virtual async Task<ActionResult> test()
        //{

        //    using (var db = new DataBaseContext())
        //    {
        //        //var id = (int)Session["logId"];
        //        // var bankLog = db.TblBankGatewayLogs.Find(id);
        //        // bankLog.Pstatus = 10003;
        //        //bankLog.PTransaction = saleReferenceId.ToString();
        //        //db.SaveChanges();
        //        var saleinvoice = db.tblSaleInvoices.Where(x => x.Id == 659).SingleOrDefault();
        //        var saleinvoicedetail = db.tblSaleInvoiceDetails.Where(x => x.InvoiceID == 659).ToList();
        //        var maxcode = db.tblSaleInvoices.Max(x => x.Code);
        //        if (maxcode == null) { maxcode = 1; }
        //        else maxcode++;
        //        saleinvoice.Code = maxcode;
        //        db.SaveChanges();
        //        tblSaleInvoice invoice = null;

        //        invoice = new tblSaleInvoice
        //        {

        //            DiscountPercent = saleinvoice.DiscountPercent,
        //            DiscountTypeId = saleinvoice.DiscountTypeId,
        //            DiscountValue = saleinvoice.DiscountValue,
        //            Notes = saleinvoice.Notes,
        //            TotalValue = saleinvoice.TotalValue,
        //            ManualCode = saleinvoice.ManualCode,
        //            IsPaidByCash = true,
        //            DeliverTypeId = saleinvoice.DeliverTypeId,
        //            CustomerId = saleinvoice.CustomerId,
        //        };



        //        var cartItems = saleinvoicedetail;
        //        short i = 1;
        //        var items1 = cartItems.Select(items => new tblSaleInvoiceDetail
        //        {
        //            BarCode = items.BarCode,
        //            Qty = items.Qty,
        //            RetailPrice = items.RetailPrice,
        //            DiscountPercent=items.DiscountPercent,
        //            DiscountValue=items.DiscountValue,
        //            RowNumber = i++
        //        }).ToList();

        //        var buyer = new tblOrganization();
        //        var finilizeInvoice = new FinalizeInvoice()
        //        {
        //            ApiKey = AppConstants.ApiKey,
        //            TblSaleInvoice = invoice,
        //            TblSaleInvoiceDetail = items1,
        //            TblOrganization = buyer,
        //            DeviceType = (string)Session["DeviceType"],
        //            SendSms = (bool)Session["SendSms"],
        //            Language = (string)Session["Language"]
        //        };


        //        var result = await WebApi.SaveInvoice(finilizeInvoice);

        //        if (result.Id != 0)
        //        {
        //            saleinvoice.Transfer = true;
        //            //bankLog.SaleInvoiceID = result.Id;
        //            db.SaveChanges();
        //            if (saleinvoice.DeliverTypeId != 4)
        //                using (var client = new HttpClient())
        //                {
        //                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
        //                    var url = $"SaveSucharge/{result.Id}/{AppConstants.ApiKey}";
        //                    await client.GetStringAsync(url);
        //                }
        //            var description = TempData["Message"] + "\r\n" + "شماره فاکتور ثبت شده :" + result.Id;

        //            TempData = new TempDataDictionary()
        //                                          {
        //                                              {"title", "خرید"},
        //                                              {"description", description},
        //                                              {"currentpage", "/Shop"},
        //                                              {"type", MessageViewModel.MessageTypes.Success}
        //                                          };
        //        }



        //    }
        //    return RedirectToAction(MVC.Message.Index());


        //}


        //ثبت فاکتور صفر اگر تخفیف داشت
        public virtual async Task<ActionResult> SaveZeroInvoice()
        {
            

               using(var db=new DataBaseContext()) { 
                    var id = (int)Session["logId"];
                    var bankLog = db.TblBankGatewayLogs.Find(id);
                    bankLog.Pstatus = 10003;
                    db.SaveChanges();
                    var saleinvoice = db.tblSaleInvoices.Where(x => x.ManualCode == bankLog.Porderid).SingleOrDefault();
                    var saleinvoicedetail = db.tblSaleInvoiceDetails.Where(x => x.InvoiceID == saleinvoice.Id).ToList();
                    var maxcode = db.tblSaleInvoices.Max(x => x.Code);
                    if (maxcode == null) { maxcode = 1; }
                    else maxcode++;
                    saleinvoice.Code = maxcode;
                    db.SaveChanges();
                    tblSaleInvoice invoice2 = null;

                    invoice2 = new tblSaleInvoice
                    {

                        DiscountPercent = saleinvoice.DiscountPercent,
                        DiscountTypeId = saleinvoice.DiscountTypeId,
                        DiscountValue = saleinvoice.DiscountValue,
                        Notes = saleinvoice.Notes,
                        TotalValue = saleinvoice.TotalValue,
                        ManualCode = saleinvoice.ManualCode,
                        IsPaidByCash = true,
                        DeliverTypeId = saleinvoice.DeliverTypeId,
                        CustomerId = saleinvoice.CustomerId,

                    };



                    var cartItemsF = saleinvoicedetail;
                    short j = 1;

                    var items2 = cartItemsF.Select(items => new tblSaleInvoiceDetail
                    {
                        BarCode = items.BarCode,
                        Qty = items.Qty,
                        RetailPrice = items.RetailPrice,
                        RowNumber = j++
                    }).ToList();

                    var buyerf = new tblOrganization();
                    var finilizeInvoicef = new FinalizeInvoice()
                    {
                        ApiKey = AppConstants.ApiKey,
                        TblSaleInvoice = invoice2,
                        TblSaleInvoiceDetail = items2,
                        TblOrganization = buyerf,
                        DeviceType = (string)Session["DeviceType"],
                        SendSms = (bool)Session["SendSms"],
                        Language = (string)Session["Language"]
                    };


                    var resultfinal = await WebApi.SaveInvoice(finilizeInvoicef);

                    if (resultfinal.Id != 0)
                    {
                        saleinvoice.Transfer = true;
                        //if use coupncode
                        if (saleinvoice.CustomerMessage != null)
                        {
                            var Coupn = db.tblSiteDiscountCodes.Where(x => x.DiscountCode == saleinvoice.CustomerMessage && x.IsUsed == false).SingleOrDefault();
                            if (Coupn != null) Coupn.IsUsed = true;
                        }
                        bankLog.SaleInvoiceID = resultfinal.Id;
                        db.SaveChanges();
                        if (saleinvoice.DeliverTypeId != 4)
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(AppConstants.ApiAddress);
                                var url = $"SaveSucharge/{resultfinal.Id}/{AppConstants.ApiKey}";
                                await client.GetStringAsync(url);
                            }
                        var description = TempData["Message"] + "\r\n" + "شماره فاکتور ثبت شده :" + resultfinal.Id;

                        TempData = new TempDataDictionary()
                                                          {
                                                              {"title", "خرید"},
                                                              {"description", description},
                                                              {"currentpage", "/Shop"},
                                                              {"type", MessageViewModel.MessageTypes.Success}
                                                          };
                    }


                }

                return RedirectToAction(MVC.Message.Index());

            
        }
    }
}