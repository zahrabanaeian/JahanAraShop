using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahanAraShop.Models
{
    public class SaveOrganizationModel
    {
        public tblOrganization TblOrganization { get; set; }
        //public tblDistributionOrganization DistributionOrganization { get; set; }
        public string ApiKey { get; set; }
    }
}