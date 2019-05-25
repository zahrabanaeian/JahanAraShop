using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahanAraShop.Controllers.Bank
{
    public partial class GatewayController : Controller
    {
        // GET: Gateway
        public virtual ActionResult CallBack()
        {
            return View();
        }
    }
}