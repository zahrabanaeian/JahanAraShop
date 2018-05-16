using JahanAraShop.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JahanAraShop.Domain.DomainModel;

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
                    //return new ResultStatus()
                    //{
                    //    StatusCode = result.StatusCode,
                    //    Message = res.Message,
                    //    Id = res.ErrorStatus == Result.ErrorStatusType.Ok ? int.Parse(res.Message) : 0
                    //};
                    return new Result(Result.ErrorStatusType.Ok, "Done.");

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
    }
}
