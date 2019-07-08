﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Calendar = Persia.Calendar;

namespace JahanAraShop.Services
{
    public class Utilities
    {
        public static string Encrypt(string str)
        {
            string encrptKey = "ka2!@#.R#mzxc54mSnT_._$Yxd8a@k_+123";
            byte[] byKey = { };
            byte[] iv = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = Encoding.UTF8.GetBytes(encrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        public static string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string decryptKey = "ka2!@#.R#mzxc54mSnT_._$Yxd8a@k_+123";
            byte[] byKey = { };
            byte[] iv = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }


        /// <summary>
        /// گرفتن تاریخ فارسی
        /// </summary>
        /// <returns></returns>
        public static string FarsiDate(DateTime dateTime)
        {
            return Calendar.ConvertToPersian(dateTime).ToString();
        }
        /// <summary>
        /// گرفتن تاریخ فارسی
        /// </summary>
        /// <returns></returns>
        public static string FarsiDateTimeNow()
        {
            var pc = new PersianCalendar();
            var dt = DateTime.Now;
            return $"{pc.GetYear(dt)}/{pc.GetMonth(dt).ToString("D2")}/{pc.GetDayOfMonth(dt).ToString("D2")}";
        }

        public enum Parameter
        {
            PostCost = 1,
            BranchId = 2,
            DiscountPrecent = 3,
            DiscountTypeId = 4,
            PromoterId = 5,
            TypeId = 6,
            PreconditionId = 7,
            WorkActivityId = 8,
            FarsiSmsText = 9,
            DeliverTypeId = 10,
            SmsUserName = 11,
            SmsPassword = 12,
            SmsNumber = 13,
            DeliverPeykId = 14,

            //user smspanel
            UserName = 40011,
            //pass smspanel
            PassWord = 40012,

            // shomare smspanel
            Number = 40013,
            OutLet= 40015



        }



        public static DateTime ParsToChrisDate(string parsDate)
        {
            int pYear = int.Parse(parsDate.Substring(0, 4));
            int pMonth = int.Parse(parsDate.Substring(5, 2));
            int pDay = int.Parse(parsDate.Substring(8, 2));
            PersianCalendar dateConvertor = new PersianCalendar();
            return dateConvertor.ToDateTime(pYear, pMonth, pDay, 0, 0, 0, 0);
        }

        public static ParameterModel ParameterModel { get; set; }
        public static string GetValueFromParameter(ParameterModel parameters, Parameter parameter)
        {
            string result;
            switch (parameter)
            {
                case Parameter.PostCost:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40001)?.Value;
                    break;
                case Parameter.BranchId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40002)?.Value;
                    break;
                case Parameter.DiscountPrecent:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40003)?.Value;
                    break;
                case Parameter.DiscountTypeId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40004)?.Value;
                    break;
                case Parameter.PromoterId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40005)?.Value;
                    break;
                case Parameter.TypeId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40006)?.Value;
                    break;
                case Parameter.PreconditionId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40007)?.Value;
                    break;
                case Parameter.WorkActivityId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40008)?.Value;
                    break;
                case Parameter.FarsiSmsText:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40009)?.Value;
                    break;
                case Parameter.DeliverTypeId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40010)?.Value;
                    break;
                case Parameter.SmsUserName:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40011)?.Value;
                    break;
                case Parameter.SmsPassword:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40012)?.Value;
                    break;
                case Parameter.SmsNumber:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40013)?.Value;
                    break;
                case Parameter.DeliverPeykId:
                    result = parameters.Parameters.FirstOrDefault(x => x.Id == 40014)?.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
            return result;
        }

        public static string GetNote(string name, string family, string mobile, string address, string persianDate, string device)
        {
            return $"این سفارش از طریق درگاه بانک ملت از سمت {device}  در تاریخ {persianDate} ایجاد شده است، مشخصات مشتری: {name} {family}  تلفن: {mobile} آدرس: {address}";
        }

        public static string RandomGenerator()
        {

            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();

        }
        public static int RandomNumber(int min = 1000, int max = 9999)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        public static string RandomString(int size = 10, bool lowerCase = true)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }


    }
}
