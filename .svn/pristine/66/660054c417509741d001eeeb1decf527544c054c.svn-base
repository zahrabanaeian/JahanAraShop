using JahanAraShop.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers
{
    public partial class NewsController : Controller
    {
        // GET: News
        public virtual ActionResult Index()
        {

            using (var db = new DataBaseContext())
            {


                var list = db.tblSiteInformations.OrderByDescending(x => x.FarsiCreateDate).ToList();
                return View(list);
            }

        }


        public virtual ActionResult GetData(int pageIndex, int pageSize)
        {
            System.Threading.Thread.Sleep(4000);
            using (var db = new DataBaseContext())
            {
                var query = (from c in db.tblSiteInformations
                             orderby c.IdtblInformation ascending
                             select c)
                             .Skip((pageIndex) * pageSize)
                             .Take(pageSize);
                return Json(query.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}