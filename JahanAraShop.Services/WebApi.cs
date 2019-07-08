﻿using JahanAraShop.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace JahanAraShop.Services
{
  public  class WebApi
    {
        public static async Task<Result> PostOrganization(OrganizationSiteModel Organization)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    var url = AppConstants.NewOrganization;
                    var jsonSaveOrganization = JsonConvert.SerializeObject(Organization);
                    var content = new StringContent(jsonSaveOrganization, Encoding.UTF8, AppConstants.JsonType);
                    var result = await client.PostAsync(url, content);
                    var response = result.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<Result>(response);
                    return new Result()
                    {
                        ErrorStatus = res.ErrorStatus,
                        Message = res.Message,
                       
                    };
                    

                }
            }
            catch (Exception ex)
            {
                //var a = ex.Message;
                //return new ResultStatus()
                //{
                //    Id = 0,
                //    Message = a + "Problem in connecting to server.",
                //    StatusCode = HttpStatusCode.NotFound
                //};
                return new Result(Result.ErrorStatusType.NotOk, ex.Message);
            }

        }

        public static async Task<ResultStatus> SaveInvoice(FinalizeInvoice finalizeInvoice)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    var url = AppConstants.FinilizeInvoice;
                    var jsonSaveInvoice = JsonConvert.SerializeObject(finalizeInvoice);
                    var content = new StringContent(jsonSaveInvoice, Encoding.UTF8, AppConstants.JsonType);
                    var result = await client.PostAsync(url, content);
                    var response = result.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<Result>(response);
                    return new ResultStatus()
                    {
                        StatusCode = result.StatusCode,
                        Message = res.Message,
                        Id = res.ErrorStatus == Result.ErrorStatusType.Ok ? int.Parse(res.Message) : 0
                    };


                }



            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return new ResultStatus()
                {
                    Id = 0,
                    Message = a + "Problem in connecting to server.",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

        }

        public static async Task<ResultStatus> SaveLocalInvoice(FinalizeInvoice finalizeInvoice)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConstants.ApiAddress);
                    var url = AppConstants.FinilizeLocalInvoice;
                    var jsonSaveInvoice = JsonConvert.SerializeObject(finalizeInvoice);
                    var content = new StringContent(jsonSaveInvoice, Encoding.UTF8, AppConstants.JsonType);
                    var result = await client.PostAsync(url, content);
                    var response = result.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<Result>(response);
                    return new ResultStatus()
                    {
                        StatusCode = result.StatusCode,
                        Message = res.Message,
                        Id = res.ErrorStatus == Result.ErrorStatusType.Ok ? int.Parse(res.Message) : 0
                    };


                }



            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return new ResultStatus()
                {
                    Id = 0,
                    Message = a + "Problem in connecting to server.",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

        }

        public static async Task<string> SendSmsForCustomer(string cellphone, string name, int id, string message, string userName, string password, string number)
        {
            var stringBuilder = new StringBuilder(message);
            stringBuilder.Replace("{Environment.NewLine}", Environment.NewLine);
            stringBuilder.Replace("{name}", name);
            stringBuilder.Replace("{id}", id.ToString());
            var msg = stringBuilder.ToString();
            using (var client = new HttpClient())
            {
                msg = WebUtility.UrlEncode(msg);
                var encodedUrl =
                    $"{AppConstants.SmsApi}method=send&arg1={userName}&arg2={password}&arg3={cellphone}&arg4={number}&arg5={msg}";
                try
                {
                    var d= await client.GetAsync(encodedUrl);
                    return d.RequestMessage.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
       
        public static async Task<string> GetSiteParameters(Utilities.Parameter parameter)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConstants.ApiAddress);
                var url = $"{AppConstants.GetParameters}/{AppConstants.ApiKey}";
                var response = await client.GetStringAsync(url);
                var resultjson = await Task.Run(() => JsonConvert.DeserializeObject<ParameterModel>(response));

                string result;
                switch (parameter)
                {
                    case Utilities.Parameter.UserName:
                        result = resultjson.Parameters.FirstOrDefault(x => x.Id == 40011)?.Value;
                        break;
                    case Utilities.Parameter.PassWord:
                        result = resultjson.Parameters.FirstOrDefault(x => x.Id == 40012)?.Value;
                        break;
                    case Utilities.Parameter.Number:
                        result = resultjson.Parameters.FirstOrDefault(x => x.Id == 40013)?.Value;
                        break;
                    case Utilities.Parameter.OutLet:
                        result = resultjson.Parameters.FirstOrDefault(x => x.Id == 40015)?.Value;
                        break;


                    default:
                        throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
                }


                return result;
            }
        }
        public static async Task<string> SendSmsOfferCdeForCustomer(string cellphone, string name, string discountcode, string expiredate,string discount,string message, string userName, string password, string number)
        {
            var stringBuilder = new StringBuilder(message);
            stringBuilder.Replace("{Environment.NewLine}", Environment.NewLine);
            stringBuilder.Replace("{name}", name);
            stringBuilder.Replace("{DiscountCode}", discountcode);
            stringBuilder.Replace("{FarsiExpiredate}", expiredate);         
            stringBuilder.Replace("{Discount}", discount);
            
            var msg = stringBuilder.ToString();
            using (var client = new HttpClient())
            {
                msg = WebUtility.UrlEncode(msg);
                var encodedUrl =
                    $"{AppConstants.SmsApi}method=send&arg1={userName}&arg2={password}&arg3={cellphone}&arg4={number}&arg5={msg}";
                try
                {
                    var d = await client.GetAsync(encodedUrl);
                    return d.RequestMessage.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
