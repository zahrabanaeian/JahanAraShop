using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahanAraShop.Services
{
   public class FinalizeInvoice
    {
        public tblSaleInvoice TblSaleInvoice { get; set; }
        public List<tblSaleInvoiceDetail> TblSaleInvoiceDetail { get; set; }
        public tblOrganization TblOrganization { get; set; }
        public string ApiKey { get; set; }
        public string DeviceType { get; set; }
        public bool SendSms { get; set; }
        public string Language { get; set; }
        public string CoupnCode { get; set; }
    }
}
