using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace JahanAraShop.Areas.Admin.Models
{
    public class NewsViewModel
    {
        public List<tblSiteInformation> ListSiteInformations { get; set; }
        public tblSiteInformation SiteInformation { get; set; }
        
    }

}