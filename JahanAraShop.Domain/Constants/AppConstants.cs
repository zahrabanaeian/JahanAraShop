﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Domain.Constants
{
  public  class AppConstants
    {
        //SMS
        public static string MesCode = "فروشگاه اینترنتی نی نی ناز \n کد امنیتی: ";
        // public static string MesVerficationCodeWrong = "کد اعتبار سنجی وارد شده نادرست است.";
        public static string MesSmsSessionIsExpired = "متاسفانه نشست شما به اتمام رسیده لطفا بر روی دکمه ارسال دوباره پیامک کلیک کنید.";
        public static string MesSmsSendFailed = "متاسفانه ارسال پیامک به این شماره امکان پذیر نمی باشد.";
        public static string MesSmsResendFailed = "ارسال پیامک مجدد به این شماره تا ده دقیقه دیگر امکان پذیر نمی باشد.";
        public static string MesWrongPass = "رمز عبور وارد شده مجاز نمی باشد.";
        public static string MesWrongConfirmPassword = "تکرار رمز عبور وارد شده صحیح نمی باشد";
        public static string MesWrongType = "لطفا در وارد کردن اطلاعات دقت داشته باشید.";
        public static string MesWrongUserNameOrPassword = "نام کاربر(شماره موبایل) یا کلمه عبور اشتباه است.";


        //Coockie keys
        public static string Basket = "NininazShopBasket";
        public static string BasketCount = "NininaziShopBasketCount";


        public static string Title = "title";
        public static string Description = "description";
        public static string Currentpage = "currentpage";
        public static string Type = "type";

        //Sessions keys
        public static string SesCode = "NininazShopSMSCode";
        public static string SesMobile = "NininazShopUserMobile";

        //Api
        //public static string ApiAddress = "http://arminapilegal.hamyaransystem.com:8686/";
        public static string ApiAddress = "http://localhost:5000/";
        public static string GetGoodGoal = "GetGoodGoal";
        public static string ApiKey = "76B0D844-440B-4597-A0DD-159C14746A88";
        public static string SendEmail = "SendEmail";
        public static string FinilizeInvoice = "FinalizeInvoice";
        public static string NewOrganization = "SaveOrganizationSite";
        public static string GetOrganization = "api/Get";
        public static string JsonType = "application/json";
        public static string SecurityError = "خطای امنیتی";
    }
}
