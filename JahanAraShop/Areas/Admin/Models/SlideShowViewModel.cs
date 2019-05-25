using JahanAraShop.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Areas.Admin.Models
{
    public class SlideShowViewModel
    {
        public SlideShowViewModel()
        {
            ListSlideShow = new List<TblSiteSlideShow>();
            SlideShow = new TblSiteSlideShow();
        }
        public TblSiteSlideShow SlideShow { get; set; }
        public SelectListItem[] listVisible = new[]
               {
                new SelectListItem { Value = "True", Text = "قابل مشاهده" },
                new SelectListItem { Value = "False", Text = "غیرقابل مشاهده" },
                };
        public List<TblSiteSlideShow> ListSlideShow { get; set; }
    }
}