using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Services
{
   public class Result
    {

        public Result()
        {

        }

        public Result(ErrorStatusType errorStatus, string message)
        {
            ErrorStatus = errorStatus;
            Message = message;
        }
        public ErrorStatusType ErrorStatus { get; set; }
        public string Message { get; set; }

        public enum ErrorStatusType
        {
            Ok = 0,
            NotOk = 1
        }
    }
}
